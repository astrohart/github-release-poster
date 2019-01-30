using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AdysTech.CredentialManager;
using GitHubReleasePoster.Extensions;
using GitHubReleasePoster.Searchers;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using NUnit.Framework;
using Octokit;

namespace GitHubReleasePoster.Tests.EndToEnd
{
    /// <summary>
    /// Methods that drive end-to-end tests of the GitHub API specific to posting and
    /// creating releases according to the features defined for the github-release-poster.
    /// </summary>
    [TestFixture]
    public class EndToEndApiTests
    {
        /// <summary>
        /// Testing body for mock releases.
        /// </summary>
        private const string MockReleaseBody = "testing";

        /// <summary>
        /// Target branch for mock releases.
        /// </summary>
        private const string MockReleaseTargetBranch = "master";

        /// <summary>
        /// Testing (i.e., "mock") release name to use for the end-to-end test in which we create
        /// a mock release where all the asset files are uploaded individually.
        /// </summary>
        /// <remarks>github-release-poster works this way since the size of any individual
        /// release asset is restricted to 2 GB or less by GitHub.  Therefore, we want to
        /// support uploading individual files from a folder one by one as the release's assets.
        /// </remarks>
        private const string MockReleaseWhoseAssetsAreIndividualFilesName = "0.7.3";

        /// <summary>
        /// Testing (i.e., "mock") release name to use for the end-to-end test in which we create
        /// a mock release where all the asset files are ZIPped first prior to upload.
        /// </summary>
        /// <remarks>This is advantageous in that the entire directory tree of a release
        /// is preserved for ready-made installation.  The only downside is that the size
        /// of the ZIP file is restricted to 2 GB or less by GitHub.
        /// </remarks>
        private const string MockReleaseWhoseAssetsIsASingleZipFileName = "0.7.4";

        /// <summary>
        /// Zip file name for the assets when they are zipped and uploaded.
        /// </summary>
        private const string AssetsZipFileName = "assets.zip";

        /// <summary>
        /// Name of the repo to test the releasing functionality against.
        /// </summary>
        private const string RepoName = "NetworkKeeper";

        private const string SampleAssetDirPath = @"C:\Users\ENS Brian Hart\Dropbox\Downloads\emu8086";

        /// <summary>
        /// Value to use for the User-Agent header
        /// </summary>
        private const string TestingProductHeaderValue = "github-release-poster-tests";

        /// <summary>
        /// The name of the Windows Credential Manager credentials to use for
        /// GitHub authentication.
        /// </summary>
        private const string WindowsCredentialName = "git:https://github.com";

        /// <summary>
        /// An end to end test that uploads a release of Notepad to GitHub.
        /// </summary>
        [Test]
        public async Task EndToEndTest_ZippedAssetFiles()
        {
            // TODO: Come up with interfaces and mocks for all this stuff.

            var client = new GitHubClient(
                new Octokit.ProductHeaderValue(TestingProductHeaderValue)
                );

            // basic authentication -- obtain from the Windows credentials of the
            // current user -- requires the AdysTech.CredentialManager NuGet package.
            // The alternative is to also prompt for a user name and password from the
            // user directly, and a third alternative is to use the GitHub XHR requests
            // to sign the user on and create for them a personal access token
            var credential = CredentialManager.GetCredentials(WindowsCredentialName);

            Assert.NotNull(credential);

            var repoOwner = credential.UserName;

            client.Credentials = new Octokit.Credentials(
                repoOwner,
                credential.Password
            );

            // If a mock release already exists with the given release name and in the
            // repo we are targeting with this end-to-end test, then we need to delete
            // the existing release, along with all its assets and tags.
            await DeleteMockReleaseIfExists(client, credential, RepoName,
                MockReleaseWhoseAssetsIsASingleZipFileName);

            // Create and post a new release.   This contains just the metadata for the
            // release (name, body, version etc.).  The files to be put into the release
            // itself come later, with a
            var newRelease = await client.Repository.Release.Create(
                repoOwner,
                RepoName,
                new Octokit.NewRelease($"v{MockReleaseWhoseAssetsIsASingleZipFileName}")
                {
                    Body = MockReleaseBody,
                    Draft = true,
                    Prerelease = false,
                    Name = MockReleaseWhoseAssetsIsASingleZipFileName,
                    TargetCommitish = MockReleaseTargetBranch
                });

            // If the new release was created successfully, then the
            Assert.NotNull(newRelease);

            /* Use SharpZipLib's FastZip class to zip up the files and folders
            in an asset directory.  Therefore, we would be able to send up a single
            ZIP file that, when exploded, replicates a desired directory structure
            on the destination machine. */
	
            Assert.IsTrue(Directory.Exists(SampleAssetDirPath));
		
            // Set up event handlers to keep the zipper software running even when
            // we have failed to zip up a particular file or folder.
            var eventHandlers = new FastZipEvents{
                FileFailure = (s1, a1) => a1.ContinueRunning = true,
                DirectoryFailure = (s2, a2) => a2.ContinueRunning = true
            };

            var zipper = new FastZip(eventHandlers)
            {
                UseZip64 = UseZip64.On,
                CompressionLevel = Deflater.CompressionLevel.BEST_COMPRESSION,
                CreateEmptyDirectories = true,
                RestoreAttributesOnExtract = true,
                RestoreDateTimeOnExtract = true
            };

            var zipFilePath = $@"{Path.GetTempPath()}\{Guid.NewGuid()}\{AssetsZipFileName}";
            var zipFileDir = Path.GetDirectoryName(zipFilePath);

            Assert.IsNotEmpty(zipFileDir);

            if (!Directory.Exists(zipFileDir))
                Directory.CreateDirectory(zipFileDir);

            zipper.CreateZip(
                zipFilePath,
                SampleAssetDirPath,
                true,
                null
            );

            using (var zipStream = File.OpenRead(zipFilePath))
            {
                var newAsset = await client.Repository.Release.UploadAsset(
                    newRelease,
                    new ReleaseAssetUpload
                    {
                        ContentType = MimeMapping.GetMimeMapping(zipFilePath),
                        FileName = Path.GetFileName(zipFilePath),
                        RawData = zipStream,
                        Timeout = TimeSpan.FromSeconds(1000)
                    }
                );

                Assert.NotNull(newAsset);
            }

            // publish the new release by setting its Draft flag to false
            newRelease = await client.Repository.Release.Edit(repoOwner, RepoName,
                newRelease.Id, new ReleaseUpdate { Draft = false, Prerelease = true });

            Assert.NotNull(newRelease);

            // Make sure the release has been published
            Assert.IsFalse(newRelease.Draft);

            // Make sure this "mock release" has been marked as pre-release
            Assert.IsTrue(newRelease.Prerelease);
        }

        [Test]
        public async Task EndToEndTest_IndividualAssetFiles()
        {
            // TODO: Come up with interfaces and mocks for all this stuff.

            var client = new GitHubClient(
                new Octokit.ProductHeaderValue(TestingProductHeaderValue)
                );

            // basic authentication -- obtain from the Windows credentials of the
            // current user -- requires the AdysTech.CredentialManager NuGet package.
            // The alternative is to also prompt for a user name and password from the
            // user directly, and a third alternative is to use the GitHub XHR requests
            // to sign the user on and create for them a personal access token
            var credential = CredentialManager.GetCredentials(WindowsCredentialName);

            Assert.NotNull(credential);

            var repoOwner = credential.UserName;

            client.Credentials = new Octokit.Credentials(
                repoOwner,
                credential.Password
            );

            // If a mock release already exists with the given release name and in the
            // repo we are targeting with this end-to-end test, then we need to delete
            // the existing release, along with all its assets and tags.
            await DeleteMockReleaseIfExists(client, credential, RepoName,
                MockReleaseWhoseAssetsAreIndividualFilesName);

            // Create and post a new release.   This contains just the metadata for the
            // release (name, body, version etc.).  The files to be put into the release
            // itself come later, with a
            var newRelease = await client.Repository.Release.Create(
                repoOwner,
                RepoName,
                new Octokit.NewRelease($"v{MockReleaseWhoseAssetsAreIndividualFilesName}")
                {
                    Body = MockReleaseBody,
                    Draft = true,
                    Prerelease = false,
                    Name = MockReleaseWhoseAssetsAreIndividualFilesName,
                    TargetCommitish = MockReleaseTargetBranch
                });

            // If the new release was created successfully, then the
            Assert.NotNull(newRelease);

            /*Iterate over the files in a directory, flattening the directory structure
	        as we go, and then upload each file one by one as a release asset.  We use
	        MimeMappings.GetMimeMapping to get the Content-Type value.

	        By flattening the directory structure within the folder, we do not mean to
	        say that any subfolders that happen to be there will be removed; it just means
	        that, once the assets are in the GitHub release, all the files will just be in
	        a list, with no regard to the arrangement they happened to be in inside the release
	        folder on the source computer.  If you want to preserve the directory tree of
	        a release, then have this software create a zip file of the release first. */

            Assert.IsTrue(Directory.Exists(SampleAssetDirPath));

            foreach (var file in FileSystemSearcher.Search(SampleAssetDirPath))
            {
                if (file == null)
                    continue;       // Null reference returned; unlikely, but skip this file if true

                var currentFilePath = file.FullName;

                if (!file.Exists)
                    continue;   // Skip files that don't exist

                if (file.IsLocked())
                    continue;   // we can only upload those files to which the user has access

                using (var assetStream = File.OpenRead(currentFilePath))
                {
                    var newAsset = await client.Repository.Release.UploadAsset(
                        newRelease,
                        new ReleaseAssetUpload
                        {
                            // Need to add a reference to System.Web.dll for MimeMapping
                            ContentType = MimeMapping.GetMimeMapping(currentFilePath),
                            FileName = file.Name,
                            RawData = assetStream,
                            Timeout = TimeSpan.FromSeconds(1000)
                        }
                    );

                    Assert.NotNull(newAsset);
                }
            }

            // publish the new release by setting its Draft flag to false
            newRelease = await client.Repository.Release.Edit(repoOwner, RepoName,
                newRelease.Id, new ReleaseUpdate { Draft = false, Prerelease = true });

            Assert.NotNull(newRelease);

            // Make sure the release has been published
            Assert.IsFalse(newRelease.Draft);

            // Make sure this "mock release" has been marked as pre-release
            Assert.IsTrue(newRelease.Prerelease);
        }

        

        [SetUp]
        public void Initialize()
        {
            // Clear all the releases out of the GitHub system whose bodies are
            // equal to "testing" -- this is commented out, because each end-to-end
            // test deletes the mock release that it creates
            //await DeleteAllReleases(r => "testing".Equals(r.Body));
        }

        /// <summary>
        /// Searches for and deletes the specified mock release, if found.
        /// </summary>
        /// <param name="client">Reference to an object implementing
        /// <see cref="T:Octokit.IGitHubClient"/> that represents a connected
        /// GitHub API client.</param>
        /// <param name="credentials">Reference to an instance of
        /// <see cref="T:System.Net.NetworkCredential"/> that supply the authentication
        /// credentials for accessing the GitHub system.  These must be the same
        /// credentials that had been used to access the system in the first place.</param>
        /// <param name="repoName">String containing the name of the repo to use.</param>
        /// <param name="releaseName">String containing the name of the mock release to delete.</param>
        /// <remarks>This method is awaitable.  This method will stop running if any of its
        /// parameters are null or contain blanks or invalid information, or if the desired
        /// mock release is not found.  This method is needed, since as we automate our
        /// end-to-end tests (that actually create real mock releases), we can't have more
        /// than one release with the same name since they all use GitHub tags (this might
        /// not be a hard-and-fast GitHub restriction, but it makes sense for a test-automation
        /// scenario).  So, we can use this method to search for and delete an existing
        /// mock release so we only have one mock release in the system at a time.</remarks>
        private static async Task DeleteMockReleaseIfExists(IGitHubClient client,
            NetworkCredential credentials, string repoName, string releaseName)
        {
            /* Quietly refuse to run if all the parameters are not supplied */

            if (client == null)
                return;

            if (credentials == null)
                return;

            // Assume that the owner of the repo matches the current user's
            // user name that was used to access GitHub in the first place.
            var repoOwner = credentials.UserName;
            if (string.IsNullOrWhiteSpace(repoOwner))
                return;

            if (string.IsNullOrWhiteSpace(repoName))
                return;

            if (string.IsNullOrWhiteSpace(releaseName))
                return;

            // Before we proceed to make a new mock release, if the mock release, its assets, and
            // all its GitHub tags already exist in the GitHub system, clear out the old
            // release before creating the mock release over again

            // First, we check to see if the mock release we created in a previous end-to-end
            // test even exists at all to begin with.  If not, then we can skip this part
            var releases =
                await client.Repository.Release.GetAll(repoOwner, repoName);

            if (!releases.Any(r => releaseName.Equals(r.Name)))
                return;

            var desiredReleaseId = releases.First(r => releaseName.Equals(r.Name)).Id;

            // If a release with the name we are going to use (hard-coded) already exists,
            // delete all of its assets, and then delete the release.  There is a master-details
            // relationship between releases and assets.
            var assets = await client.Repository.Release.GetAllAssets(repoOwner,
                repoName, desiredReleaseId);

            // Iterate through the list of the assets that are associated with the
            // mock release, and delete them.  This loop won't run if there aren't any
            // assets in the release to begin with.
            foreach (var asset in assets)
                await client.Repository.Release.DeleteAsset(
                    repoOwner, repoName, asset.Id);

            // Delete the release
            await client.Repository.Release.Delete(repoOwner, repoName, desiredReleaseId);

            // Check and see if there is a GitHub tag associated with the mock release
            // we are trying to delete.  If so, then delete it.
            var refs = await client.Git.Reference.GetAll(repoOwner, repoName);
            foreach (var tagRef in refs)
            {
                if (!tagRef.Ref.Contains($"v{releaseName}"))
                    continue;

                await client.Git.Reference.Delete(repoOwner,
                    repoName, tagRef.Ref.Replace("refs/", string.Empty));
            }
        }

        /// <summary>
        /// Helper method to clear all the releases out of a repository, along with their
        /// tags
        /// </summary>
        private async Task DeleteAllReleases(Predicate<Release> predicate = null)
        {
            var client = new GitHubClient(
                new Octokit.ProductHeaderValue(TestingProductHeaderValue)
            );

            // basic authentication -- obtain from the Windows credentials of the
            // current user -- requires the AdysTech.CredentialManager NuGet package.
            // The alternative is to also prompt for a user name and password from the
            // user directly, and a third alternative is to use the GitHub XHR requests
            // to sign the user on and create for them a personal access token
            var credential = CredentialManager.GetCredentials(WindowsCredentialName);

            Assert.NotNull(credential);

            var repoOwner = credential.UserName;

            client.Credentials = new Octokit.Credentials(
                repoOwner,
                credential.Password
            );

            foreach (var release in await client.Repository.Release.GetAll(repoOwner, RepoName))
            {
                if (predicate != null
                    && !predicate(release))
                    continue;

                // Before deleting the release, delete all its assets
                foreach(var asset in await client.Repository.Release.GetAllAssets(
                    repoOwner, RepoName, release.Id)){
                    await client.Repository.Release.DeleteAsset(repoOwner, RepoName, asset.Id);
                }

                await client.Repository.Release.Delete(repoOwner, RepoName,
                    release.Id);
            }

            // Just because we purge all the releases from a repo, does not mean all the tags
            // that were associated with them go away. We have to do another run and purge those
            // too.

            foreach (var tag in await client.Git.Reference.GetAll(repoOwner, RepoName))
            {
                if (tag.Ref.Contains("head"))
                    continue;       // this reference is for a branch; we do not want to delete it

                await client.Git.Reference.Delete(repoOwner, RepoName,
                    tag.Ref.Replace("refs/", string.Empty));
            }
        }
    }
}
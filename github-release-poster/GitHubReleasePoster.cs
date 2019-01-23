using github_release_poster.Properties;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace github_release_poster
{
    /// <summary>
    /// Posts releases to GitHub.
    /// </summary>
    public static class GitHubReleasePoster
    {
        /// <summary>
        /// Posts the new release to GitHub.
        /// </summary>
        /// <param name="repoName">(Required.) Name of the GitHub repository to which the release is to be posted.</param>
        /// <param name="repoOwner">(Required.) GitHub username of a user with push access to the repository to which the release is to be posted.</param>
        /// <param name="userAccessToken">(Required.) GitHub user access token which belongs to the repository owner and that has push access to the repository.</param>
        /// <param name="releaseAssetDir">(Required.) Full path to the directory where the assets for the current release are located.  Must be a path of a folder that currently exists.</param>
        /// <param name="release">(Required.) Reference to an instance of <see cref="T:github_releaser.NewRelease"/></param>
        /// <param name="shouldNotZip">(Optional, default is false). Set to true to upload release assets individually rather than ZIPping the contents of the release asset dir.</param>
        /// that contains the required release information in its properties.
        /// <param name="assetsZipName">Name to give to the zip file of the assets.  Ignored if <see cref="shouldNotZip"/> is set to true.</param>
        /// <returns>True if the post succeeded and all assets got uploaded; false otherwise.</returns>
        /// <remarks>The <see cref="T:github_releaser.NewRelease"/> object is serialized to JSON and then posted to the GitHub Server.  This object is assumed to have valid information.
        /// To validate the information, call the <see cref="M:github_releaser.GitHubReleaeValidator.IsReleaseValid"/> method.</remarks>
        public static bool PostNewRelease(string repoName,
            string repoOwner, string userAccessToken, string releaseAssetDir,
            NewRelease release, bool shouldNotZip = false, string assetsZipName = "assets.zip")
        {
            Console.WriteLine(Resources.PostingReleaseToWhichRepo, release.name, repoName);

            // Parameter validation
            if (string.IsNullOrWhiteSpace(repoName))
                throw new ArgumentNullException(nameof(repoName));

            if (string.IsNullOrWhiteSpace(repoOwner))
                throw new ArgumentNullException(nameof(repoOwner));

            if (string.IsNullOrWhiteSpace(userAccessToken))
                throw new ArgumentNullException(nameof(userAccessToken));

            if (string.IsNullOrWhiteSpace(releaseAssetDir))
                throw new ArgumentNullException(nameof(releaseAssetDir));

            if (!Directory.Exists(releaseAssetDir))
                throw new DirectoryNotFoundException(string.Format(
                    Resources.ReleaseAssetDirNotFound, releaseAssetDir));

            if (release == null)
                throw new ArgumentNullException(nameof(release));
            try
            {
                // Form the request body
                var json = release.ToJson();

                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine(Resources.FailedToFormatReleaseJson);
                    return false;
                }

                Console.WriteLine(Resources.SendingPostRequestToCreateRelease, release.name);

                var createReleaseUrl = string.Format(Resources.CreateReleaseApiPostURL, repoOwner, repoName);

                var client = new RestClient(createReleaseUrl);
                var request = GitHubRequestFactory.PrepareGitHubRequest(Method.POST, userAccessToken);

                request.AddHeader(Resources.AcceptHeaderName, Resources.GitHubApiV3Accept);
                request.AddParameter(Resources.UndefinedParameterName, json, ParameterType.RequestBody);

                var response = client.Execute(request);

                if (response == null || response.StatusCode != HttpStatusCode.Created)
                {
                    Console.WriteLine("ERROR: Failed to post release.");
                    return false;
                }

                var newRelease = response.Content.FromJson();

                // get the ID of the new release
                Console.WriteLine($"Posted release {newRelease.id} to {newRelease.target_commitish}.");

                Console.WriteLine(Resources.ProcessingReleaseAssets);

                request = GitHubRequestFactory.PrepareGitHubRequest(Method.POST, userAccessToken);

                string uploadUrl = newRelease.upload_url;  // use type explicitly to cast so that extension methods work
                if (string.IsNullOrWhiteSpace(uploadUrl))
                {
                    Console.WriteLine(Resources.UploadUrlNotObtainable);
                    Environment.Exit(Resources.ERROR_NOT_OBTAINED_RELEASE_UPLOAD_URL);
                }

                // process the assets
                // If the user has set shouldNotZip to false, zip up the release assets.
                if (shouldNotZip)
                {
                    // Iterate over all the files in the release asset directory and its subdirectories,
                    // one by one.  Flatten the directory tree into just a big ol' list of files.  To preserve
                    // the directory tree, distribute the release assets in ZIP format and then have your
                    // Setup program un zip the assets into their directory structure.
                    foreach (var assetFile in FileSearcher.GetAllFilesInFolder(
                        releaseAssetDir).Where(fsi => (fsi.Attributes & FileAttributes.Directory)
                                                      != FileAttributes.Directory))
                    {
                        // Get just the name and extension of the asset for use in the
                        // upload url later.  If blank is returned, then something went wrong.
                        // In that case, just skip the current file.
                        var assetFileName = Path.GetFileName(assetFile.FullName);
                        if (string.IsNullOrWhiteSpace(assetFileName))
                            continue;

                        // Read all the bytes from the file into memory.  If we encounter a
                        // zero-byte file, don't upload it.
                        var fileBytes = File.ReadAllBytes(assetFile.FullName);
                        if (fileBytes.Length == 0)
                            continue; /* do not process empty files */

                        var uploadUri = uploadUrl.ExpandUriTemplate(new
                        {
                            name = assetFileName,
                            label = assetFileName
                        });

                        // Prepare a REST request with the upload url from the create release API response
                        // above
                        client = new RestClient(
                            uploadUri
                        );

                        request.AddHeader(Resources.ContentTypeHeaderName,
                            MimeMapping.GetMimeMapping(assetFile.FullName)
                        );
                        request.AddParameter(
                            Resources.ApplicationOctetStreamMimeType, /* everything is an application/octet-stream */
                            fileBytes, ParameterType.RequestBody
                        );
                        response = client.Execute(request);

                        if (response == null || response.StatusCode != HttpStatusCode.Created)
                        {
                            Console.WriteLine(Resources.FailedToUploadAsset, assetFileName);
                            Environment.Exit(Resources.ERROR_ASSET_NOT_ACCEPTED);
                        }
                        else
                        {
                            Console.WriteLine(Resources.AssetAccepted, assetFileName);
                        }
                    }
                }
                else
                {
                    // Use GUIDs to name the zip and the folder to put it in.
                    // make a sub folder under the user's temp folder, and then inside that folder,
                    // put the zipped up assets, naming the zip file as the user has specified.
                    var outputZipFilePath = $@"{Path.GetTempPath()}\{Guid.NewGuid()}\{assetsZipName}";

                    var outputZipFileName = Path.GetFileName(outputZipFilePath);

                    if (!ZipperUpper.CompressDirectory(releaseAssetDir, outputZipFilePath))
                    {
                        Console.WriteLine(Resources.FailedToPackageReleaseForPosting);
                        return false; // Failed to zip up the release files
                    }

                    /* Determine whether the compression successfully completed.  Stop if it did not. */
                    if (!File.Exists(outputZipFilePath))
                    {
                        Console.WriteLine(Resources.FailedToZipAssets, releaseAssetDir);
                        Environment.Exit(Resources.ERROR_FAILED_TO_ZIP_ASSETS);
                    }

                    /* Determine whether the ZIP file is 2GB or bigger in size.  If it is, then stop.
                     This is because GitHub API will not accept asset files bigger than 2 GB. */
                    if (Convert.ToUInt64(new FileInfo(outputZipFilePath).Length) >= Resources.TwoGigaBytes)
                    {
                        Console.WriteLine(Resources.ZipFileTooBig, outputZipFilePath);
                        Environment.Exit(Resources.ERROR_RELEASE_ASSET_IS_TOO_BIG);
                    }

                    /* If we are here, then read all the bytes from the file into memory. */
                    var fileBytes = File.ReadAllBytes(outputZipFilePath);
                    if (!fileBytes.Any())
                        return false; // ZIP cannot be zero length

                    var uploadUri = uploadUrl.ExpandUriTemplate(new { name = assetsZipName, label = assetsZipName });

                    client = new RestClient(
                        uploadUri
                    );

                    request.AddHeader(Resources.ContentTypeHeaderName,
                        Resources.ZipFileContentType
                    );

                    request.AddParameter(Resources.ZipFileContentType,
                        fileBytes, ParameterType.RequestBody
                    );

                    response = client.Execute(request);
                    
                    if (response == null || response.StatusCode != HttpStatusCode.Created)
                    {
                        Console.WriteLine(Resources.FailedToUploadAsset, outputZipFileName);
                        Environment.Exit(Resources.ERROR_ASSET_NOT_ACCEPTED);
                    }
                    else
                    {
                        Console.WriteLine(Resources.AssetAccepted, outputZipFileName);
                    }
                }

                Console.WriteLine(Resources.ReleasePostedToGitHub, newRelease.name);
                return true;
            }
            catch
            {
                Console.WriteLine(Resources.FailedToPackageReleaseForPosting);
                return false;
            }
        }
    }
}
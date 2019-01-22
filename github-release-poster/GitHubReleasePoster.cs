using github_release_poster.Properties;
using RestSharp;
using System;
using System.IO;
using System.Runtime.Versioning;

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
        /// <remarks>The <see cref="T:github_releaser.NewRelease"/> object is serialized to JSON and then posted to the GitHub Server.  This object is assumed to have valid information.
        /// To validate the information, call the <see cref="M:github_releaser.GitHubReleaeValidator.IsReleaseValid"/> method.</remarks>
        public static void PostNewRelease(string repoName,
            string repoOwner, string userAccessToken, string releaseAssetDir,
            NewRelease release, bool shouldNotZip = false)
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

            // Form the request body
            var json = release.ToJson();

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine(Resources.FailedToFormatReleaseJson);
                return;
            }

            Console.WriteLine(Resources.SendingPostRequestToCreateRelease, release.name);

            // RestSharp code, big shout out to Postman app
            var client = new RestClient(Resources.CreateReleaseApiPostURL);
            var request = new RestRequest(Method.POST);
            request.AddHeader(Resources.CacheControlHeaderName, Resources.NoCacheHeader);
            request.AddHeader(Resources.AcceptHeaderName, Resources.GitHubApiV3Accept);
            request.AddHeader(Resources.UserAgentHeaderName, Resources.TestAppUserAgent);
            request.AddHeader(Resources.AuthorizationHeaderName, $"token {userAccessToken}");
            request.AddParameter(Resources.UndefinedParameterName, json, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response == null)
            {
                Console.WriteLine("ERROR: Failed to post release.");
                return;
            }

            var responseData = response.Content.FromJson();

            // get the ID of the new release
            Console.WriteLine($"Posted release {responseData.id} to {responseData.target_commitish}.");

            Console.WriteLine(Resources.ProcessingReleaseAssets);

            request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("User-Agent", "test app");
            request.AddHeader("Authorization", $"token {userAccessToken}");

            // process the assets
            // If the user has set shouldNotZip to false, zip up the release assets.
            if (!shouldNotZip)
            {
                // Use GUIDs to name the zip and the folder to put it in.
                var outputZipFilePath = $@"{Path.GetTempPath()}\{Guid.NewGuid()}\{Guid.NewGuid()}.zip";

                if (!ZipperUpper.CompressDirectory(releaseAssetDir, outputZipFilePath))
                {
                    Console.WriteLine(Resources.FailedToPackageReleaseForPosting);
                    return; // Failed to zip up the release files
                }

                client = new RestClient(
                    string.Format(Resources.UploadAssetURL, responseData.assets_url, 
                        Path.GetFileName(outputZipFilePath), Path.GetFileName(outputZipFilePath)));

                request.AddHeader("Content-Type", Resources.ZipFileContentType);
                request.AddParameter(Resources.ZipFileContentType, 
                    File.ReadAllBytes(outputZipFilePath), ParameterType.RequestBody);
                response = client.Execute(request);

                // TODO: Add code here to post the ZIP file to the release using the upload URL
            }
            else
            {
                foreach (var assetFile in .)
            }

            // done
        }
    }
}
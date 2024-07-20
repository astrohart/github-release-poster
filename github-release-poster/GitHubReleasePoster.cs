using github_release_poster.Properties;
using RestSharp;
using System;
using System.IO;
using System.Net;

namespace github_release_poster
{
    /// <summary> Posts releases to GitHub. </summary>
    public static class GitHubReleasePoster
    {
        /// <summary> Posts the new release to GitHub. </summary>
        /// <param name="repoName">
        /// (Required.) Name of the GitHub repository to which the
        /// release is to be posted.
        /// </param>
        /// <param name="repoOwner">
        /// (Required.) GitHub username of a user with push access
        /// to the repository to which the release is to be posted.
        /// </param>
        /// <param name="userAccessToken">
        /// (Required.) GitHub user access token which
        /// belongs to the repository owner and that has push access to the repository.
        /// </param>
        /// <param name="releaseAssetDir">
        /// (Required.) Full path to the directory where the
        /// assets for the current release are located.  Must be a path of a folder that
        /// currently exists.
        /// </param>
        /// <param name="release">
        /// (Required.) Reference to an instance of
        /// <see cref="T:github_releaser.NewRelease" />
        /// </param>
        /// <param name="shouldNotZip">
        /// (Optional, default is false). Set to true to upload
        /// release assets individually rather than ZIPping the contents of the release
        /// asset dir.
        /// </param>
        /// that contains the required release information in its properties.
        /// <param name="assetsZipName">
        /// Name to give to the zip file of the assets.
        /// Ignored if <see cref="shouldNotZip" /> is set to true.
        /// </param>
        /// <returns>
        /// True if the post succeeded and all assets got uploaded; false
        /// otherwise.
        /// </returns>
        /// <remarks>
        /// The <see cref="T:github_releaser.NewRelease" /> object is serialized
        /// to JSON and then posted to the GitHub Server.  This object is assumed to have
        /// valid information. To validate the information, call the
        /// <see cref="M:github_releaser.GitHubReleaeValidator.IsReleaseValid" /> method.
        /// </remarks>
        public static bool PostNewRelease(
            string repoName,
            string repoOwner,
            string userAccessToken,
            string releaseAssetDir,
            NewRelease release,
            bool shouldNotZip = false,
            string assetsZipName = "assets.zip"
        )
        {
            Console.WriteLine(
                Resources.PostingReleaseToWhichRepo, release.name, repoName
            );

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
                throw new DirectoryNotFoundException(
                    string.Format(
                        Resources.ReleaseAssetDirNotFound, releaseAssetDir
                    )
                );

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

                Console.WriteLine(
                    Resources.SendingPostRequestToCreateRelease, release.name
                );

                var createReleaseUrl = string.Format(
                    Resources.CreateReleaseApiPostURL, repoOwner, repoName
                );

                var client = new RestClient(createReleaseUrl);
                var request = GitHubRequestFactory.PrepareGitHubRequest(
                    Method.POST, userAccessToken
                );

                request.AddHeader(
                    Resources.AcceptHeaderName, Resources.GitHubApiV3Accept
                );
                request.AddParameter(
                    Resources.UndefinedParameterName, json,
                    ParameterType.RequestBody
                );

                var response = client.Execute(request);

                if (response == null ||
                    response.StatusCode != HttpStatusCode.Created)
                {
                    Console.WriteLine("ERROR: Failed to post release.");
                    return false;
                }

                var newRelease = response.Content.FromJson();

                // get the ID of the new release
                Console.WriteLine(
                    $"Posted release {newRelease.id} to {newRelease.target_commitish}."
                );

                Console.WriteLine(Resources.ProcessingReleaseAssets);

                request = GitHubRequestFactory.PrepareGitHubRequest(
                    Method.POST, userAccessToken
                );

                // process the assets
                // If the user has set shouldNotZip to false, zip up the release assets.
                if (shouldNotZip)
                    AssetUploader.UploadAssetsIndividually(
                        releaseAssetDir, newRelease, userAccessToken
                    );
                else
                    AssetUploader.UploadAssetsZipped(
                        releaseAssetDir, newRelease, userAccessToken,
                        assetsZipName
                    );

                Console.WriteLine(
                    Resources.ReleasePostedToGitHub, newRelease.name
                );
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
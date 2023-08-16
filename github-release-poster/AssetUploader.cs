using github_release_poster.Properties;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Tavis.UriTemplates;

namespace github_release_poster
{
    public static class AssetUploader
    {
        /// <summary>
        /// Uploads the assets in the release asset directory individually to the newly-created release.   Flattens any directory structure that might exist in the
        /// folder referred to by <see cref="releaseAssetDir"/>.
        /// </summary>
        /// <param name="releaseAssetDir">String containing the fully-qualified path to the directory containing release assets to be uploaded.</param>
        /// <param name="newRelease">Reference to an instance of a dynamic object which has been deserialized from the response JSON
        /// of the call to create the new release.</param>
        /// <param name="userAccessToken">String containing the user's API access token.</param>
        public static void UploadAssetsIndividually(string releaseAssetDir,
            dynamic newRelease, string userAccessToken)
        {
            if (string.IsNullOrWhiteSpace(releaseAssetDir))
                throw new ArgumentNullException(nameof(releaseAssetDir));

            if (!Directory.Exists(releaseAssetDir))
                throw new DirectoryNotFoundException(string.Format(Resources.DirectoryNotFound, releaseAssetDir));

            if (newRelease == null)
                throw new ArgumentNullException(nameof(newRelease));

            if (string.IsNullOrWhiteSpace(userAccessToken))
                throw new ArgumentNullException(nameof(userAccessToken));

            // Iterate over all the files in the release asset directory and its subdirectories,
            // one by one.  Flatten the directory tree into just a big ol' list of files.  To preserve
            // the directory tree, distribute the release assets in ZIP format and then have your
            // Setup program un zip the assets into their directory structure.
            foreach (var assetFile in FileSearcher.GetAllFilesInFolder(
                releaseAssetDir).Where(fsi => (fsi.Attributes & FileAttributes.Directory)
                                              != FileAttributes.Directory))
            {
                /* Make a new request each iteration of the loop over asset files */
                var request = GitHubRequestFactory.PrepareGitHubRequest(Method.POST, userAccessToken);

                // Get just the name and extension of the asset for use in the
                // upload url later.  If blank is returned, then something went wrong.
                // In that case, just skip the current file.
                var assetFileName = Path.GetFileName(assetFile.FullName);
                if (string.IsNullOrWhiteSpace(assetFileName))
                    continue;

                // If the file has zero bytes of length, do not upload it
                if (FileAndFolderHelper.FileHasZeroLength((FileInfo)assetFile))
                {
                    Console.WriteLine(
                        $"ERROR: File '{assetFileName}' has zero bytes of length.  Not uploading it.");
                    // Just skip files that have zero length
                    continue;
                }

                /* From Tavis.UriTemplates NuGet package -- works like Ruby uri_template gem */
                var uploadUrl = new UriTemplate(newRelease.upload_url.Value)
                    .AddParameters(new
                    {
                        name = assetFileName,
                        label = assetFileName
                    })
                    .Resolve();
                if (string.IsNullOrWhiteSpace(uploadUrl))
                {
                    Console.WriteLine(Resources.UploadUrlNotObtainable);
                    Environment.Exit(Resources.ERROR_NOT_OBTAINED_RELEASE_UPLOAD_URL);
                }

                // Prepare a REST request with the upload url from the create release API response
                // above
                var client = new RestClient(
                    uploadUrl
                );

                var assetMimeMapping = MimeMapping.GetMimeMapping(assetFile.FullName);
                request.AddHeader(Resources.ContentTypeHeaderName,
                    assetMimeMapping
                );

                var bytes = File.ReadAllBytes(assetFile.FullName);
                if (!bytes.Any())
                    continue;   // zero-length file

                request.AddHeader("Content-Length", bytes.Length.ToString());
                Console.WriteLine($"{bytes.Length} bytes read from file '{assetFile.FullName}'. Uploading...");

                request.AddParameter(
                    assetMimeMapping, bytes, ParameterType.RequestBody
                );

                var response = client.Execute(request);

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

        /// <summary>
        /// Uploads assets to the release, ZIPping the file tree in the release directory and then uploading the zip.
        /// </summary>
        /// <param name="releaseAssetDir">String containing the fully-qualified path to the directory containing the release's assets.</param>
        /// <param name="newRelease">Reference to an instance of a dynamic object deserialized from the JSON response to the create-new-release action.</param>
        /// <param name="userAccessToken">String containing the user's API access token.</param>
        /// <param name="assetsZipName">Name to call the new zip file of assets.  Default is "assets.zip".</param>
        /// <returns>True if the upload operation succeeded; false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">Thrown if any of the parameters of this method are null references or blank strings.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">Thrown if the release asset directory cannot be located.</exception>
        public static void UploadAssetsZipped(string releaseAssetDir,
            dynamic newRelease, string userAccessToken, string assetsZipName = "assets.zip")
        {
            if (string.IsNullOrWhiteSpace(releaseAssetDir))
                throw new ArgumentNullException(nameof(releaseAssetDir));

            if (!Directory.Exists(releaseAssetDir))
                throw new DirectoryNotFoundException(string.Format(Resources.DirectoryNotFound, releaseAssetDir));

            if (newRelease == null)
                throw new ArgumentNullException(nameof(newRelease));

            if (string.IsNullOrWhiteSpace(userAccessToken))
                throw new ArgumentNullException(nameof(userAccessToken));

            if (string.IsNullOrWhiteSpace(assetsZipName))
                throw new ArgumentNullException(nameof(assetsZipName));

            // Use GUIDs to name the zip and the folder to put it in.
            // make a sub folder under the user's temp folder, and then inside that folder,
            // put the zipped up assets, naming the zip file as the user has specified.
            var outputZipFilePath = $@"{Path.GetTempPath()}\{Guid.NewGuid()}\{assetsZipName}";

            var outputZipFileName = Path.GetFileName(outputZipFilePath);

            if (!ZipperUpper.CompressDirectory(releaseAssetDir, outputZipFilePath))
            {
                Console.WriteLine(Resources.FailedToPackageReleaseForPosting);
                Environment.Exit(Resources.ERROR_FAILED_TO_ZIP_ASSETS);
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
            {
                Console.WriteLine(Resources.AssetZipProcessResultedInZeroSizePackage);
                Environment.Exit(Resources.ERROR_FAILED_TO_ZIP_ASSETS);
            }

            /* From Tavis.UriTemplates NuGet package -- works like Ruby uri_template gem */
            var uploadUrl = new UriTemplate(newRelease.upload_url.Value)
                .AddParameters(new
                {
                    name = assetsZipName,
                    label = assetsZipName
                })
                .Resolve();

            if (string.IsNullOrWhiteSpace(uploadUrl))
            {
                Console.WriteLine(Resources.UploadUrlNotObtainable);
                Environment.Exit(Resources.ERROR_NOT_OBTAINED_RELEASE_UPLOAD_URL);
            }

            var client = new RestClient(
                uploadUrl
            );

            var request = GitHubRequestFactory.PrepareGitHubRequest(Method.POST, userAccessToken);

            request.AddHeader(Resources.ContentTypeHeaderName,
                Resources.ZipFileContentType
            );

            request.AddParameter(Resources.ZipFileContentType,
                fileBytes, ParameterType.RequestBody
            );

            var response = client.Execute(request);

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
    }
}
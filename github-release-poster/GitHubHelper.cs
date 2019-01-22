using System;
using System.IO;
using System.Net;
using github_release_poster.Properties;
using RestSharp;

namespace github_release_poster
{
    /// <summary>
    /// Provides methods and functionality to help with Web operations.
    /// </summary>
    public static class GitHubHelper
    {
        /// <summary>
        /// Gets a release asset from GitHub.  
        /// </summary>
        /// <param name="url">API URL for the particular asset, e.g.,
        /// <pre>https://api.github.com/repos/:owner/:repo/releases/assets/:asset_id.</pre></param>
        /// <param name="destinationFile">Destination where the asset is to be written on the user's computer.</param>
        /// <param name="userToken">User token for personal access to the GitHub API.</param>
        /// <returns>True if the operation succeeded and the asset now exists on the user's disk in the specified location; false otherwise.</returns>
        public static bool GetReleaseAsset(string url, string destinationFile, string userToken)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(DebugLevel.Debug, "In GitHubHelper.GetReleaseAsset");

            var result = false;

            // Dump the variable url to the log
            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: url = '{0}'", url);

            if (string.IsNullOrWhiteSpace(url))
            {
                DebugUtils.WriteLine(DebugLevel.Error,
                    "GitHubHelper.GetReleaseAsset: Blank value passed for 'url' parameter. This parameter is required.");

                DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: Result = {0}", result);

                DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: Done.");

                return result;
            }

            // Dump the variable destinationFile to the log
            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: destinationFile = '{0}'", destinationFile);

            if (string.IsNullOrWhiteSpace(destinationFile))
            {
                DebugUtils.WriteLine(DebugLevel.Error,
                    "GitHubHelper.GetReleaseAsset: Blank value passed for 'destinationFile' parameter. This parameter is required.");

                DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: Result = {0}", result);

                DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: Done.");

                return result;
            }

            // Dump the variable userToken to the log
            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: userToken = '{0}'", userToken);

            DebugUtils.WriteLine(DebugLevel.Info,
                "GitHubHelper.GetLatestRelease: Checking whether the 'userToken' required parameter is blank...");

            if (string.IsNullOrWhiteSpace(userToken))
            {
                DebugUtils.WriteLine(DebugLevel.Error,
                    "GitHubHelper.GetReleaseAsset: Blank value passed for 'userToken' parameter. This parameter is required.");

                DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: Result = {0}", result);

                return result;
            }

            DebugUtils.WriteLine(DebugLevel.Info,
                "GitHubHelper.GetLatestRelease: The 'userToken' parameter is not blank.  Continuing...");

            try
            {
                // Overwrite the destination file if it exists

                DebugUtils.WriteLine(DebugLevel.Info,
                    $"GitHubHelper.GetReleaseAsset: Checking whether the file '{destinationFile}' exists. If so, we need to overwrite it.");

                if (File.Exists(destinationFile))
                {
                    DebugUtils.WriteLine(DebugLevel.Info,
                        $"GitHubHelper.GetReleaseAsset: The file '{destinationFile}' exists.  We are overwriting it.");

                    File.Delete(destinationFile);

                    DebugUtils.WriteLine(DebugLevel.Info,
                        $"GitHubHelper.GetReleaseAsset: The existing file '{destinationFile}' has been deleted.");
                }

                DebugUtils.WriteLine(DebugLevel.Info, "GitHubHelper.GetReleaseAsset: Initializing WebClient...");

                using (var client = new WebClient())
                {
                    DebugUtils.WriteLine(DebugLevel.Info,
                        $"GitHubHelper.GetReleaseAsset: WebClient object initialized.  Requesting data from '{url}'...");

                    /* We make the API call but instead of setting the "Accept" header to be
                     application/vnd.github.v3.raw, we set it to application/octet-stream. */
                    client.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
                    client.Headers.Add(HttpRequestHeader.UserAgent, Resources.PROGRAM_NAME);
                    client.Headers.Add(HttpRequestHeader.Authorization, $"token {userToken}");

                    /* OKAY: This call SHOULD work */
                    client.DownloadFile(url, destinationFile);

                    DebugUtils.WriteLine(DebugLevel.Info, "GitHubHelper.GetReleaseAsset: Finished retrieving content.");

                    DebugUtils.WriteLine(DebugLevel.Info,
                        "GitHubHelper.GetReleaseAsset: Releasing the resources consumed by the WebClient object...");
                }

                DebugUtils.WriteLine(DebugLevel.Info,
                    "GitHubHelper.GetReleaseAsset: Resources consumed by the WebClient have been freed.");

                /* base the result of whether this function has succeeded on whether or not the destination
                   file was actually written to the user's computer. */
                result = File.Exists(destinationFile);
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                // set the result variable to false
            }

            DebugUtils.WriteLine(DebugLevel.Info,
                result
                    ? "GitHubHelper.GetReleaseAsset: The destination file '{0}' exists on the disk. {1} B retrieved."
                    : "GitHubHelper.GetReleaseAsset: The destination file '{0}' could not be downloaded. {1} B retrieved.",
                destinationFile, result ? new FileInfo(destinationFile).Length : 0);

            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: Result = {0}", result);

            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetReleaseAsset: Done.");

            return result;
        }

        /// <summary>
        /// Gets string data from the resource at the specified url and returns the data retrieved, or the empty string if an error occurred.
        /// </summary>
        /// <param name="url">URL of the data to be downloaded.</param>
        /// <param name="userToken">User token for personal access to the GitHub API.</param>
        /// <returns>String containing the JSON response from the API.</returns>
        public static string GetLatestRelease(string url, string userToken)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(DebugLevel.Debug, "In GitHubHelper.GetLatestRelease");

            var result = string.Empty;

            // Dump the variable url to the log
            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetLatestRelease: url = '{0}'", url);

            if (string.IsNullOrWhiteSpace(url))
            {
                DebugUtils.WriteLine(DebugLevel.Error,
                    "GitHubHelper.GetLatestRelease: Blank value passed for the 'url' parameter. This parameter is required.");

                DebugUtils.WriteLine(DebugLevel.Info, "GitHubHelper.GetLatestRelease: {0} B of web content retrieved.",
                    result.Length);

                return result;
            }

            // Dump the variable userToken to the log
            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetLatestRelease: userToken = '{0}'", userToken);

            DebugUtils.WriteLine(DebugLevel.Info,
                "GitHubHelper.GetLatestRelease: Checking whether the 'userToken' required parameter is blank...");

            if (string.IsNullOrWhiteSpace(userToken))
            {
                DebugUtils.WriteLine(DebugLevel.Error,
                    "GitHubHelper.GetLatestRelease: Blank value passed for 'userToken' parameter. This parameter is required.");

                DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetLatestRelease: Result = {0}", result);

                return result;
            }

            DebugUtils.WriteLine(DebugLevel.Info,
                "GitHubHelper.GetLatestRelease: The 'userToken' parameter is not blank.  Continuing...");

            try
            {
                DebugUtils.WriteLine(DebugLevel.Info,
                    "GitHubHelper.GetLatestRelease: Enabling TLS 1.2 security protocol...");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                DebugUtils.WriteLine(DebugLevel.Info, "GitHubHelper.GetLatestRelease: TLS 1.2 protocol enabled.");

                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("User-Agent", Resources.PROGRAM_NAME);
                request.AddHeader("Authorization", $"token {userToken}");
                request.AddHeader("Accept", "application/vnd.github.v3.raw");
                var response = new RestClient(url).Execute(request);

                result = response?.Content;
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                result = string.Empty;
            }

            // Normally, it is our custom to log the return value of a method such as this, which returns a value of type string.
            // However, in this particular case, in the best-case scenario, the return value from a successful GitHub API call 
            // is a massively-long JSON formatted value, and it would choke the logging subsystem to actually write out the received
            // JSON text to the log, so we skip it.  Instead, we write out how many bytes were received.
            if (!string.IsNullOrWhiteSpace(result))
            {
                DebugUtils.WriteLine(DebugLevel.Info, "GitHubHelper.GetLatestRelease: {0} B received from GitHub API.",
                    result.Length);
            }

            DebugUtils.WriteLine(DebugLevel.Debug, "GitHubHelper.GetLatestRelease: Done.");

            return result;
        }
    }
}
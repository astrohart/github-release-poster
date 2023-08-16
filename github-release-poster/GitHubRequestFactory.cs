using System;
using github_release_poster.Properties;
using RestSharp;

namespace github_release_poster
{
    /// <summary>
    /// Produces new GitHub REST request objects.
    /// </summary>
    public static class GitHubRequestFactory
    {
        /// <summary>
        /// Prepares the headers for a GitHub request.
        /// </summary>
        /// <param name="method">One of the <see cref="T:RestSharp.Method"/> values specifying the HTTP method
        /// for this request.</param>
        /// <param name="userAccessToken">String containing the user's GitHub access token.</param>
        /// <returns>Reference to an instance of <see cref="T:RestSharp.RestRequest"/> that forms the basis
        /// for the new request with the proper headers.</returns>
        public static RestRequest PrepareGitHubRequest(RestSharp.Method method, string userAccessToken)
        {
            if (string.IsNullOrWhiteSpace(userAccessToken))
                throw new ArgumentNullException(nameof(userAccessToken));

            var result = new RestRequest(method);

            result.AddHeader(Resources.CacheControlHeaderName, Resources.NoCacheHeader);
            result.AddHeader(Resources.UserAgentHeaderName, Resources.GitHubReleasePosterUserAgent);
            result.AddHeader(Resources.AuthorizationHeaderName, string.Format(Resources.GitHubAuthorizationHeaderContent, 
                userAccessToken));

            return result;
        }
    }
}
using System;
using System.IO;

namespace github_release_poster
{
    public static class GitHubReleaseValidator
    {
        public static bool IsReleaseValid(NewRelease release,
            string repoName, string repoOwner, string userAcessToken,
            string releaseAssetDir)
        {
            Console.WriteLine("Validating release metadata...");

            var result = release != null
                         && !string.IsNullOrWhiteSpace(release.tag_name)
                         && !string.IsNullOrWhiteSpace(release.name)
                         && !string.IsNullOrWhiteSpace(release.target_commitish)
                         && !string.IsNullOrWhiteSpace(repoName)
                         && !string.IsNullOrWhiteSpace(repoOwner)
                         && !string.IsNullOrWhiteSpace(userAcessToken)
                         && !string.IsNullOrWhiteSpace(releaseAssetDir)
                         && Directory.Exists(releaseAssetDir);

            Console.WriteLine(result
                ? "Release data has been validated.  No errors have been found."
                : "Failed to validate the release data.");

            return result;
        }
    }
}
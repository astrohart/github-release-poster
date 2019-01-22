using System;

namespace github_release_poster
{
    public static class GitHubReleaseValidator
    {
        public static bool IsReleaseValid(NewRelease release)
        {
            Console.WriteLine("Validating release metadata...");

            // TODO: Add more checks here

            return release != null;
        }
    }
}
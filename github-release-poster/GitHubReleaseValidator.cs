using System;

namespace github_release_poster
{
    public static class GitHubReleaseValidator
    {
        public static bool IsReleaseValid(NewRelease release)
        {
            Console.WriteLine("Validating release metadata...");
            if (release == null)
                return false;

            return true;
        }
    }
}
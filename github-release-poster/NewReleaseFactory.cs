using System;

namespace github_release_poster
{
    public static class NewReleaseFactory
    {
        public static NewRelease CreateNewRelease(string body, bool isDraft, string name, bool isPreRelease,
            string tagName, string targetBranchName)
        {
            Console.WriteLine("Initializing new release metadata...");

            var result = new NewRelease()
            {
                body = body,
                draft = isDraft,
                name = name,
                prerelease = isPreRelease,
                tag_name = tagName,
                target_commitish = targetBranchName
            };

            return result;
        }
    }
}
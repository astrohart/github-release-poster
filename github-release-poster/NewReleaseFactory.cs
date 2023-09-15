using System;

namespace github_release_poster
{
    /// <summary>
    /// Provides methods and functionality for creating and initializing new
    /// instances of <see cref="T:github_release_poster.NewRelease" />.
    /// </summary>
    public static class NewReleaseFactory
    {
        /// <summary>
        /// Creates a new instance of
        /// <see cref="T:github_release_poster.NewRelease" /> from the data provided in
        /// this method's parameters.
        /// </summary>
        /// <param name="body">String containing the body content for this release.</param>
        /// <param name="isDraft">True if this release is to be narked as a Draft.</param>
        /// <param name="name">String containing the name of this release.</param>
        /// <param name="isPreRelease">True if this release is to be marked as Pre-Release.</param>
        /// <param name="tagName">
        /// String containing the name of the Git tag to be created
        /// and associated with the commit for this release.
        /// </param>
        /// <param name="targetBranchName">
        /// String containing the name of the target Git
        /// branch.
        /// </param>
        /// <returns>
        /// Reference to an instance of
        /// <see cref="T:github_release_poster.NewRelease" /> whose properties are
        /// initialized to the values passed to this method.  No validation is done.
        /// </returns>
        /// <remarks>
        /// Applications should use a validator object on the output of this
        /// method before using this object in any API calls.
        /// </remarks>
        public static NewRelease CreateNewRelease(
            string body,
            bool isDraft,
            string name,
            bool isPreRelease,
            string tagName,
            string targetBranchName
        )
        {
            Console.WriteLine("Initializing new release metadata...");

            var result = new NewRelease
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
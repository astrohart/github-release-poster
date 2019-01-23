using System;
using System.IO;
using System.Linq;
using github_release_poster.Properties;

namespace github_release_poster
{
    /// <summary>
    /// Validates releases.
    /// </summary>
    public static class GitHubReleaseValidator
    {
        /// <summary>
        /// Validates the release specified.
        /// </summary>
        /// <param name="release">Reference to an instance of <see cref="github_release_poster.NewRelease"/>
        /// that contains the metadata for the release.</param>
        /// <param name="repoName">String containing the name of the repository to which this release is to be published.</param>
        /// <param name="repoOwner">String containing the GitHub username of the owner of the repository.</param>
        /// <param name="userAcessToken">String containing the user access token of a GitHub user with push access to the repository.</param>
        /// <param name="releaseAssetDir">String containing the path to the directory where release assets are stored.</param>
        /// <returns>True if the release is valid; false otherwise.  Terminates the program with a nonzero exit code if the
        /// release does not pass validation.</returns>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <see cref="release"/> argument is a null
        /// reference of if any of the string arguments are blank.</exception>
        public static bool IsReleaseValid(NewRelease release,
            string repoName, string repoOwner, string userAcessToken,
            string releaseAssetDir)
        {
            Console.WriteLine(Resources.ValidatingReleaseMetadata);

            // reject a null reference to the release object itself
            if (release == null) throw new ArgumentNullException(nameof(release));

            if (string.IsNullOrWhiteSpace(release.tag_name)) 
                throw new ArgumentNullException(nameof(release.tag_name));

            if (string.IsNullOrWhiteSpace(release.name)) 
                throw new ArgumentNullException(nameof(release.name));

            if (string.IsNullOrWhiteSpace(release.target_commitish)) 
                throw new ArgumentNullException(nameof(release.target_commitish));

            if (string.IsNullOrWhiteSpace(repoName)) 
                throw new ArgumentNullException(nameof(repoName));

            if (string.IsNullOrWhiteSpace(repoOwner)) 
                throw new ArgumentNullException(nameof(repoOwner));

            if (string.IsNullOrWhiteSpace(userAcessToken)) 
                throw new ArgumentNullException(nameof(userAcessToken));

            if (string.IsNullOrWhiteSpace(releaseAssetDir)) 
                throw new ArgumentNullException(nameof(releaseAssetDir));

            /* If the release asset directory does not yet exist, then the build must have failed! LOL.
             However, let's tell the user just to be sure. */
            if (!Directory.Exists(releaseAssetDir))
            {
                Console.WriteLine(Resources.ReleaseAssetDirNotFound, releaseAssetDir);
                Environment.Exit(exitCode: Resources.ERROR_RELEASE_ASSET_DIR_NOT_EXISTS);
            }

            /* The GitHub release API allows an unlimited number of assets to be posted to a release.  However,
             it limits the size of any single file to be no more than 2 GB in size.  Scan the release asset dir.
             If it, or any of its subdirectories (that the current user can access, anyway) contains a file that is
             2 GB or more in size, stop this program after displaying an error.*/
            if (FileSearcher.GetAllFilesInFolder(releaseAssetDir)
                .Where(fsi => (fsi.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
                .Any(fsi => Convert.ToUInt64(new FileInfo(fsi.FullName).Length) >= Resources.TwoGigaBytes))
            {
                Console.WriteLine(Resources.ReleaseAssetDirContainsTooBigFile);
                Environment.Exit(Resources.ERROR_RELEASE_ASSET_IS_TOO_BIG);
            }

            Console.WriteLine(Resources.ReleaseAssetsAndMetadataValid);
            return true;
        }
    }
}
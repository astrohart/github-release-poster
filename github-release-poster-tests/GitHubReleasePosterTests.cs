using github_release_poster;
using github_release_poster.Properties;
using NUnit.Framework;

namespace github_release_poster_tests
{
    [TestFixture]
    public class GitHubReleasePosterTests
    {
        /// <summary>
        /// Put your user access token that is used with GitHub here, for testing.  If there is a token here, it has
        /// most likely already been revoked, so don't think you can use it.
        /// </summary>
        private const string MyUserAccessToken = "<your-user-token-here>";

        /// <summary>
        /// Tests the GitHubReleasePoster class, and it tells it to zip up the assets before uploading.
        /// </summary>
        /// <remarks>
        /// Zipping the ass
        /// </remarks>
        [Test]
        public void PostNewReleaseWithNoZip()
        {
            // TODO: Before running this test, you must edit the Resources.Testing* values used below to match your own repo and account!

            var result = GitHubReleasePoster.PostNewRelease(
                Resources.TestingRepoName,
                Resources.TestingRepoOwner,
                MyUserAccessToken,
                Resources.TestingAssetsSourceDirPath,
                NewReleaseFactory.CreateNewRelease(
                    string.Empty,   /* Pass in something different to test the body */
                    true, /* make this release a draft, so that it does not interrupt any release trains */
                    Resources.GitHubIndivAssetTestingReleaseName,
                    false,
                    $"v{Resources.GitHubIndivAssetTestingReleaseName}",  /* ostensibly, a release name is a version number, but it does not have to be */
                    Resources.MasterBranchName),
                true
            );

            Assert.IsTrue(result, Resources.PostReleaseTestNotSucceeded);
        }

        [Test]
        public void PostNewReleaseWithZip()
        {
            // TODO: Before running this test, you must edit the Resources.Testing* values used below to match your own repo and account!

            var result = GitHubReleasePoster.PostNewRelease(
                Resources.TestingRepoName,
                Resources.TestingRepoOwner,
                MyUserAccessToken,
                Resources.TestingAssetsSourceDirPath,
                NewReleaseFactory.CreateNewRelease(
                    string.Empty,   /* Pass in something different to test the body */
                    true, /* make this release a draft, so that it does not interrupt any release trains */
                    Resources.GitHubZipAssetsTestingReleaseName,
                    false,
                    $"v{Resources.GitHubZipAssetsTestingReleaseName}",  /* ostensibly, a release name is a version number, but it does not have to be */
                    Resources.MasterBranchName),
                false
            );

            Assert.IsTrue(result, Resources.PostReleaseTestNotSucceeded);
        }
    }
}
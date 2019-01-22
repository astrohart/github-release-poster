using github_release_poster;
using NUnit.Framework;
using System;
using Resources = github_release_poster_tests.Properties.Resources;

namespace github_release_poster_tests
{
    [TestFixture]
    public class GitHubReleaseValidatorTests
    {
        /// <summary>
        /// Test the <see cref="M:github_release_poster.GitHubReleaseValidator.IsReleaseValid"/>
        /// method to determine whether it correctly deems a validly-composed release valid.
        /// </summary>
        [Test]
        public void IsReleaseValidTest()
        {
            var releaseNameAndTag = Guid.NewGuid().ToString();

            var release = NewReleaseFactory.CreateNewRelease(
                Resources.TestingValidReleaseBody,
                true,
                releaseNameAndTag,
                true,
                releaseNameAndTag,
                Resources.TestingReleaseTargetBranch
            );

            Assert.IsTrue(GitHubReleaseValidator.IsReleaseValid(release,
                Resources.TestingReleaseRepoName,
                Resources.TestingReleaseRepoOwner,
                Guid.NewGuid().ToString()
                    .Replace("-", string.Empty),
                Resources.TestingValidReleaseAssetDir
                )
            );
        }
    }
}
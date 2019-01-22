using github_release_poster;
using github_release_poster_tests.Properties;
using NUnit.Framework;
using System;

namespace github_release_poster_tests
{
    /// <summary>
    /// Provides unit tests for the methods in the <see cref="T:github_release_poster.NewReleaseFactory"/> class.
    /// </summary>
    [TestFixture]
    public class NewReleaseFactoryTests
    {
        [Test]
        public void CreateNewReleaseTest()
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

            Assert.IsNotNull(release);

            Assert.IsTrue(release.draft);
            Assert.IsTrue(release.prerelease);

            Assert.IsNotEmpty(release.name);
            Assert.AreEqual(release.name, releaseNameAndTag);
            Assert.IsNotEmpty(release.tag_name);
            Assert.AreEqual(release.tag_name, releaseNameAndTag);
            Assert.IsNotEmpty(release.target_commitish);
            Assert.AreEqual(release.target_commitish, Resources.TestingReleaseTargetBranch);
            Assert.IsNotEmpty(release.body);
            Assert.AreEqual(release.body, Resources.TestingValidReleaseBody);
        }
    }
}
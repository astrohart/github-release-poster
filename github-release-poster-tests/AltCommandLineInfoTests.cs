using System;
using github_release_poster;
using NUnit.Framework;
using System.Linq;

namespace github_release_poster_tests
{
    /// <summary>
    /// This file would otherwise be named CommandLineInfoTests but this is to avoid pushing
    /// a file to the repo that has a valid user access token embedded in it
    /// </summary>
    [TestFixture]
    public class AltCommandLineInfoTests
    {
        /// <summary>
        /// Valid command-line arguments for testing.  Since we have sensitive information (i.e., my user access token) in this file,
        /// it will not be pushed to the repo in the cloud.
        /// </summary>
        /// <remarks>Represents the argument string --name Testing --release-asset-dir "C:\Users\ENS Brian Hart\source\repos\github-release-poster\github-release-poster\bin\x64\Debug" --tag-name Testing --target-branch master --user-access-token &lt;token&gt; --repo-name github-releasse-poster --repo-owner astrohart</remarks>
        public static string[] ValidTestingArgs { get; } =
        {
            "--repo-owner astrohart",
            "--repo-name github-release-poster",
            "--user-access-token <token>",
            "--target-branch master",
            $"--tag-name {Guid.NewGuid()}",
            @"--release-asset-dir ""C:\Users\ENS Brian Hart\source\repos\github-release-poster\github-release-poster\bin\x64\Debug""",
            $"--name {Guid.NewGuid()}"
        };

        /// <summary>
        /// Tests parsing the command line with a valid set of arguments.
        /// </summary>
        [Test]
        public void ParseCommandLineWithValidArgs()
        {
            Assert.IsTrue(
                CommandLineInfo.Instance.ParseCommandLine(ValidTestingArgs));

            Assert.IsTrue(CommandLineInfo.Instance.WasCommandLineParsed);
        }

        [Test]
        public void ParseCommandLineTesWithNoArgs()
        {
            Assert.IsFalse(
                CommandLineInfo.Instance.ParseCommandLine(Enumerable.Empty<string>().ToArray())
            );

            Assert.IsFalse(CommandLineInfo.Instance.WasCommandLineParsed);
        }
    }
}
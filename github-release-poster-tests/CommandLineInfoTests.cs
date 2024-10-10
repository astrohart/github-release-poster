using PostSharp.Patterns.Threading;
using github_release_poster;
using NUnit.Framework;
using System;
using System.Linq;

namespace github_release_poster_tests
{
    [TestFixture, ExplicitlySynchronized]
    public class CommandLineInfoTests
    {
        /// <summary>
        /// Valid command-line arguments for testing.  This set of arguments
        /// represents the full set, including optional ones.
        /// </summary>
        public static string[] ValidFullTestingArgs { get; } =
        {
            "--repo-owner", "astrohart", "--repo-name",
            " github-release-poster", "--user-access-token",
            $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}",
            "--target-branch", "master", "--tag-name", $"{Guid.NewGuid()}",
            "--release-asset-dir",
            @"C:\Users\ENS Brian Hart\source\repos\github-release-poster\github-release-poster\bin\x64\Debug",
            "--name", $"{Guid.NewGuid()}", "--is-draft", "--is-pre-release",
            "--body", "Testing foo bar", "--no-zip"
        };

        /// <summary>
        /// Valid command-line arguments for testing.  Since we have sensitive
        /// information (i.e., my user access token) in this file, it will not be pushed to
        /// the repo in the cloud.
        /// </summary>
        /// <remarks>
        /// Represents the argument string --name Testing --release-asset-dir
        /// "C:\Users\ENS Brian
        /// Hart\source\repos\github-release-poster\github-release-poster\bin\x64\Debug"
        /// --tag-name Testing --target-branch master --user-access-token &lt;guid with no
        /// dashes&gt; --repo-name github-release-poster --repo-owner astrohart
        /// </remarks>
        public static string[] ValidTestingArgs { get; } =
        {
            "--repo-owner", "astrohart", "--repo-name",
            " github-release-poster", "--user-access-token",
            $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}",
            "--target-branch", "master", "--tag-name", $"{Guid.NewGuid()}",
            "--release-asset-dir",
            @"C:\Users\ENS Brian Hart\source\repos\github-release-poster\github-release-poster\bin\x64\Debug",
            "--name", $"{Guid.NewGuid()}"
        };

        [Test]
        public void ParseCommandLineTesWithNoArgs()
        {
            Assert.IsFalse(
                CommandLineInfo.Instance.ParseCommandLine(
                    Enumerable.Empty<string>()
                              .ToArray()
                )
            );

            Assert.IsFalse(CommandLineInfo.Instance.WasCommandLineParsed);
        }

        /// <summary> Tests parsing the command line with a valid set of arguments. </summary>
        [Test]
        public void ParseCommandLineWithValidArgs()
        {
            Assert.IsTrue(ValidTestingArgs.Any());

            Assert.IsTrue(
                CommandLineInfo.Instance.ParseCommandLine(ValidTestingArgs)
            );

            Assert.IsTrue(CommandLineInfo.Instance.WasCommandLineParsed);
        }

        [Test]
        public void ParseCommandLineWithValidFullSetOfArgs()
        {
            Assert.IsTrue(ValidFullTestingArgs.Any());

            Assert.IsTrue(
                CommandLineInfo.Instance.ParseCommandLine(ValidFullTestingArgs)
            );

            Assert.IsTrue(CommandLineInfo.Instance.WasCommandLineParsed);
        }
    }
}
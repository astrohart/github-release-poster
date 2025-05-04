using PostSharp.Patterns.Threading;
using github_release_poster;
using NUnit.Framework;
using System.IO;

namespace github_release_poster_tests
{
    [TestFixture, ExplicitlySynchronized]
    public class LoggingSubsystemManagerTests
    {
        /// <summary> Path to the application configuration file. </summary>
        private const string AppConfigPath =
            @"C:\Users\ENS Brian Hart\source\repos\github-release-poster\github-release-poster\App.config";

        /// <summary>
        /// Tests the
        /// <see cref="M:github_release_poster.LoggingSubsystemManager.InitializeLogging" /> method
        /// being called with no parameters.
        /// </summary>
        [Test]
        public void InitializeLoggingTestWithNoParams()
        {
            LoggingSubsystemManager.InitializeLogging();

            AssertLogFileInitialized();
        }

        /// <summary>
        /// Tests the
        /// <see cref="M:github_release_poster.LoggingSubsystemManager.InitializeLogging" /> method.
        /// </summary>
        [Test]
        public void InitializeLoggingTestWithLogFilePath()
        {
            Assert.IsTrue(File.Exists(AppConfigPath));

            LoggingSubsystemManager.InitializeLogging(false, true, AppConfigPath);

            AssertLogFileInitialized();
        }

        private static void AssertLogFileInitialized()
            => Assert.IsTrue(
                LoggingSubsystemManager.IsLoggingInitialized,
                "Failed to initialize the log file."
            );
    }
}
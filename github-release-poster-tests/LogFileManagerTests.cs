using System.IO;
using github_release_poster;
using NUnit.Framework;

namespace github_release_poster_tests
{
    [TestFixture]
    public class LogFileManagerTests
    {
        /// <summary>
        /// Path to the application configuration file.
        /// </summary>
        private const string AppConfigPath = 
            @"C:\Users\ENS Brian Hart\source\repos\github-release-poster\github-release-poster\App.config";

        /// <summary>
        /// Tests the <see cref="M:github_release_poster.LogFileManager.InitializeLogging"/> method being called
        /// with no parameters.
        /// </summary>
        [Test]
        public void InitializeLoggingTestWithNoParams()
        {
            LogFileManager.InitializeLogging();

            AssertLogFileInitialized();
        }

        /// <summary>
        /// Tests the <see cref="M:github_release_poster.LogFileManager.InitializeLogging"/> method.
        /// </summary>
        [Test]
        public void InitializeLoggingTestWithLogFilePath()
        {
            Assert.IsTrue(File.Exists(AppConfigPath));

            LogFileManager.InitializeLogging(false,
                true, AppConfigPath);

            AssertLogFileInitialized();
        }

        private static void AssertLogFileInitialized()
        {
            Assert.IsTrue(LogFileManager.IsLoggingInitialized, "Failed to initialize the log file.");
        }
    }
}
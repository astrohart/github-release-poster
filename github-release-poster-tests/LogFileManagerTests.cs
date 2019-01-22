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
        /// Tests the <see cref="M:github_release_poster.LogFileManager.InitializeLogging"/> method.
        /// </summary>
        [Test]
        public void InitializeLoggingTest()
        {
            Assert.IsTrue(File.Exists(AppConfigPath));

            LogFileManager.InitializeLogging(false,
                true, AppConfigPath);

            Assert.IsTrue(LogFileManager.IsLoggingInitialized, "Failed to initialize the log file.");
        }
    }
}
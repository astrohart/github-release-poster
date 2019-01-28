using System.IO;
using GitHubReleasePoster.Extensions;
using NUnit.Framework;

namespace GitHubReleasePoster.Tests.Extensions
{
    /// <summary>
    /// Unit tests for the <see cref="T:GitHubReleasePoster.Extensions.FileSystemInfoExtensions"/> class.
    /// </summary>
    [TestFixture]
    public class FileSystemInfoExtensionsTests
    {
        [Test]
        public void IsFolder_ForAFile_ReturnsFalse()
        {
            var info = new FileInfo(@"C:\WINDOWS\Notepad.exe");

            // The IsFolder property should return false for the
            // C:\WINDOWS\Notepad.exe file, since we have a file
            // and not a folder.
            Assert.That(info.IsFolder, Is.False);
        }

        [Test]
        public void IsFolder_ForAFolder_ReturnsTrue()
        {
            var info = new FileInfo(@"C:\WINDOWS");

            // The IsFolder property should return true for the
            // C:\WINDOWS directory.
            Assert.That(info.IsFolder, Is.True);
        }
    }
}
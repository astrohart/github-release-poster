using System.IO;
using System.Linq;
using GitHubReleasePoster.Extensions;
using GitHubReleasePoster.Searchers;
using NUnit.Framework;

namespace GitHubReleasePoster.Tests.Searchers
{
    [TestFixture]
    public class FileSystemSearcherTests
    {
        /// <summary>
        /// Tests whether a <see cref="T:System.IO.DirectoryNotFoundException"/> is thrown when
        /// a file's pathname is passed, instead of a directory, for where to start the search.
        /// </summary>
        [Test]
        public void FileSystemSearcher_NonDirectorySpecified_ThrowsException()
        {
            Assert.Throws<DirectoryNotFoundException>(() =>
                FileSystemSearcher.Search(@"C:\WINDOWS\Notepad.exe"));
        }

        /// <summary>
        /// Tests whether the <see cref="T:GitHubReleasePoster.Searchers.FileSystemSearcher.Search"/> method
        /// throws a <see cref="T:System.IO.DirectoryNotFoundException"/> when we specify a gibberish
        /// name for the directory to list files in.
        /// </summary>
        [Test]
        public void FileSystemSearcher_NonExistentDirectorySpecified_ThrowsException()
        {
            Assert.Throws<DirectoryNotFoundException>(() =>
                FileSystemSearcher.Search(@"C:\asdlkkfjsalkdfjsadklfj"));
        }

        /// <summary>
        /// Tests whether the <see cref="T:GitHubReleasePoster.Searchers.FileSystemSearcher.Search"/> method
        /// only returns the pathnames of files, and not folders, in the specified directory.
        /// </summary>
        [Test]
        public void FileSystemSearcher_SearchingWindowsDirAllFilesAllDirectories_GetsFilesOnly()
        {
            Assert.That(FileSystemSearcher.Search(@"C:\WINDOWS")
                .All(f => !f.IsFolder()));
        }

        /// <summary>
        /// Tests whether the <see cref="T:GitHubReleasePoster.Searchers.FileSystemSearcher.Search"/> method
        /// only returns file path names when we are searching just the top directory level only.
        /// </summary>
        [Test]
        public void FileSystemSearcher_SearchingWindowsDirAllFilesTopDirectoryOnly_GetsFilesOnly()
        {
            Assert.That(FileSystemSearcher.Search(@"C:\WINDOWS", "*",
                SearchOption.TopDirectoryOnly).All(f => !f.IsFolder()));
        }

        /// <summary>
        /// Tests whether filtering by *.exe works.
        /// </summary>
        [Test]
        public void FileSystemSearcher_SearchingWindowsDirExeFilesOnlyTopDirectoryOnly_GetsExeFilesOnly()
        {
            var results = FileSystemSearcher
                .Search(@"C:\WINDOWS", "*.exe",
                    SearchOption.TopDirectoryOnly);
            Assert.That(results.All(f => !f.IsFolder()
                && Path.GetExtension(f.FullName.ToLowerInvariant()) == ".exe"));
        }

        /// <summary>
        /// Tests whether we can get a list of all the *.txt files in all levels of the
        /// specified directory.
        /// </summary>
        [Test]
        public void FileSystemSearcher_SearchingWindowsDirTxtFilesOnlyAllDirectories_GetsTxtFilesOnly()
        {
            var results = FileSystemSearcher
                .Search(@"C:\WINDOWS", "*.txt");
            Assert.That(results.All(f => !f.IsFolder()
                && Path.GetExtension(f.FullName.ToLowerInvariant()).StartsWith(".txt")));
        }
    }
}
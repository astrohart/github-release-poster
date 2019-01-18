using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace github_release_poster
{
    /// <summary>
    /// Searches the file system for a particular file, starting from a root folder.
    /// </summary>
    public static class FileSearcher
    {
        /// <summary>
        /// Searches for the file whose name is given by <see cref="fileName"/> recursively starting with the folder <see cref="rootDir"/>.
        /// </summary>
        /// <param name="fileName">Name of the file you want to find.</param>
        /// <param name="rootDir">Path to the directory where you want to start the search.</param>
        /// <returns>Full path to the first copy of the file whose name matches <see cref="fileName"/>; blank string if not found.</returns>
        public static string FindFile(string fileName, string rootDir = @"C:\")
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            if (!Directory.Exists(rootDir))
            {
                return string.Empty;
            }

            var foundFiles = new FileSystemEnumerable(new DirectoryInfo(rootDir),
                fileName, SearchOption.AllDirectories);
            return !foundFiles.Any() ? string.Empty : foundFiles.First().FullName;
        }

        /// <summary>
        /// Recursively gets an enumerator of all the files in the specified folder whose names conform to the specified search pattern and option and skips any directories
        /// where the current user does not have sufficient access privileges to list files.
        /// </summary>
        /// <param name="folderPath">Full path to the starting folder for the search.  The folder must already exist on the disk.</param>
        /// <param name="searchPattern">Search pattern conforming to the rules for the <see cref="M:DirectoryInfo.EnumerateFiles"/> method.</param>
        /// <param name="option">One of the <see cref="T:System.IO.SearchOption"/> values specifying the depth of the search.</param>
        /// <returns>Reference to a <see cref="T:System.Collections.Generic.IEnumerable"/> of <see cref="T:System.IO.FileSystemInfo"/> objects that represent the
        /// files found, or the empty enumerable if the <see cref="folderPath"/> could not be located on disk or if no resuls were returned from the search.</returns>
        public static IEnumerable<FileSystemInfo> GetAllFilesInFolder(string folderPath, string searchPattern = "*", SearchOption option = SearchOption.AllDirectories)
        {
            return !Directory.Exists(folderPath)
                ? Enumerable.Empty<FileSystemInfo>()
                : new FileSystemEnumerable(new DirectoryInfo(folderPath), searchPattern, option);
        }
    }
}

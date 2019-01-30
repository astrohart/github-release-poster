using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitHubReleasePoster.Extensions;

namespace GitHubReleasePoster.Searchers
{
    /// <summary>
    /// Methods to search the user's file system.
    /// </summary>
    public class FileSystemSearcher
    {
        /// <summary>
        /// Returns a list of instances of <see cref="T:System.IO.FileSystemInfo"/> that refer
        /// to the files in the specified folder and which match the specified search pattern,
        /// conforming to the search option given.
        /// </summary>
        /// <param name="directory">String containing the full path to the directory to start the search in.</param>
        /// <param name="pattern">String containing wildcards that specifies a filter for the search, or "*" for all files.</param>
        /// <param name="option">One of the <see cref="T:System.IO.SearchOption"/> values specifying the depth of the search.</param>
        /// <remarks>The return value is an enumerator on the list.  Files that are protected by the operating system or are
        /// in use by other applications are skipped.</remarks>
        /// <returns>An enumerator into the list of files found that conform to the given search pattern and depth.</returns>
        public static IEnumerable<FileSystemInfo> Search(string directory, string pattern = "*", SearchOption option = SearchOption.AllDirectories)
        {
            if (directory == null) throw new ArgumentNullException(nameof(directory));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"The folder '{directory}' could not be located.");

            // we only want files, but sometimes entries with Folder attributes get returned,
            // so we filter out all the entries returned that are for Folders.
            var result =
                new FileSystemEnumerable(
                    new DirectoryInfo(directory),
                    pattern,
                    option)
                    .Where(f => !f.IsFolder());

            return result;
        }
    }
}
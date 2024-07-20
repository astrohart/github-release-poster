using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace github_release_poster
{
    // This code is public domain
    // Shout out to Matthew Brubaker who posted it at:
    // <https://stackoverflow.com/questions/13130052/directoryinfo-enumeratefiles-causes-unauthorizedaccessexception-and-other>
    public class FileSystemEnumerable : IEnumerable<FileSystemInfo>
    {
        private readonly ILog _logger =
            LogManager.GetLogger(typeof(FileSystemEnumerable));

        private readonly SearchOption _option;
        private readonly IList<string> _patterns;
        private readonly DirectoryInfo _root;

        /// <summary>
        /// Constructs a new instance of
        /// <see cref="T:github_release_poster.FileSystemEnumerable" /> and returns a
        /// reference to it.
        /// </summary>
        public FileSystemEnumerable(
            DirectoryInfo root,
            string pattern,
            SearchOption option
        )
        {
            _root = root;
            _patterns = new List<string> { pattern };
            _option = option;
        }

        /// <summary>
        /// Constructs a new instance of
        /// <see cref="T:github_release_poster.FileSystemEnumerable" /> and returns a
        /// reference to it.
        /// </summary>
        public FileSystemEnumerable(
            DirectoryInfo root,
            IList<string> patterns,
            SearchOption option
        )
        {
            _root = root;
            _patterns = patterns;
            _option = option;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<FileSystemInfo> GetEnumerator()
        {
            if (_root == null || !_root.Exists)
                yield break;

            IEnumerable<FileSystemInfo> matches = new List<FileSystemInfo>();

            try
            {
                _logger.DebugFormat(
                    $"Attempting to enumerate '{_root.FullName}'"
                );
                foreach (var pattern in _patterns)
                {
                    _logger.DebugFormat($"Using pattern '{pattern}'");
                    matches = matches.Concat(
                                         _root.EnumerateDirectories(
                                             pattern,
                                             SearchOption.TopDirectoryOnly
                                         )
                                     )
                                     .Concat(
                                         _root.EnumerateFiles(
                                             pattern,
                                             SearchOption.TopDirectoryOnly
                                         )
                                     );
                }
            }
            catch (UnauthorizedAccessException)
            {
                _logger.WarnFormat(
                    $"Unable to access '{_root.FullName}'. Skipping..."
                );
                yield break;
            }
            catch (PathTooLongException ptle)
            {
                _logger.Warn(
                    $@"Could not process path '{_root.Parent?.FullName}\{_root.Name}'.",
                    ptle
                );
                yield break;
            }
            catch (IOException e)
            {
                // "The symbolic link cannot be followed because its type is disabled."
                // "The specified network name is no longer available."
                _logger.Warn(
                    $@"Could not process path (check SymlinkEvaluation rules)'{_root.Parent?.FullName}\{_root.Name}'.",
                    e
                );
                yield break;
            }
            catch
            {
                // All other exceptions -- just skip it
                _logger.Warn(
                    @"Caught an unknown exception.  Perhaps we have stumbled upon a file or directory which is protected by the operating system."
                );
                yield break;
            }

            _logger.DebugFormat(
                $"Returning all objects that match the pattern(s) '{string.Join(",", _patterns)}'"
            );
            foreach (var file in matches)
                if (file != null)
                    yield return file;

            if (_option != SearchOption.AllDirectories)
                yield break;

            _logger.DebugFormat("Enumerating all child directories.");
            foreach (var dir in _root.EnumerateDirectories(
                         "*", SearchOption.TopDirectoryOnly
                     ))
            {
                _logger.DebugFormat($"Enumerating '{dir.FullName}'");
                var fileSystemInfos =
                    new FileSystemEnumerable(dir, _patterns, _option);
                foreach (var match in fileSystemInfos)
                    if (match != null)
                        yield return match;
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be
        /// used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
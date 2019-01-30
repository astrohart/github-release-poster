using System.IO;

namespace GitHubReleasePoster.Extensions
{
    /// <summary>
    /// Provides extension methods for instances of <see cref="T:System.IO.FileSystemInfo"/>.
    /// </summary>
    public static class FileSystemInfoExtensions
    {
        /// <summary>
        /// Determines whether the file system entry is a folder or a file.
        /// </summary>
        /// <param name="info">Reference to an instance of a type that is
        /// derived from <see cref="T:System.IO.FileSystemInfo"/>
        /// that refers to the file or directory you want to check.</param>
        /// <returns>True if the <see cref="T:System.IO.FileSystemInfo"/> referred to by the
        /// <see cref="info"/> parameter is a folder; false if it is a file or otherwise
        /// cannot be checked.</returns>
        public static bool IsFolder(this FileSystemInfo info)
        {
            // Can't check a null FileInfo
            if (info == null)
                return false;

            return (info.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        /// <summary>
        /// Checks whether the specified <see cref="file"/> is locked for reading/writing
        /// by another process or thread.
        /// </summary>
        /// <param name="file">Reference to an instance of an object that is derived from
        /// <see cref="T:System.IO.FileSystemInfo"/> that refers to a file system entry. The
        /// entry must be that for a file.
        /// </param>
        /// <returns>True if the file is locked by someone else; false otherwise.</returns>
        /// <remarks>For obvious reasons, a unit test of this method does not exist, since
        /// files become locked and unlocked by processes at random, so we can't hope to test
        /// this deterministically.</remarks>
        public static bool IsLocked(this FileSystemInfo file)
        {
            // Can't check a null FileInfo -- tell callers it's locked
            // so they won't try to access it
            if (file == null)
                return true;

            if (!file.Exists)
                return true;    // Don't try to open a non-existent file!

            if (file.IsFolder())
                return true;    // Don't try to check a folder

            // Attempt to open a file stream for reading on the file. If
            // an exception gets thrown, then we know we can't touch it right now
            var stream = default(FileStream);

            try
            {
                stream = File.OpenRead(file.FullName);
            }
            catch
            {
                //if we are here, then the file is unavailable
                return true;
            }
            finally
            {
                stream?.Close();
            }

            // if we are here, then the file is available
            return false;
        }
    }
}
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
    }
}
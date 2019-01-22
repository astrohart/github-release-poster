using github_release_poster.Properties;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace github_release_poster
{
    /// <summary>
    /// Produces ZIP files of directories and/or groups of files.
    /// </summary>
    public static class ZipperUpper
    {
        /// <summary>
        /// Compresses a directory full of files into a ZIP file with the same name as the directory. Does not add the
        /// directory itself to the ZIP file.
        /// </summary>
        /// <param name="directoryPath">Path to the directory containing the files to be ZIPped.</param>
        /// <param name="zipFilePath">Path to the output ZIP file.  Must not be the same as the directory that is being ZIPped.</param>
        /// <returns></returns>
        public static bool CompressDirectory(string directoryPath, string zipFilePath)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(DebugLevel.Debug, "In ZipperUpper.CompressDirectory");

            // Dump the variable directoryPath to the log
            DebugUtils.WriteLine(DebugLevel.Debug, "ZipperUpper.CompressDirectory: directoryPath = '{0}'", directoryPath);

            // Dump the variable zipFilePath to the log
            DebugUtils.WriteLine(DebugLevel.Debug, "ZipperUpper.CompressDirectory: zipFilePath = '{0}'", zipFilePath);

            if (!Directory.Exists(directoryPath))
            {
                // assume the directoryPath parameter specifies the release asset directory
                Console.WriteLine(Resources.ReleaseAssetDirNotFound, directoryPath);
                return false;
            }

            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                // Oops!  There is nowhere to deposit the zip file when we're done!
                Console.WriteLine(Resources.OutputZipFilePathBlank);
                return false;
            }

            // Check whether the zipFilePath contains characters that the operating
            // system does not allow to appear in a valid file name
            if (Path.GetFileName(zipFilePath).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Console.WriteLine(Resources.ReleaseZipFileNameContainsInvalidChars, zipFilePath);
                return false;
            }

            var zipOutputFolder = Path.GetDirectoryName(zipFilePath);

            if (string.IsNullOrWhiteSpace(zipOutputFolder))
            {
                // Oops!  There is nowhere to deposit the zip file when we're done!
                Console.WriteLine(Resources.ZipOutputFolderBlank);
                return false;
            }

            if (directoryPath.ToLower().Equals(zipOutputFolder.ToLower()))
            {
                // the output zip file is not allowed to live in the same directory as the files
                // that are being ZIPped.
                Console.WriteLine(Resources.ZipFolderMustBeDifferentFromAssetFolder);
                return false;
            }

            /* Check whether the current user has access privileges to write to the input directory and the
             directory where the output zip is going to live.  If the current user does not, then fail */
            if (!FileAndFolderHelper.IsFolderWritable(directoryPath))
            {
                // Current user does not have write access privileges to the input folder.
                Console.WriteLine(Resources.UserNotHasPermissionsToFolder, directoryPath);
                return false;
            }

            /* Create the folder where the outputted ZIP file is going to live, if that folder
             does not already exist. */
            FileAndFolderHelper.CreateDirectoryIfNotExists(
                zipOutputFolder
            );

            if (!FileAndFolderHelper.IsFolderWritable(zipOutputFolder))
            {
                // Current user does not have write access privileges to the output folder.
                Console.WriteLine(Resources.UserNotHasPermissionsToFolder, zipOutputFolder);
                return false;
            }

            // Delete the output file if it already exists.
            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);

            try
            {
                var events = new FastZipEvents();

                // try to keep going even if a particular file or folder fails
                events.FileFailure += (s1, a1) => a1.ContinueRunning = true;
                events.DirectoryFailure += (s2, a2) => a2.ContinueRunning = true;
                //events.CompletedFile += (s3, a3) => Console.WriteLine($"Finished file '{a3.Name}.");
                //events.ProcessFile += (s4, a4) => Console.WriteLine($"Zipping file {a4.Name}...");
                
                new FastZip(events)
                    .CreateZip(
                        zipFilePath,
                        directoryPath,
                        true,
                        string.Empty);
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                if (File.Exists(zipFilePath))
                    File.Delete(zipFilePath);

                return false;
            }

            return File.Exists(zipFilePath); // operation succeeded if the zip file exists at the path the user wants it at.
        }

        /// <summary>
        /// Given a reference to an instance of <see cref="T:System.IO.FileSystemInfo"/> representing a file to be
        /// ZIPped, creates an instance of a <see cref="T:ICSharpCode.SharpZipLib.Zip.ZipEntry"/> and returns a
        /// reference to the instance.
        /// </summary>
        /// <param name="fsi">Reference to an instance of <see cref="T:System.IO.FileSystemInfo"/> that
        /// represents the file for which a <see cref="T:ICSharpCode.SharpZipLib.Zip.ZipEntry"/> instance should
        /// be created.</param>
        /// <returns>Reference to an instance of <see cref="T:ICSharpCode.SharpZipLib.Zip.ZipEntry"/> representing the file
        /// to be zipped, or null if not successful.</returns>
        private static ZipEntry CreateZipEntry(FileSystemInfo fsi)
        {
            var result = default(ZipEntry);

            try
            {
                /* skip entries that come out of the file system enumerator that are
                                         for folders only, since the path names of the files stored in the ZIP files
                                         recreate folders anyway upon extraction. */
                var cleanedName = ZipEntry.CleanName(fsi.FullName);
                result = new ZipEntry(cleanedName)
                {
                    Size = new FileInfo(fsi.FullName).Length
                    ,
                    DateTime = fsi.LastWriteTime
                };
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                result = default(ZipEntry);
            }

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.IO.FileSystemInfo"/> instance
        /// passed refers to a file or a folder, and whether the file so referred to exists.
        /// </summary>
        /// <param name="fsi">Reference to an instance of <see cref="T:System.IO.FileSystemInfo"/> that represents
        /// the file to be examined.</param>
        /// <returns>True if the file referenced to exists, and is not a folder; false otherwise.</returns>
        private static bool EntryRefersToExistingFile(FileSystemInfo fsi)
            => fsi.Exists && (fsi.Attributes & FileAttributes.Directory) != FileAttributes.Directory;
    }
}
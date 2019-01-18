using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

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

            try
            {
                // Finished with validation, let's proceed to ZIP up the files in the input folder.
                var zipEntries = FileSearcher.GetAllFilesInFolder(directoryPath)
                    .Where(fsi => fsi.Exists)
                    .Select(fsi =>
                    {
                        var result = new ZipEntry(ZipEntry.CleanName(fsi.FullName))
                        {
                            Size = new FileInfo(fsi.FullName).Length
                        };
                        return result;
                    }).ToList();
                if (!zipEntries.Any())
                    return false;

                using (var zipFile = new ZipFile(zipFilePath))
                {
                    zipFile.UseZip64 = UseZip64.Off;

                    foreach (var entry in zipEntries)
                    {
                        zipFile.Add(entry);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true; // operation succeeded.
        }
    }
}

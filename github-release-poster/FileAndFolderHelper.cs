using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace github_release_poster
{
    public static class FileAndFolderHelper
    {
        public static void ClearTempFileDir()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\system32\cmd.exe",
                    Arguments =
                        "/C rd /S /Q \"" + Path.GetTempPath() + "\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                var proc = Process.Start(psi);
                proc?.WaitForExit();
            }
            catch
            {
                // nothing
            }
        }

        public static void CreateDirectoryIfNotExists(string directoryPath)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(
                DebugLevel.Info,
                "In FileAndFolderHelper.CreateDirectoryIfNotExists"
            );

            // Dump the variable directoryPath to the log
            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.CreateDirectoryIfNotExists: directoryPath = '{0}'",
                directoryPath
            );

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.CreateDirectoryIfNotExists: Checking whether the directory '{0}' exists...",
                directoryPath
            );

            if (Directory.Exists(directoryPath))
            {
                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.CreateDirectoryIfNotExists: The directory '{0}' exists.  Nothing to do.",
                    directoryPath
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.CreateDirectoryIfNotExists: Done."
                );

                return;
            }

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.CreateDirectoryIfNotExists: Now attempting to create the directory '{0}'...",
                directoryPath
            );

            try
            {
                Directory.CreateDirectory(directoryPath);

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.CreateDirectoryIfNotExists: Checking whether we were successful..."
                );

                if (Directory.Exists(directoryPath))
                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.CreateDirectoryIfNotExists: We successfully created the new directory '{0}'.",
                        directoryPath
                    );
                else
                    DebugUtils.WriteLine(
                        DebugLevel.Error,
                        "FileAndFolderHelper.CreateDirectoryIfNotExists: Failed to create the directory '{0}'.",
                        directoryPath
                    );
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                DebugUtils.WriteLine(
                    DebugLevel.Error,
                    "FileAndFolderHelper.CreateDirectoryIfNotExists: Failed to create directory '{0}'.",
                    directoryPath
                );
            }

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.CreateDirectoryIfNotExists: Done."
            );
        }

        /// <summary>
        /// If the specified directory exists, deletes it and also recursively
        /// deletes all files and folders it contains (or tries to, anyway)
        /// </summary>
        /// <param name="dir">Path to the directory to be deleted.</param>
        public static void DeleteDirIfExists(string dir)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(
                DebugLevel.Debug, "In FileAndFolderHelper.DeleteDirIfExists"
            );

            // Dump the variable dir to the log
            DebugUtils.WriteLine(
                DebugLevel.Debug,
                "FileAndFolderHelper.DeleteDirIfExists: dir = '{0}'", dir
            );

            DebugUtils.WriteLine(
                DebugLevel.Debug,
                "FileAndFolderHelper.DeleteDirIfExists: Checking whether the 'dir' parameter is not blank..."
            );

            if (string.IsNullOrWhiteSpace(dir))
            {
                DebugUtils.WriteLine(
                    DebugLevel.Error,
                    "FileAndFolderHelper.DeleteDirIfExists: Blank value passed for 'dir' parameter. This parameter is required."
                );

                DebugUtils.WriteLine(
                    DebugLevel.Debug,
                    "FileAndFolderHelper.DeleteDirIfExists: Done."
                );

                return;
            }

            DebugUtils.WriteLine(
                DebugLevel.Debug,
                "FileAndFolderHelper.DeleteDirIfExists: The 'dir' parameter is not blank."
            );

            try
            {
                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.DeleteDirIfExists: Checking whether the folder '{0}' exists and is writable...",
                    dir
                );

                if (!IsFolderWritable(dir))
                {
                    DebugUtils.WriteLine(
                        DebugLevel.Error,
                        "FileAndFolderHelper.DeleteDirIfExists: We cannot delete the folder '{0}' either because it does not exist or it is not writable by this user."
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Debug,
                        "FileAndFolderHelper.DeleteDirIfExists: Done."
                    );

                    return;
                }

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.DeleteDirIfExists: The folder '{0}' exists and is writable.",
                    dir
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.DeleteDirIfExists: Attempting to delete the directory '{0}' and all files and folders it contains...",
                    dir
                );

                Directory.Delete(dir, true);

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.DeleteDirIfExists: Finished deleting the directory '{0}' and all files and folders it contains.",
                    dir
                );
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);
            }

            DebugUtils.WriteLine(
                DebugLevel.Debug, "FileAndFolderHelper.DeleteDirIfExists: Done."
            );
        }

        /// <summary>
        /// Determines whether the file specified in the <see cref="fileInfo" />
        /// parameter has zero length.
        /// </summary>
        /// <param name="fileInfo">
        /// Reference to an instance of
        /// <see cref="T:System.IO.FileInfo" /> that refers to the file to check.
        /// </param>
        /// <returns>True if the file's length is zero bytes; false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if the
        /// <see cref="fileInfo" /> parameter has a null reference.
        /// </exception>
        public static bool FileHasZeroLength(FileInfo fileInfo)
        {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));

            return fileInfo.Length == 0;
        }

        public static List<string> GetFilesInFolder(string folder)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(
                DebugLevel.Info, "In MainWindowPresenter.GetFilesInFolder"
            );

            var result = new List<string>();

            // Check to see if the required parameter, folder, is blank, whitespace, or null. If it
            // is any of these, send an error to the log file and quit.

            // Dump the parameter folder to the log
            DebugUtils.WriteLine(
                DebugLevel.Info,
                "MainWindowPresenter.GetFilesInFolder: folder = '{0}'", folder
            );

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "MainWindowPresenter.GetFilesInFolder: Checking whether the required parameter, 'folder', is blank or not..."
            );

            if (string.IsNullOrWhiteSpace(folder))
            {
                DebugUtils.WriteLine(
                    DebugLevel.Error,
                    "MainWindowPresenter.GetFilesInFolder: Blank value passed for the 'folder' parameter. This parameter is required."
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "MainWindowPresenter.GetFilesInFolder: {0} files found.",
                    result.Count
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "MainWindowPresenter.GetFilesInFolder: Done."
                );

                // stop.
                return result;
            }

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "MainWindowPresenter.GetFilesInFolder: The parameter, 'folder', is not blank."
            );

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "MainWindowPresenter.GetFilesInFolder: Attempting to recursively get a list of all the files in the folder '{0}'...",
                folder
            );

            try
            {
                result = Directory.GetFiles(
                                      folder, "*.*", SearchOption.AllDirectories
                                  )
                                  .Where(File.Exists)
                                  .ToList();
            }
            catch (Exception)
            {
                result = new List<string>();
            }

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "MainWindowPresenter.GetFilesInFolder: {0} files found.",
                result.Count
            );

            DebugUtils.WriteLine(
                DebugLevel.Info, "MainWindowPresenter.GetFilesInFolder: Done."
            );

            return result;
        }

        /// <summary>
        /// Checks to see if the specified file exists. If not, emits a "stop"
        /// error message and returns false; otherwise, returns true.
        /// </summary>
        public static bool InsistPathExists(string fileName)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(
                DebugLevel.Info, "In FileAndFolderHelper.InsistPathExists"
            );

            var result = false;

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.InsistPathExists: Checking to make sure the file '{0}' actually exists...",
                fileName
            );

            try
            {
                if (!string.IsNullOrWhiteSpace(fileName) &&
                    File.Exists(fileName))
                {
                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.InsistPathExists: The file '{0}' was found.",
                        fileName
                    );

                    result = true;

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.InsistPathExists: Result = {0}",
                        result
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.InsistPathExists: Done."
                    );

                    return result;
                }
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                result = false;
            }

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.InsistPathExists: The file '{0}' was not found.",
                fileName
            );

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.InsistPathExists: Result = {0}", result
            );

            DebugUtils.WriteLine(
                DebugLevel.Info, "FileAndFolderHelper.InsistPathExists: Done."
            );

            return result;
        }

        /// <summary> Checks for write access for the given file. </summary>
        /// <param name="path">The filename.</param>
        /// <returns>true, if write access is allowed, otherwise false</returns>
        public static bool IsFileWriteable(string path)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(
                DebugLevel.Info, "In FileAndFolderHelper.IsFileWriteable"
            );

            var result = false;

            try
            {
                // Check whether the file has the read-only attribute set.

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: Checking whether the file '{0}' has the read only attribute set...",
                    path
                );

                if ((File.GetAttributes(path) & FileAttributes.ReadOnly) != 0)
                {
                    DebugUtils.WriteLine(
                        DebugLevel.Error,
                        "FileAndFolderHelper.IsFileWriteable: The file '{0}' is not writeable.",
                        path
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.IsFileWriteable: Result = {0}",
                        result
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.IsFileWriteable: Done."
                    );

                    return result;
                }

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: Getting the access rules for the file '{0}' and current user...",
                    path
                );

                // Get the access rules of the specified files (user groups and user names that have
                // access to the file)
                var rules = File.GetAccessControl(path)
                                .GetAccessRules(
                                    true, true, typeof(SecurityIdentifier)
                                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: Found {0} rules.",
                    rules.Count
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: Accessing the groups that the current user is a member of..."
                );

                // Get the identity of the current user and the groups that the user is in.
                var groups = WindowsIdentity.GetCurrent()
                                            .Groups;

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: The current user is a member of {0} groups.",
                    groups.Count
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: Getting the Security Identifier (SID) for the current user..."
                );

                var sidCurrentUser = WindowsIdentity.GetCurrent()
                                                    .User.Value;

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: The SID has been obtained."
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: Checking whether the file '{0}' has Deny permissions to Write Data...",
                    path
                );

                // Check if writing to the file is explicitly denied for this user or a group the
                // user is in.
                if (rules.OfType<FileSystemAccessRule>()
                         .Any(
                             r => (groups.Contains(r.IdentityReference) ||
                                   r.IdentityReference.Value ==
                                   sidCurrentUser) &&
                                  r.AccessControlType ==
                                  AccessControlType.Deny &&
                                  (r.FileSystemRights &
                                   FileSystemRights.WriteData) ==
                                  FileSystemRights.WriteData
                         ))
                {
                    DebugUtils.WriteLine(
                        DebugLevel.Error,
                        "FileAndFolderHelper.IsFileWriteable: The file '{0}' is not writeable due to security settings.",
                        path
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.IsFileWriteable: Result = {0}",
                        result
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.IsFileWriteable: Done."
                    );

                    return result;
                }

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFileWriteable: Checking if writing is allowed..."
                );

                // Check if writing is allowed
                result = rules.OfType<FileSystemAccessRule>()
                              .Any(
                                  r => (groups.Contains(r.IdentityReference) ||
                                        r.IdentityReference.Value ==
                                        sidCurrentUser) &&
                                       r.AccessControlType ==
                                       AccessControlType.Allow &&
                                       (r.FileSystemRights &
                                        FileSystemRights.WriteData) ==
                                       FileSystemRights.WriteData
                              );
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                result = false;
            }

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.IsFileWriteable: Result = {0}", result
            );

            DebugUtils.WriteLine(
                DebugLevel.Info, "FileAndFolderHelper.IsFileWriteable: Done."
            );

            return result;
        }

        /// <summary> Checks for write access for the given directory. </summary>
        /// <param name="path">The path to the directory to check.</param>
        /// <returns>true, if write access is allowed, otherwise false</returns>
        public static bool IsFolderWritable(string path)
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(
                DebugLevel.Info, "In FileAndFolderHelper.IsFolderWritable"
            );

            var result = false;

            try
            {
                DebugUtils.WriteLine(
                    DebugLevel.Debug,
                    "FileAndFolderHelper.IsFolderWritable: Checking whether the folder '{0}' exists...",
                    path
                );

                // If the folder we are testing does not even exist in the first place, then it
                // sure as hell is not writable
                if (!Directory.Exists(path))
                {
                    DebugUtils.WriteLine(
                        DebugLevel.Error,
                        "FileAndFolderHelper.IsFolderWritable: The path '{0}' could not be located.  Therefore, it does not refer to a writable folder.",
                        path
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Debug,
                        "FileAndFolderHelper.IsFolderWritable: Result = {0}",
                        false
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Debug,
                        "FileAndFolderHelper.IsFolderWritable: Done."
                    );

                    return result;
                }

                DebugUtils.WriteLine(
                    DebugLevel.Debug,
                    "FileAndFolderHelper.IsFolderWritable: The folder '{0}' exists.",
                    path
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: Getting the access rules for the folder '{0}' and current user...",
                    path
                );

                // Get the access rules of the specified files (user groups and user names that have
                // access to the folder)
                var rules = Directory.GetAccessControl(path)
                                     .GetAccessRules(
                                         true, true, typeof(SecurityIdentifier)
                                     );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: Found {0} rules.",
                    rules.Count
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: Accessing the groups that the current user is a member of..."
                );

                // Get the identity of the current user and the groups that the user is in.
                var groups = WindowsIdentity.GetCurrent()
                                            .Groups;

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: The current user is a member of {0} groups.",
                    groups.Count
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: Getting the Security Identifier (SID) for the current user..."
                );

                var sidCurrentUser = WindowsIdentity.GetCurrent()
                                                    .User.Value;

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: The SID has been obtained."
                );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: Checking whether the folder '{0}' has Deny permissions to Write Data...",
                    path
                );

                // Check if writing to the folder is explicitly denied for this user or a group the
                // user is in.
                if (rules.OfType<FileSystemAccessRule>()
                         .Any(
                             r => (groups.Contains(r.IdentityReference) ||
                                   r.IdentityReference.Value ==
                                   sidCurrentUser) &&
                                  r.AccessControlType ==
                                  AccessControlType.Deny &&
                                  (r.FileSystemRights &
                                   FileSystemRights.WriteData) ==
                                  FileSystemRights.WriteData
                         ))
                {
                    DebugUtils.WriteLine(
                        DebugLevel.Error,
                        "FileAndFolderHelper.IsFolderWritable: The folder '{0}' is not writeable due to security settings.",
                        path
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.IsFolderWritable: Result = {0}",
                        result
                    );

                    DebugUtils.WriteLine(
                        DebugLevel.Info,
                        "FileAndFolderHelper.IsFolderWritable: Done."
                    );

                    return result;
                }

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    "FileAndFolderHelper.IsFolderWritable: Checking if writing to this folder is allowed..."
                );

                // Check if writing is allowed
                result = rules.OfType<FileSystemAccessRule>()
                              .Any(
                                  r => (groups.Contains(r.IdentityReference) ||
                                        r.IdentityReference.Value ==
                                        sidCurrentUser) &&
                                       r.AccessControlType ==
                                       AccessControlType.Allow &&
                                       (r.FileSystemRights &
                                        FileSystemRights.WriteData) ==
                                       FileSystemRights.WriteData
                              );

                DebugUtils.WriteLine(
                    DebugLevel.Info,
                    result
                        ? "FileAndFolderHelper.IsFolderWritable: Writing to the folder '{0}' is allowed."
                        : "FileAndFolderHelper.IsFolderWritable: Writing to the folder '{0}' is not allowed.",
                    path
                );
            }
            catch (Exception e)
            {
                // dump all the exception info to the log
                DebugUtils.LogException(e);

                result = false;
            }

            DebugUtils.WriteLine(
                DebugLevel.Info,
                "FileAndFolderHelper.IsFolderWritable: Result = {0}", result
            );

            DebugUtils.WriteLine(
                DebugLevel.Info, "FileAndFolderHelper.IsFolderWritable: Done."
            );

            return result;
        }
    }
}
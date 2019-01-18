using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net.Config;

namespace github_release_poster
{
    /// <summary>
    /// Methods to be used to manage the application log.
    /// </summary>
    public static class LogFileManager
    {
        /// <summary>
        /// Gets the full path and filename to the log file for this application.
        /// </summary>
        private static string LogFilePath
        {
            get
            {
                var rootAppender = FileAppenderManager.GetFirstAppender();

                var result = rootAppender != null ? rootAppender.File : string.Empty;

                return result;
            }
        }

        /// <summary>
        /// Initializes the application's logging subsystem.
        /// </summary>
        /// <param name="muteDebugLevelIfReleaseMode">Set to true if we should not write out "DEBUG" messages to the log file when in the NewRelease mode.  Set to false if all messages should always be logged.</param>
        /// <param name="overwrite">Overwrites any existing logs for the application with the latest logging sent out by this instance.</param>
        /// <param name="configurationFilePathname">Specifies the path to the configuration file to be utilized for initializing log4net.  If blank, the system attempts to utilize the default App.config file.</param>
        public static void InitializeLogging(bool muteDebugLevelIfReleaseMode = true,
            bool overwrite = true, string configurationFilePathname = "")
        {
            // Attempt to get a name for the executing application.
            DebugUtils.ApplicationName = GetDebugApplicationName();

            // If we found a value for the ApplicationName, then initialize the EventLogManager.  The EventLogManager
            // is a companion component to DebugUtils which also spits out logging to the System Event Log.  This is handy
            // in the case where the user does not have write access to the C:\ProgramData directory, for example.
            if (!string.IsNullOrWhiteSpace(DebugUtils.ApplicationName))
            {
                EventLogManager.Instance.Initialize(DebugUtils.ApplicationName, EventLogType.Application);
            }

            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(DebugLevel.Info, "In LogFileManager.InitializeLogging");

            SetUpDebugUtils(muteDebugLevelIfReleaseMode);

            // Check whether the path to the configuration file is blank; or, if it's not blank, whether the specified file actually exists at the path indicated.
            // If the configuration file pathname is blank and/or it does not exist at the path indicated, then call the version of XmlConfigurator.Configure that does
            // not take any arguments.  On the other hand, if the configurationFilePathname parameter is not blank, and it specifies a file that actually does exist
            // at the specified path, then pass that path to the XmlConfigurator.Configure method.
            if (string.IsNullOrWhiteSpace(configurationFilePathname)
                || !File.Exists(configurationFilePathname))
            {
                // If the file specified by the configurationFilePathname does not actually exist, the author of this software
                // needs to know about it, so throw a FileNotFoundException
                if (!string.IsNullOrWhiteSpace(configurationFilePathname)  // only do this check for a non-blank file name.
                    && !File.Exists(configurationFilePathname))
                {
                    throw new FileNotFoundException($"The file '{configurationFilePathname}' was not found.\n\nThe application needs this file in order to continue.");
                }

                XmlConfigurator.Configure();
            }
            else
            {
                XmlConfigurator.Configure(new FileInfo(configurationFilePathname));
            }

            // Check to see if the required property, LogFilePath, is blank, whitespace, or null. If it is any of these, send an
            // error to the log file and quit.

            if (string.IsNullOrWhiteSpace(LogFilePath))
            {
                // Failed to initialize logging.
                return;
            }

            // If we are here, then the required string property, LogFilePath, has a value.

            var logFileDirectoryName = Path.GetDirectoryName(LogFilePath);
            if (string.IsNullOrWhiteSpace(logFileDirectoryName))
                return;

            var directoryInfo = new DirectoryInfo(logFileDirectoryName).Parent;
            if (directoryInfo != null)
            {
                // Check whether the user has write permissions on the directory tree that will
                // contain the log files.  Stop if the user does not.
                var logFileDirectoryParent = directoryInfo
                    .FullName;

                // Dump the variable logFileDirectoryParent to the log
                DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.InitializeLogging: logFileDirectoryParent = '{0}'", logFileDirectoryParent);

                DebugUtils.WriteLine(DebugLevel.Info,
                    "LogFileManager.InitializeLogging: Checking whether the user has write-level access to the folder '{0}'...",
                    logFileDirectoryParent);

                // Check if the user has write access to the parent directory of the log file.
                if (!FileAndFolderHelper.IsFolderWritable(logFileDirectoryParent))
                {
                    DebugUtils.WriteLine(DebugLevel.Error,
                        @"LogFileManager.InitializeLogging: The user '{0}\{1}' does not have write-level access to the folder '{2}'.",
                        Environment.UserDomainName,
                        Environment.UserName,
                        logFileDirectoryParent);

                    throw new UnauthorizedAccessException($"The current user does not have write permissions to the directory '{logFileDirectoryParent}'.");
                }
            }

            // Check whether the log file directory already exists.  If not, then try to create it.
            FileAndFolderHelper.CreateDirectoryIfNotExists(logFileDirectoryName);

            // We have to insist that the directory that the log file is in is writeable.  If we can't
            // get write access to the log file directory, then throw an exception.
            if (!FileAndFolderHelper.IsFolderWritable(logFileDirectoryName))
            {
                throw new UnauthorizedAccessException(
                    $"The current user does not have write permissions to the directory '{logFileDirectoryName}'.");
            }

            // Set options on the file appender of the logging system to minimize locking issues
            FileAppenderConfigurator.SetMinimalLock(FileAppenderManager.GetFirstAppender());

            if (overwrite)
                DeleteLogIfExists();
        }

        /// <summary>
        /// Sets up the <see cref="T:github_release_poster.DebugUtils"/> to initialize its functionality.
        /// </summary>
        /// <param name="muteDebugLevelIfReleaseMode">If set to true, does not echo any logging statements that are set to <see cref="DebugLevel.Info"/>.</param>
        /// <param name="isLogging">True to activate the functionality of writing to a log file; false to suppress.  Usually used with the <see cref="consoleOnly"/> parameter set to true.</param>
        /// <param name="consoleOnly">True to only write messages to the console; false to write them both to the console and to the log.</param>
        /// <param name="verbosity">Zero to suppress every message; greater than zero to echo every message.</param>
        public static void SetUpDebugUtils(bool muteDebugLevelIfReleaseMode,
            bool isLogging = true,
            bool consoleOnly = false,
            int verbosity = 1)
        {
            DebugUtils.IsLogging = isLogging;
            DebugUtils.ConsoleOnly = consoleOnly;
            DebugUtils.Verbosity = verbosity;
            DebugUtils.MuteDebugLevelIfReleaseMode = muteDebugLevelIfReleaseMode;

            // do not print anything in this method if verbosity is set to anything less than 2
            if (DebugUtils.Verbosity < 2)
                return;

            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(DebugLevel.Info, "In LogFileManager.SetUpDebugUtils");

            // Dump the variable DebugUtils.IsLogging to the log
            DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.SetUpDebugUtils: DebugUtils.IsLogging = {0}", DebugUtils.IsLogging);

            // Dump the variable DebugUtils.ConsoleOnly to the log
            DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.SetUpDebugUtils: DebugUtils.ConsoleOnly = {0}", DebugUtils.ConsoleOnly);

            // Dump the variable DebugUtils.Verbosity to the log
            DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.SetUpDebugUtils: DebugUtils.Verbosity = {0}", DebugUtils.Verbosity);

            // Dump the variable DebugUtils.MuteDebugLevelIfReleaseMode to the log
            DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.SetUpDebugUtils: DebugUtils.MuteDebugLevelIfReleaseMode = {0}", DebugUtils.MuteDebugLevelIfReleaseMode);

            DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.SetUpDebugUtils: Done.");
        }

        /// <summary>
        /// Deletes the log file, if it exists.
        /// </summary>
        private static void DeleteLogIfExists()
        {
            // write the name of the current class and method we are now entering, into the log
            DebugUtils.WriteLine(DebugLevel.Info, "In LogFileManager.DeleteLogIfExists");

            // Dump the variable LogFilePath to the log
            DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.DeleteLogIfExists: LogFilePath = '{0}'", LogFilePath);

            DebugUtils.WriteLine(DebugLevel.Info,
                "LogFileManager.DeleteLogIfExists: Checking whether the file with path contained in 'LogFilePath' exists...");

            if (!File.Exists(LogFilePath))
            {
                DebugUtils.WriteLine(DebugLevel.Info,
                    "LogFileManager.DeleteLogIfExists: The log file '{0}' does not exist.  Nothing to do.",
                    LogFilePath);

                return;
            }

            DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.DeleteLogIfExists: The file with path contained in 'LogFilePath' was found.");

            DebugUtils.WriteLine(DebugLevel.Info,
                "LogFileManager.DeleteLogIfExists: Checking whether the folder '{0}' is writeable...",
                Path.GetDirectoryName(LogFilePath));

            if (!FileAndFolderHelper.IsFolderWritable(Path.GetDirectoryName(LogFilePath)))
            {
                // If we cannot write to the folder where the log file to be deleted sits in, then Heaven help us!  However the software
                // should try to work at all costs, so this method should just silently fail in this case.
                DebugUtils.WriteLine(DebugLevel.Error,
                    "LogFileManager.DeleteLogIfExists: The folder '{0}' is not writeable, so we can't delete the log file '{1}' as requested.  Nothing to do.",
                    Path.GetDirectoryName(LogFilePath), LogFilePath);

                DebugUtils.WriteLine(DebugLevel.Info, "LogFileManager.DeleteLogIfExists: Done.");

                return;
            }

            DebugUtils.WriteLine(DebugLevel.Info,
                "LogFileManager.DeleteLogIfExists: The folder '{0}' is writeable, so therefore we can delete the log file '{1}'.",
                Path.GetDirectoryName(LogFilePath), LogFilePath);

            try
            {
                DebugUtils.WriteLine(DebugLevel.Info,
                    "LogFileManager.DeleteLogIfExists: Deleting the log file '{0}'...",
                    LogFilePath);

                File.Delete(LogFilePath);
            }
            catch(Exception)
            {
                Console.WriteLine(Resources.APP_HAS_INSUFFICIENT_PERMISSIONS);
            }

            // NOTE: We do no logging here since we are dealing with a freshly-emptied log file.
        }

        private static string GetDebugApplicationName()
        {
            // write the name of the current class and method we are now entering, into the log
            Console.WriteLine("In LogFileManager.GetDebugApplicationName");

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                entryAssembly = Assembly.GetExecutingAssembly();
            }

            var entryAssemblyLocation = entryAssembly.Location;

            // Dump the variable entryAssemblyLocation to the log
            Console.WriteLine($@"LogFileManager.InitializeLogging: entryAssemblyLocation = '{entryAssemblyLocation}'");

            var versionInfo = FileVersionInfo.GetVersionInfo(entryAssemblyLocation);

            var result = versionInfo.ProductName;

            Console.WriteLine($"LogFileManager.GetDebugApplicationName: Result = {result}");

            Console.WriteLine("LogFileManager.GetDebugApplicationName: Done.");

            return result;
        }
    }
}
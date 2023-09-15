using log4net.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace github_release_poster
{
    /// <summary> Methods to be used to manage the application log. </summary>
    public static class LogFileManager
    {
        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:github_release_poster.LogFileManagers" /> instance has been
        /// initialized properly.
        /// </summary>
        public static bool IsLoggingInitialized { get; private set; }

        /// <summary> Gets the name of the directory that the log file is located in. </summary>
        private static string LogFileDirectoryName
            => Path.GetDirectoryName(LogFilePath);

        /// <summary> Gets the full path and filename to the log file for this application. </summary>
        private static string LogFilePath
        {
            get
            {
                var rootAppender = FileAppenderManager.GetFirstFileAppender();

                var result = rootAppender != null
                    ? rootAppender.File
                    : string.Empty;

                return result;
            }
        }

        /// <summary> Initializes the application's logging subsystem. </summary>
        /// <param name="muteDebugLevelIfReleaseMode">
        /// Set to true if we should not write
        /// out "DEBUG" messages to the log file when in the NewRelease mode.  Set to false
        /// if all messages should always be logged.
        /// </param>
        /// <param name="overwrite">
        /// Overwrites any existing logs for the application with
        /// the latest logging sent out by this instance.
        /// </param>
        /// <param name="configurationFilePathname">
        /// Specifies the path to the configuration
        /// file to be utilized for initializing log4net.  If blank, the system attempts to
        /// utilize the default App.config file.
        /// </param>
        /// <remarks>
        /// Upon completion, this method sets the
        /// <see cref="IsLoggingInitialized" /> property.  Applications should check the
        /// value of this property to determine whether logging succeeded.
        /// </remarks>
        public static void InitializeLogging(
            bool muteDebugLevelIfReleaseMode = true,
            bool overwrite = true,
            string configurationFilePathname = ""
        )
        {
            // write the name of the current class and method we are now entering, into the log
            Console.WriteLine("In LogFileManager.InitializeLogging");

            // Check whether the path to the configuration file is blank; or, if it's not blank, whether the specified file actually exists at the path indicated.
            // If the configuration file pathname is blank and/or it does not exist at the path indicated, then call the version of XmlConfigurator.Configure that does
            // not take any arguments.  On the other hand, if the configurationFilePathname parameter is not blank, and it specifies a file that actually does exist
            // at the specified path, then pass that path to the XmlConfigurator.Configure method.
            if (string.IsNullOrWhiteSpace(configurationFilePathname) ||
                !File.Exists(configurationFilePathname))
            {
                // If the file specified by the configurationFilePathname does not actually exist, the author of this software
                // needs to know about it, so throw a FileNotFoundException
                if (!string.IsNullOrWhiteSpace(
                        configurationFilePathname
                    ) // only do this check for a non-blank file name.
                    && !File.Exists(configurationFilePathname))
                {
                    Console.WriteLine(
                        $"The file '{configurationFilePathname}' was not found.\n\nThe application needs this file in order to continue."
                    );
                    IsLoggingInitialized = false;
                    return;
                }

                XmlConfigurator.Configure();
            }
            else
            {
                XmlConfigurator.Configure(
                    new FileInfo(configurationFilePathname)
                );
            }

            // Check to see if the required property, LogFilePath, is blank, whitespace, or null. If it is any of these, send an
            // error to the log file and quit.

            if (string.IsNullOrWhiteSpace(LogFilePath))

                // if we are here, then the call above did not work, try to load the configuration from the
                // .exe.config file which may have been included as an embedded resource.
                if (!ConfigureLogFileFromEmbeddedResource())
                {
                    Console.WriteLine(
                        "LogFileManager.InitializeLogging: Failed to initialize logging from embedded configuration file."
                    );
                    return;
                }

            // If we are here, then the required string property, LogFilePath, has a value.  Check to ensure that the
            // LogFileDirectoryName read-only property also has a value, which should be the path to the folder in which
            // the log file lives.
            if (string.IsNullOrWhiteSpace(LogFileDirectoryName))
                return;

            // Check whether the parent folder of the folder in which the log file will live, is also writable by the
            // currently-logged-in user
            var directoryInfo = new DirectoryInfo(LogFileDirectoryName).Parent;
            if (directoryInfo != null)
            {
                // Check whether the user has write permissions on the directory tree that will
                // contain the log files.  Stop if the user does not.
                var logFileDirectoryParent = directoryInfo.FullName;

                // Dump the variable logFileDirectoryParent to the log
                Console.WriteLine(
                    "LogFileManager.InitializeLogging: logFileDirectoryParent = '{0}'",
                    logFileDirectoryParent
                );

                Console.WriteLine(
                    "LogFileManager.InitializeLogging: Checking whether the user has write-level access to the folder '{0}'...",
                    logFileDirectoryParent
                );

                // Check if the user has write access to the parent directory of the log file.
                if (!FileAndFolderHelper.IsFolderWritable(
                        logFileDirectoryParent
                    ))
                {
                    Console.WriteLine(
                        @"LogFileManager.InitializeLogging: The user '{0}\{1}' does not have write-level access to the folder '{2}'.",
                        Environment.UserDomainName, Environment.UserName,
                        logFileDirectoryParent
                    );

                    // Mark the IsLoggingInitialized property to false
                    IsLoggingInitialized = false;

                    return;
                }
            }

            // Check whether the log file directory already exists.  If not, then try to create it.
            FileAndFolderHelper.CreateDirectoryIfNotExists(
                LogFileDirectoryName
            );

            // We have to insist that the directory that the log file is in is writable.  If we can't
            // get write access to the log file directory, then throw an exception.
            if (!FileAndFolderHelper.IsFolderWritable(LogFileDirectoryName))
            {
                Console.WriteLine(
                    $"The current user does not have write permissions to the directory '{LogFileDirectoryName}'."
                );

                IsLoggingInitialized = false;
            }

            // Set options on the file appender of the logging system to minimize locking issues
            FileAppenderConfigurator.SetMinimalLock(
                FileAppenderManager.GetFirstFileAppender()
            );

            // If the overwrite parameter's value is set to true, then overwrite the log -- that is,
            // delete any existing log file that may already exist.
            if (overwrite)
                DeleteLogIfExists();

            // initialization succeeded
            IsLoggingInitialized = true;

            // Set up the Event Log and the DebugUtils objects.
            // Attempt to get a name for the executing application.
            DebugUtils.ApplicationName = GetDebugApplicationName();

            // If we found a value for the ApplicationName, then initialize the EventLogManager.  The EventLogManager
            // is a companion component to DebugUtils which also spits out logging to the System Event Log.  This is handy
            // in the case where the user does not have write access to the C:\ProgramData directory, for example.
            if (!string.IsNullOrWhiteSpace(DebugUtils.ApplicationName))
                EventLogManager.Instance.Initialize(
                    DebugUtils.ApplicationName, EventLogType.Application
                );

            // Set up the DebugUtils object with values that specify how we want logging done.  Be sure to specify that
            // we should not write log messages to the console under any circumstances, since this is a console application
            // that may be interactive to the user or have its stdout results parsed by another program.
            SetUpDebugUtils(muteDebugLevelIfReleaseMode, true, false, 1, true);

            // done
        }

        /// <summary>
        /// Sets up the <see cref="T:github_release_poster.DebugUtils" /> to
        /// initialize its functionality.
        /// </summary>
        /// <param name="muteDebugLevelIfReleaseMode">
        /// If set to true, does not echo any
        /// logging statements that are set to <see cref="DebugLevel.Info" />.
        /// </param>
        /// <param name="isLogging">
        /// True to activate the functionality of writing to a log
        /// file; false to suppress.  Usually used with the <see cref="consoleOnly" />
        /// parameter set to true.
        /// </param>
        /// <param name="consoleOnly">
        /// True to only write messages to the console; false to
        /// write them both to the console and to the log.
        /// </param>
        /// <param name="verbosity">
        /// Zero to suppress every message; greater than zero to
        /// echo every message.
        /// </param>
        /// <param name="noConsole">
        /// True to totally suppress any output to the console.
        /// Output will be written to any other logging output.
        /// </param>
        public static void SetUpDebugUtils(
            bool muteDebugLevelIfReleaseMode,
            bool isLogging = true,
            bool consoleOnly = false,
            int verbosity = 1,
            bool noConsole = false
        )
        {
            DebugUtils.IsLogging = isLogging;
            DebugUtils.ConsoleOnly = consoleOnly;
            DebugUtils.NoConsole = noConsole;
            DebugUtils.Verbosity = verbosity;
            DebugUtils.MuteDebugLevelIfReleaseMode =
                muteDebugLevelIfReleaseMode;
        }

        /// <summary>
        /// Attempts to run the XmlConfigurator off of a .exe.config file that is
        /// an embedded resource, if possible.
        /// </summary>
        private static bool ConfigureLogFileFromEmbeddedResource()
        {
            // write the name of the current class and method we are now entering, into the log
            Console.WriteLine(
                "In LogFileManager.ConfigureLogFileFromEmbeddedResource"
            );

            var result = false;

            var assembly = Assembly.GetCallingAssembly();
            var names = Assembly.GetExecutingAssembly()
                                .GetManifestResourceNames();
            var searchResult = names.ToList()
                                    .Find(s => s.EndsWith(".config"));
            var resourceName = string.IsNullOrWhiteSpace(searchResult)
                ? "log4net.config"
                : searchResult;

            try
            {
                using (var stream =
                       assembly.GetManifestResourceStream(resourceName))
                    XmlConfigurator.Configure(stream);

                // the configuration worked if the LogFilePath property of this class is not empty
                result = !string.IsNullOrWhiteSpace(LogFilePath);
            }
            catch
            {
                // Ignored.
                result = false;
            }

            Console.WriteLine(
                "LogFileManager.ConfigureLogFileFromEmbeddedResource: Done."
            );

            return result;
        }

        /// <summary> Deletes the log file, if it exists. </summary>
        private static void DeleteLogIfExists()
        {
            // write the name of the current class and method we are now entering, into the log
            Console.WriteLine("In LogFileManager.DeleteLogIfExists");

            Console.WriteLine(
                "LogFileManager.DeleteLogIfExists: Checking whether the folder '{0}' is writable...",
                LogFileDirectoryName
            );

            if (!FileAndFolderHelper.IsFolderWritable(LogFileDirectoryName))
            {
                // If we cannot write to the folder where the log file to be deleted sits in, then Heaven help us!  However the software
                // should try to work at all costs, so this method should just silently fail in this case.
                Console.WriteLine(
                    "LogFileManager.DeleteLogIfExists: The folder '{0}' is not writable, so we can't delete the log file '{1}' as requested.  Nothing to do.",
                    LogFileDirectoryName, LogFilePath
                );

                Console.WriteLine("LogFileManager.DeleteLogIfExists: Done.");

                return;
            }

            Console.WriteLine(
                "LogFileManager.DeleteLogIfExists: The folder '{0}' is writable, so therefore we can delete the log file '{1}'.",
                LogFileDirectoryName, LogFilePath
            );

            try
            {
                Console.WriteLine(
                    "LogFileManager.DeleteLogIfExists: Deleting the log file folder '{0}' and all files and folders within it, and then re-creating the folder...",
                    LogFileDirectoryName
                );

                if (Directory.Exists(LogFileDirectoryName))
                    Directory.Delete(LogFileDirectoryName, true);

                if (!Directory.Exists(LogFileDirectoryName))
                    Directory.CreateDirectory(LogFileDirectoryName);

                if (Directory.Exists(LogFileDirectoryName))
                    Console.WriteLine(
                        "LogFileManager:DeleteLogIfExists: Successfully deleted and re-created the folder '{0}'.",
                        LogFileDirectoryName
                    );
            }
            catch (Exception e)
            {
                // dump all the exception info to the log/console
                DebugUtils.LogException(e);
            }

            Console.WriteLine("LogFileManager.DeleteLogIfExists: Done.");
        }

        private static string GetDebugApplicationName()
        {
            // write the name of the current class and method we are now entering, into the log
            Console.WriteLine("In LogFileManager.GetDebugApplicationName");

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
                entryAssembly = Assembly.GetExecutingAssembly();

            var entryAssemblyLocation = entryAssembly.Location;

            // Dump the variable entryAssemblyLocation to the log
            Console.WriteLine(
                $@"LogFileManager.InitializeLogging: entryAssemblyLocation = '{entryAssemblyLocation}'"
            );

            var versionInfo =
                FileVersionInfo.GetVersionInfo(entryAssemblyLocation);

            var result = versionInfo.ProductName;

            Console.WriteLine(
                $"LogFileManager.GetDebugApplicationName: Result = {result}"
            );

            Console.WriteLine("LogFileManager.GetDebugApplicationName: Done.");

            return result;
        }
    }
}
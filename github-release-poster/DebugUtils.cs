using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using github_release_poster.Properties;
using log4net;

namespace github_release_poster
{
    public static class DebugUtils
    {
        ///<summary>
        /// Constructs a new instance of <see cref="DebugUtils"/>.
        ///</summary>
        static DebugUtils()
        {
            // default ExceptionStackDepth is 1 for a Windows Forms app. Set to 2 for a Console App.
            ExceptionStackDepth = 1;
        }

        /// <summary>
        /// Occurs whenever text has been emitted by a <see cref="Write"/> method.
        /// </summary>
        public static event Action<string, DebugLevel> TextEmitted;

        /// <summary>
        /// Gets or sets the name of the application.  Used for Windows event logging.  Leave blank to not send events to the Application event log.
        /// </summary>
        public static string ApplicationName { get; set; }

        public static bool ConsoleOnly { get; set; }

        /// <summary>
        /// Gets or sets the depth down the call stack from which Exception information should be obtained.
        /// </summary>
        public static int ExceptionStackDepth { get; set; }

        public static bool IsLogging { get; set; }

        /// <summary>
        /// Users should set this property to the path to the log file, if logging.
        /// </summary>
        public static string LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to mute "DEBUG" log messages in Release mode.
        /// </summary>
        public static bool MuteDebugLevelIfReleaseMode { get; set; }

        public static TextWriter Out { get; set; }

        public static int Verbosity { get; set; }

        /// <summary>
        /// Dumps a collection to the debug log.
        /// </summary>
        /// <param name="collection"></param>
        public static void DumpCollection(ICollection collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return;
            }

            foreach (var element in collection)
            {
                if (element == null)
                {
                    continue;
                }

                WriteLine(DebugLevel.Info, element.ToString());
            }
        }

        /// <summary>
        /// Writes the text of the selected control-- which is supposed to be a CommandLink -- to
        /// the log, while, at the same time, stripping out the text "recommended", if present
        /// </summary>
        /// <param name="commandLink"></param>
        public static void EchoCommandLinkText(dynamic commandLink)
        {
            if (commandLink == null)
            {
                throw new ArgumentNullException(nameof(commandLink));
            }

            WriteLine(DebugLevel.Info, "Command link '{0}' selected",
                commandLink.Text.Replace(" (recommended)", string.Empty));
        }

        public static string FormatException(Exception e)
        {
            if (e == null)
            {
                return string.Empty;
            }

            var message = string.Format(Resources.ExceptionMessageFormat, e.Message, e.StackTrace);
            WriteLine(message);
            return message;
        }

        public static void FormatExceptionAndWrite(Exception e)
        {
            FormatException(e);
        }

        /// <summary>
        /// Logs the complete information about an exception to the log, under the Error Level.
        /// Outputs the source file and line number where the exception occured, as well as the
        /// message of the exceptio and its stack trace.
        /// </summary>
        /// <param name="e">Reference to the <see cref="Exception"/> to be logged.</param>
        public static string LogException(Exception e)
        {
            if (e == null)
            {
                return string.Empty;
            }

            var message = string.Format(Resources.ExceptionMessageFormat, e.GetType(), e.Message, e.StackTrace);

            WriteLine(DebugLevel.Error, message);

            return message;
        }

        /*/// <summary>
        /// Logs the complete information about an exception to the log, under the Error Level.
        /// Outputs the source file and line number where the exception occured, as well as the
        /// message of the exceptio and its stack trace. </summary> <param name="e">Reference to the
        /// <see cref="Exception"/> to be logged.</param>
        public static void LogException(Exception e)
        {
            // write the name of the current class and method we are now entering, into the log
            WriteLine(DebugLevel.Info, "In DebugUtils.LogException");

            WriteLine(DebugLevel.Info,
                "DebugUtils.LogException: Checking whether the 'e' parameter is a valid object reference...");

            if (e == null)
            {
                WriteLine(DebugLevel.Error,
                    "DebugUtils.LogException: Unable to log the exception because a null reference was passed for the e parameter.");

                WriteLine(DebugLevel.Info, "DebugUtils.LogException: Done.");

                return;
            }

            WriteLine(DebugLevel.Info,
                "DebugUtils.LogException: The 'e' parameter contains a valid object reference.");

            WriteLine(DebugLevel.Info,
                "DebugUtils.LogException: Attempting to obtain more information about the exception...");

            var info = ExceptionInfo.Make(e);

            WriteLine(DebugLevel.Info,
                "DebugUtils.LogException: Checking whether the 'info' variable has a valid object reference...");

            if (info == null)
            {
                WriteLine(DebugLevel.Error,
                    "DebugUtils.LogException: The 'info' variable has a null reference.  Quitting.");

                WriteLine(DebugLevel.Info, "DebugUtils.LogException: Done.");

                return;
            }

            WriteLine(DebugLevel.Info,
                "DebugUtils.LogException: The 'info' variable has a valid object reference.");

            WriteLine(DebugLevel.Error, info.ToString());
        }*/

        public static void Write(DebugLevel debugLevel, string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                // cannot do anything with a blank entry.
                return;
            }

#if !DEBUG
            /* If this software is currently running in Release mode, then do not output ANY lines of text
             * that are meant to be debugging logging statements! So, that is, if we detect that the debugLevel
             * parameter is set to DebugLevel.Info, simply stop executing this method. */

            if (MuteDebugLevelIfReleaseMode
                && debugLevel == DebugLevel.Info)
                return;
#endif

            if (format.Contains("\n"))
            {
                // the format string is composed of several lines. log each line separately.

                // first, format the text with string.Format
                var formattedLogEntry = args.Any() ? string.Format(format, args) : format;
                if (string.IsNullOrWhiteSpace(formattedLogEntry))
                {
                    // stop if the format output is blank or null
                    return;
                }

                var lines = formattedLogEntry.Split('\n');
                if (!lines.Any())
                {
                    return;
                }

                // for each line, write it out at the debug level indicated, one by one
                foreach (var line in lines)
                {
                    Write(debugLevel, line);
                }
            }
            else
            {
                var content = args.Any() ? string.Format(format, args) : format;
                Write(debugLevel, content);
            }
        }

        public static void WriteLine(DebugLevel debugLevel, string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                // cannot do anything with a blank entry.
                return;
            }

            if (!LogFileManager.IsLoggingInitialized)
            {
                /* only write to the console if the log file manager is not initialized yet */
                Console.WriteLine(format, args);
                return;
            }

#if !DEBUG
            /* If this software is currently running in Release mode, then do not output ANY lines of text
             * that are meant to be debugging logging statements! So, that is, if we detect that the debugLevel
             * parameter is set to DebugLevel.Info, simply stop executing this method. */

            if (MuteDebugLevelIfReleaseMode
                && debugLevel == DebugLevel.Info)
                return;
#endif

            if (format.Contains("\n"))
            {
                // the format string is composed of several lines. log each line separately.

                // first, format the text with string.Format
                var formattedLogEntry = args.Any() ? string.Format(format, args) : format;
                if (string.IsNullOrWhiteSpace(formattedLogEntry))
                {
                    // stop if the format output is blank or null
                    return;
                }

                var lines = formattedLogEntry.Split('\n');
                if (!lines.Any())
                {
                    return;
                }

                // for each line, write it out at the level indicated, one by one
                foreach (var line in lines)
                {
                    WriteLine(debugLevel, line);
                }
            }
            else
            {
                var content = args.Any() ? string.Format(format, args) : format;
                WriteLine(debugLevel, content);
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                // cannot do anything with a blank entry.
                return;
            }
            if (format.Contains("\n"))
            {
                // the format string is composed of several lines. log each line separately.

                // first, format the text with string.Format
                var formattedLogEntry = args.Any() ? string.Format(format, args) : format;
                if (string.IsNullOrWhiteSpace(formattedLogEntry))
                {
                    // stop if the format output is blank or null
                    return;
                }

                var lines = formattedLogEntry.Split('\n');
                if (!lines.Any())
                {
                    return;
                }

                // for each line, write it out at DebugLevel.Info
                foreach (var line in lines)
                {
                    WriteLine(DebugLevel.Info, line);
                }
            }
            else
            {
                WriteLine(DebugLevel.Info, format, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="TextEmitted"/> event.
        /// </summary>
        /// <param name="text">      The text that has been written or logged.</param>
        /// <param name="debugLevel">The <see cref="DebugLevel"/> of the message.</param>
        private static void OnTextEmitted(string text, DebugLevel debugLevel)
        {
            if (TextEmitted != null)
            {
                TextEmitted(text, debugLevel);
            }
        }

        private static void Write(DebugLevel debugLevel, string content)
        {
            try
            {
                if (Verbosity == 0)
                {
                    return;
                }

                Console.Write(content);

                OnTextEmitted(content, debugLevel);

                if (ConsoleOnly)
                {
                    return;
                }

                if (!IsLogging)
                {
                    return;
                }

                // If we are being called from LINQPad, then use Debug.WriteLine
                if ("LINQPad".Equals(AppDomain.CurrentDomain.FriendlyName))
                {
                    System.Diagnostics.Debug.Write(content);
                    return;
                }

                var currentMethod = MethodBase.GetCurrentMethod();
                var logger = LogManager.GetLogger(currentMethod.DeclaringType);
                switch (debugLevel)
                {
                    case DebugLevel.Error:
                        logger.Error(content);
                        EventLogManager.Instance.Error(content);
                        break;

                    case DebugLevel.Info:
                        logger.Info(content);
                        EventLogManager.Instance.Info(content);
                        break;

                    case DebugLevel.Warning:
                        logger.Warn(content);
                        EventLogManager.Instance.Warn(content);
                        break;

                    case DebugLevel.Debug:
                        logger.Debug(content);
                        break;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private static void WriteLine(DebugLevel debugLevel, string content)
        {
            try
            {
                if (Verbosity == 0)
                {
                    return;
                }

                // If we are being called from LINQPad, then use Debug.WriteLine
                if ("LINQPad".Equals(AppDomain.CurrentDomain.FriendlyName))
                {
                    System.Diagnostics.Debug.WriteLine(content);
                    return;
                }

                Console.WriteLine(content);

                if (debugLevel != DebugLevel.Info)
                {
                    OnTextEmitted(content, debugLevel);
                }

                if (ConsoleOnly)
                {
                    return;
                }

                if (!IsLogging)
                {
                    return;
                }

                var currentMethod = MethodBase.GetCurrentMethod();
                var logger = LogManager.GetLogger(currentMethod.DeclaringType);

                switch (debugLevel)
                {
                    case DebugLevel.Error:
                        logger.Error(content);
                        EventLogManager.Instance.Error(content);
                        break;

                    case DebugLevel.Info:
                        logger.Info(content);
                        EventLogManager.Instance.Info(content);
                        break;

                    case DebugLevel.Warning:
                        logger.Warn(content);
                        EventLogManager.Instance.Warn(content);
                        break;

                    case DebugLevel.Debug:
                        //#if DEBUG   // only print DEBUG logging statements if we're in Debug mode
                        logger.Debug(content);
                        //#endif
                        break;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        /*
                private static bool LoggerConfigured { get; set; }
        */
    }
}
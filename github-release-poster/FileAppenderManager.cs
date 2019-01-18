using System;
using System.Linq;
using log4net.Appender;

namespace github_release_poster
{
    /// <summary>
    /// Provides methods to access instances of objects of type <see cref="T:log4net.Appender.FileAppender"/>.
    /// </summary>
    public static class FileAppenderManager
    {
        public static FileAppender GetAppenderByName(string name)
        {
            // Check to see if the required parameter, , is blank, whitespace, or null. If it is any of these, throw an
            // ArgumetNullException.

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            FileAppender result = null;

            var root = LoggerManager.GetRootLogger();
            if (root == null)
                return result;

            return root.Appenders.Count == 0 ? result : root.Appenders.Cast<FileAppender>().First(fa => fa.Name == name);
        }

        /// <summary>
        /// If the root logger's appenders list contains appenders, returns a reference to the first one in the list.
        /// </summary>
        /// <returns>Reference to an instance of <see cref="T:log4net.Appender.FileAppender"/>, or null if not found.</returns>
        public static FileAppender GetFirstAppender()
        {
            var root = LoggerManager.GetRootLogger();
            if (root == null)
                return null;

            return root.Appenders.Count == 0 ? null : root.Appenders.Cast<FileAppender>().First();
        }
    }
}
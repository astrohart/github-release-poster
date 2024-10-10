using log4net.Appender;
using System;
using System.Linq;

namespace github_release_poster
{
    /// <summary>
    /// Provides methods to access instances of objects of type
    /// <see cref="T:log4net.Appender.FileAppender" />.
    /// </summary>
    public static class FileAppenderManager
    {
        /// <summary>
        /// Searches for an instance of
        /// <see cref="T:log4net.Appender.FileAppender" /> whose name matches the given
        /// criteria.  If no result is found, null is returned.
        /// </summary>
        /// <param name="name">String containing the name of the appender you want.</param>
        /// <returns>
        /// Reference to an instance of the first
        /// <see cref="T:log4net.Appender.FileAppender" /> in the list of configured
        /// appenders whose name matches the value in the <see cref="name" /> parameter, or
        /// null if none was found.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if the required
        /// parameter, <see cref="name" />, is a blank string or null value.
        /// </exception>
        public static FileAppender GetAppenderByName(string name)
        {
            // Check to see if the required parameter, , is blank, whitespace, or null. If it is any of these, throw an
            // ArgumentNullException.
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            // Get a reference to the root logger.  If we can't find one, then give up.
            var root = LoggerManager.GetRootLogger();
            if (root == null)
                return null;

            // Be careful to only fetch appenders that are of the type FileAppender, as
            // many possible types of appenders might be configured
            return !root.Appenders.OfType<FileAppender>()
                        .Any()
                ? null
                : root.Appenders.OfType<FileAppender>()
                      .First(fa => fa.Name == name);
        }

        /// <summary>
        /// If the root logger's appenders list contains appenders, returns a
        /// reference to the first one in the list that is a file appender.
        /// </summary>
        /// <returns>
        /// Reference to an instance of
        /// <see cref="T:log4net.Appender.FileAppender" />, or null if not found.
        /// </returns>
        public static FileAppender GetFirstFileAppender()
        {
            // Get a reference to the root logger.  If we can't find one, then give up.
            var root = LoggerManager.GetRootLogger();
            if (root == null)
                return null;

            // make sure there is even a FileAppender in the appenders list in the
            // first place; if so, then return the first element of the appenders list
            // that is of the type FileAppender
            return !root.Appenders.OfType<FileAppender>()
                        .Any()
                ? null
                : root.Appenders.OfType<FileAppender>()
                      .First();
        }
    }
}
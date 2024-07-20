using log4net.Appender;
using System;

namespace github_release_poster
{
    /// <summary> Provides methods for configurating log4net's FileAppender </summary>
    public static class FileAppenderConfigurator
    {
        /// <summary>
        /// Sets the option to get the minimal locking option set on the
        /// specified <see cref="appender" />.
        /// </summary>
        /// <param name="appender">
        /// Reference to an instance of an object of type
        /// <see cref="T:log4net.Appender.FileAppender" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the instance of the object
        /// referenced by <see cref="appender" /> is null.
        /// </exception>
        public static void SetMinimalLock(FileAppender appender)
        {
            if (appender == null)
                throw new ArgumentNullException(nameof(appender));

            appender.ImmediateFlush = true;
            appender.LockingModel = new FileAppender.MinimalLock();
            appender.ActivateOptions();
        }
    }
}
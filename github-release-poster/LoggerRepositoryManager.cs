using log4net;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace github_release_poster
{
    /// <summary>
    /// Provides methods to access the log4net Repository.
    /// </summary>
    public static class LoggerRepositoryManager
    {
        /// <summary>
        /// Gets a reference to an instance of the log4net repository as an reference to an object of type Hierachy.
        /// </summary>
        /// <returns>Reference to an instance of a <see cref="I:ILoggerRepository"/> that derives from <see cref="T:log4net.Repository.Hierarchy"/>, or null if no such object has been found.</returns>
        public static Hierarchy GetHierarchyRepository()
        {
            return LogManager.GetRepository() as Hierarchy;
        }

        /// <summary>
        /// Wraps the <see cref="M:log4net.LogManager.GetRepository"/> method.
        /// </summary>
        /// <returns>Reference to an instance of an object that implements <see cref="ILoggerRepository"/>, or null if such an object cannot be found.</returns>
        public static ILoggerRepository GetLoggerRepository()
        {
            return LogManager.GetRepository();
        }
    }
}
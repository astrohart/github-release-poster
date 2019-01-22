using log4net.Repository.Hierarchy;

namespace github_release_poster
{
    /// <summary>
    /// Provides methods to access objects of type <see cref="T:log4net.Hierarchy.Repository.Logger"/> from log4net.
    /// </summary>
    public static class LoggerManager
    {
        /// <summary>
        /// Gets a reference to the default logger repository's root instance of <see cref="T:log4net.Hierarchy.Repository.Logger"/>.
        /// </summary>
        /// <returns>Reference to the default logger repository's root instance of <see cref="T:log4net.Hierarchy.Repository.Logger"/>, or null if not found.</returns>
        public static Logger GetRootLogger()
        {
            Logger result = null;

            var repo = LoggerRepositoryManager.GetHierarchyRepository();
            if (repo == null)
                return null;

            result = repo.Root;

            return result;
        }
    }
}
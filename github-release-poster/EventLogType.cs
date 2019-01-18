namespace github_release_poster
{
    /// <summary>
    /// The type of event log to write events to.
    /// </summary>
    public enum EventLogType
    {
        /// <summary>
        /// Events get directed to the Application event log.
        /// </summary>
        Application,

        /// <summary>
        /// Events get directed to the System event log.
        /// </summary>
        System,

        /// <summary>
        /// Events get directed to the Security event log.
        /// </summary>
        Security,

        /// <summary>
        /// No event log is to be utilized.
        /// </summary>
        None,

        /// <summary>
        /// The event log type is unknown.
        /// </summary>
        Unknown
    }
}
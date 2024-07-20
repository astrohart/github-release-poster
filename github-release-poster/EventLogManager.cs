using System.Diagnostics;

namespace github_release_poster
{
    /// <summary> Class to manage access to the event log. </summary>
    public class EventLogManager
    {
        /// <summary>
        /// Holds an reference to the one and only instance of
        /// <see cref="EventLogManager" />.
        /// </summary>
        private static EventLogManager _theEventLogManager;

        /// <summary>
        /// Constructs an instance of <see cref="T:EventLogManager" /> and
        /// returns a reference to the new instance.
        /// </summary>
        protected EventLogManager()
        {
            // set defaults
            Source = string.Empty;
            Type = EventLogType.Unknown;
        }

        /// <summary>
        /// Gets a reference to the one and only instance of
        /// <see cref="EventLogManager" />.
        /// </summary>
        public static EventLogManager Instance
            => _theEventLogManager ??
               (_theEventLogManager = new EventLogManager());

        /// <summary>
        /// Gets a value indicating whether this object has been properly
        /// initialized.
        /// </summary>
        public bool IsInitialized
            => !string.IsNullOrWhiteSpace(Source) &&
               Type != EventLogType.None && Type != EventLogType.Unknown;

        /// <summary>
        /// Gets or sets the source of events.  Typically this is the name of the
        /// application that is sending the events.
        /// </summary>
        /// <remarks>
        /// This property must be set before logging events, otherwise an error
        /// will occur.
        /// </remarks>
        public string Source { get; private set; }

        /// <summary>
        /// Gets or sets the type of log to which events are to be sent
        /// (Application, System, Security, etc.).
        /// </summary>
        /// <remarks>
        /// This property must be set before logging events, otherwise an error
        /// will occur.
        /// </remarks>
        public EventLogType Type { get; private set; }

        /// <summary>
        /// Sends an Error event to the system event log pointed to by the
        /// <see cref="Source" /> and <see cref="Type" /> properties.  The content of the
        /// logging message is specified by the <see cref="content" /> parameter.
        /// </summary>
        /// <param name="content">String specifying the content of the event log message.</param>
        public void Error(string content)
        {
            if (string.IsNullOrWhiteSpace(Source) ||
                Type == EventLogType.None || Type == EventLogType.Unknown)
                return;

            if (string.IsNullOrWhiteSpace(content))
                return;

            EventLog.WriteEntry(Source, content, EventLogEntryType.Error);
        }

        /// <summary>
        /// Sends an Info event to the system event log pointed to by the
        /// <see cref="Source" /> and <see cref="Type" /> properties.  The content of the
        /// logging message is specified by the <see cref="content" /> parameter.
        /// </summary>
        /// <param name="content">String specifying the content of the event log message.</param>
        public void Info(string content)
        {
            if (string.IsNullOrWhiteSpace(Source) ||
                Type == EventLogType.None || Type == EventLogType.Unknown)
                return;

            if (string.IsNullOrWhiteSpace(content))
                return;

            EventLog.WriteEntry(Source, content, EventLogEntryType.Information);
        }

        /// <summary> Initializes event logging for your application. </summary>
        /// <param name="eventSource">Name of the application that will be sending events.</param>
        /// <param name="type">
        /// One of the
        /// <see cref="T:github_release_poster.EventLogType" /> values that specifies the
        /// type of log to send events to.
        /// </param>
        public void Initialize(string eventSource, EventLogType type)
        {
            // Check to see if the required parameter, eventSource, is blank, whitespace, or null. If it is any of these, send an
            // error to the log file and quit.

            // The 'eventSource' parameter must not be blank.
            if (string.IsNullOrWhiteSpace(eventSource))

                // stop.
                return;

            // A log type other than 'None' or 'Unknown' must be specified.
            if (type == EventLogType.Unknown || type == EventLogType.None)
                return;

            try
            {
                // If an event source does not exist with the specified name, then create one.
                if (!EventLog.SourceExists(eventSource))
                    EventLog.CreateEventSource(eventSource, type.ToString());

                // Finally, save the event source and type settings in the Source and Type properties.
                Source = eventSource;
                Type = type;
            }
            catch
            {
                // If an exception was caught, de-initialize the member properties of this EventLogManager
                // instance.  This is to prevent this class from working in future calls.

                Source = string.Empty;
                Type = EventLogType.Unknown;
            }
        }

        /// <summary>
        /// Sends a Warning event to the system event log pointed to by the
        /// <see cref="Source" /> and <see cref="Type" /> properties.  The content of the
        /// logging message is specified by the <see cref="content" /> parameter.
        /// </summary>
        /// <param name="content">String specifying the content of the event log message.</param>
        public void Warn(string content)
        {
            if (string.IsNullOrWhiteSpace(Source) ||
                Type == EventLogType.None || Type == EventLogType.Unknown)
                return;

            if (string.IsNullOrWhiteSpace(content))
                return;

            EventLog.WriteEntry(Source, content, EventLogEntryType.Warning);
        }
    }
}
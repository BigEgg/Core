using System;
using System.Globalization;

namespace BigEgg.Logging
{
    /// <summary>
    /// Defines the class for a log model which will used by <see cref="ILogger"/>.
    /// </summary>
    public class Log : ILog
    {
        private static string logFormatWithoutPriority = "[{0}] {2}: {3}";
        private static string logFormat = "[{0}] ({1}){2}: {3}";
        private const string constLogTimeFormat = "HH:mm:ss.ffff";

        /// <summary>
        /// Initializes a new instance of the Log class with the specified category and priority.
        /// </summary>
        /// <param name="message">Log message body.</param>
        /// <param name="category">Category of the log.</param>
        /// <param name="priority">The priority of the log.</param>
        /// <exception cref="ArgumentNullException">The message parameter could not be NULL or empty.</exception>
        public Log(string message, Category category = Category.Info, Priority priority = Priority.None)
        {
            if (String.IsNullOrWhiteSpace(message)) { throw new ArgumentNullException("message"); }

            this.Time = DateTime.Now;
            this.Message = message;
            this.Category = category;
            this.Priority = priority;
        }


        #region Properties
        /// <summary>
        /// Log Time.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Log message body.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Category of the log.
        /// </summary>
        public Category Category { get; private set; }

        /// <summary>
        /// The priority of the log.
        /// </summary>
        public Priority Priority { get; private set; }

        /// <summary>
        /// The log format string.
        /// </summary>
        public static string LogFormat
        {
            get { return logFormat; }
        }

        /// <summary>
        /// The log format string which is without the priority.
        /// </summary>
        public static string LogFormatWithoutPriority
        {
            get { return logFormatWithoutPriority; }
        }
        #endregion


        /// <summary>
        /// Converts the log of this instance to its equaivalent string representation.
        /// </summary>
        public string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Converts the log of this instance to its equaivalent string representation
        /// Using the specified format and culture-specific format infomation.
        /// </summary>
        /// <param name="format">A custom log format string.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting infomation.</param>
        /// <returns>The string representation of the log.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(format)) 
            {
                if (this.Priority == Priority.None)
                {
                    format = logFormatWithoutPriority;
                }
                else
                {
                    format = logFormat;
                }
            }
            if (formatProvider == null) { formatProvider = CultureInfo.InvariantCulture; }

            return string.Format(
                formatProvider,
                format,
                Time.ToString(constLogTimeFormat),
                Priority.ToString(),
                Category.ToString(),
                Message);
        }
    }
}

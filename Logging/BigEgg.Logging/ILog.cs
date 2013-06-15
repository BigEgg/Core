using System;

namespace BigEgg.Logging
{
    public interface ILog : IFormattable
    {
        /// <summary>
        /// Log Time.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// Log message body.
        /// </summary>
        string Message { get; }
        /// <summary>
        /// Category of the log.
        /// </summary>
        Category Category { get; }
        /// <summary>
        /// The priority of the log.
        /// </summary>
        Priority Priority { get; }
    }
}

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace BigEgg.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> that logs to .NET <see cref="Trace"/> class.
    /// </summary>
    public class TraceLogger : ILogger
    {
        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="log">The log item to log.</param>
        [Export(typeof(ILogger)), Export]
        public void Log(ILog log)
        {
            if (log == null) { throw new ArgumentException("log"); }

            if (log.Category == Category.Exception)
            {
                Trace.TraceError(log.ToString());
            }
            else
            {
                Trace.TraceInformation(log.ToString());
            }
        }
    }
}

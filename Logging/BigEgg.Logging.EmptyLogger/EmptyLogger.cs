using System;
using System.ComponentModel.Composition;

namespace BigEgg.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> that does nothing. This
    /// implementation is useful when the application does not need logging
    /// but there are infrastructure pieces that assume there is a logger.
    /// </summary>
    [Export(typeof(ILogger)), Export]
    public class EmptyLogger : ILogger
    {
        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="log">The log item to log.</param>
        public void Log(ILog log)
        {
            if (log == null) { throw new ArgumentException("log"); }
        }
    }
}

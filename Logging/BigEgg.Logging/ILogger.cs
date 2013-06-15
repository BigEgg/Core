namespace BigEgg.Logging
{
    /// <summary>
    /// Defines a simple logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Write a new log with the specified category and priority.
        /// </summary>
        /// <param name="log">The log item to log.</param>
        void Log(ILog log);
    }
}

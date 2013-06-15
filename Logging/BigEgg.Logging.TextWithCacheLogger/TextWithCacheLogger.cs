using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace BigEgg.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> that logs to a text file with a cache, will automate write in every 1 minute.
    /// And will force to write the log when cached over 100 log itme, or got an Exception log.
    /// </summary>
    [Export(typeof(ILogger)), Export]
    public class TextWithCacheLogger : ILogger
    {
        #region Members
        private const String ConstLogFileNameFormat = "\\Logs\\Log_{0}.log";

        private string fileName;
        private DateTime logDate;

        private List<ILog> cachedLogs;

        private System.Threading.TimerCallback timerDelegate;
        private System.Threading.Timer timer;
        private const UInt16 ConstCacheLength = 100;                 //  Every 100 non-Error Logs, save the Logs to the File.
        private const UInt16 ConstCacheTimeInterval = 60000;         //  Every 1 minute, save the Logs to the File.
        private Object lockthis = new Object();
        #endregion


        /// <summary>
        /// Initializes a new instance of the TextWithCacheLogger class.
        /// </summary>
        public TextWithCacheLogger()
        {
            this.logDate = DateTime.Now.Date;
            this.cachedLogs = new List<ILog>();
        }

        /// <summary>
        /// Destructor of the TextWithCacheLogger Class.
        /// </summary>
        ~TextWithCacheLogger()
        {
            //  Close the Thread
            if (timer != null)
                timer.Dispose();
            timerDelegate = null;

            OutputCachedLogs(null);
        }


        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="log">The log item to log.</param>
        public void Log(ILog log)
        {
            if (log == null) { throw new ArgumentException("log"); }

            Boolean needWriteToFile = false;

            lock (lockthis)
            {
                this.cachedLogs.Add(log);

                if (this.cachedLogs.Count >= ConstCacheLength)
                    needWriteToFile = true;

                if (log.Category == Category.Exception)
                    needWriteToFile = true;
            }

            if (needWriteToFile)
                OutputCachedLogs(null);
        }


        #region Private Methods
        /// <summary>
        /// Start a thread for output.
        /// </summary>
        private void OutputThread()
        {
            //  Create a TimerCallback, it will run OutputCacheLogs(Object)
            this.timerDelegate = new System.Threading.TimerCallback(OutputCachedLogs);
            //  Create the Timer Class, to set the TimerCallback will run every ConstCacheTimeInterval millisecond.
            this.timer = new System.Threading.Timer(timerDelegate, null, ConstCacheTimeInterval, ConstCacheTimeInterval);
        }

        /// <summary>
        /// Output the cached logs
        /// </summary>
        /// <param name="state">Use for TimerCallback Class, will not use in the method</param>
        private void OutputCachedLogs(Object state)
        {
            //  Return if there don't have any logs to output
            if (this.cachedLogs.Any())
                return;


            lock (lockthis)
            {
                using (StreamWriter writer = new StreamWriter(this.fileName, true))
                {
                    foreach (ILog log in this.cachedLogs.Where(l => l.Time.Date == this.logDate))
                        writer.WriteLine(log.ToString());
                    writer.Close();
                }

                if (this.cachedLogs.Any(l => l.Time.Date != this.logDate))
                {
                    this.fileName = string.Format(ConstLogFileNameFormat, this.logDate.ToString("yyyy-MM-dd"));

                    using (StreamWriter writer = new StreamWriter(this.fileName, true))
                    {
                        foreach (ILog log in this.cachedLogs.Where(l => l.Time.Date != this.logDate))
                            writer.WriteLine(log.ToString());
                        writer.Close();
                    }

                    this.logDate = DateTime.Now.Date;
                }

                //  Clear cached logs
                this.cachedLogs.Clear();
            }
        }
        #endregion
    }
}

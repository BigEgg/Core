using System;
using System.ComponentModel.Composition;
using System.IO;

namespace BigEgg.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> that logs to a text file.
    /// </summary>
    [Export(typeof(ILogger)), Export]
    public class TextLogger : ILogger
    {
        #region Members
        private const String ConstLogFileNameFormat = "\\Logs\\Log_{0}.log";

        private string fileName;
        private DateTime logDate;
        private StreamWriter logWriter;
        #endregion

        /// <summary>
        /// Initializes a new instance of the TextLogger class.
        /// </summary>
        public TextLogger()
        {
        }


        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="log">The log item to log.</param>
        public void Log(ILog log)
        {
            if (log == null) { throw new ArgumentException("log"); }

            if (DateTime.Now.Date != this.logDate)
            {
                CloseFile();
                OpenFile();
            }

            this.logWriter.WriteLine(log.ToString());
        }


        #region Private Methods
        private void OpenFile()
        {
            if (this.logWriter == null)
            {
                this.logDate = DateTime.Now.Date;
                this.fileName = string.Format(ConstLogFileNameFormat, this.logDate.ToString("yyyy-MM-dd"));

                if (!Directory.Exists(this.fileName))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(this.fileName));
                }
                
                this.logWriter = new StreamWriter(this.fileName, true);
            }
        }

        private void CloseFile()
        {
            if (this.logWriter != null)
            {
                this.logWriter.Flush();
                this.logWriter.Close();
                this.logWriter.Dispose();
                this.logWriter = null;
            }
        }
        #endregion
    }
}

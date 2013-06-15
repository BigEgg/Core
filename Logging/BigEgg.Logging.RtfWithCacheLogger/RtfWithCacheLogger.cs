using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BigEgg.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> that logs to an rtf file with a cache, will automate write in every 1 minute.
    /// And will force to write the log when cached over 100 log itme, or got an Exception log.
    /// </summary>
    [Export(typeof(ILogger)), Export]
    public class RtfWithCacheLogger : ILogger
    {
        #region Members
        private const String ConstLogTimeFormat = "HH:mm:ss.ffff";
        private const String ConstLogFileNameFormat = "\\Logs\\Log_{0}.log";

        private string fileName;
        private DateTime logDate;

        private List<ILog> cachedLogs;

        private System.Threading.TimerCallback timerDelegate;
        private System.Threading.Timer timer;
        private const UInt16 ConstCacheLength = 100;                 //  Every 100 non-Error Logs, save the Logs to the File.
        private const UInt16 ConstCacheTimeInterval = 60000;         //  Every 1 minute, save the Logs to the File.
        private Object lockthis = new Object();

        #region RTF Settings
        private const String ConstLogTimeFormatString = "HH:mm:ss.ffff";

        private Color ConstLogTimeColor = Color.FromArgb(217, 116, 43);
        private Color ConstLogPriorityColor = Color.FromArgb(230, 180, 80);
        private Color ConstLogWarnMessageColor = Color.FromArgb(153, 77, 82);
        private Color ConstLogDebugMessageColor = Color.FromArgb(227, 230, 195);
        private Color ConstLogInfoMessageColor = Color.Black;
        private Color ConstLogExceptionMessageColor = Color.Red;

        private Font ConstLogTimeFont = new Font("Calibri", 11, FontStyle.Italic);
        private Font ConstLogPriorityFont = new Font("Calibri", 11, FontStyle.Bold);
        private Font ConstLogMessageFont = new Font("Calibri", 11, FontStyle.Regular);
        #endregion
        #endregion


        /// <summary>
        /// Initializes a new instance of the RtfWithCacheLogger class.
        /// </summary>
        public RtfWithCacheLogger()
        {
            this.logDate = DateTime.Now.Date;
            this.cachedLogs = new List<ILog>();
        }

        /// <summary>
        /// Destructor of the RtfWithCacheLogger Class.
        /// </summary>
        ~RtfWithCacheLogger()
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
                using (RichTextBox output = new RichTextBox())
                {
                    output.Rtf = String.Empty;

                    if (File.Exists(this.fileName))
                        output.LoadFile(this.fileName, RichTextBoxStreamType.RichText);

                    foreach (ILog log in this.cachedLogs.Where(l => l.Time.Date == this.logDate))
                    {
                        output.Select(output.Text.Length, 0);
                        output.SelectedRtf = FormatLogRTF(log) + Environment.NewLine;
                    }

                    output.SaveFile(this.fileName, RichTextBoxStreamType.RichText);
                }

                if (this.cachedLogs.Any(l => l.Time.Date != this.logDate))
                {
                    this.fileName = string.Format(ConstLogFileNameFormat, this.logDate.ToString("yyyy-MM-dd"));

                    using (RichTextBox output = new RichTextBox())
                    {
                        output.Rtf = String.Empty;

                        if (File.Exists(this.fileName))
                            output.LoadFile(this.fileName, RichTextBoxStreamType.RichText);

                        foreach (ILog log in this.cachedLogs.Where(l => l.Time.Date != this.logDate))
                        {
                            output.Select(output.Text.Length, 0);
                            output.SelectedRtf = FormatLogRTF(log) + Environment.NewLine;
                        }

                        output.SaveFile(this.fileName, RichTextBoxStreamType.RichText);
                    }

                    this.logDate = DateTime.Now.Date;
                }

                //  Clear cached logs
                this.cachedLogs.Clear();
            }
        }

        /// <summary>
        /// Format the log information to the RTF String.
        /// </summary>
        /// <param name="log">A log class</param>
        /// <returns>If log is null, will return String.Empty; or return the RTF String</returns>
        private String FormatLogRTF(ILog log)
        {
            if (log == null)
                return String.Empty;

            RichTextBox rtfText = new RichTextBox();
            rtfText.Text = String.Empty;

            //  Log's Time
            rtfText.SelectionColor = ConstLogTimeColor;
            rtfText.SelectionFont = ConstLogTimeFont;
            rtfText.AppendText("[" + log.Time.ToString(ConstLogTimeFormatString) + "] ");

            //  Log's Priority & Category
            rtfText.SelectionColor = ConstLogPriorityColor;
            rtfText.SelectionFont = ConstLogPriorityFont;
            rtfText.AppendText(log.Priority == Priority.None ? string.Empty : "(" + log.Priority.ToString() + ")");
            rtfText.AppendText(log.Category.ToString() + ": ");

            //  Log's Message.
            switch (log.Category)
            {
                case Category.Info:
                    rtfText.SelectionColor = ConstLogInfoMessageColor;
                    break;
                case Category.Debug:
                    rtfText.SelectionColor = ConstLogDebugMessageColor;
                    break;
                case Category.Exception:
                    rtfText.SelectionColor = ConstLogExceptionMessageColor;
                    break;
                case Category.Warn:
                    rtfText.SelectionColor = ConstLogWarnMessageColor;
                    break;
            }
            rtfText.SelectionFont = ConstLogMessageFont;
            rtfText.AppendText(log.Message);

            return rtfText.Rtf;
        }
        #endregion
    }
}

using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BigEgg.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> that logs to an rtf file.
    /// </summary>
    [Export(typeof(ILogger)), Export]
    public class RtfLogger
    {
        #region Members
        private const String ConstLogFileNameFormat = "\\Logs\\Log_{0}.rtf";

        private string fileName;
        private DateTime logDate;

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
        /// Initializes a new instance of the RTFLogger class.
        /// </summary>
        public RtfLogger()
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
                this.logDate = DateTime.Now.Date;
                this.fileName = string.Format(ConstLogFileNameFormat, this.logDate.ToString("yyyy-MM-dd"));
            }

            using (RichTextBox output = new RichTextBox())
            {
                output.Rtf = String.Empty;

                if (File.Exists(this.fileName))
                    output.LoadFile(this.fileName, RichTextBoxStreamType.RichText);

                output.Select(output.Text.Length, 0);
                output.SelectedRtf = FormatLogRTF(log) + Environment.NewLine;

                output.SaveFile(this.fileName, RichTextBoxStreamType.RichText);
            }
        }


        #region Private Methods
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

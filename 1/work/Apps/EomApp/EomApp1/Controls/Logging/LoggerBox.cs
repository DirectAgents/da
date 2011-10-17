using System;
using System.Windows.Forms;
using DAgents.Common;

namespace Mainn.Controls.Logging
{
    public partial class LoggerBox : UserControl, ILogger
    {
        public LoggerBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Wheather or not to display the primary logging area.
        /// </summary>
        /// <value></value>
        public bool ShowPrimary
        {
            get;
            set;
        }

        /// <summary>
        /// Wheather or not to display the secondary logging area.
        /// </summary>
        /// <value></value>
        public bool ShowSecondary
        {
            get;
            set;
        }

        #region ILogger Members

        public void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate()
                {
                    Log(message);
                });
            }
            else
            {
                primaryRichTextBox.AppendText(message + "\n");
                primaryRichTextBox.ScrollToCaret();
            }
        }

        public void LogError(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate()
                {
                    LogError(message);
                });
            }
            else
            {
                secondaryRichTextBox.AppendText(message + "\n");
                secondaryRichTextBox.ScrollToCaret();
            }
        }

        #endregion

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            primaryRichTextBox.Clear();
        }

        private void LogBox_Load(object sender, EventArgs e)
        {
            primaryRichTextBox.Visible = ShowPrimary;
            secondaryRichTextBox.Visible = ShowSecondary;
        }
    }
}

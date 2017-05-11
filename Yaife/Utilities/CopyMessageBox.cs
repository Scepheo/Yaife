using System;
using System.Windows.Forms;

namespace Yaife.Utilities
{
    public partial class CopyMessageBox : Form
    {
        public CopyMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Displays a dialog with text for easy copying.
        /// </summary>
        /// <param name="message">The message to display along the copyable text.</param>
        /// <param name="text">The text that should be displayed for copying.</param>
        /// <param name="caption">A caption for the dialog.</param>
        /// <returns>One of the System.Windows.Forms.DialogResult values.</returns>
        public static DialogResult Show(string message, string text, string caption)
        {
            var dialog = new CopyMessageBox
            {
                // Set dialog data
                copyLabel = { Text = message },
                copyTextBox = { Text = text },
                Text = caption
            };

            // Select the dialog text
            dialog.copyTextBox.SelectAll();

            // Display center-window
            dialog.StartPosition = FormStartPosition.CenterParent;

            return dialog.ShowDialog();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(copyTextBox.Text);
        }
    }
}

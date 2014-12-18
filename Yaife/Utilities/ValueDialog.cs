using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaife.Utilities
{
	public partial class ValueDialog<T> : Form
		where T : IComparable
	{
		/// <summary>
		/// Gets the number returned by the dialog.
		/// </summary>
		public T Value { get; private set; }

		private T defaultValue, minValue, maxValue;
		private Func<string, T> parse;

		/// <summary>
		/// Creates a dialog used for getting a number from the user.
		/// </summary>
		/// <param name="message">The message to display alongside the number entry.</param>
		/// <param name="caption">A caption for the dialog.</param>
		/// <param name="defaultValue">The default number to display in the number entry.</param>
		/// <param name="minValue">The lowest value the user is allowed to enter.</param>
		/// <param name="maxValue">The highest value the user is allowed to enter.</param>
		public ValueDialog(string message, string caption, T defaultValue, Func<string, T> parse, T minValue, T maxValue)
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterParent;

			messageLabel.Text = message;
			this.Text = caption;
			valueTextBox.Text = defaultValue.ToString();

			this.defaultValue = defaultValue;
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.parse = parse;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			T value;
			bool valid;
			
			try
			{
				value = parse(valueTextBox.Text);
				valid = true;
			}
			catch
			{
				value = defaultValue;
				valid = false;
			}

			bool inRange = (minValue.CompareTo(value) <= 0) && (maxValue.CompareTo(value) >= 0);

			if (valid && inRange)
			{
				this.Value = value;
				DialogResult = DialogResult.OK;
				Close();
			}
			else if (!valid)
			{
				MessageBox.Show("Not a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else // (!inRange)
			{
				var message = String.Format("Value not in range {0} - {1}.", minValue, maxValue);
				MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Value = defaultValue;
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}

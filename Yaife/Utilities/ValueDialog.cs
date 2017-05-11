using System;
using System.Windows.Forms;

namespace Yaife.Utilities
{
    public sealed partial class ValueDialog<T> : Form
        where T : IComparable
    {
        /// <summary>
        /// Gets the value entered by the user.
        /// </summary>
        public T Value { get; private set; }

        private readonly T _defaultValue;
        private readonly T _minValue;
        private readonly T _maxValue;
        private readonly bool _hasRange;
        private readonly Func<string, T> _parse;

        /// <summary>
        /// Creates a dialog used for getting a value from the user, with a specified minimum and
        /// maximum value to restrict the range of values the user can enter.
        /// </summary>
        /// <param name="message">The message to display alongside the number entry.</param>
        /// <param name="caption">A caption for the dialog.</param>
        /// <param name="defaultValue">The default number to display in the number entry.</param>
        /// <param name="parse">A function used to parse a string entered into a value.</param>
        /// <param name="minValue">The lowest value the user is allowed to enter.</param>
        /// <param name="maxValue">The highest value the user is allowed to enter.</param>
        public ValueDialog(string message, string caption, T defaultValue, Func<string, T> parse, T minValue, T maxValue)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            messageLabel.Text = message;
            Text = caption;
            valueTextBox.Text = defaultValue.ToString();

            _defaultValue = defaultValue;
            _minValue = minValue;
            _maxValue = maxValue;
            _hasRange = true;
            _parse = parse;
        }

        /// <summary>
        /// Creates a dialog used for getting a value from the user.
        /// </summary>
        /// <param name="message">The message to display alongside the number entry.</param>
        /// <param name="caption">A caption for the dialog.</param>
        /// <param name="defaultValue">The default number to display in the number entry.</param>
        /// <param name="parse">A function used to parse a string entered into a value.</param>
        public ValueDialog(string message, string caption, T defaultValue, Func<string, T> parse)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            messageLabel.Text = message;
            Text = caption;
            valueTextBox.Text = defaultValue.ToString();

            _defaultValue = defaultValue;
            _hasRange = false;
            _parse = parse;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            T value;
            bool valid;
            
            try
            {
                value = _parse(valueTextBox.Text);
                valid = true;
            }
            catch
            {
                value = _defaultValue;
                valid = false;
            }

            var inRange = true;

            if (_hasRange)
            {
                inRange = _minValue.CompareTo(value) <= 0 && _maxValue.CompareTo(value) >= 0;
            }

            if (valid && inRange)
            {
                Value = value;
                DialogResult = DialogResult.OK;
                Close();
            }
            else if (!valid)
            {
                MessageBox.Show("Not a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else // (!inRange)
            {
                var message = $"Value not in range {_minValue} - {_maxValue}.";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Value = _defaultValue;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

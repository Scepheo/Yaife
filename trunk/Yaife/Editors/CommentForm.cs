using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;

namespace Yaife
{
	public partial class CommentForm : Form
	{
		public string[] Comments
		{
			get
			{
				return textBox.Lines;
			}
			set
			{
				textBox.Lines = value;
			}
		}

		public CommentForm()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}

	public class CommentEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var form = new CommentForm();
			form.Comments = value as string[];

			if (form.ShowDialog() == DialogResult.OK)
				return form.Comments;
			else
				return value;
		}
	}

	public class CommentConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string[]))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is string[] && destinationType == typeof(string))
			{
				var array = value as string[];

				if (array.Length > 0)
					return array[0];
				else
					return "";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Yaife
{
	public partial class PositionedSubtitleForm : Form
	{
		public PositionedSubtitle[] PositionedSubtitles
		{
			get { return positionedSubtitlesBindingSource.Cast<PositionedSubtitle>().ToArray(); }
			set { positionedSubtitlesBindingSource.DataSource = value; }
		}

		public PositionedSubtitleForm()
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

	public class PositionedSubtitleEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var form = new PositionedSubtitleForm();
			form.PositionedSubtitles = value as PositionedSubtitle[];

			if (form.ShowDialog() == DialogResult.OK)
				return form.PositionedSubtitles;
			else
				return value;
		}
	}

	public class PositionedSubtitleConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(PositionedSubtitle[]))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is PositionedSubtitle[] && destinationType == typeof(string))
			{
				var array = value as PositionedSubtitle[];

				if (array.Length > 0)
					return array[0].Text;
				else
					return "";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

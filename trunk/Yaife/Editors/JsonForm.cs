using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Yaife
{
	public partial class JsonForm : Form
	{
		JObject internalObject;

		public dynamic Object
		{
			get
			{
				return internalObject;
			}
			set
			{
				internalObject = value;
				propertyGrid.SelectedObject = new PropertyJObject(internalObject);
			}
		}

		public JsonForm()
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

	public class JsonEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var form = new JsonForm();
			form.Object = value as JObject;

			if (form.ShowDialog() == DialogResult.OK)
				return form.Object;
			else
				return value;
		}
	}

	public class JsonConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(JObject))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is JObject && destinationType == typeof(string))
			{
				var obj = value as JObject;
				return obj.ToString();
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

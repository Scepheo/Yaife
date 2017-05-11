using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Yaife.Editors
{
    public partial class JsonForm : Form
    {
        private JObject _internalObject;

        public dynamic Object
        {
            get => _internalObject;

            set
            {
                _internalObject = value;
                propertyGrid.SelectedObject = new PropertyJObject(_internalObject);
            }
        }

        public JsonForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
            var form = new JsonForm { Object = value as JObject };
            return form.ShowDialog() == DialogResult.OK ? form.Object : value;
        }
    }

    public class JsonConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(JObject) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is JObject jObject && destinationType == typeof(string))
            {
                return jObject.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

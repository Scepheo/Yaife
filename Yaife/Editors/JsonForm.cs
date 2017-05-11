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
            get
            {
                return _internalObject;
            }
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

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
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
            if (value is JObject && destinationType == typeof(string))
            {
                var obj = value as JObject;
                return obj.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

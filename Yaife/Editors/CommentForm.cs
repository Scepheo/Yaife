using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;

namespace Yaife.Editors
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
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
            var form = new CommentForm { Comments = value as string[] };
            return form.ShowDialog() == DialogResult.OK ? form.Comments : value;
        }
    }

    public class CommentConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string[]) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var strings = value as string[];

            if (strings != null && destinationType == typeof(string))
            {
                return strings.Length > 0 ? strings[0] : "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

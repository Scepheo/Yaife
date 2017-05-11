using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Yaife.Editors
{
    public partial class SubtitleForm : Form
    {
        public Subtitle[] Subtitles
        {
            get { return subtitlesBindingSource.Cast<Subtitle>().ToArray(); }
            set { subtitlesBindingSource.DataSource = value; }
        }

        public SubtitleForm()
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

    public class SubtitleEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var form = new SubtitleForm { Subtitles = value as Subtitle[] };
            return form.ShowDialog() == DialogResult.OK ? form.Subtitles : value;
        }
    }

    public class SubtitleConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Subtitle[]) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var subtitles = value as Subtitle[];

            if (subtitles != null && destinationType == typeof(string))
            {
                return subtitles.Length > 0 ? subtitles[0].Text : "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

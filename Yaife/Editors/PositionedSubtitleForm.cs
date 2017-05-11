using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Yaife.Editors
{
    public partial class PositionedSubtitleForm : Form
    {
        public PositionedSubtitle[] PositionedSubtitles
        {
            get => positionedSubtitlesBindingSource.Cast<PositionedSubtitle>().ToArray();
            set => positionedSubtitlesBindingSource.DataSource = value;
        }

        public PositionedSubtitleForm()
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

    public class PositionedSubtitleEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var form = new PositionedSubtitleForm { PositionedSubtitles = value as PositionedSubtitle[] };
            return form.ShowDialog() == DialogResult.OK ? form.PositionedSubtitles : value;
        }
    }

    public class PositionedSubtitleConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(PositionedSubtitle[]) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is PositionedSubtitle[] subtitles && destinationType == typeof(string))
            {
                return subtitles.Length > 0 ? subtitles[0].Text : "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

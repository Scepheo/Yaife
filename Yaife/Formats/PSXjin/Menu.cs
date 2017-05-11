using System.Reflection;
using System.Windows.Forms;

namespace Yaife.Formats.PSXjin
{
    public sealed class Menu : ToolStripMenuItem
    {
        private MovieTab _tab;
        private readonly Movie _movie;

        public Menu(MovieTab tab)
        {
            Text = "PSXjin";
            _tab = tab;
            _movie = tab.Movie as Movie;

            var exportToPxm = new ToolStripMenuItem("Export to PXM");
            exportToPxm.Click += (s, e) => ExportToPxm();
            exportToPxm.ToolTipText = "Save the movie as a PXM file, for PCSX-rr.";
            DropDownItems.Add(exportToPxm);
        }

        private void ExportToPxm()
        {
            var dialog = new SaveFileDialog { Filter = "PCSX-rr|*.pxm|All Files|*.*" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = dialog.FileName;

                var pxm = ConvertToPxm(_movie);
                pxm.WriteFile(fileName);
            }
        }

        private PCSX.Movie ConvertToPxm(Movie movie)
        {
            var pxm = new PCSX.Movie { RealHeader = new PCSX.Header() };

            // Copy header values
            var movieType = movie.RealHeader.GetType();

            foreach (var member in pxm.RealHeader.GetType().GetMembers())
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    var value = movieType.GetField(member.Name).GetValue(movie.RealHeader);
                    ((FieldInfo)member).SetValue(pxm.RealHeader, value);
                }

                if (member.MemberType == MemberTypes.Property)
                {
                    var value = movieType.GetProperty(member.Name).GetValue(movie.RealHeader);
                    ((PropertyInfo)member).SetValue(pxm.RealHeader, value);
                }
            }

            pxm.RealHeader.Signature = new byte[] { 0x50, 0x58, 0x4D, 0x20 };
            pxm.RealHeader.MovieVersion = 2;
            pxm.RealHeader.EmulatorVersion = 12;

            // Our current input log should be perfectly parseable
            pxm.InputLog = movie.InputLog;

            return pxm;
        }
    }
}

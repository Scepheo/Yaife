using System.IO;
using System.Windows.Forms;
using Yaife.Utilities;

namespace Yaife.Formats.Hourglass
{
    public sealed class Menu : ToolStripMenuItem
    {
        private readonly MovieTab _tab;
        private readonly Movie _movie;

        public Menu(MovieTab tab)
        {
            Text = "Hourglass";
            _tab = tab;
            _movie = tab.Movie as Movie;

            var nullSync = new ToolStripMenuItem("Zero Desync Detection");
            nullSync.Click += (s, e) => NullSyncCheck();
            nullSync.ToolTipText = "Sets all the values in the desync detection array to 0, so it is ignored.";
            DropDownItems.Add(nullSync);

            var changeExe = new ToolStripMenuItem("Change Executable");
            changeExe.Click += (s, e) => ChangeExecutable();
            changeExe.ToolTipText = "Adjusts the exe filename, size and CRC32 to match a selected file.";
            DropDownItems.Add(changeExe);
        }

        private void NullSyncCheck()
        {
            _movie.RealHeader.DesyncDetection = new uint[16];
            _tab.HeaderControl.Refresh();
        }

        private void ChangeExecutable()
        {
            var dialog = new OpenFileDialog { Filter = "Executable|*.exe|All Files|*.*" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = Path.GetFileName(dialog.FileName);

                var data = File.ReadAllBytes(dialog.FileName);
                var crc = Crc32.CalculateCrc(data);

                _movie.RealHeader.ExeName = fileName;
                _movie.RealHeader.Crc32 = crc;
                _movie.RealHeader.ExeSize = (uint)data.Length;
            }

            _tab.HeaderControl.Refresh();
        }
    }
}

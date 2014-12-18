using System.IO;
using System.Windows.Forms;

namespace Yaife.Formats.Hourglass
{
	public class Menu : ToolStripMenuItem
	{
		private MovieTab tab;
		private Movie movie;

		public Menu(MovieTab tab)
		{
			this.Text = "HourGlass";
			this.tab = tab;
			this.movie = tab.Movie as Movie;

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
			movie.RealHeader.DesyncDetection = new uint[16];
			tab.HeaderControl.Refresh();
		}

		private void ChangeExecutable()
		{
			var dialog = new OpenFileDialog();
			dialog.Filter = "Executable|*.exe|All Files|*.*";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var fileName = Path.GetFileName(dialog.FileName);

				var data = File.ReadAllBytes(dialog.FileName);
				uint crc = Utilities.CRC32.CalculateCrc(data);

				movie.RealHeader.ExeName = fileName;
				movie.RealHeader.CRC32 = crc;
				movie.RealHeader.ExeSize = (uint)data.Length;
			}

			tab.HeaderControl.Refresh();
		}
	}
}

using System.IO;
using System.Windows.Forms;

namespace Yaife.Formats.Dolphin
{
	public class Menu : ToolStripMenuItem
	{
		private MovieTab tab;
		private Movie movie;

		public Menu(MovieTab tab)
		{
			this.Text = "Dolphin";
			this.tab = tab;
			this.movie = tab.Movie as Movie;

			var ensurePlayback = new ToolStripMenuItem("Ensure Movie Playback");
			ensurePlayback.Click += (s, e) => EnsurePlayback();
			ensurePlayback.ToolTipText = "Ensures the full playback of a movie file by setting wildly incorrect frame and tick counts.";
			DropDownItems.Add(ensurePlayback);
		}

		private void EnsurePlayback()
		{
			uint inputCount = (uint)(tab.InputGrid.RowCount - 1);

			movie.RealHeader.InputCount = inputCount;
			movie.RealHeader.FrameCount = ulong.MaxValue;
			movie.RealHeader.TickCount  = ulong.MaxValue;

			tab.HeaderControl.Refresh();
		}
	}
}

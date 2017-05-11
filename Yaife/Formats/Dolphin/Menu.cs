using System.Windows.Forms;

namespace Yaife.Formats.Dolphin
{
    public sealed class Menu : ToolStripMenuItem
    {
        private readonly MovieTab _tab;
        private readonly Movie _movie;

        public Menu(MovieTab tab)
        {
            Text = "Dolphin";
            _tab = tab;
            _movie = tab.Movie as Movie;

            var ensurePlayback = new ToolStripMenuItem("Ensure Movie Playback");
            ensurePlayback.Click += (s, e) => EnsurePlayback();
            ensurePlayback.ToolTipText = "Ensures the full playback of a movie file by setting wildly incorrect frame and tick counts.";
            DropDownItems.Add(ensurePlayback);
        }

        private void EnsurePlayback()
        {
            var inputCount = (uint)(_tab.InputGrid.RowCount - 1);

            _movie.RealHeader.InputCount = inputCount;
            _movie.RealHeader.FrameCount = ulong.MaxValue;
            _movie.RealHeader.TickCount  = ulong.MaxValue;

            _tab.HeaderControl.Refresh();
        }
    }
}

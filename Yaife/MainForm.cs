using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Yaife.Formats;
using Yaife.Utilities;

namespace Yaife
{
    public partial class MainForm : Form
    {
		// Static access, so everything can use it.
		public static StatusStrip StatusBar;
		public static ToolStripProgressBar ProgressBar;
		public static ToolStripLabel ProgressLabel;

        public MainForm()
        {
            InitializeComponent();

			// Set the static data
			ProgressBar = this.statusProgressBar;
			StatusBar = this.statusStrip;
			ProgressLabel = this.statusProgressLabel;
        }

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			closeAllTabs();
		}

		#region File Menu
		private void FileMenu_DropDownOpening(object sender, EventArgs e)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			saveAsToolStripMenuItem.Enabled = (tab != null);
			saveToolStripMenuItem.Enabled = (tab != null);

			if (tab != null)
				saveToolStripMenuItem.Text = "Save " + tab.Text;
		}

		private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Filter = FormatSelector.GetFileFilter();

			if (dialog.ShowDialog() == DialogResult.OK)
			{
#if !DEBUG
				try
				{
#endif
					var movie = FormatSelector.Open(dialog.FileName);
					var tab = new MovieTab(movie);
					MainTabControl.TabPages.Add(tab);
					MainTabControl.SelectedTab = tab;

					if (MainTabControl.TabCount == 1)
						openMovieMenu();
#if !DEBUG
				}
				catch (Exception err)
				{
					MessageBox.Show(
						"Could not open " + dialog.FileName + ":\n" + err.Message,
						"Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
						);
				}
#endif
			}
		}

		private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			saveCurrentTab();
		}

		private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			saveCurrentTabAs();
		}

		private void closeToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			closeAllTabs();
		}

		private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		#endregion

		#region Context Menu
		private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;
			saveMenuItem.Text = "Save " + tab.Text;
		}
		
		private void saveMenuItem_Click(object sender, System.EventArgs e)
		{
			saveCurrentTab();
		}

		private void saveAsMenuItem_Click(object sender, System.EventArgs e)
		{
			saveCurrentTabAs();
		}

		private void closeMenuItem_Click(object sender, System.EventArgs e)
		{
			closeCurrentTab();
		}
		#endregion

		#region Tab Handling
		private void closeCurrentTab()
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			if (tab == null)
				return;

			if (tab.PendingChanges)
			{
				var message = string.Format("{0} has unsaved changes, would you like to save?", tab.Text);
				var result = MessageBox.Show(message, "Save changes?", MessageBoxButtons.YesNoCancel);

				if (result == DialogResult.Yes)
					saveCurrentTab();
				else if (result == DialogResult.Cancel)
					return;
			}

			MainTabControl.TabPages.Remove(tab);
			closeMovieMenu();
			tab.Dispose();
		}

		private void closeAllTabs()
		{
			// Close all open tabs
			while (MainTabControl.TabPages.Count > 0)
				closeCurrentTab();
		}

		private void saveCurrentTab()
		{
			var tab = MainTabControl.SelectedTab as MovieTab;
			tab.Save();
		}

		private void saveCurrentTabAs()
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			var dialog = new SaveFileDialog();
			dialog.Filter = tab.Movie.Description + "|*" + String.Join(";*", tab.Movie.Extensions);
			dialog.FileName = Path.GetFileName(tab.Movie.Path);

			if (dialog.ShowDialog() == DialogResult.OK)
				tab.SaveAs(dialog.FileName);
		}
		#endregion

		#region Format menus
		private const int MOVIE_MENU_INDEX = 1;
		private bool movieMenuOpen = false;

		private void closeMovieMenu()
		{
			if (movieMenuOpen && MainTabControl.TabCount > 0)
			{
				movieMenuOpen = false;
				MainMenu.Items.RemoveAt(MOVIE_MENU_INDEX);
			}
		}

		private void openMovieMenu()
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			if (tab.Movie.HasMenu)
			{
				var menu = tab.Movie.GetMovieMenu(tab);
				movieMenuOpen = true;
				MainMenu.Items.Insert(MOVIE_MENU_INDEX, menu);
			}
		}

		private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			closeMovieMenu();

			if (MainTabControl.TabCount > 0)
				openMovieMenu();
		}
		#endregion

		#region Header menu
		private void headerMenu_DropDownOpening(object sender, EventArgs e)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			if (tab == null)
			{
				foreach (ToolStripMenuItem item in HeaderMenu.DropDownItems)
					item.Enabled = false;
			}
			else
			{
				var type = tab.Movie.GetType();

				var hasFrameCount = typeof(IFrameCount).IsAssignableFrom(type);
				adjustFrameCountToolStripMenuItem.Enabled = hasFrameCount;

				var hasSubtitles = typeof(ISubtitled).IsAssignableFrom(type);
				subtitlesToolStripMenuItem.Enabled = hasSubtitles;
			}
		}

		private void adjustFrameCountToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			if (tab == null)
				return;
			
			// Get the number of frames (don't count the new row at the bototm)
			uint inputCount = (uint)(tab.InputGrid.RowCount - 1);

			// Set the number of frames
			var movie = tab.Movie as IFrameCount;
			movie.SetFrameCount(inputCount);

			// Refresh
			tab.HeaderControl.Refresh();
		}

		private void exportToSubRipsrtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			if (tab == null)
				return;

			// Get the movie's fps
			var fpsDialog = new ValueDialog<float>(
				"Please enter the movie's fps (frames per second):",
				"Enter fps",
				60,
				s => float.Parse(s),
				0,
				float.MaxValue);

			float fps = 60;

			if (fpsDialog.ShowDialog() == DialogResult.OK)
				fps = fpsDialog.Value;
			else
				return;

			// Get the file name for the subtitles
			var fileDialog = new SaveFileDialog();
			fileDialog.Filter = "SubRip|*.srt|All Files|*.*";

			var result = fileDialog.ShowDialog();

			if (result != DialogResult.OK)
				return;
			
			// Get the text for the file
			var movie = tab.Movie as ISubtitled;
			var subtitles = movie.GetSubtitles();
			var text = Subtitle.ToSubRip(subtitles, fps);
			
			// Write the file
			File.WriteAllText(fileDialog.FileName, text, UTF8Encoding.UTF8);

			// Confirm
			var message = String.Format("Succesfully exported subtitles to {0}.", fileDialog.FileName);
			MessageBox.Show(message, "Success");
		}

		private void exportToSubStationAlphaassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			if (tab == null)
				return;

			// Get the movie's fps
			var fpsDialog = new ValueDialog<float>(
				"Please enter the movie's fps (frames per second):",
				"Enter fps",
				60,
				s => float.Parse(s),
				0,
				float.MaxValue);

			float fps = 60;

			if (fpsDialog.ShowDialog() == DialogResult.OK)
				fps = fpsDialog.Value;
			else
				return;

			// Get the file name for the subtitles
			var fileDialog = new SaveFileDialog();
			fileDialog.Filter = "Sub Station Alpha|*.ass|All Files|*.*";

			var result = fileDialog.ShowDialog();

			if (result != DialogResult.OK)
				return;

			// Get the text for the file
			var movie = tab.Movie as ISubtitled;
			var subtitles = movie.GetSubtitles();
			var text = Subtitle.ToSubStationAlpha(subtitles, fps);

			// Write the file
			File.WriteAllText(fileDialog.FileName, text, UTF8Encoding.UTF8);

			// Confirm
			var message = String.Format("Succesfully exported subtitles to {0}.", fileDialog.FileName);
			MessageBox.Show(message, "Success");
		}
		#endregion

		#region Input menu
		private void InputMenu_DropDownOpening(object sender, EventArgs e)
		{
			var usable = (MainTabControl.TabCount > 0);

			foreach (ToolStripMenuItem item in InputMenu.DropDownItems)
				item.Enabled = usable;
		}

		private void trimEmptyFramesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;

			if (tab == null)
				return;

			var movie = tab.Movie;
			var log = movie.InputLog;

			while (log.RowCount > 1)
			{
				var row = log.Rows[log.RowCount - 2];
				var array = new string[row.Cells.Count];

				for (int i = 0; i < row.Cells.Count; i++)
					array[i] = row.Cells[i].Value.ToString();

				if (movie.IsEmptyFrame(array))
					log.Rows.RemoveAt(log.RowCount - 2);
				else
					break;
			}

			tab.InputGrid.Refresh();
		}

		private void gotoFrame(int frameNumber)
		{
			var tab = MainTabControl.SelectedTab as MovieTab;
			var log = tab.InputGrid;

			if (frameNumber - 1 < log.RowCount)
				log.FirstDisplayedScrollingRowIndex = frameNumber - 1;
		}

		private void goToFrameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MainTabControl.SelectedTab == null)
				return;

			var tab = (MainTabControl.SelectedTab as MovieTab);
			int frameCount = tab.InputGrid.RowCount - 1;
			
			var dialog = new ValueDialog<int>("Enter frame:", "Jump To Frame", 0, (s => int.Parse(s)), 0, frameCount);

			if (dialog.ShowDialog() == DialogResult.OK)
				gotoFrame(dialog.Value);
		}
		#endregion

		#region Tools
		private void crc32Menu_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Filter = "All Files|*.*";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var data = File.ReadAllBytes(dialog.FileName);

				var hash = CRC32.CalculateCrc(data);

				CopyMessageBox.Show("CRC32:", hash.ToString(), "Calculated CRC32");
			}
		}

		private void md5Menu_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Filter = "All Files|*.*";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var data = File.ReadAllBytes(dialog.FileName);

				byte[] hash;
				using (var md5 = MD5.Create())
					hash = md5.ComputeHash(data);

				var sb = new StringBuilder();

				foreach (var b in hash)
					sb.AppendFormat("{0:X2}", b);

				CopyMessageBox.Show("MD5:", sb.ToString(), "Calculated MD5");
			}
		}

		private void sha1Menu_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Filter = "All Files|*.*";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var data = File.ReadAllBytes(dialog.FileName);

				byte[] hash;
				using (var sha = SHA1.Create())
					hash = sha.ComputeHash(data);

				var sb = new StringBuilder();

				foreach (var b in hash)
					sb.AppendFormat("{0:X2}", b);

				CopyMessageBox.Show("SHA1:", sb.ToString(), "Calculated SHA1");
			}
		}
		#endregion
	}
}

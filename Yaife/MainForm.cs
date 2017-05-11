using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Yaife.Editors;
using Yaife.FormatInterfaces;
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
            ProgressBar = statusProgressBar;
            StatusBar = statusStrip;
            ProgressLabel = statusProgressLabel;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAllTabs();
        }

#region File Menu

        private void FileMenu_DropDownOpening(object sender, EventArgs e)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;

            saveAsToolStripMenuItem.Enabled = tab != null;
            saveToolStripMenuItem.Enabled = tab != null;

            if (tab != null)
            {
                saveToolStripMenuItem.Text = "Save " + tab.Text;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = FormatSelector.GetFileFilter() };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var movie = FormatSelector.Open(dialog.FileName);
                    var tab = new MovieTab(movie);
                    MainTabControl.TabPages.Add(tab);
                    MainTabControl.SelectedTab = tab;

                    if (MainTabControl.TabCount == 1)
                    {
                        OpenMovieMenu();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(
                        "Could not open " + dialog.FileName + ":\n" + err.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );

#if DEBUG
                    throw;
#endif
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentTab();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentTabAs();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllTabs();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

#endregion

#region Context Menu

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;
            saveMenuItem.Text = "Save " + tab.Text;
        }
        
        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentTab();
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentTabAs();
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            CloseCurrentTab();
        }

#endregion

#region Tab Handling

        private void CloseCurrentTab()
        {
            var tab = MainTabControl.SelectedTab as MovieTab;
            if (tab == null) return;

            if (tab.PendingChanges)
            {
                var message = $"{tab.Text} has unsaved changes, would you like to save?";
                var result = MessageBox.Show(message, "Save changes?", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    SaveCurrentTab();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            MainTabControl.TabPages.Remove(tab);
            CloseMovieMenu();
            tab.Dispose();
        }

        private void CloseAllTabs()
        {
            while (MainTabControl.TabPages.Count > 0)
            {
                CloseCurrentTab();
            }
        }

        private void SaveCurrentTab()
        {
            var tab = MainTabControl.SelectedTab as MovieTab;
            tab.Save();
        }

        private void SaveCurrentTabAs()
        {
            var tab = MainTabControl.SelectedTab as MovieTab;

            var dialog = new SaveFileDialog
            {
                Filter = tab.Movie.Description + "|*" + string.Join(";*", tab.Movie.Extensions),
                FileName = Path.GetFileName(tab.Movie.Path)
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tab.SaveAs(dialog.FileName);
            }
        }

#endregion

#region Format menus

        private const int MovieMenuIndex = 1;
        private bool _movieMenuOpen;

        private void CloseMovieMenu()
        {
            if (_movieMenuOpen && MainTabControl.TabCount > 0)
            {
                _movieMenuOpen = false;
                MainMenu.Items.RemoveAt(MovieMenuIndex);
            }
        }

        private void OpenMovieMenu()
        {
            var tab = MainTabControl.SelectedTab as MovieTab;

            if (tab.Movie.HasMenu)
            {
                var menu = tab.Movie.GetMovieMenu(tab);
                _movieMenuOpen = true;
                MainMenu.Items.Insert(MovieMenuIndex, menu);
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseMovieMenu();

            if (MainTabControl.TabCount > 0)
            {
                OpenMovieMenu();
            }
        }

#endregion

#region Header menu

        private void HeaderMenu_DropDownOpening(object sender, EventArgs e)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;

            if (tab == null)
            {
                foreach (ToolStripMenuItem item in HeaderMenu.DropDownItems)
                {
                    item.Enabled = false;
                }
            }
            else
            {
                adjustFrameCountToolStripMenuItem.Enabled = tab.Movie is IFrameCount;
                subtitlesToolStripMenuItem.Enabled = tab.Movie is ISubtitled;
            }
        }

        private void AdjustFrameCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;

            if (tab == null)
                return;
            
            // Get the number of frames (don't count the new row at the bototm)
            var inputCount = (uint)(tab.InputGrid.RowCount - 1);

            // Set the number of frames
            var movie = tab.Movie as IFrameCount;
            movie.SetFrameCount(inputCount);

            // Refresh
            tab.HeaderControl.Refresh();
        }

        private void ExportToSubRipsrtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;
            if (tab == null) return;

            // Get the movie's fps
            var fpsDialog = new ValueDialog<float>(
                "Please enter the movie's fps (frames per second):",
                "Enter fps",
                60,
                float.Parse,
                0,
                float.MaxValue);

            float fps = 60;

            if (fpsDialog.ShowDialog() == DialogResult.OK)
            {
                fps = fpsDialog.Value;
            }
            else
            {
                return;
            }

            // Get the file name for the subtitles
            var fileDialog = new SaveFileDialog { Filter = "SubRip|*.srt|All Files|*.*" };

            var result = fileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            
            // Get the text for the file
            var movie = tab.Movie as ISubtitled;
            var subtitles = movie.GetSubtitles();
            var text = Subtitle.ToSubRip(subtitles, fps);
            
            // Write the file
            File.WriteAllText(fileDialog.FileName, text, Encoding.UTF8);

            // Confirm
            var message = $"Succesfully exported subtitles to {fileDialog.FileName}.";
            MessageBox.Show(message, "Success");
        }

        private void ExportToSubStationAlphaassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;
            if (tab == null) return;

            // Get the movie's fps
            var fpsDialog = new ValueDialog<float>(
                "Please enter the movie's fps (frames per second):",
                "Enter fps",
                60,
                float.Parse,
                0,
                float.MaxValue);

            float fps = 60;

            if (fpsDialog.ShowDialog() == DialogResult.OK)
            {
                fps = fpsDialog.Value;
            }
            else
            {
                return;
            }

            // Get the file name for the subtitles
            var fileDialog = new SaveFileDialog { Filter = "Sub Station Alpha|*.ass|All Files|*.*" };

            var result = fileDialog.ShowDialog();
            if (result != DialogResult.OK) return;

            // Get the text for the file
            var movie = tab.Movie as ISubtitled;
            var subtitles = movie.GetSubtitles();
            var text = Subtitle.ToSubStationAlpha(subtitles, fps);

            // Write the file
            File.WriteAllText(fileDialog.FileName, text, Encoding.UTF8);

            // Confirm
            var message = $"Succesfully exported subtitles to {fileDialog.FileName}.";
            MessageBox.Show(message, "Success");
        }

#endregion

#region Input menu

        private void InputMenu_DropDownOpening(object sender, EventArgs e)
        {
            var usable = MainTabControl.TabCount > 0;

            foreach (ToolStripMenuItem item in InputMenu.DropDownItems)
            {
                item.Enabled = usable;
            }
        }

        private void TrimEmptyFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;
            if (tab == null) return;

            var movie = tab.Movie;
            var log = movie.InputLog;

            while (log.RowCount > 1)
            {
                var row = log.Rows[log.RowCount - 2];
                var array = new string[row.Cells.Count];

                for (var i = 0; i < row.Cells.Count; i++)
                {
                    array[i] = row.Cells[i].Value.ToString();
                }

                if (movie.IsEmptyFrame(array))
                {
                    log.Rows.RemoveAt(log.RowCount - 2);
                }
                else
                {
                    break;
                }
            }

            tab.InputGrid.Refresh();
        }

        private void GotoFrame(int frameNumber)
        {
            var tab = MainTabControl.SelectedTab as MovieTab;
            var log = tab.InputGrid;

            if (frameNumber - 1 < log.RowCount)
            {
                log.FirstDisplayedScrollingRowIndex = frameNumber - 1;
            }
        }

        private void GoToFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainTabControl.SelectedTab == null) return;

            var tab = MainTabControl.SelectedTab as MovieTab;
            var frameCount = tab.InputGrid.RowCount - 1;
            
            var dialog = new ValueDialog<int>("Enter frame:", "Jump To Frame", 0, int.Parse, 0, frameCount);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                GotoFrame(dialog.Value);
            }
        }

#endregion

#region Tools

        private void Crc32Menu_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "All Files|*.*" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var data = File.ReadAllBytes(dialog.FileName);
                var hash = Crc32.CalculateCrc(data);
                var bytes = BitConverter.GetBytes(hash);
                var hashString = BitConverter.ToString(bytes);
                CopyMessageBox.Show("CRC32:", hash.ToString(), "Calculated CRC32");
            }
        }

        private void Md5Menu_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "All Files|*.*" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var data = File.ReadAllBytes(dialog.FileName);

                byte[] hash;

                using (var md5 = MD5.Create())
                {
                    hash = md5.ComputeHash(data);
                }

                var hashString = BitConverter.ToString(hash);
                CopyMessageBox.Show("MD5:", hashString, "Calculated MD5");
            }
        }

        private void Sha1Menu_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "All Files|*.*" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var data = File.ReadAllBytes(dialog.FileName);

                byte[] hash;

                using (var sha = SHA1.Create())
                {
                    hash = sha.ComputeHash(data);
                }

                var hashString = BitConverter.ToString(hash);

                CopyMessageBox.Show("SHA1:", hashString, "Calculated SHA1");
            }
        }

#endregion
    }
}

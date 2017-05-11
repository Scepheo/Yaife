using System.IO;
using System.Windows.Forms;

namespace Yaife
{
    public sealed class MovieTab : TabPage
    {
        public readonly IMovie Movie;
        public readonly SplitContainer SplitContainer;
        public readonly SplitterPanel HeaderPanel;
        public readonly SplitterPanel InputPanel;
        public readonly InputLog InputGrid;
        public readonly PropertyGrid HeaderControl;

        private bool _headerChanges;

        public MovieTab(IMovie movie)
        {
            Movie = movie;

            // Create the controls
            SplitContainer = new SplitContainer();
            HeaderPanel = SplitContainer.Panel1;
            InputPanel = SplitContainer.Panel2;

            // Set container properties
            SplitContainer.Orientation = Orientation.Vertical;
            SplitContainer.Dock = DockStyle.Fill;
            SplitContainer.BorderStyle = BorderStyle.FixedSingle;
            SplitContainer.SplitterDistance = SplitContainer.Width / 2;

            // Set this tab correctly
            Text = Path.GetFileName(movie.Path);
            Controls.Add(SplitContainer);

            // Fill the input log
            InputGrid = movie.InputLog;
            InputPanel.Controls.Add(InputGrid);

            // Fill the header
            HeaderControl = new PropertyGrid
            {
                SelectedObject = movie.Header,
                Dock = DockStyle.Fill
            };
            HeaderPanel.Controls.Add(HeaderControl);
            HeaderControl.ContextMenuStrip = new ContextMenuStrip();
            HeaderControl.PropertyValueChanged += (s, e) => _headerChanges = true;

            // Set status label
            MainForm.ProgressLabel.Text = "Opened file";
        }

        public void Save()
        {
            var tempFile = Path.GetTempFileName();

#if !DEBUG
            try
            {
#endif
                // Attempt to save to temporary file
                Movie.WriteFile(tempFile);

                // Copy to actual file
                File.Copy(tempFile, Movie.Path, true);

                // Reset changes
                _headerChanges = false;
                InputGrid.PendingChanges = false;

                // Set status text
                MainForm.ProgressLabel.Text = "Saved file";
#if !DEBUG
            }
            catch (Exception e)
            {
                // Display error
                MessageBox.Show(
                        "Error saving " + Movie.Path + ":\n" + e.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
            }
            finally
            {
#endif
                // Clean up temporary file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
#if !DEBUG
            }
#endif
        }

        public void SaveAs(string path)
        {
            Movie.Path = path;
            Text = Path.GetFileName(path);
            Save();
        }

        public bool PendingChanges => _headerChanges || InputGrid.PendingChanges;
    }
}

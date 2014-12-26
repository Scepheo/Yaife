using System;
using System.IO;
using System.Windows.Forms;

namespace Yaife
{
    public class MovieTab : TabPage
    {
		public IMovie Movie;
        public SplitContainer SplitContainer;
        public SplitterPanel HeaderPanel;
        public SplitterPanel InputPanel;
		public InputLog InputGrid;
        public PropertyGrid HeaderControl;

		private bool headerChanges = false;

        public MovieTab(IMovie movie)
        {
			this.Movie = movie;

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
            this.Text = Path.GetFileName(movie.Path);
            this.Controls.Add(SplitContainer);

            // Fill the input log
            InputGrid = movie.InputLog;
            InputPanel.Controls.Add(InputGrid);

            // Fill the header
			HeaderControl = new PropertyGrid();
			HeaderControl.SelectedObject = movie.Header;
            HeaderControl.Dock = DockStyle.Fill;
            HeaderPanel.Controls.Add(HeaderControl);
			HeaderControl.ContextMenuStrip = new ContextMenuStrip();
			HeaderControl.PropertyValueChanged += (s, e) => headerChanges = true;

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
				headerChanges = false;
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
			this.Text = Path.GetFileName(path);
			Save();
		}

		public bool PendingChanges
		{
			get { return headerChanges || InputGrid.PendingChanges; }
		}
    }
}

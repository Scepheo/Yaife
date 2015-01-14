using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yaife
{
    public class InputLog : DataGridView
    {
		private const string inputKey = "Input";
		private ContextMenuStrip contextMenu;
		private IContainer components;
		private ToolStripMenuItem copyMenuItem;
		private ToolStripMenuItem pasteMenuItem;
		private ToolStripMenuItem deleteMenuItem;
		private ToolStripMenuItem cutMenuItem;

		public IEnumerable<string[]> Log
		{
			get
			{
				foreach (DataGridViewRow row in this.Rows)
					if (row.Cells[0].Value != null)
					{
						var array = new string[row.Cells.Count];

						for (int i = 0; i < row.Cells.Count; i++)
							array[i] = row.Cells[i].Value.ToString();

						yield return array;
					}

				yield break;
			}
		}

		public bool PendingChanges = false;

		public InputLog() : this(new string[0], new List<IFrame>())
		{}

		public InputLog(string[] headers, IEnumerable<IFrame> frames)
		{
			InitializeComponent();

			// Add columns
			foreach (var header in headers)
				Columns.Add(header, header);

			// Settings
			AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 252);
			EnableHeadersVisualStyles = false;
			RowHeadersWidth           = 100;
			ClipboardCopyMode         = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			SelectionMode             = DataGridViewSelectionMode.FullRowSelect;
			Dock                      = DockStyle.Fill;
			AutoSizeColumnsMode       = DataGridViewAutoSizeColumnsMode.Fill;
			DefaultCellStyle.Font     = new Font(FontFamily.GenericMonospace, DefaultCellStyle.Font.Size);
			ContextMenuStrip          = contextMenu;

			// Add functionality to the context menu
			contextMenu.Opening  += (s, e) => pasteMenuItem.Enabled = Clipboard.ContainsText();
			cutMenuItem.Click    += (s, e) => cut();
			copyMenuItem.Click   += (s, e) => copy();
			pasteMenuItem.Click  += (s, e) => paste();
			deleteMenuItem.Click += (s, e) => delete();

			// Populate log
			var array = new DataGridViewRow[frames.Count()];
			int i = 0;
			int update = array.Length / 20;
			foreach (var frame in frames)
			{
				array[i] = new DataGridViewRow();
				array[i].CreateCells(this, frame.ToStrings());
				i++;

				// Update progress bar
				if (i % update == 0)
					MainForm.ProgressBar.Value = MainForm.ProgressBar.Maximum * i / array.Length;
			}

			// Insert the created rows
			Rows.InsertRange(0, array);

			// Number all rows
			numberRows(0);

			// Event handlers for row renumbering
			RowsAdded   += (s, e) => { numberRows(e.RowIndex); PendingChanges = true; };
			//RowsRemoved += (s, e) => { numberRows(e.RowIndex); PendingChanges = true; };

			// Event handler for content changing
			CellValueChanged += (s, e) => PendingChanges = true;

			// Reset progress bar
			MainForm.ProgressBar.Value = 0;
		}

		private void numberRows(int from)
		{
			for (int i = from; i < Rows.Count; i++)
				Rows[i].HeaderCell.Value = (i + 1).ToString();
		}

		private void delete()
		{
			int lowIndex = int.MaxValue;
			int count = 0;

			foreach (DataGridViewRow row in SelectedRows)
			{
				if (row.Index < lowIndex)
					lowIndex = row.Index;

				count++;

				//if (!row.IsNewRow)
					//Rows.Remove(row);
			}

			// Copy all rows
			var allRows = new DataGridViewRow[RowCount];
			Rows.CopyTo(allRows, 0);

			// Copy all rows that are not deleted
			// The "- 1" is because the last, empty row is counted, and we don't want it.
			var keptRows = new DataGridViewRow[RowCount - count - 1];
			Array.Copy(allRows, 0, keptRows, 0, lowIndex);
			Array.Copy(allRows, lowIndex + count, keptRows, lowIndex, keptRows.Length - lowIndex);

			// Find the currently "top" row
			var scrollIndex = FirstDisplayedScrollingRowIndex;

			// Clear and refill
			Rows.Clear();
			Rows.AddRange(keptRows);

			// Scroll back
			if (scrollIndex >= RowCount)
				scrollIndex = RowCount - 1;

			FirstDisplayedScrollingRowIndex = scrollIndex;

			// Renumber the rows
			numberRows(0);

			PendingChanges = true;
		}

		private void copy()
		{
			var sb = new StringBuilder();

			var rows = new DataGridViewRow[SelectedRows.Count];
			SelectedRows.CopyTo(rows, 0);

			foreach (var row in rows.OrderBy(r => r.Index))
				if (!row.IsNewRow)
					for (int i = 0; i < row.Cells.Count; i++)
						sb.Append(row.Cells[i].Value.ToString() + Environment.NewLine);

			Clipboard.SetText(sb.ToString());
		}

		private void paste()
		{
			if (!Clipboard.ContainsText())
				return;

			var split = Clipboard.GetText().Split(new string[]{ Environment.NewLine }, StringSplitOptions.None);
			var rows = new List<DataGridViewRow>();

			for (int i = 0; i + Columns.Count <= split.Length; i += Columns.Count)
			{
				var values = new string[Columns.Count];
				Array.Copy(split, i, values, 0, Columns.Count);

				var row = new DataGridViewRow();
				row.CreateCells(this, values);
				rows.Add(row);
			}

			Rows.InsertRange(this.SelectedRows[0].Index, rows.ToArray());
		}

		private void cut()
		{
			copy();
			delete();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem,
            this.deleteMenuItem});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(165, 100);
			// 
			// copyMenuItem
			// 
			this.copyMenuItem.Name = "copyMenuItem";
			this.copyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyMenuItem.Size = new System.Drawing.Size(164, 24);
			this.copyMenuItem.Text = "Copy";
			// 
			// pasteMenuItem
			// 
			this.pasteMenuItem.Name = "pasteMenuItem";
			this.pasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteMenuItem.Size = new System.Drawing.Size(164, 24);
			this.pasteMenuItem.Text = "Paste";
			// 
			// deleteMenuItem
			// 
			this.deleteMenuItem.Name = "deleteMenuItem";
			this.deleteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.deleteMenuItem.Size = new System.Drawing.Size(164, 24);
			this.deleteMenuItem.Text = "Delete";
			// 
			// cutMenuItem
			// 
			this.cutMenuItem.Name = "cutMenuItem";
			this.cutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutMenuItem.Size = new System.Drawing.Size(164, 24);
			this.cutMenuItem.Text = "Cut";
			// 
			// AbstractInputLog
			// 
			this.RowTemplate.Height = 24;
			this.contextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}
    }
}

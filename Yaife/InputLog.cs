using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yaife
{
    public sealed class InputLog : DataGridView
    {
        private const string InputKey = "Input";
        private ContextMenuStrip _contextMenu;
        private IContainer _components;
        private ToolStripMenuItem _copyMenuItem;
        private ToolStripMenuItem _pasteMenuItem;
        private ToolStripMenuItem _deleteMenuItem;
        private ToolStripMenuItem _cutMenuItem;

        public IEnumerable<string[]> Log
        {
            get
            {
                foreach (DataGridViewRow row in Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        var array = new string[row.Cells.Count];

                        for (var i = 0; i < row.Cells.Count; i++)
                        {
                            array[i] = row.Cells[i].Value.ToString();
                        }

                        yield return array;
                    }
                }
            }
        }

        public bool PendingChanges;

        public InputLog() : this(new string[0], new List<IFrame>()) {}

        public InputLog(string[] headers, IEnumerable<IFrame> frames)
        {
            InitializeComponent();

            // Add columns
            foreach (var header in headers)
            {
                Columns.Add(header, header);
            }

            // Settings
            AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 252);
            EnableHeadersVisualStyles = false;
            RowHeadersWidth           = 100;
            ClipboardCopyMode         = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            SelectionMode             = DataGridViewSelectionMode.FullRowSelect;
            Dock                      = DockStyle.Fill;
            AutoSizeColumnsMode       = DataGridViewAutoSizeColumnsMode.Fill;
            DefaultCellStyle.Font     = new Font(FontFamily.GenericMonospace, DefaultCellStyle.Font.Size);
            ContextMenuStrip          = _contextMenu;

            // Add functionality to the context menu
            _contextMenu.Opening  += (s, e) => _pasteMenuItem.Enabled = Clipboard.ContainsText();
            _cutMenuItem.Click    += (s, e) => Cut();
            _copyMenuItem.Click   += (s, e) => Copy();
            _pasteMenuItem.Click  += (s, e) => Paste();
            _deleteMenuItem.Click += (s, e) => Delete();

            // Populate log
            var array = new DataGridViewRow[frames.Count()];
            var i = 0;
            var update = array.Length / 20;

            foreach (var frame in frames)
            {
                array[i] = new DataGridViewRow();
                array[i].CreateCells(this, frame.ToStrings());
                i++;

                // Update progress bar
                if (i % update == 0)
                {
                    MainForm.ProgressBar.Value = MainForm.ProgressBar.Maximum * i / array.Length;
                }
            }

            // Insert the created rows
            Rows.InsertRange(0, array);

            // Number all rows
            NumberRows(0);

            // Event handlers for row renumbering
            RowsAdded   += (s, e) => { NumberRows(e.RowIndex); PendingChanges = true; };

            // Event handler for content changing
            CellValueChanged += (s, e) => PendingChanges = true;

            // Reset progress bar
            MainForm.ProgressBar.Value = 0;
        }

        private void NumberRows(int from)
        {
            for (var i = from; i < Rows.Count; i++)
            {
                Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void Delete()
        {
            var lowIndex = int.MaxValue;
            var count = 0;

            foreach (DataGridViewRow row in SelectedRows)
            {
                if (row.Index < lowIndex)
                {
                    lowIndex = row.Index;
                }

                count++;
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
            {
                scrollIndex = RowCount - 1;
            }

            FirstDisplayedScrollingRowIndex = scrollIndex;

            // Renumber the rows
            NumberRows(0);

            PendingChanges = true;
        }

        private void Copy()
        {
            var sb = new StringBuilder();

            var rows = new DataGridViewRow[SelectedRows.Count];
            SelectedRows.CopyTo(rows, 0);

            foreach (var row in rows.OrderBy(r => r.Index))
            {
                if (!row.IsNewRow)
                {
                    for (var i = 0; i < row.Cells.Count; i++)
                    {
                        sb.Append(row.Cells[i].Value + Environment.NewLine);
                    }
                }
            }

            Clipboard.SetText(sb.ToString());
        }

        private void Paste()
        {
            if (!Clipboard.ContainsText()) return;

            var split = Clipboard.GetText().Split(new[]{ Environment.NewLine }, StringSplitOptions.None);
            var rows = new List<DataGridViewRow>();

            for (var i = 0; i + Columns.Count <= split.Length; i += Columns.Count)
            {
                var values = new object[Columns.Count];
                Array.Copy(split, i, values, 0, Columns.Count);

                var row = new DataGridViewRow();
                row.CreateCells(this, values);
                rows.Add(row);
            }

            Rows.InsertRange(SelectedRows[0].Index, rows.ToArray());
        }

        private void Cut()
        {
            Copy();
            Delete();
        }

        private void InitializeComponent()
        {
            _components = new Container();
            _contextMenu = new ContextMenuStrip(_components);
            _copyMenuItem = new ToolStripMenuItem();
            _pasteMenuItem = new ToolStripMenuItem();
            _deleteMenuItem = new ToolStripMenuItem();
            _cutMenuItem = new ToolStripMenuItem();
            _contextMenu.SuspendLayout();
            ((ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // contextMenu
            // 
            _contextMenu.Items.AddRange(new ToolStripItem[] {
            _cutMenuItem,
            _copyMenuItem,
            _pasteMenuItem,
            _deleteMenuItem});
            _contextMenu.Name = "_contextMenu";
            _contextMenu.Size = new Size(165, 100);
            // 
            // copyMenuItem
            // 
            _copyMenuItem.Name = "_copyMenuItem";
            _copyMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            _copyMenuItem.Size = new Size(164, 24);
            _copyMenuItem.Text = "Copy";
            // 
            // pasteMenuItem
            // 
            _pasteMenuItem.Name = "_pasteMenuItem";
            _pasteMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            _pasteMenuItem.Size = new Size(164, 24);
            _pasteMenuItem.Text = "Paste";
            // 
            // deleteMenuItem
            // 
            _deleteMenuItem.Name = "_deleteMenuItem";
            _deleteMenuItem.ShortcutKeys = Keys.Delete;
            _deleteMenuItem.Size = new Size(164, 24);
            _deleteMenuItem.Text = "Delete";
            // 
            // cutMenuItem
            // 
            _cutMenuItem.Name = "_cutMenuItem";
            _cutMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            _cutMenuItem.Size = new Size(164, 24);
            _cutMenuItem.Text = "Cut";
            // 
            // AbstractInputLog
            // 
            RowTemplate.Height = 24;
            _contextMenu.ResumeLayout(false);
            ((ISupportInitialize)this).EndInit();
            ResumeLayout(false);
        }
    }
}

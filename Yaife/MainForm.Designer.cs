namespace Yaife
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HeaderMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.adjustFrameCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.subtitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToSubRipsrtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.InputMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.trimEmptyFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.goToFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.hashMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.crc32Menu = new System.Windows.Forms.ToolStripMenuItem();
			this.md5Menu = new System.Windows.Forms.ToolStripMenuItem();
			this.sha1Menu = new System.Windows.Forms.ToolStripMenuItem();
			this.MainTabControl = new System.Windows.Forms.TabControl();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.exportToSubStationAlphaassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MainMenu.SuspendLayout();
			this.contextMenu.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainMenu
			// 
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.HeaderMenu,
            this.InputMenu,
            this.toolsMenu});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(754, 28);
			this.MainMenu.TabIndex = 0;
			// 
			// FileMenu
			// 
			this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.FileMenu.Name = "FileMenu";
			this.FileMenu.Size = new System.Drawing.Size(44, 24);
			this.FileMenu.Text = "File";
			this.FileMenu.DropDownOpening += new System.EventHandler(this.FileMenu_DropDownOpening);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(218, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.saveAsToolStripMenuItem.Text = "Save As ...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(218, 6);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.closeToolStripMenuItem.Text = "Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(218, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// HeaderMenu
			// 
			this.HeaderMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adjustFrameCountToolStripMenuItem,
            this.subtitlesToolStripMenuItem});
			this.HeaderMenu.Name = "HeaderMenu";
			this.HeaderMenu.Size = new System.Drawing.Size(70, 24);
			this.HeaderMenu.Text = "Header";
			this.HeaderMenu.DropDownOpening += new System.EventHandler(this.headerMenu_DropDownOpening);
			// 
			// adjustFrameCountToolStripMenuItem
			// 
			this.adjustFrameCountToolStripMenuItem.Name = "adjustFrameCountToolStripMenuItem";
			this.adjustFrameCountToolStripMenuItem.Size = new System.Drawing.Size(208, 24);
			this.adjustFrameCountToolStripMenuItem.Text = "Adjust Frame Count";
			this.adjustFrameCountToolStripMenuItem.ToolTipText = "Sets the header frame count property to match the length of the input log.";
			this.adjustFrameCountToolStripMenuItem.Click += new System.EventHandler(this.adjustFrameCountToolStripMenuItem_Click);
			// 
			// subtitlesToolStripMenuItem
			// 
			this.subtitlesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToSubRipsrtToolStripMenuItem,
            this.exportToSubStationAlphaassToolStripMenuItem});
			this.subtitlesToolStripMenuItem.Name = "subtitlesToolStripMenuItem";
			this.subtitlesToolStripMenuItem.Size = new System.Drawing.Size(208, 24);
			this.subtitlesToolStripMenuItem.Text = "Subtitles";
			// 
			// exportToSubRipsrtToolStripMenuItem
			// 
			this.exportToSubRipsrtToolStripMenuItem.Name = "exportToSubRipsrtToolStripMenuItem";
			this.exportToSubRipsrtToolStripMenuItem.Size = new System.Drawing.Size(299, 24);
			this.exportToSubRipsrtToolStripMenuItem.Text = "Export to SubRip (.srt)";
			this.exportToSubRipsrtToolStripMenuItem.Click += new System.EventHandler(this.exportToSubRipsrtToolStripMenuItem_Click);
			// 
			// InputMenu
			// 
			this.InputMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trimEmptyFramesToolStripMenuItem,
            this.goToFrameToolStripMenuItem});
			this.InputMenu.Name = "InputMenu";
			this.InputMenu.Size = new System.Drawing.Size(55, 24);
			this.InputMenu.Text = "Input";
			this.InputMenu.DropDownOpening += new System.EventHandler(this.InputMenu_DropDownOpening);
			// 
			// trimEmptyFramesToolStripMenuItem
			// 
			this.trimEmptyFramesToolStripMenuItem.Name = "trimEmptyFramesToolStripMenuItem";
			this.trimEmptyFramesToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
			this.trimEmptyFramesToolStripMenuItem.Text = "Trim Empty Frames";
			this.trimEmptyFramesToolStripMenuItem.ToolTipText = "Removes empty frames at the end of the input log.";
			this.trimEmptyFramesToolStripMenuItem.Click += new System.EventHandler(this.trimEmptyFramesToolStripMenuItem_Click);
			// 
			// goToFrameToolStripMenuItem
			// 
			this.goToFrameToolStripMenuItem.Name = "goToFrameToolStripMenuItem";
			this.goToFrameToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
			this.goToFrameToolStripMenuItem.Text = "Go To Frame";
			this.goToFrameToolStripMenuItem.ToolTipText = "Jump to a given frame.";
			this.goToFrameToolStripMenuItem.Click += new System.EventHandler(this.goToFrameToolStripMenuItem_Click);
			// 
			// toolsMenu
			// 
			this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hashMenu});
			this.toolsMenu.Name = "toolsMenu";
			this.toolsMenu.Size = new System.Drawing.Size(57, 24);
			this.toolsMenu.Text = "Tools";
			// 
			// hashMenu
			// 
			this.hashMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crc32Menu,
            this.md5Menu,
            this.sha1Menu});
			this.hashMenu.Name = "hashMenu";
			this.hashMenu.Size = new System.Drawing.Size(176, 24);
			this.hashMenu.Text = "Calculate Hash";
			this.hashMenu.ToolTipText = "Calculate the hash for a selected file.";
			// 
			// crc32Menu
			// 
			this.crc32Menu.Name = "crc32Menu";
			this.crc32Menu.Size = new System.Drawing.Size(186, 24);
			this.crc32Menu.Text = "Calculate CRC32";
			this.crc32Menu.ToolTipText = "Calculate the 32-bit CRC for a selected file.";
			this.crc32Menu.Click += new System.EventHandler(this.crc32Menu_Click);
			// 
			// md5Menu
			// 
			this.md5Menu.Name = "md5Menu";
			this.md5Menu.Size = new System.Drawing.Size(186, 24);
			this.md5Menu.Text = "Calculate MD5";
			this.md5Menu.ToolTipText = "Calculate the MD5 hash for a selected file.";
			this.md5Menu.Click += new System.EventHandler(this.md5Menu_Click);
			// 
			// sha1Menu
			// 
			this.sha1Menu.Name = "sha1Menu";
			this.sha1Menu.Size = new System.Drawing.Size(186, 24);
			this.sha1Menu.Text = "Calculate SHA1";
			this.sha1Menu.ToolTipText = "Calculate the SHA1 hash for a selected file.";
			this.sha1Menu.Click += new System.EventHandler(this.sha1Menu_Click);
			// 
			// MainTabControl
			// 
			this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MainTabControl.ContextMenuStrip = this.contextMenu;
			this.MainTabControl.Location = new System.Drawing.Point(0, 28);
			this.MainTabControl.Name = "MainTabControl";
			this.MainTabControl.SelectedIndex = 0;
			this.MainTabControl.Size = new System.Drawing.Size(754, 468);
			this.MainTabControl.TabIndex = 1;
			this.MainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.closeMenuItem});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(218, 76);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Name = "saveMenuItem";
			this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveMenuItem.Size = new System.Drawing.Size(217, 24);
			this.saveMenuItem.Text = "Save";
			this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.Name = "saveAsMenuItem";
			this.saveAsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
			this.saveAsMenuItem.Size = new System.Drawing.Size(217, 24);
			this.saveAsMenuItem.Text = "Save As...";
			this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// closeMenuItem
			// 
			this.closeMenuItem.Name = "closeMenuItem";
			this.closeMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
			this.closeMenuItem.Size = new System.Drawing.Size(217, 24);
			this.closeMenuItem.Text = "Close";
			this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusProgressLabel,
            this.statusProgressBar});
			this.statusStrip.Location = new System.Drawing.Point(0, 494);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(754, 24);
			this.statusStrip.TabIndex = 2;
			this.statusStrip.Text = "statusStrip1";
			// 
			// statusProgressLabel
			// 
			this.statusProgressLabel.AutoSize = false;
			this.statusProgressLabel.Name = "statusProgressLabel";
			this.statusProgressLabel.Size = new System.Drawing.Size(200, 19);
			this.statusProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statusProgressBar
			// 
			this.statusProgressBar.AutoSize = false;
			this.statusProgressBar.Name = "statusProgressBar";
			this.statusProgressBar.Size = new System.Drawing.Size(200, 18);
			// 
			// exportToSubStationAlphaassToolStripMenuItem
			// 
			this.exportToSubStationAlphaassToolStripMenuItem.Name = "exportToSubStationAlphaassToolStripMenuItem";
			this.exportToSubStationAlphaassToolStripMenuItem.Size = new System.Drawing.Size(299, 24);
			this.exportToSubStationAlphaassToolStripMenuItem.Text = "Export to Sub Station Alpha (.ssa)";
			this.exportToSubStationAlphaassToolStripMenuItem.Click += new System.EventHandler(this.exportToSubStationAlphaassToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 518);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.MainTabControl);
			this.Controls.Add(this.MainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.MainMenu;
			this.Name = "MainForm";
			this.Text = "Movie Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.contextMenu.ResumeLayout(false);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
		private System.Windows.Forms.TabControl MainTabControl;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem InputMenu;
		private System.Windows.Forms.ToolStripMenuItem trimEmptyFramesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem goToFrameToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripProgressBar statusProgressBar;
		private System.Windows.Forms.ToolStripStatusLabel statusProgressLabel;
		private System.Windows.Forms.ToolStripMenuItem toolsMenu;
		private System.Windows.Forms.ToolStripMenuItem hashMenu;
		private System.Windows.Forms.ToolStripMenuItem crc32Menu;
		private System.Windows.Forms.ToolStripMenuItem md5Menu;
		private System.Windows.Forms.ToolStripMenuItem sha1Menu;
		private System.Windows.Forms.ToolStripMenuItem HeaderMenu;
		private System.Windows.Forms.ToolStripMenuItem adjustFrameCountToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem subtitlesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToSubRipsrtToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToSubStationAlphaassToolStripMenuItem;
    }
}


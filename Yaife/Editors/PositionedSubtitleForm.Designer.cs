using System.ComponentModel;
using System.Windows.Forms;

namespace Yaife.Editors
{
    partial class PositionedSubtitleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PositionedSubtitleForm));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.positionedSubtitleGrid = new System.Windows.Forms.DataGridView();
            this.Frame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionedSubtitlesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.positionedSubtitleFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.positionedSubtitleGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionedSubtitlesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionedSubtitleFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(520, 347);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 40);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(608, 347);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(91, 39);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // subtitleGrid
            // 
            this.positionedSubtitleGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionedSubtitleGrid.AutoGenerateColumns = false;
            this.positionedSubtitleGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.positionedSubtitleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.positionedSubtitleGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Frame,
            this.Length,
            this.X,
            this.Y,
            this.Color,
            this.textDataGridViewTextBoxColumn});
            this.positionedSubtitleGrid.DataSource = this.positionedSubtitlesBindingSource;
            this.positionedSubtitleGrid.EnableHeadersVisualStyles = false;
            this.positionedSubtitleGrid.Location = new System.Drawing.Point(8, 8);
            this.positionedSubtitleGrid.Name = "positionedSubtitleGrid";
            this.positionedSubtitleGrid.RowTemplate.Height = 24;
            this.positionedSubtitleGrid.Size = new System.Drawing.Size(688, 331);
            this.positionedSubtitleGrid.TabIndex = 2;
            // 
            // Frame
            // 
            this.Frame.DataPropertyName = "Frame";
            this.Frame.HeaderText = "Frame";
            this.Frame.Name = "Frame";
            this.Frame.ToolTipText = "The first frame the subtitle is displayed.";
            // 
            // Length
            // 
            this.Length.DataPropertyName = "Length";
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            this.Length.ToolTipText = "The amount of frames the subtitle remains on-screen.";
            // 
            // X
            // 
            this.X.DataPropertyName = "X";
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.ToolTipText = "The x-coordinate of the subtitle.";
            // 
            // Y
            // 
            this.Y.DataPropertyName = "Y";
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.ToolTipText = "The y-coordinate of the subtitle.";
            // 
            // Color
            // 
            this.Color.DataPropertyName = "Color";
            this.Color.HeaderText = "Color";
            this.Color.Name = "Color";
            this.Color.ToolTipText = "The color in which the subtitle will be displayed.";
            // 
            // textDataGridViewTextBoxColumn
            // 
            this.textDataGridViewTextBoxColumn.DataPropertyName = "Text";
            this.textDataGridViewTextBoxColumn.HeaderText = "Text";
            this.textDataGridViewTextBoxColumn.Name = "textDataGridViewTextBoxColumn";
            this.textDataGridViewTextBoxColumn.ToolTipText = "The text of the subtitle.";
            // 
            // subtitlesBindingSource
            // 
            this.positionedSubtitlesBindingSource.DataMember = "PositionedSubtitles";
            this.positionedSubtitlesBindingSource.DataSource = this.positionedSubtitleFormBindingSource;
            // 
            // subtitleFormBindingSource
            // 
            this.positionedSubtitleFormBindingSource.DataSource = typeof(PositionedSubtitleForm);
            // 
            // SubtitleForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(709, 395);
            this.Controls.Add(this.positionedSubtitleGrid);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SubtitleForm";
            this.Text = "Editing Subtitles";
            ((System.ComponentModel.ISupportInitialize)(this.positionedSubtitleGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionedSubtitlesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionedSubtitleFormBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button okButton;
        private Button cancelButton;
        private DataGridView positionedSubtitleGrid;
        private BindingSource positionedSubtitlesBindingSource;
        private BindingSource positionedSubtitleFormBindingSource;
        private DataGridViewTextBoxColumn Frame;
        private DataGridViewTextBoxColumn Length;
        private DataGridViewTextBoxColumn X;
        private DataGridViewTextBoxColumn Y;
        private DataGridViewTextBoxColumn Color;
        private DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
    }
}
namespace Yaife.Utilities
{
	partial class CopyMessageBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyMessageBox));
			this.copyTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.copyLabel = new System.Windows.Forms.Label();
			this.copyButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// copyTextBox
			// 
			this.copyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.copyTextBox.Location = new System.Drawing.Point(8, 38);
			this.copyTextBox.Name = "copyTextBox";
			this.copyTextBox.ReadOnly = true;
			this.copyTextBox.Size = new System.Drawing.Size(384, 22);
			this.copyTextBox.TabIndex = 0;
			this.copyTextBox.Text = "Copyable text!";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(308, 75);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(84, 30);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// copyLabel
			// 
			this.copyLabel.AutoSize = true;
			this.copyLabel.Location = new System.Drawing.Point(5, 9);
			this.copyLabel.Name = "copyLabel";
			this.copyLabel.Size = new System.Drawing.Size(70, 17);
			this.copyLabel.TabIndex = 2;
			this.copyLabel.Text = "Copy this:";
			// 
			// copyButton
			// 
			this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.copyButton.Location = new System.Drawing.Point(168, 75);
			this.copyButton.Name = "copyButton";
			this.copyButton.Size = new System.Drawing.Size(134, 30);
			this.copyButton.TabIndex = 3;
			this.copyButton.Text = "Copy to Clipboard";
			this.copyButton.UseVisualStyleBackColor = true;
			this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
			// 
			// CopyMessageBox
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(399, 108);
			this.Controls.Add(this.copyButton);
			this.Controls.Add(this.copyLabel);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.copyTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(417, 153);
			this.Name = "CopyMessageBox";
			this.Text = "CopyMessageBox";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox copyTextBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label copyLabel;
		private System.Windows.Forms.Button copyButton;
	}
}
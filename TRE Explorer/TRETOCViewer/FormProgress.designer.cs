namespace TRE_Explorer {
  partial class FormProgress {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgress));
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.label = new System.Windows.Forms.Label();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.progressBarFile = new System.Windows.Forms.ProgressBar();
      this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.flowLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // progressBar
      // 
      this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBar.Location = new System.Drawing.Point(0, 3);
      this.progressBar.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
      this.progressBar.MaximumSize = new System.Drawing.Size(290, 23);
      this.progressBar.MinimumSize = new System.Drawing.Size(290, 23);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(290, 23);
      this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.progressBar.TabIndex = 0;
      // 
      // label
      // 
      this.label.AutoEllipsis = true;
      this.label.Location = new System.Drawing.Point(12, 9);
      this.label.Name = "label";
      this.label.Size = new System.Drawing.Size(290, 13);
      this.label.TabIndex = 1;
      this.label.Text = "Progress";
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(215, 61);
      this.buttonCancel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // progressBarFile
      // 
      this.progressBarFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBarFile.Location = new System.Drawing.Point(0, 32);
      this.progressBarFile.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
      this.progressBarFile.MaximumSize = new System.Drawing.Size(290, 23);
      this.progressBarFile.MinimumSize = new System.Drawing.Size(290, 23);
      this.progressBarFile.Name = "progressBarFile";
      this.progressBarFile.Size = new System.Drawing.Size(290, 23);
      this.progressBarFile.TabIndex = 3;
      // 
      // flowLayoutPanel
      // 
      this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.flowLayoutPanel.AutoSize = true;
      this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel.Controls.Add(this.progressBar);
      this.flowLayoutPanel.Controls.Add(this.progressBarFile);
      this.flowLayoutPanel.Controls.Add(this.buttonCancel);
      this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
      this.flowLayoutPanel.Location = new System.Drawing.Point(12, 25);
      this.flowLayoutPanel.Name = "flowLayoutPanel";
      this.flowLayoutPanel.Size = new System.Drawing.Size(290, 87);
      this.flowLayoutPanel.TabIndex = 4;
      this.flowLayoutPanel.Resize += new System.EventHandler(this.flowLayoutPanel_Resize);
      // 
      // FormProgress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(314, 115);
      this.ControlBox = false;
      this.Controls.Add(this.flowLayoutPanel);
      this.Controls.Add(this.label);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormProgress";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.VisibleChanged += new System.EventHandler(this.FormProgress_VisibleChanged);
      this.flowLayoutPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    internal System.Windows.Forms.ProgressBar progressBar;
    internal System.Windows.Forms.Label label;
    private System.Windows.Forms.Button buttonCancel;
    internal System.Windows.Forms.ProgressBar progressBarFile;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
  }
}
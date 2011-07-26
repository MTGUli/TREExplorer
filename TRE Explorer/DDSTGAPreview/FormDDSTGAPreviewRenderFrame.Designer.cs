namespace TRE_Explorer {
  partial class FormDDSTGARenderFrame {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDDSTGARenderFrame));
      this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
      this.panelRenderSurfaceContainer = new System.Windows.Forms.Panel();
      this.toolStrip = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
      this.toolStripTextBoxZoomFactor = new System.Windows.Forms.ToolStripTextBox();
      this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonZoomFull = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonZoomFit = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
      this.toolStripContainer.ContentPanel.SuspendLayout();
      this.toolStripContainer.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer.SuspendLayout();
      this.toolStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStripContainer
      // 
      this.toolStripContainer.BottomToolStripPanelVisible = false;
      // 
      // toolStripContainer.ContentPanel
      // 
      this.toolStripContainer.ContentPanel.Controls.Add(this.panelRenderSurfaceContainer);
      this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(284, 239);
      this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer.LeftToolStripPanelVisible = false;
      this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer.Name = "toolStripContainer";
      this.toolStripContainer.RightToolStripPanelVisible = false;
      this.toolStripContainer.Size = new System.Drawing.Size(284, 264);
      this.toolStripContainer.TabIndex = 0;
      this.toolStripContainer.Text = "toolStripContainer1";
      // 
      // toolStripContainer.TopToolStripPanel
      // 
      this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
      // 
      // panelRenderSurfaceContainer
      // 
      this.panelRenderSurfaceContainer.AutoScroll = true;
      this.panelRenderSurfaceContainer.AutoScrollMargin = new System.Drawing.Size(1, 1);
      this.panelRenderSurfaceContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelRenderSurfaceContainer.Location = new System.Drawing.Point(0, 0);
      this.panelRenderSurfaceContainer.Name = "panelRenderSurfaceContainer";
      this.panelRenderSurfaceContainer.Size = new System.Drawing.Size(284, 239);
      this.panelRenderSurfaceContainer.TabIndex = 0;
      // 
      // toolStrip
      // 
      this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonZoomOut,
            this.toolStripTextBoxZoomFactor,
            this.toolStripButtonZoomIn,
            this.toolStripSeparator1,
            this.toolStripButtonZoomFull,
            this.toolStripButtonZoomFit,
            this.toolStripSeparator2,
            this.toolStripButtonSaveAs});
      this.toolStrip.Location = new System.Drawing.Point(3, 0);
      this.toolStrip.Name = "toolStrip";
      this.toolStrip.Size = new System.Drawing.Size(208, 25);
      this.toolStrip.TabIndex = 0;
      // 
      // toolStripButtonZoomOut
      // 
      this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonZoomOut.Image = global::TRE_Explorer.Properties.Resources.Zoom_Out;
      this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
      this.toolStripButtonZoomOut.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonZoomOut.Text = "Zoom Out";
      this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
      // 
      // toolStripTextBoxZoomFactor
      // 
      this.toolStripTextBoxZoomFactor.Enabled = false;
      this.toolStripTextBoxZoomFactor.Name = "toolStripTextBoxZoomFactor";
      this.toolStripTextBoxZoomFactor.ReadOnly = true;
      this.toolStripTextBoxZoomFactor.Size = new System.Drawing.Size(45, 25);
      this.toolStripTextBoxZoomFactor.Text = "100%";
      this.toolStripTextBoxZoomFactor.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // toolStripButtonZoomIn
      // 
      this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonZoomIn.Image = global::TRE_Explorer.Properties.Resources.Zoom_In;
      this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
      this.toolStripButtonZoomIn.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonZoomIn.Text = "Zoom In";
      this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButtonZoomFull
      // 
      this.toolStripButtonZoomFull.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonZoomFull.Image = global::TRE_Explorer.Properties.Resources.Zoom_100;
      this.toolStripButtonZoomFull.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonZoomFull.Name = "toolStripButtonZoomFull";
      this.toolStripButtonZoomFull.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonZoomFull.Text = "Zoom To Full";
      this.toolStripButtonZoomFull.Click += new System.EventHandler(this.toolStripButtonZoomFull_Click);
      // 
      // toolStripButtonZoomFit
      // 
      this.toolStripButtonZoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonZoomFit.Image = global::TRE_Explorer.Properties.Resources.Zoom_Fit;
      this.toolStripButtonZoomFit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonZoomFit.Name = "toolStripButtonZoomFit";
      this.toolStripButtonZoomFit.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonZoomFit.Text = "Zoom To Fit";
      this.toolStripButtonZoomFit.Click += new System.EventHandler(this.toolStripButtonZoomFit_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButtonSaveAs
      // 
      this.toolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonSaveAs.Image = global::TRE_Explorer.Properties.Resources.Save_As;
      this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
      this.toolStripButtonSaveAs.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonSaveAs.Text = "Save As…";
      this.toolStripButtonSaveAs.Click += new System.EventHandler(this.toolStripButtonSaveAs_Click);
      // 
      // FormDDSTGARenderFrame
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 264);
      this.Controls.Add(this.toolStripContainer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormDDSTGARenderFrame";
      this.Text = "RenderFrame";
      this.Shown += new System.EventHandler(this.FormRenderFrame_Shown);
      this.toolStripContainer.ContentPanel.ResumeLayout(false);
      this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
      this.toolStripContainer.TopToolStripPanel.PerformLayout();
      this.toolStripContainer.ResumeLayout(false);
      this.toolStripContainer.PerformLayout();
      this.toolStrip.ResumeLayout(false);
      this.toolStrip.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolStripContainer toolStripContainer;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
    private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
    private System.Windows.Forms.Panel panelRenderSurfaceContainer;
    private System.Windows.Forms.ToolStripButton toolStripButtonZoomFull;
    private System.Windows.Forms.ToolStripButton toolStripButtonZoomFit;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    public System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
    internal System.Windows.Forms.ToolStripTextBox toolStripTextBoxZoomFactor;

  }
}
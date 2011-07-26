namespace TRE_Explorer {
  partial class FormNotifyIcon {
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
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNotifyIcon));
        this.contextMenuStripNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
        this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItemNotifyIFFILFWSEditor = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItemNotifyPALEditor = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItemNotifySTFEditor = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItemNotifyTOCTREViewer = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.helpTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripTopMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.toolStripMenuItemNotifyExit = new System.Windows.Forms.ToolStripMenuItem();
        this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
        this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
        this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
        this.contextMenuStripNotify.SuspendLayout();
        this.SuspendLayout();
        // 
        // contextMenuStripNotify
        // 
        this.contextMenuStripNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator5,
            this.windowToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.helpTopicsToolStripMenuItem,
            this.toolStripTopMenuItemAbout,
            this.toolStripSeparator3,
            this.toolStripMenuItemNotifyExit});
        this.contextMenuStripNotify.Name = "contextMenuStripNotify";
        this.contextMenuStripNotify.Size = new System.Drawing.Size(138, 188);
        // 
        // openToolStripMenuItem
        // 
        this.openToolStripMenuItem.Image = global::TRE_Explorer.Properties.Resources.Open;
        this.openToolStripMenuItem.Name = "openToolStripMenuItem";
        this.openToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        this.openToolStripMenuItem.Text = "Open…";
        this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
        // 
        // toolStripSeparator5
        // 
        this.toolStripSeparator5.Name = "toolStripSeparator5";
        this.toolStripSeparator5.Size = new System.Drawing.Size(134, 6);
        // 
        // windowToolStripMenuItem
        // 
        this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemNotifyIFFILFWSEditor,
            this.toolStripMenuItemNotifyPALEditor,
            this.toolStripMenuItemNotifySTFEditor,
            this.toolStripMenuItemNotifyTOCTREViewer});
        this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
        this.windowToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        this.windowToolStripMenuItem.Text = "Window";
        // 
        // toolStripMenuItemNotifyIFFILFWSEditor
        // 
        this.toolStripMenuItemNotifyIFFILFWSEditor.Name = "toolStripMenuItemNotifyIFFILFWSEditor";
        this.toolStripMenuItemNotifyIFFILFWSEditor.Size = new System.Drawing.Size(165, 22);
        this.toolStripMenuItemNotifyIFFILFWSEditor.Text = "IFF/ILF/WS Editor";
        this.toolStripMenuItemNotifyIFFILFWSEditor.Click += new System.EventHandler(this.toolStripMenuItemNotifyIFFILFWSEditor_Click);
        // 
        // toolStripMenuItemNotifyPALEditor
        // 
        this.toolStripMenuItemNotifyPALEditor.Name = "toolStripMenuItemNotifyPALEditor";
        this.toolStripMenuItemNotifyPALEditor.Size = new System.Drawing.Size(165, 22);
        this.toolStripMenuItemNotifyPALEditor.Text = "PAL Editor";
        this.toolStripMenuItemNotifyPALEditor.Click += new System.EventHandler(this.toolStripMenuItemNotifyPALEditor_Click);
        // 
        // toolStripMenuItemNotifySTFEditor
        // 
        this.toolStripMenuItemNotifySTFEditor.Name = "toolStripMenuItemNotifySTFEditor";
        this.toolStripMenuItemNotifySTFEditor.Size = new System.Drawing.Size(165, 22);
        this.toolStripMenuItemNotifySTFEditor.Text = "STF Editor";
        this.toolStripMenuItemNotifySTFEditor.Click += new System.EventHandler(this.toolStripMenuItemNotifySTFEditor_Click);
        // 
        // toolStripMenuItemNotifyTOCTREViewer
        // 
        this.toolStripMenuItemNotifyTOCTREViewer.Name = "toolStripMenuItemNotifyTOCTREViewer";
        this.toolStripMenuItemNotifyTOCTREViewer.Size = new System.Drawing.Size(165, 22);
        this.toolStripMenuItemNotifyTOCTREViewer.Text = "TOC/TRE Viewer";
        this.toolStripMenuItemNotifyTOCTREViewer.Click += new System.EventHandler(this.toolStripMenuItemNotifyTOCTREViewer_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(134, 6);
        // 
        // settingsToolStripMenuItem
        // 
        this.settingsToolStripMenuItem.Image = global::TRE_Explorer.Properties.Resources.Settings;
        this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
        this.settingsToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        this.settingsToolStripMenuItem.Text = "Settings…";
        this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
        // 
        // toolStripSeparator2
        // 
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(134, 6);
        // 
        // helpTopicsToolStripMenuItem
        // 
        this.helpTopicsToolStripMenuItem.Image = global::TRE_Explorer.Properties.Resources.Help_Contents;
        this.helpTopicsToolStripMenuItem.Name = "helpTopicsToolStripMenuItem";
        this.helpTopicsToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        this.helpTopicsToolStripMenuItem.Text = "Help Topics";
        this.helpTopicsToolStripMenuItem.Click += new System.EventHandler(this.helpTopicsToolStripMenuItem_Click);
        // 
        // toolStripTopMenuItemAbout
        // 
        this.toolStripTopMenuItemAbout.Image = global::TRE_Explorer.Properties.Resources.Help;
        this.toolStripTopMenuItemAbout.Name = "toolStripTopMenuItemAbout";
        this.toolStripTopMenuItemAbout.Size = new System.Drawing.Size(137, 22);
        this.toolStripTopMenuItemAbout.Text = "About";
        this.toolStripTopMenuItemAbout.Click += new System.EventHandler(this.toolStripTopMenuItemAbout_Click);
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(134, 6);
        // 
        // toolStripMenuItemNotifyExit
        // 
        this.toolStripMenuItemNotifyExit.Image = global::TRE_Explorer.Properties.Resources.Quit;
        this.toolStripMenuItemNotifyExit.Name = "toolStripMenuItemNotifyExit";
        this.toolStripMenuItemNotifyExit.Size = new System.Drawing.Size(137, 22);
        this.toolStripMenuItemNotifyExit.Text = "Exit";
        this.toolStripMenuItemNotifyExit.Click += new System.EventHandler(this.toolStripMenuItemNotifyExit_Click);
        // 
        // notifyIcon
        // 
        this.notifyIcon.ContextMenuStrip = this.contextMenuStripNotify;
        this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
        this.notifyIcon.Text = "TRE Explorer";
        // 
        // folderBrowserDialog
        // 
        this.folderBrowserDialog.Description = "Select the folder to extract to:";
        // 
        // openFileDialog
        // 
        this.openFileDialog.Filter = "TRE Files|*.tre|TOC Files|*.toc";
        this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
        // 
        // saveFileDialog
        // 
        this.saveFileDialog.RestoreDirectory = true;
        this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
        // 
        // FormNotifyIcon
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(36, 36);
        this.ControlBox = false;
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Location = new System.Drawing.Point(-9999, -9999);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "FormNotifyIcon";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Text = "TRE Explorer";
        this.Load += new System.EventHandler(this.FormNotifyIcon_Load);
        this.Shown += new System.EventHandler(this.FormNotifyIcon_Shown);
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNotifyIcon_FormClosing);
        this.contextMenuStripNotify.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip contextMenuStripNotify;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNotifyExit;
    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNotifyIFFILFWSEditor;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNotifyPALEditor;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNotifySTFEditor;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNotifyTOCTREViewer;
    private System.Windows.Forms.ToolStripMenuItem toolStripTopMenuItemAbout;
    private System.Windows.Forms.ToolStripMenuItem helpTopicsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    internal System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    internal System.Windows.Forms.OpenFileDialog openFileDialog;
    internal System.Windows.Forms.SaveFileDialog saveFileDialog;
  }
}
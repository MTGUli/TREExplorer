namespace TRE_Explorer {
  partial class FormSettings {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
      this.textBoxOpenFolder = new System.Windows.Forms.TextBox();
      this.labelOpenFolder = new System.Windows.Forms.Label();
      this.buttonOpenFolderBrowse = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBoxDefaultView = new System.Windows.Forms.ComboBox();
      this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonSaveFolderBrowse = new System.Windows.Forms.Button();
      this.labelSaveFolder = new System.Windows.Forms.Label();
      this.textBoxSaveFolder = new System.Windows.Forms.TextBox();
      this.checkBoxDetailsPanePathDisplay = new System.Windows.Forms.CheckBox();
      this.labelDetailsPanePathDisplay = new System.Windows.Forms.Label();
      this.labelPromptForUpdates = new System.Windows.Forms.Label();
      this.checkBoxPromptForUpdates = new System.Windows.Forms.CheckBox();
      this.checkBoxDisplayNotifyIcon = new System.Windows.Forms.CheckBox();
      this.labelDisplayNotifyIcon = new System.Windows.Forms.Label();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageGeneral = new System.Windows.Forms.TabPage();
      this.tabPageTOCTRE = new System.Windows.Forms.TabPage();
      this.tabPageAssociations = new System.Windows.Forms.TabPage();
      this.checkedListBoxFileTypes = new System.Windows.Forms.CheckedListBox();
      this.buttonCheckNone = new System.Windows.Forms.Button();
      this.buttonCheckAll = new System.Windows.Forms.Button();
      this.tabControl.SuspendLayout();
      this.tabPageGeneral.SuspendLayout();
      this.tabPageTOCTRE.SuspendLayout();
      this.tabPageAssociations.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBoxOpenFolder
      // 
      this.textBoxOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxOpenFolder.Location = new System.Drawing.Point(79, 7);
      this.textBoxOpenFolder.Name = "textBoxOpenFolder";
      this.textBoxOpenFolder.ReadOnly = true;
      this.textBoxOpenFolder.Size = new System.Drawing.Size(168, 20);
      this.textBoxOpenFolder.TabIndex = 0;
      // 
      // labelOpenFolder
      // 
      this.labelOpenFolder.AutoSize = true;
      this.labelOpenFolder.Location = new System.Drawing.Point(6, 11);
      this.labelOpenFolder.Name = "labelOpenFolder";
      this.labelOpenFolder.Size = new System.Drawing.Size(68, 13);
      this.labelOpenFolder.TabIndex = 1;
      this.labelOpenFolder.Text = "Open Folder:";
      // 
      // buttonOpenFolderBrowse
      // 
      this.buttonOpenFolderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOpenFolderBrowse.Location = new System.Drawing.Point(253, 6);
      this.buttonOpenFolderBrowse.Name = "buttonOpenFolderBrowse";
      this.buttonOpenFolderBrowse.Size = new System.Drawing.Size(23, 23);
      this.buttonOpenFolderBrowse.TabIndex = 2;
      this.buttonOpenFolderBrowse.Text = "…";
      this.buttonOpenFolderBrowse.UseVisualStyleBackColor = true;
      this.buttonOpenFolderBrowse.Click += new System.EventHandler(this.buttonStartingFolderBrowse_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 10);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(70, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Default View:";
      // 
      // comboBoxDefaultView
      // 
      this.comboBoxDefaultView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxDefaultView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxDefaultView.FormattingEnabled = true;
      this.comboBoxDefaultView.Items.AddRange(new object[] {
            "Large Icons",
            "Small Icons",
            "List",
            "Details"});
      this.comboBoxDefaultView.Location = new System.Drawing.Point(82, 6);
      this.comboBoxDefaultView.Name = "comboBoxDefaultView";
      this.comboBoxDefaultView.Size = new System.Drawing.Size(194, 21);
      this.comboBoxDefaultView.TabIndex = 4;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(227, 193);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(146, 193);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 6;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonSaveFolderBrowse
      // 
      this.buttonSaveFolderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSaveFolderBrowse.Location = new System.Drawing.Point(253, 35);
      this.buttonSaveFolderBrowse.Name = "buttonSaveFolderBrowse";
      this.buttonSaveFolderBrowse.Size = new System.Drawing.Size(23, 23);
      this.buttonSaveFolderBrowse.TabIndex = 9;
      this.buttonSaveFolderBrowse.Text = "…";
      this.buttonSaveFolderBrowse.UseVisualStyleBackColor = true;
      this.buttonSaveFolderBrowse.Click += new System.EventHandler(this.buttonSaveFolderBrowse_Click);
      // 
      // labelSaveFolder
      // 
      this.labelSaveFolder.AutoSize = true;
      this.labelSaveFolder.Location = new System.Drawing.Point(6, 40);
      this.labelSaveFolder.Name = "labelSaveFolder";
      this.labelSaveFolder.Size = new System.Drawing.Size(67, 13);
      this.labelSaveFolder.TabIndex = 8;
      this.labelSaveFolder.Text = "Save Folder:";
      // 
      // textBoxSaveFolder
      // 
      this.textBoxSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSaveFolder.Location = new System.Drawing.Point(79, 36);
      this.textBoxSaveFolder.Name = "textBoxSaveFolder";
      this.textBoxSaveFolder.ReadOnly = true;
      this.textBoxSaveFolder.Size = new System.Drawing.Size(168, 20);
      this.textBoxSaveFolder.TabIndex = 7;
      // 
      // checkBoxDetailsPanePathDisplay
      // 
      this.checkBoxDetailsPanePathDisplay.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.checkBoxDetailsPanePathDisplay.Location = new System.Drawing.Point(82, 33);
      this.checkBoxDetailsPanePathDisplay.Name = "checkBoxDetailsPanePathDisplay";
      this.checkBoxDetailsPanePathDisplay.Size = new System.Drawing.Size(194, 26);
      this.checkBoxDetailsPanePathDisplay.TabIndex = 10;
      this.checkBoxDetailsPanePathDisplay.UseVisualStyleBackColor = true;
      // 
      // labelDetailsPanePathDisplay
      // 
      this.labelDetailsPanePathDisplay.AutoSize = true;
      this.labelDetailsPanePathDisplay.Location = new System.Drawing.Point(6, 33);
      this.labelDetailsPanePathDisplay.Name = "labelDetailsPanePathDisplay";
      this.labelDetailsPanePathDisplay.Size = new System.Drawing.Size(69, 26);
      this.labelDetailsPanePathDisplay.TabIndex = 11;
      this.labelDetailsPanePathDisplay.Text = "Details Pane\r\nPath Display:";
      // 
      // labelPromptForUpdates
      // 
      this.labelPromptForUpdates.Location = new System.Drawing.Point(6, 63);
      this.labelPromptForUpdates.Name = "labelPromptForUpdates";
      this.labelPromptForUpdates.Size = new System.Drawing.Size(69, 26);
      this.labelPromptForUpdates.TabIndex = 12;
      this.labelPromptForUpdates.Text = "Prompt For Updates:";
      // 
      // checkBoxPromptForUpdates
      // 
      this.checkBoxPromptForUpdates.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.checkBoxPromptForUpdates.Location = new System.Drawing.Point(79, 63);
      this.checkBoxPromptForUpdates.Name = "checkBoxPromptForUpdates";
      this.checkBoxPromptForUpdates.Size = new System.Drawing.Size(197, 26);
      this.checkBoxPromptForUpdates.TabIndex = 13;
      this.checkBoxPromptForUpdates.UseVisualStyleBackColor = true;
      // 
      // checkBoxDisplayNotifyIcon
      // 
      this.checkBoxDisplayNotifyIcon.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.checkBoxDisplayNotifyIcon.Location = new System.Drawing.Point(79, 95);
      this.checkBoxDisplayNotifyIcon.Name = "checkBoxDisplayNotifyIcon";
      this.checkBoxDisplayNotifyIcon.Size = new System.Drawing.Size(197, 26);
      this.checkBoxDisplayNotifyIcon.TabIndex = 16;
      this.checkBoxDisplayNotifyIcon.UseVisualStyleBackColor = true;
      // 
      // labelDisplayNotifyIcon
      // 
      this.labelDisplayNotifyIcon.Location = new System.Drawing.Point(6, 95);
      this.labelDisplayNotifyIcon.Name = "labelDisplayNotifyIcon";
      this.labelDisplayNotifyIcon.Size = new System.Drawing.Size(69, 26);
      this.labelDisplayNotifyIcon.TabIndex = 15;
      this.labelDisplayNotifyIcon.Text = "Display NotifyIcon:";
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add(this.tabPageGeneral);
      this.tabControl.Controls.Add(this.tabPageTOCTRE);
      this.tabControl.Controls.Add(this.tabPageAssociations);
      this.tabControl.Location = new System.Drawing.Point(12, 12);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(290, 175);
      this.tabControl.TabIndex = 17;
      this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
      // 
      // tabPageGeneral
      // 
      this.tabPageGeneral.Controls.Add(this.labelOpenFolder);
      this.tabPageGeneral.Controls.Add(this.checkBoxDisplayNotifyIcon);
      this.tabPageGeneral.Controls.Add(this.textBoxOpenFolder);
      this.tabPageGeneral.Controls.Add(this.labelDisplayNotifyIcon);
      this.tabPageGeneral.Controls.Add(this.buttonOpenFolderBrowse);
      this.tabPageGeneral.Controls.Add(this.textBoxSaveFolder);
      this.tabPageGeneral.Controls.Add(this.checkBoxPromptForUpdates);
      this.tabPageGeneral.Controls.Add(this.labelSaveFolder);
      this.tabPageGeneral.Controls.Add(this.labelPromptForUpdates);
      this.tabPageGeneral.Controls.Add(this.buttonSaveFolderBrowse);
      this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
      this.tabPageGeneral.Name = "tabPageGeneral";
      this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageGeneral.Size = new System.Drawing.Size(282, 149);
      this.tabPageGeneral.TabIndex = 0;
      this.tabPageGeneral.Text = "General";
      this.tabPageGeneral.UseVisualStyleBackColor = true;
      // 
      // tabPageTOCTRE
      // 
      this.tabPageTOCTRE.Controls.Add(this.comboBoxDefaultView);
      this.tabPageTOCTRE.Controls.Add(this.labelDetailsPanePathDisplay);
      this.tabPageTOCTRE.Controls.Add(this.label2);
      this.tabPageTOCTRE.Controls.Add(this.checkBoxDetailsPanePathDisplay);
      this.tabPageTOCTRE.Location = new System.Drawing.Point(4, 22);
      this.tabPageTOCTRE.Name = "tabPageTOCTRE";
      this.tabPageTOCTRE.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageTOCTRE.Size = new System.Drawing.Size(282, 149);
      this.tabPageTOCTRE.TabIndex = 1;
      this.tabPageTOCTRE.Text = "TOC/TRE";
      this.tabPageTOCTRE.UseVisualStyleBackColor = true;
      // 
      // tabPageAssociations
      // 
      this.tabPageAssociations.Controls.Add(this.buttonCheckNone);
      this.tabPageAssociations.Controls.Add(this.buttonCheckAll);
      this.tabPageAssociations.Controls.Add(this.checkedListBoxFileTypes);
      this.tabPageAssociations.Location = new System.Drawing.Point(4, 22);
      this.tabPageAssociations.Name = "tabPageAssociations";
      this.tabPageAssociations.Size = new System.Drawing.Size(282, 149);
      this.tabPageAssociations.TabIndex = 2;
      this.tabPageAssociations.Text = "Associations";
      this.tabPageAssociations.UseVisualStyleBackColor = true;
      // 
      // checkedListBoxFileTypes
      // 
      this.checkedListBoxFileTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.checkedListBoxFileTypes.FormattingEnabled = true;
      this.checkedListBoxFileTypes.IntegralHeight = false;
      this.checkedListBoxFileTypes.Items.AddRange(new object[] {
            "ANS",
            "APT",
            "ASH",
            "CDF",
            "CEF",
            "CMP",
            "EFT",
            "FLR",
            "IFF",
            "ILF",
            "LAT",
            "LAY",
            "LMG",
            "LOD",
            "LSB",
            "LTN",
            "MGN",
            "MKR",
            "MSH",
            "PAL",
            "PLN",
            "POB",
            "PRT",
            "PSH",
            "PST",
            "SAT",
            "SFP",
            "SHT",
            "SKT",
            "SND",
            "SPR",
            "STF",
            "SWH",
            "TRE",
            "TRN",
            "TRT",
            "TOC",
            "WS"});
      this.checkedListBoxFileTypes.Location = new System.Drawing.Point(3, 3);
      this.checkedListBoxFileTypes.Name = "checkedListBoxFileTypes";
      this.checkedListBoxFileTypes.Size = new System.Drawing.Size(276, 114);
      this.checkedListBoxFileTypes.TabIndex = 1;
      // 
      // buttonCheckNone
      // 
      this.buttonCheckNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonCheckNone.Location = new System.Drawing.Point(204, 123);
      this.buttonCheckNone.Name = "buttonCheckNone";
      this.buttonCheckNone.Size = new System.Drawing.Size(75, 23);
      this.buttonCheckNone.TabIndex = 4;
      this.buttonCheckNone.Text = "Select None";
      this.buttonCheckNone.UseVisualStyleBackColor = true;
      this.buttonCheckNone.Click += new System.EventHandler(this.buttonCheckNone_Click);
      // 
      // buttonCheckAll
      // 
      this.buttonCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonCheckAll.Location = new System.Drawing.Point(3, 123);
      this.buttonCheckAll.Name = "buttonCheckAll";
      this.buttonCheckAll.Size = new System.Drawing.Size(75, 23);
      this.buttonCheckAll.TabIndex = 3;
      this.buttonCheckAll.Text = "Select All";
      this.buttonCheckAll.UseVisualStyleBackColor = true;
      this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
      // 
      // FormSettings
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(314, 228);
      this.ControlBox = false;
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.tabControl);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormSettings";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.Shown += new System.EventHandler(this.FormSettings_Shown);
      this.tabControl.ResumeLayout(false);
      this.tabPageGeneral.ResumeLayout(false);
      this.tabPageGeneral.PerformLayout();
      this.tabPageTOCTRE.ResumeLayout(false);
      this.tabPageTOCTRE.PerformLayout();
      this.tabPageAssociations.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxOpenFolder;
    private System.Windows.Forms.Label labelOpenFolder;
    private System.Windows.Forms.Button buttonOpenFolderBrowse;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboBoxDefaultView;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonSaveFolderBrowse;
    private System.Windows.Forms.Label labelSaveFolder;
    private System.Windows.Forms.TextBox textBoxSaveFolder;
    private System.Windows.Forms.CheckBox checkBoxDetailsPanePathDisplay;
    private System.Windows.Forms.Label labelDetailsPanePathDisplay;
    private System.Windows.Forms.Label labelPromptForUpdates;
    private System.Windows.Forms.CheckBox checkBoxPromptForUpdates;
    private System.Windows.Forms.CheckBox checkBoxDisplayNotifyIcon;
    private System.Windows.Forms.Label labelDisplayNotifyIcon;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageGeneral;
    private System.Windows.Forms.TabPage tabPageTOCTRE;
    private System.Windows.Forms.TabPage tabPageAssociations;
    private System.Windows.Forms.CheckedListBox checkedListBoxFileTypes;
    private System.Windows.Forms.Button buttonCheckNone;
    private System.Windows.Forms.Button buttonCheckAll;
  }
}
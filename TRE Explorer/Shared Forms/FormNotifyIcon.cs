using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SWGLib;

namespace TRE_Explorer {
  public partial class FormNotifyIcon : Form {
    #region Global Variables
    internal FormTOCTREViewer m_FormTOCTREViewer;
    internal FormIFFILFWSEditor m_FormIFFILFWSEditor;
    internal FormPALEditor m_FormPALEditor;
    internal FormSTFEditor m_FormSTFEditor;

    internal List<String> m_TemporaryFiles = new List<String>();
    internal List<String> m_TemporaryPaths = new List<String>();
    private IntPtr _ClipboardViewerNext;

    internal Boolean m_Exitting = false;
    #endregion

    #region Clipboard Functions
    private void RegisterClipboardViewer() {
      _ClipboardViewerNext = SetClipboardViewer(this.Handle);
    }

    private void UnregisterClipboardViewer() {
      ChangeClipboardChain(this.Handle, _ClipboardViewerNext);
    }

    protected override void WndProc(ref Message m) {
      if (m.Msg == 0x0308) {
        if (this.m_FormIFFILFWSEditor != null) {
          this.m_FormIFFILFWSEditor.toolStripButtonIffPaste.Enabled = Clipboard.ContainsText();
        }
        SendMessage(_ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
      } else {
        base.WndProc(ref m);
      }
    }
    #endregion

    #region Form Functions
    public FormNotifyIcon() {
      InitializeComponent();

      this.notifyIcon.Visible = TRE_Explorer.Properties.Settings.Default.DisplayNotifyIcon;
      Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
    }

    private void Application_ApplicationExit(object sender, EventArgs e) {
      this.m_Exitting = true;
    }

    private void FormNotifyIcon_FormClosing(object sender, FormClosingEventArgs e) {
      UnregisterClipboardViewer();

      this.RemoveTemporaryFiles();
    }

    private void FormNotifyIcon_Load(object sender, EventArgs e) {
      this.m_FormTOCTREViewer = new FormTOCTREViewer(this);
      this.m_FormTOCTREViewer.VisibleChanged += new EventHandler(m_FormTOCTREViewer_VisibleChanged);
      this.m_FormIFFILFWSEditor = new FormIFFILFWSEditor(this);
      this.m_FormIFFILFWSEditor.VisibleChanged += new EventHandler(m_FormIFFILFWSEditor_VisibleChanged);
      this.m_FormPALEditor = new FormPALEditor(this);
      this.m_FormPALEditor.VisibleChanged += new EventHandler(m_FormPALEditor_VisibleChanged);
      this.m_FormSTFEditor = new FormSTFEditor(this);
      this.m_FormSTFEditor.VisibleChanged += new EventHandler(m_FormSTFEditor_VisibleChanged);

      this.RegisterClipboardViewer();

      if (TRE_Explorer.Properties.Settings.Default.UpgradeSettings) {
        TRE_Explorer.Properties.Settings.Default.Upgrade();
        TRE_Explorer.Properties.Settings.Default.UpgradeSettings = false;
        TRE_Explorer.Properties.Settings.Default.Save();
      }

      if (TRE_Explorer.Properties.Settings.Default.OpenFolder == "null") {
        TRE_Explorer.Properties.Settings.Default.OpenFolder = Utilities.GetGameFolder(false);
        TRE_Explorer.Properties.Settings.Default.Save();
      }

      if (TRE_Explorer.Properties.Settings.Default.SaveFolder == "null") {
        TRE_Explorer.Properties.Settings.Default.SaveFolder = Utilities.GetGameFolder(false);
        TRE_Explorer.Properties.Settings.Default.Save();
      }

      this.saveFileDialog.InitialDirectory = TRE_Explorer.Properties.Settings.Default.SaveFolder;
      this.openFileDialog.InitialDirectory = TRE_Explorer.Properties.Settings.Default.OpenFolder;
      this.folderBrowserDialog.SelectedPath = TRE_Explorer.Properties.Settings.Default.SaveFolder;

      String[] arguments = Environment.GetCommandLineArgs();
      if ((arguments.Length > 1) && (File.Exists(arguments[1]))) {
        if ((arguments[1].ToLower().EndsWith(".tre")) || (arguments[1].ToLower().EndsWith(".toc"))) {
          this.TOCTRELoadFile(arguments[1]);
        } else if ((arguments[1].ToLower().EndsWith(".ans")) || (arguments[1].ToLower().EndsWith(".apt")) || (arguments[1].ToLower().EndsWith(".ash")) || (arguments[1].ToLower().EndsWith(".cdf")) || (arguments[1].ToLower().EndsWith(".cef")) || (arguments[1].ToLower().EndsWith(".cmp")) || (arguments[1].ToLower().EndsWith(".eft")) || (arguments[1].ToLower().EndsWith(".flr")) || (arguments[1].ToLower().EndsWith(".iff")) || (arguments[1].ToLower().EndsWith(".ilf")) || (arguments[1].ToLower().EndsWith(".lat")) || (arguments[1].ToLower().EndsWith(".lay")) || (arguments[1].ToLower().EndsWith(".lmg")) || (arguments[1].ToLower().EndsWith(".lod")) || (arguments[1].ToLower().EndsWith(".lsb")) || (arguments[1].ToLower().EndsWith(".ltn")) || (arguments[1].ToLower().EndsWith(".mgn")) || (arguments[1].ToLower().EndsWith(".mkr")) || (arguments[1].ToLower().EndsWith(".msh")) || (arguments[1].ToLower().EndsWith(".pln")) || (arguments[1].ToLower().EndsWith(".pob")) || (arguments[1].ToLower().EndsWith(".prt")) || (arguments[1].ToLower().EndsWith(".psh")) || (arguments[1].ToLower().EndsWith(".pst")) || (arguments[1].ToLower().EndsWith(".sat")) || (arguments[1].ToLower().EndsWith(".sfp")) || (arguments[1].ToLower().EndsWith(".sht")) || (arguments[1].ToLower().EndsWith(".skt")) || (arguments[1].ToLower().EndsWith(".snd")) || (arguments[1].ToLower().EndsWith(".spr")) || (arguments[1].ToLower().EndsWith(".swh")) || (arguments[1].ToLower().EndsWith(".trn")) || (arguments[1].ToLower().EndsWith(".trt")) || (arguments[1].ToLower().EndsWith(".ws"))) {
          this.IFFILFWSLoadFile(arguments[1]);
        } else if (arguments[1].ToLower().EndsWith(".pal")) {
          this.PALLoadFile(arguments[1]);
        } else if (arguments[1].ToLower().EndsWith(".stf")) {
          this.STFLoadFile(arguments[1]);
        } else {
          this.IFFILFWSLoadFile(arguments[1]);
        }
      }

      if ((!this.m_FormIFFILFWSEditor.Visible) && (!this.m_FormPALEditor.Visible) && (!this.m_FormSTFEditor.Visible)) {
        this.m_FormTOCTREViewer.Show();
      }
    }

    private void FormNotifyIcon_Shown(object sender, EventArgs e) {
      this.Hide();

      if (this.m_FormIFFILFWSEditor.Visible) {
        this.m_FormIFFILFWSEditor.Focus();
      }

      if (this.m_FormPALEditor.Visible) {
        this.m_FormPALEditor.Focus();
      }

      if (this.m_FormSTFEditor.Visible) {
        this.m_FormSTFEditor.Focus();
      }

      if (this.m_FormTOCTREViewer.Visible) {
        this.m_FormTOCTREViewer.Focus();
      }
    }
    #endregion

    #region ContextMenuStrip Functions
    private void toolStripMenuItemNotifyExit_Click(object sender, EventArgs e) {
      if (!this.m_Exitting) {
        this.m_Exitting = true;
        Application.Exit();
      }
    }

    private void toolStripMenuItemNotifyTOCTREViewer_Click(object sender, EventArgs e) {
      this.ToggleTOCTREViewer();
    }

    private void toolStripMenuItemNotifyIFFILFWSEditor_Click(object sender, EventArgs e) {
      this.ToggleIFFILFWSEditor();
    }

    private void toolStripMenuItemNotifyPALEditor_Click(object sender, EventArgs e) {
      this.TogglePALEditor();
    }

    private void toolStripMenuItemNotifySTFEditor_Click(object sender, EventArgs e) {
      this.ToggleSTFEditor();
    }
    #endregion

    #region Visibility Toggle Functions
    internal void ToggleTOCTREViewer() {
      this.m_FormTOCTREViewer.Visible = !this.m_FormTOCTREViewer.Visible;
    }

    internal void ToggleIFFILFWSEditor() {
      if (this.m_FormIFFILFWSEditor != null) {
        if (this.m_FormIFFILFWSEditor.Visible) {
          if (this.m_FormIFFILFWSEditor.PromptForChanges()) {
            this.m_FormIFFILFWSEditor.Visible = false;
          }
        } else {
          this.m_FormIFFILFWSEditor.Visible = true;
        }
      }
    }

    internal void TogglePALEditor() {
      if (this.m_FormPALEditor != null) {
        if (this.m_FormPALEditor.Visible) {
          if (this.m_FormPALEditor.PromptForChanges()) {
            this.m_FormPALEditor.Visible = false;
          }
        } else {
          this.m_FormPALEditor.Visible = true;
        }
      }
    }

    internal void ToggleSTFEditor() {
      if (this.m_FormSTFEditor != null) {
        if (this.m_FormSTFEditor.Visible) {
          if (this.m_FormSTFEditor.PromptForChanges()) {
            this.m_FormSTFEditor.Visible = false;
          }
        } else {
          this.m_FormSTFEditor.Visible = true;
        }
      }
    }
    #endregion

    #region IFFILFWSEditor Functions
    private void m_FormIFFILFWSEditor_VisibleChanged(object sender, EventArgs e) {
      this.toolStripMenuItemNotifyIFFILFWSEditor.Checked = this.m_FormIFFILFWSEditor.Visible;

      this.m_FormIFFILFWSEditor.iFFILFWSEditorToolStripMenuItem.Checked = this.m_FormIFFILFWSEditor.Visible;
      this.m_FormPALEditor.iFFILFWSEditorToolStripMenuItem.Checked = this.m_FormIFFILFWSEditor.Visible;
      this.m_FormSTFEditor.iFFILFWSEditorToolStripMenuItem.Checked = this.m_FormIFFILFWSEditor.Visible;
      this.m_FormTOCTREViewer.iFFILFWSEditorToolStripMenuItem.Checked = this.m_FormIFFILFWSEditor.Visible;

      if (!this.m_FormTOCTREViewer.Visible && !this.m_FormIFFILFWSEditor.Visible && !this.m_FormPALEditor.Visible && !this.m_FormSTFEditor.Visible && !this.m_Exitting) {
        this.m_Exitting = true;
        try {
          Application.Exit();
        } catch {
          this.notifyIcon.Visible = false;
          this.RemoveTemporaryFiles();
          Process.GetCurrentProcess().Kill();
        }
      }
    }
    #endregion

    #region STFEditor Functions
    private void m_FormSTFEditor_VisibleChanged(object sender, EventArgs e) {
      this.toolStripMenuItemNotifySTFEditor.Checked = this.m_FormSTFEditor.Visible;

      this.m_FormIFFILFWSEditor.sTFEditorToolStripMenuItem.Checked = this.m_FormSTFEditor.Visible;
      this.m_FormPALEditor.sTFEditorToolStripMenuItem.Checked = this.m_FormSTFEditor.Visible;
      this.m_FormSTFEditor.sTFEditorToolStripMenuItem.Checked = this.m_FormSTFEditor.Visible;
      this.m_FormTOCTREViewer.sTFEditorToolStripMenuItem.Checked = this.m_FormSTFEditor.Visible;

      if (!this.m_FormTOCTREViewer.Visible && !this.m_FormIFFILFWSEditor.Visible && !this.m_FormPALEditor.Visible && !this.m_FormSTFEditor.Visible && !this.m_Exitting) {
        this.m_Exitting = true;
        try {
          Application.Exit();
        } catch {
          this.notifyIcon.Visible = false;
          this.RemoveTemporaryFiles();
          Process.GetCurrentProcess().Kill();
        }
      }
    }
    #endregion

    #region PALEditor Functions
    void m_FormPALEditor_VisibleChanged(object sender, EventArgs e) {
      this.toolStripMenuItemNotifyPALEditor.Checked = this.m_FormPALEditor.Visible;

      this.m_FormIFFILFWSEditor.pALEditorToolStripMenuItem.Checked = this.m_FormPALEditor.Visible;
      this.m_FormPALEditor.pALEditorToolStripMenuItem.Checked = this.m_FormPALEditor.Visible;
      this.m_FormSTFEditor.pALEditorToolStripMenuItem.Checked = this.m_FormPALEditor.Visible;
      this.m_FormTOCTREViewer.pALEditorToolStripMenuItem.Checked = this.m_FormPALEditor.Visible;

      if (!this.m_FormTOCTREViewer.Visible && !this.m_FormIFFILFWSEditor.Visible && !this.m_FormPALEditor.Visible && !this.m_FormSTFEditor.Visible && !this.m_Exitting) {
        this.m_Exitting = true;
        try {
          Application.Exit();
        } catch {
          this.notifyIcon.Visible = false;
          this.RemoveTemporaryFiles();
          Process.GetCurrentProcess().Kill();
        }
      }
    }
    #endregion

    #region TREExplorer Functions
    private void m_FormTOCTREViewer_VisibleChanged(object sender, EventArgs e) {
      this.toolStripMenuItemNotifyTOCTREViewer.Checked = this.m_FormTOCTREViewer.Visible;

      this.m_FormIFFILFWSEditor.tOCTREViewerToolStripMenuItem.Checked = this.m_FormTOCTREViewer.Visible;
      this.m_FormPALEditor.tOCTREViewerToolStripMenuItem.Checked = this.m_FormTOCTREViewer.Visible;
      this.m_FormSTFEditor.tOCTREViewerToolStripMenuItem.Checked = this.m_FormTOCTREViewer.Visible;
      this.m_FormTOCTREViewer.tOCTREViewerToolStripMenuItem.Checked = this.m_FormTOCTREViewer.Visible;

      if (!this.m_FormTOCTREViewer.Visible && !this.m_FormIFFILFWSEditor.Visible && !this.m_FormPALEditor.Visible && !this.m_FormSTFEditor.Visible && !this.m_Exitting) {
        this.m_Exitting = true;
        try {
          Application.Exit();
        } catch {
          this.notifyIcon.Visible = false;
          this.RemoveTemporaryFiles();
          Process.GetCurrentProcess().Kill();
        }
      }
    }
    #endregion

    #region ContextMenu Functions
    private void openToolStripMenuItem_Click(object sender, EventArgs e) {
      this.Open();
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.Settings(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, 0, 0);
    }

    private void toolStripTopMenuItemAbout_Click(object sender, EventArgs e) {
      this.About(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, 0, 0);
    }

    private void helpTopicsToolStripMenuItem_Click(object sender, EventArgs e) {
      String filename = Path.Combine(Path.GetTempPath(), "TRE Explorer.chm");
      if (!File.Exists(filename)) {
        FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
        binaryWriter.Write(TRE_Explorer.Properties.Resources.TRE_Explorer_chm);
        binaryWriter.Close();
        fileStream.Close();

        this.m_TemporaryFiles.Add(filename);
      }

      Help.ShowHelp(this, filename, HelpNavigator.TableOfContents);
    }

    private void saveFileDialog_FileOk(object sender, CancelEventArgs e) {
      this.saveFileDialog.InitialDirectory = this.saveFileDialog.FileName.Substring(0, this.saveFileDialog.FileName.LastIndexOf("\\"));
    }

    private void openFileDialog_FileOk(object sender, CancelEventArgs e) {
      this.openFileDialog.InitialDirectory = this.openFileDialog.FileName.Substring(0, this.openFileDialog.FileName.LastIndexOf("\\"));
      if ((!this.openFileDialog.FileName.ToLower().EndsWith(".tre")) && (!this.openFileDialog.FileName.ToLower().EndsWith(".toc"))) {
        this.saveFileDialog.InitialDirectory = this.openFileDialog.InitialDirectory;
      }
    }
    #endregion

    #region Settings Functions
    internal void Settings(Form form) {
      this.Settings(form.Width, form.Height, form.Top, form.Left);
    }

    internal void Settings(Int32 width, Int32 height, Int32 top, Int32 left) {
      FormSettings formSettings = new FormSettings();
      formSettings.Top = top + ((height - formSettings.Height) / 2);
      formSettings.Left = left + ((width - formSettings.Width) / 2);
      if (formSettings.ShowDialog() == DialogResult.OK) {
        this.m_FormTOCTREViewer.listView.View = TRE_Explorer.Properties.Settings.Default.DefaultView;
        this.m_FormTOCTREViewer.CheckView();
        this.folderBrowserDialog.SelectedPath = TRE_Explorer.Properties.Settings.Default.SaveFolder;
        this.openFileDialog.InitialDirectory = TRE_Explorer.Properties.Settings.Default.OpenFolder;
        this.saveFileDialog.InitialDirectory = TRE_Explorer.Properties.Settings.Default.SaveFolder;
        this.notifyIcon.Visible = TRE_Explorer.Properties.Settings.Default.DisplayNotifyIcon;
      }
      formSettings.Close();
    }
    #endregion

    #region About Functions
    internal void About(Form form) {
      this.About(form.Width, form.Height, form.Top, form.Left);
    }

    internal void About(Int32 width, Int32 height, Int32 top, Int32 left) {
      FormAbout formAbout = new FormAbout();
      formAbout.Top = top + ((height - formAbout.Height) / 2);
      formAbout.Left = left + ((width - formAbout.Width) / 2);
      formAbout.ShowDialog();
      formAbout.Close();
    }
    #endregion

    #region Open Functions
    internal void Open() {
      this.openFileDialog.FileName = String.Empty;
      this.openFileDialog.Filter = "All Supported Files|*.ANS;*.APT;*.ASH;*.CDF;*.CEF;*.CMP;*.EFT;*.FLR;*.IFF;*.ILF;*.LAT;*.LAY;*.LMG;*.LOD;*.LSB;*.LTN;*.MGN;*.MKR;*.MSH;*.PAL;*.PLN;*.POB;*.PRT;*.PSH;*.PST;*.SAT;*.SFP;*.SHT;*.SKT;*.SND;*.SPR;*.STF;*.SWH;*.TOC;*.TRE;*.TRN;*.TRT;*.WS|TRE Files|*.TRE|TOC Files|*.TOC|IFF Files|*.ANS;*.APT;*.ASH;*.CDF;*.CEF;*.CMP;*.EFT;*.FLR;*.IFF;*.LAT;*.LAY;*.LMG;*.LOD;*.LSB;*.LTN;*.MGN;*.MKR;*.MSH;*.PLN;*.POB;*.PRT;*.PSH;*.PST;*.SAT;*.SFP;*.SHT;*.SKT;*.SND;*.SPR;*.SWH;*.TRN;*.TRT|ILF Files|*.ILF|WS Files|*.WS|PAL Files|*.PAL|STF Files|*.STF";
      this.openFileDialog.FilterIndex = 1;

      if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
        try {
          if ((this.openFileDialog.FileName.ToLower().EndsWith(".tre")) || (this.openFileDialog.FileName.ToLower().EndsWith(".toc"))) {
            this.TOCTRELoadFile(this.openFileDialog.FileName);
          } else if ((this.openFileDialog.FileName.ToLower().EndsWith(".ans")) || (this.openFileDialog.FileName.ToLower().EndsWith(".apt")) || (this.openFileDialog.FileName.ToLower().EndsWith(".ash")) || (this.openFileDialog.FileName.ToLower().EndsWith(".cdf")) || (this.openFileDialog.FileName.ToLower().EndsWith(".cef")) || (this.openFileDialog.FileName.ToLower().EndsWith(".cmp")) || (this.openFileDialog.FileName.ToLower().EndsWith(".eft")) || (this.openFileDialog.FileName.ToLower().EndsWith(".flr")) || (this.openFileDialog.FileName.ToLower().EndsWith(".iff")) || (this.openFileDialog.FileName.ToLower().EndsWith(".lat")) || (this.openFileDialog.FileName.ToLower().EndsWith(".lay")) || (this.openFileDialog.FileName.ToLower().EndsWith(".lmg")) || (this.openFileDialog.FileName.ToLower().EndsWith(".lod")) || (this.openFileDialog.FileName.ToLower().EndsWith(".lsb")) || (this.openFileDialog.FileName.ToLower().EndsWith(".ltn")) || (this.openFileDialog.FileName.ToLower().EndsWith(".mgn")) || (this.openFileDialog.FileName.ToLower().EndsWith(".mkr")) || (this.openFileDialog.FileName.ToLower().EndsWith(".msh")) || (this.openFileDialog.FileName.ToLower().EndsWith(".pln")) || (this.openFileDialog.FileName.ToLower().EndsWith(".pob")) || (this.openFileDialog.FileName.ToLower().EndsWith(".prt")) || (this.openFileDialog.FileName.ToLower().EndsWith(".psh")) || (this.openFileDialog.FileName.ToLower().EndsWith(".pst")) || (this.openFileDialog.FileName.ToLower().EndsWith(".sat")) || (this.openFileDialog.FileName.ToLower().EndsWith(".sfp")) || (this.openFileDialog.FileName.ToLower().EndsWith(".sht")) || (this.openFileDialog.FileName.ToLower().EndsWith(".skt")) || (this.openFileDialog.FileName.ToLower().EndsWith(".snd")) || (this.openFileDialog.FileName.ToLower().EndsWith(".spr")) || (this.openFileDialog.FileName.ToLower().EndsWith(".swh")) || (this.openFileDialog.FileName.ToLower().EndsWith(".trn")) || (this.openFileDialog.FileName.ToLower().EndsWith(".trt"))) {
            this.IFFILFWSLoadFile(this.openFileDialog.FileName);
          } else if (this.openFileDialog.FileName.ToLower().EndsWith(".ilf")) {
            this.IFFILFWSLoadFile(this.openFileDialog.FileName);
          } else if (this.openFileDialog.FileName.ToLower().EndsWith(".pal")) {
            this.PALLoadFile(this.openFileDialog.FileName);
          } else if (this.openFileDialog.FileName.ToLower().EndsWith(".stf")) {
            this.STFLoadFile(this.openFileDialog.FileName);
          } else if (this.openFileDialog.FileName.ToLower().EndsWith(".ws")) {
            this.IFFILFWSLoadFile(this.openFileDialog.FileName);
          } else {
            this.IFFILFWSLoadFile(this.openFileDialog.FileName);
          }
        } catch {
          MessageBox.Show("Could not open " + this.openFileDialog.FileName, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    internal void OpenIFFILFWS() {
      this.openFileDialog.FileName = String.Empty;
      this.openFileDialog.Filter = "All Supported Files|*.ANS;*.APT;*.ASH;*.CDF;*.CEF;*.CMP;*.EFT;*.FLR;*.IFF;*.ILF;*.LAT;*.LAY;*.LMG;*.LOD;*.LSB;*.LTN;*.MGN;*.MKR;*.MSH;*.PLN;*.POB;*.PRT;*.PSH;*.PST;*.SAT;*.SFP;*.SHT;*.SKT;*.SND;*.SPR;*.SWH;*.TRN;*.TRT;*.WS|IFF Files|*.ANS;*.APT;*.ASH;*.CDF;*.CEF;*.CMP;*.EFT;*.FLR;*.IFF;*.LAT;*.LAY;*.LMG;*.LOD;*.LSB;*.LTN;*.MGN;*.MKR;*.MSH;*.PLN;*.POB;*.PRT;*.PSH;*.PST;*.SAT;*.SFP;*.SHT;*.SKT;*.SND;*.SPR;*.SWH;*.TRN;*.TRT|ILF Files|*.ILF|WS Files|*.WS";
      this.openFileDialog.FilterIndex = 1;

      if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
        try {
          this.IFFILFWSLoadFile(this.openFileDialog.FileName);
        } catch (Exception exception) {
          MessageBox.Show(exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    internal void OpenPAL() {
      this.openFileDialog.FileName = String.Empty;
      this.openFileDialog.Filter = "PAL Files|*.PAL";
      this.openFileDialog.FilterIndex = 1;

      if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
        try {
          this.PALLoadFile(this.openFileDialog.FileName);
        } catch (Exception exception) {
          MessageBox.Show(exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    internal void OpenSTF() {
      this.openFileDialog.FileName = String.Empty;
      this.openFileDialog.Filter = "STF File|*.STF";
      this.openFileDialog.FilterIndex = 1;

      if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
        try {
          this.STFLoadFile(this.openFileDialog.FileName);
        } catch (Exception exception) {
          MessageBox.Show(exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    internal void OpenTOCTRE() {
      this.openFileDialog.FileName = String.Empty;
      this.openFileDialog.Filter = "TRE/TOC Files|*.TRE;*.TOC|TRE Files|*.TRE|TOC Files|*.TOC";
      this.openFileDialog.FilterIndex = 1;

      if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
        try {
          this.TOCTRELoadFile(this.openFileDialog.FileName);
        } catch {
          MessageBox.Show("Could not open " + this.openFileDialog.FileName, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    internal void IFFILFWSLoadFile(String filename) {
      try {
        this.m_FormIFFILFWSEditor.IFFLoadFile(filename);
        this.m_FormIFFILFWSEditor.Visible = true;
      } catch {
      }
    }

    internal void PALLoadFile(String filename) {
      try {
        this.m_FormPALEditor.PALLoadFile(filename);
        this.m_FormPALEditor.Visible = true;
      } catch {
      }
    }

    internal void STFLoadFile(String filename) {
      try {
        this.m_FormSTFEditor.STFLoadFile(filename);
        this.m_FormSTFEditor.Visible = true;
      } catch {
      }
    }

    internal void TOCTRELoadFile(String filename) {
      try {
        this.m_FormTOCTREViewer.TOCTRELoadFile(filename);
        this.m_FormTOCTREViewer.Visible = true;
      } catch {
      }
    }
    #endregion

    #region Temporary File Functions
    private void RemoveTemporaryFiles() {
      foreach (String filename in this.m_TemporaryFiles.ToArray()) {
        try {
          File.Delete(filename);
        } catch {
        }
      }
      foreach (String path in this.m_TemporaryPaths.ToArray()) {
        try {
          String pathToDelete = path;
          if (path.StartsWith(Path.GetTempPath())) {
            pathToDelete = path.Substring(Path.GetTempPath().Length);
            pathToDelete = Path.Combine(Path.GetTempPath(), pathToDelete.Substring(0, pathToDelete.IndexOf('\\')));
          }
          Directory.Delete(pathToDelete, true);
        } catch {
        }
      }
    }
    #endregion

    #region User32.dll Imports
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(IntPtr hwnd, Int32 wMsg, IntPtr wParam, IntPtr lParam);
    #endregion
  }
}
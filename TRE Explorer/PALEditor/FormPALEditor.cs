using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SWGLib;

namespace TRE_Explorer {
  public partial class FormPALEditor : Form {
    public FormPALEditor(FormNotifyIcon formNotifyIcon) {
      InitializeComponent();
      this.m_FormNotifyIcon = formNotifyIcon;
      this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }

    protected override void OnVisibleChanged(EventArgs e) {
      base.OnVisibleChanged(e);

      if (!this.Visible) {
        this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        this.toolStripStatusLabel.Text = "0 colors";
        this.m_PALFile = null;
        this.toolStripButtonPalSave.Enabled = false;
        this.toolStripButtonPalSaveAs.Enabled = false;
        this.flowLayoutPanelPalEditor.SuspendLayout();
        if (this.flowLayoutPanelPalEditor.Controls.Count > 0) {
          foreach (Button button in this.flowLayoutPanelPalEditor.Controls) {
            button.Dispose();
          }
          this.flowLayoutPanelPalEditor.Controls.Clear();
        }
        this.flowLayoutPanelPalEditor.ResumeLayout(true);
      }
    }

    private FormNotifyIcon m_FormNotifyIcon;
    internal PALFile m_PALFile;
    private Button m_LastClickedPAL;
    private Boolean m_HasChanges = false;

    public delegate void HasChangesChangedEvent(object sender, EventArgs e);
    public event HasChangesChangedEvent HasChangesChanged;

    public Boolean HasChanges {
      get {
        return this.m_HasChanges;
      }
      set {
        if (this.m_HasChanges != value) {
          this.m_HasChanges = value;
          if (this.HasChangesChanged != null) {
            HasChangesChanged(this, new EventArgs());
          }
        }
      }
    }

    private void FormPALEditor_HasChangesChanged(object sender, EventArgs e) {
      this.toolStripButtonPalSave.Enabled = this.HasChanges;
      if (this.HasChanges) {
        if (!this.Text.EndsWith("*")) {
          this.Text += "*";
        }
      } else {
        if (this.Text.EndsWith("*")) {
          this.Text = this.Text.Substring(0, this.Text.Length - 1);
        }
      }
    }

    internal Boolean PromptForChanges() {
      if ((this.HasChanges) && (this.m_PALFile != null)) {
        switch (MessageBox.Show("Do you want to save the changes to " + this.m_PALFile.Filename + "?", "TRE Explorer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
          case DialogResult.Yes:
            this.m_PALFile.Save(this.m_PALFile.Filename);
            this.HasChanges = false;
            return true;

          case DialogResult.No:
            this.HasChanges = false;
            return true;

          case DialogResult.Cancel:
            return false;
        }
      }

      return true;
    }

    private void FormPALEditor_FormClosing(object sender, FormClosingEventArgs e) {
      if ((e.CloseReason != CloseReason.ApplicationExitCall) && (e.CloseReason != CloseReason.WindowsShutDown)) {
        e.Cancel = true;
        if (PromptForChanges()) {
          this.Visible = false;
        }
      } else if (!this.PromptForChanges()) {
        e.Cancel = true;
      }
    }

    private void toolStripButtonPalAddColor_Click(object sender, EventArgs e) {
      if (this.m_PALFile != null) {
        this.colorDialog.Color = Color.White;
        this.colorDialog.FullOpen = true;
        if (this.colorDialog.ShowDialog() == DialogResult.OK) {
          this.m_PALFile.AddColor(this.colorDialog.Color);
          Button button = new Button();
          button.BackColor = this.colorDialog.Color;
          button.FlatAppearance.BorderColor = Color.Black;
          button.FlatAppearance.BorderSize = 4;
          button.FlatAppearance.MouseDownBackColor = this.colorDialog.Color;
          button.FlatAppearance.MouseOverBackColor = this.colorDialog.Color;
          button.FlatStyle = FlatStyle.Flat;
          button.Size = new Size(46, 46);
          button.Text = this.m_PALFile.HexadecimalIndex(this.colorDialog.Color);
          button.Click += new EventHandler(flowLayoutPanelPalEditorButton_Click);
          this.flowLayoutPanelPalEditor.Controls.Add(button);

          if (this.m_PALFile.Colors.Length >= 256) {
            this.toolStripButtonPalAddColor.Enabled = false;
          } else {
            this.toolStripButtonPalAddColor.Enabled = true;
          }

          this.toolStripButtonPalChangeAllColors.Enabled = (this.m_PALFile.Colors.Length > 0);
        }
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripButtonPalChangeColor_Click(object sender, EventArgs e) {
      if (this.m_LastClickedPAL != null) {
        Int32 index = this.flowLayoutPanelPalEditor.Controls.IndexOf(m_LastClickedPAL);
        this.colorDialog.FullOpen = true;
        if ((index != -1) && (this.colorDialog.ShowDialog() == DialogResult.OK)) {
          Button button = (Button)this.flowLayoutPanelPalEditor.Controls[index];
          Color color = this.colorDialog.Color;
          button.BackColor = color;
          button.FlatAppearance.MouseDownBackColor = color;
          button.FlatAppearance.MouseOverBackColor = color;
          this.m_PALFile.ReplaceColor(index, color);
          button.Text = this.m_PALFile.HexadecimalIndex(color);
          this.HasChanges = true;
        }
      }
    }

    private void toolStripButtonPalChangeAllColors_Click(object sender, EventArgs e) {
      if (this.m_PALFile != null) {
        this.colorDialog.FullOpen = true;
        if (this.colorDialog.ShowDialog() == DialogResult.OK) {
          Color color = this.colorDialog.Color;
          this.flowLayoutPanelPalEditor.SuspendLayout();
          foreach (Control control in this.flowLayoutPanelPalEditor.Controls) {
            ((Button)control).BackColor = color;
            ((Button)control).FlatAppearance.MouseDownBackColor = color;
            ((Button)control).FlatAppearance.MouseOverBackColor = color;
          }
          for (Int32 counter = 0; counter < this.m_PALFile.Colors.Length; counter++) {
            this.m_PALFile.ReplaceColor(counter, color);
          }
          foreach (Button button in this.flowLayoutPanelPalEditor.Controls) {
            button.Text = this.m_PALFile.FindColor(button.BackColor).ToString();
          }
          this.flowLayoutPanelPalEditor.ResumeLayout(true);
          this.HasChanges = true;
        }
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripButtonPalExit_Click(object sender, EventArgs e) {
      if (PromptForChanges()) {
        this.Visible = false;
      }
    }

    private void toolStripButtonPalRemoveColor_Click(object sender, EventArgs e) {
      if (this.m_LastClickedPAL != null) {
        Int32 index = this.flowLayoutPanelPalEditor.Controls.IndexOf(m_LastClickedPAL);
        if (index != -1) {
          this.m_PALFile.RemoveColor(index);
          this.flowLayoutPanelPalEditor.Controls.RemoveAt(index);
          this.m_LastClickedPAL = null;
          this.toolStripButtonPalChangeColor.Enabled = false;
          this.toolStripButtonPalRemoveColor.Enabled = false;

          if (this.m_PALFile.Colors.Length >= 256) {
            this.toolStripButtonPalAddColor.Enabled = false;
          } else {
            this.toolStripButtonPalAddColor.Enabled = true;
          }

          this.toolStripButtonPalChangeAllColors.Enabled = (this.m_PALFile.Colors.Length > 0);

          this.HasChanges = true;
        }
      }
    }

    private void toolStripButtonPalSaveAs_Click(object sender, EventArgs e) {
      if (this.m_PALFile != null) {
        this.m_FormNotifyIcon.saveFileDialog.FileName = this.m_PALFile.Filename;
        this.m_FormNotifyIcon.saveFileDialog.Filter = "PAL Files|*.PAL";
        this.m_FormNotifyIcon.saveFileDialog.FilterIndex = 1;

        if (this.m_FormNotifyIcon.saveFileDialog.ShowDialog() == DialogResult.OK) {
          FileStream fileStream = new FileStream(this.m_FormNotifyIcon.saveFileDialog.FileName, FileMode.Create);
          BinaryWriter binaryWriter = new BinaryWriter(fileStream);
          binaryWriter.Write(this.m_PALFile.Bytes);
          binaryWriter.Close();
          fileStream.Close();

          this.m_PALFile.Filename = this.m_FormNotifyIcon.saveFileDialog.FileName;
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_PALFile.Filename;

          this.HasChanges = false;
        }
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripButtonPalChangeAllColors_EnabledChanged(object sender, EventArgs e) {
      this.changeAllColorsToolStripMenuItem.Enabled = this.toolStripButtonPalChangeAllColors.Enabled;
    }

    private void flowLayoutPanelPalEditorButton_Click(object sender, EventArgs e) {
      this.m_LastClickedPAL = (Button)sender;
      this.colorDialog.Color = ((Button)sender).BackColor;
      this.toolStripButtonPalChangeColor.Enabled = true;
      this.toolStripButtonPalRemoveColor.Enabled = true;
    }

    private void flowLayoutPanelPalEditor_ControlAdded(object sender, ControlEventArgs e) {
      if (this.flowLayoutPanelPalEditor.Visible) {
        this.toolStripStatusLabel.Text = this.flowLayoutPanelPalEditor.Controls.Count + " color" + ((this.flowLayoutPanelPalEditor.Controls.Count == 1) ? "" : "s");
        this.flowLayoutPanelPalEditor.SuspendLayout();
        foreach (Button button in this.flowLayoutPanelPalEditor.Controls) {
          button.Text = this.m_PALFile.HexadecimalIndex(button.BackColor);
        }
        this.flowLayoutPanelPalEditor.ResumeLayout(true);
      }
    }

    private void flowLayoutPanelPalEditor_ControlRemoved(object sender, ControlEventArgs e) {
      if (this.flowLayoutPanelPalEditor.Visible) {
        this.toolStripStatusLabel.Text = this.flowLayoutPanelPalEditor.Controls.Count + " color" + ((this.flowLayoutPanelPalEditor.Controls.Count == 1) ? "" : "s");
        this.flowLayoutPanelPalEditor.SuspendLayout();
        foreach (Button button in this.flowLayoutPanelPalEditor.Controls) {
          button.Text = this.m_PALFile.HexadecimalIndex(button.BackColor);
        }
        this.flowLayoutPanelPalEditor.ResumeLayout(true);
      }
    }

    internal void PALDisplay() {
      try {
        this.HasChangesChanged -= new HasChangesChangedEvent(FormPALEditor_HasChangesChanged);
      } catch {
      }

      if (this.m_PALFile != null) {
        this.flowLayoutPanelPalEditor.SuspendLayout();
        try {
          foreach (Button button in this.flowLayoutPanelPalEditor.Controls) {
            try {
              button.Dispose();
            } catch {
            }
          }
        } catch {
        }
        this.flowLayoutPanelPalEditor.Controls.Clear();

        foreach (Color color in this.m_PALFile.Colors) {
          Button button = new Button();
          button.BackColor = color;
          button.FlatAppearance.BorderColor = Color.Black;
          button.FlatAppearance.BorderSize = 4;
          button.FlatAppearance.MouseDownBackColor = color;
          button.FlatAppearance.MouseOverBackColor = color;
          button.FlatStyle = FlatStyle.Flat;
          button.Size = new Size(46, 46);
          button.Text = this.m_PALFile.HexadecimalIndex(color);
          button.Click += new EventHandler(flowLayoutPanelPalEditorButton_Click);
          this.flowLayoutPanelPalEditor.Controls.Add(button);
        }
        this.flowLayoutPanelPalEditor.ResumeLayout(true);
        if (this.m_PALFile.Colors.Length < 256) {
          this.toolStripButtonPalAddColor.Enabled = true;
        }

        this.toolStripStatusLabel.Text = this.m_PALFile.Colors.Length + " color" + ((this.m_PALFile.Colors.Length == 1) ? String.Empty : "s");

        this.toolStripButtonPalSaveAs.Enabled = true;
        if ((this.m_PALFile.Filename != null) && (this.m_PALFile.Filename != String.Empty)) {
          if (!this.m_PALFile.Filename.Contains("\\")) {
            this.m_PALFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_PALFile.Filename);
          } else if (this.m_PALFile.Filename.StartsWith(Path.GetTempPath())) {
            this.m_PALFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_PALFile.Filename.Substring(Path.GetTempPath().Length));
          }
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_PALFile.Filename;
        } else {
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        this.toolStripButtonPalChangeAllColors.Enabled = (this.m_PALFile.Colors.Length > 0);

        this.HasChangesChanged += new HasChangesChangedEvent(FormPALEditor_HasChangesChanged);

        this.Focus();
      }
    }

    internal void PALLoadFile(String filename) {
      this.m_PALFile = new PALFile(filename);
      PALDisplay();
    }

    private void toolStripButtonPalSave_Click(object sender, EventArgs e) {
      FileStream fileStream = new FileStream(this.m_PALFile.Filename, FileMode.Create);
      BinaryWriter binaryWriter = new BinaryWriter(fileStream);
      binaryWriter.Write(this.m_PALFile.Bytes);
      binaryWriter.Close();
      fileStream.Close();

      this.HasChanges = false;
    }

    private void toolStripTopMenuItemAbout_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.About(this);
    }

    private void addColorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonPalAddColor.PerformClick();
    }

    private void removeColorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonPalRemoveColor.PerformClick();
    }

    private void changeColorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonPalChangeColor.PerformClick();
    }

    private void changeAllColorsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonPalChangeAllColors.PerformClick();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonPalSave.PerformClick();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonPalSaveAs.PerformClick();
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonPalExit.PerformClick();
    }

    private void toolStripButtonPalChangeColor_EnabledChanged(object sender, EventArgs e) {
      this.changeColorToolStripMenuItem.Enabled = this.toolStripButtonPalChangeColor.Enabled;
    }

    private void toolStripButtonPalAddColor_EnabledChanged(object sender, EventArgs e) {
      this.addColorToolStripMenuItem.Enabled = this.toolStripButtonPalAddColor.Enabled;
    }

    private void toolStripButtonPalRemoveColor_EnabledChanged(object sender, EventArgs e) {
      this.removeColorToolStripMenuItem.Enabled = this.toolStripButtonPalRemoveColor.Enabled;
    }

    private void toolStripButtonPalSave_EnabledChanged(object sender, EventArgs e) {
      this.saveToolStripMenuItem.Enabled = this.toolStripButtonPalSave.Enabled;
    }

    private void toolStripButtonPalSaveAs_EnabledChanged(object sender, EventArgs e) {
      this.saveAsToolStripMenuItem.Enabled = this.toolStripButtonPalSaveAs.Enabled;
    }

    private void helpTopicsToolStripMenuItem_Click(object sender, EventArgs e) {
      String filename = Path.Combine(Path.GetTempPath(), "TRE Explorer.chm");
      if (!File.Exists(filename)) {
        FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
        binaryWriter.Write(TRE_Explorer.Properties.Resources.TRE_Explorer_chm);
        binaryWriter.Close();
        fileStream.Close();

        this.m_FormNotifyIcon.m_TemporaryFiles.Add(filename);
      }

      Help.ShowHelp(this, filename, HelpNavigator.TableOfContents);
    }

    private void iFFILFWSEditorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.ToggleIFFILFWSEditor();
    }

    private void pALEditorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.TogglePALEditor();
    }

    private void sTFEditorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.ToggleSTFEditor();
    }

    private void tOCTREViewerToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.ToggleTOCTREViewer();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
      if (!this.m_FormNotifyIcon.m_Exitting) {
        this.m_FormNotifyIcon.m_Exitting = true;
        Application.Exit();
      }
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.Settings(this);
    }

    private void toolStripButtonSettings_Click(object sender, EventArgs e) {
      this.settingsToolStripMenuItem.PerformClick();
    }

    private void toolStripButtonAbout_Click(object sender, EventArgs e) {
      this.toolStripTopMenuItemAbout.PerformClick();
    }

    private void toolStripTopMenuItemOpenTRETOC_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenTOCTRE();
    }

    private void toolStripTopMenuItemOpenIFF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenIFFILFWS();
    }

    private void toolStripTopMenuItemOpenPAL_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenPAL();
    }

    private void toolStripTopMenuItemOpenSTF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenSTF();
    }

    private void toolStripTopMenuItemOpen_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.Open();
    }

    private void toolStripSplitButtonOpen_ButtonClick(object sender, EventArgs e) {
      this.m_FormNotifyIcon.Open();
    }

    private void toolStripMenuItemOpenTRETOC_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenTOCTRE();
    }

    private void toolStripMenuItemOpenIFF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenIFFILFWS();
    }

    private void toolStripMenuItemOpenPAL_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenPAL();
    }

    private void toolStripMenuItemOpenSTF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenSTF();
    }
  }
}

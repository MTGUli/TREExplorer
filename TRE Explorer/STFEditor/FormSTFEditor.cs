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
  public partial class FormSTFEditor : Form {
    public FormSTFEditor(FormNotifyIcon formNotifyIcon) {
      InitializeComponent();
      this.m_FormNotifyIcon = formNotifyIcon;
      this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }

    protected override void OnVisibleChanged(EventArgs e) {
      base.OnVisibleChanged(e);

      if (!this.Visible) {
        this.m_STFFile = null;
        this.dataGridViewStf.DataSource = null;
        this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      }
    }

    internal STFFile m_STFFile;
    private FormNotifyIcon m_FormNotifyIcon;
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

    private void FormSTFEditor_HasChangesChanged(object sender, EventArgs e) {
      this.toolStripButtonStfSave.Enabled = this.HasChanges;
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
      if ((this.HasChanges) && (this.m_STFFile != null)) {
        switch (MessageBox.Show("Do you want to save the changes to " + this.m_STFFile.Filename + "?", "TRE Explorer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
          case DialogResult.Yes:
            this.m_STFFile.Save(this.m_STFFile.Filename);
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

    internal void STFLoadFile(String filename) {
      this.m_STFFile = new STFFile(filename);
      this.STFDisplay();
    }

    internal void STFDisplay() {
      try {
        this.HasChangesChanged -= new HasChangesChangedEvent(FormSTFEditor_HasChangesChanged);
      } catch {
      }

      if (this.m_STFFile != null) {
        this.dataGridViewStf.DataSource = this.m_STFFile.DataTable;
        this.dataGridViewStf.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

        this.toolStripButtonStfSaveAs.Enabled = true;

        if ((this.m_STFFile.Filename != null) && (this.m_STFFile.Filename != String.Empty)) {
          if (!this.m_STFFile.Filename.Contains("\\")) {
            this.m_STFFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_STFFile.Filename);
          } else if (this.m_STFFile.Filename.StartsWith(Path.GetTempPath())) {
            this.m_STFFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_STFFile.Filename.Substring(Path.GetTempPath().Length));
          }
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_STFFile.Filename;
        } else {
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        this.HasChanges = false;
        this.HasChangesChanged += new HasChangesChangedEvent(FormSTFEditor_HasChangesChanged);

        this.Focus();
      }
    }

    private void toolStripButtonStfExit_Click(object sender, EventArgs e) {
      if (this.PromptForChanges()) {
        this.Visible = false;
      }
    }

    private void toolStripButtonStfSaveAs_Click(object sender, EventArgs e) {
      if (this.m_STFFile != null) {
        this.m_FormNotifyIcon.saveFileDialog.Filter = "STF Files|*.STF";
        this.m_FormNotifyIcon.saveFileDialog.FilterIndex = 1;
        this.m_FormNotifyIcon.saveFileDialog.FileName = this.m_STFFile.Filename.Substring(this.m_STFFile.Filename.LastIndexOf("\\") + 1);
        if (this.m_FormNotifyIcon.saveFileDialog.ShowDialog() == DialogResult.OK) {
          this.m_STFFile.Save(this.m_FormNotifyIcon.saveFileDialog.FileName);
          this.m_STFFile.Filename = this.m_FormNotifyIcon.saveFileDialog.FileName;
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_FormNotifyIcon.saveFileDialog.FileName;
          this.HasChanges = false;
        }
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void dataGridViewStf_DataError(object sender, DataGridViewDataErrorEventArgs e) {
      MessageBox.Show(e.Exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      this.dataGridViewStf.CancelEdit();
    }

    private void FormSTFEditor_FormClosing(object sender, FormClosingEventArgs e) {
      if ((e.CloseReason != CloseReason.ApplicationExitCall) && (e.CloseReason != CloseReason.WindowsShutDown)) {
        e.Cancel = true;
        if (this.PromptForChanges()) {
          this.Visible = false;
        }
      } else if (!this.PromptForChanges()) {
        e.Cancel = true;
      }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonStfSave.PerformClick();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonStfSaveAs.PerformClick();
    }

    private void toolStripButtonStfSave_Click(object sender, EventArgs e) {
      if ((this.m_STFFile != null) && (this.m_STFFile.Filename != null) && (this.m_STFFile.Filename != String.Empty)) {
        this.m_STFFile.Save(this.m_STFFile.Filename);
      }
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonStfExit.PerformClick();
    }

    private void toolStripButtonStfSave_EnabledChanged(object sender, EventArgs e) {
      this.saveToolStripMenuItem.Enabled = this.toolStripButtonStfSave.Enabled;
    }

    private void toolStripTopMenuItemAbout_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.About(this);
    }

    private void toolStripButtonStfSaveAs_EnabledChanged(object sender, EventArgs e) {
      this.saveAsToolStripMenuItem.Enabled = this.toolStripButtonStfSaveAs.Enabled;
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

    private void dataGridViewStf_UserAddedRow(object sender, DataGridViewRowEventArgs e) {
      this.HasChanges = true;
    }

    private void dataGridViewStf_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
      this.HasChanges = true;
    }

    private void dataGridViewStf_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
      this.HasChanges = true;

      this.dataGridViewStf[e.ColumnIndex, e.RowIndex].Style.BackColor = this.dataGridViewStf.DefaultCellStyle.BackColor;

      foreach (Char character in this.dataGridViewStf[e.ColumnIndex, e.RowIndex].Value.ToString().ToCharArray()) {
        if ((Int32)character > 127) {
          this.dataGridViewStf[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
          return;
        }
      }
    }

    private void toolStripButtonSearch_Click(object sender, EventArgs e) {
      if ((this.m_STFFile != null) && (this.toolStripTextBoxSearch.Text != String.Empty)) {
        Int32 rowIndex = (((this.dataGridViewStf.SelectedCells != null) && (this.dataGridViewStf.SelectedCells.Count > 0)) ? this.dataGridViewStf.SelectedCells[this.dataGridViewStf.SelectedCells.Count - 1].RowIndex : 0);
        Int32 columnIndex = (((this.dataGridViewStf.SelectedCells != null) && (this.dataGridViewStf.SelectedCells.Count > 0)) ? this.dataGridViewStf.SelectedCells[this.dataGridViewStf.SelectedCells.Count - 1].ColumnIndex : 0);

        if ((columnIndex == (this.dataGridViewStf.ColumnCount - 1)) && (rowIndex == (this.dataGridViewStf.RowCount - 1))) {
          MessageBox.Show("End of file reached.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        } else if (columnIndex == (this.dataGridViewStf.ColumnCount - 1)) {
          columnIndex = 0;
          rowIndex++;
        } else {
          columnIndex++;
        }

        for (Int32 row = rowIndex; row < this.dataGridViewStf.RowCount; row++) {
          for (Int32 column = ((row == rowIndex) ? columnIndex : 0); column < this.dataGridViewStf.ColumnCount; column++) {
            if ((this.dataGridViewStf[column, row].Value != null) && (this.dataGridViewStf[column, row].Value.ToString().ToLower().Contains(this.toolStripTextBoxSearch.Text.ToLower()))) {
              this.dataGridViewStf.ClearSelection();
              this.dataGridViewStf[column, row].Selected = true;
              this.dataGridViewStf.FirstDisplayedCell = this.dataGridViewStf[column, row];
              return;
            }
          }
        }

        MessageBox.Show("End of file reached.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
      } else {
        MessageBox.Show("You must first load a file and enter a search term.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripTextBoxSearch_KeyDown(object sender, KeyEventArgs e) {
      if (e.KeyData == Keys.Enter) {
        this.toolStripButtonSearch.PerformClick();
        e.Handled = true;
      }
    }

    private void findToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripTextBoxSearch.Focus();
      if (this.toolStripTextBoxSearch.Text != String.Empty) {
        this.toolStripTextBoxSearch.Select(0, this.toolStripTextBoxSearch.Text.Length);
      }
    }

    private void findAgainToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonSearch_Click(this.toolStripButtonSearch, new EventArgs());
    }
  }
}

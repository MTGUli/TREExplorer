using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BrendanGrant.Helpers.FileAssociation;

namespace TRE_Explorer {
  public partial class FormSettings : Form {
    public FormSettings() {
      InitializeComponent();
    }

    private void FormSettings_Shown(object sender, EventArgs e) {
      this.textBoxOpenFolder.Text = TRE_Explorer.Properties.Settings.Default.OpenFolder;
      this.textBoxSaveFolder.Text = TRE_Explorer.Properties.Settings.Default.SaveFolder;
      switch (TRE_Explorer.Properties.Settings.Default.DefaultView) {
        case View.Details:
          this.comboBoxDefaultView.SelectedIndex = 3;
          break;

        case View.LargeIcon:
          this.comboBoxDefaultView.SelectedIndex = 0;
          break;

        case View.List:
          this.comboBoxDefaultView.SelectedIndex = 2;
          break;

        case View.SmallIcon:
          this.comboBoxDefaultView.SelectedIndex = 1;
          break;
      }
      this.checkBoxDetailsPanePathDisplay.Checked = TRE_Explorer.Properties.Settings.Default.DetailsDisplaysFullPath;
      this.checkBoxPromptForUpdates.Checked = TRE_Explorer.Properties.Settings.Default.PromptForUpdates;
      this.checkBoxDisplayNotifyIcon.Checked = TRE_Explorer.Properties.Settings.Default.DisplayNotifyIcon;
    }

    private void buttonStartingFolderBrowse_Click(object sender, EventArgs e) {
      this.folderBrowserDialog.SelectedPath = this.textBoxOpenFolder.Text;
      this.folderBrowserDialog.Description = "Select a starting open folder:";
      if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK) {
        this.textBoxOpenFolder.Text = this.folderBrowserDialog.SelectedPath;
      }
    }

    private void buttonOK_Click(object sender, EventArgs e) {
      TRE_Explorer.Properties.Settings.Default.OpenFolder = this.textBoxOpenFolder.Text;
      TRE_Explorer.Properties.Settings.Default.SaveFolder = this.textBoxSaveFolder.Text;

      switch (this.comboBoxDefaultView.SelectedItem.ToString()) {
        case "Large Icons":
          TRE_Explorer.Properties.Settings.Default.DefaultView = View.LargeIcon;
          break;

        case "Small Icons":
          TRE_Explorer.Properties.Settings.Default.DefaultView = View.SmallIcon;
          break;

        case "List":
          TRE_Explorer.Properties.Settings.Default.DefaultView = View.List;
          break;

        case "Details":
          TRE_Explorer.Properties.Settings.Default.DefaultView = View.Details;
          break;
      }
      TRE_Explorer.Properties.Settings.Default.DetailsDisplaysFullPath = this.checkBoxDetailsPanePathDisplay.Checked;
      TRE_Explorer.Properties.Settings.Default.PromptForUpdates = this.checkBoxPromptForUpdates.Checked;
      TRE_Explorer.Properties.Settings.Default.DisplayNotifyIcon = this.checkBoxDisplayNotifyIcon.Checked;
      TRE_Explorer.Properties.Settings.Default.Save();

      for (Int32 index = 0; index < this.checkedListBoxFileTypes.Items.Count; index++) {
        FileAssociationInfo fileAssociationInfo = new FileAssociationInfo("." + this.checkedListBoxFileTypes.Items[index].ToString().ToLower());
        if (this.checkedListBoxFileTypes.GetItemChecked(index)) {
          if (!fileAssociationInfo.Exists) {
            fileAssociationInfo.Create("TRE Explorer");
          } else {
            fileAssociationInfo.ProgID = "TRE Explorer";
          }
        } else {
          if ((fileAssociationInfo.Exists) && (fileAssociationInfo.ProgID == "TRE Explorer")) {
            fileAssociationInfo.Delete();
          }
        }
      }

      this.DialogResult = DialogResult.OK;
    }

    private void buttonSaveFolderBrowse_Click(object sender, EventArgs e) {
      this.folderBrowserDialog.SelectedPath = this.textBoxSaveFolder.Text;
      this.folderBrowserDialog.Description = "Select a starting save folder:";
      if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK) {
        this.textBoxSaveFolder.Text = this.folderBrowserDialog.SelectedPath;
      }
    }

    private void buttonCheckAll_Click(object sender, EventArgs e) {
      for (Int32 counter = 0; counter < this.checkedListBoxFileTypes.Items.Count; counter++) {
        this.checkedListBoxFileTypes.SetItemChecked(counter, true);
      }
    }

    private void buttonCheckNone_Click(object sender, EventArgs e) {
      for (Int32 counter = 0; counter < this.checkedListBoxFileTypes.Items.Count; counter++) {
        this.checkedListBoxFileTypes.SetItemChecked(counter, false);
      }
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e) {
      if (this.tabControl.SelectedTab == this.tabPageAssociations) {
        ProgramAssociationInfo programAssociationInfo = new ProgramAssociationInfo("TRE Explorer");
        if (!programAssociationInfo.Exists) {
          programAssociationInfo.Create(new ProgramVerb("Open", "\"" + Application.ExecutablePath + "\" \"%1\""));
          programAssociationInfo.DefaultIcon = new ProgramIcon(Application.ExecutablePath, 0);
        } else {
          if (programAssociationInfo.Verbs.Length > 0) {
            for (Int32 counter = 0; counter < programAssociationInfo.Verbs.Length; counter++) {
              if ((programAssociationInfo.Verbs[counter].Name == "Open") && (programAssociationInfo.Verbs[counter].Command != "\"" + Application.ExecutablePath + "\" \"%1\"")) {
                programAssociationInfo.RemoveVerb(programAssociationInfo.Verbs[counter]);
                programAssociationInfo.AddVerb(new ProgramVerb("Open", "\"" + Application.ExecutablePath + "\" \"%1\""));
                break;
              }
            }
          } else {
            programAssociationInfo.AddVerb(new ProgramVerb("Open", "\"" + Application.ExecutablePath + "\" \"%1\""));
          }
          if (programAssociationInfo.DefaultIcon.Path != Application.ExecutablePath) {
            programAssociationInfo.DefaultIcon = new ProgramIcon(Application.ExecutablePath, 0);
          }
        }

        for (Int32 counter = 0; counter < this.checkedListBoxFileTypes.Items.Count; counter++) {
          FileAssociationInfo fileAssociationInfo = new FileAssociationInfo("." + this.checkedListBoxFileTypes.Items[counter].ToString().ToLower());
          if (fileAssociationInfo.Exists) {
            if (fileAssociationInfo.ProgID == "TRE Explorer") {
              this.checkedListBoxFileTypes.SetItemChecked(counter, true);
            } else {
              this.checkedListBoxFileTypes.SetItemChecked(counter, false);
            }
          } else {
            this.checkedListBoxFileTypes.SetItemChecked(counter, false);
          }
        }
      }
    }
  }
}
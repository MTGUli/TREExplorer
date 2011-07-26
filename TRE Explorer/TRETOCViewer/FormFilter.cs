using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TRE_Explorer {
  public partial class FormFilter : Form {
    private String[] m_Containers;
    private String[] m_FileTypes;

    public void Initialize(String[] Containers, String[] FileTypes) {
      this.m_Containers = Containers;
      this.m_FileTypes = FileTypes;
    }

    public FormFilter() {
      InitializeComponent();
      this.m_Containers = null;
    }

    private void FormFilter_Shown(object sender, EventArgs e) {
      String lastContainer = this.comboBoxContainer.Text;
      String lastFileType = this.comboBoxFileType.Text;

      this.comboBoxContainer.SuspendLayout();
      this.comboBoxFileType.SuspendLayout();

      this.comboBoxContainer.Items.Clear();
      this.comboBoxContainer.Items.Add("All");
      if (this.m_Containers != null) {
        foreach (String Container in this.m_Containers) {
          if (Container.Trim().Length > 0) {
            this.comboBoxContainer.Items.Add(Container);
          }
        }
      }
      if (this.comboBoxContainer.Items.Contains(lastContainer)) {
        this.comboBoxContainer.SelectedIndex = this.comboBoxContainer.Items.IndexOf(lastContainer);
      } else {
        this.comboBoxContainer.SelectedIndex = 0;
      }

      this.comboBoxFileType.Items.Clear();
      this.comboBoxFileType.Items.Add("All");
      if (this.m_FileTypes != null) {
        foreach (String FileType in this.m_FileTypes) {
          if ((FileType.Trim().Length > 0) && (!FileType.Contains("/")) & (FileType != "File Folder")) {
            this.comboBoxFileType.Items.Add(FileType);
          }
        }
      }
      if (this.comboBoxFileType.Items.Contains(lastFileType)) {
        this.comboBoxFileType.SelectedIndex = this.comboBoxFileType.Items.IndexOf(lastFileType);
      } else {
        this.comboBoxFileType.SelectedIndex = 0;
      }

      this.comboBoxContainer.ResumeLayout();
      this.comboBoxFileType.ResumeLayout();
    }

    private void buttonOK_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
    }
  }
}
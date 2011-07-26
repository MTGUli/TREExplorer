using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;

namespace TRE_Explorer {
  public partial class FormIFFEditorFind : Form {
    private FormIFFILFWSEditor m_FormIFFILFWSEditor;

    public FormIFFEditorFind(FormIFFILFWSEditor formIFFILFWSEditor) {
      InitializeComponent();
      this.dynamicByteProvider = new DynamicByteProvider(new Byte[0]);
      this.hexBoxFind.ByteProvider = this.dynamicByteProvider;
      this.m_FormIFFILFWSEditor = formIFFILFWSEditor;
      this.comboBoxSearchRegion.SelectedIndex = 1;
    }

    public Byte[] GetFindBytes() {
      if ((this.radioButtonHexadecimal.Checked) && (this.dynamicByteProvider.Bytes.ToArray().Length > 0)) {
        return this.dynamicByteProvider.Bytes.ToArray();
      } else if ((this.radioButtonString.Checked) && (this.textBoxFind.Text.Length > 0)) {
        return Encoding.ASCII.GetBytes(this.textBoxFind.Text);
      } else {
        return null;
      }
    }

    public String GetFindString() {
      if ((this.radioButtonHexadecimal.Checked) && (this.dynamicByteProvider.Bytes.ToArray().Length > 0)) {
        return new String(Encoding.ASCII.GetChars(this.dynamicByteProvider.Bytes.ToArray()));
      } else if ((this.radioButtonString.Checked) && (this.textBoxFind.Text.Length > 0)) {
        return this.textBoxFind.Text;
      } else {
        return null;
      }
    }

    private DynamicByteProvider dynamicByteProvider;

    private void radioButtonString_CheckedChanged(object sender, EventArgs e) {
      this.textBoxFind.Enabled = this.radioButtonString.Checked;
      this.checkBoxMatchCase.Enabled = this.radioButtonString.Checked;
    }

    private void radioButtonHexadecimal_CheckedChanged(object sender, EventArgs e) {
      this.hexBoxFind.Enabled = this.radioButtonHexadecimal.Checked;
    }

    private void FormIFFEditorFind_Shown(object sender, EventArgs e) {
      if (!this.hexBoxFind.InsertActive) {
        this.hexBoxFind.Focus();
        SendKeys.Send("{INSERT}");
      }
      this.m_FormIFFILFWSEditor.m_SearchAllNodes = (this.comboBoxSearchRegion.Text == "All Nodes");

      if (this.radioButtonHexadecimal.Checked) {
        this.hexBoxFind.Focus();
        if (this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength > 0) {
          Byte[] tempBytes = new Byte[this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength];
          for (Int32 counter = 0; counter < this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength; counter++) {
            tempBytes[counter] = this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.Bytes[(Int32)(this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart + counter)];
          }
          this.dynamicByteProvider = new DynamicByteProvider(tempBytes);
          this.hexBoxFind.ByteProvider = this.dynamicByteProvider;
          this.textBoxFind.Text = new String(Encoding.ASCII.GetChars(tempBytes));

          this.hexBoxFind.Select(0, this.dynamicByteProvider.Length);
          this.textBoxFind.Select(0, this.textBoxFind.Text.Length);
        } else {
          if (this.dynamicByteProvider.Length > 0) {
            this.hexBoxFind.Select(0, this.dynamicByteProvider.Length);
          }
          if (this.textBoxFind.Text.Length > 0) {
            this.textBoxFind.Select(0, this.textBoxFind.Text.Length);
          }
        }
      } else if (this.radioButtonString.Checked) {
        this.textBoxFind.Focus();
        if (this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength > 0) {
          Byte[] tempBytes = new Byte[this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength];
          for (Int32 counter = 0; counter < this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength; counter++) {
            tempBytes[counter] = this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.Bytes[(Int32)(this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart + counter)];
          }
          this.dynamicByteProvider = new DynamicByteProvider(tempBytes);
          this.hexBoxFind.ByteProvider = this.dynamicByteProvider;
          this.textBoxFind.Text = new String(Encoding.ASCII.GetChars(tempBytes));

          this.hexBoxFind.Select(0, this.dynamicByteProvider.Length);
          this.textBoxFind.Select(0, this.textBoxFind.Text.Length);
        } else {
          if (this.dynamicByteProvider.Length > 0) {
            this.hexBoxFind.Select(0, this.dynamicByteProvider.Length);
          }
          if (this.textBoxFind.Text.Length > 0) {
            this.textBoxFind.Select(0, this.textBoxFind.Text.Length);
          }
        }
      }
    }

    private void buttonFindNext_Click(object sender, EventArgs e) {
      if ((this.radioButtonHexadecimal.Checked) && (this.dynamicByteProvider.Bytes.ToArray().Length > 0)) {
        this.m_FormIFFILFWSEditor.m_StringSearch = false;
        this.m_FormIFFILFWSEditor.m_CaseInsensitive = false;
        this.DialogResult = DialogResult.OK;
      } else if ((this.radioButtonString.Checked) && (this.textBoxFind.Text.Length > 0)) {
        this.m_FormIFFILFWSEditor.m_StringSearch = true;
        this.m_FormIFFILFWSEditor.m_CaseInsensitive = !this.checkBoxMatchCase.Checked;
        this.DialogResult = DialogResult.OK;
      } else {
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private void FormIFFEditorFind_KeyDown(object sender, KeyEventArgs e) {
      if (e.KeyCode == Keys.Enter) {
        this.buttonFindNext_Click(this.buttonFindNext, new EventArgs());
        e.Handled = true;
      }
    }

    private void comboBoxSearchRegion_SelectedIndexChanged(object sender, EventArgs e) {
      this.m_FormIFFILFWSEditor.m_SearchAllNodes = (this.comboBoxSearchRegion.Text == "All Nodes");
    }
  }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using SWGLib;

namespace TRE_Explorer {
  public partial class FormIFFEditorReplace : Form {
    private DynamicByteProvider dynamicByteProviderFindWhat;
    private DynamicByteProvider dynamicByteProviderReplaceWith;
    private FormIFFILFWSEditor m_FormIFFILFWSEditor;

    public Byte[] GetFindBytes() {
      if (this.tabControl.SelectedTab == this.tabPageHexadecimal) {
        return this.dynamicByteProviderFindWhat.Bytes.ToArray();
      } else if (this.tabControl.SelectedTab == this.tabPageString) {
        return Encoding.ASCII.GetBytes(this.textBoxFindWhat.Text);
      } else {
        return null;
      }
    }

    public String GetFindString() {
      if (this.tabControl.SelectedTab == this.tabPageHexadecimal) {
        return new String(Encoding.ASCII.GetChars(this.dynamicByteProviderFindWhat.Bytes.ToArray()));
      } else if (this.tabControl.SelectedTab == this.tabPageString) {
        return this.textBoxFindWhat.Text;
      } else {
        return null;
      }
    }

    public Byte[] GetReplaceBytes() {
      if (this.tabControl.SelectedTab == this.tabPageHexadecimal) {
        return this.dynamicByteProviderReplaceWith.Bytes.ToArray();
      } else if (this.tabControl.SelectedTab == this.tabPageString) {
        return Encoding.ASCII.GetBytes(this.textBoxReplaceWith.Text);
      } else {
        return null;
      }
    }

    public String GetReplaceString() {
      if (this.tabControl.SelectedTab == this.tabPageHexadecimal) {
        return new String(Encoding.ASCII.GetChars(this.dynamicByteProviderReplaceWith.Bytes.ToArray()));
      } else if (this.tabControl.SelectedTab == this.tabPageString) {
        return this.textBoxReplaceWith.Text;
      } else {
        return null;
      }
    }

    public FormIFFEditorReplace(FormIFFILFWSEditor formIFFILFWSEditor) {
      InitializeComponent();
      this.m_FormIFFILFWSEditor = formIFFILFWSEditor;
      this.dynamicByteProviderFindWhat = new DynamicByteProvider(new Byte[0]);
      this.hexBoxFindWhat.ByteProvider = this.dynamicByteProviderFindWhat;
      this.dynamicByteProviderReplaceWith = new DynamicByteProvider(new Byte[0]);
      this.hexBoxReplaceWith.ByteProvider = this.dynamicByteProviderReplaceWith;
      this.comboBoxSearchIn.SelectedIndex = 0;
    }

    private void buttonFindNext_Click(object sender, EventArgs e) {
      if ((GetFindBytes() != null) && (GetFindBytes().Length > 0)) {
        this.m_FormIFFILFWSEditor.m_FindBufferIFF = this.GetFindBytes();
        this.m_FormIFFILFWSEditor.m_FindStringIFF = this.GetFindString();

        this.m_FormIFFILFWSEditor.m_CaseInsensitive = ((this.tabControl.SelectedTab == this.tabPageString) ? !this.checkBoxMatchCase.Checked : false);
        this.m_FormIFFILFWSEditor.m_SearchAllNodes = (this.comboBoxSearchIn.Text == "All Nodes");
        this.m_FormIFFILFWSEditor.m_StringSearch = (this.tabControl.SelectedTab == this.tabPageString);

        this.m_FormIFFILFWSEditor.IFFFindNext();
      } else {
        MessageBox.Show("You must first enter a value to find.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonReplace_Click(object sender, EventArgs e) {
      if ((GetFindBytes() != null) && (GetFindBytes().Length > 0)) {
        this.m_FormIFFILFWSEditor.m_FindBufferIFF = this.GetFindBytes();
        this.m_FormIFFILFWSEditor.m_FindStringIFF = this.GetFindString();

        this.m_FormIFFILFWSEditor.m_CaseInsensitive = ((this.tabControl.SelectedTab == this.tabPageString) ? !this.checkBoxMatchCase.Checked : false);
        this.m_FormIFFILFWSEditor.m_SearchAllNodes = (this.comboBoxSearchIn.Text == "All Nodes");
        this.m_FormIFFILFWSEditor.m_StringSearch = (this.tabControl.SelectedTab == this.tabPageString);

        Int32 location = -2;
        if ((this.tabControl.SelectedTab == this.tabPageString) ? !this.checkBoxMatchCase.Checked : false) {
          location = this.m_FormIFFILFWSEditor.Find(this.GetFindString(), this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.Bytes.ToArray(), 0, true);
        } else {
          location = this.m_FormIFFILFWSEditor.Find(this.GetFindBytes(), this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.Bytes.ToArray(), 0);
        }
        if (location != -2) {
          if (this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength > 0) {
            Byte[] buffer = new Byte[this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength];
            for (Int32 counter = 0; counter < this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength; counter++) {
              buffer[counter] = this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.Bytes[(Int32)this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart + counter];
            }

            String findString = BitConverter.ToString(GetFindBytes()).Replace('-', ' ');
            String selectedString = BitConverter.ToString(buffer).Replace('-', ' ');

            if ((this.tabControl.SelectedTab == this.tabPageString) ? !this.checkBoxMatchCase.Checked : false) {
              findString = this.GetFindString().ToLower();
              selectedString = new String(Encoding.ASCII.GetChars(buffer)).ToLower();
            }

            if (findString != selectedString) {
              this.m_FormIFFILFWSEditor.IFFFindNext();
            } else {
              this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.DeleteBytes(this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart, this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength);
              this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.InsertBytes(this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart, GetReplaceBytes());
              this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength = GetReplaceBytes().Length;

              this.m_FormIFFILFWSEditor.IFFFindNext();
            }
          } else {
            this.m_FormIFFILFWSEditor.IFFFindNext();
          }
        } else {
          MessageBox.Show("Value not found.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      } else {
        MessageBox.Show("You must first enter a value to find.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private Int32 Find(IFFFile.IFFNode iffNode) {
      return this.Find(iffNode, 0);
    }

    private Int32 Find(IFFFile.IFFNode iffNode, Int32 startIndex) {
      Int32 returnValue = -1;
      if ((iffNode == null) || (iffNode.ID.Contains("FORM")) || (iffNode.Data == null)) {
        returnValue = -1;
      } else if (this.tabControl.SelectedTab == this.tabPageString) {
        returnValue = this.m_FormIFFILFWSEditor.Find(this.GetFindString(), iffNode.Data, startIndex, !this.checkBoxMatchCase.Checked);
      } else if (this.tabControl.SelectedTab == this.tabPageHexadecimal) {
        returnValue = this.m_FormIFFILFWSEditor.Find(this.GetFindBytes(), iffNode.Data, startIndex);
      }
      return returnValue;
    }

    private void Replace(Int32 startIndex, ByteCollection byteCollection) {
      this.m_FormIFFILFWSEditor.HasChanges = true;
      byteCollection.RemoveRange(startIndex, this.GetFindBytes().Length);
      if ((this.GetReplaceBytes() != null) && (this.GetReplaceBytes().Length > 0)) {
        byteCollection.InsertRange(startIndex, this.GetReplaceBytes());
      }
    }

    private Int32 ReplaceAll(IFFFile.IFFNode iffNode, ref Int32 replacements) {
      ByteCollection byteCollection = new ByteCollection(iffNode.Data);
      Int32 startIndex = this.Find(iffNode);
      Int32 lastStartIndex = -1;
      while (startIndex >= 0) {
        this.Replace(startIndex, byteCollection);
        replacements++;
        lastStartIndex = startIndex;
        startIndex = this.Find(iffNode, (startIndex + ((this.GetReplaceBytes() != null) ? this.GetReplaceBytes().Length : 0)));
      }
      iffNode.Data = byteCollection.ToArray();

      return lastStartIndex;
    }

    private void buttonReplaceAll_Click(object sender, EventArgs e) {
      if ((GetFindBytes() != null) && (GetFindBytes().Length > 0)) {
        this.m_FormIFFILFWSEditor.m_FindBufferIFF = this.GetFindBytes();
        this.m_FormIFFILFWSEditor.m_FindStringIFF = this.GetFindString();

        this.m_FormIFFILFWSEditor.m_CaseInsensitive = ((this.tabControl.SelectedTab == this.tabPageString) ? !this.checkBoxMatchCase.Checked : false);
        this.m_FormIFFILFWSEditor.m_SearchAllNodes = (this.comboBoxSearchIn.Text == "All Nodes");
        this.m_FormIFFILFWSEditor.m_StringSearch = (this.tabControl.SelectedTab == this.tabPageString);

        TreeNode treeNode = null;
        try {
          treeNode = ((this.m_FormIFFILFWSEditor.m_SearchAllNodes) ? this.m_FormIFFILFWSEditor.treeViewIff.Nodes[0] : this.m_FormIFFILFWSEditor.treeViewIff.SelectedNode);
        } catch {
          MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        IFFFile.IFFNode iffNode = this.m_FormIFFILFWSEditor.FindIFFNode(treeNode);
        Int32 startIndex = this.Find(iffNode);
        Int32 lastStartIndex = -1;
        Int32 replacements = 0;

        if ((!(startIndex >= 0)) && (!this.m_FormIFFILFWSEditor.m_SearchAllNodes)) {
          MessageBox.Show("Value not found.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        } else if (!this.m_FormIFFILFWSEditor.m_SearchAllNodes) {
          lastStartIndex = this.ReplaceAll(iffNode, ref replacements);

          this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF = new DynamicByteProvider(iffNode.Data);
          this.m_FormIFFILFWSEditor.hexBoxIff.ByteProvider = this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF;

          if ((this.GetReplaceBytes() != null) && (this.GetReplaceBytes().Length > 0)) {
            this.m_FormIFFILFWSEditor.hexBoxIff.Find(this.GetReplaceBytes(), lastStartIndex);
          } else {
            this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart = lastStartIndex;
            this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength = 0;
          }
          MessageBox.Show(replacements + " replacement" + ((replacements == 1) ? String.Empty : "s") + " made.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        } else if (this.m_FormIFFILFWSEditor.m_SearchAllNodes) {
          TreeNode lastTreeNode = null;
          while (treeNode != null) {
            iffNode = this.m_FormIFFILFWSEditor.FindIFFNode(treeNode);
            startIndex = this.Find(iffNode);
            if (startIndex >= 0) {
              lastStartIndex = this.ReplaceAll(iffNode, ref replacements);
              lastTreeNode = treeNode;
            }
            treeNode = this.m_FormIFFILFWSEditor.NextNode(treeNode, false);
          }
          if (lastTreeNode != null) {
            if (this.m_FormIFFILFWSEditor.treeViewIff.SelectedNode != lastTreeNode) {
              this.m_FormIFFILFWSEditor.treeViewIff.SelectedNode = lastTreeNode;
            } else {
              this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF = new DynamicByteProvider(this.m_FormIFFILFWSEditor.FindIFFNode(lastTreeNode).Data);
              this.m_FormIFFILFWSEditor.hexBoxIff.ByteProvider = this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF;
            }
            if ((this.GetReplaceBytes() != null) && (this.GetReplaceBytes().Length > 0)) {
              this.m_FormIFFILFWSEditor.hexBoxIff.Find(this.GetReplaceBytes(), lastStartIndex);
            } else {
              this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart = lastStartIndex;
              this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength = 0;
            }
            MessageBox.Show(replacements + " replacement" + ((replacements == 1) ? String.Empty : "s") + " made.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
          } else {
            MessageBox.Show("Value not found.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      } else {
        MessageBox.Show("You must first enter a value to find.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void FormIFFEditorReplace_Shown(object sender, EventArgs e) {
      if (this.tabControl.SelectedTab == this.tabPageString) {
        this.textBoxFindWhat.Focus();
        if (this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength > 0) {
          Byte[] tempBytes = new Byte[this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength];
          for (Int32 counter = 0; counter < this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength; counter++) {
            tempBytes[counter] = this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.Bytes[(Int32)(this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart + counter)];
          }
          this.dynamicByteProviderFindWhat = new DynamicByteProvider(tempBytes);
          this.hexBoxFindWhat.ByteProvider = this.dynamicByteProviderFindWhat;
          this.textBoxFindWhat.Text = new String(Encoding.ASCII.GetChars(tempBytes));

          this.hexBoxFindWhat.Select(0, this.dynamicByteProviderFindWhat.Length);
          this.textBoxFindWhat.Select(0, this.textBoxFindWhat.Text.Length);
        } else {
          if ((this.dynamicByteProviderFindWhat != null) && (this.dynamicByteProviderFindWhat.Length > 0)) {
            this.hexBoxFindWhat.Select(0, this.dynamicByteProviderFindWhat.Length);
          }
          if ((this.textBoxFindWhat.Text != String.Empty) && (this.textBoxFindWhat.Text.Length > 0)) {
            this.textBoxFindWhat.Select(0, this.textBoxFindWhat.Text.Length);
          }
        }
      } else if (this.tabControl.SelectedTab == this.tabPageHexadecimal) {
        this.hexBoxFindWhat.Focus();
        if (this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength > 0) {
          Byte[] tempBytes = new Byte[this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength];
          for (Int32 counter = 0; counter < this.m_FormIFFILFWSEditor.hexBoxIff.SelectionLength; counter++) {
            tempBytes[counter] = this.m_FormIFFILFWSEditor.m_DynamicByteProviderIFF.Bytes[(Int32)(this.m_FormIFFILFWSEditor.hexBoxIff.SelectionStart + counter)];
          }
          this.dynamicByteProviderFindWhat = new DynamicByteProvider(tempBytes);
          this.hexBoxFindWhat.ByteProvider = this.dynamicByteProviderFindWhat;
          this.textBoxFindWhat.Text = new String(Encoding.ASCII.GetChars(tempBytes));

          this.hexBoxFindWhat.Select(0, this.dynamicByteProviderFindWhat.Length);
          this.textBoxFindWhat.Select(0, this.textBoxFindWhat.Text.Length);
        } else {
          if ((this.dynamicByteProviderFindWhat != null) && (this.dynamicByteProviderFindWhat.Length > 0)) {
            this.hexBoxFindWhat.Select(0, this.dynamicByteProviderFindWhat.Length);
          }
          if ((this.textBoxFindWhat.Text != String.Empty) && (this.textBoxFindWhat.Text.Length > 0)) {
            this.textBoxFindWhat.Select(0, this.textBoxFindWhat.Text.Length);
          }
        }
      }
      this.m_FormIFFILFWSEditor.m_SearchAllNodes = (this.comboBoxSearchIn.Text == "All Nodes");
      this.m_FormIFFILFWSEditor.m_CaseInsensitive = ((this.tabControl.SelectedTab == this.tabPageString) ? !this.checkBoxMatchCase.Checked : false);
    }

    private void buttonCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
    }

    private void comboBoxSearchIn_SelectedIndexChanged(object sender, EventArgs e) {
      this.m_FormIFFILFWSEditor.m_SearchAllNodes = (this.comboBoxSearchIn.Text == "All Nodes");
    }

    private void checkBoxMatchCase_CheckedChanged(object sender, EventArgs e) {
      this.m_FormIFFILFWSEditor.m_CaseInsensitive = !this.checkBoxMatchCase.Checked;
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e) {
      if (this.tabControl.SelectedTab == this.tabPageHexadecimal) {
        this.m_FormIFFILFWSEditor.m_CaseInsensitive = false;
      } else if (this.tabControl.SelectedTab == this.tabPageString) {
        this.m_FormIFFILFWSEditor.m_CaseInsensitive = !this.checkBoxMatchCase.Checked;
      } else {
        this.m_FormIFFILFWSEditor.m_CaseInsensitive = false;
      }
    }
  }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TRE_Explorer {
  public partial class FormWSItemManager : Form {
    internal List<String> m_ItemNames;
    private Int32 m_MinimumLength;

    public FormWSItemManager(String[] ItemNames, Int32 MinimumLength) {
      InitializeComponent();
      this.m_ItemNames = new List<String>();
      this.m_ItemNames.AddRange(ItemNames);
      this.m_MinimumLength = MinimumLength;
    }

    private void FormWSItemManager_Shown(object sender, EventArgs e) {
      this.listBoxItemNames.Items.Clear();
      this.listBoxItemNames.Items.AddRange(this.m_ItemNames.ToArray());
    }

    private void buttonOK_Click(object sender, EventArgs e) {
      if (this.listBoxItemNames.Items.Count >= this.m_MinimumLength) {
        this.m_ItemNames.Clear();
        foreach (Object obj in this.listBoxItemNames.Items) {
          this.m_ItemNames.Add(obj.ToString());
        }
        this.DialogResult = DialogResult.OK;
      } else {
        MessageBox.Show("You must have at least " + this.m_MinimumLength + " item" + ((this.m_MinimumLength == 1) ? "" : "s") + " in this list. There are currently " + this.listBoxItemNames.Items.Count + " item" + ((this.listBoxItemNames.Items.Count == 1) ? "" : "s") + " present.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
    }

    private void buttonAddItem_Click(object sender, EventArgs e) {
      if ((this.textBoxItemName.Text != String.Empty) && (!this.m_ItemNames.Contains(this.textBoxItemName.Text))) {
        this.m_ItemNames.Add(this.textBoxItemName.Text);
        this.listBoxItemNames.Items.Add(this.textBoxItemName.Text);
      }
    }

    private void listBoxItemNames_SelectedIndexChanged(object sender, EventArgs e) {
      if (this.listBoxItemNames.SelectedIndex > 0) {
        this.buttonMoveItemToTop.Enabled = true;
        this.buttonMoveItemUp.Enabled = true;
      } else {
        this.buttonMoveItemToTop.Enabled = false;
        this.buttonMoveItemUp.Enabled = false;
      }
      if ((this.listBoxItemNames.SelectedIndex >= 0) && (this.listBoxItemNames.SelectedIndex < (this.listBoxItemNames.Items.Count - 1))) {
        this.buttonMoveItemToBottom.Enabled = true;
        this.buttonMoveItemDown.Enabled = true;
      } else {
        this.buttonMoveItemToBottom.Enabled = false;
        this.buttonMoveItemDown.Enabled = false;
      }
      if ((this.listBoxItemNames.SelectedIndex >= 0) && (this.listBoxItemNames.SelectedIndex < this.listBoxItemNames.Items.Count)) {
        this.buttonRemoveItem.Enabled = true;
      } else {
        this.buttonRemoveItem.Enabled = false;
      }
    }

    private void buttonMoveItemToTop_Click(object sender, EventArgs e) {
      String ItemName = this.listBoxItemNames.SelectedItem.ToString();
      this.listBoxItemNames.Items.RemoveAt(this.listBoxItemNames.SelectedIndex);
      this.listBoxItemNames.Items.Insert(0, ItemName);
      this.listBoxItemNames.SelectedIndex = 0;
    }

    private void buttonMoveItemUp_Click(object sender, EventArgs e) {
      String ItemName = this.listBoxItemNames.SelectedItem.ToString();
      Int32 Index = this.listBoxItemNames.SelectedIndex;
      this.listBoxItemNames.Items.RemoveAt(Index);
      this.listBoxItemNames.Items.Insert((Index - 1), ItemName);
      this.listBoxItemNames.SelectedIndex = (Index - 1);
    }

    private void buttonMoveItemDown_Click(object sender, EventArgs e) {
      String ItemName = this.listBoxItemNames.SelectedItem.ToString();
      Int32 Index = this.listBoxItemNames.SelectedIndex;
      this.listBoxItemNames.Items.RemoveAt(Index);
      this.listBoxItemNames.Items.Insert((Index + 1), ItemName);
      this.listBoxItemNames.SelectedIndex = (Index + 1);
    }

    private void buttonMoveItemToBottom_Click(object sender, EventArgs e) {
      String ItemName = this.listBoxItemNames.SelectedItem.ToString();
      this.listBoxItemNames.Items.RemoveAt(this.listBoxItemNames.SelectedIndex);
      this.listBoxItemNames.Items.Add(ItemName);
      this.listBoxItemNames.SelectedIndex = (this.listBoxItemNames.Items.Count - 1);
    }

    private void textBoxItemName_TextChanged(object sender, EventArgs e) {
      this.buttonAddItem.Enabled = (this.textBoxItemName.Text.Length > 0);
    }

    private void buttonRemoveItem_Click(object sender, EventArgs e) {
      this.listBoxItemNames.Items.RemoveAt(this.listBoxItemNames.SelectedIndex);
    }
  }
}
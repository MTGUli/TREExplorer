namespace TRE_Explorer {
  partial class FormWSItemManager {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWSItemManager));
      this.listBoxItemNames = new System.Windows.Forms.ListBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.textBoxItemName = new System.Windows.Forms.TextBox();
      this.buttonAddItem = new System.Windows.Forms.Button();
      this.buttonMoveItemUp = new System.Windows.Forms.Button();
      this.buttonMoveItemDown = new System.Windows.Forms.Button();
      this.buttonMoveItemToTop = new System.Windows.Forms.Button();
      this.buttonMoveItemToBottom = new System.Windows.Forms.Button();
      this.buttonRemoveItem = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // listBoxItemNames
      // 
      this.listBoxItemNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxItemNames.FormattingEnabled = true;
      this.listBoxItemNames.IntegralHeight = false;
      this.listBoxItemNames.Location = new System.Drawing.Point(12, 41);
      this.listBoxItemNames.Name = "listBoxItemNames";
      this.listBoxItemNames.Size = new System.Drawing.Size(420, 252);
      this.listBoxItemNames.TabIndex = 0;
      this.listBoxItemNames.SelectedIndexChanged += new System.EventHandler(this.listBoxItemNames_SelectedIndexChanged);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(387, 299);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 1;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(306, 299);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // textBoxItemName
      // 
      this.textBoxItemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxItemName.Location = new System.Drawing.Point(12, 13);
      this.textBoxItemName.Name = "textBoxItemName";
      this.textBoxItemName.Size = new System.Drawing.Size(420, 20);
      this.textBoxItemName.TabIndex = 3;
      this.textBoxItemName.TextChanged += new System.EventHandler(this.textBoxItemName_TextChanged);
      // 
      // buttonAddItem
      // 
      this.buttonAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAddItem.Enabled = false;
      this.buttonAddItem.Image = global::TRE_Explorer.Properties.Resources.Add_Item;
      this.buttonAddItem.Location = new System.Drawing.Point(438, 12);
      this.buttonAddItem.Name = "buttonAddItem";
      this.buttonAddItem.Size = new System.Drawing.Size(24, 23);
      this.buttonAddItem.TabIndex = 4;
      this.buttonAddItem.UseVisualStyleBackColor = true;
      this.buttonAddItem.Click += new System.EventHandler(this.buttonAddItem_Click);
      // 
      // buttonMoveItemUp
      // 
      this.buttonMoveItemUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonMoveItemUp.Enabled = false;
      this.buttonMoveItemUp.Image = global::TRE_Explorer.Properties.Resources.Move_Up;
      this.buttonMoveItemUp.Location = new System.Drawing.Point(438, 142);
      this.buttonMoveItemUp.Name = "buttonMoveItemUp";
      this.buttonMoveItemUp.Size = new System.Drawing.Size(24, 23);
      this.buttonMoveItemUp.TabIndex = 5;
      this.buttonMoveItemUp.UseVisualStyleBackColor = true;
      this.buttonMoveItemUp.Click += new System.EventHandler(this.buttonMoveItemUp_Click);
      // 
      // buttonMoveItemDown
      // 
      this.buttonMoveItemDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonMoveItemDown.Enabled = false;
      this.buttonMoveItemDown.Image = global::TRE_Explorer.Properties.Resources.Move_Down;
      this.buttonMoveItemDown.Location = new System.Drawing.Point(438, 171);
      this.buttonMoveItemDown.Name = "buttonMoveItemDown";
      this.buttonMoveItemDown.Size = new System.Drawing.Size(24, 23);
      this.buttonMoveItemDown.TabIndex = 6;
      this.buttonMoveItemDown.UseVisualStyleBackColor = true;
      this.buttonMoveItemDown.Click += new System.EventHandler(this.buttonMoveItemDown_Click);
      // 
      // buttonMoveItemToTop
      // 
      this.buttonMoveItemToTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonMoveItemToTop.Enabled = false;
      this.buttonMoveItemToTop.Image = global::TRE_Explorer.Properties.Resources.Move_Top;
      this.buttonMoveItemToTop.Location = new System.Drawing.Point(438, 113);
      this.buttonMoveItemToTop.Name = "buttonMoveItemToTop";
      this.buttonMoveItemToTop.Size = new System.Drawing.Size(24, 23);
      this.buttonMoveItemToTop.TabIndex = 7;
      this.buttonMoveItemToTop.UseVisualStyleBackColor = true;
      this.buttonMoveItemToTop.Click += new System.EventHandler(this.buttonMoveItemToTop_Click);
      // 
      // buttonMoveItemToBottom
      // 
      this.buttonMoveItemToBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonMoveItemToBottom.Enabled = false;
      this.buttonMoveItemToBottom.Image = global::TRE_Explorer.Properties.Resources.Move_Bottom;
      this.buttonMoveItemToBottom.Location = new System.Drawing.Point(438, 200);
      this.buttonMoveItemToBottom.Name = "buttonMoveItemToBottom";
      this.buttonMoveItemToBottom.Size = new System.Drawing.Size(24, 23);
      this.buttonMoveItemToBottom.TabIndex = 8;
      this.buttonMoveItemToBottom.UseVisualStyleBackColor = true;
      this.buttonMoveItemToBottom.Click += new System.EventHandler(this.buttonMoveItemToBottom_Click);
      // 
      // buttonRemoveItem
      // 
      this.buttonRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonRemoveItem.Enabled = false;
      this.buttonRemoveItem.Image = global::TRE_Explorer.Properties.Resources.Remove_Item;
      this.buttonRemoveItem.Location = new System.Drawing.Point(438, 41);
      this.buttonRemoveItem.Name = "buttonRemoveItem";
      this.buttonRemoveItem.Size = new System.Drawing.Size(24, 23);
      this.buttonRemoveItem.TabIndex = 9;
      this.buttonRemoveItem.UseVisualStyleBackColor = true;
      this.buttonRemoveItem.Click += new System.EventHandler(this.buttonRemoveItem_Click);
      // 
      // FormWSItemManager
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(474, 334);
      this.ControlBox = false;
      this.Controls.Add(this.buttonRemoveItem);
      this.Controls.Add(this.buttonMoveItemToBottom);
      this.Controls.Add(this.buttonMoveItemToTop);
      this.Controls.Add(this.buttonMoveItemDown);
      this.Controls.Add(this.buttonMoveItemUp);
      this.Controls.Add(this.buttonAddItem);
      this.Controls.Add(this.textBoxItemName);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.listBoxItemNames);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormWSItemManager";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.Shown += new System.EventHandler(this.FormWSItemManager_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox listBoxItemNames;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.TextBox textBoxItemName;
    private System.Windows.Forms.Button buttonAddItem;
    private System.Windows.Forms.Button buttonMoveItemUp;
    private System.Windows.Forms.Button buttonMoveItemDown;
    private System.Windows.Forms.Button buttonMoveItemToTop;
    private System.Windows.Forms.Button buttonMoveItemToBottom;
    private System.Windows.Forms.Button buttonRemoveItem;
  }
}
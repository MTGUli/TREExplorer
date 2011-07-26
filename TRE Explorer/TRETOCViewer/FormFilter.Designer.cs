namespace TRE_Explorer {
  partial class FormFilter {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFilter));
      this.labelContainer = new System.Windows.Forms.Label();
      this.comboBoxContainer = new System.Windows.Forms.ComboBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.comboBoxFileType = new System.Windows.Forms.ComboBox();
      this.labelFileType = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // labelContainer
      // 
      this.labelContainer.AutoSize = true;
      this.labelContainer.Location = new System.Drawing.Point(12, 16);
      this.labelContainer.Name = "labelContainer";
      this.labelContainer.Size = new System.Drawing.Size(55, 13);
      this.labelContainer.TabIndex = 0;
      this.labelContainer.Text = "Container:";
      // 
      // comboBoxContainer
      // 
      this.comboBoxContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxContainer.FormattingEnabled = true;
      this.comboBoxContainer.Location = new System.Drawing.Point(71, 12);
      this.comboBoxContainer.Name = "comboBoxContainer";
      this.comboBoxContainer.Size = new System.Drawing.Size(201, 21);
      this.comboBoxContainer.TabIndex = 1;
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(197, 66);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(116, 66);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // comboBoxFileType
      // 
      this.comboBoxFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxFileType.FormattingEnabled = true;
      this.comboBoxFileType.Location = new System.Drawing.Point(71, 39);
      this.comboBoxFileType.Name = "comboBoxFileType";
      this.comboBoxFileType.Size = new System.Drawing.Size(201, 21);
      this.comboBoxFileType.TabIndex = 4;
      // 
      // labelFileType
      // 
      this.labelFileType.AutoSize = true;
      this.labelFileType.Location = new System.Drawing.Point(12, 43);
      this.labelFileType.Name = "labelFileType";
      this.labelFileType.Size = new System.Drawing.Size(53, 13);
      this.labelFileType.TabIndex = 5;
      this.labelFileType.Text = "File Type:";
      // 
      // FormFilter
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(284, 101);
      this.ControlBox = false;
      this.Controls.Add(this.labelFileType);
      this.Controls.Add(this.comboBoxFileType);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.comboBoxContainer);
      this.Controls.Add(this.labelContainer);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormFilter";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.Shown += new System.EventHandler(this.FormFilter_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelContainer;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    internal System.Windows.Forms.ComboBox comboBoxContainer;
    private System.Windows.Forms.Label labelFileType;
    internal System.Windows.Forms.ComboBox comboBoxFileType;
  }
}
namespace TRE_Explorer {
  partial class FormIFFEditorFind {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIFFEditorFind));
      this.radioButtonString = new System.Windows.Forms.RadioButton();
      this.radioButtonHexadecimal = new System.Windows.Forms.RadioButton();
      this.textBoxFind = new System.Windows.Forms.TextBox();
      this.hexBoxFind = new Be.Windows.Forms.HexBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonFindNext = new System.Windows.Forms.Button();
      this.comboBoxSearchRegion = new System.Windows.Forms.ComboBox();
      this.labelSearchIn = new System.Windows.Forms.Label();
      this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // radioButtonString
      // 
      this.radioButtonString.AutoSize = true;
      this.radioButtonString.Checked = true;
      this.radioButtonString.Location = new System.Drawing.Point(12, 14);
      this.radioButtonString.Name = "radioButtonString";
      this.radioButtonString.Size = new System.Drawing.Size(52, 17);
      this.radioButtonString.TabIndex = 1;
      this.radioButtonString.TabStop = true;
      this.radioButtonString.Text = "String";
      this.radioButtonString.UseVisualStyleBackColor = true;
      this.radioButtonString.CheckedChanged += new System.EventHandler(this.radioButtonString_CheckedChanged);
      // 
      // radioButtonHexadecimal
      // 
      this.radioButtonHexadecimal.AutoSize = true;
      this.radioButtonHexadecimal.Location = new System.Drawing.Point(12, 48);
      this.radioButtonHexadecimal.Name = "radioButtonHexadecimal";
      this.radioButtonHexadecimal.Size = new System.Drawing.Size(86, 17);
      this.radioButtonHexadecimal.TabIndex = 3;
      this.radioButtonHexadecimal.Text = "Hexadecimal";
      this.radioButtonHexadecimal.UseVisualStyleBackColor = true;
      this.radioButtonHexadecimal.CheckedChanged += new System.EventHandler(this.radioButtonHexadecimal_CheckedChanged);
      // 
      // textBoxFind
      // 
      this.textBoxFind.Location = new System.Drawing.Point(113, 12);
      this.textBoxFind.Name = "textBoxFind";
      this.textBoxFind.Size = new System.Drawing.Size(217, 20);
      this.textBoxFind.TabIndex = 0;
      this.textBoxFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormIFFEditorFind_KeyDown);
      // 
      // hexBoxFind
      // 
      this.hexBoxFind.Enabled = false;
      this.hexBoxFind.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexBoxFind.LineInfoForeColor = System.Drawing.Color.Empty;
      this.hexBoxFind.Location = new System.Drawing.Point(113, 38);
      this.hexBoxFind.Name = "hexBoxFind";
      this.hexBoxFind.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
      this.hexBoxFind.Size = new System.Drawing.Size(217, 37);
      this.hexBoxFind.TabIndex = 2;
      this.hexBoxFind.VScrollBarVisible = true;
      this.hexBoxFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormIFFEditorFind_KeyDown);
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(255, 108);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonFindNext
      // 
      this.buttonFindNext.Location = new System.Drawing.Point(174, 108);
      this.buttonFindNext.Name = "buttonFindNext";
      this.buttonFindNext.Size = new System.Drawing.Size(75, 23);
      this.buttonFindNext.TabIndex = 7;
      this.buttonFindNext.Text = "Find Next";
      this.buttonFindNext.UseVisualStyleBackColor = true;
      this.buttonFindNext.Click += new System.EventHandler(this.buttonFindNext_Click);
      // 
      // comboBoxSearchRegion
      // 
      this.comboBoxSearchRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxSearchRegion.FormattingEnabled = true;
      this.comboBoxSearchRegion.Items.AddRange(new object[] {
            "This Node",
            "All Nodes"});
      this.comboBoxSearchRegion.Location = new System.Drawing.Point(113, 81);
      this.comboBoxSearchRegion.Name = "comboBoxSearchRegion";
      this.comboBoxSearchRegion.Size = new System.Drawing.Size(217, 21);
      this.comboBoxSearchRegion.TabIndex = 4;
      this.comboBoxSearchRegion.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchRegion_SelectedIndexChanged);
      // 
      // labelSearchIn
      // 
      this.labelSearchIn.AutoSize = true;
      this.labelSearchIn.Location = new System.Drawing.Point(12, 85);
      this.labelSearchIn.Name = "labelSearchIn";
      this.labelSearchIn.Size = new System.Drawing.Size(53, 13);
      this.labelSearchIn.TabIndex = 5;
      this.labelSearchIn.Text = "Search In";
      // 
      // checkBoxMatchCase
      // 
      this.checkBoxMatchCase.AutoSize = true;
      this.checkBoxMatchCase.Location = new System.Drawing.Point(12, 111);
      this.checkBoxMatchCase.Name = "checkBoxMatchCase";
      this.checkBoxMatchCase.Size = new System.Drawing.Size(83, 17);
      this.checkBoxMatchCase.TabIndex = 8;
      this.checkBoxMatchCase.Text = "Match Case";
      this.checkBoxMatchCase.UseVisualStyleBackColor = true;
      // 
      // FormIFFEditorFind
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(342, 143);
      this.ControlBox = false;
      this.Controls.Add(this.checkBoxMatchCase);
      this.Controls.Add(this.labelSearchIn);
      this.Controls.Add(this.comboBoxSearchRegion);
      this.Controls.Add(this.buttonFindNext);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.hexBoxFind);
      this.Controls.Add(this.textBoxFind);
      this.Controls.Add(this.radioButtonHexadecimal);
      this.Controls.Add(this.radioButtonString);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormIFFEditorFind";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.Shown += new System.EventHandler(this.FormIFFEditorFind_Shown);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormIFFEditorFind_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RadioButton radioButtonString;
    private System.Windows.Forms.RadioButton radioButtonHexadecimal;
    private System.Windows.Forms.TextBox textBoxFind;
    private Be.Windows.Forms.HexBox hexBoxFind;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonFindNext;
    private System.Windows.Forms.Label labelSearchIn;
    internal System.Windows.Forms.ComboBox comboBoxSearchRegion;
    private System.Windows.Forms.CheckBox checkBoxMatchCase;
  }
}
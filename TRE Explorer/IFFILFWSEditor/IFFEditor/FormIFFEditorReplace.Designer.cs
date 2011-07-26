namespace TRE_Explorer {
  partial class FormIFFEditorReplace {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIFFEditorReplace));
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageString = new System.Windows.Forms.TabPage();
      this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
      this.labelReplaceWithString = new System.Windows.Forms.Label();
      this.labelFindWhatString = new System.Windows.Forms.Label();
      this.textBoxReplaceWith = new System.Windows.Forms.TextBox();
      this.textBoxFindWhat = new System.Windows.Forms.TextBox();
      this.tabPageHexadecimal = new System.Windows.Forms.TabPage();
      this.labelReplaceWithHexadecimal = new System.Windows.Forms.Label();
      this.labelFindWhatHexadecimal = new System.Windows.Forms.Label();
      this.hexBoxReplaceWith = new Be.Windows.Forms.HexBox();
      this.hexBoxFindWhat = new Be.Windows.Forms.HexBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonFindNext = new System.Windows.Forms.Button();
      this.buttonReplace = new System.Windows.Forms.Button();
      this.buttonReplaceAll = new System.Windows.Forms.Button();
      this.comboBoxSearchIn = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.tabControl.SuspendLayout();
      this.tabPageString.SuspendLayout();
      this.tabPageHexadecimal.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add(this.tabPageString);
      this.tabControl.Controls.Add(this.tabPageHexadecimal);
      this.tabControl.Location = new System.Drawing.Point(12, 12);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(318, 113);
      this.tabControl.TabIndex = 0;
      this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
      // 
      // tabPageString
      // 
      this.tabPageString.Controls.Add(this.checkBoxMatchCase);
      this.tabPageString.Controls.Add(this.labelReplaceWithString);
      this.tabPageString.Controls.Add(this.labelFindWhatString);
      this.tabPageString.Controls.Add(this.textBoxReplaceWith);
      this.tabPageString.Controls.Add(this.textBoxFindWhat);
      this.tabPageString.Location = new System.Drawing.Point(4, 22);
      this.tabPageString.Name = "tabPageString";
      this.tabPageString.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageString.Size = new System.Drawing.Size(310, 87);
      this.tabPageString.TabIndex = 0;
      this.tabPageString.Tag = "String";
      this.tabPageString.Text = "String";
      this.tabPageString.UseVisualStyleBackColor = true;
      // 
      // checkBoxMatchCase
      // 
      this.checkBoxMatchCase.AutoSize = true;
      this.checkBoxMatchCase.Location = new System.Drawing.Point(9, 64);
      this.checkBoxMatchCase.Name = "checkBoxMatchCase";
      this.checkBoxMatchCase.Size = new System.Drawing.Size(83, 17);
      this.checkBoxMatchCase.TabIndex = 5;
      this.checkBoxMatchCase.Text = "Match Case";
      this.checkBoxMatchCase.UseVisualStyleBackColor = true;
      this.checkBoxMatchCase.CheckedChanged += new System.EventHandler(this.checkBoxMatchCase_CheckedChanged);
      // 
      // labelReplaceWithString
      // 
      this.labelReplaceWithString.AutoSize = true;
      this.labelReplaceWithString.Location = new System.Drawing.Point(6, 42);
      this.labelReplaceWithString.Name = "labelReplaceWithString";
      this.labelReplaceWithString.Size = new System.Drawing.Size(75, 13);
      this.labelReplaceWithString.TabIndex = 3;
      this.labelReplaceWithString.Text = "Replace With:";
      // 
      // labelFindWhatString
      // 
      this.labelFindWhatString.AutoSize = true;
      this.labelFindWhatString.Location = new System.Drawing.Point(6, 10);
      this.labelFindWhatString.Name = "labelFindWhatString";
      this.labelFindWhatString.Size = new System.Drawing.Size(59, 13);
      this.labelFindWhatString.TabIndex = 2;
      this.labelFindWhatString.Text = "Find What:";
      // 
      // textBoxReplaceWith
      // 
      this.textBoxReplaceWith.Location = new System.Drawing.Point(87, 38);
      this.textBoxReplaceWith.Name = "textBoxReplaceWith";
      this.textBoxReplaceWith.Size = new System.Drawing.Size(217, 20);
      this.textBoxReplaceWith.TabIndex = 1;
      // 
      // textBoxFindWhat
      // 
      this.textBoxFindWhat.Location = new System.Drawing.Point(87, 6);
      this.textBoxFindWhat.Name = "textBoxFindWhat";
      this.textBoxFindWhat.Size = new System.Drawing.Size(217, 20);
      this.textBoxFindWhat.TabIndex = 0;
      // 
      // tabPageHexadecimal
      // 
      this.tabPageHexadecimal.Controls.Add(this.labelReplaceWithHexadecimal);
      this.tabPageHexadecimal.Controls.Add(this.labelFindWhatHexadecimal);
      this.tabPageHexadecimal.Controls.Add(this.hexBoxReplaceWith);
      this.tabPageHexadecimal.Controls.Add(this.hexBoxFindWhat);
      this.tabPageHexadecimal.Location = new System.Drawing.Point(4, 22);
      this.tabPageHexadecimal.Name = "tabPageHexadecimal";
      this.tabPageHexadecimal.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageHexadecimal.Size = new System.Drawing.Size(310, 87);
      this.tabPageHexadecimal.TabIndex = 1;
      this.tabPageHexadecimal.Tag = "Hexadecimal";
      this.tabPageHexadecimal.Text = "Hexadecimal";
      this.tabPageHexadecimal.UseVisualStyleBackColor = true;
      // 
      // labelReplaceWithHexadecimal
      // 
      this.labelReplaceWithHexadecimal.AutoSize = true;
      this.labelReplaceWithHexadecimal.Location = new System.Drawing.Point(6, 59);
      this.labelReplaceWithHexadecimal.Name = "labelReplaceWithHexadecimal";
      this.labelReplaceWithHexadecimal.Size = new System.Drawing.Size(75, 13);
      this.labelReplaceWithHexadecimal.TabIndex = 3;
      this.labelReplaceWithHexadecimal.Text = "Replace With:";
      // 
      // labelFindWhatHexadecimal
      // 
      this.labelFindWhatHexadecimal.AutoSize = true;
      this.labelFindWhatHexadecimal.Location = new System.Drawing.Point(6, 15);
      this.labelFindWhatHexadecimal.Name = "labelFindWhatHexadecimal";
      this.labelFindWhatHexadecimal.Size = new System.Drawing.Size(59, 13);
      this.labelFindWhatHexadecimal.TabIndex = 2;
      this.labelFindWhatHexadecimal.Text = "Find What:";
      // 
      // hexBoxReplaceWith
      // 
      this.hexBoxReplaceWith.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexBoxReplaceWith.LineInfoForeColor = System.Drawing.Color.Empty;
      this.hexBoxReplaceWith.Location = new System.Drawing.Point(87, 47);
      this.hexBoxReplaceWith.Name = "hexBoxReplaceWith";
      this.hexBoxReplaceWith.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
      this.hexBoxReplaceWith.Size = new System.Drawing.Size(217, 37);
      this.hexBoxReplaceWith.TabIndex = 1;
      this.hexBoxReplaceWith.VScrollBarVisible = true;
      // 
      // hexBoxFindWhat
      // 
      this.hexBoxFindWhat.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexBoxFindWhat.LineInfoForeColor = System.Drawing.Color.Empty;
      this.hexBoxFindWhat.Location = new System.Drawing.Point(87, 3);
      this.hexBoxFindWhat.Name = "hexBoxFindWhat";
      this.hexBoxFindWhat.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
      this.hexBoxFindWhat.Size = new System.Drawing.Size(217, 37);
      this.hexBoxFindWhat.TabIndex = 0;
      this.hexBoxFindWhat.VScrollBarVisible = true;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(255, 158);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "Close";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonFindNext
      // 
      this.buttonFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonFindNext.Location = new System.Drawing.Point(174, 158);
      this.buttonFindNext.Name = "buttonFindNext";
      this.buttonFindNext.Size = new System.Drawing.Size(75, 23);
      this.buttonFindNext.TabIndex = 1;
      this.buttonFindNext.Text = "Find Next";
      this.buttonFindNext.UseVisualStyleBackColor = true;
      this.buttonFindNext.Click += new System.EventHandler(this.buttonFindNext_Click);
      // 
      // buttonReplace
      // 
      this.buttonReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonReplace.Location = new System.Drawing.Point(93, 158);
      this.buttonReplace.Name = "buttonReplace";
      this.buttonReplace.Size = new System.Drawing.Size(75, 23);
      this.buttonReplace.TabIndex = 2;
      this.buttonReplace.Text = "Replace";
      this.buttonReplace.UseVisualStyleBackColor = true;
      this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
      // 
      // buttonReplaceAll
      // 
      this.buttonReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonReplaceAll.Location = new System.Drawing.Point(12, 158);
      this.buttonReplaceAll.Name = "buttonReplaceAll";
      this.buttonReplaceAll.Size = new System.Drawing.Size(75, 23);
      this.buttonReplaceAll.TabIndex = 3;
      this.buttonReplaceAll.Text = "Replace All";
      this.buttonReplaceAll.UseVisualStyleBackColor = true;
      this.buttonReplaceAll.Click += new System.EventHandler(this.buttonReplaceAll_Click);
      // 
      // comboBoxSearchIn
      // 
      this.comboBoxSearchIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxSearchIn.FormattingEnabled = true;
      this.comboBoxSearchIn.Items.AddRange(new object[] {
            "This Node",
            "All Nodes"});
      this.comboBoxSearchIn.Location = new System.Drawing.Point(103, 131);
      this.comboBoxSearchIn.Name = "comboBoxSearchIn";
      this.comboBoxSearchIn.Size = new System.Drawing.Size(217, 21);
      this.comboBoxSearchIn.TabIndex = 6;
      this.comboBoxSearchIn.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchIn_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(25, 135);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Search In";
      // 
      // FormIFFEditorReplace
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(342, 193);
      this.ControlBox = false;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboBoxSearchIn);
      this.Controls.Add(this.buttonReplaceAll);
      this.Controls.Add(this.buttonReplace);
      this.Controls.Add(this.buttonFindNext);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.tabControl);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormIFFEditorReplace";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.Shown += new System.EventHandler(this.FormIFFEditorReplace_Shown);
      this.tabControl.ResumeLayout(false);
      this.tabPageString.ResumeLayout(false);
      this.tabPageString.PerformLayout();
      this.tabPageHexadecimal.ResumeLayout(false);
      this.tabPageHexadecimal.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelReplaceWithString;
    private System.Windows.Forms.Label labelFindWhatString;
    private System.Windows.Forms.TextBox textBoxReplaceWith;
    private System.Windows.Forms.TextBox textBoxFindWhat;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonFindNext;
    private Be.Windows.Forms.HexBox hexBoxReplaceWith;
    private Be.Windows.Forms.HexBox hexBoxFindWhat;
    private System.Windows.Forms.Label labelFindWhatHexadecimal;
    private System.Windows.Forms.Label labelReplaceWithHexadecimal;
    private System.Windows.Forms.Button buttonReplace;
    private System.Windows.Forms.Button buttonReplaceAll;
    private System.Windows.Forms.Label label1;
    internal System.Windows.Forms.ComboBox comboBoxSearchIn;
    internal System.Windows.Forms.TabControl tabControl;
    internal System.Windows.Forms.TabPage tabPageString;
    internal System.Windows.Forms.CheckBox checkBoxMatchCase;
    internal System.Windows.Forms.TabPage tabPageHexadecimal;
  }
}
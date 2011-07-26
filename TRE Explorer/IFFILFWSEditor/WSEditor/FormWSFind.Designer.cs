namespace TRE_Explorer {
  partial class FormWSFind {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWSFind));
      this.comboBoxItemNames = new System.Windows.Forms.ComboBox();
      this.listBoxResults = new System.Windows.Forms.ListBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonGoTo = new System.Windows.Forms.Button();
      this.buttonFind = new System.Windows.Forms.Button();
      this.comboBoxTypes = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // comboBoxItemNames
      // 
      this.comboBoxItemNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxItemNames.FormattingEnabled = true;
      this.comboBoxItemNames.Location = new System.Drawing.Point(139, 13);
      this.comboBoxItemNames.Name = "comboBoxItemNames";
      this.comboBoxItemNames.Size = new System.Drawing.Size(294, 21);
      this.comboBoxItemNames.TabIndex = 0;
      // 
      // listBoxResults
      // 
      this.listBoxResults.FormattingEnabled = true;
      this.listBoxResults.IntegralHeight = false;
      this.listBoxResults.Location = new System.Drawing.Point(12, 40);
      this.listBoxResults.Name = "listBoxResults";
      this.listBoxResults.Size = new System.Drawing.Size(450, 253);
      this.listBoxResults.TabIndex = 2;
      this.listBoxResults.SelectedIndexChanged += new System.EventHandler(this.listBoxResults_SelectedIndexChanged);
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(387, 299);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonGoTo
      // 
      this.buttonGoTo.Enabled = false;
      this.buttonGoTo.Location = new System.Drawing.Point(306, 299);
      this.buttonGoTo.Name = "buttonGoTo";
      this.buttonGoTo.Size = new System.Drawing.Size(75, 23);
      this.buttonGoTo.TabIndex = 4;
      this.buttonGoTo.Text = "Go To";
      this.buttonGoTo.UseVisualStyleBackColor = true;
      this.buttonGoTo.Click += new System.EventHandler(this.buttonGoTo_Click);
      // 
      // buttonFind
      // 
      this.buttonFind.Image = global::TRE_Explorer.Properties.Resources.Find;
      this.buttonFind.Location = new System.Drawing.Point(439, 12);
      this.buttonFind.Name = "buttonFind";
      this.buttonFind.Size = new System.Drawing.Size(23, 23);
      this.buttonFind.TabIndex = 1;
      this.buttonFind.UseVisualStyleBackColor = true;
      this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
      // 
      // comboBoxTypes
      // 
      this.comboBoxTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTypes.FormattingEnabled = true;
      this.comboBoxTypes.Location = new System.Drawing.Point(12, 13);
      this.comboBoxTypes.Name = "comboBoxTypes";
      this.comboBoxTypes.Size = new System.Drawing.Size(121, 21);
      this.comboBoxTypes.TabIndex = 5;
      this.comboBoxTypes.SelectedIndexChanged += new System.EventHandler(this.comboBoxTypes_SelectedIndexChanged);
      // 
      // FormWSFind
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new System.Drawing.Size(474, 334);
      this.ControlBox = false;
      this.Controls.Add(this.comboBoxTypes);
      this.Controls.Add(this.buttonGoTo);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.listBoxResults);
      this.Controls.Add(this.buttonFind);
      this.Controls.Add(this.comboBoxItemNames);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormWSFind";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBoxItemNames;
    private System.Windows.Forms.Button buttonFind;
    private System.Windows.Forms.ListBox listBoxResults;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonGoTo;
    private System.Windows.Forms.ComboBox comboBoxTypes;
  }
}
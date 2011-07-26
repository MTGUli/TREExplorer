namespace TRE_Explorer {
  partial class FormILFMatrixCalculator {
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormILFMatrixCalculator));
      this.trackBarZRoll = new System.Windows.Forms.TrackBar();
      this.trackBarYYaw = new System.Windows.Forms.TrackBar();
      this.trackBarXPitch = new System.Windows.Forms.TrackBar();
      this.panelRenderSurface = new System.Windows.Forms.Panel();
      this.buttonReset = new System.Windows.Forms.Button();
      this.labelRoll = new System.Windows.Forms.Label();
      this.labelYaw = new System.Windows.Forms.Label();
      this.labelPitch = new System.Windows.Forms.Label();
      this.radioButtonFront = new System.Windows.Forms.RadioButton();
      this.radioButtonBack = new System.Windows.Forms.RadioButton();
      this.radioButtonTop = new System.Windows.Forms.RadioButton();
      this.radioButtonBottom = new System.Windows.Forms.RadioButton();
      this.radioButtonRight = new System.Windows.Forms.RadioButton();
      this.radioButtonLeft = new System.Windows.Forms.RadioButton();
      this.labelMatrix = new System.Windows.Forms.Label();
      this.dataGridViewMatrix = new System.Windows.Forms.DataGridView();
      this.Columnx1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Columnx2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Columnx3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonZReset = new System.Windows.Forms.Button();
      this.buttonYReset = new System.Windows.Forms.Button();
      this.buttonXReset = new System.Windows.Forms.Button();
      this.buttonZMinus = new TRE_Explorer.RepeatButton();
      this.buttonZPlus = new TRE_Explorer.RepeatButton();
      this.buttonYMinus = new TRE_Explorer.RepeatButton();
      this.buttonYPlus = new TRE_Explorer.RepeatButton();
      this.buttonXMinus = new TRE_Explorer.RepeatButton();
      this.buttonXPlus = new TRE_Explorer.RepeatButton();
      this.numberBoxXPitch = new TRE_Explorer.NumberBox();
      this.numberBoxYYaw = new TRE_Explorer.NumberBox();
      this.numberBoxZRoll = new TRE_Explorer.NumberBox();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarZRoll)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarYYaw)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarXPitch)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).BeginInit();
      this.SuspendLayout();
      // 
      // trackBarZRoll
      // 
      this.trackBarZRoll.Location = new System.Drawing.Point(444, 153);
      this.trackBarZRoll.Maximum = 180;
      this.trackBarZRoll.Minimum = -180;
      this.trackBarZRoll.Name = "trackBarZRoll";
      this.trackBarZRoll.Size = new System.Drawing.Size(174, 45);
      this.trackBarZRoll.TabIndex = 9;
      this.trackBarZRoll.TickFrequency = 45;
      this.trackBarZRoll.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.trackBarZRoll.Scroll += new System.EventHandler(this.trackBar_Scroll);
      // 
      // trackBarYYaw
      // 
      this.trackBarYYaw.Location = new System.Drawing.Point(444, 89);
      this.trackBarYYaw.Maximum = 180;
      this.trackBarYYaw.Minimum = -180;
      this.trackBarYYaw.Name = "trackBarYYaw";
      this.trackBarYYaw.Size = new System.Drawing.Size(174, 45);
      this.trackBarYYaw.TabIndex = 6;
      this.trackBarYYaw.TickFrequency = 45;
      this.trackBarYYaw.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.trackBarYYaw.Scroll += new System.EventHandler(this.trackBar_Scroll);
      // 
      // trackBarXPitch
      // 
      this.trackBarXPitch.Location = new System.Drawing.Point(444, 25);
      this.trackBarXPitch.Maximum = 180;
      this.trackBarXPitch.Minimum = -180;
      this.trackBarXPitch.Name = "trackBarXPitch";
      this.trackBarXPitch.Size = new System.Drawing.Size(174, 45);
      this.trackBarXPitch.TabIndex = 3;
      this.trackBarXPitch.TickFrequency = 45;
      this.trackBarXPitch.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.trackBarXPitch.Scroll += new System.EventHandler(this.trackBar_Scroll);
      // 
      // panelRenderSurface
      // 
      this.panelRenderSurface.Location = new System.Drawing.Point(12, 12);
      this.panelRenderSurface.Name = "panelRenderSurface";
      this.panelRenderSurface.Size = new System.Drawing.Size(400, 300);
      this.panelRenderSurface.TabIndex = 24;
      // 
      // buttonReset
      // 
      this.buttonReset.Location = new System.Drawing.Point(444, 204);
      this.buttonReset.Name = "buttonReset";
      this.buttonReset.Size = new System.Drawing.Size(174, 23);
      this.buttonReset.TabIndex = 15;
      this.buttonReset.Text = "Reset All";
      this.buttonReset.UseVisualStyleBackColor = true;
      this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
      // 
      // labelRoll
      // 
      this.labelRoll.Location = new System.Drawing.Point(444, 137);
      this.labelRoll.Name = "labelRoll";
      this.labelRoll.Size = new System.Drawing.Size(87, 13);
      this.labelRoll.TabIndex = 27;
      this.labelRoll.Text = "Roll";
      this.labelRoll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelYaw
      // 
      this.labelYaw.Location = new System.Drawing.Point(444, 73);
      this.labelYaw.Name = "labelYaw";
      this.labelYaw.Size = new System.Drawing.Size(87, 13);
      this.labelYaw.TabIndex = 26;
      this.labelYaw.Text = "Yaw";
      this.labelYaw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelPitch
      // 
      this.labelPitch.Location = new System.Drawing.Point(444, 9);
      this.labelPitch.Name = "labelPitch";
      this.labelPitch.Size = new System.Drawing.Size(87, 13);
      this.labelPitch.TabIndex = 25;
      this.labelPitch.Text = "Pitch";
      this.labelPitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // radioButtonFront
      // 
      this.radioButtonFront.AutoSize = true;
      this.radioButtonFront.Checked = true;
      this.radioButtonFront.Cursor = System.Windows.Forms.Cursors.Default;
      this.radioButtonFront.Location = new System.Drawing.Point(12, 318);
      this.radioButtonFront.Name = "radioButtonFront";
      this.radioButtonFront.Size = new System.Drawing.Size(49, 17);
      this.radioButtonFront.TabIndex = 16;
      this.radioButtonFront.TabStop = true;
      this.radioButtonFront.Text = "Front";
      this.radioButtonFront.UseVisualStyleBackColor = true;
      this.radioButtonFront.CheckedChanged += new System.EventHandler(this.radioButtonFront_CheckedChanged);
      // 
      // radioButtonBack
      // 
      this.radioButtonBack.AutoSize = true;
      this.radioButtonBack.Location = new System.Drawing.Point(82, 318);
      this.radioButtonBack.Name = "radioButtonBack";
      this.radioButtonBack.Size = new System.Drawing.Size(50, 17);
      this.radioButtonBack.TabIndex = 17;
      this.radioButtonBack.Text = "Back";
      this.radioButtonBack.UseVisualStyleBackColor = true;
      this.radioButtonBack.CheckedChanged += new System.EventHandler(this.radioButtonBack_CheckedChanged);
      // 
      // radioButtonTop
      // 
      this.radioButtonTop.AutoSize = true;
      this.radioButtonTop.Location = new System.Drawing.Point(153, 318);
      this.radioButtonTop.Name = "radioButtonTop";
      this.radioButtonTop.Size = new System.Drawing.Size(44, 17);
      this.radioButtonTop.TabIndex = 18;
      this.radioButtonTop.Text = "Top";
      this.radioButtonTop.UseVisualStyleBackColor = true;
      this.radioButtonTop.CheckedChanged += new System.EventHandler(this.radioButtonTop_CheckedChanged);
      // 
      // radioButtonBottom
      // 
      this.radioButtonBottom.AutoSize = true;
      this.radioButtonBottom.Location = new System.Drawing.Point(218, 318);
      this.radioButtonBottom.Name = "radioButtonBottom";
      this.radioButtonBottom.Size = new System.Drawing.Size(58, 17);
      this.radioButtonBottom.TabIndex = 19;
      this.radioButtonBottom.Text = "Bottom";
      this.radioButtonBottom.UseVisualStyleBackColor = true;
      this.radioButtonBottom.CheckedChanged += new System.EventHandler(this.radioButtonBottom_CheckedChanged);
      // 
      // radioButtonRight
      // 
      this.radioButtonRight.AutoSize = true;
      this.radioButtonRight.Location = new System.Drawing.Point(297, 318);
      this.radioButtonRight.Name = "radioButtonRight";
      this.radioButtonRight.Size = new System.Drawing.Size(50, 17);
      this.radioButtonRight.TabIndex = 20;
      this.radioButtonRight.Text = "Right";
      this.radioButtonRight.UseVisualStyleBackColor = true;
      this.radioButtonRight.CheckedChanged += new System.EventHandler(this.radioButtonRight_CheckedChanged);
      // 
      // radioButtonLeft
      // 
      this.radioButtonLeft.AutoSize = true;
      this.radioButtonLeft.Location = new System.Drawing.Point(368, 318);
      this.radioButtonLeft.Name = "radioButtonLeft";
      this.radioButtonLeft.Size = new System.Drawing.Size(43, 17);
      this.radioButtonLeft.TabIndex = 21;
      this.radioButtonLeft.Text = "Left";
      this.radioButtonLeft.UseVisualStyleBackColor = true;
      this.radioButtonLeft.CheckedChanged += new System.EventHandler(this.radioButtonLeft_CheckedChanged);
      // 
      // labelMatrix
      // 
      this.labelMatrix.Location = new System.Drawing.Point(418, 230);
      this.labelMatrix.Name = "labelMatrix";
      this.labelMatrix.Size = new System.Drawing.Size(226, 13);
      this.labelMatrix.TabIndex = 28;
      this.labelMatrix.Text = "Rotation Matrix";
      this.labelMatrix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // dataGridViewMatrix
      // 
      this.dataGridViewMatrix.AllowUserToAddRows = false;
      this.dataGridViewMatrix.AllowUserToDeleteRows = false;
      this.dataGridViewMatrix.AllowUserToResizeColumns = false;
      this.dataGridViewMatrix.AllowUserToResizeRows = false;
      this.dataGridViewMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridViewMatrix.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.dataGridViewMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMatrix.ColumnHeadersVisible = false;
      this.dataGridViewMatrix.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Columnx1,
            this.Columnx2,
            this.Columnx3});
      this.dataGridViewMatrix.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dataGridViewMatrix.Location = new System.Drawing.Point(418, 246);
      this.dataGridViewMatrix.Name = "dataGridViewMatrix";
      this.dataGridViewMatrix.ReadOnly = true;
      this.dataGridViewMatrix.RowHeadersVisible = false;
      this.dataGridViewMatrix.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
      this.dataGridViewMatrix.RowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewMatrix.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      this.dataGridViewMatrix.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.dataGridViewMatrix.RowTemplate.DefaultCellStyle.Format = "F3";
      this.dataGridViewMatrix.RowTemplate.DefaultCellStyle.NullValue = "0";
      this.dataGridViewMatrix.Size = new System.Drawing.Size(226, 63);
      this.dataGridViewMatrix.TabIndex = 29;
      this.dataGridViewMatrix.TabStop = false;
      // 
      // Columnx1
      // 
      this.Columnx1.HeaderText = "x1";
      this.Columnx1.Name = "Columnx1";
      this.Columnx1.ReadOnly = true;
      // 
      // Columnx2
      // 
      this.Columnx2.HeaderText = "x2";
      this.Columnx2.Name = "Columnx2";
      this.Columnx2.ReadOnly = true;
      // 
      // Columnx3
      // 
      this.Columnx3.HeaderText = "x3";
      this.Columnx3.Name = "Columnx3";
      this.Columnx3.ReadOnly = true;
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(569, 315);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 22;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(488, 315);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 23;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonZReset
      // 
      this.buttonZReset.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.buttonZReset.Location = new System.Drawing.Point(418, 165);
      this.buttonZReset.Name = "buttonZReset";
      this.buttonZReset.Size = new System.Drawing.Size(20, 20);
      this.buttonZReset.TabIndex = 14;
      this.buttonZReset.Text = "q";
      this.buttonZReset.UseVisualStyleBackColor = true;
      this.buttonZReset.Click += new System.EventHandler(this.buttonZReset_Click);
      // 
      // buttonYReset
      // 
      this.buttonYReset.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.buttonYReset.Location = new System.Drawing.Point(418, 101);
      this.buttonYReset.Name = "buttonYReset";
      this.buttonYReset.Size = new System.Drawing.Size(20, 20);
      this.buttonYReset.TabIndex = 13;
      this.buttonYReset.Text = "q";
      this.buttonYReset.UseVisualStyleBackColor = true;
      this.buttonYReset.Click += new System.EventHandler(this.buttonYReset_Click);
      // 
      // buttonXReset
      // 
      this.buttonXReset.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.buttonXReset.Location = new System.Drawing.Point(418, 37);
      this.buttonXReset.Name = "buttonXReset";
      this.buttonXReset.Size = new System.Drawing.Size(20, 20);
      this.buttonXReset.TabIndex = 12;
      this.buttonXReset.Text = "q";
      this.buttonXReset.UseVisualStyleBackColor = true;
      this.buttonXReset.Click += new System.EventHandler(this.buttonXReset_Click);
      // 
      // buttonZMinus
      // 
      this.buttonZMinus.ButtonDelay = 500;
      this.buttonZMinus.ButtonSpeed = 34;
      this.buttonZMinus.Location = new System.Drawing.Point(624, 178);
      this.buttonZMinus.Name = "buttonZMinus";
      this.buttonZMinus.Size = new System.Drawing.Size(20, 20);
      this.buttonZMinus.TabIndex = 11;
      this.buttonZMinus.Text = "▼";
      this.buttonZMinus.UseVisualStyleBackColor = true;
      this.buttonZMinus.Click += new System.EventHandler(this.buttonZMinus_Click);
      // 
      // buttonZPlus
      // 
      this.buttonZPlus.ButtonDelay = 500;
      this.buttonZPlus.ButtonSpeed = 34;
      this.buttonZPlus.Location = new System.Drawing.Point(624, 153);
      this.buttonZPlus.Name = "buttonZPlus";
      this.buttonZPlus.Size = new System.Drawing.Size(20, 20);
      this.buttonZPlus.TabIndex = 10;
      this.buttonZPlus.Text = "▲";
      this.buttonZPlus.UseVisualStyleBackColor = true;
      this.buttonZPlus.Click += new System.EventHandler(this.buttonZPlus_Click);
      // 
      // buttonYMinus
      // 
      this.buttonYMinus.ButtonDelay = 500;
      this.buttonYMinus.ButtonSpeed = 34;
      this.buttonYMinus.Location = new System.Drawing.Point(624, 114);
      this.buttonYMinus.Name = "buttonYMinus";
      this.buttonYMinus.Size = new System.Drawing.Size(20, 20);
      this.buttonYMinus.TabIndex = 8;
      this.buttonYMinus.Text = "▼";
      this.buttonYMinus.UseVisualStyleBackColor = true;
      this.buttonYMinus.Click += new System.EventHandler(this.buttonYMinus_Click);
      // 
      // buttonYPlus
      // 
      this.buttonYPlus.ButtonDelay = 500;
      this.buttonYPlus.ButtonSpeed = 34;
      this.buttonYPlus.Location = new System.Drawing.Point(624, 89);
      this.buttonYPlus.Name = "buttonYPlus";
      this.buttonYPlus.Size = new System.Drawing.Size(20, 20);
      this.buttonYPlus.TabIndex = 7;
      this.buttonYPlus.Text = "▲";
      this.buttonYPlus.UseVisualStyleBackColor = true;
      this.buttonYPlus.Click += new System.EventHandler(this.buttonYPlus_Click);
      // 
      // buttonXMinus
      // 
      this.buttonXMinus.ButtonDelay = 500;
      this.buttonXMinus.ButtonSpeed = 34;
      this.buttonXMinus.Location = new System.Drawing.Point(624, 50);
      this.buttonXMinus.Name = "buttonXMinus";
      this.buttonXMinus.Size = new System.Drawing.Size(20, 20);
      this.buttonXMinus.TabIndex = 5;
      this.buttonXMinus.Text = "▼";
      this.buttonXMinus.UseVisualStyleBackColor = true;
      this.buttonXMinus.Click += new System.EventHandler(this.buttonXMinus_Click);
      // 
      // buttonXPlus
      // 
      this.buttonXPlus.ButtonDelay = 500;
      this.buttonXPlus.ButtonSpeed = 34;
      this.buttonXPlus.Location = new System.Drawing.Point(624, 25);
      this.buttonXPlus.Name = "buttonXPlus";
      this.buttonXPlus.Size = new System.Drawing.Size(20, 20);
      this.buttonXPlus.TabIndex = 4;
      this.buttonXPlus.Text = "▲";
      this.buttonXPlus.UseVisualStyleBackColor = true;
      this.buttonXPlus.Click += new System.EventHandler(this.buttonXPlus_Click);
      // 
      // numberBoxXPitch
      // 
      this.numberBoxXPitch.Location = new System.Drawing.Point(531, 5);
      this.numberBoxXPitch.MaxLength = 4;
      this.numberBoxXPitch.Name = "numberBoxXPitch";
      this.numberBoxXPitch.Size = new System.Drawing.Size(87, 20);
      this.numberBoxXPitch.TabIndex = 0;
      this.numberBoxXPitch.Text = "0";
      this.numberBoxXPitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numberBoxXPitch.Value = 0;
      this.numberBoxXPitch.TextChanged += new System.EventHandler(this.numberBoxXPitch_TextChanged);
      // 
      // numberBoxYYaw
      // 
      this.numberBoxYYaw.Location = new System.Drawing.Point(531, 69);
      this.numberBoxYYaw.MaxLength = 4;
      this.numberBoxYYaw.Name = "numberBoxYYaw";
      this.numberBoxYYaw.Size = new System.Drawing.Size(87, 20);
      this.numberBoxYYaw.TabIndex = 1;
      this.numberBoxYYaw.Text = "0";
      this.numberBoxYYaw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numberBoxYYaw.Value = 0;
      this.numberBoxYYaw.TextChanged += new System.EventHandler(this.numberBoxYYaw_TextChanged);
      // 
      // numberBoxZRoll
      // 
      this.numberBoxZRoll.Location = new System.Drawing.Point(531, 133);
      this.numberBoxZRoll.MaxLength = 4;
      this.numberBoxZRoll.Name = "numberBoxZRoll";
      this.numberBoxZRoll.Size = new System.Drawing.Size(87, 20);
      this.numberBoxZRoll.TabIndex = 2;
      this.numberBoxZRoll.Text = "0";
      this.numberBoxZRoll.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numberBoxZRoll.Value = 0;
      this.numberBoxZRoll.TextChanged += new System.EventHandler(this.numberBoxZRoll_TextChanged);
      // 
      // FormILFMatrixCalculator
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(656, 350);
      this.ControlBox = false;
      this.Controls.Add(this.numberBoxZRoll);
      this.Controls.Add(this.numberBoxYYaw);
      this.Controls.Add(this.numberBoxXPitch);
      this.Controls.Add(this.buttonXReset);
      this.Controls.Add(this.buttonYReset);
      this.Controls.Add(this.buttonZReset);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.labelMatrix);
      this.Controls.Add(this.dataGridViewMatrix);
      this.Controls.Add(this.radioButtonLeft);
      this.Controls.Add(this.radioButtonRight);
      this.Controls.Add(this.radioButtonBottom);
      this.Controls.Add(this.radioButtonTop);
      this.Controls.Add(this.radioButtonBack);
      this.Controls.Add(this.radioButtonFront);
      this.Controls.Add(this.buttonZMinus);
      this.Controls.Add(this.buttonZPlus);
      this.Controls.Add(this.buttonYMinus);
      this.Controls.Add(this.buttonYPlus);
      this.Controls.Add(this.buttonXMinus);
      this.Controls.Add(this.buttonXPlus);
      this.Controls.Add(this.labelPitch);
      this.Controls.Add(this.labelYaw);
      this.Controls.Add(this.labelRoll);
      this.Controls.Add(this.buttonReset);
      this.Controls.Add(this.trackBarZRoll);
      this.Controls.Add(this.trackBarYYaw);
      this.Controls.Add(this.trackBarXPitch);
      this.Controls.Add(this.panelRenderSurface);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormILFMatrixCalculator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "TRE Explorer";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormILFMatrixCalculator_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.trackBarZRoll)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarYYaw)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarXPitch)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TrackBar trackBarZRoll;
    private System.Windows.Forms.TrackBar trackBarYYaw;
    private System.Windows.Forms.TrackBar trackBarXPitch;
    private System.Windows.Forms.Panel panelRenderSurface;
    private System.Windows.Forms.Button buttonReset;
    private System.Windows.Forms.Label labelRoll;
    private System.Windows.Forms.Label labelYaw;
    private System.Windows.Forms.Label labelPitch;
    private TRE_Explorer.RepeatButton buttonXPlus;
    private TRE_Explorer.RepeatButton buttonXMinus;
    private TRE_Explorer.RepeatButton buttonYPlus;
    private TRE_Explorer.RepeatButton buttonYMinus;
    private TRE_Explorer.RepeatButton buttonZPlus;
    private TRE_Explorer.RepeatButton buttonZMinus;
    private System.Windows.Forms.RadioButton radioButtonFront;
    private System.Windows.Forms.RadioButton radioButtonBack;
    private System.Windows.Forms.RadioButton radioButtonTop;
    private System.Windows.Forms.RadioButton radioButtonBottom;
    private System.Windows.Forms.RadioButton radioButtonRight;
    private System.Windows.Forms.RadioButton radioButtonLeft;
    private System.Windows.Forms.Label labelMatrix;
    private System.Windows.Forms.DataGridView dataGridViewMatrix;
    private System.Windows.Forms.DataGridViewTextBoxColumn Columnx1;
    private System.Windows.Forms.DataGridViewTextBoxColumn Columnx2;
    private System.Windows.Forms.DataGridViewTextBoxColumn Columnx3;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonZReset;
    private System.Windows.Forms.Button buttonYReset;
    private System.Windows.Forms.Button buttonXReset;
    private NumberBox numberBoxXPitch;
    private NumberBox numberBoxYYaw;
    private NumberBox numberBoxZRoll;
  }
}


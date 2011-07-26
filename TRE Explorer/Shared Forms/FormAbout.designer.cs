namespace TRE_Explorer {
    partial class FormAbout {
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
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
          this.dragonHeadPicture = new System.Windows.Forms.PictureBox();
          this.labelText = new System.Windows.Forms.Label();
          this.textBoxAcknowledgements = new System.Windows.Forms.TextBox();
          ((System.ComponentModel.ISupportInitialize)(this.dragonHeadPicture)).BeginInit();
          this.SuspendLayout();
          // 
          // dragonHeadPicture
          // 
          this.dragonHeadPicture.Image = ((System.Drawing.Image)(resources.GetObject("dragonHeadPicture.Image")));
          this.dragonHeadPicture.Location = new System.Drawing.Point(101, 12);
          this.dragonHeadPicture.Name = "dragonHeadPicture";
          this.dragonHeadPicture.Size = new System.Drawing.Size(90, 90);
          this.dragonHeadPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
          this.dragonHeadPicture.TabIndex = 0;
          this.dragonHeadPicture.TabStop = false;
          this.dragonHeadPicture.Click += new System.EventHandler(this.FormAbout_Click);
          // 
          // labelText
          // 
          this.labelText.Location = new System.Drawing.Point(12, 105);
          this.labelText.Name = "labelText";
          this.labelText.Size = new System.Drawing.Size(268, 41);
          this.labelText.TabIndex = 1;
          this.labelText.Text = "TRE Explorer 1.0.0.0 for use with Star Wars Galaxies\r\n\r\nCoded by Melva I\'Tah of M" +
              "afia (Gorath)";
          this.labelText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
          this.labelText.Click += new System.EventHandler(this.FormAbout_Click);
          // 
          // textBoxAcknowledgements
          // 
          this.textBoxAcknowledgements.BackColor = System.Drawing.SystemColors.Control;
          this.textBoxAcknowledgements.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.textBoxAcknowledgements.Location = new System.Drawing.Point(15, 149);
          this.textBoxAcknowledgements.Multiline = true;
          this.textBoxAcknowledgements.Name = "textBoxAcknowledgements";
          this.textBoxAcknowledgements.ReadOnly = true;
          this.textBoxAcknowledgements.ScrollBars = System.Windows.Forms.ScrollBars.Both;
          this.textBoxAcknowledgements.Size = new System.Drawing.Size(265, 105);
          this.textBoxAcknowledgements.TabIndex = 2;
          this.textBoxAcknowledgements.Text = resources.GetString("textBoxAcknowledgements.Text");
          // 
          // FormAbout
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.AutoSize = true;
          this.ClientSize = new System.Drawing.Size(292, 266);
          this.Controls.Add(this.textBoxAcknowledgements);
          this.Controls.Add(this.labelText);
          this.Controls.Add(this.dragonHeadPicture);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "FormAbout";
          this.ShowIcon = false;
          this.ShowInTaskbar = false;
          this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
          this.Text = "TRE Explorer";
          this.Shown += new System.EventHandler(this.FormAbout_Shown);
          this.Click += new System.EventHandler(this.FormAbout_Click);
          ((System.ComponentModel.ISupportInitialize)(this.dragonHeadPicture)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox dragonHeadPicture;
        private System.Windows.Forms.Label labelText;
      private System.Windows.Forms.TextBox textBoxAcknowledgements;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TRE_Explorer {
  public partial class FormAbout : Form {
    public FormAbout() {
      InitializeComponent();
    }

    private void FormAbout_Shown(object sender, EventArgs e) {
      this.labelText.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " for use with Star Wars Galaxies\r\n\r\nCoded by Melva I'Tah of Mafia (Gorath)";
      this.textBoxAcknowledgements.Select(0, 0);
    }

    private void FormAbout_Click(object sender, EventArgs e) {
      this.Close();
    }
  }
}
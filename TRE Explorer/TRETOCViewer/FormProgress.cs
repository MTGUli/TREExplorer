using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TRE_Explorer {
  public partial class FormProgress : Form {
    private FormTOCTREViewer m_Form;

    public FormProgress(FormTOCTREViewer form) {
      InitializeComponent();

      this.m_Form = form;
    }

    private void buttonCancel_Click(object sender, EventArgs e) {
      this.m_Form.Invoke(this.m_Form.delegateStopThread);
    }

    private void FormProgress_VisibleChanged(object sender, EventArgs e) {
      if (!this.Visible) {
        this.m_Form.Invoke(this.m_Form.delegateSetProgress, new Object[] { new Int32[] { 0 } });
      }
    }

    private void flowLayoutPanel_Resize(object sender, EventArgs e) {
      this.Height -= this.ClientSize.Height - (this.flowLayoutPanel.Top + this.flowLayoutPanel.Height);
    }
  }
}
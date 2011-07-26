using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TRE_Explorer {
  public partial class FormILFRenderSurface : Form {
    public FormILFRenderSurface() {
      InitializeComponent();
      this.TopLevel = false;
    }

    protected override void OnPaint(PaintEventArgs e) {
      ((FormILFMatrixCalculator)this.Parent.FindForm()).Render();
    }

    protected override void OnPaintBackground(PaintEventArgs e) {
      // Do nothing
    }
  }
}

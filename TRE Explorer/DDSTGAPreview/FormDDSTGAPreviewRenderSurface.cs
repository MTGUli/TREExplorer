using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TRE_Explorer {
  public partial class FormDDSTGAPreviewRenderSurface : Form {
    private Single m_ZoomFactor = 1F;
    private FormDDSTGARenderFrame m_FormRenderFrame;

    public Single ZoomFactor {
      get {
        return this.m_ZoomFactor;
      }
      set {
        Single zoomFactor = Math.Min(Math.Max(value, 0.125F), 32F);
        this.m_ZoomFactor = zoomFactor;
        String zoomFactorString = String.Format("{0:n" + ((zoomFactor < 1F) ? 1 : 0) + "}", (this.m_ZoomFactor * 100));
        this.m_FormRenderFrame.toolStripTextBoxZoomFactor.Text = ((zoomFactorString.EndsWith(".0")) ? zoomFactorString.Substring(0, zoomFactorString.LastIndexOf(".")) : zoomFactorString) + "%";
        this.m_FormRenderFrame.toolStripTextBoxZoomFactor.Invalidate();
        this.Invalidate();
      }
    }

    public FormDDSTGAPreviewRenderSurface(FormDDSTGARenderFrame Parent) {
      InitializeComponent();
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
      this.FormBorderStyle = FormBorderStyle.None;
      this.TopLevel = false;
      this.m_FormRenderFrame = Parent;
    }

    protected override void OnPaint(PaintEventArgs pe) {
      if ((this.m_FormRenderFrame.Device != null) && (this.m_FormRenderFrame.Texture != null) && (this.Visible) && (this.WindowState != FormWindowState.Minimized)) {
        try {
          this.m_FormRenderFrame.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1F, 0);

          SurfaceDescription surfaceDescription = this.m_FormRenderFrame.Texture.GetLevelDescription(0);
          Size size = new Size(surfaceDescription.Width, surfaceDescription.Height);

          Size targetSize = new Size(Math.Max(1, (Int32)((Single)size.Width * this.m_ZoomFactor)), Math.Max(1, (Int32)((Single)size.Height * this.m_ZoomFactor)));
          if (this.Size != targetSize) {
            this.Size = targetSize;
          }

          this.m_FormRenderFrame.Device.BeginScene();

          Sprite sprite = new Sprite(this.m_FormRenderFrame.Device);
          sprite.Begin(SpriteFlags.None);
          sprite.Draw2D(this.m_FormRenderFrame.Texture, new Rectangle(new Point(0, 0), size), new SizeF(((Single)size.Width * this.m_ZoomFactor), ((Single)size.Height * this.m_ZoomFactor)), new PointF(0F, 0F), Color.FromArgb(255, 255, 255, 255));
          sprite.End();
          sprite.Dispose();

          this.m_FormRenderFrame.Device.EndScene();
          this.m_FormRenderFrame.Device.Present();
        } catch {
        }
      }
    }
  }
}

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
  public partial class FormDDSTGARenderFrame : Form {
    private FormDDSTGAPreviewRenderSurface m_FormRenderSurface;
    private Device m_Device;
    private Texture m_Texture;
    private String m_Filename;
    private FormNotifyIcon m_FormNotifyIcon;

    public Device Device {
      get {
        return this.m_Device;
      }
    }
    public Texture Texture {
      get {
        return this.m_Texture;
      }
    }

    public FormDDSTGARenderFrame() {
      InitializeComponent();
    }

    public FormDDSTGARenderFrame(String Filename, FormNotifyIcon formNotifyIcon) {
      InitializeComponent();

      this.m_Filename = Filename;
      this.m_FormNotifyIcon = formNotifyIcon;

      InitializeD3D();
    }

    private void InitializeD3D() {
      if (this.m_Filename != null) {
        try {
          this.m_FormRenderSurface = new FormDDSTGAPreviewRenderSurface(this);
          this.m_FormRenderSurface.Anchor = AnchorStyles.Top | AnchorStyles.Left;
          this.m_FormRenderSurface.Location = new Point(0, 0);
          this.m_FormRenderSurface.Size = new Size(1, 1);
          this.m_FormRenderSurface.Visible = true;
          this.panelRenderSurfaceContainer.Controls.Add(this.m_FormRenderSurface);

          PresentParameters presentParameters = new PresentParameters();
          presentParameters.Windowed = true;
          presentParameters.SwapEffect = SwapEffect.Discard;
          presentParameters.BackBufferFormat = Format.Unknown;
          presentParameters.AutoDepthStencilFormat = DepthFormat.D16;
          presentParameters.EnableAutoDepthStencil = true;

          DeviceCaps deviceCaps = Manager.GetDeviceCaps(Manager.Adapters.Default.Adapter, DeviceType.Hardware).DeviceCaps;
          CreateFlags createFlags = ((deviceCaps.SupportsHardwareTransformAndLight) ? CreateFlags.HardwareVertexProcessing : CreateFlags.SoftwareVertexProcessing);
          if (deviceCaps.SupportsPureDevice) {
            createFlags |= CreateFlags.PureDevice;
          }

          if (this.m_Device != null) {
            this.m_Device.Dispose();
          }

          this.m_Device = new Device(0, DeviceType.Hardware, this.m_FormRenderSurface, createFlags, presentParameters);
          this.m_Device.DeviceReset += new EventHandler(this.OnResetDevice);
          this.m_Device.DeviceLost += new EventHandler(this.OnDeviceLost);
          this.m_Device.DeviceResizing += new CancelEventHandler(this.OnDeviceResizing);
          OnResetDevice(this.m_Device, null);

          SetUpViews();
        } catch {
        }
      }
    }

    public void OnDeviceResizing(object sender, CancelEventArgs e) {
    }

    public void OnDeviceLost(object sender, EventArgs e) {
    }

    public void OnResetDevice(object sender, EventArgs e) {
      try {
        Device device = (Device)sender;
        device.RenderState.CullMode = Cull.None;
        device.RenderState.Lighting = false;

        LoadTexture(this.m_Filename);
        SetUpViews();
      } catch {
      }
    }

    private void LoadTexture(String Filename) {
      try {
        if (this.m_Texture != null) {
          this.m_Texture.Dispose();
        }
        this.m_Texture = TextureLoader.FromFile(this.m_Device, Filename);
        this.m_Filename = Filename;
      } catch {
      }
    }

    private void SetUpViews() {
      try {
        this.m_Device.Transform.Projection = Matrix.PerspectiveFovLH((Single)Math.PI / 4, this.m_FormRenderSurface.Width / this.m_FormRenderSurface.Height, 0.1F, 100F);
        this.m_Device.RenderState.CullMode = Cull.None;
        this.m_Device.RenderState.Lighting = false;
      } catch {
      }
    }

    internal void toolStripButtonSaveAs_Click(object sender, EventArgs e) {
      if (this.m_Texture != null) {
        this.m_FormNotifyIcon.saveFileDialog.FileName = this.m_Filename.Substring(this.m_Filename.LastIndexOf("\\") + 1);
        this.m_FormNotifyIcon.saveFileDialog.FileName = this.m_FormNotifyIcon.saveFileDialog.FileName.Substring(0, this.m_FormNotifyIcon.saveFileDialog.FileName.LastIndexOf("."));
        this.m_FormNotifyIcon.saveFileDialog.Filter = "BMP Files|*.bmp|JPG Files|*.jpg;*.jpeg|PNG Files|*.png|TGA Files|*.tga";
        this.m_FormNotifyIcon.saveFileDialog.FilterIndex = 3;
        if (this.m_FormNotifyIcon.saveFileDialog.ShowDialog() == DialogResult.OK) {
          ImageFileFormat imageFileFormat;
          switch (this.m_FormNotifyIcon.saveFileDialog.FileName.Substring(this.m_FormNotifyIcon.saveFileDialog.FileName.LastIndexOf(".") + 1).ToLower()) {
            case "bmp":
              imageFileFormat = ImageFileFormat.Bmp;
              break;

            case "jpeg":
            case "jpg":
              imageFileFormat = ImageFileFormat.Jpg;
              break;

            case "png":
              imageFileFormat = ImageFileFormat.Png;
              break;

            case "tga":
              imageFileFormat = ImageFileFormat.Tga;
              break;

            default:
              imageFileFormat = ImageFileFormat.Png;
              break;
          }
          try {
            TextureLoader.Save(this.m_FormNotifyIcon.saveFileDialog.FileName, imageFileFormat, this.m_Texture);
          } catch {
            MessageBox.Show("An error occurred while saving the texture.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }

    private void toolStripButtonZoomIn_Click(object sender, EventArgs e) {
      if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
        ((Panel)this.Parent).AutoScroll = false;
      }
      if (this.m_FormRenderSurface.ZoomFactor < 0.125F) {
        this.m_FormRenderSurface.ZoomFactor = 0.125F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 0.167F) {
        this.m_FormRenderSurface.ZoomFactor = 0.167F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 0.25F) {
        this.m_FormRenderSurface.ZoomFactor = 0.25F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 0.333F) {
        this.m_FormRenderSurface.ZoomFactor = 0.333F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 0.5F) {
        this.m_FormRenderSurface.ZoomFactor = 0.5F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 0.667F) {
        this.m_FormRenderSurface.ZoomFactor = 0.667F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 1F) {
        this.m_FormRenderSurface.ZoomFactor = 1F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 2F) {
        this.m_FormRenderSurface.ZoomFactor = 2F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 3F) {
        this.m_FormRenderSurface.ZoomFactor = 3F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 4F) {
        this.m_FormRenderSurface.ZoomFactor = 4F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 5F) {
        this.m_FormRenderSurface.ZoomFactor = 5F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 6F) {
        this.m_FormRenderSurface.ZoomFactor = 6F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 7F) {
        this.m_FormRenderSurface.ZoomFactor = 7F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 8F) {
        this.m_FormRenderSurface.ZoomFactor = 8F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 12F) {
        this.m_FormRenderSurface.ZoomFactor = 12F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 16F) {
        this.m_FormRenderSurface.ZoomFactor = 16F;
      } else if (this.m_FormRenderSurface.ZoomFactor < 32F) {
        this.m_FormRenderSurface.ZoomFactor = 32F;
      }
      if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
        ((Panel)this.Parent).AutoScroll = true;
      }
    }

    private void toolStripButtonZoomOut_Click(object sender, EventArgs e) {
      if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
        ((Panel)this.Parent).AutoScroll = false;
      }
      if (this.m_FormRenderSurface.ZoomFactor > 32F) {
        this.m_FormRenderSurface.ZoomFactor = 32F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 16F) {
        this.m_FormRenderSurface.ZoomFactor = 16F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 12F) {
        this.m_FormRenderSurface.ZoomFactor = 12F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 8F) {
        this.m_FormRenderSurface.ZoomFactor = 8F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 7F) {
        this.m_FormRenderSurface.ZoomFactor = 7F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 6F) {
        this.m_FormRenderSurface.ZoomFactor = 6F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 5F) {
        this.m_FormRenderSurface.ZoomFactor = 5F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 4F) {
        this.m_FormRenderSurface.ZoomFactor = 4F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 3F) {
        this.m_FormRenderSurface.ZoomFactor = 3F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 2F) {
        this.m_FormRenderSurface.ZoomFactor = 2F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 1F) {
        this.m_FormRenderSurface.ZoomFactor = 1F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 0.667F) {
        this.m_FormRenderSurface.ZoomFactor = 0.667F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 0.5F) {
        this.m_FormRenderSurface.ZoomFactor = 0.5F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 0.333F) {
        this.m_FormRenderSurface.ZoomFactor = 0.333F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 0.25F) {
        this.m_FormRenderSurface.ZoomFactor = 0.25F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 0.167F) {
        this.m_FormRenderSurface.ZoomFactor = 0.167F;
      } else if (this.m_FormRenderSurface.ZoomFactor > 0.125F) {
        this.m_FormRenderSurface.ZoomFactor = 0.125F;
      }
      if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
        ((Panel)this.Parent).AutoScroll = true;
      }
    }

    private void toolStripButtonZoomFull_Click(object sender, EventArgs e) {
      if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
        ((Panel)this.Parent).AutoScroll = false;
      }
      this.m_FormRenderSurface.ZoomFactor = 1F;
      if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
        ((Panel)this.Parent).AutoScroll = true;
      }
    }

    private void toolStripButtonZoomFit_Click(object sender, EventArgs e) {
      if (this.m_Texture != null) {
        if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
          ((Panel)this.Parent).AutoScroll = false;
        }
        SurfaceDescription surfaceDescription = this.m_Texture.GetLevelDescription(0);
        Size textureSize = new Size(surfaceDescription.Width, surfaceDescription.Height);
        Size panelSize = new Size(this.panelRenderSurfaceContainer.Size.Width - 1, this.panelRenderSurfaceContainer.Size.Height - 1);
        Single widthFactor = (Single)panelSize.Width / (Single)textureSize.Width;
        Single heightFactor = (Single)panelSize.Height / (Single)textureSize.Height;
        this.m_FormRenderSurface.ZoomFactor = (1F / (Single)Math.Max(textureSize.Width, textureSize.Height));
        this.m_FormRenderSurface.ZoomFactor = Math.Min(widthFactor, heightFactor);
        if ((this.Parent != null) && (this.Parent.GetType() == typeof(Panel))) {
          ((Panel)this.Parent).AutoScroll = true;
        }
      }
    }

    private void FormRenderFrame_Shown(object sender, EventArgs e) {
      if (this.m_Texture != null) {
        SurfaceDescription surfaceDescription = this.m_Texture.GetLevelDescription(0);
        if ((surfaceDescription.Width > this.Width) || (surfaceDescription.Height > this.Height)) {
          this.toolStripButtonZoomFit_Click(this.toolStripButtonZoomFit, new EventArgs());
        }
      }
    }
  }
}

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
using SWGLib;

namespace TRE_Explorer {
  public partial class FormILFMatrixCalculator : Form {
    #region Enumerations
    private enum ViewAngle { Top, Bottom, Front, Back, Right, Left };
    #endregion

    #region Global variables
    private Int32 m_OriginalXPitch = 0;
    private Int32 m_OriginalYYaw = 0;
    private Int32 m_OriginalZRoll = 0;

    private FormILFRenderSurface m_FormRenderSurface;
    private Device m_Device;
    private Mesh m_Mesh;
    private Material[] m_Material;
    private Texture[] m_Texture;
    private Single m_MeshRadius;
    private Vector3 m_MechCenter;
    private Boolean m_Paused;
    private ViewAngle m_ViewAngle = ViewAngle.Front;
    private const Double m_Threshold = 0.998;

    private RotationMatrix xPitchMatrix = new RotationMatrix();
    private RotationMatrix yYawMatrix = new RotationMatrix();
    private RotationMatrix zRollMatrix = new RotationMatrix();
    private RotationMatrix rotationMatrix = new RotationMatrix();

    private Single xPitch {
      get {
        return Geometry.DegreeToRadian((Int32)(-this.trackBarXPitch.Value));
      }
    }
    private Single yYaw {
      get {
        return Geometry.DegreeToRadian((Int32)(-this.trackBarYYaw.Value));
      }
    }
    private Single zRoll {
      get {
        return Geometry.DegreeToRadian((Int32)this.trackBarZRoll.Value);
      }
    }

    public RotationMatrix RotationMatrix {
      get {
        return this.rotationMatrix;
      }
    }
    #endregion

    #region Form Functions
    public FormILFMatrixCalculator() : this(new RotationMatrix(new EulerAngles(0, 0, 0))) { }

    public FormILFMatrixCalculator(RotationMatrix matrix) {
      this.m_Device = null;
      this.m_Mesh = null;
      this.m_Paused = false;

      InitializeComponent();

      this.m_FormRenderSurface = new FormILFRenderSurface();
      this.panelRenderSurface.Controls.Add(this.m_FormRenderSurface);
      this.m_FormRenderSurface.Visible = true;

      if (!this.InitializeGraphics()) {
        MessageBox.Show("Failed to initialize Direct3D.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      EulerAngles eulerAngles = matrix.ToEulerAngles();

      this.trackBarXPitch.Value = eulerAngles.xPitch;
      this.trackBarYYaw.Value = eulerAngles.yYaw;
      this.trackBarZRoll.Value = eulerAngles.zRoll;

      this.m_OriginalXPitch = eulerAngles.xPitch;
      this.m_OriginalYYaw = eulerAngles.yYaw;
      this.m_OriginalZRoll = eulerAngles.zRoll;

      this.UpdateMatrix();
    }

    private void FormILFMatrixCalculator_FormClosing(object sender, FormClosingEventArgs e) {
      if (this.m_Device != null) {
        this.m_Device.Dispose();
      }
      if ((this.m_Texture != null) && (this.m_Texture.Length > 0)) {
        foreach (Texture texture in this.m_Texture) {
          texture.Dispose();
        }
      }
    }
    #endregion

    #region Overridden Functions
    protected override void OnPaint(PaintEventArgs e) {
      this.Render();
    }

    protected override void OnResize(EventArgs e) {
      this.m_Paused = ((this.WindowState == FormWindowState.Minimized) || (!this.Visible));
    }
    #endregion

    #region Helper Functions
    private RotationMatrix DataGridViewToMatrix(DataGridView dataGridView) {
      RotationMatrix returnValue = new RotationMatrix();

      for (Int32 row = 0; row < 3; row++) {
        for (Int32 column = 0; column < 3; column++) {
          returnValue[row][column] = (Single)dataGridView.Rows[row].Cells[column].Value;
        }
      }

      return returnValue;
    }

    private void MatrixToDataGridView(RotationMatrix matrix, DataGridView dataGridView) {
      dataGridView.Rows.Clear();
      for (Int32 row = 0; row < 3; row++) {
        dataGridView.Rows.Add(new Object[] { matrix[row][0], matrix[row][1], matrix[row][2] });
      }
      dataGridView.SelectAll();
    }
    #endregion

    #region Initialize 3D Environment
    public Boolean InitializeGraphics() {
      try {
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
        this.OnResetDevice(this.m_Device, null);
        this.m_Paused = false;
        return true;
      } catch (DirectXException) {
        return false;
      }
    }

    public void OnResetDevice(object sender, EventArgs e) {
      Device device = (Device)sender;
      device.RenderState.Clipping = true;
      device.RenderState.CullMode = Cull.CounterClockwise;
      device.RenderState.AlphaBlendEnable = true;
      device.RenderState.Ambient = Color.White;
      device.RenderState.ZBufferEnable = true;

      if (this.m_Mesh != null) {
        this.m_Mesh.Dispose();
      }

      ExtendedMaterial[] extendedMaterial;

      MemoryStream memoryStream = new MemoryStream(TRE_Explorer.Properties.Resources.frn_vet_at_at_toy_l0);
      this.m_Mesh = Mesh.FromStream(memoryStream, MeshFlags.Managed, device, out extendedMaterial);
      memoryStream.Close();

      if ((extendedMaterial != null) && (extendedMaterial.Length > 0)) {
        if (this.m_Texture != null) {
          foreach (Texture texture in this.m_Texture) {
            texture.Dispose();
          }
        }

        this.m_Material = new Material[extendedMaterial.Length];
        this.m_Texture = new Texture[extendedMaterial.Length];

        for (Int32 counter = 0; counter < extendedMaterial.Length; counter++) {
          this.m_Material[counter] = extendedMaterial[counter].Material3D;
          this.m_Material[counter].Ambient = this.m_Material[counter].Diffuse;

          if ((extendedMaterial[counter].TextureFilename != null) && (extendedMaterial[counter].TextureFilename != String.Empty)) {
            if (TRE_Explorer.Properties.Resources.ResourceManager.GetObject(extendedMaterial[counter].TextureFilename.Replace(".", "_")) != null) {
              if (TRE_Explorer.Properties.Resources.ResourceManager.GetObject(extendedMaterial[counter].TextureFilename.Replace(".", "_")).GetType() == typeof(Byte[])) {
                memoryStream = new MemoryStream((Byte[])TRE_Explorer.Properties.Resources.ResourceManager.GetObject(extendedMaterial[counter].TextureFilename.Replace(".", "_")));
              } else if (TRE_Explorer.Properties.Resources.ResourceManager.GetObject(extendedMaterial[counter].TextureFilename.Replace(".", "_")).GetType() == typeof(Bitmap)) {
                memoryStream = new MemoryStream();

                Bitmap bitmap = (Bitmap)TRE_Explorer.Properties.Resources.ResourceManager.GetObject(extendedMaterial[counter].TextureFilename.Replace(".", "_"));
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                bitmap.Dispose();
              }

              memoryStream.Seek(0L, SeekOrigin.Begin);
              this.m_Texture[counter] = TextureLoader.FromStream(device, memoryStream);
              memoryStream.Close();
            } else if (File.Exists(extendedMaterial[counter].TextureFilename)) {
              this.m_Texture[counter] = TextureLoader.FromFile(device, extendedMaterial[counter].TextureFilename);
            }
          }
        }
      }

      this.m_Mesh = this.m_Mesh.Clone(this.m_Mesh.Options.Value, CustomVertex.PositionNormalTextured.Format, this.m_Device);
      this.m_Mesh.ComputeNormals();

      VertexBuffer vertexBuffer = this.m_Mesh.VertexBuffer;
      GraphicsStream graphicsStream = vertexBuffer.Lock(0, 0, LockFlags.None);
      this.m_MeshRadius = Geometry.ComputeBoundingSphere(graphicsStream, this.m_Mesh.NumberVertices, this.m_Mesh.VertexFormat, out this.m_MechCenter);
      vertexBuffer.Unlock();
    }
    #endregion

    #region Display 3D Environment
    public void Render() {
      if (!this.m_Paused) {
        this.m_Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0F, 0);
        this.m_Device.BeginScene();

        this.SetupMatrices();

        for (Int32 counter = 0; counter < this.m_Material.Length; counter++) {
          this.m_Device.Material = this.m_Material[counter];
          this.m_Device.SetTexture(0, this.m_Texture[counter]);
          this.m_Mesh.DrawSubset(counter);
        }

        String text = String.Empty;
        switch (this.m_ViewAngle) {
          case ViewAngle.Front:
            text = " Left►\r\nD\r\no\r\nw\r\nn\r\n▼";
            break;

          case ViewAngle.Back:
            text = " Right►\r\nD\r\no\r\nw\r\nn\r\n▼";
            break;

          case ViewAngle.Bottom:
          case ViewAngle.Top:
            text = " Right►\r\nB\r\na\r\nc\r\nk\r\n▼";
            break;

          case ViewAngle.Left:
            text = " Back►\r\nD\r\no\r\nw\r\nn\r\n▼";
            break;

          case ViewAngle.Right:
            text = " Front►\r\nD\r\no\r\nw\r\nn\r\n▼";
            break;
        }

        System.Drawing.Font fontBase = new System.Drawing.Font("Courier New", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        Microsoft.DirectX.Direct3D.Font font = new Microsoft.DirectX.Direct3D.Font(this.m_Device, fontBase);
        font.DrawText(null, text, new Point(0, 0), Color.Blue);
        font.Dispose();

        this.m_Device.EndScene();
        this.m_Device.Present();
      }
    }

    private void SetupMatrices() {
      this.m_Device.Transform.World = Matrix.Translation(-this.m_MechCenter.X, -this.m_MechCenter.Y, -this.m_MechCenter.Z) * Matrix.RotationY(Geometry.DegreeToRadian(180)) * Matrix.RotationYawPitchRoll(Geometry.DegreeToRadian(-this.trackBarYYaw.Value), Geometry.DegreeToRadian(this.trackBarXPitch.Value), Geometry.DegreeToRadian(-this.trackBarZRoll.Value));
      this.m_Device.Transform.View = Matrix.LookAtLH(ViewAngleToVector(), new Vector3(0F, 0F, 0F), new Vector3(0F, 1F, 0F));
      this.m_Device.Transform.Projection = Matrix.PerspectiveFovLH((Single)Math.PI / 4F, (Single)this.m_FormRenderSurface.Width / (Single)this.m_FormRenderSurface.Height, 0F, this.m_MeshRadius);
    }

    private Vector3 ViewAngleToVector() {
      Single multiplier = 2.5F;

      switch (this.m_ViewAngle) {
        case ViewAngle.Front:
          return new Vector3(0F, 0F, (-this.m_MeshRadius * multiplier));

        case ViewAngle.Back:
          return new Vector3(0F, 0F, (this.m_MeshRadius * multiplier));

        case ViewAngle.Top:
          return new Vector3(0F, (this.m_MeshRadius * multiplier), Geometry.DegreeToRadian(1));

        case ViewAngle.Bottom:
          return new Vector3(0F, (-this.m_MeshRadius * multiplier), Geometry.DegreeToRadian(-1));

        case ViewAngle.Left:
          return new Vector3((this.m_MeshRadius * multiplier), 0F, 0F);

        case ViewAngle.Right:
          return new Vector3((-this.m_MeshRadius * multiplier), 0F, 0F);
      }

      return new Vector3(0F, 0F, (-this.m_MeshRadius * multiplier));
    }
    #endregion

    #region Update UI Displays
    private void UpdateMatrix() {
      this.rotationMatrix = new RotationMatrix(new EulerAngles(this.yYaw, this.xPitch, this.zRoll));

      MatrixToDataGridView(this.rotationMatrix, this.dataGridViewMatrix);

      UpdateLabels();

      this.m_FormRenderSurface.Invalidate();
    }

    private void UpdateLabels() {
      this.numberBoxXPitch.Text = this.trackBarXPitch.Value.ToString();
      this.numberBoxYYaw.Text = this.trackBarYYaw.Value.ToString();
      this.numberBoxZRoll.Text = this.trackBarZRoll.Value.ToString();
    }
    #endregion

    #region UI Buttons
    private void buttonOK_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
    }

    private void buttonReset_Click(object sender, EventArgs e) {
      this.trackBarXPitch.Value = this.m_OriginalXPitch;
      this.trackBarYYaw.Value = this.m_OriginalYYaw;
      this.trackBarZRoll.Value = this.m_OriginalZRoll;

      this.UpdateMatrix();
    }
    #endregion

    #region Angle Values Changed
    private void trackBar_Scroll(object sender, EventArgs e) {
      if (((TrackBar)sender).Enabled) {
        this.UpdateMatrix();
      }
    }

    private void buttonXPlus_Click(object sender, EventArgs e) {
      if (this.trackBarXPitch.Value < this.trackBarXPitch.Maximum) {
        this.trackBarXPitch.Value++;
        this.UpdateMatrix();
      }
    }

    private void buttonXMinus_Click(object sender, EventArgs e) {
      if (this.trackBarXPitch.Value > this.trackBarXPitch.Minimum) {
        this.trackBarXPitch.Value--;
        this.UpdateMatrix();
      }
    }

    private void buttonYPlus_Click(object sender, EventArgs e) {
      if (this.trackBarYYaw.Value < this.trackBarYYaw.Maximum) {
        this.trackBarYYaw.Value++;
        this.UpdateMatrix();
      }
    }

    private void buttonYMinus_Click(object sender, EventArgs e) {
      if (this.trackBarYYaw.Value > this.trackBarYYaw.Minimum) {
        this.trackBarYYaw.Value--;
        this.UpdateMatrix();
      }
    }

    private void buttonZPlus_Click(object sender, EventArgs e) {
      if (this.trackBarZRoll.Value < this.trackBarZRoll.Maximum) {
        this.trackBarZRoll.Value++;
        this.UpdateMatrix();
      }
    }

    private void buttonZMinus_Click(object sender, EventArgs e) {
      if (this.trackBarZRoll.Value > this.trackBarZRoll.Minimum) {
        this.trackBarZRoll.Value--;
        this.UpdateMatrix();
      }
    }

    private void buttonXReset_Click(object sender, EventArgs e) {
      this.trackBarXPitch.Value = this.m_OriginalXPitch;

      this.UpdateMatrix();
    }

    private void buttonYReset_Click(object sender, EventArgs e) {
      this.trackBarYYaw.Value = this.m_OriginalYYaw;

      this.UpdateMatrix();
    }

    private void buttonZReset_Click(object sender, EventArgs e) {
      this.trackBarZRoll.Value = this.m_OriginalZRoll;

      this.UpdateMatrix();
    }
    #endregion

    #region View Angles
    private void radioButtonFront_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton)sender).Checked) {
        this.m_ViewAngle = ViewAngle.Front;
        this.m_FormRenderSurface.Invalidate();
      }
    }

    private void radioButtonBack_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton)sender).Checked) {
        this.m_ViewAngle = ViewAngle.Back;
        this.m_FormRenderSurface.Invalidate();
      }
    }

    private void radioButtonTop_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton)sender).Checked) {
        this.m_ViewAngle = ViewAngle.Top;
        this.m_FormRenderSurface.Invalidate();
      }
    }

    private void radioButtonBottom_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton)sender).Checked) {
        this.m_ViewAngle = ViewAngle.Bottom;
        this.m_FormRenderSurface.Invalidate();
      }
    }

    private void radioButtonRight_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton)sender).Checked) {
        this.m_ViewAngle = ViewAngle.Right;
        this.m_FormRenderSurface.Invalidate();
      }
    }

    private void radioButtonLeft_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton)sender).Checked) {
        this.m_ViewAngle = ViewAngle.Left;
        this.m_FormRenderSurface.Invalidate();
      }
    }
    #endregion

    #region NumberBoxes Changed
    private void numberBoxXPitch_TextChanged(object sender, EventArgs e) {
      this.trackBarXPitch.Value = this.numberBoxXPitch.Value;
      UpdateMatrix();
    }

    private void numberBoxYYaw_TextChanged(object sender, EventArgs e) {
      this.trackBarYYaw.Value = this.numberBoxYYaw.Value;
      UpdateMatrix();
    }

    private void numberBoxZRoll_TextChanged(object sender, EventArgs e) {
      this.trackBarZRoll.Value = this.numberBoxZRoll.Value;
      UpdateMatrix();
    }
    #endregion
  }

  #region RepeatButton Class
  public class RepeatButton : Button {
    private Timer m_RepeatTimer = new Timer();
    private Timer m_DelayTimer = new Timer();

    public Int32 ButtonSpeed {
      get {
        return this.m_RepeatTimer.Interval;
      }
      set {
        this.m_RepeatTimer.Interval = value;
      }
    }
    public Int32 ButtonDelay {
      get {
        return this.m_DelayTimer.Interval;
      }
      set {
        this.m_DelayTimer.Interval = value;
      }
    }

    public RepeatButton() {
      Single repetitionsPerSecond = (((30F - 2.5F) / 31F) * ((Single)SystemInformation.KeyboardSpeed - 1F)) + 2.5F;

      this.m_RepeatTimer = new Timer();
      this.m_RepeatTimer.Interval = (Int32)Math.Round((1000F / repetitionsPerSecond), 0);
      this.m_RepeatTimer.Tick += new EventHandler(RepeatTimer_Tick);

      this.m_DelayTimer = new Timer();
      this.m_DelayTimer.Interval = ((SystemInformation.KeyboardDelay + 1) * 250);
      this.m_DelayTimer.Tick += new EventHandler(DelayTimer_Tick);
    }

    protected override void OnMouseDown(MouseEventArgs mevent) {
      this.PerformClick();
      this.m_DelayTimer.Start();
    }

    protected override void OnMouseUp(MouseEventArgs mevent) {
      this.m_DelayTimer.Stop();
      this.m_RepeatTimer.Stop();
    }

    private void RepeatTimer_Tick(object sender, EventArgs e) {
      this.PerformClick();
    }

    private void DelayTimer_Tick(object sender, EventArgs e) {
      this.m_RepeatTimer.Start();
      this.m_DelayTimer.Stop();
    }
  }
  #endregion

  #region NumberBox Class
  public class NumberBox : TextBox {
    private Int32 m_Value = 0;
    public Int32 Value {
      get {
        return this.m_Value;
      }
      set {
        this.Text = value.ToString();
        this.Select(this.Text.Length, 0);
      }
    }

    protected override void OnTextChanged(EventArgs e) {
      String firstCharacter = "-0123456789";
      String restCharacters = "0123456789";
      if ((this.Text == null) || (this.Text == String.Empty)) {
        this.Text = "0";
      } else {
        if (!firstCharacter.Contains(this.Text[0].ToString())) {
          this.Text = this.m_Value.ToString();
          return;
        }
        for (Int32 counter = 1; counter < this.Text.Length; counter++) {
          if (!restCharacters.Contains(this.Text[counter].ToString())) {
            this.Text = this.m_Value.ToString();
            return;
          }
        }
        if (((this.Text.Length > 0) && (this.Text[0] != '-')) || ((this.Text.Length > 1) && (this.Text[0] == '-'))) {
          Int32 tempValue = Int32.Parse(this.Text);
          if ((tempValue > 180) || (tempValue < -180)) {
            Int32 reversals = Math.Abs(tempValue / 180);
            tempValue %= 180;
            if ((reversals % 2) == 1) {
              tempValue *= -1;
            }
            this.Text = tempValue.ToString();
            this.Select(this.Text.Length, 0);
          } else {
            this.m_Value = tempValue;
            base.OnTextChanged(e);
          }
        }
      }
    }
  }
  #endregion
}
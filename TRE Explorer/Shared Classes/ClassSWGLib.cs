using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Microsoft.Win32;

namespace SWGLib {
  public class WSFile {
    #region Subclasses
    public class WSNode {
      private Int32 m_ID;
      private Int32 m_ParentID;
      private Int32 m_ObjectIndex;
      private Single m_oX;
      private Single m_oY;
      private Single m_oZ;
      private Single m_oW;
      private Single m_Scale;
      private Single m_X;
      private Single m_Y;
      private Single m_Z;
      private Single m_Type;
      private Byte[] m_POBCRC;
      private WSNode m_Parent;
      private List<WSNode> m_Nodes;

      public Int32 ID {
        get {
          return this.m_ID;
        }
        set {
          this.m_ID = value;
        }
      }
      public Int32 ParentID {
        get {
          return this.m_ParentID;
        }
        set {
          this.m_ParentID = value;
        }
      }
      public Int32 ObjectIndex {
        get {
          return this.m_ObjectIndex;
        }
        set {
          this.m_ObjectIndex = value;
        }
      }
      public Single oX {
        get {
          return this.m_oX;
        }
        set {
          this.m_oX = value;
        }
      }
      public Single oY {
        get {
          return this.m_oY;
        }
        set {
          this.m_oY = value;
        }
      }
      public Single oZ {
        get {
          return this.m_oZ;
        }
        set {
          this.m_oZ = value;
        }
      }
      public Single oW {
        get {
          return this.m_oW;
        }
        set {
          this.m_oW = value;
        }
      }
      public Single Scale {
        get {
          return this.m_Scale;
        }
        set {
          this.m_Scale = value;
        }
      }
      public Single X {
        get {
          return this.m_X;
        }
        set {
          this.m_X = value;
        }
      }
      public Single Y {
        get {
          return this.m_Y;
        }
        set {
          this.m_Y = value;
        }
      }
      public Single Z {
        get {
          return this.m_Z;
        }
        set {
          this.m_Z = value;
        }
      }
      public Single Type {
        get {
          return this.m_Type;
        }
        set {
          this.m_Type = value;
        }
      }
      public Byte[] POBCRC {
        get {
          return this.m_POBCRC;
        }
        set {
          this.m_POBCRC = value;
        }
      }
      public WSNode Parent {
        get {
          return this.m_Parent;
        }
        set {
          this.m_Parent = value;
        }
      }
      public List<WSNode> Nodes {
        get {
          return this.m_Nodes;
        }
        set {
          this.m_Nodes = value;
        }
      }

      public WSNode(Int32 id, Int32 parent, Int32 objectIndex, Single ox, Single oy, Single oz, Single ow, Single scale, Single x, Single y, Single z, Single type, Byte[] Unknown) {
        this.m_ID = id;
        this.m_ObjectIndex = objectIndex;
        this.m_oW = ow;
        this.m_oX = ox;
        this.m_oY = oy;
        this.m_oZ = oz;
        this.m_ParentID = parent;
        this.m_Scale = scale;
        this.m_Type = type;
        this.m_X = x;
        this.m_Y = y;
        this.m_Z = z;
        this.POBCRC = Unknown;
        this.m_Nodes = new List<WSNode>();
        this.m_Parent = null;
      }

      public WSNode() {
        this.m_Nodes = new List<WSNode>();
      }
    }
    #endregion

    #region Private Members
    private List<WSNode> m_Nodes;
    private String[] m_Types;
    private String m_Filename;
    #endregion

    #region Public Members
    public List<WSNode> Nodes {
      get {
        return this.m_Nodes;
      }
      set {
        this.m_Nodes = value;
      }
    }
    public String[] Types {
      get {
        return this.m_Types;
      }
      set {
        this.m_Types = value;
      }
    }
    public String Filename {
      get {
        return this.m_Filename;
      }
      set {
        this.m_Filename = value;
      }
    }
    public Byte[] Bytes {
      get {
        return GetBytes();
      }
    }
    public Int32 MaximumObjectIndex {
      get {
        return GetMaximumObjectIndex(this.m_Nodes.ToArray());
      }
    }
    public Int32 NextAvailableID {
      get {
        return (GetNextAvailableID(this.m_Nodes.ToArray()) + 1);
      }
    }
    #endregion

    #region Private Functions
    private Int32 GetMaximumObjectIndex(WSNode[] wsNodes) {
      Int32 returnValue = 0;
      foreach (WSNode wsNode in wsNodes) {
        returnValue = Math.Max(returnValue, wsNode.ObjectIndex);
        if (wsNode.Nodes.Count > 0) {
          returnValue = Math.Max(returnValue, GetMaximumObjectIndex(wsNode.Nodes.ToArray()));
        }
      }
      return returnValue;
    }

    private Int32 GetNextAvailableID(WSNode[] wsNodes) {
      Int32 returnValue = 0;
      foreach (WSNode wsNode in wsNodes) {
        returnValue = Math.Max(returnValue, wsNode.ID);
        if (wsNode.Nodes.Count > 0) {
          returnValue = Math.Max(returnValue, GetNextAvailableID(wsNode.Nodes.ToArray()));
        }
      }
      return returnValue;
    }

    private WSNode FindChildren(Int32 ID, WSNode[] wsNodes) {
      foreach (WSNode wsNode in wsNodes) {
        if (wsNode.ID == ID) {
          return wsNode;
        } else {
          if (wsNode.Nodes.Count > 0) {
            WSNode returnValue = FindChildren(ID, wsNode.Nodes.ToArray());
            if (returnValue != null) {
              return returnValue;
            }
          }
        }
      }
      return null;
    }

    private Byte[] GetChildBytes(WSNode[] wsNodes) {
      List<Byte> listBytes = new List<Byte>();
      String NODSFORM = ((wsNodes[0].ParentID == 0) ? "NODSFORM" : "FORM");

      foreach (WSNode wsNode in wsNodes) {
        Byte[] childBytes = new Byte[0];
        if (wsNode.Nodes.Count > 0) {
          childBytes = GetChildBytes(wsNode.Nodes.ToArray());
        }

        MemoryStream tempMemoryStream = new MemoryStream();
        BinaryWriter tempBinaryWriter = new BinaryWriter(tempMemoryStream);
        tempBinaryWriter.Write(wsNode.ID);
        tempBinaryWriter.Write(wsNode.ParentID);
        tempBinaryWriter.Write(wsNode.ObjectIndex);
        tempBinaryWriter.Write(wsNode.oX);
        tempBinaryWriter.Write(wsNode.oY);
        tempBinaryWriter.Write(wsNode.oZ);
        tempBinaryWriter.Write(wsNode.oW);
        tempBinaryWriter.Write(wsNode.Scale);
        tempBinaryWriter.Write(wsNode.X);
        tempBinaryWriter.Write(wsNode.Y);
        tempBinaryWriter.Write(wsNode.Z);
        tempBinaryWriter.Write(wsNode.Type);
        tempBinaryWriter.Write(wsNode.POBCRC);

        Byte[] tempNodeBytes = tempMemoryStream.ToArray();
        tempBinaryWriter.Close();
        tempMemoryStream.Close();

        listBytes.AddRange(Encoding.ASCII.GetBytes(NODSFORM));
        listBytes.AddRange(BitConverter.GetBytes(Utilities.EndianFlip(tempNodeBytes.Length + childBytes.Length + 24)));
        listBytes.AddRange(Encoding.ASCII.GetBytes("NODEFORM"));
        listBytes.AddRange(BitConverter.GetBytes(Utilities.EndianFlip(tempNodeBytes.Length + childBytes.Length + 12)));
        listBytes.AddRange(Encoding.ASCII.GetBytes("0000DATA"));
        listBytes.AddRange(BitConverter.GetBytes(Utilities.EndianFlip(tempNodeBytes.Length)));
        listBytes.AddRange(tempNodeBytes);
        if (wsNode.Nodes.Count > 0) {
          listBytes.AddRange(childBytes);
        }

        NODSFORM = "FORM";
      }
      Byte[] nodeBytes = listBytes.ToArray();

      listBytes.Clear();

      return nodeBytes;
    }

    private Byte[] GetBytes() {
      Byte[] nodeBytes = GetChildBytes(this.m_Nodes.ToArray());

      List<Byte> listBytes = new List<Byte>();
      foreach (String type in this.m_Types) {
        listBytes.AddRange(Encoding.ASCII.GetBytes(type));
        listBytes.Add((Byte)0x00);
      }
      listBytes.Reverse();
      listBytes.AddRange(BitConverter.GetBytes(Utilities.EndianFlip(this.m_Types.Length)));
      listBytes.AddRange(BitConverter.GetBytes(listBytes.Count));
      listBytes.AddRange(Encoding.ASCII.GetBytes("LNTO"));
      listBytes.Reverse();
      Byte[] otnlBytes = listBytes.ToArray();

      listBytes.Clear();

      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
      binaryWriter.Write(Encoding.ASCII.GetBytes("FORM"));
      binaryWriter.Write(Utilities.EndianFlip(24 + nodeBytes.Length + otnlBytes.Length));
      binaryWriter.Write(Encoding.ASCII.GetBytes("WSNPFORM"));
      binaryWriter.Write(Utilities.EndianFlip(12 + nodeBytes.Length + otnlBytes.Length));
      binaryWriter.Write(Encoding.ASCII.GetBytes("0001FORM"));
      binaryWriter.Write(Utilities.EndianFlip(nodeBytes.Length));
      binaryWriter.Write(nodeBytes);
      binaryWriter.Write(otnlBytes);

      Byte[] bytes = memoryStream.ToArray();
      binaryWriter.Close();
      memoryStream.Close();

      return bytes;
    }

    private void ParseStream(Stream Stream) {
      Stream.Seek(0L, SeekOrigin.Begin);
      BinaryReader binaryReader = new BinaryReader(Stream);
      String FORM = new String(binaryReader.ReadChars(4));
      binaryReader.ReadBytes(4); // Length

      String WSNPFORM = new String(binaryReader.ReadChars(8));
      binaryReader.ReadBytes(4); // Length

      String t0001FORM = new String(binaryReader.ReadChars(8));

      Byte[] buffer = binaryReader.ReadBytes(4);
      Int32 Length = Utilities.EndianFlip(BitConverter.ToInt32(buffer, 0));
      Byte[] byteNodes = binaryReader.ReadBytes(Length);

      this.m_Nodes = new List<WSNode>();
      ParseNodes(byteNodes);

      String OTNL = new String(binaryReader.ReadChars(4));
      binaryReader.ReadBytes(4); // Length
      binaryReader.ReadBytes(4); // String count

      List<String> listTypes = new List<String>();
      while (Stream.Position < Stream.Length) {
        listTypes.Add(Utilities.ReadString(binaryReader));
      }
      this.m_Types = listTypes.ToArray();
    }

    private void ParseNodes(Byte[] data) {
      MemoryStream memoryStream = new MemoryStream(data);
      BinaryReader binaryReader = new BinaryReader(memoryStream);
      while (memoryStream.Position < memoryStream.Length) {
        String FORM = new String(binaryReader.ReadChars(4));
        Byte[] buffer = binaryReader.ReadBytes(4);
        if (Utilities.IsAlpha(buffer)) {
          FORM += new String(Encoding.ASCII.GetChars(buffer));
          buffer = binaryReader.ReadBytes(4); // Length
        }

        String NODEFORM = new String(binaryReader.ReadChars(8));
        binaryReader.ReadBytes(4); // Length

        String t0000DATA = new String(binaryReader.ReadChars(8));
        binaryReader.ReadBytes(4); // Length

        Int32 ID = binaryReader.ReadInt32();
        Int32 Parent = binaryReader.ReadInt32();
        Int32 ObjectIndex = binaryReader.ReadInt32();
        Single oX = binaryReader.ReadSingle();
        Single oY = binaryReader.ReadSingle();
        Single oZ = binaryReader.ReadSingle();
        Single oW = binaryReader.ReadSingle();
        Single Scale = binaryReader.ReadSingle();
        Single X = binaryReader.ReadSingle();
        Single Y = binaryReader.ReadSingle();
        Single Z = binaryReader.ReadSingle();
        Single Type = binaryReader.ReadSingle();
        Byte[] Unknown = binaryReader.ReadBytes(4);

        WSNode wsNode = new WSNode(ID, Parent, ObjectIndex, oX, oY, oZ, oW, Scale, X, Y, Z, Type, Unknown);
        if (wsNode.ParentID == 0) {
          this.m_Nodes.Add(wsNode);
        } else {
          WSNode tempNode = this.FindNodeByID(wsNode.ParentID);
          tempNode.Nodes.Add(wsNode);
          wsNode.Parent = tempNode;
        }
      }

      binaryReader.Close();
      memoryStream.Close();
    }
    #endregion

    #region Public Functions
    public WSNode FindNodeByID(Int32 ID) {
      if (this.m_Nodes.Count > 0) {
        return FindChildren(ID, this.m_Nodes.ToArray());
      }
      return null;
    }

    public void Save(String Filename) {
      try {
        Directory.CreateDirectory(Filename.Substring(0, Filename.LastIndexOf('\\')));
      } catch {
      }

      FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None);
      BinaryWriter binaryWriter = new BinaryWriter(fileStream);
      binaryWriter.Write(this.Bytes);
      binaryWriter.Close();
      fileStream.Close();
    }

    public void Open(Byte[] Data) {
      this.m_Filename = String.Empty;
      MemoryStream memoryStream = new MemoryStream(Data);
      ParseStream(memoryStream);
      memoryStream.Close();
    }

    public void Open(String Filename) {
      this.m_Filename = Filename;
      FileStream fileStream = new FileStream(this.m_Filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
      ParseStream(fileStream);
      fileStream.Close();
    }

    public void Open(Stream Stream) {
      this.ParseStream(Stream);
    }

    public void Close() {
      this.m_Filename = String.Empty;
      this.m_Nodes = new List<WSNode>();
      this.m_Types = null;
    }

    public WSFile(Byte[] Data) {
      this.Open(Data);
    }

    public WSFile(String Filename) {
      this.Open(Filename);
    }

    public WSFile(Stream Stream) {
      this.Open(Stream);
    }

    public WSFile() {
      this.Close();
    }
    #endregion
  }

  public class ILFFile {
    #region Subclasses
    public class ILFNode {
      public String Object;
      public String Cell;
      /* public Single M11;
      public Single M21;
      public Single M31;
      public Single M12;
      public Single M22;
      public Single M32;
      public Single M13;
      public Single M23;
      public Single M33; */
      public RotationMatrix RotationMatrix = new RotationMatrix();
      public Single W1;
      public Single W2;
      public Single W3;

      public ILFNode() {
        // stuff
      }

      public Boolean Equals(ILFNode ilfNode) {
        if (this.Object != ilfNode.Object) {
          return false;
        }
        if (this.Cell != ilfNode.Cell) {
          return false;
        }
        if (this.RotationMatrix[0][0] != ilfNode.RotationMatrix[0][0]) {
          return false;
        }
        if (this.RotationMatrix[0][1] != ilfNode.RotationMatrix[0][1]) {
          return false;
        }
        if (this.RotationMatrix[0][2] != ilfNode.RotationMatrix[0][2]) {
          return false;
        }
        if (this.RotationMatrix[1][0] != ilfNode.RotationMatrix[1][0]) {
          return false;
        }
        if (this.RotationMatrix[1][1] != ilfNode.RotationMatrix[1][1]) {
          return false;
        }
        if (this.RotationMatrix[1][2] != ilfNode.RotationMatrix[1][2]) {
          return false;
        }
        if (this.RotationMatrix[2][0] != ilfNode.RotationMatrix[2][0]) {
          return false;
        }
        if (this.RotationMatrix[2][1] != ilfNode.RotationMatrix[2][1]) {
          return false;
        }
        if (this.RotationMatrix[2][2] != ilfNode.RotationMatrix[2][2]) {
          return false;
        }
        if (this.W1 != ilfNode.W1) {
          return false;
        }
        if (this.W2 != ilfNode.W2) {
          return false;
        }
        if (this.W3 != ilfNode.W3) {
          return false;
        }
        return true;
      }
    }
    #endregion

    #region Private members
    private String m_Filename;
    private List<ILFNode> m_Nodes;
    #endregion

    #region Public Members
    public String Filename {
      get {
        return this.m_Filename;
      }
      set {
        this.m_Filename = value;
      }
    }
    public List<ILFNode> Nodes {
      get {
        return this.m_Nodes;
      }
    }
    public Byte[] Bytes {
      get {
        return GetBytes();
      }
    }
    #endregion

    #region Private Functions
    private Byte[] GetBytes() {
      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

      binaryWriter.Write(Encoding.ASCII.GetBytes("0000"));
      foreach (ILFNode ilfNode in this.m_Nodes.ToArray()) {
        List<Byte> listNodeBytes = new List<Byte>();
        listNodeBytes.AddRange(Encoding.ASCII.GetBytes(ilfNode.Object));
        listNodeBytes.Add((Byte)0x00);
        listNodeBytes.AddRange(Encoding.ASCII.GetBytes(ilfNode.Cell));
        listNodeBytes.Add((Byte)0x00);
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[0][0]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[0][1]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[0][2]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.W1));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[1][0]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[1][1]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[1][2]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.W2));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[2][0]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[2][1]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.RotationMatrix[2][2]));
        listNodeBytes.AddRange(BitConverter.GetBytes(ilfNode.W3));

        Int32 NODELength = listNodeBytes.ToArray().Length;

        binaryWriter.Write(Encoding.ASCII.GetBytes("NODE"));
        binaryWriter.Write(Utilities.EndianFlip(NODELength));
        binaryWriter.Write(listNodeBytes.ToArray());
      }

      Byte[] INLYFORMBytes = memoryStream.ToArray();
      memoryStream = new MemoryStream();
      binaryWriter = new BinaryWriter(memoryStream);

      Int32 INLYFORMLength = INLYFORMBytes.Length;
      Int32 FORMLength = INLYFORMLength + 12;

      binaryWriter.Write(Encoding.ASCII.GetBytes("FORM"));
      binaryWriter.Write(Utilities.EndianFlip(FORMLength));
      binaryWriter.Write(Encoding.ASCII.GetBytes("INLYFORM"));
      binaryWriter.Write(Utilities.EndianFlip(INLYFORMLength));
      binaryWriter.Write(INLYFORMBytes);

      Byte[] FORMBytes = memoryStream.ToArray();
      binaryWriter.Close();
      memoryStream.Close();

      return FORMBytes;
    }

    private void ParseStream(Stream Stream) {
      Stream.Seek(0L, SeekOrigin.Begin);
      BinaryReader binaryReader = new BinaryReader(Stream);

      String FORMID = new String(binaryReader.ReadChars(4));
      Int32 FORMLength = binaryReader.ReadInt32();
      String INLYFORMID = new String(binaryReader.ReadChars(8));
      Int32 INLYFORMLength = binaryReader.ReadInt32();
      String t0000 = new String(binaryReader.ReadChars(4));

      while (Stream.Position < Stream.Length) {
        String NODEID = new String(binaryReader.ReadChars(4));
        Int32 NODELength = binaryReader.ReadInt32();

        ILFNode ilfNode = new ILFNode();
        ilfNode.Object = Utilities.ReadString(binaryReader);
        ilfNode.Cell = Utilities.ReadString(binaryReader);
        ilfNode.RotationMatrix[0][0] = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[0][1] = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[0][2] = binaryReader.ReadSingle();
        ilfNode.W1 = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[1][0] = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[1][1] = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[1][2] = binaryReader.ReadSingle();
        ilfNode.W2 = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[2][0] = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[2][1] = binaryReader.ReadSingle();
        ilfNode.RotationMatrix[2][2] = binaryReader.ReadSingle();
        ilfNode.W3 = binaryReader.ReadSingle();

        this.m_Nodes.Add(ilfNode);
      }

      binaryReader.Close();
    }
    #endregion

    #region Public Functions
    public void Save(String Filename) {
      try {
        Directory.CreateDirectory(Filename.Substring(0, Filename.LastIndexOf('\\')));
      } catch {
      }

      FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None);
      BinaryWriter binaryWriter = new BinaryWriter(fileStream);
      binaryWriter.Write(this.Bytes);
      binaryWriter.Close();
      fileStream.Close();
    }

    public void Open(String Filename) {
      this.m_Filename = Filename;
      this.m_Nodes = new List<ILFNode>();

      FileStream fileStream = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Read);
      ParseStream(fileStream);
      fileStream.Close();
    }

    public void Open(Byte[] Data) {
      this.m_Filename = String.Empty;
      this.m_Nodes = new List<ILFNode>();

      MemoryStream memoryStream = new MemoryStream(Data);
      ParseStream(memoryStream);
      memoryStream.Close();
    }

    public void Open(Stream Stream) {
      this.m_Filename = String.Empty;
      this.m_Nodes = new List<ILFNode>();

      ParseStream(Stream);
    }

    public void Close() {
      this.m_Filename = String.Empty;
      this.m_Nodes = new List<ILFNode>();
    }

    public ILFFile(String Filename) {
      this.Open(Filename);
    }

    public ILFFile(Byte[] Data) {
      this.Open(Data);
    }

    public ILFFile(Stream Stream) {
      this.Open(Stream);
    }

    public ILFFile() {
      this.Close();
    }
    #endregion
  }

  public class PALFile {
    #region Private Members
    private String m_Filename;
    private List<Color> m_Colors;
    #endregion

    #region Public Members
    public Color[] Colors {
      get {
        return m_Colors.ToArray();
      }
    }
    public Byte[] Bytes {
      get {
        return this.GetBytes();
      }
    }
    public String Filename {
      get {
        return this.m_Filename;
      }
      set {
        this.m_Filename = value;
      }
    }
    #endregion

    #region Private Functions
    private Byte[] GetBytes() {
      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

      List<Byte> listBytes = new List<Byte>();
      listBytes.Add((Byte)0x00);
      listBytes.Add((Byte)0x03);
      listBytes.Add(((this.m_Colors.Count == 256) ? (Byte)0x00 : (Byte)this.m_Colors.Count));
      listBytes.Add(((this.m_Colors.Count == 256) ? (Byte)0x01 : (Byte)0x00));
      foreach (Color color in this.m_Colors) {
        listBytes.Add(color.R);
        listBytes.Add(color.G);
        listBytes.Add(color.B);
        listBytes.Add((Byte)0x00);
      }
      Byte[] colorBytes = listBytes.ToArray();
      Int32 colorLength = colorBytes.Length;
      Int32 PALdataLength = colorLength + 12;

      listBytes.Clear();

      binaryWriter.Write(Encoding.ASCII.GetBytes("RIFF"));
      binaryWriter.Write(PALdataLength);
      binaryWriter.Write(Encoding.ASCII.GetBytes("PAL data"));
      binaryWriter.Write(colorLength);
      binaryWriter.Write(colorBytes);

      Byte[] RIFFBytes = memoryStream.ToArray();

      binaryWriter.Close();
      memoryStream.Close();

      return RIFFBytes;
    }

    private void ParseStream(Stream Stream) {
      Stream.Seek(0L, SeekOrigin.Begin);
      BinaryReader binaryReader = new BinaryReader(Stream);

      String RIFF = new String(binaryReader.ReadChars(4));
      binaryReader.ReadInt32(); // RIFF length
      String PALdata = new String(binaryReader.ReadChars(8));
      binaryReader.ReadInt32(); // PAL data length
      binaryReader.ReadByte(); // 0x00
      binaryReader.ReadByte(); // 0x03
      Byte colorCountByte1 = binaryReader.ReadByte();
      Byte colorCountByte2 = binaryReader.ReadByte();
      Int32 colorCount = 0;

      if ((colorCountByte1 == (Byte)0x00) && (colorCountByte2 == (Byte)0x01)) {
        colorCount = 256;
      } else {
        colorCount = (Int32)colorCountByte1;
      }

      List<Color> listColors = new List<Color>();
      while (Stream.Position < Stream.Length) {
        Int32 red = (Int32)binaryReader.ReadByte();
        Int32 green = (Int32)binaryReader.ReadByte();
        Int32 blue = (Int32)binaryReader.ReadByte();
        binaryReader.ReadByte();

        listColors.Add(Color.FromArgb(red, green, blue));
      }
      binaryReader.Close();
      Stream.Close();

      this.m_Colors = new List<Color>();
      this.m_Colors.AddRange(listColors.ToArray());
    }
    #endregion

    #region Public Functions
    public void AddColor(Color Color) {
      if (this.m_Colors.Count < 256) {
        this.m_Colors.Add(Color);
      }
    }

    public void RemoveColor(Int32 Index) {
      if ((Index >= 0) && (Index < this.m_Colors.Count)) {
        this.m_Colors.RemoveAt(Index);
      }
    }

    public void ReplaceColor(Int32 Index, Color Color) {
      this.m_Colors[Index] = Color;
    }

    public Int32 FindColor(Color Color) {
      return this.m_Colors.IndexOf(Color);
    }

    public String HexadecimalIndex(Color color) {
      Int32 index = FindColor(color);

      String hexadecimal = "0123456789ABCDEF";
      Int32 row = index / 16;
      Int32 column = index % 16;
      return hexadecimal.Substring(row, 1) + hexadecimal.Substring(column, 1);
    }

    public void Save(String Filename) {
      try {
        Directory.CreateDirectory(Filename.Substring(0, Filename.LastIndexOf('\\')));
      } catch {
      }

      FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None);
      BinaryWriter binaryWriter = new BinaryWriter(fileStream);
      binaryWriter.Write(this.Bytes);
      binaryWriter.Close();
      fileStream.Close();
    }

    public void Open(String Filename) {
      this.m_Filename = Filename;
      FileStream fileStream = new FileStream(this.m_Filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
      ParseStream(fileStream);
      fileStream.Close();
    }

    public void Open(Byte[] Data) {
      this.m_Filename = String.Empty;
      MemoryStream memoryStream = new MemoryStream(Data);
      ParseStream(memoryStream);
      memoryStream.Close();
    }

    public void Open(Stream Stream) {
      this.m_Filename = String.Empty;
      ParseStream(Stream);
    }

    public void Close() {
      this.m_Filename = null;
      this.m_Colors = null;
    }

    public PALFile(String Filename) {
      this.m_Filename = Filename;
      FileStream fileStream = new FileStream(this.m_Filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
      this.ParseStream(fileStream);
      fileStream.Close();
    }

    public PALFile(Byte[] Data) {
      this.m_Filename = String.Empty;
      MemoryStream memoryStream = new MemoryStream(Data);
      this.ParseStream(memoryStream);
      memoryStream.Close();
    }

    public PALFile(Stream Stream) {
      this.m_Filename = String.Empty;
      this.ParseStream(Stream);
    }

    public PALFile() {
      this.m_Colors = null;
      this.m_Filename = null;
    }
    #endregion
  }

  [Serializable]
  public class IFFFile {
    #region Subclasses
    [Serializable]
    public class IFFNode {
      private List<IFFNode> m_Children;
      private Byte[] m_Data;
      private String m_ID;
      private IFFNode m_Parent;

      public List<IFFNode> Children {
        get {
          return this.m_Children;
        }
        set {
          this.m_Children = value;
        }
      }

      public Byte[] Data {
        get {
          return this.m_Data;
        }
        set {
          this.m_Data = value;
        }
      }

      public String ID {
        get {
          return this.m_ID;
        }
        set {
          this.m_ID = value;
        }
      }

      public IFFNode Parent {
        get {
          return m_Parent;
        }
        set {
          this.m_Parent = value;
        }
      }

      public Int32 Index {
        get {
          if (this.m_Parent != null) {
            try {
              return this.m_Parent.m_Children.IndexOf(this);
            } catch {
              return -1;
            }
          } else {
            return -1;
          }
        }
      }

      public IFFNode() {
        this.m_ID = String.Empty;
        this.m_Data = null;
        this.m_Parent = null;
        this.m_Children = new List<IFFNode>();
      }

      public IFFNode(String ID, Byte[] Data) {
        this.m_ID = ID;
        this.m_Data = Data;
        this.m_Parent = null;
        this.m_Children = new List<IFFNode>();
      }

      public IFFNode(String ID, Byte[] Data, IFFNode Parent) {
        this.m_ID = ID;
        this.m_Data = Data;
        this.m_Parent = Parent;
        this.m_Children = new List<IFFNode>();
      }

      public IFFNode(String ID, Byte[] Data, IFFNode[] Children) {
        this.m_ID = ID;
        this.m_Data = Data;
        this.m_Parent = null;
        this.m_Children = new List<IFFNode>(Children);
      }

      public IFFNode(String ID, Byte[] Data, IFFNode Parent, IFFNode[] Children) {
        this.m_ID = ID;
        this.m_Data = Data;
        this.m_Parent = Parent;
        if (Children == null) {
          this.m_Children = new List<IFFNode>();
        } else {
          this.m_Children = new List<IFFNode>(Children);
        }
      }
    }
    public class IFFDataTable : DataTable {
      private String[] m_Types;
      private IFFFile m_IFFFile;

      public String[] Types {
        get {
          return this.m_Types;
        }
        set {
          this.m_Types = value;
        }
      }

      public Byte[] Bytes {
        get {
          return GetBytes();
        }
      }

      private Byte[] GetBytes() {
        this.AcceptChanges();

        MemoryStream memoryStream;
        BinaryWriter binaryWriter;
        List<Byte> listBytes = new List<Byte>();

        // Encode cols

        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        listBytes.Clear();

        for (Int32 counter = 0; counter < this.Columns.Count; counter++) {
          listBytes.AddRange(Encoding.ASCII.GetBytes(this.Columns[counter].ColumnName));
          listBytes.Add((Byte)0x00);
        }

        binaryWriter.Write(Encoding.ASCII.GetBytes("COLS"));
        binaryWriter.Write(Utilities.EndianFlip(listBytes.ToArray().Length + 4));
        binaryWriter.Write(this.Columns.Count);
        binaryWriter.Write(listBytes.ToArray());

        Byte[] colsBytes = memoryStream.ToArray();

        // Encode types

        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        listBytes.Clear();

        for (Int32 counter = 0; counter < this.Columns.Count; counter++) {
          listBytes.AddRange(Encoding.ASCII.GetBytes(this.Types[counter]));
          listBytes.Add((Byte)0x00);
        }

        binaryWriter.Write(Encoding.ASCII.GetBytes("TYPE"));
        binaryWriter.Write(Utilities.EndianFlip(listBytes.ToArray().Length));
        binaryWriter.Write(listBytes.ToArray());

        Byte[] typeBytes = memoryStream.ToArray();

        // Encode rows

        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        listBytes.Clear();

        for (Int32 row = 0; row < this.Rows.Count; row++) {
          for (Int32 column = 0; column < this.Columns.Count; column++) {
            switch (this.Types[column].Substring(0, 1)) {
              case "s":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.Add((Byte)0x00);
                } else {
                  listBytes.AddRange(Encoding.ASCII.GetBytes(this.Rows[row][column].ToString()));
                  listBytes.Add((Byte)0x00);
                }
                break;

              case "b":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.AddRange(BitConverter.GetBytes(0));
                } else {
                  if ((Boolean)this.Rows[row][column]) {
                    listBytes.AddRange(BitConverter.GetBytes(1));
                  } else {
                    listBytes.AddRange(BitConverter.GetBytes(0));
                  }
                }
                break;

              case "i":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.AddRange(BitConverter.GetBytes((Int32)0));
                } else {
                  listBytes.AddRange(BitConverter.GetBytes(Convert.ToInt32(this.Rows[row][column])));
                }
                break;

              case "I":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.AddRange(BitConverter.GetBytes((Int32)0));
                } else {
                  listBytes.AddRange(BitConverter.GetBytes(Convert.ToInt32(this.Rows[row][column])));
                }
                break;

              case "f":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.AddRange(BitConverter.GetBytes((Single)0));
                } else {
                  listBytes.AddRange(BitConverter.GetBytes(Convert.ToSingle(this.Rows[row][column])));
                }
                break;

              case "e":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.AddRange(BitConverter.GetBytes((Int32)0));
                } else {
                  listBytes.AddRange(BitConverter.GetBytes(Convert.ToInt32(this.Rows[row][column])));
                }
                break;

              case "h":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.AddRange(BitConverter.GetBytes((Int32)0));
                } else {
                  listBytes.AddRange(BitConverter.GetBytes(Convert.ToInt32(this.Rows[row][column])));
                }
                break;

              case "z":
                if (this.Rows[row][column].GetType() == typeof(DBNull)) {
                  listBytes.AddRange(BitConverter.GetBytes((Int32)0));
                } else {
                  listBytes.AddRange(BitConverter.GetBytes(Convert.ToInt32(this.Rows[row][column])));
                }
                break;
            }
          }
        }

        binaryWriter.Write(Encoding.ASCII.GetBytes("ROWS"));
        binaryWriter.Write(Utilities.EndianFlip(listBytes.ToArray().Length + 4));
        binaryWriter.Write(this.Rows.Count);
        binaryWriter.Write(listBytes.ToArray());

        Byte[] rowsBytes = memoryStream.ToArray();

        // Encode dtiiform

        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        listBytes.Clear();

        listBytes.AddRange(Encoding.ASCII.GetBytes("0001"));
        listBytes.AddRange(colsBytes);
        listBytes.AddRange(typeBytes);
        listBytes.AddRange(rowsBytes);

        binaryWriter.Write(Encoding.ASCII.GetBytes("DTIIFORM"));
        binaryWriter.Write(Utilities.EndianFlip(listBytes.ToArray().Length));
        binaryWriter.Write(listBytes.ToArray());

        Byte[] dtiiformBytes = memoryStream.ToArray();

        // Encode form

        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        listBytes.Clear();

        binaryWriter.Write(Encoding.ASCII.GetBytes("FORM"));
        binaryWriter.Write(Utilities.EndianFlip(dtiiformBytes.Length));
        binaryWriter.Write(dtiiformBytes);

        Byte[] formBytes = memoryStream.ToArray();

        // Clear variables
        binaryWriter.Close();
        memoryStream.Close();
        listBytes.Clear();

        return formBytes;
      }

      public IFFDataTable(IFFFile IFFFile) {
        this.m_IFFFile = IFFFile;

        MemoryStream memoryStream = new MemoryStream(this.m_IFFFile.Node.Data);
        BinaryReader binaryReader = new BinaryReader(memoryStream);

        // FORM
        /* String ID = new String(binaryReader.ReadChars(4));
        if (ID != "FORM") {
          return null;
        }
        UInt32 formLength = Utilities.EndianFlip(binaryReader.ReadUInt32()); */

        // DTIIFORM
        String ID = new String(binaryReader.ReadChars(8));
        if (ID != "DTIIFORM") {
          return;
        }
        UInt32 dtiiformLength = Utilities.EndianFlip(binaryReader.ReadUInt32());

        // 0001
        String t0001 = new String(binaryReader.ReadChars(4));

        // COLS
        ID = new String(binaryReader.ReadChars(4));
        if (ID != "COLS") {
          return;
        }
        UInt32 colsLength = Utilities.EndianFlip(binaryReader.ReadUInt32());
        Int32 colsCount = binaryReader.ReadInt32();
        Byte[] buffer = binaryReader.ReadBytes((Int32)colsLength - 4);
        String[] colsCaptions = Utilities.ReadStrings(buffer, colsCount);

        // TYPE
        ID = new String(binaryReader.ReadChars(4));
        if (ID != "TYPE") {
          return;
        }
        UInt32 typeLength = Utilities.EndianFlip(binaryReader.ReadUInt32());
        buffer = binaryReader.ReadBytes((Int32)typeLength);
        String[] types = Utilities.ReadStrings(buffer, colsCount);
        this.m_Types = types;

        // ROWS
        ID = new String(binaryReader.ReadChars(4));
        if (ID != "ROWS") {
          return;
        }
        UInt32 rowsLength = Utilities.EndianFlip(binaryReader.ReadUInt32());
        Int32 rowsCount = binaryReader.ReadInt32();

        // Determine column types and generate columns
        for (Int32 counter = 0; counter < colsCount; counter++) {
          Type type = typeof(String);
          String caption = colsCaptions[counter];
          if (types[counter][0] == 's') {
            type = typeof(String);
          }
          if (types[counter][0] == 'b') {
            type = typeof(Boolean);
          }
          if (types[counter][0] == 'i') {
            type = typeof(Int32);
          }
          if (types[counter][0] == 'I') {
            type = typeof(Int32);
          }
          if (types[counter][0] == 'f') {
            type = typeof(Single);
          }
          if (types[counter][0] == 'e') { // enum, TBI
            type = typeof(Int32);
            caption = types[counter];
          }
          if (types[counter][0] == 'h') { // ?, TBI
            type = typeof(Int32);
          }
          if (types[counter][0] == 'z') { // z(other.iff),foreign key ?, TBI
            type = typeof(Int32);
          }
          // for enum type (IFF type = 'e') we return the enum definition in the Caption field of the columns
          // todo: parse the enum definition instead and replace values with the enum members
          this.Columns.Add(colsCaptions[counter], type).Caption = caption;
        }

        // Generate rows
        for (Int32 counter1 = 0; counter1 < rowsCount; counter1++) {
          try {
            DataRow dataRow = this.NewRow();
            for (Int32 counter2 = 0; counter2 < colsCount; counter2++) {
              if (types[counter2][0] == 's') {
                dataRow[counter2] = Utilities.ReadString(binaryReader);
              }
              if (types[counter2][0] == 'b') {
                Int32 b = binaryReader.ReadInt32();
                dataRow[counter2] = (b != 0);
              }
              if (types[counter2][0] == 'i') {
                dataRow[counter2] = binaryReader.ReadInt32();
              }
              if (types[counter2][0] == 'I') {
                dataRow[counter2] = binaryReader.ReadInt32();
              }
              if (types[counter2][0] == 'f') {
                dataRow[counter2] = binaryReader.ReadSingle();
              }
              if (types[counter2][0] == 'e') {
                dataRow[counter2] = binaryReader.ReadInt32();
              }
              if (types[counter2][0] == 'h') {
                dataRow[counter2] = binaryReader.ReadInt32();
              }
              if (types[counter2][0] == 'z') {
                dataRow[counter2] = binaryReader.ReadInt32();
              }
            }
            this.Rows.Add(dataRow);
          } catch {
            // an exception has occurred
          }
        }

        binaryReader.Close();
        memoryStream.Close();
      }

      public IFFDataTable(String[] Types) {
        this.m_Types = Types;
      }

      public IFFDataTable() {
        this.Types = null;
      }
    }
    #endregion

    #region Private Members
    private String m_Filename;
    private IFFNode m_Node;
    private IFFDataTable m_DataTable;
    #endregion

    #region Public Members
    public String Filename {
      get {
        return this.m_Filename;
      }
      set {
        this.m_Filename = value;
      }
    }
    public IFFNode Node {
      get {
        return this.m_Node;
      }
      set {
        this.m_Node = value;
      }
    }
    public IFFDataTable DataTable {
      get {
        return this.m_DataTable;
      }
    }
    public Byte[] Bytes {
      get {
        return ParseChild(this.m_Node);
      }
    }
    public Boolean IsDataTable {
      get {
        if (((this.Node != null) && (this.Node.ID == "FORM")) && ((this.Node.Children[0] != null) && (this.Node.Children[0].ID == "DTIIFORM")) && ((this.Node.Children[0].Children[0] != null) && (this.Node.Children[0].Children[0].ID == "0001COLS")) && ((this.Node.Children[0].Children[1] != null) && (this.Node.Children[0].Children[1].ID == "TYPE")) && ((this.Node.Children[0].Children[2] != null) && (this.Node.Children[0].Children[2].ID == "ROWS"))) {
          return true;
        } else {
          return false;
        }
      }
    }
    #endregion

    #region Private Functions
    private void ParseNode(IFFNode iffNode) {
      MemoryStream memoryStream = new MemoryStream(iffNode.Data);
      BinaryReader binaryReader = new BinaryReader(memoryStream);

      while (memoryStream.Position < memoryStream.Length) {
        String ID = new String(binaryReader.ReadChars(4));

        IFFNode iffChild = new IFFNode();
        if (memoryStream.Position < memoryStream.Length) {
          Int32 Length = 0;
          Byte[] Data = new Byte[0];

          try {
            Byte[] buffer1 = binaryReader.ReadBytes(4);
            if (Utilities.IsAlpha(buffer1)) {
              ID += new String(Encoding.ASCII.GetChars(buffer1));
              buffer1 = binaryReader.ReadBytes(4);
            }
            Length = Utilities.EndianFlip(BitConverter.ToInt32(buffer1, 0));
            Data = binaryReader.ReadBytes(Length);
          } catch {
            iffChild.Children = new List<IFFNode>();
            iffChild.Data = null;
            iffChild.ID = ID;
            iffChild.Parent = iffNode;

            iffNode.Children.Add(iffChild);
          }

          iffChild.Children = new List<IFFNode>();
          iffChild.Data = Data;
          iffChild.ID = ID;
          iffChild.Parent = iffNode;

          iffNode.Children.Add(iffChild);

          if (iffChild.ID.Contains("FORM")) {
            ParseNode(iffChild);
          }
        } else {
          iffChild.Children = new List<IFFNode>();
          iffChild.Data = null;
          iffChild.ID = ID;
          iffChild.Parent = iffNode;

          iffNode.Children.Add(iffChild);
        }
      }

      binaryReader.Close();
      memoryStream.Close();
    }

    private void ParseStream(Stream Stream) {
      Stream.Seek(0L, SeekOrigin.Begin);
      BinaryReader binaryReader = new BinaryReader(Stream);

      String ID = new String(binaryReader.ReadChars(4));
      Int32 Length;
      Byte[] Data;

      Byte[] buffer = binaryReader.ReadBytes(4);
      if (Utilities.IsAlpha(buffer)) {
        ID += new String(Encoding.ASCII.GetChars(buffer));
        buffer = binaryReader.ReadBytes(4);
      }

      if (!ID.Contains("FORM")) {
        throw new IOException("File does not contain valid FORM data.");
      }

      Length = Utilities.EndianFlip(BitConverter.ToInt32(buffer, 0));
      Data = binaryReader.ReadBytes((Int32)(Stream.Length - Stream.Position));

      Stream.Seek(0, SeekOrigin.Begin);
      buffer = binaryReader.ReadBytes((Int32)Stream.Length);

      binaryReader.Close();
      Stream.Close();

      IFFNode iffNode = new IFFNode(ID, Data);

      this.m_Node = iffNode;
      ParseNode(iffNode);

      if (this.IsDataTable) {
        this.m_DataTable = new IFFDataTable(this);
      } else {
        this.m_DataTable = null;
      }
    }

    private Byte[] ParseChildren(IFFNode[] iffNodes) {
      List<Byte[]> listByteArrays = new List<Byte[]>();
      foreach (IFFNode iffChunk in iffNodes) {
        listByteArrays.Add(ParseChild(iffChunk));
      }
      listByteArrays.Reverse();

      List<Byte> listBytes = new List<Byte>();
      foreach (Byte[] byteArray in listByteArrays.ToArray()) {
        listBytes.AddRange(byteArray);
      }
      listByteArrays.Clear();

      Byte[] returnValue = listBytes.ToArray();
      listBytes.Clear();

      return returnValue;
    }

    private Byte[] ParseChild(IFFNode iffNode) {
      Byte[] childrenBytes = null;
      if (iffNode.Children.Count > 0) {
        List<IFFNode> tempChildren = new List<IFFNode>();
        tempChildren.AddRange(iffNode.Children.ToArray());
        tempChildren.Reverse();

        childrenBytes = ParseChildren(tempChildren.ToArray());

        tempChildren.Clear();
      }

      Int32 Length = 0;
      if (childrenBytes != null) {
        Length += childrenBytes.Length;
      }
      if ((iffNode.Data != null) && (!iffNode.ID.Contains("FORM"))) {
        Length += iffNode.Data.Length;
      }

      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

      binaryWriter.Write(Encoding.ASCII.GetBytes(iffNode.ID));
      if (Length > 0) {
        binaryWriter.Write(Utilities.EndianFlip(Length));
        if ((iffNode.Data != null) && (!iffNode.ID.Contains("FORM"))) {
          binaryWriter.Write(iffNode.Data);
        }
        if (childrenBytes != null) {
          binaryWriter.Write(childrenBytes);
        }
      }

      Byte[] returnValue = memoryStream.ToArray();
      binaryWriter.Close();
      memoryStream.Close();

      return returnValue;
    }
    #endregion

    #region Public Functions
    public void Save(String Filename) {
      try {
        Directory.CreateDirectory(Filename.Substring(0, Filename.LastIndexOf('\\')));
      } catch {
      }

      FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None);
      BinaryWriter binaryWriter = new BinaryWriter(fileStream);
      if (this.IsDataTable) {
        binaryWriter.Write(this.DataTable.Bytes);
      } else {
        binaryWriter.Write(this.Bytes);
      }
      binaryWriter.Close();
      fileStream.Close();
    }

    public void Open(String Filename) {
      this.m_Filename = Filename;

      FileStream fileStream = new FileStream(Filename, FileMode.Open);
      BinaryReader binaryReader = new BinaryReader(fileStream);
      ParseStream(fileStream);
      binaryReader.Close();
      fileStream.Close();
    }

    public void Open(Byte[] Data) {
      this.m_Filename = String.Empty;

      MemoryStream memoryStream = new MemoryStream(Data);
      ParseStream(memoryStream);
      memoryStream.Close();
    }

    public void Open(Stream Stream) {
      this.m_Filename = String.Empty;

      ParseStream(Stream);
    }

    public void Close() {
      this.m_Filename = String.Empty;
      this.m_Node = null;
    }

    public IFFFile(String Filename) {
      this.Open(Filename);
    }

    public IFFFile(Byte[] Data) {
      this.Open(Data);
    }

    public IFFFile(Stream Stream) {
      this.Open(Stream);
    }

    public IFFFile() {
      this.Close();
    }
    #endregion
  }

  public class STFFile {
    #region Private Structures
    private struct STFString {
      public UInt32 ID;
      public String Name;
      public String Value;
    }
    #endregion

    #region Private Members
    private DataColumn m_DataColumnID;
    private DataColumn m_DataColumnName;
    private DataColumn m_DataColumnValue;
    private DataTable m_DataTable;
    private UInt32 m_Header;
    private Byte m_ByteFlag;
    private String m_Filename;
    #endregion

    #region Public Members
    public String Filename {
      get {
        return this.m_Filename;
      }
      set {
        this.m_Filename = value;
      }
    }
    public DataTable DataTable {
      get {
        return this.m_DataTable;
      }
    }
    public Byte[] Bytes {
      get {
        return GetBytes();
      }
    }
    #endregion

    #region Private Functions
    private void InitializeDataTable() {
      this.m_DataColumnID = new DataColumn();
      this.m_DataColumnID.AllowDBNull = false;
      this.m_DataColumnID.AutoIncrement = false;
      this.m_DataColumnID.Caption = "ID";
      this.m_DataColumnID.ColumnName = "ID";
      this.m_DataColumnID.DataType = typeof(Int32);
      this.m_DataColumnID.Unique = true;

      this.m_DataColumnName = new DataColumn();
      this.m_DataColumnName.AllowDBNull = true;
      this.m_DataColumnName.AutoIncrement = false;
      this.m_DataColumnName.Caption = "Name";
      this.m_DataColumnName.ColumnName = "Name";
      this.m_DataColumnName.DataType = typeof(String);
      this.m_DataColumnName.Unique = false;

      this.m_DataColumnValue = new DataColumn();
      this.m_DataColumnValue.AllowDBNull = true;
      this.m_DataColumnValue.AutoIncrement = false;
      this.m_DataColumnValue.Caption = "Value";
      this.m_DataColumnValue.ColumnName = "Value";
      this.m_DataColumnValue.DataType = typeof(String);
      this.m_DataColumnValue.Unique = false;

      this.m_DataTable = new DataTable("STFFile");
      this.m_DataTable.Columns.AddRange(new DataColumn[] { this.m_DataColumnID, this.m_DataColumnName, this.m_DataColumnValue });
    }

    private void ParseStream(Stream Stream) {
      Stream.Seek(0L, SeekOrigin.Begin);
      BinaryReader binaryReader = new BinaryReader(Stream);
      this.m_Header = binaryReader.ReadUInt32();
      this.m_ByteFlag = binaryReader.ReadByte();
      UInt32 nextIndex = binaryReader.ReadUInt32();
      UInt32 stringCount = binaryReader.ReadUInt32();

      STFString[] tempSTFString = new STFString[stringCount];
      Hashtable tempSTFIndex = new Hashtable();
      for (Int32 counter = 0; counter < stringCount; counter++) {
        UInt32 ID = binaryReader.ReadUInt32();
        binaryReader.ReadInt32();
        String Value = Encoding.ASCII.GetString(Encoding.Convert(Encoding.Unicode, Encoding.ASCII, binaryReader.ReadBytes((Int32)(binaryReader.ReadUInt32() * 2))));

        tempSTFIndex.Add(ID, counter);
        tempSTFString[counter].ID = ID;
        tempSTFString[counter].Value = Value;
      }

      for (Int32 counter = 0; counter < stringCount; counter++) {
        tempSTFString[(Int32)tempSTFIndex[binaryReader.ReadUInt32()]].Name = Encoding.ASCII.GetString(binaryReader.ReadBytes((Int32)binaryReader.ReadUInt32()));
      }
      tempSTFIndex.Clear();

      InitializeDataTable();
      foreach (STFString stfString in tempSTFString) {
        this.m_DataTable.Rows.Add(new Object[] { stfString.ID, stfString.Name, stfString.Value });
      }
      this.m_DataColumnID.AutoIncrement = true;
      this.m_DataColumnID.AutoIncrementSeed = nextIndex;
      this.m_DataColumnID.AutoIncrementStep = 1;

      binaryReader.Close();
    }

    private Byte[] GetBytes() {
      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

      binaryWriter.Write(this.m_Header);
      binaryWriter.Write(this.m_ByteFlag);
      binaryWriter.Write((UInt32)((Int32)this.m_DataTable.Compute("MAX(ID)", String.Empty) + 1));
      binaryWriter.Write((UInt32)this.m_DataTable.Rows.Count);

      foreach (DataRow dataRow in this.m_DataTable.Rows) {
        binaryWriter.Write((UInt32)((Int32)dataRow["ID"]));
        binaryWriter.Write(UInt32.MaxValue);
        binaryWriter.Write((UInt32)dataRow["Value"].ToString().Length);
        binaryWriter.Write(Encoding.Convert(Encoding.ASCII, Encoding.Unicode, Encoding.ASCII.GetBytes((String)dataRow["Value"])));
      }

      foreach (DataRow dataRow in this.m_DataTable.Rows) {
        binaryWriter.Write((UInt32)((Int32)dataRow["ID"]));
        binaryWriter.Write((UInt32)dataRow["Name"].ToString().Length);
        binaryWriter.Write(Encoding.ASCII.GetBytes((String)dataRow["Name"]));
      }

      Byte[] returnValue = memoryStream.ToArray();
      binaryWriter.Close();
      memoryStream.Close();

      return returnValue;
    }
    #endregion

    #region Public Functions
    public void AddString(String Name, String Value) {
      if (this.m_DataTable != null) {
        this.m_DataTable.Rows.Add(new Object[] { ((Int32)this.m_DataTable.Compute("MAX(ID)", String.Empty) + 1), Name, Value });
      }
    }

    public void RemoveString(Int32 ID) {
      if (this.m_DataTable != null) {
        DataView dataView = new DataView(this.m_DataTable);
        dataView.RowFilter = "ID = " + ID;
        if (dataView.Count == 1) {
          this.m_DataTable.Rows.Remove(dataView[0].Row);
        }
      }
    }

    public void ModifyString(Int32 OldID, Int32 NewID, String Name, String Value) {
      if (this.m_DataTable != null) {
        DataView dataView = new DataView(this.m_DataTable);
        dataView.RowFilter = "ID = " + OldID;
        if (dataView.Count == 1) {
          DataRow dataRow = dataView[0].Row;

          dataView.RowFilter = "ID = " + NewID;
          if ((OldID == NewID) || (dataView.Count == 0)) {
            dataRow["ID"] = NewID;
            dataRow["Name"] = Name;
            dataRow["Value"] = Value;
          } else {
            this.m_DataTable.Rows.Remove(dataRow);
            dataRow = dataView[0].Row;

            dataRow["ID"] = NewID;
            dataRow["Name"] = Name;
            dataRow["Value"] = Value;
          }
        }
      }
    }

    public void Save(String Filename) {
      try {
        Directory.CreateDirectory(Filename.Substring(0, Filename.LastIndexOf('\\')));
      } catch {
      }

      FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None);
      BinaryWriter binaryWriter = new BinaryWriter(fileStream);
      binaryWriter.Write(this.Bytes);
      binaryWriter.Close();
      fileStream.Close();
    }

    public void Open(String Filename) {
      this.m_Filename = Filename;

      FileStream fileStream = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
      ParseStream(fileStream);
      fileStream.Close();
    }

    public void Open(Byte[] Data) {
      this.m_Filename = String.Empty;

      MemoryStream memoryStream = new MemoryStream(Data);
      ParseStream(memoryStream);
      memoryStream.Close();
    }

    public void Open(Stream Stream) {
      this.m_Filename = String.Empty;

      ParseStream(Stream);
    }

    public void Close() {
      this.m_Filename = String.Empty;

      this.m_ByteFlag = 0;
      this.m_Header = 0;
      InitializeDataTable();
    }

    public STFFile(String Filename) {
      this.Open(Filename);
    }

    public STFFile(Byte[] Data) {
      this.Open(Data);
    }

    public STFFile(Stream Stream) {
      this.Open(Stream);
    }

    public STFFile() {
      this.Close();
    }
    #endregion
  }

  public class TREFile {
    #region Classes
    public class TREIndex {
      private DataRow m_DataRow;

      public Int32 Checksum {
        get {
          return (Int32)this.m_DataRow["Checksum"];
        }
      }
      public String Filename {
        get {
          return (String)this.m_DataRow["Filename"];
        }
      }
      public Int32 Final_Size {
        get {
          return (Int32)this.m_DataRow["Final_Size"];
        }
      }
      public Int32 Format {
        get {
          return (Int32)this.m_DataRow["Format"];
        }
      }
      public String Name {
        get {
          return (String)this.m_DataRow["Name"];
        }
      }
      public Int32 Offset {
        get {
          return (Int32)this.m_DataRow["Offset"];
        }
      }
      public Int32 Size {
        get {
          return (Int32)this.m_DataRow["Size"];
        }
      }
      public Int32 Uk1 {
        get {
          return (Int32)this.m_DataRow["Uk1"];
        }
      }

      public TREIndex(DataRow dataRow) {
        this.m_DataRow = dataRow;
      }
    }
    #endregion

    #region Private Members
    private DataTable m_DataTable;
    private DataColumn m_DataColumnName;
    private DataColumn m_DataColumnChecksum;
    private DataColumn m_DataColumnFinalSize;
    private DataColumn m_DataColumnOffset;
    private DataColumn m_DataColumnFormat;
    private DataColumn m_DataColumnSize;
    private DataColumn m_DataColumnUk1;
    private DataColumn m_DataColumnFilename;
    private DataColumn m_DataColumnPath;
    private DataColumn m_DataColumnFileType;
    private DataColumn m_DataColumnSearchName;
    private DataColumn m_DataColumnSortName;
    private String m_Filename;
    #endregion

    #region Public Members
    public DataTable DataTable {
      get {
        return this.m_DataTable;
      }
    }
    public String Filename {
      get {
        return this.m_Filename;
      }
    }
    public TREIndex this[Int32 index] {
      get {
        if ((index >= 0) && (index < this.Count)) {
          return new TREIndex(this.m_DataTable.Rows[index]);
        } else {
          throw new ArgumentOutOfRangeException();
        }
      }
    }
    public Int32 Count {
      get {
        return ((this.m_DataTable != null) ? this.m_DataTable.Rows.Count : 0);
      }
    }
    #endregion

    #region Private Functions
    private void InitializeDataTable() {
      this.m_DataColumnChecksum = new DataColumn();
      this.m_DataColumnChecksum.AllowDBNull = false;
      this.m_DataColumnChecksum.AutoIncrement = false;
      this.m_DataColumnChecksum.Caption = "Checksum";
      this.m_DataColumnChecksum.ColumnName = "Checksum";
      this.m_DataColumnChecksum.DataType = typeof(Int32);
      this.m_DataColumnChecksum.Unique = false;

      this.m_DataColumnFilename = new DataColumn();
      this.m_DataColumnFilename.AllowDBNull = false;
      this.m_DataColumnFilename.AutoIncrement = false;
      this.m_DataColumnFilename.Caption = "Filename";
      this.m_DataColumnFilename.ColumnName = "Filename";
      this.m_DataColumnFilename.DataType = typeof(String);
      this.m_DataColumnFilename.Unique = false;

      this.m_DataColumnFinalSize = new DataColumn();
      this.m_DataColumnFinalSize.AllowDBNull = false;
      this.m_DataColumnFinalSize.AutoIncrement = false;
      this.m_DataColumnFinalSize.Caption = "Final Size";
      this.m_DataColumnFinalSize.ColumnName = "Final_Size";
      this.m_DataColumnFinalSize.DataType = typeof(Int32);
      this.m_DataColumnFinalSize.Unique = false;

      this.m_DataColumnFormat = new DataColumn();
      this.m_DataColumnFormat.AllowDBNull = false;
      this.m_DataColumnFormat.AutoIncrement = false;
      this.m_DataColumnFormat.Caption = "Format";
      this.m_DataColumnFormat.ColumnName = "Format";
      this.m_DataColumnFormat.DataType = typeof(Int32);
      this.m_DataColumnFormat.Unique = false;

      this.m_DataColumnName = new DataColumn();
      this.m_DataColumnName.AllowDBNull = false;
      this.m_DataColumnName.AutoIncrement = false;
      this.m_DataColumnName.Caption = "Name";
      this.m_DataColumnName.ColumnName = "Name";
      this.m_DataColumnName.DataType = typeof(String);
      this.m_DataColumnName.Unique = false;

      this.m_DataColumnOffset = new DataColumn();
      this.m_DataColumnOffset.AllowDBNull = false;
      this.m_DataColumnOffset.AutoIncrement = false;
      this.m_DataColumnOffset.Caption = "Offset";
      this.m_DataColumnOffset.ColumnName = "Offset";
      this.m_DataColumnOffset.DataType = typeof(Int32);
      this.m_DataColumnOffset.Unique = false;

      this.m_DataColumnSize = new DataColumn();
      this.m_DataColumnSize.AllowDBNull = false;
      this.m_DataColumnSize.AutoIncrement = false;
      this.m_DataColumnSize.Caption = "Size";
      this.m_DataColumnSize.ColumnName = "Size";
      this.m_DataColumnSize.DataType = typeof(Int32);
      this.m_DataColumnSize.Unique = false;

      this.m_DataColumnUk1 = new DataColumn();
      this.m_DataColumnUk1.AllowDBNull = false;
      this.m_DataColumnUk1.AutoIncrement = false;
      this.m_DataColumnUk1.Caption = "Uk1";
      this.m_DataColumnUk1.ColumnName = "Uk1";
      this.m_DataColumnUk1.DataType = typeof(Int32);
      this.m_DataColumnUk1.Unique = false;

      this.m_DataColumnPath = new DataColumn();
      this.m_DataColumnPath.AllowDBNull = false;
      this.m_DataColumnPath.AutoIncrement = false;
      this.m_DataColumnPath.Caption = "Path";
      this.m_DataColumnPath.ColumnName = "Path";
      this.m_DataColumnPath.DataType = typeof(String);
      this.m_DataColumnPath.Unique = false;

      this.m_DataColumnFileType = new DataColumn();
      this.m_DataColumnFileType.AllowDBNull = false;
      this.m_DataColumnFileType.AutoIncrement = false;
      this.m_DataColumnFileType.Caption = "File Type";
      this.m_DataColumnFileType.ColumnName = "File_Type";
      this.m_DataColumnFileType.DataType = typeof(String);
      this.m_DataColumnFileType.Unique = false;

      this.m_DataColumnSearchName = new DataColumn();
      this.m_DataColumnSearchName.AllowDBNull = true;
      this.m_DataColumnSearchName.AutoIncrement = false;
      this.m_DataColumnSearchName.Caption = "Search Name";
      this.m_DataColumnSearchName.ColumnName = "SearchName";
      this.m_DataColumnSearchName.DataType = typeof(String);
      this.m_DataColumnSearchName.Unique = false;

      this.m_DataColumnSortName = new DataColumn();
      this.m_DataColumnSortName.AllowDBNull = false;
      this.m_DataColumnSortName.AutoIncrement = false;
      this.m_DataColumnSortName.Caption = "Sort Name";
      this.m_DataColumnSortName.ColumnName = "SortName";
      this.m_DataColumnSortName.DataType = typeof(String);
      this.m_DataColumnSortName.Unique = false;

      this.m_DataTable = new DataTable();
      this.m_DataTable.TableName = "TREindex";
      this.m_DataTable.Columns.AddRange(new DataColumn[] { this.m_DataColumnName, this.m_DataColumnChecksum, this.m_DataColumnFinalSize, this.m_DataColumnOffset, this.m_DataColumnFormat, this.m_DataColumnSize, this.m_DataColumnUk1, this.m_DataColumnFilename, this.m_DataColumnPath, this.m_DataColumnFileType, this.m_DataColumnSearchName, this.m_DataColumnSortName });
    }

    private void ParseStreamTOC(Stream Stream) {
      BinaryReader binaryReader = new BinaryReader(Stream);
      String COT1000 = new String(binaryReader.ReadChars(8));
      if (COT1000 != " COT1000") {
        return;
      }
      binaryReader.ReadInt32();
      binaryReader.ReadInt32(); // nameCount
      Int32 indexLength = binaryReader.ReadInt32();
      Int32 namesLength = binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32(); // filenamesCount
      Int32 filenamesLength = binaryReader.ReadInt32();

      Byte[] bufferFilenames = binaryReader.ReadBytes(filenamesLength);
      Byte[] bufferIndex = binaryReader.ReadBytes(indexLength);
      Byte[] bufferNames = binaryReader.ReadBytes(namesLength);
      binaryReader.Close();

      String currentFolder = this.m_Filename.Substring(0, this.m_Filename.LastIndexOf("\\") + 1);

      MemoryStream memoryStream = new MemoryStream(bufferFilenames);
      binaryReader = new BinaryReader(memoryStream);
      List<String> listFilenames = new List<String>();
      while (memoryStream.Position < memoryStream.Length) {
        listFilenames.Add(currentFolder + Utilities.ReadString(binaryReader));
      }
      binaryReader.Close();
      memoryStream.Close();

      memoryStream = new MemoryStream(bufferNames);
      binaryReader = new BinaryReader(memoryStream);
      List<String> listNames = new List<String>();
      while (memoryStream.Position < memoryStream.Length) {
        listNames.Add(Utilities.ReadString(binaryReader));
      }
      binaryReader.Close();
      memoryStream.Close();

      memoryStream = new MemoryStream(bufferIndex);
      binaryReader = new BinaryReader(memoryStream);
      this.m_DataTable.Rows.Clear();
      foreach (String Name in listNames) {
        Int32 Format = binaryReader.ReadInt16();
        String Filename = listFilenames[binaryReader.ReadInt16()];
        binaryReader.ReadInt32();
        binaryReader.ReadInt32();
        Int32 Offset = binaryReader.ReadInt32();
        Int32 Final_Size = binaryReader.ReadInt32();
        Int32 Size = binaryReader.ReadInt32();

        this.m_DataTable.Rows.Add(new Object[] { Name, 0, Final_Size, Offset, Format, Size, 0, Filename, "/" + Name.Substring(0, Name.LastIndexOf("/") + 1), Name.Substring(Name.LastIndexOf(".") + 1).ToUpper() + " File", Name.Substring(Name.LastIndexOf("/") + 1), Name.Substring(Name.LastIndexOf("/") + 1) });
      }

      binaryReader.Close();
      memoryStream.Close();
    }

    private void ParseStreamTRE(Stream Stream) {
      BinaryReader binaryReader = new BinaryReader(Stream);
      String EERT5000 = new String(binaryReader.ReadChars(8));
      if (EERT5000 != "EERT5000") {
        return;
      }
      Int32 namesCount = binaryReader.ReadInt32();
      Int32 tocOffset = binaryReader.ReadInt32();
      Int32 tocFormat = binaryReader.ReadInt32();
      Int32 tocSize = binaryReader.ReadInt32();
      Int32 tocFinal_Size = namesCount * 24;
      Int32 namesFormat = binaryReader.ReadInt32();
      Int32 namesSize = binaryReader.ReadInt32();
      Int32 namesFinal_Size = binaryReader.ReadInt32();

      Byte[] bufferIndex;
      Stream.Seek(tocOffset, SeekOrigin.Begin);
      if (tocFormat == 0) {
        bufferIndex = binaryReader.ReadBytes(tocFinal_Size);
      } else {
        Byte[] buffer = binaryReader.ReadBytes(tocSize);
        Inflater inflater = new Inflater(false);
        inflater.SetInput(buffer);
        bufferIndex = new Byte[tocFinal_Size];
        inflater.Inflate(bufferIndex);
      }
      Byte[] bufferNames;
      if (namesFormat == 0) {
        bufferNames = binaryReader.ReadBytes(namesFinal_Size);
      } else {
        Byte[] buffer = binaryReader.ReadBytes(namesSize);
        Inflater inflater = new Inflater(false);
        inflater.SetInput(buffer);
        bufferNames = new Byte[namesFinal_Size];
        inflater.Inflate(bufferNames);
      }
      binaryReader.Close();

      MemoryStream memoryStream = new MemoryStream(bufferNames);
      binaryReader = new BinaryReader(memoryStream);
      List<String> listNames = new List<String>();
      while (memoryStream.Position < memoryStream.Length) {
        listNames.Add(Utilities.ReadString(binaryReader));
      }
      binaryReader.Close();
      memoryStream.Close();

      memoryStream = new MemoryStream(bufferIndex);
      binaryReader = new BinaryReader(memoryStream);
      this.m_DataTable.Rows.Clear();
      foreach (String Name in listNames) {
        String Filename = this.m_Filename;
        Int32 Checksum = binaryReader.ReadInt32();
        Int32 Final_Size = binaryReader.ReadInt32();
        Int32 Offset = binaryReader.ReadInt32();
        Int32 Format = binaryReader.ReadInt32();
        Int32 Size = binaryReader.ReadInt32();
        Int32 Uk1 = binaryReader.ReadInt32();

        this.m_DataTable.Rows.Add(new Object[] { Name, Checksum, Final_Size, Offset, Format, Size, Uk1, Filename, "/" + Name.Substring(0, Name.LastIndexOf("/") + 1), Name.Substring(Name.LastIndexOf(".") + 1).ToUpper() + " File", Name.Substring(Name.LastIndexOf("/") + 1), Name.Substring(Name.LastIndexOf("/") + 1) });
      }
      binaryReader.Close();
      memoryStream.Close();
    }
    #endregion

    #region Public Functions
    public Byte[] InflateFile(Int32 RowIndex) {
      try {
        return Utilities.InflateFile(this.m_DataTable.Rows[RowIndex]);
      } catch {
        return null;
      }
    }

    public void Close() {
      this.m_Filename = String.Empty;
      InitializeDataTable();
    }

    public void Open(String Filename) {
      this.m_Filename = Filename;
      InitializeDataTable();

      FileStream fileStream = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
      if (Filename.ToLower().EndsWith(".toc")) {
        ParseStreamTOC(fileStream);
      } else if (Filename.ToLower().EndsWith(".tre")) {
        ParseStreamTRE(fileStream);
      }
      fileStream.Close();
    }

    public TREFile(String Filename) {
      this.Open(Filename);
    }

    public TREFile() {
      this.Close();
    }
    #endregion
  }
  
  public class RotationMatrix {
    #region Private Members
    private RotationMatrixRow m_0 = new RotationMatrixRow();
    private RotationMatrixRow m_1 = new RotationMatrixRow();
    private RotationMatrixRow m_2 = new RotationMatrixRow();
    #endregion

    #region Public Members
    public RotationMatrixRow this[Int32 row] {
      get {
        switch (row) {
          case 0:
            return this.m_0;

          case 1:
            return this.m_1;

          case 2:
            return this.m_2;

          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      set {
        switch (row) {
          case 0:
            this.m_0 = value;
            break;

          case 1:
            this.m_1 = value;
            break;

          case 2:
            this.m_2 = value;
            break;

          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
    #endregion

    #region Operators
    public static RotationMatrix operator *(RotationMatrix matrix1, RotationMatrix matrix2) {
      RotationMatrix returnValue = new RotationMatrix();

      returnValue[0][0] = (matrix1[0][0] * matrix2[0][0]) + (matrix1[0][1] * matrix2[1][0]) + (matrix1[0][2] * matrix2[2][0]);
      returnValue[0][1] = (matrix1[0][0] * matrix2[0][1]) + (matrix1[0][1] * matrix2[1][1]) + (matrix1[0][2] * matrix2[2][1]);
      returnValue[0][2] = (matrix1[0][0] * matrix2[0][2]) + (matrix1[0][1] * matrix2[1][2]) + (matrix1[0][2] * matrix2[2][2]);
      returnValue[1][0] = (matrix1[1][0] * matrix2[0][0]) + (matrix1[1][1] * matrix2[1][0]) + (matrix1[1][2] * matrix2[2][0]);
      returnValue[1][1] = (matrix1[1][0] * matrix2[0][1]) + (matrix1[1][1] * matrix2[1][1]) + (matrix1[1][2] * matrix2[2][1]);
      returnValue[1][2] = (matrix1[1][0] * matrix2[0][2]) + (matrix1[1][1] * matrix2[1][2]) + (matrix1[1][2] * matrix2[2][2]);
      returnValue[2][0] = (matrix1[2][0] * matrix2[0][0]) + (matrix1[2][1] * matrix2[1][0]) + (matrix1[2][2] * matrix2[2][0]);
      returnValue[2][1] = (matrix1[2][0] * matrix2[0][1]) + (matrix1[2][1] * matrix2[1][1]) + (matrix1[2][2] * matrix2[2][1]);
      returnValue[2][2] = (matrix1[2][0] * matrix2[0][2]) + (matrix1[2][1] * matrix2[1][2]) + (matrix1[2][2] * matrix2[2][2]);

      return returnValue;
    }
    #endregion

    #region Public Functions
    public RotationMatrix() {
    }

    public RotationMatrix(Single[][] value) {
      if ((value.Length <= 0) || (value.Length > 3)) {
        throw new ArgumentOutOfRangeException();
      } else {
        if ((value[0].Length <= 0) || (value[0].Length > 3) || (value[1].Length <= 0) || (value[1].Length > 3) || (value[2].Length <= 0) || (value[2].Length > 3)) {
          throw new ArgumentOutOfRangeException();
        } else {
          this.m_0 = new RotationMatrixRow(value[0]);
          this.m_1 = new RotationMatrixRow(value[1]);
          this.m_2 = new RotationMatrixRow(value[2]);
        }
      }
    }

    public RotationMatrix(Single M00, Single M01, Single M02, Single M10, Single M11, Single M12, Single M20, Single M21, Single M22) {
      this.m_0[0] = M00;
      this.m_0[1] = M01;
      this.m_0[2] = M02;

      this.m_1[0] = M10;
      this.m_1[1] = M11;
      this.m_1[2] = M12;

      this.m_2[0] = M20;
      this.m_2[1] = M21;
      this.m_2[2] = M22;
    }

    public RotationMatrix(EulerAngles eulerAngles) {
      RotationMatrix rotationMatrix = eulerAngles.ToMatrix();

      for (Int32 row = 0; row < 3; row++) {
        for (Int32 column = 0; column < 3; column++) {
          this[row][column] = rotationMatrix[row][column];
        }
      }
    }

    public EulerAngles ToEulerAngles() {
      EulerAngles returnValue = new EulerAngles();

      Single calculatedXPitch = (Single)Math.Asin(this[1][2]);
      Single calculatedYYaw = 0F;
      Single calculatedZRoll = 0F;

      if (Math.Cos(calculatedXPitch) > 0.001) {
        calculatedYYaw = (Single)(-Math.Atan2(this[0][2], this[2][2]));
        calculatedZRoll = (Single)Math.Atan2(this[1][0], this[1][1]);
      } else {
        calculatedYYaw = 0F;
        calculatedZRoll = (Single)Math.Atan2(-this[0][1], this[0][0]);
      }

      returnValue.xPitch = (Int32)Math.Round(Geometry.RadianToDegree(calculatedXPitch), 0);
      returnValue.yYaw = (Int32)Math.Round(Geometry.RadianToDegree(calculatedYYaw), 0);
      returnValue.zRoll = (Int32)Math.Round(Geometry.RadianToDegree(calculatedZRoll), 0);

      return returnValue;
    }
    #endregion
  }

  public class RotationMatrixRow {
    #region Private Members
    private Single m_0 = 0F;
    private Single m_1 = 0F;
    private Single m_2 = 0F;
    #endregion

    #region Public Members
    public Single this[Int32 column] {
      get {
        switch (column) {
          case 0:
            return this.m_0;

          case 1:
            return this.m_1;

          case 2:
            return this.m_2;

          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      set {
        if ((value > 1) || (value < -1)) {
          throw new ArgumentOutOfRangeException();
        } else {
          switch (column) {
            case 0:
              this.m_0 = value;
              break;

            case 1:
              this.m_1 = value;
              break;

            case 2:
              this.m_2 = value;
              break;

            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
    }
    #endregion

    #region Public Functions
    public RotationMatrixRow() {
    }

    public RotationMatrixRow(Single[] value) {
      if ((value.Length <= 0) || (value.Length > 3)) {
        throw new ArgumentOutOfRangeException();
      } else {
        this.m_0 = value[0];
        this.m_0 = value[1];
        this.m_0 = value[2];
      }
    }
    #endregion
  }
  
  public class EulerAngles {
    #region Private Members
    private Int32 m_xPitch;
    private Int32 m_yYaw;
    private Int32 m_zRoll;
    #endregion

    #region Public Members
    public Int32 xPitch {
      get {
        return this.m_xPitch;
      }
      set {
        this.m_xPitch = value;
      }
    }
    public Int32 yYaw {
      get {
        return this.m_yYaw;
      }
      set {
        this.m_yYaw = value;
      }
    }
    public Int32 zRoll {
      get {
        return this.m_zRoll;
      }
      set {
        this.m_zRoll = value;
      }
    }
    #endregion

    #region Public Functions
    public EulerAngles() {
      this.m_xPitch = 0;
      this.m_yYaw = 0;
      this.m_zRoll = 0;
    }

    public EulerAngles(Int32 yYawDegrees, Int32 xPitchDegrees, Int32 zRollDegrees) {
      this.m_xPitch = xPitchDegrees;
      this.m_yYaw = yYawDegrees;
      this.m_zRoll = zRollDegrees;
    }

    public EulerAngles(Single yYawRadians, Single xPitchRadians, Single zRollRadians) {
      this.m_xPitch = (Int32)Math.Round(Geometry.RadianToDegree(xPitchRadians), 0);
      this.m_yYaw = (Int32)Math.Round(Geometry.RadianToDegree(yYawRadians), 0);
      this.m_zRoll = (Int32)Math.Round(Geometry.RadianToDegree(zRollRadians), 0);
    }

    public EulerAngles(RotationMatrix rotationMatrix) {
      EulerAngles eulerAngles = rotationMatrix.ToEulerAngles();

      this.m_xPitch = eulerAngles.xPitch;
      this.m_yYaw = eulerAngles.yYaw;
      this.m_zRoll = eulerAngles.zRoll;
    }

    public RotationMatrix ToMatrix() {
      RotationMatrix returnValue = new RotationMatrix();

      Single xPitch = Geometry.DegreeToRadian(this.xPitch);
      Single yYaw = Geometry.DegreeToRadian(this.yYaw);
      Single zRoll = Geometry.DegreeToRadian(this.zRoll);

      returnValue[0][0] = (Single)(Math.Cos(yYaw) * Math.Cos(zRoll) + Math.Sin(yYaw) * Math.Sin(xPitch) * Math.Sin(zRoll));
      returnValue[0][1] = (Single)(-Math.Cos(yYaw) * Math.Sin(zRoll) + Math.Sin(yYaw) * Math.Sin(xPitch) * Math.Cos(zRoll));
      returnValue[0][2] = (Single)(Math.Sin(yYaw) * Math.Cos(xPitch));

      returnValue[1][0] = (Single)(Math.Cos(xPitch) * Math.Sin(zRoll));
      returnValue[1][1] = (Single)(Math.Cos(xPitch) * Math.Cos(zRoll));
      returnValue[1][2] = (Single)(-Math.Sin(xPitch));

      returnValue[2][0] = (Single)(-Math.Sin(yYaw) * Math.Cos(zRoll) + Math.Cos(yYaw) * Math.Sin(xPitch) * Math.Sin(zRoll));
      returnValue[2][1] = (Single)(Math.Sin(yYaw) * Math.Sin(zRoll) + Math.Cos(yYaw) * Math.Sin(xPitch) * Math.Cos(zRoll));
      returnValue[2][2] = (Single)(Math.Cos(yYaw) * Math.Cos(xPitch));

      return returnValue;
    }
    #endregion
  }

  public static class Utilities {
    #region Compression Functions
    public static Byte[] InflateFile(DataRow DataRow) {
      try {
        return InflateFile((String)DataRow["Filename"], (Int32)DataRow["Offset"], (Int32)DataRow["Format"], (Int32)DataRow["Final_Size"], (Int32)DataRow["Size"]);
      } catch {
        return null;
      }
    }

    public static Byte[] InflateFile(String Filename, Int32 Offset, Int32 Format, Int32 Final_Size, Int32 Size) {
      String filename = Filename;
      if (!File.Exists(filename)) {
        if (filename.Contains("\\")) {
          filename = filename.Substring(filename.LastIndexOf("\\") + 1);
        }
        if (File.Exists(Path.Combine(Utilities.GetGameFolder(false), filename))) {
          filename = Path.Combine(Utilities.GetGameFolder(false), filename);
        } else if (File.Exists(Path.Combine(Utilities.GetGameFolder(true), filename))) {
          filename = Path.Combine(Utilities.GetGameFolder(true), filename);
        } else {
          return null;
        }
      }
      FileStream fileStream;
      try {
        fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
      } catch {
        return null;
      }

      fileStream.Seek(Offset, SeekOrigin.Begin);
      BinaryReader binaryReader = new BinaryReader(fileStream);

      Byte[] returnValue;
      if (Format == 0) {
        returnValue = binaryReader.ReadBytes(Final_Size);
      } else {
        Byte[] buffer = binaryReader.ReadBytes(Size);
        Inflater inflater = new Inflater(false);
        inflater.SetInput(buffer);
        returnValue = new Byte[Final_Size];
        try {
          inflater.Inflate(returnValue);
        } catch {
          return null;
        }
      }
      binaryReader.Close();
      fileStream.Close();

      return returnValue;
    }
    #endregion

    #region Generic Functions
    public static String GetGameFolder(Boolean UseTestCenter) {
      try {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\StarWarsGalaxies");
        return (String)registryKey.GetValue(((UseTestCenter) ? "TestCenterPath" : "Path"));
      } catch {
        return null;
      }
    }

    public static String SizeToString(Object obj) {
      try {
        if ((obj != null) && ((obj.GetType() == typeof(Int64)) || (obj.GetType() == typeof(Int32)))) {
          String[] unitList = new String[] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
          Int32 unitIndex = 0;
          Double byteCount = 0;
          String byteSuffix = String.Empty;
          if (obj.GetType() == typeof(Int32)) {
            byteCount = (Int32)obj;
          } else {
            byteCount = (Int64)obj;
          }

          while ((byteCount > 999) && (unitIndex < unitList.Length)) {
            byteCount /= 1024;
            unitIndex++;
          }

          String returnValue = String.Empty;
          if ((unitIndex == 0) || (byteCount >= 99.95)) {
            returnValue = byteCount.ToString("F0");
          } else if (byteCount >= 9.995) {
            returnValue = byteCount.ToString("F1");
          } else {
            returnValue = byteCount.ToString("F2");
          }

          return returnValue + " " + unitList[unitIndex];
        } else {
          return "0 bytes";
        }
      } catch {
        return "0 bytes";
      }
    }
    #endregion

    #region Hexadecimal Functions
    public static Boolean IsAlpha(Byte[] data) {
      foreach (Byte character in data) {
        if ((((character <= 0x2f) || (character >= 0x3a)) && ((character <= 0x40) || (character >= 0x5c))) && (character != 0x20)) {
          return false;
        }
      }
      return true;
    }

    public static Boolean IsAlphaList(Byte[] data) {
      if ((data == null) || (data.Length == 0)) {
        return false;
      }

      foreach (Byte character in data) {
        if (((character > 0x01) && (character < 0x2D)) || ((character > 0x39) && (character < 0x41)) || ((character > 0x5A) && (character < 0x5C)) || ((character > 0x5C) && (character < 0x5F)) || ((character > 0x5F) && (character < 0x61)) || (character > 0x7A)) {
          return false;
        }
      }
      return true;
    }

    public static UInt32 EndianFlip(UInt32 unsignedInteger) {
      if (BitConverter.IsLittleEndian) {
        return (UInt32)(((unsignedInteger & 0x000000ff) << 24) + ((unsignedInteger & 0x0000ff00) << 8) + ((unsignedInteger & 0x00ff0000) >> 8) + ((unsignedInteger & 0xff000000) >> 24));
      } else {
        return unsignedInteger;
      }
    }

    public static Int32 EndianFlip(Int32 signedInteger) {
      if (BitConverter.IsLittleEndian) {
        return (Int32)(((signedInteger & 0x000000ff) << 24) + ((signedInteger & 0x0000ff00) << 8) + ((signedInteger & 0x00ff0000) >> 8) + ((signedInteger & 0xff000000) >> 24));
      } else {
        return signedInteger;
      }
    }

    public static String ReadString(BinaryReader binaryReader) {
      String returnValue = String.Empty;
      Byte character;
      while ((character = binaryReader.ReadByte()) != 0) {
        returnValue += (Char)character;
      }
      return returnValue;
    }

    public static String[] ReadStrings(Byte[] buffer, Int32 count) {
      List<String> names = new List<String>();
      String name = String.Empty;
      foreach (Byte character in buffer) {
        if (character == 0) {
          names.Add(name);
          name = String.Empty;
        } else
          name += (Char)character;
      }
      return names.ToArray();
    }
    #endregion
  }
}
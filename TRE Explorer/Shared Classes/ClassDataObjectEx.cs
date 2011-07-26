using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using SWGLib;

namespace TRE_Explorer {
  public class DataObjectEx : DataObject, System.Runtime.InteropServices.ComTypes.IDataObject {
    private static readonly TYMED[] ALLOWED_TYMEDS = new TYMED[] { TYMED.TYMED_HGLOBAL, TYMED.TYMED_ISTREAM, TYMED.TYMED_ENHMF, TYMED.TYMED_MFPICT, TYMED.TYMED_GDI };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct FILEDESCRIPTOR {
      public UInt32 dwFlags;
      public Guid clsid;
      public Size sizel;
      public Point pointl;
      public UInt32 dwFileAttributes;
      public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
      public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
      public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
      public UInt32 nFileSizeHigh;
      public UInt32 nFileSizeLow;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public String cFileName;
    }

    private DataRow[] m_SelectedItems;
    private Int32 m_lindex;
    private String m_BaseFolder;

    public DataObjectEx(DataRow[] selectedItems, String baseFolder) {
      m_SelectedItems = selectedItems;
      m_BaseFolder = baseFolder;
    }

    public DataObjectEx(DataRowCollection selectedItems, String baseFolder) {
      this.m_SelectedItems = new DataRow[selectedItems.Count];
      for (Int32 counter = 0; counter < selectedItems.Count; counter++) {
        this.m_SelectedItems[counter] = selectedItems[counter];
      }
      this.m_BaseFolder = baseFolder;
    }

    public override object GetData(string format, bool autoConvert) {
      if (String.Compare(format, NativeMethods.CFSTR_FILEDESCRIPTORW, StringComparison.OrdinalIgnoreCase) == 0 && m_SelectedItems != null) {
        base.SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, GetFileDescriptor(m_SelectedItems));
      } else if ((String.Compare(format, NativeMethods.CFSTR_FILECONTENTS, StringComparison.OrdinalIgnoreCase) == 0) && m_SelectedItems != null) {
        base.SetData(NativeMethods.CFSTR_FILECONTENTS, GetFileContents(m_SelectedItems, m_lindex));
      } else if (String.Compare(format, NativeMethods.CFSTR_PERFORMEDDROPEFFECT, StringComparison.OrdinalIgnoreCase) == 0) {
        //TODO: Cleanup routines after paste has been performed
      }
      return base.GetData(format, autoConvert);
    }

    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    void System.Runtime.InteropServices.ComTypes.IDataObject.GetData(ref FORMATETC formatetc, out STGMEDIUM medium) {
      if (formatetc.cfFormat == (Int16)DataFormats.GetFormat(NativeMethods.CFSTR_FILECONTENTS).Id)
        m_lindex = formatetc.lindex;

      medium = new System.Runtime.InteropServices.ComTypes.STGMEDIUM();
      if (GetTymedUseable(formatetc.tymed)) {
        if (formatetc.cfFormat == (Int16)DataFormats.GetFormat(NativeMethods.CFSTR_FILECONTENTS).Id) {
          if ((formatetc.tymed & TYMED.TYMED_ISTREAM) != TYMED.TYMED_NULL) {
            medium.tymed = TYMED.TYMED_ISTREAM;
            // medium.unionmember = Marshal.GetComInterfaceForObject(GetFileContents(this.m_SelectedItems, formatetc.lindex), typeof(IStreamWrapper));
            medium.unionmember = NativeMethods.VirtualAlloc(IntPtr.Zero, new UIntPtr(1), NativeMethods.MEM_COMMIT, NativeMethods.PAGE_READWRITE);
            if (medium.unionmember == IntPtr.Zero) {
              throw new OutOfMemoryException();
            }

            try {
              ((System.Runtime.InteropServices.ComTypes.IDataObject)this).GetDataHere(ref formatetc, ref medium);
              return;
            } catch {
              NativeMethods.VirtualFree(medium.unionmember, new UIntPtr(1), NativeMethods.MEM_DECOMMIT);
              medium.unionmember = IntPtr.Zero;
              throw;
            }
          }
        } else {
          if ((formatetc.tymed & TYMED.TYMED_HGLOBAL) != TYMED.TYMED_NULL) {
            medium.tymed = TYMED.TYMED_HGLOBAL;
            medium.unionmember = NativeMethods.GlobalAlloc(NativeMethods.GHND | NativeMethods.GMEM_DDESHARE, 1);
            if (medium.unionmember == IntPtr.Zero) {
              throw new OutOfMemoryException();
            }

            try {
              ((System.Runtime.InteropServices.ComTypes.IDataObject)this).GetDataHere(ref formatetc, ref medium);
              return;
            } catch {
              NativeMethods.GlobalFree(new HandleRef((STGMEDIUM)medium, medium.unionmember));
              medium.unionmember = IntPtr.Zero;
              throw;
            }
          }
        }
        medium.tymed = formatetc.tymed;
        ((System.Runtime.InteropServices.ComTypes.IDataObject)this).GetDataHere(ref formatetc, ref medium);
      } else {
        Marshal.ThrowExceptionForHR(NativeMethods.DV_E_TYMED);
      }
    }

    private static Boolean GetTymedUseable(TYMED tymed) {
      for (Int32 i = 0; i < ALLOWED_TYMEDS.Length; i++) {
        if ((tymed & ALLOWED_TYMEDS[i]) != TYMED.TYMED_NULL) {
          return true;
        }
      }
      return false;
    }

    private MemoryStream GetFileDescriptor(DataRow[] SelectedItems) {
      MemoryStream FileDescriptorMemoryStream = new MemoryStream();
      // Write out the FILEGROUPDESCRIPTOR.cItems value
      FileDescriptorMemoryStream.Write(BitConverter.GetBytes(SelectedItems.Length), 0, sizeof(UInt32));

      FILEDESCRIPTOR FileDescriptor = new FILEDESCRIPTOR();
      foreach (DataRow si in SelectedItems) {
        String FileName = (String)si["Name"];
        if (FileName.StartsWith(m_BaseFolder)) {
          FileName = FileName.Substring(this.m_BaseFolder.Length);
        }
        FileDescriptor.cFileName = FileName.Replace('/', '\\');
        Int64 FileWriteTimeUtc = DateTime.Now.ToFileTimeUtc();
        Int32 FinalSize = Math.Max((Int32)si["Final_Size"], 1);
        FileDescriptor.ftLastWriteTime.dwHighDateTime = (Int32)(FileWriteTimeUtc >> 32);
        FileDescriptor.ftLastWriteTime.dwLowDateTime = (Int32)(FileWriteTimeUtc & 0xFFFFFFFF);
        FileDescriptor.nFileSizeHigh = (UInt32)(FinalSize >> 32);
        FileDescriptor.nFileSizeLow = (UInt32)(FinalSize & 0xFFFFFFFF);
        FileDescriptor.dwFlags = NativeMethods.FD_WRITESTIME | NativeMethods.FD_FILESIZE | NativeMethods.FD_PROGRESSUI;

        // Marshal the FileDescriptor structure into a byte array and write it to the MemoryStream.
        Int32 FileDescriptorSize = Marshal.SizeOf(FileDescriptor);
        IntPtr FileDescriptorPointer = Marshal.AllocHGlobal(FileDescriptorSize);
        Marshal.StructureToPtr(FileDescriptor, FileDescriptorPointer, true);
        Byte[] FileDescriptorByteArray = new Byte[FileDescriptorSize];
        Marshal.Copy(FileDescriptorPointer, FileDescriptorByteArray, 0, FileDescriptorSize);
        Marshal.FreeHGlobal(FileDescriptorPointer);
        FileDescriptorMemoryStream.Write(FileDescriptorByteArray, 0, FileDescriptorByteArray.Length);
      }
      return FileDescriptorMemoryStream;
    }

    private ComIStreamWrapper GetFileContents(DataRow[] SelectedItems, Int32 FileNumber) {
      MemoryStream FileContentMemoryStream = null;
      if (SelectedItems != null && FileNumber < SelectedItems.Length) {
        FileContentMemoryStream = new MemoryStream();
        DataRow si = SelectedItems[FileNumber];

        Byte[] bBuffer = Utilities.InflateFile(si);
        if ((bBuffer == null) || (bBuffer.Length == 0)) {
          bBuffer = new Byte[1];
        }
        FileContentMemoryStream.Write(bBuffer, 0, bBuffer.Length);
      }
      return new ComIStreamWrapper(FileContentMemoryStream);
    }
  }

  public class NativeMethods {
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GlobalAlloc(Int32 uFlags, Int32 dwBytes);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GlobalFree(HandleRef handle);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, UInt32 flAllocationType, UInt32 flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, UInt32 dwFreeType);

    // Clipboard formats used for cut/copy/drag operations
    public const string CFSTR_PREFERREDDROPEFFECT = "Preferred DropEffect";
    public const string CFSTR_PERFORMEDDROPEFFECT = "Performed DropEffect";
    public const string CFSTR_FILEDESCRIPTORW = "FileGroupDescriptorW";
    public const string CFSTR_FILECONTENTS = "FileContents";

    // File Descriptor Flags
    public const Int32 FD_CLSID = 0x00000001;
    public const Int32 FD_SIZEPOINT = 0x00000002;
    public const Int32 FD_ATTRIBUTES = 0x00000004;
    public const Int32 FD_CREATETIME = 0x00000008;
    public const Int32 FD_ACCESSTIME = 0x00000010;
    public const Int32 FD_WRITESTIME = 0x00000020;
    public const Int32 FD_FILESIZE = 0x00000040;
    public const Int32 FD_PROGRESSUI = 0x00004000;
    public const Int32 FD_LINKUI = 0x00008000;

    // Global Memory Flags
    public const Int32 GMEM_MOVEABLE = 0x0002;
    public const Int32 GMEM_ZEROINIT = 0x0040;
    public const Int32 GHND = (GMEM_MOVEABLE | GMEM_ZEROINIT);
    public const Int32 GMEM_DDESHARE = 0x2000;
    public const Int32 MEM_COMMIT = 0x1000;
    public const Int32 MEM_RESERVE = 0x2000;
    public const Int32 MEM_DECOMMIT = 0x4000;
    public const Int32 MEM_RELEASE = 0x8000;
    public const Int32 MEM_RESET = 0x80000;
    public const Int32 MEM_TOP_DOWN = 0x100000;
    public const Int32 PAGE_NOACCESS = 0x01;
    public const Int32 PAGE_READONLY = 0x02;
    public const Int32 PAGE_READWRITE = 0x04;
    public const Int32 PAGE_WRITECOPY = 0x08;
    public const Int32 PAGE_EXECUTE = 0x10;
    public const Int32 PAGE_EXECUTE_READ = 0x20;
    public const Int32 PAGE_EXECUTE_READWRITE = 0x40;
    public const Int32 PAGE_EXECUTE_WRITECOPY = 0x80;

    // IDataObject constants
    public const Int32 DV_E_TYMED = unchecked((Int32)0x80040069);
  }
}

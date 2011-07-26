using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using SWGLib;

namespace TRE_Explorer {
  public partial class FormIFFILFWSEditor : Form {
    #region Form Functions
    public FormIFFILFWSEditor(FormNotifyIcon formNotifyIcon) {
      InitializeComponent();
      this.m_FormNotifyIcon = formNotifyIcon;
      this.m_FormIFFEditorFind = new FormIFFEditorFind(this);
      this.m_FormIFFEditorReplace = new FormIFFEditorReplace(this);
      this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }

    protected override void OnVisibleChanged(EventArgs e) {
      base.OnVisibleChanged(e);

      if (!this.Visible) {
        this.m_IFFFile = null;
        this.dataGridViewIff.DataSource = null;
        this.m_ILFFile = null;
        this.m_WSFile = null;
        if (this.m_DynamicByteProviderWS != null) {
          this.m_DynamicByteProviderWS = null;
        }
        if (this.m_DynamicByteProviderIFF != null) {
          this.m_DynamicByteProviderIFF = null;
        }
        this.treeViewIff.Nodes.Clear();
        this.toolStripButtonIffSave.Enabled = false;
        this.toolStripButtonIffSaveAs.Enabled = false;
      } else {
        this.hexBoxIff_Resize(this.hexBoxIff, new EventArgs());
      }
    }

    private void FormIFFILFWSEditor_HasChangesChanged(object sender, EventArgs e) {
      this.toolStripButtonIffSave.Enabled = this.HasChanges;
      if (this.HasChanges) {
        if (!this.Text.EndsWith("*")) {
          this.Text += "*";
        }
      } else {
        if (this.Text.EndsWith("*")) {
          this.Text = this.Text.Substring(0, this.Text.Length - 1);
        }
      }
    }

    private void FormIFFILFWSEditor_FormClosing(object sender, FormClosingEventArgs e) {
      if ((e.CloseReason != CloseReason.ApplicationExitCall) && (e.CloseReason != CloseReason.WindowsShutDown)) {
        e.Cancel = true;
        if (PromptForChanges()) {
          this.Visible = false;
        }
      } else {
        if (!PromptForChanges()) {
          e.Cancel = true;
        }
      }
    }

    private void FormIFFILFWSEditor_Load(object sender, EventArgs e) {
      this.m_FormIFFEditorFind.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormIFFEditorFind.Height) / 2F);
      this.m_FormIFFEditorFind.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormIFFEditorFind.Width) / 2F);
      this.m_FormIFFEditorReplace.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormIFFEditorReplace.Height) / 2F);
      this.m_FormIFFEditorReplace.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormIFFEditorReplace.Width) / 2F);
      if (this.m_FormILFMatrixCalculator != null) {
        this.m_FormILFMatrixCalculator.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormILFMatrixCalculator.Height) / 2F);
        this.m_FormILFMatrixCalculator.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormILFMatrixCalculator.Width) / 2F);
      }
    }

    private void FormIFFILFWSEditor_Move(object sender, EventArgs e) {
      this.m_FormIFFEditorFind.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormIFFEditorFind.Height) / 2F);
      this.m_FormIFFEditorFind.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormIFFEditorFind.Width) / 2F);
      this.m_FormIFFEditorReplace.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormIFFEditorReplace.Height) / 2F);
      this.m_FormIFFEditorReplace.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormIFFEditorReplace.Width) / 2F);
      if (this.m_FormILFMatrixCalculator != null) {
        this.m_FormILFMatrixCalculator.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormILFMatrixCalculator.Height) / 2F);
        this.m_FormILFMatrixCalculator.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormILFMatrixCalculator.Width) / 2F);
      }
    }

    private void FormIFFILFWSEditor_Resize(object sender, EventArgs e) {
      this.m_FormIFFEditorFind.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormIFFEditorFind.Height) / 2F);
      this.m_FormIFFEditorFind.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormIFFEditorFind.Width) / 2F);
      this.m_FormIFFEditorReplace.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormIFFEditorReplace.Height) / 2F);
      this.m_FormIFFEditorReplace.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormIFFEditorReplace.Width) / 2F);
      if (this.m_FormILFMatrixCalculator != null) {
        this.m_FormILFMatrixCalculator.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormILFMatrixCalculator.Height) / 2F);
        this.m_FormILFMatrixCalculator.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormILFMatrixCalculator.Width) / 2F);
      }
    }
    #endregion

    #region Global Variables
    private FormNotifyIcon m_FormNotifyIcon;

    private FormIFFEditorFind m_FormIFFEditorFind;
    private FormIFFEditorReplace m_FormIFFEditorReplace;
    private FormILFMatrixCalculator m_FormILFMatrixCalculator = new FormILFMatrixCalculator();
    private FormWSItemManager m_FormWSItemManager;
    private FormWSFind m_FormWSFind;

    internal DynamicByteProvider m_DynamicByteProviderIFF;
    private DynamicByteProvider m_DynamicByteProviderWS;
    private IFFFile m_IFFFile;
    private ILFFile m_ILFFile;
    private WSFile m_WSFile;
    private TreeNode m_TreeNodeDragSource = null;
    private TreeNode m_TreeNodeHover = null;
    private InsertLocation m_MarkerPosition = InsertLocation.Above;
    private TreeNode m_TreeNodeDropTarget = null;
    private InsertLocation m_InsertPosition = InsertLocation.Above;
    private DateTime m_HoverStart = DateTime.Now;

    internal Byte[] m_FindBufferIFF;
    internal String m_FindStringIFF;
    internal Boolean m_SearchAllNodes = true;
    internal Boolean m_StringSearch = true;
    internal Boolean m_CaseInsensitive = true;

    private Boolean m_HasChanges = false;

    public delegate void HasChangesChangedEvent(object sender, EventArgs e);
    public event HasChangesChangedEvent HasChangesChanged;

    public Boolean HasChanges {
      get {
        return this.m_HasChanges;
      }
      set {
        if (this.m_HasChanges != value) {
          this.m_HasChanges = value;
          if (this.HasChangesChanged != null) {
            HasChangesChanged(this, new EventArgs());
          }
        }
      }
    }
    #endregion

    #region Helper Functions
    private Boolean CompareByteArrays(Byte[] array1, Byte[] array2) {
      if (array1.Length != array2.Length) {
        return false;
      }

      for (Int32 counter = 0; counter < array1.Length; counter++) {
        if (array1[counter] != array2[counter]) {
          return false;
        }
      }

      return true;
    }

    private Boolean IsChildOf(TreeNode parent, TreeNode child) {
      List<TreeNode> listParents = new List<TreeNode>();
      TreeNode pointer = child;
      while (pointer.Parent != null) {
        pointer = pointer.Parent;
        listParents.Add(pointer);
      }

      return listParents.Contains(parent);
    }

    private void UpdateIlfAngles(RotationMatrix rotationMatrix) {
      EulerAngles eulerAngles = new EulerAngles(rotationMatrix);
      this.textBoxIlfPitch.Text = ((eulerAngles.xPitch > 0) ? "+" : String.Empty) + eulerAngles.xPitch + "°";
      this.textBoxIlfRoll.Text = ((eulerAngles.zRoll > 0) ? "+" : String.Empty) + eulerAngles.zRoll + "°";
      this.textBoxIlfYaw.Text = ((eulerAngles.yYaw > 0) ? "+" : String.Empty) + eulerAngles.yYaw + "°";
    }

    private Int32 FindBytes(Byte[] findArray, Byte[] inArray, Int32 startIndex) {
      String findString = BitConverter.ToString(findArray).Replace('-', ' ');
      String inString = BitConverter.ToString(inArray).Replace('-', ' ');

      Int32 stringIndex = inString.IndexOf(findString, (startIndex * 3));

      if (stringIndex == -1) {
        if (inString.IndexOf(findString) == -1) {
          return -2;
        } else {
          return -1;
        }
      } else {
        return stringIndex / 3;
      }
    }

    private Boolean CompareStringArrays(String[] array1, String[] array2) {
      if ((array1 == null) && (array2 == null)) {
        return true;
      } else if (array1 == null) {
        return false;
      } else if (array2 == null) {
        return false;
      } else if (array1.Length != array2.Length) {
        return false;
      } else {
        for (Int32 counter = 0; counter < array1.Length; counter++) {
          if (array1[counter] != array2[counter]) {
            return false;
          }
        }

        return true;
      }
    }

    internal Boolean PromptForChanges() {
      if ((this.HasChanges) && (this.m_IFFFile != null)) {
        switch (MessageBox.Show("Do you want to save the changes to " + this.m_IFFFile.Filename + "?", "TRE Explorer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
          case DialogResult.Yes:
            this.m_IFFFile.Save(this.m_IFFFile.Filename);
            this.HasChanges = false;
            return true;

          case DialogResult.No:
            this.HasChanges = false;
            return true;

          case DialogResult.Cancel:
            return false;
        }
      } else if ((this.HasChanges) && (this.m_ILFFile != null)) {
        switch (MessageBox.Show("Do you want to save the changes to " + this.m_ILFFile.Filename + "?", "TRE Explorer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
          case DialogResult.Yes:
            this.m_ILFFile.Save(this.m_ILFFile.Filename);
            this.HasChanges = false;
            return true;

          case DialogResult.No:
            this.HasChanges = false;
            return true;

          case DialogResult.Cancel:
            return false;
        }
      } else if ((this.HasChanges) && (this.m_WSFile != null)) {
        switch (MessageBox.Show("Do you want to save the changes to " + this.m_WSFile.Filename + "?", "TRE Explorer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
          case DialogResult.Yes:
            this.m_WSFile.Save(this.m_WSFile.Filename);
            this.HasChanges = false;
            return true;

          case DialogResult.No:
            this.HasChanges = false;
            return true;

          case DialogResult.Cancel:
            return false;
        }
      }

      return true;
    }

    private Int32 FindString(String findText, Byte[] withinByte, Int32 startIndex, Boolean caseInsensitive) {
      Char[] withinText = Encoding.ASCII.GetChars(withinByte);

      if ((startIndex + findText.Length) > withinText.Length) {
        return -1;
      }

      for (Int32 counter1 = startIndex; counter1 < (withinText.Length - findText.Length); counter1++) {
        for (Int32 counter2 = 0; counter2 < findText.Length; counter2++) {
          if ((!caseInsensitive) && (withinText[counter1 + counter2].ToString() != findText[counter2].ToString())) {
            break;
          } else if ((caseInsensitive) && (withinText[counter1 + counter2].ToString().ToLower() != findText[counter2].ToString().ToLower())) {
            break;
          } else if (counter2 == (findText.Length - 1)) {
            return counter1;
          }
        }
      }

      return -1;
    }
    #endregion

    #region Filetype Functions
    internal TreeNode NextNode(TreeNode startingNode, Boolean ignoreChildren) {
      if ((startingNode.Nodes.Count > 0) && (!ignoreChildren)) {
        return startingNode.Nodes[0];
      } else if (startingNode.NextNode != null) {
        return startingNode.NextNode;
      } else if (startingNode.Parent != null) {
        return this.NextNode(startingNode.Parent, true);
      } else {
        return null;
      }
    }

    private TreeNode CreateIFFNode(IFFFile.IFFNode iffNode) {
      TreeNode treeNode = new TreeNode(iffNode.ID);
      if (iffNode.ID.Contains("FORM")) {
        treeNode.ImageIndex = 1;
      } else {
        treeNode.ImageIndex = 0;
      }
      this.treeViewIff.Nodes.Add(treeNode);
      return treeNode;
    }

    private TreeNode CreateIFFNode(IFFFile.IFFNode iffNode, TreeNode treeBranch) {
      TreeNode treeNode = new TreeNode(iffNode.ID);
      if (iffNode.ID.Contains("FORM")) {
        treeNode.ImageIndex = 1;
      } else {
        treeNode.ImageIndex = 0;
      }
      treeBranch.Nodes.Add(treeNode);
      return treeNode;
    }

    internal IFFFile.IFFNode FindIFFNode(TreeNode treeNode) {
      List<Int32> listIndexes = new List<Int32>();
      listIndexes.Add(treeNode.Index);
      while (treeNode.Parent != null) {
        treeNode = treeNode.Parent;
        listIndexes.Add(treeNode.Index);
      }
      listIndexes.RemoveAt(listIndexes.Count - 1);
      listIndexes.Reverse();
      IFFFile.IFFNode iffNode = this.m_IFFFile.Node;
      foreach (Int32 index in listIndexes.ToArray()) {
        iffNode = iffNode.Children[index];
      }
      return iffNode;
    }

    private void IFFFind() {
      if (m_FormIFFEditorFind.ShowDialog() == DialogResult.OK) {
        m_FindBufferIFF = m_FormIFFEditorFind.GetFindBytes();
        m_FindStringIFF = m_FormIFFEditorFind.GetFindString();
        IFFFindNext();
      }
    }

    internal Int32 Find(Byte[] find, Byte[] within, Int32 startIndex) {
      return this.FindBytes(find, within, startIndex);
    }

    internal Int32 Find(String find, Byte[] within, Int32 startIndex, Boolean caseInsensitive) {
      return this.FindString(find, within, startIndex, caseInsensitive);
    }

    private TreeNode FindNextNode(TreeNode startingNode) {
      TreeNode nodePointer = startingNode;
      IFFFile.IFFNode iffPointer = this.FindIFFNode(nodePointer);
      Boolean containsFind = false;
      if ((!iffPointer.ID.Contains("FORM")) && (iffPointer.Data != null)) {
        if (this.m_StringSearch) {
          containsFind = (this.FindString(this.m_FindStringIFF, iffPointer.Data, 0, this.m_CaseInsensitive) >= 0);
        } else {
          containsFind = (this.FindBytes(this.m_FindBufferIFF, iffPointer.Data, 0) >= 0);
        }
      }
      while (((iffPointer.ID.Contains("FORM")) || (iffPointer.Data == null) || (!containsFind)) && (this.NextNode(nodePointer, false) != null)) {
        nodePointer = this.NextNode(nodePointer, false);
        iffPointer = this.FindIFFNode(nodePointer);
        if ((!iffPointer.ID.Contains("FORM")) && (iffPointer.Data != null)) {
          if (this.m_StringSearch) {
            containsFind = (this.FindString(this.m_FindStringIFF, iffPointer.Data, 0, this.m_CaseInsensitive) >= 0);
          } else {
            containsFind = (this.FindBytes(this.m_FindBufferIFF, iffPointer.Data, 0) >= 0);
          }
        } else {
          containsFind = false;
        }
      }

      if ((iffPointer.ID.Contains("FORM")) || (iffPointer.Data == null) || (!containsFind)) {
        switch (MessageBox.Show("End of file reached. Continue at the beginning?", "TRE Explorer", MessageBoxButtons.YesNo, MessageBoxIcon.Information)) {
          case DialogResult.No:
            return null;

          case DialogResult.Yes:
            return this.FindNextNode(this.treeViewIff.Nodes[0]);

          default:
            return null;
        }
      } else {
        return nodePointer;
      }
    }

    internal void IFFFindNext() {
      if ((this.m_FindBufferIFF == null) || (this.m_FindBufferIFF.Length == 0)) {
        IFFFind();
        return;
      }

      TreeNode nodePointer = this.treeViewIff.SelectedNode;
      IFFFile.IFFNode iffPointer = FindIFFNode(nodePointer);
      Int64 result = 0;
      if ((iffPointer.ID.Contains("FORM")) || (iffPointer.Data == null)) {
        result = -1L;
      } else {
        if (this.m_StringSearch) {
          result = (Int64)FindString(this.m_FindStringIFF, this.m_DynamicByteProviderIFF.Bytes.ToArray(), (Int32)(this.hexBoxIff.SelectionStart + this.hexBoxIff.SelectionLength), this.m_CaseInsensitive);
          if (result != -1) {
            Byte[] buffer = new Byte[this.m_FindStringIFF.Length];
            for (Int32 counter = 0; counter < this.m_FindStringIFF.Length; counter++) {
              buffer[counter] = this.m_DynamicByteProviderIFF.Bytes[(Int32)(counter + result)];
            }
            this.hexBoxIff.Find(buffer, result);
          }
        } else {
          result = this.hexBoxIff.Find(this.m_FindBufferIFF, this.hexBoxIff.SelectionStart + this.hexBoxIff.SelectionLength);
        }
      }

      if (result == -1L) {
        if (this.m_SearchAllNodes) {
          if (NextNode(nodePointer, false) != null) {
            nodePointer = FindNextNode(NextNode(nodePointer, false));
            if (nodePointer != null) {
              if (this.treeViewIff.SelectedNode == nodePointer) {
                this.hexBoxIff.Select(0, 0);
                IFFFindNext();
              } else {
                this.treeViewIff.SelectedNode = nodePointer;
                IFFFindNext();
              }
            }
          } else {
            switch (MessageBox.Show("End of file reached. Continue at the beginning?", "TRE Explorer", MessageBoxButtons.YesNo, MessageBoxIcon.Information)) {
              case DialogResult.No:
                break;

              case DialogResult.Yes:
                nodePointer = FindNextNode(this.treeViewIff.Nodes[0]);
                if (nodePointer != null) {
                  if (this.treeViewIff.SelectedNode == nodePointer) {
                    this.hexBoxIff.Select(0, 0);
                    IFFFindNext();
                  } else {
                    this.treeViewIff.SelectedNode = nodePointer;
                    IFFFindNext();
                  }
                }
                break;

              default:
                break;
            }
          }
        } else {
          MessageBox.Show("End of node reached.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }

    internal void IFFLoadFile(String filename) {
      this.treeViewIff.ContextMenuStrip = null;

      if (filename.ToLower().EndsWith(".ilf")) {
        this.ILFLoadFile(filename);
        return;
      }

      if (filename.ToLower().EndsWith(".ws")) {
        this.WSLoadFile(filename);
        return;
      }

      this.m_ILFFile = null;
      this.m_WSFile = null;

      try {
        try {
          this.m_IFFFile = new IFFFile(filename);
        } catch (Exception exception) {
          MessageBox.Show(exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
          this.Visible = false;
          return;
        }

        try {
          this.HasChangesChanged -= new HasChangesChangedEvent(FormIFFILFWSEditor_HasChangesChanged);
        } catch {
        }

        if ((this.m_IFFFile != null) && (this.m_IFFFile.Filename != null) && (this.m_IFFFile.Filename != String.Empty)) {
          if (!this.m_IFFFile.Filename.Contains("\\")) {
            this.m_IFFFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_IFFFile.Filename);
          } else if (this.m_IFFFile.Filename.StartsWith(Path.GetTempPath())) {
            this.m_IFFFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_IFFFile.Filename.Substring(Path.GetTempPath().Length));
          }
        }

        this.splitContainerIff.Visible = false;
        this.dataGridViewIff.Visible = false;
        this.panelIlf.Visible = false;
        this.panelWs.Visible = false;
        this.hexBoxIff.Visible = false;
        this.m_ILFFile = null;
        this.hexBoxIff.ByteProvider = null;
        this.m_DynamicByteProviderIFF = null;
        this.toolStripSeparator3.Visible = false;
        this.toolStripTextBoxSearch.Visible = false;
        this.toolStripButtonSearch.Visible = false;

        if (this.m_IFFFile.IsDataTable) {
          this.dataGridViewIff.DataSource = this.m_IFFFile.DataTable;
          this.dataGridViewIff.Visible = true;
        } else {
          this.treeViewIff.ContextMenuStrip = this.contextMenuStripTreeViewIFF;
          
          this.treeViewIff.Nodes.Clear();
          IFFRecurseNode(this.m_IFFFile.Node, CreateIFFNode(this.m_IFFFile.Node));

          this.splitContainerIff.Visible = true;
          this.hexBoxIff.Visible = true;

          this.treeViewIff.SelectedNode = this.treeViewIff.Nodes[0];
        }

        this.toolStripButtonIffSaveAs.Enabled = true;

        this.HasChanges = false;
        this.HasChangesChanged += new HasChangesChangedEvent(FormIFFILFWSEditor_HasChangesChanged);

        this.Focus();
      } catch (Exception exception) {
        MessageBox.Show(exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.Visible = false;
      }
    }

    private void IFFRecurseNode(IFFFile.IFFNode iffNode, TreeNode treeNode) {
      this.treeViewIff.AfterSelect -= new TreeViewEventHandler(treeViewIff_AfterSelect);
      this.treeViewIff.SuspendLayout();
      foreach (IFFFile.IFFNode iffChild in iffNode.Children) {
        if (iffChild.Children.Count > 0) {
          IFFRecurseNode(iffChild, CreateIFFNode(iffChild, treeNode));
        } else {
          CreateIFFNode(iffChild, treeNode);
        }
      }
      this.treeViewIff.ResumeLayout(false);
      this.treeViewIff.AfterSelect += new TreeViewEventHandler(treeViewIff_AfterSelect);
    }

    internal void IFFReplaceFindNext(Byte[] findBytes) {
      m_FindBufferIFF = findBytes;
      IFFFindNext();
    }

    private void ILFLoadFile(String filename) {
      this.treeViewIff.ContextMenuStrip = null;

      try {
        this.HasChangesChanged -= new HasChangesChangedEvent(FormIFFILFWSEditor_HasChangesChanged);
      } catch {
      }

      this.splitContainerIff.Visible = false;
      this.dataGridViewIff.Visible = false;
      this.panelIlf.Visible = false;
      this.panelWs.Visible = false;
      this.hexBoxIff.Visible = false;
      this.m_IFFFile = null;
      this.m_WSFile = null;

      try {
        this.m_ILFFile = new ILFFile(filename);

        if ((this.m_ILFFile != null) && (this.m_ILFFile.Filename != null) && (this.m_ILFFile.Filename != String.Empty)) {
          if (!this.m_ILFFile.Filename.Contains("\\")) {
            this.m_ILFFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_ILFFile.Filename);
          } else if (this.m_ILFFile.Filename.StartsWith(Path.GetTempPath())) {
            this.m_ILFFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_ILFFile.Filename.Substring(Path.GetTempPath().Length));
          }
        }

        this.treeViewIff.SuspendLayout();
        this.treeViewIff.Nodes.Clear();
        this.treeViewIff.Nodes.Add("INLYFORM", "INLYFORM");
        this.treeViewIff.Nodes[0].ImageIndex = 1;

        foreach (ILFFile.ILFNode ilfChunk in this.m_ILFFile.Nodes) {
          TreeNode treeNode = new TreeNode(ilfChunk.Object);
          treeNode.Name = ilfChunk.Object;
          treeNode.Tag = ilfChunk.Object;
          this.treeViewIff.Nodes[0].Nodes.Add(treeNode);
        }
        this.treeViewIff.ResumeLayout(false);
        this.splitContainerIff.Visible = true;
        this.panelIlf.Visible = true;

        this.toolStripButtonIffSaveAs.Enabled = true;

        this.HasChanges = false;
        this.HasChangesChanged += new HasChangesChangedEvent(FormIFFILFWSEditor_HasChangesChanged);
      } catch {
      }
    }

    private TreeNode WSFindTreeNode(String ID, TreeNode treeBranch) {
      foreach (TreeNode treeNode in treeBranch.Nodes) {
        if (treeNode.Name == ID) {
          return treeNode;
        }
        if (treeNode.Nodes.Count > 0) {
          TreeNode returnValue = WSFindTreeNode(ID, treeNode);
          if (returnValue != null) {
            return returnValue;
          }
        }
      }
      return null;
    }

    private void WSLoadFile(String filename) {
      this.treeViewIff.ContextMenuStrip = null;

      this.splitContainerIff.Visible = false;
      this.dataGridViewIff.Visible = false;
      this.panelIlf.Visible = false;
      this.hexBoxIff.Visible = false;
      this.m_IFFFile = null;
      this.m_ILFFile = null;

      this.m_WSFile = new WSFile(filename);

      try {
        this.HasChangesChanged -= new HasChangesChangedEvent(FormIFFILFWSEditor_HasChangesChanged);
      } catch {
      }

      if ((this.m_WSFile != null) && (this.m_WSFile.Filename != null) && (this.m_WSFile.Filename != String.Empty)) {
        if (!this.m_WSFile.Filename.Contains("\\")) {
          this.m_WSFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_WSFile.Filename);
        } else if (this.m_WSFile.Filename.StartsWith(Path.GetTempPath())) {
          this.m_WSFile.Filename = Path.Combine(this.m_FormNotifyIcon.saveFileDialog.InitialDirectory, this.m_WSFile.Filename.Substring(Path.GetTempPath().Length));
        }
      }

      this.comboBoxWsObject.SuspendLayout();
      this.comboBoxWsObject.Items.Clear();
      this.comboBoxWsObject.Items.AddRange(this.m_WSFile.Types);
      this.comboBoxWsObject.ResumeLayout(false);

      this.treeViewIff.SuspendLayout();
      this.treeViewIff.Nodes.Clear();
      TreeNode treeNode = new TreeNode();
      treeNode.Name = "WSNPFORM";
      treeNode.Text = "WSNPFORM";
      treeNode.ImageIndex = 1;
      this.treeViewIff.Nodes.Add(treeNode);
      this.WSRecurseNode(m_WSFile.Nodes.ToArray(), this.treeViewIff.Nodes[0]);
      this.treeViewIff.ResumeLayout(false);
      this.splitContainerIff.Visible = true;
      this.panelWs.Visible = true;
      this.toolStripButtonIffManageItems.Visible = true;

      this.toolStripButtonIffSaveAs.Enabled = true;

      this.HasChanges = false;
      this.HasChangesChanged += new HasChangesChangedEvent(FormIFFILFWSEditor_HasChangesChanged);
    }

    internal static String WSObjectType(String filename) {
      if (filename.StartsWith("object/building/")) {
        return "Building";
      }
      if (filename.StartsWith("object/cell/")) {
        return "Cell";
      }
      if (filename.StartsWith("object/soundobject/")) {
        return "Sound";
      }
      if (filename.StartsWith("object/static/creature/")) {
        return "Creature";
      }
      if (filename.StartsWith("object/static/flora/")) {
        return "Flora";
      }
      if (filename.StartsWith("object/static/installation/")) {
        return "Installation";
      }
      if (filename.StartsWith("object/static/item/")) {
        return "Item";
      }
      if (filename.StartsWith("object/static/particle/")) {
        return "Particle";
      }
      if (filename.StartsWith("object/static/structure/")) {
        return "Structure";
      }
      if (filename.StartsWith("object/static/terrain/")) {
        return "Terrain";
      }
      if (filename.StartsWith("object/static/vehicle/")) {
        return "Vehicle";
      }
      if (filename.StartsWith("object/tangible/camp/")) {
        return "Camp";
      }
      if (filename.StartsWith("object/tangible/collection/")) {
        return "Collection";
      }
      if (filename.StartsWith("object/tangible/crafting/station/")) {
        return "Crafting Station";
      }
      if (filename.StartsWith("object/tangible/furniture/")) {
        return "Furniture";
      }
      if (filename.StartsWith("object/tangible/gravestone/")) {
        return "Gravestone";
      }
      if (filename.StartsWith("object/tangible/instrument/")) {
        return "Instrument";
      }
      if (filename.StartsWith("object/tangible/microphone/")) {
        return "Microphone";
      }
      if (filename.StartsWith("object/tangible/sign/")) {
        return "Sign";
      }
      if (filename.StartsWith("object/tangible/speaker/")) {
        return "Speaker";
      }
      if (filename.StartsWith("object/tangible/terminal/")) {
        return "Terminal";
      }
      if (filename.StartsWith("object/tangible/quest/")) {
        return "Quest";
      }
      return String.Empty;
    }

    private void WSRecurseNode(WSFile.WSNode[] wsNodes, TreeNode treeBranch) {
      foreach (WSFile.WSNode wsNode in wsNodes) {
        String ObjectType = WSObjectType(this.m_WSFile.Types[wsNode.ObjectIndex]);
        TreeNode treeNode = new TreeNode();
        treeNode.Name = wsNode.ID.ToString();
        treeNode.Text = wsNode.ID.ToString() + ((ObjectType != String.Empty) ? " [" + ObjectType + "]" : "");
        treeNode.Tag = wsNode.ObjectIndex;
        if (wsNode.ParentID == 0) {
          treeBranch.Nodes.Add(treeNode);
        } else {
          List<Int32> listID = new List<Int32>();
          WSFile.WSNode tempWSNode = wsNode;
          while ((tempWSNode != null) && (tempWSNode.ParentID != 0)) {
            listID.Add(tempWSNode.ParentID);
            tempWSNode = m_WSFile.FindNodeByID(tempWSNode.ParentID);
          }
          listID.Reverse();
          TreeNode tempTreeNode = this.treeViewIff.Nodes[0];
          foreach (Int32 ID in listID) {
            tempTreeNode = tempTreeNode.Nodes[tempTreeNode.Nodes.IndexOfKey(ID.ToString())];
          }
          tempTreeNode.Nodes.Add(treeNode);
        }

        if (wsNode.Nodes.Count > 0) {
          treeNode.ImageIndex = 1;
          this.WSRecurseNode(wsNode.Nodes.ToArray(), treeNode);
        } else {
          treeNode.ImageIndex = 0;
        }
      }
    }
    #endregion

    #region ToolStripIFFEditor Functions
    private void toolStripButtonIffCopy_Click(object sender, EventArgs e) {
      try {
        this.hexBoxIff.Focus();

        if (this.hexBoxIff.SelectionLength > 0) {
          String copy = String.Empty;

          for (Int32 counter = 0; counter < this.hexBoxIff.SelectionLength; counter++) {
            Byte buffer = this.m_DynamicByteProviderIFF.Bytes[counter + (Int32)this.hexBoxIff.SelectionStart];
            if ((buffer <= 0x09) || (buffer == 0x0b) || (buffer == 0x0c) || ((buffer >= 0x0e) && (buffer <= 0x1f)) || (buffer >= 0x7f)) {
              copy += "<bh:" + BitConverter.ToString(new Byte[] { buffer }).ToLower() + ">";
            } else {
              copy += new String(Encoding.ASCII.GetChars(new Byte[] { buffer }));
            }
          }

          Clipboard.SetText(copy);
        }
      } catch {
      }
    }

    private void toolStripButtonIffCut_Click(object sender, EventArgs e) {
      try {
        this.hexBoxIff.Focus();

        if (this.hexBoxIff.SelectionLength > 0) {
          String copy = String.Empty;

          for (Int32 counter = 0; counter < this.hexBoxIff.SelectionLength; counter++) {
            Byte buffer = this.m_DynamicByteProviderIFF.Bytes[counter + (Int32)this.hexBoxIff.SelectionStart];
            if ((buffer <= 0x09) || (buffer == 0x0b) || (buffer == 0x0c) || ((buffer >= 0x0e) && (buffer <= 0x1f)) || (buffer >= 0x7f)) {
              copy += "<bh:" + BitConverter.ToString(new Byte[] { buffer }).ToLower() + ">";
            } else {
              copy += new String(Encoding.ASCII.GetChars(new Byte[] { buffer }));
            }
          }

          Clipboard.SetText(copy);

          Int64 length = this.hexBoxIff.SelectionLength;
          this.hexBoxIff.SelectionLength = 0;
          this.m_DynamicByteProviderIFF.DeleteBytes(this.hexBoxIff.SelectionStart, length);
        }
      } catch {
      }
    }

    private void toolStripButtonIffExit_Click(object sender, EventArgs e) {
      if (PromptForChanges()) {
        this.Visible = false;
      }
    }

    private void toolStripButtonIffFind_Click(object sender, EventArgs e) {
      if (this.m_IFFFile != null) {
        // if ((this.treeViewIff.SelectedNode != null) && (!this.treeViewIff.SelectedNode.Text.Contains("FORM"))) {
        if (this.treeViewIff.SelectedNode != null) {
          IFFFind();
        } else {
          MessageBox.Show("You must first select a node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      } else if (this.m_WSFile != null) {
        this.m_FormWSFind = new FormWSFind(this.m_WSFile);
        this.m_FormWSFind.Top = this.Top + ((this.Height - this.m_FormWSFind.Height) / 2);
        this.m_FormWSFind.Left = this.Left + ((this.Width - this.m_FormWSFind.Width) / 2);
        if (this.m_FormWSFind.ShowDialog() == DialogResult.OK) {
          TreeNode treeNode = WSFindTreeNode(this.m_FormWSFind.SelectedID, this.treeViewIff.Nodes[0]);
          if (treeNode != null) {
            this.treeViewIff.SelectedNode = treeNode;
          }
        }
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripButtonIffInsert_Click(object sender, EventArgs e) {
      this.hexBoxIff.Focus();
      SendKeys.Send("{INSERT}");
    }

    private void toolStripButtonIffPaste_Click(object sender, EventArgs e) {
      try {
        this.hexBoxIff.Focus();

        String paste = Clipboard.GetText();
        List<Byte> buffer = new List<Byte>();
        Int32 counter = 0;
        while (counter < paste.Length) {
          if (paste.Substring(counter, 7).StartsWith("<bh:")) {
            buffer.Add(Convert.ToByte(paste.Substring(counter + 4, 2), 16));
            counter += 7;
          } else {
            buffer.Add(Encoding.ASCII.GetBytes(paste.Substring(counter++, 1))[0]);
          }
        }

        Int32 length = (Int32)this.hexBoxIff.SelectionLength;
        this.hexBoxIff.SelectionLength = 0;
        this.m_DynamicByteProviderIFF.DeleteBytes(this.hexBoxIff.SelectionStart, length);
        this.m_DynamicByteProviderIFF.InsertBytes(this.hexBoxIff.SelectionStart, buffer.ToArray());
        this.hexBoxIff.SelectionLength = buffer.Count;
      } catch {
      }
    }

    private void toolStripButtonIffReplace_Click(object sender, EventArgs e) {
      if (this.m_IFFFile != null) {
        if (this.treeViewIff.SelectedNode != null) {
          Boolean caseInsensitive = this.m_CaseInsensitive;
          Boolean searchAllNodes = this.m_SearchAllNodes;
          Boolean stringSearch = this.m_StringSearch;

          this.m_FormIFFEditorReplace.tabControl.SelectedTab = ((this.m_StringSearch) ? this.m_FormIFFEditorReplace.tabPageString : this.m_FormIFFEditorReplace.tabPageHexadecimal);
          this.m_FormIFFEditorReplace.checkBoxMatchCase.Checked = !this.m_CaseInsensitive;
          this.m_FormIFFEditorReplace.comboBoxSearchIn.SelectedIndex = this.m_FormIFFEditorReplace.comboBoxSearchIn.Items.IndexOf(((this.m_SearchAllNodes) ? "All Nodes" : "This Node"));

          this.m_FormIFFEditorReplace.ShowDialog();

          this.m_CaseInsensitive = caseInsensitive;
          this.m_SearchAllNodes = searchAllNodes;
          this.m_StringSearch = stringSearch;
        } else {
          MessageBox.Show("You must first select a node to search.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripButtonIffSaveAs_Click(object sender, EventArgs e) {
      if (this.m_ILFFile != null) {
        String saveFilename = this.m_ILFFile.Filename.Substring(this.m_ILFFile.Filename.LastIndexOf("\\") + 1);

        this.m_FormNotifyIcon.saveFileDialog.FileName = saveFilename;
        this.m_FormNotifyIcon.saveFileDialog.Filter = saveFilename.Substring(saveFilename.LastIndexOf(".") + 1).ToUpper() + " Files|*." + saveFilename.Substring(saveFilename.LastIndexOf(".") + 1).ToUpper();
        this.m_FormNotifyIcon.saveFileDialog.FilterIndex = 1;

        if (this.m_FormNotifyIcon.saveFileDialog.ShowDialog() == DialogResult.OK) {
          this.m_ILFFile.Save(this.m_FormNotifyIcon.saveFileDialog.FileName);
          this.HasChanges = false;

          this.m_ILFFile.Filename = this.m_FormNotifyIcon.saveFileDialog.FileName;
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_ILFFile.Filename;
        }
      } else if (this.m_WSFile != null) {
        String saveFilename = this.m_WSFile.Filename.Substring(this.m_WSFile.Filename.LastIndexOf("\\") + 1);

        this.m_FormNotifyIcon.saveFileDialog.FileName = saveFilename;
        this.m_FormNotifyIcon.saveFileDialog.Filter = saveFilename.Substring(saveFilename.LastIndexOf(".") + 1).ToUpper() + " Files|*." + saveFilename.Substring(saveFilename.LastIndexOf(".") + 1).ToUpper();
        this.m_FormNotifyIcon.saveFileDialog.FilterIndex = 1;

        if (this.m_FormNotifyIcon.saveFileDialog.ShowDialog() == DialogResult.OK) {
          this.m_WSFile.Save(this.m_FormNotifyIcon.saveFileDialog.FileName);
          this.HasChanges = false;

          this.m_WSFile.Filename = this.m_FormNotifyIcon.saveFileDialog.FileName;
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_WSFile.Filename;
        }
      } else if (this.m_IFFFile != null) {
        String saveFilename = this.m_IFFFile.Filename.Substring(this.m_IFFFile.Filename.LastIndexOf("\\") + 1);

        this.m_FormNotifyIcon.saveFileDialog.FileName = saveFilename;
        this.m_FormNotifyIcon.saveFileDialog.Filter = saveFilename.Substring(saveFilename.LastIndexOf(".") + 1).ToUpper() + " Files|*." + saveFilename.Substring(saveFilename.LastIndexOf(".") + 1).ToUpper();
        this.m_FormNotifyIcon.saveFileDialog.FilterIndex = 1;

        if (this.m_FormNotifyIcon.saveFileDialog.ShowDialog() == DialogResult.OK) {
          if ((this.m_DynamicByteProviderIFF != null) && (this.m_DynamicByteProviderIFF.HasChanges()) && (this.treeViewIff.SelectedNode != null)) {
            IFFFile.IFFNode iffNode = FindIFFNode(this.treeViewIff.SelectedNode);
            iffNode.Data = this.m_DynamicByteProviderIFF.Bytes.ToArray();
          }

          this.m_IFFFile.Save(this.m_FormNotifyIcon.saveFileDialog.FileName);
          this.HasChanges = false;

          this.m_IFFFile.Filename = this.m_FormNotifyIcon.saveFileDialog.FileName;
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_IFFFile.Filename;
        }
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripButtonIffFind_VisibleChanged(object sender, EventArgs e) {
      this.findToolStripMenuItem.Enabled = (this.toolStripButtonIffFind.Visible || this.toolStripButtonSearch.Visible);
      this.findAgainToolStripMenuItem.Enabled = (this.toolStripButtonIffFind.Visible || this.toolStripButtonSearch.Visible);
    }

    private void toolStripButtonIffReplace_VisibleChanged(object sender, EventArgs e) {
      this.replaceToolStripMenuItem.Enabled = this.toolStripButtonIffReplace.Visible;
      this.selectAllToolStripMenuItem.Enabled = this.toolStripButtonIffReplace.Visible;
      this.selectNoneToolStripMenuItem.Enabled = this.toolStripButtonIffReplace.Visible;
    }

    private void toolStripButtonIffCut_VisibleChanged(object sender, EventArgs e) {
      this.cutToolStripMenuItem.Enabled = this.toolStripButtonIffCut.Visible;
    }

    private void toolStripButtonIffCopy_VisibleChanged(object sender, EventArgs e) {
      this.copyToolStripMenuItem.Enabled = this.toolStripButtonIffCopy.Visible;
      this.selectAllToolStripMenuItem.Enabled = this.toolStripButtonIffCopy.Visible;
      this.selectNoneToolStripMenuItem.Enabled = this.toolStripButtonIffCopy.Visible;
      this.findAgainToolStripMenuItem.Enabled = this.toolStripButtonIffCopy.Visible;
    }

    private void toolStripButtonIffPaste_VisibleChanged(object sender, EventArgs e) {
      this.pasteToolStripMenuItem.Enabled = this.toolStripButtonIffPaste.Visible;
    }

    private void toolStripButtonIffAddNode_Click(object sender, EventArgs e) {
      if (this.m_ILFFile != null) {
        ILFFile.ILFNode ilfNode = new ILFFile.ILFNode();
        ilfNode.Cell = "r1";
        ilfNode.Object = "New" + this.treeViewIff.Nodes[0].Nodes.Count;
        ilfNode.W1 = 0;
        ilfNode.W2 = 0;
        ilfNode.W3 = 0;
        ilfNode.RotationMatrix = new RotationMatrix(new EulerAngles(0, 0, 0));
        this.m_ILFFile.Nodes.Add(ilfNode);
        TreeNode treeNode = new TreeNode("New" + this.treeViewIff.Nodes[0].Nodes.Count);
        treeNode.Name = "New" + this.treeViewIff.Nodes[0].Nodes.Count;
        treeNode.Tag = "New" + this.treeViewIff.Nodes[0].Nodes.Count;
        this.treeViewIff.Nodes[0].Nodes.Add(treeNode);

        this.HasChanges = true;
      } else if (this.m_IFFFile != null) {
        if (this.treeViewIff.SelectedNode != null) {
          IFFFile.IFFNode iffNode = FindIFFNode(this.treeViewIff.SelectedNode);
          if (iffNode.ID.Contains("FORM")) {
            IFFFile.IFFNode newiffNode = new IFFFile.IFFNode("NWND", new Byte[0], iffNode, null);
            iffNode.Children.Add(newiffNode);
            TreeNode treeNode = new TreeNode("NWND");
            this.treeViewIff.SelectedNode.Nodes.Add(treeNode);

            this.HasChanges = true;
          } else {
            MessageBox.Show("You may only add nodes to FORM nodes.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        } else {
          MessageBox.Show("You must first select a node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      } else if (this.m_WSFile != null) {
        if (this.treeViewIff.SelectedNode != null) {
          if (this.treeViewIff.SelectedNode.Name == "WSNPFORM") {
            WSFile.WSNode wsNode = new WSFile.WSNode(this.m_WSFile.NextAvailableID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new Byte[4]);
            this.m_WSFile.Nodes.Add(wsNode);

            String ObjectType = WSObjectType(this.m_WSFile.Types[wsNode.ObjectIndex]);

            TreeNode treeNode = new TreeNode();
            treeNode.Name = wsNode.ID.ToString();
            treeNode.Text = wsNode.ID.ToString() + ((ObjectType != String.Empty) ? " [" + ObjectType + "]" : "");
            treeNode.Tag = wsNode.ObjectIndex;
            this.treeViewIff.Nodes[0].Nodes.Add(treeNode);

            this.HasChanges = true;
          } else {
            WSFile.WSNode wsParent = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
            if (!this.CompareByteArrays(wsParent.POBCRC, new Byte[4])) {
              WSFile.WSNode wsNode = new WSFile.WSNode(this.m_WSFile.NextAvailableID, wsParent.ID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new Byte[4]);
              wsNode.Parent = wsParent;
              wsParent.Nodes.Add(wsNode);

              String ObjectType = WSObjectType(this.m_WSFile.Types[wsNode.ObjectIndex]);

              TreeNode treeNode = new TreeNode();
              treeNode.Name = wsNode.ID.ToString();
              treeNode.Text = wsNode.ID.ToString() + ((ObjectType != String.Empty) ? " [" + ObjectType + "]" : "");
              treeNode.Tag = wsNode.ObjectIndex;
              this.treeViewIff.SelectedNode.Nodes.Add(treeNode);

              this.HasChanges = true;
            } else {
              MessageBox.Show("You may only add nodes to the WSNPFORM node or nodes with valid POB values.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          }
        } else {
          MessageBox.Show("You must first select a node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void toolStripButtonIffRemoveNode_Click(object sender, EventArgs e) {
      if (this.m_ILFFile != null) {
        if (this.treeViewIff.SelectedNode != null) {
          if (this.treeViewIff.SelectedNode == this.treeViewIff.Nodes[0]) {
            MessageBox.Show("You may not remove the root node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
          } else {
            TreeNode treeNode = this.treeViewIff.SelectedNode;
            this.m_ILFFile.Nodes.RemoveAt(treeNode.Index);
            if (this.treeViewIff.Nodes[0].Nodes.Count == 1) {
              this.treeViewIff.SelectedNode = this.treeViewIff.Nodes[0];
            } else {
              if (treeNode.Index == 0) {
                this.treeViewIff.SelectedNode = this.treeViewIff.Nodes[0].Nodes[1];
              } else if (treeNode.Index == (this.treeViewIff.Nodes[0].Nodes.Count - 1)) {
                this.treeViewIff.SelectedNode = this.treeViewIff.Nodes[0].Nodes[(this.treeViewIff.Nodes[0].Nodes.Count - 2)];
              } else {
                this.treeViewIff.SelectedNode = this.treeViewIff.Nodes[0].Nodes[(treeNode.Index + 1)];
              }
            }
            treeNode.Remove();

            this.HasChanges = true;
          }
        } else {
          MessageBox.Show("You must first select a node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      } else if (this.m_IFFFile != null) {
        if ((this.treeViewIff.SelectedNode != null)) {
          if (this.treeViewIff.SelectedNode == this.treeViewIff.Nodes[0]) {
            MessageBox.Show("You may not remove the root node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
          } else {
            IFFFile.IFFNode iffNode = FindIFFNode(this.treeViewIff.SelectedNode);
            IFFFile.IFFNode iffParent = iffNode.Parent;
            iffParent.Children.Remove(iffNode);
            iffNode.Parent = null;
            TreeNode treeNode = this.treeViewIff.SelectedNode;
            TreeNode treeParent = this.treeViewIff.SelectedNode.Parent;
            this.treeViewIff.SelectedNode = treeParent;
            treeNode.Remove();

            this.HasChanges = true;
          }
        } else {
          MessageBox.Show("You must first select a node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      } else if (this.m_WSFile != null) {
        if (this.treeViewIff.SelectedNode == this.treeViewIff.Nodes[0]) {
          MessageBox.Show("You may not remove the root node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } else {
          WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
          this.treeViewIff.SelectedNode.Remove();
          if (wsNode.ParentID != 0) {
            wsNode.Parent.Nodes.Remove(wsNode);
          } else {
            this.m_WSFile.Nodes.Remove(wsNode);
          }

          this.HasChanges = true;
        }
      }
    }

    private void toolStripButtonIffAddNode_VisibleChanged(object sender, EventArgs e) {
      this.toolStripTopMenuItemIffAddNode.Enabled = this.toolStripButtonIffAddNode.Visible;
    }

    private void toolStripButtonIffRemoveNode_VisibleChanged(object sender, EventArgs e) {
      this.toolStripTopMenuItemIffRemoveNode.Enabled = this.toolStripButtonIffRemoveNode.Visible;
    }

    private void toolStripButtonIffRenameNode_Click(object sender, EventArgs e) {
      if (this.m_IFFFile != null) {
        if (this.treeViewIff.SelectedNode != null) {
          this.treeViewIff.Focus();
          this.treeViewIff.SelectedNode.BeginEdit();
        } else {
          MessageBox.Show("You must first select a node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void toolStripButtonIffRenameNode_VisibleChanged(object sender, EventArgs e) {
      this.toolStripTopMenuItemIffRenameNode.Enabled = this.toolStripButtonIffRenameNode.Visible;
    }

    private void toolStripButtonIffManageItems_Click(object sender, EventArgs e) {
      if (this.m_WSFile != null) {
        this.m_FormWSItemManager = new FormWSItemManager(this.m_WSFile.Types, (this.m_WSFile.MaximumObjectIndex + 1));
        this.m_FormWSItemManager.Top = this.Top + ((this.Height - this.m_FormWSItemManager.Height) / 2);
        this.m_FormWSItemManager.Left = this.Left + ((this.Width - this.m_FormWSItemManager.Width) / 2);
        if (this.m_FormWSItemManager.ShowDialog() == DialogResult.OK) {
          if (!CompareStringArrays(this.m_WSFile.Types, this.m_FormWSItemManager.m_ItemNames.ToArray())) {
            this.m_WSFile.Types = this.m_FormWSItemManager.m_ItemNames.ToArray();
            this.HasChanges = true;

            this.comboBoxWsObject.Items.Clear();
            this.comboBoxWsObject.Items.AddRange(this.m_WSFile.Types);
            this.treeViewIff_AfterSelect(this.treeViewIff, new TreeViewEventArgs(this.treeViewIff.SelectedNode));
          }
        }
      }
    }

    private void toolStripButtonIffManageItems_VisibleChanged(object sender, EventArgs e) {
      this.manageItemsToolStripMenuItem.Enabled = this.toolStripButtonIffManageItems.Visible;
    }

    private void toolStripButtonIffInsert_VisibleChanged(object sender, EventArgs e) {
      this.toolStripStatusLabelInsertOverwrite.Visible = (this.dataGridViewIff.Visible || this.toolStripButtonIffInsert.Visible);
    }

    private void toolStripButtonIffSave_Click(object sender, EventArgs e) {
      if (this.m_ILFFile != null) {
        this.m_ILFFile.Save(this.m_ILFFile.Filename);
        this.HasChanges = false;
      } else if (this.m_WSFile != null) {
        this.m_WSFile.Save(this.m_WSFile.Filename);
        this.HasChanges = false;
      } else if (this.m_IFFFile != null) {
        if ((this.m_DynamicByteProviderIFF != null) && (this.m_DynamicByteProviderIFF.HasChanges()) && (this.treeViewIff.SelectedNode != null)) {
          IFFFile.IFFNode iffNode = FindIFFNode(this.treeViewIff.SelectedNode);
          iffNode.Data = this.m_DynamicByteProviderIFF.Bytes.ToArray();
        }

        this.m_IFFFile.Save(this.m_IFFFile.Filename);
        this.HasChanges = false;
      } else {
        MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void toolStripButtonIffSave_EnabledChanged(object sender, EventArgs e) {
      this.saveToolStripMenuItem.Enabled = this.toolStripButtonIffSave.Enabled;
    }

    private void toolStripButtonIffSaveAs_EnabledChanged(object sender, EventArgs e) {
      this.saveAsToolStripMenuItem.Enabled = this.toolStripButtonIffSaveAs.Enabled;
    }

    private void toolStripButtonIffCut_EnabledChanged(object sender, EventArgs e) {
      this.cutToolStripMenuItem.Enabled = this.toolStripButtonIffCut.Enabled;
    }

    private void toolStripButtonIffCopy_EnabledChanged(object sender, EventArgs e) {
      this.copyToolStripMenuItem.Enabled = this.toolStripButtonIffCopy.Enabled;
    }

    private void toolStripButtonIffPaste_EnabledChanged(object sender, EventArgs e) {
      this.pasteToolStripMenuItem.Enabled = this.toolStripButtonIffPaste.Enabled;
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.Settings(this);
    }

    private void toolStripButtonSettings_Click(object sender, EventArgs e) {
      this.settingsToolStripMenuItem.PerformClick();
    }

    private void toolStripButtonAbout_Click(object sender, EventArgs e) {
      this.toolStripTopMenuItemAbout.PerformClick();
    }

    private void toolStripSplitButtonOpen_ButtonClick(object sender, EventArgs e) {
      this.m_FormNotifyIcon.Open();
    }

    private void toolStripMenuItemOpenTRETOC_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenTOCTRE();
    }

    private void toolStripMenuItemOpenIFF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenIFFILFWS();
    }

    private void toolStripMenuItemOpenPAL_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenPAL();
    }

    private void toolStripMenuItemOpenSTF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenSTF();
    }
    #endregion

    #region SplitContainerIFF Functions
    private void splitContainerIff_VisibleChanged(object sender, EventArgs e) {
      if (this.splitContainerIff.Visible) {
        this.toolStripSeparator10.Visible = true;
        this.toolStripSeparator12.Visible = true;
        this.toolStripButtonIffCopy.Visible = true;
        this.toolStripButtonIffCut.Visible = true;
        this.toolStripButtonIffFind.Visible = true;
        this.toolStripButtonIffInsert.Visible = true;
        this.toolStripButtonIffPaste.Visible = true;
        this.toolStripButtonIffReplace.Visible = true;
        this.toolStripButtonIffAddNode.Visible = true;
        this.toolStripButtonIffRemoveNode.Visible = true;
        this.toolStripSeparator14.Visible = true;
        this.toolStripButtonIffRenameNode.Visible = true;

        this.hexBoxIff_SelectionLengthChanged(this.hexBoxIff, new EventArgs());

        this.toolStripStatusLabelInsertOverwrite.Visible = (this.dataGridViewIff.Visible || this.toolStripButtonIffInsert.Visible);
        this.toolStripStatusLabelInsertOverwrite.Text = ((this.hexBoxIff.InsertActive) ? "INS" : "OVR");
      }

      if ((this.splitContainerIff.Visible) || (this.dataGridViewIff.Visible)) {
        this.toolStripStatusLabelInsertOverwrite.Visible = true;
      } else {
        this.toolStripStatusLabelInsertOverwrite.Visible = false;
      }
    }
    #endregion

    #region TreeViewIFF Functions
    private void treeViewIff_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
      if (e.Label == null) {
        e.CancelEdit = true;
        return;
      }
      IFFFile.IFFNode iffNode = FindIFFNode(this.treeViewIff.SelectedNode);
      if ((e.Label.Length == 4) || (e.Label.Length == 8)) {
        if ((this.treeViewIff.SelectedNode.Nodes.Count > 0) && (!e.Label.Contains("FORM"))) {
          e.CancelEdit = true;
          MessageBox.Show("Nodes with children must contain FORM in their name.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } else {
          if (e.Label.ToUpper() != e.Node.Text) {
            iffNode.ID = e.Label.ToUpper();
            this.HasChanges = true;
            this.treeViewIff_AfterSelect(this.treeViewIff, new TreeViewEventArgs(this.treeViewIff.SelectedNode));
          } else {
            e.CancelEdit = true;
          }
        }
      } else {
        MessageBox.Show("Node name must be either 4 or 8 characters long.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
      }
    }

    private void treeViewIff_AfterSelect(object sender, TreeViewEventArgs e) {
      if (this.m_ILFFile != null) {
        if (e.Node.Tag != null) {
          ILFFile.ILFNode ilfChunk = this.m_ILFFile.Nodes[e.Node.Index];

          this.labelIlfCell.Visible = true;
          this.labelIlfObject.Visible = true;
          this.groupBoxIlfCoordinates.Visible = true;
          this.groupBoxIlfRotation.Visible = true;
          this.textBoxIlfCell.Visible = true;
          this.textBoxIlfObject.Visible = true;

          this.textBoxIlfObject.Text = ilfChunk.Object;
          this.textBoxIlfCell.Text = ilfChunk.Cell;
          this.numericUpDownIlfM11.Value = (Decimal)ilfChunk.RotationMatrix[0][0];
          this.numericUpDownIlfM12.Value = (Decimal)ilfChunk.RotationMatrix[0][1];
          this.numericUpDownIlfM13.Value = (Decimal)ilfChunk.RotationMatrix[0][2];
          this.numericUpDownIlfW1.Value = (Decimal)ilfChunk.W1;
          this.numericUpDownIlfM21.Value = (Decimal)ilfChunk.RotationMatrix[1][0];
          this.numericUpDownIlfM22.Value = (Decimal)ilfChunk.RotationMatrix[1][1];
          this.numericUpDownIlfM23.Value = (Decimal)ilfChunk.RotationMatrix[1][2];
          this.numericUpDownIlfW2.Value = (Decimal)ilfChunk.W2;
          this.numericUpDownIlfM31.Value = (Decimal)ilfChunk.RotationMatrix[2][0];
          this.numericUpDownIlfM32.Value = (Decimal)ilfChunk.RotationMatrix[2][1];
          this.numericUpDownIlfM33.Value = (Decimal)ilfChunk.RotationMatrix[2][2];
          this.numericUpDownIlfW3.Value = (Decimal)ilfChunk.W3;

          UpdateIlfAngles(ilfChunk.RotationMatrix);
        } else {
          this.labelIlfCell.Visible = false;
          this.labelIlfObject.Visible = false;
          this.groupBoxIlfCoordinates.Visible = false;
          this.groupBoxIlfRotation.Visible = false;
          this.textBoxIlfCell.Visible = false;
          this.textBoxIlfObject.Visible = false;
        }
      } else if (this.m_WSFile != null) {
        if ((e.Node != null) && (e.Node.Name != "WSNPFORM") && (e.Node.Name != String.Empty) && (e.Node.Name != null)) {
          WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(e.Node.Name));
          this.labelWsID.Visible = true;
          this.labelWsObject.Visible = true;
          this.labelWsoW.Visible = true;
          this.labelWsoX.Visible = true;
          this.labelWsoY.Visible = true;
          this.labelWsoZ.Visible = true;
          this.labelWsParent.Visible = true;
          this.labelWsScale.Visible = true;
          this.labelWsType.Visible = true;
          this.labelWsPOBCRC.Visible = true;
          this.labelWsX.Visible = true;
          this.labelWsY.Visible = true;
          this.labelWsZ.Visible = true;
          this.numericUpDownWsID.Visible = true;
          this.comboBoxWsObject.Visible = true;
          this.numericUpDownWsoW.Visible = true;
          this.numericUpDownWsoX.Visible = true;
          this.numericUpDownWsoY.Visible = true;
          this.numericUpDownWsoZ.Visible = true;
          this.numericUpDownWsParent.Visible = true;
          this.numericUpDownWsScale.Visible = true;
          this.numericUpDownWsType.Visible = true;
          this.hexBoxWsPOBCRC.Visible = true;
          this.numericUpDownWsX.Visible = true;
          this.numericUpDownWsY.Visible = true;
          this.numericUpDownWsZ.Visible = true;

          this.numericUpDownWsID.Value = wsNode.ID;
          this.comboBoxWsObject.SelectedIndex = wsNode.ObjectIndex;
          this.numericUpDownWsoW.Value = (Decimal)wsNode.oW;
          this.numericUpDownWsoX.Value = (Decimal)wsNode.oX;
          this.numericUpDownWsoY.Value = (Decimal)wsNode.oY;
          this.numericUpDownWsoZ.Value = (Decimal)wsNode.oZ;
          this.numericUpDownWsParent.Value = wsNode.ParentID;
          this.numericUpDownWsScale.Value = (Decimal)wsNode.Scale;
          this.numericUpDownWsType.Value = (Decimal)wsNode.Type;
          this.numericUpDownWsX.Value = (Decimal)wsNode.X;
          this.numericUpDownWsY.Value = (Decimal)wsNode.Y;
          this.numericUpDownWsZ.Value = (Decimal)wsNode.Z;
          try {
            this.m_DynamicByteProviderWS.Changed -= new EventHandler(m_DynamicByteProviderWS_Changed);
          } catch {
          }
          this.m_DynamicByteProviderWS = new DynamicByteProvider(wsNode.POBCRC);
          this.m_DynamicByteProviderWS.Changed += new EventHandler(m_DynamicByteProviderWS_Changed);
          this.hexBoxWsPOBCRC.ByteProvider = this.m_DynamicByteProviderWS;
        } else {
          this.labelWsID.Visible = false;
          this.labelWsObject.Visible = false;
          this.labelWsoW.Visible = false;
          this.labelWsoX.Visible = false;
          this.labelWsoY.Visible = false;
          this.labelWsoZ.Visible = false;
          this.labelWsParent.Visible = false;
          this.labelWsScale.Visible = false;
          this.labelWsType.Visible = false;
          this.labelWsPOBCRC.Visible = false;
          this.labelWsX.Visible = false;
          this.labelWsY.Visible = false;
          this.labelWsZ.Visible = false;
          this.numericUpDownWsID.Visible = false;
          this.comboBoxWsObject.Visible = false;
          this.numericUpDownWsoW.Visible = false;
          this.numericUpDownWsoX.Visible = false;
          this.numericUpDownWsoY.Visible = false;
          this.numericUpDownWsoZ.Visible = false;
          this.numericUpDownWsParent.Visible = false;
          this.numericUpDownWsScale.Visible = false;
          this.numericUpDownWsType.Visible = false;
          this.hexBoxWsPOBCRC.Visible = false;
          this.numericUpDownWsX.Visible = false;
          this.numericUpDownWsY.Visible = false;
          this.numericUpDownWsZ.Visible = false;
        }
      } else if (this.m_IFFFile != null) {
        IFFFile.IFFNode iffNode = FindIFFNode(e.Node);
        if ((!iffNode.ID.Contains("FORM")) && (iffNode.Data != null)) {
          try {
            this.m_DynamicByteProviderIFF.Changed -= new EventHandler(m_DynamicByteProviderIFF_Changed);
          } catch {
          }

          this.m_DynamicByteProviderIFF = new DynamicByteProvider(iffNode.Data);
          this.m_DynamicByteProviderIFF.Changed += new EventHandler(m_DynamicByteProviderIFF_Changed);
          this.hexBoxIff.ByteProvider = this.m_DynamicByteProviderIFF;

        } else {
          this.hexBoxIff.ByteProvider = null;
          this.m_DynamicByteProviderIFF = null;
        }
      }
    }

    private void m_DynamicByteProviderWS_Changed(object sender, EventArgs e) {
      WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
      Byte[] tempBytes = new Byte[4];
      if (this.m_DynamicByteProviderWS.Bytes.Count >= 4) {
        for (Int32 counter = 0; counter < 4; counter++) {
          tempBytes[counter] = this.m_DynamicByteProviderWS.Bytes[counter];
        }

        if ((wsNode.POBCRC[0] != tempBytes[0]) && (wsNode.POBCRC[1] != tempBytes[1]) && (wsNode.POBCRC[2] != tempBytes[2]) && (wsNode.POBCRC[3] != tempBytes[3])) {
          wsNode.POBCRC = tempBytes;
          this.HasChanges = true;
        }
      }
    }

    private void m_DynamicByteProviderIFF_Changed(object sender, EventArgs e) {
      if ((this.m_IFFFile != null) && (this.m_DynamicByteProviderIFF != null) && (this.treeViewIff.SelectedNode != null)) {
        IFFFile.IFFNode iffNode = FindIFFNode(this.treeViewIff.SelectedNode);
        iffNode.Data = this.m_DynamicByteProviderIFF.Bytes.ToArray();
        this.HasChanges = true;
      }
    }

    private void treeViewIff_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) {
      if (this.m_IFFFile == null) {
        e.CancelEdit = true;
      } else if (e.Node == this.treeViewIff.Nodes[0]) {
        e.CancelEdit = true;
        MessageBox.Show("You may not edit the name of that node.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void treeViewIff_DragDrop(object sender, DragEventArgs e) {
      if ((this.m_IFFFile != null) && (!this.m_IFFFile.IsDataTable) && (this.m_TreeNodeDragSource != null)) {
        if (this.m_TreeNodeDropTarget != null) {
          IFFFile.IFFNode draggedIFFNode = FindIFFNode(this.m_TreeNodeDragSource);
          IFFFile.IFFNode droppedIFFNode = FindIFFNode(this.m_TreeNodeDropTarget);

          this.m_TreeNodeDragSource.Remove();
          draggedIFFNode.Parent.Children.Remove(draggedIFFNode);

          Int32 index = ((this.m_InsertPosition == InsertLocation.Above) ? this.m_TreeNodeDropTarget.Index : this.m_TreeNodeDropTarget.Index + 1);

          if ((index) == this.m_TreeNodeDropTarget.Parent.Nodes.Count) {
            this.m_TreeNodeDropTarget.Parent.Nodes.Add(this.m_TreeNodeDragSource);
            droppedIFFNode.Parent.Children.Add(draggedIFFNode);
            draggedIFFNode.Parent = droppedIFFNode.Parent;
          } else {
            this.m_TreeNodeDropTarget.Parent.Nodes.Insert(index, this.m_TreeNodeDragSource);
            droppedIFFNode.Parent.Children.Insert(index, draggedIFFNode);
            draggedIFFNode.Parent = droppedIFFNode.Parent;
          }

          this.HasChanges = true;
        }
      }

      this.m_TreeNodeDragSource = null;
      this.m_InsertPosition = InsertLocation.Above;
      this.m_TreeNodeDropTarget = null;
      this.m_MarkerPosition = InsertLocation.Above;
      this.m_TreeNodeHover = null;
      this.m_HoverStart = DateTime.Now;
      Cursor = Cursors.Default;
      this.treeViewIff.Refresh();
    }

    private void treeViewIff_DragOver(object sender, DragEventArgs e) {
      if ((this.m_IFFFile != null) && (e.Data.GetDataPresent(typeof(TreeNode))) && (e.AllowedEffect == DragDropEffects.Move)) {
        TreeNode treeNode = this.treeViewIff.GetNodeAt(this.treeViewIff.PointToClient(new Point(e.X, e.Y)));
        if (treeNode != null) {
          if (treeNode == this.m_TreeNodeDragSource) {
            e.Effect = DragDropEffects.None;
            return;
          }

          Point cursorPosition = this.treeViewIff.PointToClient(Cursor.Position);

          InsertLocation insertPosition = InsertLocation.Above;
          if (cursorPosition.Y > (treeNode.Bounds.Top + (treeNode.Bounds.Height / 2))) {
            insertPosition = InsertLocation.Below;
          }

          if ((this.m_TreeNodeHover != treeNode) || (this.m_MarkerPosition != insertPosition) || ((new TimeSpan(DateTime.Now.Ticks - this.m_HoverStart.Ticks).TotalMilliseconds > SystemInformation.MenuShowDelay) && (treeNode.Nodes.Count > 0) && (!treeNode.IsExpanded))) {
            Int32 delta = this.treeViewIff.Height - cursorPosition.Y;

            if ((delta < (this.treeViewIff.Height / 2)) && (delta > 0)) {
              if (treeNode.NextVisibleNode != null) {
                treeNode.NextVisibleNode.EnsureVisible();
              }
            } else if ((delta > (this.treeViewIff.Height / 2)) && (delta < this.treeViewIff.Height)) {
              if (treeNode.PrevVisibleNode != null) {
                treeNode.PrevVisibleNode.EnsureVisible();
              }
            }

            this.m_TreeNodeHover = treeNode;
            this.m_MarkerPosition = insertPosition;

            e.Effect = DragDropEffects.Move;
            if ((this.m_TreeNodeHover.Nodes.Count > 0) && (!this.m_TreeNodeHover.IsExpanded) && (new TimeSpan(DateTime.Now.Ticks - this.m_HoverStart.Ticks).TotalMilliseconds > SystemInformation.MenuShowDelay)) {
              this.m_TreeNodeHover.Expand();
            }
            this.treeViewIff.Refresh();

            this.m_TreeNodeDropTarget = treeNode;
            this.m_InsertPosition = insertPosition;

            Int32 insertPositionVertical = treeNode.Bounds.Top;
            Int32 insertPositionLeft = treeNode.Bounds.Left;
            Int32 insertPositionRight = ((treeNode.PrevVisibleNode != null) ? Math.Max(treeNode.Bounds.Right, treeNode.PrevVisibleNode.Bounds.Right) : treeNode.Bounds.Right);
            if (insertPosition == InsertLocation.Below) {
              insertPositionVertical = treeNode.Bounds.Bottom;
              if ((treeNode.NextVisibleNode != null) && (treeNode.NextVisibleNode.Level > treeNode.Level)) {
                insertPositionLeft = treeNode.NextVisibleNode.Bounds.Left;
                this.m_InsertPosition = InsertLocation.Above;
                this.m_TreeNodeDropTarget = treeNode.NextVisibleNode;
              }
              insertPositionRight = ((treeNode.NextVisibleNode != null) ? Math.Max(treeNode.Bounds.Right, treeNode.NextVisibleNode.Bounds.Right) : treeNode.Bounds.Right);
            }

            if ((this.m_TreeNodeDragSource == this.m_TreeNodeDropTarget) || (IsChildOf(this.m_TreeNodeDragSource, this.m_TreeNodeDropTarget))) {
              e.Effect = DragDropEffects.None;
              if (this.m_TreeNodeHover != null) {
                this.m_TreeNodeHover = null;
                this.treeViewIff.Refresh();
              }
              this.m_HoverStart = DateTime.Now;
              return;
            }

            Graphics graphics = this.treeViewIff.CreateGraphics();
            graphics.FillPolygon(Brushes.Black, new Point[] { new Point(insertPositionLeft - 4, insertPositionVertical - 5), new Point(insertPositionLeft, insertPositionVertical - 1), new Point(insertPositionLeft, insertPositionVertical), new Point(insertPositionLeft - 4, insertPositionVertical + 4) });
            graphics.DrawLine(new Pen(Brushes.Black, 2), new Point(insertPositionLeft, insertPositionVertical), new Point(insertPositionRight, insertPositionVertical));
            graphics.FillPolygon(Brushes.Black, new Point[] { new Point(insertPositionRight + 4, insertPositionVertical - 5), new Point(insertPositionRight, insertPositionVertical - 1), new Point(insertPositionRight, insertPositionVertical), new Point(insertPositionRight + 4, insertPositionVertical + 4) });

            this.m_HoverStart = DateTime.Now;
          }
        } else {
          e.Effect = DragDropEffects.None;
          if (this.m_TreeNodeHover != null) {
            this.m_TreeNodeHover = null;
            this.treeViewIff.Refresh();
          }
        }
      }
    }

    private void treeViewIff_GiveFeedback(object sender, GiveFeedbackEventArgs e) {
      try {
        if ((this.m_TreeNodeDragSource != null) && (e.Effect == DragDropEffects.Move)) {
          Bitmap tempBitmap = new Bitmap(16, 16);
          Graphics graphics = Graphics.FromImage(tempBitmap);
          SizeF stringSize = graphics.MeasureString(this.m_TreeNodeDragSource.Text, this.treeViewIff.Font);
          graphics.Dispose();
          tempBitmap = new Bitmap(16 + (Int32)Math.Ceiling(stringSize.Width), 16 + (Int32)Math.Ceiling(stringSize.Height));
          graphics = Graphics.FromImage(tempBitmap);
          Cursors.Default.Draw(graphics, new Rectangle(0, 0, 32, 32));
          graphics.FillRectangle(new SolidBrush(SystemColors.Highlight), 15, 15, (Single)Math.Ceiling(stringSize.Width), (Single)Math.Ceiling(stringSize.Height));
          graphics.DrawRectangle(Pens.Black, 15, 15, (Single)Math.Ceiling(stringSize.Width), (Single)Math.Ceiling(stringSize.Height));
          Pen pen = new Pen(Color.OrangeRed);
          pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
          graphics.DrawRectangle(pen, 15, 15, (Single)Math.Ceiling(stringSize.Width), (Single)Math.Ceiling(stringSize.Height));
          pen.Dispose();
          graphics.DrawString(this.m_TreeNodeDragSource.Text, this.treeViewIff.Font, new SolidBrush(SystemColors.HighlightText), new PointF(15, 15));
          graphics.Dispose();
          Bitmap bitmap = new Bitmap(tempBitmap.Width * 2, tempBitmap.Height * 2);
          graphics = Graphics.FromImage(bitmap);
          graphics.DrawImageUnscaledAndClipped(tempBitmap, new Rectangle(tempBitmap.Width, tempBitmap.Height, tempBitmap.Width, tempBitmap.Height));
          tempBitmap.Dispose();
          graphics.Dispose();
          Icon icon = Icon.FromHandle(bitmap.GetHicon());
          bitmap.Dispose();
          Cursor cursor = new Cursor(icon.Handle);
          icon.Dispose();
          e.UseDefaultCursors = false;
          Cursor = cursor;
        } else {
          this.treeViewIff.Refresh();
          e.UseDefaultCursors = true;
          Cursor = Cursors.Default;
        }
      } catch (Exception exception) {
        MessageBox.Show(exception.Message);
      }
    }

    private void treeViewIff_ItemDrag(object sender, ItemDragEventArgs e) {
      if ((this.m_IFFFile != null) && (!this.m_IFFFile.IsDataTable)) {
        this.m_TreeNodeDragSource = (TreeNode)e.Item;
        if (this.treeViewIff.DoDragDrop((TreeNode)e.Item, DragDropEffects.Move) == DragDropEffects.None) {
          this.m_TreeNodeDragSource = null;
          this.m_InsertPosition = InsertLocation.Above;
          this.m_TreeNodeDropTarget = null;
          this.m_MarkerPosition = InsertLocation.Above;
          this.m_TreeNodeHover = null;
          this.m_HoverStart = DateTime.Now;
          Cursor = Cursors.Default;
          this.treeViewIff.Refresh();
        }
      }
    }
    #endregion

    #region HexBoxIFF Functions
    private void hexBoxIff_InsertActiveChanged(object sender, EventArgs e) {
      if (this.hexBoxIff.InsertActive) {
        this.toolStripButtonIffInsert.Image = TRE_Explorer.Properties.Resources.Insert;
        this.toolStripButtonIffInsert.Text = "Insert";
        this.toolStripStatusLabelInsertOverwrite.Text = "INS";
      } else {
        this.toolStripButtonIffInsert.Image = TRE_Explorer.Properties.Resources.Overwrite;
        this.toolStripButtonIffInsert.Text = "Overwrite";
        this.toolStripStatusLabelInsertOverwrite.Text = "OVR";
      }
    }

    private void hexBoxIff_Resize(object sender, EventArgs e) {
      this.hexBoxIff.BytesPerLine = (Int32)(((Double)this.hexBoxIff.ClientSize.Width - (Double)47) / (Double)32);
    }

    private void hexBoxIff_SelectionLengthChanged(object sender, EventArgs e) {
      if (this.hexBoxIff.SelectionLength == 0L) {
        this.toolStripButtonIffCut.Enabled = false;
        this.toolStripButtonIffCopy.Enabled = false;
      } else {
        this.toolStripButtonIffCut.Enabled = true;
        this.toolStripButtonIffCopy.Enabled = true;
      }
    }

    private void hexBoxIff_VisibleChanged(object sender, EventArgs e) {
      if (this.hexBoxIff.Visible) {
        this.toolStripSeparator10.Visible = true;
        this.toolStripSeparator12.Visible = true;
        this.toolStripButtonIffCopy.Visible = true;
        this.toolStripButtonIffCut.Visible = true;
        this.toolStripButtonIffFind.Visible = true;
        this.toolStripButtonIffInsert.Visible = true;
        this.toolStripButtonIffPaste.Visible = true;
        this.toolStripButtonIffReplace.Visible = true;
        this.toolStripButtonIffAddNode.Visible = true;
        this.toolStripButtonIffRemoveNode.Visible = true;
        this.toolStripSeparator14.Visible = true;
        this.toolStripButtonIffRenameNode.Visible = true;
        this.toolStripButtonIffManageItems.Visible = false;

        this.toolStripButtonIffCopy.Enabled = (this.hexBoxIff.SelectionLength > 0);
        this.toolStripButtonIffCut.Enabled = (this.hexBoxIff.SelectionLength > 0);
        this.toolStripButtonIffPaste.Enabled = Clipboard.ContainsText();
        
        if (this.m_IFFFile != null) {
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_IFFFile.Filename;
        }
      } else {
        this.toolStripButtonIffCopy.Enabled = false;
        this.toolStripButtonIffCut.Enabled = false;
        this.toolStripButtonIffPaste.Enabled = false;
      }
    }
    #endregion

    #region DataGridViewIFF Functions
    private void dataGridViewIff_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
      this.HasChanges = true;
    }

    private void dataGridViewIff_DataError(object sender, DataGridViewDataErrorEventArgs e) {
      MessageBox.Show(e.Exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      this.dataGridViewIff.CancelEdit();
    }

    private void dataGridViewIff_UserAddedRow(object sender, DataGridViewRowEventArgs e) {
      this.HasChanges = true;
    }

    private void dataGridViewIff_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
      this.HasChanges = true;
    }

    private void dataGridViewIff_VisibleChanged(object sender, EventArgs e) {
      if (this.dataGridViewIff.Visible) {
        this.toolStripSeparator10.Visible = false;
        this.toolStripSeparator12.Visible = false;
        this.toolStripButtonIffCopy.Visible = false;
        this.toolStripButtonIffCut.Visible = false;
        this.toolStripButtonIffFind.Visible = false;
        this.toolStripButtonIffInsert.Visible = false;
        this.toolStripButtonIffPaste.Visible = false;
        this.toolStripButtonIffReplace.Visible = false;
        this.toolStripButtonIffAddNode.Visible = false;
        this.toolStripButtonIffRemoveNode.Visible = false;
        this.toolStripSeparator14.Visible = false;
        this.toolStripButtonIffRenameNode.Visible = false;
        this.toolStripButtonIffManageItems.Visible = false;

        this.cutToolStripMenuItem.Enabled = false;
        this.copyToolStripMenuItem.Enabled = false;
        this.pasteToolStripMenuItem.Enabled = false;
        this.findAgainToolStripMenuItem.Enabled = false;
        this.findToolStripMenuItem.Enabled = false;
        this.replaceToolStripMenuItem.Enabled = false;

        this.toolStripSeparator3.Visible = true;
        this.toolStripTextBoxSearch.Visible = true;
        this.toolStripButtonSearch.Visible = true;

        if (this.m_IFFFile != null) {
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_IFFFile.Filename;
        }
      } else {
        this.toolStripSeparator3.Visible = false;
        this.toolStripTextBoxSearch.Visible = false;
        this.toolStripButtonSearch.Visible = false;

        if ((this.toolStripStatusLabelInsertOverwrite.Text != "INS") && (this.toolStripStatusLabelInsertOverwrite.Text != "OVR")) {
          this.toolStripStatusLabelInsertOverwrite.Text = String.Empty;
        }
      }

      if ((this.splitContainerIff.Visible) || (this.dataGridViewIff.Visible)) {
        this.toolStripStatusLabelInsertOverwrite.Visible = true;
      } else {
        this.toolStripStatusLabelInsertOverwrite.Visible = false;
      }
    }
    #endregion

    #region PanelILF Functions
    private void buttonIlfMatrixCalculator_Click(object sender, EventArgs e) {
      this.m_FormILFMatrixCalculator = new FormILFMatrixCalculator(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
      this.m_FormILFMatrixCalculator.Top = this.Top + (Int32)(((Single)this.Height - (Single)m_FormILFMatrixCalculator.Height) / 2);
      this.m_FormILFMatrixCalculator.Left = this.Left + (Int32)(((Single)this.Width - (Single)m_FormILFMatrixCalculator.Width) / 2);


      if (this.m_FormILFMatrixCalculator.ShowDialog() == DialogResult.OK) {
        this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix = m_FormILFMatrixCalculator.RotationMatrix;

        if (this.numericUpDownIlfM11.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[0][0]) {
          this.numericUpDownIlfM11.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[0][0];
          this.HasChanges = true;
        }
        if (this.numericUpDownIlfM12.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[0][1]) {
          this.numericUpDownIlfM12.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[0][1];
          this.HasChanges = true;
        }
        if (this.numericUpDownIlfM13.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[0][2]) {
          this.numericUpDownIlfM13.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[0][2];
          this.HasChanges = true;
        }

        if (this.numericUpDownIlfM21.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[1][0]) {
          this.numericUpDownIlfM21.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[1][0];
          this.HasChanges = true;
        }
        if (this.numericUpDownIlfM22.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[1][1]) {
          this.numericUpDownIlfM22.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[1][1];
          this.HasChanges = true;
        }
        if (this.numericUpDownIlfM23.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[1][2]) {
          this.numericUpDownIlfM23.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[1][2];
          this.HasChanges = true;
        }

        if (this.numericUpDownIlfM31.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[2][0]) {
          this.numericUpDownIlfM31.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[2][0];
          this.HasChanges = true;
        }
        if (this.numericUpDownIlfM32.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[2][1]) {
          this.numericUpDownIlfM32.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[2][1];
          this.HasChanges = true;
        }
        if (this.numericUpDownIlfM33.Value != (Decimal)m_FormILFMatrixCalculator.RotationMatrix[2][2]) {
          this.numericUpDownIlfM33.Value = (Decimal)m_FormILFMatrixCalculator.RotationMatrix[2][2];
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownIlfM11_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[0][0] != (Single)this.numericUpDownIlfM11.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[0][0] = (Single)this.numericUpDownIlfM11.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM12_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[0][1] != (Single)this.numericUpDownIlfM12.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[0][1] = (Single)this.numericUpDownIlfM12.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM13_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[0][2] != (Single)this.numericUpDownIlfM13.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[0][2] = (Single)this.numericUpDownIlfM13.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM21_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[1][0] != (Single)this.numericUpDownIlfM21.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[1][0] = (Single)this.numericUpDownIlfM21.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM22_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[1][1] != (Single)this.numericUpDownIlfM22.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[1][1] = (Single)this.numericUpDownIlfM22.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM23_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[1][2] != (Single)this.numericUpDownIlfM23.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[1][2] = (Single)this.numericUpDownIlfM23.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM31_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[2][0] != (Single)this.numericUpDownIlfM31.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[2][0] = (Single)this.numericUpDownIlfM31.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM32_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[2][1] != (Single)this.numericUpDownIlfM32.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[2][1] = (Single)this.numericUpDownIlfM32.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfM33_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[2][2] != (Single)this.numericUpDownIlfM33.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix[2][2] = (Single)this.numericUpDownIlfM33.Value;
          this.HasChanges = true;

          UpdateIlfAngles(this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].RotationMatrix);
        }
      }
    }

    private void numericUpDownIlfW1_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].W1 != (Single)this.numericUpDownIlfW1.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].W1 = (Single)this.numericUpDownIlfW1.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownIlfW2_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].W2 != (Single)this.numericUpDownIlfW2.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].W2 = (Single)this.numericUpDownIlfW2.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownIlfW3_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].W3 != (Single)this.numericUpDownIlfW3.Value) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].W3 = (Single)this.numericUpDownIlfW3.Value;
          this.HasChanges = true;
        }
      }
    }

    private void panelIlf_VisibleChanged(object sender, EventArgs e) {
      if (this.panelIlf.Visible) {
        this.toolStripSeparator10.Visible = false;
        this.toolStripSeparator12.Visible = false;
        this.toolStripButtonIffCopy.Visible = false;
        this.toolStripButtonIffCut.Visible = false;
        this.toolStripButtonIffFind.Visible = false;
        this.toolStripButtonIffInsert.Visible = false;
        this.toolStripButtonIffPaste.Visible = false;
        this.toolStripButtonIffReplace.Visible = false;
        this.toolStripButtonIffAddNode.Visible = true;
        this.toolStripButtonIffRemoveNode.Visible = true;
        this.toolStripSeparator14.Visible = true;
        this.toolStripButtonIffRenameNode.Visible = false;
        this.toolStripButtonIffManageItems.Visible = false;
        this.splitContainerIff.SplitterDistance = 310;
        this.groupBoxIlfRotation.Height = 74;
        if (this.m_ILFFile != null) {
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_ILFFile.Filename;
        }
      } else {
        this.splitContainerIff.SplitterDistance = 155;
      }
    }

    private void textBoxIlfObject_TextChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].Object != this.textBoxIlfObject.Text) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].Object = this.textBoxIlfObject.Text;
          this.HasChanges = true;

          this.treeViewIff.SelectedNode.Name = this.textBoxIlfObject.Text;
          this.treeViewIff.SelectedNode.Tag = this.textBoxIlfObject.Text;
          this.treeViewIff.SelectedNode.Text = this.textBoxIlfObject.Text;
        }
      }
    }

    private void textBoxIlfCell_TextChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_ILFFile != null)) {
        if (this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].Cell != this.textBoxIlfCell.Text) {
          this.m_ILFFile.Nodes[this.treeViewIff.SelectedNode.Index].Cell = this.textBoxIlfCell.Text;
          this.HasChanges = true;
        }
      }
    }
    #endregion

    #region PanelWS Functions
    private void comboBoxWsObject_SelectedIndexChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.ObjectIndex != this.comboBoxWsObject.SelectedIndex) {
          wsNode.ObjectIndex = this.comboBoxWsObject.SelectedIndex;
          this.HasChanges = true;

          this.treeViewIff.SelectedNode.Text = wsNode.ID.ToString() + ((WSObjectType(this.m_WSFile.Types[wsNode.ObjectIndex]) != String.Empty) ? " [" + WSObjectType(this.m_WSFile.Types[wsNode.ObjectIndex]) + "]" : String.Empty);
          this.treeViewIff.SelectedNode.Tag = wsNode.ObjectIndex;
        }
      }
    }

    private void numericUpDownWsoX_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.oW != (Single)this.numericUpDownWsoW.Value) {
          wsNode.oW = (Single)this.numericUpDownWsoW.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsoY_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.oY != (Single)this.numericUpDownWsoY.Value) {
          wsNode.oY = (Single)this.numericUpDownWsoY.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsoZ_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.oZ != (Single)this.numericUpDownWsoZ.Value) {
          wsNode.oZ = (Single)this.numericUpDownWsoZ.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsoW_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.oW != (Single)this.numericUpDownWsoW.Value) {
          wsNode.oW = (Single)this.numericUpDownWsoW.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsX_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.X != (Single)this.numericUpDownWsX.Value) {
          wsNode.X = (Single)this.numericUpDownWsX.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsY_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.Y != (Single)this.numericUpDownWsY.Value) {
          wsNode.Y = (Single)this.numericUpDownWsY.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsZ_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.Z != (Single)this.numericUpDownWsZ.Value) {
          wsNode.Z = (Single)this.numericUpDownWsZ.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsScale_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.Scale != (Single)this.numericUpDownWsScale.Value) {
          wsNode.Scale = (Single)this.numericUpDownWsScale.Value;
          this.HasChanges = true;
        }
      }
    }

    private void numericUpDownWsType_ValueChanged(object sender, EventArgs e) {
      if ((this.treeViewIff.SelectedNode != null) && (this.m_WSFile != null)) {
        WSFile.WSNode wsNode = this.m_WSFile.FindNodeByID(Int32.Parse(this.treeViewIff.SelectedNode.Name));
        if (wsNode.Type != (Single)this.numericUpDownWsType.Value) {
          wsNode.Type = (Single)this.numericUpDownWsType.Value;
          this.HasChanges = true;
        }
      }
    }

    private void panelWs_VisibleChanged(object sender, EventArgs e) {
      if (this.panelWs.Visible) {
        this.toolStripSeparator12.Visible = false;
        this.toolStripButtonIffCopy.Visible = false;
        this.toolStripButtonIffCut.Visible = false;
        this.toolStripButtonIffInsert.Visible = false;
        this.toolStripButtonIffPaste.Visible = false;
        this.toolStripButtonIffReplace.Visible = false;
        this.toolStripButtonIffRenameNode.Visible = false;
        this.toolStripButtonIffFind.Visible = true;
        this.toolStripButtonIffAddNode.Visible = true;
        this.toolStripButtonIffRemoveNode.Visible = true;
        this.toolStripSeparator10.Visible = true;
        this.toolStripSeparator14.Visible = true;
        this.toolStripButtonIffManageItems.Visible = true;
        if (this.m_WSFile != null) {
          this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_WSFile.Filename;
        }
      } else {
        this.toolStripButtonIffManageItems.Visible = false;
        this.treeViewIff.TreeViewNodeSorter = null;
      }
    }
    #endregion

    #region HexBoxWsPOBCRC Functions
    private void hexBoxWsPOBCRC_Leave(object sender, EventArgs e) {
      if ((this.m_DynamicByteProviderWS != null) && (this.m_DynamicByteProviderWS.Length != 4)) {
        MessageBox.Show("POB must be 4 bytes.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.hexBoxWsPOBCRC.Focus();
      }
    }
    #endregion

    #region MenuStrip Functions
    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonIffSave.PerformClick();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonIffSaveAs.PerformClick();
    }

    private void toolStripTopMenuItemClose_Click(object sender, EventArgs e) {
      this.toolStripButtonIffExit.PerformClick();
    }

    private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonIffCopy.PerformClick();
    }

    private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonIffCut.PerformClick();
    }

    private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonIffPaste.PerformClick();
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
      if ((this.hexBoxIff != null) && (this.m_DynamicByteProviderIFF != null)) {
        this.hexBoxIff.SelectionStart = 0;
        this.hexBoxIff.SelectionLength = this.m_DynamicByteProviderIFF.Length;
      }
    }

    private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e) {
      if ((this.hexBoxIff != null) && (this.m_DynamicByteProviderIFF != null)) {
        this.hexBoxIff.SelectionLength = 0;
      }
    }

    private void findToolStripMenuItem_Click(object sender, EventArgs e) {
      if ((this.m_IFFFile != null) && (this.m_IFFFile.IsDataTable)) {
        this.toolStripTextBoxSearch.Focus();
        if (this.toolStripTextBoxSearch.Text != String.Empty) {
          this.toolStripTextBoxSearch.Select(0, this.toolStripTextBoxSearch.Text.Length);
        }
      } else {
        this.toolStripButtonIffFind.PerformClick();
      }
    }

    private void findAgainToolStripMenuItem_Click(object sender, EventArgs e) {
      if ((this.m_IFFFile != null) && (this.m_IFFFile.IsDataTable)) {
        this.toolStripButtonSearch.PerformClick();
      } else {
        if ((this.treeViewIff.SelectedNode != null) && ((!this.treeViewIff.SelectedNode.Text.Contains("FORM")) || (this.m_SearchAllNodes))) {
          this.IFFFindNext();
        } else {
          MessageBox.Show("You must first select a non-FORM node to search.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void replaceToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonIffReplace.PerformClick();
    }

    private void toolStripTopMenuItemIffAddNode_Click(object sender, EventArgs e) {
      this.toolStripButtonIffAddNode.PerformClick();
    }

    private void toolStripTopMenuItemIffRemoveNode_Click(object sender, EventArgs e) {
      this.toolStripButtonIffRemoveNode.PerformClick();
    }

    private void toolStripTopMenuItemIffRenameNode_Click(object sender, EventArgs e) {
      this.toolStripButtonIffRenameNode.PerformClick();
    }

    private void manageItemsToolStripMenuItem_Click(object sender, EventArgs e) {
      this.toolStripButtonIffManageItems.PerformClick();
    }

    private void toolStripTopMenuItemAbout_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.About(this);
    }

    private void helpTopicsToolStripMenuItem_Click(object sender, EventArgs e) {
      String filename = Path.Combine(Path.GetTempPath(), "TRE Explorer.chm");
      if (!File.Exists(filename)) {
        FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
        binaryWriter.Write(TRE_Explorer.Properties.Resources.TRE_Explorer_chm);
        binaryWriter.Close();
        fileStream.Close();

        this.m_FormNotifyIcon.m_TemporaryFiles.Add(filename);
      }

      Help.ShowHelp(this, filename, HelpNavigator.TableOfContents);
    }

    private void iFFILFWSEditorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.ToggleIFFILFWSEditor();
    }

    private void pALEditorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.TogglePALEditor();
    }

    private void sTFEditorToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.ToggleSTFEditor();
    }

    private void tOCTREViewerToolStripMenuItem_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.ToggleTOCTREViewer();
    }

    private void toolStripTopMenuItemOpenTRETOC_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenTOCTRE();
    }

    private void toolStripTopMenuItemOpenIFF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenIFFILFWS();
    }

    private void toolStripTopMenuItemOpenPAL_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenPAL();
    }

    private void toolStripTopMenuItemOpenSTF_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.OpenSTF();
    }

    private void toolStripTopMenuItemOpen_Click(object sender, EventArgs e) {
      this.m_FormNotifyIcon.Open();
    }

    private void toolStripTopMenuItemExit_Click(object sender, EventArgs e) {
      if (!this.m_FormNotifyIcon.m_Exitting) {
        this.m_FormNotifyIcon.m_Exitting = true;
        Application.Exit();
      }
    }
    #endregion

    private void dataGridViewIff_KeyDown(object sender, KeyEventArgs e) {
      // CTRL+V
      if ((this.m_IFFFile != null) && (this.m_IFFFile.IsDataTable) && (this.dataGridViewIff.CurrentCell != null) && (e.KeyCode == Keys.V) && (!e.Alt) && (e.Control) && (!e.Shift) && (Clipboard.ContainsText())) {
        if ((Clipboard.GetText().Contains("\r")) || (Clipboard.GetText().Contains("\n")) || (Clipboard.GetText().Contains("\t"))) {
          Int32 columnIndex = this.dataGridViewIff.CurrentCell.ColumnIndex;
          Int32 rowIndex = this.dataGridViewIff.CurrentCell.RowIndex;
          String[] splitRow = Clipboard.GetText().Replace("\r\n", "\r").Split(new Char[] { '\r' });
          for (Int32 row = rowIndex; row < rowIndex + splitRow.Length; row++) {
            String[] splitColumn = splitRow[row - rowIndex].Split(new Char[] { '\t' });
            for (Int32 column = columnIndex; column < columnIndex + splitColumn.Length; column++) {
              try {
                switch (this.dataGridViewIff[column, row].Value.GetType().FullName) {
                  case "System.String":
                    this.dataGridViewIff[column, row].Value = splitColumn[column - columnIndex];
                    break;

                  case "System.Int32":
                    this.dataGridViewIff[column, row].Value = Int32.Parse(splitColumn[column - columnIndex]);
                    break;

                  case "System.Single":
                    this.dataGridViewIff[column, row].Value = Single.Parse(splitColumn[column - columnIndex]);
                    break;

                  case "System.Boolean":
                    this.dataGridViewIff[column, row].Value = Boolean.Parse(splitColumn[column - columnIndex]);
                    break;
                }
              } catch {
              }
            }
          }
        } else {
          foreach (DataGridViewCell dataGridViewCell in this.dataGridViewIff.SelectedCells) {
            switch (dataGridViewCell.Value.GetType().FullName) {
              case "System.String":
                dataGridViewCell.Value = Clipboard.GetText();
                break;
              
              case "System.Int32":
                dataGridViewCell.Value = Int32.Parse(Clipboard.GetText());
                break;
              
              case "System.Single":
                dataGridViewCell.Value = Single.Parse(Clipboard.GetText());
                break;

              case "System.Boolean":
                dataGridViewCell.Value = Boolean.Parse(Clipboard.GetText());
                break;
            }
          }
        }
      }

      // DEL
      if ((this.m_IFFFile != null) && (this.m_IFFFile.IsDataTable) && (this.dataGridViewIff.SelectedRows.Count == 0) && (this.dataGridViewIff.SelectedCells.Count > 0) && (e.KeyCode == Keys.Delete) && (!e.Alt) && (!e.Control) && (!e.Shift)) {
        e.Handled = true;
        foreach (DataGridViewCell dataGridViewCell in this.dataGridViewIff.SelectedCells) {
          if (dataGridViewCell.ValueType == typeof(String)) {
            dataGridViewCell.Value = String.Empty;
          } else if (dataGridViewCell.ValueType == typeof(Int32)) {
            dataGridViewCell.Value = 0;
          } else if (dataGridViewCell.ValueType == typeof(Single)) {
            dataGridViewCell.Value = 0F;
          } else if (dataGridViewCell.ValueType == typeof(Boolean)) {
            dataGridViewCell.Value = false;
          }
        }
      }
    }

    private void toolStripTextBoxSearch_KeyDown(object sender, KeyEventArgs e) {
      if (e.KeyData == Keys.Enter) {
        this.toolStripButtonSearch.PerformClick();
        e.Handled = true;
      }
    }

    private void toolStripButtonSearch_Click(object sender, EventArgs e) {
      if ((this.m_IFFFile != null) && (this.m_IFFFile.IsDataTable) && (this.toolStripTextBoxSearch.Text != String.Empty)) {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (DataGridViewCell dataGridViewCell in this.dataGridViewIff.Rows[0].Cells) {
          stringBuilder.AppendLine(dataGridViewCell.ColumnIndex.ToString());
        }

        Int32 rowIndex = (((this.dataGridViewIff.SelectedCells != null) && (this.dataGridViewIff.SelectedCells.Count > 0)) ? this.dataGridViewIff.SelectedCells[this.dataGridViewIff.SelectedCells.Count - 1].RowIndex : 0);
        Int32 columnIndex = (((this.dataGridViewIff.SelectedCells != null) && (this.dataGridViewIff.SelectedCells.Count > 0)) ? this.dataGridViewIff.SelectedCells[this.dataGridViewIff.SelectedCells.Count - 1].ColumnIndex : 0);

        if ((columnIndex == (this.dataGridViewIff.ColumnCount - 1)) && (rowIndex == (this.dataGridViewIff.RowCount - 1))) {
          MessageBox.Show("End of file reached.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        } else if (columnIndex == (this.dataGridViewIff.ColumnCount - 1)) {
          columnIndex = 0;
          rowIndex++;
        } else {
          columnIndex++;
        }

        for (Int32 row = rowIndex; row < this.dataGridViewIff.RowCount; row++) {
          for (Int32 column = ((row == rowIndex) ? columnIndex : 0); column < this.dataGridViewIff.ColumnCount; column++) {
            if ((this.dataGridViewIff[column, row].Value != null) && (this.dataGridViewIff[column, row].Value.ToString().ToLower().Contains(this.toolStripTextBoxSearch.Text.ToLower()))) {
              this.dataGridViewIff.ClearSelection();
              this.dataGridViewIff[column, row].Selected = true;
              this.dataGridViewIff.FirstDisplayedCell = this.dataGridViewIff[column, row];

              return;
            }
          }
        }

        MessageBox.Show("End of file reached.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Information);
      } else {
        MessageBox.Show("You must first load a file and enter a search term.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void findDataTableToolStripMenuItem_Click(object sender, EventArgs e) {
      if (this.toolStripTextBoxSearch.Visible) {
        this.toolStripTextBoxSearch.Focus();
        if (this.toolStripTextBoxSearch.Text.Length > 0) {
          this.toolStripTextBoxSearch.Select(0, this.toolStripTextBoxSearch.Text.Length);
        }
      }
    }

    private void findAgainDataTableToolStripMenuItem_Click(object sender, EventArgs e) {
      if (this.toolStripButtonSearch.Visible) {
        this.toolStripButtonSearch.PerformClick();
      }
    }

    private void toolStripButtonSearch_VisibleChanged(object sender, EventArgs e) {
      this.findToolStripMenuItem.Enabled = (this.toolStripButtonIffFind.Visible || this.toolStripButtonSearch.Visible);
      this.findAgainToolStripMenuItem.Enabled = (this.toolStripButtonIffFind.Visible || this.toolStripButtonSearch.Visible);
    }

    private void dataGridViewIff_SelectionChanged(object sender, EventArgs e) {
      if ((this.dataGridViewIff.SelectedCells.Count > 0) && (Control.MouseButtons == MouseButtons.Left)) {
        List<Int32> rows = new List<Int32>();
        List<Int32> columns = new List<Int32>();
        foreach (DataGridViewCell dataGridViewCell in this.dataGridViewIff.SelectedCells) {
          if (!rows.Contains(dataGridViewCell.RowIndex)) {
            rows.Add(dataGridViewCell.RowIndex);
          }
          if (!columns.Contains(dataGridViewCell.ColumnIndex)) {
            columns.Add(dataGridViewCell.ColumnIndex);
          }
        }
        this.toolStripStatusLabelInsertOverwrite.Text = rows.Count + "R x " + columns.Count + "C";
      } else if (this.dataGridViewIff.SelectedCells.Count > 0) {
        String[] columns = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        Int32 columnIndex = this.dataGridViewIff.CurrentCell.ColumnIndex + 1;
        this.toolStripStatusLabelInsertOverwrite.Text = ((columnIndex > 26) ? columns[(columnIndex / 26) - 1] + columns[columnIndex % 26] : columns[columnIndex - 1]) + (this.dataGridViewIff.CurrentCell.RowIndex + 1).ToString();
      } else {
        this.toolStripStatusLabelInsertOverwrite.Text = String.Empty;
      }
    }

    private void dataGridViewIff_MouseUp(object sender, MouseEventArgs e) {
      this.dataGridViewIff_SelectionChanged(this.dataGridViewIff, new EventArgs());
    }

    private void contextMenuStripTreeViewIFF_Opening(object sender, CancelEventArgs e) {
      TreeNode treeNode = this.treeViewIff.GetNodeAt(this.treeViewIff.PointToClient(Cursor.Position));
      if ((treeNode != null) && (this.treeViewIff.Nodes.Count > 0)) {
        if (treeNode != this.treeViewIff.SelectedNode) {
          this.treeViewIff.SelectedNode = treeNode;
        }

        if (treeNode == this.treeViewIff.Nodes[0]) {
          this.toolStripMenuItemContextCopy.Enabled = false;
          this.toolStripMenuItemContextCut.Enabled = false;
          this.toolStripMenuItemContextPaste.Enabled = false;
          this.toolStripMenuItemContextRemoveNode.Enabled = false;
          this.toolStripMenuItemContextRenameNode.Enabled = false;
        } else {
          this.toolStripMenuItemContextCopy.Enabled = true;
          this.toolStripMenuItemContextCut.Enabled = true;
          this.toolStripMenuItemContextRemoveNode.Enabled = true;
          this.toolStripMenuItemContextRenameNode.Enabled = true;
        }
        this.toolStripMenuItemContextAddNode.Enabled = (treeNode.Text.Contains("FORM"));
        this.toolStripMenuItemContextPaste.Enabled = ((treeNode.Text.Contains("FORM")) && (Clipboard.ContainsData(typeof(IFFFile.IFFNode).FullName)));

        this.contextMenuStripTreeViewIFF.Invalidate();
      } else {
        e.Cancel = true;
      }
    }

    private void toolStripMenuItemContextCopy_Click(object sender, EventArgs e) {
      if (this.treeViewIff.SelectedNode != null) {
        Clipboard.SetData(typeof(IFFFile.IFFNode).FullName, this.FindIFFNode(this.treeViewIff.SelectedNode));
      }
    }

    private void toolStripMenuItemContextCut_Click(object sender, EventArgs e) {
      if (this.treeViewIff.SelectedNode != null) {
        IFFFile.IFFNode iffNode = this.FindIFFNode(this.treeViewIff.SelectedNode);
        Clipboard.SetData(typeof(IFFFile.IFFNode).FullName, iffNode);
        this.treeViewIff.SelectedNode.Remove();
        iffNode.Parent.Children.Remove(iffNode);

        this.HasChanges = true;
      }
    }

    private void toolStripMenuItemContextPaste_Click(object sender, EventArgs e) {
      if (this.treeViewIff.SelectedNode != null) {
        IFFFile.IFFNode iffSource = (IFFFile.IFFNode)Clipboard.GetData(typeof(IFFFile.IFFNode).FullName);
        IFFFile.IFFNode iffDestination = this.FindIFFNode(this.treeViewIff.SelectedNode);

        iffSource.Parent = iffDestination;
        iffDestination.Children.Add(iffSource);

        this.IFFRecurseNode(iffSource, this.CreateIFFNode(iffSource, this.treeViewIff.SelectedNode));

        this.HasChanges = true;
      }
    }
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Be.Windows.Forms;
using BrightIdeasSoftware;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using Media;
using SWGLib;

namespace TRE_Explorer
{
    #region Structures
    public struct ExtractData
    {
        public String Path;
        public DataTable DataTable;
    }

    public struct LoadReturn
    {
        public TREFile TREFile;
        public TreeNode TreeNode;
        public String[] Containers;
        public String[] FileTypes;
    }

    public struct ExtractChainData
    {
        public String Path;
        public DataRow DataRow;
    }
    #endregion

    #region Delegates
    internal delegate void DelegateSetProgress(Int32[] progress);
    internal delegate void DelegateSetTitle(String title);
    internal delegate void DelegateSetCaption(String caption);
    internal delegate void DelegateSetLabel(String label);
    internal delegate void DelegateStopThread();
    internal delegate void DelegateLoadThreadFinished(LoadReturn loadReturn);
    internal delegate void DelegateExtractThreadFinished();
    #endregion

    public partial class FormTOCTREViewer : Form
    {
        #region Global Variables
        private FormNotifyIcon m_FormNotifyIcon;
        private FormFilter m_FormFilter;
        private FormProgress m_FormProgress;

        private TreeNode m_LastClickedTreeNode = new TreeNode();

        private FormDDSTGARenderFrame m_FormDDSTGARenderFrame;
        private MP3Player m_MP3Player = new MP3Player();
        private SoundPlayer m_SoundPlayer;
        private IFFFile m_IFFFilePreview;
        private PALFile m_PALFilePreview;
        private STFFile m_STFFilePreview;
        private Int32 m_LastPreviewed = -1;

        private DataView m_DataViewListView;
        private String m_DataViewListViewSortColumn = "SortName";
        private SortOrder m_DataViewListViewSortOrder = SortOrder.Ascending;
        private String m_StaticFilter = String.Empty;
        private TREFile m_TREFile;

        private Boolean m_PreviewPaneVisible = false;
        private Boolean m_DetailsPaneVisible = false;
        private Boolean m_NavigationPaneVisible = false;
        private String m_FormTOCTREViewer_Text = String.Empty;

        private Thread m_Thread;
        private ManualResetEvent m_ManualResetEvent_StopThread;
        private ManualResetEvent m_ManualResetEvent_ThreadStopped;

        internal DelegateSetProgress delegateSetProgress;
        internal DelegateSetTitle delegateSetTitle;
        internal DelegateSetCaption delegateSetCaption;
        internal DelegateSetLabel delegateSetLabel;
        internal DelegateStopThread delegateStopThread;
        internal DelegateLoadThreadFinished delegateLoadThreadFinished;
        internal DelegateExtractThreadFinished delegateExtractThreadFinished;
        #endregion

        #region Delegate Functions
        private void SetProgress(Int32[] progress)
        {
            if (progress.Length == 2)
            {
                this.m_FormProgress.progressBarFile.Visible = true;
            }
            else
            {
                this.m_FormProgress.progressBarFile.Visible = false;
            }

            if (progress[0] == -1)
            {
                if (m_FormProgress.progressBar.Style != ProgressBarStyle.Marquee)
                {
                    m_FormProgress.progressBar.Style = ProgressBarStyle.Marquee;
                }
            }
            else
            {
                if (m_FormProgress.progressBar.Style != ProgressBarStyle.Continuous)
                {
                    m_FormProgress.progressBar.Style = ProgressBarStyle.Continuous;
                }
                Int32 value = Math.Min(m_FormProgress.progressBar.Maximum, Math.Max(m_FormProgress.progressBar.Minimum, progress[0]));
                if (m_FormProgress.progressBar.Value != value)
                {
                    m_FormProgress.progressBar.Value = value;
                }
            }

            if (progress.Length == 2)
            {
                if (progress[1] == -1)
                {
                    if (m_FormProgress.progressBarFile.Style != ProgressBarStyle.Marquee)
                    {
                        m_FormProgress.progressBarFile.Style = ProgressBarStyle.Marquee;
                    }
                }
                else
                {
                    if (m_FormProgress.progressBarFile.Style != ProgressBarStyle.Continuous)
                    {
                        m_FormProgress.progressBarFile.Style = ProgressBarStyle.Continuous;
                    }
                    Int32 value = Math.Min(m_FormProgress.progressBarFile.Maximum, Math.Max(m_FormProgress.progressBarFile.Minimum, progress[1]));
                    if (m_FormProgress.progressBarFile.Value != value)
                    {
                        m_FormProgress.progressBarFile.Value = value;
                    }
                }
            }
        }

        private void SetTitle(String title)
        {
            if (this.Text != title)
            {
                this.Text = title;
            }
        }

        private void SetCaption(String caption)
        {
            if (m_FormProgress.Text != caption)
            {
                m_FormProgress.Text = caption;
            }
        }

        private void SetLabel(String label)
        {
            if (m_FormProgress.label.Text != label)
            {
                m_FormProgress.label.Text = label;
            }
        }
        #endregion

        #region Thread Functions
        private void StopThread()
        {
            if ((m_Thread != null) && m_Thread.IsAlive)
            {
                m_ManualResetEvent_StopThread.Set();

                while (m_Thread.IsAlive)
                {
                    if (WaitHandle.WaitAll((new ManualResetEvent[] { m_ManualResetEvent_ThreadStopped }), 100, true))
                    {
                        break;
                    }

                    Application.DoEvents();
                }
            }

            this.m_FormProgress.Hide();

            this.treeView.Enabled = true;
            this.listView.Enabled = true;
            this.toolStripButtonBack.Enabled = true;
            this.toolStripButtonExport.Enabled = (this.listView.SelectedIndices.Count > 0);
            this.toolStripSplitButtonOpen.Enabled = true;
            this.toolStripButtonSearch.Enabled = (this.toolStripTextBoxSearch.Text.Length > 0);
            this.toolStripSplitButtonLayout.Enabled = true;
            this.toolStripSplitButtonViews.Enabled = true;
            this.toolStripTextBoxSearch.Enabled = true;
            this.toolStripButtonAbout.Enabled = true;
            this.toolStripButtonFilter.Enabled = true;
            this.toolStripButtonExportFileChain.Enabled = ((this.m_IFFFilePreview != null) && (!this.m_IFFFilePreview.IsDataTable));

            this.m_Thread = null;

            m_ManualResetEvent_StopThread.Reset();
            m_ManualResetEvent_ThreadStopped.Reset();
        }

        private void LoadThreadFinished(LoadReturn loadReturn)
        {
            this.m_FormProgress.Hide();

            if (loadReturn.TREFile != null)
            {
                this.SuspendLayout();
                this.m_TREFile = loadReturn.TREFile;
                this.m_DataViewListView = new DataView(this.m_TREFile.DataTable);

                this.treeView.SuspendLayout();
                this.treeView.Nodes.Clear();
                this.treeView.Nodes.Add(loadReturn.TreeNode);
                this.treeView.ResumeLayout(false);
                PopulateFilters(loadReturn.Containers, loadReturn.FileTypes);

                this.treeView.CollapseAll();
                this.treeView.Nodes[0].Expand();
                this.treeView.SelectedNode = this.treeView.Nodes[0];

                this.toolStripMenuItemNavigationPane.Checked = true;
                this.toolStripMenuItemPreviewPane.Checked = true;
                this.toolStripMenuItemDetailsPane.Checked = true;
                this.ResumeLayout(false);

                this.Focus();
            }
            else
            {
                this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                this.toolStripMenuItemNavigationPane.Checked = false;
                this.toolStripMenuItemPreviewPane.Checked = false;
                this.toolStripMenuItemDetailsPane.Checked = false;
            }

            m_ManualResetEvent_StopThread.Reset();
            m_ManualResetEvent_ThreadStopped.Reset();
        }

        private void LoadThread(Object filename)
        {
            LoadProcess loadProcess = new LoadProcess(m_ManualResetEvent_StopThread, m_ManualResetEvent_ThreadStopped, this, filename.ToString());
            loadProcess.Run();
        }

        private void ExtractThreadFinished()
        {
            this.m_FormProgress.Hide();

            this.treeView.Enabled = true;
            this.listView.Enabled = true;
            this.toolStripButtonBack.Enabled = true;
            this.toolStripButtonExport.Enabled = (this.listView.SelectedIndices.Count > 0);
            this.toolStripSplitButtonOpen.Enabled = true;
            this.toolStripButtonSearch.Enabled = (this.toolStripTextBoxSearch.Text.Length > 0);
            this.toolStripSplitButtonLayout.Enabled = true;
            this.toolStripSplitButtonViews.Enabled = true;
            this.toolStripTextBoxSearch.Enabled = true;
            this.toolStripButtonAbout.Enabled = true;
            this.toolStripButtonFilter.Enabled = true;
            this.toolStripButtonExportFileChain.Enabled = ((this.m_IFFFilePreview != null) && (!this.m_IFFFilePreview.IsDataTable));

            m_ManualResetEvent_StopThread.Reset();
            m_ManualResetEvent_ThreadStopped.Reset();
        }

        private void ExtractThread(Object passedObject)
        {
            ExtractData passedData = (ExtractData)passedObject;
            ExtractProcess extractProcess = new ExtractProcess(m_ManualResetEvent_StopThread, m_ManualResetEvent_ThreadStopped, this, passedData.DataTable, passedData.Path);
            extractProcess.Run();
        }

        private void ExtractChainThread(Object passedObject)
        {
            ExtractChainData extractChainData = (ExtractChainData)passedObject;
            ExtractChainProcess extractChainProcess = new ExtractChainProcess(m_ManualResetEvent_StopThread, m_ManualResetEvent_ThreadStopped, this, this.m_DataViewListView.Table, extractChainData.DataRow, extractChainData.Path);
            extractChainProcess.Run();
        }
        #endregion

        #region Helper Functions
        private void CheckSortBy()
        {
            this.toolStripMenuItemNameListView.Checked = false;
            this.toolStripMenuItemTypeListView.Checked = false;
            this.toolStripMenuItemSizeListView.Checked = false;
            this.toolStripMenuItemAscendingListView.Checked = false;
            this.toolStripMenuItemDescendingListView.Checked = false;

            this.toolStripTopMenuItemName.Checked = false;
            this.toolStripTopMenuItemType.Checked = false;
            this.toolStripTopMenuItemSize.Checked = false;
            this.toolStripTopMenuItemAscending.Checked = false;
            this.toolStripTopMenuItemDescending.Checked = false;

            switch (this.m_DataViewListViewSortOrder)
            {
                case SortOrder.Ascending:
                    this.toolStripMenuItemAscendingListView.Checked = true;
                    this.toolStripTopMenuItemAscending.Checked = true;
                    break;

                case SortOrder.Descending:
                    this.toolStripMenuItemDescendingListView.Checked = true;
                    this.toolStripTopMenuItemDescending.Checked = true;
                    break;
            }

            switch (this.m_DataViewListViewSortColumn)
            {
                case "SortName":
                    this.toolStripMenuItemNameListView.Checked = true;
                    this.toolStripTopMenuItemName.Checked = true;
                    break;

                case "final_size":
                    this.toolStripMenuItemSizeListView.Checked = true;
                    this.toolStripTopMenuItemSize.Checked = true;
                    break;

                case "file_type":
                    this.toolStripMenuItemTypeListView.Checked = true;
                    this.toolStripTopMenuItemType.Checked = true;
                    break;
            }
        }

        internal void CheckView()
        {
            this.toolStripMenuItemDetails.Checked = false;
            this.toolStripMenuItemLargeIcons.Checked = false;
            this.toolStripMenuItemList.Checked = false;
            this.toolStripMenuItemSmallIcons.Checked = false;

            this.toolStripMenuItemDetailsListView.Checked = false;
            this.toolStripMenuItemLargeIconsListView.Checked = false;
            this.toolStripMenuItemListListView.Checked = false;
            this.toolStripMenuItemSmallIconsListView.Checked = false;

            this.toolStripTopMenuItemDetails.Checked = false;
            this.toolStripTopMenuItemLargeIcons.Checked = false;
            this.toolStripTopMenuItemList.Checked = false;
            this.toolStripTopMenuItemSmallIcons.Checked = false;

            switch (this.listView.View)
            {
                case View.Details:
                    this.toolStripMenuItemDetails.Checked = true;
                    this.toolStripMenuItemDetailsListView.Checked = true;
                    this.toolStripTopMenuItemDetails.Checked = true;
                    this.listView.ShowItemToolTips = true;
                    break;

                case View.LargeIcon:
                    this.toolStripMenuItemLargeIcons.Checked = true;
                    this.toolStripMenuItemLargeIconsListView.Checked = true;
                    this.toolStripTopMenuItemLargeIcons.Checked = true;
                    this.listView.ShowItemToolTips = false;
                    break;

                case View.List:
                    this.toolStripMenuItemList.Checked = true;
                    this.toolStripMenuItemListListView.Checked = true;
                    this.toolStripTopMenuItemList.Checked = true;
                    this.listView.ShowItemToolTips = true;
                    break;

                case View.SmallIcon:
                    this.toolStripMenuItemSmallIcons.Checked = true;
                    this.toolStripMenuItemSmallIconsListView.Checked = true;
                    this.toolStripTopMenuItemSmallIcons.Checked = true;
                    this.listView.ShowItemToolTips = false;
                    break;
            }
        }

        private Boolean CompareByteArrays(Byte[] array1, Byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (Int32 counter = 0; counter < array1.Length; counter++)
            {
                if (array1[counter] != array2[counter])
                {
                    return false;
                }
            }

            return true;
        }

        private Int32 ImageIndex(String filetype)
        {
            if (filetype.ToLower() == "file folder")
            {
                return 0;
            }

            String[] fileTypes = new String[] { "ANS", "APT", "ASH", "BIK", "CDF", "CEF", "CFG", "CMP", "DDS", "EFT", "FFE", "FLR", "IFF", "ILF", "INC", "LAT", "LAY", "LMG", "LOD", "LSB", "LTN", "MGN", "MKR", "MP3", "MSH", "PAL", "PLN", "POB", "PRT", "PSH", "PST", "QST", "SAT", "SFP", "SHT", "SKT", "SND", "SPR", "STF", "SWH", "TGA", "TRN", "TRT", "UI", "VSH", "WAV", "WS" };
            for (Int32 counter = 0; counter < fileTypes.Length; counter++)
            {
                if (filetype.ToLower().Equals(fileTypes[counter].ToLower() + " file"))
                {
                    return counter + 1;
                }
            }

            return -1;
        }

        private Boolean IsChildOf(TreeNode parent, TreeNode child)
        {
            List<TreeNode> listParents = new List<TreeNode>();
            TreeNode pointer = child;
            while (pointer.Parent != null)
            {
                pointer = pointer.Parent;
                listParents.Add(pointer);
            }

            return listParents.Contains(parent);
        }

        private void PopulateFilters(String[] Containers, String[] FileTypes)
        {
            this.m_FormFilter.Initialize(Containers, FileTypes);
        }

        private DataTable RecurseFolder(DataRow dataRow)
        {
            DataTable returnValue = this.m_DataViewListView.ToTable();
            returnValue.Rows.Clear();

            DataView tempDataView = new DataView(this.m_TREFile.DataTable);
            tempDataView.RowFilter = "Path LIKE '/" + dataRow["name"].ToString().Trim(new Char[] { '/' }) + "/%'";

            DataTable dataTable = tempDataView.ToTable();
            tempDataView = new DataView(dataTable);

            tempDataView.RowFilter = "File_Type NOT LIKE 'File Folder'";
            foreach (DataRowView folderDataRowView in tempDataView)
            {
                returnValue.ImportRow(folderDataRowView.Row);
            }

            return returnValue;
        }

        private DataTable RetrieveSelection()
        {
            DataTable returnValue = this.m_DataViewListView.ToTable();
            returnValue.Rows.Clear();

            DataTable tempDataTable = this.m_DataViewListView.ToTable();
            tempDataTable.Rows.Clear();

            foreach (Int32 index in this.listView.SelectedIndices)
            {
                tempDataTable.ImportRow(this.m_DataViewListView[index].Row);
            }
            DataView tempDataView = new DataView(tempDataTable);

            tempDataView.RowFilter = "File_Type LIKE 'File Folder'";
            foreach (DataRowView dataRowView in tempDataView)
            {
                returnValue.Merge(RecurseFolder(dataRowView.Row));
            }
            tempDataView.RowFilter = "File_Type NOT LIKE 'File Folder'";
            foreach (DataRowView dataRowView in tempDataView)
            {
                returnValue.ImportRow(dataRowView.Row);
            }

            return returnValue;
        }

        private void SelectTreeNode(String path)
        {
            TreeNode treeBranch = this.treeView.Nodes[0];
            treeBranch.Expand();
            foreach (String node in path.Trim(new Char[] { '/' }).Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                treeBranch = treeBranch.Nodes[node];
                treeBranch.Expand();
            }

            this.treeView.SelectedNode = treeBranch;
            this.listView.SelectedIndices.Clear();
        }

        private String TextToFilter(String text)
        {
            String filter = String.Empty;
            Int32 position = 0;
            foreach (String searchterm in text.Split(new Char[] { '|', '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                filter += ((filter == String.Empty) ? "" : ((text.Substring((position - 1), 1) == "|") ? " OR " : " AND ")) + "(SearchName " + ((searchterm.StartsWith("!")) ? "NOT " : String.Empty) + "LIKE '%" + ((searchterm.StartsWith("!")) ? searchterm.Substring(1) : searchterm) + "%')";
                position += (searchterm.Length + 1);
            }
            return filter;
        }
        #endregion

        #region FileType Functions
        internal void TOCTRELoadFile(String filename)
        {
            try
            {
                HidePreviewControls();

                this.pictureBoxDetails.Image = new Bitmap(this.pictureBoxDetails.Width, this.pictureBoxDetails.Height);
                this.webBrowserDetails.DocumentText = "<BODY BGCOLOR=\"#" + String.Format("{0:X}", SystemColors.Control.R) + String.Format("{0:X}", SystemColors.Control.G) + String.Format("{0:X}", SystemColors.Control.B) + "\"></BODY>";
                this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                this.treeView.Nodes.Clear();
                m_FormProgress.Show();

                this.listView.VirtualListSize = 0;
                this.listView.Refresh();

                this.m_TREFile = null;

                this.Visible = true;

                m_Thread = new Thread(new ParameterizedThreadStart(this.LoadThread));
                m_Thread.Name = "Load";
                m_Thread.Start(filename);
            }
            catch (Exception exception)
            {
                this.listView.Items.Clear();
                this.toolStripButtonExport.Enabled = false;
                this.Text = this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                MessageBox.Show("An exception occurred while loading " + filename + ": " + exception.Message, "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Preview Functions
        private void formRenderFrame_VisibleChanged(object sender, EventArgs e)
        {
            if (((this.m_FormDDSTGARenderFrame != null) && (this.m_FormDDSTGARenderFrame.Visible)))
            {
                this.saveAsToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.saveAsToolStripMenuItem.Enabled = false;
            }
        }

        private String ASCIIDump(Byte[] data)
        {
            StringBuilder stringBuilder = new StringBuilder(data.Length, data.Length);

            foreach (Byte character in data)
            {
                if (character > 0x1F && !(character > 0x7E && character < 0xA0))
                {
                    stringBuilder.Append((Char)character);
                }
                else
                {
                    stringBuilder.Append('.');
                }
                if (stringBuilder.Length >= (5 * 1024))
                {
                    stringBuilder.Append("…");
                    return stringBuilder.ToString();
                }
            }

            return stringBuilder.ToString();
        }

        private Boolean HidePreviewControls()
        {
            try
            {
                if (this.m_FormDDSTGARenderFrame != null)
                {
                    this.m_FormDDSTGARenderFrame.Visible = false;
                }

                this.textBoxPreview.Visible = false;
                this.dataGridViewPreview.Visible = false;
                this.buttonPlaySound.Visible = false;
                this.flowLayoutPanelPalPreview.Visible = false;

                this.toolStripIffPreview.Visible = false;
                this.toolStripStfPreview.Visible = false;
                this.toolStripPalPreview.Visible = false;

                this.m_LastPreviewed = -1;

                this.formRenderFrame_VisibleChanged(this.m_FormDDSTGARenderFrame, new EventArgs());

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void PreviewDDS(DataRow dataRow)
        {
            HidePreviewControls();
            try
            {
                Byte[] buffer = Utilities.InflateFile(dataRow);
                if ((buffer != null) && (buffer.Length > 0))
                {
                    String Name = (String)dataRow["Name"];

                    String path = Path.Combine(Path.GetTempPath(), Name.Replace("/", "\\").Substring(0, Name.LastIndexOf("/")));
                    String file = Name.Substring(Name.LastIndexOf("/") + 1);
                    String filename = Path.Combine(path, file);

                    if (!File.Exists(filename))
                    {
                        try
                        {
                            Directory.CreateDirectory(path);
                        }
                        catch
                        {
                        }

                        FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                        fileStream.Write(buffer, 0, buffer.Length);
                        fileStream.Close();
                        this.m_FormNotifyIcon.m_TemporaryFiles.Add(filename);
                        this.m_FormNotifyIcon.m_TemporaryPaths.Add(path);
                    }

                    try
                    {
                        this.m_FormDDSTGARenderFrame = new FormDDSTGARenderFrame(filename, this.m_FormNotifyIcon);
                        this.m_FormDDSTGARenderFrame.VisibleChanged += new EventHandler(formRenderFrame_VisibleChanged);
                        this.m_FormDDSTGARenderFrame.TopLevel = false;
                        this.m_FormDDSTGARenderFrame.FormBorderStyle = FormBorderStyle.None;
                        this.m_FormDDSTGARenderFrame.Dock = DockStyle.Fill;
                        this.panelPreviewBackground.Controls.Add(m_FormDDSTGARenderFrame);
                        this.m_FormDDSTGARenderFrame.Show();
                        this.m_FormDDSTGARenderFrame.BringToFront();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                HidePreviewControls();
            }
        }

        private void PreviewIFF(DataRow dataRow)
        {
            HidePreviewControls();
            Byte[] buffer = Utilities.InflateFile(dataRow);
            if ((buffer != null) && (buffer.Length > 0))
            {
                this.m_IFFFilePreview = new IFFFile(buffer);
                if (this.m_IFFFilePreview.IsDataTable)
                {
                    this.toolStripButtonIffEditFile.Image = TRE_Explorer.Properties.Resources.Edit_IFF_DataTable;

                    this.dataGridViewPreview.Visible = true;
                    this.toolStripIffPreview.Visible = true;

                    this.dataGridViewPreview.DataSource = this.m_IFFFilePreview.DataTable;
                    this.dataGridViewPreview.Invalidate();
                }
                else
                {
                    this.toolStripButtonIffEditFile.Image = TRE_Explorer.Properties.Resources.Edit_IFF;

                    this.textBoxPreview.Visible = true;
                    this.textBoxPreview.BringToFront();
                    this.textBoxPreview.Text = this.ASCIIDump(buffer);
                    this.toolStripIffPreview.Visible = (this.textBoxPreview.Text.StartsWith("FORM"));
                }
            }
        }

        private void PreviewINCQST(DataRow dataRow)
        {
            HidePreviewControls();
            Byte[] buffer = Utilities.InflateFile(dataRow);
            if ((buffer != null) && (buffer.Length > 0))
            {
                this.textBoxPreview.Visible = true;
                this.textBoxPreview.BringToFront();
                this.textBoxPreview.Text = new String(Encoding.ASCII.GetChars(buffer));
            }
        }

        private void PreviewMP3WAV()
        {
            HidePreviewControls();
            this.buttonPlaySound.Visible = true;
            this.buttonPlaySound.BringToFront();
        }

        private void PreviewOther(DataRow dataRow)
        {
            HidePreviewControls();
            this.toolStripButtonIffEditFile.Image = TRE_Explorer.Properties.Resources.Edit_IFF;
            Byte[] buffer = Utilities.InflateFile(dataRow);
            if ((buffer != null) && (buffer.Length > 0))
            {
                this.textBoxPreview.Visible = true;
                this.textBoxPreview.BringToFront();
                this.textBoxPreview.Text = this.ASCIIDump(buffer);
                if (this.textBoxPreview.Text.StartsWith("FORM"))
                {
                    this.toolStripIffPreview.Visible = true;
                    this.m_IFFFilePreview = new IFFFile(buffer);
                }
                else
                {
                    this.toolStripIffPreview.Visible = false;
                    this.m_IFFFilePreview = null;
                }
            }
        }

        private void PreviewPAL(DataRow dataRow)
        {
            HidePreviewControls();
            Byte[] buffer = Utilities.InflateFile(dataRow);
            if ((buffer != null) && (buffer.Length > 0))
            {
                String Name = (String)dataRow["Name"];
                this.m_PALFilePreview = new PALFile(new MemoryStream(buffer));
                this.m_PALFilePreview.Filename = Name.Substring(Name.LastIndexOf("/") + 1);
                this.flowLayoutPanelPalPreview.Visible = true;
                this.flowLayoutPanelPalPreview.BringToFront();
                this.flowLayoutPanelPalPreview.SuspendLayout();
                foreach (Color color in this.m_PALFilePreview.Colors)
                {
                    Button button = new Button();
                    button.Size = new Size(32, 32);
                    button.BackColor = color;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.Black;
                    button.FlatAppearance.BorderSize = 0;
                    button.FlatAppearance.MouseDownBackColor = color;
                    button.FlatAppearance.MouseOverBackColor = color;
                    button.Font = new Font("Microsoft Sans Serif", 6, FontStyle.Regular, GraphicsUnit.Point);
                    button.Text = this.m_PALFilePreview.HexadecimalIndex(color);
                    this.flowLayoutPanelPalPreview.Controls.Add(button);
                }
                this.flowLayoutPanelPalPreview.ResumeLayout(true);
            }
        }

        private void PreviewSTF(DataRow dataRow)
        {
            HidePreviewControls();
            Byte[] buffer = Utilities.InflateFile(dataRow);
            if ((buffer != null) && (buffer.Length > 0))
            {
                String Name = (String)dataRow["Name"];
                this.m_STFFilePreview = new STFFile(buffer);
                this.m_STFFilePreview.Filename = Name.Substring(Name.LastIndexOf("/") + 1);
                this.dataGridViewPreview.DataSource = this.m_STFFilePreview.DataTable;
                this.dataGridViewPreview.Visible = true;
                this.toolStripStfPreview.Visible = true;
            }
        }
        #endregion

        #region Form Functions
        public FormTOCTREViewer(FormNotifyIcon formNotifyIcon)
        {
            InitializeComponent();

            this.m_FormNotifyIcon = formNotifyIcon;

            this.listView.View = TRE_Explorer.Properties.Settings.Default.DefaultView;
            CheckView();

            m_FormFilter = new FormFilter();
            m_FormProgress = new FormProgress(this);
            m_FormProgress.Resize += new EventHandler(m_FormProgress_Resize);

            m_ManualResetEvent_StopThread = new ManualResetEvent(false);
            m_ManualResetEvent_ThreadStopped = new ManualResetEvent(false);

            delegateSetProgress = new DelegateSetProgress(this.SetProgress);
            delegateSetTitle = new DelegateSetTitle(this.SetTitle);
            delegateSetCaption = new DelegateSetCaption(this.SetCaption);
            delegateSetLabel = new DelegateSetLabel(this.SetLabel);
            delegateStopThread = new DelegateStopThread(this.StopThread);
            delegateLoadThreadFinished = new DelegateLoadThreadFinished(this.LoadThreadFinished);
            delegateExtractThreadFinished = new DelegateExtractThreadFinished(this.ExtractThreadFinished);

            this.listView.VirtualListSize = 0;
        }

        private void m_FormProgress_Resize(object sender, EventArgs e)
        {
            if ((this != null) && (this.Visible) && (this.WindowState != FormWindowState.Minimized))
            {
                if (this.m_FormProgress != null)
                {
                    this.m_FormProgress.Top = this.Top + (Int32)(((Single)this.Height - (Single)m_FormProgress.Height) / 2F);
                    this.m_FormProgress.Left = this.Left + (Int32)(((Single)this.Width - (Single)m_FormProgress.Width) / 2F);
                }
            }
        }

        private void FormTOCTREViewer_Activated(object sender, EventArgs e)
        {
            if (m_FormProgress.Visible)
            {
                m_FormProgress.Focus();
            }
        }

        private void FormTOCTREViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason != CloseReason.ApplicationExitCall) && (e.CloseReason != CloseReason.WindowsShutDown))
            {
                e.Cancel = true;
                this.Visible = false;
            }
            else
            {
                TRE_Explorer.Properties.Settings.Default.Save();

                StopThread();

                if (!HidePreviewControls())
                {
                    e.Cancel = true;
                    return;
                }

                try
                {
                    String lastFilename = m_MP3Player.FileName;
                    m_MP3Player.Stop();
                    m_MP3Player.Close();
                    File.Delete(lastFilename);
                }
                catch
                {
                }
                try
                {
                    m_SoundPlayer.Stop();
                }
                catch
                {
                }

                if (this.m_FormDDSTGARenderFrame != null)
                {
                    this.m_FormDDSTGARenderFrame.Close();
                }
            }
        }

        private void FormTOCTREViewer_Enter(object sender, EventArgs e)
        {
            if (m_FormProgress.Visible)
            {
                m_FormProgress.Focus();
            }
        }

        private void FormTOCTREViewer_Load(object sender, EventArgs e)
        {
            this.m_FormProgress.Top = this.Top + (Int32)(((Single)this.Height - (Single)m_FormProgress.Height) / 2F);
            this.m_FormProgress.Left = this.Left + (Int32)(((Single)this.Width - (Single)m_FormProgress.Width) / 2F);
            this.m_FormFilter.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormFilter.Height) / 2F);
            this.m_FormFilter.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormFilter.Width) / 2F);

            this.Focus();

            this.toolStripButtonSearch.Image = TRE_Explorer.Properties.Resources.Find;
            this.toolStripButtonSearch.Tag = true;

            this.splitContainerDetailsContent.Panel2Collapsed = true;
            this.splitContainerFilesPreview.Panel2Collapsed = true;
            this.splitContainerFolderTreeContent.Panel1Collapsed = true;

            this.toolStripMenuItemDetailsPane.Checked = false;
            this.toolStripMenuItemNavigationPane.Checked = false;
            this.toolStripMenuItemPreviewPane.Checked = false;

            this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            HidePreviewControls();

            CheckView();
            CheckSortBy();

            this.webBrowserDetails.DocumentText = "<BODY BGCOLOR=\"#" + String.Format("{0:X}", SystemColors.Control.R) + String.Format("{0:X}", SystemColors.Control.G) + String.Format("{0:X}", SystemColors.Control.B) + "\"></BODY>";
        }

        private void FormTOCTREViewer_Move(object sender, EventArgs e)
        {
            if ((this != null) && (this.Visible) && (this.WindowState != FormWindowState.Minimized))
            {
                if (this.m_FormProgress != null)
                {
                    this.m_FormProgress.Top = this.Top + (Int32)(((Single)this.Height - (Single)m_FormProgress.Height) / 2F);
                    this.m_FormProgress.Left = this.Left + (Int32)(((Single)this.Width - (Single)m_FormProgress.Width) / 2F);
                }
                if (this.m_FormFilter != null)
                {
                    this.m_FormFilter.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormFilter.Height) / 2F);
                    this.m_FormFilter.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormFilter.Width) / 2F);
                }
            }
        }

        private void FormTOCTREViewer_Resize(object sender, EventArgs e)
        {
            if ((this != null) && (this.Visible) && (this.WindowState != FormWindowState.Minimized))
            {
                if (this.m_FormProgress != null)
                {
                    this.m_FormProgress.Top = this.Top + (Int32)(((Single)this.Height - (Single)m_FormProgress.Height) / 2F);
                    this.m_FormProgress.Left = this.Left + (Int32)(((Single)this.Width - (Single)m_FormProgress.Width) / 2F);
                }
                if (this.m_FormFilter != null)
                {
                    this.m_FormFilter.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormFilter.Height) / 2F);
                    this.m_FormFilter.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormFilter.Width) / 2F);
                }
            }
        }
        #endregion

        #region ListView Functions
        private String ConstructFilter(String filter, Boolean includeFolders)
        {
            String rowFilter = filter;
            if ((this.m_StaticFilter != null) && (this.m_StaticFilter != String.Empty))
            {
                rowFilter = "(" + filter + ") AND " + ((includeFolders) ? "(" : String.Empty) + "(" + this.m_StaticFilter + ")" + ((includeFolders) ? " OR (File_Type = 'File Folder'))" : String.Empty);
            }
            return rowFilter;
        }

        private void PopulateListView(String filter)
        {
            try
            {
                this.listView.ShowItemToolTips = false;
                this.listView.SuspendLayout();

                this.m_DataViewListView.RowFilter = ConstructFilter(filter, ((filter.Contains("SearchName") ? false : true)));
                this.listView.VirtualListSize = this.m_DataViewListView.Count;
                this.SortListView();
                this.listView.ResumeLayout(true);
                if ((this.listView.View == View.Details) || (this.listView.View == View.List))
                {
                    this.listView.ShowItemToolTips = true;
                }

                this.listView_SelectedIndexChanged(this.listView, new EventArgs());

                if (filter.Contains("SearchName"))
                {
                    this.toolStripButtonSearch.Image = TRE_Explorer.Properties.Resources.Cancel;
                    this.toolStripButtonSearch.Tag = false;
                }
                else
                {
                    this.toolStripButtonSearch.Image = TRE_Explorer.Properties.Resources.Find;
                    this.toolStripButtonSearch.Tag = true;
                }
            }
            catch
            {
            }
        }

        private void SortListView(Int32 columnIndex)
        {
            this.SortListView(this.listView.Columns[columnIndex].Tag.ToString());
        }

        private void SortListView(String column)
        {
            if (column == this.m_DataViewListViewSortColumn)
            {
                if (this.m_DataViewListViewSortOrder == SortOrder.Ascending)
                {
                    this.m_DataViewListViewSortOrder = SortOrder.Descending;
                }
                else
                {
                    this.m_DataViewListViewSortOrder = SortOrder.Ascending;
                }
            }
            else
            {
                if (this.m_DataViewListViewSortOrder == SortOrder.None)
                {
                    this.m_DataViewListViewSortOrder = SortOrder.Ascending;
                }
            }
            this.m_DataViewListViewSortColumn = column;

            this.SortListView();
        }

        private void SortListView(SortOrder sortOrder)
        {
            this.m_DataViewListViewSortOrder = sortOrder;

            this.SortListView();
        }

        private void SortListView()
        {
            if (this.Text.Contains("/"))
            {
                this.m_DataViewListView.Sort = this.m_DataViewListViewSortColumn + " " + ((this.m_DataViewListViewSortOrder == SortOrder.Ascending) ? "ASC" : "DESC") + ((this.m_DataViewListViewSortColumn != "SortName") ? ", SortName " + ((this.m_DataViewListViewSortOrder == SortOrder.Ascending) ? "ASC" : "DESC") : "");
                this.listView.Refresh();
                this.CheckSortBy();
            }
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(e.Column);
        }

        private void listView_ItemActivate(object sender, EventArgs e)
        {
            if (this.listView.SelectedIndices.Count == 1)
            {
                DataRowView dataRowView = this.m_DataViewListView[this.listView.SelectedIndices[0]];
                if (dataRowView["File_Type"].ToString() == "File Folder")
                {
                    this.listView.SuspendLayout();
                    String path = dataRowView["Name"].ToString().TrimEnd(new Char[] { '/' });
                    SelectTreeNode(path);
                    this.listView.ResumeLayout(false);
                }
                else
                {
                    this.toolStripButtonExport.PerformClick();
                }
            }
            else
            {
                this.toolStripButtonExport.PerformClick();
            }
        }

        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            /* String BaseFolder = "/";
            if ((Boolean)this.toolStripButtonSearch.Tag) {
              BaseFolder = (String)this.treeView.SelectedNode.Tag;
              if (BaseFolder.Length > 1) {
                BaseFolder = BaseFolder.TrimStart(new Char[] { '/' });
              }
              if (!BaseFolder.EndsWith("/")) {
                BaseFolder += "/";
              }
            }
            DataObjectEx dataObjectEx = new DataObjectEx(this.RetrieveSelection().Rows, BaseFolder);
            dataObjectEx.SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, null);
            dataObjectEx.SetData(NativeMethods.CFSTR_FILECONTENTS, null);
            dataObjectEx.SetData(NativeMethods.CFSTR_PERFORMEDDROPEFFECT, null);
            this.listView.DoDragDrop(dataObjectEx, DragDropEffects.Move); */
        }

        private void listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            try
            {
                if (this.m_TREFile != null)
                {
                    if (e.ItemIndex < this.m_DataViewListView.Count)
                    {
                        DataRowView dataRowView = this.m_DataViewListView[e.ItemIndex];
                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.ImageIndex = this.ImageIndex((String)dataRowView["File_Type"]);
                        listViewItem.Name = (String)dataRowView["Name"];
                        listViewItem.Text = ((String)dataRowView["SortName"]).Trim();
                        listViewItem.ToolTipText = ((String)dataRowView["SortName"]).Trim() + "\r\n" + dataRowView["File_Type"] + ((dataRowView["File_Type"].ToString() != "File Folder") ? "\r\nSize: " + Utilities.SizeToString(dataRowView["Final_Size"]) + "\r\nContainer: " + ((String)dataRowView["Filename"]).Substring(((String)dataRowView["Filename"]).LastIndexOf("\\") + 1) : "");
                        listViewItem.SubItems.Add((String)dataRowView["File_Type"]);
                        listViewItem.SubItems.Add(Utilities.SizeToString(dataRowView["Final_Size"]));
                        listViewItem.SubItems.Add(((String)dataRowView["Filename"]).Substring(((String)dataRowView["Filename"]).LastIndexOf("\\") + 1));
                        listViewItem.SubItems.Add((String)dataRowView["Path"]);
                        listViewItem.SubItems.Add(Utilities.SizeToString((Int32)dataRowView["Size"]));

                        e.Item = listViewItem;
                    }
                    else
                    {
                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.SubItems.AddRange(new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty });
                        e.Item = listViewItem;
                    }
                }
                else
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.SubItems.AddRange(new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty });
                    e.Item = listViewItem;
                }
            }
            catch
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems.AddRange(new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty });
                e.Item = listViewItem;
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.m_DataViewListView != null)
            {
                if ((this.listView.SelectedIndices.Count > 0) && (this.listView.SelectedIndices[0] < this.m_DataViewListView.Count))
                {
                    DataRow dataRow = this.m_DataViewListView[this.listView.SelectedIndices[0]].Row;
                    String name = (String)dataRow["Name"];
                    this.toolStripButtonExport.Enabled = true;

                    if (this.listView.SelectedIndices.Count == 1)
                    {
                        if ((this.m_LastPreviewed != this.listView.SelectedIndices[0]) && ((String)dataRow["File_Type"] != "File Folder"))
                        {
                            this.m_IFFFilePreview = null;
                            this.m_PALFilePreview = null;
                            this.m_STFFilePreview = null;

                            this.m_LastPreviewed = this.listView.SelectedIndices[0];
                            if ((name.ToLower().EndsWith(".dds")) || (name.ToLower().EndsWith(".tga")))
                            {
                                this.PreviewDDS(dataRow);
                            }
                            else if (name.ToLower().EndsWith(".iff"))
                            {
                                this.PreviewIFF(dataRow);
                            }
                            else if (name.ToLower().EndsWith(".stf"))
                            {
                                this.PreviewSTF(dataRow);
                            }
                            else if ((name.ToLower().EndsWith(".wav")) || (name.ToLower().EndsWith(".mp3")))
                            {
                                this.PreviewMP3WAV();
                            }
                            else if (name.ToLower().EndsWith(".pal"))
                            {
                                this.PreviewPAL(dataRow);
                            }
                            else if ((name.ToLower().EndsWith(".inc")) || (name.ToLower().EndsWith(".qst")))
                            {
                                this.PreviewINCQST(dataRow);
                            }
                            else
                            {
                                this.PreviewOther(dataRow);
                            }
                        }

                        Int32 imageIndex = this.ImageIndex(dataRow["File_Type"].ToString());
                        if (imageIndex != -1)
                        {
                            this.pictureBoxDetails.Image = new Bitmap(this.imageListLarge.Images[imageIndex], this.pictureBoxDetails.Width, this.pictureBoxDetails.Height);
                            name = name.Trim(new Char[] { '/' });
                            if (name.Contains("/"))
                            {
                                name = name.Substring(name.LastIndexOf("/") + 1);
                            }
                            if (dataRow["File_Type"].ToString() == "File Folder")
                            {
                                this.webBrowserDetails.DocumentText = "<BODY BGCOLOR=\"#" + String.Format("{0:X}", SystemColors.Control.R) + String.Format("{0:X}", SystemColors.Control.G) + String.Format("{0:X}", SystemColors.Control.B) + "\"><FONT FACE=\"Microsoft Sans Serif\" SIZE=\"2\">" + ((TRE_Explorer.Properties.Settings.Default.DetailsDisplaysFullPath) ? ((String)dataRow["Name"]).Trim() : name).TrimStart(new Char[] { '/' }) + "<BR>" + dataRow["File_Type"].ToString() + "</FONT></BODY>";
                            }
                            else
                            {
                                this.webBrowserDetails.DocumentText = "<BODY BGCOLOR=\"#" + String.Format("{0:X}", SystemColors.Control.R) + String.Format("{0:X}", SystemColors.Control.G) + String.Format("{0:X}", SystemColors.Control.B) + "\"><TABLE HEIGHT=\"*\" WIDTH=\"*\" BORDER=\"0\" CELPADDING=\"1\" CELLSPACING=\"1\"><TR VALIGN=\"TOP\"><TD ALIGN=\"LEFT\" NOWRAP><FONT FACE=\"Microsoft Sans Serif\" SIZE=\"2\">" + ((TRE_Explorer.Properties.Settings.Default.DetailsDisplaysFullPath) ? ((String)dataRow["Name"]).Trim() : name) + "<BR>" + dataRow["File_Type"].ToString() + "</FONT></TD><TD ALIGN=\"RIGHT\" NOWRAP><FONT COLOR=\"#888888\" FACE=\"Microsoft Sans Serif\" SIZE=\"2\">Size: <BR>Container: <BR>Compressed Size: </FONT></TD><TD ALIGN=\"LEFT\" NOWRAP><FONT FACE=\"Microsoft Sans Serif\" SIZE=\"2\">" + Utilities.SizeToString((Int32)dataRow["Final_Size"]) + "<BR>" + ((String)dataRow["Filename"]).Substring(((String)dataRow["Filename"]).LastIndexOf("\\") + 1) + "<BR>" + Utilities.SizeToString((Int32)dataRow["Size"]) + "</FONT></TD></TR></TABLE></BODY>";
                            }
                        }
                    }
                    else if (this.listView.SelectedIndices.Count > 1)
                    {
                        HidePreviewControls();

                        try
                        {
                            /* this.pictureBoxDetails.Image = new Bitmap(this.pictureBoxDetails.Width, this.pictureBoxDetails.Height);
                            Graphics graphics = Graphics.FromImage(this.pictureBoxDetails.Image);
                            foreach (Int32 index in this.listView.SelectedIndices) {
                              Int32 imageIndex = this.ImageIndex(this.dataViewListView[index]["file_type"].ToString());
                              if (imageIndex != -1) {
                                graphics.DrawImage(this.imageListLarge.Images[imageIndex], 0, 0, this.pictureBoxDetails.Width, this.pictureBoxDetails.Height);
                              }
                            } */
                            this.pictureBoxDetails.Image = this.imageListLarge.Images[this.ImageIndex((String)dataRow["File_Type"])];
                        }
                        catch
                        {
                        }

                        this.webBrowserDetails.DocumentText = "<BODY BGCOLOR=\"#" + String.Format("{0:X}", SystemColors.Control.R) + String.Format("{0:X}", SystemColors.Control.G) + String.Format("{0:X}", SystemColors.Control.B) + "\"><FONT FACE=\"Microsoft Sans Serif\" SIZE=\"2\">" + String.Format("{0:n0}", this.listView.SelectedIndices.Count) + " item" + ((this.listView.SelectedIndices.Count == 1) ? "" : "s") + " selected</FONT></BODY>";
                    }
                }
                else
                {
                    HidePreviewControls();

                    this.pictureBoxDetails.Image = new Bitmap(this.imageListLarge.Images[0], this.pictureBoxDetails.Width, this.pictureBoxDetails.Height);
                    this.webBrowserDetails.DocumentText = "<BODY BGCOLOR=\"#" + String.Format("{0:X}", SystemColors.Control.R) + String.Format("{0:X}", SystemColors.Control.G) + String.Format("{0:X}", SystemColors.Control.B) + "\"><FONT FACE=\"Microsoft Sans Serif\" SIZE=\"2\">" + String.Format("{0:n0}", this.m_DataViewListView.Count) + " item" + ((this.m_DataViewListView.Count == 1) ? "" : "s") + "</FONT></BODY>";
                }
            }
            else
            {
                HidePreviewControls();

                this.pictureBoxDetails.Image = new Bitmap(this.pictureBoxDetails.Width, this.pictureBoxDetails.Height);
                this.webBrowserDetails.DocumentText = "<BODY BGCOLOR=\"#" + String.Format("{0:X}", SystemColors.Control.R) + String.Format("{0:X}", SystemColors.Control.G) + String.Format("{0:X}", SystemColors.Control.B) + "\"></BODY>";
            }

            this.toolStripButtonExport.Enabled = (this.listView.SelectedIndices.Count > 0);
            this.toolStripButtonExportFileChain.Enabled = ((this.listView.SelectedIndices.Count == 1) && (this.m_IFFFilePreview != null) && (!this.m_IFFFilePreview.IsDataTable));
        }

        private void listView_VisibleChanged(object sender, EventArgs e)
        {
            this.toolStripButtonBack.Enabled = this.listView.Visible;
            this.selectAllToolStripMenuItem.Enabled = this.listView.Visible;
            this.selectNoneToolStripMenuItem.Enabled = this.listView.Visible;
            this.invertSelectionToolStripMenuItem.Enabled = this.listView.Visible;

            if (this.listView.Visible)
            {
                this.toolStripMenuItemDetailsPane.Checked = this.m_DetailsPaneVisible;
                this.toolStripMenuItemNavigationPane.Checked = this.m_NavigationPaneVisible;
                this.toolStripMenuItemPreviewPane.Checked = this.m_PreviewPaneVisible;
                if (this.m_FormTOCTREViewer_Text != String.Empty)
                {
                    this.Text = this.m_FormTOCTREViewer_Text;
                }
                else
                {
                    this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                }
            }
            else
            {
                this.m_DetailsPaneVisible = this.toolStripMenuItemDetailsPane.Checked;
                this.m_NavigationPaneVisible = this.toolStripMenuItemNavigationPane.Checked;
                this.m_PreviewPaneVisible = this.toolStripMenuItemPreviewPane.Checked;
                this.m_FormTOCTREViewer_Text = this.Text;

                this.toolStripMenuItemDetailsPane.Checked = false;
                this.toolStripMenuItemNavigationPane.Checked = false;
                this.toolStripMenuItemPreviewPane.Checked = false;
            }
        }
        #endregion

        #region ToolStripMain Functions
        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.About(this);
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            if (this.treeView.Nodes.Count > 0)
            {
                String path = String.Empty;
                if ((Boolean)this.toolStripButtonSearch.Tag)
                {
                    path = this.treeView.SelectedNode.Tag.ToString().Trim(new Char[] { '/' });
                    if (path.Contains("/"))
                    {
                        path = "/" + path.Substring(0, path.LastIndexOf("/")) + "/";
                    }
                    else
                    {
                        path = "/";
                    }
                }
                else
                {
                    path = this.treeView.SelectedNode.Tag.ToString();
                }

                this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_TREFile.Filename.Substring(this.m_TREFile.Filename.LastIndexOf("\\") + 1) + path;

                PopulateListView("Path = '" + path + "'");

                SelectTreeNode(path);
            }
        }

        private void toolStripButtonExtract_Click(object sender, EventArgs e)
        {
            if ((this.listView.SelectedIndices.Count == 1) && ((String)this.m_DataViewListView[this.listView.SelectedIndices[0]]["File_Type"] != "File Folder"))
            {
                if ((String)this.m_DataViewListView[this.listView.SelectedIndices[0]]["File_Type"] != "File Folder")
                {
                    DataRow dataRow = this.m_DataViewListView[this.listView.SelectedIndices[0]].Row;
                    String Name = (String)dataRow["Name"];
                    Byte[] buffer = Utilities.InflateFile(dataRow);
                    this.m_FormNotifyIcon.saveFileDialog.FileName = Name.Substring(Name.LastIndexOf("/") + 1);
                    this.m_FormNotifyIcon.saveFileDialog.Filter = Name.Substring(Name.LastIndexOf(".") + 1).ToUpper() + " Files|*." + Name.Substring(Name.LastIndexOf(".") + 1).ToLower();
                    if (this.m_FormNotifyIcon.saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            FileStream fileStream = new FileStream(this.m_FormNotifyIcon.saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
                            fileStream.Write(buffer, 0, buffer.Length);
                            fileStream.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                if (this.m_FormNotifyIcon.folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    this.treeView.Enabled = false;
                    this.listView.Enabled = false;
                    this.toolStripButtonBack.Enabled = false;
                    this.toolStripButtonExport.Enabled = false;
                    this.toolStripSplitButtonOpen.Enabled = false;
                    this.toolStripButtonSearch.Enabled = false;
                    this.toolStripSplitButtonLayout.Enabled = false;
                    this.toolStripSplitButtonViews.Enabled = false;
                    this.toolStripTextBoxSearch.Enabled = false;
                    this.toolStripButtonAbout.Enabled = false;
                    this.toolStripButtonFilter.Enabled = false;
                    this.toolStripButtonExportFileChain.Enabled = false;

                    ExtractData passedData = new ExtractData();
                    passedData.Path = this.m_FormNotifyIcon.folderBrowserDialog.SelectedPath;
                    passedData.DataTable = this.RetrieveSelection();

                    this.m_FormProgress.Show();

                    m_Thread = new Thread(new ParameterizedThreadStart(this.ExtractThread));
                    m_Thread.Name = "Extract";
                    m_Thread.Start(passedData);
                }
            }
        }

        private void toolStripButtonFilter_Click(object sender, EventArgs e)
        {
            if (this.Text.Contains("/"))
            {
                if (this.m_FormFilter.ShowDialog() == DialogResult.OK)
                {
                    if ((this.m_FormFilter.comboBoxContainer.SelectedIndex == 0) && (this.m_FormFilter.comboBoxFileType.SelectedIndex == 0))
                    {
                        this.m_StaticFilter = String.Empty;
                    }
                    else if ((this.m_FormFilter.comboBoxContainer.SelectedIndex != 0) && (this.m_FormFilter.comboBoxFileType.SelectedIndex == 0))
                    {
                        this.m_StaticFilter = "Filename LIKE '%\\" + this.m_FormFilter.comboBoxContainer.Text + "'";
                    }
                    else if ((this.m_FormFilter.comboBoxContainer.SelectedIndex == 0) && (this.m_FormFilter.comboBoxFileType.SelectedIndex != 0))
                    {
                        this.m_StaticFilter = "File_Type = '" + this.m_FormFilter.comboBoxFileType.Text + "'";
                    }
                    else
                    {
                        this.m_StaticFilter = "(Filename LIKE '%\\" + this.m_FormFilter.comboBoxContainer.Text + "' AND File_Type = '" + this.m_FormFilter.comboBoxFileType.Text + "')";
                    }
                    String filter = String.Empty;
                    if ((!((Boolean)this.toolStripButtonSearch.Tag)) && (this.toolStripTextBoxSearch.Text != String.Empty))
                    {
                        filter = "Path LIKE '" + this.treeView.SelectedNode.Tag.ToString() + "%' AND (" + TextToFilter(this.toolStripTextBoxSearch.Text) + ")";
                    }
                    else
                    {
                        filter = "Path LIKE '" + this.treeView.SelectedNode.Tag.ToString() + "'";
                    }
                    PopulateListView(filter);
                }
            }
            else
            {
                MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            if (this.m_TREFile != null)
            {
                if ((Boolean)this.toolStripButtonSearch.Tag)
                {
                    String filter = "Path LIKE '/" + this.treeView.SelectedNode.Tag.ToString().Trim(new Char[] { '/' }) + "%' AND (" + TextToFilter(this.toolStripTextBoxSearch.Text) + ")";

                    this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_TREFile.Filename.Substring(this.m_TREFile.Filename.LastIndexOf("\\") + 1) + this.treeView.SelectedNode.FullPath.Trim(new Char[] { '/' }).Replace("\\", "/") + "/*" + this.toolStripTextBoxSearch.Text + "*";

                    PopulateListView(filter);

                    this.toolStripButtonSearch.Image = TRE_Explorer.Properties.Resources.Cancel;
                    this.toolStripButtonSearch.Tag = false;
                }
                else
                {
                    this.toolStripButtonBack_Click(this.toolStripButtonBack, new EventArgs());

                    this.toolStripButtonSearch.Image = TRE_Explorer.Properties.Resources.Find;
                    this.toolStripButtonSearch.Tag = true;
                }
            }
            else
            {
                MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemDetails_Click(object sender, EventArgs e)
        {
            this.listView.View = View.Details;
            CheckView();
        }

        private void toolStripMenuItemDetailsPane_CheckedChanged(object sender, EventArgs e)
        {
            this.splitContainerDetailsContent.Panel2Collapsed = !this.toolStripMenuItemDetailsPane.Checked;
            this.detailsPaneToolStripMenuItem.Checked = this.toolStripMenuItemDetailsPane.Checked;
        }

        private void toolStripMenuItemLargeIcons_Click(object sender, EventArgs e)
        {
            this.listView.View = View.LargeIcon;
            CheckView();
        }

        private void toolStripMenuItemList_Click(object sender, EventArgs e)
        {
            this.listView.View = View.List;
            CheckView();
        }

        private void toolStripMenuItemNavigationPane_CheckedChanged(object sender, EventArgs e)
        {
            this.splitContainerFolderTreeContent.Panel1Collapsed = !this.toolStripMenuItemNavigationPane.Checked;
            this.navigationPaneToolStripMenuItem.Checked = this.toolStripMenuItemNavigationPane.Checked;
        }

        private void toolStripMenuItemOpenIFF_Click(object sender, EventArgs e)
        {
            this.toolStripTopMenuItemOpenIFF.PerformClick();
        }

        private void toolStripMenuItemOpenPAL_Click(object sender, EventArgs e)
        {
            this.toolStripTopMenuItemOpenPAL.PerformClick();
        }

        private void toolStripMenuItemOpenSTF_Click(object sender, EventArgs e)
        {
            this.toolStripTopMenuItemOpenSTF.PerformClick();
        }

        private void toolStripMenuItemOpenTRETOC_Click(object sender, EventArgs e)
        {
            this.toolStripTopMenuItemOpenTRETOC.PerformClick();
        }

        private void toolStripMenuItemPreviewPane_CheckedChanged(object sender, EventArgs e)
        {
            this.splitContainerFilesPreview.Panel2Collapsed = !this.toolStripMenuItemPreviewPane.Checked;
            this.previewPaneToolStripMenuItem.Checked = this.toolStripMenuItemPreviewPane.Checked;
        }

        private void toolStripMenuItemSmallIcons_Click(object sender, EventArgs e)
        {
            this.listView.View = View.SmallIcon;
            CheckView();
        }

        private void toolStripSplitButtonLayout_ButtonClick(object sender, EventArgs e)
        {
            if (this.toolStripSplitButtonLayout.DropDown.Visible)
            {
                this.toolStripSplitButtonLayout.HideDropDown();
            }
            else
            {
                this.toolStripSplitButtonLayout.ShowDropDown();
            }
        }

        private void toolStripSplitButtonOpen_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.Open();
        }

        private void toolStripSplitButtonViews_ButtonClick(object sender, EventArgs e)
        {
            switch (this.listView.View)
            {
                case View.Details:
                    this.listView.View = View.LargeIcon;
                    break;

                case View.LargeIcon:
                    this.listView.View = View.List;
                    break;

                case View.List:
                    this.listView.View = View.SmallIcon;
                    break;

                case View.SmallIcon:
                    this.listView.View = View.Details;
                    break;
            }
            CheckView();
        }

        private void toolStripTextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.toolStripButtonSearch.PerformClick();
                e.Handled = true;
            }
        }

        private void toolStripTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (this.toolStripTextBoxSearch.Text.Length > 0)
            {
                this.toolStripButtonSearch.Enabled = true;
                this.toolStripButtonSearch.Image = TRE_Explorer.Properties.Resources.Find;
                this.toolStripButtonSearch.Tag = true;
            }
            else
            {
                this.toolStripButtonSearch.Enabled = false;
                if (!(Boolean)this.toolStripButtonSearch.Tag)
                {
                    this.toolStripButtonBack_Click(this.toolStripButtonBack, new EventArgs());
                }
            }

        }

        private void toolStripButtonBack_EnabledChanged(object sender, EventArgs e)
        {
            this.backToolStripMenuItem.Enabled = this.toolStripButtonBack.Enabled;
        }

        private void toolStripButtonExport_EnabledChanged(object sender, EventArgs e)
        {
            this.exportToolStripMenuItem.Enabled = this.toolStripButtonExport.Enabled;
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            this.settingsToolStripMenuItem.PerformClick();
        }

        private void toolStripButtonExportFileChain_Click(object sender, EventArgs e)
        {
            if (this.m_FormNotifyIcon.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.treeView.Enabled = false;
                this.listView.Enabled = false;
                this.toolStripButtonBack.Enabled = false;
                this.toolStripButtonExport.Enabled = false;
                this.toolStripSplitButtonOpen.Enabled = false;
                this.toolStripButtonSearch.Enabled = false;
                this.toolStripSplitButtonLayout.Enabled = false;
                this.toolStripSplitButtonViews.Enabled = false;
                this.toolStripTextBoxSearch.Enabled = false;
                this.toolStripButtonAbout.Enabled = false;
                this.toolStripButtonFilter.Enabled = false;
                this.toolStripButtonExportFileChain.Enabled = false;

                this.m_FormProgress.Show();

                ExtractChainData extractChainData = new ExtractChainData();
                extractChainData.DataRow = this.m_DataViewListView[this.listView.SelectedIndices[0]].Row;
                extractChainData.Path = this.m_FormNotifyIcon.folderBrowserDialog.SelectedPath;

                m_Thread = new Thread(new ParameterizedThreadStart(this.ExtractChainThread));
                m_Thread.Name = "ExtractChain";
                m_Thread.Start(extractChainData);
            }
        }

        private void toolStripButtonExportFileChain_EnabledChanged(object sender, EventArgs e)
        {
            this.exportChainToolStripMenuItem.Enabled = ((ToolStripButton)sender).Enabled;
        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            if ((this.m_Thread != null) && (this.m_Thread.IsAlive))
            {
                this.StopThread();
            }

            this.Visible = false;
        }

        private void helpTopicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = Path.Combine(Path.GetTempPath(), "TRE Explorer.chm");
            if (!File.Exists(filename))
            {
                FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                binaryWriter.Write(TRE_Explorer.Properties.Resources.TRE_Explorer_chm);
                binaryWriter.Close();
                fileStream.Close();

                this.m_FormNotifyIcon.m_TemporaryFiles.Add(filename);
            }

            Help.ShowHelp(this, filename, HelpNavigator.TableOfContents);
        }

        private void toolStripMenuItemIFFEditor_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.m_FormIFFILFWSEditor.Visible = !this.m_FormNotifyIcon.m_FormIFFILFWSEditor.Visible;
        }

        private void toolStripMenuItemPALEditor_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.m_FormPALEditor.Visible = !this.m_FormNotifyIcon.m_FormPALEditor.Visible;
        }

        private void toolStripMenuItemSTFEditor_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.m_FormSTFEditor.Visible = !this.m_FormNotifyIcon.m_FormSTFEditor.Visible;
        }

        private void toolStripMenuItemTOCTREViewer_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.m_FormNotifyIcon.m_Exitting)
            {
                this.m_FormNotifyIcon.m_Exitting = true;
                Application.Exit();
            }
        }
        #endregion

        #region MenuStripMain Functions
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.m_Thread != null)
            {
                this.StopThread();
            }

            this.Visible = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((this.m_FormDDSTGARenderFrame != null) && (this.m_FormDDSTGARenderFrame.Visible))
            {
                this.m_FormDDSTGARenderFrame.toolStripButtonSaveAs_Click(this.m_FormDDSTGARenderFrame.toolStripButtonSaveAs, new EventArgs());
            }
        }

        private void detailsPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItemDetailsPane.Checked = !this.toolStripMenuItemDetailsPane.Checked;
        }

        private void previewPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItemPreviewPane.Checked = !this.toolStripMenuItemPreviewPane.Checked;
        }

        private void navigationPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItemNavigationPane.Checked = !this.toolStripMenuItemNavigationPane.Checked;
        }

        private void iFFILFWSEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.ToggleIFFILFWSEditor();
        }

        private void pALEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.TogglePALEditor();
        }

        private void sTFEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.ToggleSTFEditor();
        }

        private void tOCTREViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.ToggleTOCTREViewer();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((this.m_DataViewListView != null) && (this.m_DataViewListView.Count > 0))
            {
                this.listView.SuspendLayout();
                this.listView.SelectedIndexChanged -= new EventHandler(listView_SelectedIndexChanged);
                for (Int32 index = 0; index < this.m_DataViewListView.Count; index++)
                {
                    this.listView.SelectedIndices.Add(index);
                }
                this.listView.ResumeLayout(false);
                this.listView.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
                this.listView_SelectedIndexChanged(this.listView, new EventArgs());
                this.listView.Focus();
            }
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView.SelectedIndexChanged -= new EventHandler(listView_SelectedIndexChanged);
            this.listView.SuspendLayout();
            this.listView.SelectedIndices.Clear();
            this.listView.ResumeLayout(false);
            this.listView.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
            this.listView_SelectedIndexChanged(this.listView, new EventArgs());
            this.listView.Focus();
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView.Visible)
            {
                if ((this.m_DataViewListView != null) && (this.m_DataViewListView.Count > 0))
                {
                    this.listView.SelectedIndexChanged -= new EventHandler(listView_SelectedIndexChanged);
                    this.listView.SuspendLayout();
                    for (Int32 index = 0; index < this.m_DataViewListView.Count; index++)
                    {
                        if (this.listView.SelectedIndices.Contains(index))
                        {
                            this.listView.SelectedIndices.Remove(index);
                        }
                        else
                        {
                            this.listView.SelectedIndices.Add(index);
                        }
                    }
                    this.listView.ResumeLayout(false);
                    this.listView.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
                    this.listView_SelectedIndexChanged(this.listView, new EventArgs());
                    this.listView.Focus();
                }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.Settings(this);
        }

        private void exportChainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripButtonExportFileChain.PerformClick();
        }

        private void toolStripTopMenuItemOpen_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.Open();
        }

        private void toolStripTopMenuItemOpenIFF_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.OpenIFFILFWS();
        }

        private void toolStripTopMenuItemOpenPAL_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.OpenPAL();
        }

        private void toolStripTopMenuItemOpenSTF_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.OpenSTF();
        }

        private void toolStripTopMenuItemOpenTRETOC_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.OpenTOCTRE();
        }
        #endregion

        #region ContextMenuStripTreeView Functions
        private void toolStripMenuItemExportFolderTreeView_Click(object sender, EventArgs e)
        {
            if (this.Text.Contains("/"))
            {
                if (this.m_FormNotifyIcon.folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    this.treeView.Enabled = false;
                    this.listView.Enabled = false;
                    this.toolStripButtonBack.Enabled = false;
                    this.toolStripButtonExport.Enabled = false;
                    this.toolStripSplitButtonOpen.Enabled = false;
                    this.toolStripButtonSearch.Enabled = false;
                    this.toolStripSplitButtonLayout.Enabled = false;
                    this.toolStripSplitButtonViews.Enabled = false;
                    this.toolStripTextBoxSearch.Enabled = false;
                    this.toolStripButtonAbout.Enabled = false;
                    this.toolStripButtonFilter.Enabled = false;

                    DataView tempDataView = new DataView(this.m_TREFile.DataTable, "(path LIKE '/" + m_LastClickedTreeNode.Tag.ToString().Trim(new Char[] { '/' }) + "/%') AND (File_Type NOT LIKE 'File Folder')", String.Empty, DataViewRowState.CurrentRows);
                    DataTable tempDataTable = this.m_DataViewListView.ToTable();
                    tempDataTable.Rows.Clear();
                    foreach (DataRowView dataRowView in tempDataView)
                    {
                        tempDataTable.ImportRow(dataRowView.Row);
                    }

                    ExtractData passedData = new ExtractData();
                    passedData.Path = this.m_FormNotifyIcon.folderBrowserDialog.SelectedPath;
                    passedData.DataTable = tempDataTable;

                    this.m_FormProgress.Show();

                    m_Thread = new Thread(new ParameterizedThreadStart(this.ExtractThread));
                    m_Thread.Start(passedData);
                }
            }
            else
            {
                MessageBox.Show("You must first load a file.", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemOpenTreeView_Click(object sender, EventArgs e)
        {
            this.treeView.SelectedNode = m_LastClickedTreeNode;
        }
        #endregion

        #region ContextMenuStripListView Functions
        private void contextMenuStripListView_Opening(object sender, CancelEventArgs e)
        {
            this.toolStripMenuItemCopy.Visible = false;
            this.toolStripSeparator7.Visible = false;
            this.toolStripMenuItemExportFolderListView.Visible = false;
            this.toolStripMenuItemOpenListView.Visible = false;
            this.toolStripMenuItemExportListView.Visible = false;
            this.toolStripMenuItemExportCurrentFolderListView.Visible = false;
            this.toolStripMenuItemViewListView.Visible = false;
            this.toolStripMenuItemSortByListView.Visible = false;
            this.toolStripMenuItemExportMultipleListView.Visible = false;
            this.toolStripMenuItemExportFileChain.Visible = false;

            if (this.listView.SelectedIndices.Count > 1)
            {
                this.toolStripMenuItemExportMultipleListView.Visible = true;
                this.toolStripSeparator7.Visible = this.webBrowserDetails.Focused;
            }
            else if (this.listView.SelectedIndices.Count == 1)
            {
                if ((this.m_IFFFilePreview != null) && (!this.m_IFFFilePreview.IsDataTable))
                {
                    this.toolStripMenuItemExportFileChain.Visible = true;
                    this.toolStripSeparator7.Visible = this.webBrowserDetails.Focused;
                }

                if (this.m_DataViewListView[this.listView.SelectedIndices[0]]["file_type"].ToString() == "File Folder")
                {
                    this.toolStripMenuItemExportFolderListView.Visible = true;
                    this.toolStripMenuItemOpenListView.Visible = true;
                    this.toolStripSeparator7.Visible = this.webBrowserDetails.Focused;
                }
                else
                {
                    this.toolStripMenuItemExportListView.Visible = true;
                    this.toolStripSeparator7.Visible = this.webBrowserDetails.Focused;
                }
            }
            else if (!this.webBrowserDetails.Focused)
            {
                this.toolStripMenuItemExportCurrentFolderListView.Visible = true;
                this.toolStripMenuItemSortByListView.Visible = true;
                this.toolStripMenuItemViewListView.Visible = true;
                this.toolStripSeparator7.Visible = false;
            }

            if (this.webBrowserDetails.Focused)
            {
                this.toolStripMenuItemCopy.Visible = true;
                this.toolStripMenuItemCopy.Enabled = (this.WebBrowserSelectedText() != String.Empty);
            }

            this.contextMenuStripListView.Invalidate();
        }

        private void toolStripMenuItemExportFolderListView_Click(object sender, EventArgs e)
        {
            if (this.m_FormNotifyIcon.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.treeView.Enabled = false;
                this.listView.Enabled = false;
                this.toolStripButtonBack.Enabled = false;
                this.toolStripButtonExport.Enabled = false;
                this.toolStripSplitButtonOpen.Enabled = false;
                this.toolStripButtonSearch.Enabled = false;
                this.toolStripSplitButtonLayout.Enabled = false;
                this.toolStripSplitButtonViews.Enabled = false;
                this.toolStripTextBoxSearch.Enabled = false;
                this.toolStripButtonAbout.Enabled = false;
                this.toolStripButtonFilter.Enabled = false;

                DataView tempDataView = new DataView(this.m_TREFile.DataTable, "(path LIKE '/" + this.m_DataViewListView[this.listView.SelectedIndices[0]]["name"].ToString().TrimStart(new Char[] { '/' }) + "%') AND (File_Type NOT LIKE 'File Folder')", String.Empty, DataViewRowState.CurrentRows);
                DataTable tempDataTable = this.m_DataViewListView.ToTable();
                tempDataTable.Rows.Clear();
                foreach (DataRowView dataRowView in tempDataView)
                {
                    tempDataTable.ImportRow(dataRowView.Row);
                }

                ExtractData passedData = new ExtractData();
                passedData.Path = this.m_FormNotifyIcon.folderBrowserDialog.SelectedPath;
                passedData.DataTable = tempDataTable;

                this.m_FormProgress.Show();

                m_Thread = new Thread(new ParameterizedThreadStart(this.ExtractThread));
                m_Thread.Start(passedData);
            }
        }

        private void toolStripMenuItemOpenListView_Click(object sender, EventArgs e)
        {
            this.listView_ItemActivate(sender, e);
        }

        private void toolStripMenuItemExportListView_Click(object sender, EventArgs e)
        {
            this.toolStripButtonExtract_Click(sender, e);
        }

        private void toolStripMenuItemViewListView_Click(object sender, EventArgs e)
        {
            this.toolStripSplitButtonViews_ButtonClick(sender, e);
        }

        private void toolStripMenuItemExportCurrentFolderListView_Click(object sender, EventArgs e)
        {
            this.m_LastClickedTreeNode = this.treeView.SelectedNode;
            this.toolStripMenuItemExportFolderTreeView_Click(sender, e);
        }

        private void toolStripMenuItemLargeIconsListView_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItemLargeIcons_Click(sender, e);
        }

        private void toolStripMenuItemSmallIconsListView_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItemSmallIcons_Click(sender, e);
        }

        private void toolStripMenuItemListListView_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItemList_Click(sender, e);
        }

        private void toolStripMenuItemDetailsListView_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItemDetails_Click(sender, e);
        }

        private void toolStripMenuItemExportMultipleListView_Click(object sender, EventArgs e)
        {
            this.toolStripButtonExtract_Click(sender, e);
        }

        private void toolStripMenuItemNameListView_Click(object sender, EventArgs e)
        {
            SortListView("SortName");
            CheckSortBy();
        }

        private void toolStripMenuItemTypeListView_Click(object sender, EventArgs e)
        {
            SortListView("file_type");
            CheckSortBy();
        }

        private void toolStripMenuItemSizeListView_Click(object sender, EventArgs e)
        {
            SortListView("final_size");
            CheckSortBy();
        }

        private void toolStripMenuItemAscendingListView_Click(object sender, EventArgs e)
        {
            SortListView(SortOrder.Ascending);
            CheckSortBy();
        }

        private void toolStripMenuItemDescendingListView_Click(object sender, EventArgs e)
        {
            SortListView(SortOrder.Descending);
            CheckSortBy();
        }

        private void exportFileChainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripButtonExportFileChain.PerformClick();
        }
        #endregion

        #region TreeView Functions
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.Text = "TRE Explorer " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " - " + this.m_TREFile.Filename.Substring(this.m_TREFile.Filename.LastIndexOf("\\") + 1) + this.treeView.SelectedNode.Tag;
            PopulateListView("Path = '" + this.treeView.SelectedNode.Tag + "'");
        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_LastClickedTreeNode = this.treeView.GetNodeAt(e.X, e.Y);
            }
        }
        #endregion

        #region Button Functions
        private void buttonPlaySound_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = this.m_DataViewListView[this.listView.SelectedIndices[0]].Row;
                String Name = (String)dataRow["Name"];
                Byte[] buffer = Utilities.InflateFile(dataRow);
                if (Name.ToLower().EndsWith(".wav"))
                {
                    try
                    {
                        m_SoundPlayer.Stop();
                    }
                    catch
                    {
                    }
                    try
                    {
                        m_MP3Player.Stop();
                        m_MP3Player.Close();
                    }
                    catch
                    {
                    }

                    MemoryStream memoryStream = new MemoryStream(buffer);
                    m_SoundPlayer = new SoundPlayer(memoryStream);
                    m_SoundPlayer.Play();
                }
                else if (Name.ToLower().EndsWith(".mp3"))
                {
                    try
                    {
                        m_SoundPlayer.Stop();
                    }
                    catch
                    {
                    }
                    try
                    {
                        m_MP3Player.Stop();
                        m_MP3Player.Close();
                    }
                    catch
                    {
                    }

                    String path = Path.Combine(Path.GetTempPath(), Name.Replace("/", "\\").Substring(0, Name.LastIndexOf("/")));
                    String file = Name.Substring(Name.LastIndexOf("/") + 1);
                    String filename = Path.Combine(path, file);

                    if (!File.Exists(filename))
                    {
                        try
                        {
                            Directory.CreateDirectory(path);
                        }
                        catch
                        {
                        }

                        FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                        fileStream.Write(buffer, 0, buffer.Length);
                        fileStream.Close();
                        this.m_FormNotifyIcon.m_TemporaryFiles.Add(filename);
                        this.m_FormNotifyIcon.m_TemporaryPaths.Add(path);
                    }

                    m_MP3Player.Open(filename);
                    m_MP3Player.Play();
                }
            }
            catch
            {
            }
        }

        private void buttonPlaySound_Move(object sender, EventArgs e)
        {
            this.buttonStopSound.Left = this.buttonPlaySound.Left + this.buttonPlaySound.Width + this.buttonPlaySound.Margin.Right + this.buttonStopSound.Margin.Left;
        }

        private void buttonPlaySound_VisibleChanged(object sender, EventArgs e)
        {
            this.buttonStopSound.Visible = this.buttonPlaySound.Visible;
        }

        private void buttonStopSound_Click(object sender, EventArgs e)
        {
            try
            {
                m_SoundPlayer.Stop();
            }
            catch
            {
            }
            try
            {
                m_MP3Player.Stop();
                m_MP3Player.Close();
            }
            catch
            {
            }
        }
        #endregion

        #region PictureBoxDetails Functions
        private void pictureBoxDetails_Resize(object sender, EventArgs e)
        {
            this.pictureBoxDetails.Width = this.pictureBoxDetails.Height;
            this.webBrowserDetails.Left = this.pictureBoxDetails.Left + this.pictureBoxDetails.Width + 6;
            this.webBrowserDetails.Width = this.panelDetailsBackground.ClientSize.Width - (this.webBrowserDetails.Left + 2);
        }
        #endregion

        #region ToolStripSTFPreview Functions
        private void toolStripButtonStfEditFile_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.m_FormSTFEditor.m_STFFile = this.m_STFFilePreview;
            this.m_FormNotifyIcon.m_FormSTFEditor.STFDisplay();
            this.m_FormNotifyIcon.m_FormSTFEditor.Visible = true;
        }
        #endregion

        #region ToolStripIFFPreview Functions
        private void toolStripButtonIffEditFile_Click(object sender, EventArgs e)
        {
            DataRow dataRow = this.m_DataViewListView[this.listView.SelectedIndices[0]].Row;
            String Name = (String)dataRow["Name"];

            String path = Path.Combine(Path.GetTempPath(), Name.Replace("/", "\\").Substring(0, Name.LastIndexOf("/")));
            String file = Name.Substring(Name.LastIndexOf("/") + 1);
            String filename = Path.Combine(path, file);

            if (!File.Exists(filename))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                }

                Byte[] buffer = Utilities.InflateFile(dataRow);
                FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Close();
                this.m_FormNotifyIcon.m_TemporaryFiles.Add(filename);
                this.m_FormNotifyIcon.m_TemporaryPaths.Add(path);
            }

            this.m_FormNotifyIcon.IFFILFWSLoadFile(filename);
        }
        #endregion

        #region FlowLayoutPanelPALPreview Functions
        private void flowLayoutPanelPalPreview_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.flowLayoutPanelPalPreview.Visible)
            {
                this.flowLayoutPanelPalPreview.SuspendLayout();
                foreach (Control control in this.flowLayoutPanelPalPreview.Controls)
                {
                    control.Dispose();
                }
                this.flowLayoutPanelPalPreview.Controls.Clear();
                this.flowLayoutPanelPalPreview.ResumeLayout(false);
            }
            this.toolStripPalPreview.Visible = this.flowLayoutPanelPalPreview.Visible;
        }
        #endregion

        #region ToolStripPALPreview Functions
        private void toolStripButtonPalEditFile_Click(object sender, EventArgs e)
        {
            this.m_FormNotifyIcon.m_FormPALEditor.m_PALFile = this.m_PALFilePreview;
            this.m_FormNotifyIcon.m_FormPALEditor.PALDisplay();
            this.m_FormNotifyIcon.m_FormPALEditor.Visible = true;
        }
        #endregion

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripTextBoxSearch.Focus();
            if (this.toolStripTextBoxSearch.Text.Length > 0)
            {
                this.toolStripTextBoxSearch.Select(0, this.toolStripTextBoxSearch.Text.Length);
            }
        }

        private void webBrowserDetails_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (((e.KeyCode & Keys.C) == Keys.C) && (e.Control))
            {
                String selectedText = WebBrowserSelectedText();
                if (selectedText != String.Empty)
                {
                    Clipboard.SetText(selectedText);
                }
                else
                {
                    Clipboard.Clear();
                }
            }
        }

        private String WebBrowserSelectedText()
        {
            Boolean hasText = Clipboard.ContainsText();
            String clipboardText = String.Empty;
            if (hasText)
            {
                clipboardText = Clipboard.GetText();
            }

            Clipboard.Clear();
            this.webBrowserDetails.Document.ExecCommand("Copy", false, null);
            String selectedText = String.Empty;
            if (Clipboard.ContainsText())
            {
                selectedText = Clipboard.GetText();
            }

            if (hasText)
            {
                Clipboard.SetText(clipboardText);
            }
            else
            {
                Clipboard.Clear();
            }

            return selectedText;
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            String selectedText = WebBrowserSelectedText();
            if (selectedText != String.Empty)
            {
                Clipboard.SetText(selectedText);
            }
            else
            {
                Clipboard.Clear();
            }
        }

        private void FormTOCTREViewer_Shown(object sender, EventArgs e)
        {
            if ((this != null) && (this.Visible) && (this.WindowState != FormWindowState.Minimized))
            {
                if (this.m_FormProgress != null)
                {
                    this.m_FormProgress.Top = this.Top + (Int32)(((Single)this.Height - (Single)m_FormProgress.Height) / 2F);
                    this.m_FormProgress.Left = this.Left + (Int32)(((Single)this.Width - (Single)m_FormProgress.Width) / 2F);
                }
                if (this.m_FormFilter != null)
                {
                    this.m_FormFilter.Top = this.Top + (Int32)(((Single)this.Height - (Single)this.m_FormFilter.Height) / 2F);
                    this.m_FormFilter.Left = this.Left + (Int32)(((Single)this.Width - (Single)this.m_FormFilter.Width) / 2F);
                }
            }
        }
    }

    public class DataSetHelper
    {
        #region DataSetHelper Class
        private DataSet dataSet;

        public DataSetHelper(ref DataSet DataSet)
        {
            dataSet = DataSet;
        }

        private bool ColumnEqual(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value)
                return true;
            if (A == DBNull.Value || B == DBNull.Value)
                return false;
            return (A.Equals(B));
        }

        public DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
        {
            DataTable dt = new DataTable(TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            if (dataSet != null)
                dataSet.Tables.Add(dt);
            return dt;
        }
        #endregion
    }

    public class LoadProcess
    {
        #region LoadProcess Class
        ManualResetEvent m_EventStop;
        ManualResetEvent m_EventStopped;
        FormTOCTREViewer m_form;
        String m_filename;
        TREFile m_TREFile;
        Boolean m_Cancelled = false;

        public LoadProcess(ManualResetEvent eventStop, ManualResetEvent eventStopped, FormTOCTREViewer form, String filename)
        {
            m_EventStop = eventStop;
            m_EventStopped = eventStopped;
            this.m_form = form;
            m_filename = filename;
        }

        private TreeNode CreateTreeNodes(DataTable dataTable)
        {
            TreeNode returnValue = new TreeNode();
            returnValue.Name = "/";
            returnValue.Text = "/";
            returnValue.Tag = "/";
            returnValue.ContextMenuStrip = m_form.contextMenuStripTreeView;

            DataSet dataSet = new DataSet();
            DataSetHelper dataSetHelper = new DataSetHelper(ref dataSet);
            DataTable dataTablePaths = dataSetHelper.SelectDistinct("Paths", dataTable, "path");

            for (Int32 counter = 0; counter < dataTablePaths.Rows.Count; counter++)
            {
                DataRow dataRow = dataTablePaths.Rows[counter];
                TreeNode treeNode = returnValue;
                String fullpath = "/";
                foreach (String folder in dataRow[0].ToString().Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    fullpath += folder + "/";
                    if (!treeNode.Nodes.ContainsKey(folder))
                    {
                        treeNode = treeNode.Nodes.Add(folder, folder);
                        treeNode.ContextMenuStrip = m_form.contextMenuStripTreeView;
                        treeNode.Tag = fullpath;
                    }
                    else
                    {
                        treeNode = treeNode.Nodes[folder];
                    }
                }

                if (m_EventStop.WaitOne(0, true))
                {
                    this.m_Cancelled = true;
                    m_EventStopped.Set();

                    return null;
                }
            }

            return returnValue;
        }

        private String[][] CreateFilterLists(DataTable dataTable)
        {
            DataSet dataSet = new DataSet();
            DataSetHelper dataSetHelper = new DataSetHelper(ref dataSet);
            DataTable dataTableContainers = dataSetHelper.SelectDistinct("Containers", dataTable, "filename");
            DataTable dataTableFileTypes = dataSetHelper.SelectDistinct("FileTypes", dataTable, "file_type");

            List<String[]> returnValue = new List<String[]>();
            List<String> listString = new List<String>();

            for (Int32 counter = 0; counter < dataTableContainers.Rows.Count; counter++)
            {
                DataRow dataRow = dataTableContainers.Rows[counter];
                listString.Add(dataRow[0].ToString().Substring(dataRow[0].ToString().LastIndexOf("\\") + 1));

                if (m_EventStop.WaitOne(0, true))
                {
                    m_EventStopped.Set();

                    return null;
                }
            }
            returnValue.Add(listString.ToArray());

            listString.Clear();
            for (Int32 counter = 0; counter < dataTableFileTypes.Rows.Count; counter++)
            {
                DataRow dataRow = dataTableFileTypes.Rows[counter];
                listString.Add(dataRow[0].ToString());

                if (m_EventStop.WaitOne(0, true))
                {
                    this.m_Cancelled = true;
                    m_EventStopped.Set();

                    return null;
                }
            }
            returnValue.Add(listString.ToArray());
            listString.Clear();

            return returnValue.ToArray();
        }

        private void AddFolders(TreeNode treeBranch)
        {
            if (treeBranch.Nodes.Count > 0)
            {
                foreach (TreeNode treeNode in treeBranch.Nodes)
                {
                    String container = treeNode.Tag.ToString().Trim(new Char[] { '/' });
                    if (container.Contains("/"))
                    {
                        container = "/" + container.Substring(0, container.LastIndexOf("/")) + "/";
                    }
                    else
                    {
                        container = "/";
                    }
                    this.m_TREFile.DataTable.Rows.Add(new Object[] { treeNode.Tag.ToString(), 0, 0, 0, 0, 0, 0, String.Empty, container, "File Folder", null, " " + treeNode.Text });
                    if (treeNode.Nodes.Count > 0)
                    {
                        AddFolders(treeNode);
                    }

                    if (this.m_EventStop.WaitOne(0, true))
                    {
                        this.m_Cancelled = true;
                        this.m_EventStopped.Set();

                        return;
                    }
                }
            }
        }

        public void Run()
        {
            LoadReturn loadReturn = new LoadReturn();
            loadReturn.TREFile = null;
            loadReturn.TreeNode = null;
            loadReturn.Containers = null;
            loadReturn.FileTypes = null;

            this.m_form.Invoke(this.m_form.delegateSetCaption, new Object[] { "Loading " + this.m_filename.Substring(this.m_filename.LastIndexOf("\\") + 1) });
            this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { -1 } });

            this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { "Reading " + this.m_filename.Substring(this.m_filename.LastIndexOf(".") + 1).ToUpper() + "…" });
            m_TREFile = new TREFile(this.m_filename);

            if (m_TREFile.DataTable.Rows.Count > 0)
            {
                if (!this.m_Cancelled)
                {
                    this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { "Creating Folder Tree…" });
                    TreeNode treeNode = CreateTreeNodes(m_TREFile.DataTable);

                    if (!this.m_Cancelled)
                    {
                        this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { "Generating Filter Lists…" });
                        String[][] filters = CreateFilterLists(m_TREFile.DataTable);
                        AddFolders(treeNode);

                        if (!this.m_Cancelled)
                        {
                            loadReturn.TREFile = this.m_TREFile;
                            loadReturn.TreeNode = treeNode;
                            loadReturn.Containers = filters[0];
                            loadReturn.FileTypes = filters[1];
                        }
                    }
                }
                this.m_form.Invoke(this.m_form.delegateLoadThreadFinished, new Object[] { loadReturn });
            }
            else
            {
                this.m_form.Invoke(this.m_form.delegateLoadThreadFinished, new Object[] { loadReturn });
            }
        }
        #endregion
    }

    public class ExtractProcess
    {
        #region ExtractProcess Class
        ManualResetEvent m_EventStop;
        ManualResetEvent m_EventStopped;
        FormTOCTREViewer m_form;
        DataTable m_DataTable;
        String m_ExtractPath;

        public ExtractProcess(ManualResetEvent EventStop, ManualResetEvent EventStopped, FormTOCTREViewer Form, DataTable DataTable, String ExtractPath)
        {
            this.m_EventStop = EventStop;
            this.m_EventStopped = EventStopped;
            this.m_form = Form;
            this.m_DataTable = DataTable;
            this.m_ExtractPath = ExtractPath;
        }

        private void WriteFile(DataRow DataRow)
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(m_ExtractPath, ((String)DataRow["Name"]).Substring(0, ((String)DataRow["Name"]).LastIndexOf("/")).Replace('/', '\\')));
                Byte[] buffer = Utilities.InflateFile(DataRow);
                FileStream fileStream = new FileStream(Path.Combine(m_ExtractPath, ((String)DataRow["Name"]).Replace('/', '\\')), FileMode.Create, FileAccess.Write, FileShare.None);
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + " (" + ((String)DataRow["Name"]).Substring(0, ((String)DataRow["Name"]).LastIndexOf("/")).Replace('/', '\\') + ")", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Run()
        {
            this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { 0 } });
            this.m_form.Invoke(this.m_form.delegateSetCaption, new Object[] { "Extracting - 0%" });
            this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { "Extracting…" });

            Int32 progress = 0;
            Int32 lastProgress = 0;
            for (Int32 counter = 0; counter < this.m_DataTable.Rows.Count; counter++)
            {
                WriteFile(this.m_DataTable.Rows[counter]);

                progress = (Int32)(((Double)counter / (Double)this.m_DataTable.Rows.Count) * 100);
                if (progress != lastProgress)
                {
                    this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { progress } });
                    this.m_form.Invoke(this.m_form.delegateSetCaption, new Object[] { "Extracting - " + progress + "%" });
                    lastProgress = progress;
                }

                if (m_EventStop.WaitOne(0, true))
                {
                    m_EventStopped.Set();

                    return;
                }
            }

            this.m_form.Invoke(this.m_form.delegateExtractThreadFinished);
        }
        #endregion
    }

    public class ExtractChainProcess
    {
        #region ExtractChainProcess Class
        ManualResetEvent m_EventStop;
        ManualResetEvent m_EventStopped;
        FormTOCTREViewer m_form;
        DataTable m_DataTable;
        DataRow m_DataRow;
        String m_ExtractPath;
        List<String> parsedFilenames = new List<String>();
        DataTable ExtractTable;
        List<Char> VALID_CHARACTERS = new List<Char>();
        List<String> totalFilenames = new List<String>();
        Int32 lastProgress = -1;
        String lastCaption = String.Empty;
        String lastLabel = String.Empty;
        Int32 parsedNodes = 0;
        Int32 totalNodes = 0;

        public ExtractChainProcess(ManualResetEvent EventStop, ManualResetEvent EventStopped, FormTOCTREViewer Form, DataTable DataTable, DataRow DataRow, String ExtractPath)
        {
            this.m_EventStop = EventStop;
            this.m_EventStopped = EventStopped;
            this.m_form = Form;
            this.m_DataTable = DataTable;
            this.m_DataRow = DataRow;
            this.m_ExtractPath = ExtractPath;
            this.parsedFilenames = new List<String>();
            this.ExtractTable = DataTable.Clone();
            this.ExtractTable.Rows.Clear();
            this.VALID_CHARACTERS.AddRange(new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', ',', '.', '/', '\\', '_', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' });
        }

        private String[] FollowChain(IFFFile.IFFNode[] iffNodes)
        {
            List<String> returnValue = new List<String>();
            Int32 lastFileProgress = -1;
            this.totalNodes += iffNodes.Length;

            foreach (IFFFile.IFFNode iffNode in iffNodes)
            {
                if (m_EventStop.WaitOne(0, true))
                {
                    m_EventStopped.Set();

                    return null;
                }

                if ((!iffNode.ID.Contains("FORM")) && (iffNode.Data != null) && (iffNode.Data.Length > 4))
                {
                    List<Byte> byteList = new List<Byte>(iffNode.Data);
                    if ((!byteList.Contains((Byte)'\\')) && (!byteList.Contains((Byte)'/')))
                    {
                        continue;
                    }

                    StringBuilder stringBuilder = new StringBuilder();
                    Int32 totalBytes = iffNode.Data.Length;
                    Int32 parsedBytes = 0;
                    foreach (Byte character in iffNode.Data)
                    {
                        if (m_EventStop.WaitOne(0, true))
                        {
                            m_EventStopped.Set();

                            return null;
                        }

                        parsedBytes++;

                        if (this.VALID_CHARACTERS.Contains((Char)character))
                        {
                            stringBuilder.Append((Char)character);
                        }

                        if ((!this.VALID_CHARACTERS.Contains((Char)character)) || (parsedBytes == totalBytes))
                        {
                            if ((stringBuilder.Length > 3) && ((stringBuilder.ToString().Contains("/")) || (stringBuilder.ToString().Contains("\\"))))
                            {
                                String filename = stringBuilder.ToString().Replace('\\', '/');
                                Boolean found = false;

                                while (filename.Length > 0)
                                {
                                    if (m_EventStop.WaitOne(0, true))
                                    {
                                        m_EventStopped.Set();

                                        return null;
                                    }

                                    DataRow[] dataRows = null;
                                    try
                                    {
                                        dataRows = this.m_DataTable.Select("Name = '" + filename + "'");
                                    }
                                    catch
                                    {
                                        dataRows = null;
                                    }

                                    if ((dataRows != null) && (dataRows.Length > 0))
                                    {
                                        found = true;
                                        if (!returnValue.Contains(filename))
                                        {
                                            returnValue.Add(filename);
                                        }
                                        if (!this.totalFilenames.Contains(filename))
                                        {
                                            this.totalFilenames.Add(filename);
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        filename = filename.Substring(1);
                                    }
                                }

                                if (!found)
                                {
                                    filename = stringBuilder.ToString().Replace('\\', '/');
                                    DataRow[] dataRows = null;
                                    try
                                    {
                                        dataRows = this.m_DataTable.Select("Name LIKE '%" + ((filename.StartsWith("/")) ? String.Empty : "/") + filename + "'");
                                    }
                                    catch
                                    {
                                        dataRows = null;
                                    }
                                    if ((dataRows != null) && (dataRows.Length > 0))
                                    {
                                        for (Int32 counter = 0; counter < dataRows.Length; counter++)
                                        {
                                            if ((String)dataRows[counter]["File_Type"] != "File Folder")
                                            {
                                                filename = (String)dataRows[counter]["Name"];
                                                if (!returnValue.Contains(filename))
                                                {
                                                    returnValue.Add(filename);
                                                }
                                                if (!this.totalFilenames.Contains(filename))
                                                {
                                                    this.totalFilenames.Add(filename);
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            stringBuilder = new StringBuilder();
                        }
                    }
                }
                else if (iffNode.Children.Count > 0)
                {
                    String[] tempFilenames = null;
                    try
                    {
                        tempFilenames = FollowChain(iffNode.Children.ToArray());
                    }
                    catch
                    {
                        tempFilenames = null;
                    }
                    if (tempFilenames != null)
                    {
                        foreach (String tempFilename in tempFilenames)
                        {
                            if (m_EventStop.WaitOne(0, true))
                            {
                                m_EventStopped.Set();

                                return null;
                            }

                            if (!returnValue.Contains(tempFilename))
                            {
                                returnValue.Add(tempFilename);
                            }
                            if (!this.totalFilenames.Contains(tempFilename))
                            {
                                this.totalFilenames.Add(tempFilename);
                            }
                        }
                    }
                }

                this.parsedNodes++;

                Int32 progress = (Int32)(((Single)this.parsedNodes / (Single)this.totalNodes) * 100F);
                if (progress != lastFileProgress)
                {
                    lastFileProgress = progress;
                    this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { this.lastProgress, progress } });
                }
            }

            return returnValue.ToArray();
        }

        private String[] FollowChain(String[] IFFFilenames)
        {
            List<String> returnValue = new List<String>();

            foreach (String IFFFilename in IFFFilenames)
            {
                if (m_EventStop.WaitOne(0, true))
                {
                    m_EventStopped.Set();

                    return null;
                }

                if ((IFFFilename != null) && (IFFFilename != String.Empty) && (!this.totalFilenames.Contains(IFFFilename)))
                {
                    this.totalFilenames.Add(IFFFilename);
                }

                if ((IFFFilename != null) && (IFFFilename != String.Empty) && (!this.parsedFilenames.Contains(IFFFilename)))
                {
                    this.parsedFilenames.Add(IFFFilename);

                    Int32 progress = (Int32)(((Single)this.parsedFilenames.Count / (Single)this.totalFilenames.Count) * 100F);
                    if (this.lastProgress != progress)
                    {
                        this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { progress, 0 } });
                        this.lastProgress = progress;
                    }

                    String caption = "Constructing File Chain - " + progress + "%";
                    if (this.lastCaption != caption)
                    {
                        this.m_form.Invoke(this.m_form.delegateSetCaption, new Object[] { caption });
                        this.lastCaption = caption;
                    }

                    String label = "Scanning " + IFFFilename + "…";
                    if (this.lastLabel != label)
                    {
                        this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { label });
                        this.lastLabel = label;
                    }

                    DataRow[] dataRows = null;
                    try
                    {
                        dataRows = this.m_DataTable.Select("Name = '" + IFFFilename + "'");
                    }
                    catch
                    {
                        continue;
                    }
                    if ((dataRows == null) || (dataRows.Length <= 0))
                    {
                        continue;
                    }

                    this.ExtractTable.Rows.Add(dataRows[0].ItemArray);

                    Byte[] buffer = Utilities.InflateFile(dataRows[0]);
                    if ((buffer.Length < 4) || (buffer[0] != (Byte)'F') || (buffer[1] != (Byte)'O') || (buffer[2] != (Byte)'R') || (buffer[3] != (Byte)'M'))
                    {
                        continue;
                    }

                    IFFFile iffFile = null;
                    try
                    {
                        iffFile = new IFFFile(buffer);
                    }
                    catch
                    {
                        continue;
                    }

                    if ((iffFile == null) || (iffFile.IsDataTable))
                    {
                        continue;
                    }

                    String[] filenames = null;
                    try
                    {
                        this.parsedNodes = 0;
                        this.totalNodes = 0;
                        filenames = FollowChain(new IFFFile.IFFNode[] { iffFile.Node });
                    }
                    catch
                    {
                        continue;
                    }

                    if (filenames != null)
                    {
                        foreach (String filename in filenames)
                        {
                            if (m_EventStop.WaitOne(0, true))
                            {
                                m_EventStopped.Set();

                                return null;
                            }

                            if (!returnValue.Contains(filename))
                            {
                                returnValue.Add(filename);
                            }
                        }
                    }
                }
            }

            if (returnValue.Count > 0)
            {
                String[] tempFilenames = FollowChain(returnValue.ToArray());
                if (tempFilenames != null)
                {
                    foreach (String tempFilename in tempFilenames)
                    {
                        if (m_EventStop.WaitOne(0, true))
                        {
                            m_EventStopped.Set();

                            return null;
                        }

                        if ((!this.parsedFilenames.Contains(tempFilename)) && (!returnValue.Contains(tempFilename)))
                        {
                            returnValue.Add(tempFilename);
                        }
                    }
                }

                return returnValue.ToArray();
            }
            else
            {
                return null;
            }
        }

        private void WriteFile(DataRow DataRow)
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(m_ExtractPath, ((String)DataRow["Name"]).Substring(0, ((String)DataRow["Name"]).LastIndexOf("/")).Replace('/', '\\')));
                Byte[] buffer = Utilities.InflateFile(DataRow);
                FileStream fileStream = new FileStream(Path.Combine(m_ExtractPath, ((String)DataRow["Name"]).Replace('/', '\\')), FileMode.Create, FileAccess.Write, FileShare.None);
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + " (" + ((String)DataRow["Name"]).Substring(0, ((String)DataRow["Name"]).LastIndexOf("/")).Replace('/', '\\') + ")", "TRE Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Run()
        {
            this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { 0, 0 } });
            this.m_form.Invoke(this.m_form.delegateSetCaption, new Object[] { "Constructing File Chain - 0%" });
            this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { "Constructing File Chain…" });

            this.lastProgress = -1;
            this.lastLabel = String.Empty;
            this.lastCaption = String.Empty;

            FollowChain(new String[] { (String)this.m_DataRow["Name"] });

            if (m_EventStop.WaitOne(0, true))
            {
                m_EventStopped.Set();

                return;
            }

            this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { 0 } });
            this.m_form.Invoke(this.m_form.delegateSetCaption, new Object[] { "Extracting - 0%" });
            this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { "Extracting…" });

            this.lastProgress = -1;
            this.lastLabel = String.Empty;
            this.lastCaption = String.Empty;

            for (Int32 counter = 0; counter < this.ExtractTable.Rows.Count; counter++)
            {
                WriteFile(this.ExtractTable.Rows[counter]);

                Int32 progress = (Int32)(((Double)counter / (Double)this.ExtractTable.Rows.Count) * 100F);
                if (this.lastProgress != progress)
                {
                    this.m_form.Invoke(this.m_form.delegateSetProgress, new Object[] { new Int32[] { progress } });
                    this.lastProgress = progress;
                }

                String caption = "Extracting - " + progress + "%";
                if (this.lastCaption != caption)
                {
                    this.m_form.Invoke(this.m_form.delegateSetCaption, new Object[] { caption });
                    this.lastCaption = caption;
                }

                String label = "Extracting " + this.ExtractTable.Rows[counter]["Name"] + "…";
                if (this.lastLabel != label)
                {
                    this.m_form.Invoke(this.m_form.delegateSetLabel, new Object[] { label });
                    this.lastLabel = label;
                }

                if (m_EventStop.WaitOne(0, true))
                {
                    m_EventStopped.Set();

                    return;
                }
            }

            this.m_form.Invoke(this.m_form.delegateExtractThreadFinished);
        }
        #endregion
    }

    #region Enumerations
    public enum InsertLocation { Above, Below };
    #endregion
}
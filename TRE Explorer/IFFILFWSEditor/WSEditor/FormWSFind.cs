using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SWGLib;

namespace TRE_Explorer {
  public partial class FormWSFind : Form {
    private WSFile m_WSFile;
    internal String SelectedID;
    private DataTable m_DataTable;
    private DataColumn m_DataColumnType;
    private DataColumn m_DataColumnName;
    private DataView m_DataView;

    private void InitializeDataTable() {
      this.m_DataColumnType = new DataColumn();
      this.m_DataColumnType.AllowDBNull = false;
      this.m_DataColumnType.AutoIncrement = false;
      this.m_DataColumnType.Caption = "Type";
      this.m_DataColumnType.ColumnName = "Type";
      this.m_DataColumnType.DataType = typeof(String);
      this.m_DataColumnType.Unique = false;

      this.m_DataColumnName = new DataColumn();
      this.m_DataColumnName.AllowDBNull = false;
      this.m_DataColumnName.AutoIncrement = false;
      this.m_DataColumnName.Caption = "Name";
      this.m_DataColumnName.ColumnName = "Name";
      this.m_DataColumnName.DataType = typeof(String);
      this.m_DataColumnName.Unique = false;

      this.m_DataTable = new DataTable();
      this.m_DataTable.Columns.AddRange(new DataColumn[] { this.m_DataColumnType, this.m_DataColumnName });

      this.m_DataView = new DataView(this.m_DataTable);
    }

    public FormWSFind(WSFile wsFile) {
      InitializeComponent();
      this.m_WSFile = wsFile;

      InitializeDataTable();
      foreach (String type in this.m_WSFile.Types) {
        String ObjectType = FormIFFILFWSEditor.WSObjectType(type);
        this.m_DataTable.Rows.Add(new Object[] { ((ObjectType == String.Empty) ? "Unknown" : ObjectType) , type });
      }

      DataSet dataSet = new DataSet();
      DataSetHelper dataSetHelper = new DataSetHelper(ref dataSet);
      DataTable dataTableTypes = dataSetHelper.SelectDistinct("Types", this.m_DataTable, "Type");

      this.comboBoxTypes.Items.Clear();
      foreach (DataRow dataRow in dataTableTypes.Rows) {
        this.comboBoxTypes.Items.Add(dataRow["Type"]);
      }
      this.comboBoxTypes.SelectedIndex = 0;
    }

    private String[] SearchNodes(WSFile.WSNode[] wsNodes) {
      this.m_DataView.RowFilter = "Name LIKE '%/" + this.comboBoxItemNames.Text + "'";
      String ObjectName = this.m_DataView[0]["Name"].ToString();
      String ObjectType = FormIFFILFWSEditor.WSObjectType(ObjectName);

      List<String> listString = new List<String>();
      foreach (WSFile.WSNode wsNode in wsNodes) {
        if (this.m_WSFile.Types[wsNode.ObjectIndex] == ObjectName) {
          listString.Add(wsNode.ID + ((ObjectType != String.Empty) ? " [" + ObjectType + "]" : ""));
        }
        if (wsNode.Nodes.Count > 0) {
          listString.AddRange(SearchNodes(wsNode.Nodes.ToArray()));
        }
      }
      return listString.ToArray();
    }

    private void buttonFind_Click(object sender, EventArgs e) {
      this.listBoxResults.Items.Clear();
      this.listBoxResults.Items.AddRange(SearchNodes(this.m_WSFile.Nodes.ToArray()));
    }

    private void buttonGoTo_Click(object sender, EventArgs e) {
      String ID = this.listBoxResults.SelectedItem.ToString();
      if (ID.Contains(" ")) {
        ID = ID.Substring(0, ID.IndexOf(" "));
      }
      this.SelectedID = ID;
      this.DialogResult = DialogResult.OK;
    }

    private void listBoxResults_SelectedIndexChanged(object sender, EventArgs e) {
      this.buttonGoTo.Enabled = ((this.listBoxResults.SelectedIndex >= 0) && (this.listBoxResults.SelectedIndex < this.listBoxResults.Items.Count));
    }

    private void comboBoxTypes_SelectedIndexChanged(object sender, EventArgs e) {
      if (this.comboBoxTypes.Text != String.Empty) {
        this.m_DataView.RowFilter = "Type = '" + this.comboBoxTypes.Text + "'";
        this.m_DataView.Sort = "Name ASC";
        this.comboBoxItemNames.Items.Clear();
        foreach (DataRowView dataRowView in this.m_DataView) {
          String type = this.comboBoxTypes.Text.ToLower().Replace(' ', '/');
          if (type == "sound") {
            type = "soundobject";
          }
          String name = dataRowView["Name"].ToString();
          name = name.Substring(name.IndexOf(type) + type.Length + 1);
          this.comboBoxItemNames.Items.Add(name);
        }
        this.comboBoxItemNames.SelectedIndex = 0;
        this.m_DataView.RowFilter = String.Empty;
      }
    }
  }
}
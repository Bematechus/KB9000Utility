using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmLayers : Form
    {
        private string _StrInit = "";

        public frmLayers()
        {
            InitializeComponent();
            init_grid(grdList);
            
        }
        private void init_grid(DataGridView grd)
        {
            grd.Columns.Clear();
            grd.AllowUserToAddRows = false;
            grd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grd.AllowUserToDeleteRows = false;
            grd.EditMode = DataGridViewEditMode.EditProgrammatically;
            grd.MultiSelect = false;
            grd.RowHeadersWidth = 25;
            grd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            grd.AutoGenerateColumns = false;
            grd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataGridViewTextBoxColumn col;

            col = new DataGridViewTextBoxColumn();
            col.Name = "Layer";
            col.DataPropertyName = "";
            col.HeaderText = "Shift Level";
            col.ValueType = typeof(string);
            col.Frozen = false;
            col.FillWeight = 60;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            grd.Columns.Add(col);

            


        }

        private void init_grd_data()
        {
            grdList.Rows.Clear();
            //grdList.Rows.Add(9);
            int nindex = 0;
            for (int i = 0; i < 9; i++)
            {
                string s =  string.Format("[ShiftLevel{0}]", i + 1);
                if (_StrInit.IndexOf(s) < 0)
                {
                    grdList.Rows.Add(1);
                    grdList.Rows[nindex].Cells[0].Value = s;
                    nindex++;
                }
            }
            
            
        }

        private void frmLayers_Load(object sender, EventArgs e)
        {
            init_grd_data();
        }

        protected virtual string GetSelectItemText(string colName)
        {
            DataGridViewRow r = grdList.CurrentRow;
            if (r == null) return "";
            return (string)(r.Cells[colName].Value);

        }

        public string input_layer(string strInit)
        {
            _StrInit = strInit;
            if (this.ShowDialog() == DialogResult.OK)
            {
                return GetSelectItemText("Layer");

            }
            else
                return "";

        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void grdList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK_Click(null, null);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmSlideKeyContent : Form
    {
        public frmSlideKeyContent()
        {
            InitializeComponent();
            init_grid(grdList);
        }

        private DiagramKey _SlideKey = null;

        private void init_grid(DataGridView grd)
        {
            grd.Columns.Clear();
            grd.AllowUserToAddRows = false;

            grd.AllowUserToDeleteRows = false;
            grd.EditMode = DataGridViewEditMode.EditProgrammatically;
            grd.MultiSelect = false;
            grd.RowHeadersWidth = 25;
            grd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            grd.AutoGenerateColumns = false;
            grd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataGridViewTextBoxColumn col;




            col = new DataGridViewTextBoxColumn();
            col.Name = "Function";
            col.DataPropertyName = "";
            col.HeaderText = "Function";
            col.ValueType = typeof(string);
            col.Frozen = false;
            col.FillWeight = 60;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            grd.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "Content";
            col.DataPropertyName = "";
            col.HeaderText = "Content";
            col.ValueType = typeof(string);
            col.Frozen = false;
            col.FillWeight = 200;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            grd.Columns.Add(col);

            DataGridViewButtonColumn btn = null;
            btn = new DataGridViewButtonColumn();
            btn.Name = "Edit";
            btn.DataPropertyName = "";
            btn.HeaderText = "Edit";
            btn.Text = "Edit";
            btn.UseColumnTextForButtonValue = true;
            btn.Frozen = false;
            btn.FillWeight = 100;
            btn.SortMode = DataGridViewColumnSortMode.NotSortable;
            btn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            grd.Columns.Add(btn);



        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            save_key(_SlideKey);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected virtual string GetSelectItemText(string colName)
        {
            DataGridViewRow r = grdList.CurrentRow;
            if (r == null) return "";
            return (string)(r.Cells[colName].Value);

        }
      
        private void show_slidev_key(DiagramKeySlideV key)
        {
            //show codes
            grdList.Rows.Clear();
            grdList.Rows.Add(4);

            grdList.Rows[0].Cells[0].Value = "Up";
            grdList.Rows[0].Cells[1].Value = key.SlideUp.ToString();


            grdList.Rows[1].Cells[0].Value = "Up Hold";
            grdList.Rows[1].Cells[1].Value = key.SlideUpHold.ToString();

            grdList.Rows[2].Cells[0].Value = "Down";
            grdList.Rows[2].Cells[1].Value = key.SlideDown.ToString();

            grdList.Rows[3].Cells[0].Value = "Down Hold";
            grdList.Rows[3].Cells[1].Value = key.SlideDownHold.ToString();


        }

        private void save_slidev_key(DiagramKeySlideV key)
        {
            //up
            string s = grdList.Rows[0].Cells[1].Value.ToString();
            key.SlideUp.SetKeyCodeString(s);


            //Up Hold;
            s = grdList.Rows[1].Cells[1].Value.ToString();
            key.SlideUpHold.SetKeyCodeString(s);

            //Down";
            s = grdList.Rows[2].Cells[1].Value.ToString();
            key.SlideDown.SetKeyCodeString(s);

            // "Down Hold";
            s = grdList.Rows[3].Cells[1].Value.ToString();
            key.SlideDownHold.SetKeyCodeString(s);


        }

        private void show_slideh_key(DiagramKeySlideH key)
        {
            //show codes
            grdList.Rows.Clear();
            grdList.Rows.Add(4);

            grdList.Rows[0].Cells[0].Value = "Left";
            grdList.Rows[0].Cells[1].Value = key.SlideLeft.ToString();


            grdList.Rows[1].Cells[0].Value = "Left Hold";
            grdList.Rows[1].Cells[1].Value = key.SlideLeftHold.ToString();

            grdList.Rows[2].Cells[0].Value = "Right";
            grdList.Rows[2].Cells[1].Value = key.SlideRight.ToString();

            grdList.Rows[3].Cells[0].Value = "Right Hold";
            grdList.Rows[3].Cells[1].Value = key.SlideRightHold.ToString();


        }

        private void save_slideh_key(DiagramKeySlideH key)
        {
            //up
            string s = grdList.Rows[0].Cells[1].Value.ToString();
            key.SlideLeft.SetKeyCodeString(s);


            //Up Hold;
            s = grdList.Rows[1].Cells[1].Value.ToString();
            key.SlideLeftHold.SetKeyCodeString(s);

            //Down";
            s = grdList.Rows[2].Cells[1].Value.ToString();
            key.SlideRight.SetKeyCodeString(s);

            // "Down Hold";
            s = grdList.Rows[3].Cells[1].Value.ToString();
            key.SlideRightHold.SetKeyCodeString(s);


        }

        private void show_key(DiagramKey key)
        {
         
            switch(key.TypeName)
            {
                case DiagramKey.KEY_SLIDEV:
                    {
                        show_slidev_key((DiagramKeySlideV)key);
                    }
                    break;
                case DiagramKey.KEY_SLIDEH:
                    {
                        show_slideh_key((DiagramKeySlideH)key);
                    }
                    break;
            }
        }
        private void save_key(DiagramKey key)
        {
            switch (key.TypeName)
            {
                case DiagramKey.KEY_SLIDEV:
                    {
                        save_slidev_key((DiagramKeySlideV)key);
                    }
                    break;
                case DiagramKey.KEY_SLIDEH:
                    {
                        save_slideh_key((DiagramKeySlideH)key);
                    }
                    break;
            }
        }
        public void edit_slide_button(DiagramKey key)
        {
            if (key.TypeName != DiagramKey.KEY_SLIDEH &&
                key.TypeName != DiagramKey.KEY_SLIDEV)
                return;
            _SlideKey = key;
            show_key(key);
            if (this.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {
             
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2) //edit col
                return;
            string strInit = (string)(grdList.Rows[e.RowIndex].Cells[1].Value);
            frmKeyCode frm = new frmKeyCode();
           
            string s = frm.InputKeyCode(strInit);
            if (s == string.Empty) return;


            grdList.Rows[e.RowIndex].Cells[1].Value = s;
           

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
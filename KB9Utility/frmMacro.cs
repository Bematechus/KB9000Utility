using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmMacro : Form
    {
        public frmMacro()
        {
            InitializeComponent();
            tsbClear.Image = Util.get_image("clear");
            tsbUndo.Image = Util.get_image("undo");
            tsbDelay.Image = Util.get_image("delay");
            
            tsbSpecial.Image = Util.get_image("specialkey");
            tsbCombinatioin.Image = Util.get_image("combination");
            //tsbMacro.Image = Util.get_image("macro");
            tsbRepeat.Image = Util.get_image("repeat");
           // init_grid(grdList);
        }
        private bool _ForEditMacro = false;
        public bool ForEditMacro
        {
            get 
            {
                return _ForEditMacro;
            }
            set 
            {
                _ForEditMacro = value;
            }
        }

        private KB9TextBox GetFocusedTextBox()
        {
            KB9TextBox[] ar = new KB9TextBox[]
            {
                txtMacro1,
                txtMacro2,
                txtMacro3,
                txtMacro4,
                txtMacro5,
              
            };

            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i].Visible &&
                    ar[i].Focused)
                    return ar[i];
            }
            return null;
        }

        private string GetSelectedMacro()
        {
            RadioButton[] ar = new RadioButton[]
            {
                rbMacro1,
                rbMacro2,
                rbMacro3,
                rbMacro4,
                rbMacro5,
              
            };

            for (int i = 0; i < ar.Length; i++)
            {
                if (
                    ar[i].Checked)
                {
                    string s = string.Format("Macro{0}", i + 1);
                    return s;
                }
            }
            return "";
        }

        //private void init_grid(DataGridView grd)
        //{
        //    grd.Columns.Clear();
        //    grd.AllowUserToAddRows = false;
        //    grd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //    grd.AllowUserToDeleteRows = false;
        //    grd.EditMode = DataGridViewEditMode.EditProgrammatically;
        //    grd.MultiSelect = false;
        //    grd.RowHeadersWidth = 25;
        //    grd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        //    grd.AutoGenerateColumns = false;
        //    grd.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
        //    DataGridViewTextBoxColumn col;
            

            

        //    col = new DataGridViewTextBoxColumn();
        //    col.Name = "Macro";
        //    col.DataPropertyName = "";
        //    col.HeaderText = "Macro";
        //    col.ValueType = typeof(string);
        //    col.Frozen = false;
        //    col.FillWeight = 60;
        //    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
        //    grd.Columns.Add(col);

        //    col = new DataGridViewTextBoxColumn();
        //    col.Name = "Data";
        //    col.DataPropertyName = "";
        //    col.HeaderText = "Contents";
        //    col.ValueType = typeof(string);
        //    col.Frozen = false;
        //    col.FillWeight = 200;
        //    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
        //    grd.Columns.Add(col);

        //    DataGridViewButtonColumn btn = null;
        //    btn = new DataGridViewButtonColumn();
        //    btn.Name = "Edit";
        //    btn.DataPropertyName = "";
        //    btn.HeaderText = "Edit";
        //    btn.Text = "Edit";
        //    btn.UseColumnTextForButtonValue = true;
        //    btn.Frozen = false;
        //    btn.FillWeight = 100;
        //    btn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //    btn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //    grd.Columns.Add(btn);

           

        //}
    
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //protected virtual string GetSelectItemText()
        //{
        //    //DataGridViewRow r = grdList.CurrentRow;
        //    //if (r == null) return "";
        //    //return (string)(r.Cells[colName].Value);

        //    return "";
        //}

        public string input_macro(DEditor editor)
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                return GetSelectedMacro();

            }
            else
                return "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void show_macros()
        {

            
            txtMacro1.Text = Program.MainForm.editorMain.Macro1.ToString();
            txtMacro2.Text = Program.MainForm.editorMain.Macro2.ToString();
            txtMacro3.Text = Program.MainForm.editorMain.Macro3.ToString();
            txtMacro4.Text = Program.MainForm.editorMain.Macro4.ToString();
            txtMacro5.Text = Program.MainForm.editorMain.Macro5.ToString();


            

            //show codes
            //grdList.Rows.Clear();
            //grdList.Rows.Add(5);
            //grdList.Rows[0].Cells[0].Value = "Macro1";
            //grdList.Rows[0].Cells[1].Value = Program.MainForm.editorMain.Macro1.ToString();


            //grdList.Rows[1].Cells[0].Value = "Macro2";
            //grdList.Rows[1].Cells[1].Value = Program.MainForm.editorMain.Macro2.ToString();

            //grdList.Rows[2].Cells[0].Value = "Macro3";
            //grdList.Rows[2].Cells[1].Value = Program.MainForm.editorMain.Macro3.ToString();

            //grdList.Rows[3].Cells[0].Value = "Macro4";
            //grdList.Rows[3].Cells[1].Value = Program.MainForm.editorMain.Macro4.ToString();

            //grdList.Rows[4].Cells[0].Value = "Macro5";
            //grdList.Rows[4].Cells[1].Value = Program.MainForm.editorMain.Macro5.ToString();
        }
        private void frmMacro_Load(object sender, EventArgs e)
        {
            show_macros();
            if (this.ForEditMacro)
            {
                btnOK.Visible = false;
                btnCancel.Text = "&Exit";
                rbMacro1.Visible = false;
                rbMacro2.Visible = false;
                rbMacro3.Visible = false;
                rbMacro4.Visible = false;
                rbMacro5.Visible = false;

            }
            else
            {//insert macro to text

                btnOK.Text = "&Insert Macro";
                tlMacro.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            }

           
        }

        private void lblMacro1_MouseClick(object sender, MouseEventArgs e)
        {
            rbMacro1.Checked = true;
        }

        private void lblMacro2_MouseClick(object sender, MouseEventArgs e)
        {
            rbMacro2.Checked = true;
        }

        private void lblMacro3_MouseClick(object sender, MouseEventArgs e)
        {
            rbMacro3.Checked = true;
        }

        private void lblMacro4_MouseClick(object sender, MouseEventArgs e)
        {
            rbMacro4.Checked = true;
        }

        private void lblMacro5_MouseClick(object sender, MouseEventArgs e)
        {
            rbMacro5.Checked = true;
        }

        private void txtMacro1_Enter(object sender, EventArgs e)
        {
            rbMacro1.Checked = true;
            frmOnScreenKbd.Instance().FocusedTextBox = GetFocusedTextBox();
        }

        private void txtMacro2_Enter(object sender, EventArgs e)
        {
            rbMacro2.Checked = true;
            frmOnScreenKbd.Instance().FocusedTextBox = GetFocusedTextBox();
        }

        private void txtMacro3_Enter(object sender, EventArgs e)
        {
            rbMacro3.Checked = true;
            frmOnScreenKbd.Instance().FocusedTextBox = GetFocusedTextBox();
        }

        private void txtMacro4_Enter(object sender, EventArgs e)
        {
            rbMacro4.Checked = true;
            frmOnScreenKbd.Instance().FocusedTextBox = GetFocusedTextBox();
        }

        private void txtMacro5_Enter(object sender, EventArgs e)
        {
            rbMacro5.Checked = true;
            frmOnScreenKbd.Instance().FocusedTextBox = GetFocusedTextBox();
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {

            KB9TextBox t = GetFocusedTextBox();
            if (t == null)
                return;
            t.Clear();
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            if (t == null)
                return;
            t.Undo();
        }

        private void tsbDelay_Click(object sender, EventArgs e)
        {
            frmOnScreenKbd.Instance().Hide();
            KB9TextBox t = GetFocusedTextBox();
            if (t == null) return;
            frmDelay frm = new frmDelay();
            decimal d = frm.InputDelay();
            if (d <= 0)
                return;
            //d *= 10; //delay time is nx0.1 format, so need to x10 to make string
            //int n = (int)d;
            //string s = n.ToString();
            //s = "Pause" + s;
            string s = frmDelay.makePauseString(d);
            t.AddKeyCode(s, true);
        }

        private void tsbSpecial_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            if (t == null)
                return;

            frmOnScreenKbd.show_special_list_modeless(this, t);
            /*
            frmSpecialKey frmKeyList = frmSpecialKey.Instance();
            if (frmKeyList == null)
                frmKeyList = new frmSpecialKey();
            frmKeyList.TopMost = true;
            //set it initial position
            int dx = this.Width;// -frmKeyList.Width;
            int dy = 0;//
            Point pt = this.Location;
            pt.Offset(dx, dy);
            frmKeyList.Location = pt;
            frmKeyList.FocusedTextBox = t;
            if (frmKeyList.FocusedTextBox == null)
            {
                frmKeyList.Hide();
                //frmKeyList = null;
            }
            else
            {

                //if (!frmKeyList.Visible)
                frmKeyList.Visible = false;
                    frmKeyList.Show(this);
            }
             * */
        }

        private void tsbCombinatioin_Click(object sender, EventArgs e)
        {
            frmOnScreenKbd.Instance().Hide();
            KB9TextBox t = GetFocusedTextBox();
            if (t == null) return;
            frmCombination frm = new frmCombination();
            string s = frm.InputCombinationKeys();

            if (s.Length > 0)
            {
                t.AddKeyCode(s, true);
            }
        }

        private void tsbRepeat_Click(object sender, EventArgs e)
        {
            frmOnScreenKbd.Instance().Hide();
            KB9TextBox t = GetFocusedTextBox();
            if (t == null) return;
            frmRepeat frm = new frmRepeat();
            int n = frm.InputRepeat();
            if (n <= 0)
                return;


            string s = n.ToString();
            s = "Repeat" + s;
            t.AddKeyCode(s, true);
        }

        private void tsbLevel_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            if (t == null) return;
            frmLayers frm = new frmLayers();
            string s = frm.input_layer(t.Text);

            if (s == string.Empty)
                return;

            t.AddKeyCode(s, false);
        }

        private void txtMacro2_OnTextChanged(object sender)
        {
            DEditor editor = Program.MainForm.editorMain;
            editor.Macro2.SetKeyCodeString(txtMacro2.Text);
        }

        private void txtMacro1_OnTextChanged(object sender)
        {
            DEditor editor = Program.MainForm.editorMain;
            string s = txtMacro1.Text;
            editor.Macro1.SetKeyCodeString( s);
            
        }

        private void txtMacro3_OnTextChanged(object sender)
        {
            DEditor editor = Program.MainForm.editorMain;
            editor.Macro3.SetKeyCodeString(txtMacro3.Text);
        }

        private void txtMacro4_OnTextChanged(object sender)
        {
            DEditor editor = Program.MainForm.editorMain;
            editor.Macro4.SetKeyCodeString(txtMacro4.Text);
        }

        private void txtMacro5_OnTextChanged(object sender)
        {
            DEditor editor = Program.MainForm.editorMain;
            editor.Macro5.SetKeyCodeString(txtMacro5.Text);
        }

        private void txtMacro1_Validating(object sender, CancelEventArgs e)
        {
            
            KB9TextBox t = (KB9TextBox)sender;
            if (t == null)
                return;
            System.Diagnostics.Debug.Print("txtMacro1_Validating " + t.Name);
            int nres = KB9Validation.ValidateKeyTextBox2(this, t);
            if (nres == 0)
            {
                t.ClearUndo();
                return;
            }

            e.Cancel = true;

            if (nres == 3) //cancel changes
            {

                t.Undo();

            }
           
        }

        //private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex != 2) //edit col
        //        return;
        //    string strInit =(string)( grdList.Rows[e.RowIndex].Cells[1].Value);
        //    frmKeyCode frm = new frmKeyCode();
        //    frm.EditingMacro = true;
        //    string s = frm.InputKeyCode(strInit);
        //    if (s == string.Empty) return;

            
        //    grdList.Rows[e.RowIndex].Cells[1].Value = s;
        //        //update macro
        //    DEditor editor = Program.MainForm.editorMain;
        //    switch(e.RowIndex)
        //    {
        //        case 0:
        //            editor.Macro1.SetKeyCodeString( s);
        //            break;
        //        case 1:
        //            editor.Macro2.SetKeyCodeString( s);
        //            break;
        //        case 2: 
        //            editor.Macro3.SetKeyCodeString( s);
        //            break;
        //        case 3: 
        //            editor.Macro4.SetKeyCodeString( s);
        //            break;
        //        case 4:
        //            editor.Macro5.SetKeyCodeString( s);
        //            break;
        //        default:
        //            return;

        //    }




        //}

        //private void grdList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    btnOK_Click(null, null);
        //}
    }
}
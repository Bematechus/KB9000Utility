using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmKeyCode : Form
    {
        public const string SEPARATOR = "[Separator]";
        public const string SHIFTLEVEL = "[ShiftLevel";
        public const int LEVEL_COUNT = 9;
        public frmKeyCode()
        {
            InitializeComponent();
            tsbClear.Image = Util.get_image("clear");
            tsbUndo.Image = Util.get_image("undo");
            tsbDelay.Image = Util.get_image("delay");
            //tsbScan.Image = Util.get_image("number");
            //tsmMultiple.Image = Util.get_image("multiplelayer");
            //tsmSingle.Image = Util.get_image("singlelayer");
            tsbSpecial.Image = Util.get_image("specialkey");
            tsbCombinatioin.Image = Util.get_image("combination");
            tsbMacro.Image = Util.get_image("macro");
            tsbRepeat.Image = Util.get_image("repeat");
            UpdateGUI();
            
        }

        private bool _EditingMacro = false;
        public bool EditingMacro
        {
            get { return _EditingMacro; }
            set 
            { 
                _EditingMacro = value;
                tsmMacro.Visible = (!_EditingMacro);
                tsbMacro.Visible = (!_EditingMacro);
                
            }
        }

        

        private void chkMultipleLevel_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }
        private void UpdateGUI()
        {
            /*
            bool bMultipleLevel = chkMultipleLevel.Checked;

            Control[] ar = new Control[]
                {               
             
                    txtLevel1,
                    txtLevel2,
                    txtLevel3,
                    txtLevel4,
                    txtLevel5,
                    txtLevel6,
                    txtLevel7,
                    txtLevel8,
                    txtLevel9,
                    label1, 
                    label2, 
                    label3, 
                    label4, 
                    label5, 
                    label6, 
                    label7, 
                    label8, 
                    label9, 
                    
                };
            for (int i = 0; i < ar.Length; i++)
            {
                ar[i].Visible = bMultipleLevel;
                    
            }
            arrange_controls();
             * */
            //tblLevel.Visible = bMultipleLevel;
            //txtKeyCode.Visible = !bMultipleLevel;

            //adjust form size
            //if (bMultipleLevel)
            //{
            //    this.Size = new Size(700, 540);
            //}
            //else
            //{
            //    this.Size = new Size(700, 210);
            //}
           
           
        }

        private void enable_menus(bool bEnable)
        {
            ToolStripMenuItem[] menus = new ToolStripMenuItem[]
            {
                tsmClear, tsmUndo, tsmDelay, tsmRepeat, tsmSpecial, tsmCombination, tsmMacro,
                
            };

            for (int i = 0; i < menus.Length; i++)
                menus[i].Enabled = bEnable;

            ToolStripButton[] controls = new ToolStripButton[]
            {
                tsbClear, tsbUndo, tsbDelay, tsbRepeat, tsbSpecial, tsbCombinatioin, tsbMacro
            };

            for (int i = 0; i < controls.Length; i++)
                controls[i].Enabled = bEnable;

           // if (bEnable)
            {
                UpdateUndoMenu();
                
            }
        }

        private void UpdateUndoMenu()
        {
           // if (bEnable)
            {
                KB9TextBox t = GetFocusedTextBox();
                if (t != null)
                {
                    tsbUndo.Enabled = (t.CanUndo());
                    tsmUndo.Enabled = (t.CanUndo());

                }
                else
                {
                    tsbUndo.Enabled = false;
                    tsmUndo.Enabled = false;
                }
                

            }
        }

        private void chkMultipleLevel_Click(object sender, EventArgs e)
        {
          //  UpdateGUI();
        }

        private bool ValidateKeyTextBox(KB9TextBox ctrlText)
        {
            return KB9Validation.ValidateKeyTextBox(this, ctrlText);
            //string strKeys = ctrlText.Text;// GetInputedKeyCodes();
            //if (strKeys == string.Empty) return true;
            //KB9Validation kb9 = new KB9Validation();
            //KB9Validation.ERR_VALIDATION err = kb9.ValidateKB9Keycodes(strKeys);
            //if (err == KB9Validation.ERR_VALIDATION.OK) return true;

            //string err_msg = ValidateErrMsg(err);
            //if (err == KB9Validation.ERR_VALIDATION.Less_AltUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_RAltUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_CtrlUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_RCtrlUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_RShiftUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_ShiftUp)
            //{


            //    err_msg += "\n\rDo you want utility append this key to end?";

            //    DialogResult result = MessageBox.Show(err_msg, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            //    if (result == DialogResult.Yes)
            //    {
            //        string strAppend = AppendErrKey(err);
            //        ctrlText.AddKeyCode(strAppend, false);
            //    }
            //}
            //else
            //{
            //    err_msg += "\n\rPlease fixed it manually.";
            //    MessageBox.Show(err_msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            
            //return false;

            
        }
        //private string ValidateErrMsg(KB9Validation.ERR_VALIDATION err)
        //{
        //    switch(err)
        //    {
        //        case KB9Validation.ERR_VALIDATION.Less_ShiftDown:
        //            return "Shift down less than up.";
        //        case KB9Validation.ERR_VALIDATION.Less_ShiftUp:
        //            return "Shift up less than down.";
        //        case KB9Validation.ERR_VALIDATION.Less_RShiftDown:
        //            return "RightShift down less than up.";
        //        case KB9Validation.ERR_VALIDATION.Less_RShiftUp:
        //            return "RightShift up less than down.";
        //        case KB9Validation.ERR_VALIDATION.Less_CtrlDown:
        //            return "Control down less than up.";
        //        case KB9Validation.ERR_VALIDATION.Less_CtrlUp:
        //            return "Control up less than down.";
        //        case KB9Validation.ERR_VALIDATION.Less_RCtrlDown:
        //            return "RightControl down less than up.";
        //        case KB9Validation.ERR_VALIDATION.Less_RCtrlUp:
        //            return "RightControl up less than down.";
        //        case KB9Validation.ERR_VALIDATION.Less_AltDown:
        //            return "Alt down less than up.";
        //        case KB9Validation.ERR_VALIDATION.Less_AltUp:
        //            return "Alt up less than down.";
        //        case KB9Validation.ERR_VALIDATION.Less_RAltDown:
        //            return "RightAlt down less than up.";
        //        case KB9Validation.ERR_VALIDATION.Less_RAltUp:
        //            return "RightAlt up less than down.";
        //        case KB9Validation.ERR_VALIDATION.DownUp_Messed:
        //            return "Up before down.";
        //    }
        //    return "";
        //}

        //private string AppendErrKey(KB9Validation.ERR_VALIDATION err)
        //{
        //    switch (err)
        //    {
        //        case KB9Validation.ERR_VALIDATION.Less_ShiftDown:
        //            return KB9KeyDefinition.KeyDisplayName(Keys.ShiftKey);
        //        case KB9Validation.ERR_VALIDATION.Less_ShiftUp:
        //            return KB9KeyDefinition.KeyUpDisplayName(Keys.ShiftKey);
        //        case KB9Validation.ERR_VALIDATION.Less_RShiftDown:
        //            return KB9KeyDefinition.KeyDisplayName(Keys.RShiftKey);
        //        case KB9Validation.ERR_VALIDATION.Less_RShiftUp:
        //            return KB9KeyDefinition.KeyUpDisplayName(Keys.RShiftKey);
        //        case KB9Validation.ERR_VALIDATION.Less_CtrlDown:
        //            return KB9KeyDefinition.KeyDisplayName(Keys.ControlKey);
        //        case KB9Validation.ERR_VALIDATION.Less_CtrlUp:
        //            return KB9KeyDefinition.KeyUpDisplayName(Keys.ControlKey);
        //        case KB9Validation.ERR_VALIDATION.Less_RCtrlDown:
        //            return KB9KeyDefinition.KeyDisplayName(Keys.RControlKey);
        //        case KB9Validation.ERR_VALIDATION.Less_RCtrlUp:
        //            return KB9KeyDefinition.KeyUpDisplayName(Keys.RControlKey);
        //        case KB9Validation.ERR_VALIDATION.Less_AltDown:
        //            return KB9KeyDefinition.KeyDisplayName(Keys.Menu);
        //        case KB9Validation.ERR_VALIDATION.Less_AltUp:
        //            return KB9KeyDefinition.KeyUpDisplayName(Keys.Menu);
        //        case KB9Validation.ERR_VALIDATION.Less_RAltDown:
        //            return KB9KeyDefinition.KeyDisplayName(Keys.RMenu);
        //        case KB9Validation.ERR_VALIDATION.Less_RAltUp:
        //            return KB9KeyDefinition.KeyUpDisplayName(Keys.RMenu);

        //    }
        //    return "";
        //}

        private bool ValidateKeys()
        {
            //if (!chkMultipleLevel.Checked)
            //{
                return ValidateKeyTextBox(txtKeyCode);
            /*}

            else
            {
                KB9TextBox[] ar = new KB9TextBox[]
                {               
                    txtKeyCode,
                    txtLevel1,
                    txtLevel2,
                    txtLevel3,
                    txtLevel4,
                    txtLevel5,
                    txtLevel6,
                    txtLevel7,
                    txtLevel8,
                    txtLevel9  };
                for (int i=0; i< ar.Length; i++)
                {
                    if (!ValidateKeyTextBox(ar[i]))
                        return false;
                }
            }*/
            //return true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateKeys()) return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private KB9TextBox GetFocusedTextBox()
        {
            KB9TextBox[] ar = new KB9TextBox[]
            {
                txtKeyCode,
                //txtLevel1,
                //txtLevel2,
                //txtLevel3,
                //txtLevel4,
                //txtLevel5,
                //txtLevel6,
                //txtLevel7,
                //txtLevel8,
                //txtLevel9,
            };

            for (int i=0; i< ar.Length; i++)
            {
                if (ar[i].Visible &&
                    ar[i].Focused)
                    return ar[i];
            }
            return null;
        }

        private void tsmClear_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            if (t == null)
                return;
            t.Clear();
        }

        private void toolStripMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            tsmClear_Click(null, null);
        }

        private void tsmUndo_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            if (t == null)
                return;
            t.Undo();
            UpdateUndoMenu();
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            tsmUndo_Click(null, null);
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            tsmClear_Click(null, null);
        }

        private void tsmScan_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();

            frmAscii frm = new frmAscii();
            string strscan = frm.InputAscii();
            if (strscan.Length >0)
            {
                t.AddKeyCode(strscan, true);
            }
            //frm.ShowDialog();
        }

        private void tsmDelay_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            frmDelay frm = new frmDelay();
            decimal d = frm.InputDelay();
            if (d <= 0)
                return;
            
            /*
            d *= 10; //delay time is nx0.1 format, so need to x10 to make string
            int n = (int)d;
            string s = n.ToString();
            
            s = "Pause" + s;
             * */
            string s = frmDelay.makePauseString(d);
            t.AddKeyCode(s, true);
            //frm.ShowDialog();
        }

        private void tsmSpecial_Click(object sender, EventArgs e)
        {

            frmOnScreenKbd.show_special_list_modeless(this, GetFocusedTextBox());
            //KB9TextBox t = GetFocusedTextBox();
            //frmSpecialKey frm = new frmSpecialKey();
            //string s = frm.InputSpecialKey();
            //if (s.Length >0)
            //{
            //    t.AddKeyCode(s);
            //}
          //  frm.ShowDialog();

            ////

            //KB9TextBox t = GetFocusedTextBox();
            //if (t == null)
            //    return;


            //frmSpecialKey frmKeyList = frmSpecialKey.Instance();
            //if (frmKeyList == null)
            //    frmKeyList = new frmSpecialKey();
            //frmKeyList.TopMost = true;
            ////set it initial position
            //int dx = this.Width;// -frmKeyList.Width;
            //int dy = 0;//
            //Point pt = this.Location;
            //pt.Offset(dx, dy);
            //frmKeyList.Location = pt;
            //frmKeyList.FocusedTextBox = t;
            //if (frmKeyList.FocusedTextBox == null)
            //{
            //    frmKeyList.Hide();
            //    //frmKeyList = null;
            //}
            //else
            //{

            //    //if (!frmKeyList.Visible)
            //    frmKeyList.Visible = false;
            //    frmKeyList.Show(this);
            //}
        }

        private void tsbDelay_Click(object sender, EventArgs e)
        {
            tsmDelay_Click(null, null);

        }

        private void tsbLayer_Click(object sender, EventArgs e)
        {
            tsmScan_Click(null, null);
        }

        private void tsbSpecial_Click(object sender, EventArgs e)
        {
            tsmSpecial_Click(null, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tsmCombination_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            if (t == null) return;
            frmCombination frm = new frmCombination();
            string s = frm.InputCombinationKeys();
            
            if (s.Length >0)
            {
                t.AddKeyCode(s, true);
            }
            
        }

        private void tsbCombinatioin_Click(object sender, EventArgs e)
        {
            tsmCombination_Click(null, null);
        }
        /// <summary>
        /// edit the key code string.
        /// 
        /// </summary>
        /// <param name="strInit"></param>
        /// <returns></returns>
        public string InputKeyCode(string strInit)
        {
            SetInitKeyCodes(strInit);
            if (this.ShowDialog() == DialogResult.OK)
            {
                return GetInputedKeyCodes();
            }
            else
                return strInit;
        }

        private string GetInputedKeyCodes()
        {
            //if (chkMultipleLevel.Checked)
            //{
            //    string strReturn = txtKeyCode.Text;
            //    for (int i=1; i<= LEVEL_COUNT; i++ )
            //    {
            //        string s = GetMultipleLevelTextBox(i).Text;
            //        if (s.Length > 0)
            //        {
            //            //if (strReturn.Length > 0)
            //                //strReturn +=  SEPARATOR;
            //            strReturn += (SHIFTLEVEL + (i).ToString()+ "]");
            //            strReturn += s;
            //        }
            //    }
            //    return strReturn;
            //}
            //else
            //{
                return txtKeyCode.Text;
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strInit"></param>
        /// <returns></returns>
        private void SetInitKeyCodes(string strInit)
        {
            //1. Check if it is multiple level codes
            //bool bMultiple = (strInit.IndexOf(SHIFTLEVEL) >= 0);
            //chkMultipleLevel.Checked = bMultiple;
            //if (bMultiple)
            //{
            //    List<string> ar = new List<string>();
            //    int ncount = SeparateKeyCodesToList(strInit, ar);
            //    if (ncount>0)
            //    {
            //        for (int i=0; i< ncount; i++)
            //        {
            //            KB9TextBox t = GetMultipleLevelTextBox(i);
            //            if (t != null)
            //            {
            //                t.ParseString(ar[i]);
            //            }
            //        }
            //    }
            //}
            //else
            //{
                txtKeyCode.ParseString(strInit);
                txtKeyCode.ClearUndo();
            //}

            //clear all undo in textbox
            //for (int i=0; i< LEVEL_COUNT+1; i++)
            //{
            //    GetMultipleLevelTextBox(i).ClearUndo();
            //}

            UpdateUndoMenu();
            
        }
        private KB9TextBox GetMultipleLevelTextBox(int nIndex)
        {
            KB9TextBox[] ar = new KB9TextBox[]
            {
                txtKeyCode,
                //txtLevel1,
                //txtLevel2,
                //txtLevel3,
                //txtLevel4,
                //txtLevel5,
                //txtLevel6,
                //txtLevel7,
                //txtLevel8,
                //txtLevel9,
            };
            if (nIndex >= 0 && nIndex < ar.Length)
                return ar[nIndex];
            else
                return null;
        }
        /// <summary>
        /// Format: qqqqqqq[ShiftLevel1]bbbbbddfcd[ShiftLevel3]dksf;askdf[ShiftLevel9]kdkdkdkdk
        /// </summary>
        /// <param name="strKeys"></param>
        /// <param name="ar"></param>
        /// <returns></returns>
        private int SeparateKeyCodesToList(string strKeys, List<string> ar)
        {
            ar.Clear();
            string s = strKeys;
            for (int i = 0; i < LEVEL_COUNT+1; i++) //add normal level, the index=0
                ar.Add(string.Empty);
            int nlastIndex = 0;
            while (true)
            {
                int n = s.IndexOf(SHIFTLEVEL);
                if (n >= 0)
                {
                    string str = s.Substring(0, n);
                    if (str.Length > 0)
                    {
                        ar[nlastIndex] = str;
                        //ar.Add(str);
                    }
                    //[ShiftLevel#]
                    string strlevel = s.Substring(n + SHIFTLEVEL.Length, 1);
                    nlastIndex = int.Parse(strlevel);
                    s = s.Substring(n + SHIFTLEVEL.Length+2);


                }
                else
                {
                    if (s.Length > 0)
                    {
                        ar[nlastIndex] = s;
                        //ar.Add(s);
                    }
                    break;
                }
            }
            return ar.Count;
        }

        private void tsmRepeat_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            frmRepeat frm = new frmRepeat();
            int n = frm.InputRepeat();
            if (n <= 0)
                return;
            
            
            string s = n.ToString();
            s = "Repeat" + s;
            t.AddKeyCode(s, true);
           
        }

        private void tstMacro_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            frmMacro frm = new frmMacro();
            string s = frm.input_macro(null);
            if (s == string.Empty)
                return;

            t.AddKeyCode(s, true);
        }

        private void tsbRepeat_Click(object sender, EventArgs e)
        {
            tsmRepeat_Click(sender, e);
        }

        private void tsbMacro_Click(object sender, EventArgs e)
        {
            tstMacro_Click(sender, e);
        }

        private void arrange_controls()
        {
            //this.SuspendLayout();
            //Control[,] ar = new Control[,]{
            //    {txtKeyCode,null},
            //    //{txtLevel1,label1},
            //    //{txtLevel2,label2},
            //    //{txtLevel3,label3},
            //    //{txtLevel4,label4},
            //    //{txtLevel5,label5},
            //    //{txtLevel6,label6},
            //    //{txtLevel7,label7},
            //    //{txtLevel8,label8},
            //    //{txtLevel9,label9},
            //    {btnOK,btnCancel},
            //};
            //Control lastVisible = null;
            //for (int i=1; i< ar.Length/2; i++)
            //{
            //    if (ar[i - 1,0].Visible)
            //        lastVisible = ar[i - 1,0];
            //    if (lastVisible == null)
            //        continue;
            //    ar[i,0].Top = lastVisible.Bottom + 3;
            //    if (ar[i, 1] != null)
            //        ar[i, 1].Top = ar[i, 0].Top;

            //}
            //btnOK.Top += 5;
            //btnCancel.Top = btnOK.Top;



            //this.Height = btnOK.Bottom + GetTitleBarHeight() + GetTitleBarHeight() / 2;// toolStripMain.Height;
            //this.ResumeLayout();
        }

        private int GetTitleBarHeight()
        {
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);

            int titleHeight = screenRectangle.Top - this.Top;
            return titleHeight;
        }
        private void txtKeyCode_Resize(object sender, EventArgs e)
        {
            arrange_controls();
        }

        private void frmKeyCode_Load(object sender, EventArgs e)
        {
            arrange_controls();
            enable_menus(false);
            
        }

        private void txtKeyCode_Enter(object sender, EventArgs e)
        {
            enable_menus(true);
            frmOnScreenKbd.Instance().FocusedTextBox = GetFocusedTextBox();
            
        }

        private void txtKeyCode_Leave(object sender, EventArgs e)
        {
            enable_menus(false);
        }

        private void txtKeyCode_OnTextChanged(object sender)
        {


            UpdateUndoMenu();
        }
    }
}
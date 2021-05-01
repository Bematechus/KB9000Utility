using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{ 
    public partial class frmOnScreenKbd : Form
    {
        static public frmOnScreenKbd frmForm = null;
        static public frmOnScreenKbd Instance()
        {
            if (frmForm == null)
                frmForm = new frmOnScreenKbd();

            return frmForm;
        }
        private KB9TextBox _KB9TextBox = null;
        public KB9TextBox FocusedTextBox
        {
            get { return _KB9TextBox; }
            set 
            {
                if (_KB9TextBox != value)
                {
                    _KB9TextBox = value;
                    reset_all_function_buttons();
                }
            }
        }

        private bool _ForCombination = false;

        public frmOnScreenKbd()
        {
            InitializeComponent();
            _ForCombination = false;
            init_keys_list();
            init_keys_data(false);
            show_keys(false);
            //timerBlink.Start();
        }
        public frmOnScreenKbd(bool bForCombination)
        {
            InitializeComponent();
            _ForCombination = bForCombination;
            init_keys_list();
            init_keys_data(bForCombination);
        }
        private void init_keys_list()
        {
            /*
            grdKeys.Columns.Clear();
            grdKeys.AllowUserToAddRows = false;
            grdKeys.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdKeys.AllowUserToDeleteRows = false;
            grdKeys.EditMode = DataGridViewEditMode.EditProgrammatically;
            grdKeys.MultiSelect = false;
            grdKeys.RowHeadersVisible = false;
            grdKeys.RowHeadersWidth = 25;
            grdKeys.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            grdKeys.AutoGenerateColumns = false;
            
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            
            col.Name = "Name";
            col.HeaderText = "Name";
            col.ValueType = typeof(string);
            col.Frozen = false;
            col.FillWeight = 200;
            col.Visible = true;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grdKeys.Columns.Add(col);



            col = new DataGridViewTextBoxColumn();
            col.Name = "Description";
            col.ValueType = typeof(string);
            col.Frozen = false;
            col.Visible = true;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col.FillWeight = 400;
            grdKeys.Columns.Add(col);

           */

        }
        private void init_keys_data(bool bForCombination)
        {
            /*
            add_key("[F1]"    , "Function key <F1>");
            add_key("[F2]"    , "Function key <F2>");
            add_key("[F3]"    , "Function key <F3>");
            add_key("[F4]"    , "Function key <F4>");
            add_key("[F5]"    , "Function key <F5>");
            add_key("[F6]"    , "Function key <F6>");
            add_key("[F7]"    , "Function key <F7>");
            add_key("[F8]"    , "Function key <F8>");
            add_key("[F9]"    , "Function key <F9>");
            add_key("[F10]"    , "Function key <F10>");
            add_key("[F11]"    , "Function key <F11>");
            add_key("[F12]"    , "Function key <F12>");
            add_key("[ESC]"    , "Function key <Esc>");
            add_key("[Backspace]"    , "key <Backspace>");
            add_key("[Tab]"    , "key <Tab>");
            add_key("[CapsLock]"    , "key <Caps Lock>");
            add_key("[Enter]"    , "key <Enter>");
            if (!bForCombination)
            {

                add_key("[Shift]", "key <Shift> on left");
                add_key("[RightShift]", "key <Shift> on right");
                add_key("[Ctrl]", "key <Ctrl> on left");
                add_key("[RightCtrl]", "key <Ctrl> on right");
                add_key("[Alt]", "key <Alt> on left");
                add_key("[RightAlt]", "Key <Alt> on right");
                add_key("[Win]", "key <Win> on left");
                add_key("[RightWin]", "key <Win> on right");
            
                add_key("[#RightShift]"    , "break code for key <Shift> on right");
                add_key("[#Ctrl]"    , "break code for key <Ctrl> on left");
                add_key("[#RightCtrl]"    , "break code for key <Ctrl> on right");
                add_key("[#Alt]"    , "break code for key <Alt> on left");
                add_key("[#RightAlt]"    , "break code for key <Alt> on right");
                add_key("[#Win]"    , "break code for key <Win> on left");
                add_key("[#RightWin]"    , "break code for key <Win> on right");
            }
            add_key("[Space]"    , "key <Space bar>");
            add_key("[Insert]"    , "Function key <Insert>");
            add_key("[Delete]"    , "Function key <Delete>");
            add_key("[Home]"    , "Function key <Home>");
            add_key("[End]"    , "Function key <End>");
            add_key("[PageUp]"    , "Function key <Page Up>");
            add_key("[PageDown]"    , "Function key <Page Down>");
            add_key("[ArrowUp]"    , "Function key <Up Arrow>");
            add_key("[ArrowLeft]"    , "Function key <Left Arrow>");
            add_key("[ArrowRight]"    , "Function key <Right Arrow>");
            add_key("[ArrowDown]"    , "Function key <Down Arrow>");
            add_key("[NumLock]"    , "Function key <Num Lock>");
            add_key("[ScrollLk]"    , "Function key <Scroll Lock>");
            add_key("[PrintScreen]"    , "Function key <Print Screen>");
            add_key("[PauseBreak]"    , "Function key <Pause Break>");
	
            add_key("[Menu]"    , "Function key <Menu>");
            add_key("[Ctrl+PauseBreak]"    , "Function key <Ctrl>+<Pause Break>");
            add_key("[Ctrl+PrintScreen]"    , "Function key <Ctrl>+<Print Screen>");
            add_key("[Alt+PrintScreen]"    , "Function key <Alt>+<Print Screen>");
            	
            add_key("[PAD0]"    , "number key <0> on keypad");
            add_key("[PAD1]"    , "number key <1> on keypad");
            add_key("[PAD2]"    , "number key <2> on keypad");
            add_key("[PAD3]"    , "number key <3> on keypad");
            add_key("[PAD4]"    , "number key <4> on keypad");
            add_key("[PAD5]"    , "number key <5> on keypad");
            add_key("[PAD6]"    , "number key <6> on keypad");
            add_key("[PAD7]"    , "number key <7> on keypad");
            add_key("[PAD8]"    , "number key <8> on keypad");
            add_key("[PAD9]"    , "number key <9> on keypad");
            add_key("[PAD+]"    , "number key <+> on keypad");
            add_key("[PAD-]"    , "number key <-> on keypad");
            add_key("[PAD*]"    , "number key <*> on keypad");
            add_key("[PAD/]"    , "number key </> on keypad");
            add_key("[PAD.]"    , "number key <.> on keypad");
            add_key("[PADEnter]"    , "number key <Enter> on keypad");
            	
            add_key("[NextTrack]"    , "Next Track");
            add_key("[PrevTrack]"    , "Previous Track");
            add_key("[Stop]"    , "Stop");
            add_key("[PlayPause]"    , "Play / Pause");
            add_key("[Mute]"    , "Mute");
            add_key("[VolumeUp]"    , "Volume Up");
            add_key("[VolumeDown]"    , "Volume Down");
            add_key("[Mail]","Mail");
            add_key("[Calculator]"    , "Calculator");
            add_key("[My Computer]"    , "My Computer");
            add_key("[wwwSearch]"    , "www Search");
            add_key("[wwwHome]"    , "www Home");
            add_key("[wwwBack]"    , "www Back");
            add_key("[wwwForward]"    , "www Forward");
            add_key("[wwwStop]"    , "www Stop");
            add_key("[wwwRefresh]"    , "www Refresh");
            add_key("[wwwFavorites]"    , "www Favorites");
	
            add_key("[SysPower]"    , "System Power");
            add_key("[SysSleep]"    , "System Sleep");
            add_key("[SysWake]", "System Wake");

            */


        }
        private void add_key( string name, string description)
        {
            /*
            object[] ar = {name, description};
            grdKeys.Rows.Add(ar);
             * */
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_ForCombination)
            {


                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {


                if (this.FocusedTextBox == null)
                    return;
                string s = GetSelectedKeyCode();
                if (s.Length <= 0) return;
                this.FocusedTextBox.AddKeyCode(s);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
           
            

        }
        public string GetSelectedKeyCode()
        {
            return m_strPressedKeyCode;
            /*
            int r = grdKeys.CurrentCell.RowIndex;
            if (r < 0)
                return "";
            return grdKeys.Rows[r].Cells[0].Value.ToString();
             * */
        }

        public string InputSpecialKey()
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                return GetSelectedKeyCode();

                //int r = grdKeys.CurrentCell.RowIndex;
                //if (r < 0)
                //    return "";
                //return grdKeys.Rows[r].Cells[0].Value.ToString();
            }
            return "";
        }

        private void grdKeys_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK_Click(null, null);
        }

        private void frmSpecialKey_Load(object sender, EventArgs e)
        {
            if (_ForCombination)
                btnOK.Text = "OK";
            else
                btnOK.Text = "Insert";

            show_keys(false);
            m_strPressedKeyCode = "";

        }

        //static public void show_special_list_modeless2(Form caller, KB9TextBox txtFocused)
        //{

        //    KB9TextBox t = txtFocused;
        //    if (t == null)
        //        return;

        //    frmSpecialKey frmKeyList = frmSpecialKey.Instance();
        //    //if (frmKeyList == null)
        //    //    frmKeyList = new frmSpecialKey();
        //    frmKeyList.TopMost = true;
        //    //set it initial position
        //    int dx = caller.Width;// -frmKeyList.Width;
        //    int dy = 0;//
        //    Point pt = caller.Location;
        //    pt.Offset(dx, dy);
        //    frmKeyList.Location = pt;
        //    frmKeyList.FocusedTextBox = t;
        //    if (frmKeyList.FocusedTextBox == null)
        //    {
        //        frmKeyList.Hide();
                
        //    }
        //    else
        //    {
        //        frmKeyList.Visible = false;
        //        frmKeyList.Show(caller);
        //    }
        //}

        static public void show_special_list_modeless(Form caller, KB9TextBox txtFocused)
        {

            KB9TextBox t = txtFocused;
            if (t == null)
                return;
            Control c = t.Parent;
            frmOnScreenKbd frmKeyList = frmOnScreenKbd.Instance();
            
            frmKeyList.TopMost = true;
            //set it initial position
            Point point = t.Location;
            point = c.PointToScreen(point);
            //point = caller.PointToClient(ppt);
            //point.X += t.Width;
            point.Y += t.Height;
          
            
            frmKeyList.Location = point; //form location is screen coordinate
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
                frmKeyList.Show(caller);
            }
        }

        private void frmSpecialKey_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_ForCombination)
                return;
            else
                frmForm = null;
        }
        //////////////////////////////////////////////////
        //-----------------------------TABLE for keys -----------------------
        COnScreenKbdKey[] m_Keys = new COnScreenKbdKey[]
            {
                new COnScreenKbdKey("Esc", "[ESC]", "Esc", "[ESC]"), //0
                new COnScreenKbdKey("F1", "[F1]", "F1", "[F1]"),
                new COnScreenKbdKey("F2", "[F2]", "F2", "[F2]"),
                new COnScreenKbdKey("F3", "[F3]", "F3", "[F3]"),
                new COnScreenKbdKey("F4", "[F4]", "F4", "[F4]"),

                new COnScreenKbdKey("F5", "[F5]", "F5", "[F5]"),
                new COnScreenKbdKey("F6", "[F6]", "F6", "[F6]"),
                new COnScreenKbdKey("F7", "[F7]", "F7", "[F7]"),
                new COnScreenKbdKey("F8", "[F8]", "F8", "[F8]"),
                new COnScreenKbdKey("F9", "[F9]", "F9", "[F9]"), 

                new COnScreenKbdKey("F10", "[F10]", "F10", "[F10]"),//10
                new COnScreenKbdKey("F11", "[F11]", "F11", "[F11]"),
                new COnScreenKbdKey("F12", "[F12]", "F12", "[F12]"),
                new COnScreenKbdKey("`", "`", "~", "~"),
                new COnScreenKbdKey("1", "1", "!", "!"),

                new COnScreenKbdKey("2", "2", "@", "@"),
                new COnScreenKbdKey("3", "3", "#", "#"),
                new COnScreenKbdKey("4", "4", "$", "$"),
                new COnScreenKbdKey("5", "5", "%", "%"),
                new COnScreenKbdKey("6", "6", "^", "^"),

                new COnScreenKbdKey("7", "7", "&", "&"),//20
                new COnScreenKbdKey("8", "8", "*", "*"),
                new COnScreenKbdKey("9", "9", "(", "("),
                new COnScreenKbdKey("0", "0", ")", ")"),
                new COnScreenKbdKey("-", "-", "_", "_"),

                new COnScreenKbdKey("=", "=", "+", "+"),
                new COnScreenKbdKey("\\", "\\\\", "|", "|"),
                new COnScreenKbdKey("Backspace", "[Backspace]", "Backspace", "[Backspace]"),
                new COnScreenKbdKey("Tab", "[Tab]", "Tab", "[Tab]"),
                new COnScreenKbdKey("q", "q", "Q", "Q"),

                new COnScreenKbdKey("w", "w", "W", "W"),//30
                new COnScreenKbdKey("e", "e", "E", "E"),
                new COnScreenKbdKey("r", "r", "R", "R"),
                new COnScreenKbdKey("t", "t", "T", "T"),
                new COnScreenKbdKey("y", "y", "Y", "Y"),

                new COnScreenKbdKey("u", "u", "U", "U"),
                new COnScreenKbdKey("i", "i", "I", "I"),
                new COnScreenKbdKey("o", "o", "O", "O"),
                new COnScreenKbdKey("p", "p", "P", "P"),
                new COnScreenKbdKey("[", "\\[", "{", "{"),//

                new COnScreenKbdKey("]", "\\]", "}", "}"),//40
                new COnScreenKbdKey("Enter", "[Enter]", "Enter", "[Enter]"),
                new COnScreenKbdKey("Caps Lock", "[CapsLock]", "Caps Lock", "[CapsLock]"),
                new COnScreenKbdKey("a", "a", "A", "A"),
                new COnScreenKbdKey("s", "s", "S", "S"),

                new COnScreenKbdKey("d", "d", "D", "D"),
                new COnScreenKbdKey("f", "f", "F", "F"),
                new COnScreenKbdKey("g", "g", "G", "G"),
                new COnScreenKbdKey("h", "h", "H", "H"),
                new COnScreenKbdKey("j", "j", "J", "J"),

                new COnScreenKbdKey("k", "k", "K", "K"),//50
                new COnScreenKbdKey("l", "l", "L", "L"),
                new COnScreenKbdKey(";", ";", ":", ":"),
                new COnScreenKbdKey("'", "'", "\"", "\""),
                new COnScreenKbdKey("Shift", "[Shift]", "Shift", "[Shift]", "[#Shift]"),

                new COnScreenKbdKey("z", "z", "Z", "Z"),
                new COnScreenKbdKey("x", "x", "X", "X"),
                new COnScreenKbdKey("c", "c", "C", "C"),
                new COnScreenKbdKey("v", "v", "V", "V"),
                new COnScreenKbdKey("b", "b", "B", "B"),

                new COnScreenKbdKey("n", "n", "N", "N"),//60
                new COnScreenKbdKey("m", "m", "M", "M"),
                new COnScreenKbdKey(",", ",", "<", "<"),
                new COnScreenKbdKey(".", ".", ">", ">"),
                new COnScreenKbdKey("/", "/", "?", "?"),

                new COnScreenKbdKey("Shift", "[RightShift]", "Shift", "[RightShift]", "[#RightShift]"),
                new COnScreenKbdKey("Ctrl", "[Ctrl]", "Ctrl", "[Ctrl]", "[#Ctrl]"),
                new COnScreenKbdKey("Win", "[Win]", "Win", "[Win]", "[#Win]"),
                new COnScreenKbdKey("Alt", "[Alt]", "Alt", "[Alt]", "[#Alt]"),
                new COnScreenKbdKey("Space", "[Space]", "Space", "[Space]"),

                new COnScreenKbdKey("Alt", "[RightAlt]", "Alt", "[RightAlt]", "[#RightAlt]"),//70
                new COnScreenKbdKey("Win", "[RightWin]", "Win", "[RightWin]", "[#RightWin]"),
                new COnScreenKbdKey("Menu", "[Menu]", "Menu", "[Menu]"),
                new COnScreenKbdKey("Ctrl", "[RightCtrl]", "Ctrl", "[RightCtrl]", "[#RightCtrl]"),
                new COnScreenKbdKey("Sleep", "[SysSleep]", "Sleep", "[SysSleep]"),

                new COnScreenKbdKey("WakeUp", "[SysWake]", "WakeUp", "[SysWake]"),
                new COnScreenKbdKey("Power", "[SysPower]", "Power", "[SysPower]"),
                new COnScreenKbdKey("PrnSc", "[PrintScreen]", "PrnSc", "[PrintScreen]"),
                new COnScreenKbdKey("ScrLk", "[ScrollLk]", "ScrLk", "[ScrollLk]"),
                new COnScreenKbdKey("Pause", "[PauseBreak]", "Pause", "[PauseBreak]"),

                new COnScreenKbdKey("Ins", "[Insert]", "Ins", "[Insert]"),//80
                new COnScreenKbdKey("Home", "[Home]", "Home", "[Home]"),
                new COnScreenKbdKey("PgUp", "[PageUp]", "PgUp", "[PageUp]"),
                new COnScreenKbdKey("Del", "[Delete]", "Del", "[Delete]"),
                new COnScreenKbdKey("End", "[End]", "End", "[End]"),

                new COnScreenKbdKey("PgDn", "[PageDown]", "PgDn", "[PageDown]"),
                new COnScreenKbdKey("¡ü", "[ArrowUp]", "¡ü", "[ArrowUp]"),
                new COnScreenKbdKey("¡û", "[ArrowLeft]", "¡û", "[ArrowLeft]"),
                new COnScreenKbdKey("¡ý", "[ArrowDown]", "¡ý", "[ArrowDown]"),
                new COnScreenKbdKey("¡ú", "[ArrowRight]", "¡ú", "[ArrowRight]"),

                new COnScreenKbdKey("Num", "[NumLock]", "Num", "[NumLock]"),//90
                new COnScreenKbdKey("/", "[PAD/]", "/", "[PAD/]"),
                new COnScreenKbdKey("*", "[PAD*]", "*", "[PAD*]"),
                new COnScreenKbdKey("-", "[PAD-]", "-", "[PAD-]"),
                new COnScreenKbdKey("7", "[PAD7]", "7", "[PAD7]"),

                new COnScreenKbdKey("8", "[PAD8]", "8", "[PAD8]"),
                new COnScreenKbdKey("9", "[PAD9]", "9", "[PAD9]"),
                new COnScreenKbdKey("+", "[PAD+]", "+", "[PAD+]"),
                new COnScreenKbdKey("4", "[PAD4]", "4", "[PAD4]"),
                new COnScreenKbdKey("5", "[PAD5]", "5", "[PAD5]"),

                new COnScreenKbdKey("6", "[PAD6]", "6", "[PAD6]"),//100
                new COnScreenKbdKey("1", "[PAD1]", "1", "[PAD1]"),
                new COnScreenKbdKey("2", "[PAD2]", "2", "[PAD2]"),
                new COnScreenKbdKey("3", "[PAD3]", "3", "[PAD3]"),
                new COnScreenKbdKey("Enter", KB9Const.PAD_Enter, "Enter", KB9Const.PAD_Enter),

                new COnScreenKbdKey("0", "[PAD0]", "0", "[PAD0]"),
                new COnScreenKbdKey(".", "[PAD.]", ".", "[PAD.]"), //106

                new COnScreenKbdKey("", "[PrevTrack]", "", "[PrevTrack]"),//107
                new COnScreenKbdKey("", "[PlayPause]", "", "[PlayPause]"),
                new COnScreenKbdKey("", "[Stop]", "", "[Stop]"),

                new COnScreenKbdKey("", "[NextTrack]", "", "[NextTrack]"),//110
                new COnScreenKbdKey("", "[VolumeUp]", "", "[VolumeUp]"),
                new COnScreenKbdKey("", "[VolumeDown]", "", "[VolumeDown]"),
                new COnScreenKbdKey("", "[Mute]", "", "[Mute]"),
                new COnScreenKbdKey("", "[wwwSearch]", "", "[wwwSearch]"),

                new COnScreenKbdKey("", "[wwwStop]", "", "[wwwStop]"),
                new COnScreenKbdKey("", "[wwwHome]", "", "[wwwHome]"),
                new COnScreenKbdKey("", "[wwwBack]", "", "[wwwBack]"),
                new COnScreenKbdKey("", "[wwwForward]", "", "[wwwForward]"),
                new COnScreenKbdKey("", "[wwwFavorites]", "", "[wwwFavorites]"),

                new COnScreenKbdKey("", "[wwwRefresh]", "", "[wwwRefresh]"),//120
                new COnScreenKbdKey("", "[Mail]", "", "[Mail]"),
                new COnScreenKbdKey("", "[Calculator]", "", "[Calculator]"),
                new COnScreenKbdKey("", "[MyComputer]", "", "[MyComputer]"),//123


               

            };

        private CKbdKey shift_is_down()
        {
            //if (btnKey54.BackColor == KeyDownColor)
            //    return btnKey54;
            //else if ( btnKey65.BackColor == KeyDownColor)
            //    return btnKey65;
            if (btnKey124.BackColor == KeyDownColor)
                return btnKey123;
            return null;
        }

        private CKbdKey alt_is_down()
        {
            if (btnKey68.BackColor == KeyDownColor)
                return btnKey68;
            else if (btnKey70.BackColor == KeyDownColor)
                return btnKey70;

            return null;
        }

        private CKbdKey ctrl_is_down()
        {
            if (btnKey66.BackColor == KeyDownColor)
                return btnKey66;
            else if (btnKey73.BackColor == KeyDownColor)
                return btnKey73;

            return null;
        }

        private CKbdKey win_is_down()
        {
            if (btnKey67.BackColor == KeyDownColor)
                return btnKey67;
            else if (btnKey71.BackColor == KeyDownColor)
                return btnKey71;

            return null;
        }
        /************************************************************************/
        /* 
         * return the key code .
         */
        /************************************************************************/
        private string DoOnScreenKeyDownUp(CKbdKey btn, COnScreenKbdKey key)
        {
            CKbdKey btnDown = null;
            bool bWithUpCode = false;
            if (key.m_strCode == "[Shift]" ||
                key.m_strCode == "[RightShift]")
            {//shift key
                btnDown = shift_is_down();
                show_keys(btnDown == null); //toggle
                bWithUpCode = true;
            }
            else if (key.m_strCode == "[Alt]" ||
              key.m_strCode == "[RightAlt]")
            { //alt key
                btnDown = alt_is_down();
                bWithUpCode = true;
            }
            else if (key.m_strCode == "[Ctrl]" ||
               key.m_strCode == "[RightCtrl]")
            { //alt key
                btnDown = ctrl_is_down();
                bWithUpCode = true;
            }
            else if (key.m_strCode == "[Win]" ||
               key.m_strCode == "[RightWin]")
            { //alt key
                btnDown = win_is_down();
                bWithUpCode = true;
            }

            if (!bWithUpCode)
            {
                
                return "";
            }

            if (btnDown != null) //it is down
            {//toggle it, now it should been up
                btnDown.BackColor = KeyUpColor;
                btnDown.ForeColor = btnKey0.ForeColor;
                btn.BackColor = KeyUpColor;
                btn.ForeColor = btnKey0.ForeColor;
                return ((COnScreenKbdKey)btnDown.Tag).m_strUpCode;
            }
            else
            {
                btn.BackColor = KeyDownColor;
                return key.m_strShiftCode;
            }

          
              

           
        }

        /************************************************************************/
        /* 
         * return the key code .
         */
        /************************************************************************/
        private bool IsShiftKeyUpClick(CKbdKey btn, COnScreenKbdKey key)
        {
            CKbdKey btnDown = null;
            bool bWithUpCode = false;
            if (key.m_strCode == "[Shift]" ||
                key.m_strCode == "[RightShift]")
            {//shift key
                btnDown = shift_is_down();
                bWithUpCode = true;
                show_keys(btnDown == null); //toggle
                //return (btnDown != null); //toggle
                
            }
           

            if (!bWithUpCode)
                return false;
            

            if (btnDown != null) //it is down
            {//toggle it, now it should been up
                btnDown.BackColor = KeyUpColor;
                btnDown.ForeColor = btnKey0.ForeColor;
                btn.BackColor = KeyUpColor;
                btn.ForeColor = btnKey0.ForeColor;
                return true;
                
            }
            else
            {
                btn.BackColor = KeyDownColor;
                return false;
            }





        }



        Color KeyDownColor = Color.Brown;
        Color KeyUpColor = Color.SteelBlue;
        //private Color KeyUpColor
        //{
        //    get 
        //    {
        //        return btnKey0.BackColor;
        //    }
        //}
        private void reset_all_function_buttons()
        {
            CKbdKey[] ar = new CKbdKey[]{
                btnKey54,
                btnKey66,
                btnKey67,
                btnKey68,
                btnKey65,
                btnKey70,
                btnKey71,
                btnKey73,
                

            };
            for (int i=0; i< ar.Length; i++)
            {
                ar[i].BackColor = btnKey0.BackColor;
            }
        }

        private void append_function_key_up()
        {
            if ( key_is_down(btnKey54) ) //shift
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.SHIFT_UP);
            }
            if ( key_is_down(btnKey66) ) //ctrl
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.CTRL_UP);
            }
            if ( key_is_down(btnKey67) ) //win
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.WIN_UP);
            }
            if (key_is_down(btnKey68)) //alt
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.ALT_UP);
            }
            if (key_is_down(btnKey65)) //rshift
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.RSHIFT_UP);
            }
            if (key_is_down(btnKey70)) //ralt
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.RALT_UP);
            }
            if (key_is_down(btnKey71)) //rwin
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.RWIN_UP);
            }
            if (key_is_down(btnKey73)) //rctrl
            {
                this.FocusedTextBox.AddKeyCode(KB9Const.RCTRL_UP);
            }
        }

        private bool key_is_down(CKbdKey button)
        {
            return (button.BackColor == KeyDownColor);
                 
        }
        private bool handle_function_keys(CKbdKey button, COnScreenKbdKey key)
        {
            if (key.m_strCode == KB9Const.CTRL_DOWN ||
                key.m_strCode == KB9Const.RCTRL_DOWN ||
                key.m_strCode == KB9Const.SHIFT_DOWN ||
                key.m_strCode == KB9Const.RSHIFT_DOWN ||
                key.m_strCode == KB9Const.ALT_DOWN ||
                key.m_strCode == KB9Const.RALT_DOWN ||
                key.m_strCode == KB9Const.WIN_DOWN ||
                key.m_strCode == KB9Const.RWIN_DOWN)
            {

                if (shift_is_down() != null)
                    m_strPressedKeyCode = key.m_strShiftCode;
                else
                    m_strPressedKeyCode = key.m_strCode;

                if (_ForCombination)
                {//in combination, don't allow keyup code.
                    return true; //the function keys is not allowed in combination
                  //  this.DialogResult = DialogResult.OK;
                   // this.Close();
                }
                else
                {

                    if (this.FocusedTextBox == null)
                        return false;
                    string s = GetSelectedKeyCode();
                    if (s.Length <= 0) return false;

                    if (!key_is_down(button))
                        this.FocusedTextBox.AddKeyCode(key.m_strCode);
                    else
                        this.FocusedTextBox.AddKeyCode(key.m_strUpCode);
                    /*
                    if (key.m_strUpCode.Length >0)
                    //if (strKeyWithUp.Length >0)
                        this.FocusedTextBox.AddKeyCode(key.m_strUpCode);
                     * */
                }
                if (button.BackColor == KeyDownColor)
                    button.BackColor = btnKey0.BackColor;
                else
                    button.BackColor = KeyDownColor;

                return true;
            }
            else //normal key pressed
            {

                return false;
            }
        }


        private string m_strPressedKeyCode = "";
        private void OnKeyClicked(object sender, EventArgs e)
        {
            
            CKbdKey btn = (CKbdKey)sender;
            
            COnScreenKbdKey key = (COnScreenKbdKey)btn.Tag;
            if (handle_function_keys(btn, key))
                return;
            //if (IsShiftKeyUpClick(btn, key))
            //{
            //    //show_keys(false);
            //    return;
            //}

            if (shift_is_down() != null)
                m_strPressedKeyCode = key.m_strShiftCode;
            else
                m_strPressedKeyCode = key.m_strCode;
            //some keys with keyup code
            //string strKeyWithUp = DoOnScreenKeyDownUp(btn, key);


            if (_ForCombination)
            {//in combination, don't allow keyup code.
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                //for normal input, allow keyup code.
                //if (strKeyWithUp.Length > 0)
                //    m_strPressedKeyCode = strKeyWithUp;

                if (this.FocusedTextBox == null)
                    return;
                string s = GetSelectedKeyCode();
                if (s.Length <= 0) return;
                //this.FocusedTextBox.fix_function_keys_up(null);

                this.FocusedTextBox.AddKeyCode(s);
                /*
                if (key.m_strUpCode.Length >0)
                //if (strKeyWithUp.Length >0)
                    this.FocusedTextBox.AddKeyCode(key.m_strUpCode);
                 * */
                append_function_key_up();
                reset_all_function_buttons();
            }

        }
        /// <summary>
        /// it is old version keyclick event, just for backup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyClickedOld(object sender, EventArgs e)
        {
            CKbdKey btn = (CKbdKey)sender;


            COnScreenKbdKey key = (COnScreenKbdKey)btn.Tag;

            
            if (shift_is_down() != null)
                m_strPressedKeyCode = key.m_strShiftCode;
            else
                m_strPressedKeyCode = key.m_strCode;
            //some keys with keyup code
            string strKeyWithUp = DoOnScreenKeyDownUp(btn, key);


            if (_ForCombination)
            {//in combination, don't allow keyup code.

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                //for normal input, allow keyup code.
                if (strKeyWithUp.Length > 0)
                    m_strPressedKeyCode = strKeyWithUp;

                if (this.FocusedTextBox == null)
                    return;
                string s = GetSelectedKeyCode();
                if (s.Length <= 0) return;
                this.FocusedTextBox.AddKeyCode(s);
                
            }

        }

       private void show_keys(bool bShift)
       {
           int ncount = m_Keys.Length;
           for (int i=0; i< ncount; i++)
           {
               string name = string.Format("btnKey{0}", i);
               CKbdKey btn = GetButton(name);
               if (btn != null)
               {
                   if (!bShift)
                   {


                       btn.Text = m_Keys[i].m_strText;
                       //btn.Tag = m_Keys[i].m_strCode;
                   }
                   else
                   {
                       btn.Text = m_Keys[i].m_strShiftText;
                       //btn.Tag = m_Keys[i].m_strShiftCode;
                   }
                   btn.Tag = m_Keys[i];
                   //disable Ctrl , Shift, Alt and Win when edit combination
                   if (this._ForCombination)
                   {
                       if (btn.Text.Equals("Ctrl") ||
                           btn.Text.Equals("Alt") ||
                           btn.Text.Equals("Shift") ||
                           btn.Text.Equals("Win"))
                           btn.Enabled = false;
                   }
                   btn.Refresh();
               }
           }
       }

        private CKbdKey GetButton(string name)
        {
            return (CKbdKey)this.Controls.Find(name, true)[0];
            
        }

        private void reverse_key_forecolor(CKbdKey key)
        {
            if (key != null)
            {
                if (key.ForeColor == key.BackColor)
                {
                    key.ForeColor = btnKey0.ForeColor;
                }
                else
                {
                    key.ForeColor = key.BackColor;
                }
                key.Refresh();
            }
        }
        private void timerBlink_Tick(object sender, EventArgs e)
        {
            reverse_key_forecolor( shift_is_down());
            reverse_key_forecolor(ctrl_is_down());
            reverse_key_forecolor(alt_is_down());
            reverse_key_forecolor(win_is_down());
            
        }

        private void btnKey124_Click(object sender, EventArgs e)
        {
            CKbdKey btnDown = null;
            btnDown = btnKey124;
            if (shift_is_down()!= null)
            {//toggle it, now it should been up
                btnDown.BackColor = KeyUpColor;
                btnDown.ForeColor = btnKey0.ForeColor;
                show_keys(false);
            }
            else
            {
                btnDown.BackColor = KeyDownColor;
                show_keys(true);
            }
           

        }

        private void btnKey125_Click(object sender, EventArgs e)
        {
            
        }

        private void btnKey125_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.FocusedTextBox != null)
                this.FocusedTextBox.deleteCellBeforeCaret();
        }

   }
}
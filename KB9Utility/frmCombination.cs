using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmCombination : Form
    {
        public frmCombination()
        {
            InitializeComponent();
            this.txtKey0.SingleKey = true;
            this.txtKey1.SingleKey = true;

            this.txtKey0.LowerKey = true;
            this.txtKey1.LowerKey = true;
            this.txtKey2.LowerKey = true;
            

            //tsbScan.Image = Util.get_image("number");
            tsbSpecial.Image = Util.get_image("specialkey");

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
        private KB9TextBox GetFocusedTextBox()
        {
            KB9TextBox[] ar = new KB9TextBox[]
            {
                
                txtKey0,
                txtKey1,
                txtKey2
                
            };

            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i].Visible &&
                    ar[i].Focused)
                    return ar[i];
            }
            return null;
        }

        private void tsbSpecial_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            frmOnScreenKbd frm = new frmOnScreenKbd(true);
            frm.StartPosition = FormStartPosition.CenterParent;
            string s = frm.InputSpecialKey();
           // s = s.Replace("[", "");
           // s = s.Replace("]", "");
            if (s.Length <= 0) return;
            t.AddKeyCode(s, false);

        }

        private void tsbScan_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetFocusedTextBox();
            frmAscii frm = new frmAscii();
            string s = frm.InputAscii();
            if (s.Length <= 0) return;
            t.AddKeyCode(s, true);
        }

        public string InputCombinationKeys()
        {
            if (this.ShowDialog() != DialogResult.OK)
                return string.Empty;
            string s = string.Empty;
            s = remove_key_symbol(txtKey0.Text);
            if (txtKey1.Text != string.Empty)
            {
                if (s != string.Empty)
                    s += "+";
                s += remove_key_symbol(txtKey1.Text);
            }

            if (txtKey2.Text != string.Empty)
            {
                if (s != string.Empty)
                    s += "+";
                s += remove_key_symbol(txtKey2.Text);
            }
            return s;
        }

        private string remove_key_symbol(string strKey)
        {
            string s = strKey;
            if (s.IndexOf('[') >=0 && s.IndexOf(']') >=0)
            {
                s = s.Replace("[", "");
                s = s.Replace("]", "");
            }
            return s;
        }

        private void enable_menu(bool benable)
        {
            tsbSpecial.Enabled = benable;
        }

        private void txtKey0_Enter(object sender, EventArgs e)
        {
            enable_menu(true);
        }

        private void txtKey0_Leave(object sender, EventArgs e)
        {
            enable_menu(false);
        }
    }
}
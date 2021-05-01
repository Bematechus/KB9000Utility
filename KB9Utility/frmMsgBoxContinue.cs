using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmMsgBoxContinue : Form
    {
        public string _Title;
        public string _Msg;
        public string _StrYes;
        public string _StrNo;
        
        public MessageBoxIcon _IconMsg;

        public frmMsgBoxContinue()
        {
            InitializeComponent();
            picIcon.Image = SystemIcons.Question.ToBitmap();
        }

        static public DialogResult MsgBox(string title, string msg, string strYes, string strNo, MessageBoxIcon iconMsg)
        {
            frmMsgBoxContinue frm = new frmMsgBoxContinue();
            frm._Title = title;
            frm._Msg = msg;
            frm._StrYes = strYes;
            frm._StrNo = strNo;
            
            frm._IconMsg = iconMsg;
            return frm.ShowDialog();
        }

        private void frmMsgbox_Load(object sender, EventArgs e)
        {
            this.Text = _Title;
            this.lblMsg.Text = _Msg;
            btnYes.Text = _StrYes;
            btnNo.Text = _StrNo;
            //btnCancel.Text = _StrCancel;

        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }
    }
}
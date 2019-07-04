using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmDataWrite : Form
    {

        private string _Data;
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public frmDataWrite()
        {
            InitializeComponent();
        }



        private void frmDataWrite_Load(object sender, EventArgs e)
        {
            txtData.Text = this.Data;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        static public DialogResult show_data(string strData)
        {
            frmDataWrite frm = new frmDataWrite();
            frm.Data = strData;
            return frm.ShowDialog();
        }
    }
}

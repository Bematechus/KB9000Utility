using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmConfirmWrite : Form
    {
        public frmConfirmWrite()
        {
            InitializeComponent();
            picIcon.Image = SystemIcons.Question.ToBitmap();
        }

        private void frmConfirmWrite_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Hide();
        }
        static public System.Windows.Forms.DialogResult confirmWrite()
        {
            frmConfirmWrite frm = new frmConfirmWrite();
            System.Windows.Forms.DialogResult result = frm.ShowDialog();
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmKeyCodeError : Form
    {
        public frmKeyCodeError()
        {
            InitializeComponent();
            picIcon.Image = SystemIcons.Warning.ToBitmap();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Hide();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Hide();
        }
        static public DialogResult ConfirmError()
        {
            frmKeyCodeError frm = new frmKeyCodeError();
            return frm.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmTestKB : Form
    {
        public frmTestKB()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtText_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frmTestKB_KeyDown(object sender, KeyEventArgs e)
        {
            //lblText.Text += e.KeyCode.ToString();
        }

        private void frmTestKB_Load(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtText.Text = "";
            txtText.Focus();
        }
    }
}
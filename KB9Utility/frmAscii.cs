using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmAscii : Form
    {
        public frmAscii()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public string InputAscii()
        {
            
            numVal.Value = 1;
            if (this.ShowDialog() == DialogResult.OK)
            {
                int n =(int) numVal.Value;
                return n.ToString("d3");
            }
            else
                return "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmInputStructure : Form
    {
        private DEditor m_editor = null;
        public frmInputStructure(DEditor editor)
        {
            m_editor = editor;
            InitializeComponent();
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

        public string InputDataStructure()
        {
            if (this.ShowDialog() == DialogResult.Cancel)
                return "";

            string s = txtData.Text;
            s = s.Replace("<0x0d>", "\r");
            s = s.Replace("<0x0a>", "\n");

            return s;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtData.Text = m_editor.CreateCVS();
        }
    }
}
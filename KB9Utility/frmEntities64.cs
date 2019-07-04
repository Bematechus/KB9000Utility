using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmEntities64 : Form
    {

        public enum Fix_Type
        {
            Auto=0,
            Custom,

        }

        private Fix_Type _FixType = Fix_Type.Auto;
        public Fix_Type FixType
        {
            get
            {
                return _FixType;
            }
            set
            {
                _FixType = value;
            }
        }
        private bool _ForAddNew = false;
        public frmEntities64()
        {
            InitializeComponent();
            picIcon.Image = SystemIcons.Warning.ToBitmap();
            _ForAddNew = false;
        }

        public frmEntities64(bool bWillAddNew)
        {
            InitializeComponent();
            picIcon.Image = SystemIcons.Warning.ToBitmap();
            _ForAddNew = bWillAddNew;
        }

        private void frmEntities64_Load(object sender, EventArgs e)
        {
            if (_ForAddNew)
            {
                lblText.Text = "Only 64 keys are allowed in the template.";
                rbAuto.Enabled = false;
                rbCustom.Enabled = false;
            }
            else
            {
                lblText.Text = "Only 64 keys are allowed in the template.\nThere are more than 64 keys in the template!";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbAuto.Checked)
            {
                this.FixType = Fix_Type.Auto;
            }
            else
                this.FixType = Fix_Type.Custom;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
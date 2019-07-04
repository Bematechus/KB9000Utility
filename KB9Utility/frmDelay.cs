using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmDelay : Form
    {
        public frmDelay()
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
            if (!isInputValid())
                return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public Boolean isInputValid()
        {
            //decimal d;
            //decimal.Round(
            //numSec.Update();
            string s = getInputString();// decimal.Round(numSec.Value).ToString();
            //s += ".";
           // s += numDotSec.Value.ToString();
            try
            {
                decimal d =  decimal.Parse(s);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private string getInputString()
        {
            string s = decimal.Round(numSec.Value).ToString();
            s += ".";
            s += decimal.Round(numDotSec.Value).ToString();
            return s;
        }

        public decimal InputDelay()
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                //string s = numSec.Value.ToString();
                string s = getInputString();// decimal.Round(numSec.Value).ToString();
                //s += ".";
                //s += numDotSec.Value.ToString();
                //s += ".";
                //s += numDotSec.Value.ToString();
                try
                {
                    return decimal.Parse(s);
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
            else
                return 0;
        }

        static public string makePauseString(decimal delay)
        {
            return "Pause" + delay.ToString("f1");

        }
    }
}
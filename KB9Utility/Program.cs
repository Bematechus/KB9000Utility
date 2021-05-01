using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KB9Utility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static frmMain g_frmMain = null;
        [STAThread]
        static void Main()
        {

            //20140126
            bool runone;
            System.Threading.Mutex run = new System.Threading.Mutex(true, "kb9000_utility", out runone);
            if (runone)
            {


                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                g_frmMain = new frmMain();
                //Application.Run(new frmMain());
                Application.Run(g_frmMain);
            }
            else
            {
                MessageBox.Show("The KB9000 utility is running!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        static public frmMain MainForm
        {
            get 
            {
                return g_frmMain;
            }
        }
    }
}
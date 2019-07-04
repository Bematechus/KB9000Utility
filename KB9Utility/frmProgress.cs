
//#define _DEBUG_ME

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;



namespace KB9Utility
{
    public partial class frmProgress : Form
    {
      

        public enum OperationsType
        {
            Unknown = 0,
            Detect ,
            Read,
            Write
        }
        private const int MIN_PROGRESS = 0;
        private const int MAX_PROGRESS = 100;

        //save the data read from kb9
        private string m_strData = "";
        private KB9API.KB9API_ERROR m_error;
        public KB9API.KB9API_ERROR KB9Error
        {
            get 
            {
                return m_error;
            }
            set
            {
                m_error = value;
            }
        }

        private OperationsType m_operationType = OperationsType.Unknown;
        public frmProgress()
        {
            InitializeComponent();
            pbProgress.Minimum = MIN_PROGRESS;
            pbProgress.Maximum = MAX_PROGRESS;

        }
#if _DEBUG_ME
        int m_val=0;

#endif
        private void timerOperation_Tick(object sender, EventArgs e)
        {
            int nval = 0;
            if (pbProgress.Value >= pbProgress.Maximum)
            {
                nval = MAX_PROGRESS;
            }
            else
            {


                switch (m_operationType)
                {
                    case OperationsType.Read:
                        {
                            nval = KB9API.ReadingKB9Progress();
                        }
                        break;
                    case OperationsType.Write:
                        {
                            nval = KB9API.WritingKB9Progress();
                        }
                        break;
                    default:
                        break;
                }
            }
#if _DEBUG_ME
            m_val+=15;
            nval = m_val;

#endif
            set_value( nval);

        }
        DateTime m_lastReceiveNewValue = DateTime.Now;

        private void set_value(int nvalue)
        {
            if (pbProgress.Value >= pbProgress.Maximum)
                finished_progress();

            if (pbProgress.Value != nvalue)
                m_lastReceiveNewValue = DateTime.Now;

            if (nvalue <= pbProgress.Maximum &&
                nvalue >= pbProgress.Minimum)
                pbProgress.Value = nvalue;
            if (nvalue >= pbProgress.Maximum)
                pbProgress.Value = pbProgress.Maximum;

            //if (nvalue >= pbProgress.Maximum)
            //    finished_progress();
            else if (nvalue < 0) //error
            {
                this.KB9Error = (KB9API.KB9API_ERROR)nvalue;
                finished_progress();
                return;
            }
            //check timeout
            DateTime dt = DateTime.Now;
            TimeSpan span = dt - m_lastReceiveNewValue;
            if (span.TotalSeconds > 5)
            {
                this.KB9Error = KB9API.KB9API_ERROR.FUNC_UNKOWN_ERROR;
                finished_progress();
                return;
            }

           

        }
        private void finished_progress()
        {
            timerOperation.Enabled = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void lkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (m_operationType == OperationsType.Write)
            {
                DialogResult r =  frmMsgBoxContinue.MsgBox("Confirm", "All template data in keyboard will erased.  Do you want to continue?", "Continue Download", "Cancel Download", MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                    return;


                //MessageBox.Show("All template data in keyboard will erased.  Do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            }


            //stop_operation();
            cancel_kb9000();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private static bool FindKB9000()
        {
            try
            {
                

                KB9API.KB9_PORT nPort = KB9API.KB9_PORT.Unknown;

                KB9API.KB9API_ERROR result = KB9API.AutoDetectKB9(ref nPort);

                if (result == KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
                    KB9API.m_PortType = nPort;
                else
                    KB9API.m_PortType = KB9API.KB9_PORT.Unknown;

                return (KB9API.m_PortType != KB9API.KB9_PORT.Unknown);
            }
            catch (System.Exception ex)
            {
                return false;
            }
            

        }
        
        /// <summary>
        /// API for read KB9000.
        /// Usage:
        ///     Call this function from frmMain window
        /// </summary>
        /// <param name="strTemplate">the template data readed from KB9000.</param>
        public KB9API.KB9API_ERROR read_kb9000(ref string strData)
        {

            try
            {
#if _DEBUG_ME
#else
                if (!FindKB9000())
                    return KB9API.KB9API_ERROR.FUNC_NODEVICE_DRIVER;
#endif
                this.KB9Error = KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;
                //start_operation(_ThreadParam);
                m_operationType = OperationsType.Read;
                m_strData = "";
                KB9API.StartReadingKB9(KB9API.m_PortType);

                DialogResult result =  show_progress("Reading KB9000 ..."); //blocked
#if _DEBUG_ME
                return KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;
#else

                if (result == DialogResult.Cancel)
                {
                    return KB9API.KB9API_ERROR.FUNC_UNKOWN_ERROR;
                }
                else
                {
                    if (this.KB9Error != KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
                    {
                        return this.KB9Error;
                    }
                    else
                    {


                        KB9API.ReadKB9Data(ref m_strData);
                        strData = m_strData;
                        return KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;
                    }
                }
#endif
            }
            catch (System.Exception ex)
            {
                return KB9API.KB9API_ERROR.FUNC_UNHANDLED_EXCEPTION;
            }
            
        }
        /// <summary>
        /// API for write template data to KB9000.
        /// Usage:
        ///     Call it in frmMain window
        /// </summary>
        /// <param name="strTemplate">the data structure to write to kb9k</param>
        public KB9API.KB9API_ERROR write_kb9000(string strTemplate)
        {

            try
            {
                
 #if _DEBUG_ME
#else               
                if (!FindKB9000())
                    return KB9API.KB9API_ERROR.FUNC_NODEVICE_DRIVER;
#endif
                this.KB9Error = KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;
                m_operationType = OperationsType.Write;
                KB9API.StartWriting(KB9API.m_PortType, strTemplate);

                DialogResult result = show_progress("Writing KB9000 ...");
#if _DEBUG_ME
                return KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;
#endif
                if (result == DialogResult.Cancel)
                {
                    return KB9API.KB9API_ERROR.FUNC_UNKOWN_ERROR;
                }
                else
                {
                    if (this.KB9Error != KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
                    {
                        return this.KB9Error;
                    }
                    else
                    {
                        //uint nwriten = KB9API.GetWritingResult();
                        //MessageBox.Show("Writen len = " + nwriten.ToString());
                        return KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;
                    }
                }
            }
            catch (System.Exception ex)
            {
                return KB9API.KB9API_ERROR.FUNC_UNHANDLED_EXCEPTION;
            }
        }
      
        /// <summary>
        /// Sync function, waiting until return
        /// API for cancel current kb9000 operations
        /// </summary>
        /// <returns></returns>
        public bool cancel_kb9000()
        {
            try
            {
                this.KB9Error = KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;
                return (KB9API.Cancel() != KB9API.KB9API_ERROR.FUNC_SUCCESSFUL);
            
            }
            catch (System.Exception ex)
            {
                return false;
            }
                

            
            
        }
       
        public DialogResult show_progress(string title)
        {
            this.Text = title;
            return this.ShowDialog();
        }
       
        private void frmProgress_Load(object sender, EventArgs e)
        {
            timerOperation.Enabled = true; //for test
            timerOperation_Tick(sender, e);
        }

        private void frmProgress_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerOperation.Enabled = false;
        }
    }
}
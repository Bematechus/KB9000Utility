using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace KB9Utility
{
    /// <summary>
    /// The KB9000 DLL API interface
    /// </summary>
    public class KB9API
    {
        public enum KB9_PORT
        {
            Unknown = 0,
            USB,
            PS2,
        }
        public enum KB9API_ERROR
        {

            
            // return codes. ( 0 -- 100: progress steps)
             
             FUNC_SUCCESSFUL	    =		0,
             FUNC_OPENPORT_FAIL	    =		-1,// Can't connect to port of KB9000 USB/PS2 driver
             FUNC_GET_ID_FAIL		=		-2,// Can't access keyboard ID data
             FUNC_WRONG_ID_FAIL		=		-3,// Wrong ID data gotton
             FUNC_DEVICEIO_FAIL		=		-4,// Can't access the package data
             FUNC_PACKET_FAIL		=		-5,// Wrong package data gotton
             FUNC_LRC_CHECK_FAIL	=		-6,// data packet LRC failed
             FUNC_NOMEMORY_ERROR	=		-7,// can't apply PC memory
             FUNC_THREAD_ERROR		=		-8,// can't create a thread
             FUNC_WRONG_INTERFACE	=		-9,// wrong interface number
             FUNC_ACTIOJN_NOFINISH	=		-10,// action doesn't finished
             FUNC_NOACTION_EXECUTE	=		-11,// no action executed
             FUNC_NODEVICE_DRIVER	=		-12,// No device driver detected
             FUNC_STOP_FAIL			=		-13,// isn't stop the action executed
             FUNC_BUFFER_SMALL		=		-14,// too small buffer to hold all data.
             FUNC_UNKOWN_ERROR      =       -15,
             FUNC_UNHANDLED_EXCEPTION =     -16, //for unhandled exceptions

            //////////////////////////////////////////////////////////////////
             LINE_0LENGTH_WRONG		=		-100,// zero length of key codes in one line
             DATA_CODE_WRONG		=		-101,// wrong code in utility data packet
             LINE_LENGTH_WRONG		=		-102,// too many key codes in one line
             TOTAL_KEYNUMBER_WRONG	=		-103,// more than 64 key defined

             TOTAL_PROPERTYLINE_WRONG=		-104,// error in property line
             TOTAL_KEYSIZELINE_WRONG=		-105,// error in key's size of key line
             TOTAL_BEEPPROP_WRONG	=		-106,// error in key's beep property of key line
             KEY_CONTENT_WRONG		=		-107,// error in key's content
	         LINE_BKTPAIR_WRONG		=		-108,// error in pairs of '[' and ']'
	         LINE_COMBINA_WRONG		=		-109,// error in combination key
             TOTAL_KEYOVERLAP_WRONG	=		-110,// error in key overlap
             TOTAL_KEYBODATA_WRONG  =       -111, //// No data in the device

             FUNC_LOGFILE_FAIL		=		-200, // error in log file
            /// <summary>
            /// ////////////////////
            /// </summary>

            //Unknown = 0,
            //Successed = 1,
            //Canceled = 2,
            //NoHardware = -1,
            //HardwareError = -2,//read/write error
            //BufferError = -3,//3 - data buffer error (data buffer passed to API is invalid)
            //FormatError = -4, //4 - data format error (data in buffer cannot be converted correctly)

        }

#region _API_Definition_

        /************************************************************************
         *
         * Detect if the KB9000 connected.
         * 
         * in C++ bool is one byte. I can not use int or bool of c#, it return wrong boolean value.
         * Just use byte as its return value.
         * Parameters:
         * @nInterface:
         * 		0 - RS232, N/A
         * 		1 - USB, 
         *		2 - PS2, 
         * return:
         * 	TRUE: connection detected.
         * 	FALSE: no connection detected.
         * KB9ACCESS_API bool DetectKB9(int nInterfaceIndex);
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "DetectKB9")]//, CharSet = CharSet.Auto, CallingConvention = CallingConvention.ThisCall)]
        private static extern byte DetectKB9Device(int nInterfaceIndex);


        /************************************************************************
         *
         * Function: startReadKB9
         * Description:
         * 	Read the defined data from the keypad via the specified interface, 
         * 	and the data is in ASCII (0x20 ?0x7f) text format.
         * 	
         * Parameters:
         * 	IN pBuffer: point to a buffer. (buffer size should be large enough)
         * 	IN nBufferSize: the buffer size in bytes.
         *  IN nInterface:
         * 		0 - RS232, N/A
         * 		1 - USB, 
         *		2 - PS2, 
         * return:
         * 	 0: starting successful. 
         *   1 -- 100: progress steps.
         *   < 0: error. (code defined above)
         * KB9ACCESS_API int startReadKB9(BYTE * pDataBuffer, ULONG nBufferSize, int nInterfaceIndex);
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "startReadKB9")]
        private static extern int API_startReadKB9(byte[] pDataBuffer, uint nBufferSize, int nInterfaceIndex);

        /************************************************************************
         *
         * Function: statusReadKB9Progress
         * Description:
         * 	request the reading action status.
         * 	
         * return:
         *   0 -- 100: progress steps.
         *   < 0: error. (code defined above)
         * KB9ACCESS_API int statusReadKB9Progress(void);
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "statusReadKB9Progress")]
        private static extern int API_statusReadKB9Progress();

        /************************************************************************
         *
         * Function: getReadKB9Data
         * Description:
         * 	get reading data of the length (pNumberOfBytesRead bytes) in the buffer
         *  specified in function startReadKB9( * ) if return code is seccessful.
         *  Or, pNumberOfBytesRead = 0.
         * 	
         * Parameters:
         *  OUT pNumberOfBytesRead: pointer to a ULONG to hold the length of data read in bytes.
         *
         * return:
         * 	 0: reading successful. 
         *   < 0: error. (code defined above)
         * KB9ACCESS_API int getReadKB9Data(ULONG *pNumberOfBytesRead);
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "getReadKB9Data")]
        private static extern int API_getReadKB9Data(ref uint pNumberOfBytesRead);


        /************************************************************************
         *
         * Function: startWriteKB9
         * Description:
         * 	write the defined data into the keypad via the specified interface, 
         * 	and the data is in ASCII (0x20 ?0x7f) text format.
         * 	
         * Parameters:
         * 	IN pBuffer: point to a buffer.
         * 	IN nBufferSize: the buffer size in bytes.
         *  IN nInterface:
         * 		0 - RS232, N/A
         * 		1 - USB, 
         *		2 - PS2, 
         * return:
         * 	 0: starting successful. 
         *   1 -- 100: progress steps.
         *   < 0: error. (code defined above)
         * KB9ACCESS_API int startWriteKB9(BYTE * pDataBuffer, ULONG nNumberOfBytesToWrite, int nInterfaceIndex);
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "startWriteKB9")]
        private static extern int API_startWriteKB9(byte[] pDataBuffer, uint nNumberOfBytesToWrite, int nInterfaceIndex);

        /************************************************************************
         *
         * Function:  statusWriteKB9Progress
         * Description:
         * 	request the keypad action status.
         * 	
         * return:
         *   0 -- 100: progress steps.
         *   < 0: error. (code defined above)
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "statusWriteKB9Progress")]
        private static extern int API_statusWriteKB9Progress();

        /************************************************************************
         *
         * Function: getWriteKB9Result
         * Description:
         * 	get the result of writing data action.
         * 	
         * Parameters:
         *  OUT pNumberOfBytesWritten: pointer to a ULONG to hold the length of data written in bytes.
         *
         * return:
         * 	 0: writing successful. 
         *   < 0: error. (error code defined later)
         * KB9ACCESS_API int getWriteKB9Result(ULONG *pNumberOfBytesWritten);
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "getWriteKB9Result")]
        private static extern int API_getWriteKB9Result(ref uint pNumberOfBytesWritten);

        /************************************************************************
         *
         * Function:  stopAccessKB9
         * Description:
         * 	forcefully stop the executing action.
         * 	
         * return:
         * 	 0: stopping successful. 
         *   < 0: error. (code defined above)
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "stopAccessKB9")]
        private static extern int API_stopAccessKB9();


        /************************************************************************
         *
         * Function:  getLeftSpaceBytesKB9
         * Description:
         * 	get the byte number of data space left in KB9000 device.
         * 	
         * Parameters:
         * 	IN pBuffer: point to a data string.
         * 	IN nNumberOfBytes: the length in bytes of the data string.
         *  OUT pBytesOFSpaceLeft: pointer to an int to hold the total space left in bytes.
         * 	
         * return:
         * 	 0: successful. 
         *   < 0: error. (code defined above)
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint ="getLeftSpaceBytesKB9")]
        private static extern int API_getLeftSpaceBytesKB9(byte[] pDataBuffer,uint nNumberOfBytes, ref int pBytesOFSpaceLeft );
        //KB9ACCESS_API int getLeftSpaceBytesKB9(BYTE * pDataBuffer, ULONG nNumberOfBytes, int *pBytesOFSpaceLeft);

        /*
        KB9ACCESS_API int getDllVersionKB9(BYTE * pDllVersion);
        */
        [DllImport("Kb9AccessDll.dll", EntryPoint = "getDllVersionKB9")]
        private static extern int API_getDllVersionKB9(byte[] pDataBuffer, uint nStringSize, int nInterfaceIndex);

        
        /************************************************************************
         *
         * Function:  EnableDisableLogFile
         * Description:
         * 	set enable/disable flag in DLL to enable log file or not.
         * 	
         * Parameters:
         * 	bFlag: TRUE - enable, FALSE - disable
         * 	
         * return:
         * 	 0: successful. 
         *   < 0: error. (code defined above)
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "EnableDisableLogFile")]
         private static extern int API_EnableDisableLogFile(bool bFlag);

        /************************************************************************
         *
         * Function:  EnableDisableLogFile
         * Description:
         * 	set enable/disable flag in DLL to enable log file or not.
         * 	
         * Parameters:
         * 	bFlag: true - enable, false - disable
         * 	
         * return:
         * 	 0: successful. 
         *   < 0: error. (code defined above)
        ************************************************************************/
        [DllImport("Kb9AccessDll.dll", EntryPoint = "ClearLogFile")]
         private static extern int API_ClearLogFile();

#endregion


        static public KB9_PORT m_PortType = KB9_PORT.USB;

        /************************************************************************/
        public static KB9API_ERROR DetectKB9(KB9_PORT nPortType)//, IntPtr msgReceiver)
        {

            byte nresult = DetectKB9Device((int)nPortType);
#if DEBUG
            //string s = "DEBUG --> Port type=" + nPortType.ToString();
            //s +=( ", detect result value = " + ((int)nresult).ToString() );
            //MessageBox.Show(s );//"DEBUG --> USB detect error:" + ((int)nresult).ToString());
#endif
            if (nresult !=0 ) //true
            //if (DetectKB9Device(1) != 0)
                return KB9API_ERROR.FUNC_SUCCESSFUL;
            else//others value is false
                return KB9API_ERROR.FUNC_OPENPORT_FAIL;
        }
        /// <summary>
        /// auto search usb and ps/2 port, finding kb9000 device.
        /// </summary>
        /// <param name="msgReceiver"></param>
        /// <returns></returns>
        public static KB9API_ERROR AutoDetectKB9( ref KB9_PORT kb9Port)
        {
            KB9API_ERROR result = KB9API_ERROR.FUNC_SUCCESSFUL;
            //1. detect usb port first, as usb will take less time than ps/2
            result = DetectKB9(KB9_PORT.USB);
            if (result == KB9API_ERROR.FUNC_SUCCESSFUL)
            {
                kb9Port = KB9_PORT.USB;
                return result;
            }

            //2. detect ps/2
            result = DetectKB9(KB9_PORT.PS2);
            if (result == KB9API_ERROR.FUNC_SUCCESSFUL)
            {
                kb9Port = KB9_PORT.PS2;
                return result;
            }

            kb9Port = KB9_PORT.Unknown;
            return KB9API_ERROR.FUNC_OPENPORT_FAIL;
        }
        private const int BUFFER_SIZE = 20480;
        static byte[] buffer_read = new byte[BUFFER_SIZE];
        public static KB9API_ERROR StartReadingKB9(KB9_PORT nPortType)
        {
            KB9API_ERROR result = (KB9API_ERROR)API_startReadKB9(buffer_read, BUFFER_SIZE, (int)nPortType);
            
            return result;

            
        }
        /// <summary>
        /// return 0 -- 100
        /// </summary>
        /// <returns></returns>
        public static int ReadingKB9Progress()
        {
            return API_statusReadKB9Progress();
            
        }

       
        /// <summary>
        /// static KB9ACCESS_API int getReadKB9Data(unsigned char* pData, unsigned long nDataNumberReturned);
        /// </summary>
        /// <param name="nPortType"></param>
        /// <param name="msgReceiver"></param>
        /// <param name="strRead"></param>
        /// <returns></returns>
        public static KB9API_ERROR ReadKB9Data( ref string strRead)
        {
            uint readlen = 0;
            KB9API_ERROR result = (KB9API_ERROR)API_getReadKB9Data(ref readlen);

            if (result != KB9API_ERROR.FUNC_SUCCESSFUL)
                return result;
            if (readlen <= 0)
                return KB9API_ERROR.FUNC_UNKOWN_ERROR;
         
            //convert buffer to  string
            string s =  Encoding.ASCII.GetString(buffer_read, 0, (int)readlen);
            strRead = s;
            return KB9API_ERROR.FUNC_SUCCESSFUL;
        }



        public static KB9API_ERROR StartWriting(KB9_PORT nPortType, string strData )
        {
            //字符串转为 bytes 数组
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strData);
            int nlen = bytes.Length;

            return (KB9API_ERROR)API_startWriteKB9(bytes, (uint)nlen, (int)nPortType);
            
        }

        public static int WritingKB9Progress()
        {
            return API_statusWriteKB9Progress();
            
        }

        public static uint GetWritingResult()
        {
            uint writenlen = 0;
            KB9API_ERROR result = (KB9API_ERROR)API_getWriteKB9Result(ref writenlen);
            if (result != KB9API_ERROR.FUNC_SUCCESSFUL)
                return 0;
            return writenlen;
        }



        public static KB9API_ERROR Cancel()
        {
            return (KB9API_ERROR)API_stopAccessKB9();
            
        }

        public static int GetLeftSpaceBytesKB9(string strData)
        {
            //字符串转为 bytes 数组
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strData);
            int nlen = bytes.Length;
            //System.Diagnostics.Debug.Print("data len=" + nlen.ToString());

            int nleft = 0;
            KB9API_ERROR result = (KB9API_ERROR)API_getLeftSpaceBytesKB9(bytes, (uint)nlen, ref nleft);
            if (result == KB9API_ERROR.FUNC_SUCCESSFUL)
                return nleft;
            else
            {
                return (int)result;
            }


        }

        public static KB9API_ERROR GetDllVersion(KB9_PORT nPortType, ref string dllVersion, ref string firmwareVersion)
        {
            byte[] ver = new byte[256];
           // uint readlen = 0;
            KB9API_ERROR result = (KB9API_ERROR)API_getDllVersionKB9(ver,256, (int)nPortType);
            // return V1.0.1.14 & Vd8.10
            string s = Encoding.ASCII.GetString(ver);
            int n = s.IndexOf("\0");
            string str = s.Substring(0, n);

            n = str.IndexOf("&");
            if (n < 0)
            {
                dllVersion = str;
                firmwareVersion = "";
            }
            else
            {
                dllVersion = str.Substring(0, n);
                if (n + 1 < str.Length)
                    firmwareVersion = str.Substring(n + 1);
                else
                    firmwareVersion = "";
                dllVersion.Trim();
                firmwareVersion.Trim();
                dllVersion = dllVersion.Replace("V", "");
                firmwareVersion = firmwareVersion.Replace("V", "");
            }

            return result;

            //if (result != KB9API_ERROR.FUNC_SUCCESSFUL)
            //    return result;
            

            ////convert buffer to  string
            //return KB9API_ERROR.FUNC_SUCCESSFUL;
        }


        public static KB9API_ERROR EnableDisableLogFile(bool bFlag)
        {
            try
            {
                KB9API_ERROR result = (KB9API_ERROR)API_EnableDisableLogFile(bFlag);
                return result;
            }
            catch (System.Exception ex)
            {
                return KB9API_ERROR.FUNC_UNKOWN_ERROR;
            }
            
        }


        /************************************************************************
         *
         * Function:  EnableDisableLogFile
         * Description:
         * 	set enable/disable flag in DLL to enable log file or not.
         * 	
         * Parameters:
         * 	bFlag: true - enable, false - disable
         * 	
         * return:
         * 	 0: successful. 
         *   < 0: error. (code defined above)
        ************************************************************************/
        public static KB9API_ERROR ClearLogFile()
        {
            KB9API_ERROR result = (KB9API_ERROR)API_ClearLogFile();
            return result;
        }
    }
}

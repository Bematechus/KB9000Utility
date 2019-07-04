using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    /// <summary>
    /// validate the keycode is correct or not
    /// </summary>
    public class KB9Validation
    {

        public enum ERR_VALIDATION
        {
            OK = 0,

            Less_ShiftDown,
            Less_ShiftUp,
            Less_RShiftDown,
            Less_RShiftUp,
        
            Less_CtrlDown,
            Less_CtrlUp,
            Less_RCtrlDown,
            Less_RCtrlUp,
        
            Less_AltDown,
            Less_AltUp,
            Less_RAltDown,
            Less_RAltUp,

            Less_WinDown,
            Less_WinUp,
            Less_RWinDown,
            Less_RWinUp,

            DownUp_Messed,
            Single_Function_With_Up, //it is a single function key, but with its "up" break code. 
                                     //we need to remove break code
            Repeat_Must_Follow_Key, //20141210
            Repeat_Can_Not_Follow_Combination_Macro_Pause,


        }

        private enum ERR_KEYPAIR
        {
            OK = 0,
            Less_Down,
            Less_Up,
            Up_Before_Down
        }

        /// <summary>
        /// check if give text is valid kb9000 keycodes
        /// </summary>
        /// <param name="strKeycodes"></param>
        /// <returns></returns>
        public ERR_VALIDATION ValidateKB9Keycodes(string strKeycodes)
        {
            ERR_VALIDATION err = CheckShiftUp(strKeycodes);

            if (err != ERR_VALIDATION.OK)
                return err;

            err = CheckCtrlUp(strKeycodes);
            if (err != ERR_VALIDATION.OK)
                return err;

            err = CheckAltUp(strKeycodes);
            if (err != ERR_VALIDATION.OK)
                return err;

            err = CheckWinUp(strKeycodes);
            if (err != ERR_VALIDATION.OK)
                return err;

            err = ValidateRepeatFollowingKeys(strKeycodes);
            if (err != ERR_VALIDATION.OK)
                return err;
            return ERR_VALIDATION.OK;
            
        }
        /// <summary>
        /// It is now hanging up. However, I think it is better to check for this error in the editor.  
        /// That is, when defocus from the key, if [RepeatN] is not followed by a key, then flag an error 
        /// similar to the [Ctrl][#Ctrl] checks.  It should be easy to check as the error occurs only 
        /// when Repeat is at end of the definition.  Error message should be ¡°Repeat function must 
        /// be followed by a key.¡±  When OK is clicked, it should return to the edit box.
        /// 
        /// In addition, [Repeat]  cannot be followed by ¡°Key Combination¡±, ¡°Macro¡± and ¡°Pause¡±.  
        /// Please also check these limitations at defocus and flag error with message ¡°Repeat function 
        /// cannot be followed by Key Combination, Macro or Pause.¡±
        /// </summary>
        /// <param name="strKeycodes"></param>
        /// <returns></returns>
        private ERR_VALIDATION ValidateRepeatFollowingKeys(string strKeycodes)
        {
            //1. check if no key follows 

            int nStartIndex = strKeycodes.IndexOf("[Repeat");
            if (nStartIndex < 0)
                return ERR_VALIDATION.OK;
            int nEndIndex = strKeycodes.IndexOf("]", nStartIndex);
            if (nEndIndex == strKeycodes.Length - 1)
                return ERR_VALIDATION.Repeat_Must_Follow_Key;
            //find next following key
            if (nEndIndex < 0)
                return ERR_VALIDATION.OK;
            //2. Check if follow combination, macro and pause
            string s = strKeycodes.Substring(nEndIndex+1);
            if (s.Equals(""))
                return ERR_VALIDATION.OK;
            string strFirst = s.Substring(0, 1);
            if (!strFirst.Equals("["))
                return ERR_VALIDATION.OK;
            nEndIndex = s.IndexOf("]");
            s = s.Substring(0, nEndIndex + 1);
            if (s.IndexOf("[Macro") >= 0 ||
                s.IndexOf("[Pause") >= 0 ||
                s.IndexOf("+") >=0)
                return ERR_VALIDATION.Repeat_Can_Not_Follow_Combination_Macro_Pause;

            return ERR_VALIDATION.OK;

           

        }
        /// <summary>
        /// 
        /// </summary>
        private bool _AllowSingleFunctionKey = true;
        public bool AllowSingleFunctionKey
        {
            get
            {
                return _AllowSingleFunctionKey;
            }
            set
            {
                _AllowSingleFunctionKey = value;
            }
        }

        /// <summary>
        /// check if the [#Shift] was existed with [Shift]
        /// </summary>
        /// <param name="strKeyCodes"></param>
        /// <returns></returns>
        private ERR_VALIDATION CheckShiftUp(string strKeyCodes)
        {
            string s = strKeyCodes;
            if (this.AllowSingleFunctionKey)
            {
             //   s = adjust_allow_last_extend_function_key(s, KB9Const.SHIFT_DOWN);
             //   s = adjust_allow_last_extend_function_key(s, KB9Const.RSHIFT_DOWN);
                if (s == KB9Const.SHIFT_DOWN ||  s == KB9Const.RSHIFT_DOWN)
                    return ERR_VALIDATION.OK;

                if (s == KB9Const.Shift_DownUp() ||
                    s == KB9Const.RShift_DownUp())
                    return ERR_VALIDATION.Single_Function_With_Up;
            }
            

            //if there is [Shift], or [RShift], string must have corresponded [#Shift] or [#RShift]
            ERR_KEYPAIR err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.ShiftKey), KB9KeyDefinition.KeyUpDisplayName(Keys.ShiftKey));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_ShiftDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_ShiftUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.RShiftKey), KB9KeyDefinition.KeyUpDisplayName(Keys.RShiftKey));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_RShiftDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_RShiftUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            return ERR_VALIDATION.OK;
        }


        /// <summary>
        /// check if the [#Shift] was existed with [Shift]
        /// </summary>
        /// <param name="strKeyCodes"></param>
        /// <returns></returns>
        private ERR_VALIDATION CheckWinUp(string strKeyCodes)
        {
            string s = strKeyCodes;
           
            if (this.AllowSingleFunctionKey)
            {
                if (s == KB9Const.WIN_DOWN || s == KB9Const.RWIN_DOWN)
                    return ERR_VALIDATION.OK;
                if (s == KB9Const.Win_DownUp() ||
                    s == KB9Const.RWin_DownUp())
                    return ERR_VALIDATION.Single_Function_With_Up;
                //s = adjust_allow_last_extend_function_key(s, KB9Const.WIN_DOWN);
                //s = adjust_allow_last_extend_function_key(s, KB9Const.RWIN_DOWN);
            }
            //if there is [Win], or [RWin], string must have corresponded [#Shift] or [#RShift]
            ERR_KEYPAIR err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.LWin), KB9KeyDefinition.KeyUpDisplayName(Keys.LWin));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_WinDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_WinUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.RWin), KB9KeyDefinition.KeyUpDisplayName(Keys.RWin));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_RWinDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_RWinUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            return ERR_VALIDATION.OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKeyCodes"></param>
        /// <returns></returns>
        private ERR_VALIDATION CheckCtrlUp(string strKeyCodes)
        {
            string s = strKeyCodes;
            //if (s == "[Ctrl]" ||
            //   s == "[RightCtrl]")
            //    return ERR_VALIDATION.OK;
            if (this.AllowSingleFunctionKey)
            {
                if (s == KB9Const.CTRL_DOWN || s == KB9Const.RCTRL_DOWN)
                   return ERR_VALIDATION.OK;

               if (s == KB9Const.Ctrl_DownUp() ||
                   s == KB9Const.RCtrl_DownUp())
                   return ERR_VALIDATION.Single_Function_With_Up;
                //s = adjust_allow_last_extend_function_key(s, KB9Const.CTRL_DOWN);
                //s = adjust_allow_last_extend_function_key(s, KB9Const.RCTRL_DOWN);
            }
            //if there is [Shift], or [RShift], string must have corresponded [#Shift] or [#RShift]
            ERR_KEYPAIR err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.ControlKey), KB9KeyDefinition.KeyUpDisplayName(Keys.ControlKey));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_CtrlDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_CtrlUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.RControlKey), KB9KeyDefinition.KeyUpDisplayName(Keys.RControlKey));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_RCtrlDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_RCtrlUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            return ERR_VALIDATION.OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKeyCodes"></param>
        /// <returns></returns>
        private ERR_VALIDATION CheckAltUp(string strKeyCodes)
        {
            string s = strKeyCodes;
           
            if (this.AllowSingleFunctionKey)
            { 
                if (s == KB9Const.ALT_DOWN ||  s == KB9Const.RALT_DOWN)
                    return ERR_VALIDATION.OK;
                if (s == KB9Const.Alt_DownUp() ||
                    s == KB9Const.RAlt_DownUp())
                    return ERR_VALIDATION.Single_Function_With_Up;
                //s = adjust_allow_last_extend_function_key(s, KB9Const.ALT_DOWN);
                //s = adjust_allow_last_extend_function_key(s, KB9Const.RALT_DOWN);
            }
            //if there is [Shift], or [RShift], string must have corresponded [#Shift] or [#RShift]
            ERR_KEYPAIR err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.Menu), KB9KeyDefinition.KeyUpDisplayName(Keys.Menu));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_AltDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_AltUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            err = CheckKeyPair(s, KB9KeyDefinition.KeyDisplayName(Keys.RMenu), KB9KeyDefinition.KeyUpDisplayName(Keys.RMenu));
            if (err == ERR_KEYPAIR.Less_Down)
                return ERR_VALIDATION.Less_RAltDown;
            if (err == ERR_KEYPAIR.Less_Up)
                return ERR_VALIDATION.Less_RAltUp;
            if (err == ERR_KEYPAIR.Up_Before_Down)
                return ERR_VALIDATION.DownUp_Messed;

            return ERR_VALIDATION.OK;
        }

      

        private ERR_KEYPAIR CheckKeyPair2(string strText, string strKeyDown, string strKeyUp)
        {
            List<int> arDown = new List<int>();
            int nDownCount = FindAllString(strText, strKeyDown, arDown);
           // if (nDownCount == 0)
           //     return ERR_KEYPAIR.OK;

            List<int> arUp = new List<int>();
            int nUpCount = FindAllString(strText, strKeyUp, arUp) ;
           
            if (nDownCount > nUpCount)
                return ERR_KEYPAIR.Less_Up;
            else if (nDownCount < nUpCount)
                return ERR_KEYPAIR.Less_Down;

            if (nDownCount == 0 &&nUpCount == 0)
                return ERR_KEYPAIR.OK;

            if (arUp[0] < arDown[0])
                return ERR_KEYPAIR.Up_Before_Down;

            return ERR_KEYPAIR.OK;



        }

        private bool IsStringPosition(string strText, int nPosition,  string strCheck)
        {
            int n = strText.IndexOf(strCheck, nPosition);
            return (n == nPosition);
        }

        private ERR_KEYPAIR CheckKeyPair(string strText, string strKeyDown, string strKeyUp)
        {

            List<int> arDown = new List<int>();
            int nDownCount = FindAllString(strText, strKeyDown, arDown);
           

            List<int> arUp = new List<int>();
            int nUpCount = FindAllString(strText, strKeyUp, arUp);

            List<int> arFind = arDown;
            arFind.AddRange(arUp);
            arFind.Sort();

            int nResult = 0;
            for (int i = 0; i < arFind.Count; i++)
            {
                int n = arFind[i];
                if ( IsStringPosition(strText,n,  strKeyDown) )
                    nResult++;
                else
                    nResult--;
                if (nResult < 0)
                    return ERR_KEYPAIR.Less_Down;
            }
            if (nResult > 0)
                return ERR_KEYPAIR.Less_Up;
            else if (nResult < 0)
                return ERR_KEYPAIR.Less_Down;
            else
                return ERR_KEYPAIR.OK;

            //if (nDownCount > nUpCount)
            //    return ERR_KEYPAIR.Less_Up;
            //else if (nDownCount < nUpCount)
            //    return ERR_KEYPAIR.Less_Down;

            //if (nDownCount == 0 && nUpCount == 0)
            //    return ERR_KEYPAIR.OK;

            //if (arUp[0] < arDown[0])
            //    return ERR_KEYPAIR.Up_Before_Down;

            //return ERR_KEYPAIR.OK;



        }



        /// <summary>
        /// find all strFind in strText, and save index in ar
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="strFind"></param>
        /// <param name="ar"></param>
        /// <returns></returns>
        private int FindAllString(string strText, string strFind, List<int> ar)
        {
            int nstartindex = 0;
            int nindex = -1;
            while ( (nindex = strText.IndexOf(strFind, nstartindex)) >=0)
            {
                nstartindex = nindex + 1;
                ar.Add(nindex);
            }
            return ar.Count;
        }

#region ____Validate_KB9TextBox___
        static public bool ValidateKeyTextBox(Form frmCaller, KB9TextBox ctrlText)
        {
            string strKeys = ctrlText.Text;// GetInputedKeyCodes();
            if (strKeys == string.Empty) return true;
            KB9Validation kb9 = new KB9Validation();
            KB9Validation.ERR_VALIDATION err = kb9.ValidateKB9Keycodes(strKeys);
            if (err == KB9Validation.ERR_VALIDATION.OK) return true;

            string err_msg = ValidateErrMsg(err);
            if (err == KB9Validation.ERR_VALIDATION.Less_AltUp ||
                err == KB9Validation.ERR_VALIDATION.Less_RAltUp ||
                err == KB9Validation.ERR_VALIDATION.Less_CtrlUp ||
                err == KB9Validation.ERR_VALIDATION.Less_RCtrlUp ||
                err == KB9Validation.ERR_VALIDATION.Less_RShiftUp ||
                err == KB9Validation.ERR_VALIDATION.Less_ShiftUp ||
                err == KB9Validation.ERR_VALIDATION.Less_WinUp ||
                err == KB9Validation.ERR_VALIDATION.Less_RWinUp)
            {


                err_msg += "\n\rDo you want utility append this key to end?";

                DialogResult result = MessageBox.Show(frmCaller, err_msg, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    string strAppend = AppendErrKey(err);
                    ctrlText.AddKeyCode(strAppend, false);
                }
            }
            else
            {
                err_msg += "\n\rPlease fixed it manually.";
                MessageBox.Show(frmCaller, err_msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;

        }

        /************************************************************************/
        /* 
         * return: 0: ok
         *          1: messagebox shown, and correct
         *          2: messagebox shown, and manually correct them
         *          3: cancel edit, undo
         */
        /************************************************************************/
        static public int ValidateKeyTextBox2(Form frmCaller, KB9TextBox ctrlText)
        {
            string strKeys = ctrlText.Text;// GetInputedKeyCodes();
            if (strKeys == string.Empty) return 0;
            KB9Validation kb9 = new KB9Validation();
            KB9Validation.ERR_VALIDATION err = kb9.ValidateKB9Keycodes(strKeys);
            if (err == KB9Validation.ERR_VALIDATION.OK) return 0;
            string err_msg = ValidateErrMsg(err);
            string strlog = "Key contents validation error";
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog += ("Key contents:" + strKeys);
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog += err_msg;

            KB9Logger.Err(strlog);
            



            if (err == KB9Validation.ERR_VALIDATION.Single_Function_With_Up)
            { //if it is single function key, just keep down key.
                ctrlText.RemoveLastKey();
                ctrlText.Refresh();
                return 0;
            }

            if (err == ERR_VALIDATION.Repeat_Must_Follow_Key ||
                err == ERR_VALIDATION.Repeat_Can_Not_Follow_Combination_Macro_Pause)
            {
                MessageBox.Show(frmCaller,
                                err_msg,
                                "Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return 2;
            }

            if (err == ERR_VALIDATION.Less_AltDown ||
                err == ERR_VALIDATION.Less_CtrlDown ||
                err == ERR_VALIDATION.Less_RAltDown ||
                err == ERR_VALIDATION.Less_RCtrlDown ||
                err == ERR_VALIDATION.Less_RShiftDown ||
                err == ERR_VALIDATION.Less_RWinDown ||
                err == ERR_VALIDATION.Less_ShiftDown ||
                err == ERR_VALIDATION.Less_WinDown)
            {
                DialogResult r = frmKeyCodeLessDown.ConfirmCancel();// MessageBox.Show(frmCaller, 
                if (r == DialogResult.Yes)
                    return 3;
                else
                    return 2;

            }

            //DialogResult result = MessageBox.Show(frmCaller, 
            //                                      "Key release codes does not match with corresponding key press codes.  Automatic fix by inserting key release codes at the end?",
            //                                      "Warning", 
            //                                      MessageBoxButtons.YesNo, 
            //                                      MessageBoxIcon.Warning);
            DialogResult result = frmKeyCodeError.ConfirmError();// MessageBox.Show(frmCaller, 

            if (result == DialogResult.Yes)
            {
                int nloop = 0;
                while (err != ERR_VALIDATION.OK)
                {


                    if (err == KB9Validation.ERR_VALIDATION.Less_AltUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RAltUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_CtrlUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RCtrlUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RShiftUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_ShiftUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_WinUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RWinUp)
                    {
                        string strAppend = AppendErrKey(err);
                        //ctrlText.AddKeyCode(strAppend, false);
                        ctrlText.AppendKeyCode(strAppend);
                    }
                    //else if (err == KB9Validation.ERR_VALIDATION.Single_Function_With_Up)
                    //{
                    //    ctrlText.RemoveLastKey();
                    //}
                    else
                    { //less down, remove last up 
                        string strRemove = RemoveErrKey(err);
                        ctrlText.RemoveLastKeyCode(strRemove);


                    }
                    strKeys = ctrlText.Text;
                    err = kb9.ValidateKB9Keycodes(strKeys);
                    nloop++;
                    if (nloop > 256)
                    {
                        MessageBox.Show(frmCaller, "The automatic fix failed, please correct it manually.",
                                        "Warning",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        ctrlText.Refresh();
                        //log
                        strlog = "Can not fixed automaticly.";
                        strlog += KB9Logger.LOG_LF;
                        strlog += KB9Logger.LOG_TAB;
                        strlog += "Result:";
                        strlog += ctrlText.Text;
                        KB9Logger.Err(strlog);
                        return 2;
                    }

                }
                ctrlText.Refresh();
                //log
                strKeys = ctrlText.Text;
                strlog = "Auto fixed result:";
                strlog += KB9Logger.LOG_LF;
                strlog += KB9Logger.LOG_TAB;
                strlog += strKeys;
                KB9Logger.Log(strlog);

            }
            else
            {
                return 2;
            }

           

            return 1;

        }

        /// <summary>
        /// this function is for single funciton key(without "up" code).
        /// But, while write to kb9, we have to apppend the "up" code.
        /// we do this work at here.
        /// return:
        ///     "": can not fix it.
        ///     Others string: corrrected string.
        /// 
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        static public string auto_fix_key_content(string strText)
        {
            string strKeys = strText;
            if (strKeys == string.Empty) return "";
            KB9Validation kb9 = new KB9Validation();
            kb9.AllowSingleFunctionKey = false;

            KB9Validation.ERR_VALIDATION err = kb9.ValidateKB9Keycodes(strKeys);
            if (err == KB9Validation.ERR_VALIDATION.OK) return strKeys;

            
            
            int nloop = 0;
            while (err != ERR_VALIDATION.OK)
            {
                if (err == KB9Validation.ERR_VALIDATION.Less_AltUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RAltUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_CtrlUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RCtrlUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RShiftUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_ShiftUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_WinUp ||
                    err == KB9Validation.ERR_VALIDATION.Less_RWinUp)
                {
                    string strAppend = AppendErrKey(err);
                    
                    strKeys += (strAppend);
                }
                else
                { //less down, remove last up 
                    string strRemove = RemoveErrKey(err);
                    int nindex = strKeys.LastIndexOf(strRemove);
                    strKeys = strKeys.Remove(nindex, strRemove.Length);

                    //ctrlText.RemoveLastKeyCode(strRemove);


                }
                //strKeys = ctrlText.Text;
                err = kb9.ValidateKB9Keycodes(strKeys);
                nloop++;
                if (nloop > 256)
                {
                   
                    return "";
                }

            }
            
            return strKeys;

        }



        static public string ValidateKeyTextBoxMsg(Form frmCaller, KB9TextBox ctrlText)
        {
            if (ctrlText == null) return "";
            string strKeys = ctrlText.Text;// GetInputedKeyCodes();
            if (strKeys == string.Empty) 
                return "";
            KB9Validation kb9 = new KB9Validation();
            KB9Validation.ERR_VALIDATION err = kb9.ValidateKB9Keycodes(strKeys);
            if (err == KB9Validation.ERR_VALIDATION.OK) 
                return "";

            string err_msg = ValidateErrMsg(err);
            return err_msg;

            //if (err == KB9Validation.ERR_VALIDATION.Less_AltUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_RAltUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_CtrlUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_RCtrlUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_RShiftUp ||
            //    err == KB9Validation.ERR_VALIDATION.Less_ShiftUp)
            //{

            //    return err_msg;
            //    //err_msg += "\n\rDo you want utility append this key to end?";

            //    //DialogResult result = MessageBox.Show(frmCaller, err_msg, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            //    //if (result == DialogResult.Yes)
            //    //{
            //    //    string strAppend = AppendErrKey(err);
            //    //    ctrlText.AddKeyCode(strAppend, false);
            //    //}
            //}
            //else
            //{
            //    err_msg += "\n\rPlease fixed it manually.";
            //    MessageBox.Show(frmCaller, err_msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            //return false;

        }


        static private string ValidateErrMsg(KB9Validation.ERR_VALIDATION err)
        {
            switch (err)
            {
                case KB9Validation.ERR_VALIDATION.Less_ShiftDown:
                    return "[Shift] down less than up.";
                case KB9Validation.ERR_VALIDATION.Less_ShiftUp:
                    return "[Shift] up less than down.";
                case KB9Validation.ERR_VALIDATION.Less_RShiftDown:
                    return "[RightShift] down less than up.";
                case KB9Validation.ERR_VALIDATION.Less_RShiftUp:
                    return "[RightShift] up less than down.";
                case KB9Validation.ERR_VALIDATION.Less_CtrlDown:
                    return "[Control] down less than up.";
                case KB9Validation.ERR_VALIDATION.Less_CtrlUp:
                    return "[Control] up less than down.";
                case KB9Validation.ERR_VALIDATION.Less_RCtrlDown:
                    return "[RightControl] down less than up.";
                case KB9Validation.ERR_VALIDATION.Less_RCtrlUp:
                    return "[RightControl] up less than down.";
                case KB9Validation.ERR_VALIDATION.Less_AltDown:
                    return "[Alt] down less than up.";
                case KB9Validation.ERR_VALIDATION.Less_AltUp:
                    return "[Alt] up less than down.";
                case KB9Validation.ERR_VALIDATION.Less_RAltDown:
                    return "[RightAlt] down less than up.";
                case KB9Validation.ERR_VALIDATION.Less_RAltUp:
                    return "[RightAlt] up less than down.";
                case KB9Validation.ERR_VALIDATION.DownUp_Messed:
                    return "Key Up event happened before key down event.";
                case KB9Validation.ERR_VALIDATION.Single_Function_With_Up:
                    return "Single function key just keep down event, remove up key.";
                case ERR_VALIDATION.Repeat_Can_Not_Follow_Combination_Macro_Pause:
                    return "Repeat function cannot be followed by Key Combination, Macro or Pause.";
                case ERR_VALIDATION.Repeat_Must_Follow_Key:
                    return "Repeat function must be followed by a key.";  
            }
            return "";
        }

        static private string AppendErrKey(KB9Validation.ERR_VALIDATION err)
        {
            switch (err)
            {
                case KB9Validation.ERR_VALIDATION.Less_ShiftDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.ShiftKey);
                case KB9Validation.ERR_VALIDATION.Less_ShiftUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.ShiftKey);
                case KB9Validation.ERR_VALIDATION.Less_RShiftDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.RShiftKey);
                case KB9Validation.ERR_VALIDATION.Less_RShiftUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RShiftKey);
                case KB9Validation.ERR_VALIDATION.Less_CtrlDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.ControlKey);
                case KB9Validation.ERR_VALIDATION.Less_CtrlUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.ControlKey);
                case KB9Validation.ERR_VALIDATION.Less_RCtrlDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.RControlKey);
                case KB9Validation.ERR_VALIDATION.Less_RCtrlUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RControlKey);
                case KB9Validation.ERR_VALIDATION.Less_AltDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.Menu);
                case KB9Validation.ERR_VALIDATION.Less_AltUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.Menu);
                case KB9Validation.ERR_VALIDATION.Less_RAltDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.RMenu);
                case KB9Validation.ERR_VALIDATION.Less_RAltUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RMenu);

                case KB9Validation.ERR_VALIDATION.Less_WinDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.LWin);
                case KB9Validation.ERR_VALIDATION.Less_WinUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.LWin);
                case KB9Validation.ERR_VALIDATION.Less_RWinDown:
                    return KB9KeyDefinition.KeyDisplayName(Keys.RWin);
                case KB9Validation.ERR_VALIDATION.Less_RWinUp:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RWin);
                


            }
            return "";
        }

        static private string RemoveErrKey(KB9Validation.ERR_VALIDATION err)
        {
            switch (err)
            {
                case KB9Validation.ERR_VALIDATION.Less_ShiftDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.ShiftKey);
                //case KB9Validation.ERR_VALIDATION.Less_ShiftUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.ShiftKey);
                case KB9Validation.ERR_VALIDATION.Less_RShiftDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RShiftKey);
                //case KB9Validation.ERR_VALIDATION.Less_RShiftUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.RShiftKey);
                case KB9Validation.ERR_VALIDATION.Less_CtrlDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.ControlKey);
                //case KB9Validation.ERR_VALIDATION.Less_CtrlUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.ControlKey);
                case KB9Validation.ERR_VALIDATION.Less_RCtrlDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RControlKey);
                //case KB9Validation.ERR_VALIDATION.Less_RCtrlUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.RControlKey);
                case KB9Validation.ERR_VALIDATION.Less_AltDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.Menu);
                //case KB9Validation.ERR_VALIDATION.Less_AltUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.Menu);
                case KB9Validation.ERR_VALIDATION.Less_RAltDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RMenu);
                //case KB9Validation.ERR_VALIDATION.Less_RAltUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.RMenu);

                case KB9Validation.ERR_VALIDATION.Less_WinDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.LWin);
                //case KB9Validation.ERR_VALIDATION.Less_WinUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.LWin);
                case KB9Validation.ERR_VALIDATION.Less_RWinDown:
                    return KB9KeyDefinition.KeyUpDisplayName(Keys.RWin);
                //case KB9Validation.ERR_VALIDATION.Less_RWinUp:
                //    return KB9KeyDefinition.KeyUpDisplayName(Keys.RWin);
                //case KB9Validation.ERR_VALIDATION.Single_Function_With_Up:


            }
            return "";
        }
#endregion

        private bool is_last_string(string strText, string strFind)
        {
            string strCode = strText;
            int nindex = strCode.LastIndexOf(strFind);

            if (nindex < 0)
                return false;

            if ( (nindex + strFind.Length) >= strCode.Length)
                return true;
            return false;
        }

        private string adjust_allow_last_extend_function_key(string strText, string strKey)
        {
            if (!is_last_string(strText, strKey))
                return strText;
            int nindex = strText.LastIndexOf(strKey);
            string s = strText.Remove(nindex);
            return s;


        }

        
    }
}

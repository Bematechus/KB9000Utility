using System;
using System.Collections.Generic;
using System.Text;

namespace KB9Utility
{
    public class KeyEditingType
    {
        string strKeycode = "";
        public KeyEditingType()
        {
            strKeycode = "";
        }
        public KeyEditingType(string strInit)
        {
            strKeycode = strInit;
        }
        public override string ToString()
        {
            return strKeycode;
            //return base.ToString();
        }
        public string GetKeyCodeString()
        {
            return strKeycode;
        }
        public void SetKeyCodeString(string val)
        {
            strKeycode = val;
        }

          /// <summary>
        /// Out all key code with []
        /// @bCapsEffect:
        ///         Whether the caps_lock_effect worked.
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToCSV()
        {
            return ToCSV(false);

            
        }
        /// <summary>
        /// convert pause1.3 to pause13
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        private string convertPause(string strCode)
        {
            string strChanged = strCode;
            int nStart = 0;
            for (int i = 0; i < 1000; i++)
            {


                nStart = strChanged.IndexOf("[Pause", nStart);
                if (nStart < 0)
                    return strChanged;
                int nEnd = strChanged.IndexOf("]", nStart);
                if (nEnd < 0)
                    return strChanged;
                string s = strChanged.Substring(nStart, nEnd - nStart + 1);
                string strNew = s.Replace(".", "");
                strChanged = strChanged.Remove(nStart, nEnd - nStart + 1);
                strChanged = strChanged.Insert(nStart, strNew);
                nStart = nEnd;
            }
            return strChanged;


        }
        ////
        /// convert pause13 to pause1.3
        /// ///
        /// 
        public string convertPauseToReader(string strCode)
        {
            int nStart = 0;
            do
            {
                nStart = strCode.IndexOf("[Pause", nStart);
                if (nStart < 0)
                    return strCode;
                int nEnd = strCode.IndexOf("]", nStart);
                if (nEnd < 0)
                    return strCode;
                string strOriginal = strCode.Substring(nStart, nEnd - nStart + 1);
                string s = strOriginal;
                s = s.Replace("[Pause", "");
                s = s.Replace("]", "");
                if (s.Equals("Break"))
                {
                    nStart++;
                    continue;
                }
                //int n = decimal.Parse(s);
                decimal d = decimal.Parse(s);

                d = d / 10;
                s = d.ToString("f1");

                string strNew = "[Pause" + s + "]";// s.Replace(".", "");
                strCode = strCode.Remove(nStart, strOriginal.Length);
                strCode = strCode.Insert(nStart, strNew);
                //strCode = strCode.Replace(strOriginal, strNew);
                nStart++;
            }
            while (nStart >= 0);

            return strCode;

        }
        /// <summary>
        /// note that if the content is the shifted keys such as !, @, #, ? etc, then this feature is not supported
        ///      Keys allowed to support ¡°emulate keyboard¡± (select True/False)
        /// `   -   =   [   ]   \   ;    ¡®    ,   .   /           0-9         a-z
        /// 
        /// Keys NOT allowed to support ¡°emulate keyboard¡± (option always False)
        ///                     ~   !  @  #  $  %  ^  &  *  (  )  _  +  {  }  |  :  ¡±  <  >  ?        A-Z
        ///                  These keys are formed with ¡°Shift¡± and thus do not support the feature.

                             


        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        static public bool isEmulateKbdKey(char ch)
        {
            char[] ar = { '`','-', '=','[',']','\\',';','\'',',','.','/'};

            if (ch >= '0' && ch <= '9' )
                return true;

            //if (ch == ';' || ch == '=')
            //    return true;

            //if (ch >= 'A' && ch <= ']')
            //    return true;

            //if (ch >= '`' )
            //    return true;

            if (ch >= 'a' && ch <= 'z')
                return true;

            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i] == ch)
                    return true;
            }
            return false;
        }

        static public bool isDisableEnumateKey(char ch)
        {
            char[] ar = {'~','!','@', '#',  '$',  '%',  '^',  '&',  '*',  '(',  ')',  '_',  '+',  '{',  '}',  '|',  
                            ':',  '"',  '<',  '>',  '?' };
            if (ch >= 'A' && ch <= 'Z')
                return true;
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i] == ch)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Out all key code with []
        /// @bCapsEffect:
        ///         Whether the caps_lock_effect worked.
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToCSV(bool bCapsEffect)
        {
            string s =  this.ToString();
            s = expand_combination(s);
            if (bCapsEffect)
            { //if caps effect enabled, append [ ] to this single character
                if (s.Length == 1)
                {
                    char ch = s[0];
                    //if (ch >= 'a' && ch <= 'z')
                    if (isEmulateKbdKey(ch)) //20141208
                        s = "[" + s + "]";
                }
                else if (s.Equals("\\["))
                {
                    s = "[{]";
                }
                else if (s.Equals("\\]"))
                {
                    s = "[}]";
                }
                else if (s.Equals("\\\\"))
                {
                    s = "[\\]";
                }
                
            }
            //20140203
            s = KB9Validation.auto_fix_key_content(s);
            s = convertPause(s);
            return s;
            //List<string> lst = new List<string>();
            //int n = Util.ParseKeyContents(s, lst);
            //string strReturn = "";
            //for (int i=0; i< n; i++)
            //{
            //    s = lst[i];
            //    if (s == string.Empty)
            //        continue;
            //    if (!ExistBracket(s))
            //    {
            //        s = "[" + s + "]";
            //        lst[i] = s;
            //    }
            //    strReturn += lst[i];
            //}
            //return strReturn;

        }
        private bool ExistBracket(string str)
        {
            if (str.IndexOf("[") >= 0 && str.IndexOf("]") >= 0)
                return true;
            return false;
        }
   
        /// <summary>
        /// [][][]format key content
        /// </summary>
        /// <param name="strCSV"></param>
        /// <returns></returns>
        public bool FromCSV(string strCSV)
        {
            string s = strCSV;

            List<string> lst = new List<string>();
            int n = Util.ParseKeyContents(s, lst);
            string strResult = "";
            for (int i = 0; i < n; i++)
            {
                s = lst[i];
                s = s.Trim();
                if (s == string.Empty)
                {
                    lst[i] = "";
                    continue;
                }
                if (is_combination_content(s) ||
                    Util.is_special_key(s) ||
                    is_macro(s) ||
                    is_pause(s) ||
                    is_repeat(s))
                    
                {
                    strResult += s;
                    continue;
                }
                if (is_slidev(s) ||
                    is_slideh(s))
                {
                    
                    continue;
                }
                //deteal with some special keys
                if (s.Equals("[{]") )
                {
                    s = "\\[";
                    //lst[i] = s;
                    //strResult += s;
                }
                else if (s.Equals("[}]"))
                {
                    s = "\\]";
                }
                else if (s.Equals("[\\]"))
                {
                    s = "\\\\";
                   // lst[i] = s;
                    //strResult += s;
                }
                else
                {


                    //now it is a normal key, but with "[]";
                    s = s.Replace("\\[", "\t");//make sure we don't remove "\[" char
                    s = s.Replace("[", "");
                    s = s.Replace("\t", "\\[");

                    s = s.Replace("\\]", "\t");
                    s = s.Replace("]", "");
                    s = s.Replace("\t", "\\]");
                }
                lst[i] = s;
                strResult += s;

            }
            strResult = convertPauseToReader(strResult);

            SetKeyCodeString(strResult);
            return true;
        }
        private bool is_combination_content(string str)
        {
            return Util.is_combination_content(str);

            //int n = str.IndexOf("+");
            //string s = str;
            //s = s.Replace("]", "");

            //if (n >= 0 && n != (s.Length -1) )
            //{
            //    //if (str.Equals("[PAD+]"))
            //    //    return false;
            //    if (str.Length > 3)
            //        return true;
            //}
            //return false;
        }
        private bool is_repeat(string str)
        {
            return (str.IndexOf("[Repeat") >= 0);
                
        }

        private bool is_pause(string str)
        {
            return (str.IndexOf("[Pause") >= 0);

        }

        private bool is_macro(string str)
        {
            return (str.IndexOf("[Macro") >= 0);

        }
        private bool is_slidev(string str)
        {
            return (str.IndexOf("[SlideV]") >= 0);

        }

        private bool is_slideh(string str)
        {
            return (str.IndexOf("[SlideH]") >= 0);

        }

        /// <summary>
        /// expand all combination keys
        /// 
        ///1.       For combination keys, e.g: [Ctrl + shift + a], parse to
        ///"[Ctrl][Shift]a[#Shift][#Ctrl]" before writing to kb9000. Do this with Ctrl,
        ///Shift , Alt, and Win keys.
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        private string expand_combination(string strCode)
        {
            string s = strCode;

            List<string> lst = new List<string>();
            int n = Util.ParseKeyContents(s, lst);
            string strResult = "";
            for (int i = 0; i < n; i++)
            {
                s = lst[i];
                s = s.Trim();
                if (s == string.Empty)
                {
                    continue;
                }
                if (is_combination_content(s))
                {
                    strResult += parse_combination_keys( s );   
                }
                else
                {
                    strResult += s;
                }

            }
            return strResult;
        }
        
        private string parse_combination_keys(string strKeys)
        {

            if (strKeys.IndexOf("Shift") < 0 &&
                strKeys.IndexOf("RightShift") < 0 &&
                strKeys.IndexOf("Ctrl") < 0 &&
                strKeys.IndexOf("RightCtrl") < 0 &&
                strKeys.IndexOf("Alt") < 0 &&
                strKeys.IndexOf("RightAlt") < 0 &&
                strKeys.IndexOf("Win") < 0 &&
                strKeys.IndexOf("RightWin") < 0)
                return strKeys;
                

            string s = strKeys;
            s = s.Replace("[", "");
            s = s.Replace("]", "");

            string[] strArray = s.Split(new char[] { '+' });

            string strStart = "";
            string strEnd = "";
            for (int i=0; i< strArray.Length; i++)
            {
                s = strArray[i];
                if (s == "Shift" ||
                    s == "RightShift" ||
                    s == "Ctrl" ||
                    s == "RightCtrl" ||
                    s == "Alt" ||
                    s == "RightAlt" ||
                    s == "Win" ||
                    s == "RightWin")
                {
                    string code = ("[" + s + "]");
                    strStart += code;

                    code = ("[#" + s + "]");
                    strEnd = code + strEnd;

                }
                else
                {
                    string code = ("[" + s + "]");
                    if (Util.is_special_key(code))
                        strStart += code;
                    else
                        strStart += s;
                }
            }

            return strStart + strEnd;
            
        }

        public static bool operator ==(KeyEditingType code1, KeyEditingType code2)
        {
            return (code1.GetKeyCodeString() == code2.GetKeyCodeString());
        }

        public static bool operator !=( KeyEditingType code1, KeyEditingType code2)
        {
            return (code1.GetKeyCodeString() != code2.GetKeyCodeString());
        }

        public int getKeysCount()
        {
            string s = this.ToString();
            List<string> lst = new List<string>();

            Util.ParseKeyContents(s, lst);
            return lst.Count;
        }

        public bool isDisableEmulateKey()
        {
            string s = this.ToString();
            if (s.Length != 1)
                return false;
            char ch = s[0];

            return isDisableEnumateKey(ch); //20141208
                
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Deployment;
using System.Windows.Forms;
namespace KB9Utility
{
    public class Util
    {
        static double mmpinch
        {
            get { return 25.4; }
        }
        static double WindowResolutionPPI
        {
            get 
            {
                Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                return g.DpiX;
                //return 96; 
            }
        }
        static double MacResolutionPPI
        {
            get { return 72; }
        }

        static double mm2pixel(double mm)
        {
            double d = mm / mmpinch * WindowResolutionPPI;
            return d;
        }
        static double pixel2mm(double pixel)
        {
            double d = pixel / WindowResolutionPPI * mmpinch;
            return d;
        }
        static public Image get_image(string name)
        {
            return (Image)Util.ResourceManager.GetObject(name);
        }
        static private System.Resources.ResourceManager _RM;
        static public System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (_RM == null)
                {
                    _RM = new System.Resources.ResourceManager("KB9Utility.Properties.Resources", Assembly.GetExecutingAssembly());//RetailRes.Properties.Resources));

                }
                return _RM;
            }
            set
            {
                _RM = value;
            }
        }

        static public bool string2bool(string str, bool bdefault)
        {
            bool bresult = false;
            if (bool.TryParse(str, out bresult))
                return bresult;
            else
                return bdefault;

        }

        static public int string2int(string str, int ndefault)
        {
            int nresult = 0;
            if (int.TryParse(str, out nresult))
                return nresult;
            else
                return ndefault;
        }
        /// <summary>
        /// the \n\r as the line one line.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        static public int string2lines(string str, List<string> lines)
        {
            //remove all \r
            string s = str;
            s = s.Replace("\r", "");
            return string2array(s, "\n", lines, true);
          

        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ar"></param>
        /// <returns></returns>
        static public int string2array(string str, string separateChar, List<string> ar, bool bIgnoreEmpty)
        {
            
            string s = str;

            /////
            int nindex = -1;
            while ((nindex = s.IndexOf(separateChar))>=0)
            {
                string ss = s.Substring(0, nindex);
                if (ss.Length > 0)
                {
                    ar.Add(ss);
                }
                else
                {
                    if (!bIgnoreEmpty)
                        ar.Add(ss);
                }
                s = s.Substring(nindex + 1);
            }
            if (s.Length > 0)
            {
                s = s.Replace(separateChar, "");
                if (s.Length > 0)
                {
                    ar.Add(s);
                }
                else
                {
                    if (!bIgnoreEmpty)
                        ar.Add(s);
                }
            }
            else
            {
                if (!bIgnoreEmpty)
                    ar.Add(s);
            }
            return ar.Count;
            
        }

        static public int KB9DPI
        {
            get {
                return 300;
            }
        }

        static public string GetFileName(string pathName)
        {
            string s = pathName;
            int n = s.LastIndexOf("\\");
            if (n >= 0)
                s = s.Substring(n+1);
            n = s.LastIndexOf(".");
            if (n >=0)
            {
                s = s.Substring(0, n);
            }
            return s;
        }



        /// <summary>
        /// parse the text to token.
        ///  save all pieces to lst
        /// </summary>
        /// <param name="text"></param>
        static public int ParseKeyContents(string text, List<string> lst)
        {
            char ch;
            string strToken = "";
            bool bLeftBracket = false;
            bool bCreateToken = false;
            int nIndex = 0;
            while (nIndex < text.Length)
            {
                ch = text[nIndex];
                if (ch == '[')
                {

                    //start new token
                    bCreateToken = false;
                    bLeftBracket = true;
                    strToken += ch;
                    nIndex++;

                }
                else if (ch == ']')
                {
                    //the end of token
                    bCreateToken = true;
                    bLeftBracket = false;
                    strToken += ch;
                    nIndex++;

                }
                else if (ch == '\\')
                {
                    if (NextCharIsSame(text, nIndex, ch))
                    {//"\\\\"
                        strToken = "\\\\";
                        nIndex += 2;
                        bCreateToken = true;
                    }
                    else if (NextCharIsSame(text, nIndex, '['))
                    {
                        strToken = "\\[";
                        nIndex += 2;
                        bCreateToken = true;
                    }
                    else if (NextCharIsSame(text, nIndex, ']'))
                    {
                        strToken = "\\]";
                        nIndex += 2;
                        bCreateToken = true;
                    }


                }
                else
                {
                    if (bLeftBracket)
                    {
                        strToken += ch;
                        nIndex++;
                    }
                    else
                    {
                        strToken = "";
                        strToken += ch;
                        bCreateToken = true;
                        nIndex++;
                    }
                }

                if (bCreateToken)
                {//add token
                    bCreateToken = false;
                    //AddKeyCode(strToken);
                    strToken = strToken.Trim();
                    if (strToken != string.Empty)
                        lst.Add(strToken);
                    strToken = "";

                }
            }

            return lst.Count;
            
        }


       static private bool NextCharIsSame(string text, int nIndex, char ch)
        {
            if (nIndex + 1 >= text.Length)
                return false;
            if (text[nIndex + 1] == ch)
                return true;
            return false;
        }

        //static public string LF
        //{
        //    get
        //    {
        //        return "\r\n";
        //    }
        //}

        static public bool is_special_key(string strKeyContent)
        {
            string[] keys = new string[]{
                                        "[F1]",//	Function key <F1>
                                        "[F2]",//	Function key <F2>
                                        "[F3]",//	Function key <F3>
                                        "[F4]",//	Function key <F4>
                                        "[F5]",//	Function key <F5>
                                        "[F6]",//	Function key <F6>
                                        "[F7]",//	Function key <F7>
                                        "[F8]",//	Function key <F8>
                                        "[F9]",//	Function key <F9>
                                        "[F10]",//	Function key <F10>
                                        "[F11]",//	Function key <F11>
                                        "[F12]",//	Function key <F12>
                                        "[ESC]",//	Function key <Esc>
                                        "[Backspace]",//	key <Backspace>
                                        "[Tab]",//	key <Tab>
                                        "[CapsLock]",//	key <Caps Lock>
                                        "[Enter]",//	key <Enter>
                                        "[Shift]",//	key <Shift> on left
                                        "[RightShift]",//	key <Shift> on right
                                        "[Ctrl]",//	key <Ctrl> on left
                                        "[RightCtrl]",//	key <Ctrl> on right
                                        "[Alt]",//
                                        "[RightAlt]",//	key <Alt> on left key <Alt> on right
                                        "[Win]",//	key <Win> on left
                                        "[RightWin]",//	key <Win> on right
                                        "[#RightShift]",//break code for key <Shift> on right
                                        "[#Ctrl]",//	break code for key <Ctrl> on left
                                        "[#RightCtrl]",//	break code for key <Ctrl> on right
                                        "[#Alt]",//	break code for key <Alt> on left
                                        "[#RightAlt]",//	break code for key <Alt> on right
                                        "[#Win]",//	break code for key <Win> on left
                                        "[#RightWin]",//
                                        "[Space]",//	break code for key <Win> on right key <Space bar>
                                        "[Insert]",//	Function key <Insert>
                                        "[Delete]",//	Function key <Delete>
                                        "[Home]",//	Function key <Home>
                                        "[End]",//	Function key <End>
                                        "[PageUp]",//	Function key <Page Up>
                                        "[PageDown]",//	Function key <Page Down>
                                        "[ArrowUp]",//	Function key <Up Arrow>
                                        "[ArrowLeft]",//	Function key <Left Arrow>
                                        "[ArrowRight]",//	Function key <Right Arrow>
                                        "[ArrowDown]",//	Function key <Down Arrow>
                                        "[NumLock]",//	Function key <Num Lock>
                                        "[ScrollLk]",//	Function key <Scroll Lock>
                                        "[PrintScreen]",//	Function key <Print Screen>
                                        "[PauseBreak]",//	Function key <Pause Break>	
                                        "[Menu]",//	Function key <Menu>
                                        "[Ctrl+PauseBreak]",//	Function key <Ctrl>+<Pause Break>
                                        "[Ctrl+PrintScreen]",//	Function key <Ctrl>+<Print Screen>
                                        "[Alt+PrintScreen]",//	Function key <Alt>+<Print Screen>
                                        "[PAD0]",//	number key <0> on keypad
                                        "[PAD1]",//	number key <1> on keypad
                                        "[PAD2]",//	number key <2> on keypad
                                        "[PAD3]",//	number key <3> on keypad
                                        "[PAD4]",//	number key <4> on keypad
                                        "[PAD5]",//	number key <5> on keypad
                                        "[PAD6]",//	number key <6> on keypad
                                        "[PAD7]",//	number key <7> on keypad
                                        "[PAD8]",//	number key <8> on keypad
                                        "[PAD9]",//	number key <9> on keypad
                                        "[PAD+]",//	number key <+> on keypad
                                        "[PAD-]",//	number key <-> on keypad
                                        "[PAD*]",//	number key <*> on keypad
                                        "[PAD/]",//  number key </> on keypad
                                        "[PAD.]",//	number key <.> on keypad
                                        KB9Const.PAD_Enter,//	number key <Enter> on keypad
                                        "[NextTrack]",//	Next Track
                                        "[PrevTrack]",//	Previous Track
                                        "[Stop]",//	Stop
                                        "[PlayPause]",//	Play / Pause
                                        "[Mute]",//	Mute
                                        "[VolumeUp]",//	Volume Up
                                        "[VolumeDown]",//	Volume Down
                                        "[Mail]",//	Mail
                                        "[Calculator]",//	Calculator
                                        "[MyComputer]",//	My Computer
                                        "[wwwSearch]",//	www Search
                                        "[wwwHome]",//	www Home
                                        "[wwwBack]",//	www Back
                                        "[wwwForward]",//	www Forward
                                        "[wwwStop]",//	www Stop
                                        "[wwwRefresh]",//	www Refresh
                                        "[wwwFavorites]",//	www Favorites
                                        "[SysPower]",//	System Power
                                        "[SysSleep]",//	System Sleep
                                        "[SysWake]",//	System Wake
                                     };

            for (int i=0; i< keys.Length; i++)
            {
                if (keys[i] == strKeyContent)
                    return true;
            }
            return false;

        }

        static public string BracketString(string s)
        {
            return ("[" + s + "]");

        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        /// <summary>
        /// return application path without last "\"
        /// </summary>
        /// <returns></returns>
        static public string GetAppPath()
        {
            string s = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            int n = s.LastIndexOf("\\");
            return s.Substring(0, n);

        }

        static public bool is_combination_content(string str)
        {
            int n = str.IndexOf("+");
            string s = str;
            s = s.Replace("]", "");

            if (n >= 0 && n != (s.Length - 1))
            {
                //if (str.Equals("[PAD+]"))
                //    return false;
                if (str.Length > 3)
                    return true;
            }
            return false;
        }
    }
}

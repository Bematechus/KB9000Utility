using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public class Key2Char
    {
        public Key2Char(Keys key, char ch, char shiftch, char caplockch)
        {
            this._Key = key;
            this._ShiftChar = shiftch;
            this._Char = ch;
            this._CaplockChar = caplockch;
        }
        private Keys _Key;
        public Keys Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        private char _Char;
        public char Char
        {
            get { return _Char; }
            set { _Char = value; }
        }
        private char _ShiftChar;
        public char ShiftChar
        {
            get { return _ShiftChar; }
            set { _ShiftChar = value; }
        }

        private char _CaplockChar;
        public char CaplockChar
        {
            get { return _CaplockChar; }
            set { _CaplockChar = value; }
        }
    }

    public class KB9KeyDefinition
    {
        static public char ConvertKey2Char(KeyEventArgs e)
        {
            Key2Char[] ar = new Key2Char[] {
                new Key2Char( Keys.A, 'a','A','A' ),
                new Key2Char( Keys.B, 'b','B','B' ),
                new Key2Char( Keys.C, 'c','C','C' ),
                new Key2Char( Keys.D, 'd','D','D' ),
                new Key2Char( Keys.E, 'e','E','E' ),
                new Key2Char( Keys.F, 'f','F','F' ),
                new Key2Char( Keys.G, 'g','G','G' ),
                new Key2Char( Keys.H, 'h','H','H' ),
                new Key2Char( Keys.I, 'i','I','I' ),
                new Key2Char( Keys.J, 'j','J','J' ),
                new Key2Char( Keys.K, 'k','K','K' ),
                new Key2Char( Keys.L, 'l','L','L' ),
                new Key2Char( Keys.M, 'm','M','M' ),
                new Key2Char( Keys.N, 'n','N','N' ),
                new Key2Char( Keys.O, 'o','O','O' ),
                new Key2Char( Keys.P, 'p','P','P' ),
                new Key2Char( Keys.Q, 'q','Q','Q' ),
                new Key2Char( Keys.R, 'r','R' ,'R'),
                new Key2Char( Keys.S, 's','S' ,'S'),
                new Key2Char( Keys.T, 't','T','T' ),
                new Key2Char( Keys.U, 'u','U','U' ),
                new Key2Char( Keys.V, 'v','V','V' ),
                new Key2Char( Keys.W, 'w','W','W' ),
                new Key2Char( Keys.X, 'x','X','X' ),
                new Key2Char( Keys.Y, 'y','Y','Y' ),
                new Key2Char( Keys.Z, 'z','Z','Z' ),
                new Key2Char( Keys.Oemtilde, '`','~', '`' ),
                new Key2Char( Keys.D1, '1','!', '1' ),
                new Key2Char( Keys.D2, '2','@' , '2'),
                new Key2Char( Keys.D3, '3','#' , '3'),
                new Key2Char( Keys.D4, '4','$' , '4'),
                new Key2Char( Keys.D5, '5','%' , '5'),
                new Key2Char( Keys.D6, '6','^' , '6'),
                new Key2Char( Keys.D7, '7','&' , '7'),
                new Key2Char( Keys.D8, '8','*' , '8'),
                new Key2Char( Keys.D9, '9','(' , '9'),
                new Key2Char( Keys.D0, '0',')' , '0'),
                new Key2Char( Keys.OemMinus, '-','_', '-' ),
                new Key2Char( (Keys)187, '=','+' , '='),
                new Key2Char( (Keys)220, '\\','|' , '\\'),
                new Key2Char( Keys.OemOpenBrackets, '[','{' , '['),
                new Key2Char( Keys.OemCloseBrackets, ']','}' , ']'),
                new Key2Char( Keys.OemSemicolon, ';',':' , ';'),
                new Key2Char( Keys.OemQuotes, '\'','"' , '\\'),
                new Key2Char( Keys.Oemcomma, ',','<' , ','),
                new Key2Char( Keys.OemPeriod, '.','>' , '.'),
                new Key2Char(( Keys)191, '/','?' , '/'),
                

                };
            bool bCapsLock = Console.CapsLock;

            Keys key = e.KeyCode;
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i].Key == key)
                {
                    if (e.Shift )//|| bCapsLock)
                        return ar[i].ShiftChar;
                    if (bCapsLock)
                        return ar[i].CaplockChar;
                    else
                        return ar[i].Char;
                }
            }
            return (char)(0);
        }

        static public string KeyUpDisplayName(Keys key)
        {
            string s = KeyName(key);
            if (s != string.Empty)
            {
                s = "[#" + s + "]";
            }
            return s;
        }

        static public string KeyDisplayName(Keys key)
        {
            string s = KeyName(key);
            if (s != string.Empty)
            {
                s = "[" + s + "]";
            }
            return s;
        }
        static public string KeyName(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                    return "ESC";
                case Keys.Back:
                    return "Backspace";
                case Keys.Tab:
                    return "Tab";
                case Keys.CapsLock:
                    return "CapsLock";
                case Keys.Enter:
                    return "Enter";
                case Keys.ShiftKey:
                case Keys.Shift:
                    return "Shift";
                case Keys.RShiftKey:
                    return "RightShift";
                case Keys.ControlKey:
                case Keys.Control:
                    return "Ctrl";
                case Keys.RControlKey:
                    return "RightCtrl";
                case Keys.LMenu:
                case Keys.Menu:
                    return "Alt";
                case Keys.RMenu:
                    return "RightAlt";
                //case Keys.Space:
                //case Keys.Insert:
                //case Keys.Delete:
                //case Keys.Home:
                //case Keys.End:
                
                //case Keys.NumLock:
                //case Keys.PrintScreen:
                //    return key.ToString();
                case Keys.PageDown:
                    return "PageDown";
                case Keys.PageUp:
                    return "PageUp";
                case Keys.Up:
                    return "ArrowUp";
                case Keys.Down:
                    return "ArrowDown";
                case Keys.Left:
                    return "ArrowLeft";
                case Keys.Right:
                    return "ArrowRight";
                case Keys.Scroll:
                    return "ScrollLk";
                case Keys.Pause:
                    return "PauseBreak";
                case Keys.LWin:
                    return "Win";
                case Keys.RWin:
                    return "RightWin";
                case Keys.Apps:
                    return "Menu";
                case Keys.NumPad0:
                    return "PAD0";
                case Keys.NumPad1:
                    return "PAD1";
                case Keys.NumPad2:
                    return "PAD2";
                case Keys.NumPad3:
                    return "PAD3";
                case Keys.NumPad4:
                    return "PAD4";
                case Keys.NumPad5:
                    return "PAD5";
                case Keys.NumPad6:
                    return "PAD6";
                case Keys.NumPad7:
                    return "PAD7";
                case Keys.NumPad8:
                    return "PAD8";
                case Keys.NumPad9:
                    return "PAD9";
                case Keys.Subtract:
                    return "PAD-";
                case Keys.Multiply:
                    return "PAD*";
                case Keys.Divide:
                    return "PAD/";
                case Keys.Add:
                    return "PAD+";
                case Keys.Decimal:
                    return "PAD.";
                default:
                    return key.ToString();
            }
            
        }

        static protected bool IsFunctionsKey(Keys key)
        {
            int n = (int)key;

            if (key >= Keys.F1 && key <= Keys.F24)
            {

                return true;
            }
            else if (key == Keys.Escape ||
                    key == Keys.Back ||
                    key == Keys.Tab ||
                    key == Keys.Capital ||
                    key == Keys.Enter ||
                // key == Keys.Shift ||
                // key == Keys.RShiftKey ||
                    key == Keys.ControlKey ||
                   key == Keys.RControlKey ||
                   key == Keys.Alt ||
                   key == Keys.RMenu ||
                   key == Keys.Space ||
                   key == Keys.Insert ||
                   key == Keys.Home ||
                   key == Keys.End ||
                   key == Keys.PageUp ||
                   key == Keys.PageDown ||
                   key == Keys.Up ||
                   key == Keys.Down ||
                   key == Keys.Left ||
                   key == Keys.Right ||
                   key == Keys.NumLock ||
                   key == Keys.Scroll ||
                   key == Keys.PrintScreen ||
                   key == Keys.Pause ||
                   key == Keys.LWin ||
                   key == Keys.RWin ||
                   key == Keys.Menu ||
                   key == Keys.Apps)
                return true;
            return false;

        }


        static protected bool IsTestKbFunctionsKey(Keys key)
        {
            int n = (int)key;

            if (key >= Keys.F1 && key <= Keys.F24)
            {

                return true;
            }
            else if (key == Keys.Escape ||
                    key == Keys.Back ||
                    key == Keys.Tab ||
                    key == Keys.Capital ||
                    key == Keys.Enter ||
                    key == Keys.Shift ||
                    key == Keys.ShiftKey ||
                    key == Keys.RShiftKey ||
                    key == Keys.ControlKey ||
                   key == Keys.RControlKey ||
                   key == Keys.Alt ||
                   key == Keys.RMenu ||
                   key == Keys.Space ||
                   key == Keys.Insert ||
                   key == Keys.Home ||
                   key == Keys.End ||
                   key == Keys.PageUp ||
                   key == Keys.PageDown ||
                   key == Keys.Up ||
                   key == Keys.Down ||
                   key == Keys.Left ||
                   key == Keys.Right ||
                   key == Keys.NumLock ||
                   key == Keys.Scroll ||
                   key == Keys.PrintScreen ||
                   key == Keys.Pause ||
                   key == Keys.LWin ||
                   key == Keys.RWin ||
                   key == Keys.Menu ||
                   key == Keys.Left ||
                   key == Keys.Right ||
                   key == Keys.Up||
                   key == Keys.Down ||
                   key == Keys.Delete ||
                   key == Keys.Back ||
                   key == Keys.Apps)
                return true;
            return false;

        }

        static protected  bool IsPadNumberKey(Keys key)
        {
            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                return true;
            if (key == Keys.Subtract ||
               key == Keys.Multiply ||
               key == Keys.Divide ||
               key == Keys.Add ||
               key == Keys.Decimal)
            {
                return true;
            }
            return false;
        }

        static public bool IsHandledKey(Keys key)
        {
            if (IsFunctionsKey(key) ||
                IsPadNumberKey(key))
                return true;
            return false;
        }

        static public bool IsTestKbHandledKey(Keys key)
        {
            if (IsTestKbFunctionsKey(key) ||
                IsPadNumberKey(key))
                return true;
            return false;
        }
    }
}

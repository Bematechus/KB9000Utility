using System;
using System.Collections.Generic;
using System.Text;

namespace KB9Utility
{
    public class KB9Const
    {
        //inter char delay
        public const int DEFAULT_InterCharDelay = 2;
        public const int Min_InterCharDelay = 1;
        public const int Max_InterCharDelay = 100;
        //touch sensitivity
        public const int DEFAULT_Sensitivity = 5;//20140408, change from 50, to 5
        public const int Min_Sensitivity = 1;
        public const int Max_Sensitivity = 10;
        //touch delay
        public const int DEFAULT_TouchDelay = 5; ////20140408, change from 50, to 5
        public const int Min_TouchDelay = 1;
        public const int Max_TouchDelay = 10;
        //beep duration
        public const int DEFAULT_BeepDuration = 3;
        public const int Min_BeepDuration = 0;
        public const int Max_BeepDuration = 10;
        //beep pitch
        public const int DEFAULT_BeepPitch = 5;
        public const int Min_BeepPitch = 1;
        public const int Max_BeepPitch = 10;


        public const string  CSV_Macro_Flag = "[0,0,1,1][0][0]";
        public const string LF = "\r\n";
        public const string CSV_SlideH = "SlideH";
        public const string CSV_SlideV = "SlideV";
        public const string CSV_SlideL = "[SlideL]";
        public const string CSV_SlideU = "[SlideU]";

        public const float UNITS_MM = 0.5F;//0.5mm
        public const float MM_MARGIN = 5.0F;

        public const int UNITS_CX = 372; //(176mm + 10mm) / 0.5mm
        public const int UNITS_CY = 148; //( 74mm + 10mm) /0.5m
        public const int UNITS_MARGIN = 10; // 5mm / 0.5mm
        public const int UNITS_MAX_WIDTH = 352; //176mm / 0.5mm
        public const int UNITS_MAX_HEIGHT = 128; //64mm / 0.5mm

        public const int UNIT_PIXELS = 6;
        public const int PIXELS_CX = 2232; //UNITS_CX * UNIT_PIXELS
        public const int PIXELS_CY = 888; // UNITS_CY * UNIT_PIXELS
        public const int PIXELS_MARGIN = 60; // UNITS_MARGIN * UNIT_PIXELS

        public const int IMAGE_CX = 2197;//186mm/25.4 * 300
        public const int IMAGE_CY = 874;//74mm/25.4 * 300
        public const int IMAGE_DPI = 300;


        public const int MARKER_SIZE = 6;

        public const string CTRL_DOWN = "[Ctrl]";
        public const string CTRL_UP = "[#Ctrl]";
        public static string Ctrl_DownUp()
        {
            return CTRL_DOWN + CTRL_UP;
        }

        public const string RCTRL_DOWN = "[RightCtrl]";
        public const string RCTRL_UP = "[#RightCtrl]";
        static public string RCtrl_DownUp()
        {
            return RCTRL_DOWN + RCTRL_UP;
        }


        public const string ALT_DOWN = "[Alt]";
        public const string ALT_UP = "[#Alt]";
        static public string Alt_DownUp()
        {
            return ALT_DOWN + ALT_UP;
        }
        public const string RALT_DOWN = "[RightAlt]";
        public const string RALT_UP = "[#RightAlt]";
        static public string RAlt_DownUp()
        {
            return RALT_DOWN + RALT_UP;
        }

        public const string SHIFT_DOWN = "[Shift]";
        public const string SHIFT_UP = "[#Shift]";
        static public string Shift_DownUp()
        {
            return SHIFT_DOWN + SHIFT_UP;
        }

        public const string RSHIFT_DOWN = "[RightShift]";
        public const string RSHIFT_UP = "[#RightShift]";
        static public string RShift_DownUp()
        {
            return RSHIFT_DOWN + RSHIFT_UP;
        }
        public const string WIN_DOWN = "[Win]";
        public const string WIN_UP = "[#Win]";
        static public string Win_DownUp()
        {
            return WIN_DOWN + WIN_UP;
        }
        public const string RWIN_DOWN = "[RightWin]";
        public const string RWIN_UP = "[#RightWin]";
        static public string RWin_DownUp()
        {
            return RWIN_DOWN + RWIN_UP;
        }

        public const string PAD_Enter = "[PADEnter]";

    }
}

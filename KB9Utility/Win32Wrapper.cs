using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ColorTextBoxControl {

    /// <summary>
    /// Provides flags for the ExtTexOut function
    /// </summary>
    [Flags]
    //public enum ETOOptions : uint {
    //    ETO_CLIPPED = 0x4,
    //    ETO_GLYPH_INDEX = 0x10,
    //    ETO_IGNORELANGUAGE = 0x1000,
    //    ETO_NUMERICSLATIN = 0x800,
    //    ETO_NUMERICSLOCAL = 0x400,
    //    ETO_OPAQUE = 0x2,
    //    ETO_PDY = 0x2000,
    //    ETO_RTLREADING = 0x800,
    //}

    /// <summary>
    /// This enumeration encapsulates all constant window message definitions.
    /// </summary>
    /// 

    //public enum WndMsg {
    //    WM_NULL                 =       0x0000,
    //    WM_CREATE               =       0x0001,
    //    WM_DESTROY              =       0x0002,
    //    WM_MOVE                 =       0x0003,
    //    WM_SIZE                 =       0x0005,
    //    WM_ACTIVATE             =       0x0006,
    //    WM_SETFOCUS             =       0x0007,
    //    WM_KILLFOCUS            =       0x0008,
    //    WM_ENABLE               =       0x000A,
    //    WM_SETREDRAW            =       0x000B,
    //    WM_SETTEXT              =       0x000C,
    //    WM_GETTEXT              =       0x000D,
    //    WM_GETTEXTLENGTH        =       0x000E,
    //    WM_PAINT                =       0x000F,
    //    WM_CLOSE                =       0x0010,
        
    //    /* #ifndef _WIN32_WCE */
    //    WM_QUERYENDSESSION      =       0x0011,
    //    WM_QUERYOPEN            =       0x0013,
    //    WM_ENDSESSION           =       0x0016,

    //    WM_QUIT                 =       0x0012,
    //    WM_ERASEBKGND           =       0x0014,
    //    WM_SYSCOLORCHANGE       =       0x0015,
    //    WM_SHOWWINDOW           =       0x0018,
    //    WM_WININICHANGE         =       0x001A,

    //    /* #if(WINVER >= 0x0400) */
    //    WM_SETTINGCHANGE        =       WM_WININICHANGE,

    //    WM_DEVMODECHANGE        =       0x001B,
    //    WM_ACTIVATEAPP          =       0x001C,
    //    WM_FONTCHANGE           =       0x001D,
    //    WM_TIMECHANGE           =       0x001E,
    //    WM_CANCELMODE           =       0x001F,
    //    WM_SETCURSOR            =       0x0020,
    //    WM_MOUSEACTIVATE        =       0x0021,
    //    WM_CHILDACTIVATE        =       0x0022,
    //    WM_QUEUESYNC            =       0x0023,

    //    WM_GETMINMAXINFO        =       0x0024
    //}

    /***** Enumerations used in logical fonts: *****/

    public enum FontWeight : int {
        FW_DONTCARE = 0,
        FW_THIN = 100,
        FW_EXTRALIGHT = 200,
        FW_LIGHT = 300,
        FW_NORMAL = 400,
        FW_MEDIUM = 500,
        FW_SEMIBOLD = 600,
        FW_BOLD = 700,
        FW_EXTRABOLD = 800,
        FW_HEAVY = 900,
    }

    public enum FontCharSet : byte {
        ANSI_CHARSET = 0,
        DEFAULT_CHARSET = 1,
        SYMBOL_CHARSET = 2,
        SHIFTJIS_CHARSET = 128,
        HANGEUL_CHARSET = 129,
        HANGUL_CHARSET = 129,
        GB2312_CHARSET = 134,
        CHINESEBIG5_CHARSET = 136,
        OEM_CHARSET = 255,
        JOHAB_CHARSET = 130,
        HEBREW_CHARSET = 177,
        ARABIC_CHARSET = 178,
        GREEK_CHARSET = 161,
        TURKISH_CHARSET = 162,
        VIETNAMESE_CHARSET = 163,
        THAI_CHARSET = 222,
        EASTEUROPE_CHARSET = 238,
        RUSSIAN_CHARSET = 204,
        MAC_CHARSET = 77,
        BALTIC_CHARSET = 186,
    }

    public enum FontPrecision : byte {
        OUT_DEFAULT_PRECIS = 0,
        OUT_STRING_PRECIS = 1,
        OUT_CHARACTER_PRECIS = 2,
        OUT_STROKE_PRECIS = 3,
        OUT_TT_PRECIS = 4,
        OUT_DEVICE_PRECIS = 5,
        OUT_RASTER_PRECIS = 6,
        OUT_TT_ONLY_PRECIS = 7,
        OUT_OUTLINE_PRECIS = 8,
        OUT_SCREEN_OUTLINE_PRECIS = 9,
        OUT_PS_ONLY_PRECIS = 10,
    }

    public enum FontClipPrecision : byte {
        CLIP_DEFAULT_PRECIS = 0,
        CLIP_CHARACTER_PRECIS = 1,
        CLIP_STROKE_PRECIS = 2,
        CLIP_MASK = 0xf,
        CLIP_LH_ANGLES = (1 << 4),
        CLIP_TT_ALWAYS = (2 << 4),
        CLIP_DFA_DISABLE = (4 << 4),
        CLIP_EMBEDDED = (8 << 4),
    }

    public enum FontQuality : byte {
        DEFAULT_QUALITY = 0,
        DRAFT_QUALITY = 1,
        PROOF_QUALITY = 2,
        NONANTIALIASED_QUALITY = 3,
        ANTIALIASED_QUALITY = 4,
        CLEARTYPE_QUALITY = 5,
        CLEARTYPE_NATURAL_QUALITY = 6,
    }

    [Flags]
    public enum FontPitchAndFamily : byte {
        DEFAULT_PITCH = 0,
        FIXED_PITCH = 1,
        VARIABLE_PITCH = 2,
        FF_DONTCARE = (0 << 4),
        FF_ROMAN = (1 << 4),
        FF_SWISS = (2 << 4),
        FF_MODERN = (3 << 4),
        FF_SCRIPT = (4 << 4),
        FF_DECORATIVE = (5 << 4),
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class LOGFONT {
        [MarshalAs(UnmanagedType.I4)]
        public int lfHeight = 0;
        [MarshalAs(UnmanagedType.I4)]
        public int lfWidth = 0;
        [MarshalAs(UnmanagedType.I4)]
        public int lfEscapement = 0;
        [MarshalAs(UnmanagedType.I4)]
        public int lfOrientation = 0;
        [MarshalAs(UnmanagedType.I4)]
        public int lfWeight = 0;
        public byte lfItalic = 0;
        public byte lfUnderline = 0;
        public byte lfStrikeOut = 0;
        public byte lfCharSet = 0;
        public byte lfOutPrecision = 0;
        public byte lfClipPrecision = 0;
        public byte lfQuality = 0;
        public byte lfPitchAndFamily = 0;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName = string.Empty;

        public LOGFONT(string lfFaceName) {
            this.lfFaceName = lfFaceName;
            lfHeight = lfWidth = lfEscapement = lfOrientation = 0;
            lfWeight = (int)FontWeight.FW_NORMAL;
            lfItalic = lfUnderline = lfStrikeOut = 0;
            lfCharSet = (byte)FontCharSet.DEFAULT_CHARSET;
            lfOutPrecision = (byte)FontPrecision.OUT_DEFAULT_PRECIS;
            lfClipPrecision = (byte)FontClipPrecision.CLIP_DEFAULT_PRECIS;
            lfQuality = (byte)FontQuality.CLEARTYPE_QUALITY;
            lfPitchAndFamily = (byte)(FontPitchAndFamily.DEFAULT_PITCH | FontPitchAndFamily.FF_DONTCARE);
        }

    }

    /// <summary>
    /// A size structure to be used in Win32 API calls (similar to the .Net Size struct)
    /// </summary>
    public struct SIZE {
        [MarshalAs(UnmanagedType.I4)]
        public int cx;
        [MarshalAs(UnmanagedType.I4)]
        public int cy;
    }

    /// <summary>
    /// A rectangle structure to be used in Win32 API calls (similar to the .Net Rectangle struct)
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT {
        [MarshalAs(UnmanagedType.I4)]
        public int Left;
        [MarshalAs(UnmanagedType.I4)]
        public int Top;
        [MarshalAs(UnmanagedType.I4)]
        public int Right;
        [MarshalAs(UnmanagedType.I4)]
        public int Bottom;

        public RECT(int left_, int top_, int right_, int bottom_) {
            Left = left_;
            Top = top_;
            Right = right_;
            Bottom = bottom_;
        }

        public int Height { get { return Bottom - Top + 1; } }
        public int Width { get { return Right - Left + 1; } }
        public Size Size { get { return new Size(Width, Height); } }
        public Point Location { get { return new Point(Left, Top); } }

        /// <summary>
        /// Converts this RECT structure to a System.Drawing.Rectangle
        /// </summary>
        /// <returns>A Rectangle corresponding to this RECT structure.</returns>
        public Rectangle ToRectangle() { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }

        public static RECT FromRectangle(Rectangle rectangle) {
            return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        public override int GetHashCode() {
            return Left ^ ((Top << 13) | (Top >> 0x13))
              ^ ((Width << 0x1a) | (Width >> 6))
              ^ ((Height << 7) | (Height >> 0x19));
        }

        #region Operator overloads

        public static implicit operator Rectangle(RECT rect) {
            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public static implicit operator RECT(Rectangle rect) {
            return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /*public override bool Equals(object obj) {
            if (obj is RECT) {
                RECT comp = (RECT)obj;
                if (comp.Left == this.Left && comp.Top == this.Top &&
                    comp.Right == this.Right && comp.Bottom == this.Bottom) return true;
            }
            return false;
        }
        */

        #endregion
    }


    /// <summary>
    /// This class acts as a wrapper for "gdi32.dll" API calls. It defines some static methods to be used from
    /// within the ColorTextBox control for rendering.
    /// </summary>
    public class PlatformInvokeGDI32 {

        #region Class Variables
        public const int SRCCOPY = 13369376;
        #endregion

        #region Class Functions

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        internal static extern IntPtr DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        internal static extern IntPtr DeleteObject(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BitBlt(IntPtr hdcDest,
            [MarshalAs(UnmanagedType.I4)]int xDest,
            [MarshalAs(UnmanagedType.I4)]int yDest,
            [MarshalAs(UnmanagedType.I4)]int wDest,
            [MarshalAs(UnmanagedType.I4)]int hDest, 
            IntPtr hdcSource,
            [MarshalAs(UnmanagedType.I4)]int xSrc,
            [MarshalAs(UnmanagedType.I4)]int ySrc,
            [MarshalAs(UnmanagedType.I4)]int RasterOp);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        internal static extern IntPtr CreateCompatibleBitmap(
            IntPtr hdc,
            [MarshalAs(UnmanagedType.I4)]int nWidth,
            [MarshalAs(UnmanagedType.I4)]int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateBitmap")]
        internal static extern IntPtr CreateBitmap(
            [MarshalAs(UnmanagedType.I4)]Int32 width,
            [MarshalAs(UnmanagedType.I4)]Int32 height,
            [MarshalAs(UnmanagedType.U4)]UInt32 planes,
            [MarshalAs(UnmanagedType.U4)]UInt32 bpp, 
            IntPtr bits);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "ExtTextOutW")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ExtTextOut(
            IntPtr hdc,
            [MarshalAs(UnmanagedType.I4)]Int32 X,
            [MarshalAs(UnmanagedType.I4)]Int32 Y,
            [MarshalAs(UnmanagedType.U4)]UInt32 fuOptions,
            [In, MarshalAs(UnmanagedType.Struct)] ref RECT lprc,
            [MarshalAs(UnmanagedType.LPWStr)]string lpString,
            [MarshalAs(UnmanagedType.U4)]UInt32 cbCount,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)]int[] lpDx);

        [DllImport("gdi32.dll", EntryPoint = "TextOutW")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool TextOut(
            IntPtr hdc,
            [MarshalAs(UnmanagedType.I4)]int nXStart,
            [MarshalAs(UnmanagedType.I4)]int nYStart,
            [MarshalAs(UnmanagedType.LPWStr)]string lpString,
            [MarshalAs(UnmanagedType.I4)]int cbString);

        [DllImport("gdi32.dll", EntryPoint = "GetTextExtentPointW")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetTextExtentPoint(
            IntPtr hdc,
            [MarshalAs(UnmanagedType.LPWStr)]string lpString,
            [MarshalAs(UnmanagedType.I4)]int cbString,
            [MarshalAs(UnmanagedType.Struct)]ref Size lpSize);

        [DllImport("gdi32.dll", EntryPoint = "SetTextColor")]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern uint SetTextColor(
            IntPtr hdc,
            [MarshalAs(UnmanagedType.I4)]int crColor);

        [DllImport("gdi32.dll", EntryPoint = "SetBkColor")]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern int SetBkColor(
            IntPtr hdc,
            [MarshalAs(UnmanagedType.I4)]int crColor);

        [DllImport("gdi32.dll", EntryPoint = "CreateFontIndirectW")]
        internal static extern IntPtr CreateFontIndirect(
            [In, MarshalAs(UnmanagedType.Struct)] ref LOGFONT lplf);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        internal static extern IntPtr SelectObject(
            IntPtr hdc, 
            IntPtr hgdiobj);

        //public static extern bool InvertRect(IntPtr hDC, ref System.Drawing.Rectangle lprc);

        #endregion
    }

    /// <summary>
    /// This class acts as a wrapper for "user32.dll" API calls. It defines some static methods to be used from
    /// within the ColorTextBox control for handling device contexts.
    /// </summary>
    public class PlatformInvokeUSER32 {

        #region Class Variables
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        #endregion

        #region Class Functions
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        internal static extern IntPtr GetDC(
            IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        internal static extern int GetSystemMetrics(
            [MarshalAs(UnmanagedType.I4)]int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        internal static extern IntPtr GetWindowDC(
            [MarshalAs(UnmanagedType.I4)]Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        internal static extern IntPtr ReleaseDC(
            IntPtr hWnd, 
            IntPtr hDc);

        [DllImport("user32.dll", EntryPoint = "InvertRect")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InvertRect(
            IntPtr hDC, 
            [In, MarshalAs(UnmanagedType.Struct)]ref RECT lprc);

        #endregion
    }
 
}

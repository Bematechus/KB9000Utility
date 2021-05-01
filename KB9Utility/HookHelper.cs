using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace KB9Utility
{
    public class HookHelper
    {
         #region Delegates

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        #endregion

        #region ��������

        private HookProc KeyboardHookProcedure;
        private FileStream MyFs; // ����������ctrl alt delete

        private const byte LLKHF_ALTDOWN = 0x20;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_ESCAPE = 0x1B;
        private const byte VK_F4 = 0x73;
        private const byte VK_LCONTROL = 0xA2;
        private const byte VK_NUMLOCK = 0x90;
        private const byte VK_RCONTROL = 0xA3;
        private const byte VK_SHIFT = 0x10;
        private const byte VK_TAB = 0x09;
        public const int WH_KEYBOARD = 13;
        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE = 7;
        private const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;
        private static int hKeyboardHook = 0;

        #endregion

        #region ����ת��

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //   ж�ع���  

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //   ������һ������  

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        //   ȡ�õ�ǰ�̱߳��  

        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        #endregion

        #region ����

        /// <summary>
        /// ���ӻص������������������ȼ���
        /// <remark> 
        /// Author:ZhangRongHua 
        /// Create DateTime: 2009-6-19 20:19
        /// Update History:     
        ///  </remark>
        /// </summary>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w param.</param>
        /// <param name="lParam">The l param.</param>
        /// <returns></returns>
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            KeyMSG m = (KeyMSG) Marshal.PtrToStructure(lParam, typeof (KeyMSG));

            if (((Keys) m.vkCode == Keys.LWin) || ((Keys) m.vkCode == Keys.RWin) ||
                ((m.vkCode == VK_TAB) && ((m.flags & LLKHF_ALTDOWN) != 0)) ||
                ((m.vkCode == VK_ESCAPE) && ((m.flags & LLKHF_ALTDOWN) != 0)) ||
                ((m.vkCode == VK_F4) && ((m.flags & LLKHF_ALTDOWN) != 0)) ||
                (m.vkCode == VK_ESCAPE) && ((GetKeyState(VK_LCONTROL) & 0x8000) != 0) ||
                (m.vkCode == VK_ESCAPE) && ((GetKeyState(VK_RCONTROL) & 0x8000) != 0)
                )
            {
                return 1;
            }

            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }

         
        /// <summary>
        /// ����Hook���������������������
        /// <remark> 
        /// Author:ZhangRongHua 
        /// Create DateTime: 2009-6-19 20:20
        /// Update History:     
        ///  </remark>
        /// </summary>
        public void HookStart()
        {
            if (hKeyboardHook == 0)
            {
                //   ����HookProcʵ��  

                KeyboardHookProcedure = new HookProc(KeyboardHookProc);

                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD,
                                                 KeyboardHookProcedure,
                                                 Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                                                 0);

                //   ������ù���ʧ��  

                if (hKeyboardHook == 0)
                {
                    HookStop();

                    //throw new Exception("SetWindowsHookEx   failedeeeeeeee.");
                }

                //�ö��������ķ�������������������Ҳ��ر���.��������������ʹ򿪲���
                MyFs = new FileStream(Environment.ExpandEnvironmentVariables("%windir%\\system32\\taskmgr.exe"),
                                      FileMode.Open);
                byte[] MyByte = new byte[(int) MyFs.Length];
                MyFs.Write(MyByte, 0, (int) MyFs.Length);
            }
        }



        /// <summary>
        /// ж��hook,���ر�����ȡ�����������������
        /// <remark> 
        /// Author:ZhangRongHua 
        /// Create DateTime: 2009-6-19 20:21
        /// Update History:     
        ///  </remark>
        /// </summary>
        public void HookStop()
        {
            bool retKeyboard = true;

            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);

                hKeyboardHook = 0;
            }

            if (null != MyFs)
            {
                MyFs.Close();
            }

            if (!(retKeyboard))
            {
                throw new Exception("UnhookWindowsHookEx     failedsssss.");
            }
        }

        #endregion

        #region Nested type: KeyMSG

        public struct KeyMSG
        {
            public int dwExtraInfo;
            public int flags;
            public int scanCode;

            public int time;
            public int vkCode;
        }

        #endregion
    }

    
}

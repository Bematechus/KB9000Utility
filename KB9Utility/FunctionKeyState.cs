using System;
using System.Collections.Generic;
using System.Text;

namespace KB9Utility
{
    public class FunctionKeyState
    {
        private bool _LShiftDown = false;
        public bool LShiftDown
        {
            get 
            {
                return _LShiftDown;
            }
            set
            {
                _LShiftDown = value;
            }
        }

        private bool _RShiftDown = false;
        public bool RShiftDown
        {
            get
            {
                return _RShiftDown;
            }
            set
            {
                _RShiftDown = value;
            }
        }


        private bool _LAltDown = false;
        public bool LAltDown
        {
            get
            {
                return _LAltDown;
            }
            set
            {
                _LAltDown = value;
            }
        }


        private bool _RAltDown = false;
        public bool RAltDown
        {
            get
            {
                return _RAltDown;
            }
            set
            {
                _RAltDown = value;
            }
        }

        private bool _LCtrlDown = false;
        public bool LCtrlDown
        {
            get
            {
                return _LCtrlDown;
            }
            set
            {
                _LCtrlDown = value;
            }
        }

        private bool _RCtrlDown = false;
        public bool RCtrlDown
        {
            get
            {
                return _RCtrlDown;
            }
            set
            {
                _RCtrlDown = value;
            }
        }

        private bool _LWinDown = false;
        public bool LWinDown
        {
            get
            {
                return _LWinDown;
            }
            set
            {
                _LWinDown = value;
            }
        }

        private bool _RWinDown = false;
        public bool RWinDown
        {
            get
            {
                return _RWinDown;
            }
            set
            {
                _RWinDown = value;
            }
        }

    }
}

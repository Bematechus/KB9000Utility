using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace KB9Utility
{
    public class KbdState
    {
        private Dictionary<Keys, bool> _KbdState = new Dictionary<Keys, bool>();

        public KbdState()
        {
            reset();
        }

        public void set_down(Keys key)
        {
            _KbdState[key] = true;
        }
        public void set_up(Keys key)
        {
            _KbdState[key] = false;
        }
        public bool is_down(Keys key)
        {
            if (_KbdState.ContainsKey(key))
                return _KbdState[key];
            else
                return false;
        }
        public void reset()
        {
            _KbdState.Clear();
        }
    }
}

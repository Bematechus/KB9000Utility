using System;
using System.Collections.Generic;
using System.Text;

namespace KB9Utility
{
    class COnScreenKbdKey
    {
        public string m_strText;
        public string m_strCode;
        public string m_strShiftText;
        public string m_strShiftCode;
        public string m_strUpCode;

        public COnScreenKbdKey(string strText, string strCode, string strShiftText, string strShiftCode, string strUpCode)
        {
            m_strText = strText;
             m_strCode = strCode;
             m_strShiftText = strShiftText;
             m_strShiftCode = strShiftCode;
             m_strUpCode = strUpCode;
        }

        public COnScreenKbdKey(string strText, string strCode, string strShiftText, string strShiftCode)
        {
            m_strText = strText;
            m_strCode = strCode;
            m_strShiftText = strShiftText;
            m_strShiftCode = strShiftCode;
            m_strUpCode = "";
        }
    }
}

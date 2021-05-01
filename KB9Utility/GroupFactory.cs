using System;
using System.Collections.Generic;
using System.Text;

namespace KB9Utility
{
    public class GroupFactory
    {

        static private int m_sCurrentGroup = 0;

        public static int GetNewGroup()
        {
            m_sCurrentGroup++;
	        return m_sCurrentGroup;
        }
	    
    }
}

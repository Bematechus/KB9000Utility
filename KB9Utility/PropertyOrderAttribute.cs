//
// (C) Paul Tingey 2004 
//
using System;

namespace KB9Utility
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        //
        // Simple attribute to allow the order of a property to be specified
        //
        private int order;
		
        public PropertyOrderAttribute(int order)
        {
            this.order = order;
        }

        public int Order
        {
            get
            {
                return order;
            }
        }
    }
}

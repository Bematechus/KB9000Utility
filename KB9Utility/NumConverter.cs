using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace KB9Utility
{
    public class NumConverter : ExpandableObjectConverter
    {
       
            public override bool CanConvertTo(ITypeDescriptorContext context,
                                   System.Type destinationType)
            {
                if (destinationType == typeof(int))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }
            
            //
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
            {
                try
                {
                    if (destinationType == typeof(System.String) && value is int)
                    {
                        return value.ToString();
                    }
                    return base.ConvertTo(context, culture, value, destinationType);
                }
                catch
                {
                    throw new ArgumentException("Value is error.");
                }
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
            {
                if (sourceType == typeof(string) ||
                    sourceType == typeof(int))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                if (value is string)
                {

                    string s = (string)value;
                    int n = int.Parse(s);
                    return n;

                }
                //if (value is int)
                //{
                //    return value;
                //}
                return value;

            }
            catch
            {
                throw new ArgumentException("Value is error.");
            }
        }   
    }
}

using System;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

public class RectangleFConverter : TypeConverter
{

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        string str = value as string;
        if (value == null) return base.ConvertFrom(context, culture, value);
        str = str.Trim();
        if (str.Length == 0) return null;
        if (culture == null) culture = CultureInfo.CurrentCulture;
        char ch = culture.TextInfo.ListSeparator[0];
        string[] strArray = str.Split(new char[] { ch });
        float[] numArray = new float[strArray.Length];
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
        for (int i = 0; i < numArray.Length; i++)
        {
            numArray[i] = (float)converter.ConvertFromString(context, culture, strArray[i]);
        }
        if (numArray.Length != 4) throw new ArgumentException("Invalid format");
        return new RectangleF(numArray[0], numArray[1], numArray[2], numArray[3]);

    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == null) throw new ArgumentNullException("destinationType");
        if (value is RectangleF)
        {
            if (destinationType == typeof(string))
            {
                RectangleF rect = (RectangleF)value;
                if (culture == null) culture = CultureInfo.CurrentCulture;
                string separator = culture.TextInfo.ListSeparator + " ";
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
                string[] strArray = new string[4];
                int num = 0;
                strArray[num++] = converter.ConvertToString(context, culture, rect.X);
                strArray[num++] = converter.ConvertToString(context, culture, rect.Y);
                strArray[num++] = converter.ConvertToString(context, culture, rect.Width);
                strArray[num++] = converter.ConvertToString(context, culture, rect.Height);

                return string.Join(separator, strArray);
            }
            if (destinationType == typeof(InstanceDescriptor))
            {
                RectangleF rect2 = (RectangleF)value;
                ConstructorInfo constructor = typeof(RectangleF).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) });
                if (constructor != null) return new InstanceDescriptor(constructor, new object[] { rect2.X, rect2.Y, rect2.Width, rect2.Height});
            }
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
        if (propertyValues == null) throw new ArgumentNullException("propertyValues");
        object xvalue = propertyValues["X"];
        object yvalue = propertyValues["Y"];
        object wvalue = propertyValues["Width"];
        object hvalue = propertyValues["Height"];

        if (((xvalue == null) || (yvalue == null)) || (!(xvalue is float) || !(yvalue is float)) ||
            (wvalue == null) || hvalue == null || (!(wvalue is float) || !(hvalue is float)))
        {
            throw new ArgumentException("Invalid property value entry");
        }
        return new RectangleF((float)xvalue, (float)yvalue, (float)wvalue, (float)hvalue);
    }

    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    {
        return true;
    }

    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
        return TypeDescriptor.GetProperties(typeof(RectangleF), attributes).Sort(new string[] { "X", "Y", "Width", "Height" });
    }

    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
}
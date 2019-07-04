using System;
using System.Collections.Generic;
using System.Text;
//using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace KB9Utility
{
    public class ColorEditorUI : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            if (value == null)
                cd.Color = Color.Gray;
            else
                cd.Color = (Color)value;
            if (cd.ShowDialog() == DialogResult.OK)
                return cd.Color;
            else
                return value;
        }
    }
}

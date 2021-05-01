using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;

namespace KB9Utility
{
    class KeyCodeEditor : UITypeEditor
    {
        //public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        //{
        //    return true;
        //}

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {


            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }



            frmKeyCode frm = new frmKeyCode();
            string inputed = frm.InputKeyCode(value.ToString());
            return (new KeyEditingType(inputed));



        }


    }

}

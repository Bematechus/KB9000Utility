using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Reflection;


namespace KB9Utility
{
    //[TypeConverter(typeof(PropertiesDeluxeTypeConverter))]
    public class DiagramKeyArea: DiagramKey
    {
      
        public DiagramKeyArea()
        {

	      
	        this.Title = ( "Key" );
	        this.TypeName = ( DiagramKey.KEY_BUTTON );
          
           

        }

        public override DiagramEntity Clone()
        {

	        
            DiagramKeyArea obj = new DiagramKeyArea();
	        obj.Copy( this );
	        return obj;

        }
        public override void Copy(DiagramEntity obj)
        {
            base.Copy(obj);
            DiagramKeyArea k = (DiagramKeyArea)obj;
            bool b = k.ContentModified;
            this.KeyCode = k.KeyCode;
            this.ContentModified = b;
            this.CapsEffect = k.CapsEffect;

            //restore contentmodified property

        }

        public override bool isEqual(DiagramEntity key)
        {
            if (!(key is DiagramKeyArea))
                return false;
            DiagramKeyArea k = (DiagramKeyArea)key;
            if (!base.isEqual(k)) return false;
            if (!this.KeyCode.ToString().Equals(k.KeyCode.ToString()) )
                return false;
            return true;

        }


        public override void Export(int nLayerIndex, CLCIXML xml) 
       
        {
           base.Export(nLayerIndex, xml);
	        xml.new_attribute("keycode", this.KeyCode.ToString());
            xml.new_attribute("capseffect", this.CapsEffect.ToString());
	        xml.back_to_parent();

        }

        public override bool FromXml(CLCIXML xml)
        {
            if (!base.FromXml(xml))
                return false;

            string s = "";
            xml.get_attribute("keycode",ref s);
            this.KeyCode.SetKeyCodeString(s);

            s = "false";
            xml.get_attribute("capseffect", ref s);
            this.CapsEffect = bool.Parse(s);
            return true;
        }

        protected override bool contains_data()
        {
            return (this.KeyCode.ToString().Length >0);
        }
        //protected void enable_caps_lock_effect_property(bool benable)
        //{

        //    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["CapsEffect"];
        //    ReadOnlyAttribute attribute = (ReadOnlyAttribute)
        //                                  descriptor.Attributes[typeof(ReadOnlyAttribute)];
        //    FieldInfo fieldToChange = attribute.GetType().GetField("isReadOnly",
        //                                     System.Reflection.BindingFlags.NonPublic |
        //                                     System.Reflection.BindingFlags.Instance);
        //    fieldToChange.SetValue(attribute, (!benable));

        //}
        ///// <summary>
        ///// check if we need to enable/disable caps lock_effect property
        ///// </summary>
        ///// <param name="keyEditingData"></param>
        //protected void check_caps_lock_effect_property_state(KeyEditingType keyEditingData)
        //{
           
            
        //    string s = keyEditingData.ToString();
        //    if (s.Length != 1) //just a -- z and one character can enable it
        //    {
        //        enable_caps_lock_effect_property(false);
        //    }
        //    else
        //    {
        //        char ch = s[0];
        //        if (ch >= 'a' && ch <= 'z')
        //            enable_caps_lock_effect_property(true);
        //        else
        //            enable_caps_lock_effect_property(false);
        //    }
        //    FirePropertiesChangedEvent(true);
            
        //}


        private KeyEditingType _KeyCode = new KeyEditingType();// = "";
        //[//CategoryAttribute("全局设置"),
        // ReadOnlyAttribute(true)]
        //DefaultValueAttribute("欢迎使用应用程序！")]  
        [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("The key contains code value")]
        [ReadOnly(false)]
        public KeyEditingType KeyCode
        {
            get { return _KeyCode; }
            set
            {
                if (_KeyCode != value)
                {
                   // FirePropertiesBeforeChangedEvent();
                    //MessageBox.Show("a");
                    this.ContentModified = true;
                    _KeyCode = value;
                   // check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent(true);
                    FirePropertiesChangedEvent();

                }
            }
        }
        public void SetKeyCodeString(string s)
        {
            string str = this.KeyCode.GetKeyCodeString();
            this.KeyCode.SetKeyCodeString(s);
            if (str != s)
                FirePropertiesChangedEvent();
        }
        public override string GetTooltipsText()
        {
            return this.KeyCode.ToString();
        }
        /// <summary>
        /// [Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>
        /// [35,6,60,41][0][0][2]<0x0d><0x0a>
        /// </summary>
        /// <returns></returns>
        public override string CreateCVS()
        {
            string strReturn = base.CreateCVS();
            strReturn += this.KeyCode.ToCSV(this.CapsEffect);


            strReturn += KB9Const.LF;
            return strReturn;
        }

          //[126,26,154,42][0][0][S][u][m][m][a][r][y]<0x0d><0x0a>
        public override bool FromCSV(string strCSV)
        {
            if (!base.FromCSV(strCSV))
                return false;
            string s = strCSV;
            //just need data from third "]"
            s = SubCSV(s, 2);
            bool b =  this.KeyCode.FromCSV(s);
            if (!b)
                return false;
            bool bEmulateKBD = false;
            if (is_caps_effected_key_contents(s))
            {
                this.CapsEffect = true;
                bEmulateKBD = true;
            }

            if (bEmulateKBD)
            {
                if (s.Length == 3)
                {
                    s = s.ToLower();
                    s = s.Replace("[", "");
                    s = s.Replace("]", "");
                    this.KeyCode.SetKeyCodeString(s);
                    this.CapsEffect = true;
                }
                return true;
            }


            //convert Upper Alpha char to lower
            //20141215
            //List<string> lst = new List<string>();
            //int n = Util.ParseKeyContents(s, lst);
            //if (s.Length == 3)
            //{
            //    s = s.ToLower();
            //    s = s.Replace("[", "");
            //    s = s.Replace("]", "");
            //    this.KeyCode.SetKeyCodeString(s);
            //    this.CapsEffect = true;
            //}
            if (Util.is_combination_content(s))
            {
                //string s = strKeys;
                s = s.Replace("[", "");
                s = s.Replace("]", "");

                string[] strArray = s.Split(new char[] { '+' });
                string str = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    str = strArray[i];
                    if (str.Length != 1) continue;
                    strArray[i] = strArray[i].ToLower();
                }
                s = "[";
                for (int i = 0; i < strArray.Length; i++)
                {
                    s += strArray[i];
                    if (i < strArray.Length - 1)
                        s += "+";
                }
                s += "]";
                this.KeyCode.SetKeyCodeString(s);
            }



            //for (int i = 0; i < n; i++)
            //{
            //    if (lst[i].Length != 1)
            //        continue;
            //    lst[i] = lst[i].ToLower();
            //}
            //string str = "[";
            //for (int i = 0; i < n; i++)
            //{
            //    str += lst[i];

            //}
            //str += "]";

            //this.KeyCode.SetKeyCodeString(str);
            return true;

        }


        //protected void enable_caps_lock_effect_property(bool benable)
        //{

        //    PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["CapsEffect"];
        //    ReadOnlyAttribute attribute = (ReadOnlyAttribute)
        //                                  descriptor.Attributes[typeof(ReadOnlyAttribute)];
        //    FieldInfo fieldToChange = attribute.GetType().GetField("isReadOnly",
        //                                     System.Reflection.BindingFlags.NonPublic |
        //                                     System.Reflection.BindingFlags.Instance);
        //    fieldToChange.SetValue(attribute, (!benable));

        //}

        protected bool is_caps_lock_effectable()
        {
            string s = this.KeyCode.ToString();
            if (s.Length != 1) //just a -- z and one character can enable it
            {
                if (s.Equals("\\[") ||
                    s.Equals("\\]") ||
                    s.Equals("\\\\") ||
                    s.Equals("[") ||
                    s.Equals("]") ||
                    s.Equals("\\") )
                    return true;
                return false;
            }
            else
            {
                char ch = s[0];
                return KeyEditingType.isEmulateKbdKey(ch);
                //if (ch >= 'a' && ch <= 'z' ||
                //    ch >= '0' && ch <='9')
                //    return true;
                
            }

            //return false;
        }

        ///// <summary>
        ///// check if we need to enable/disable caps lock_effect property
        ///// </summary>
        ///// <param name="keyEditingData"></param>
        //protected void check_caps_lock_effect_property_state(KeyEditingType keyEditingData)
        //{
        //    return;
        //    /*
            
        //    string s = keyEditingData.ToString();
        //    if (s.Length != 1) //just a -- z and one character can enable it
        //    {
        //        enable_caps_lock_effect_property(false);
        //    }
        //    else
        //    {
        //        char ch = s[0];
        //        if (ch >= 'a' && ch <= 'z')
        //            enable_caps_lock_effect_property(true);
        //        else
        //            enable_caps_lock_effect_property(false);
        //    }
        //    FirePropertiesChangedEvent(true);
        //     */
        //}

        /************************************************************************/
        /* 
         * 
         * Add a key property in key: Enable Caps Lock Effect. The idea is to provide
whether a key is effect by Cap lock or not.  The default option will be off,
and this option is also grey out by default. It only enable when the key
contents is a single letter from a to z. When this option is off, nothing
changes. If this option is on, when writing to keyboard, the single a will
become [a]. In really keyboard and test output: This [a] when Caps Lock is
on, it will output A, if Caps Lock is off, it will output a. 
         */
        /************************************************************************/
        private bool _CapsEffect = false;
        [Description("It only worked when the key contents is a single letter from a to z")]
        [Category("Key")]
        //[DisplayName("Enable Caps Lock Effect")]
        [DisplayName("Emulate keyboard \nIf Emulate keyboard is off, key 'a' \nwill only output a once even you press \nand hold it; and it always output a,\nnot matter if the Caps Lock is on\nor off. If emulate keyboard is on,\nkey 'a' will continue outputting if you\nhold it; also it will output A if Caps Lock is on.")]
        [ReadOnly(false)]
        [PropertyAttributesProvider("DynamicPropertyAttributesProvider")]
        public bool CapsEffect
        {
            get 
            {
                if (!is_caps_lock_effectable())
                { //make sure just one key can enable this property
                  //  if (this.KeyCode.
                    setCapEffectAttributeReadOnly(true);
                    
                    _CapsEffect = false;
                    if (this.KeyCode.isDisableEmulateKey())
                        return false;
                    if (this.KeyCode.getKeysCount() == 1) //with []
                        return true;
                    
                }
                else
                    setCapEffectAttributeReadOnly(false);
               // _CapsEffect = is_caps_lock_effectable();
                return _CapsEffect; 
            }
            set
            {
                if (is_caps_lock_effectable())
                {

                    if (_CapsEffect != value)
                    {
                        this.PropertiesModified = true;
                        _CapsEffect = value;
                        FirePropertiesChangedEvent();
                    }
                }
            }
        }

        private void setCapEffectAttributeReadOnly(bool bReadOnly)
        {
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["CapsEffect"];
            ReadOnlyAttribute attribute = (ReadOnlyAttribute)
                                          descriptor.Attributes[typeof(ReadOnlyAttribute)];
            FieldInfo fieldToChange = attribute.GetType().GetField("isReadOnly",
                                             System.Reflection.BindingFlags.NonPublic |
                                             System.Reflection.BindingFlags.Instance);
            fieldToChange.SetValue(attribute, bReadOnly);

        }
        public void DynamicPropertyAttributesProvider(PropertyAttributes attributes)
        {
          //  attributes.DisplayName = dynamicPropertyDisplayName;
            attributes.IsReadOnly = (!is_caps_lock_effectable());
           // attributes.IsBrowsable = dynamicPropertyIsVisible;
        }

        override public string getContentText()
        {
            return this.KeyCode.ToString();
        }

    }



}

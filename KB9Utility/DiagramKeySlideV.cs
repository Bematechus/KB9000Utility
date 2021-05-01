using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace KB9Utility
{
    public class DiagramKeySlideV : DiagramKey
    {


         public DiagramKeySlideV()
        {

	      
	        this.Title = ( "SlideV" );
            this.TypeName = DiagramKey.KEY_SLIDEV;//
          
           

        }

        public override DiagramEntity Clone()
        {

	        
            DiagramKeySlideV obj = new DiagramKeySlideV();
	        obj.Copy( this );
	        return obj;

        }
        public override void Copy(DiagramEntity obj)
        {
            base.Copy(obj);
            DiagramKeySlideV k = (DiagramKeySlideV)obj;
            bool b = k.ContentModified;
            this.SlideUp = k.SlideUp;
            this.SlideUpHold = k.SlideUpHold;
            this.SlideDown = k.SlideDown;
            this.SlideDownHold = k.SlideDownHold;
            this.ContentModified = b;

        }

       


        public override void Export(int nLayerIndex, CLCIXML xml) 
       
        {
           base.Export(nLayerIndex, xml);
	        xml.new_attribute("slideup", this.SlideUp.ToString());
            xml.new_attribute("slideuphold", this.SlideUpHold.ToString());
            xml.new_attribute("slidedown", this.SlideDown.ToString());
            xml.new_attribute("slidedownhold", this.SlideDownHold.ToString());
	        xml.back_to_parent();

        }

        public override bool FromXml(CLCIXML xml)
        {
            if (!base.FromXml(xml))
                return false;

            string s = "";

            xml.get_attribute("slideup", ref s);
            this.SlideUp.SetKeyCodeString(s);

            s = string.Empty;
            xml.get_attribute("slideuphold", ref s);
            this.SlideUpHold.SetKeyCodeString(s);

            s = string.Empty;
            xml.get_attribute("slidedown", ref s);
            this.SlideDown.SetKeyCodeString(s);

            s = string.Empty;
            xml.get_attribute("slidedownhold", ref s);
            this.SlideDownHold.SetKeyCodeString(s);

            return true;
        }

        protected override void Draw_Logo(Graphics g, Rectangle rectVirtualEntityWithOffset)
        {
            if (!this.ShowLogo) return;
            //Image img = Util.get_image("logoud");
            //if (img == null)
            //    return;
            //Rectangle rt = rectVirtualEntityWithOffset;
            //int nsize = LOGO_SIZE;
            //rt.X = rt.Right - nsize;
            //rt.Width = nsize;
            //rt.Height = nsize;
            //g.DrawImage(img, rt);

            System.Drawing.Drawing2D.AdjustableArrowCap lineCap =
                    new System.Drawing.Drawing2D.AdjustableArrowCap(3, 2, true);

            Pen redArrowPen = new Pen(this.ForeColor, 1);
            redArrowPen.CustomEndCap = lineCap;
            redArrowPen.CustomStartCap = lineCap;
            Rectangle rt = rectVirtualEntityWithOffset;
            int nsize = LOGO_SIZE;
            Point ptStart = new Point(rt.Right - nsize/2, rt.Y );
            Point ptEnd = new Point(rt.Right - nsize/2, rt.Y +nsize);
            Pen pen = new Pen(this.ForeColor, 1);
            g.DrawLine(pen, ptStart, ptEnd);//
            
            //header top
            int nSizeX = 3;
            int nSizeY = 3;
           
            Point[] ptsTop = new Point[]{
                new Point(ptStart.X, ptStart.Y-1),
                new Point(ptStart.X - nSizeX-2, ptStart.Y+nSizeY+2),
                new Point(ptStart.X + nSizeX+2, ptStart.Y+nSizeY+2),

            };
            g.FillPolygon(new SolidBrush(this.ForeColor), ptsTop);//, FillMode.Winding);
            
            //header bottom
            Point[] ptsBottom = new Point[]{
                new Point(ptEnd.X, ptEnd.Y+1),
                new Point(ptEnd.X - nSizeX-1, ptEnd.Y-nSizeY-1),
                new Point(ptEnd.X + nSizeX+2, ptEnd.Y-nSizeY-1),

            };
            g.FillPolygon(new SolidBrush(this.ForeColor), ptsBottom, FillMode.Winding);
        }


        protected override bool contains_data()
        {
            return (this.SlideUp.ToString().Length > 0 ||
                    this.SlideUpHold.ToString().Length > 0 ||
                    this.SlideDown.ToString().Length > 0 ||
                    this.SlideDownHold.ToString().Length > 0);

        }
   
        private KeyEditingType _SlideUp = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide up")]
        [Description("Slide Up")]
        [ReadOnly(false)]
        public KeyEditingType SlideUp
        {
            get { return _SlideUp; }
            set
            {
                if (_SlideUp != value)
                {
                    this.ContentModified = true;
                    _SlideUp = value;
                    //check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }

        private KeyEditingType _SlideDown = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide down")]
        [Description("Slide down key code")]
        [ReadOnly(false)]
        public KeyEditingType SlideDown
        {
            get { return _SlideDown; }
            set
            {
                if (_SlideDown != value)
                {
                    this.ContentModified = true;
                    _SlideDown = value;
                   // check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }

        private KeyEditingType _SlideUpHold = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide up hold")]
        [Description("Slide up and hold")]
        [ReadOnly(false)]
        public KeyEditingType SlideUpHold
        {
            get { return _SlideUpHold; }
            set
            {
                if (_SlideUpHold != value)
                {
                    this.ContentModified = true;
                    _SlideUpHold = value;
                   // check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }



        private KeyEditingType _SlideDownHold = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide down hold")]
        [Description("Slide down and hold")]
        [ReadOnly(false)]
        public KeyEditingType SlideDownHold
        {
            get { return _SlideDownHold; }
            set
            {
                if (_SlideDownHold != value)
                {
                    this.ContentModified = true;
                    _SlideDownHold = value;
                    //check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }

        public override string GetTooltipsText()
        {
            string s = "Up:" + this.SlideUp.ToString();
            s += "\r\n";
            s += ("Up Hold: " + this.SlideUpHold.ToString());
            s += "\r\n";
            s += ("Down:" + this.SlideDown.ToString());
            s += "\r\n";
            s += ("Down Hold:" + this.SlideDownHold.ToString());
            return s;

        }


        /// <summary>
        /// [Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>
        /// [35,6,60,41][0][0][2]<0x0d><0x0a>
        ///   [SlideU] keycontent1[SlideD] keycontent2 [SlideUH] keycontent3 [SlideDH] keycontent4
        ///         Keycontents1/2/3/4 are key strokes for left slide, right slide, left-hold slide, right-hold slide respectively.




        /// </summary>
        /// <returns></returns>
        public override string CreateCVS()
        {
            string strPre = base.CreateCVS() + "[SlideU]";
            string strReturn = "";
            string strLine = "";

            strLine = strPre + this.SlideUp.ToCSV();// +KB9Const.LF;
            strReturn += strLine;

            strLine = "[SlideD]" + this.SlideDown.ToCSV();// +KB9Const.LF;
            strReturn += strLine;

            strLine = "[SlideUH]" + this.SlideUpHold.ToCSV();// +KB9Const.LF;
            strReturn += strLine;

            strLine = "[SlideDH]" + this.SlideDownHold.ToCSV() + KB9Const.LF;
            strReturn += strLine;


            return strReturn;
        }

        public  string CreateCVS_removed()
        {
            string strPre = base.CreateCVS() + "[SlideV]";
            string strReturn = "";
            string strLine = "";

            strLine = strPre + this.SlideUp.ToCSV() + KB9Const.LF;
            strReturn += strLine;

            strLine = strPre + this.SlideDown.ToCSV() + KB9Const.LF;
            strReturn += strLine;

            strLine = strPre + this.SlideUpHold.ToCSV() + KB9Const.LF;
            strReturn += strLine;

            strLine = strPre + this.SlideDownHold.ToCSV() + KB9Const.LF;
            strReturn += strLine;


            return strReturn;
        }

        /// <summary>
        /// [SlideV]: Vertical slide key (takes 4 lines)
       //Vertical slide key (takes 1 line only)

       //               [SlideU] keycontent1[SlideD] keycontent2 [SlideUH] keycontent3 [SlideDH] keycontent4

       //               Keycontents1/2/3/4 are key strokes for left slide, right slide, left-hold slide, right-hold slide respectively

        public override bool FromCSV(string strCSV)
        {
            if (!base.FromCSV(strCSV))
                return false;
            string s = strCSV;


            string slideU = get_sub_string(s, "[SlideU]", "[SlideD]");
            this.SlideUp.FromCSV(slideU);

            string slideD = get_sub_string(s, "[SlideD]", "[SlideUH");
            this.SlideDown.FromCSV(slideD);

            string slideUH = get_sub_string(s, "[SlideUH]", "[SlideDH]");
            this.SlideUpHold.FromCSV(slideUH);

            string slideDH = get_sub_string(s, "[SlideDH]", KB9Const.LF);
            this.SlideDownHold.FromCSV(slideDH);

            return true;
        }
        public bool FromCSV_removed(string strCSV0, string strCSV1, string strCSV2, string strCSV3)
        {
            if (!base.FromCSV(strCSV0))
                return false;
            string s = strCSV0;
            //just need data from third "]"
            s = SubCSV(s, 2);
            this.SlideUp.FromCSV(s);
            //if (is_caps_effected_key_contents(s))
            //    this.CapsEffect = true;

            s = strCSV1;
            //just need data from third "]"
            s = SubCSV(s, 2);
            this.SlideDown.FromCSV(s);
            //if (is_caps_effected_key_contents(s))
            //    this.CapsEffect = true;

            s = strCSV2;
            //just need data from third "]"
            s = SubCSV(s, 2);
            this.SlideUpHold.FromCSV(s);
            //if (is_caps_effected_key_contents(s))
            //    this.CapsEffect = true;


            s = strCSV3;
            //just need data from third "]"
            s = SubCSV(s, 2);
            this.SlideDownHold.FromCSV(s);
            //if (is_caps_effected_key_contents(s))
            //    this.CapsEffect = true;
            return true;
        }

        public override bool isEqual(DiagramEntity key)
        {
            if (!(key is DiagramKeySlideV))
                return false;
            if (!base.isEqual(key)) return false;
            DiagramKeySlideV k = (DiagramKeySlideV)key;
            if (!this.SlideDown.GetKeyCodeString().Equals(k.SlideDown.GetKeyCodeString()))
                return false;
            if (!this.SlideDownHold.GetKeyCodeString().Equals(k.SlideDownHold.GetKeyCodeString()))
                return false;
            if (!this.SlideUpHold.GetKeyCodeString().Equals(k.SlideUpHold.GetKeyCodeString()))
                return false;
            if (!this.SlideUp.GetKeyCodeString().Equals(k.SlideUp.GetKeyCodeString()))
                return false;
            return true;

        }

        override public string getContentText()
        {
            string s = this.SlideDown.GetKeyCodeString();
            s += this.SlideDownHold.GetKeyCodeString();
            s += this.SlideUpHold.GetKeyCodeString();
            s += this.SlideUp.GetKeyCodeString();

            return s;
        }
    }

    
}

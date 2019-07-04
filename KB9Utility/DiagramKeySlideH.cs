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
    public class DiagramKeySlideH : DiagramKey
    {

        public DiagramKeySlideH()
        {

	     
	        this.Title = ( "SlideH" );
            this.TypeName = DiagramKey.KEY_SLIDEH;// ("SLIDEH");
         
           

        }

        public override DiagramEntity Clone()
        {


            DiagramKeySlideH obj = new DiagramKeySlideH();
	        obj.Copy( this );
	        return obj;

        }
        public override void Copy(DiagramEntity obj)
        {
            base.Copy(obj);
            DiagramKeySlideH k = (DiagramKeySlideH)obj;
            bool b = k.ContentModified;
            this.SlideLeft = k.SlideLeft;
            this.SlideLeftHold = k.SlideLeftHold;
            this.SlideRight = k.SlideRight;
            this.SlideRightHold = k.SlideRightHold;

            this.ContentModified = b;

        }

       


        public override void Export(int nLayerIndex, CLCIXML xml) 
       
        {
           base.Export(nLayerIndex, xml);
	        xml.new_attribute("slideleft", this.SlideLeft.ToString());
            xml.new_attribute("slidelefthold", this.SlideLeftHold.ToString());
            xml.new_attribute("slideright", this.SlideRight.ToString());
            xml.new_attribute("sliderighthold", this.SlideRightHold.ToString());
	        xml.back_to_parent();

        }

        public override bool FromXml(CLCIXML xml)
        {
            if (!base.FromXml(xml))
                return false;

            string s = "";

            xml.get_attribute("slideleft", ref s);
            this.SlideLeft.SetKeyCodeString(s);

            s = string.Empty;
            xml.get_attribute("slidelefthold", ref s);
            this.SlideLeftHold.SetKeyCodeString(s);

            s = string.Empty;
            xml.get_attribute("slideright", ref s);
            this.SlideRight.SetKeyCodeString(s);

            s = string.Empty;
            xml.get_attribute("sliderighthold", ref s);
            this.SlideRightHold.SetKeyCodeString(s);

            return true;
        }


        protected override void Draw_Logo(Graphics g, Rectangle rectVirtualEntityWithOffset)
        {
            if (!this.ShowLogo) return;
            //Image img = Util.get_image("logolr");
            //if (img == null)
            //    return;
            //Rectangle rt = rectVirtualEntityWithOffset;
            //int nsize = LOGO_SIZE;
            //rt.X = rt.Right - nsize;
            //rt.Width = nsize;
            //rt.Height = nsize;
            //g.DrawImage(img, rt);



            System.Drawing.Drawing2D.AdjustableArrowCap lineCap =
                new System.Drawing.Drawing2D.AdjustableArrowCap(4, 2, true);

            Pen redArrowPen = new Pen(this.ForeColor, 1);
            //redArrowPen.CustomEndCap = lineCap;
            //redArrowPen.CustomStartCap = lineCap;
            Rectangle rt = rectVirtualEntityWithOffset;
            int nsize = LOGO_SIZE;
            Point ptStart = new Point(rt.Right - nsize, rt.Y+ LOGO_SIZE/2);
            Point ptEnd = new Point(rt.Right, rt.Y + LOGO_SIZE / 2);
        
            g.DrawLine(redArrowPen,ptStart, ptEnd);//


            //header top
            int nSizeX = 3;
            int nSizeY = 3;

            Point[] ptsLeft = new Point[]{
                new Point(ptStart.X, ptStart.Y),
                 new Point(ptStart.X + nSizeX+2, ptStart.Y-nSizeY-2),
               new Point(ptStart.X + nSizeX+2, ptStart.Y+nSizeY+2),

            };
            g.FillPolygon(new SolidBrush(this.ForeColor), ptsLeft);//, FillMode.Winding);
            
            //header bottom
            Point[] ptsRight = new Point[]{
                new Point(ptEnd.X+1, ptEnd.Y),
                new Point(ptEnd.X - nSizeX-2, ptEnd.Y-nSizeY-2),
                new Point(ptEnd.X - nSizeX-1, ptEnd.Y+nSizeY+2),

            };
            g.FillPolygon(new SolidBrush(this.ForeColor), ptsRight, FillMode.Winding);

        }

        protected override bool contains_data()
        {
            return (this.SlideLeft.ToString().Length > 0 ||
                    this.SlideLeftHold.ToString().Length >0 ||
                    this.SlideRight.ToString().Length >0 ||
                    this.SlideRightHold.ToString().Length >0 );
                
        }

   
        private KeyEditingType _SlideLeft = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide Left")]
        [Description("Slide Left")]
        [ReadOnly(false)]
        public KeyEditingType SlideLeft
        {
            get { return _SlideLeft; }
            set
            {
                if (_SlideLeft != value)
                {
                   // FirePropertiesBeforeChangedEvent();
                    this.ContentModified = true;
                    _SlideLeft = value;
                   // check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }

        private KeyEditingType _SlideRight = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide right")]
        [Description("Slide right key code")]
        [ReadOnly(false)]
        public KeyEditingType SlideRight
        {
            get { return _SlideRight; }
            set
            {
                if (_SlideRight != value)
                {
                   // FirePropertiesBeforeChangedEvent();
                    this.ContentModified = true;
                    _SlideRight = value;
                    //check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }

        private KeyEditingType _SlideLeftHold = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide left Hold")]
        [Description("Slide left and hold")]
        [ReadOnly(false)]
        public KeyEditingType SlideLeftHold
        {
            get { return _SlideLeftHold; }
            set
            {
                if (_SlideLeftHold != value)
                {
                   // FirePropertiesBeforeChangedEvent();
                    this.ContentModified = true;
                    _SlideLeftHold = value;
                    //check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }



        private KeyEditingType _SlideRightHold = new KeyEditingType();
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("Slide right hold")]
        [Description("Slide right and hold")]
        [ReadOnly(false)]
        public KeyEditingType SlideRightHold
        {
            get { return _SlideRightHold; }
            set
            {
                if (_SlideRightHold != value)
                {
                   // FirePropertiesBeforeChangedEvent();
                    this.ContentModified = true;
                    _SlideRightHold = value;
                    //check_caps_lock_effect_property_state(value);
                    FirePropertiesChangedEvent();
                }
            }
        }


        public override string GetTooltipsText()
        {
            string s = "Left:" + this.SlideLeft.ToString() ;
            s += "\r\n";
            s +=("Left Hold: "+ this.SlideLeftHold.ToString());
            s += "\r\n";
            s += ("Right:"+ this.SlideRight.ToString());
            s += "\r\n";
            s +=("Right Hold:" + this.SlideRightHold.ToString());
            return s;
            
        }

        /// <summary>
        /// [Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>
        /// [35,6,60,41][0][0][2]<0x0d><0x0a>
        /// ********************* REMOVED ***************
        /// [SlideH]: Horizontal slide key (takes 4 lines)
        //Line1: [SlideH]keycontent1  (keycontent1 is key strokes for left slide)
        //Line2: [SlideH]keycontent2  (keycontent2 is key strokes for right slide)
        //Line3: [SlideH]keycontent3  (keycontent3 is key strokes for left-hold slide)
        //Line4: [SlideH]keycontent4  (keycontent4 is key strokes for right-hold slide)
        /* *********** ABOVE REMOVED ************
         * 
         * New definition: 20140221, Edmond and peter ask new definition.
          Horizontal slide key (takes 1 line only)
                      [SlideL] keycontent1[SlideR] keycontent2 [SlideLH] keycontent3 [SlideRH] keycontent4
                      Keycontents1/2/3/4 are key strokes for left slide, right slide, left-hold slide, right-hold slide respectively.
          Vertical slide key (takes 1 line only)
                      [SlideU] keycontent1[SlideD] keycontent2 [SlideUH] keycontent3 [SlideDH] keycontent4
                      Keycontents1/2/3/4 are key strokes for left slide, right slide, left-hold slide, right-hold slide respectively.
        Example:
            [326,0,18,106][110][110][SlideU][ArrowUp][SlideUH][PageUp][SlideD][ArrowDown][SlideDH][PageDown]<0x0d><0x0a>
        Note that if any of the key contents are not used, then leave the field empty.
    Example
            [326,0,18,106][110][110][SlideU][ArrowUp][SlideUH][PageUp][SlideD][SlideDH]<0x0d><0x0a>


         */
        /// </summary>
        /// <returns></returns>
        public override string CreateCVS()
        {
            string strPre = base.CreateCVS() + "[SlideL]";
            string strReturn = "";
            string strLine = "";

            strLine = strPre + this.SlideLeft.ToCSV();// +KB9Const.LF;
            strReturn += strLine;

            strLine = "[SlideR]" + this.SlideRight.ToCSV();// +KB9Const.LF;
            strReturn += strLine;

            strLine = "[SlideLH]" + this.SlideLeftHold.ToCSV();// +KB9Const.LF;
            strReturn += strLine;

            strLine = "[SlideRH]" + this.SlideRightHold.ToCSV() + KB9Const.LF;
            strReturn += strLine;


            return strReturn;
        }

        public  string CreateCVS_Removed()
        {
            string strPre = base.CreateCVS() + "[SlideH]";
            string strReturn = "";
            string strLine = "";

            strLine = strPre + this.SlideLeft.ToCSV() + KB9Const.LF;
            strReturn += strLine;

            strLine = strPre + this.SlideRight.ToCSV() + KB9Const.LF;
            strReturn += strLine;

            strLine = strPre + this.SlideLeftHold.ToCSV() + KB9Const.LF;
            strReturn += strLine;

            strLine = strPre + this.SlideRightHold.ToCSV() + KB9Const.LF;
            strReturn += strLine;

            
            return strReturn;
        }

        /// <summary>
        /// [SlideL] keycontent1[SlideR] keycontent2 [SlideLH] keycontent3 [SlideRH] keycontent4
        /// </summary>
        /// <param name="strCSV0"></param>
        /// <param name="strCSV1"></param>
        /// <param name="strCSV2"></param>
        /// <param name="strCSV3"></param>
        /// <returns></returns>
        public override bool FromCSV(string strCSV)
        {
            if (!base.FromCSV(strCSV))
                return false;
            string s = strCSV;
            
            
            string slideL = get_sub_string(s, "[SlideL]", "[SlideR]");
            this.SlideLeft.FromCSV(slideL);
           
            string slideR = get_sub_string(s, "[SlideR]", "[SlideLH");
            this.SlideRight.FromCSV(slideR);
           
            string slideLH = get_sub_string(s, "[SlideLH]", "[SlideRH]");
            this.SlideLeftHold.FromCSV(slideLH);
           
            string slideRH = get_sub_string(s, "[SlideRH]", KB9Const.LF);
            this.SlideRightHold.FromCSV(slideRH);
           
            return true;
        }

        public  bool FromCSV_removed(string strCSV0, string strCSV1, string strCSV2, string strCSV3)
        {
            if (!base.FromCSV(strCSV0))
                return false;
            string s = strCSV0;
            //just need data from third "]"
            s = SubCSV(s, 2);
            this.SlideLeft.FromCSV(s);
            // if (is_caps_effected_key_contents(s))
            //   this.CapsEffect = true;



            s = strCSV1;
            //just need data from third "]"
            s = SubCSV(s, 2);

            this.SlideRight.FromCSV(s);
            //if (is_caps_effected_key_contents(s))
            //    this.CapsEffect = true;

            s = strCSV2;
            //just need data from third "]"
            s = SubCSV(s, 2);
            this.SlideLeftHold.FromCSV(s);
            //if (is_caps_effected_key_contents(s))
            //    this.CapsEffect = true;

            s = strCSV3;
            //just need data from third "]"
            s = SubCSV(s, 2);
            this.SlideRightHold.FromCSV(s);
            //if (is_caps_effected_key_contents(s))
            //    this.CapsEffect = true;
            return true;
        }

        public override bool isEqual(DiagramEntity key)
        {
            if (!(key is DiagramKeySlideH))
                return false;
            if (!base.isEqual(key)) return false;
            DiagramKeySlideH k = (DiagramKeySlideH)key;
            if (!this.SlideLeft.GetKeyCodeString().Equals(k.SlideLeft.GetKeyCodeString()))
                return false;
            if (!this.SlideLeftHold.GetKeyCodeString().Equals(k.SlideLeftHold.GetKeyCodeString()))
                return false;
            if (!this.SlideRightHold.GetKeyCodeString().Equals(k.SlideRightHold.GetKeyCodeString()))
                return false;
            if (!this.SlideRight.GetKeyCodeString().Equals(k.SlideRight.GetKeyCodeString()))
                return false;
            return true;

        }

        override public string getContentText()
        {
            string s = this.SlideLeft.GetKeyCodeString();
            s += this.SlideLeftHold.GetKeyCodeString();
            s += this.SlideRightHold.GetKeyCodeString();
            s += this.SlideRight.GetKeyCodeString();

            return s;
        }

    }
}

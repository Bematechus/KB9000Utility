using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;


namespace KB9Utility
{
    /************************************************************************/
    /* 
     * base class for all Key type
     */
    /************************************************************************/
    public class DiagramKey: DiagramEntity
    {

        public const string KEY_BUTTON = "BUTTON";
        public const string KEY_SLIDEV = "SLIDEV";
        public const string KEY_SLIDEH = "SLIDEH";

        protected const int LOGO_SIZE = 16;

        public DiagramKey()
       
        {

	        //this.MinimumSize = ( new Size( 1, 1 ) );
	        //this.Title = ( "Key" );
	        //this.TypeName = ( "Key" );
            this.MinimumSize = new Size(50, 50);
           
        }

        public override DiagramEntity Clone()
       
        {


            DiagramKey obj = new DiagramKey();
	        obj.Copy( this );
	        return obj;

        }
        public override void Copy(DiagramEntity obj)
        {
            base.Copy(obj);
            DiagramKey k = (DiagramKey)obj;
            this.BackColor = k.BackColor;
            this.Font = k.Font;
            this.ForeColor = k.ForeColor;
            if (k.Image != null)
                this.Image = (Image) k.Image.Clone();

            this.ImageAlign = k.ImageAlign;
           // this.KeyCode = k.KeyCode;
            this.RoundCornerRadius = k.RoundCornerRadius;
            this.BorderThickness = k.BorderThickness;
            this.BorderColor = k.BorderColor;
            this.TextRotation = k.TextRotation;
            this.TitleAlign = k.TitleAlign;
            this.ContentModified = k.ContentModified;
            this.BeepDuration = k.BeepDuration;
            this.BeepPitch = k.BeepPitch;
            

        }

        protected const int CORDER_R = 5;
        protected void DrawBackground(Graphics g, Rectangle rect, double zoom)
        {
            SolidBrush br = new SolidBrush(this.BackColor);

            //rect.X -= 1;
            //rect.Y -= 1;
            Rectangle rt = rect;
            int borderThickness = this.BorderThickness;
            borderThickness = (int)decimal.Round(((decimal)borderThickness * (decimal)zoom));
            if (this.BorderThickness >0 )
            {//make sure 1 border thickness
                if (borderThickness == 0)
                    borderThickness = 1;

            }

            //if (this.BorderThickness > 0)
            //{
                if (this.BorderThickness > 0)
                {//remove the border thickness
                    //int f = this.BorderThickness / 2;   
                    rt = remove_border_thickness(rt, borderThickness);

                    //int f = borderThickness / 2;
                    //if (f == 0)
                    //    f = 1; //min 1
                    //if (f > 1)
                    //{
                    //    rt.X += f;
                    //    rt.Y += f;
                    //    rt.Height -= (f * 2);
                    //    rt.Width -= (f * 2);
                    //}
                }
            //}

            int nradius = (int)decimal.Round((decimal)this.RoundCornerRadius * (decimal)zoom);

            if (this.RoundCornerRadius>0)
            {
                FillRoundRectangle(g, br, rt, nradius);//this.RoundCornerRadius);
            }
            else
            {

                g.FillRectangle(br, rt);
            }

            //if (this.ShowBorder)
             if (this.BorderThickness>0)
            {
               
                Pen pen = new Pen(this.BorderColor, borderThickness);// this.BorderThickness);

                if (this.RoundCornerRadius > 0)
                {
                     DrawRoundRectangle(g, pen, rt, nradius);//RoundCornerRadius);
                }
                else
                    g.DrawRectangle(pen, rt);
               
            }
        }

        private Rectangle remove_border_thickness(Rectangle rt, int nThickness)
        {
            int f = nThickness / 2;
            if (nThickness % 2 > 0)
                f += 1;
            if (f == 0)
                f = 1; //min 1
            if (f > 1)
            {
                rt.X += f;
                rt.Y += f;
                rt.Height -= (f * 2);
                rt.Width -= (f * 2);
            }
            return rt;
        }
        protected int get_max_corner_radius()
        {
            Rectangle rt = this.Bounds;

            if (this.BorderThickness > 0)
            {//remove the border thickness
                rt = remove_border_thickness(rt, this.BorderThickness);

                //int f = this.BorderThickness / 2;
                //if (f == 0)
                //    f = 1; //min 1
                //if (f > 1)
                //{
                //    rt.X += f;
                //    rt.Y += f;
                //    rt.Height -= (f * 2);
                //    rt.Width -= (f * 2);
                //}
            }

            int nmax = rt.Width;
            if (rt.Height < nmax)
                nmax = rt.Height;
            return (nmax / 2-1);
        }

        //protected int get_max_border_thickness()
        //{
        //    Rectangle rt = this.Bounds;
        //    int nmax = rt.Width;
        //    if (rt.Height < nmax)
        //        nmax = rt.Height;
        //    return (nmax / 2 - 1);
        //}

        protected void DrawTitle( Graphics g, Rectangle rect )
        {

            Font font = ZoomFont();// this.Font;

            SolidBrush br = new SolidBrush(this.ForeColor);
            RectangleF drawRect = new RectangleF(rect.Location, new Size(rect.Width, rect.Height));
            StringFormat drawFormat = new StringFormat();
           
            //convert ContentAlign to StringAlignment
            Int32 lNum = (Int32)Math.Log((Double)this.TitleAlign, 2);
            drawFormat.LineAlignment = (StringAlignment)(lNum / 4);
            drawFormat.Alignment = (StringAlignment)(lNum % 4);


           
            ///////////

            //rotation degrees
            if (this.TextRotation != 0)
            {
                Region rg = g.Clip;
                g.Clip = new Region( drawRect);
                SizeF txt = g.MeasureString(this.Title, font);
                SizeF sz = drawRect.Size;// g.Graphics.VisibleClipBounds.Size;

                g.TranslateTransform(drawRect.Left + drawRect.Width / 2, drawRect.Top + drawRect.Height / 2);
                g.RotateTransform(this.TextRotation);
                //g.DrawString(Text, this.Font, Brushes.Black, new RectangleF(0, 0, sz.Height, sz.Width), format);
                RectangleF rt = drawRect;
                float w = (drawRect.Width / 2);
                float h = (drawRect.Height / 2);
                float nmax = (w > h ? w : h);



                rt.X = -1 * nmax;//(drawRect.Width / 2);
                rt.Y = -1 * nmax;//(drawRect.Height / 2);
                rt.Width = nmax * 2;
                rt.Height = nmax * 2;
                drawFormat.LineAlignment = StringAlignment.Center;
                drawFormat.Alignment = StringAlignment.Center;

                g.DrawString(this.Title, font, br, rt, drawFormat);
                g.ResetTransform();
                g.Clip = rg;
            }
            else
                g.DrawString(this.Title, font, br, drawRect, drawFormat);
        }

        protected void DrawOverlappedArea(Graphics g, Rectangle rectEntityClient )
        {
            Rectangle rt = this.ParentContainer.GetOverlappedRect(this);
            rt = ZoomRectangle(rt, this.Zoom);

            

            if (rt.IsEmpty) return;
            Rectangle rtBounds = this.Bounds;
            rtBounds = ZoomRectangle(rtBounds, this.Zoom);

            Point pt = new Point(rectEntityClient.X - rtBounds.X, rectEntityClient.Y - rtBounds.Y);
            rt.Offset(pt);
            //SolidBrush br = new SolidBrush(Color.Red);
            HatchBrush br = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Red, Color.Yellow);

            
            
            g.FillRectangle(br, rt);
            
        }

        protected void DrawOverlappedArea2(Graphics g, Rectangle rectEntityClient)
        {
            List<Rectangle> lst = new List<Rectangle>();
            int ncount = this.ParentContainer.GetOverlappedRect2(this, lst);
            if (ncount <= 0)
                return;
            Rectangle rtBounds = this.Bounds;
            rtBounds = ZoomRectangle(rtBounds, this.Zoom);

            for (int i = 0; i < ncount; i++)
            {
                Rectangle rt = lst[i];
                rt = ZoomRectangle(rt, this.Zoom);
                if (rt.IsEmpty) return;
                Point pt = new Point(rectEntityClient.X - rtBounds.X, rectEntityClient.Y - rtBounds.Y);
                rt.Offset(pt);
                //SolidBrush br = new SolidBrush(Color.Red);
                HatchBrush br = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Red, Color.Yellow);
                g.FillRectangle(br, rt);
            }

        }

        protected void DrawImage(Graphics g, Rectangle rectVirtualEntityWithOffsetAndZoom, double dblZoom)
        {
            if (this.Image == null) return;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            ContentAlignment align =  this.ImageAlign ;
            int imgWidth = this.Image.Width;
            int imgHeight = this.Image.Height;

            Rectangle rtBounds = this.Bounds;
            if (imgHeight > rtBounds.Height ||
                imgWidth > rtBounds.Width)
                g.DrawImage(this.Image, rectVirtualEntityWithOffsetAndZoom);
            else
            {//use align
                //zoom img
                imgWidth = (int)( decimal.Round(((decimal)imgWidth * (decimal)dblZoom)) );
                imgHeight = (int)(decimal.Round(((decimal)imgHeight * (decimal)dblZoom)));
                Rectangle rc = GetImageAlignmentRect(rectVirtualEntityWithOffsetAndZoom, imgWidth, imgHeight, align);
                g.DrawImage(this.Image, rc);
            }
        }

        static public Rectangle GetImageAlignmentRect(Rectangle rectVirtualEntityWithOffsetAndZoom, int nImgZoomedW,int nImgZoomedH, ContentAlignment align)
        {
            int x = 0;
            int y = 0;
            int nButtonW = rectVirtualEntityWithOffsetAndZoom.Width;
            int nButtonH = rectVirtualEntityWithOffsetAndZoom.Height;

            switch (align)
            {
                case ContentAlignment.BottomCenter:
                    {
                        x = (nButtonW - nImgZoomedW) / 2;
                        y = nButtonH - nImgZoomedH;
                    }
                    break;
                case ContentAlignment.BottomLeft:
                    {
                        x = 0;
                        y = nButtonH - nImgZoomedH;
                    }
                    break;
                case ContentAlignment.BottomRight:
                    {
                        x = nButtonW - nImgZoomedW;
                        y = nButtonH - nImgZoomedH;
                    }
                    break;
                case ContentAlignment.MiddleCenter:
                    {
                        x = (nButtonW - nImgZoomedW) / 2;
                        y = (nButtonH - nImgZoomedH) / 2;
                    }
                    break;
                case ContentAlignment.MiddleLeft:
                    {
                        x = 0;
                        y = (nButtonH - nImgZoomedH) / 2;
                    }
                    break;
                case ContentAlignment.MiddleRight:
                    {
                        x = nButtonW - nImgZoomedW;
                        y = (nButtonH - nImgZoomedH) / 2;
                    }
                    break;
                case ContentAlignment.TopCenter:
                    {
                        x = (nButtonW - nImgZoomedW) / 2;
                        y = 0;
                    }
                    break;
                case ContentAlignment.TopLeft:
                    {
                        x = 0;
                        y = 0;
                    }
                    break;
                case ContentAlignment.TopRight:
                    {
                        x = nButtonW - nImgZoomedW;
                        y = 0;
                    }
                    break;
                default:
                    {

                    }
                    break;

            }

            Rectangle rt = rectVirtualEntityWithOffsetAndZoom;
            rt.X= rt.X + x;
            rt.Y = rt.Y + y;
            rt.Width = nImgZoomedW;
            rt.Height = nImgZoomedH;
            return rt;

        }

        protected virtual void Draw_Logo(Graphics g, Rectangle rectVirtualEntityWithOffset)
        {

        }


        protected virtual bool contains_data()
        {
            return false;
        }
        /// <summary>
        /// K = Key background
        /// L = Legend background
        /// N = Indicator color
        /// A = integer of (K+L)/2
        /// If abs(K-L) > 128 then N = A
        /// If abs(K-L) <=128 then N = (A+128) mod 256
        /// Examples:
        /// R value of key background                       R value of legend background                    R value of indicator
        /// 20                                               50                                             163
        /// 20                                               200                                            110
        /// 160                                              200                                            52
        /// 120                                              140                                            2
        /// </summary>
        /// <returns></returns>
        protected Color GetIndicatorColor()
        {
            if (this.ParentContainer == null)
                return this.ForeColor;
            if (this.ParentContainer.ParentEditor == null)
                return this.ForeColor;

            Color editorBG = this.ParentContainer.ParentEditor.BackColor;
            Color keyBG = this.BackColor;
            int r, g, b;
            r = CalcuateIndicatorColor(keyBG.R, editorBG.R);
            g = CalcuateIndicatorColor(keyBG.G, editorBG.G);
            b = CalcuateIndicatorColor(keyBG.B, editorBG.B);

            Color c = Color.FromArgb(r, g, b);
            return c;
            
        }
        protected int CalcuateIndicatorColor(int keyColor, int legendColor)
        {
            int a = (keyColor + legendColor)/2;
            int kl = Math.Abs(keyColor - legendColor);

            if (kl > 128)
                return a;
            else
                return ((a + 128) % 256);

        }

        /************************************************************************/
        /* 
         * if the key contents is empty, show icon to inform customer.
         * Default:
         *      show a red triangle at left-top corner
         * 
         *   Thanks for the implementation.  It is perfect.  
         *   However, now I realized that the icon hint on status bar is now confusing 
         *   the user as the triangles looks same in the key and in the legend sheet corner.  
         *   So, user may misunderstood.  As the legend sheet corner is fixed, 
         *   I think the only solution is to move the key triangle to either the top-right 
         *   { or the bottom-left x and change the status bar hint accordingly.

         */
        /************************************************************************/
        protected virtual void Draw_Warning(Graphics g, Rectangle rectVirtualEntityWithOffset)
        {
                    
            if (!this.ShowLogo) return;
            if (this.contains_data()) return;
            int x = rectVirtualEntityWithOffset.X;
            int y = rectVirtualEntityWithOffset.Y;

            //triangle of  left-top
            //Point[] pts = new Point[]{
            //    new Point(x, y),
            //    new Point(x + LOGO_SIZE/2, y),
            //    new Point(x, y + LOGO_SIZE/2)

            //};
            //triangle of  top-right; 
            x += rectVirtualEntityWithOffset.Width;
            
            Point[] pts = new Point[]{
                new Point(x, y),
                new Point(x - LOGO_SIZE/2, y),
                new Point(x, y + LOGO_SIZE/2)

            };

           // HatchBrush br = new HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Red, Color.Yellow);
            SolidBrush br = new SolidBrush( GetIndicatorColor());//  this.ForeColor);

            g.FillPolygon(br, pts, FillMode.Winding);

        }

        protected virtual void Draw_Content_Modified(Graphics g, Rectangle rectVirtualEntityWithOffset)
        {

            if (!this.ShowLogo) return;
            if (!this.ContentModified)
                return;
           
           

            int x = rectVirtualEntityWithOffset.Right;
            int y = rectVirtualEntityWithOffset.Bottom;
            //x -=  LOGO_SIZE;
            
            //triangle of right-top
            Point[] pts = new Point[]{
                new Point(x, y),
                new Point(x - LOGO_SIZE/2, y),
                 new Point(x - LOGO_SIZE/2, y - LOGO_SIZE/2),
                new Point(x, y - LOGO_SIZE/2),
               

            };
            //string text = "*";
            //Font ft = new Font("Arial", 10);// this.Font;

            //SizeF sz = g.MeasureString(text, ft, 99999, StringFormat.GenericTypographic);
            //x -= (int)(sz.Width * 3 / 2);
            //y -= (int)(sz.Height);// / 2);

            //g.DrawString(text, ft, new SolidBrush(this.ForeColor), new PointF(x, y));
           // HatchBrush br = new HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Red, Color.Yellow);
            SolidBrush br = new SolidBrush( GetIndicatorColor());// this.ForeColor);

            g.FillPolygon(br, pts, FillMode.Winding);
            
        }

        protected override void Draw(Graphics g, Rectangle rectVirtualEntityWithOffsetAndZoom,double zoom)
       
        {
            
            //ControlPaint.DrawButton(g, rect, ButtonState.Normal);
            DrawBackground(g, rectVirtualEntityWithOffsetAndZoom, zoom);
           
            DrawImage(g, rectVirtualEntityWithOffsetAndZoom, zoom);
	        
            Draw_Logo(g, rectVirtualEntityWithOffsetAndZoom);
            Draw_Warning(g, rectVirtualEntityWithOffsetAndZoom);
            Draw_Content_Modified(g, rectVirtualEntityWithOffsetAndZoom);
            DrawOverlappedArea2(g, rectVirtualEntityWithOffsetAndZoom);
            DrawTitle(g, rectVirtualEntityWithOffsetAndZoom);

           
        }

        
        public override void Export(int nLayerIndex, CLCIXML xml) 
        
        {
           base.Export(nLayerIndex, xml);



           xml.new_attribute("bg", ColorTranslator.ToWin32(this.BackColor).ToString());
           xml.new_attribute("fg", ColorTranslator.ToWin32(this.ForeColor).ToString());
	        base.export_font(xml, _LogFont);
            xml.new_attribute("image", ImageToBase64(this.Image, ImageFormat.Png ) );
            xml.new_attribute("beeppitch", this.BeepPitch.ToString());
            xml.new_attribute("beepduration", this.BeepDuration.ToString());
            xml.new_attribute("roundcornerradius", this.RoundCornerRadius.ToString());
            xml.new_attribute("borderthickness", this.BorderThickness.ToString());
            xml.new_attribute("bordercolor", ColorTranslator.ToWin32(this.BorderColor).ToString());
            xml.new_attribute("textrotation", this.TextRotation.ToString());
            xml.new_attribute("titlealign", ((int)this.TitleAlign).ToString());
            xml.new_attribute("imagealign", ((int)this.ImageAlign).ToString());
            //xml.new_attribute("capseffect", this.CapsEffect.ToString());


	       

	
        }

        public override bool FromXml(CLCIXML xml)
        {
            if (!base.FromXml(xml))
                return false;

            string s = "";

            xml.get_attribute("bg", ref s);// this.BackColor.ToString());
            this.BackColor = ColorTranslator.FromWin32(int.Parse(s));

            xml.get_attribute("fg", ref s);// this.ForeColor.ToString());
            this.ForeColor = ColorTranslator.FromWin32(int.Parse(s));

            if (_LogFont == null) _LogFont = new WinAPI.LOGFONT();
            base.import_font(xml,ref _LogFont);

            xml.get_attribute("image", ref s);
            if (s.Length>0)
                this._Image = Base64ToImage(s);


            xml.get_attribute("beeppitch",ref s);
            if (s.Length >0)
                this.BeepPitch =  int.Parse(s);


            xml.get_attribute("beepduration",ref s);
            if (s.Length > 0)
                this.BeepDuration = int.Parse(s);

            s = "";
            xml.get_attribute("roundcornerradius",ref s);
            if (s != string.Empty)
                this.RoundCornerRadius = int.Parse(s);
            else
                this.RoundCornerRadius = 0;

            s = "0";
            xml.get_attribute("borderthickness",ref s);
            this.BorderThickness = int.Parse(s);


            xml.get_attribute("bordercolor", ref s);// this.ForeColor.ToString());
            this.BorderColor = ColorTranslator.FromWin32(int.Parse(s));

            s = "0";
            xml.get_attribute("textrotation", ref s);
            this.TextRotation = int.Parse(s);



            s = s = ((int)(ContentAlignment.MiddleCenter)).ToString(); 
            xml.get_attribute("titlealign",ref s);
            this.TitleAlign =(ContentAlignment)( int.Parse(s));

            s = ((int)(ContentAlignment.MiddleCenter)).ToString();
            xml.get_attribute("imagealign", ref s);
            this.ImageAlign = (ContentAlignment)(int.Parse(s));

           // s = "false";
           // xml.get_attribute("capseffect", ref s);
           // this.CapsEffect = bool.Parse(s);

            

            return true;
        }


        [Description("Button text")]
        [DisplayName("Key label")]
        [Category("Legend")]
        [Browsable(true)]
        [ReadOnly(false)]
        virtual public string Caption
        {
            get { return this.Title; }
            set
            {
                this.Title = value;
                

            }
        }

        protected ContentAlignment _TitleAlign = ContentAlignment.MiddleCenter;
        [Description("Key text align setting")]
        [DisplayName("Key label align")]
        [Category("Legend")]
        [ReadOnly(false)]
        public ContentAlignment TitleAlign
        {
            get { return _TitleAlign; }
            set
            {
                if (_TitleAlign != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _TitleAlign = value;
                    FirePropertiesChangedEvent();
                }
            }
        }


        protected Color _BackColor = Color.LightGray;
        [Description("Background color")]
        [Category("Legend")]
        [ReadOnly(false)]
        [Editor(typeof(ColorEditorUI), typeof(UITypeEditor))]
        [TypeConverter(typeof(ColorConverterUI))]
        [DisplayName("Fill Color")]
        public Color BackColor
        {
            get { return _BackColor; }
            set {
                if (_BackColor != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BackColor = value;
                    FirePropertiesChangedEvent();
                }
            }
        }

        /*==============================================
        * 
        * 
        * 
        * 
        * 
       ==============================================*/
        protected Color _ForeColor = Color.Black;
        [Description("Foreground color")]
        [Category("Legend")]
        [ReadOnly(false)]
        [Editor(typeof(ColorEditorUI), typeof(UITypeEditor))]
        [TypeConverter(typeof(ColorConverterUI))]
        [DisplayName("Font Color")]
        public Color ForeColor
        {
            get { return _ForeColor; }
            set 
            {

                if (_ForeColor != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _ForeColor = value;
                    FirePropertiesChangedEvent();
                }
            }
        }

        protected Color _BorderColor = Color.Black;
        [Description("Border color")]
        [Category("Legend")]
        [DisplayName("Border color")]
        [ReadOnly(false)]
        [Editor(typeof(ColorEditorUI), typeof(UITypeEditor))]
        [TypeConverter(typeof(ColorConverterUI))]
        public Color BorderColor
        {
            get { return _BorderColor; }
            set
            {
                if (_BorderColor != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BorderColor = value;
                    FirePropertiesChangedEvent();
                }
            }
        }

        protected int _BorderThickness = 0;
        [Description("Border Thickness")]
        [Category("Legend")]
        [DisplayName("Border thickness")]
        [ReadOnly(false)]
        public int BorderThickness
        {
            get { return _BorderThickness; }
            set
            {
                if (_BorderThickness != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    int nmax = get_max_corner_radius();
                    if (value > nmax)
                        _BorderThickness = nmax;
                    else
                        _BorderThickness = value;
                    //_BorderThickness = value;
                    update_round_corner_radius();
                    //if (this.RoundCornerRadius > nmax)
                    //    this._RoundCornerRadius = nmax;
                    FirePropertiesChangedEvent(true);
                }
            }
        }
        override protected void update_border_thickness()
        {
            int nmax = get_max_corner_radius();
            if (this.BorderThickness > nmax)
                this.BorderThickness = nmax;
        }
        protected int _RoundCornerRadius = 0;
        [Description("round corner radius.")]
        [Category("Legend")]
        [DisplayName("Round corner radius")]
        public int RoundCornerRadius
        {
            get { return _RoundCornerRadius; }
            set
            {
                if (_RoundCornerRadius != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    int nmax = get_max_corner_radius();
                    if (value > nmax)
                        _RoundCornerRadius = nmax;
                    else
                        _RoundCornerRadius = value;
                    //if (this.BorderThickness > nmax)
                    //    this._BorderThickness = nmax;

                    update_border_thickness();
                    FirePropertiesChangedEvent(true);
                }

            }
        }

        override protected void update_round_corner_radius()
        {
            int nmax = get_max_corner_radius();
            if (this.RoundCornerRadius > nmax)
                this.RoundCornerRadius = nmax;
        }

        protected int _TextRotation = 0;
        [Description("Text Rotations")]
        [Category("Legend")]
        [DisplayName("Text rotation angle")]
        [ReadOnly(false)]
        public int TextRotation
        {
            get { return _TextRotation; }
            set
            {
                if (_TextRotation != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _TextRotation = value;
                    FirePropertiesChangedEvent();
                }
            }
        }

        protected WinAPI.LOGFONT _LogFont = null;
        [Description("Font")]
        [TypeConverter(typeof(FilteredFontConverter))]
        [Category("Legend")]
        [ReadOnly(false)]
        public Font Font
        {
            get
            {
                //   int nold = 0;
                if (_LogFont == null)
                {
                    _LogFont = new WinAPI.LOGFONT();

                    GetFont(ref _LogFont);

                }


                Font ft = Font.FromLogFont(_LogFont);
                Font font = new Font(ft.FontFamily, Math.Abs(_LogFont.lfHeight), ft.Style, GraphicsUnit.Pixel);

                //System.Diagnostics.Debug.Print(font.ToString());
                return font;


            }
            set
            {
                FirePropertiesBeforeChangedEvent();
                if (_LogFont == null)
                {
                    _LogFont = new WinAPI.LOGFONT();
                }
                this.PropertiesModified = true;
                Font font = value;
                font.ToLogFont(_LogFont);
                FirePropertiesChangedEvent();
            }
        }


        protected Image _Image = null;
        [DefaultValue((string)null)] //allow the "delete"key to set image back to (none)
        [Description("Image")]
        [Category("Legend")]
        [ReadOnly(false)]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _Image = value;
                    FirePropertiesChangedEvent();
                }
            }
        }


        protected ContentAlignment _ImageAlign = ContentAlignment.MiddleCenter;
        //[DefaultValue((string)null)] //allow the "delete"key to set image back to (none)
        [Description("Image align")]
        [Category("Legend")]
        [ReadOnly(false)]
        public ContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                if (_ImageAlign != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _ImageAlign = value;
                    FirePropertiesChangedEvent();
                }
            }
        }


        //protected bool _RoundCorner = false;
        //[Description("Draw key with round corner. ")]
        //[Category("Legend")]
        //public bool RoundCorner
        //{
        //    get { return _RoundCorner; }
        //    set
        //    {
        //        if (_RoundCorner != value)
        //        {


        //            _RoundCorner = value;
        //            FirePropertiesChangedEvent();
        //        }

        //    }
        //}
        //protected int _RoundCornerRadius = 0;
        //[Description("round corner radius.")]
        //[Category("Legend")]
        //public int RoundCornerRadius
        //{
        //    get { return _RoundCornerRadius; }
        //    set
        //    {
        //        if (_RoundCornerRadius != value)
        //        {
        //            int nmax = get_max_corner_radius();
        //            if (value > nmax)
        //                _RoundCornerRadius = nmax;
        //            else
        //                _RoundCornerRadius = value;
        //            FirePropertiesChangedEvent();
        //        }

        //    }
        //}
        
       
        //protected bool _ShowBorder = false;
        //[Description("Show key border lines")]
        //[Category("Legend")]
        //public bool ShowBorder
        //{
        //    get { return _ShowBorder; }
        //    set
        //    {
        //        if (_ShowBorder != value)
        //        {


        //            _ShowBorder = value;
        //            FirePropertiesChangedEvent();
        //        }
        //    }

        //}



        protected Font ZoomFont()
        {
            WinAPI.LOGFONT lf = new WinAPI.LOGFONT();
            Font ft = this.Font;
            ft.ToLogFont(lf);
            lf.lfHeight = (int)decimal.Round((decimal)(lf.lfHeight) * (decimal)this.Zoom);
            return Font.FromLogFont(lf);
        }

       



        //private bool _Beep = false;
        /// <summary>
        /// beep or no beep = 0
        /// </summary>
        /// 
        //[Description("Beep while key pressed")]
        //public bool Beep
        //{
        //    get { return _Beep; }
        //    set
        //    {
        //        if (_Beep != value)
        //        {
        //            _Beep = value;
        //            this.Modified = true;
        //        }
        //    }
        //}
        /// <summary>
        /// range from 1000 -- 4000
        /// 20140408, from 0--255 to 1--10
        /// </summary>
        private int _BeepPitch = KB9Const.DEFAULT_BeepPitch;
        [Description("Beep Pitch value. Value Range: [1, 10]")]//20140408, from 0--255, to 1--10
        [Category("Key")]
        [DisplayName("Beep pitch")]
        [ReadOnly(false)]
        public int BeepPitch
        {
            get { return _BeepPitch; }
            set
            {
                int val = value;
                if (val >=KB9Const.Min_BeepPitch && val <= KB9Const.Max_BeepPitch)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BeepPitch = value;
                    FirePropertiesChangedEvent();
                }
                //20140429, set to max, min
                else if (val < KB9Const.Min_BeepPitch)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BeepPitch = KB9Const.Min_BeepPitch;
                    FirePropertiesChangedEvent();
                }
                else if (val > KB9Const.Max_BeepPitch)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BeepPitch = KB9Const.Max_BeepPitch;
                    FirePropertiesChangedEvent();
                }
            }
        }

        private int _BeepDuration = KB9Const.DEFAULT_BeepDuration;//  3;
        [Description("Beep duration value. Value Range: [0, 10]")] //20140408, from 0--255, to 0--10
        [Category("Key")]
        [DisplayName("Beep duration")]
        [ReadOnly(false)]
        public int BeepDuration
        {
            get { return _BeepDuration; }
            set
            {
                int val = value;
                if (val >=KB9Const.Min_BeepDuration && val <= KB9Const.Max_BeepDuration)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BeepDuration = value;
                    FirePropertiesChangedEvent();
                }
                //20140429, set to max, min
                else if (val < KB9Const.Min_BeepDuration)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BeepDuration = KB9Const.Min_BeepDuration;
                    FirePropertiesChangedEvent();
                }
                else if (val > KB9Const.Max_BeepDuration)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _BeepDuration = KB9Const.Max_BeepDuration;
                    FirePropertiesChangedEvent();
                }
            }
        }

//        protected void enable_caps_lock_effect_property(bool benable)
//        {

//            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["CapsEffect"];
//            ReadOnlyAttribute attribute = (ReadOnlyAttribute)
//                                          descriptor.Attributes[typeof(ReadOnlyAttribute)];
//            FieldInfo fieldToChange = attribute.GetType().GetField("isReadOnly",
//                                             System.Reflection.BindingFlags.NonPublic |
//                                             System.Reflection.BindingFlags.Instance);
//            fieldToChange.SetValue(attribute, (!benable));

//        }

//        /// <summary>
//        /// check if we need to enable/disable caps lock_effect property
//        /// </summary>
//        /// <param name="keyEditingData"></param>
//        protected void check_caps_lock_effect_property_state(KeyEditingType keyEditingData)
//        {
//            return;
//            /*
            
//            string s = keyEditingData.ToString();
//            if (s.Length != 1) //just a -- z and one character can enable it
//            {
//                enable_caps_lock_effect_property(false);
//            }
//            else
//            {
//                char ch = s[0];
//                if (ch >= 'a' && ch <= 'z')
//                    enable_caps_lock_effect_property(true);
//                else
//                    enable_caps_lock_effect_property(false);
//            }
//            FirePropertiesChangedEvent(true);
//             */
//        }

//        /************************************************************************/
//        /* 
//         * 
//         * Add a key property in key: Enable Caps Lock Effect. The idea is to provide
//whether a key is effect by Cap lock or not.  The default option will be off,
//and this option is also grey out by default. It only enable when the key
//contents is a single letter from a to z. When this option is off, nothing
//changes. If this option is on, when writing to keyboard, the single a will
//become [a]. In really keyboard and test output: This [a] when Caps Lock is
//on, it will output A, if Caps Lock is off, it will output a. 
//         */
//        /************************************************************************/
//        private bool _CapsEffect = false;
//        [Description("It only worked when the key contents is a single letter from a to z")]
//        [Category("Key")]
//        [DisplayName("Enable Caps Lock Effect")]
//        [ReadOnly(false)]
//        public bool CapsEffect
//        {
//            get { return _CapsEffect; }
//            set
//            {
                
//                if (_CapsEffect != value)
//                {
//                    this.PropertiesModified = true;
//                    _CapsEffect = value;
//                    FirePropertiesChangedEvent();
//                }
//            }
//        }
//        //Enable Caps Lock Effect
       
        //protected ContentAlignment _ImageAlign = ContentAlignment.MiddleCenter;
        //[Description("Image align setting")]
        //public ContentAlignment ImageAlign
        //{
        //    get { return _ImageAlign; }
        //    set 
        //    {
        //        if (_ImageAlign != value)
        //        {
        //            _ImageAlign = value;
        //            FirePropertiesChangedEvent();
        //        }

        //    }
        //}

        /*==============================================
        * 
        * 
        * 
        * 
        * 
       ==============================================*/
        public static void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.DrawPath(pen, path);
            }
        }
        public static void FillRoundRectangle(Graphics g, Brush brush, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.FillPath(brush, path);
            }
        }
        //internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        //{
        //    GraphicsPath roundedRect = new GraphicsPath();
        //    roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
        //    roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
        //    roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
        //    roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
        //    roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
        //    roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
        //    roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
        //    roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
        //    roundedRect.CloseFigure();
        //    return roundedRect;
        //}
       // public static GraphicsPath Create(int x, int y, int width, int height,
        public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect,
                                      int radius)//, RectangleCorners corners)
        {
            int xw = rect.Right;// x + width;
            int yh = rect.Bottom;// y + height;
            int xwr = xw - radius;
            int yhr = yh - radius;
            int xr = rect.Left + radius;
            int yr = rect.Top + radius;
            int r2 = radius * 2;
            int xwr2 = xw - r2;
            int yhr2 = yh - r2;
            int x = rect.Left;
            int y = rect.Top;

            GraphicsPath p = new GraphicsPath();
            p.StartFigure();

            //Top Left Corner
           // if ((RectangleCorners.TopLeft & corners) == RectangleCorners.TopLeft)
            {
                p.AddArc(x, y, r2, r2, 180, 90);
            }
            //else
            //{
            //    p.AddLine(x, yr, x, y);
            //    p.AddLine(x, y, xr, y);
            //}

            //Top Edge
            p.AddLine(xr, y, xwr, y);

            //Top Right Corner
            //if ((RectangleCorners.TopRight & corners) == RectangleCorners.TopRight)
            {
                p.AddArc(xwr2, y, r2, r2, 270, 90);
            }
            //else
            //{
            //    p.AddLine(xwr, y, xw, y);
            //    p.AddLine(xw, y, xw, yr);
            //}

            //Right Edge
            p.AddLine(xw, yr, xw, yhr);

            //Bottom Right Corner
            //if ((RectangleCorners.BottomRight & corners) == RectangleCorners.BottomRight)
            {
                p.AddArc(xwr2, yhr2, r2, r2, 0, 90);
            }
            //else
            //{
            //    p.AddLine(xw, yhr, xw, yh);
            //    p.AddLine(xw, yh, xwr, yh);
            //}

            //Bottom Edge
            p.AddLine(xwr, yh, xr, yh);

            //Bottom Left Corner
            //if ((RectangleCorners.BottomLeft & corners) == RectangleCorners.BottomLeft)
            {
                p.AddArc(x, yhr2, r2, r2, 90, 90);
            }
            //else
            //{
            //    p.AddLine(xr, yh, x, yh);
            //    p.AddLine(x, yh, x, yhr);
            //}

            //Left Edge
            p.AddLine(x, yhr, x, yr);

            p.CloseFigure();
            return p;
        }

        public override string ExportCVS()
        {
            return CreateCVS();
        }
        //
        /// <summary>
        /// [Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>
        /// e.g: [35,6,60,41][0][0][2]<0x0d><0x0a>
        /// </summary>
        /// <returns></returns>
        public virtual string CreateCVS()
        {
            Rectangle rt = this.UnitsBounds;
            //rt.X *= 2; //0.5mm as unit
            //rt.Y *= 2;
            //rt.Width *= 2;
            //rt.Height *= 2;
            string s = string.Format("[{0},{1},{2},{3}][{4}][{5}]",
                                      rt.X,
                                      rt.Y,
                                      rt.Width,
                                      rt.Height,
                                      this.BeepPitch,
                                      this.BeepDuration);

            return s;
        }
        private void RemoveAllRightBracket(List<string> lst)
        {
            for (int i=0; i< lst.Count; i++)
            {
                string s = lst[i];
                s = s.Replace("[", "");
                lst[i] = s;
            }
        }
        //[126,26,154,42][0][0][S][u][m][m][a][r][y]<0x0d><0x0a>
        public virtual bool FromCSV(string strCSV)
        {
            List<string> ar = new List<string>();
            if (strCSV.Length <= 0) return false;
            if (Util.string2array(strCSV, "]", ar, false) <= 0)
                return false;
            if (ar.Count < 3)
                return false;

            RemoveAllRightBracket(ar);
            

            string s = ar[0];//rectangle
            List<string> arRect = new List<string>();
            if (Util.string2array(s, ",", arRect, false) <= 0)
                return false;
            if (arRect.Count < 4)
                return false;

            int x = Util.string2int(arRect[0], 0);// +10; //0.5 mm unit, convert to 1mm, there are 5mm margin
            int y = Util.string2int(arRect[1], 0);// +10; //+ 5mm margin
            int w = Util.string2int(arRect[2], 0) ;
            int h = Util.string2int(arRect[3], 0) ;

            Rectangle rt = new Rectangle(x, y, w, h);

            this.UnitsBounds = rt;

            //beep
            s = ar[1];
            this.BeepPitch = Util.string2int(s, 0);

            //beep duration
            s = ar[2];
            this.BeepDuration = Util.string2int(s, 0);
            
            return true;

        }

        public virtual bool FromCSV(string strCSV0,string strCSV1,string strCSV2,string strCSV3)
        {
            return FromCSV(strCSV0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strData">Original string</param>
        /// <param name="strFrom">Find this string first, get data after it</param>
        /// <param name="strTo">The end string, get data before it.</param>
        /// <returns></returns>
        protected string get_sub_string(string strData, string strFrom, string strTo)
        {
            int n1 = strData.IndexOf(strFrom);

            if (n1 < 0)
                return "";
            n1 += strFrom.Length;
            int n2 = strData.IndexOf(strTo, n1);

            //n2 -= 1;
            try
            {

                if (n2 < 0)
                {
                    return strData.Substring(n1);
                }
                else
                {
                    return strData.Substring(n1, n2 - n1);
                }

            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                return "";
            }

        }

        protected string SubCSV(string strCSV, int FromBracketIndex)
        {
           
            string s = strCSV;
            //just need data from third "]"
            for (int i = 0; i < FromBracketIndex+1; i++)
            {
                int n = s.IndexOf("]");
                s = s.Substring(n + 1);
            }
            return s;
            

        }

        /// <summary>
        /// 
        /// [a ~z] is the caps effected key content
        /// Emulate keyboard effect 
        /// </summary>
        /// <param name="strKeyContents"></param>
        /// <returns></returns>
        protected bool is_caps_effected_key_contents(string strKeyContents)
        {
            string s = strKeyContents;
            s = s.Replace("\r", "");
            s = s.Replace("\n", "");

            if (s.Length != 3)
                return false;
            if (s.IndexOf("[") < 0 ||
                s.IndexOf("]") < 0)
                return false;
            return true; //20141208
            //char ch = s[1];
            //if (ch >= 'a' && ch <= 'z')
            //    return true;
            //return false;


        }
        override public bool isEqual(DiagramEntity key)
        {
            DiagramKey k = (DiagramKey)key;

            if (!this.BackColor.Equals(k.BackColor))   return false;
            if (!this.BeepDuration.Equals(k.BeepDuration)) return false;
            if (!this.BeepPitch.Equals(k.BeepPitch)) return false;
            if (!this.BorderColor.Equals(k.BorderColor)) return false;
            if (!this.BorderThickness.Equals( k.BorderThickness)) return false;
            if (!this.Bounds.Equals(k.Bounds)) return false;
            if (!this.Caption.Equals(k.Caption)) return false;
            if (!this.Font.Equals(k.Font)) return false;
            if (!this.ForeColor.Equals( k.ForeColor)) return false;
            //if (this.Image != null && key.Image != null)
            //{
            //    if (!this.Image.Equals(key.Image)) return false;
            //}
            //else
            //{
            //    if (this.Image == null && key.Image == null)
            //    {
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            if (!this.ImageAlign.Equals(k.ImageAlign)) return false;
            
            
            if (!this.TextRotation.Equals(k.TextRotation)) return false;
            if (!this.Title.Equals(k.Title)) return false;
            if (!this.TitleAlign.Equals(k.TitleAlign)) return false;
            
            return true;
        }
       
    }

    public class FilteredFontConverter : FontConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
        
            return null;
        
        }


        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
                                         CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((Font)value).FontFamily.Name + "," + Math.Round(( (Font)value).SizeInPoints, 0).ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                                            CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] v = ((string)value).Split(new char[] { ',' });
                float f  = float.Parse(v[1]);
               //int dpi =  Screen.PrimaryScreen.BitsPerPixel;// CreateGraphics();
                Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
                float pixels = f * graphics.DpiX / 72;
                return new Font(v[0], (float)(pixels) , GraphicsUnit.Pixel);
                //return new Point(int.Parse(v[0]), int.Parse(v[1]));
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

       
    }
}

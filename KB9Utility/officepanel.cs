using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace KB9Utility
{
    class OfficePanel : System.Windows.Forms.Panel
    {


        public OfficePanel()
        {
            //timer1.Interval = 1;
            //timer1.Tick += new EventHandler(timer1_Tick);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        
        int X0;
        int XF;
        int Y0;
        int YF;
        int T = 2;
        int i_Zero = 180;
        int i_Sweep = 90;
        int X; int Y;
        GraphicsPath path;
        int D = -1;
        int R0 = 215;
        int G0 = 227;
        int B0 = 242;
        Color _BaseColor = Color.FromArgb(215,227,242);
        Color _BaseColorOn = Color.FromArgb(215, 227, 242);
        //int i_mode = 0; //0 Entering, 1 Leaving
        //int i_factor = 8;
        //int i_fR = 1; int i_fG = 1; int i_fB = 1;
        int i_Op = 255;
        
        string S_TXT = "";

        public Color BaseColor
        {
            get
            {
                return _BaseColor;
            }
            set
            {
                _BaseColor = value;
                R0 = _BaseColor.R;
                B0 = _BaseColor.B;
                G0 = _BaseColor.G;
            }
        }

        public Color BaseColorOn
        {
            get
            {
                return _BaseColorOn;
            }
            set
            {
                _BaseColorOn = value;
                R0 = _BaseColor.R;
                B0 = _BaseColor.B;
                G0 = _BaseColor.G;
            }
        }

        public string Caption
        {
            get 
            {
                return S_TXT;
            }
            set 
            {
                S_TXT = value;
                this.Refresh();
            }
        }


        public int Opacity
        {
            get
            {
                return i_Op;
            }
            set
            {
                if (value < 256 | value > -1)
                { i_Op = value; }
            }

        }

         
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            X0 = 0; XF = this.Width + X0 - 3;
            Y0 = 0; YF = this.Height + Y0 -3 ;

            Point P0 = new Point(X0, Y0);
            Point PF = new Point(X0, Y0 + YF - 15);

            Pen b1 = new Pen(Color.FromArgb(i_Op,R0 - 18, G0 - 17, B0 - 19));
            Pen b2 = new Pen(Color.FromArgb(i_Op,R0 - 39, G0 - 24, B0 - 3));
            Pen b3 = new Pen(Color.FromArgb(i_Op,R0 + 14, G0 + 9, B0 + 3));
            Pen b4 = new Pen(Color.FromArgb(i_Op,R0 - 8, G0 - 4, B0 - 2));
            Pen b5 = new Pen(Color.FromArgb(i_Op,R0, G0, B0));
            Pen b6 = new Pen(Color.FromArgb(i_Op,R0 - 16, G0 - 11, B0 - 5));
            Pen b8 = new Pen(Color.FromArgb(i_Op,R0, G0 + 4, B0));
            Pen b7 = new Pen(Color.FromArgb(i_Op,R0 - 22, G0 - 10, B0));

            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            Brush B4 = b4.Brush;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            X = X0; Y = Y0; i_Zero = 180; D = 0;
            DrawArc();
            e.Graphics.DrawPath(b1, path);
            DrawArc();
            e.Graphics.DrawPath(b2, path);
            DrawArc();
            e.Graphics.DrawPath(b3, path);

            DrawArc2(0, 20);
            e.Graphics.FillPath(b5.Brush, path);
            LinearGradientBrush brocha = new LinearGradientBrush(P0, PF, b6.Color,Color.FromArgb(215,231,245));// b8.Color);
            //LinearGradientBrush brocha = new LinearGradientBrush(P0, PF, Color.Red, Color.White);
            DrawArc2(15, YF - 15);
            e.Graphics.FillPath(brocha, path);
            DrawArc2(YF - 18, 18);
            
            Point P_EX = Cursor.Position;
            P_EX = this.PointToClient(P_EX);

            //int ix = this.Width / 2 - S_TXT.Length*(int)this.Font.Size/2;
            SizeF szstr = measureString(e.Graphics, S_TXT, this.Font);
            int ix = this.Width / 2 - (int)(szstr.Width)/2;
            PointF P_TXT = new PointF(ix, this.Height-18);
            Pen pen = new Pen(this.ForeColor);
            e.Graphics.DrawString(S_TXT,this.Font,pen.Brush,P_TXT);
            
            base.OnPaint(e);
        }

        protected SizeF measureString(Graphics g, string s, Font f)
        {
            g.PageUnit = GraphicsUnit.Pixel;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            SizeF sz = g.MeasureString(s, f, 99999, StringFormat.GenericTypographic);
            sz.Width += 0.3F;
            

            return sz;
        }
     

        public void DrawArc()
        {
            X = X0; Y = Y0; i_Zero = 180; D++;
            path = new GraphicsPath();
            path.AddArc(X + D, Y + D, T, T, i_Zero, i_Sweep); i_Zero += 90; X += XF;
            path.AddArc(X - D, Y + D, T, T, i_Zero, i_Sweep); i_Zero += 90; Y += YF;
            path.AddArc(X - D, Y - D, T, T, i_Zero, i_Sweep); i_Zero += 90; X -= XF;
            path.AddArc(X + D, Y - D, T, T, i_Zero, i_Sweep); i_Zero += 90; Y -= YF;
            path.AddArc(X + D, Y + D, T, T, i_Zero, i_Sweep);
        }

        public void DrawArc2(int OF_Y, int SW_Y)
        {
            X = X0; Y = Y0 + OF_Y; i_Zero = 180;
            path = new GraphicsPath();
            path.AddArc(X + D, Y + D, T, T, i_Zero, i_Sweep); i_Zero += 90; X += XF;
            path.AddArc(X - D, Y + D, T, T, i_Zero, i_Sweep); i_Zero += 90; Y += SW_Y;
            path.AddArc(X - D, Y - D, T, T, i_Zero, i_Sweep); i_Zero += 90; X -= XF;
            path.AddArc(X + D, Y - D, T, T, i_Zero, i_Sweep); i_Zero += 90; Y -= SW_Y;
            path.AddArc(X + D, Y + D, T, T, i_Zero, i_Sweep);
        }

       

        
    }
}

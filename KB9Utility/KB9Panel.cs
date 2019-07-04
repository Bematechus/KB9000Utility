using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace KB9Utility
{
    public class KB9Panel : Panel
    {
        private int _BarHeight = 18;
        public int BarHeight
        {
            get
            {
                return _BarHeight;
            }
            set
            {
                _BarHeight = value;
            }
        }

        private string _Title = "";
        public string Title
        {
            get 
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        private Color _TopBarColor = Color.FromArgb(199, 216, 237);
        public Color TopBarColor
        {
            get { return _TopBarColor; }
            set { _TopBarColor = value;}
        }

        private Color _BottomBarColor = Color.FromArgb(194, 216, 240);
        public Color BottomBarColor
        {
            get { return _BottomBarColor; }
            set { _BottomBarColor = value; }
        }

        private Color _BorderColor = Color.FromArgb(199, 216, 237);
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }


        public KB9Panel()
        {
            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            
            if (this.ClientRectangle.Width <= 0 ||
                this.ClientRectangle.Height <= 0) return;
            LinearGradientBrush br = new LinearGradientBrush(this.ClientRectangle, this.ColorFrom, this.ColorTo, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(br, this.ClientRectangle);

        }

        private Color _ColorFrom = Color.FromArgb(200,217,237);
        public Color ColorFrom
        {
            get{
                return _ColorFrom;
            }
            set
            {
                _ColorFrom = value;
            }
        }

        private Color _ColorTo = Color.FromArgb(227, 244, 255);
        public Color ColorTo
        {
            get
            {
                return _ColorTo;
            }
            set
            {
                _ColorTo = value;
            }
        }
    }
}

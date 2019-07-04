using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace KB9Utility
{
    public partial class frmPreview : Form
    {

        //private int begin_x;        //图片开始位置
        //private int begin_y;


        //private Image image_ori;    //最原始的图片
        //private Image image_dest;   //经缩放后的图片

        //private float zoom;           //缩小放大百份比，每10%为一个阶梯。每次缩放都基于最原始的图片

        //private Point m_StarPoint = Point.Empty;        //for 拖动
        //private Point m_ViewPoint = Point.Empty;
        //private bool m_StarMove = false;

        //int w;                      //缩放后的图片大小
        //int h;

        //Image img;
        //Point mouseDown;
        //int startx = 0;                         // offset of image when mouse was pressed
        //int starty = 0;
        //int imgx = 0;                         // current offset of image
        //int imgy = 0;

        //bool mousepressed = false;  // true as long as left mousebutton is pressed
        //float zoom = 1;
        bool m_bMousePressed = false;
        Point m_ptMouseDown;
        Point m_ptScrollDown;
        Point m_ptImage = new Point(0, 0);
        Point m_ptStart = new Point(0, 0);
        float m_zoom = 1;

        private Image _Image = null;
        public Image DisplayImage
        {
            get 
            {
                return _Image;
            }
            set
            {
                _Image = value;
            }
        }

        public frmPreview()
        {
            InitializeComponent();
            
            
        }

        private void frmPreview_Load(object sender, EventArgs e)
        {
           // pictureBox.Image = this.DisplayImage;
            //img = this.DisplayImage;
           // picViewer.Image = this.DisplayImage;

            Graphics g = this.CreateGraphics();

            //// Fit whole image
            //zoom = Math.Min(
            //    ((float)pictureBox.Height / (float)img.Height) * (img.VerticalResolution / g.DpiY),
            //    ((float)pictureBox.Width / (float)img.Width) * (img.HorizontalResolution / g.DpiX)
            //);

            // Fit width
            m_zoom = ((float)picViewer.Width / (float)this.DisplayImage.Width);// * (this.DisplayImage.HorizontalResolution / g.DpiX);
           // m_zoom = 1;
            picViewer.AutoScrollMinSize = new Size((int)(this.DisplayImage.Width * m_zoom), (int)(this.DisplayImage.Height * m_zoom));
            picViewer.Refresh();
        }
        static public void PreviewImage(Image img)
        {
            frmPreview frm = new frmPreview();
            frm.DisplayImage = img;
            frm.ShowDialog();
        }

        //Point m_ptStart;
        //private void picPreview_MouseDown(object sender, MouseEventArgs e)
        //{
        //    MouseEventArgs mouse = e as MouseEventArgs;

        //    if (mouse.Button == MouseButtons.Left)
        //    {
        //        if (!mousepressed)
        //        {
        //            mousepressed = true;
        //            mouseDown = mouse.Location;
        //            startx = imgx;
        //            starty = imgy;
        //        }
        //    }

        //}

        //private void picPreview_MouseMove(object sender, MouseEventArgs e)
        //{
        //    MouseEventArgs mouse = e as MouseEventArgs;

        //    if (mouse.Button == MouseButtons.Left)
        //    {
        //        Point mousePosNow = mouse.Location;

        //        int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
        //        int deltaY = mousePosNow.Y - mouseDown.Y;

        //        imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
        //        imgy = (int)(starty + (deltaY / zoom));

        //        pictureBox.Refresh();
        //    }

        //}

        //private void picPreview_MouseUp(object sender, MouseEventArgs e)
        //{
        //    mousepressed = false;

        //}

        //protected override void OnMouseWheel(MouseEventArgs e)
        //{
        //    float oldzoom = zoom;

        //    if (e.Delta > 0)
        //    {
        //        zoom += 0.1F;
        //    }

        //    else if (e.Delta < 0)
        //    {
        //        zoom = Math.Max(zoom - 0.1F, 0.01F);
        //    }

        //    MouseEventArgs mouse = e as MouseEventArgs;
        //    Point mousePosNow = mouse.Location;

        //    int x = mousePosNow.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
        //    int y = mousePosNow.Y - pictureBox.Location.Y;

        //    int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
        //    int oldimagey = (int)(y / oldzoom);

        //    int newimagex = (int)(x / zoom);     // Where in the IMAGE will it be when the new zoom i made
        //    int newimagey = (int)(y / zoom);

        //    imgx = newimagex - oldimagex + imgx;  // Where to move image to keep focus on one point
        //    imgy = newimagey - oldimagey + imgy;
            
        //    pictureBox.Refresh();  // calls imageBox_Paint
        //}

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            float oldzoom = m_zoom;

            if (e.Delta > 0)
            {
                m_zoom += 0.1F;
            }

            else if (e.Delta < 0)
            {
                m_zoom = Math.Max(m_zoom - 0.1F, 0.1F);
            }

            MouseEventArgs mouse = e as MouseEventArgs;
            Point mousePosNow = mouse.Location;

            int x = mousePosNow.X - picViewer.Location.X;    // Where location of the mouse in the pictureframe
            int y = mousePosNow.Y - picViewer.Location.Y;

            int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / m_zoom);     // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int)(y / m_zoom);

            m_ptImage.X = newimagex - oldimagex + m_ptImage.X;// imgx;  // Where to move image to keep focus on one point

           m_ptImage.Y = newimagey - oldimagey + m_ptImage.Y;// imgy;
           Size sz = new Size((int)(this.DisplayImage.Width * m_zoom), (int)(this.DisplayImage.Height * m_zoom)); 

           picViewer.AutoScrollMinSize =sz;
           //picViewer.AutoScrollPosition = m_ptImage;
           //picViewer.Location = m_ptImage;
           //picViewer.Size = new Size((int)(this.DisplayImage.Width * m_zoom), (int)(this.DisplayImage.Height * m_zoom));
           picViewer.Refresh();  // calls imageBox_Paint
        }
       
        private void picPreview_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.Clear(Color.Black);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
           // e.Graphics.ScaleTransform(m_zoom, m_zoom);
            //e.Graphics.DrawImage(this.DisplayImage, m_ptImage.X, m_ptImage.Y);
            //e.Graphics.DrawImage(this.DisplayImage,picViewer.AutoScrollOffset.X, picViewer.AutoScrollOffset.Y);// m_ptImage.X, m_ptImage.Y);
            Point pt = picViewer.AutoScrollPosition;
            //pt.X *= -1;
            //pt.Y *= -1;
            Rectangle rt = new Rectangle(pt, picViewer.AutoScrollMinSize);
            Size szImg = picViewer.AutoScrollMinSize;
            Size szClient = picViewer.ClientRectangle.Size;
            if (szImg.Height < szClient.Height)
                rt.Y = (szClient.Height - szImg.Height) / 2;
            if (szImg.Width < szClient.Width)
                rt.X = (szClient.Width - szImg.Width) / 2;
            e.Graphics.DrawImage(this.DisplayImage, rt);



        }
        /************************************************************************/
        /* 
         * 
         */
        /************************************************************************/

        
        private void picViewer_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                if (!m_bMousePressed)
                {
                    m_bMousePressed = true;
                    m_ptMouseDown = mouse.Location;
                    m_ptStart = m_ptImage;
                    m_ptScrollDown = picViewer.AutoScrollPosition;
                }
            }
        }

        //protected override void  OnMouseMove(MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        ////    MessageBox.Show("1");
        //}
        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    base.OnMouseDown(e);
        //    MessageBox.Show("2");
        //}
        private void picViewer_MouseMove(object sender, MouseEventArgs e)
        {

            
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                picViewer.Cursor = Cursors.SizeAll;
                Point mousePosNow = mouse.Location;
                Point scrDown = picViewer.PointToScreen(m_ptMouseDown);
                Point scrNow = picViewer.PointToScreen(mousePosNow);
                //int deltaX = mousePosNow.X - m_ptMouseDown.X; // the distance the mouse has been moved since mouse was pressed
                //int deltaY = mousePosNow.Y - m_ptMouseDown.Y;

                int deltaX = scrNow.X - scrDown.X; // the distance the mouse has been moved since mouse was pressed
                int deltaY = scrNow.Y - scrDown.Y;

                m_ptImage.X = (int)(m_ptStart.X + (deltaX / m_zoom));  // calculate new offset of image based on the current zoom factor
                m_ptImage.Y = (int)(m_ptStart.Y + (deltaY / m_zoom));
                Point pt = m_ptScrollDown;// picViewer.AutoScrollPosition;
                pt.X =Math.Abs( pt.X) - (int)(deltaX / m_zoom);
                pt.Y =Math.Abs(pt.Y) - (int)(deltaY / m_zoom);
                //ptOld.X = Math.Abs(ptOld.X);
                //ptOld.Y = Math.Abs(ptOld.Y);
                //if (ptOld.X * m_ptScrollDown.X < 0)
                //    ptOld.X = 0;
                //if (ptOld.Y * m_ptScrollDown.Y < 0)
                //    ptOld.Y = 0;
                picViewer.AutoScrollPosition = pt;

                //picViewer.Location = m_ptImage;
                picViewer.Refresh();
            }

        }

        private void picViewer_MouseUp(object sender, MouseEventArgs e)
        {
            picViewer.Cursor = Cursors.Arrow;
            //picViewer.Location = m_ptImage;
            m_bMousePressed = false;
        }

        private void picViewer_Scroll(object sender, ScrollEventArgs e)
        {
            picViewer.Refresh();
        }



        /*

        /// <summary>
        /// 缩放最原始的图片到image_dest
        /// </summary>
        private void zoom_image(bool chec)
        {
            w = Convert.ToInt32(image_ori.Width * zoom / 100);
            h = Convert.ToInt32(image_ori.Height * zoom / 100);
            if (w < 1 || h < 1) return;
            if (chec)
            {
                if (begin_x + pictureBox.Width > w) begin_x = w - pictureBox.Width;
                if (begin_y + pictureBox.Height > h) begin_y = h - pictureBox.Height;
                if (begin_x < 0) begin_x = 0;
                if (begin_y < 0) begin_y = 0;
            }
            Bitmap resizedBmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(resizedBmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.DrawImage(image_ori, new Rectangle(0, 0, w, h), new Rectangle(0, 0, image_ori.Width, image_ori.Height), GraphicsUnit.Pixel);

            int ww, hh;
            ww = w;
            hh = h;
            if (pictureBox.Width < ww) ww = pictureBox.Width;
            if (pictureBox.Height < hh) hh = pictureBox.Height;
            try
            {
                pictureBox.Image = resizedBmp.Clone(new RectangleF((float)begin_x, (float)begin_y, ww, hh), PixelFormat.Format24bppRgb);   //在图片框上显示区域图片
            }
            catch
            {

            }
            g.Dispose();
        }

        private void picPreview_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
            m_StarMove = true;
            m_StarPoint = e.Location; 
        }

        private void picPreview_MouseMove(object sender, MouseEventArgs e)
        {
            m_StarMove = true;
        }

        private void picPreview_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_StarMove)
            {
                int x, y;
                x = m_StarPoint.X - e.X;
                y = m_StarPoint.Y - e.Y;

                if (x > 0)
                {
                    if (begin_x + x <= w - pictureBox.Width) begin_x += x;
                    else begin_x = w - pictureBox.Width;
                }
                else
                {
                    if (begin_x + x <= 0) begin_x = 0;
                    else begin_x += x;
                }
                if (y > 0)
                {
                    if (begin_y + y <= h - pictureBox.Height) begin_y += y;
                    else begin_y = h - pictureBox.Height;
                }
                else
                {
                    if (begin_y + y <= 0) begin_y = 0;
                    else begin_y += y;
                }
                zoom_image(false);
            }
            m_StarMove = false;
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            float min = pictureBox.Width / image_ori.Width;
            float min_y = pictureBox.Height / image_ori.Height;
            if (min < min_y) min = min_y;
            else min = min_y;

            zoom -= 10;
            if (zoom < min) zoom = min;
            zoom_image(true);
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            float min = pictureBox.Width / image_ori.Width;
            float min_y = pictureBox.Height / image_ori.Height;
            if (min < min_y) min = min_y;
            else min = min_y;

            zoom += 10;
            if (zoom > 200) zoom = 200;
            zoom_image(true);
        }
         * */
    }
}
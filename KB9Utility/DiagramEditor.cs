using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Design;

namespace KB9Utility
{
    public partial class DiagramEditor : ScrollableControl 
    {
        private const int MAX_ENTITIES_COUN = 64;

        public enum RESTRAIN
        {
            RESTRAINT_NONE = 0,
            RESTRAINT_VIRTUAL,
            RESTRAINT_MARGIN,
        };
        public enum KeyInterface
        {
            KEY_NONE = 0,
            KEY_ARROW = 1, // Arrow keys
            KEY_PGUPDOWN = 4,	// Pg up & pg down
            KEY_DELETE = 8,	// Delete key
            KEY_ESCAPE = 16,	// Escape key
            KEY_INSERT = 32,	// Insert key
            KEY_PLUSMINUS = 64,	// Plus- and minus key
            KEY_CTRL = 128,	// Ctrl+A,Z,X,C,V, Enter
            KEY_ALL = int.MaxValue// 0xFFFFFFFF,
        };
        public enum WheelMode
        {
            WHEEL_SCROLL = 0,
            WHEEL_ZOOM
        };

        public enum MouseMode
        {
            // Current mouse mode
            MODE_NONE = 0,
            MODE_RUBBERBANDING,
            MODE_MOVING,
            MODE_RESIZING,
            MODE_DRAWING,
            MODE_BGRESIZING,
        };

        public enum EditorCmd
        {
            EDIT_CUT = 0,
	        EDIT_COPY,
	        EDIT_PASTE,
            EDIT_UNDO,
	        EDIT_GROUP,
	        EDIT_UNGROUP,

            EDIT_SIZE_WIDTH,
            EDIT_SIZE_HEIGHT,
            EDIT_SIZE_BOTH,

            EDIT_ALIGN_TOP,
            EDIT_ALIGN_BOTTOM,
            EDIT_ALIGN_LEFT,
            EDIT_ALIGN_RIGHT,
            EDIT_ALIGN_MIDDLE,
            EDIT_ALIGN_CENTER

        };


        public delegate void EventShowDiagramEntityProperties(DiagramEditor sender, DiagramEntity entity);
        public event EventShowDiagramEntityProperties OnShowDiagramEntityProperties;

        public delegate void EventChanged(DiagramEditor sender);
        public event EventChanged OnChanged;


        public delegate void EventEditEntityKeyContent(DiagramEditor sender, DiagramEntity entity);
        public event EventEditEntityKeyContent OnEditEntityKeyContent;

        //if we change the properties (round corder radius or thickness), need this event to refresh grid
        public delegate void EventRefreshPropertiesGrid(DiagramEditor sender, DiagramEntity entity);
        public event EventRefreshPropertiesGrid OnRefreshPropertiesGrid;

        public delegate void EventEditorUndo(DiagramEditor sender, UndoItem undo);
        public event EventEditorUndo OnEditorUndo;

        private MouseMode m_interactMode;		// Current mouse-mode
        private DiagramEntity.DEHT m_subMode;			// Sub-mode for resizing (corner)
        private Rectangle m_selectionRect;	// Rect to draw for rubberbanding
        private Point m_deltaPoint;		// Offset to object when moving
  

        private DiagramEntity m_multiSelObj;	// Primary object when moving multiple

        private Point m_contextMenuPoint;
      

        private KeyInterface m_keyInterface;		// Flags for the keys the editor will handle
//        private Timer m_middleButtonPanningTimer = new Timer();
        // Data pointer
        protected DiagramEntityContainer m_objs;// = new DiagramEntityContainer(null) ;	// Pointer to data

        private Object _ParentObject = null;
         [Browsable(false)]
        public Object ParentForm
        {
            get { return _ParentObject; }
            set { _ParentObject = value; }
        }

        //private WheelMode m_scrollwheel;		// Scroll wheel mode, scoll or zoom
        ///////////////////////////////////////////////////////////////////////////////

        public DiagramEditor()
        {

            InitializeComponent();

            //this.TabStop = true;

            //this.SetStyle(ControlStyles.UserPaint
            //    | ControlStyles.UserMouse
            //    | ControlStyles.ResizeRedraw
            //    | ControlStyles.AllPaintingInWmPaint
            //    | ControlStyles.Opaque
            //    | ControlStyles.CacheText
            //    | ControlStyles.EnableNotifyMessage

            //    //| ControlStyles.OptimizedDoubleBuffer
            //    , true);
            //this.SetStyle(ControlStyles.ContainerControl
            //    | ControlStyles.OptimizedDoubleBuffer
            //    , false);
            //this.DoubleBuffered = true;
            //this.UpdateStyles();

            this.TabStop = true;

            this.BackColor = SystemColors.Window;
            
            
            this.BackgroundResizeZone = 10;
            this.BackgroundResizeSelected = false;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            m_objs = null;
            m_multiSelObj = null;
        

            m_keyInterface = KeyInterface.KEY_ALL;

        
            Clear();
            m_objs = new DiagramEntityContainer(null);

            m_objs.OnAddNew += new DiagramEntityContainer.EventAddNew(OnContainerAddNew);
            m_objs.ParentEditor = this;
            Create();
            this.DoubleBuffered = true;
            this.AutoScroll = true;
            this.Zoom = 0.2;
            
            FireShowPropertiesEvent(null);
        }

      
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WinAPI.UM_REFRESH)
            {
                this.RedrawWindow();
            //    OnSetCursor();
            }
            else
                base.WndProc(ref m);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
            
            Graphics g = pe.Graphics;
                     

            // Getting coordinate data
            Rectangle rect = this.ClientRectangle;


            Rectangle totalRectAfterZoomAndOffset;
            int virtwidth = (int)decimal.Round((decimal)((decimal)this.VirtualSize.Width) * (decimal)this.Zoom);// +1;
            int virtheight = (int)decimal.Round((decimal)((decimal)this.VirtualSize.Height) * (decimal)this.Zoom);// +1;
            totalRectAfterZoomAndOffset = new Rectangle(0, 0, virtwidth, virtheight);

            int x = 0;
            if (this.HorizontalScroll.Visible)
                x = this.AutoScrollPosition.X;
            int y = 0;
            if (this.VerticalScroll.Visible)
                y =  this.AutoScrollPosition.Y;
            totalRectAfterZoomAndOffset.Offset(x, y);
           
            g.SetClip(this.ClientRectangle);
            // Painting
            EraseBackground(g, rect);

            Draw(g, totalRectAfterZoomAndOffset, false);

           
            //ControlPaint.DrawBorder(pe.Graphics, ClientRectangle,
            //                     Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset,
            //                     Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset,
            //                     Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset,
            //                     Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset);
        }

        /////////////////////////////////////////////////////////////////


        public void Clear()
        /* ============================================================
	        Function :		Clear
	        Description :	Clears internal run-time variables.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to reset internal states.

           ============================================================*/
        {

            // Clearing internal states and vars
            m_selectionRect = new Rectangle(0, 0, 0, 0);// SetRectEmpty();
            m_interactMode = MouseMode.MODE_NONE;
            //m_zoom = 1.0;
            this.BackgroundResizeSelected = false;
             //m_bgResizeSelected = false;
            m_deltaPoint = new Point(0, 0);
            m_multiSelObj = null;
            if (m_objs != null)
                m_objs.ClearMultipleSelectedMain();
            //m_drawing = false;
            _IsDrawingObj = false;
            //this.MiddleButtonPanning = false;
            //SetPanning(false);

            // delete m_drawObj;
            //m_drawObj = null;
            this.DrawingObject = null;

            if (this.Handle != null)
            {

                SetupScrollbars();
                SetHScroll(0);
                SetVScroll(0);

                
                RedrawWindow();

            }

        }

        
        protected void RedrawWindow()
        {
            this.Refresh();
        }
        public  bool Create()
        /* ============================================================
	        Function :		Create
	        Description :	Creates a "CDiagramEditor" window.
	        Access :		Public

	        Return :		bool							-	"true" if success
	        Parameters :	DWORD dwStyle					-	Window styles for 
														        the editor
					        const RECT &rect				-	Window rectangle
					        CWnd *pParentWnd				-	Parent of the 
														        editor
					        CDiagramEntityContainer* data	-	Pointer to data. 
														        Might be "null".
        					
	        Usage :			If data is "null", a "CDiagramEntityContainer" 
					        will be created internally, and the instance 
					        of the editor will be responsible for the 
					        container deletion.

           ============================================================*/
        {
            //if (data == null)
            //{
            //    m_internalData = new DiagramEntityContainer(null);
            //    SetInternalDiagramEntityContainer(m_internalData);
            //}
            //else
            //    SetDiagramEntityContainer(data);


            //bool res = CWnd::Create( null, null, dwStyle, rect, pParentWnd, null );
            Size virtualSize;
            Rectangle rect = new Rectangle(this.Location, new Size(this.Width, this.Height));
            if (this.VirtualSize.Width == 0 && this.VirtualSize.Height == 0)
                virtualSize = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            else
                virtualSize = this.VirtualSize;

            SetInternalVirtualSize(virtualSize);


            return true;

        }

        public void New()
        /* ============================================================
	        Function :		New
	        Description :	Clears the current 'page' and creates a 
					        new one.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to start a new drawing.

           ============================================================*/
        {
            
            //SetRedraw(false);
            Clear();
            m_objs.Clear();
            this.BackColor = Color.White;
            this.BorderColor = Color.White;
            this.Image = null;
            //SetRedraw(true);
            RedrawWindow();

        }

       

        [Browsable(false)]
       public  DiagramEntityContainer DiagramEntityContainer
        /* ============================================================
            Function :		GetDiagramEntityContainer
            Description :	Returns a pointer to the data container.
            Access :		Public

            Return :		CDiagramEntityContainer*	-	The current data 
                                                            container (might 
                                                            be "null").
            Parameters :	none

            Usage :			If modifications are made to the contents of 
                            the container, "SetModified" must be called as 
                            appropriate. If visual changes are expected, 
                            "RedrawWindow" must be called for the editor.

           ============================================================*/
        {

           get {   return m_objs; }

        }

        public void Draw( Graphics g, Rectangle rectVirtualWithOffsetAndZoom, bool bToImage ) 
        /* ============================================================
	        Function :		Draw
	        Description :	Calls a series of (virtual) functions to 
					        draw to "dc". "rect" is the total rectangle 
					        to draw to.
	        Access :		Public

	        Return :		void
	        Parameters :	CDC* dc		-	The "CDC" to draw to.
					        Rectangle rect	-	The complete rectangle 
									        (including non-visible areas)
        					
	        Usage :			Should not normally be called from user code. 
					        Can be called to draw the complete window. 
					        Can be overriden to change drawing order.

           ============================================================*/
        {

            double zoom = this.Zoom;

            DrawBackground(g, rectVirtualWithOffsetAndZoom, zoom);

            //prepare the grid location coordinates for draw objects
            DrawGrid(g, rectVirtualWithOffsetAndZoom, zoom, true);
            //if (this.ShowGridLines)
            //    DrawGrid(g, rectVirtualWithOffsetAndZoom, zoom);


            DrawObjects(g, rectVirtualWithOffsetAndZoom, zoom, bToImage);
            if (this.ShowVirtualScreenMargin)
                DrawMargins(g, rectVirtualWithOffsetAndZoom, zoom);

	        if( this.BackgroundSizable && this.BackgroundResizeSelected )
		        DrawSelectionMarkers( g );
            if (this.ShowGridLines)
                DrawGrid(g, rectVirtualWithOffsetAndZoom, zoom, false);
	        //if( GetPanning() )
            //if (this.MiddleButtonPanning)
            //    DrawPanning(g, rectVirtualWithOffset);

        }

    

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor painting virtuals

       protected void EraseBackground(Graphics g, Rectangle rect ) 
        /* ============================================================
	        Function :		EraseBackground
	        Description :	Draws the non-client background
	        Access :		Protected

	        Return :		void
	        Parameters :	CDC* dc		-	"CDC" to draw to.
					        Rectangle rect	-	Total rect to draw to.
        					
	        Usage :			Virtual. Can be overridden in a derived class 
					        to erase the non-client area.

           ============================================================*/
        {
            SolidBrush br = new SolidBrush( this.NoneClientBackgourndColor);
            
            g.FillRectangle(br, rect);
	        //dc.FillSolidRect( rect, m_nonClientBkgndCol );

        }
        /// <summary>
       ///  Please add a triangle mark to the top left of legend sheet (not the layout area), 
       ///  similar to the empty key content triangle.  The triangle size should be 2.5mm x 2.5mm of the 
       ///  legend sheet scale (that is zoom in and out with legend sheet like the key layouts).  
       ///  Color = [legend sheet background color +128] mod 256, for all 3 RGB values.  
       ///  This is for easy identification of the orientation of the legend sheet during installation 
       ///  into the KB9000 by cutting this triangle away when cutting out the legend sheet.

        /// </summary>
        /// <param name="gc"></param>
        /// <param name="rectLegendAfterZoom"></param>
        /// <param name="zoom"></param>
       protected void draw_top_left_triangle(Graphics gc, Rectangle rectLegendAfterZoom, double zoom)
       {
           int nW = 30; //default size, 2.5mm/0.5*6pixel = 30.
           int nH = 30;
           nW = (int)(nW * zoom);
           nH = (int)(nH * zoom);

           Color bg = Color.White;// this.BackColor;
           //int r = ( (bg.R + 128) % 256 );
           //int g = ((bg.G + 128) % 256);
           //int b = ((bg.B + 128) % 256);
           //bg = Color.FromArgb(r, g, b);
           int x = 0;// rectLegendAfterZoom.Left;
           int y = 0;// rectLegendAfterZoom.Top;
           //triangle of left-top
           Point[] pts = new Point[]{
                new Point(x, y),
                new Point(x + nW, y),
                new Point(x, y + nH) 

            };

           // HatchBrush br = new HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Red, Color.Yellow);
           SolidBrush br = new SolidBrush(bg);//  this.ForeColor);

           gc.FillPolygon(br, pts, FillMode.Winding);
           gc.DrawPolygon(new Pen(Color.Black), pts);


       }
        protected virtual void DrawBackground(Graphics g, Rectangle rectVirtualWithOffsetAndZoom, double zoom) 
        /* ============================================================
	        Function :		DrawBackground
	        Description :	Draws the virtual background
	        Access :		Protected

	        Return :		void
	        Parameters :	CDC* dc			-	"CDC" to draw to.
					        Rectangle rect		-	Total rect to draw to.
					        double (zoom)	-	Current zoom level. 
										        Coordinates can be 
										        multiplied with this 
										        value to get scaled.
        					
	        Usage :			Virtual. Can be overridden in a derived 
					        class to draw the virtual paper area.

           ============================================================*/
        {
           // EraseBackground(g, rect);
             SolidBrush br = new SolidBrush( this.BackColor);
            //draw border
             SolidBrush brBorder = new SolidBrush(this.BorderColor);
             g.FillRectangle(brBorder, rectVirtualWithOffsetAndZoom);

            
             Rectangle rect = GetEntitiesContainerRectangle(rectVirtualWithOffsetAndZoom, zoom);
            //end border
            //draw container
             g.FillRectangle(br,rect);// rectVirtualWithOffsetAndZoom);

             DrawImage(g, rectVirtualWithOffsetAndZoom, zoom);

             draw_top_left_triangle(g, rect, zoom);
            // Pen pen = new Pen(Color.Red, 1);
            //g.DrawRectangle(pen, rect);
	       // dc.FillSolidRect( &rect, m_bkgndCol );

        }

        protected void DrawImage(Graphics g, Rectangle rectVirtualEntityWithOffsetAndZoom, double dblZoom)
        {
            if (this.Image == null) return;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            ContentAlignment align = ContentAlignment.MiddleCenter;// this.ImageAlign;
            int imgWidth = this.Image.Width;
            int imgHeight = this.Image.Height;

            Rectangle rtBounds = this.Bounds;
            if (imgHeight > rtBounds.Height ||
                imgWidth > rtBounds.Width)
                g.DrawImage(this.Image, rectVirtualEntityWithOffsetAndZoom);
            else
            {//use align
                //zoom img
                imgWidth = (int)(decimal.Round(((decimal)imgWidth * (decimal)dblZoom)));
                imgHeight = (int)(decimal.Round(((decimal)imgHeight * (decimal)dblZoom)));
                Rectangle rc =DiagramKey.GetImageAlignmentRect(rectVirtualEntityWithOffsetAndZoom, imgWidth, imgHeight, align);
                g.DrawImage(this.Image, rc);
            }
        }

       

        protected virtual void DrawGrid(Graphics g, Rectangle rect, double zoom, bool bPrepartion) 
        /* ============================================================
	        Function :		DrawGrid
	        Description :	Draws the grid
	        Access :		Protected

	        Return :		void
	        Parameters :	CDC* dc		-	"CDC" to draw to.
					        Rectangle rect	-	Total rect to draw to.
					        double zoom	-	Current zoom level. 
									        Coordinates can be 
									        multiplied with this value 
									        to get scaled.
        					
	        Usage :			Virtual. Can be overridden in a derived 
					        class to draw the grid. Will not be called 
					        if the grid is not visible.

           ============================================================*/
        {

            Color gridcol = this.GridLinesColor;// GetGridColor();
            DashStyle gridstyle = this.GridStyle;// GetGridPenStyle();

	        Pen pen = new Pen(gridcol,1) ;
            pen.DashStyle = gridstyle;
	        //pen.CreatePen( gridstyle, 1, gridcol );

	        //dc.SelectObject( &pen );

	        // To avoid accumulating rounding errors, we don't
	        // precalculate the grid size for the given zoom...

	        int width = rect.Width;
	        int height = rect.Height;

            int stepx = this.VirtualSize.Width / this.GridSize.Width;
            int stepy = this.VirtualSize.Height / this.GridSize.Height;

	        // ...instead we calculate the position of each line.
	        for( int x = 0 ; x <= stepx ; x++ )
	        {
                g.DrawLine(pen, (float)decimal.Round((decimal)((decimal)this.GridSize.Width * (decimal)x) * (decimal)zoom), 0,
                           (float)decimal.Round((decimal)((decimal)this.GridSize.Width * (decimal)x) * (decimal)zoom), height);
		        //dc.MoveTo( round( < double >( GetGridSize().Width * x ) * zoom ), 0 );
		        //dc.LineTo( round( < double >( GetGridSize().Width * x ) * zoom ), height );
	        }

	        for( int y = 0; y <= stepy ; y++ )
	        {
                g.DrawLine(pen, 0, (float)decimal.Round((decimal)(this.GridSize.Height * (decimal)y) * (decimal)zoom),
                                width, (float)decimal.Round((decimal)(this.GridSize.Height * (decimal)y) * (decimal)zoom));

		        //dc.MoveTo( 0, round( < double >( GetGridSize().Height * y ) * zoom ) );
		        //dc.LineTo( width, round( < double >( GetGridSize().Height * y ) * zoom ) );
	        }

	        //dc.SelectStockObject( BLACK_PEN );

        }
        private Rectangle GetEntitiesContainerRectangle(Rectangle rectVirtualWithOffsetAndZoom, double zoom)
        {
            Padding padding = this.VirtualScreenMargin;

            decimal l = decimal.Round((decimal)padding.Left * (decimal)zoom);
            decimal t = decimal.Round((decimal)padding.Top * (decimal)zoom);
            decimal r = decimal.Round((decimal)padding.Right * (decimal)zoom);
            decimal b = decimal.Round((decimal)padding.Bottom * (decimal)zoom);

            Rectangle rect = rectVirtualWithOffsetAndZoom;


            Point leftTop = new Point(rect.Left + (int)l, rect.Top + (int)t);
            Point leftBottom = new Point(rect.Left + (int)l, rect.Bottom - (int)b - 1);
            Point rightTop = new Point(rect.Right - (int)r - 1, rect.Top + (int)t);
           // Point rightBottom = new Point(rect.Right - (int)r - 1, rect.Bottom - (int)b - 1);

            return new Rectangle(leftTop, new Size(rightTop.X - leftTop.X, leftBottom.Y - leftTop.Y));


        }
        protected void DrawMargins(Graphics g, Rectangle rectVirtualWithOffsetAndZoom, double zoom) 
        /* ============================================================
	        Function :		DrawMargins
	        Description :	Draws the margins.
	        Access :		Protected

	        Return :		void
	        Parameters :	CDC* dc		-	"CDC" to draw to.
					        Rectangle rect	-	Total rect to draw to.
					        double zoom	-	Current zoom level. 
									        Coordinates can be 
									        multiplied with this value 
									        to get scaled.
        					
	        Usage :			Virtual. Can be overridden in a derived 
					        class to draw the margins. Will not be 
					        called if margins are not visible.

           ============================================================*/
        {

	        Pen pen = new Pen( this.MarginColor, 1);
            pen.DashStyle = DashStyle.Solid;


            //Rectangle rt = GetEntitiesContainerRectangle(rectVirtualWithOffsetAndZoom, zoom);
            //g.DrawRectangle(pen, rt);
            
            //use the grid coordinate to draw border

            Rectangle r = new Rectangle();
            r.X = DEditor.m_lstX[0];
            r.Y = DEditor.m_lstY[0];
            //the coordinate is the last "cm" position, it is not .5 position.
            // The last one in m_lstX and m_lstY is the half cm. So, "-2".
            r.Width = DEditor.m_lstX[DEditor.m_lstX.Count - 2] - r.X;
            r.Height = DEditor.m_lstY[DEditor.m_lstY.Count - 2] - r.Y;

            //scroll bar offset
            Rectangle rect = rectVirtualWithOffsetAndZoom;
            r.X += rect.X;
            r.Y += rect.Y;
            g.DrawRectangle(pen, r);

            /*
	        //pen.CreatePen( PS_SOLID, 0, m_marginColor );
	        //dc.SelectObject( &pen );
            Padding padding = this.VirtualScreenMargin;

            decimal l =decimal.Round( (decimal)padding.Left * (decimal)zoom);
            decimal t =decimal.Round( (decimal)padding.Top * (decimal)zoom);
            decimal r =decimal.Round( (decimal)padding.Right * (decimal)zoom);
            decimal b =decimal.Round( (decimal)padding.Bottom * (decimal)zoom);

            Rectangle rect = rectVirtualWithOffsetAndZoom;

            
            Point leftTop = new Point(rect.Left + (int)l, rect.Top + (int)t);
            Point leftBottom = new Point(rect.Left + (int)l, rect.Bottom - (int)b - 1);
            Point rightTop = new Point(rect.Right - (int)r - 1, rect.Top + (int)t);
            Point rightBottom = new Point(rect.Right - (int)r - 1, rect.Bottom - (int)b - 1);

	        if(padding.Left >0)
                g.DrawLine(pen,leftTop,leftBottom );
		       
	        if( padding.Right >0 )
	        
                g.DrawLine(pen,rightTop,rightBottom );
		    
	        if( padding.Top >0)
                g.DrawLine(pen,leftTop,rightTop );
		    
	        if( padding.Bottom >0)
                 g.DrawLine(pen,leftBottom,rightBottom );
		    */

	        


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectTotalVirtualWithOffsetAndZoom"> The virtual rectangle value, it has been zoomed and offset by scrollbar</param>
        /// <param name="zoom"></param>
        /// <param name="bToImage"> The image and GUI drawing has different coordinate., TRUE: it is outputing to 
        /// image</param>
        protected void DrawObjects(Graphics g, Rectangle rectTotalVirtualWithOffsetAndZoom, double zoom, bool bToImage) 
        /* ============================================================
	        Function :		DrawObjects
	        Description :	Draws the object.
	        Access :		Protected

	        Return :		void
	        Parameters :	CDC* dc		-	"CDC" to draw to.
					        double zoom	-	Current zoom level. 
									        Coordinates can be 
									        multiplied with this value 
									        to get scaled.
        					
	        Usage :			Virtual. Can be overridden in a derived 
					        class to draw the data objects.

           ============================================================*/
        {

	        if( m_objs != null )
	        {
		        int count = 0;
		        DiagramEntity obj;
		        while( ( obj = m_objs.GetAt( count++ ) ) != null )
                    obj.DrawObject(g, rectTotalVirtualWithOffsetAndZoom, zoom, bToImage);
	        }

        }

       protected  void DrawSelectionMarkers( Graphics g ) 
        /* ============================================================
	        Function :		DrawSelectionMarkers
	        Description :	Draws the selection markers.
	        Access :		Protected

	        Return :		void
	        Parameters :	CDC* dc	-	The "CDC" to draw to.
        					
	        Usage :			Virtual. Can be overridden in a derived 
					        class to draw the selection markers in 
					        another way than the default black 
					        rectangles. The selection rects are 
					        displayed if the editor has background 
					        resizing enabled and the user clicks in 
					        the resize area around the virtual page 
					        border. Selection markers are displayed 
					        to allow resizing of the virtual page 
					        with the mouse. 

           ============================================================*/
        {

	        // Draw selection markers
	        Rectangle rectSelect;

	        //dc.SelectStockObject( BLACK_BRUSH );
            
            SolidBrush br = new SolidBrush(Color.Black);

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_TOPLEFT );
            g.FillRectangle(br, rectSelect);
	        //dc.Rectangle( rectSelect );

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_TOPMIDDLE );
	        //dc.Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_TOPRIGHT );
	        //dc.Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_BOTTOMLEFT );
	        //dc.Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_BOTTOMMIDDLE );
	        //dc.Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_BOTTOMRIGHT );
	        //dc.Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_RIGHTMIDDLE );
	        //dc.Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_LEFTMIDDLE );
	        //dc.Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

        }

       public Rectangle GetSelectionMarkerRect( DiagramEntity.DEHT marker ) 
        /* ============================================================
	        Function :		GetSelectionMarkerRect
	        Description :	Returns the selection rect for marker
	        Access :		Public

	        Return :		Rectangle		-	The rect of the selection 
									        marker.
	        Parameters :	UINT marker	-	The marker to get the rect 
									        for ("DEHT_"-constants 
									        defined in DiagramEntity.h)
        					
	        Usage :			Virtual. Can be overridden in a derived 
					        class to change the selection rects for 
					        the virtual page. The selection rects are 
					        displayed if the editor has background 
					        resizing enabled and the user clicks in 
					        the resize area around the virtual page 
					        border. Selection markers are displayed 
					        to allow resizing of the virtual page 
					        with the mouse.
					        "marker" can be one of the following:
						        "DEHT_NONE" No hit-point
						        "DEHT_BODY" Inside object body
						        "DEHT_TOPLEFT" Top-left corner
						        "DEHT_TOPMIDDLE" Middle top-side
						        "DEHT_TOPRIGHT" Top-right corner
						        "DEHT_BOTTOMLEFT" Bottom-left corner
						        "DEHT_BOTTOMMIDDLE" Middle bottom-side
						        "DEHT_BOTTOMRIGHT" Bottom-right corner
						        "DEHT_LEFTMIDDLE" Middle left-side
						        "DEHT_RIGHTMIDDLE" Middle right-side
           ============================================================*/
        {

            Rectangle rect = new Rectangle(0, 0, (int)decimal.Round((decimal)(this.VirtualSize.Width) * (decimal)this.Zoom),
                                (int)decimal.Round((decimal)(this.VirtualSize.Height) * (decimal)this.Zoom));

	        Rectangle rectMarker = new Rectangle();
	        int horz = this.MarkerSize.Width / 2;
            int vert = this.MarkerSize.Height / 2;

	        switch( marker )
	        {
		        case DiagramEntity.DEHT.DEHT_TOPLEFT:
			        rectMarker = new Rectangle( rect.Left - horz, 
								        rect.Top - vert,
                                        this.MarkerSize.Width,//rect.Left + horz, 
                                        this.MarkerSize.Height);//rect.Top + vert);
		        break;

		        case DiagramEntity.DEHT.DEHT_TOPMIDDLE:
			        rectMarker = new Rectangle( rect.Left + ( rect.Width / 2 ) - horz, 
								        rect.Top - vert,
                                        this.MarkerSize.Width,//rect.Left + ( rect.Width / 2 ) + horz, 
                                        this.MarkerSize.Height);//rect.Top + vert );
		        break;

		        case DiagramEntity.DEHT.DEHT_TOPRIGHT:
			        rectMarker = new Rectangle( rect.Right - horz, 
								        rect.Top - vert,
                                        this.MarkerSize.Width,//rect.Right + horz, 
                                        this.MarkerSize.Height);//rect.Top + vert );
		        break;

		        case DiagramEntity.DEHT.DEHT_BOTTOMLEFT:
			        rectMarker = new Rectangle( rect.Left - horz, 
								        rect.Bottom - vert,
                                        this.MarkerSize.Width,//rect.Left + horz, 
                                        this.MarkerSize.Height);//rect.Bottom + vert );
		        break;

		        case DiagramEntity.DEHT.DEHT_BOTTOMMIDDLE:
			        rectMarker = new Rectangle( rect.Left + ( rect.Width / 2 ) - horz, 
								        rect.Bottom - vert,
                                        this.MarkerSize.Width,//rect.Left + ( rect.Width / 2 ) + horz, 
                                        this.MarkerSize.Height);//rect.Bottom + vert );
		        break;

		        case DiagramEntity.DEHT.DEHT_BOTTOMRIGHT:
			        rectMarker = new Rectangle( rect.Right - horz, 
								        rect.Bottom - vert,
                                        this.MarkerSize.Width,//rect.Right + horz, 
                                        this.MarkerSize.Height);//rect.Bottom + vert );
		        break;

		        case DiagramEntity.DEHT.DEHT_LEFTMIDDLE:
			        rectMarker = new Rectangle( rect.Left - horz, 
								        rect.Top + ( rect.Height / 2 ) - vert,
                                        this.MarkerSize.Width,//rect.Left + horz, 
                                        this.MarkerSize.Height);//rect.Top + ( rect.Height / 2 ) + vert );
		        break;

		        case DiagramEntity.DEHT.DEHT_RIGHTMIDDLE:
			        rectMarker = new Rectangle( rect.Right - horz, 
								        rect.Top + ( rect.Height / 2 ) - vert,
                                        this.MarkerSize.Width,//rect.Right + horz, 
                                        this.MarkerSize.Height);//rect.Top + ( rect.Height / 2 ) + vert );
		        break;
	        }

	        return rectMarker;

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor property accessors
        //Touch area is 176mm x 64mm.  Legend sheet size should be 186mm x
        //74mm (larger than touch area by 5mm on each side).
        //186mm = 703 pixel
        //74mm = 280 pixel
        //176mm = 665
        //64mm = 242
        //5mm = 19
        [Browsable(false)]
        public Size VirtualSize
        {
            get 
            {
                Rectangle rect = new Rectangle(0, 0, 0, 0);
                if (this.Handle != null)
                {
                    rect = this.ClientRectangle;
                    //GetClientRect( &rect );
                }
                Size size = new Size(0, 0);

                if (m_objs != null)
                    size = m_objs.VirtualSize;

                if (size.Width == 0)
                    size.Width = rect.Right;
                if (size.Height == 0)
                    size.Height = rect.Bottom;

                return size;
            }
            set
            {
                if (m_objs == null) return;

                SetInternalVirtualSize(value);
                m_objs.Modified = (true);
            }
        }

      

        private void SetInternalVirtualSize(  Size size )
        /* ============================================================
	        Function :		SetInternalVirtualSize
	        Description :	Changes the virtual page size without 
					        setting the data as modified.
	        Access :		Private

	        Return :		void
	        Parameters :	const Size& size	-	New virtual size.
        					
	        Usage :			Internal function. 

           ============================================================*/
        {
            if (m_objs != null && size != this.VirtualSize)
	        {

		        m_objs.VirtualSize = ( size );

		        SetupScrollbars();
		        if( this.Handle != null )
			        RedrawWindow();

	        }

        }

      



        private Color _NoneClientBackgourndColor = SystemColors.AppWorkspace;// .ButtonShadow;
        [Browsable(false)]
        public Color NoneClientBackgourndColor
        {
            get { return _NoneClientBackgourndColor; }
            set { 
                _NoneClientBackgourndColor = value;
                RedrawWindow();
            }
        }

    

        /// <summary>
        /// range 2 -- 255
        /// 20140408, change range to 1--100
        /// </summary>
        protected int _InterCharDelay = KB9Const.DEFAULT_InterCharDelay;
        [Description("Inter-Character delay time. Value Range: [1, 100]")]
        [Category("Keyboard")]
        [DisplayName("Inter-character delay")]
        //[TypeConverter(typeof(NumConverter))]
        public int InterCharDelay
        {
            get { return _InterCharDelay; }
            set
            {
                int val = value;
                if (val >= KB9Const.Min_InterCharDelay && val <= KB9Const.Max_InterCharDelay)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    // this.m_objs.Snapshot();
                    _InterCharDelay = value;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
                else if (val > KB9Const.Max_InterCharDelay)
                {//20140429, set to max, min
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    //  this.m_objs.Snapshot();
                    _InterCharDelay = KB9Const.Max_InterCharDelay;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
                else if (val < KB9Const.Min_InterCharDelay)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    // this.m_objs.Snapshot();
                    _InterCharDelay = KB9Const.Min_InterCharDelay;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
            }
        }

        protected int _TouchDelay = KB9Const.DEFAULT_TouchDelay;
        [Description("Touch Delay Time. Value Range: [1, 10]")] //20140408, change from 1--100, to 1--10
        [DisplayName("Touch delay")]
        [Category("Keyboard")]
        //[TypeConverter(typeof(NumConverter))]
        public int TouchDelay
        {
            get { return _TouchDelay; }
            set
            {
                int val = value;
                if (val >= KB9Const.Min_TouchDelay && val <= KB9Const.Max_TouchDelay)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    // this.m_objs.Snapshot();
                    _TouchDelay = value;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
                //20140429, set to max, min
                else if (val < KB9Const.Min_TouchDelay)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    //this.m_objs.Snapshot();
                    _TouchDelay = KB9Const.Min_TouchDelay;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
                else if (val > KB9Const.Max_TouchDelay)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    //this.m_objs.Snapshot();
                    _TouchDelay = KB9Const.Max_TouchDelay;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
            }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        protected int _Sensitivity = KB9Const.DEFAULT_Sensitivity;
        [Description("Sensitivity. Value Range: [1, 10]")]//20140408, change from 1--100, to 1--10
        [DisplayName("Touch sensitivity")]
        [Category("Keyboard")]
        //[TypeConverter(typeof(NumConverter))]
        public int Sensitivity
        {
            get { return _Sensitivity; }
            set
            {
                int val = value;
                if (val >= KB9Const.Min_Sensitivity && val <= KB9Const.Max_Sensitivity)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    // this.m_objs.Snapshot();
                    _Sensitivity = value;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
                //20140429, set to max, min
                else if (val < KB9Const.Min_Sensitivity)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    //this.m_objs.Snapshot();
                    _Sensitivity = KB9Const.Min_Sensitivity;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
                else if (val > KB9Const.Max_Sensitivity)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    //this.m_objs.Snapshot();
                    _Sensitivity = KB9Const.Max_Sensitivity;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                }
            }
        }
        protected Image _Image = null;
        [Category("Legend Sheet")]
        [DefaultValue((string)null)] //allow the "delete"key to set image back to (none)
        [DisplayName("Background image")]
        [ReadOnly(false)]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    _Image = value;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                    RedrawWindow();
                    //Refresh();
                }
            }
        }
        
       


        [Category("Legend Sheet")]
        [DisplayName("Background color")]
        [Editor(typeof(ColorEditorUI), typeof(UITypeEditor))]
        [TypeConverter(typeof(ColorConverterUI))]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (base.BackColor != value)
                {
                    if (m_objs != null)
                        m_objs.Snapshot(this);
                    base.BackColor = value;
                    this.Modified = true;
                    if (this.OnRefreshPropertiesGrid != null)
                        this.OnRefreshPropertiesGrid(this, null);
                    
                    //Refresh();
                }
            }
        }



        private Color _BorderColor = Color.White;
        [Category("Legend Sheet")]
        [DisplayName("Border color")]
        [Browsable(false)]
        //[Editor(typeof(ColorEditorUI), typeof(UITypeEditor))]
        //[TypeConverter(typeof(ColorConverterUI))]
        public Color BorderColor
        {
            get { return this.BackColor; }// _BorderColor; }
            set
            {
                //_BorderColor = value;
                //this.Modified = true;
                //this.Refresh();
            }
        }

        private bool _ShowGridLines = true;
        [Description("Show the background grid dot lines or not")]
        [DisplayName("Show grid")]
        [Category("Legend Sheet")]
        public bool ShowGridLines
        {
            get { return _ShowGridLines; }
            set 
            {
                if (_ShowGridLines != value)
                {


                    _ShowGridLines = value;

                    RedrawWindow();
                }
            }
        }


      

      

        private Color _GridLinesColor = Color.FromArgb(192, 192, 192);
        [Browsable(false)]
        public Color GridLinesColor
        {
            get { return _GridLinesColor; }
            set {
                _GridLinesColor = value;
                RedrawWindow();
            }
        }
      

      
        private Size _GridSize = new Size(KB9Const.UNIT_PIXELS, KB9Const.UNIT_PIXELS);
        [Browsable(false)]
        public Size GridSize
        {
            get { return _GridSize; }
            set {
                _GridSize = value;
                RedrawWindow();
            }
        }
     

        private DashStyle _GridStyle = DashStyle.Dot;
        [Browsable(false)]
        public DashStyle GridStyle
        {
            get { return _GridStyle; }
            set { 
                    _GridStyle = value;
                    RedrawWindow();

                }
        }
      

        private bool _SnapToGrid = true;
        [Description("Snap component to nearest grid line.")]
        [Browsable(false)]
        public bool SnapToGrid
        {
            get { return _SnapToGrid; }
            set
            {
                _SnapToGrid = value;
            }
        }
      
        private bool _BackgroundSizable = false;
        [Browsable(false)]
        public bool BackgroundSizable
        {
            get { return _BackgroundSizable; }
            set { _BackgroundSizable = value;}
        }

        private bool _BackgroundResizeSelected = false; //true if we are resizing the backgound
        [Browsable(false)]
        protected bool BackgroundResizeSelected
        {
            get { return _BackgroundResizeSelected; }
            set 
            { 
                _BackgroundResizeSelected = value;
                //if (_BackgroundResizeSelected )
                //{//fire event
                //    if (OnShowDiagramEntityProperties != null )
                //    {
                //        OnShowDiagramEntityProperties(this, null);
                //    }

                //}
            }
        }

        private int _BackgroundResizeZone = 10;
        [Browsable(false)]
        public int BackgroundResizeZone
        {
            get { return _BackgroundResizeZone; }
            set {_BackgroundResizeZone = value;}
        }
      


       

       

        private Padding _VirtualScreenMargin = new Padding(KB9Const.PIXELS_MARGIN, KB9Const.PIXELS_MARGIN, KB9Const.PIXELS_MARGIN, KB9Const.PIXELS_MARGIN);
        [Browsable(false)]
        public Padding VirtualScreenMargin
        {
            get { return _VirtualScreenMargin; }
            set { _VirtualScreenMargin = value;}
        }

      

     
        private Color _MarginColor = Color.FromArgb(128, 128, 255);
        [Browsable(false)]
        public Color MarginColor
        {
            get { return _MarginColor; }
            set {
                _MarginColor = value;
                RedrawWindow();

            }
        }
      
       

        private bool _ShowVirtualScreenMargin = true;
        [Browsable(false)]
        public bool ShowVirtualScreenMargin
        {
            get { return _ShowVirtualScreenMargin; }
            set {
                _ShowVirtualScreenMargin = value;
                RedrawWindow();
            }
        }

       

      
        private RESTRAIN _Restraints = RESTRAIN.RESTRAINT_MARGIN;// RESTRAINT_VIRTUAL;// .RESTRAINT_NONE;
        [Browsable(false)]
        public RESTRAIN Restraints
        {
            get { return _Restraints; }
            set {
                _Restraints = value;
            }
        }
       

       

       

        private bool _IsDrawingObj = false;
        [Browsable(false)]
        public bool IsDrawingObj
        {
            get { return _IsDrawingObj; }
            //set { _IsDrawing = value;}
        }
        /* ============================================================
           Function :		GetMultidraw
           Description :	Returns the multi draw state
           Access :		Public

           Return :		bool	-	"true" if multi draw is set
           Parameters :	none

           Usage :			Multi draw means that the user can continue 
                           to add controls after one is placed.

          ============================================================*/
        private bool _MultipleDrawOneObject = false;
        [Browsable(false)]
        public bool MultipleDrawOneObject
        {
            get { return _MultipleDrawOneObject; }
            set 
            {
                _MultipleDrawOneObject = value;
                //m_drawing = false;
                _IsDrawingObj = false;
                m_interactMode = MouseMode.MODE_NONE;
                //delete m_drawObj;
                //m_drawObj = null;
                this.DrawingObject = null;
            }
        }

       
       
        /* ============================================================
            Function :		SetZoom
            Description :	Set the current zoom level.
            Access :		Public

            Return :		void
            Parameters :	double zoom	-	New zoom level. 1.0 means 
                            no zoom.

            Usage :			If the virtual screen should be zoomed in 
                            to double size, the zoom value should be 
                            2.0, for example.
         =============================================================*/
       private double _Zoom =0.3;// 1.0;
        [Browsable(false)]
       public double Zoom
       {
           get { return _Zoom; }
           set {
               _Zoom = Math.Max(this.ZoomMin, value);

               if (_Zoom != this.ZoomMin)
                   _Zoom = Math.Min( this.ZoomMax, value);

               SetupScrollbars();
                
               RedrawWindow();

           }
       }
        /// <summary>
        /// tempary change the zoom value for export image
        /// </summary>
        /// <param name="dblZoom"></param>
       public void SetPropertiesForExportImage(double dblZoom, bool bShowGrid, bool bShowMargin)
       {
           _Zoom = dblZoom;
           _ShowGridLines = bShowGrid;
           _ShowVirtualScreenMargin = bShowMargin;
       }


      
        private double _ZoomStep = 0.05;
        [Browsable(false)]
        public double ZoomStep
        {
            get { return _ZoomStep; }
            set {
                _ZoomStep = value;
            }
        }
       

       
       
        private double _ZoomMin = 0.1;
        [Browsable(false)]
        public double ZoomMin
        {
            get 
            { //current resolution 60%
                Screen scr = Screen.FromControl(this);
                //return (double)(scr.WorkingArea.Width) * (double)0.6 / (double)(DiagramEntityContainer.VIRTUALSIZE_CX);
                return (double)(scr.WorkingArea.Width) * (double)0.6 / (double)(KB9Const.PIXELS_CX);
                //return _ZoomMin; 
            }
            set {
                _ZoomMin = Math.Max(0, value); 
            }
        }
       
        //     Usage :			The zoom level will never go below or above 
       //                     the min and max zoom levels
        private double _ZoomMax = 1.0;
        [Browsable(false)]
        public double ZoomMax 
        {
            get { return _ZoomMax; }
            set
            {
                _ZoomMax = Math.Max(0, value);
            }
        }
      
       
        [Browsable(false)]
        public bool Modified
        {
            get 
            {
                bool res = false;
                if (m_objs != null)
                    res = m_objs.Modified;

                return res;
            }
            set
            {
                if (m_objs != null)
                    m_objs.Modified = (value);

            }
        }

       

      

        private Size _MarkerSize = new Size(KB9Const.MARKER_SIZE, KB9Const.MARKER_SIZE);
        [Browsable(false)]
        public Size MarkerSize
        {
            get { return _MarkerSize; }
            set 
            {
                _MarkerSize = value;
            }
        }
       

       

       public  KeyInterface GetKeyboardInterface() 
        /* ============================================================
	        Function :		GetKeyboardInterface
	        Description :	Returns the flags for the keyboard 
					        interface
	        Access :		Public

	        Return :		UINT	-	The current flags
	        Parameters :	none

	        Usage :			The keyboard interface decides what keys 
					        should be reacted on. The following flags 
					        can be used:
						        "KEY_ARROW" Will handle arrow keys. If shift is pressed, the selected objects will be resized, moved otherwise.
						        "KEY_PGUPDOWN" Will handle Pg up & pg down. If Ctrl is pressed, the selected object will move to the top or the bottom of the z-order, one step up or down otherwise.
						        "KEY_DELETE" Will handle delete key. The selected object(s) will be deleted, put into the container clipboard if Ctrl is pressed.
						        "KEY_ESCAPE" Will handle escape key. If multi-draw mode, no object type is selected for drawing.
						        "KEY_INSERT" Will handle insert key. The selected object will be copied if Ctrl is pressed, duplicated otherwise.
						        "KEY_PLUSMINUS" Will handle the plus- and minus key. Will zoom in or out.
						        "KEY_CTRL" Will handle Ctrl+A,Z,X,C,V and Enter keys. A = Select all Z = Undo X = Cut C = Copy V = Paste Enter = Show property dialog for the selected object.
					        KEY_ALL sets all flags. KEY_NONE no flags.

           ============================================================*/
        {

	        return m_keyInterface;

        }

       public  void SetKeyboardInterface( KeyInterface keyInterface )
        /* ============================================================
	        Function :		SetKeyboardInterface
	        Description :	Sets the keyboard interface flag.
	        Access :		Public

	        Return :		void
	        Parameters :	int keyInterface	-	The new flags
        					
	        Usage :			Call to set the keys the editor will handle.
					        See "GetKeyboardInterface".

           ============================================================*/
        {

	        m_keyInterface = keyInterface;

        }

      

        
        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor mouse handling
        
        protected override void  OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
            if (m_bCancelMouseDownEvent)
            {
                m_bCancelMouseDownEvent = false;
                return;
            }
 	      
            if (e.Button == MouseButtons.Left)
                OnLButtonDown(e);
            else if (e.Button == MouseButtons.Right)
            {
                OnRButtonDown(e);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                OnMButtonDown(e);
            }
        }
        /// <summary>
        /// get all entities count
        /// </summary>
        /// <returns></returns>
        private int GetEntitiesCount()
        {
            if (m_objs == null)
                return 0;
            int ncount = 0;
            for (int i=0; i< m_objs.GetSize(); i++)
            {
                DiagramEntity entity = m_objs.GetAt(i);
                if (entity.TypeName == DiagramKeyMatrix.KEY_MATRIX)
                {
                    ncount += ((DiagramKeyMatrix)entity).Entities.Count;
                }
                else
                {
                    ncount++;
                }
            }

            return ncount;
        }

        private bool _NewButtonResize = false;//true: it is a new button, and resizing.
        private Point _LButtonDownPoint = new Point(-1,-1);
        private DateTime _LButtonDownDateTime = new DateTime();
        protected void OnLButtonDown( MouseEventArgs e ) 
        {

            m_bMouseMovingDistanceCheckPassed = false;
            _NewButtonResize = false;
	        //SetPanning( false );
            //this.MiddleButtonPanning = false;
            Point point = e.Location;
           _LButtonDownPoint = point;
           _LButtonDownDateTime = DateTime.Now;
            
	        // Declarations
	        int count = 0;
	        DiagramEntity obj = null;
            Point virtpoint = point;// new Point(point);
            ClientToVirtual(ref virtpoint);
            CheckAndSetMultipleSelectedMain(point);

	        // Setting up
            this.Focus();
            this.Capture = true;
	        //SetFocus();
	        //SetCapture();

	        // Resetting modes
	        if( m_interactMode !=MouseMode.MODE_DRAWING )
	        {
		        m_interactMode = MouseMode.MODE_NONE;
		        m_subMode = DiagramEntity.DEHT.DEHT_NONE;
	        }

            this.BackgroundResizeSelected = false;
	        //m_bgResizeSelected = false;
	        m_multiSelObj = null;

	        // If we click on an already selected object, 
	        // and have more than one object selected,
	        // we want to move all selected objects
	        if( m_objs!= null && GetSelectCount() > 1 && !( Control.ModifierKeys == Keys.Control ) )
	        {

		        while( ( obj = m_objs.GetAt( count++ ) ) != null )
		        {
			        if( obj.GetHitCode( virtpoint ) == DiagramEntity.DEHT.DEHT_BODY && obj.Selected )
			        {
				        m_interactMode = MouseMode.MODE_MOVING;
                        Rectangle rect = obj.Bounds;// GetRect();

				        // We might need to use a denormalized
				        // rect, as lines will not be normalized.
				        bool useright = false;
				        bool usebottom = false;
				        if( rect.Left > rect.Right )
					        useright = true;
				        if( rect.Top > rect.Bottom )
					        usebottom = true;

                        VirtualToClient(ref  rect);

				        int startx = rect.Left;
				        if( useright )
					        startx = rect.Right;
				        int starty = rect.Top;
				        if( usebottom )
					        starty = rect.Bottom;

				        // Anchor object
				        m_multiSelObj = obj;

				        // Offset from top-left corner
				        // in the anchor object.
                        Point pt = new Point( startx, starty );
				        m_deltaPoint = new Point(point.X - pt.X, point.Y - pt.Y);// = point - (new Point( startx, starty ));

			        }
		        }

		        if( m_interactMode == MouseMode.MODE_MOVING )
		        {
			        // We have clicked in one of many selected objects.
			        // Set cursor and snapshot for Undo.
                    //this.Cursor = m_multiSelObj.GetCursor( DiagramEntity.DEHT.DEHT_BODY ) ;
                    m_objs.PreSnapshot();
                    FireShowPropertiesEvent(m_multiSelObj);
		        }

	        }

	        if( m_objs != null && m_interactMode == MouseMode.MODE_DRAWING )
	        {
                //check max count
                //if (GetEntitiesCount() > MAX_ENTITIES_COUN)
                //{
                //    this.DrawingObject = null;
                //    m_interactMode = MouseMode.MODE_NONE;
                //    return;
                //}

		        // The user wants to draw an object.
		        // We add it to the data container and sets the appropriate mode

		        if( OutsideRestraints( virtpoint ) )
		        {
                    
			        m_interactMode = MouseMode.MODE_NONE;
			        return;
		        }

		        m_objs.Snapshot();

		        UnselectAll();

                DiagramEntity newobj = this.DrawingObject.Clone();// m_drawObj.Clone();

		        // If snap-to-grid is on, we must
		        // update the desired position
		        if( this.SnapToGrid )
		        {

			        virtpoint.X = SnapX( virtpoint.X );
			        virtpoint.X = SnapY( virtpoint.X );

		        }

		        //newobj.SetRect(new  Rectangle( virtpoint.X, virtpoint.Y, 1,1));//virtpoint.X , virtpoint.Y ) );
                newobj.Bounds = (new Rectangle(virtpoint.X, virtpoint.Y, 1, 1));//virtpoint.X , virtpoint.Y ) );
		        //newobj.Selected = ( true );

		        // Add the object to the container
		        AddObject( newobj );
                newobj.Selected = true; //david
		        // Set modes
		        m_interactMode = MouseMode.MODE_RESIZING;
		        m_subMode =DiagramEntity.DEHT.DEHT_BOTTOMRIGHT;
                _NewButtonResize = true;
                FireShowPropertiesEvent(newobj);

                //if (!check_max_entities())
                //{
                //    m_interactMode = MouseMode.MODE_NONE;
                //    return;
                //}

	        }

	        if( m_objs != null && m_interactMode == MouseMode.MODE_NONE )
	        {

		        // Clearing states
		        // If Ctrl is not held down, we
		        // clear all selections
		        if( !( Control.ModifierKeys == Keys.Control ) )
			        UnselectAll();

		        count = GetObjectCount();
		        bool goon = true;

		        // We check if we click on any object. If that is 
		        // the case, we return on what part of the object 
		        // we clicked.
		        while( goon && ( (obj = m_objs.GetAt( --count ) )!= null) )
		        {
                    Rectangle rect = obj.Bounds;// GetRect();
                    VirtualToClient(ref rect);

			        DiagramEntity.DEHT hitCode = obj.GetHitCode( point, rect );
			        if( hitCode !=DiagramEntity.DEHT.DEHT_NONE )
			        {
				        goon = false;
				        if( !( Control.ModifierKeys == Keys.Control ) )
				        {
					        // We unselect all again, as we might
					        // have overlapping objects
					        UnselectAll();
					        obj.Selected = ( true );
				        }
				        else
				        {
                            //anchor first ctrl+click object
                            if (this.GetSelectCount() == 1)
                            {
                                if (this.GetSelectedObject() != obj)
                                    this.GetSelectedObject().SelectedMain = true;
                            }
					        // We toggle the selection of the 
					        // object if Ctrl is pressed
                            obj.Selected = (!obj.Selected);

				        }

				        // We set the appropriate mode, either
				        // moving or resizing depending on where
				        // the click hit.
				        if( hitCode == DiagramEntity.DEHT.DEHT_BODY && !( Control.ModifierKeys == Keys.Control ) )
				        {
					       
                           // this.Cursor = obj.GetCursor(DiagramEntity.DEHT.DEHT_BODY);
					        m_interactMode = MouseMode.MODE_MOVING;
                            //20140214
                            _LockMouseMove = true;
                            Rectangle rt = obj.Bounds;// GetRect();

					        // We might need to use a denormalized
					        // rect, as lines will not be normalized.
					        bool useright = false;
					        bool usebottom = false;
					        if( rt.Left > rt.Right )
						        useright = true;
					        if( rt.Top > rt.Bottom )
						        usebottom = true;

                            VirtualToClient(ref rt);

					        int startx = rt.Left;
					        if( useright )
						        startx = rt.Right;
					        int starty = rt.Top;
					        if( usebottom )
						        starty = rt.Bottom;
					        if( GetSelectCount() > 1 )
						        m_multiSelObj = obj;
                            Point pt = new  Point( startx, starty );
					        m_deltaPoint = new Point( point.X - pt.X, point.Y - pt.Y);
                            FireShowPropertiesEvent(obj);
                            //for matrix key
                            if (GetSelectCount() == 1)
                            {


                                if (obj.TypeName == DiagramKeyMatrix.KEY_MATRIX)
                                {
                                    Point ptVirtual = point;
                                    ClientToVirtual(ref ptVirtual);
                                    DiagramEntity clickedEntity = ((DiagramKeyMatrix)obj).hit_child(ptVirtual);
                                    if (clickedEntity != null)
                                    {
                                        FireShowPropertiesEvent(clickedEntity);
                                    }
                                }
                            }
                            ////////

				        }
				        else if( !( Control.ModifierKeys == Keys.Control  ) )
				        {
					        m_interactMode = MouseMode.MODE_RESIZING;
					        m_subMode = hitCode;
                            FireShowPropertiesEvent(obj);
				        }
				        else
					        m_interactMode = MouseMode.MODE_NONE;
			        }
		        }

		        // Save to undo-stack...
                if (m_interactMode != MouseMode.MODE_NONE) 
                    m_objs.PreSnapshot();
			    

		        // If no objects were selected, we assume either 
		        // rubberbanding or background resize
		        if( !IsAnyObjectSelected() )
		        {
                    //if( this.BackgroundSizable)// m_bgResize )
                    //{
                    //    // If we allow background resizing, we test if
                    //    // the click was inside the resize zone.
                    //    if ((virtpoint.X >= this.VirtualSize.Width - this.BackgroundResizeZone &&
                    //        virtpoint.X <= this.VirtualSize.Width)
                    //        ||
                    //        (virtpoint.Y >= this.VirtualSize.Height - this.BackgroundResizeZone &&
                    //        virtpoint.Y <= this.VirtualSize.Height)
                    //         )
                    //    {
                    //        //m_bgResizeSelected = true;
                    //        this.BackgroundResizeSelected = true;
                    //        DiagramEntity.DEHT hitCode = GetHitCode( virtpoint );
                    //        if( hitCode != DiagramEntity.DEHT.DEHT_NONE && hitCode != DiagramEntity.DEHT.DEHT_BODY )
                    //        {
                    //            // It was - set the appropriate mode
                    //            m_interactMode = MouseMode.MODE_BGRESIZING;
                    //            m_subMode = hitCode;
                    //            m_objs.Snapshot();
                    //        }

                    //        RedrawWindow();
                    //    }
                    //}

			        if( m_interactMode == MouseMode.MODE_NONE )
			        {
				        // If nothing else is set, we assume
				        // rubberbanding.
                        m_selectionRect = new Rectangle(point, new Size(0, 0));

				        m_interactMode = MouseMode.MODE_RUBBERBANDING;
			        }
		        }
	        }

	        // Cleaning up and redrawing as necessary.
	        RemoveUnselectedPropertyDialogs();
	        if( m_interactMode != MouseMode.MODE_NONE )
		        RedrawWindow();

	        

        }
        protected override void  OnMouseMove(MouseEventArgs e)
        {
        //    if (e.Button == MouseButtons.Left)
        //        OnLButtonMove(e);
            if (e.Button == MouseButtons.Right)
            {
                OnRButtonMove(e);
            }
            else
                OnLButtonMove(e);
        }

        private bool m_bMouseMovingDistanceCheckPassed = false; 
        /// <summary>
        /// just call this function in LButton Moveing event
        /// _LButtonDownPoint has been updated while mouse_down.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool Is_Moving_Entity(MouseEventArgs e)
        {
            if (m_bMouseMovingDistanceCheckPassed)
                return true;
            if (Math.Abs(e.Location.X - _LButtonDownPoint.X) > 5 ||
              Math.Abs(e.Location.Y - _LButtonDownPoint.Y) > 5)
            {
                m_bMouseMovingDistanceCheckPassed = true;
                return true;
            }
            return false;

            //int x = Math.Abs(e.Location.X - _LButtonDownPoint.X);
            //int y = Math.Abs(e.Location.Y - _LButtonDownPoint.Y);
            //int d = (int)Math.Sqrt(x * x + y * y);
            //return (d > 20);

        }
        private const long Time_ms = 10000;//1 ms = 10000ticks
        private bool LockMouseTimeout()
        {
            DateTime dt = DateTime.Now;
            long ticks = dt.Ticks - _LButtonDownDateTime.Ticks;
            //1 ticks  = 100ns
            //1ms = 1000000ns
            if (ticks >Time_ms *200) //0.1 s
                return true;
            return false;

        }
        /* ============================================================
	        Function :		OnMouseMove
	        Description :	Handles the "WM_MOUSEMOVE" message. We handle 
					        moving or resizing of objects, or resizing 
					        of the virtual screen.
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nFlags		-	Key-down flags
					        Point point	-	Mouse position
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/
        private bool _LockMouseMove = false;
        protected void  OnLButtonMove(MouseEventArgs e)
        {
            
            Point point = e.Location;
 	        //base.OnMouseMove(e);
            //System.Diagnostics.Debug.Print("OnMouseMove");
            //CheckMouseStoped();
            if (_LockMouseMove)
            {
                if (LockMouseTimeout())
                    _LockMouseMove = false;
                else
                    return ;

                //if ((Math.Abs(point.Y - _LButtonDownPoint.Y) > KB9Const.UNIT_PIXELS * 4) ||
                //      (Math.Abs(point.X - _LButtonDownPoint.X) > KB9Const.UNIT_PIXELS * 4))
                //    _LockMouseMove = false;
                //else
                //    return;
            }
            _LockMouseMove = false;
            if (_NewButtonResize) //while add new button, make mouse can move to any direction
            {//change resize direction
                if (point.Y - _LButtonDownPoint.Y >= 0) //down
                {


                    if (point.X - _LButtonDownPoint.X >= 0)//move to right
                        m_subMode = DiagramEntity.DEHT.DEHT_BOTTOMRIGHT;
                    else
                        m_subMode = DiagramEntity.DEHT.DEHT_BOTTOMLEFT;
                }
                else
                {
                    if (point.X - _LButtonDownPoint.X >= 0)//move to right
                        m_subMode = DiagramEntity.DEHT.DEHT_TOPRIGHT;
                    else
                        m_subMode = DiagramEntity.DEHT.DEHT_TOPLEFT;
                }
            }


            if (m_interactMode == MouseMode.MODE_NONE)
            {
                OnSetCursor();
                base.OnMouseMove(e);
                return;
            }
	        //if( m_interactMode != MouseMode.MODE_NONE )
	        {

		        //CClientDC	dc( this );
                Graphics g = this.CreateGraphics();

		        Rectangle clientRect = this.ClientRectangle;
		        //GetClientRect( &clientRect );
                Point virtpoint = point;// new Point(point);
                ClientToVirtual(ref virtpoint);

		       
		        if( m_interactMode == MouseMode.MODE_RESIZING )
		        {

                    m_objs.ClearMultipleSelectedMain();

                    if (this.SnapToGrid)
			        {

				        virtpoint.X = SnapX( virtpoint.X );
				        virtpoint.Y = SnapY( virtpoint.Y );

			        }

			        // If we are moving, we will update one or 
			        // two sides of the object.
			        double ypos =(double) ( virtpoint.Y );
                    double xpos = (double)(virtpoint.X);

			        DiagramEntity obj = GetSelectedObject();
			        if( obj != null)
			        {
				        Size minimum = obj.MinimumSize;
				        Size maximum = obj.MaximumSize;
				        if( m_subMode == DiagramEntity.DEHT.DEHT_BOTTOMMIDDLE )
				        {

					        // Constraints
					        if( minimum.Height != -1 && (ypos - obj.Bounds.Top < minimum.Height) )
                                ypos = obj.Bounds.Top + minimum.Height;
                            if (maximum.Height != -1 && (ypos - obj.Bounds.Top > maximum.Height))
                                ypos = obj.Bounds.Top + maximum.Height;

					        AdjustForRestraints(ref xpos, ref ypos );
                            //Rectangle rt = obj.Location;
                            //rt.Height = ypos - rt.Top;
                            //obj.Location = rt;
					        obj.SetRect( obj.Bounds.Left, obj.Bounds.Top, obj.Bounds.Right, (int)ypos );

				        }
				        else if( m_subMode == DiagramEntity.DEHT.DEHT_TOPMIDDLE )
				        {

					        // Constraints
					        if( minimum.Height != -1 && obj.Bounds.Bottom - ypos < minimum.Height )
                                ypos = obj.Bounds.Bottom - minimum.Height;
                            if (maximum.Height != -1 && obj.Bounds.Bottom - ypos > maximum.Height)
                                ypos = obj.Bounds.Bottom - maximum.Height;

					        AdjustForRestraints( ref xpos,ref  ypos );
                            //Rectangle rt = obj.Location;
                            
                            //rt.Height = rt.Bottom - (int)ypos;
                            //rt.Y = ypos;
                            //obj.Location = rt;
					        obj.SetRect( obj.Bounds.Left, (int)ypos, obj.Bounds.Right, obj.Bounds.Bottom );

				        }
				        else if( m_subMode == DiagramEntity.DEHT.DEHT_LEFTMIDDLE )
				        {

					        // Constraints
					        if( minimum.Width != -1 && obj.Bounds.Right - xpos < minimum.Width )
                                xpos = obj.Bounds.Right - minimum.Width;
                            if (maximum.Width != -1 && obj.Bounds.Right - xpos > maximum.Width)
                                xpos = obj.Bounds.Right - maximum.Width;

					        AdjustForRestraints(ref  xpos, ref ypos );
                            //Rectangle rt = obj.Location;
                            //rt.Width = rt.Right -(i) xpos;
                            //rt.X = xpos;
                            //obj.Location =rt;

					        obj.SetRect((int) xpos, obj.Bounds.Top, obj.Bounds.Right, obj.Bounds.Bottom );

				        }
				        else if( m_subMode == DiagramEntity.DEHT. DEHT_RIGHTMIDDLE )
				        {

					        // Constraints
					        if( minimum.Width != -1 && xpos - obj.Bounds.Left < minimum.Width )
						        xpos = obj.Bounds.Left + minimum.Width;
                            if (maximum.Width != -1 && xpos - obj.Bounds.Left > maximum.Width)
                                xpos = obj.Bounds.Left + maximum.Width;

					        AdjustForRestraints(ref  xpos,ref  ypos );
					        obj.SetRect( obj.Bounds.Left, obj.Bounds.Top,(int) xpos, obj.Bounds.Bottom );

				        }
				        else if( m_subMode == DiagramEntity.DEHT. DEHT_TOPLEFT )
				        {

					        // Constraints
					        if( minimum.Height != -1 && obj.Bounds.Bottom - ypos < minimum.Height )
                                ypos = obj.Bounds.Bottom - minimum.Height;
                            if (minimum.Width != -1 && obj.Bounds.Right - xpos < minimum.Width)
                                xpos = obj.Bounds.Right - minimum.Width;
                            if (maximum.Height != -1 && obj.Bounds.Bottom - ypos > maximum.Height)
                                ypos = obj.Bounds.Bottom - maximum.Height;
                            if (maximum.Width != -1 && obj.Bounds.Right - xpos > maximum.Width)
                                xpos = obj.Bounds.Right - maximum.Width;

					        AdjustForRestraints( ref xpos, ref ypos );
                            obj.SetRect((int)xpos, (int)ypos, obj.Bounds.Right, obj.Bounds.Bottom);

				        }
				        else if( m_subMode == DiagramEntity.DEHT. DEHT_TOPRIGHT )
				        {

					        // Constraints
					        if( minimum.Height != -1 && obj.Bounds.Bottom - ypos < minimum.Height )
                                ypos = obj.Bounds.Bottom - minimum.Height;
                            if (minimum.Width != -1 && xpos - obj.Bounds.Left < minimum.Width)
                                xpos = obj.Bounds.Left + minimum.Width;
                            if (maximum.Height != -1 && obj.Bounds.Bottom - ypos > maximum.Height)
                                ypos = obj.Bounds.Bottom - maximum.Height;
                            if (maximum.Width != -1 && xpos - obj.Bounds.Left > maximum.Width)
                                xpos = obj.Bounds.Left + maximum.Width;

					        AdjustForRestraints(ref  xpos,ref  ypos );
                            obj.SetRect(obj.Bounds.Left, (int)ypos, (int)xpos, obj.Bounds.Bottom);

				        }
				        else if( m_subMode == DiagramEntity.DEHT. DEHT_BOTTOMLEFT )
				        {

					        // Constraints
                            if (minimum.Height != -1 && ypos - obj.Bounds.Top < minimum.Height)
                                ypos = obj.Bounds.Top + minimum.Height;
                            if (minimum.Width != -1 && obj.Bounds.Right - xpos < minimum.Width)
                                xpos = obj.Bounds.Right - minimum.Width;
                            if (maximum.Height != -1 && ypos - obj.Bounds.Top > maximum.Height)
                                ypos = obj.Bounds.Top + maximum.Height;
                            if (maximum.Width != -1 && obj.Bounds.Right - xpos > maximum.Width)
                                xpos = obj.Bounds.Right - maximum.Width;

					        AdjustForRestraints( ref xpos,ref  ypos );
                            obj.SetRect((int)xpos, obj.Bounds.Top, obj.Bounds.Right, (int)ypos);

				        }
				        else if( m_subMode == DiagramEntity.DEHT. DEHT_BOTTOMRIGHT )
				        {

					        // Constraints
                            if (minimum.Height != -1 && ypos - obj.Bounds.Top < minimum.Height)
                                ypos = obj.Bounds.Top + minimum.Height;
                            if (minimum.Width != -1 && xpos - obj.Bounds.Left < minimum.Width)
                                xpos = obj.Bounds.Left + minimum.Width;
                            if (maximum.Height != -1 && ypos - obj.Bounds.Top > maximum.Height)
                                ypos = obj.Bounds.Top + maximum.Height;
                            if (maximum.Width != -1 && xpos - obj.Bounds.Left > maximum.Width)
                                xpos = obj.Bounds.Left + maximum.Width;

					        AdjustForRestraints( ref xpos, ref ypos );
                            obj.SetRect(obj.Bounds.Left, obj.Bounds.Top, (int)xpos,(int) ypos);

				        }

			        }

			        // Scroll if we are outside any edge
			        ScrollPoint( point );
			        RedrawWindow();
                   // FireShowPropertiesEvent(obj);
		        }
		        else if( m_objs != null && m_interactMode == MouseMode. MODE_MOVING )
		        {
                    
			        // If we move, we just update the positions
			        // of all the objects.
                    double offsetx = (double)decimal.Round((decimal)(m_deltaPoint.X) / (decimal)this.Zoom);
                    double offsety = (double)decimal.Round((decimal)(m_deltaPoint.Y) / (decimal)this.Zoom);
			        int count = 0;
			        DiagramEntity obj;
			        double width;
			        double height;

			        double left;
			        double top;

			        if( m_multiSelObj != null )
			        {
                        if (Is_Moving_Entity(e))
                            this.Cursor = m_multiSelObj.GetCursor(DiagramEntity.DEHT.DEHT_BODY);
                        else //20141126
                            return;
				        left = virtpoint.X - offsetx;
				        top = virtpoint.Y - offsety;
                        if (this.SnapToGrid)
				        {
					        left = SnapX(  ( int )( left ) );
					        top = SnapY( ( int )( top ) );
				        }

                        offsetx = left - m_multiSelObj.Bounds.Left;
                        offsety = top - m_multiSelObj.Bounds.Top;

				        InsideRestraints(ref offsetx, ref offsety );

                        m_multiSelObj.SetRect(m_multiSelObj.Bounds.Left + (int)offsetx, m_multiSelObj.Bounds.Top + (int)offsety, m_multiSelObj.Bounds.Right + (int)offsetx, m_multiSelObj.Bounds.Bottom +(int) offsety);

				        while( ( obj = m_objs.GetAt( count++ ) ) != null)
					        if( obj.Selected && obj != m_multiSelObj )
						        obj.MoveRect( offsetx, offsety );

                       // FireShowPropertiesEvent(m_multiSelObj);
			        }
			        else
			        {

				        obj = GetSelectedObject();
                        
				        if( obj != null )
				        {
                           // if (Math.Abs(e.Location.X - _LButtonDownPoint.X) > KB9Const.UNIT_PIXELS || Math.Abs(e.Location.Y - _LButtonDownPoint.Y) > KB9Const.UNIT_PIXELS)
                            if (Is_Moving_Entity(e))
                            {
                                //System.Diagnostics.Debug.Print("offsetx={0}", offsetx);
                                //System.Diagnostics.Debug.Print("offsety={0}", offsety);

                                this.Cursor = obj.GetCursor(DiagramEntity.DEHT.DEHT_BODY);
                            }
                            else //20141126
                                return;
                            width = obj.Bounds.Width;//.Right - obj.GetLeft();
					        height = obj.Bounds.Height;//.GetBottom() - obj.GetTop();

					        left = virtpoint.X - offsetx;
					        top = virtpoint.Y - offsety;

                            if (this.SnapToGrid)
					        {
						        left = SnapX( ( int )( left ) );
						        top = SnapY( ( int )( top ) );
					        }

					        double right = left + width;
					        double bottom = top + height;

					        AdjustForRestraints(ref left,ref top, ref right,ref bottom );
                            obj.SetRect((int)left, (int)top, (int)right, (int)bottom);

				        }
                        //FireShowPropertiesEvent(obj);
			        }

			        // Scroll if we are outside any edge
			        Point outside = ScrollPoint( point );
			        RedrawWindow();

		        }
		        else if( m_interactMode == MouseMode. MODE_RUBBERBANDING )
		        {

			        // We are selecting objects
                    Rectangle rect = m_selectionRect;// new Rectangle(m_selectionRect);

			        // Erase old selection rect
			        if( m_selectionRect.Left != m_selectionRect.Right || m_selectionRect.Top != m_selectionRect.Bottom )
			        {
				        rect  = NormalizeRect(rect);
                        ControlPaint.DrawFocusRectangle(g, rect);
				        //dc.DrawFocusRect( rect );
			        }

			        // Scroll if we are outside any edge
			        Point outside = ScrollPoint( point );
			        m_selectionRect.Offset( -outside.X, -outside.Y );

			        // Update and draw the selection rect
                    //m_selectionRect = new Rectangle()
                    m_selectionRect.Width = point.X - m_selectionRect.Left;
                    m_selectionRect.Height = point.Y - m_selectionRect.Top;
			        //m_selectionRect.Right = point.X;
			        //m_selectionRect.Bottom = point.Y;
			        rect = m_selectionRect;
                    rect = NormalizeRect(rect);
                    RedrawWindow();
                    ControlPaint.DrawFocusRectangle(g, rect);
                    
			        //dc.DrawFocusRect( rect );

		        }

	        }

	        //CWnd::OnMouseMove( nFlags, point );
            base.OnMouseMove(e);
        }

        protected override void  OnMouseUp(MouseEventArgs e)
        {
            
 	         base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
                OnLButtonUp(e);
            else if (e.Button == MouseButtons.Right)
                OnRButtonUp(e);
            else if (e.Button == MouseButtons.Middle)
            {
                
            }
        }
        
       protected  void OnLButtonUp( MouseEventArgs e )
        /* ============================================================
	        Function :		OnLButtonUp
	        Description :	Handles the "WM_LBUTTONUP" message. Mainly, 
					        we snap to grid if appropriate.
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nFlags		-	not interested.
					        Point point	-	The position of the mouse.
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/
        {

            m_bMouseMovingDistanceCheckPassed = false;
            if (_LButtonDownPoint != null &&  e.Location != _LButtonDownPoint)
            {
                if (m_objs != null &&
                    this.GetSelectCount() >0)
                {
                    if (m_objs.SaveSnapshot())
                        FireChangedEvent();
                }
                
                _LButtonDownPoint = new Point(-1,-1);
            }
	        // Reset modes
	        //ReleaseCapture();
            this.Capture = false;

	        int count = 0;
	        DiagramEntity obj;

	        if( m_objs!= null && m_interactMode == MouseMode.MODE_MOVING )
	        {

		        // We set all the object rectangles to trigger
		        // the virtual MoveObject function, thus 
		        // allowing derived editors to know that 
		        // objects has been moved.
		        while( ( obj = m_objs.GetAt( count++ ) ) != null )
		        {
                    Rectangle rtOld = obj.Bounds;// 
			        if( obj.Selected )
			        {
                        Rectangle rect = obj.Bounds;// 
                        if (this.SnapToGrid)
				        {
					        // If we move objects, and snap to grid is on
					        // we snap here
					        int height = rect.Height;
					        int width = rect.Width;

					        // Find closest snap-points
                            rect = new Rectangle(SnapX(rect.Left), SnapY(rect.Top), width, height);
					       
				        }

				        MoveObject( obj, rect );
                        if (obj.Bounds != rtOld)
                        {
                            m_objs.Modified = (true);
                            FireShowPropertiesEvent(obj);
                        }
			        }
		        }
                //FireShowPropertiesEvent();
                

	        }
	        else if( m_objs!= null && m_interactMode == MouseMode.MODE_RESIZING )
	        {
		        // If we resize objects, and snap to grid is on
		        // we snap here

                if (this.SnapToGrid)
		        {
			        while( ( obj = m_objs.GetAt( count++ ) ) != null)
			        {
                        Rectangle rtOld = obj.Bounds;// 
				        if( obj.Selected )
				        {
                            Rectangle rect = obj.Bounds;// GetRect();
					        Rectangle newrect = rect;

					        // Find closest snap-points
                            int l = SnapX( rect.Left );
                            int t = SnapY( rect.Top );
                            int w = SnapX( rect.Right ) - l;
                            int h = SnapY ( rect.Bottom ) - t;
                            rect = new Rectangle(l, t, w, h);

					        //rect.left = SnapX( rect.left );
					        //rect.top = SnapY( rect.top );
					        //rect.right = SnapX( rect.right );
					        //rect.bottom = SnapY ( rect.bottom );

					        // Updating rect depending on resize-type.
					        switch( m_subMode )
					        {
						        case  DiagramEntity.DEHT. DEHT_BOTTOMMIDDLE:
							        //newrect.Bottom = rect.Bottom;
                                    newrect.Height = rect.Bottom - newrect.Top;
						        break;
						        case DiagramEntity.DEHT.DEHT_TOPMIDDLE:
                                    newrect.Y = rect.Top;
							        //newrect.Top = rect.Top;
						        break;
						        case DiagramEntity.DEHT.DEHT_LEFTMIDDLE:
							        //newrect.Left = rect.Left;
                                    newrect.X = rect.Left;
						        break;
						        case DiagramEntity.DEHT.DEHT_RIGHTMIDDLE:
							        //newrect.Right = rect.Right;
                                    newrect.Width = rect.Right - newrect.Left;
						        break;
						        case DiagramEntity.DEHT.DEHT_TOPLEFT:
							        //newrect.Top = rect.Top;
                                    newrect.Y = rect.Top;
							        //newrect.Left = rect.Left;
                                    newrect.X = rect.Left;
						        break;
						        case DiagramEntity.DEHT.DEHT_TOPRIGHT:
							        //newrect.Top = rect.Top;
                                    newrect.Y = rect.Top;
							        //newrect.Right = rect.Right;
                                    newrect.Width = rect.Right - newrect.Left;
						        break;
						        case DiagramEntity.DEHT.DEHT_BOTTOMLEFT:
							        //newrect.Bottom = rect.Bottom;
                                    newrect.Height = rect.Bottom - newrect.Top;
							        //newrect.Left = rect.Left;
                                    newrect.X = rect.Left;
						        break;
						        case DiagramEntity.DEHT.DEHT_BOTTOMRIGHT:
							        //newrect.Bottom = rect.Bottom;
                                    newrect.Height = rect.Bottom - newrect.Top;
							        //newrect.Right = rect.Right;
                                    newrect.Width = rect.Right - newrect.Left;
						        break;
					        }

					        obj.Bounds = newrect;// SetRect( newrect );

                            if (obj.Bounds != rtOld)
                            {
                                m_objs.Modified = (true);
                                FireShowPropertiesEvent(obj);
                            }
				        }
                        
			        }
		        }
                //m_objs.Modified = (true);

	        }
	        else if( m_objs!= null && m_interactMode ==  MouseMode.MODE_RUBBERBANDING )
	        {

		        // Remove all former selections
		        UnselectAll();

		        // We convert the selection rect to virtual coordinates, 
		        // and make sure that the rect contains at least some 
		        // width and height ( IntersectRect will not work otherwise )
                Rectangle rect = m_selectionRect;// new Rectangle(m_selectionRect);
                ClientToVirtual(ref  rect);
		        //if( rect.TopLeft() == rect.BottomRight() )
                if (rect.Location == new Point(rect.Right, rect.Bottom))
			        rect.Inflate( 1, 1 );

		        // We loop all objects, checking if we got any overlap.
		        count = 0;
		        while( ( obj = m_objs.GetAt( count++ ) ) != null )
		        {
			        if( obj.BodyInRect( rect ) )
				        obj.Selected = ( true );
		        }

                m_objs.SetMultipleSelectedMain(null);

	        }

	        // Redraw and reset modes
	        RedrawWindow();
            if (this.MultipleDrawOneObject && this.IsDrawingObj && this.DrawingObject != null)// m_drawObj!= null )
            {
                m_interactMode = MouseMode.MODE_DRAWING;
            }
            else
            {
                //m_drawing = false;
                _IsDrawingObj = false;
                m_interactMode = MouseMode.MODE_NONE;
                this.Cursor = Cursors.Arrow;
            }

	        m_subMode = DiagramEntity.DEHT.DEHT_NONE;
            
	        //CWnd::OnLButtonUp( nFlags, point );
           //show properties of virutal screen
           if (GetSelectCount() <=0)
               FireShowPropertiesEvent(null);
           if (GetSelectCount() >= 1)//20140128
           {
               Front(); //make sure selected is in front
               FireShowPropertiesEvent(GetSelectedObject());
           }

        }

        protected override void  OnMouseDoubleClick(MouseEventArgs e)
        {
 	        base.OnMouseDoubleClick(e);
            if (e.Button == MouseButtons.Left)
            {
                OnLButtonDblClk(e);
            }
            else
            {
                
            }
        }

        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    base.OnMouseClick(e);
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        OnLButtonDblClk(e);
        //    }
        //}

        protected void OnLButtonDblClk( MouseEventArgs e ) 
        {
            Point point = e.Location;
            if (GetSelectCount() == 1)
            {
                DiagramEntity obj = GetSelectedObject();
                Rectangle rect = obj.Bounds;// GetRect();
                VirtualToClient(ref rect);

                DiagramEntity.DEHT hitCode = obj.GetHitCode(point, rect);
                if (hitCode != DiagramEntity.DEHT.DEHT_NONE)
                {
                    ShowKeyContentEditor();
                }

            }
	        

	        

        }
        private Point _RButtonDownPoint = new Point(-1, -1);
        private Point _ScrollPoint = new Point(-1, -1);
        protected void OnRButtonDown( MouseEventArgs e ) 
        /* ============================================================
	        Function :		OnRButtonDown
	        Description :	Handles the "WM_RBUTTONDOWN" message. 
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nFlags		-	not interested
					        Point point	-	not interested
        					
	        Usage :			

           ============================================================*/
        {

            Point point = e.Location;
           
            _RButtonDownPoint = point;
            _ScrollPoint = this.AutoScrollPosition;
	        //SetPanning( false );
            //this.MiddleButtonPanning = false;

	        if( m_objs != null)
	        {
		        int count = 0;
		        DiagramEntity obj;
		        Point virtpoint = point;// new Point( point );
                ClientToVirtual(ref  virtpoint);

		        // We check if we click on any object body. If that is 
		        // the case, we select the object
		        while( ( obj = m_objs.GetAt( count++ ) ) != null )
		        {
			        if( obj.GetHitCode( virtpoint ) == DiagramEntity.DEHT.DEHT_BODY )
			        {
				        if( !obj.Selected )
				        {
					        UnselectAll();
					        obj.Selected = ( true );
				        }
			        }
		        }

		        RedrawWindow();
	        }

	        m_interactMode = MouseMode. MODE_NONE;
	        //m_drawing = false;
            _IsDrawingObj = false;
           
	      

        }

        private void show_popup_menu(MouseEventArgs e)
        {
            Point point = e.Location;
            Point screen = point;// new Point(point);
            Point virtpoint = point;// new Point(point);

            //ClientToScreen( &screen );
            ClientToVirtual(ref virtpoint);

            if (GetSelectCount() == 1)
            {
                DiagramEntity obj = GetSelectedObject();
                if (obj.GetHitCode(virtpoint) == DiagramEntity.DEHT.DEHT_BODY)
                    obj.ShowPopup(screen, this);
                else
                    ShowPopup(screen);
            }
            else
            {
                //if clicked in certain selected object, mark it as main in all selected 
                //int ncount = GetSelectCount();
                //DiagramEntity retval = null;

                //
                CheckAndSetMultipleSelectedMain(point);

                ShowPopup(screen);
            }

            //CWnd::OnRButtonUp( nFlags, point );
            //show properties of virutal screen
            if (GetSelectCount() <= 0)
                FireShowPropertiesEvent(null);
        }
        protected void  OnRButtonMove(MouseEventArgs e)
        {
            //this.Cursor = Cursors.Hand;
            Point pt = new Point();
            
            pt.X =Math.Abs(_ScrollPoint.X) + (int)( Math.Abs(_RButtonDownPoint.X)-Math.Abs(e.X)    );
            pt.Y =Math.Abs( _ScrollPoint.Y )+ (int)(Math.Abs(_RButtonDownPoint.Y)- Math.Abs(e.Y)   );
            //System.Diagnostics.Debug.Print("=============================");
            //System.Diagnostics.Debug.Print(_ScrollPoint.ToString());
            //System.Diagnostics.Debug.Print(_RButtonDownPoint.ToString());
            //System.Diagnostics.Debug.Print(e.Location.ToString());
            //System.Diagnostics.Debug.Print(pt.ToString());

            this.AutoScrollPosition = pt;

        }
        protected void OnRButtonUp( MouseEventArgs e  ) 
        /* ============================================================
	        Function :		OnRButtonUp
	        Description :	Handles the "WM_RBUTTONUP" message. We show 
					        popup menues as appropriate.
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nFlags		-	not interested
					        Point point	-	not interested
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/
        {
            this.Cursor = Cursors.Default;
            if (_RButtonDownPoint != null && e.Location == _RButtonDownPoint)
            {

                show_popup_menu(e);
                _RButtonDownPoint = new Point(-1, -1);
            }
            _RButtonDownPoint = new Point(-1, -1);
            _ScrollPoint = new Point(-1, -1);
            
            //Point point = e.Location;
            //Point screen = point;// new Point(point);
            //Point virtpoint = point;// new Point(point);

            ////ClientToScreen( &screen );
            //ClientToVirtual(ref virtpoint);

            //if (GetSelectCount() == 1)
            //{
            //    DiagramEntity obj = GetSelectedObject();
            //    if (obj.GetHitCode(virtpoint) == DiagramEntity.DEHT.DEHT_BODY)
            //        obj.ShowPopup(screen, this);
            //    else
            //        ShowPopup(screen);
            //}
            //else
            //{
            //    //if clicked in certain selected object, mark it as main in all selected 
            //    //int ncount = GetSelectCount();
            //    //DiagramEntity retval = null;
                
            //    //
            //    CheckAndSetMultipleSelectedMain(point);
              
            //    ShowPopup(screen);
            //}

            ////CWnd::OnRButtonUp( nFlags, point );
            ////show properties of virutal screen
            //if (GetSelectCount() <= 0)
            //    FireShowPropertiesEvent(null);
        }

        private void CheckAndSetMultipleSelectedMain(Point point)
        {
            if (GetSelectCount() <= 1) return;
            Point virtpoint = point;// new Point(point);

            //ClientToScreen( &screen );
            ClientToVirtual(ref virtpoint);
            if (m_objs != null)
            {
                //check if it click on selected object
                DiagramEntity obj;
                int count = 0;
                while ((obj = m_objs.GetAt(count++)) != null)
                {
                    if (obj.Selected)
                    {
                        if (obj.GetHitCode(virtpoint) == DiagramEntity.DEHT.DEHT_BODY)
                        {
                            m_objs.SetMultipleSelectedMain(obj);
                            m_multiSelObj = obj;
                            this.RedrawWindow();
                        }
                    }
                    //           retval = obj;
                }
            }
        }

        //int OnGetDlgCode() 
        ///* ============================================================
        //    Function :		OnGetDlgCode
        //    Description :	Handles the "WM_GETDLGCODE" message. We 
        //                    return the keys we want.
        //    Access :		Protected

        //    Return :		UINT	-	"DLGC_WANTALLKEYS", we want all 
        //                                keys.
        //    Parameters :	none

        //    Usage :			Called from MFC. Do not call from code.

        //   ============================================================*/
        //{

        //    return DLGC_WANTALLKEYS;

        //}
        /* ============================================================
         * enable arrow key in onkeydown
          ============================================================*/
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            e.IsInputKey = true;
        }

        /* ============================================================
	        Function :		OnKeyDown
	        Description :	Handles the "WM_KEYDOWN" message. We provide 
					        a simple keyboard interface.
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nChar		-	Character pressed
					        UINT nRepCnt	-	not interested
					        UINT nFlags		-	not interested
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/

        protected override void  OnKeyDown(KeyEventArgs e)
        {
           
            //System.Diagnostics.Debug.Print(e.KeyCode.ToString());

 	         base.OnKeyDown(e);


            Keys nChar = e.KeyCode;

	        //SetPanning( false );
            //this.MiddleButtonPanning = false;

	        if( m_keyInterface != KeyInterface.KEY_NONE )
	        {
		        // Clearing the flags
		        //GetAsyncKeyState( VK_SHIFT );
		        //GetAsyncKeyState( VK_CONTROL );

		        if( m_objs != null)
		        {

			        int count = 0;
			        DiagramEntity obj;

                    double offsetstepx = 1.0 / this.Zoom;
                    double offsetstepy = 1.0 / this.Zoom;
                    if (this.SnapToGrid)
			        {
                        offsetstepx = (double)(this.GridSize.Width);// *this.Zoom;
                        offsetstepy = (double)(this.GridSize.Height);// *this.Zoom;
			        }

			        offsetstepx = Math.Max( offsetstepx, 1 );
			        offsetstepy = Math.Max( offsetstepy, 1 );

			        double offsetx = 0.0;
			        double offsety = 0.0;

			        bool move = false;
			        bool resize = false;
			        bool redraw = true;
        			
                    bool bShiftDown = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
                    bool bCtrlDown =  ((Control.ModifierKeys & Keys.Control) == Keys.Control);

                    //System.Diagnostics.Debug.Print(e.KeyCode.ToString());

			        if( ( m_keyInterface & KeyInterface.KEY_ARROW ) !=0 && ( nChar ==Keys.Down ) )
			        {
				        offsety=offsetstepy;
				        //if( GetAsyncKeyState( VK_SHIFT ) )
                        if (bShiftDown)
					        resize = true;
				        else
					        move = true;
			        }
			        else if( ( m_keyInterface &KeyInterface.KEY_ARROW )!=0 && ( nChar ==Keys.Up ) )
			        {
				        offsety=-offsetstepy;
				        //if( GetAsyncKeyState( VK_SHIFT ) )
                        if (bShiftDown)
					        resize = true;
				        else
					        move = true;
			        }
                    else if ((m_keyInterface & KeyInterface.KEY_ARROW) != 0 && (nChar == Keys.Left))
			        {
				        offsetx=-offsetstepx;
				        //if( GetAsyncKeyState( VK_SHIFT ) )
                        if (bShiftDown)
					        resize = true;
				        else
					        move = true;
			        }
                    else if ((m_keyInterface & KeyInterface.KEY_ARROW) != 0 && (nChar == Keys.Right))
			        {
				        offsetx=offsetstepx;
				        //if( GetAsyncKeyState( VK_SHIFT ) )
                        if (bShiftDown)
					        resize = true;
				        else
					        move = true;
			        }
                    else if ((m_keyInterface & KeyInterface.KEY_DELETE) != 0 && (nChar == Keys.Delete))
			        {
				        //if( GetAsyncKeyState( VK_SHIFT ) )
                        if (bShiftDown)
					        Cut();
				        else
					        DeleteAllSelected();
			        }
                    else if ((m_keyInterface & KeyInterface.KEY_INSERT) != 0 && (nChar == Keys.Insert))
			        {
				        //if( GetAsyncKeyState( VK_SHIFT ) )
                        if (bShiftDown)
					        Paste(false);
				        else
					        Duplicate();
			        }
                    else if (nChar == Keys.Tab)
                    {
                        MoveFocusToNext();
                    }
                    else if ((m_keyInterface & KeyInterface.KEY_CTRL) != 0 && nChar == Keys.Return && bCtrlDown)// GetAsyncKeyState( VK_CONTROL ) )
                        ShowKeyContentEditor();
                    else 
                    if ((m_keyInterface & KeyInterface.KEY_CTRL) != 0 && nChar == Keys.A && bCtrlDown)//GetAsyncKeyState( VK_CONTROL ) )
				        SelectAll();
                    else if ((m_keyInterface & KeyInterface.KEY_CTRL) != 0 && nChar == Keys.X && bCtrlDown)//GetAsyncKeyState( VK_CONTROL ) )
				        Cut();
                    else if ((m_keyInterface & KeyInterface.KEY_CTRL) != 0 && nChar == Keys.V && bCtrlDown)//GetAsyncKeyState( VK_CONTROL ) )
				        Paste(false);
                    else if ((m_keyInterface & KeyInterface.KEY_CTRL) != 0 && nChar == Keys.C && bCtrlDown)//GetAsyncKeyState( VK_CONTROL ) )
				        Copy();
                    else if ((m_keyInterface & KeyInterface.KEY_CTRL) != 0 && nChar == Keys.Z && bCtrlDown)//GetAsyncKeyState( VK_CONTROL ) )
				        Undo();
                    //else if ((m_keyInterface & KeyInterface.KEY_PGUPDOWN) != 0 && nChar == Keys.Next && bCtrlDown)//GetAsyncKeyState( VK_CONTROL ) )
                    //{
                    //    //BottomEditor();
                    //}
                    //else if ((m_keyInterface & KeyInterface.KEY_PGUPDOWN) != 0 && nChar == Keys.Prior && bCtrlDown)//GetAsyncKeyState( VK_CONTROL ) )
                    //{
                    //    //   Front();
                    //}
                    //else if ((m_keyInterface & KeyInterface.KEY_PGUPDOWN) != 0 && nChar == Keys.Next)
                    //{
                    //    //Down();
                    //}
                    //else if ((m_keyInterface & KeyInterface.KEY_PGUPDOWN) != 0 && nChar == Keys.Prior)
                    //{
                    //    //Up();
                    //}

			        if( move )
			        {
				        InsideRestraints(ref offsetx, ref offsety );
				        m_objs.Snapshot();
				        while( ( obj = m_objs.GetAt( count++ ) )!= null )
				        {
					        if( obj.Selected )
					        {
                                double width = obj.Bounds.Width;// GetRight() - obj.GetLeft();
                                double height = obj.Bounds.Height;// GetBottom() - obj.GetTop();

                                double left = obj.Bounds.Left + offsetx;
                                double top = obj.Bounds.Top + offsety;

                                if (this.SnapToGrid)
						        {
							        left = SnapX( ( int )( left ) );
							        top = SnapY( ( int )( top ) );
						        }

						        double right = left + width;
						        double bottom = top + height;

						        AdjustForRestraints(ref left,ref top,ref right,ref bottom );
                                MoveObject(obj, new Rectangle((int)decimal.Round((decimal)left), 
                                    (int)decimal.Round((decimal)top),
                                    (int)decimal.Round((decimal)(right - left)), 
                                    (int)decimal.Round((decimal)(bottom - top))));
						        redraw = true;
                                m_objs.Modified = (true);
					        }
				        }
                        //20140128
                        if (GetSelectCount() >= 1)
                            FireShowPropertiesEvent(GetSelectedObject());
			        }

			        if( resize )
			        {
				        m_objs.Snapshot();
				        InsideResizeRestraints(ref offsetx, ref offsety );
				        while( ( obj = m_objs.GetAt( count++ ) ) != null)
				        {
					        if( obj.Selected )
					        {

                                double right = obj.Bounds.Right + offsetx;
                                double bottom = obj.Bounds.Bottom + offsety;
                                if (this.SnapToGrid)
						        {
							        right = SnapX( ( int )( right ) );
							        bottom = SnapY( ( int )( bottom ) );
						        }

                                obj.SetRect(obj.Bounds.Left, obj.Bounds.Top, (int)right, (int)bottom);

                                m_objs.Modified = (true);
						        redraw = true;
					        }
				        }
			        }
                    //20140128
                    if (GetSelectCount() >= 1)
                        FireShowPropertiesEvent(GetSelectedObject());

			        if( redraw )
				        RedrawWindow();
		        }

		        if( ( (m_keyInterface & KeyInterface.KEY_ESCAPE) !=0) && ( nChar ==Keys.Escape ) )
		        {
			        m_interactMode = MouseMode. MODE_NONE;
			        //m_drawing = false;
                    _IsDrawingObj = false;
		        }

		        // Keys independent of any data
		        if( ( (m_keyInterface & KeyInterface.KEY_PLUSMINUS)!=0 ) && ( nChar == Keys.Add ) )
                    this.Zoom = (this.Zoom + this.ZoomStep);

		        if( ( (m_keyInterface & KeyInterface.KEY_PLUSMINUS)!=0 ) && ( nChar ==Keys.Subtract ) )
                    this.Zoom = (this.Zoom - this.ZoomStep);
	        }

	        //CWnd::OnKeyDown( nChar, nRepCnt, nFlags );
            //show properties of virutal screen
            if (GetSelectCount() <= 0)
                FireShowPropertiesEvent(null);
        }

        /* ============================================================
	        Function :		OnSetCursor
	        Description :	Handles the "WM_SETCURSOR" message. We set 
					        cursors as appropriate.
	        Access :		Protected

	        Return :		bool			-	not interested
	        Parameters :	CWnd* pWnd		-	not interested
					        UINT nHitTest	-	not interested
					        UINT message	-	not interested
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/
        private void  OnSetCursor()
        {
 	         //base.OnCursorChanged(e);

 //       bool OnSetCursor( CWnd* pWnd, UINT nHitTest, UINT message ) 
        
        //{

	        //bool res = true;
	        Cursor cursor = GetCursor();
            if (cursor == null)
                cursor = Cursors.Arrow;
	        
                this.Cursor = cursor;
		        //::SetCursor( cursor );
	        //else
		     //   res = CWnd::OnSetCursor( pWnd, nHitTest, message );

	        //return res;

        }

       public Cursor GetCursor( DiagramEntity.DEHT  hit ) 
        /* ============================================================
	        Function :		GetCursor
	        Description :	Returns a "HCURSOR" for the hit-point value 
					        (defined in DiagramEntity.h) 
	        Access :		Public

	        Return :		HCURSOR	-	The cursor to show, "null" if 
								        default.
	        Parameters :	int hit	-	The hit-point value
        					
	        Usage :			Virtual. Can be overridden in a derived
					        class to display other resize cursors. 
					        "hit" can be one of the following:
						        "DEHT_NONE" No hit-point
						        "DEHT_BODY" Inside object body
						        "DEHT_TOPLEFT" Top-left corner
						        "DEHT_TOPMIDDLE" Middle top-side
						        "DEHT_TOPRIGHT" Top-right corner
						        "DEHT_BOTTOMLEFT" Bottom-left corner
						        "DEHT_BOTTOMMIDDLE" Middle bottom-side
						        "DEHT_BOTTOMRIGHT" Bottom-right corner
						        "DEHT_LEFTMIDDLE" Middle left-side
						        "DEHT_RIGHTMIDDLE" Middle right-side

           ============================================================*/
        {

	        // We only show cursors for a subset of the hit-point codes.
	        Cursor cursor =Cursors.Arrow;//  LoadCursor( null, IDC_ARROW );
	        switch( hit )
	        {
		        case DiagramEntity.DEHT.DEHT_TOPRIGHT:
			        cursor =Cursors.SizeNESW;//  LoadCursor( null, IDC_SIZENESW );
		        break;
		        case DiagramEntity.DEHT.DEHT_BOTTOMMIDDLE:
			        cursor =Cursors.SizeNS;// LoadCursor( null, IDC_SIZENS );
		        break;
		        case DiagramEntity.DEHT.DEHT_BOTTOMRIGHT:
			        cursor =Cursors.SizeNWSE;// LoadCursor( null, IDC_SIZENWSE );
		        break;
		        case DiagramEntity.DEHT.DEHT_BOTTOMLEFT:
			        cursor =Cursors.SizeNESW;// LoadCursor( null, IDC_SIZENESW );
		        break;
		        case DiagramEntity.DEHT.DEHT_RIGHTMIDDLE:
			        cursor =Cursors.SizeWE;// LoadCursor( null, IDC_SIZEWE );
		        break;
	        }

	        return cursor;

        }

       public  DiagramEntity.DEHT GetHitCode( Point point )
        /* ============================================================
	        Function :		GetHitCode
	        Description :	Will return the hit-point value (defined in 
					        DiagramEntity.h) of point.
	        Access :		Public

	        Return :		int				-	The hit-point define of 
										        point. "DEHT_NONE" if none.
	        Parameters :	Point point	-	The point to hit-test.
        					
	        Usage :			Virtual. Can be overridden in a derived 
					        class to return other hit-point values. 
					        Should normally not be called from code.
					        The hit point can be one of the following:
						        "DEHT_NONE" No hit-point
						        "DEHT_BODY" Inside object body
						        "DEHT_TOPLEFT" Top-left corner
						        "DEHT_TOPMIDDLE" Middle top-side
						        "DEHT_TOPRIGHT" Top-right corner
						        "DEHT_BOTTOMLEFT" Bottom-left corner
						        "DEHT_BOTTOMMIDDLE" Middle bottom-side
						        "DEHT_BOTTOMRIGHT" Bottom-right corner
						        "DEHT_LEFTMIDDLE" Middle left-side
						        "DEHT_RIGHTMIDDLE" Middle right-side

           ============================================================*/
        {

            Rectangle rect = new Rectangle(0, 0, this.VirtualSize.Width, this.VirtualSize.Height);

	        DiagramEntity.DEHT result = DiagramEntity.DEHT.DEHT_NONE;

	        if( rect.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_BODY;

	        Rectangle rectTest;

	        // We return all selection marker points for aestethical 
	        // reasons, even if we can't resize the background to 
	        // the top and left.
            VirtualToClient(ref point);
	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_TOPLEFT );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_TOPLEFT;

	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_TOPMIDDLE );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_TOPMIDDLE;

	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_TOPRIGHT );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_TOPRIGHT;

	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_BOTTOMLEFT );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_BOTTOMLEFT;

	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_BOTTOMMIDDLE );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_BOTTOMMIDDLE;

	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_BOTTOMRIGHT );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_BOTTOMRIGHT;

	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_LEFTMIDDLE );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_LEFTMIDDLE;

	        rectTest = GetSelectionMarkerRect( DiagramEntity.DEHT.DEHT_RIGHTMIDDLE );
	        if( rectTest.Contains( point ) )
		        result = DiagramEntity.DEHT.DEHT_RIGHTMIDDLE;

	        return result;

        }
        protected void InsideResizeRestraints(ref  double offsetx, ref double offsety)
        /* ============================================================
	      while use key arrow to resize object, check offset

           ============================================================*/
        {
            if (this.Restraints != RESTRAIN.RESTRAINT_NONE)
            {

                int count = 0;
                DiagramEntity obj;
                double minleft = 0xffffffff;
                double mintop = 0xffffffff;
                double maxright = 0;
                double maxbottom = 0;

                while ((obj = m_objs.GetAt(count++)) != null)
                {
                    if (obj.Selected)
                    {

                        // Correcting, depending on restraint mode.
                        // Note that checks will have to be made for all 
                        // coordinates against all sides, as the coordinates 
                        // might not be normalized (as for a line, for example).

                        double left = obj.Bounds.Left;// +x;
                        double top = obj.Bounds.Top;// +y;
                        double right = obj.Bounds.Right + offsetx;
                        double bottom = obj.Bounds.Bottom + offsety;

                        minleft = Math.Min(minleft, left);
                        minleft = Math.Min(minleft, right);
                        mintop = Math.Min(mintop, top);
                        mintop = Math.Min(mintop, bottom);

                        maxright = Math.Max(maxright, left);
                        maxright = Math.Max(maxright, right);
                        maxbottom = Math.Max(maxbottom, top);
                        maxbottom = Math.Max(maxbottom, bottom);


                    }
                }

                double leftedge = 0;
                double rightedge = 0;
                double topedge = 0;
                double bottomedge = 0;

                if (this.Restraints == RESTRAIN.RESTRAINT_VIRTUAL)
                {
                    leftedge = 0;
                    topedge = 0;
                    rightedge = this.VirtualSize.Width;
                    bottomedge = this.VirtualSize.Height;
                }
                else if (this.Restraints == RESTRAIN.RESTRAINT_MARGIN)
                {
                    Padding padding = this.VirtualScreenMargin;

                    leftedge = padding.Left + 1;
                    topedge = padding.Top + 1;
                    rightedge = this.VirtualSize.Width - padding.Right - 1;
                    bottomedge = this.VirtualSize.Height - padding.Bottom - 1;
                }

                if (minleft < leftedge)
                    offsetx = offsetx - (minleft - leftedge);
                if (mintop < topedge)
                    offsety = offsety - (mintop - topedge);
                if (maxright > rightedge)
                    offsetx = rightedge - (maxright - offsetx);
                if (maxbottom > bottomedge)
                    offsety = bottomedge - (maxbottom - offsety);

            }
        }
       protected void InsideRestraints(ref  double x,ref double y )
        /* ============================================================
	        Function :		InsideRestraints
	        Description :	Returns the max x and y that the selected 
					        objects can move, with the desired x and y 
					        as in-parameters.
	        Access :		Protected

	        Return :		void
	        Parameters :	double& x	-	Desired x-movement
					        double& y	-	Desired y-movement.
        					
	        Usage :			Internal function. Will not actually move 
					        the objects, only calculate the maximum 
					        possible movement.

           ============================================================*/
        {
            if (this.Restraints != RESTRAIN.RESTRAINT_NONE)
	        {

		        int count = 0;
		        DiagramEntity obj;
		        double minleft = 0xffffffff;
		        double mintop = 0xffffffff;
		        double maxright = 0;
		        double maxbottom = 0;

		        while( ( obj = m_objs.GetAt( count++ ) )!= null )
		        {
			        if( obj.Selected )
			        {

				        // Correcting, depending on restraint mode.
				        // Note that checks will have to be made for all 
				        // coordinates against all sides, as the coordinates 
				        // might not be normalized (as for a line, for example).

				        double left = obj.Bounds.Left + x;
                        double top = obj.Bounds.Top + y;
                        double right = obj.Bounds.Right + x;
                        double bottom = obj.Bounds.Bottom + y;

				        minleft = Math.Min( minleft, left );
				        minleft = Math.Min( minleft, right );
				        mintop = Math.Min( mintop, top );
				        mintop = Math.Min( mintop, bottom );

				        maxright = Math.Max( maxright, left );
				        maxright = Math.Max( maxright, right );
				        maxbottom = Math.Max( maxbottom, top );
				        maxbottom = Math.Max( maxbottom, bottom );


			        }
		        }

		        double leftedge = 0;
		        double rightedge = 0;
		        double topedge = 0;
		        double bottomedge = 0;

                if (this.Restraints == RESTRAIN.RESTRAINT_VIRTUAL)
		        {
			        leftedge = 0;
			        topedge = 0;
                    rightedge = this.VirtualSize.Width;
                    bottomedge = this.VirtualSize.Height;
		        }
                else if (this.Restraints == RESTRAIN.RESTRAINT_MARGIN)
		        {
                    Padding padding = this.VirtualScreenMargin;

                    leftedge = padding.Left+1;
                    topedge = padding.Top+1;
                    rightedge = this.VirtualSize.Width - padding.Right-1;
                    bottomedge = this.VirtualSize.Height - padding.Bottom-1;
		        }

		        if( minleft < leftedge )
			        x = x - ( minleft - leftedge );
		        if( mintop < topedge )
			        y = y - ( mintop - topedge );
		        if( maxright > rightedge )
			        x = rightedge - ( maxright - x );
		        if( maxbottom > bottomedge )
			        y = bottomedge - ( maxbottom - y );

	        }
        }

        protected void AdjustForRestraints( ref double left,ref  double top,ref double right,ref double bottom )
        /* ============================================================
	        Function :		AdjustForRestraints
	        Description :	Adjust the rect represented by the in-
					        params to the current constraints.
	        Access :		Protected

	        Return :		void
	        Parameters :	double& left	-	Desired left position. 
										        Contains the corrected 
										        left position on return.
					        double& top		-	Desired top position.
										        Contains the corrected 
										        top position on return.
					        double& right	-	Desired right position.
										        Contains the corrected 
										        right position on return.
					        double& bottom	-	Desired bottom position.
										        Contains the corrected 
										        bottom position on return.
        					
	        Usage :			Internal function. Will not resize the rect. 
					        The coordinates need not be normalized.

           ============================================================*/
        {

	        // Saving the size
	        double width =Math.Abs( right - left );
	        double height = Math.Abs( bottom - top );

	        // Correcting, depending on restraint mode.
	        // Note that checks will have to be made for all 
	        // coordinates against all sides, as the coordinates 
	        // might not be normalized (as for a line, for example).
            if (this.Restraints == RESTRAIN.RESTRAINT_VIRTUAL)
	        {

		        if( left < 0 )
		        {
			        left = 0;
			        right = left + width;
		        }
		        if( top < 0 )
		        {
			        top = 0;
			        bottom = top + height;
		        }
		        if( right < 0 )
		        {
			        right = 0;
			        left = right + width;
		        }
		        if( bottom < 0 )
		        {
			        bottom = 0;
			        top = bottom + height;
		        }
                Size virtualSize = this.VirtualSize;
                if (right > virtualSize.Width)
		        {
                    right = virtualSize.Width;
			        left = right - width;
		        }
                if (bottom > virtualSize.Height)
		        {
                    bottom = virtualSize.Height;
			        top = bottom - height;
		        }

                if (left > virtualSize.Width)
		        {
                    left = virtualSize.Width;
			        right = left - width;
		        }
                if (top > virtualSize.Height)
		        {
                    top = virtualSize.Height;
			        bottom = top - height;
		        }
	        }
            else if (this.Restraints == RESTRAIN.RESTRAINT_MARGIN)
	        {
                Padding padding = this.VirtualScreenMargin;
                int marginMinLeft = padding.Left + 1;
                int marginMinTop = padding.Top + 1;

                if (left < marginMinLeft)
		        {
                    left = marginMinLeft;
			        right = left + width;
		        }
                if (top < marginMinTop)
		        {
                    top = marginMinTop;
			        bottom = top + height;
		        }
                if (right < marginMinLeft)
		        {
                    right = marginMinLeft;
			        left = right + width;
		        }
                if (bottom < marginMinTop)// m_topMargin )
		        {
                    bottom = marginMinTop;// m_topMargin;
			        top = bottom + height;
		        }
                Size virtualSize = this.VirtualSize;
                int marginMaxRight = virtualSize.Width - padding.Right - 1;
                int marginMaxBottom = virtualSize.Height - padding.Bottom - 1;
                if (right > marginMaxRight)// virtualSize.Width - padding.Right)
		        {
                    right = marginMaxRight;// (virtualSize.Width - padding.Right);// m_rightMargin );
			        left = right - width;
		        }
                if (bottom >marginMaxBottom)// virtualSize.Height - padding.Bottom)// m_bottomMargin )
		        {
                    bottom = marginMaxBottom;// (virtualSize.Height - padding.Bottom);// m_bottomMargin );
			        top = bottom - height;
		        }
                if (left >marginMaxRight)// virtualSize.Width - padding.Right)// m_rightMargin )
		        {
                    left = marginMaxRight;// (virtualSize.Width - padding.Right);// m_rightMargin );
			        right = left - width;
		        }
                if (top >marginMaxBottom)// virtualSize.Height - padding.Bottom)// m_bottomMargin )
		        {
                    top = marginMaxBottom;// (virtualSize.Height - padding.Bottom);// m_bottomMargin );
			        bottom = top - height;
		        }
	        }

        }

       protected void AdjustForRestraints(ref double xpos,ref  double ypos )
        /* ============================================================
	        Function :		AdjustForRestraints
	        Description :	Adjust the rect represented by the in-
					        params to the current constraints.
	        Access :		Protected

	        Return :		void
	        Parameters :	double& xpos	-	Desired x position. 
										        Contains the corrected 
										        x position on return.
					        double& ypos	-	Desired y position. 
										        Contains the corrected 
										        y position on return.
        					
	        Usage :			Internal function. Will only correct 
					        position parameters, not change any object.

           ============================================================*/
        {

            Size virtualSize = this.VirtualSize;
            if (this.Restraints == RESTRAIN.RESTRAINT_VIRTUAL)
	        {
		        xpos = Math.Max( xpos, 0 );
                xpos = Math.Min(xpos, virtualSize.Width);
		        ypos = Math.Max( ypos, 0 );
                ypos = Math.Min(ypos, virtualSize.Height);
	        }
            else if (this.Restraints == RESTRAIN.RESTRAINT_MARGIN)
	        {
                Padding padding = this.VirtualScreenMargin;

		        xpos = Math.Max( xpos, padding.Left+1);// m_leftMargin );
                xpos = Math.Min(xpos, virtualSize.Width - padding.Right-1);
		        ypos = Math.Max( ypos, padding.Top+1);
                ypos = Math.Min(ypos, virtualSize.Height - padding.Bottom-1);
	        }

        }

        protected Size GetContainingSize() 
        /* ============================================================
	        Function :		GetContainingSize
	        Description :	Return the minimum size enveloping all 
					        objects.
	        Access :		Protected

	        Return :		Size	-	Minimum size necessary to 
								        contain all objects.
	        Parameters :	none

	        Usage :			Internal function. Calculates the minimum
					        size necessary for all objects.

           ============================================================*/
        {

	        int count = 0;
	        DiagramEntity obj;
	        int maxx = 0;
	        int maxy = 0;

	        if( m_objs != null)
	        {
		        while( ( obj = m_objs.GetAt( count++ ) )!= null )
		        {
                    maxx = (int)Math.Max(decimal.Round((decimal)obj.Bounds.Right), (decimal)maxx);
                    maxy = (int)Math.Max(decimal.Round((decimal)obj.Bounds.Bottom), (decimal)maxy);
		        }
	        }

	        return new Size( maxx, maxy );

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor scrolling
        protected override void  OnScroll(ScrollEventArgs se)
        {
 	        
            if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                OnHScroll(se);
            if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
                OnVScroll(se);
           base.OnScroll(se); 
            this.RedrawWindow();
        }

       protected  void OnHScroll( ScrollEventArgs se ) 
        /* ============================================================
	        Function :		OnHScroll
	        Description :	Handles the "WM_VSCROLL" message. Updates the 
					        screen.
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nSBCode			-	Type of scroll operation
					        UINT nPos				-	New scroll position
					        CScrollBar* pScrollBar	-	
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/
        {
           /*
	        //SCROLLINFO si;
	        //si.cbSize = sizeof( SCROLLINFO );
	        //si.fMask = SIF_POS | SIF_RANGE;
	        //GetScrollInfo( SB_HORZ, &si );
            ScrollEventType nSBCode = se.Type;
            Point pt = this.AutoScrollPosition;
	        switch( nSBCode )
	        {
		        case ScrollEventType.First:// SB_LEFT:			// Scroll to far left.
			        //si.nPos = si.nMin;
			        break;
                //case SB_LINELEFT:		// Scroll left.
                //    si.nPos = Math.Max( si.nPos - 1, si.nMin );
                //    break;
                //case SB_LINERIGHT:		// Scroll right.
                //    si.nPos = Math.Min( si.nPos + 1, si.nMax );
                //    break;
                //case SB_PAGELEFT:		// Scroll one page left.
                //    si.nPos = Math.Max( si.nPos - < int >( si.nPage ), si.nMin );
                //    break;
                //case SB_PAGERIGHT:		// Scroll one page right.
                //    si.nPos = Math.Min( si.nPos + < int >( si.nPage ), si.nMax );
                //    break;
                case ScrollEventType.Last:// SB_RIGHT:			// Scroll to far right.
                    //si.nPos = si.nMax;
                    //pt.X = this.au
                    break;
                case ScrollEventType.ThumbPosition:// SB_THUMBPOSITION:	// Scroll to absolute position. The current position is specified by the nPos parameter.
                    //si.nPos = nPos;
                    pt.X = se.NewValue;
                    break;
                case ScrollEventType.ThumbTrack:// SB_THUMBTRACK:		//
                    pt.X = se.NewValue;
                    //si.nPos = nPos;
                    break;
                  default:
                    break;
	        }
           this.AutoScrollPosition = pt;
	        SetHScroll( si.nPos );
        //	SetScrollInfo( SB_HORZ, &si );
	        RedrawWindow();
	        CWnd::OnHScroll( nSBCode, nPos, pScrollBar );
      */     
        }

       protected  void OnVScroll( ScrollEventArgs se ) 
        /* ============================================================
	        Function :		OnVScroll
	        Description :	Handles the "WM_VSCROLL" message. Updates the 
					        screen.
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nSBCode			-	Type of scroll 
												        operation.
					        UINT nPos				-	New scroll position.
					        CScrollBar* pScrollBar	-	
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/
        {
            /*
	        SCROLLINFO si;
	        si.cbSize = sizeof( SCROLLINFO );
	        si.fMask = SIF_POS | SIF_RANGE;
	        GetScrollInfo( SB_VERT, &si );
	        switch( nSBCode )
	        {
		        case SB_TOP:			// Scroll to far left.
			        si.nPos = si.nMin;
			        break;
		        case SB_LINEUP:			// Scroll left.
			        si.nPos = Math.Max( si.nPos - 1, si.nMin );
			        break;
		        case SB_LINEDOWN:		// Scroll right.
			        si.nPos = Math.Min( si.nPos + 1, si.nMax );
			        break;
		        case SB_PAGEUP:			// Scroll one page left.
			        si.nPos = Math.Max( si.nPos - < int >( si.nPage ), si.nMin );
			        break;
		        case SB_PAGEDOWN:		// Scroll one page right.
			        si.nPos = Math.Min( si.nPos + < int >( si.nPage ), si.nMax );
			        break;
		        case SB_BOTTOM:			// Scroll to far right.
			        si.nPos = si.nMax;
			        break;
		        case SB_THUMBPOSITION:	// Scroll to absolute position. The current position is specified by the nPos parameter.
			        si.nPos = nPos;
			        break;
		        case SB_THUMBTRACK:		//
			        si.nPos = nPos;
			        break;
	        }

	        SetVScroll( si.nPos );

        //	SetScrollInfo( SB_VERT, &si );
	        RedrawWindow();

	        CWnd::OnVScroll( nSBCode, nPos, pScrollBar );
            */
        }

       protected  int HScrollEditor( int scroll )
       /* ============================================================
           Function :		HScrollEditor
           Description :	Scrolls the editor window scroll steps
                           horisontally.
           Access :		Protected

           Return :		int			-	The number of steps actually 
                                           scrolled.
           Parameters :	int scroll	-	The number of steps to scroll.
        					
           Usage :			Internal function

          ============================================================*/
       {
            int retval = 0;
            //int desiredpos = this.AutoScrollPosition.X + scroll;;
            //this.AutoScrollPosition.X = desiredpos;
            //retval = scroll - ( desiredpos - this.AutoScrollPosition.X );
            // if( retval >0)
            //    RedrawWindow();

            //return retval;
	       
            
      
        
                Rectangle clientRect;
                clientRect = this.ClientRectangle;

            
                if(clientRect.Width < this.VirtualSize.Width )	
                {
                    int desiredpos = 0;
                    
        //            SCROLLINFO si;
        //            si.cbSize = sizeof( SCROLLINFO );
        //            if( GetScrollInfo( SB_HORZ, &si ) )
        //            {
                        desiredpos = Math.Abs(  this.AutoScrollPosition.X) + scroll;
                        Point pt = this.AutoScrollPosition;
                        pt.X = desiredpos;
                    this.AutoScrollPosition =pt;//.X = desiredpos;
                    retval = scroll - ( desiredpos - this.AutoScrollPosition.X );
        //                si.nPos = desiredpos;

        //                SetHScroll( si.nPos );
        ////				SetScrollInfo( SB_HORZ, &si );
        //                GetScrollInfo( SB_HORZ, &si );
        //                retval = scroll - ( desiredpos - si.nPos );
                      if( retval >0)
                            RedrawWindow();
        //            }
                }
        

	        return retval;

        }

       protected  int VScrollEditor( int scroll )
       /* ============================================================
           Function :		VScrollEditor
           Description :	Scrolls the editor window scroll steps
                           vertically.
           Access :		Protected

           Return :		int			-	The number of steps actually 
                                           scrolled.
           Parameters :	int scroll	-	The number of steps to scroll.
        					
           Usage :			Internal function.

          ============================================================*/
       {

	        int retval = 0;

	        //if( m_hWnd )
	        {
		        Rectangle clientRect = this.ClientRectangle;
		        //GetClientRect( &clientRect );
		        if(clientRect.Height < this.VirtualSize.Height )
		        {
			        int desiredpos = 0;

			        //SCROLLINFO si;
			        //si.cbSize = sizeof( SCROLLINFO );
			        //if( GetScrollInfo( SB_VERT, &si ) )
			        {
				        desiredpos =Math.Abs( this.AutoScrollPosition.Y) + scroll;
                        Point pt = this.AutoScrollPosition;
                        pt.Y = desiredpos;
                        this.AutoScrollPosition = pt;//.Y = desiredpos;
				        //si.nPos = desiredpos;

				        //SetVScroll( si.nPos );
        
				        //GetScrollInfo( SB_VERT, &si );
				        retval = scroll - ( desiredpos - this.AutoScrollPosition.Y );
				        if( retval >0)
					        RedrawWindow();
			        }
		        }
	        }

	        return retval;

        }

       protected  Point ScrollPoint( Point point )
        /* ============================================================
	        Function :		ScrollPoint
	        Description :	Scrolls the editor if point is outside the 
					        window.
	        Access :		Protected

	        Return :		Point			-	The pixels scrolled 
										        horisontally and 
										        vertically.
	        Parameters :	Point point	-	The position of the mouse 
										        pointer.
        					
	        Usage :			Internal function.

           ============================================================*/
        {

	        Point desiredPT = new Point( 0, 0 );
	        Rectangle clientRect = this.ClientRectangle;
	        

	        if( point.X > clientRect.Right )
                desiredPT.X = HScrollEditor(10);

	        if( point.X < 0 )
                desiredPT.X = HScrollEditor(-10);

	        if( point.Y > clientRect.Bottom )
                desiredPT.Y = VScrollEditor(10);

	        if( point.Y < 0 )
                desiredPT.Y = VScrollEditor(-10);

	        return desiredPT;

        }

       protected  void SetupScrollbars()
        /* ============================================================
	        Function :		SetupScrollbars
	        Description :	Sets up/removes scrollbars depending on the 
					        relation between the client- and virtual 
					        rect.
	        Access :		Protected

	        Return :		void
	        Parameters :	none

	        Usage :			Internal function.

           ============================================================*/
        {
            Size virtualSize = this.VirtualSize;
            decimal width = decimal.Round((decimal)((decimal)virtualSize.Width) * (decimal)this.Zoom);
            decimal height = decimal.Round((decimal)((decimal)virtualSize.Height) * (decimal)this.Zoom);


            this.AutoScrollMinSize = new Size((int)width, (int )height);

            //if( m_hWnd )
            //{
            //    Rectangle rect;
            //    GetClientRect( rect );

            //    DWORD add = 0;
            //    DWORD remove = 0;

            //    if( decimal.Round( < double >( GetVirtualSize().Width ) * GetZoom() ) <= rect.right )
            //    {
            //        if( GetStyle() & WS_HSCROLL )
            //            remove |= WS_HSCROLL;
            //    }
            //    else
            //    {
            //        if( !( GetStyle() & WS_HSCROLL ) )
            //            add |= WS_HSCROLL;
            //    }
            //    if( decimal.Round( < double >( GetVirtualSize().Height ) * GetZoom() ) <= rect.bottom )
            //    {
            //        if( GetStyle() & WS_VSCROLL )
            //            remove |= WS_VSCROLL;
            //    }
            //    else
            //    {
            //        if( !( GetStyle() & WS_VSCROLL ) )
            //            add |= WS_VSCROLL;
            //    }

            //    SCROLLINFO si;
            //    si.cbSize = sizeof( SCROLLINFO );
            //    si.fMask = SIF_RANGE | SIF_PAGE;
            //    si.nMin = 0;
            //    int width = decimal.Round( < double >( GetVirtualSize().Width ) * GetZoom() );
            //    if( width > rect.right )
            //    {
            //        si.nMax = width;
            //        si.nPage = rect.right;
            //    }
            //    else
            //    {
            //        si.nMax = 0;
            //        si.nPage = 0;
            //    }

            //    SetScrollInfo( SB_HORZ, &si );
        		
            //    int height = decimal.Round( < double >( GetVirtualSize().Height ) * GetZoom() );
            //    if( height > rect.bottom )
            //    {
            //        si.nMax = height;
            //        si.nPage = rect.bottom;
            //    }
            //    else
            //    {
            //        si.nMax = 0;
            //        si.nPage = 0;
            //    }

            //    SetScrollInfo( SB_VERT, &si );

            //    if( add || remove )
            //        ModifyStyle( remove, add, SWP_FRAMECHANGED );

            //}

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor sizing
        protected override void  OnSizeChanged(EventArgs e)
        {
 	         base.OnSizeChanged(e);
             SetupScrollbars();
        }
        //void OnSize( UINT nType, int cx, int cy ) 
        ///* ============================================================
        //    Function :		OnSize
        //    Description :	Handles the "WM_SIZE" message. Sets up/modifies 
        //                    scrollbars as necessary.
        //    Access :		Protected

        //    Return :		void
        //    Parameters :	UINT nType	-	
        //                    int cx		-	The new x size
        //                    int cy		-	The new y size
        					
        //    Usage :			Called from MFC. Do not call from code.

        //   ============================================================*/
        //{

        //    CWnd::OnSize( nType, cx, cy );
        //    SetupScrollbars();

        //}

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor coordinate handling

        public void ClientToVirtual(ref  Rectangle rect ) 
        /* ============================================================
	        Function :		ScreenToVirtual
	        Description :	Converts rect from screen- to virtual 
					        coordinates.
	        Access :		Public
        					
	        Return :		void
	        Parameters :	Rectangle& rect	-	The rect to convert
        					
	        Usage :			Call to - for example - display coordinates,

           ============================================================*/
        {

	        // We have to normalize, add scroll bar positions and 
	        // apply current zoom.

	        rect  = NormalizeRect(rect);

            //SCROLLINFO sih;
            //SCROLLINFO siv;

            //sih.cbSize = sizeof( SCROLLINFO );
            //sih.fMask = SIF_POS;
            //siv.cbSize = sizeof( SCROLLINFO );
            //siv.fMask = SIF_POS;

            //if( !::GetScrollInfo( m_hWnd, SB_HORZ, &sih ) )
            //    sih.nPos = 0;
            //if( !::GetScrollInfo( m_hWnd, SB_VERT, &siv ) )
            //    siv.nPos = 0;
            Point pt = this.AutoScrollPosition;
            pt.X = Math.Abs(pt.X);
            pt.Y = Math.Abs(pt.Y);
            rect = new Rectangle((int)decimal.Round((decimal)(rect.Left + pt.X) / (decimal)this.Zoom),
                          (int)decimal.Round((decimal)(rect.Top + pt.Y) / (decimal)this.Zoom),
                          (int)decimal.Round((decimal)(rect.Width) / (decimal)this.Zoom),//(int)decimal.Round((decimal)(rect.Right + pt.Y) / (decimal)GetZoom()),
                          (int)decimal.Round((decimal)(rect.Height) / (decimal)this.Zoom));//(int)decimal.Round((decimal)(rect.Bottom + pt.X) / (decimal)GetZoom()));

        }

        public void ClientToVirtual(ref Point point) 
        /* ============================================================
	        Function :		ScreenToVirtual
	        Description :	Converts point from screen- to virtual 
					        coordinates.
	        Access :		Public

	        Return :		void
	        Parameters :	Point& point	-	The point to convert.	
        					
	        Usage :			Call to - for example - display coordinates,

           ============================================================*/
        {

	        // Convert point to the virtual
	        // coordinate system. We have to normalize, 
	        // add scroll bar positions and apply current 
	        // scale.

            //SCROLLINFO sih;
            //SCROLLINFO siv;

            //sih.cbSize = sizeof( SCROLLINFO );
            //sih.fMask = SIF_POS;
            //siv.cbSize = sizeof( SCROLLINFO );
            //siv.fMask = SIF_POS;

            //if( !::GetScrollInfo( m_hWnd, SB_HORZ, &sih ) )
            //    sih.nPos = 0;
            //if( !::GetScrollInfo( m_hWnd, SB_VERT, &siv ) )
            //    siv.nPos = 0;
            Point pt = this.AutoScrollPosition;
            pt.X = Math.Abs(pt.X);
            pt.Y = Math.Abs(pt.Y);
            point.X = (int)decimal.Round((decimal)(point.X + pt.X) / (decimal)this.Zoom);
            point.Y = (int)decimal.Round((decimal)(point.Y + pt.Y) / (decimal)this.Zoom);

        }

        public void ClientToVirtual(ref Size size) 
        /* ============================================================
	        Function :		ScreenToVirtual
	        Description :	Converts size from screen- to virtual 
					        coordinates.
	        Access :		Public

	        Return :		void
	        Parameters :	Size& size	-	The size to convert.
        					
	        Usage :			Call to - for example - display coordinates,

           ============================================================*/
        {

	        // We have to deduct scroll bar positions 
	        // and apply current zoom.


            //SCROLLINFO sih;
            //SCROLLINFO siv;

            //sih.cbSize = sizeof( SCROLLINFO );
            //sih.fMask = SIF_POS;
            //siv.cbSize = sizeof( SCROLLINFO );
            //siv.fMask = SIF_POS;

            //if( !::GetScrollInfo( m_hWnd, SB_HORZ, &sih ) )
            //    sih.nPos = 0;
            //if( !::GetScrollInfo( m_hWnd, SB_VERT, &siv ) )
            //    siv.nPos = 0;
            Point pt = this.AutoScrollPosition;
            pt.X = Math.Abs(pt.X);
            pt.Y = Math.Abs(pt.Y);
            //David: I feel here there is a bug.
            //size.Width = (int)decimal.Round((decimal)(size.Width + pt.X) / (decimal)GetZoom());
            //size.Height = (int)decimal.Round((decimal)(size.Height + pt.Y) / (decimal)GetZoom());
            size.Width = (int)decimal.Round((decimal)(size.Width) / (decimal)this.Zoom);
            size.Height = (int)decimal.Round((decimal)(size.Height) / (decimal)this.Zoom);

        }

        public void VirtualToClient(ref Rectangle rect) 
        /* ============================================================
	        Function :		VirtualToScreen
	        Description :	Converts rect from the virtual to the 
					        screen coordinate system. 
	        Access :		Public

	        Return :		void
	        Parameters :	Rectangle& rect	-	The rect to convert. Will be 
									        normalized on return.
        					
	        Usage :			Call to - for example - display coordinates,

           ============================================================*/
        {

	        // We have to normalize, deduct scroll bar positions 
	        // and apply current zoom.

            rect = NormalizeRect(rect);

            //SCROLLINFO sih;
            //SCROLLINFO siv;

            //sih.cbSize = sizeof( SCROLLINFO );
            //sih.fMask = SIF_POS;
            //siv.cbSize = sizeof( SCROLLINFO );
            //siv.fMask = SIF_POS;

            //if( !::GetScrollInfo( m_hWnd, SB_HORZ, &sih ) )
            //    sih.nPos = 0;
            //if( !::GetScrollInfo( m_hWnd, SB_VERT, &siv ) )
            //    siv.nPos = 0;
            Point pt = this.AutoScrollPosition;
            rect = new Rectangle((int)decimal.Round((decimal)(rect.Left) * (decimal)this.Zoom - Math.Abs((decimal)(pt.X))),
                          (int)decimal.Round((decimal)(rect.Top) * (decimal)this.Zoom - Math.Abs((decimal)(pt.Y))),
                          (int)decimal.Round((decimal)(rect.Width) * (decimal)this.Zoom),//(int)decimal.Round((decimal)(rect.Right) * (decimal)GetZoom() - (decimal)(pt.X)),
                          (int)decimal.Round((decimal)(rect.Height) * (decimal)this.Zoom));//(int)decimal.Round((decimal)(rect.Bottom) * (decimal)GetZoom() - (decimal)(pt.Y)));

        }

        public void VirtualToClient(ref Point point ) 
        /* ============================================================
	        Function :		VirtualToScreen
	        Description :	Converts point from the virtual to the 
					        screen coordinate system. 
	        Access :		Public

	        Return :		void
	        Parameters :	Point& point	-	The point to convert. 
        					
	        Usage :			Call to - for example - display coordinates,

           ============================================================*/
        {
            Point pt = this.AutoScrollPosition;
            point.X = (int)decimal.Round((decimal)(point.X) * (decimal)this.Zoom) - Math.Abs(pt.X);
            point.Y = (int)decimal.Round((decimal)(point.Y) * (decimal)this.Zoom) - Math.Abs(pt.Y);


        }

       protected int SnapX( int coord ) 
        /* ============================================================
	        Function :		SnapX
	        Description :	Snaps coord vertically to the closest 
					        grid point.
	        Access :		Protected

	        Return :		int			-	The resulting x-coordinate.
	        Parameters :	int coord	-	The coordinate to snap
        					
	        Usage :			Internal function. Will snap even if snap 
					        to grid is off.

           ============================================================*/
        {

	        decimal x = ( decimal )( coord ) ;
	        decimal gridx = ( decimal )( this.GridSize.Width );

            //int index = (int)decimal.Round(x / gridx) / 2;
            //if ((int)decimal.Round(x / gridx) % 2 == 0)
            //{
            //    return DEditor.m_lstX[index];
            //}
            //else
                return (int)decimal.Round( x / gridx ) * this.GridSize.Width ;


        }

        protected int SnapY( int coord ) 
        /* ============================================================
	        Function :		SnapY
	        Description :	Snaps coord horisontally to the closest 
					        grid point.
	        Access :		Protected

	        Return :		int			-	The resulting y-coordinate.
	        Parameters :	int coord	-	The coordinate to snap
        					
	        Usage :			Internal function. Will snap even if snap 
					        to grid is off.

           ============================================================*/
        {

	        decimal y = ( decimal )( coord );
	        decimal gridy = ( decimal )(this.GridSize.Height);// m_gridSize.Height );
            return (int)decimal.Round(y / gridy) * this.GridSize.Height;// m_gridSize.Height;

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor object manipulations

        public bool IsAnyObjectSelected() 
        /* ============================================================
	        Function :		IsAnyObjectSelected
	        Description :	Returns "true" if any object is selected.
	        Access :		Public

	        Return :		bool	-	"true" if any object is selected.
	        Parameters :	none

	        Usage :			Call to check if any objects are selected.

           ============================================================*/
        {

	        bool res = false;
	        if( m_objs != null )
	        {
		        int count = 0;
		        DiagramEntity obj;
                while ((obj = m_objs.GetAt(count++)) != null)
			        if( obj.Selected )
				        res = true;
	        }

	        return res;

        }

        public int GetUndoCount()
        {
            return this.m_objs.UndoStack.Count;
        }
        public int GetCopyCount()
        {
            return this.m_objs.ObjectsInPaste();
        }

        public int GetSelectCount() 
        /* ============================================================
	        Function :		GetSelectCount
	        Description :	Returns the number of selected objects in 
					        the data container.
	        Access :		Public

	        Return :		int	-	The number of selected objects.
	        Parameters :	none

	        Usage :			Call to get the number of selected objects.

           ============================================================*/
        {

	        int res = 0;

	        if( m_objs != null)
		        res = m_objs.GetSelectCount();

	        return res;

        }

        public DiagramEntity GetSelectedObject() 
        /* ============================================================
	        Function :		GetSelectedObject
	        Description :	Returns the top selected object in the 
					        container.
	        Access :		Public

	        Return :		DiagramEntity	-	The top selected object, 
										        or "null" if none.
	        Parameters :	none

	        Usage :			Call to get the currently selected object. 
					        Note that this function will return a single 
					        object (top in the z-order) even if several 
					        are selected.

           ============================================================*/
        {

	        int count = 0;
	        DiagramEntity retval = null;
	        DiagramEntity obj;

            if (m_objs != null)
                while ((obj = m_objs.GetAt(count++)) != null)
			        if( obj.Selected )
				        retval = obj;

	        return retval;

        }

        public int GetSelectedObjects(List<DiagramEntity> ar)
        /* ============================================================
            Function :		GetSelectedObject
            Description :	Returns the top selected object in the 
                            container.
            Access :		Public

            Return :		DiagramEntity	-	The top selected object, 
                                                or "null" if none.
            Parameters :	none

            Usage :			Call to get the currently selected object. 
                            Note that this function will return a single 
                            object (top in the z-order) even if several 
                            are selected.

           ============================================================*/
        {

            int count = 0;
            
            DiagramEntity obj;

            if (m_objs != null)
                while ((obj = m_objs.GetAt(count++)) != null)
                    if (obj.Selected)
                    {
                        ar.Add((DiagramEntity)obj);
                        
                    }

            return ar.Count;

        }

       public  int GetObjectCount() 
        /* ============================================================
	        Function :		GetObjectCount
	        Description :	Returns the number of objects in the container.
	        Access :		Public

	        Return :		int	-	The number of objects.
	        Parameters :	none

	        Usage :			Call to get the number of objects in the 
					        data container.

           ============================================================*/
        {

	        int size = 0;
            if (m_objs != null)
		        size = m_objs.GetSize();

	        return size;

        }

       public  void AddObject( DiagramEntity obj ) 
        /* ============================================================
	        Function :		AddObject
	        Description :	Adds "obj" to the data container.
	        Access :		Public

	        Return :		void
	        Parameters :	DiagramEntity obj	-	The object to add.
        					
	        Usage :			Called to add objects to the container after 
					        - for example - a load operation. Note that 
					        the modified flag must be set manually if 
					        this is desired (if implementing an Append 
					        command, for example).
					        This function can be overridden to trap
					        object additions.

           ============================================================*/
        {

            if (m_objs != null)
		        m_objs.Add( obj );

        }

        public DiagramEntity GetObject( int index )  
        /* ============================================================
	        Function :		GetObject
	        Description :	Gets the object at index "index".
	        Access :		Public

	        Return :		DiagramEntity	-	The object, "null" if not 
										        found.
	        Parameters :	int index		-	The index to get the 
										        object from
        					
	        Usage :			Call to get a specific object from the 
					        container. 
					        The objects are the responsibility of the 
					        container and should not be deleted.

           ============================================================*/
        {

	        DiagramEntity obj = null;
            if (m_objs != null)
		        obj = m_objs.GetAt( index );

	        return obj;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>true: passed , false: error</returns>
        public bool check_max_entities(bool bWillAddNew)
        {
            if (bWillAddNew)
            {
                if (GetEntitiesCount() >= MAX_ENTITIES_COUN)
                {
                    string s = "Only 64 keys are allowed in the template.";
                    MessageBox.Show(s, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else
            {
                if (GetEntitiesCount() > MAX_ENTITIES_COUN)
                {
                    frmEntities64 frm = new frmEntities64(bWillAddNew);

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (frm.FixType == frmEntities64.Fix_Type.Auto)
                        {
                            this.Undo();
                        }

                    }
                    return false;
                }
            }
            return true;

            //if (GetEntitiesCount() > MAX_ENTITIES_COUN)
            //{
            //    // string s = "Only 64 keys are allowed in the template.\n There are more than 64 keys in the template!";
            //    // MessageBox.Show(s, "Message");
            //    string s = "Only 64 keys are allowed in the template.";
            //    if (bWillAddNew)
            //    {
            //        MessageBox.Show(s, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else
            //    {


            //        frmEntities64 frm = new frmEntities64(bWillAddNew);

            //        if (frm.ShowDialog() == DialogResult.OK)
            //        {
            //                if (frm.FixType == frmEntities64.Fix_Type.Auto)
            //                {
            //                    this.Undo();
            //                }

            //        }
            //    }
            //    return false;
            //    //m_interactMode = MouseMode.MODE_NONE;
            //    // return;
            //}
            //return true;
        }
        /* ============================================================
            Function :		StartDrawingObject
            Description :	Starts drawing an object of type "obj".
            Access :		Public

            Return :		void
            Parameters :	DiagramEntity obj	-	Pointer to an object 
                                                    of the desired type.

            Usage :			Should be called with a pointer to the 
                            appropriate object type when the user 
                            selects to draw an object. obj becomes 
                            the property of the editor and should not 
                            be deleted.

           ============================================================*/
        private DiagramEntity _DrawingObject = null;
        [Browsable(false)]
        public DiagramEntity DrawingObject
        {
            get { return _DrawingObject; }
            set 
            {
                _DrawingObject = value;
                if (!check_max_entities(true))
                    _DrawingObject = null;
                //if (GetEntitiesCount() >= MAX_ENTITIES_COUN)
                //{
                //   // string s = "Only 64 keys are allowed in the template.\n There are more than 64 keys in the template!";
                //   // MessageBox.Show(s, "Message");
                //    frmEntities64 frm = new frmEntities64();
                //    if (frm.ShowDialog() == DialogResult.OK)
                //    {
                //        if (frm.FixType == frmEntities64.Fix_Type.Auto)
                //        {
                //            this.Undo();
                //        }
                //    }
                //    _DrawingObject = null;
                //    //m_interactMode = MouseMode.MODE_NONE;
                //   // return;
                //}    

                if (_DrawingObject != null)
                {
                    //m_drawing = true;
                    _IsDrawingObj = true;
                    m_interactMode = MouseMode.MODE_DRAWING;
                    this.Cursor = Cursors.Cross;
                    
                }
                else
                {
                    //m_drawing = false;
                    _IsDrawingObj = false;
                    m_interactMode = MouseMode.MODE_NONE;
                    
                    this.Cursor = Cursors.Arrow;
                }

                
            }
        }

        //public void StartDrawingObject( DiagramEntity obj )
        ///* ============================================================
        //    Function :		StartDrawingObject
        //    Description :	Starts drawing an object of type "obj".
        //    Access :		Public

        //    Return :		void
        //    Parameters :	DiagramEntity obj	-	Pointer to an object 
        //                                            of the desired type.
        					
        //    Usage :			Should be called with a pointer to the 
        //                    appropriate object type when the user 
        //                    selects to draw an object. obj becomes 
        //                    the property of the editor and should not 
        //                    be deleted.

        //   ============================================================*/
        //{

        //    m_drawObj = null;
        //    m_drawObj = obj;

        //    if( obj != null)
        //    {
        //        //m_drawing = true;
        //        _IsDrawingObj = true;
        //        m_interactMode = MouseMode.MODE_DRAWING;
        //        this.Cursor = Cursors.Cross;
        //        //::SetCursor( LoadCursor( null, IDC_CROSS ) );
        //    }
        //    else
        //    {
        //        //m_drawing = false;
        //        _IsDrawingObj = false;
        //        m_interactMode = MouseMode. MODE_NONE;
        //        //::SetCursor( LoadCursor( null, IDC_ARROW ) );
        //        this.Cursor = Cursors.Arrow;
        //    }

        //}

       public  bool IsDrawing() 
        /* ============================================================
	        Function :		IsDrawing
	        Description :	Returns if the user is drawing or not
	        Access :		Public

	        Return :		bool	-	"true" if the user is currently 
								        drawing
	        Parameters :	none

	        Usage :			Called for command enabling if multi-draw
					        mode is enabled.

           ============================================================*/
        {
	        return ( this.DrawingObject != null) && this.IsDrawingObj;
        }

        public void UnselectAll()
        /* ============================================================
	        Function :		UnselectAll
	        Description :	Unselects all objects in the container.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to unselect all objects. 
					        Should not be callable if "GetObjectCount() 
					        == 0" (there are no objects in the container).

           ============================================================*/
        {

	        if( m_objs !=null )
	        {
		        int count = 0;
		        DiagramEntity obj;
		        while( ( obj = m_objs.GetAt( count++ ) ) != null )
			        obj.Selected = ( false );
	        }

        }

       public  void SelectAll()
        /* ============================================================
	        Function :		SelectAll
	        Description :	Selects all objects.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to select all objects. 
					        Should not be callable if "GetObjectCount() 
					        == 0" (there are no objects in the container).

           ============================================================*/
        {

	        if( m_objs != null)
	        {
		        int count = 0;
		        DiagramEntity obj = null;
		        while( ( obj = m_objs.GetAt( count++ ) )!= null )
			        obj.Selected = ( true );
                m_objs.SetMultipleSelectedMain(null);
               // FireShowPropertiesEvent(this.GetSelectedObject());
		        RedrawWindow();
	        }

        }

        public void Unselect()
        {
            if (m_objs != null)
            {
                int count = 0;
                DiagramEntity obj = null;
                while ((obj = m_objs.GetAt(count++)) != null)
                {
                    obj.Selected = (false);
                }
                RedrawWindow();
            }
        }

       public  void DeleteAllSelected()
        /* ============================================================
	        Function :		DeleteAllSelected
	        Description :	Deletes all selected objects from the 
					        container.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to delete all selected objects from the 
					        editor. Should not be callable if 
					        "GetSelectCount() == 0" ( or "IsAnyObjectSelected() 
					        == false" )

           ============================================================*/
        {

	        if( m_objs != null )
	        {
		        m_objs.Snapshot();
		        m_objs.RemoveAllSelected();
		        RedrawWindow();
	        }

        }

        public void DeleteAll()
        /* ============================================================
	        Function :		DeleteAll
	        Description :	Deletes all objects from the container.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to delete all objects from the editor.

           ============================================================*/
        {

	        if( m_objs!= null )
	        {
		        m_objs.Snapshot();
		        m_objs.RemoveAll();
		        RedrawWindow();
	        }

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor layout

        //public void LeftAlignSelected()
        ///* ============================================================
        //    Function :		LeftAlignSelected
        //    Description :	Aligns all selected objects to the left of
        //                    the top selected object in the data container. 
        //    Access :		Public

        //    Return :		void
        //    Parameters :	none

        //    Usage :			Call to align the left edge of all selected 
        //                    objects.
        //                    Should only be callable if "GetSelectCount() > 
        //                    1", that is, more than one object is selected.

        //   ============================================================*/
        //{

        //    if( GetSelectCount() > 1 )
        //    {
        //        m_objs.Snapshot();
        //        DiagramEntity obj = GetSelectedObject();
        //        if( obj != null )
        //        {
        //            double left = obj.GetLeft();
        //            int count = 0;
        //            while( ( obj = m_objs.GetAt( count++ ) )  != null)
        //            {
        //                if( obj.IsSelected() )
        //                {
        //                    double width = obj.GetRight() - obj.GetLeft();
        //                    double right = left + width;
        //                    obj.SetRect( left, obj.GetTop(), right, obj.GetBottom() );
        //                }
        //            }

        //        }
        //        this.Modified =  true ;
        //        RedrawWindow();
        //    }

        //}

       //public  void RightAlignSelected()
       // /* ============================================================
       //     Function :		RightAlignSelected
       //     Description :	Aligns all selected objects to the right of
       //                     the top selected object in the data container. 
       //     Access :		Public

       //     Return :		void
       //     Parameters :	none

       //     Usage :			Call to align the right edge of all selected 
       //                     objects.
       //                     Should only be callable if "GetSelectCount() > 
       //                     1", that is, more than one object is selected.

       //    ============================================================*/
       // {

       //     if( GetSelectCount() > 1 )
       //     {
       //         m_objs.Snapshot();
       //         DiagramEntity obj = GetSelectedObject();
       //         if( obj != null )
       //         {
       //             double right = obj.GetRight();
       //             int count = 0;
       //             while( ( obj = m_objs.GetAt( count++ ) ) != null)
       //             {
       //                 if( obj.IsSelected() )
       //                 {
       //                     double width = obj.GetRight() - obj.GetLeft();
       //                     double left = right - width;
       //                     obj.SetRect( left, obj.GetTop(), right, obj.GetBottom() );
       //                 }
       //             }

       //         }
       //         this.Modified = (true);
       //         RedrawWindow();
       //     }

       // }

       //public  void TopAlignSelected()
       // /* ============================================================
       //     Function :		TopAlignSelected
       //     Description :	Aligns all selected objects to the top of
       //                     the top selected object in the data container. 
       //     Access :		Public

       //     Return :		void
       //     Parameters :	none

       //     Usage :			Call to align the top of all selected 
       //                     objects.
       //                     Should only be callable if "GetSelectCount() > 
       //                     1", that is, more than one object is selected.

       //    ============================================================*/
       // {

       //     if( GetSelectCount() > 1 )
       //     {
       //         m_objs.Snapshot();
       //         DiagramEntity obj = GetSelectedObject();
       //         if( obj != null )
       //         {
       //             double top = obj.GetTop();
       //             int count = 0;
       //             while( ( obj = m_objs.GetAt( count++ ) )  != null)
       //             {
       //                 if( obj.IsSelected() )
       //                 {
       //                     double height = obj.GetBottom() - obj.GetTop();
       //                     double bottom = top + height;
       //                     obj.SetRect( obj.GetLeft(), top, obj.GetRight(), bottom );
       //                 }
       //             }

       //         }
       //         this.Modified = (true);
       //         RedrawWindow();
       //     }

       // }

       //public  void BottomAlignSelected()
       // /* ============================================================
       //     Function :		BottomAlignSelected
       //     Description :	Aligns all selected objects to the bottom of
       //                     the top selected object in the data container. 
       //     Access :		Public

       //     Return :		void
       //     Parameters :	none

       //     Usage :			Call to align the bottom of all selected 
       //                     objects.
       //                     Should only be callable if "GetSelectCount() > 
       //                     1", that is, more than one object is selected.

       //    ============================================================*/
       // {

       //     if( GetSelectCount() > 1 )
       //     {
       //         m_objs.Snapshot();
       //         DiagramEntity obj = GetSelectedObject();
       //         if( obj  != null)
       //         {
       //             double bottom = obj.GetBottom();
       //             int count = 0;
       //             while( ( obj = m_objs.GetAt( count++ ) ) != null )
       //             {
       //                 if( obj.IsSelected() )
       //                 {
       //                     double height = obj.GetBottom() - obj.GetTop();
       //                     double top = bottom - height;
       //                     obj.SetRect( obj.GetLeft(), top, obj.GetRight(), bottom );
       //                 }
       //             }

       //         }
       //         this.Modified = (true);
       //         RedrawWindow();
       //     }

       // }

        //public void MakeSameSizeSelected()
        ///* ============================================================
        //    Function :		MakeSameSizeSelected
        //    Description :	Makes all selected objects the same size as 
        //                    the top selected object in the data container.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	none

        //    Usage :			Call to make all selected objects the same 
        //                    size.
        //                    Should only be called if "GetSelectCount() > 
        //                    1", that is, more than one object is selected.

        //   ============================================================*/
        //{

        //    if( GetSelectCount() > 1 )
        //    {
        //        m_objs.Snapshot();
        //        DiagramEntity obj = GetSelectedObject();
        //        if( obj != null )
        //        {
        //            double width = obj.GetRight() - obj.GetLeft();
        //            double height = obj.GetBottom() - obj.GetTop();
        //            int count = 0;
        //            while( ( obj = m_objs.GetAt( count++ ) ) != null )
        //                if( obj.IsSelected() )
        //                    obj.SetRect( obj.GetLeft(), obj.GetTop(), obj.GetLeft() + width, obj.GetTop() + height );

        //        }
        //        this.Modified = (true);
        //        RedrawWindow();
        //    }

        //}

       public  void Duplicate()
        /* ============================================================
	        Function :		Duplicate
	        Description :	Duplicates the currently selected object.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to put a copy of the selected object on 
					        the screen.

           ============================================================*/
        {

            if (!check_max_entities(true))
                return;

	        if( GetSelectCount() == 1 )
	        {
		        DiagramEntity obj = GetSelectedObject();
		        if( obj  != null)
		        {
			        m_objs.Snapshot();
			        m_objs.Duplicate( obj );
			        obj.Selected = ( false );
			        RedrawWindow();
		        }
	        }
        }

      public   void Up()
        /* ============================================================
	        Function :		Up
	        Description :	Moves the currently selected object one 
					        step up in the object z-order.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to move the selected object one step 
					        up in the z-order.
					        This command should only be callable if
					        "GetSelectCount()" returns 1, that is, if one 
					        and only one object is selected.

           ============================================================*/
        {

	        if( GetSelectCount() == 1 )
	        {
		        DiagramEntity obj = GetSelectedObject();
		        if( obj != null )
		        {
			        m_objs.Up( obj );
			        RedrawWindow();
		        }
	        }
        }

       public  void Down()
        /* ============================================================
	        Function :		Down
	        Description :	Moves the currently selected object one 
					        step down in the object z-order.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to move the selected object one step 
					        down in the z-order.
					        This command should only be callable if
					        "GetSelectCount()" returns 1, that is, if one 
					        and only one object is selected.

           ============================================================*/
        {

	        if( GetSelectCount() == 1 )
	        {
		        DiagramEntity obj = GetSelectedObject();
		        if( obj  != null)
		        {
			        m_objs.Down( obj );
			        RedrawWindow();
		        }
	        }
        }

        public void Front()
        /* ============================================================
	        Function :		Front
	        Description :	Moves the selected object to the front of 
					        all other objects.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to move the selected object to the 
					        top of the z-order. 
					        This command should only be callable if
					        "GetSelectCount()" returns 1, that is, if one 
					        and only one object is selected.

           ============================================================*/
        {

	        if( GetSelectCount() == 1 )
	        {
		        DiagramEntity obj = GetSelectedObject();
		        if( obj  != null)
		        {
			        m_objs.Front( obj );
			        RedrawWindow();
		        }
	        }
        }

        public void BottomEditor()
        /* ============================================================
	        Function :		BottomEditor
	        Description :	Moves the selected object to the bottom of 
					        all objects.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to move the selected object to the 
					        bottom of the z-order. 
					        This command should only be callable if
					        "GetSelectCount()" returns 1, that is, if one 
					        and only one object is selected.

           ============================================================*/
        {

	        if( GetSelectCount() == 1 )
	        {
		        DiagramEntity obj = GetSelectedObject();
		        if( obj  != null)
		        {
			        m_objs.Bottom( obj );
			        RedrawWindow();
		        }
	        }
        }

        public void ShowKeyContentEditor()
        {

	        if( GetSelectCount() == 1 )
	        {
		        DiagramEntity obj = GetSelectedObject();
                
                if( obj  != null)
                {
                    if (OnEditEntityKeyContent != null)
                        OnEditEntityKeyContent(this, obj);
                }
                
	        }

        }

       private  void RemoveUnselectedPropertyDialogs()
        /* ============================================================
	        Function :		RemoveUnselectedPropertyDialogs
	        Description :	Removes all property dialogs for unselected 
					        objects.
	        Access :		Private

	        Return :		void
	        Parameters :	none

	        Usage :			Internal function. Call to remove all 
					        property dialogs for non-selected objects 
					        from screen.

           ============================================================*/
        {

            //if( m_objs  != null)
            //{
            //    int count = 0;
            //    DiagramEntity obj;
            //    //while( ( obj = m_objs.GetAt( count++ ) )  != null)
            //    //    if( !obj.Selected )
            //    //        obj.ShowProperties( this, false );
            //}

        }

        static public ToolStripMenuItem CloneMenuItem(ToolStripMenuItem item, EventHandler evHandler, string text)
        {
            string s = text;
            if (s.Length <= 0)
                s = item.Text;

            ToolStripMenuItem m = new ToolStripMenuItem(s, item.Image, evHandler, item.ShortcutKeys);
            
            return m;
        }

       // private static ContextMenuStrip m_contextMenu = null;
        protected ContextMenuStrip GetPopupMenu()
        /* ============================================================
	        Function :		CDiagramMenu::GetPopupMenu
	        Description :	Gets a menu pointer to the desired popup 
					        menu.
	        Access :		Public

	        Return :		CMenu*					-	A pointer to 
												        the popup menu
	        Parameters :	CDiagramEditor* editor	-	The editor 
												        calling for a 
												        menu.
        					
	        Usage :			Call to get the popup menu for the editor.

           ============================================================*/
        {
            
            ContextMenuStrip menu = new ContextMenuStrip();
          

            
            frmMain frm = frmMain.g_MainForm;

            ToolStripMenuItem menuCut = CloneMenuItem(frm.editCut, frm.editCut_Click, "");
            menu.Items.Add(menuCut);

            ToolStripMenuItem menuCopy = CloneMenuItem(frm.editCopy, frm.editCopy_Click, "");
            menu.Items.Add(menuCopy);

            
            ToolStripMenuItem menuPaste = CloneMenuItem(frm.editPaste, ContextMenu_Click, "");
            menuPaste.Tag = (int)EditorCmd.EDIT_PASTE;
            menu.Items.Add(menuPaste);

            ToolStripMenuItem menuDel = CloneMenuItem(frm.editDel, frm.editDel_Click, "");
            menu.Items.Add(menuDel);

            menu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem menuWidth = CloneMenuItem(frm.formatSameWidth, frm.formatMakeSameWidth_Click, "Same Width");
            menu.Items.Add(menuWidth);
            ToolStripMenuItem menuHeight = CloneMenuItem(frm.formatSameHeight, frm.formatMakeSameHeight_Click, "Same Height");
            menu.Items.Add(menuHeight);

            ToolStripMenuItem menuBoth = CloneMenuItem(frm.formatSameBoth , frm.formatMakeSameBoth_Click, "Same Both");
            menu.Items.Add(menuBoth);
            
            menu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem menuLeft = CloneMenuItem(frm.formatAlignLeft, frm.formatAlignLeft_Click, "Align Lefts");
            menu.Items.Add(menuLeft);
            ToolStripMenuItem menuCenters = CloneMenuItem(frm.formatAlignCenter, frm.formatAlignCenters_Click, "Align Centers");
            menu.Items.Add(menuCenters);
            ToolStripMenuItem menuRight = CloneMenuItem(frm.formatAlignRight, frm.formatAlignRight_Click, "Align Rights");
            menu.Items.Add(menuRight);
            
            menu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem menuTop = CloneMenuItem(frm.formatAlignTop, frm.formatAlignTop_Click, "Align Top");
            menu.Items.Add(menuTop);

            ToolStripMenuItem menuMiddle = CloneMenuItem(frm.formatAlignMiddle, frm.formatAlignMiddles_Click, "Align Middles");
            menu.Items.Add(menuMiddle);

            ToolStripMenuItem menuBottom = CloneMenuItem(frm.formatAlignBottom, frm.formatAlignBottom_Click, "Align Bottoms");
            menu.Items.Add(menuBottom);

            menu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem menuUndo = CloneMenuItem(frm.editUndo, frm.editUndo_Click, "");
            menu.Items.Add(menuUndo);

            ToolStripMenuItem menuRedo = CloneMenuItem(frm.editRedo, frm.editRedo_Click, "");
            menu.Items.Add(menuRedo);
           
            bool bselected = IsAnyObjectSelected();
            
            menuCut.Enabled = bselected;
            menuCopy.Enabled = bselected;
            menuDel.Enabled = bselected;

            bool bMultipleSelected = (GetSelectCount() > 1);
            menuWidth.Enabled = bMultipleSelected;
            menuHeight.Enabled = bMultipleSelected;
            menuBoth.Enabled = bMultipleSelected;

            menuLeft.Enabled = bMultipleSelected;
            menuCenters.Enabled = bMultipleSelected;
            menuRight.Enabled = bMultipleSelected;

            menuTop.Enabled = bMultipleSelected;
            menuMiddle.Enabled = bMultipleSelected;
            menuBottom.Enabled = bMultipleSelected;


     

            bool bpaste = (this.DiagramEntityContainer != null && this.DiagramEntityContainer.ObjectsInPaste() > 0);
            menuPaste.Enabled = bpaste;

            bool bundo = (this.DiagramEntityContainer != null && this.DiagramEntityContainer.UndoStack.Count > 0);
            menuUndo.Enabled = bundo;

            bool bredo = (this.DiagramEntityContainer != null && this.DiagramEntityContainer.RedoStock!= null);
            menuRedo.Enabled = bredo;
                

            return menu;

        }


        public void ShowPopup( Point point ) 
        /* ============================================================
	        Function :		ShowPopup
	        Description :	Shows a popup menu in response to a right-
					        click if no object is selected.
	        Access :		Public

	        Return :		void
	        Parameters :	Point point	-	The point to track the 
										        menu from.
        					
	        Usage :			Internal function. Call to show the editor 
					        popup menu.

           ============================================================*/
        {

            //if( m_popupMenu == null )
            //    m_popupMenu = new DiagramMenu();

	        //if( m_popupMenu  != null )
	        {
                ContextMenuStrip menu = GetPopupMenu();
                
              
                if (menu != null)
                {
                    m_contextMenuPoint = point;
                    menu.Show(this, point);
                    
                    //WinAPI.PostMessage(this.Handle, WinAPI.UM_REFRESH, 0, 0);
                   // this.RedrawWindow();
                }
			        //menu.TrackPopupMenu( TPM_LEFTALIGN | TPM_RIGHTBUTTON, point.X, point.Y, this );
	        }

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor copy/paste/undo

        protected void OnEditCut()
        /* ============================================================
	        Function :		OnEditCut
	        Description :	Command handler for the MFC standard 
					        "ID_EDIT_CUT" command.
	        Access :		Protected

	        Return :		void
	        Parameters :	none

	        Usage :			Called from MFC. Call "Cut" from code instead.

           ============================================================*/
        {

	        Cut();

        }

       protected  void OnEditCopy()
        /* ============================================================
	        Function :		OnEditCopy
	        Description :	Command handler for the MFC standard 
					        "ID_EDIT_COPY" command.
	        Access :		Protected

	        Return :		void
	        Parameters :	none

	        Usage :			Called from MFC. Call "Copy" from code 
					        instead.

           ============================================================*/
        {

	        Copy();

        }

        protected void OnEditPaste()
        /* ============================================================
	        Function :		OnEditPaste
	        Description :	Command handler for the MFC standard 
					        "ID_EDIT_PASTE" command.
	        Access :		Protected

	        Return :		void
	        Parameters :	none

	        Usage :			Called from MFC. Call "Paste" from code 
					        instead.

           ============================================================*/
        {

	        Paste(true);

        }

        protected void OnEditGroup()
        /* ============================================================
	        Function :		OnEditGroup
	        Description :	Handler for the "ID_EDIT_GROUP" command
	        Access :		Protected

	        Return :		void
	        Parameters :	none

	        Usage :			Groups the currently selected objects. 
					        Grouped objects can be moved as a 
					        single entity. Technically, when one object 
					        in a group is selected, all other objects 
					        are also selected automatically.

           ============================================================*/
        {

	        Group();

        }

        protected void OnEditUngroup()
        /* ============================================================
	        Function :		OnEditUngroup
	        Description :	Handler for the "ID_EDIT_UNGROUP" command
	        Access :		Protected

	        Return :		void
	        Parameters :	none

	        Usage :			Ungroups the currently selected objects.
					        Grouped objects can be moved as a 
					        single entity. Technically, when one object 
					        in a group is selected, all other objects 
					        are also selected automatically.

           ============================================================*/
        {

	        Ungroup();

        }

       public   void Cut()
        /* ============================================================
	        Function :		Cut
	        Description :	Cut the currently selected items to the 
					        internal "CDiagramEntityContainer" clipboard.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to delete and place the currently 
					        selected objects to the 'clipboard'.

           ============================================================*/
        {

	        if( m_objs  != null)
	        {
                //this.Focus();
		        m_objs.Snapshot();
		        m_objs.CopyAllSelected();
		        DeleteAllSelected();
               // BeginInvoke(this.OnShowDiagramEntityProperties);
               // this.Invalidate();
	        }

        }

       public   void Copy()
        /* ============================================================
	        Function :		Copy
	        Description :	Copy the currently selected items to the 
					        internal "CDiagramEntityContainer" clipboard.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to copy the selected objects to the 
					        'clipboard'.

           ============================================================*/
        {

           //debug group
        
	        if( m_objs != null )
	        {
		      //  m_objs.Snapshot(); //20140208
		        m_objs.CopyAllSelected();
	        }

        }

       public   void Paste(bool fromPupopMenu)
        /* ============================================================
	        Function :		Paste
	        Description :	Paste copied or cut objects to the screen.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to paste the contents of the clipboard 
					        to the screen.

           ============================================================*/
        {
         
	        // Removes the current selection and pastes the contents
	        // of the data paste-array.
	        if( m_objs != null )
	        {

                if (!check_max_entities(true))
                    return;

		        //SetRedraw( false );
		        m_objs.Snapshot();
		        UnselectAll();
               
		        List<DiagramEntity> pasted =  m_objs.Paste();
                //20140208
                //change their name to unique
                //nomatter this command comes from where, we need to create new name for them
           /*     for (int i = 0; i < pasted.Count; i++)
                {
                    DiagramKey obj = (DiagramKey)pasted[i];

                    if (m_objs.exist_same_title(obj.Title))
                    {
                        obj.PauseEvent = true;
                        obj.Title = CreateUniqueName(obj.TypeName);
                        obj.PauseEvent = false;
                    }
                }
             */
                //adjust the position
                if (fromPupopMenu)
                {
                    Point lefttop = m_objs.GetSelectedStartPoint(pasted);

                    Point pt = m_contextMenuPoint;// Control.MousePosition;
                    //pt = this.PointToClient(pt);
                    ClientToVirtual(ref pt);
                    Point offset = new Point(pt.X - lefttop.X, pt.Y - lefttop.Y);
                    for (int i=0; i< pasted.Count; i++)
                    {
                        DiagramEntity obj = (DiagramEntity)pasted[i];

                        /* //20140208, make all paste(from popup menu, or shortcut keys) can create unique name
                         * //so , i move these code to top
                        if (m_objs.exist_same_title(obj.Title))
                        {
                            obj.PauseEvent = true;
                            obj.Title = CreateUniqueName(obj.TypeName);
                            obj.PauseEvent = false;
                        }
                         * */
                        Point pos = obj.Bounds.Location;
                        pos.Offset(offset);
                        double top = pos.Y, left = pos.X, right =left +  obj.Bounds.Width;
                        double bottom = top + obj.Bounds.Height;
                        AdjustForRestraints(ref left, ref top, ref right, ref bottom);
                        Rectangle rt = new Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top));
                        obj.Bounds = rt;

                    }

                }
		        //SetRedraw( true );
		        RedrawWindow();
	        }

        }

       public void Redo()
       {

           if (m_objs != null)
           {
               UndoItem undo = m_objs.Redo();
               if (undo == null) return;
               if (undo.EditorPropertiesEnabled)
               {
                   base.BackColor = undo.EditorBackColor;
                   this._InterCharDelay = undo.EditorInterCharDelay;
                   this._Sensitivity = undo.EditorSensitivity;
                   this._TouchDelay = undo.EditorTouchDelay;

               }
               if (this.OnEditorUndo != null)
                   this.OnEditorUndo(this, undo);
               RedrawWindow();
           }

       }
       public bool CanRedo()
       {
           return (m_objs.RedoStock.Count >0);
       }
        public  void Undo()
        /* ============================================================
	        Function :		Undo
	        Description :	Undo the last operation.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to restore the objects to the last 
					        snapshot.

           ============================================================*/
        {

	        if( m_objs != null )
	        {
		        UndoItem undo =  m_objs.Undo();
                if (undo == null) return;
                if (undo.EditorPropertiesEnabled)
                {
                    base.BackColor = undo.EditorBackColor;
                    this._InterCharDelay = undo.EditorInterCharDelay;
                    this._Sensitivity = undo.EditorSensitivity;
                    this._TouchDelay = undo.EditorTouchDelay;
                    //if (undo.LastFocusedEditBoxIndex > 0)
                    {
                        if (this.OnEditorUndo != null)
                            this.OnEditorUndo(this, undo);
                    }
                }
		        RedrawWindow();
	        }

        }

        public  void Group()
        /* ============================================================
	        Function :		OnEditGroup
	        Description :	Groups the currently selected objects.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to group the currently selected 
					        objects. Grouped objects can be moved as a 
					        single entity. Technically, when one object 
					        in a group is selected, all other objects 
					        are also selected automatically.

           ============================================================*/
        {

	        if( m_objs != null )
	        {
		        SetRedraw( false );
		        m_objs.Snapshot();
		        m_objs.Group();
		        SetRedraw( true );
		        RedrawWindow();
	        }

        }

        public  void Ungroup()
        /* ============================================================
	        Function :		OnEditUngroup
	        Description :	Ungroups the currently selected objects.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to ungroup the currently selected 
					        objects. Grouped objects can be moved as a 
					        single entity. Technically, when one object 
					        in a group is selected, all other objects 
					        are also selected automatically.

           ============================================================*/
        {

	        if( m_objs != null )
	        {
		        SetRedraw( false );
		        m_objs.Snapshot();
		        m_objs.Ungroup();
		        SetRedraw( true );
		        RedrawWindow();
	        }

        }

       public  void UpdateCut( Menu menu, int nItemIndex ) 
        /* ============================================================
	        Function :		UpdateCut
	        Description :	Command enabling for a Cut command UI-
					        element.
	        Access :		Public

	        Return :		void
	        Parameters :	CCmdUI* pCmdUI	-	Command element to 
										        update
        					
	        Usage :			Can be called from a doc/view command update 
					        function

           ============================================================*/
        {

	        // We can only cut if there is something selected

	        //pCmdUI.Enable( IsAnyObjectSelected() );
            menu.MenuItems[nItemIndex].Enabled = IsAnyObjectSelected();

        }

        public void UpdateCopy( Menu menu, int nItemIndex ) 
        /* ============================================================
	        Function :		UpdateCopy
	        Description :	Command enabling for a Copy command UI-
					        element.
	        Access :		Public

	        Return :		void
	        Parameters :	CCmdUI* pCmdUI	-	Command element to 
										        update
        					
	        Usage :			Can be called from a doc/view command update 
					        function

           ============================================================*/
        {

             menu.MenuItems[nItemIndex].Enabled = IsAnyObjectSelected();
	        //pCmdUI.Enable( IsAnyObjectSelected() );

        }

        public void UpdatePaste( Menu menu, int nItemIndex) 
        /* ============================================================
	        Function :		UpdatePaste
	        Description :	Command enabling for a Paste command UI-
					        element.
	        Access :		Public

	        Return :		void
	        Parameters :	CCmdUI* pCmdUI	-	Command element to 
										        update
        					
	        Usage :			Can be called from a doc/view command update 
					        function

           ============================================================*/
        {

	        if( m_objs != null)
                menu.MenuItems[nItemIndex].Enabled =( m_objs.ObjectsInPaste() >0);
		        //pCmdUI.Enable( m_objs.ObjectsInPaste() );

        }

        public void UpdateUndo(  Menu menu, int nItemIndex ) 
        /* ============================================================
	        Function :		UpdateUndo
	        Description :	Command enabling for an Undo command UI-
					        element.
	        Access :		Public

	        Return :		void
	        Parameters :	CCmdUI* pCmdUI	-	Command element to 
										        update
        					
	        Usage :			Can be called from a doc/view command update 
					        function

           ============================================================*/
        {

	        if( m_objs != null)
                menu.MenuItems[nItemIndex].Enabled = m_objs.IsUndoPossible() ;
		        //pCmdUI.Enable( m_objs.IsUndoPossible() );

        }

        public void UpdateGroup( Menu menu, int nItemIndex  ) 
        /* ============================================================
	        Function :		UpdateGroup
	        Description :	Command enabling for an Group command UI-
					        element.
	        Access :		Public

	        Return :		void
	        Parameters :	CCmdUI* pCmdUI	-	Command element to 
										        update
        					
	        Usage :			Can be called from a doc/view command update 
					        function

           ============================================================*/
        {

	        if( m_objs != null)
                 menu.MenuItems[nItemIndex].Enabled = ( GetSelectCount() > 1 ) ;
		        //pCmdUI.Enable( GetSelectCount() > 1 );

        }

       public  void UpdateUngroup( Menu menu, int nItemIndex  ) 
        /* ============================================================
	        Function :		UpdateUngroup
	        Description :	Command enabling for an Ungroup command UI-
					        element.
	        Access :		Public

	        Return :		void
	        Parameters :	CCmdUI* pCmdUI	-	Command element to 
										        update
        					
	        Usage :			Can be called from a doc/view command update 
					        function

           ============================================================*/
        {

	        if( m_objs != null)
                menu.MenuItems[nItemIndex].Enabled = ( GetSelectCount() > 1 ) ;
		       // pCmdUI.Enable( GetSelectCount() > 1 );

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor saving

        public virtual void ExportToXml( CLCIXML xml )
        /* ============================================================
	        Function :		ExportToXml
	        Description :	Saves a string representation of the 
					        container (normally the virtual size) and 
					        all objects to "stra".
	        Access :		Public

	        Return :		void
	        Parameters :	CStringArray& stra	-	The array to fill
        					
	        Usage :			Call to save the data of the editor to a 
					        "CStringArray". Virtual. Can be overridden in 
					        a derived class to add non-container data 
					        before and after the objects

           ============================================================*/
        {
            


        }


        public virtual bool ImportFromXml(CLCIXML xml)
        /* ============================================================
	        Function :		LoadFromXml
	        Description :	Sets the virtual size from a string 
					        representation of the paper.
	        Access :		Public

	        Return :		bool				-	"true" if "str" is a 
											        representation of a 
											        'paper'.
	        Parameters :	const string& str	-	String representation.
        					
	        Usage :			Call to set the size of the virtual paper 
					        from a string.

           ============================================================*/
        {
            return false;
        }


      

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor command handling

        protected void ContextMenu_Click(System.Object sender, System.EventArgs e)
        {
            // Insert code to handle Click event.
            string s = sender.ToString();
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            EditorCmd command = (EditorCmd)menuItem.Tag;

            OnEditorCommand(command);
        }
        /************************************************************************/
        /* 
         * */
        /************************************************************************/
        protected void OnEditorCommand(EditorCmd command)
        {
               switch(command)
               {
                   case EditorCmd.EDIT_CUT:
                       {
                           OnEditCut();
                       }
                       break;
                   case EditorCmd.EDIT_COPY:
                       {
                           OnEditCopy();
                       }
                       break;
                   
                   case EditorCmd.EDIT_PASTE:
                       {
                           OnEditPaste();
                       }
                       break;
                   case EditorCmd.EDIT_GROUP:
                       {
                           OnEditGroup();
                       }
                       break;
                   case EditorCmd.EDIT_UNGROUP:
                       {
                           OnEditUngroup();
                       }
                       break;
                   case EditorCmd.EDIT_SIZE_WIDTH:
                       {
                           OnEditSameSize(DiagramEntityContainer.EntitiesSameSize.SAME_WIDTH);
                       }
                       break;
                   case EditorCmd.EDIT_SIZE_HEIGHT:
                       {
                           OnEditSameSize(DiagramEntityContainer.EntitiesSameSize.SAME_HEIGHT);
                       }
                       break;
                   case EditorCmd.EDIT_SIZE_BOTH:
                       {
                           OnEditSameSize(DiagramEntityContainer.EntitiesSameSize.SAME_BOTH);
                       }
                       break;

                   case EditorCmd.EDIT_ALIGN_TOP:
                       {
                           OnEditAlign(DiagramEntityContainer.EntitiesAlign .ALIGN_TOP);
                       }
                       break;
                   case EditorCmd.EDIT_ALIGN_BOTTOM:
                       {
                           OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_BOTTOM);
                       }
                       break;
                   case EditorCmd.EDIT_ALIGN_LEFT:
                       {
                           OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_LEFT);
                       }
                       break;
                   case EditorCmd.EDIT_ALIGN_RIGHT:
                       {
                           OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_RIGHT);
                       }
                       break;
                   case EditorCmd.EDIT_ALIGN_MIDDLE:
                       {
                           OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_MIDDLE);
                       }
                       break;
                   case EditorCmd.EDIT_ALIGN_CENTER:
                       {
                           OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_CENTER);
                       }
                       break;
                   case EditorCmd.EDIT_UNDO:
                       {
                           Undo();
                       }
                       break;
                   default:
                       break;
               }
        }

       public void OnObjectCommand(DiagramEntity.EntityCmd nCmd )
        /* ============================================================
	        Function :		OnObjectCommand
	        Description :	Called from "WM_COMMAND" messages in the 
					        range "CMD_START" to "CMD_END" inclusive 
					        (defined in DiagramEntity.h). Those are 
					        messages that will be passed on to all 
					        selected objects.
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nID	-	The command to send.
        					
	        Usage :			Called from MFC. Do not call from code.

           ============================================================*/
        {

	        if( m_objs != null )
	        {
		        m_objs.Snapshot();
                m_objs.SendMessageToObjects(nCmd, true, null, (this));
		        RedrawWindow();
	        }

        }

        public MouseMode GetInteractMode() 
        /* ============================================================
	        Function :		GetInteractMode
	        Description :	Get the interact mode.
	        Access :		Protected

	        Return :		int		-	Current interact mode
	        Parameters :	none

	        Usage :			The interact mode can be one of the 
					        following:
						        "MODE_NONE" No mode
						        "MODE_RUBBERBANDING" User is selecting
						        "MODE_MOVING" User is moving object(s)
						        "MODE_RESIZING" User is resizing object
						        "MODE_DRAWING" User is drawing object
						        "MODE_BGRESIZING" User is resizing the virtual paper

           ============================================================*/
        {

	        return m_interactMode;

        }

        protected void SetInteractMode( MouseMode interactMode, DiagramEntity.DEHT subMode )
        /* ============================================================
	        Function :		SetInteractMode
	        Description :	Sets the current mode of the editor
	        Access :		Protected

	        Return :		void
	        Parameters :	int interactMode	-	The mode to set
					        int subMode			-	The point of the 
											        selected object, if 
											        appropriate.
        					
	        Usage :			Call to set the interact mode of the 
					        editor. The interact mode can be one of 
					        the following:
						        "MODE_NONE" No mode
						        "MODE_RUBBERBANDING" User is selecting
						        "MODE_MOVING" User is moving object(s)
						        "MODE_RESIZING" User is resizing object
						        "MODE_DRAWING" User is drawing object
						        "MODE_BGRESIZING" User is resizing the virtual paper
					        The submode, relevant when the mode is 
					        MODE_RESIZING or MODE_BGRESIZING, can be 
					        one of the following:
						        "DEHT_NONE" No hit
						        "DEHT_BODY" Object body
						        "DEHT_TOPLEFT" Top-left corner
						        "DEHT_TOPMIDDLE" Top-side
						        "DEHT_TOPRIGHT" Top-right corner
						        "DEHT_BOTTOMLEFT" Bottom left corner
						        "DEHT_BOTTOMMIDDLE" Bottom-side
						        "DEHT_BOTTOMRIGHT" Bottom-right corner
						        "DEHT_LEFTMIDDLE" Left side
						        "DEHT_RIGHTMIDDLE" Right side
					        and indicates the point where the mouse is 
					        as regards the currently selected object 
					        (or the background).

           ============================================================*/
        {

	        m_interactMode = interactMode;
	        m_subMode = subMode;

        }

       //protected  DiagramEntity GetDrawingObject()
       // /* ============================================================
       //     Function :		GetDrawingObject
       //     Description :	Gets the object the user will draw.
       //     Access :		Protected

       //     Return :		DiagramEntity	-	Object
       //     Parameters :	none

       //     Usage :			Call to get the object that will be drawn 
       //                     when the user clicks the 'paper'.

       //    ============================================================*/
       // {

       //     return m_drawObj;

       // }

        //public Color GetBackgroundColor() 
        ///* ============================================================
        //    Function :		GetBackgroundColor
        //    Description :	Accessor for the background color
        //    Access :		Public

        //    Return :		Color	-	Current background color
        //    Parameters :	none

        //    Usage :			Call to get the background color

        //   ============================================================*/
        //{

        //    return m_bkgndCol;

        //}

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor zoom to fit functions

        public void ZoomToFitScreen()
        /* ============================================================
	        Function :		ZoomToFitScreen
	        Description :	Zooms and scrolls so that all of the 
					        current diagram is visible on screen.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to make the complete diagram visible.

           ============================================================*/
        {

	        //SetRedraw( false );
	        Rectangle rect = this.ClientRectangle;

            Point start = this.DiagramEntityContainer.GetStartPoint();
	        ZoomToFit( start, new Size( rect.Width, rect.Height ) );
	       // SetRedraw( true );
	        RedrawWindow();

        }

       public  bool ZoomToFit( Point start, Size size )
        /* ============================================================
	        Function :		ZoomToFit
	        Description :	Zooms the diagram to the size "size" and 
					        sets the scrollbar positions at "start".
	        Access :		Public

	        Return :		bool			-	"true" if any zoom (if
										        that is the case, the 
										        screen should be redrawn.
	        Parameters :	Point start	-	Starting position of the 
										        diagram, non-zoomed.
					        Size size		-	Size the diagram should 
										        be zoomed to.
        					
	        Usage :			Call to zoom the diagram to a specific size.

           ============================================================*/
        {

	        bool result = false;

	        if( ZoomToFit( size ) )
	        {

		        // Set scrollbar positions
		        //SCROLLINFO si;
		        //si.cbSize = sizeof( SCROLLINFO );
		        //si.fMask = SIF_POS;
		        //if( GetScrollInfo( SB_HORZ, &si ) )
                if (this.HorizontalScroll.Visible)
		        {
                    int nscroll = (int)decimal.Round((decimal)(start.X - this.MarkerSize.Width) * (decimal)this.Zoom);
			        SetHScroll( nscroll );

                    nscroll = (int)decimal.Round((decimal)(start.Y - this.MarkerSize.Height) * (decimal)this.Zoom);
			        SetVScroll( nscroll );

		        }
		        result = true;
	        }

	        return result;

        }

       public  bool ZoomToFit( Size size )
        /* ============================================================
	        Function :		ZoomToFit
	        Description :	Zooms the diagram to the size "size".
	        Access :		Public

	        Return :		bool		-	"true" if zoomed. The screen
									        should be redrawn if that is 
									        the case.
	        Parameters :	Size size	-	Size to zoom to.
        					
	        Usage :			Call to zoom the diagram to a specific size.

           ============================================================*/
        {

	        bool result = false;
            Size objects = this.DiagramEntityContainer.GetTotalSize();

	        if( objects.Width>0 && objects.Height>0 )
	        {
		        // We add a little margin
                objects.Width += this.MarkerSize.Width * 2;
                objects.Height += this.MarkerSize.Height * 2;

		        // Calculate new zoom
		        double zoomx = ( double )( size.Width ) / ( double )( objects.Width );
		        double zoomy = ( double )( size.Height ) / ( double )( objects.Height );

                this.Zoom = (Math.Min(zoomx, zoomy));
		        result = true;

	        }
        	
	        return result;

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor mouse wheel handling
        //static int mouseDelta = 0;
         //private const int WHEEL_DELTA = 120;
        protected override void  OnMouseWheel(MouseEventArgs e)
            /* ============================================================
	        Function :		OnMouseWheel
	        Description :	Handles the "WM_MOUSEWHEEL"-message.
	        Access :		Protected

	        Return :		bool			-	From default
	        Parameters :	UINT nFlags		-	Not used
					        short zDelta	-	Delta (notches * "WHEEL_DELTA")
					        Point pt		-	Mouse position
        					
	        Usage :			Called from MFC. Scrolls the vertically.

           ============================================================*/
        {
 	             
            //}
            //        bool OnMouseWheel( UINT nFlags, short zDelta, Point pt )
        
       // {
                
                 

	        if( this.ScrollWheelMode == WheelMode.WHEEL_SCROLL )
	        {
                base.OnMouseWheel(e);
                RedrawWindow();
              
	        }
	        else if( this.ScrollWheelMode ==WheelMode.WHEEL_ZOOM )
	        {
                double d = 0;
		        if( e.Delta > 0 )
                    d = (this.Zoom + this.ZoomStep);
		        else
                    d = (this.Zoom - this.ZoomStep);
                if (d > this.ZoomMax)
                    this.Zoom = this.ZoomMax;
                else if (d < this.ZoomMin)
                    this.Zoom = this.ZoomMin;
                else
                    this.Zoom = d;


                RedrawWindow();
                FireChangedEvent();
	        }

	      
        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEditor panning
        
       protected void OnMButtonDown( MouseEventArgs e) 
        /* ============================================================
	        Function :		OnMButtonDown
	        Description :	Handler for the "WM_MBUTTONDOWN"-message
	        Access :		Protected

	        Return :		void
	        Parameters :	UINT nFlags		-	Not used
					        Point point	-	Current mouse position
        					
	        Usage :			Called from MFC. Don't call from code. 
					        We start panning.

           ============================================================*/
        {
            return;
           /*
            Point point = e.Location;


	        if( !this.MiddleButtonPanning )
	        {
		       // SCROLLINFO si;
		       // if( GetScrollInfo( SB_HORZ, &si ) ||
			   //     GetScrollInfo( SB_VERT, &si ) )
                if (this.HorizontalScroll.Visible ||
                    this.VerticalScroll.Visible)
		        {
			        //SetPanningOrigin( point );
                     ScreenToVirtual(ref point);
                     this.MiddleButtonPanningVirtualOrigin =point;
			        //SetPanning( true );
                    this.MiddleButtonPanning = true;
                    this.MiddleButtonPanningScrollBarOrigin = this.AutoScrollPosition;
		        }
	        }
	        else
	        {
		        //SetPanning( false );
                this.MiddleButtonPanning = false;
	        }

	        //CWnd::OnMButtonDown( nFlags, point );
	        RedrawWindow();
            //show properties of virutal screen
            if (GetSelectCount() <= 0)
                FireShowPropertiesEvent(null);
           */
        }

        //void OnTimer( UINT nIDEvent )
        ///* ============================================================
        //    Function :		OnTimer
        //    Description :	Handler for the "WM_TIMER"-message
        //    Access :		Protected

        //    Return :		void
        //    Parameters :	UINT nIDEvent	-	Current timer
        					
        //    Usage :			Called from MFC. We pan if we are in 
        //                    panning mode.

        //   ============================================================*/
        //{

        //    //if( nIDEvent == m_panningTimer )
        //    //{

        //    //    KillTimer( m_panningTimer );

        //    //     MSG* msg = GetCurrentMessage();
        //    //    Point test = GetPanningOrigin();
        //    //    Point point( msg.pt );
        //    //    ScreenToClient( &point );

        //    //    int diffx = ( point.X - test.X ) / 16;
        //    //    int diffy = ( point.Y - test.Y ) / 16;

        //    //    SCROLLINFO sih;
        //    //    sih.cbSize = sizeof( SCROLLINFO );
        //    //    sih.fMask = SIF_POS;
        //    //    sih.nPos = 0;
        //    //    SCROLLINFO siv;
        //    //    siv.cbSize = sizeof( SCROLLINFO );
        //    //    siv.fMask = SIF_POS;
        //    //    siv.nPos = 0;

        //    //    GetScrollInfo( SB_HORZ, &sih );
        //    //    GetScrollInfo( SB_VERT, &siv );

        //    //    sih.nPos += diffx;
        //    //    siv.nPos += diffy;

        //    //    SetHScroll( sih.nPos );
        //    //    SetVScroll( siv.nPos );
        //    //    RedrawWindow();

        //    //    m_panningTimer = SetTimer( m_panningTimer, 10, null );

        //    //}

        //}

        protected override void  OnLostFocus(EventArgs e)
            /* ============================================================
	        Function :		OnKillFocus
	        Description :	Handler for the "WM_KILLFOCUS"-message
	        Access :		Protected

	        Return :		void
	        Parameters :	CWnd* pNewWnd	-	Window getting the focus
        					
	        Usage :			Called from MFC. We kill the panning

           ============================================================*/

        {
 	         base.OnLostFocus(e);


            //if( this.MiddleButtonPanning  )
            //    //SetPanning( false );
            //    this.MiddleButtonPanning = false;

	        //CWnd::OnKillFocus( pNewWnd );

        }

        //private bool _MiddleButtonPanning = false;
        //[Browsable(false)]
        //public bool MiddleButtonPanning
        //{
        //    get { return _MiddleButtonPanning; }
        //    set 
        //    {
        //        if (_MiddleButtonPanning && value == false)
        //        {
        //            m_middleButtonPanningTimer.Stop();
        //            this.Capture = false;
        //        }
        //        //    KillTimer( m_panningTimer );

        //        _MiddleButtonPanning = value;

        //        if (_MiddleButtonPanning)
        //        {
        //            this.Capture = true;
        //            m_middleButtonPanningTimer.Start();
        //        }
        //        //    m_panningTimer = SetTimer( m_panningTimer, 10, null );
                
        //    }
        //}
       //public  bool GetPanning()
       // /* ============================================================
       //     Function :		GetPanning
       //     Description :	Accessor for the panning flag.
       //     Access :		Public

       //     Return :		bool	-	"true" if we are currently 
       //                                 panning.
       //     Parameters :	none

       //     Usage :			Call to see if we are panning

       //    ============================================================*/
       // {

       //     return m_panning;

       // }

        private Point _MiddleButtonPanningVirtualOrigin = new Point(0, 0);
        [Browsable(false)]
        public Point MiddleButtonPanningVirtualOrigin
        {
            get { return _MiddleButtonPanningVirtualOrigin; }
            set 
            {

                _MiddleButtonPanningVirtualOrigin = value;
            }
        }

        private Point _MiddleButtonPanningScrollBarOrigin = new Point(0, 0);
        [Browsable(false)]
        public Point MiddleButtonPanningScrollBarOrigin
        {
            get { return _MiddleButtonPanningScrollBarOrigin; }
            set
            {

                _MiddleButtonPanningScrollBarOrigin = value;
            }
        }

        //public Point GetPanningOrigin()
        ///* ============================================================
        //    Function :		GetPanningOrigin
        //    Description :	Accessor for the panning origin.
        //    Access :		Public

        //    Return :		Point	-	Origin for panning.
        //    Parameters :	none

        //    Usage :			Call to get the panning origin. The panning 
        //                    origin is the point where the user clicked 
        //                    the middle mouse button.

        //   ============================================================*/
        //{

        //    return m_panOrigin;

        //}

        //public void SetPanning( bool panning )
        ///* ============================================================
        //    Function :		SetPanning
        //    Description :	Accessor for the panning flag
        //    Access :		Public

        //    Return :		void
        //    Parameters :	bool panning	-	"true" if we should 
        //                                        start panning.
        					
        //    Usage :			Call to start or stop panning.

        //   ============================================================*/
        //{

        //    //if( m_panning && panning == false )
        //    //    KillTimer( m_panningTimer );

        //    m_panning = panning;

        //    //if( m_panning )
        //    //    m_panningTimer = SetTimer( m_panningTimer, 10, null );

        //}

       //public  void SetPanningOrigin( Point point )
       // /* ============================================================
       //     Function :		SetPanningOrigin
       //     Description :	Sets the panning origin.
       //     Access :		Public

       //     Return :		void
       //     Parameters :	Point point	-	The origin for the 
       //                                         panning.
        					
       //     Usage :			Call to set the origin when starting to 
       //                     pan the screen. The panning origin is the
       //                     point where the user clicked the middle 
       //                     mouse button.

       //    ============================================================*/
       // {

       //     m_panOrigin = point;

       // }

        //public void DrawPanning( Graphics g, Rectangle totalRect) 
        ///* ============================================================
        //    Function :		DrawPanning
        //    Description :	Draws the panning origin marker.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	CDC* dc	-	"CDC" to draw to
        					
        //    Usage :			Call to draw the panning origin marker. You
        //                    can override this to draw a different 
        //                    origin marker for the pan.

        //   ============================================================*/
        //{

        //    DiagramEditor  local = (DiagramEditor)( this );

        //    //int nPosX = 0;
        //    //int nPosY = 0;
        //    //SCROLLINFO sih;
        //    //sih.cbSize = sizeof( SCROLLINFO );
        //    //sih.fMask = SIF_POS;
        //    //SCROLLINFO siv;
        //    //siv.cbSize = sizeof( SCROLLINFO );
        //    //siv.fMask = SIF_POS;
        //    //if( !local.GetScrollInfo( SB_HORZ, &sih ) )
        //    //if (!local.HorizontalScroll.Visible)
        //    //    nPosX = 0;
        //    //if( !local.VerticalScroll.Visible )
        //    //    nPosY = 0;

        //    SolidBrush br = new SolidBrush(Color.White);

        //    //dc.SelectStockObject( WHITE_BRUSH );
        //    Pen pen = new Pen(Color.FromArgb(128, 128, 128), 1);
        //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

        //    //pen.CreatePen( PS_SOLID, 0, RGB( 128, 128, 128 ) );

        //    //dc.SelectObject( &pen );
        //    Point pt = this.MiddleButtonPanningVirtualOrigin;// GetPanningOrigin();
        //    Rectangle	rect = new Rectangle( pt.X - 16,// +Math.Abs(local.MiddleButtonPanningScrollBarOrigin.X),//  local.AutoScrollPosition.X, 
        //                                      pt.Y - 16,// +Math.Abs( local.MiddleButtonPanningScrollBarOrigin.Y),// local.AutoScrollPosition.Y , 
        //                                      16,//pt.X + 16 +local.AutoScrollPosition.X, 
        //                                      16);//pt.Y + 16 +local.AutoScrollPosition.Y);
        //    rect.Offset(totalRect.Location);
        //    g.DrawEllipse(pen, rect);

        //    //dc.Ellipse( rect );

        //    rect.Inflate( -12, -12 );
        //    //dc.SelectStockObject( LTGRAY_BRUSH );
        //    SolidBrush brGray = new SolidBrush(Color.LightGray);
        //    Pen penGray = new Pen(Color.LightGray, 1);
        //    //dc.Ellipse( rect );
        //    g.DrawEllipse(penGray, rect);

        //    //dc.SelectStockObject( BLACK_PEN );

        //}

        protected void MoveObject( DiagramEntity obj, Rectangle rect )
        /* ============================================================
	        Function :		MoveObject
	        Description :	Moves an object to "rect"
	        Access :		Protected

	        Return :		void
	        Parameters :	DiagramEntity obj	-	Object to move
					        const Rectangle& rect	-	New position
        					
	        Usage :			This function can be overridden to trap
					        object movement.

           ============================================================*/
        {

            obj.Bounds =rect;
	        //obj.SetRect( rect );

        }

        
        

        protected Cursor GetCursor()
        /* ============================================================
	        Function :		GetCursor
	        Description :	Gets a handle to the cursor appropriate 
					        depending on mode, position etc.
	        Access :		Protected

	        Return :		HCURSOR	-	Appropriate cursor
	        Parameters :	none

	        Usage :			Call to get a cursor to display depending on 
					        the current context.

           ============================================================*/
        {

            Cursor result = null;// Cursors.Arrow;

            //if( this.MiddleButtonPanning )
            //{
            //    //const MSG* msg = GetCurrentMessage();
            //    Point pt = Cursor.Position;

            //    Point test = this.MiddleButtonPanningVirtualOrigin;// GetPanningOrigin();
            //    Point point = pt;// new Point(pt.X, pt.Y);
            //    point = this.PointToClient(point);
            //    ClientToVirtual(ref point);
            //    //ScreenToClient( &point );

            //    if( point.X > test.X + 16 )
            //    {
            //        if ( point.Y > test.Y + 16 )
            //            result = Cursors.SizeNWSE;//m_cursorSouthEast;
            //        else if( point.Y < test.Y - 16 )
            //            result = Cursors.SizeNESW;//m_cursorNorthEast;
            //        else
            //            result = Cursors.SizeWE;// m_cursorEast;
            //    }
            //    else if( point.X < test.X - 16 )
            //    {
            //        if ( point.Y > test.Y + 16 )
            //            result = Cursors.SizeNESW;//m_cursorSouthWest;
            //        else if( point.Y < test.Y  - 16 )
            //            result = Cursors.SizeNWSE;//m_cursorNorthWest;
            //        else
            //            result = Cursors.SizeWE;// m_cursorWest;
            //    }
            //    else if( point.Y < test.Y )
            //        result = Cursors.SizeNS;//m_cursorSouth;
            //    else
            //        result = Cursors.SizeNS;// m_cursorNorth;


            //}
            //else 
            if( m_interactMode == MouseMode.MODE_DRAWING )
	        {
		        result = Cursors.Cross;// LoadCursor( null, IDC_CROSS );
	        }
	        else if( m_objs != null)
	        {

		        int count = 0;
		        DiagramEntity obj = null;
		        //const MSG* msg = GetCurrentMessage();
                Point pt = Cursor.Position;
		        Point point = pt;// new Point(pt );
		        //ScreenToClient( &point );
                 point = this.PointToClient(point);
                 //ScreenToVirtual(ref point);
		        while( ( obj = m_objs.GetAt( count++ ) ) != null )
		        {
			        if( obj.Selected )
			        {

                        Rectangle rect = obj.Bounds;// GetRect();
                        VirtualToClient(ref rect);

				        DiagramEntity.DEHT hitCode = obj.GetHitCode( point, rect );
				        if( hitCode != DiagramEntity.DEHT.DEHT_NONE && hitCode != DiagramEntity.DEHT.DEHT_BODY )
					        result = obj.GetCursor( hitCode );

			        }
		        }

		        if( result == null )
		        {
			        DiagramEntity.DEHT  hitCode = GetHitCode( point );
			        if( hitCode != DiagramEntity.DEHT.DEHT_NONE && hitCode != DiagramEntity.DEHT.DEHT_BODY && this.BackgroundResizeSelected )
				        result = GetCursor( hitCode );
		        }
	        }

	        return result;

        }

        private WheelMode _ScrollWheelMode = WheelMode.WHEEL_ZOOM;// .WHEEL_SCROLL;
        [Browsable(false)]
        public WheelMode ScrollWheelMode
        {
            get { return _ScrollWheelMode; }
            set {
                _ScrollWheelMode = value;
            }
        }
       //public  void SetScrollWheelMode( WheelMode mode )
       // /* ============================================================
       //     Function :		SetScrollWheelMode
       //     Description :	Accessor for the scroll wheel mode flag.
       //     Access :		Public

       //     Return :		void
       //     Parameters :	int mode	-	New mode
        					
       //     Usage :			"mode" can be any of
       //                         "WHEEL_SCROLL" The editor will scroll when the scroll wheel is rolled.
       //                         "WHEEL_ZOOM" The editor will zoom when the scroll wheel is rolled.
       //    ============================================================*/
       // {

       //     m_scrollwheel = mode;

       // }

       // public WheelMode GetScrollWheelMode() 
       // /* ============================================================
       //     Function :		GetScrollWheelMode
       //     Description :	Accessor for the scroll wheel mode flag.
       //     Access :		Public

       //     Return :		int	-	Current mode
       //     Parameters :	none

       //     Usage :			The mode can be any of
       //                         "WHEEL_SCROLL" The editor will scroll when the scroll wheel is rolled.
       //                         "WHEEL_ZOOM" The editor will zoom when the scroll wheel is rolled.
       //    ============================================================*/
       // {

       //     return m_scrollwheel;

       // }

        protected void SetHScroll( int pos )
        /* ============================================================
	        Function :		SetHScroll
	        Description :	Sets the horizontal scrollbar position to 
					        "pos".
	        Access :		Protected

	        Return :		void
	        Parameters :	int pos	-	New position
        					
	        Usage :			Called when the scroll bar position is set. 
					        Can be overrided to trigger scroll bar 
					        events.

           ============================================================*/
        {

	        pos = Math.Max( 0, pos );

            //SCROLLINFO si;
            //si.cbSize = sizeof( SCROLLINFO );
            //si.fMask = SIF_POS;
            //si.nPos = pos;

            //SetScrollInfo( SB_HORZ, &si );
            Point pt = this.AutoScrollPosition;
            pt.X = pos;

            this.AutoScrollPosition = pt;//.X = pos;
        }

        protected void SetVScroll( int pos )
        /* ============================================================
	        Function :		SetVScroll
	        Description :	Sets the vertical scrollbar position to 
					        "pos".
	        Access :		Protected

	        Return :		void
	        Parameters :	int pos	-	New position
        					
	        Usage :			Called when the scroll bar position is set. 
					        Can be overrided to trigger scroll bar 
					        events.

           ============================================================*/
        {

	        pos = Math.Max( 0, pos );

            //SCROLLINFO si;
            //si.cbSize = sizeof( SCROLLINFO );
            //si.fMask = SIF_POS;
            //si.nPos = pos;

            //SetScrollInfo( SB_VERT, &si );
            Point pt = this.AutoScrollPosition;
            pt.Y = pos;
            this.AutoScrollPosition = pt;//.Y = pos;

        }

       protected  bool OutsideRestraints( Point point )
        /* ============================================================
	        Function :		OutsideRestraints
	        Description :	Checks if "point" lies outside the current 
					        restraints.
	        Access :		Protected

	        Return :		bool			-	"true" if "point" is 
										        outside the current 
										        constraints.
	        Parameters :	Point point	-	Coordinates to check.
        					
	        Usage :			Call to see if coordinates are outside the 
					        current restraints.

           ============================================================*/
        {

	        bool result = false;

            Size virtualSize = this.VirtualSize;
            if (this.Restraints == RESTRAIN.RESTRAINT_VIRTUAL)
	        {

		        if( point.X < 0 )
			        result = true;
		        if( point.Y < 0 )
			        result = true;

                if (point.X > virtualSize.Width)
			        result = true;
                if (point.Y > virtualSize.Height)
			        result = true;

	        }
            else if (this.Restraints == RESTRAIN.RESTRAINT_MARGIN)
	        {

                Padding padding = this.VirtualScreenMargin;

		        if( point.X < padding.Left)// m_leftMargin )
			        result = true;
		        if( point.Y < padding.Top)// m_topMargin )
			        result = true;

                if (point.X > virtualSize.Width - padding.Right)// m_rightMargin )
			        result = true;
                if (point.Y > virtualSize.Height - padding.Bottom)// m_bottomMargin )
			        result = true;

	        }

	        return result;

        }

       public  void ScrollIntoView()
        /* ============================================================
	        Function :		ScrollIntoView
	        Description :	Scrolls the selected items into view.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to scroll the selected item(s) into 
					        view.

           ============================================================*/
        {

	        if( GetSelectCount() >0)
	        {
                Point start = this.DiagramEntityContainer.GetStartPoint();
                Size objects = this.DiagramEntityContainer.GetTotalSize();
		        Rectangle rect = this.ClientRectangle;
		        //GetClientRect( rect );
		        Size screen = new Size( rect.Width, rect.Height );
		        if( objects.Width > screen.Width || objects.Height > screen.Height )
		        {

			        ZoomToFitScreen();

		        }
		        else
		        {

			        int leftmargin = ( screen.Width - objects.Width ) / 2;
			        int topmargin = ( screen.Height - objects.Height ) / 2;
			        int leftstart = start.X - leftmargin;
			        int topstart = start.Y - topmargin;

			        SetHScroll( leftstart );
			        SetVScroll( topstart );
			        RedrawWindow();

		        }
	        }

        }

        public void ScrollIntoView( DiagramEntity obj )
        /* ============================================================
	        Function :		ScrollIntoView
	        Description :	Scrolls "obj" into view.
	        Access :		Public

	        Return :		void
	        Parameters :	DiagramEntity obj	-	Object to scroll 
											        into view

	        Usage :			Call to scroll "obj" into view.

           ============================================================*/
        {

            Rectangle objrect = obj.Bounds;// GetRect();
            objrect = NormalizeRect(objrect);
	       // objrect.NormalizeRect();
            
	        Point start = new Point(objrect.Left, objrect.Top);//  .TopLeft();
	        Size objects = new Size( objrect.Width, objrect.Height );
	        Rectangle rect = this.ClientRectangle;
	        //GetClientRect( rect );
	        Size screen = new Size( rect.Width, rect.Height );
	        if( objects.Width > screen.Width || objects.Height > screen.Height )
	        {

		        ZoomToFitScreen();

	        }
	        else
	        {

		        int leftmargin = ( screen.Width - objects.Width ) / 2;
		        int topmargin = ( screen.Height - objects.Height ) / 2;
		        int leftstart = start.X - leftmargin;
		        int topstart = start.Y - topmargin;

		        SetHScroll( leftstart );
		        SetVScroll( topstart );
		        RedrawWindow();

	        }

        }

        public Rectangle NormalizeRect (Rectangle rect)
        {
            int l = rect.Left;
            int r = rect.Right;
            if (r <l)
            {
                //swap
                int n = l ;
                l = r;
                r = n;
            }

            int t = rect.Top;
            int b = rect.Bottom;
            if (b <t)
            {
                int n = t;
                t = b;
                b = n;
            }
            return new Rectangle(l, t, r - l, b - t);
        }


        private void SetRedraw(bool bSuspend)
        {

        }
        /************************************************************************/
        /* 
         * make selected object same with
         */
        /************************************************************************/
        public void OnEditSameSize(DiagramEntityContainer.EntitiesSameSize sameSize)
        {
            if (m_objs != null)
            {
                SetRedraw(false);
                m_objs.Snapshot();
                m_objs.SameSize(sameSize);
                SetRedraw(true);
                this.Modified = (true);
                RedrawWindow();
            }
        }
                       
                       
        
        public void OnEditAlign(DiagramEntityContainer.EntitiesAlign align)
        {
            if (m_objs != null)
            {
                SetRedraw(false);
                m_objs.Snapshot();
                m_objs.AlignPosition(align);
                SetRedraw(true);
                this.Modified = (true);
                RedrawWindow();
            }
        }
        
        virtual protected void FireShowPropertiesEvent(DiagramEntity entity)
        {
            if (OnShowDiagramEntityProperties != null)
                OnShowDiagramEntityProperties(this, entity);
        }
        virtual protected void FireChangedEvent()
        {
            if (OnChanged != null)
                OnChanged(this);
        }
        /* ============================================================
	        Function :		OnEntitySelected
	        Description :	The diagramentity was selected nomatter which select way.
         
	        Access :		Protected

	        Return :		void
	        Parameters :	DiagramEntity entity	-	this entity just changed to selected state

	        Usage :			Connect it to DiagramEntity OnEntitySelected event.

           ============================================================*/
        virtual protected void OnEntitySelected(DiagramEntity entity)
        {
            //System.Diagnostics.Debug.Print("selected");
            if (OnShowDiagramEntityProperties != null)
                OnShowDiagramEntityProperties(this, entity);
        }
        /// <summary>
        /// before change property value in entity, we need to backup them first for undo.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="bPausedEvent"></param>
        /// <param name="bJustRefreshPropertiesGrid"></param>
        virtual public void OnEntityPropertiesWillChange(DiagramEntity entity, bool bPausedEvent, bool bJustRefreshPropertiesGrid)
        {
           
          
            if (bPausedEvent)
            { //don't cause whole screen redraw, it will cause inputing caret position issues!!!     
                return;
            }
            this.m_objs.Snapshot();

          
        }
        virtual public void OnEntityPropertiesChanged(DiagramEntity entity, bool bPausedEvent, bool bJustRefreshPropertiesGrid)
        {
            //System.Diagnostics.Debug.Print("selected");
            //if (OnShowDiagramEntityProperties != null)
            //    OnShowDiagramEntityProperties(this, entity);
            //20140207, move this at top of "bPausedEvent" check.
            if (bJustRefreshPropertiesGrid)
            {
                if (OnRefreshPropertiesGrid != null)
                {
                    OnRefreshPropertiesGrid(this, entity);

                }
                //return;
            }
            ////////////////////
            if (bPausedEvent)
            { //don't cause whole screen redraw, it will cause inputing caret position issues!!!
                this.Modified = true;
                return;
            }
            if (OnChanged != null)
                OnChanged(this);
            this.Modified = true;

            m_objs.ClearRedo();

            this.RedrawWindow();
        }

        /* ============================================================
	        Function :		OnContainerAddNew
	        Description :	If container add a new entity to it, fire this event.
                            We need to some initial work for new entity.
         
	        Access :		Protected

	        Return :		void
	        Parameters :	DiagramEntity entity	-	new entity that has been add to container

	        Usage :			Connect it to container OnAddNew event.

           ============================================================*/
        virtual protected void OnContainerAddNew(DiagramEntity entity)
        {
            if (entity.OnEntitySelected == null)
                entity.OnEntitySelected += new DiagramEntity.EventEntitySelected(OnEntitySelected);

            if (entity.OnEntityPropertiesChanged == null)
                entity.OnEntityPropertiesChanged += new DiagramEntity.EventEntityPropertiesChanged(OnEntityPropertiesChanged);
            //undo
            if (entity.OnEntityPropertiesBeforeChange == null)
                entity.OnEntityPropertiesBeforeChange += new DiagramEntity.EventEntityPropertiesBeforeChange(OnEntityPropertiesWillChange);

            
        }

        public void OnEditSpacing(DiagramEntityContainer.EntitiesSpacing spacing)
        {
            m_objs.SameSpacing(spacing);
            this.RedrawWindow();
        }
        /// <summary>
        /// User tab key to move focus.
        /// 1. compare X, then Y
        /// </summary>
        public void MoveFocusToNext()
        {
            if (GetSelectCount() != 1)
                return;
            
            DiagramEntity entity = GetSelectedObject();
            DiagramEntity obj = m_objs.GetNextTabEntity(entity);

            m_objs.UnselectAll();
            if (obj == null)
                return;
            obj.Selected = true;
            this.Refresh();

        }
     


        //#region _Show_Tips
        //private Point _LastMousePosition;
        //private const float _HoverTolerance = 5.0F;
        //private const int HOVER_TIME = 500;
        //private bool _ListeningHover = false;
        //private Timer _TimerHover = new Timer();

        //private bool _ShowTips = false;
        //[Browsable(false)]
        //public bool ShowTips
        //{
        //    get { return _ShowTips; }
        //    set 
        //    { 
        //        _ShowTips = value;
        //        if (!_ShowTips)
        //        {
        //            HideTooltips();
        //        }
        //    }
        //}
        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //    if (!_ShowTips) return;
        //    _LastMousePosition = Control.MousePosition;
        //    _TimerHover.Interval = HOVER_TIME;
        //    _TimerHover.Start();
        //    _TimerHover.Tick += new EventHandler(timerHover_Tick);
        //    _ListeningHover = true; //listen to MouseMove events
        //}

        //void timerHover_Tick(object sender, EventArgs e)
        //{
        //   // OnShowTips();
        //}
        //protected override void OnMouseLeave(EventArgs e)
        //{
        //    base.OnMouseLeave(e);

        //    HideTooltips();

        //    //_LastTipEntity = null;
        //    //_ToolTip.Hide(this);
        //    //_TimerHover.Stop();
        //    //_ListeningHover = false; //stop listening
        //    //if (!_ShowTips) return;
        //}

        //protected void HideTooltips()
        //{
        //    _TimerHover.Stop();
        //    _LastTipEntity = null;
        //    _ToolTip.Hide(this);
            
        //    _ListeningHover = false; //stop listening
        //}

        //protected void CheckMouseStoped()
        //{
        //    if (!_ShowTips) return;
        //    if (_ListeningHover)
        //    {
        //        if (Math.Abs(Control.MousePosition.X - _LastMousePosition.X) > _HoverTolerance ||
        //            Math.Abs(Control.MousePosition.Y - _LastMousePosition.Y) > _HoverTolerance)
        //        {
        //            //mouse moved beyond tolerance - reset timer
        //            _TimerHover.Stop();
        //            _TimerHover.Start();
        //            _LastMousePosition = Control.MousePosition;
        //        }
        //    }
        //}
        //private ToolTip _ToolTip = new ToolTip();
        //private DiagramEntity _LastTipEntity = null;
        //protected void OnShowTips()
        //{
        //    //System.Diagnostics.Debug.Print("OnMouseHover");
            
        //    Point pt = Control.MousePosition;
        //    pt = this.PointToClient(pt);
        //    ClientToVirtual(ref pt);
        //    DiagramEntity hoverEntity = null;
        //    DiagramEntity obj = null;
        //    int count = 0;
        //    while ((obj = m_objs.GetAt(count++)) != null)
        //    {
        //        Rectangle rect = obj.Bounds;// GetRect();
        //        if (rect.Contains(pt))
        //        {
        //            hoverEntity = obj;
        //            break;
        //        }

        //    }

            
        //    if (hoverEntity == null) return;
        //    if (_ToolTip.GetToolTip(this) == null)
        //    {
        //        _LastTipEntity = null;
        //    }

        //    if (_LastTipEntity == hoverEntity) return;
        //    _LastTipEntity = hoverEntity;
        //    ShowEntityTips(hoverEntity);

        //    //_ToolTip.Hide(this);
        //    //// ToolTip toolTip = new ToolTip();
        //    //Rectangle rt = hoverEntity.Bounds;
        //    //System.Diagnostics.Debug.Print(rt.ToString());
        //    //_ToolTip.IsBalloon = true;
        //    //pt.X = rt.Left + rt.Width / 2;
        //    //pt.Y = rt.Top;
        //    //VirtualToScreen(ref pt);
        //    //System.Diagnostics.Debug.Print(pt.ToString());
        //    //_ToolTip.ShowAlways = true;
        //    //_ToolTip.Show( ((DiagramKeyArea)hoverEntity).KeyCode.ToString(), this, pt);//, 1000);
        //}
        //private void ShowEntityTips(DiagramEntity entity)
        //{
        //    _ToolTip.Hide(this);
            
        //    Rectangle rt = entity.Bounds;
        //    //System.Diagnostics.Debug.Print(rt.ToString());
        //   // _ToolTip.IsBalloon = true;
        //    Point pt = new Point();
        //    pt.X = rt.Left + rt.Width / 3;
        //    pt.Y = rt.Top+rt.Height /10;
        //    VirtualToClient(ref pt);
        //   // System.Diagnostics.Debug.Print(pt.ToString());
        //    _ToolTip.ShowAlways = true;
        //    _ToolTip.ToolTipTitle = entity.Title;
        //    // Point ptScreen = Control.MousePosition;
        //    //Point ptClient = this.PointToClient(ptScreen);
        //    //_ToolTip.Show(((DiagramKeyArea)entity).KeyCode.ToString(), this,  pt);
        //    _ToolTip.Show(entity.GetTooltipsText(), this, pt);
        //}
        //#endregion
        private bool m_bCancelMouseDownEvent = false;

        public void ResetMouseEvent()
        {
            this.Capture = false;
            m_interactMode = MouseMode.MODE_NONE;
            m_subMode = DiagramEntity.DEHT.DEHT_NONE;
            m_bCancelMouseDownEvent = true;
        }

        //protected override void OnLeave(EventArgs e)
        //{
        //    base.OnLeave(e);
        //    ResetMouseEvent();
        //}

        public void SetSelectedEntity(DiagramEntity entity)
        {
            entity.Selected = true;
        }

        public string CreateUniqueName(string keyTypeName)
        {
            return m_objs.get_key_unique_title(keyTypeName);
        }

        #region Shadow unneeded properties.

        [Browsable(false)]
        public new string AccessibleDescription { get { return base.AccessibleDescription; } set { base.AccessibleDescription = value; } }
        [Browsable(false)]
        public new string AccessibleName { get { return base.AccessibleName; } set { base.AccessibleName = value; } }
        [Browsable(false)]
        public new AccessibleRole AccessibleRole { get { return base.AccessibleRole; } set { base.AccessibleRole = value; } }
        [Browsable(false)]
        public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }
        [Browsable(false)]
        public new ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }
        [Browsable(false)]
        public new Cursor Cursor { get { return base.Cursor; } set { base.Cursor = value; } }

        [Browsable(false)]
        public new RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

        [Browsable(false)]
        public new string Text { get { return base.Text; } set { base.Text = value; } }
        [Browsable(false)]
        public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

        [Browsable(false)]
        public new bool AllowDrop { get { return base.AllowDrop; } set { base.AllowDrop = value; } }

        [Browsable(false)]
        public new ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
        [Browsable(false)]
        public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

        [Browsable(false)]
        public new ControlBindingsCollection DataBindings { get { return base.DataBindings; } }// set { base.DataBindings = value; } }
        [Browsable(false)]
        public new bool Enabled { get { return base.Enabled; } set { base.Enabled = value; } }
        [Browsable(false)]
        public new int TabIndex { get { return base.TabIndex; } set { base.TabIndex = value; } }
        [Browsable(false)]
        public new bool Visible { get { return base.Visible; } set { base.Visible = value; } }
        [Browsable(false)]
        public new bool TabStop { get { return base.TabStop; } set { base.TabStop = value; } }
        [Browsable(false)]
        public new object Tag { get { return base.Tag; } set { base.Tag = value; } }

        [Browsable(false)]
        public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

        [Browsable(false)]
        public new AnchorStyles Anchor { get { return base.Anchor; } set { base.Anchor = value; } }
         [Browsable(false)]
        public new bool AutoScroll { get { return base.AutoScroll; } set { base.AutoScroll = value; } }
        [Browsable(false)]
        public new Size AutoScrollMargin { get { return base.AutoScrollMargin; } set { base.AutoScrollMargin = value; } }
        [Browsable(false)]
        public new Size AutoScrollMinSize { get { return base.AutoScrollMinSize; } set { base.AutoScrollMinSize = value; } }


        [Browsable(false)]
        public new DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

        [Browsable(false)]
        public new Point Location { get { return base.Location; } set { base.Location = value; } }
        [Browsable(false)]
        public new Padding Margin { get { return base.Margin; } set { base.Margin = value; } }
        [Browsable(false)]
        public new Padding Padding { get { return base.Padding; } set { base.Padding = value; } }

        [Browsable(false)]
        public new Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }
        [Browsable(false)]
        public new Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }
        [Browsable(false)]
        public new Size Size { get { return base.Size; } set { base.Size = value; } }

        [Browsable(false)]
        public new Font Font { get { return base.Font; } set { base.Font = value; } }
        [Browsable(false)]
        public new Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }
        #endregion
    }
}

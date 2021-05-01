using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel;
using System.IO;
using System.Drawing.Drawing2D;

namespace KB9Utility
{
    public class DiagramEntity
    {

        public delegate void EventEntitySelected(DiagramEntity entity);
        public EventEntitySelected OnEntitySelected;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="bPausedEvent">if program has disable firing event</param>
        public delegate void EventEntityPropertiesChanged(DiagramEntity entity, bool bPausedEvent, bool bJustRefreshPropertiesGrid);
        public EventEntityPropertiesChanged OnEntityPropertiesChanged;

        //the new properties will been set to entity, 
        //use it for undo/redo
        public delegate void EventEntityPropertiesBeforeChange(DiagramEntity entity, bool bPausedEvent, bool bJustRefreshPropertiesGrid);
        public EventEntityPropertiesBeforeChange OnEntityPropertiesBeforeChange;

        private bool _PauseEvent = false;
        [Browsable(false)]
        public bool PauseEvent
        {
            get 
            {
                return _PauseEvent;
            }
            set
            {
                _PauseEvent = value;
            }
        }

        public enum EntityCmd{
            CMD_START		=	100,
            CMD_CUT				,
            CMD_COPY			,
            CMD_DUPLICATE		,
            CMD_PROPERTIES		,
            CMD_UP				,
            CMD_DOWN			,
            CMD_FRONT			,
            CMD_BOTTOM			,
            CMD_SELECT_GROUP	,
            CMD_END			,
        };
        public enum DEHT{
             DEHT_NONE		=		0,
             DEHT_BODY				,
             DEHT_TOPLEFT			,
             DEHT_TOPMIDDLE			,
             DEHT_TOPRIGHT			,
             DEHT_BOTTOMLEFT		,
             DEHT_BOTTOMMIDDLE		,
             DEHT_BOTTOMRIGHT		,
             DEHT_LEFTMIDDLE		,
             DEHT_RIGHTMIDDLE		
        }


        private string _GUID = "";
        public void setID(string strID)
        {
            _GUID = strID;
        }
        public string getID()
        {
            return _GUID;
        }
        static public string createNewGUID()
        {
            return System.Guid.NewGuid().ToString();

        }
        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEntity

       public DiagramEntity()
        /* ============================================================
	        Function :		CDiagramEntity::CDiagramEntity
	        Description :	Constructor
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			

           ============================================================*/
        {
            this.setID(createNewGUID());
            this.ParentContainer = (null);
	        //SetPropertyDialog( null, 0 );
	        Clear();
	        this.TypeName = ( ( "basic" ) );

            this.GroupID = (0);
        }

        ~DiagramEntity()
        /* ============================================================
	        Function :		CDiagramEntity::~CDiagramEntity
	        Description :	Destructor
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			

           ============================================================*/
        {
        }

        public void Clear()
        /* ============================================================
	        Function :		CDiagramEntity::Clear
	        Description :	Zero all properties of this object.
	        Access :		Protected

	        Return :		void
	        Parameters :	none

	        Usage :			Call to initialize the object.

           ============================================================*/
        {

	        //SetRect( 0.0, 0.0, 0.0, 0.0 );
            this.Bounds = new Rectangle();
            this.MarkerSize = (new Size(KB9Const.MARKER_SIZE, KB9Const.MARKER_SIZE));
	        SetConstraints(new  Size( 1, 1 ), new Size( -1, -1 ) );
            this.ParentContainer = (null);	        
            Selected = ( false );

	        //this.Name = ( ""  );
            _SelectedMain = false;

        }

        virtual public DiagramEntity Clone()
        /* ============================================================
	        Function :		CDiagramEntity::Clone
	        Description :	Clone this object to a new object.
	        Access :		Public

	        Return :		CDiagramEntity*	-	The new object.
	        Parameters :	none

	        Usage :			Call to create a clone of the object. The 
					        caller will have to delete the object.

           ============================================================*/
        {
            DiagramEntity obj = new DiagramEntity();
	        obj.Copy( this );
            //obj.Selected = this.Selected;
	        return obj;
        }

        public virtual void Copy(DiagramEntity obj)
        /* ============================================================
	        Function :		CDiagramEntity::Copy
	        Description :	Copy the information in "obj" to this object.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to copy 
											        from.
        					
	        Usage :			Copies basic information. from "obj" to this.
					        "GetType" can be used to check for the correct 
					        object type in overridden versions.
           ============================================================*/
        {

	        Clear();

	        this.MarkerSize = ( obj.MarkerSize );
	        SetConstraints( obj.MinimumSize, obj.MaximumSize );

	        this.Selected = ( obj.Selected );//must call this first, then setparent.

            this.ParentContainer = (obj.ParentContainer);
	        this.TypeName  = ( obj.TypeName);//GetEntityType() );
	        this.Title = ( obj.Title );
            //this.Name = (obj.Name);
            this.Bounds = obj.Bounds;
            this.setID(obj.getID());

	        //SetRect( obj.GetLeft(), obj.GetTop(), obj.GetRight(), obj.GetBottom() );

        }

        //
        /// <summary>
        /// [Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>
        /// e.g: [35,6,60,41][0][0][2]<0x0d><0x0a>
        /// </summary>
        /// <returns></returns>
        public virtual string ExportCVS()
        {

            return "";
        }

        public virtual bool FromXml(  CLCIXML xml )
        /* ============================================================
	        Function :		CDiagramEntity::FromString
	        Description :	Sets the values for an object from "str". 
	        Access :		Public

	        Return :		BOOL				-	"TRUE" if "str" 
											        represents an 
											        object of this 
											        type.
	        Parameters :	const CString& str	-	Possible text 
											        format 
											        representation.
        					
	        Usage :			Can be called to fill an existing object 
					        with information from a string created with 
					        "GetString".

           ============================================================*/
        {

            string s = "";
            
            
            
            Rectangle rt = new Rectangle(0,0,0,0);

            xml.get_attribute("x", ref s);
            rt.X = int.Parse(s);

            xml.get_attribute("y", ref s );
            rt.Y = int.Parse(s);

            xml.get_attribute("w", ref s);
            rt.Width = int.Parse(s);

            xml.get_attribute("h", ref s);
            rt.Height = int.Parse(s);
            this.PhyBounds = rt;

            xml.get_attribute("text", ref s);
            this.Title = s;

            return true;

            //bool result = false;

            ////CString data( str );
            ////CString header = GetHeaderFromString( data );
            ////if( header == GetType() )
            ////    if( GetDefaultFromString( data ) )
            //        result = true;

            //return result;

        }


        //???????????????????????????????????????????????
        public void SetRect(int left, int top, int right, int bottom)
        /* ============================================================
            Function :		CDiagramEntity::SetRect
            Description :	Sets the object rectangle.
            Access :		Public

            Return :		void
            Parameters :	double left		-	Left edge
                            double top		-	Top edge
                            double right	-	Right edge
                            double bottom	-	Bottom edge
        					
            Usage :			Call to place the object.

           ============================================================*/
        {

            Rectangle rt = new Rectangle(left, top, right - left, bottom - top);
            this.Bounds = rt;
            //System.Diagnostics.Debug.Print("--------------------------------------------");
            //System.Diagnostics.Debug.Print("l=" + this.Location.Left.ToString());

            //System.Diagnostics.Debug.Print("t=" + this.Location.Top.ToString());
            //System.Diagnostics.Debug.Print("w=" + this.Location.Width.ToString());
            //System.Diagnostics.Debug.Print("h=" + this.Location.Height.ToString());

            //SetLeft(left);
            //SetTop(top);
            //SetRight(right);
            //SetBottom(bottom);

            //if (GetMinimumSize().Width != -1)
            //    if (GetRect().Width < GetMinimumSize().Width)
            //        SetRight(GetLeft() + GetMinimumSize().Width);

            //if (GetMinimumSize().Height != -1)
            //    if (GetRect().Height < GetMinimumSize().Height)
            //        SetBottom(GetTop() + GetMinimumSize().Height);

            //if (GetMaximumSize().Width != -1)
            //    if (GetRect().Width > GetMaximumSize().Width)
            //        SetRight(GetLeft() + GetMaximumSize().Width);

            //if (GetMaximumSize().Height != -1)
            //    if (GetRect().Height > GetMaximumSize().Height)
            //        SetBottom(GetTop() + GetMaximumSize().Height);

            //if (GetPropertyDialog() != null)
            //    GetPropertyDialog().SetValues();

        }

        public void MoveRect( double x, double y )
        /* ============================================================
	        Function :		CDiagramEntity::MoveRect
	        Description :	Moves the object rectangle.
	        Access :		Public

	        Return :		void
	        Parameters :	double x	-	Move x steps horizontally.
					        double y	-	Move y steps vertically.
        					
	        Usage :			Call to move the object on screen.

           ============================================================*/
        {
            Rectangle rt = this.Bounds;
            rt.Offset((int)x, (int)y);
            this.Bounds = rt;
	        //SetRect( GetLeft() + x, GetTop() + y, GetRight() + x, GetBottom() + y );

        }

        private bool _Selected = false;
        [Browsable(false)]
        virtual public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                bool bOldValue = _Selected;
                _Selected = value;

                if (value && (this.GroupID != 0))
                {
                    DiagramEntityContainer parent =this.ParentContainer;
                    if (parent != null)
                        parent.SendMessageToObjects((EntityCmd.CMD_SELECT_GROUP), false, this, null);
                }
                //
                if (_Selected && !bOldValue && this.ParentContainer != null)
                {

                   // System.Diagnostics.Debug.Print("OnEntitySelected");
                    if (OnEntitySelected != null)
                        OnEntitySelected(this);
                }
                if (!_Selected)
                {
                    this.SelectedMain = false;
                }
            }
        }

        //public void Select( bool selected )
        ///* ============================================================
        //    Function :		CDiagramEntity::Select
        //    Description :	Sets the object select state.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	BOOL selected	-	"TRUE" to select, "FALSE" 
        //                                        to unselect.
        					
        //    Usage :			Call to select/deselect the object.

        //   ============================================================*/
        //{

        //    m_selected = selected;

        //    if( selected && (GetGroup() !=0) )
        //    {
        //        DiagramEntityContainer parent = GetParent();
        //        if( parent != null )
        //            parent.SendMessageToObjects(( EntityCmd.CMD_SELECT_GROUP ), false, this, null );
        //    }

        //}

        //public bool IsSelected() 
        ///* ============================================================
        //    Function :		CDiagramEntity::IsSelected
        //    Description :	Checks if the object is selected.
        //    Access :		Public

        //    Return :		BOOL	-	"TRUE" if the object is selected.
        //    Parameters :	none

        //    Usage :			Call to see if the object is selected.

        //   ============================================================*/
        //{

        //    return m_selected;

        //}

        public bool BodyInRect( Rectangle rect ) 
        /* ============================================================
	        Function :		CDiagramEntity::BodyInRect
	        Description :	Used to see if any part of the object lies 
					        in "rect".
	        Access :		Public

	        Return :		BOOL		-	"TRUE" if any part of the 
									        object lies inside rect.
	        Parameters :	CRect rect	-	The rectangle to check.
        					
	        Usage :			Call to see if the object overlaps - for 
					        example - a selection rubberband.

           ============================================================*/
        {

	        bool result = false;
            Rectangle rectEntity = this.Bounds;// GetRect();
	        Rectangle rectIntersect;

	        //rect.NormalizeRect();
	        //rectEntity.NormalizeRect();
            
	        rectIntersect = Rectangle.Intersect( rect, rectEntity );
	        if( !rectIntersect.IsEmpty )
		        result = true;

	        return result;

        }

        virtual public DEHT GetHitCode(Point point) 
        /* ============================================================
	        Function :		CDiagramEntity::GetHitCode
	        Description :	Returns the hit point constant for "point".
	        Access :		Public

	        Return :		int				-	The hit point, 
										        "DEHT_NONE" if none.
	        Parameters :	CPoint point	-	The point to check
        					
	        Usage :			Call to see in what part of the object point 
					        lies. The hit point can be one of the following:
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

            Rectangle rect = this.Bounds;// GetRect();
	        return GetHitCode( point, rect );

        }

        /* ============================================================
           Function :		CDiagramEntity::DoMessage
           Description :	Message handler for the object.
           Access :		Public

           Return :		BOOL					-	"TRUE" to stop 
                                                       further processing.
           Parameters :	UINT msg				-	The message.
                           CDiagramEntity* sender	-	Original sender of 
                                                       this message, or 
                                                       "NULL" if not an object.

           Usage :			The container can send messages to all 
                           objects. The messages should lie in the 
                           range "CMD_START" to "CMD_STOP" inclusively - 
                           a few are already predefined in 
                           DiagramEntity.h. This function will be 
                           called as response to those messages. This 
                           mechanism is already used for sending back 
                           messages from "CDiagramEditor" to the 
                           relevant object when a object popup menu 
                           alternative is selected.

          ============================================================*/
        protected void ContextMenu_Click(System.Object sender, System.EventArgs e)
        {
            // Insert code to handle Click event.
            string s = sender.ToString();
            MenuItem menuItem = (MenuItem)sender;
            EntityCmd command = (EntityCmd)menuItem.Tag;
            DoMessage(command, this, null);
        }



        public bool  DoMessage( EntityCmd command, DiagramEntity sender, Control from )
        {

	        bool stop = false;

            switch (command)
	        {
		        case EntityCmd.CMD_CUT:
                    if (this.ParentContainer != null && this.Selected)
			        {
				        stop = true;
                        this.ParentContainer.Cut(this);
			        }
		        break;

		        case EntityCmd.CMD_COPY:
                    if (this.ParentContainer != null && this.Selected)
			        {
                        stop = true;
                        this.ParentContainer.Copy(this);
			        }
		        break;

		        case EntityCmd.CMD_UP:
                    if (this.ParentContainer != null && this.Selected)
			        {
                        stop = true;
                        this.ParentContainer.Up(this);
			        }
		        break;

		        case EntityCmd.CMD_DOWN:
                    if (this.ParentContainer != null && this.Selected)
			        {
                        stop = true;
                        this.ParentContainer.Down(this);
			        }
		        break;

		        case EntityCmd.CMD_FRONT:
                    if (this.ParentContainer != null && this.Selected)
			        {
                        stop = true;
                        this.ParentContainer.Front(this);
			        }
		        break;

		        case EntityCmd.CMD_BOTTOM:
                    if (this.ParentContainer != null && this.Selected)
			        {
                        stop = true;
                        this.ParentContainer.Bottom(this);
			        }
		        break;

		        case EntityCmd.CMD_DUPLICATE:
                    if (this.ParentContainer != null && this.Selected)
			        {
                        stop = true;
                        this.ParentContainer.Duplicate(this);
				        Selected = ( false );
			        }
		        break;

		        case EntityCmd.CMD_PROPERTIES:
                    if (this.Selected)
			        {
				        //ShowProperties( from, true);
                        stop = true;
			        }
		        break;

		        case EntityCmd.CMD_SELECT_GROUP:
			        if( sender != this )
                        if (sender.GroupID == this.GroupID)
                        {
                            //m_selected = true;
                            _Selected = true;
                        }
		        break;

	        }

	        return stop;

        }

        virtual public void OnMatrixUngroup_Click(object sender, EventArgs e)
        {
        }

        public void ShowPopup( Point point, Control parent )
        /* ============================================================
	        Function :		CDiagramEntity::ShowPopup
	        Description :	Shows the popup menu for the object.
	        Access :		Public

	        Return :		void
	        Parameters :	CPoint point	-	The point to track.
					        CWnd* parent	-	The parent "CWnd" of the 
										        menu (should be the 
										        "CDiagramEditor")

	        Usage :			The function uses hardcoded strings to 
					        avoid having to include resource file 
					        fragments. Derived classes needing a non-
					        standard or localized menu should load 
					        menues from resources instead.

           ============================================================*/
        {

            frmMain frm = frmMain.g_MainForm;


            ContextMenuStrip menu = new ContextMenuStrip();
            
            menu.Items.Add(DiagramEditor.CloneMenuItem(frm.editCut, frm.editCut_Click, ""));
            menu.Items.Add(DiagramEditor.CloneMenuItem(frm.editCopy, frm.editCopy_Click, ""));

            menu.Items.Add(DiagramEditor.CloneMenuItem(frm.editDel, frm.editDel_Click, ""));
            //for matrix
            if (this.TypeName == DiagramKeyMatrix.KEY_MATRIX)
            {
                menu.Items.Add("-");
                menu.Items.Add("Ungroup", null, OnMatrixUngroup_Click);


            }
            /*
            ////if( menu.CreatePopupMenu() )
            //{
            MenuItem item = new MenuItem("Cut" , new EventHandler(ContextMenu_Click));
            item.Tag = EntityCmd.CMD_CUT;
            menu.MenuItems.Add( item);//"Cut" , new EventHandler(ContextMenu_Click)   );

            item = new MenuItem("Copy", new EventHandler(ContextMenu_Click));
            item.Tag = EntityCmd.CMD_COPY;
            menu.MenuItems.Add(item);//
            */
            //item = new MenuItem("Duplicate", new EventHandler(ContextMenu_Click));
            //item.Tag = EntityCmd.CMD_DUPLICATE;
            //menu.MenuItems.Add(item);//

            //menu.MenuItems.Add("-");

            //item = new MenuItem("Up", new EventHandler(ContextMenu_Click));
            //item.Tag = EntityCmd.CMD_UP;
            //menu.MenuItems.Add(item);//

            //item = new MenuItem("Down", new EventHandler(ContextMenu_Click));
            //item.Tag = EntityCmd.CMD_DOWN;
            //menu.MenuItems.Add(item);//

            //item = new MenuItem("To front", new EventHandler(ContextMenu_Click));
            //item.Tag = EntityCmd.CMD_FRONT;
            //menu.MenuItems.Add(item);//

            //item = new MenuItem("To back", new EventHandler(ContextMenu_Click));
            //item.Tag = EntityCmd.CMD_BOTTOM;
            //menu.MenuItems.Add(item);//

            //menu.MenuItems.Add("-");

            //item = new MenuItem("Properties", new EventHandler(ContextMenu_Click));
            //item.Tag = EntityCmd.CMD_BOTTOM;
            //menu.MenuItems.Add(item);//

            //menu.MenuItems.Add( "Copy"  , new EventHandler(ContextMenu_Click));
            //menu.MenuItems.Add("Duplicate", new EventHandler(ContextMenu_Click));
            //menu.MenuItems.Add("-");
            //menu.MenuItems.Add("Up", new EventHandler(ContextMenu_Click));
            //menu.MenuItems.Add("Down" , new EventHandler(ContextMenu_Click) );
            //menu.MenuItems.Add("To front" , new EventHandler(ContextMenu_Click) );
            //menu.MenuItems.Add("To back" , new EventHandler(ContextMenu_Click) );
            //menu.MenuItems.Add("-");
            //menu.MenuItems.Add("Properties" , new EventHandler(ContextMenu_Click) );
            menu.Show(parent, point);
            WinAPI.PostMessage(parent.Handle, WinAPI.UM_REFRESH, 0, 0);
            //    menu.TrackPopupMenu(TPM_LEFTALIGN | TPM_RIGHTBUTTON, point.x, point.y, parent );

            //}

        }

        //public void ShowProperties( Control parent, bool show )
        ///* ============================================================
        //    Function :		CDiagramEntity::ShowProperties
        //    Description :	Shows the property dialog for the object.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	CWnd* parent	-	Parent of the dialog
        //                    BOOL show		-	"TRUE" to show, "FALSE" 
        //                                        to hide.

        //    Usage :			Call to show the property dialog for this 
        //                    object.

        //   ============================================================*/
        //{

        //    //if( m_propertydlg )
        //    //{
        //    //    if( show )
        //    //    {
        //    //        if( !m_propertydlg->m_hWnd )
        //    //            m_propertydlg->Create( ( UINT ) m_propertydlgresid, parent );

        //    //        m_propertydlg->ShowWindow( SW_SHOW );
        //    //        m_propertydlg->SetValues();
        //    //        m_propertydlg->SetFocus();
        //    //    }
        //    //    else
        //    //        if( m_propertydlg->m_hWnd )
        //    //            m_propertydlg->ShowWindow( SW_HIDE );
        //    //}

        //}
        protected Rectangle ZoomRectangle(Rectangle rect, double zoom)
        {
            //make sure it is exactly same as grid
            

            Rectangle rt = new Rectangle((int)decimal.Round((decimal)rect.Left * (decimal)zoom),   
                                            (int)decimal.Round((decimal)rect.Top * (decimal)zoom),
                                             (int)decimal.Round((decimal)rect.Width * (decimal)zoom),
                                            (int)decimal.Round((decimal)rect.Height * (decimal)zoom));
            return rt;
        }
        /************************************************************************/
        /* 
         * 
         * Make sure the border fit to the grid
         */
        /************************************************************************/
        protected Rectangle ZoomDrawingRectangle( double zoom)
        {
            //make sure it is exactly same as grid

            Rectangle rect = this.PhyBounds;
            //reset its zero position, make sure it is same as grid
            rect.X -= KB9Const.UNITS_MARGIN;
            rect.Y -= KB9Const.UNITS_MARGIN;
            
            //convert to pixel rect
            rect.X *= KB9Const.UNIT_PIXELS;
            rect.Y *= KB9Const.UNIT_PIXELS;
            rect.Width *= KB9Const.UNIT_PIXELS;
            rect.Height *= KB9Const.UNIT_PIXELS;
            //for zoom width, and height
            int nRight = rect.Right;
            int nBottom = rect.Bottom;

            int w =(int)decimal.Round( (decimal)nRight * (decimal)zoom - (decimal)rect.Left * (decimal)zoom);
            int h =(int)decimal.Round( (decimal)nBottom * (decimal)zoom - (decimal)rect.Top * (decimal)zoom);


            //zoom rect
            rect.X = (int)decimal.Round((decimal)rect.Left * (decimal)zoom);
            rect.Y = (int)decimal.Round((decimal)rect.Top * (decimal)zoom);
            //zoom for width and height
            //nRight = (int)decimal.Round((decimal)nRight * (decimal)zoom);
            //nBottom = (int)decimal.Round((decimal)nBottom * (decimal)zoom);

            rect.Width =w;// nRight - rect.Left;// (int)decimal.Round((decimal)rect.Width * (decimal)zoom);
            rect.Height =h;// nBottom - rect.Top;// (int)decimal.Round((decimal)rect.Height * (decimal)zoom);

            //zoom margin
            int margin = (int)(decimal.Round( (decimal)KB9Const.PIXELS_MARGIN * (decimal)zoom));
            rect.X += margin;
            rect.Y += margin;

            return rect;
            /*

            int l = (rect.Left - KB9Const.PIXELS_MARGIN) / KB9Const.UNIT_PIXELS;
            if (l % 2 == 0)
                l = DEditor.m_lstX[l / 2] + KB9Const.PIXELS_MARGIN;
            else
                l = (int)decimal.Round((decimal)rect.Left * (decimal)zoom);

            
            Rectangle rt = new Rectangle(l,//(int)decimal.Round((decimal)rect.Left * (decimal)zoom),
                                            (int)decimal.Round((decimal)rect.Top * (decimal)zoom),
                                             (int)decimal.Round((decimal)rect.Width * (decimal)zoom),
                                            (int)decimal.Round((decimal)rect.Height * (decimal)zoom));
            return rt;
             * */
        }

        /************************************************************************/
        /* 
         * 
         * Make sure the border fit to the grid
         */
        /************************************************************************/
        protected Rectangle ZoomDrawingRectangle2(double zoom)
        {
            //make sure it is exactly same as grid

            Rectangle rect = this.PhyBounds;
            //reset its zero position, make sure it is same as grid
            rect.X -= KB9Const.UNITS_MARGIN;
            rect.Y -= KB9Const.UNITS_MARGIN;

            int nRight = rect.Right;
            int nBottom = rect.Bottom;

            rect.X = DEditor.m_lstX[rect.X];
            rect.Y = DEditor.m_lstY[rect.Y];

            nRight = DEditor.m_lstX[nRight];
            nBottom = DEditor.m_lstY[nBottom];

            rect.Width = nRight - rect.X;
            rect.Height = nBottom - rect.Y;


       /*     //convert to pixel rect
            rect.X *= KB9Const.UNIT_PIXELS;
            rect.Y *= KB9Const.UNIT_PIXELS;
            rect.Width *= KB9Const.UNIT_PIXELS;
            rect.Height *= KB9Const.UNIT_PIXELS;
            //for zoom width, and height
            int nRight = rect.Right;
            int nBottom = rect.Bottom;

            int w = (int)decimal.Round((decimal)nRight * (decimal)zoom - (decimal)rect.Left * (decimal)zoom);
            int h = (int)decimal.Round((decimal)nBottom * (decimal)zoom - (decimal)rect.Top * (decimal)zoom);


            //zoom rect
            rect.X = (int)decimal.Round((decimal)rect.Left * (decimal)zoom);
            rect.Y = (int)decimal.Round((decimal)rect.Top * (decimal)zoom);
            //zoom for width and height
            //nRight = (int)decimal.Round((decimal)nRight * (decimal)zoom);
            //nBottom = (int)decimal.Round((decimal)nBottom * (decimal)zoom);

            rect.Width = w;// nRight - rect.Left;// (int)decimal.Round((decimal)rect.Width * (decimal)zoom);
            rect.Height = h;// nBottom - rect.Top;// (int)decimal.Round((decimal)rect.Height * (decimal)zoom);

            //zoom margin
            int margin = (int)(decimal.Round((decimal)KB9Const.PIXELS_MARGIN * (decimal)zoom));
            rect.X += margin;
            rect.Y += margin;
            */
            return rect;
            /*

            int l = (rect.Left - KB9Const.PIXELS_MARGIN) / KB9Const.UNIT_PIXELS;
            if (l % 2 == 0)
                l = DEditor.m_lstX[l / 2] + KB9Const.PIXELS_MARGIN;
            else
                l = (int)decimal.Round((decimal)rect.Left * (decimal)zoom);

            
            Rectangle rt = new Rectangle(l,//(int)decimal.Round((decimal)rect.Left * (decimal)zoom),
                                            (int)decimal.Round((decimal)rect.Top * (decimal)zoom),
                                             (int)decimal.Round((decimal)rect.Width * (decimal)zoom),
                                            (int)decimal.Round((decimal)rect.Height * (decimal)zoom));
            return rt;
             * */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="totalRect"></param>
        /// <param name="zoom"></param>
        /// <param name="bToImage"></param>
        public virtual void DrawObject(Graphics g, Rectangle rectTotalVirtualWithOffsetAndZoom, double zoom, bool bToImage)
        {

            this.Zoom = (zoom);

            Rectangle rect ;//= null;
            if (!bToImage)
            {

                //the old zoomrectangle can not fit the border to grid, 
                //so I write a zoomdrawingrectangle to fix this bug.
                //rect = ZoomRectangle(this.Bounds, zoom);
                rect = ZoomDrawingRectangle2(zoom);
            }
            else
                rect = ZoomRectangle(this.ImgBounds, zoom);

            rect.Offset(rectTotalVirtualWithOffsetAndZoom.Location);
            
	        Draw( g, rect, zoom );

	        if( this.Selected )
		        DrawSelectionMarkers( g, rect );

        }
        /// <summary>
        /// ??????????????????????????????
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        protected virtual void Draw( Graphics g, Rectangle rectVirtualEntityWithOffsetAndZoom,double zoom )
        /* ============================================================
	        Function :		CDiagramEntity::Draw
	        Description :	Draws the object.
	        Access :		Public

	        Return :		void
	        Parameters :	CDC* dc		-	The "CDC" to draw to. 
					        CRect rect	-	The real rectangle of the 
									        object.

	        Usage :			The function should clean up all selected 
					        objects. Note that the "CDC" is a memory "CDC", 
					        so creating a memory "CDC" in this function 
					        will probably not speed up the function.

           ============================================================*/
        {

            //dc->SelectStockObject( BLACK_PEN );
            //dc->SelectStockObject( WHITE_BRUSH );

            //dc->Rectangle( rect );


        }

       public Cursor GetCursor(DEHT hit) 
        /* ============================================================
	        Function :		CDiagramEntity::GetCursor
	        Description :	Returns the cursor for the given hit point.
	        Access :		Public

	        Return :		HCURSOR	-	The cursor to show
	        Parameters :	int hit	-	The hit point constant ("DEHT_") 
								        to get the cursor for.

	        Usage :			Call to get the cursor for a specific hit 
					        point constant.
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
	        Cursor cursor = null;
	        switch( (DEHT)hit )
	        {
		        case DEHT.DEHT_BODY:
			        cursor = Cursors.SizeAll;// LoadCursor( NULL, IDC_SIZEALL );
		        break;
		        case DEHT.DEHT_TOPLEFT:
			        cursor =Cursors.SizeNWSE;// LoadCursor( NULL, IDC_SIZENWSE );
		        break;
		        case DEHT.DEHT_TOPMIDDLE:
			        cursor =Cursors.SizeNS;// LoadCursor( NULL, IDC_SIZENS );
		        break;
		        case DEHT.DEHT_TOPRIGHT:
			        cursor =Cursors.SizeNESW;// LoadCursor( NULL, IDC_SIZENESW );
		        break;
		        case DEHT.DEHT_BOTTOMLEFT:
			        cursor =Cursors.SizeNESW;// LoadCursor( NULL, IDC_SIZENESW );
		        break;
		        case DEHT.DEHT_BOTTOMMIDDLE:
			        cursor =Cursors.SizeNS;// LoadCursor( NULL, IDC_SIZENS );
		        break;
		        case DEHT.DEHT_BOTTOMRIGHT:
			        cursor =Cursors.SizeNWSE;// LoadCursor( NULL, IDC_SIZENWSE );
		        break;
		        case DEHT.DEHT_LEFTMIDDLE:
			        cursor =Cursors.SizeWE;// LoadCursor( NULL, IDC_SIZEWE );
		        break;
		        case DEHT.DEHT_RIGHTMIDDLE:
			        cursor =Cursors.SizeWE;// LoadCursor( NULL, IDC_SIZEWE );
		        break;
	        }
	        return cursor;
        }

        public void DrawSelectionMarkers( Graphics g, Rectangle rect )
        /* ============================================================
	        Function :		CDiagramEntity::DrawSelectionMarkers
	        Description :	Draws the selection markers for the 
					        object.
	        Access :		Protected

        					
	        Return :		void
	        Parameters :	CDC* dc		-	The "CDC" to draw to
					        CRect rect	-	The real object rectangle.
        					
	        Usage :			"rect" is the true rectangle (zoomed) of the 
					        object.

           ============================================================*/
        {

	        // Draw selection markers
	        Rectangle rectSelect = new Rectangle();

            Color color = Color.Blue;// .FromArgb(220, 236, 248);
            Pen pen = new Pen(color);//Color.Black);
            SolidBrush br = new SolidBrush(color);
            Rectangle rtMarker = new Rectangle(0, 0, KB9Const.MARKER_SIZE, KB9Const.MARKER_SIZE);
            LinearGradientBrush brGradient = new LinearGradientBrush(rtMarker, color, Color.White, LinearGradientMode.Vertical);

            List<DEHT> ar = new List<DEHT>();

            ar.Add(DEHT.DEHT_TOPLEFT);
            ar.Add(DEHT.DEHT_TOPMIDDLE);

            ar.Add(DEHT.DEHT_TOPRIGHT);
            ar.Add(DEHT.DEHT_BOTTOMLEFT);
            ar.Add(DEHT.DEHT_BOTTOMMIDDLE);
            ar.Add(DEHT.DEHT_BOTTOMRIGHT);
            ar.Add(DEHT.DEHT_RIGHTMIDDLE);
            ar.Add(DEHT.DEHT_LEFTMIDDLE);

            for (int i = 0; i < ar.Count; i++)
            {
                rectSelect = GetSelectionMarkerRect(ar[i], rect);
                if (this.SelectedMain)
                {
                    g.DrawRectangle(pen, rectSelect);
                }
                else
                    g.FillRectangle(br, rectSelect);
            }
            /*
                rectSelect = GetSelectionMarkerRect(DEHT.DEHT_TOPLEFT, rect);
                g.FillRectangle(br, rectSelect );

	        //dc->Rectangle( rectSelect );
            
	        rectSelect = GetSelectionMarkerRect( DEHT.DEHT_TOPMIDDLE, rect );
	        //dc->Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);
            //g.DrawRectangle(pen, rectSelect );

	        rectSelect = GetSelectionMarkerRect( DEHT.DEHT_TOPRIGHT, rect );
	        //dc->Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);
            //g.DrawRectangle(pen, rectSelect );

	        rectSelect = GetSelectionMarkerRect( DEHT.DEHT_BOTTOMLEFT, rect );
	        //dc->Rectangle( rectSelect );
            //g.DrawRectangle(pen, rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DEHT.DEHT_BOTTOMMIDDLE, rect );
	        //dc->Rectangle( rectSelect );
            //g.DrawRectangle(pen, rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DEHT.DEHT_BOTTOMRIGHT, rect );
	        //g.DrawRectangle(pen, rectSelect );//dc->Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DEHT.DEHT_RIGHTMIDDLE, rect );
	        //g.DrawRectangle(pen, rectSelect );//dc->Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);

	        rectSelect = GetSelectionMarkerRect( DEHT.DEHT_LEFTMIDDLE, rect );
	        //g.DrawRectangle(pen, rectSelect );//dc->Rectangle( rectSelect );
            g.FillRectangle(br, rectSelect);
            */
        }

        Rectangle GetSelectionMarkerRect( DEHT marker, Rectangle rect ) 
        /* ============================================================
	        Function :		CDiagramEntity::GetSelectionMarkerRect
	        Description :	Gets the selection marker rectangle for 
					        marker, given the true object rectangle 
					        "rect".
	        Access :		Protected

        					
	        Return :		CRect		-	The marker rectangle
	        Parameters :	UINT marker	-	The marker type ("DEHT_"-
									        constants defined in 
									        DiargramEntity.h)
					        CRect rect	-	The object rectangle
        					
	        Usage :			"marker" can be one of the following:
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

	        Rectangle rectMarker = new Rectangle();
	        int horz =this.MarkerSize.Width / 2;
            int vert = this.MarkerSize.Height / 2;

	        switch( marker )
	        {
		        case DEHT.DEHT_TOPLEFT:
			        rectMarker = new Rectangle( rect.Left - horz,
								        rect.Top - vert,
                                        this.MarkerSize.Width,//rect.Left + horz,
                                        this.MarkerSize.Height);//rect.Top + vert );
		        break;

		        case DEHT.DEHT_TOPMIDDLE:
			        rectMarker= new Rectangle( rect.Left + ( rect.Width / 2 ) - horz,
								        rect.Top - vert,
                                         this.MarkerSize.Width,//rect.Left + ( rect.Width / 2 ) + horz,
                                        this.MarkerSize.Height);//rect.Top + vert );
		        break;

		        case DEHT.DEHT_TOPRIGHT:
			        rectMarker= new Rectangle( rect.Right - horz,
								        rect.Top - vert,
                                         this.MarkerSize.Width,//rect.Right + horz,
                                        this.MarkerSize.Height);//rect.Top + vert );
		        break;

		        case DEHT.DEHT_BOTTOMLEFT:
			        rectMarker= new Rectangle( rect.Left - horz,
								        rect.Bottom - vert,
                                         this.MarkerSize.Width,//rect.Left + horz,
                                        this.MarkerSize.Height);//rect.Bottom + vert );
		        break;

		        case DEHT.DEHT_BOTTOMMIDDLE:
			        rectMarker= new Rectangle( rect.Left + ( rect.Width / 2 ) - horz,
								        rect.Bottom - vert,
                                         this.MarkerSize.Width,//rect.Left + ( rect.Width / 2 ) + horz,
                                        this.MarkerSize.Height);//rect.Bottom + vert );
		        break;

		        case DEHT.DEHT_BOTTOMRIGHT:
			        rectMarker= new Rectangle( rect.Right - horz,
								        rect.Bottom - vert,
                                         this.MarkerSize.Width,//rect.Right + horz,
                                       this.MarkerSize.Height);// rect.Bottom + vert );
		        break;

		        case DEHT.DEHT_LEFTMIDDLE:
			        rectMarker= new Rectangle( rect.Left - horz,
								        rect.Top + ( rect.Height / 2 ) - vert,
                                         this.MarkerSize.Width,//rect.Left + horz,
                                       this.MarkerSize.Height);// rect.Top + ( rect.Height / 2 ) + vert );
		        break;

		        case DEHT.DEHT_RIGHTMIDDLE:
			        rectMarker= new Rectangle( rect.Right - horz,
								        rect.Top + ( rect.Height / 2 ) - vert,
                                         this.MarkerSize.Width,//rect.Right + horz,
                                        this.MarkerSize.Height);//rect.Top + ( rect.Height / 2 ) + vert );
		        break;
	        }

	        return rectMarker;

        }

        private DiagramEntityContainer _ParentContainer = null;
        [Browsable(false)]
        public DiagramEntityContainer ParentContainer
        {
            get { return _ParentContainer; }
            set { _ParentContainer = value;}
        }

        //public void SetParent( DiagramEntityContainer  parent )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetParent
        //    Description :	Set the container owning the object.
        //    Access :		Protected

        //    Return :		void
        //    Parameters :	CDiagramEntityContainer * parent	-	the 
        //                                                            parent.
        					
        //    Usage :			Call to set the parent of the object. 
        //                    Objects must know their parent, to allow 
        //                    copying etc. 

        //   ============================================================*/
        //{

        //    m_parent = parent;

        //}

        public void GetFont( ref WinAPI.LOGFONT lf  )
        /* ============================================================
	        Function :		CDiagramEntity::GetFont
	        Description :	Returns the system GUI font in a "LOGFONT" 
					        scaled to the zoom level of the object.
	        Access :		Protected

        					
	        Return :		void
	        Parameters :	LOGFONT& lf	-	The "LOGFONT" for the system
									        GUI font.
        					
	        Usage :			Call to get the system font. Note that MS 
					        Sans Serif will not scale below 8 points.

           ============================================================*/
        {
            Font font = SystemFonts.DialogFont;
            
            font.ToLogFont(lf);
            lf.lfHeight = -32;
	        
	        //lf.lfHeight =(int)decimal.Round( (decimal )( lf.lfHeight ) * (decimal)this.Zoom );


        }

        private string _TypeName = "";
        [Browsable(false)]
        public string TypeName
        {
            get { return _TypeName; }
            set {_TypeName = value;}
        }


        private string _Title = "";
        [Description("Button text")]
        [DisplayName("Key label")]
        [Category("Legend")]
        [Browsable(false)]
        virtual public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    FirePropertiesBeforeChangedEvent();
                    this.PropertiesModified = true;
                    _Title = value;
                    FirePropertiesChangedEvent();
                }
            }
        }

    




        private int PixelsToPhysicalMM(int nPixels)
        {
            decimal d = nPixels;
            d = d / (decimal)Util.KB9DPI * (decimal)25.4; //300dpi image
            return (int)decimal.Round(d);
        }


        private int PixelsToPhysicalUnits(int nPixels)
        {
            decimal d = nPixels;
            d = d / (decimal)Util.KB9DPI * (decimal)25.4*(decimal)2; //300dpi image
            return (int)decimal.Round(d);
        }

        private int PhysicalMMToPixels(int nMM)
        {
            decimal d = nMM;
            d = d / (decimal)25.4 * (decimal)Util.KB9DPI ;
            return (int)decimal.Round(d);
        }

        private int PhysicalUnitsToPixels(int nUnits)
        {
            decimal d = nUnits;
            d = (d/(decimal)2) / (decimal)25.4 * (decimal)Util.KB9DPI;
            return (int)decimal.Round(d);
        }

        private float ConvertUnit2MM(int nUnits)
        {
            return (float)nUnits * KB9Const.UNITS_MM;

        }



        private RectangleF ConvertUnit2MM( Rectangle rect)
        {
            RectangleF rt = rect;
            rt.X = ConvertUnit2MM(rect.X);
            rt.Y = ConvertUnit2MM(rect.Y);
            rt.Width = ConvertUnit2MM(rect.Width);
            rt.Height = ConvertUnit2MM(rect.Height);
            return rt;

        }

        private int ConvertMM2Unit(float fltMM)
        {
            float f =  fltMM / KB9Const.UNITS_MM;
            decimal d =(decimal) f;
            return (int)decimal.Round(d);


        }

        private Rectangle ConvertMM2Unit(RectangleF rect)
        {
            Rectangle rt = new Rectangle();
            rt.X = ConvertMM2Unit(rect.X);
            rt.Y = ConvertMM2Unit(rect.Y);
            rt.Width = ConvertMM2Unit(rect.Width);
            rt.Height = ConvertMM2Unit(rect.Height);
            return rt;

        }
        
        [DisplayName("Key bounds(mm)")]
        [Browsable(true)]
        [Category("Key")]
        [ReadOnly(false)]
        [TypeConverter(typeof(RectangleFConverter))]
        public RectangleF MMBounds
        {
            get
            {
                Rectangle rt = this.PhyBounds;
                rt.X -= KB9Const.UNITS_MARGIN;
                rt.Y -= KB9Const.UNITS_MARGIN;
                return ConvertUnit2MM(rt);

                
            }
            set
            {
                if (this.ParentContainer.IsMatrixEntity(this))
                    return;
                RectangleF rt = value;
                rt.X += KB9Const.MM_MARGIN;
                rt.Y += KB9Const.MM_MARGIN;
                this.PhyBounds = ConvertMM2Unit(rt);


            }
        }

        [DisplayName("Key bounds(units)")]
        [Browsable(false)]
        [Category("Key")]
        public Rectangle UnitsBounds
        {
            get
            {
                Rectangle rt = this.PhyBounds;
                rt.X -= KB9Const.UNITS_MARGIN;
                rt.Y -= KB9Const.UNITS_MARGIN;
                return (rt);


            }
            set
            {
                Rectangle rt = value;
                rt.X += (int)KB9Const.UNITS_MARGIN;
                rt.Y += (int)KB9Const.UNITS_MARGIN;
                this.PhyBounds = (rt);


            }
        }
        /// <summary>
        /// convert pixel bounds to physcial bounds in kb9000
        /// </summary>
        /// 
        private Rectangle _UnitBounds = new Rectangle(0, 0, 0, 0); //the bounds in (0.5mm) units.
        [Browsable(false)]
        virtual public Rectangle PhyBounds
        {
            get 
            {
                return _UnitBounds;
                /*
                Rectangle rt = this.Bounds;
                //rt.X = PixelsToPhysicalMM(rt.X - DiagramEntityContainer.VIRTUALMARGIN);
                //rt.Y = PixelsToPhysicalMM(rt.Y - DiagramEntityContainer.VIRTUALMARGIN);
                //rt.Width = PixelsToPhysicalMM(rt.Width );//- DiagramEntityContainer.VIRTUALMARGIN);
                //rt.Height = PixelsToPhysicalMM(rt.Height);// - DiagramEntityContainer.VIRTUALMARGIN);

                rt.X = PixelsToPhysicalUnits(rt.X - DiagramEntityContainer.VIRTUALMARGIN);
                rt.Y = PixelsToPhysicalUnits(rt.Y - DiagramEntityContainer.VIRTUALMARGIN);
                rt.Width = PixelsToPhysicalUnits(rt.Width);//- DiagramEntityContainer.VIRTUALMARGIN);
                rt.Height = PixelsToPhysicalUnits(rt.Height);// - DiagramEntityContainer.VIRTUALMARGIN);
                return rt;
                */
            }
            set 
            {
                /*
                Rectangle rt = value;
                //rt.X = PhysicalMMToPixels(rt.X);// +DiagramEntityContainer.VIRTUALMARGIN;
                //rt.Y = PhysicalMMToPixels(rt.Y);// + DiagramEntityContainer.VIRTUALMARGIN;
                //rt.Width = PhysicalMMToPixels(rt.Width);// + DiagramEntityContainer.VIRTUALMARGIN;
                //rt.Height = PhysicalMMToPixels(rt.Height);// + DiagramEntityContainer.VIRTUALMARGIN;


                rt.X = PhysicalUnitsToPixels(rt.X) +DiagramEntityContainer.VIRTUALMARGIN;
                rt.Y = PhysicalUnitsToPixels(rt.Y) + DiagramEntityContainer.VIRTUALMARGIN;
                rt.Width = PhysicalUnitsToPixels(rt.Width);// + DiagramEntityContainer.VIRTUALMARGIN;
                rt.Height = PhysicalUnitsToPixels(rt.Height);// + DiagramEntityContainer.VIRTUALMARGIN;
                this.Bounds = rt;
                 * */

                Rectangle rtOld = _UnitBounds;

                _UnitBounds = value;

                Size minSize = ConvertPixel2Unit(this.MinimumSize);
                if (this.MinimumSize.Width == -1) minSize.Width = -1;
                if (this.MinimumSize.Height == -1) minSize.Height = -1;

                Size maxSize = ConvertPixel2Unit(this.MaximumSize);
                if (this.MaximumSize.Width == -1) maxSize.Width = -1;
                if (this.MaximumSize.Height == -1) maxSize.Height = -1;

                if (minSize.Width != -1)
                    if (_UnitBounds.Width < minSize.Width)
                        _UnitBounds.Width = minSize.Width;


                if (minSize.Height != -1)
                    if (_UnitBounds.Height < minSize.Height)
                        _UnitBounds.Height = minSize.Height;


                if (maxSize.Width != -1)
                    if (_UnitBounds.Width > maxSize.Width)
                        _UnitBounds.Width = maxSize.Width;
                

                if (maxSize.Height != -1)
                    if (_UnitBounds.Height > maxSize.Height)
                        _UnitBounds.Height = maxSize.Height;
                
                //strict the x, y

                int maxX = KB9Const.UNITS_CX - KB9Const.UNITS_MARGIN;
                int maxY = KB9Const.UNITS_CY - KB9Const.UNITS_MARGIN;

                if (_UnitBounds.Width >maxX - KB9Const.UNITS_MARGIN )
                    _UnitBounds.Width = maxX - KB9Const.UNITS_MARGIN ;
                if (_UnitBounds.Height > maxY - KB9Const.UNITS_MARGIN )
                    _UnitBounds.Height = maxY -  KB9Const.UNITS_MARGIN ;


                if (_UnitBounds.Right > maxX)
                    _UnitBounds.X = maxX - _UnitBounds.Width;
                if (_UnitBounds.Bottom > maxY)
                    _UnitBounds.Y = maxY - _UnitBounds.Height;

                int minX = KB9Const.UNITS_MARGIN;
                int minY = KB9Const.UNITS_MARGIN;

                if (_UnitBounds.X < minX)
                    _UnitBounds.X = minX;
                if (_UnitBounds.Y < minY)
                    _UnitBounds.Y = minY;



                if (rtOld != _UnitBounds)
                {
                    //int nmax = get_max_corner_radius();
                    //if (this.RoundCornerRadius > nmax)
                    //    this.RoundCornerRadius = nmax;
                    this.PropertiesModified = true;
                    update_round_corner_radius();
                    update_border_thickness();
                    FirePropertiesChangedEvent();
                    

                    
                }


            }
        }
        virtual protected void update_round_corner_radius()
        {

        }
        virtual protected void update_border_thickness()
        {

        }

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
       


        //private const int  _UnitSize = 6;

        private int ConvertPixel2Unit(int nPixels)
        {
            decimal d = ((decimal)nPixels / (decimal)KB9Const.UNIT_PIXELS);
            return (int)decimal.Round(d);
        }

        private Rectangle ConvertPixel2Unit(Rectangle rect)
        {
            Rectangle rt = rect;
            rt.X = ConvertPixel2Unit(rt.X);
            rt.Y = ConvertPixel2Unit(rt.Y);
            rt.Width = ConvertPixel2Unit(rt.Width);
            rt.Height = ConvertPixel2Unit(rt.Height);
            return rt;

        }

        private Size ConvertPixel2Unit(Size size)
        {
            Size sz = size;
            sz.Width = ConvertPixel2Unit(sz.Width);
            sz.Height = ConvertPixel2Unit(sz.Height);
            return sz;
        }


        private int ConvertUnit2Pixel(int nUnit)
        {
            return nUnit * KB9Const.UNIT_PIXELS;
        }

        private Rectangle ConvertUnit2Pixel(Rectangle rect)
        {
            Rectangle rt = rect;
            rt.X = ConvertUnit2Pixel(rt.X);
            rt.Y = ConvertUnit2Pixel(rt.Y);
            rt.Width = ConvertUnit2Pixel(rt.Width);
            rt.Height = ConvertUnit2Pixel(rt.Height);
            return rt;
        }

        private Size ConvertUnit2Pixel(Size size)
        {
            Size sz = size;
            sz.Width = ConvertUnit2Pixel(sz.Width);
            sz.Height = ConvertUnit2Pixel(sz.Height);
            return sz;

        }

        private int ConvertUnit2Image(int nUnit)
        {
            decimal d = decimal.Round((decimal)nUnit * (decimal)0.5 / (decimal)25.4 * (decimal)KB9Const.IMAGE_DPI);
            return (int)d;
        }

        private Rectangle ConvertUnit2Image(Rectangle rect)
        {
            Rectangle rt = rect;
            rt.X = ConvertUnit2Image(rt.X);
            rt.Y = ConvertUnit2Image(rt.Y);
            rt.Width = ConvertUnit2Image(rt.Width);
            rt.Height = ConvertUnit2Image(rt.Height);
            return rt;
        }

        private Size ConvertUnit2Image(Size size)
        {
            Size sz = size;
            sz.Width = ConvertUnit2Image(sz.Width);
            sz.Height = ConvertUnit2Image(sz.Height);
            return sz;

        }

        
        
        //private Rectangle _Location = new Rectangle();
        [Description("Key location and size")]
        [Browsable(false)]
        public Rectangle Bounds
        {
            get 
            {
                Rectangle rt = this.PhyBounds;
                return ConvertUnit2Pixel(rt);

            }
            set
            {
                Rectangle rtOld = this.PhyBounds;

                Rectangle rt = value;
                this.PhyBounds = ConvertPixel2Unit(rt);

               
            }
        }

        
        //for drawing to image.
        [Browsable(false)]
        public Rectangle ImgBounds
        {
            get
            {
                Rectangle rt = this.PhyBounds;
                return ConvertUnit2Image(rt);

            }
            //set
            //{
            //    Rectangle rtOld = this.PhyBounds;

            //    Rectangle rt = value;
            //    this.PhyBounds = ConvertPixel2Unit(rt);


            //}
        }

        //public double GetBottom() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetBottom
        //    Description :	Gets the bottom edge of the object 
        //                    rectangle
        //    Access :		Public

        //    Return :		double	-	Bottom postion
        //    Parameters :	none

        //    Usage :			Call to get the bottom edge of the object.
        //                    Note that if minimum sizes are not set for 
        //                    the object, the top edge might be bigger 
        //                    than the bottom.
        //                    The object coordinates are expressed as 
        //                    double values to allow unlimited zoom.

        //   ============================================================*/
        //{

        //    return m_bottom;

        //}

        //public void SetLeft( double left )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetLeft
        //    Description :	Sets the left edge of the object rectangle
        //    Access :		Public

        //    Return :		void
        //    Parameters :	double left	-	New left position
        					
        //    Usage :			Call to set the left edge of the object.
        //                    The object coordinates are expressed as 
        //                    double values to allow unlimited zoom.

        //   ============================================================*/
        //{

        //    m_left = (float)left;

        //}

        //public void SetRight( double right )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetRight
        //    Description :	Sets the right edge of the object 
        //                    rectangle
        //    Access :		Public

        //    Return :		void
        //    Parameters :	double right	-	New right position
        					
        //    Usage :			Call to set the right edge of the object.
        //                    The object coordinates are expressed as 
        //                    double values to allow unlimited zoom.

        //   ============================================================*/
        //{

        //    m_right =(float) right;

        //}

        //public void SetTop( double top )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetTop
        //    Description :	Sets the top edge of the object rectangle
        //    Access :		Public

        //    Return :		void
        //    Parameters :	double top	-	New top position
        					
        //    Usage :			Call to set the top edge of the object.
        //                    The object coordinates are expressed as 
        //                    double values to allow unlimited zoom.

        //   ============================================================*/
        //{
        ////if( top == m_bottom )
        ////    top = top;
        //    m_top = (float)top;

        //}

        //public void SetBottom( double bottom )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetBottom
        //    Description :	Sets the bottom edge of the object 
        //                    rectangle
        //    Access :		Public

        //    Return :		void
        //    Parameters :	double bottom	-	New bottom position
        					
        //    Usage :			Call to set the bottom edge of the object.
        //                    The object coordinates are expressed as 
        //                    double values to allow unlimited zoom.

        //   ============================================================*/
        //{

        //    m_bottom = (float)bottom;

        //}

        private Size _MarkerSize = new Size(KB9Const.MARKER_SIZE, KB9Const.MARKER_SIZE);
        [Browsable(false)]
        public Size MarkerSize
        {
            get { return _MarkerSize; }
            set { _MarkerSize = value;}
        }
        //public void SetMarkerSize( Size markerSize )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetMarkerSize
        //    Description :	Gets the size of selection markers
        //    Access :		Protected
        //    Access :		Public

        //    Return :		void
        //    Parameters :	CSize markerSize	-	The new size of a 
        //                                            selection marker
        					
        //    Usage :			Call to set a new selection marker size for 
        //                    the object.

        //   ============================================================*/
        //{

        //    m_markerSize = markerSize;

        //}

        //public Size GetMarkerSize() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetMarkerSize
        //    Description :	Gets the size of selection marker
        //    Access :		Protected
        //    Access :		Public

        //    Return :		CSize	-	The current size of a 
        //                                selection marker
        //    Parameters :	none

        //    Usage :			Call to get the selection marker size for
        //                    the object.

        //   ============================================================*/
        //{

        //    return m_markerSize;

        //}

        private Size _MinimumSize = new Size(10,10);
        [Browsable(false)]
        public Size MinimumSize
        {
            get { return _MinimumSize; }
            set { _MinimumSize = value; }
        }

        //public void SetMinimumSize( Size minimumSize )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetMinimumSize
        //    Description :	Sets the minimum size for instances of 
        //                    this object.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	CSize minimumSize	-	The minimum allowed 
        //                                            size
        					
        //    Usage :			Call to set the minimum size of the object.
        //                    It is not possible to resize an object to a 
        //                    size smaller than the minimum allowed size.

        //   ============================================================*/
        //{

        //    m_minimumSize = minimumSize;

        //}

        //public Size GetMinimumSize() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetMinimumSize
        //    Description :	Gets the minimum size for instances of 
        //                    this object.
        //    Access :		Public

        //    Return :		CSize	-	The minimum allowed size
        //    Parameters :	none

        //    Usage :			Call to get the minimum size of the object.
        //                    It is not possible to resize an object to a 
        //                    size smaller than the minimum allowed size.

        //   ============================================================*/
        //{

        //    return m_minimumSize;

        //}

        private Size _MaximumSize = new Size(-1, -1);
        [Browsable(false)]
        public Size MaximumSize
        {
            get { return _MaximumSize; }
            set {
                _MaximumSize = value;
            }
        }

        //public void SetMaximumSize( Size maximumSize )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetMaximumSize
        //    Description :	Sets the maximum size for instances of 
        //                    this object.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	CSize maximumSize	-	The maximum allowed 
        //                                            size.
        					
        //    Usage :			Call to set the maximum size of the object.
        //                    It is not possible to resize an object to a 
        //                    size larger than the maximum allowed size.

        //   ============================================================*/
        //{

        //    m_maximumSize = maximumSize;

        //}

        //public Size GetMaximumSize() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetMaximumSize
        //    Description :	Returns the maximum size for instances of 
        //                    this object.
        //    Access :		Public

        //    Return :		CSize	-	The maximum allowed size.
        //    Parameters :	none

        //    Usage :			Call to get the maximum size of the object.
        //                    It is not possible to resize an object to a 
        //                    size larger than the maximum allowed size.

        //   ============================================================*/
        //{

        //    return m_maximumSize;

        //}

        public void SetConstraints( Size min, Size max )
        /* ============================================================
	        Function :		CDiagramEntity::SetConstraints
	        Description :	Sets the minimum and maximum sizes for 
					        instances of this object. -1 means no 
					        constraints.
	        Access :		Public

	        Return :		void
	        Parameters :	CSize min	-	Minimum size
					        CSize max	-	Maximum size
        					
	        Usage :			Call to set the minimum and maximum sizes 
					        of the object.
					        It is not possible to resize an object to 
					        smaller or bigger than the min- and max 
					        size.

           ============================================================*/
        {

	        //m_minimumSize = min;
	       // /m_maximumSize = max;
            this.MaximumSize = max;
            this.MinimumSize = min;

        }

        //public DiagramEntityContainer GetParent() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetParent
        //    Description :	Returns a pointer to the parent container.
        //    Access :		Protected

        //    Return :		CDiagramEntityContainer*	-	Parent
        //                                                    container.
        //    Parameters :	none

        //    Usage :			Call to get the parent of the object.

        //   ============================================================*/
        //{

        //    return m_parent;

        //}

        //public void SetPropertyDialog( DiagramPropertyDlg dlg, int dlgresid )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetPropertyDialog
        //    Description :	Sets the property dialog pointer.
        //    Access :		Protected

        //    Return :		void
        //    Parameters :	CDiagramPropertyDlg* dlg	-	a pointer 
        //                                                    to a dialog 
        //                                                    instance. 
        //                    UINT dlgresid				-	The resource 
        //                                                    id of the 
        //                                                    dialog template.
        					
        //    Usage :			Call to set a property dialog for the object 
        //                    (normally in the "ctor"). 

        //   ============================================================*/
        //{

        //    m_propertydlg = dlg;
        //    m_propertydlgresid = dlgresid;

        //    if( dlg != null )
        //        m_propertydlg.SetEntity( this );

        //}

        //public DiagramPropertyDlg GetPropertyDialog() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetPropertyDialog
        //    Description :	Returns a pointer to the class property 
        //                    dialog.
        //    Access :		Protected

        //    Return :		CDiagramPropertyDlg*	-	The dialog 
        //                                                pointer. "NULL" 
        //                                                if none.
        //    Parameters :	none

        //    Usage :			Call to get a pointer to the object property 
        //                    dialog.

        //   ============================================================*/
        //{

        //    return m_propertydlg;

        //}
        /* ============================================================
            Function :		CDiagramEntity::GetZoom
            Description :	Returns the zoom level for the object.
            Access :		Public

            Return :		double	-	
            Parameters :	none

            Usage :			Internal function. Can be called by derived 
                            classes to get the zoom level. The zoom 
                            level is set by the owning editor when 
                            drawing the object, is read-only and this 
                            function should only be called from "Draw".

           ============================================================*/
        private double _Zoom = 1.0;
        [Browsable(false)]
        public double Zoom
        {
            get { return _Zoom; }
            set { _Zoom = value;}
        }

        //public double GetZoom() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetZoom
        //    Description :	Returns the zoom level for the object.
        //    Access :		Public

        //    Return :		double	-	
        //    Parameters :	none

        //    Usage :			Internal function. Can be called by derived 
        //                    classes to get the zoom level. The zoom 
        //                    level is set by the owning editor when 
        //                    drawing the object, is read-only and this 
        //                    function should only be called from "Draw".

        //   ============================================================*/
        //{

        //    return m_zoom;

        //}

        //public void SetZoom( double zoom )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetZoom
        //    Description :	Sets the zoom level
        //    Access :		Protected

        //    Return :		nothing
        //    Parameters :	double zoom	-	The new zoom level
        					
        //    Usage :			Internal call.

        //   ============================================================*/
        //{
        //    m_zoom = zoom;
        //}

        private int _GroupID = 0;
        [Browsable(false)]
        public int GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value;}
        }
        //public int GetGroup() 
        ///* ============================================================
        //    Function :		CDiagramEntity::GetGroup
        //    Description :	Gets the object group.
        //    Access :		Public

        //    Return :		int	-	Group of object
        //    Parameters :	none

        //    Usage :			Call to get the group for the object. All 
        //                    objects in a group are selected together.

        //   ============================================================*/
        //{

        //    return m_group;

        //}

        //public void SetGroup( int group )
        ///* ============================================================
        //    Function :		CDiagramEntity::SetGroup
        //    Description :	Sets the object group to "group".
        //    Access :		Public

        //    Return :		void
        //    Parameters :	int group	-	New group to set
        					
        //    Usage :			Call to set a group for the object. All 
        //                    objects in a group are selected together.

        //   ============================================================*/
        //{

        //    m_group = group;

        //}

        public virtual void Export(int nLayerIndex, CLCIXML xml) 
        /* ============================================================
	        Function :		CDiagramEntity::Export
	        Description :	Exports the object to format
	        Access :		Public

	        Return :		CString		-	The object representation 
									        in format.
	        Parameters :	UINT format	-	The format to export to.
        					
	        Usage :			Virtual function to allow easy exporting of 
					        the objects to different text based formats.

           ============================================================*/
        {

	        string s;
	        s = "k"+nLayerIndex.ToString();
	        xml.new_group(s, true);
	        xml.new_attribute("keytype", this.TypeName);
	        Rectangle rt = this.PhyBounds;
           // Rectangle rt = this.Bounds;

	        xml.new_attribute("x", rt.X.ToString());
	        xml.new_attribute("y",rt.Y.ToString());
	        xml.new_attribute("w",rt.Width.ToString());
	        xml.new_attribute("h",rt.Height.ToString());

            xml.new_attribute("text", this.Title);


        }

        virtual public DEHT GetHitCode(Point point, Rectangle rect) 
        /* ============================================================
	        Function :		CDiagramEntity::GetHitCode
	        Description :	Returns the hit point constant for "point" 
					        assuming the object rectangle "rect".
	        Access :		Public

	        Return :		int				-	The hit point, 
										        "DEHT_NONE" if none.
	        Parameters :	CPoint point	-	The point to check
					        CRect rect		-	The rect to check
        					
	        Usage :			Call to see in what part of the object point 
					        lies. The hit point can be one of the following:
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

	        DEHT result = DEHT.DEHT_NONE;

	        if( rect.Contains( point ) )
                result = DEHT.DEHT_BODY;

	        Rectangle rectTest;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_TOPLEFT, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_TOPLEFT;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_TOPMIDDLE, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_TOPMIDDLE;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_TOPRIGHT, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_TOPRIGHT;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_BOTTOMLEFT, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_BOTTOMLEFT;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_BOTTOMMIDDLE, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_BOTTOMMIDDLE;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_BOTTOMRIGHT, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_BOTTOMRIGHT;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_LEFTMIDDLE, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_LEFTMIDDLE;

            rectTest = GetSelectionMarkerRect(DEHT.DEHT_RIGHTMIDDLE, rect);
            if (rectTest.Contains(point))
                result = DEHT.DEHT_RIGHTMIDDLE;

	        return result;

        }
        /* ============================================================
	        Function :		SelectedMain
	        Description :	In multiple selected items, we need a main one for reference of align/size ...
	        Access :		Public

	        Return :		bool		-	if this object is main in multple selected items
									        
	        Parameters :	bool        - set this objec to main or not
        					
	        Usage :			while multiple selected, call this property

           ============================================================*/
        private bool _SelectedMain = false;
        [Browsable(false)]
        public bool SelectedMain
        {
            get { return _SelectedMain; }
            set { _SelectedMain = value;}
        }

        protected virtual void FirePropertiesChangedEvent(bool bJustRefreshPropertiesGrid)
        {
            //if (this.PauseEvent) return;
            if (OnEntityPropertiesChanged != null)
                OnEntityPropertiesChanged(this, this.PauseEvent, bJustRefreshPropertiesGrid);
        }

        protected virtual void FirePropertiesChangedEvent()
        {
            //if (this.PauseEvent) return;
            if (OnEntityPropertiesChanged != null)
                OnEntityPropertiesChanged(this, this.PauseEvent, false);
        }
        protected virtual void FirePropertiesBeforeChangedEvent()
        {
            //if (this.PauseEvent) return;
            if (OnEntityPropertiesBeforeChange != null)
                OnEntityPropertiesBeforeChange(this, this.PauseEvent, false);
        }


        public void export_font(CLCIXML xml,  WinAPI.LOGFONT pFont)
        {
        	
            xml.new_attribute("fontname", pFont.lfFaceName);
            xml.new_attribute("fontheight",pFont.lfHeight.ToString());
            xml.new_attribute("fontitalic",pFont.lfItalic.ToString());
            xml.new_attribute("fontstrikeout", pFont.lfStrikeOut.ToString());
            xml.new_attribute("fontweight", ((int)(pFont.lfWeight)).ToString() );
            xml.new_attribute("fontunderline",pFont.lfUnderline.ToString());
            xml.new_attribute("fontwidth", pFont.lfWidth.ToString() );
        }

        /************************************************************************/
        /* 
        import font information from xml fil.e
        */
        /************************************************************************/
        public void import_font(CLCIXML xml, ref WinAPI.LOGFONT pFont)
        {
	        string s = "";
	        xml.get_attribute("fontname", ref s);
	        pFont.lfFaceName = s;

	        xml.get_attribute(("fontheight"),ref s);
	        pFont.lfHeight =int.Parse(s);//

	        xml.get_attribute(("fontitalic"),ref s);
	        pFont.lfItalic =bool.Parse(s);

	        xml.get_attribute(("fontstrikeout"),ref s);
	        pFont.lfStrikeOut =bool.Parse(s);

	        xml.get_attribute(("fontweight"),ref s);
	        pFont.lfWeight =(WinAPI.FontWeight) int.Parse(s);

	        xml.get_attribute(("fontunderline"),ref s);
	        pFont.lfUnderline =bool.Parse(s);

	        xml.get_attribute(("fontwidth"),ref s);
	        pFont.lfWidth  =int.Parse(s);

            pFont.lfCharSet = WinAPI.FontCharSet.DEFAULT_CHARSET;


        }

        static public string ImageToBase64(Image image,  System.Drawing.Imaging.ImageFormat format)
        {
            if (image == null) return "";
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        static public Image Base64ToImage(string base64String)
        {
            if (base64String.Length <= 0)
                return null;
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public virtual string GetTooltipsText()
        {
            return string.Empty;
        }

        private bool _ShowLogo = true;
        [Browsable(false)]
        public bool ShowLogo
        {
            get
            {
                return _ShowLogo;
            }
            set
            {

                _ShowLogo = value;
            }
        }
        /// <summary>
        /// record if the properties changed or not.
        /// </summary>
        private bool _ContentModified = false;
        [Browsable(false)]
        public bool ContentModified
        {
            get 
            {
                return _ContentModified;
            }
            set 
            {
                _ContentModified = value;
            }
        }

        /// <summary>
        /// record if the properties changed or not.
        /// </summary>
        private bool _PropertiesModified = false;
        [Browsable(false)]
        public bool PropertiesModified
        {
            get
            {
                return _PropertiesModified;
            }
            set
            {
                _PropertiesModified = value;
            }
        }

        virtual public bool isEqual(DiagramEntity key)
        {
            
            if (!this.Bounds.Equals(key.Bounds)) return false;
            
            if (!this.Title.Equals(key.Title)) return false;
            

            return true;
        }

        virtual public string getContentText()
        {
            return "";
        }
    }
   }

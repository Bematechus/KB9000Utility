using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace KB9Utility
{
    public class DiagramEntityContainer
    {
      


        public delegate void EventAddNew(DiagramEntity entity);
        public EventAddNew OnAddNew;

        public enum EntitiesSameSize
        {
            SAME_HEIGHT = 0,
            SAME_WIDTH,
            SAME_BOTH
        };

        public enum EntitiesAlign
        {
            ALIGN_LEFT = 0,
            ALIGN_RIGHT,
            ALIGN_TOP,
            ALIGN_BOTTOM,
            ALIGN_MIDDLE,//align center of all selected buttons to center between
                         //left-most and right-most edges of the selected buttons
            ALIGN_CENTER //align center of all selected buttons to center between
                         //top-most and bottom-most edge of the selected buttons
        }

        public enum EntitiesSpacing
        {
            SPACING_HORIZONTAL = 0,
            SPACING_VERTICAL
        }

	    // Data
	    private List<object> m_objs = new List<object>();
        //private List<object> m_undo = new List<object>();
	    //private int				m_maxstacksize;
	    //private Size			m_virtualSize;

	    private DiagramClipboardHandler	m_clip;
	    private DiagramClipboardHandler	m_internalClip = new DiagramClipboardHandler();

	  

        public DiagramEntityContainer( DiagramClipboardHandler clip )
        /* ============================================================
	        Function :		CDiagramEntityContainer::CDiagramEntityContainer
	        Description :	Constructor
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			

           ============================================================*/
        {

	        m_clip = clip;

	        //this.UndoStackSize = ( 0 );
	        Clear();
	        this.VirtualSize = ( new Size( 0, 0 ) );

        }

        private DiagramEditor _ParentEditor = null;
        public DiagramEditor ParentEditor
        {
            get
            {
                return _ParentEditor;
            }
            set
            {
                _ParentEditor = value;
            }
        }

       

        public void Clear()
        /* ============================================================
	        Function :		CDiagramEntityContainer::Clear
	        Description :	Removes all data from the data and undo.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to remove data from the container. The 
					        Paste-array will be kept.

           ============================================================*/
        {

	        RemoveAll();
	        ClearUndo();
	        this.Modified = ( false );

        }

        //public string GetString() 
        ///* ============================================================
        //    Function :		CDiagramEntityContainer::GetString
        //    Description :	Returns a string representation of the 
        //                    virtual paper size
        //    Access :		Public

        //    Return :		CString	-	Resulting string
        //    Parameters :	none

        //    Usage :			Call to get a string representing the paper 
        //                    size of the container. The format is 
        //                    "paper:x,y;" where "x" and "y" are the 
        //                    horisontal and vertical sizes.

        //   ============================================================*/
        //{

        //    //CString str;
        //    //str.Format( _T( "paper:%i,%i;" ), GetVirtualSize().cx, GetVirtualSize().cy );
        //    //return str;
        //    return "";

        //}

        public bool ImportFromXml( CLCIXML xml, int nCount )
        /* ============================================================
	        Function :		CDiagramEntityContainer::FromString
	        Description :	Sets the virtual paper size from a string.
	        Access :		Public

	        Return :		BOOL				-	"TRUE" if the string 
											        represented a 
											        paper.
	        Parameters :	const CString& str	-	The string 
											        representation.
        					
	        Usage :			Call to set the paper size of the container 
					        from a string. The format is "paper:x,y;" 
					        where "x" and "y" are the horisontal and 
					        vertical sizes.

           ============================================================*/
        {

	        bool result = true;

            for (int i = 0; i < nCount; i++ )
            {
                string s;
                s = "k"+i.ToString();
                if (!xml.get_first_group(s))
                    continue;
                s = "";
                xml.get_attribute("keytype", ref s);
                DiagramEntity k = null;
                switch (s)
                {
                    case DiagramKey.KEY_BUTTON:// "BUTTON":
                        {


                            k = new DiagramKeyArea();
                            k.FromXml(xml);
                        }
                        break;
                    case DiagramKey.KEY_SLIDEV:
                        {


                            k = new DiagramKeySlideV();
                            k.FromXml(xml);
                        }
                        break;
                    case DiagramKey.KEY_SLIDEH:
                        {


                            k = new DiagramKeySlideH();
                            k.FromXml(xml);
                        }
                        break;
                    case DiagramKeyMatrix.KEY_MATRIX:
                        { //for matrix
                            k = new DiagramKeyMatrix();
                            k.FromXml(xml);
                        }
                        break;
                    default:
                        continue;
                }
                
                xml.back_to_parent();
                Add(k);
            }


                return result;

        }

        public void ExportToXml( CLCIXML xml )
        /* ============================================================
            Function :		ExportToXml
            Description :	Exports all objects to format format.
            Access :		Public

            Return :		void
            Parameters :	CStringArray& stra	-	"CStingArray" that 
                                                    will be filled with 
                                                    data on return. 
                            UINT format			-	Format to save to.
        					
            Usage :			Call to export the contents of the container 
                            to a "CStringArray". "Export" will - of course - 
                            have to be defined for the derived objects.

           ============================================================*/
        {
            int max = GetSize();
            for (int t = 0; t < max; t++)
            {
                DiagramEntity obj = GetAt(t);

                obj.Export(t, xml);
            }



        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEntityContainer data access

        public int GetSize() 
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetSize
	        Description :	Returns the number of objects in the data
					        container.
	        Access :		Public

	        Return :		int		-	The number of objects.
	        Parameters :	none

	        Usage :			Call to get the number of objects currently 
					        in the data array of the container.

           ============================================================*/
        {

	        return m_objs.Count;

        }

        public void Add( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Add
	        Description :	Add an object to the data.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to add.
        					
	        Usage :			Call to add a new object to the container.

           ============================================================*/
        {

            obj.ParentContainer = (this);
	        m_objs.Add( obj );
            this.Modified = (true);
            if (OnAddNew != null)
                OnAddNew(obj);
        }

        public DiagramEntity GetAt( int index ) 
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetAt
	        Description :	Gets the object at position "index".
	        Access :		Public

	        Return :		CDiagramEntity*	-	The object or "NULL" if 
										        out of range.
	        Parameters :	int index		-	The index to get data 
										        from
        					
	        Usage :			Call to get a specific object from the 
					        container.

           ============================================================*/
        {

	        DiagramEntity result = null;
	        if( index < m_objs.Count && index >= 0 )
		        result = ( DiagramEntity)( m_objs[ index ] );
	        return result;

        }

        public void SetAt( int index, DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::SetAt
	        Description :	Sets an object at position "index".
	        Access :		Public

	        Return :		void
	        Parameters :	int index			-	Index to set data 
											        at.
					        CDiagramEntity* obj	-	Object to set.
        					
	        Usage :			Internal function. Used by "Swap".

           ============================================================*/
        {

	        m_objs[index] =  obj ;
            this.Modified = (true);

        }

        public void RemoveAt( int index )
        /* ============================================================
	        Function :		CDiagramEntityContainer::RemoveAt
	        Description :	Removes the object at index.
	        Access :		Public

	        Return :		void
	        Parameters :	int index	-	The index of the object 
									        to remove.
        					
	        Usage :			Call to remove a specific object. Memory is 
					        freed.

           ============================================================*/
        {

	        DiagramEntity obj = GetAt( index );
	        if( obj != null)
	        {
		       // delete obj;
		        m_objs.RemoveAt( index );
                this.Modified = (true);
	        }

        }

        public void RemoveAll()
        /* ============================================================
	        Function :		CDiagramEntityContainer::RemoveAll
	        Description :	Removes all data objects
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to remove all data objects in the 
					        container. Undo- and paste arrays are not 
					        emptied.
					        Allocated memory is released. Undo and 
					        paste not deleted.

           ============================================================*/
        {

	        int max = m_objs.Count;
	        if( max>0 )
	        {

                //for( int t = 0 ; t < max ; t++ )
                //{
                //    DiagramEntity obj = atic_cast< CDiagramEntity* >( m_objs.GetAt( t ) );
                //    delete obj;
                //}

		        m_objs.Clear();
                this.Modified = (true);

	        }

        }

        public void RemoveAllSelected()
        /* ============================================================
	        Function :		CDiagramEntityContainer::RemoveAllSelected
	        Description :	Removes all selected objects
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to remove all selected objects from the 
					        container. Releases allocated data

           ============================================================*/
        {

	        int max = m_objs.Count - 1;
	        for( int t = max ; t >= 0 ; t-- )
		        if( GetAt( t ). Selected )
			        RemoveAt( t );

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEntityContainer property access

        private Size _VirtualSize = new Size();
        public Size VirtualSize
        {
            get { return _VirtualSize; }
            set { _VirtualSize = value;}
        }

        //public void SetVirtualSize( Size size )
        ///* ============================================================
        //    Function :		CDiagramEntityContainer::SetVirtualSize
        //    Description :	Sets the current virtual paper size.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	CSize size	-	The size to set
        					
        //    Usage :			Call to set the paper size. Note that 
        //                    "SetModified( TRUE )" might have to be called 
        //                    as well.

        //   ============================================================*/
        //{

        //    m_virtualSize = size;

        //}

        //public Size GetVirtualSize() 
        ///* ============================================================
        //    Function :		CDiagramEntityContainer::GetVirtualSize
        //    Description :	Gets the virtual paper size.
        //    Access :		Public

        //    Return :		CSize	-	The current size
        //    Parameters :	none

        //    Usage :			Call to get the current paper size.

        //   ============================================================*/
        //{

        //    return m_virtualSize;

        //}

        private bool _Modified = false;
        public bool Modified
        {
            get { return _Modified; }
            set { 
                _Modified = value;
                if (!_Modified)
                {
                    SetAllEntitiesModified(false);
                }
            }
        }

        //public bool IsModified() 
        ///* ============================================================
        //    Function :		CDiagramEntityContainer::IsModified
        //    Description :	Returns the state of the modified-flag.
        //    Access :		Public

        //    Return :		BOOL	-	"TRUE" if data is changed
        //    Parameters :	none

        //    Usage :			Call to see if data is modified.

        //   ============================================================*/
        //{

        //    return m_dirty;

        //}

        //public void SetModified( bool dirty )
        ///* ============================================================
        //    Function :		CDiagramEntityContainer::SetModified
        //    Description :	Sets the state of the modified flag
        //    Access :		Public

        //    Return :		void
        //    Parameters :	BOOL dirty	-	"TRUE" if data is changed.
        					
        //    Usage :			Call to mark the data as modified.

        //   ============================================================*/
        //{

        //    m_dirty = dirty;

        //}

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEntityContainer single object handlers

        public void Remove( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Remove
	        Description :	Removes the object.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to 
											        remove.
        					
	        Usage :			Call to remove "obj" - if it exists - from the 
					        container. Allocated memory is released.

           ============================================================*/
        {

	        int index = Find( obj );
            if (index != -1)
            {
                RemoveAt(index);
                this.ParentEditor.Refresh();
            }

        }

        public void Duplicate( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Duplicate
	        Description :	Duplicates the object and adds the new 
					        one 10 pixels offset down and right.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to 
											        duplicate.	
        					
	        Usage :			Call to create a copy of the selected 
					        element.

           ============================================================*/
        {

	        int index = Find( obj );
	        if( index != -1 )
	        {
		        DiagramEntity newobj = obj.Clone();
                Rectangle rt = obj.Bounds;
                rt.Offset(new Point(10, 10));
                newobj.Bounds = rt;
                newobj.Title = get_key_unique_title(newobj.TypeName);
		        //newobj.SetRect( newobj.GetLeft() + 10, newobj.GetTop() + 10, newobj.GetRight() + 10, newobj.GetBottom() + 10 );
                newobj.setID(DiagramEntity.createNewGUID());
                Add( newobj );
	        }

        }

        public void Cut( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Cut
	        Description :	Cuts out the object and puts it into the 
					        'clipboard'
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to cut.
        					
	        Usage :			Call in response to a Cut-command. See also 
					        the functions for copy/paste below.

           ============================================================*/
        {

	        Copy( obj );
	        Remove( obj );

        }

        public void Copy( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Copy
	        Description :	Copies the object to the 'clipboard'.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to copy.	
        					
	        Usage :			Call in response to a Copy-command. Note 
					        that obj will only be copied to the 
					        clipboard, not the screen. See also the 
					        functions for copy/paste below.

           ============================================================*/
        {

            if (obj == null) return;
	        //ASSERT( obj );

	        if( m_clip == null )
		        m_clip = m_internalClip;

	        if( obj != null )
		        m_clip.Copy( obj );

        }

        public void Up( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Up
	        Description :	Moves the object one step up in the z-
					        order.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to move.	
        					
	        Usage :			Call to move "obj" in the z-order.

           ============================================================*/
        {

	        int index = Find( obj );
	        Swap( index, index + 1);

        }

        public void Down( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Down
	        Description :	Moves the object one step down in the z-
					        order.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to move.	
        					
	        Usage :			Call to move "obj" in the z-order.

           ============================================================*/
        {

	        int index = Find( obj );
	        Swap( index, index - 1);

        }

        public void Front( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Front
	        Description :	Moves "obj" to the top of the z-order.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to move.	
        					
	        Usage :			Call to move "obj" in the z-order.

           ============================================================*/
        {

	        int index = Find( obj );
	        m_objs.RemoveAt( index );
	        m_objs.Add( obj );
            //this.Modified = (true);

        }

        public void Bottom( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramEntityContainer::Bottom
	        Description :	Moves "obj" to the bottom of the z-order.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to move.
        					
	        Usage :			Call to move "obj" in the z-order.

           ============================================================*/
        {

	        int index = Find( obj );
	        m_objs.RemoveAt( index );
	        m_objs.Insert( 0, obj );
            this.Modified = (true);

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEntityContainer copy/paste is implemented as separate class.

        public void SetClipboardHandler(DiagramClipboardHandler clip)
        /* ============================================================
	        Function :		CDiagramEntityContainer::SetClipboardHandler
	        Description :	Sets the container clipboard class.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramClipboardHandler* clip	-	A pointer
														        to the
														        class
        					
	        Usage :			Call to set the clipboard handler for this 
					        container. The same clipboard handler 
					        instance can be used for several containers 
					        to allow several editors (in an MDI-
					        application) to share the same clipboard.

           ============================================================*/
        {

	        m_clip = clip;

        }

        DiagramClipboardHandler GetClipboardHandler()
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetClipboardHandler
	        Description :	Returns a pointer to the current clipboard 
					        handler.
	        Access :		Public

	        Return :		CDiagramClipboardHandler*	-	Current handler.
	        Parameters :	none

	        Usage :			Call to get a pointer to the current handler.

           ============================================================*/
        {

	        return m_clip;

        }

        public void CopyAllSelected()
        /* ============================================================
	        Function :		CDiagramEntityContainer::CopyAllSelected
	        Description :	Clones all selected object to the paste 
					        array.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to copy all selected objects to the 
					        clipboard. "Paste" will put them on screen.

           ============================================================*/
        {

	        if( m_clip == null )
		        m_clip = m_internalClip;

	        m_clip.CopyAllSelected( this );

        }

        public int ObjectsInPaste()
        /* ============================================================
	        Function :		CDiagramEntityContainer::ObjectsInPaste
	        Description :	Returns the number of objects in the paste 
					        array.
	        Access :		Public

	        Return :		int		-	The number of objects.
	        Parameters :	none

	        Usage :			Call to get the number of objects in the 
					        clipboard.

           ============================================================*/
        {

	        if( m_clip == null )
		        m_clip = m_internalClip;

	        return m_clip.ObjectsInPaste();

        }

        public void ClearPaste()
        /* ============================================================
	        Function :		CDiagramEntityContainer::ClearPaste
	        Description :	Clears the paste-array.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to clear the clipboard. All memory is 
					        released.

           ============================================================*/
        {

	        if( m_clip == null )
		        m_clip = m_internalClip;

	        m_clip.ClearPaste();

        }

        public List<DiagramEntity> Paste()
        /* ============================================================
	        Function :		CDiagramEntityContainer::Paste
	        Description :	Clones the contents of the paste array 
					        into the container data array.
	        Access :		Public

	        Return :		void
	        Parameters :	pt: virtual point value.

	        Usage :			Call to paste the contents of the clipboard 
					        to screen.

           ============================================================*/
        {

	        if( m_clip == null )
		        m_clip = m_internalClip;

	        return m_clip.Paste( this);

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEntityContainer message handling

        public void SendMessageToObjects(DiagramEntity.EntityCmd command, bool selected, DiagramEntity sender, Control from)
        /* ============================================================
	        Function :		CDiagramEntityContainer::SendMessageToObjects
	        Description :	Sends "command" to objects. 
	        Access :		Public

	        Return :		void
	        Parameters :	int command				-	The command to send.
					        BOOL selected			-	If "TRUE", the command 
												        will only be sent to 
												        selected objects, 
												        otherwise, it will be 
												        sent to all objects.
					        CDiagramEntity* sender	-	Original sender 
												        or "NULL" if not 
												        an object.

	        Usage :			Call this member to send messages to 
					        (selected) objects in the range "CMD_START" 
					        to "CMD_END" inclusively (defined in 
					        DiagramEntity.h). Calls the object "DoCommand".

           ============================================================*/
        {

	        bool stop = false;
	        int max = m_objs.Count;
	        for( int t = 0 ; t < max ; t++ )
	        {
		        DiagramEntity obj = GetAt( t );
		        if( !stop && ( !selected || obj.Selected ) )
		        {
			        stop = obj.DoMessage( command, sender, from );
                    this.Modified = (true);
		        }
	        }

        }

        /////////////////////////////////////////////////////////////////////////////
        // CDiagramEntityContainer private helpers

        public int Find(DiagramEntity testobj)
        /* ============================================================
	        Function :		CDiagramEntityContainer::Find
	        Description :	Finds the index of object "testobj" in the 
					        data array.
	        Access :		Protected

	        Return :		int						-	Index of the 
												        object or -1 
												        if not found.
	        Parameters :	CDiagramEntity* testobj	-	Object to find.
        					
	        Usage :			Internal function. 

           ============================================================*/
        {

	        int index = -1;
	        DiagramEntity obj = null;
	        int count = 0;
	        while( ( obj = GetAt( count ) ) != null )
	        {
		        if( obj == testobj )
			        index = count;
		        count++;
	        }

	        return index;

        }

        public void Swap(int index1, int index2)
        /* ============================================================
	        Function :		CDiagramEntityContainer::Swap
	        Description :	Swaps the elements at "index1" and "index2".
	        Access :		Private

	        Return :		void
	        Parameters :	int index1	-	First object to swap
					        int index2	-	Second object to swap
        					
	        Usage :			Internal function. Used to move objects up 
					        or down in the z-order.

           ============================================================*/
        {

	        int max = m_objs.Count;
	        if( index1 >= 0 && index1 < max && index2 >= 0 && index2 < max )
	        {
		        DiagramEntity obj1 = GetAt( index1 );
		        DiagramEntity obj2 = GetAt( index2 );
		        SetAt( index1, obj2 );
		        SetAt( index2, obj1 );
	        }

        }

        private void setTabIndex(UndoItem undo)
        {
            Object objParent = GetParentForm();
            if (objParent != null)
            {
                frmMain frm = (frmMain)objParent;
                undo.LastFocusedEditBoxIndex = frm.GetFocusedTextBoxIndex();

            }
        }
        /// <summary>
        /// redo last operation
        /// </summary>
        public UndoItem Redo()
        {

            if (this.RedoStock.Count>0)
            {
                //Snapshot();
                UndoItem snapshot = SnapshotBeforeRedo();
                setTabIndex(snapshot);
                //Object objParent = GetParentForm();
                //if (objParent != null)
                //{
                //    frmMain frm = (frmMain)objParent;
                //    snapshot.LastFocusedEditBoxIndex = frm.GetFocusedTextBoxIndex();

                //}
               // UndoItem undoPre = SnapshotRedo();

                // We remove all current data
                RemoveAll();

                // We get the last entry from the undo-stack
                // and clone it into the container data
                //UndoItem undo = (UndoItem)(this.RedoStock);//.GetAt( this.UndoStack[ .GetUpperBound() ) );
                UndoItem undo = PopupRedoStock();
                int count = undo.SnapshotItems.Count;// GetSize();
                for (int t = 0; t < count; t++)
                {

                    DiagramEntity obj = (DiagramEntity)(undo.SnapshotItems[t]);
                    Add(obj.Clone());

                }
                
                return undo;
                //this.UnselectAll();
                //this.RedoStock = null;
                //this.RedoStock = undoPre;// null;

            }
            return null;

        }
        /// <summary>
        /// Thank you, David.  The undo/redo is working correctly now.  
        /// However, I noticed that with the definition I proposed yesterday, 
        /// it is still not possible to see the correct edited contents of the keys.  
        /// This is because when I click on the undo button, it has already 
        /// removed the last content edit step although the key is in focus.  
        /// Similarly, when clicking on redo, the new content after the redo 
        /// cannot be seen as key focus is already moved to the next step.
        /// To solve this problem, I think we need to add a step to the redo 
        /// list whenever key content modification is recorded as a step.  
        /// That is, after recording the content change to the redo list, automatically 
        /// add another step “focus to key” to the redo list.  This way, when stepping 
        /// through the undo / redo operations, it would provide one more step 
        /// at points where key contents are changed.  Thus, the content before 
        /// the undo or after the redo will be shown before user click on the buttons again 
        /// to go to next step.  Is this possible?
        /// </summary>
        /// <param name="lastRedo"></param>
        /// <param name="willUndo"></param>
        /// <returns></returns>
        private bool checkAppendFocusRedo(UndoItem lastScreen, UndoItem willUndo)
        {
            //1. willUndo just one foucsed, and just its contents changed.
            if (willUndo.getHightlightCount() != 1)
                return false;
            DiagramEntity entityWill = willUndo.getHightlightEntity();
            if (entityWill == null) return false;

            DiagramEntity entityCurrent = lastScreen.findSamePositionEntity(entityWill);
            if (entityCurrent == null) return false;
            string current = entityCurrent.getContentText();
            string undoContent = entityWill.getContentText();
            if (current.Equals(undoContent))
                return false;

            UndoItem undo = lastScreen.clone();
            undo.hightlightNothing();
            //DiagramEntity entityRedo = undo.findSamePositionEntity(entityWill);
            undo.focusSamePositionEntity(entityWill);
            //entityRedo.Selected = true;
            if (this.RedoStock.Count > 0)
            {
                UndoItem lastRedo =(UndoItem) this._RedoStack[this.RedoStock.Count - 1];
                if (lastRedo.isEqual(undo))
                    return true;
            }
            System.Diagnostics.Debug.Print("checkAppendFocusRedo add redo");
            this.RedoStock.Add(undo);
            return true;



        }
        private Object GetParentForm()
        {
            return this.ParentEditor.ParentForm;
        }
        //???????????????????????????????????????????????????????????????????
        public UndoItem Undo()
        /* ============================================================
	        Function :		CDiagramEntityContainer::Undo
	        Description :	Sets the container data to the last entry 
					        in the undo stack.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to undo the last operation

           ============================================================*/
        {

            if( this.UndoStack.Count>0 )
            {
                //save previous screen, 20140530
                UndoItem redo = SnapshotForRedo(this.ParentEditor);
                setTabIndex(redo);
                System.Diagnostics.Debug.Print("redo=" + this.RedoStock.Count.ToString());

                //Object objParent = GetParentForm();
                //if (objParent != null)
                //{
                //    frmMain frm = (frmMain)objParent;
                //    redo.LastFocusedEditBoxIndex = frm.GetFocusedTextBoxIndex();

                //}
                int nindex = this.UndoStack.Count - 1;
                UndoItem undo = (UndoItem)(this.UndoStack[nindex]);//.GetAt( this.UndoStack[ .GetUpperBound() ) );
                //while (redo.isAnythingSame(undo))
                //{

                //   // if (undo.RecordForFocus) break;
                //    this.UndoStack.RemoveAt(nindex);
                //    nindex --;
                //    if (nindex >= 0)
                //        undo = (UndoItem)(this.UndoStack[nindex]);
                //    else
                //        return null;
                //}
                // We remove all current data
                RemoveAll();

                // We get the last entry from the undo-stack
                // and clone it into the container data
                
                if (!undo.RecordForFocus)
                    checkAppendFocusRedo(redo, undo);
                //System.Diagnostics.Debug.Print("redo=" + this.RedoStock.Count.ToString());

                int count = undo.SnapshotItems.Count;// GetSize();
                for( int t = 0 ; t < count ; t++ )
                {

                    DiagramEntity obj = (DiagramEntity)(undo.SnapshotItems[t]);
                    Add( obj.Clone() );

                }
                //this.UnselectAll();

                // Set the saved virtual size as well
               // this.VirtualSize = ( undo.EditorSize );

                // We remove the entry from the undo-stack
                //delete undo;

                this.UndoStack.RemoveAt(this.UndoStack.Count - 1);
                return undo;

            }
            return null;

        }
        private void check_undo_stack_size()
        {
            if (this.UndoStackSize > 0 && this.UndoStack.Count >= this.UndoStackSize)
            {

                this.UndoStack.RemoveAt(0);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="focusedEntity"></param>
        /// <returns></returns>
        public UndoItem SnapshotAppendFocusTrace(DiagramEntity focusedEntity, int nTextBoxIndex)
        {
            if (this.UndoStackSize == 0) return null; //no undo support
            check_undo_stack_size();
 
            UndoItem undo = new UndoItem();
           
            // Save all objects
            int count = m_objs.Count;
            for (int t = 0; t < count; t++)
            {
                DiagramEntity d = GetAt(t).Clone();
                undo.SnapshotItems.Add(d);
                DiagramEntity entity = GetAt(t);
               
                if (entity == focusedEntity)
                    d.Selected = true;
                else
                {
                    if (entity is DiagramKeyMatrix)
                    {
                        DiagramKeyMatrix m = (DiagramKeyMatrix)entity;
                        int n = m.Entities.Count;
                        for (int i = 0; i < n; i++)
                        {
                            if (m.Entities[i] == focusedEntity)
                            {
                                d.Selected = true;
                                ((DiagramKeyMatrix)d).Entities[i].Selected = true;
                            }
                        }
                    }
                    else
                    {
                        d.Selected = false;
                    }
                }
                
            }
            
            
            //UndoItem undo = this.Snapshot();
            SnapshotEditor(undo);
            undo.RecordForFocus = true;
            undo.LastFocusedEditBoxIndex = nTextBoxIndex;
            // Add to undo stack
           // if (isSameWithLastUndo(undo))
           //     return null;
            System.Diagnostics.Debug.Print("undo --> SnapshotAppendFocusTrace");
            appendUndo(undo);
            //this.UndoStack.Add(undo);

           // System.Diagnostics.Debug.Print("undo=" + this.UndoStack.Count.ToString());
            return undo;
        }

        /// <summary>
        /// Use it to compare screen objects changed or not
        /// </summary>
        /// <returns></returns>
        public UndoItem CopyScreenByOriginalObject()
        {
           
            UndoItem undo = new UndoItem();
            // Save all objects
            int count = m_objs.Count;
            for (int t = 0; t < count; t++)
                undo.SnapshotItems.Add(GetAt(t).Clone());
            SnapshotEditor(undo);
           
            return undo;
        }


        public UndoItem Snapshot()
        /* ============================================================
	        Function :		CDiagramEntityContainer::Snapshot
	        Description :	Copies the current state of the data to 
					        the undo-stack.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to add the current state to the undo-stack. 
					        If the undo stack has a maximum size and 
					        the stack will grow above the stack limit, 
					        the first undo array will be removed.

           ============================================================*/
        {
            if (this.UndoStackSize == 0) return null; //no undo support
            check_undo_stack_size();

            UndoItem undo = new UndoItem();
  
            // Save all objects
            int count = m_objs.Count;
            for( int t = 0 ; t < count ; t++ )
                undo.SnapshotItems.Add(GetAt(t).Clone());
            SnapshotEditor(undo);
            if (isSameWithLastUndo(undo))
                return null;
            //if( this.UndoStack.Count>0 )
            //{
            //    UndoItem lastUndo = (UndoItem)this.UndoStack[this.UndoStack.Count - 1];
            //    if (lastUndo.isEqualWithoutFunctionCheck(undo))
            //        return null;

            //}
            // Add to undo stack
            System.Diagnostics.Debug.Print("undo --> Snapshot");
            appendUndo(undo);
            this._UndoPreparation = null; //clear it.
            //this.UndoStack.Add(undo);
            
            return undo;
            

        }

        public UndoItem SnapshotBeforeRedo()
        {
            if (this.UndoStackSize == 0) return null; //no undo support
            check_undo_stack_size();

            UndoItem undo = new UndoItem();

            // Save all objects
            int count = m_objs.Count;
            for (int t = 0; t < count; t++)
                undo.SnapshotItems.Add(GetAt(t).Clone());
            SnapshotEditor(undo);
            //if (isSameWithLastUndo(undo))
            //    return null;
          
            // Add to undo stack

            //this.UndoStack.Add(undo);
            System.Diagnostics.Debug.Print("undo --> SnapshotBeforeRedo");
            appendUndo(undo);
            return undo;


        }

        private bool isSameWithLastUndo(UndoItem undo)
        {
            if (this.UndoStack.Count == 0)
                return false;
            if (this.UndoStack.Count > 0)
            {
                UndoItem lastUndo = (UndoItem)this.UndoStack[this.UndoStack.Count - 1];
                if (lastUndo.isEqualWithoutFunctionCheck(undo))
                    return true;

            }
            return false;
        }

        public void Snapshot(DiagramEditor editor)
        {

            UndoItem undo = Snapshot();
            if (undo == null) return;

            undo.EditorBackColor = editor.BackColor;
            undo.EditorInterCharDelay = editor.InterCharDelay;
            undo.EditorSensitivity = editor.Sensitivity;
            undo.EditorTouchDelay = editor.TouchDelay;
            undo.EditorPropertiesEnabled = true;




        }


        public UndoItem SnapshotForRedo(DiagramEditor editor)
        {
            System.Diagnostics.Debug.Print("SnapshotForRedo add redo");
            UndoItem undo = SnapshotRedo(editor);
            this.RedoStock.Add(undo);
            return undo;
            /*
            UndoItem undo = new UndoItem();
            if (undo != null)
            {

               
                // Save all objects
                int count = m_objs.Count;
                for (int t = 0; t < count; t++)
                    undo.SnapshotItems.Add(GetAt(t).Clone());

                this.RedoStock.Add(undo);
            }
            */

        }
        protected void SnapshotEditor(UndoItem undo)
        {
            DiagramEditor editor = this.ParentEditor;
            if (editor == null) return;
            undo.EditorBackColor = editor.BackColor;
            undo.EditorInterCharDelay = editor.InterCharDelay;
            undo.EditorSensitivity = editor.Sensitivity;
            undo.EditorTouchDelay = editor.TouchDelay;
            undo.EditorPropertiesEnabled = true;
        }

        public UndoItem SnapshotRedo(DiagramEditor editor)
        {

            UndoItem undo = new UndoItem();
            if (undo != null)
            {


                // Save all objects
                int count = m_objs.Count;
                for (int t = 0; t < count; t++)
                    undo.SnapshotItems.Add(GetAt(t).Clone());
                SnapshotEditor(undo);
                //undo.EditorBackColor = editor.BackColor;
                //undo.EditorInterCharDelay = editor.InterCharDelay;
                //undo.EditorSensitivity = editor.Sensitivity;
                //undo.EditorTouchDelay = editor.TouchDelay;
                //undo.EditorPropertiesEnabled = true;
            }
            return undo;


        }

        private UndoItem _UndoPreparation = null;

        public void PreSnapshot()
        /* ============================================================
	        Function :		DiagramEntityContainer::PreSnapshot
	        Description :	Copies the current state of the data to 
					        the undoitem. Save it after confirm.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call it in mousedown event, then call SaveSnapshot in 
         *                  mouseup event, if there are any changes
           ============================================================*/
        {
           

            UndoItem undo = new UndoItem();

          

            if (undo != null)
            {

                // Save current virtual size
                //undo.EditorSize = this.VirtualSize;

                // Save all objects
                int count = m_objs.Count;
                for (int t = 0; t < count; t++)
                    undo.SnapshotItems.Add(GetAt(t).Clone());

                _UndoPreparation = undo;
                SnapshotEditor(undo);

            }

        }
        private void appendUndo(UndoItem undo)
        {
            this.UndoStack.Add(undo);
            System.Diagnostics.Debug.Print("undo=" + this.UndoStack.Count.ToString());
            //if (this.UndoStack.Count == 5)
            //{
            //    int a = 0;
            //}

        }
        public bool SaveSnapshotAfterLostFocus(int nTextBoxIndex)
        {
            bool bresult = false;
            if (_UndoPreparation != null)
            {
                check_undo_stack_size();
                
               
               if (this.UndoStack.Count > 0)
               {
                   UndoItem lastUndo = (UndoItem)this.UndoStack[this.UndoStack.Count - 1];
                   if (lastUndo.isEqual(_UndoPreparation))
                       return true;

               }
               _UndoPreparation.LastFocusedEditBoxIndex = nTextBoxIndex;
               appendUndo(_UndoPreparation);
               System.Diagnostics.Debug.Print("SaveSnapshotAfterLostFocus, appendUndo");
                //this.UndoStack.Add(_UndoPreparation);
                bresult = true;
            }
            _UndoPreparation = null;
            return bresult;
        }

        public bool SaveSnapshot()
        {
            bool bresult = false;
            if (_UndoPreparation != null)
            {
                check_undo_stack_size();
                //if (this.UndoStackSize > 0 && this.UndoStack.Count >= this.UndoStackSize)
                //{
                //    this.UndoStack.RemoveAt(0);
                //}

                //check if anything changed, 20141204 add this 
                UndoItem screen = this.CopyScreenByOriginalObject();
                if (screen.isEqualWithoutFunctionCheck(_UndoPreparation))
                    return false;


                if (isSameWithLastUndo(_UndoPreparation))
                {
                    _UndoPreparation = null;
                    return true;
                }
                //if (this.UndoStack.Count > 0)
                //{
                //    UndoItem lastUndo = (UndoItem)this.UndoStack[this.UndoStack.Count - 1];
                //    if (lastUndo.isEqualWithoutFunctionCheck(_UndoPreparation))
                //        return true;

                //}
                //this.UndoStack.Add(_UndoPreparation);
                System.Diagnostics.Debug.Print("SaveSnapshot, appendUndo");
                appendUndo(_UndoPreparation);

                bresult = true;
            }
            _UndoPreparation = null;
            return bresult;
        }
        public void ClearPreSnapshort()
        {
            _UndoPreparation = null;
        }

        public void ClearUndo()
        /* ============================================================
	        Function :		CDiagramEntityContainer::ClearUndo
	        Description :	Remove all undo arrays from the undo stack
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to clear the undo-stack. All memory will 
					        be deleted.

           ============================================================*/
        {
            this.UndoStack.Clear();

            _UndoPreparation = null;
            /*
            int count = this.UndoStack.GetSize() - 1;
            for( int t = count ; t >= 0 ; t-- )
            {
                CUndoItem* undo = static_cast< CUndoItem* >( this.UndoStack.GetAt( t ) );
                // Remove the stack entry itself.
                delete undo;
            }

            this.UndoStack.RemoveAll();
            */
        }

        public bool IsUndoPossible() 
        /* ============================================================
	        Function :		CDiagramEntityContainer::IsUndoPossible
	        Description :	Check if it is possible to undo.
	        Access :		Public

	        Return :		BOOL	-	"TRUE" if undo is possible.
	        Parameters :	none

	        Usage :			Use this call for command enabling

           ============================================================*/
        {

            return (this.UndoStack.Count > 0);

        }

        /* ============================================================
            Function :		CDiagramEntityContainer::SetUndoStackSize
            Description :	Sets the size of the undo stack.
            Access :		Public

            Return :		void
            Parameters :	int maxstacksize	-	New size. 
                                                    -1 means no limit,
                                                     0 no undo.

            Usage :			Call to set the max undo stack size.

           ============================================================*/
        private int _UndoStackSize = 50; //20141124 change it from 10 to 50
        public int UndoStackSize
        {
            get { return _UndoStackSize; }
            set { _UndoStackSize = value;}
        }

        //public void SetUndoStackSize(int maxstacksize)
        ///* ============================================================
        //    Function :		CDiagramEntityContainer::SetUndoStackSize
        //    Description :	Sets the size of the undo stack.
        //    Access :		Public

        //    Return :		void
        //    Parameters :	int maxstacksize	-	New size. -1 means 
        //                                            no limit, 0 no undo.
        					
        //    Usage :			Call to set the max undo stack size.

        //   ============================================================*/
        //{

        //    m_maxstacksize = maxstacksize;

        //}

        //public int GetUndoStackSize() 
        ///* ============================================================
        //    Function :		CDiagramEntityContainer::GetUndoStackSize
        //    Description :	Returns the size of the undo-stack
        //    Access :		Public

        //    Return :		int	-	Current size
        //    Parameters :	none

        //    Usage :			Call to get the max undo stack size.

        //   ============================================================*/
        //{

        //    return m_maxstacksize;

        //}

        public void PopUndo()
        /* ============================================================
	        Function :		CUMLEntityContainer::PopUndo
	        Description :	Pops the undo stack (removes the last stack
					        item)
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call do undo the last Snapshot

           ============================================================*/
        {

            int size = this.UndoStack.Count;
	        if( size >0 )
	        {
                //delete this.UndoStack.GetAt( size - 1 );
                this.UndoStack.RemoveAt(size - 1);
	        }

        }

        public List<object> GetData() 
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetData
	        Description :	Accessor for the internal data array
	        Access :		Public

	        Return :		CObArray*	-	A pointer to the internal 
									        data array.
	        Parameters :	none

	        Usage :			Call to access the internal data array. To 
					        be used in derived classes.

           ============================================================*/
        { 

	        return m_objs; 

        }

        public List<object> GetPaste()	
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetPaste
	        Description :	Accessor for the internal paste array
	        Access :		Protected

	        Return :		CObArray*	-	A pointer to the paste 
									        array
	        Parameters :	none

	        Usage :			Call to access the internal paste array. To 
					        be used in derived classes.

           ============================================================*/
        { 

	        List<object> arr = null;
	        if( m_clip != null )
		        arr = m_clip.GetData();

	        return arr;

        }

        private List<object> _UndoStack = new List<object>();
        public List<object> UndoStack
        {
            get { return _UndoStack; }

        }

        //private UndoItem _Redo = null;
        private List<object> _RedoStack = new List<object>();
        public List<object> RedoStock
        {
            get { return _RedoStack; }
            set { _RedoStack = value; }
        }

        public void ClearRedo()
        {
            this.RedoStock.Clear();
        }
        public UndoItem PopupRedoStock()
        {
            int ncount = this.RedoStock.Count;
            if (ncount <= 0)
                return null;
            UndoItem item =(UndoItem)this.RedoStock[ncount - 1];
            this.RedoStock.RemoveAt(ncount - 1);
            return item;
        }

        public void Group()
        /* ============================================================
	        Function :		CDiagramEntityContainer::Group
	        Description :	Groups the currently selected objects into 
					        the same group.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to group all selected items into the 
					        same group.
					        Grouped objects can be moved as a 
					        single entity. Technically, when one object 
					        in a group is selected, all other objects 
					        are also selected automatically.

           ============================================================*/
        {

	        DiagramEntity obj = null;
	        int count = 0;
	        int group = GroupFactory.GetNewGroup();
	        while( ( obj = GetAt( count ) ) != null)
	        {
		        if( obj.Selected )
			        obj.GroupID = ( group );
		        count++;
	        }

        }

        public void Ungroup()
        /* ============================================================
	        Function :		CDiagramEntityContainer::Ungroup
	        Description :	Ungroups the currently selected objects.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to ungroup all selected items. 
					        Grouped objects can be moved as a 
					        single entity. Technically, when one object 
					        in a group is selected, all other objects 
					        are also selected automatically.

           ============================================================*/
        {

	        DiagramEntity obj;
	        int count = 0;
	        while( ( obj = GetAt( count ) ) != null )
	        {
		        if( obj.Selected )
                    obj.GroupID = (0);
		        count++;
	        }

        }

        public Size GetTotalSize()
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetTotalSize
	        Description :	Gets the minimum bounding size for the 
					        objects in the container.
	        Access :		

	        Return :		CSize	-	Minimum bounding size
	        Parameters :	none

	        Usage :			Call to get the screen size of the objects 
					        in the container.

           ============================================================*/
        {
	        Point start = GetStartPoint();
	        double width = 0;
	        double height = 0;

	        DiagramEntity obj = null;
	        int count = 0;
	        while( ( obj = GetAt( count ) ) != null )
	        {

		        width = Math.Max( width, obj.Bounds.Left);// GetLeft() );
                width = Math.Max(width, obj.Bounds.Right);// GetRight());
                height = Math.Max(height, obj.Bounds.Top);// obj.GetTop());
                height = Math.Max(height, obj.Bounds.Bottom);// obj.GetBottom());

		        count++;

	        }

	        return new Size( (int)(decimal.Round( (decimal)(width - start.X) )),(int)( decimal.Round((decimal)( height - start.Y ) )));

        }

        public Point GetStartPoint()
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetStartPoint
	        Description :	Gets the starting screen position of the 
					        objects in the container (normally the 
					        top-left corner of the top-left object).
	        Access :		

	        Return :		CPoint	-	Top-left position of the 
								        objects.
	        Parameters :	none

	        Usage :			Call to get the starting point on screen of 
					        the objects.

           ============================================================*/
        {

            double startx = int.MaxValue;// 2000.0;
            double starty = int.MaxValue;// 2000.0;

	        DiagramEntity obj;
	        int count = 0;

	        while( ( obj = GetAt( count ) ) != null )
	        {

		        startx = Math.Min( startx, obj.Bounds.Left);// GetLeft() );
                startx = Math.Min(startx, obj.Bounds.Right);// GetRight());
                starty = Math.Min(starty, obj.Bounds.Top);// GetTop());
                starty = Math.Min(starty, obj.Bounds.Bottom);// GetBottom());

		        count++;

	        }
            //
            return new Point((int)(decimal.Round((decimal)startx)), (int)(decimal.Round((decimal)starty)));

        }

        public Point GetSelectedStartPoint()
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetStartPoint
	        Description :	Gets the starting screen position of the 
					        objects in the container (normally the 
					        top-left corner of the top-left object).
	        Access :		

	        Return :		CPoint	-	Top-left position of the 
								        objects.
	        Parameters :	none

	        Usage :			Call to get the starting point on screen of 
					        the objects.

           ============================================================*/
        {

            double startx = int.MaxValue;// 2000.0;
            double starty = int.MaxValue;// 2000.0;

            DiagramEntity obj;
            int count = 0;

            while ((obj = GetAt(count)) != null)
            {
                if (obj.Selected)
                {


                    startx = Math.Min(startx, obj.Bounds.Left);// GetLeft() );
                    startx = Math.Min(startx, obj.Bounds.Right);// GetRight());
                    starty = Math.Min(starty, obj.Bounds.Top);// GetTop());
                    starty = Math.Min(starty, obj.Bounds.Bottom);// GetBottom());
                }
                count++;

            }
            //
            return new Point((int)(decimal.Round((decimal)startx)), (int)(decimal.Round((decimal)starty)));

        }


        public Point GetSelectedStartPoint(List<DiagramEntity> selectedObjs)
        /* ============================================================
	        Function :		CDiagramEntityContainer::GetStartPoint
	        Description :	Gets the starting screen position of the 
					        objects in the container (normally the 
					        top-left corner of the top-left object).
	        Access :		

	        Return :		CPoint	-	Top-left position of the 
								        objects.
	        Parameters :	none

	        Usage :			Call to get the starting point on screen of 
					        the objects.

           ============================================================*/
        {

            double startx = int.MaxValue;// 2000.0;
            double starty = int.MaxValue;// 2000.0;

            DiagramEntity obj;
            //int count = 0;

            //while ((obj = GetAt(count)) != null)
                for (int i = 0; i < selectedObjs.Count; i++ )
                {
                    obj = (DiagramEntity)selectedObjs[i];
                    // if (obj.Selected)
                    {


                        startx = Math.Min(startx, obj.Bounds.Left);// GetLeft() );
                        startx = Math.Min(startx, obj.Bounds.Right);// GetRight());
                        starty = Math.Min(starty, obj.Bounds.Top);// GetTop());
                        starty = Math.Min(starty, obj.Bounds.Bottom);// GetBottom());
                    }
                    //count++;

                }
            //
            return new Point((int)(decimal.Round((decimal)startx)), (int)(decimal.Round((decimal)starty)));

        }




        public int GetSelectCount() 
        /* ============================================================
	        Function :		int	CDiagramEntityContainer::GetSelectCount
	        Description :	Gets the number of currently selected 
					        objects in the container.
	        Access :		

	        Return :		int		-	Currently selected objects.
	        Parameters :	none

	        Usage :			Call to get the number of selected objects.

           ============================================================*/
        {

	        int max = GetSize();
	        int count = 0;
	        for( int t = 0 ; t < max ; t++ )
		        if( GetAt( t ).Selected )
			        count++;

	        return count;

        }

        public void SelectAll()
        {
	        int max = GetSize();
	        for( int t = 0 ; t < max ; t++ )
		        GetAt( t ).Selected = ( true );
        }

        public void UnselectAll()
        {
	        int max = GetSize();
	        for( int t = 0 ; t < max ; t++ )
		        GetAt( t ).Selected = ( false );
        }
        public void ClearMultipleSelectedMain()
        {
            for (int t = 0; t < this.GetSize(); t++)
            {
                
                 GetAt(t).SelectedMain = false;
                
              
            }
        }
        //if multiple selected, set which one is the main one.
        public void SetMultipleSelectedMain(DiagramEntity entity)
        {
            if (GetSelectCount() <= 1) return;
            int max = GetSize();
            //int count = 0;
            int nSelectedIndex = 0;
            for (int t = 0; t < max; t++)
            {
                if (GetAt(t).Selected)
                {
                    if (entity == null)
                    {
                        if (nSelectedIndex == 0) //first one
                        {
                            GetAt(t).SelectedMain = true;
                            //continue;
                        }
                        else
                            GetAt(t).SelectedMain = false;
                    }
                    else if (entity == GetAt(t))
                    {
                        GetAt(t).SelectedMain = true;
                    }
                    else
                        GetAt(t).SelectedMain = false;

                    nSelectedIndex++;
                }
             //       count++;
            }
            //return count;
        }

        public DiagramEntity GetMultipleSelectedMain()
        {
            if (GetSelectCount() <= 1) return null;
            int max = GetSize();
            
            
            for (int t = 0; t < max; t++)
            {
                if (GetAt(t).Selected)
                {
                    if (GetAt(t).SelectedMain)
                        return GetAt(t);
                }
               
            }
            return null;
        }

        public void SameSize(EntitiesSameSize sameSize)
        {
            if (GetSelectCount() <= 1) return;
            DiagramEntity objMain = GetMultipleSelectedMain();
            if (objMain == null) return;
            int height = objMain.Bounds.Height;// (int)(objMain.GetBottom() - objMain.GetTop());
            int width = objMain.Bounds.Width;// (int)(objMain.GetRight() - objMain.GetLeft());

            DiagramEntity obj = null;
            int count = 0;
            
            
            while ((obj = GetAt(count)) != null)
            {
                if (obj.Selected)
                {
                    Rectangle rt = obj.Bounds;
                    if (sameSize == EntitiesSameSize.SAME_HEIGHT)
                        rt.Height = height;
                    //obj.SetBottom(obj.GetTop() + height);        
                    else if (sameSize == EntitiesSameSize.SAME_WIDTH)
                        rt.Width = width;
                        //obj.SetRight(obj.GetLeft() + width);
                    else
                    {
                        rt.Height = height;
                        rt.Width = width;
                        //obj.SetBottom(obj.GetTop() + height);
                        //obj.SetRight(obj.GetLeft() + width);
                    }
                    obj.Bounds = rt;
                }
                count++;
            }
        }
        /// <summary>
        /// get left-most, right-most, top-most or bottom-most entity in selected entities
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        private DiagramEntity GetSelectedMostEntity(EntitiesAlign align)
        {
            if (GetSelectCount() <= 1) return null;
            DiagramEntity obj = null;
            int count = 0;

            DiagramEntity objResult = null;

            while ((obj = GetAt(count)) != null)
            {
                if (obj.Selected)
                {
                    if (objResult == null)
                        objResult = obj;

                    Rectangle rt = obj.Bounds;
                    Rectangle rtResult = objResult.Bounds;

                    if (align == EntitiesAlign.ALIGN_LEFT)
                    {
                        if (rt.X <rtResult.X)
                            objResult = obj;
                    }
                    else if (align == EntitiesAlign.ALIGN_RIGHT)
                    {
                        if (rt.Right >rtResult.Right)
                            objResult = obj;
                    }
                    else if (align == EntitiesAlign.ALIGN_TOP)
                    {
                        
                        if (rt.Y <rtResult.Y)
                            objResult = obj;
                        

                    }
                    else if (align == EntitiesAlign.ALIGN_BOTTOM)
                    {
                        if (rt.Y > rtResult.Y)
                            objResult = obj;

                        
                    }
                    else if (align == EntitiesAlign.ALIGN_MIDDLE) //align center of all selected buttons to center between
                    // left-most and right-most edges of the selected buttons
                    {
                       
                    }
                    else if (align == EntitiesAlign.ALIGN_CENTER)//align center of all selected buttons to center between
                    //top-most and bottom-most edge of the selected buttons
                    {
                       
                    }

                    

                }
                count++;
            }

            return objResult;
        }

        
        public void AlignPosition(EntitiesAlign align)
        {
            if (GetSelectCount() <= 1) return;
            DiagramEntity objMain = GetMultipleSelectedMain();
            //if (objMain == null) return;

            DiagramEntity obj = null;
            int count = 0;
            DiagramEntity objMost = GetSelectedMostEntity(align);

            while ((obj = GetAt(count)) != null)
            {
                if (obj.Selected)
                {
                    int width = obj.Bounds.Width;// (int)(obj.GetRight() - obj.GetLeft());
                    int height = obj.Bounds.Height;// (int)(obj.GetBottom() - obj.GetTop());
                    Rectangle rt = obj.Bounds;

                    if (align == EntitiesAlign.ALIGN_LEFT)
                    {
                        if (objMost != null)
                            rt.X = objMost.Bounds.Left;
                    }
                    else if (align == EntitiesAlign.ALIGN_RIGHT)
                    {
                        if (objMost != null)
                            rt.X = objMost.Bounds.Right - rt.Width;
                        
                    }
                    else if (align == EntitiesAlign.ALIGN_TOP)
                    {
                        if (objMost != null)
                            rt.Y = objMost.Bounds.Top;
                        

                    }
                    else if (align == EntitiesAlign.ALIGN_BOTTOM)
                    {
                        if (objMost != null)
                            rt.Y = objMost.Bounds.Bottom - rt.Height;
                        
                        
                    }
                    else if (align == EntitiesAlign.ALIGN_MIDDLE) //align center of all selected buttons to center between
                                                                   // left-most and right-most edges of the selected buttons
                    {

                        if (objMain != null)
                        {
                            Point mainCenter = objMain.Bounds.Location;
                            mainCenter.Y += (objMain.Bounds.Height / 2);
                            rt.Y = mainCenter.Y - rt.Height / 2;
                        }

                    }
                    else if (align == EntitiesAlign.ALIGN_CENTER)//align center of all selected buttons to center between
                                                                 //top-most and bottom-most edge of the selected buttons
                    {
                    
                        if (objMain != null)
                        {


                            Point mainMiddle = objMain.Bounds.Location;
                            mainMiddle.X += (objMain.Bounds.Width / 2);
                            rt.X = mainMiddle.X - rt.Width / 2;
                        }
                    }

                    obj.Bounds = rt;

                }
                count++;
            }
        }
        private DiagramEntity MostLeft(List<object> arObjs)
        {
            int nMin = int.MaxValue;
            DiagramEntity objMin = null;
            for (int i=0; i< arObjs.Count; i++)
            {
                DiagramEntity obj = (DiagramEntity)arObjs[i];
                if (obj.Bounds.Left < nMin)
                {
                    objMin = obj;
                    nMin = obj.Bounds.Left;
                }

            }
            return objMin;

        }

        private DiagramEntity MostTop(List<object> arObjs)
        {
            int nMin = int.MaxValue;
            DiagramEntity objMin = null;
            for (int i = 0; i < arObjs.Count; i++)
            {
                DiagramEntity obj = (DiagramEntity)arObjs[i];
                if (obj.Bounds.Top < nMin)
                {
                    objMin = obj;
                    nMin = obj.Bounds.Top;
                }

            }
            return objMin;

        }

        private List<object> SortObjs(List<object> objs, EntitiesSpacing spacing)
        {
            DiagramEntity obj = null;
            List<object> ar = new List<object>();

            do
            {
                if (spacing == EntitiesSpacing.SPACING_HORIZONTAL)
                    obj = MostLeft(objs);
                else if (spacing == EntitiesSpacing.SPACING_VERTICAL)
                    obj = MostTop(objs);
                else
                    return null;
                if (obj != null)
                {


                    ar.Add(obj);
                    objs.Remove(obj);
                }


            } while (obj != null);

            return ar;
        }

        private int AverageSpacing(List<object> objs, EntitiesSpacing spacing)
        {
            int ntotal = 0;
            for (int i=0; i< objs.Count-1; i++)
            {
                DiagramEntity obj1 = (DiagramEntity)objs[i];
                DiagramEntity obj2 = (DiagramEntity)objs[i+1];
                int ndistance = 0;
                if (spacing == EntitiesSpacing.SPACING_HORIZONTAL)
                    ndistance = obj2.Bounds.Left - obj1.Bounds.Right;
                else if (spacing == EntitiesSpacing.SPACING_VERTICAL)
                    ndistance = obj2.Bounds.Top - obj1.Bounds.Bottom;
                else
                    return 0;
                ntotal += ndistance;
            }

            int n =(int)decimal.Round( (decimal)ntotal / (decimal)(objs.Count-1));
            return n;
        }

        private int AverageSpacingUnits(List<object> objs, EntitiesSpacing spacing)
        {
            int ntotal = 0;
            for (int i = 0; i < objs.Count - 1; i++)
            {
                DiagramEntity obj1 = (DiagramEntity)objs[i];
                DiagramEntity obj2 = (DiagramEntity)objs[i + 1];
                int ndistance = 0;
                if (spacing == EntitiesSpacing.SPACING_HORIZONTAL)
                    ndistance = obj2.UnitsBounds.Left - obj1.UnitsBounds.Right;
                else if (spacing == EntitiesSpacing.SPACING_VERTICAL)
                    ndistance = obj2.UnitsBounds.Top - obj1.UnitsBounds.Bottom;
                else
                    return 0;
                ntotal += ndistance;
            }

            int n = ntotal / objs.Count;
            return n;
        }

        public void SameSpacingUnits(EntitiesSpacing spacing)
        {

            if (GetSelectCount() <= 2) return;

            Snapshot();
            DiagramEntity obj = null;
            int count = 0;

            List<object> selectedObjs = new List<object>();

            while ((obj = GetAt(count)) != null)
            {
                if (obj.Selected)
                {
                    selectedObjs.Add(obj);
                }
                count++;
            }
            //sort them by distance to left position
            selectedObjs = SortObjs(selectedObjs, spacing);
            int naveragespacing = AverageSpacingUnits(selectedObjs, spacing);
            if (naveragespacing <= 0)
                naveragespacing = 1;
            for (int i = 1; i < selectedObjs.Count; i++)
            {
                obj = (DiagramEntity)selectedObjs[i];
                DiagramEntity obj0 = (DiagramEntity)selectedObjs[i - 1];
                Rectangle rt = obj.UnitsBounds;
                switch (spacing)
                {

                    case EntitiesSpacing.SPACING_HORIZONTAL:
                        {

                            rt.X = obj0.UnitsBounds.Right + naveragespacing;
                        }
                        break;
                    case EntitiesSpacing.SPACING_VERTICAL:
                        {
                            rt.Y = obj0.UnitsBounds.Bottom + naveragespacing;
                        }
                        break;
                    default:
                        break;
                }
                obj.UnitsBounds = rt;
            }

        }


        public void SameSpacing(EntitiesSpacing spacing)
        {

            if (GetSelectCount() <= 2) return;

            Snapshot();
            DiagramEntity obj = null;
            int count = 0;

            List<object> selectedObjs = new List<object>();

            while ((obj = GetAt(count)) != null)
            {
                if (obj.Selected)
                {
                    selectedObjs.Add(obj);
                }
                count++;
            }
            //sort them by distance to left position
            selectedObjs = SortObjs(selectedObjs, spacing);
            int naveragespacing = AverageSpacing(selectedObjs, spacing);
            if (naveragespacing <= 0)
                naveragespacing = 1;
            for (int i = 1; i < selectedObjs.Count; i++)
            {
                obj = (DiagramEntity)selectedObjs[i];
                DiagramEntity obj0 = (DiagramEntity)selectedObjs[i-1];
                Rectangle rt = obj.Bounds;
                switch (spacing)
                {

                    case EntitiesSpacing.SPACING_HORIZONTAL:
                        {
                            
                            rt.X = obj0.Bounds.Right + naveragespacing;
                        }
                        break;
                    case EntitiesSpacing.SPACING_VERTICAL:
                        {
                            rt.Y = obj0.Bounds.Bottom + naveragespacing;
                        }
                        break;
                    default:
                        break;
                }
                obj.Bounds = rt;
            }

        }
        /// <summary>
        /// check if given entity overlapped with others entities
        /// If overlapped, return the rectangle 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Rectangle GetOverlappedRect(DiagramEntity entity)
        {
            Rectangle rect = entity.Bounds;
            Rectangle rtReturn = new Rectangle(0,0,0,0);
            for (int i=0; i< GetSize(); i++)
            {
                DiagramEntity d = GetAt(i);
                if (d == entity) //its self, skip
                    continue;

                Rectangle rt = d.Bounds;
                if (rect.IntersectsWith(rt))
                {
                    Rectangle r = rect;
                    r.Intersect(rt);
                    if (rtReturn.IsEmpty)
                        rtReturn = r;
                    else
                    {
                        Rectangle rtTmp = rtReturn;
                        
                        rtTmp.X = Math.Min(rtReturn.X, r.X);
                        rtTmp.Y =Math.Min(rtReturn.Y,  r.Y);
                        int right = Math.Max(rtReturn.Right , r.Right);
                        int bottom = Math.Max(rtReturn.Bottom , r.Bottom);
                        rtTmp.Width = right - rtTmp.X;
                        rtTmp.Height = bottom - rtTmp.Y;

                        rtReturn = rtTmp;
                        
                    }
                    
                }
            }
            return rtReturn;
        }

        /// <summary>
        /// check if given entity overlapped with others entities
        /// If overlapped, return the rectangle 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GetOverlappedRect2(DiagramEntity entity, List<Rectangle> lstOverlapped)
        {
            lstOverlapped.Clear();
            Rectangle rect = entity.Bounds;
            Rectangle rtReturn = new Rectangle(0, 0, 0, 0);
            for (int i = 0; i < GetSize(); i++)
            {
                DiagramEntity d = GetAt(i);
                if (d.TypeName == DiagramKeyMatrix.KEY_MATRIX)
                {
                    //continue;
                    ((DiagramKeyMatrix)d).GetOverlappedRect2(entity, lstOverlapped);
                }
                else
                {
                    if (d == entity) //its self, skip
                        continue;


                    Rectangle rt = d.Bounds;
                    if (rect.IntersectsWith(rt))
                    {
                        Rectangle r = rect;
                        r.Intersect(rt);
                        lstOverlapped.Add(r);



                    }
                }
            }
            return lstOverlapped.Count;
        }


        public void show_entity_icon(bool bShow)
        {
            for (int i = 0; i < GetSize(); i++)
            {
                DiagramEntity d = GetAt(i);
                d.ShowLogo = bShow;
            }
        }
        /// <summary>
        /// according to the type get a unique name.
        /// </summary>
        /// <param name="strTypeName"></param>
        /// <returns></returns>
        public string get_key_unique_title(string strTypeName)
        {
            string strType = "";
            switch(strTypeName)
            {
                case DiagramKey.KEY_BUTTON:
                    {
                        strType = "Key";
                    }
                    
                    break;
                case DiagramKey.KEY_SLIDEH:
                    {
                        strType = "SlideH";
                    }
                    break;
                case DiagramKey.KEY_SLIDEV:
                    {
                        strType = "SlideV";
                    }
                    break;
                default:
                    break;
            }
            string s = "";
            for (int i=1; i< 256; i++)
            {
                s = string.Format("{0}{1}", strType, i.ToString());
                if (exist_same_title(s))
                    continue;
                else
                    return s;
            }
            return "";
        }

        public bool exist_same_title(string title)
        {
            int ncount = m_objs.Count;

            for (int i=0; i< ncount; i++)
            {
                DiagramEntity entity = GetAt(i);
                if (entity == null)
                    continue;
                if (entity.Title == title)
                    return true;
            }
            return false;
        }

        /************************************************************************/
        /* 
         * 
         */
        /************************************************************************/
        private int GetTabOrderedEntities(List<DiagramEntity> ar)
        {
            ar.Clear();
            for (int i = 0; i < m_objs.Count; i++)
            {
                ar.Add((DiagramEntity)m_objs[i]);
            }
            List<DiagramEntity> arRowsHeader = new List<DiagramEntity>();

            //find rows, 
            List<DiagramEntity> arCopied = new List<DiagramEntity>();
            
            ar.Sort(SortEntityX);
            AppendListData(arCopied, ar);

            for (int i = 0; i < ar.Count; i++ )
            {
                if (!IsBehindEntity(arRowsHeader, ar[i]))
                {
                    arRowsHeader.Add(ar[i]);
                    
                }
                else
                {
                    arCopied.Remove(ar[i]);
                }

            }
            //arrange entities to rows.
            //sort header first
            arRowsHeader.Sort(SortEntityY);
            //
            List<DiagramEntity> arSorted = new List<DiagramEntity>();
            List<DiagramEntity> arRow = new List<DiagramEntity>();
            for (int i = 0; i < arRowsHeader.Count; i++ )
            {
                DiagramEntity entity = arRowsHeader[i];
                arRow.Clear();
           //     arRow.Add(entity);
                GetAllBehindThisEntity(ar, entity, arRow);
                RemoveListData(ar, arRow);
                arRow.Sort(SortEntityX);
                AppendListData(arSorted, arRow);
            }

            //    ar.Sort(SortEntityY2);
            //ar.Sort(SortEntityY);
            //ar.Sort(SortEntityX);

            ar.Clear();
            AppendListData(ar, arSorted);
            return ar.Count;
        }
        private void RemoveListData(List<DiagramEntity> arDest, List<DiagramEntity> arSrc)
        {
            for (int i = 0; i < arSrc.Count; i++)
            {
                arDest.Remove(arSrc[i]);
                
            }
        }
        private void AppendListData(List<DiagramEntity> arDest, List<DiagramEntity> arSrc)
        {
            for (int i=0; i< arSrc.Count; i++)
            {
                arDest.Add(arSrc[i]);
            }
        }

        private bool IsBehindEntity(List<DiagramEntity> ar, DiagramEntity entity)//, int nIndex)
        {
            Rectangle rtEntity = entity.PhyBounds;
            //int Y = rtEntity.Top;
            //int nIndex = ;
            int nCount = ar.Count;
            //if (nIndex >= ar.Count)
            //    nIndex = ar.Count;
            for (int i= 0; i< nCount; i++)
            {
                DiagramEntity obj = (DiagramEntity)ar[i];
                Rectangle rtObj = obj.PhyBounds;
                rtObj.X = 0;
                rtObj.Width = KB9Const.UNITS_CX;
                rtObj.Intersect(rtEntity);
                if (!rtObj.IsEmpty)
                    return true;

                //if (Y >= obj.PhyBounds.Top &&
                //    Y <= obj.PhyBounds.Bottom)
                //    return true;
            }
            return false;
        }

        private bool GetAllBehindThisEntity(List<DiagramEntity> arOriginal, DiagramEntity entity,List<DiagramEntity> arRow)
        {
            Rectangle rtEntity = entity.PhyBounds;
            rtEntity.Width = KB9Const.UNITS_CX;
            rtEntity.X = 0;
            //int T = rtEntity.Top;
            //int B = rtEntity.Bottom;

            for (int i = 0; i < arOriginal.Count; i++)
            {
                DiagramEntity obj = (DiagramEntity)arOriginal[i];
                Rectangle rtObj = obj.PhyBounds;
                rtObj.Intersect(rtEntity);
                
                //if (obj == entity)
                //    continue;
                //int y = obj.PhyBounds.Top;
                //if (y >= T && y <= B)
                if (!rtObj.IsEmpty)
                {
                    arRow.Add(obj);
                }
                 
            }
            return true;
        }

        public DiagramEntity GetNextTabEntity(DiagramEntity entity)
        {
            
            List<DiagramEntity> ar = new List<DiagramEntity>();
            if (GetTabOrderedEntities(ar) <= 0)
                return null;
            
            if (entity == null)
                return ar[0];
            int nindex = -1;
            for (int i=0; i< ar.Count; i++)
            {
                if (ar[i] == entity)
                    nindex = i;
            }
            if (nindex < 0)
                return null;
            nindex++;
            if (nindex >= ar.Count)
                nindex = 0;
            return ar[nindex];


        }

        //Sort(Comparison<(Of <(T>)>))方法排序,这中方法需要先编写一个对象比较的方法,然后
        //把这个比较方法委托给List的Sort方法。
        //对象比较的方法
        private static int SortEntityX(DiagramEntity obj1, DiagramEntity obj2)
        {
            int res = 0;
            if ((obj1 == null) && (obj2 == null))
            {
                return 0;
            }
            else if ((obj1 != null) && (obj2 == null))
            {
                return 1;
            }
            else if ((obj1 == null) && (obj2 != null))
            {
                return -1;
            }
            if (obj1.PhyBounds.X > obj2.PhyBounds.X)
            {
                res = 1;
            }
            else if (obj1.PhyBounds.X < obj2.PhyBounds.X)
            {
                res = -1;
            }
            else
            {
                if (obj1.PhyBounds.Y > obj2.PhyBounds.Y)
                {
                    res = 1;
                }
                else if (obj1.PhyBounds.Y < obj2.PhyBounds.Y)
                {
                    res = -1;
                }
                else
                {//x==x, y==y, check text
                    res = obj1.Title.CompareTo(obj2.Title);




                }
            }
            return res;
        }
        private static int SortEntityY(DiagramEntity obj1, DiagramEntity obj2)
        {
            int res = 0;
            if ((obj1 == null) && (obj2 == null))
            {
                return 0;
            }
            else if ((obj1 != null) && (obj2 == null))
            {
                return 1;
            }
            else if ((obj1 == null) && (obj2 != null))
            {
                return -1;
            }
            if (obj1.PhyBounds.Y > obj2.PhyBounds.Y)
            {
                res = 1;
                if (obj1.PhyBounds.Y < obj2.PhyBounds.Bottom)
                {//
                    //if (obj1.PhyBounds.X > obj2.PhyBounds.X)
                    //{
                    //    res = 1;
                    //}
                    if (obj1.PhyBounds.X < obj2.PhyBounds.X)
                    {
                        res = -1;
                    }
                }
            }
            else if (obj1.PhyBounds.Y < obj2.PhyBounds.Y)
            {
                res = -1;
                //if (obj1.PhyBounds.Y < obj2.PhyBounds.Bottom)
                //{//
                //    if (obj1.PhyBounds.X > obj2.PhyBounds.X)
                //    {
                //        res = 1;
                //    }
                //    else if (obj1.PhyBounds.X < obj2.PhyBounds.X)
                //    {
                //        res = -1;
                //    }
                //}
            }
            else
            {
                if (obj1.PhyBounds.X > obj2.PhyBounds.X)
                {
                    res = 1;
                }
                else if (obj1.PhyBounds.X < obj2.PhyBounds.X)
                {
                    res = -1;
                }
                else
                {//x==x, y==y, check text
                    res = obj1.Title.CompareTo(obj2.Title);




                }
            }
            return res;
        }

        private static int SortEntityY2(DiagramEntity obj1, DiagramEntity obj2)
        {
            int res = 0;
            if ((obj1 == null) && (obj2 == null))
            {
                return 0;
            }
            else if ((obj1 != null) && (obj2 == null))
            {
                return 1;
            }
            else if ((obj1 == null) && (obj2 != null))
            {
                return -1;
            }
            if (obj1.PhyBounds.Y > obj2.PhyBounds.Y)
            {
                res = 1;
               
            }
            else if (obj1.PhyBounds.Y < obj2.PhyBounds.Y)
            {
                res = -1;
               
            }
            else
            {
               
            }
            return res;
        }

        public void SetAllEntitiesModified(bool bModifed)
        {
            int ncount = m_objs.Count;
            for (int i=0; i< ncount; i++)
            {
                GetAt(i).ContentModified = bModifed;
                GetAt(i).PropertiesModified = bModifed;
            }
        }

        public bool IsMatrixEntity(DiagramEntity entity)
        {
            int ncount = m_objs.Count;
            for (int i = 0; i < ncount; i++)
            {
                if (GetAt(i).TypeName == DiagramKeyMatrix.KEY_MATRIX)
                {
                    DiagramKeyMatrix matrix = (DiagramKeyMatrix)GetAt(i);
                    for (int j=0; j< matrix.Entities.Count; j++)
                    {
                        if (matrix.Entities[j] == entity)
                            return true;
                    }
                }
                
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace KB9Utility
{
    public class DiagramClipboardHandler
    {


        private List<object> m_paste = new List<object>();
            ///////////////////////////////////
    /* ==========================================================================
	    Class :			CDiagramClipboardHandler

	    Author :		Johan Rosengren, Abstrakt Mekanik AB

	    Date :			2004-04-30

	    Purpose :		"CDiagramClipboardHandler" handles the copy/paste 
					    functionality for a "CDiagramEntityContainer". It's a 
					    separate class to allow several containers to share 
					    the same clipboard in an MDI-application.

	    Description :	"CDiagramClipboardHandler" copy/paste is implemented as 
					    a "CObArray" with "CDiagramEntity"-derived objects. As 
					    soon as objects are put to the 'clipboard', they are 
					    cloned into the paste array. As soon as objects are 
					    pasted, they are cloned from the paste array. 

	    Usage :			"CDiagramEntityContainer" takes a pointer to an instance 
					    of "CDiagramClipboardHandler". The clipboard handler must 
					    live as long as the "CDiagramEntityContainer". Several 
					    "CDiagramEntityContainer"'s can share the same clipboard 
					    handler.
       ========================================================================
					    26/6 2004	Added group handling (Unruled Boy).
       ========================================================================*/



    /////////////////////////////////////////////////////////////////////////////
    // 

       public DiagramClipboardHandler()
    /* ============================================================
	    Function :		CDiagramClipboardHandler::CDiagramClipboardHandler
	    Description :	Constructor
	    Access :		Public

	    Return :		void
	    Parameters :	none

	    Usage :			

       ============================================================*/
        {
        }

        ~DiagramClipboardHandler()
        /* ============================================================
	        Function :		CDiagramClipboardHandler::~CDiagramClipboardHandler
	        Description :	Destructor
	        Access :		Public
        					
	        Return :		void
	        Parameters :	none

	        Usage :			

           ============================================================*/
        {

	        ClearPaste();

        }

        public void Copy( DiagramEntity obj )
        /* ============================================================
	        Function :		CDiagramClipboardHandler::Copy
	        Description :	Copies the object "obj" to the 'clipboard'.
	        Access :		Public
        					
	        Return :		void
	        Parameters :	CDiagramEntity* obj	-	The object to copy.	
        					
	        Usage :			Call in response to a Copy-command. Note 
					        that "obj" will only be copied to the 
					        clipboard, not the screen. See also the 
					        functions for copy/paste below.

           ============================================================*/
        {

	        ClearPaste();
	        DiagramEntity newobj = obj.Clone();
	        newobj.Selected = ( true );
	        newobj.MoveRect( 10, 10 );
	        m_paste.Add( newobj );

        }

        public void CopyAllSelected( DiagramEntityContainer container )
        /* ============================================================
	        Function :		CDiagramClipboardHandler::CopyAllSelected
	        Description :	Clones all selected object to the paste 
					        array.
	        Access :		Public
        					
	        Return :		void
	        Parameters :	none

	        Usage :			Call to copy all selected objects to the 
					        clipboard. "Paste" will put them on screen.

           ============================================================*/
        {

	        ClearPaste();
            List<object> arr = container.GetData();
	        

	        int	max = arr.Count;
	        for( int t = 0 ; t < max ; t++ )
	        {
		        DiagramEntity obj = (DiagramEntity)( arr[ t ] );
		        if( obj.Selected )
		        {
			        DiagramEntity newobj = obj.Clone();
			        newobj.Selected = ( true );
			        newobj.MoveRect( 10, 10 );
                    newobj.GroupID = (obj.GroupID);
			        m_paste.Add( newobj );
		        }
	        }

        }

        public int ObjectsInPaste()
        /* ============================================================
	        Function :		CDiagramClipboardHandler::ObjectsInPaste
	        Description :	Returns the number of objects in the paste 
					        array.
	        Access :		Public
        					
	        Return :		int		-	The number of objects.
	        Parameters :	none

	        Usage :			Call to get the number of objects in the 
					        clipboard.

           ============================================================*/
        {

	        return m_paste.Count;

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

	        //int count = m_paste.Count - 1;
	       // for( int t = count ; t >= 0 ; t-- )
		     //   delete static_cast< CDiagramEntity* >( m_paste.GetAt( t ) );

	        m_paste.Clear();

        }

        public List<DiagramEntity> Paste( DiagramEntityContainer container)
        /* ============================================================
	        Function :		CDiagramClipboardHandler::Paste
	        Description :	Clones the contents of the paste array 
					        into the container data array.
	        Access :		Public
        					
	        Return :		void
	        Parameters :	none

	        Usage :			Call to paste the contents of the clipboard 
					        to screen.

           ============================================================*/
        {

	        
            List<int> oldgroup = new List<int>();
	        List<int>	newgroup = new List<int>();

	        int max = m_paste.Count;
	        for( int t = 0 ; t < max ; t++ )
	        {
		        DiagramEntity obj = (DiagramEntity)( m_paste[t] );
                if (obj.GroupID != 0)
		        {
			        int size = oldgroup.Count;
			        bool found = false;
			        for( int i = 0 ; i < size ; i++ )
                        if (obj.GroupID == (int)(oldgroup[i]))
					        found = true;

			        if( !found )
			        {
                        oldgroup.Add(obj.GroupID);
				        newgroup.Add( GroupFactory.GetNewGroup() );
			        }
		        }
	        }

            List<DiagramEntity> pastedObjs = new List<DiagramEntity>();

	        for( int t = 0 ; t < max ; t++ )
	        {
		        DiagramEntity obj = ( DiagramEntity)( m_paste[t] );
		        DiagramEntity clone = obj.Clone();

		        int group = 0;
                if (obj.GroupID != 0)
		        {
			        int size = oldgroup.Count;
			        for( int i = 0 ; i < size ; i++ )
                        if (obj.GroupID == (int)(oldgroup[i]))
					        group = newgroup[ i ];
		        }

                clone.GroupID = (group);
                clone.setID(DiagramEntity.createNewGUID());
		        container.Add( clone );
                pastedObjs.Add(clone);
	        }

            return pastedObjs;

        }

       public  List<object> GetData() 
        /* ============================================================
	        Function :		CDiagramClipboardHandler::GetData
	        Description :	Get a pointer to the clipboard data
	        Access :		Public
        					
	        Return :		CObArray*	-	The clipboard data
	        Parameters :	none

	        Usage :			Call to get the clipboard data.

           ============================================================*/
        { 
        	
	        return m_paste; 

        }

    }
}

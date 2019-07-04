using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public class DiagramPropertyDlg: Form
    {
        
	private DiagramEntity	m_entity;
	private Control			m_redrawWnd;
   /* ==========================================================================
	Class :			CDiagramPropertyDlg

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-03-31

	Purpose :		"CDiagramPropertyDlg" represents a property dialog for a 
					"CDiagramEntity" object.	

	Description :	The class is a pure virtual class derived from "CDialog".

	Usage :			Create a property dialog in the resource editor, but 
					map it to "CDiagramPropertyDlg" instead of "CDialog". Add 
					an override of the pure virtual function "SetValues". 
					This function is expected to put data from "m_entity" in 
					appropriate fields in the dialog. Set data in "m_entity" 
					as appropriate (from an Apply-button handler or 
					whatever appropriate), and call "Redraw()"
					if needed.

					In the "CDiagramEntity"-derived class, add a member of 
					the "CDiagramPropertyDlg"-derived class, and call 
					"SetPropertyDialog" in the constructor.

					The dialog is displayed as a modeless dialog. 
					"CDiagramEditor" will hide the dialog automatically when 
					another object is selected, no special Close-button is 
					necessary.

   ========================================================================*/



            DiagramPropertyDlg( int res, Control parent ) 
        /* ============================================================
	        Function :		CDiagramPropertyDlg::CDiagramPropertyDlg
	        Description :	constructor
	        Access :		Public

	        Return :		void
	        Parameters :	UINT res		-	Resource ID of the 
										        dialog template.
					        CWnd* parent	-	Parent of the dialog - 
										        the "CDiagramEditor".
        					
	        Usage :			

           ============================================================*/
        {

	        m_entity = null;
	        m_redrawWnd = null;

        }

        public void SetEntity( DiagramEntity entity )
        /* ============================================================
	        Function :		CDiagramPropertyDlg::SetEntity
	        Description :	Sets the "CDiagramEntity"-entity derived 
					        object that is associated with this dialog.
	        Access :		Public

	        Return :		void
	        Parameters :	CDiagramEntity* entity	-	Set the object 
												        for this dialog.
        					
	        Usage :			Call to set the "CDiagramEntity"-derived
					        object to be associated with this dialog.

           ============================================================*/
        {

	        m_entity = entity;

        }

        DiagramEntity GetEntity()
        /* ============================================================
	        Function :		CDiagramPropertyDlg::GetEntity
	        Description :	Returns the "CDiagramEntity" object of this 
					        dialog
	        Access :		Public

	        Return :		CDiagramEntity*	-	The object attached to 
										        this dialog
	        Parameters :	none

	        Usage :			Call to get the "CDiagramEntity"-derived 
					        object associated to this dialog.

           ============================================================*/
        {

	        return m_entity;

        }

        public bool Create( int nIDTemplate, Control pParentWnd )
        /* ============================================================
	        Function :		CDiagramPropertyDlg::Create
	        Description :	Creates the dialog.
	        Access :		Public

	        Return :		BOOL				-	"TRUE" if window was 
											        created ok.
	        Parameters :	UINT nIDTemplate	-	Resource id of 
											        dialog template.
					        CWnd* pParentWnd	-	Parent of dialog 
											        (normally the 
											        "CDiagramEditor")
        					
	        Usage :			Called internally to create the property 
					        dialog.

           ============================================================*/
        {

            //BOOL result;

            //result = CDialog::Create( nIDTemplate, pParentWnd );
	        SetRedrawWnd( pParentWnd );

	        return true;

        }

        public Control GetRedrawWnd() 
        /* ============================================================
	        Function :		CDiagramPropertyDlg::GetRedrawWnd
	        Description :	Get the window that should be redrawn when 
					        changes are made in this dialog.
	        Access :		Public

	        Return :		CWnd*	-	The window
	        Parameters :	none

	        Usage :			Call to get the window that should be 
					        redrawn when applying changes in the box. 
					        This member is used as the editor will not 
					        redraw properly in a MDI-application 
					        ("GetParent()" returns the frame instead of 
					        the editor).

           ============================================================*/
        { 

	        return m_redrawWnd; 

        }

        public void SetRedrawWnd( Control redrawWnd ) 
        /* ============================================================
	        Function :		CDiagramPropertyDlg::SetRedrawWnd
	        Description :	Set the window that should be redrawn in 
					        response to changes in this dialog.
	        Access :		Public

	        Return :		void
	        Parameters :	CWnd* redrawWnd	-	Normally the editor.
        					
	        Usage :			Call to set the window that should be 
					        redrawn when applying changes in the box. 
					        This member is used as the editor will not 
					        redraw properly in a MDI-application 
					        ("GetParent()" returns the frame instead of 
					        the editor).

           ============================================================*/
        { 

	        m_redrawWnd = redrawWnd; 

        }

        public void Redraw() 
        /* ============================================================
	        Function :		CDiagramPropertyDlg::Redraw
	        Description :	Redraw the parent window of the dialog.
	        Access :		Public

	        Return :		void
	        Parameters :	none

	        Usage :			Call to redraw the editor window when 
					        applying changes in the box. 
					        This member is used as the editor will not 
					        redraw properly in a MDI-application 
					        ("GetParent()" returns the frame instead of 
					        the editor).


           ============================================================*/
        { 

	        Control wnd = GetRedrawWnd();
	        if( wnd != null )
		        wnd.Refresh();// ->RedrawWindow();

        }

	    public virtual void	SetValues()
        {

        }


    }
}

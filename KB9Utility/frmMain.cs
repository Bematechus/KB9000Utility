using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using KB9Utility.Properties; //for setting
using Microsoft.Win32;
using System.Reflection;
//For add new properties
/************************************************************************/
/* 
 * 1.project properties 
 * 2.To settings, add new one
 * 3.add "using [your project].Properties"
 * 4.You can use them now. Settings.Default.***
 * 5.Save setting 
 *    Settings.Default.Save（
 * 
 * 
 * 
 * //------------------------------------------------
 * 1.       Touch area is 170mm x 60mm but the button coordinates should be 340 x 120 as we are using 0.5mm grid.  
 * This touch coordinate (340 x 120) will be used in firmware (0-339, 0-119).

2.       In xml file, the pixel coordinates are used instead of touch coordinates.  
 * I think it’s better to use touch coordinates.  As users may use 72 dpi or 96 dpi monitors (or others), 
 * the actual pixel coordinates may be different from PC to PC.  If touch coordinates are used as property, 
 * the utility can calculate the correct pixel locations using the dpi resolution value and display correctly.


 * */
/************************************************************************/
namespace KB9Utility
{
    public partial class frmMain : Form
    {
        private const string DEFAULT_FILE_NAME = "Key Template.xml";
        private const string DEFAULT_TITLE = "KB9000 Utility";
        public static frmMain g_MainForm = null;

        protected MruStripMenu _mruMenu;
        static string _MRURegKey = "SOFTWARE\\LogicControls\\KB9000Utility";

        public enum EDIT_STATE
        {
            None = -1,
            Unknown = 0,
            Container_Focused,
            EntityButton_Focused,
            EntitySlideV_Focused,
            EntitySlideH_Focused,
            EntityMatrix_Focused,
            CodeEditor_Focused,
            TitleEditor_Focused, //
            PropertyGrid_Focused,


        }

        public enum API_FUNCTION
        {
            Read,
            Write,
            Detect,
            FreeMemory,
            DLLVersion,
        }

        private EDIT_STATE _EditingState = EDIT_STATE.None;
        protected EDIT_STATE EditingState
        {
            get
            {
                return _EditingState;
            }
            set
            {
               // if (_EditingState != value)
                {


                    _EditingState = value;
                 //   System.Diagnostics.Debug.Print(_EditingState.ToString());
                    UpdateGUIState();
                }
            }
        }

        public frmMain()
        {
            InitializeComponent();
            editorMain.ParentForm = this;

            UpdateGUI();
          
            frmMain.g_MainForm = this;
          
            //editorMain.ShowTips = Settings.Default.ShowTooltips;
            init_mru();
            this.EditingState = EDIT_STATE.Unknown;
        }

        private void init_mru()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(_MRURegKey);
            _mruMenu = new MruStripMenu(fileRecent, new MruStripMenu.ClickedHandler(OnMruFile), _MRURegKey + "\\MRU",true, 5 );


        }
        private void OnMruFile(int number, String filename)
        {
            if (editorMain.Modified)
            {

                //System.Resources.ResourceManager rm;
                //rm = new System.Resources.ResourceManager("KB9Utility.Properties.Resources", Assembly.GetExecutingAssembly());

                //string strmsg = rm.GetString("Msg_Save_File_Changes");

                

                              

                DialogResult result = MessageBox.Show(
                                 "Do you want to save current file changes?",
                                "Confirm",
                                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                // You may want to use exception handlers in your code

                if (result == DialogResult.Yes)
                {

                   saveToolStripMenuItem_Click(null, null);
                    
                }
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                
            }
            _mruMenu.SetFirstFile(number);

            open_file(filename);
            editorMain.Modified = false;
            editorMain.Focus();
            this.editorMain_OnShowDiagramEntityProperties(editorMain, null);
            editorMain.Refresh();

        }
        private string _EditingFile = "";
        public string EditingFile
        {
            get 
            {
                if (_EditingFile.Length <= 0)
                    _EditingFile = DEFAULT_FILE_NAME;


                return Util.GetFileName( _EditingFile);
            }
            set
            {
                _EditingFile = value;
            }
        }



        /// <summary>
        /// save the data to xml file.
        /// the file name is "editing.xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            string s = this._EditingFile;
            string f = "";
            if (s.IndexOf("\\") >= 0)
                f = s;
            else
                f = GetAppPath() + "\\" + DEFAULT_FILE_NAME;
            //Save_Configuration(default_configuration_file_name());
            Save_Configuration(f);
            editorMain.Refresh();
            KB9Logger.Log("save file: " + f);
        }

        private void Save_Configuration(string fileName)
        {
            CLCIXML xml = new CLCIXML();
            xml.new_doc_with_root("KB9000");

            editorMain.ExportToXml(xml);
            xml.write_file(fileName);
        }
        /// <summary>
        /// It was fired while editor container selected one entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="entity"></param>
        private void editorMain_OnShowDiagramEntityProperties(DiagramEditor sender, DiagramEntity entity)
        {
            if (editorMain.GetSelectCount() <= 1)
            {

                //don't show matrix entity property
                if (entity != null && entity.TypeName == DiagramKeyMatrix.KEY_MATRIX)
                {
                    DiagramKeyMatrix obj = (DiagramKeyMatrix)entity;
                    //entity = obj.get_selected_entity();
                    if (obj.get_selected_entity() == null)
                    {
                        //pgProperties.SelectedObject = null;// obj.get_selected_entity();
                        //return;
                    }
                    else
                    {
                        entity = obj.get_selected_entity();
                    }
                    //pgProperties.SelectedObject = obj.get_selected_entity();
                        
                    //return;

                }
                ////////////
                //System.Diagnostics.Debug.Print("editorMain_OnShowDiagramEntityProperties");
                bool bshowkeys = false;

                if (pgProperties.SelectedObject != entity)
                    bshowkeys = true;

                pgProperties.SelectedObject = null;
                if (entity == null)
                {
                    pgProperties.SelectedObject = sender;
                    this.EditingState = EDIT_STATE.Container_Focused;
                }
                else
                {
                    if (editorMain.GetSelectCount() == 1)
                    {
                        if (entity is DiagramKeyMatrix)
                        {
                            
                            //List<DiagramEntity> ar = new List<DiagramEntity>();


                            pgProperties.SelectedObjects = ((DiagramKeyMatrix)entity).Entities.ToArray();
                            
                        }
                        else
                        {
                            pgProperties.SelectedObject = entity;
                        }
                        //editorMain.Front();
                    }
                    else
                        pgProperties.SelectedObject = null;

                }
                pgProperties.Refresh();
                update_state(entity);

                //20141125
                if (bshowkeys) //old is if (!bshowkeys) , but it is worked.
                    show_keycodes(entity);

                //if double clicked, it should start to edit contents.
                //but the showproperties called in clickup event, 
                //we need to prevent the toolbar reseted in showpropertes function.
                if (GetEditingTextBox() != null)
                {
                    lblKeyCode0_Enter(null, null);
                }
            }
            else
            {
                show_mulitple_selected_entities_properties();
                enable_tab_pages(false, 1);
                UpdateGUI();
                //List<DiagramKey> ar = new List<DiagramKey>();
                //if (editorMain.GetSelectedObjects(ar) > 0)
                //    pgProperties.SelectedObjects = ar.ToArray();
                //else
                //    pgProperties.SelectedObjects = null;
            }
         
        }

        private void show_mulitple_selected_entities_properties()
        {
            List<DiagramEntity> ar = new List<DiagramEntity>();
           // List<DiagramKey> ar = new List<DiagramKey>();
            if (editorMain.GetSelectedObjects(ar) > 0)
                pgProperties.SelectedObjects = ar.ToArray();
            else
                pgProperties.SelectedObjects = null;
        }

        private void update_state(DiagramEntity entity)
        {
            if (entity == null)
            {
                this.EditingState = EDIT_STATE.Container_Focused;
                return;
            }
            switch (entity.TypeName)
            {
                case DiagramKey.KEY_BUTTON:
                    this.EditingState = EDIT_STATE.EntityButton_Focused;
                    break;
                case DiagramKey.KEY_SLIDEH:
                    this.EditingState = EDIT_STATE.EntitySlideH_Focused;
                    break;
                case DiagramKey.KEY_SLIDEV:
                    this.EditingState = EDIT_STATE.EntitySlideV_Focused;
                    break;
                case DiagramKeyMatrix.KEY_MATRIX:
                    this.EditingState = EDIT_STATE.EntityMatrix_Focused;
                    break;
                default:
                    this.EditingState = EDIT_STATE.Unknown;
                    break;

            }
        }

      
       

       
        private void show_kb9textbox_text(KB9TextBox txt,  string strText)
        {
            txt.EnableTextChangedEvent = false;
            txt.Text = strText;
            
            txt.EnableTextChangedEvent = true;
        }
        /*
        private void show_keycodes(DiagramEntity entity)
        {
            tabCodes.SuspendLayout();
            this.PauseTextBoxChangedEvent = true;
            if (editorMain.GetSelectCount() > 1)
                entity = null;
            if (entity == null)
            {

                tabCodes.Enabled = false;
                enable_tab_pages(false, 1);
                
            }
            else
            {
                tabCodes.Enabled = true;
                
                switch(entity.TypeName)
                {
                    case DiagramKey.KEY_BUTTON:
                        {
                            enable_tab_pages(true, 1);
                            tpCode0.Text = entity.Title;

                            DiagramKeyArea key = (DiagramKeyArea)entity;
                            string s = key.KeyCode.ToString();
                            show_kb9textbox_text(lblKeyCode0,  s);
                            lblKeyCode0.ClearUndo();
                        }
                        break;
                    case DiagramKey.KEY_SLIDEV:
                        {
                            enable_tab_pages(true, 4);
                            DiagramKeySlideV key = (DiagramKeySlideV)entity;
                            string title = entity.Title;
                            tpLabel.Text = title;
                            tpCode0.Text = "Up";
                            show_kb9textbox_text(lblKeyCode0 , key.SlideUp.ToString());
                            lblKeyCode0.ClearUndo();

                            //tpCode1.Text = title + ": Up Hold";
                            tpCode1.Text = "Up Hold";
                            show_kb9textbox_text(lblKeyCode1 ,  key.SlideUpHold.ToString());
                            lblKeyCode1.ClearUndo();

                            //tpCode2.Text = title + ": Down";
                            tpCode2.Text = "Down";
                            show_kb9textbox_text(lblKeyCode2 ,  key.SlideDown.ToString());
                            lblKeyCode2.ClearUndo();

                            //tpCode3.Text = title + ": Down Hold";
                            tpCode3.Text = "Down Hold";
                            show_kb9textbox_text(lblKeyCode3 , key.SlideDownHold.ToString());
                            lblKeyCode3.ClearUndo();
                            
                            
                        }
                        break;
                    case DiagramKey.KEY_SLIDEH:
                        {
                            enable_tab_pages(true, 4);
                            DiagramKeySlideH key = (DiagramKeySlideH)entity;
                            string title = entity.Title;

                            tpLabel.Text = title;

                            tpCode0.Text = "Left";
                            lblKeyCode0.Text = key.SlideLeft.ToString();
                            lblKeyCode0.ClearUndo();

                            //tpCode1.Text = title + ": Left Hold";
                            tpCode1.Text = "Left Hold";
                            lblKeyCode1.Text = key.SlideLeftHold.ToString();
                            lblKeyCode1.ClearUndo();

                            //tpCode2.Text = title + ": Right";
                            tpCode2.Text = "Right";
                            lblKeyCode2.Text = key.SlideRight.ToString();
                            lblKeyCode2.ClearUndo();

                            //tpCode3.Text = title + ": Right Hold";
                            tpCode3.Text = "Right Hold";
                            lblKeyCode3.Text = key.SlideRightHold.ToString();
                            lblKeyCode3.ClearUndo();
                        }
                        break;
                    default:
                        break;

                }
                //lblKeyCode.Text = entity.GetTooltipsText();
            }
            this.PauseTextBoxChangedEvent = false;
            tabCodes.ResumeLayout();
        }
        */

        private void show_keycodes(DiagramEntity entity)
        {
            tabCodes.SuspendLayout();
            this.PauseTextBoxChangedEvent = true;
            if (editorMain.GetSelectCount() > 1)
                entity = null;
            if (entity == null)
            {

                tabCodes.Enabled = false;
                //enable_tab_pages(false, 1);

            }
            else
            {
                tabCodes.Enabled = true;

                switch (entity.TypeName)
                {
                    case DiagramKey.KEY_BUTTON:
                        {
                            //enable_tab_pages(true, 1);
                            //tpCode0.Text = entity.Title;

                            DiagramKeyArea key = (DiagramKeyArea)entity;
                            string s = key.KeyCode.ToString();
                            show_kb9textbox_text(lblKeyCode0, s);
                            lblKeyCode0.ClearUndo();
                        }
                        break;
                    case DiagramKey.KEY_SLIDEV:
                        {
                            //enable_tab_pages(true, 4);
                            DiagramKeySlideV key = (DiagramKeySlideV)entity;
                            //string title = entity.Title;
                            //tpLabel.Text = title;
                            //tpCode0.Text = "Up";
                            show_kb9textbox_text(lblKeyCode0, key.SlideUp.ToString());
                            lblKeyCode0.ClearUndo();

                            //tpCode1.Text = title + ": Up Hold";
                           // tpCode1.Text = "Up Hold";
                            show_kb9textbox_text(lblKeyCode1, key.SlideUpHold.ToString());
                            lblKeyCode1.ClearUndo();

                            //tpCode2.Text = title + ": Down";
                            //tpCode2.Text = "Down";
                            show_kb9textbox_text(lblKeyCode2, key.SlideDown.ToString());
                            lblKeyCode2.ClearUndo();

                            //tpCode3.Text = title + ": Down Hold";
                            //tpCode3.Text = "Down Hold";
                            show_kb9textbox_text(lblKeyCode3, key.SlideDownHold.ToString());
                            lblKeyCode3.ClearUndo();


                        }
                        break;
                    case DiagramKey.KEY_SLIDEH:
                        {
                            enable_tab_pages(true, 4);
                            DiagramKeySlideH key = (DiagramKeySlideH)entity;
                            //string title = entity.Title;

                            //tpLabel.Text = title;

                            //tpCode0.Text = "Left";
                            lblKeyCode0.Text = key.SlideLeft.ToString();
                            lblKeyCode0.ClearUndo();

                            //tpCode1.Text = title + ": Left Hold";
                            //tpCode1.Text = "Left Hold";
                            lblKeyCode1.Text = key.SlideLeftHold.ToString();
                            lblKeyCode1.ClearUndo();

                            //tpCode2.Text = title + ": Right";
                            //tpCode2.Text = "Right";
                            lblKeyCode2.Text = key.SlideRight.ToString();
                            lblKeyCode2.ClearUndo();

                            //tpCode3.Text = title + ": Right Hold";
                            //tpCode3.Text = "Right Hold";
                            lblKeyCode3.Text = key.SlideRightHold.ToString();
                            lblKeyCode3.ClearUndo();
                        }
                        break;
                    default:
                        break;

                }
                //lblKeyCode.Text = entity.GetTooltipsText();
            }
            this.PauseTextBoxChangedEvent = false;
            tabCodes.ResumeLayout();
        }


        private void btnKeyArea_Click(object sender, EventArgs e)
        {
            DiagramKeyArea obj = new DiagramKeyArea();
            obj.Title = editorMain.CreateUniqueName(obj.TypeName);
            editorMain.DrawingObject = obj;
            
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private string CreateApplicationTitle()
        {
            //int n = IntPtr.Size*8;// sizeof(int) * 8;
            //int n = Environment.bit.Is64BitOperatingSystem() ? 64 : 32;
            //string s = " (" + n.ToString() + "bits)";
            return this.EditingFile + " - " + DEFAULT_TITLE;// +s;// KB9000 Utility";
        }
        /*=============================================================
         * According the the editor state, update menu/toolbar status
         ==============================================================*/
        private void UpdateGUI()
        {

            UpdateGUIState();
            this.Text = CreateApplicationTitle();// this.EditingFile + " - KB9000 Utility";
            /*

            int nSelectedCount = editorMain.GetSelectCount();
            int copyCount = editorMain.GetCopyCount();
            int undoCount = editorMain.GetUndoCount();

            

            bool bIsSelected = (nSelectedCount > 0);
            bool bIsMultipleSelected = (nSelectedCount > 1);

            editCopy.Enabled = bIsSelected;
            btnCopy.Enabled = bIsSelected;

            editCut.Enabled = bIsSelected;
            btnCut.Enabled = bIsSelected;

            editDel.Enabled = bIsSelected;
            
            editPaste.Enabled = (copyCount > 0);
            btnPaste.Enabled = editPaste.Enabled;

            editUndo.Enabled = (undoCount > 0);
            btnUndo.Enabled = editUndo.Enabled;

            editUnselect.Enabled = bIsSelected;

            formatAlign.Enabled = bIsMultipleSelected;
            btnAlignLeft.Enabled = bIsMultipleSelected;
            btnAlignCenter.Enabled = bIsMultipleSelected;
            btnAlignRight.Enabled = bIsMultipleSelected;
            btnAlignTop.Enabled = bIsMultipleSelected;
            btnAlignMiddle.Enabled = bIsMultipleSelected;
            btnAlignBottom.Enabled = bIsMultipleSelected;
            btnSameWidth.Enabled = bIsMultipleSelected;
            btnSameHeight.Enabled = bIsMultipleSelected;
            btnSameBoth.Enabled = bIsMultipleSelected;

            formatMakeSame.Enabled = bIsMultipleSelected;
            formatHEqual.Enabled = (nSelectedCount > 2); 
            formatVEqual.Enabled = (nSelectedCount > 2);

            int n = (int)(editorMain.Zoom * 100);
            if (n > tkbZoom.Maximum) n = tkbZoom.Maximum;
            if (n < tkbZoom.Minimum) n = tkbZoom.Minimum;
            tkbZoom.Value = n;

            this.Text = this.EditingFile + " - KB9000 Utility";

            
            show_keycodes(editorMain.GetSelectedObject());
            

            enable_code_editing_buttons((GetEditingTextBox() != null));
            */

            
        }

        public void editCopy_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.OnCopy(null, null);
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.Copy();
            }
            else
                editorMain.Copy();
            UpdateGUI();

        }

        public void editCut_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.OnCut(null, null);
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.Cut();
            }
            else
                editorMain.Cut();
            UpdateGUI();
        }

        public void editPaste_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.OnPaste(null, null);
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.Paste();
            }
            else
                editorMain.Paste(false);
            UpdateGUI();
        }

        public void editDel_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.OnDelete(null, null);
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //SendKeys.Send("{Delete}");
            }
            else
                editorMain.DeleteAllSelected();
            UpdateGUI();
        }

        public void editUndo_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.OnUndo(null, null);
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.Undo();
            }
            else
                editorMain.Undo();

            //editorMain_OnShowDiagramEntityProperties(editorMain, editorMain.GetSelectedObject());

            //UpdateGUI();
        }

        public void editSelectAll_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.SelectAll();
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.SelectAll();
            }
            else
            {


                this.PauseTextBoxChangedEvent = true;
                editorMain.SelectAll();
                this.PauseTextBoxChangedEvent = false;
                //editorMain_OnShowDiagramEntityProperties(editorMain, editorMain.GetSelectedObject());
            }
            UpdateGUI();
        }

        public void editUnselect_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.Unselect();
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.DeselectAll();
            }
            else
                editorMain.Unselect();
            UpdateGUI();
        }

        public void editRemoveAll_Click(object sender, EventArgs e)
        {
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.Clear();
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.Text = string.Empty;
            }
            else
                editorMain.DeleteAll();
            UpdateGUI();
        }

        public void formatAlignLeft_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_LEFT);
            UpdateGUI();
        }

        public void formatAlignRight_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_RIGHT);
            UpdateGUI();
        }

        public void formatAlignTop_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_TOP);
            UpdateGUI();
        }

        public void formatAlignBottom_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_BOTTOM);
            UpdateGUI();
        }

        public void formatMakeSameWidth_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditSameSize(DiagramEntityContainer.EntitiesSameSize.SAME_WIDTH);
            UpdateGUI();
        }

        public void formatMakeSameHeight_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditSameSize(DiagramEntityContainer.EntitiesSameSize.SAME_HEIGHT);
            UpdateGUI();
        }

        public void formatMakeSameBoth_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditSameSize(DiagramEntityContainer.EntitiesSameSize.SAME_BOTH);
            UpdateGUI();
        }

        public void formatHEqual_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditSpacing(DiagramEntityContainer.EntitiesSpacing.SPACING_HORIZONTAL);
            UpdateGUI();
        }

        public void formatVEqual_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditSpacing(DiagramEntityContainer.EntitiesSpacing.SPACING_VERTICAL);
            UpdateGUI();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string f = dlg.FileName;
            string file = f.ToUpper();
            editorMain.UnselectAll(); //prevent the selected mark!

            //Image img = editorMain.CreateImage(); 
            //if (file.IndexOf(".JPG")>0)
            //    img.Save(f, ImageFormat.Jpeg);
            //else if (file.IndexOf(".PNG")>0)
            //    img.Save(f, ImageFormat.Png);
            //else if (file.IndexOf(".BMP") > 0)
            //    img.Save(f, ImageFormat.Bmp);
            //else
            {
                Save_Configuration(f);
                this.EditingFile = f;
                _mruMenu.AddFile(f);
            }
            editorMain.Refresh();
            UpdateGUI();

            KB9Logger.Log("save as file: " + f);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            this.Close();
            //DialogResult result;
            //if (editorMain.Modified)
            //{
            //    result = MessageBox.Show("You data was changed. Do you want to save them then exit?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        saveToolStripMenuItem_Click(null, null);
            //        this.Close();
            //    }
            //    else if (result == DialogResult.No)
            //    {
            //        this.Close();
            //    }
            //    else
            //        return;
            //}

            //else
            //{
            //    result = MessageBox.Show("Are you sure you want to exit this application?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //    if (result == DialogResult.Yes)
            //        this.Close();
            //    else
            //        return;
            //}
        }

        private void fileNew_Click(object sender, EventArgs e)
        {
            //if (editorMain.Modified)
            close_onscreen_kbd();

            if (editorMain.GetObjectCount() >0)
            {
                if (editorMain.Modified)
                {


                    if (!confirm_lost_data(false))
                        return;
                }

                //DialogResult result = MessageBox.Show("The contents will been lost. Do you continue?", "Question", 
                //                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (result != DialogResult.Yes)
                //    return;
            }
            editorMain.New();
            editorMain.reset();

            editorMain_OnShowDiagramEntityProperties(editorMain, null);
            this.EditingFile = DEFAULT_FILE_NAME;
            editorMain.Modified = false;
            UpdateGUI();

        }

        /// <summary>
        /// return application path without last "\"
        /// </summary>
        /// <returns></returns>
        static public string GetAppPath()
        {
            string s = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            int n = s.LastIndexOf("\\");
            return s.Substring(0,n);
            
        }

        private string default_configuration_file_name()
        {
            return GetAppPath() + "\\" + DEFAULT_FILE_NAME;
        }

        private bool is_editing_default_file()
        {
            string s = this.EditingFile + ".xml";

            if (s == DEFAULT_FILE_NAME || 
                this.EditingFile.Length <=0)
                return true;
            return false;
        }

        /************************************************************************/
        /* 
         * true: continue next 
         * false: stop
         */
        /************************************************************************/
        private bool confirm_lost_data(bool bForOpenFile)
        {
            DialogResult result;
            if (!bForOpenFile)
            {//it is for "New"


                if (editorMain.GetObjectCount() > 0)
                {
                    if (is_editing_default_file())
                    {
                         result = MessageBox.Show("The contents will be lost. Do you want to continue?",
                                                                Application.ProductName,
                                                                MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Warning);
                        return (result == DialogResult.Yes);
                    }
                }
            }
            if (!editorMain.Modified)
                return true;

             result = ask_for_save_changes();
            if (result == DialogResult.Yes)
            {
                saveToolStripMenuItem_Click(null, null);
                return true;
            }
            else if (result == DialogResult.No)
            {
                return true;
            }
            else // (result == DialogResult.Cancel)
            {
                return false;
            }
        }

        private void fileOpen_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (!confirm_lost_data(true))
                return;

            //DialogResult result = MessageBox.Show("The contents will been lost. Do you continue?", "Question",
            //                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (result != DialogResult.Yes)
            //    return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                open_file(dlg.FileName);
                _mruMenu.AddFile(dlg.FileName);
                editorMain.Focus();
                //editorMain.New();
                
                //Load_Configuration(dlg.FileName);
                //this.EditingFile = dlg.FileName;
                //UpdateGUI();
                KB9Logger.Log("Open file: " + dlg.FileName);
            }
            editorMain.Focus();
            this.editorMain_OnShowDiagramEntityProperties(editorMain, null);

        }
        private void open_file(string fileName)
        {
            editorMain.New();

            Load_Configuration(fileName);
            this.EditingFile = fileName;
            editorMain.Modified = false;
            editorMain.ClearRedo();
            editorMain.ClearUndo();
            UpdateGUI();
        }

        private void Load_Configuration(string fileName)
        {
            CLCIXML xml = new CLCIXML();
            if (!xml.open_file(fileName, "KB9000", false))
                return;
            editorMain.ImportFromXml(xml);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Load_Configuration(default_configuration_file_name());
            setStatusBarText("");
            //tsMsg0.Text = "";
            tkbZoom.Minimum = (int)decimal.Round(((decimal)editorMain.ZoomMin * 100));
           // tkbZoom.Minimum = (int)(editorMain.ZoomMin*100);
            editorMain.Modified = false;
            //restore setting
            int n = Settings.Default.WinState;

            FormWindowState state = FormWindowState.Normal;
            if ( n>=0)
                state = (FormWindowState)n;
            int winWidth = Settings.Default.WinWidth;
            int winHeight = Settings.Default.WinHeight;

            if (winHeight>0 &&winWidth >0)
            {
                if (state == FormWindowState.Normal )
                {


                    Rectangle rt = this.Bounds;
                    rt.Width = winWidth;
                    rt.Height = winHeight;
                    this.Bounds = rt;
                }
            }

            
            
             this.WindowState = state;
             tkbZoom.Minimum = (int)decimal.Round( ( (decimal) editorMain.ZoomMin * 100));
             editorMain.Zoom = ((double)Settings.Default.Zoom)/100;
             int nval = (int)(editorMain.Zoom * 100);
            if (nval < tkbZoom.Minimum)
                nval = tkbZoom.Minimum;
            if (nval > tkbZoom.Maximum)
                nval = tkbZoom.Maximum;
            tkbZoom.Value = nval;//(int)(editorMain.Zoom * 100);
            

             editorMain.Focus();
             editorMain_OnShowDiagramEntityProperties(editorMain, null);
             bool bLog = Settings.Default.EnableLog;

             KB9Logger.EnableLog(bLog);
             helpLog.Checked = bLog;
             KB9API.EnableDisableLogFile(bLog);//20150112, dll log

             bool bEnable3X = Settings.Default.Enable3X;
             fileEnablePrint3X.Checked = bEnable3X;

            //if (Settings.Default.EnableLog)
            //{
            //    KB9Logger.EnableLog(true);
            //    helpLog.Checked = true;
            //    KB9API.EnableDisableLogFile(true);//20150112, dll log
            //}
            //else
            //{
            //    KB9Logger.EnableLog(false);
            //    helpLog.Checked = false;
            //    KB9API.EnableDisableLogFile(false);//20150112, dll log
            //}

            KB9Logger.Log("KB9Utility started");
        }

        private void helpAbout_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            fileNew_Click(sender, e);
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            fileOpen_Click(sender, e);
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void tsbSaveAs_Click(object sender, EventArgs e)
        {
            saveAsToolStripMenuItem_Click(sender, e);
        }

        private void tsbCut_Click(object sender, EventArgs e)
        {
            editCut_Click(sender, e);
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            editPaste_Click(sender, e);
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            editUndo_Click(sender, e);
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            editCopy_Click(sender, e);
        }

        private DialogResult ask_for_save_changes()
        {
             DialogResult result;
             //result = MessageBox.Show("Do you want to save the changes?", "Question", 
             //                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

             result = frmMsgbox.MsgBox("Question", "Do you want to save the changes?", "Save", "Do not save", "Return to edit", MessageBoxIcon.Question);

            return result;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;
            if (editorMain.Modified)
            {
                e.Cancel = false;
                result = ask_for_save_changes(); // MessageBox.Show("Do you want to save the changes?", "Question", 
                         //               MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(null, null);
                    //this.Close();
                }
                else if (result == DialogResult.No)
                {
                    //this.Close();
                }
                else
                    e.Cancel = true;
                    //return;
            }

            else
            {
                //result = MessageBox.Show("Are you sure you want to exit this application?", "Question", 
                //                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //if (result != DialogResult.Yes)
                //    e.Cancel = true;    
                
                
            }
        }



        public void formatAlignMiddles_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_MIDDLE);
            UpdateGUI();
        }

        private void tsbAlignLeft_Click(object sender, EventArgs e)
        {
            formatAlignLeft_Click(sender, e);
        }

        private void tsbAlignCenter_Click(object sender, EventArgs e)
        {
            formatAlignCenters_Click(sender, e);
        }

        public void formatAlignCenters_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            editorMain.OnEditAlign(DiagramEntityContainer.EntitiesAlign.ALIGN_CENTER);
            UpdateGUI();
        }

        private void tsbAlignRight_Click(object sender, EventArgs e)
        {
            formatAlignRight_Click(sender, e);
        }

        private void tsbAlignTop_Click(object sender, EventArgs e)
        {
            formatAlignTop_Click(sender, e);
        }

        private void tsbAlignMiddle_Click(object sender, EventArgs e)
        {
            formatAlignMiddles_Click(sender, e);
        }

        private void tsbAlignBottom_Click(object sender, EventArgs e)
        {
            formatAlignBottom_Click(sender, e);
        }

        private void tsbButton_Click(object sender, EventArgs e)
        {
            btnKeyArea_Click(sender, e);
        }

        private void editorMain_OnChanged(DiagramEditor sender)
        {
            UpdateGUI(); 
            update_showing_keycodes();
        }

        private void update_showing_keycodes()
        {

           
            if (editorMain.GetSelectCount() != 1)
            {
                 this.PauseTextBoxChangedEvent = true;
                lblKeyCode0.Text = string.Empty;
                lblKeyCode1.Text = string.Empty;
                lblKeyCode2.Text = string.Empty;
                lblKeyCode3.Text = string.Empty;
                 this.PauseTextBoxChangedEvent = false;
            }
            else
            {
       
                //show_keycodes(editorMain.GetSelectedObject());
                show_keycodes(GetCurrentEntity());
                
            }

           
        }

        private DiagramEntity GetCurrentEntity()
        {
            DiagramEntity entity = editorMain.GetSelectedObject();

            if (entity.TypeName == DiagramKeyMatrix.KEY_MATRIX)
            {
                entity = ((DiagramKeyMatrix)entity).get_selected_entity();
            
            }
            return entity;
        }

        private string result_message(KB9API.KB9API_ERROR result)
        {
            string strmsg = "";

            switch (result)
            {
                case KB9API.KB9API_ERROR.FUNC_SUCCESSFUL:
                    {
                        //if (succeedMsg != string.Empty)
                        //    strmsg = succeedMsg;
                        //else
                            strmsg = "Operation succeeded.";
                    }
                    break;

                case KB9API.KB9API_ERROR.FUNC_OPENPORT_FAIL:
                    {
                        strmsg = " Can't connect to KB9000.";// Can't connect to port of KB9000 USB/PS2 driver";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_GET_ID_FAIL:
                    {
                        strmsg = "KB9000 ID read fail.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_WRONG_ID_FAIL:
                    {
                        strmsg = "KB9000 ID incorrect.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_DEVICEIO_FAIL:
                    {
                        strmsg = "Device communcation fail.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_PACKET_FAIL:
                    {
                        strmsg = "Data packet error.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_LRC_CHECK_FAIL:
                    {
                        strmsg = "Data packet LRC.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_NOMEMORY_ERROR:
                    {
                        strmsg = "Application memory fail.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_THREAD_ERROR:
                    {
                        strmsg = "Application thread creation fail.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_WRONG_INTERFACE:
                    {
                        strmsg = "Interface identification.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_ACTIOJN_NOFINISH:
                    {
                        strmsg = "Action aborted.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_NOACTION_EXECUTE:
                    {
                        strmsg = "No action executed.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_NODEVICE_DRIVER:
                    {
                        strmsg = "No device driver detected";//"No device driver detected.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_STOP_FAIL:
                    {
                        strmsg = "Can't stop activity.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_BUFFER_SMALL:
                    {
                        strmsg = "Buffer too small for all data.";
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_UNKOWN_ERROR:
                    {
                        strmsg = "Operation Cancelled."; //"Unknown error.";//
                    }
                    break;
                case KB9API.KB9API_ERROR.FUNC_UNHANDLED_EXCEPTION:
                    {
                        strmsg = "Unhandled exception.";
                    }
                    break;

  //////////////////////////////////////////////////////////////////
                case KB9API.KB9API_ERROR.LINE_0LENGTH_WRONG://		=		-100,// zero length of key codes in one line
                    {
                        strmsg = "Unexpected blank line.";
                    }
                    break;
                case KB9API.KB9API_ERROR.DATA_CODE_WRONG://		=		-101,// 
                    {
                        strmsg = "Data error.";
                    }
                    break;
                case KB9API.KB9API_ERROR.LINE_LENGTH_WRONG://		=		-102,//
                    {
                        strmsg = "Too many key codes in one line.";
                    }
                    break;
                case KB9API.KB9API_ERROR.TOTAL_KEYNUMBER_WRONG://	=		-103,// 
                    {
                        strmsg = "More than 64 key defined.";
                    }
                    break;

                case KB9API.KB9API_ERROR.TOTAL_PROPERTYLINE_WRONG://=		-104,// 
                    {
                        strmsg = "Error in property line.";
                    }
                    break;
              case KB9API.KB9API_ERROR.TOTAL_KEYSIZELINE_WRONG://=		-105,// 
                  {
                      strmsg = "Error in key size.";
                  }
                  break;
              case KB9API.KB9API_ERROR.TOTAL_BEEPPROP_WRONG://	=		-106,// 
                  {
                      strmsg = "Error in key beep property.";
                  }
                  break;
              case KB9API.KB9API_ERROR.KEY_CONTENT_WRONG://		=		-107,// 
                  {
                      strmsg = "Error in key content.";
                  }
                  break;
              case KB9API.KB9API_ERROR.LINE_BKTPAIR_WRONG://		=		-108,// 
                  {
                      strmsg = "Error in code bracket pairs.";
                  }
                  break;
              case KB9API.KB9API_ERROR.LINE_COMBINA_WRONG://		=		-109,// 
                  {
                      strmsg = "Error in combination key.";
                  }
                  break;
              case KB9API.KB9API_ERROR.TOTAL_KEYOVERLAP_WRONG://	=		-110,// 
                  {
                      strmsg = "Overlapped key layout.";
                  }
                  break;
              case  KB9API.KB9API_ERROR.TOTAL_KEYBODATA_WRONG://			-111// No data in the device
                {
                    strmsg = "No data in KB9000.";
                }
                break;
              case KB9API.KB9API_ERROR.FUNC_LOGFILE_FAIL:
                {
                    strmsg = "DLL log file error";
                }
                break;
            }
            int n = (int)result;

            if (n != 0)
                strmsg = ("Error #" +Math.Abs( n).ToString() + ": " + strmsg);
 
            return strmsg;
        }
        /// <summary>
        /// 
        /// show the operation result in a messagebox
        /// </summary>
        /// <param name="result"></param>
        private void kb9000_operation_result(KB9API.KB9API_ERROR result, string succeedMsg, API_FUNCTION api)
        {
            string strmsg = "";
            strmsg = result_message(result);
            if (result == KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
            {
                 if (succeedMsg != string.Empty)
                       strmsg = succeedMsg;
            }

            string title = "Message";
            if (api == API_FUNCTION.Detect)
                title = "KB9000 Detection";
            else if (api == API_FUNCTION.DLLVersion)
                title = "KB9000 Device Information";


            if (strmsg != string.Empty)
                MessageBox.Show(strmsg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        ///     -1: unknow
        ///     
        /// </returns>
        private int DetectKB9000FreeSpace()
        {
            //KB9API.KB9_PORT port = KB9API.KB9_PORT.Unknown;
            //KB9API.KB9API_ERROR result = KB9API.AutoDetectKB9(ref port);
            

            
            //if (result == KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
            //{

                string strData = editorMain.CreateCVS();
                int nBytes = KB9API.GetLeftSpaceBytesKB9( strData);
                return nBytes;
            //}
            //else
            //    return -1;
        }

        private void kb9000Detect_Click(object sender, EventArgs e)
        {
            try
            {

                kb9000Detect2_Click(sender, e);
            }
            catch (Exception err)
            {
                string strmsg = "Error in KB9000 detection.\n" + err.Message;

                MessageBox.Show(strmsg, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void kb9000Detect2_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            //frmProgress frm = new frmProgress();
            //frm.AutoDetectKB9();
            KB9API.KB9_PORT port = KB9API.KB9_PORT.Unknown;
            KB9API.KB9API_ERROR result = KB9API.AutoDetectKB9(ref port);
            string dllver = "";
            //result = KB9API.KB9API_ERROR.FUNC_SUCCESSFUL; //debug
            //KB9API.KB9API_ERROR result = frm.GetAsyncOperationResult();
            if (result == KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
            {
                string firmwarever = "";
                 KB9API.KB9API_ERROR res = KB9API.GetDllVersion(port, ref dllver, ref firmwarever);
                // res = KB9API.KB9API_ERROR.FUNC_STOP_FAIL; //debug;
                 if (res != KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
                 {
                     kb9000_operation_result(res, "", API_FUNCTION.DLLVersion);
                     return;
                 }
                string s = "KB9000 API version: " + dllver;
                s += "\n";
                s += "Firmware version: " + firmwarever;
                s += "\n";
                s += ("KB9000 was found at " + port.ToString());

                //string strData = editorMain.CreateCVS();
                //int nBytes = KB9API.GetLeftSpaceBytesKB9(port, strData);
                //s += "\n";
                //s += ("KB9000 free memory: " + nBytes.ToString() + " bytes");

                kb9000_operation_result(result,s, API_FUNCTION.Detect);// "KB9000 was found at " + port.ToString());
            }
            else
                kb9000_operation_result(result, "", API_FUNCTION.Detect);
            
        }

        private void kb9000Test_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            frmTestKB frm = new frmTestKB();
            frm.ShowDialog();
        }

        private void kb9000Write_Click(object sender, EventArgs e)
        {
            try
            {

                kb9000Write2_Click(sender, e);
            }
            catch (Exception err)
            {
                string strmsg = "Error in write process.";

                MessageBox.Show(strmsg, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void kb9000Write2_Click(object sender, EventArgs e)
        {
            if (!editorMain.check_max_entities(false))
                return;

            close_onscreen_kbd();
            if (editorMain.ErrorOverlapped())
            {
                string msg = "The buttons is overlapped, please correct them!";
                MessageBox.Show("The buttons is overlapped, please correct them!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                KB9Logger.Log("Write KB Error: "+ msg);
                return;
            }
            DiagramKey errorKey = editorMain.ErrorKeyUp();
            if (errorKey != null)
            {
                string strMsg = string.Format("Button '{0}' has error. Its contents miss some 'KeyUp' code", errorKey.Title);
                MessageBox.Show(strMsg,"Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                KB9Logger.Log("Write KB Error: " + strMsg);
                return;
            }
            string s = editorMain.CreateCVS();
            System.Diagnostics.Debug.Print(s);
            //if (frmDataWrite.show_data(s) == System.Windows.Forms.DialogResult.Cancel)
            if (frmConfirmWrite.confirmWrite() == System.Windows.Forms.DialogResult.Cancel)
                return;

            //MessageBox.Show(s);
            frmProgress frm = new frmProgress();
            KB9API.KB9API_ERROR result = frm.write_kb9000(s);
            //KB9API.KB9API_ERROR result = KB9API.KB9API_ERROR.FUNC_SUCCESSFUL;

            string strresult = result_message(result);

            string strmsg = "Write data: ";
            uint nwriten = KB9API.GetWritingResult();
            if (nwriten == 0)
            {
                if (result == KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
                    strresult = "Operation failed";
            }
                
            strmsg += nwriten.ToString();
            strmsg += "\n";
            strmsg += strresult;

            string title = "Write Process";


            MessageBox.Show(strmsg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
          
           
           // kb9000_operation_result(result,"");

            log_write_read("Write", result, s);
            /*
           // KB9Logger.Log("Write KB Return:  Code: " + ((int)result).ToString() + "\r\n\t\t Message" + result_message(result));
            string strlog = "Write KB Return:";
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog += "Result code:";
            strlog += ((int)result).ToString();
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog += "Result Message:" + result_message(result);
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;

            strlog += "Write data:";
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog += s;
            strlog += KB9Logger.LOG_LF;
           // strlog += KB9Logger.LOG_LF;
            KB9Logger.Log(strlog);

            */
        }

        private void kb9000Read_Click(object sender, EventArgs e)
        {
            try
            {

                kb9000Read2_Click(sender, e);
            }
            catch (Exception err)
            {
                string strmsg = "Error in read process.";

                MessageBox.Show(strmsg, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void kb9000Read2_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();

            if (!confirm_lost_data(true))
                return;

            //string s = "0,nt,50,100,50,9600,n,8,1\r\n"+
                        //"0,Button,30,20,242,84,[NumPad1][NumPad2][NumPad3][NumPad4][NumPad5][NumPad6][NumPad7][NumPad8][NumPad9]\r\n"+
                        //"1,Button,526,20,157,80,1234567890\r\n"+
                        //"2,Button,277,20,242,84,";
            //editRemoveAll_Click(null, null);
            editorMain.DeleteAll();
            
            editorMain.Focus();

            //string s = "[0,0,1,1][0][0][F2]m[Enter][F3]\r\n" +
            //            "[0,0,1,1][0][0]This is Marco 2.\r\n" +
            //            "[0,0,1,1][0][0]\r\n" +
            //            "[0,0,1,1][0][0]\r\n" +
            //            "[0,0,1,1][0][0]\r\n" +
            //            "[0,0,60,60][100][120][F12][Pause0.5]0000[Pause2][Enter][Enter][Home]\r\n" +
                        //"[0,0,60,60][100][120][PauseBreak]\r\n" +
                                                         /*
                                                                    "[65,0,60,60][100][120][Macro1]H\r\n" +
                                                                    "[130,0,60,60][100][120]S\r\n" +
                                                                    "[0,68,60,60][100][120]BI\r\n" +
                                                                    "[65,68,60,60][100][120][Macro1]R\r\n" +
                                                                    "[130,68,60,60][100][120][F1]\r\n" +
                                                                    "[200,0,38,24][150][60][7]\r\n" +
             
                                                                    "[200,0,38,24][150][60][B]\r\n" +
             
                                                                    "[242,0,38,24][150][60][Pause4]\r\n" +
                                                                    "[284,0,38,24][150][60][Pause14]\r\n" +
                                                                    "[200,28,38,24][150][60]4\r\n" +
                                                                    "[242,28,38,24][150][60]5\r\n" +
                                                                    "[284,28,38,24][150][60]6\r\n" +
                                                                    "[200,56,38,24][150][60]1\r\n" +
                                                                    "[242,56,38,24][150][60]2\r\n" +
                                                                    "[284,56,38,24][150][60]3\r\n" +
                                                                    "[200,82,38,24][150][60]0\r\n" +
                                                                    "[242,82,80,24][150][60][Enter]\r\n" +
                                                                    "[200,110,144,18][110][110][SlideL][ArrowLeft][SlideR][Home][SlideLH][ArrowLeft][SlideRH][Ctrl][End][#Ctrl]\r\n" +
                                                                    //"[200,110,144,18][110][110][SlideL][SlideR][Home][SlideLH][SlideRH]\r\n" +
                                                                    "[326,0,18,106][110][110][SlideU][ArrowUp][SlideD][PageUp][SlideUH][ArrowDown][SlideDH][PageDown]\r\n" +
            //                                               */
            //  "[2][2][50][50]\r\n";
//            string s = "[0,0,1,1][0][0]\r\n"
//+ "[0,0,1,1][0][0]\r\n"
//+ "[0,0,1,1][0][0]\r\n"
//+ "[0,0,1,1][0][0]\r\n"
//+ "[0,0,1,1][0][0]\r\n"
//+ "[95,38,117,54][5][3][Pause03]3452345[Pause14]5645\r\n"
//+ "[2][2][5][5]\r\n";

//            editorMain.LoadCSV(s);
//            //MessageBox.Show("OK", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            editorMain.Focus();
//            return;
            
            frmProgress frm = new frmProgress();
            string strData = "";
            KB9API.KB9API_ERROR result = frm.read_kb9000(ref strData);

            //result = KB9API.KB9API_ERROR.DATA_CODE_WRONG;//test

            if (result == KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
            {
                //MessageBox.Show(strData);
                editorMain.LoadCSV(strData);
                
            }
            editorMain.ClearRedo();
            editorMain.ClearUndo();
            string strresult = result_message(result);

            string strmsg = "Read data: ";
            int nread = strData.Length;//
            if (nread == 0)
            {
                if (result == KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
                    strresult = "Operation failed";
            }

            strmsg += nread.ToString();
            strmsg += "\n";
            strmsg += strresult;

            string title = "Read Process";

            MessageBox.Show(strmsg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

            
          //  kb9000_operation_result(result, "");

            //KB9Logger.Log("Read KB Return: " + ((int)result).ToString());
           // KB9Logger.Log("Read KB Return:  Code: " + ((int)result).ToString() + "\r\n\t\t Message" + result_message(result));
            log_write_read("Read", result, strData);
            editorMain.Focus();

            //string strlog = "Read KB Return:";
            //strlog += KB9Logger.LOG_LF;
            //strlog += KB9Logger.LOG_TAB;
            //strlog += "Result code:";
            //strlog += ((int)result).ToString() ;
            //strlog += KB9Logger.LOG_LF;
            //strlog += KB9Logger.LOG_TAB;
            //strlog +=  "Result Message:" + result_message(result);
            //strlog += KB9Logger.LOG_LF;
            //strlog += KB9Logger.LOG_TAB;

            //strlog += "Read data:";
            //strlog += KB9Logger.LOG_LF;
            //strlog += KB9Logger.LOG_TAB;
            //strlog += strData;
            //strlog += KB9Logger.LOG_LF;
            ////strlog += KB9Logger.LOG_LF;
            //KB9Logger.Log(strlog);
            //List<string> ar = new List<string>();
            //int n = Util.string2lines(s, ar);
            //for (int i = 0; i < ar.Count; i++)
            //    System.Diagnostics.Debug.Print(ar[i]);
             
        }

        private void log_write_read(string strReadWite ,KB9API.KB9API_ERROR result, string data )
        {
            string strlog = strReadWite;
            strlog += " KB9000 Return:";
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog += "Result code:";
            strlog += ((int)result).ToString() ;
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog +=  "Result Message:" + result_message(result);
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;

            strlog += (strReadWite + " Data:");
            strlog += KB9Logger.LOG_LF;
            strlog += KB9Logger.LOG_TAB;
            strlog += data;
            strlog += KB9Logger.LOG_LF;
            //strlog += KB9Logger.LOG_LF;
            KB9Logger.Log(strlog);
        }

        private void editShowTooltips_Click(object sender, EventArgs e)
        {
           // editorMain.ShowTips = editShowTooltips.Checked;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Settings.Default.ShowTooltips = editShowTooltips.Checked  ;
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WinWidth = this.Bounds.Size.Width;
                Settings.Default.WinHeight = this.Bounds.Size.Height;
            }
            else
            {
                Settings.Default.WinWidth = 0;
                Settings.Default.WinHeight = 0;
            }
            Settings.Default.WinState =(int) this.WindowState;
            Settings.Default.Zoom = (int)(editorMain.Zoom * 100);
            Settings.Default.EnableLog = helpLog.Checked;
            Settings.Default.Save();

            _mruMenu.SaveToRegistry();

            KB9Logger.Log("KB9Utility closed");

            if (Settings.Default.EnableLog)
            {
                KB9Logger.Instance().Stop();
            }

        }

       

        private void btnSlideV_Click(object sender, EventArgs e)
        {
            DiagramKeySlideV obj = new DiagramKeySlideV();
            obj.Title = editorMain.CreateUniqueName(obj.TypeName);
            editorMain.DrawingObject = obj;
        }

        private void btnSlideH_Click(object sender, EventArgs e)
        {
            DiagramKeySlideH obj = new DiagramKeySlideH();
            obj.Title = editorMain.CreateUniqueName(obj.TypeName);
            editorMain.DrawingObject = obj;
        }

        private void lblKeyCode_Click(object sender, EventArgs e)
        {
            return;
            //if (editorMain.GetSelectCount() != 1)
            //    return;

            //Label lbl = (Label)sender;
            //Color oldbg = lbl.BackColor;
            //Color oldfg = lbl.ForeColor;
            //lbl.BackColor = SystemColors.Highlight;// Color.CadetBlue;
            //lbl.ForeColor = SystemColors.HighlightText;

            //DiagramKey key = (DiagramKey)(editorMain.GetSelectedObject());

            //string strInit = string.Empty;

            //if (key.TypeName == DiagramKey.KEY_BUTTON)
            //{
            //    strInit = ((DiagramKeyArea)key).KeyCode.ToString();
            //}
            //else if (key.TypeName == DiagramKey.KEY_SLIDEV)
            //{
            //    if (lbl.Name == "lblKeyCode0")
            //        strInit = ((DiagramKeySlideV)key).SlideUp.ToString();
            //    else if (lbl.Name == "lblKeyCode1")
            //        strInit = ((DiagramKeySlideV)key).SlideUpHold.ToString();
            //    else if (lbl.Name == "lblKeyCode2")
            //        strInit = ((DiagramKeySlideV)key).SlideDown.ToString();
            //    else if (lbl.Name == "lblKeyCode3")
            //        strInit = ((DiagramKeySlideV)key).SlideDownHold.ToString();
            //}
            //else if (key.TypeName == DiagramKey.KEY_SLIDEH)
            //{
            //    if (lbl.Name == "lblKeyCode0")
            //        strInit = ((DiagramKeySlideH)key).SlideLeft.ToString();
            //    else if (lbl.Name == "lblKeyCode1")
            //        strInit = ((DiagramKeySlideH)key).SlideLeftHold.ToString();
            //    else if (lbl.Name == "lblKeyCode2")
            //        strInit = ((DiagramKeySlideH)key).SlideRight.ToString();
            //    else if (lbl.Name == "lblKeyCode3")
            //        strInit = ((DiagramKeySlideH)key).SlideRightHold.ToString();
            //}
            ////else

            ////    return;

            //frmKeyCode frm = new frmKeyCode();
            //string inputed = frm.InputKeyCode(strInit);


            //if (key.TypeName == DiagramKey.KEY_BUTTON)
            //{
            //    ((DiagramKeyArea)key).KeyCode.SetKeyCodeString(inputed);

            //}
            //else if (key.TypeName == DiagramKey.KEY_SLIDEV)
            //{
            //    if (lbl.Name == "lblKeyCode0")
            //        ((DiagramKeySlideV)key).SlideUp.SetKeyCodeString(inputed);
            //    else if (lbl.Name == "lblKeyCode1")
            //        ((DiagramKeySlideV)key).SlideUpHold.SetKeyCodeString(inputed);

            //    else if (lbl.Name == "lblKeyCode2")
            //        ((DiagramKeySlideV)key).SlideDown.SetKeyCodeString(inputed);

            //    else if (lbl.Name == "lblKeyCode3")
            //        ((DiagramKeySlideV)key).SlideDownHold.SetKeyCodeString(inputed);
            //}
            //else if (key.TypeName == DiagramKey.KEY_SLIDEH)
            //{
            //    if (lbl.Name == "lblKeyCode0")
            //        ((DiagramKeySlideH)key).SlideLeft.SetKeyCodeString(inputed);
            //    else if (lbl.Name == "lblKeyCode1")
            //        ((DiagramKeySlideH)key).SlideLeftHold.SetKeyCodeString(inputed);
            //    else if (lbl.Name == "lblKeyCode2")
            //        ((DiagramKeySlideH)key).SlideRight.SetKeyCodeString(inputed);
            //    else if (lbl.Name == "lblKeyCode3")
            //        ((DiagramKeySlideH)key).SlideRightHold.SetKeyCodeString(inputed);
            //}
            ////else
            ////    return;

            //editorMain_OnShowDiagramEntityProperties(editorMain, key);
            //lbl.BackColor = oldbg;
            //lbl.ForeColor = oldfg;

        }

        //private void lblKeyCode1_Click(object sender, EventArgs e)
        //{
        //    if (editorMain.GetSelectCount() != 1)
        //        return;

        //    Label lbl = (Label)sender;

            
        //    DiagramKey key = (DiagramKey)(editorMain.GetSelectedObject());

        //    string strInit = string.Empty;

        //    if (key.TypeName == "BUTTON")
        //    {
        //        strInit = ((DiagramKeyArea)key).KeyCode.ToString();
        //    }
        //    else if (key.TypeName == "SLIDEV")
        //    {
        //        if (lbl.Name == "lblKeyCode")
        //            strInit = ((DiagramKeySlideV)key).SlideUp.ToString();
        //        else if (lbl.Name == "lblKeyCode1")
        //            strInit = ((DiagramKeySlideV)key).SlideUpHold.ToString();
        //        else if (lbl.Name == "lblKeyCode2")
        //            strInit = ((DiagramKeySlideV)key).SlideDown.ToString();
        //        else if (lbl.Name == "lblKeyCode3")
        //            strInit = ((DiagramKeySlideV)key).SlideDownHold.ToString();
        //    }
        //    else if (key.TypeName == "SLIDEH")
        //    {
        //        if (lbl.Name == "lblKeyCode")
        //            strInit = ((DiagramKeySlideH)key).SlideLeft.ToString();
        //        else if (lbl.Name == "lblKeyCode1")
        //            strInit = ((DiagramKeySlideH)key).SlideLeftHold.ToString();
        //        else if (lbl.Name == "lblKeyCode2")
        //            strInit = ((DiagramKeySlideH)key).SlideRight.ToString();
        //        else if (lbl.Name == "lblKeyCode3")
        //            strInit = ((DiagramKeySlideH)key).SlideRightHold.ToString();
        //    }
        //    else
        //        return;

        //    frmKeyCode frm = new frmKeyCode();
        //    string inputed = frm.InputKeyCode(strInit);


        //    if (key.TypeName == "BUTTON")
        //    {
        //        ((DiagramKeyArea)key).KeyCode.SetKeyCodeString( inputed);

        //    }
        //    else if (key.TypeName == "SLIDEV")
        //    {
        //        if (lbl.Name == "lblKeyCode")
        //            ((DiagramKeySlideV)key).SlideUp.SetKeyCodeString(inputed);       
        //        else if (lbl.Name == "lblKeyCode1")
        //            ((DiagramKeySlideV)key).SlideUpHold.SetKeyCodeString(inputed);
                    
        //        else if (lbl.Name == "lblKeyCode2")
        //            ((DiagramKeySlideV)key).SlideDown.SetKeyCodeString(inputed);
                    
        //        else if (lbl.Name == "lblKeyCode3")
        //            ((DiagramKeySlideV)key).SlideDownHold.SetKeyCodeString(inputed);
        //    }
        //    else if (key.TypeName == "SLIDEH")
        //    {
        //        if (lbl.Name == "lblKeyCode")
        //            ((DiagramKeySlideH)key).SlideLeft.SetKeyCodeString(inputed);
        //        else if (lbl.Name == "lblKeyCode1")
        //            ((DiagramKeySlideH)key).SlideLeftHold.SetKeyCodeString(inputed);
        //        else if (lbl.Name == "lblKeyCode2")
        //            ((DiagramKeySlideH)key).SlideRight.SetKeyCodeString(inputed);
        //        else if (lbl.Name == "lblKeyCode3")
        //            ((DiagramKeySlideH)key).SlideRightHold.SetKeyCodeString(inputed);
        //    }
        //    else
        //        return;

        //    editorMain_OnShowDiagramEntityProperties(editorMain, key);
        //}

        //private void lblKeyCode2_Click(object sender, EventArgs e)
        //{

        //}

        //private void lblKeyCode3_Click(object sender, EventArgs e)
        //{

        //}

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            //editorMain.Zoom -= editorMain.ZoomStep;
            int n =  tkbZoom.Value - (int)(editorMain.ZoomStep * 100);// (int)(editorMain.Zoom * 100);
            if (n <= tkbZoom.Minimum)
                n = tkbZoom.Minimum;
            tkbZoom.Value = n;// -= (int)(editorMain.ZoomStep*100);// (int)(editorMain.Zoom * 100);

        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            int n = tkbZoom.Value + (int)(editorMain.ZoomStep * 100);// (int)(editorMain.Zoom * 100);
            if (n >= tkbZoom.Maximum)
                n = tkbZoom.Maximum;
            tkbZoom.Value = n;// -= (int)(editorMain.ZoomStep*100);// (int)(editorMain.Zoom * 100);

        
        }

        private void tkbZoom_ValueChanged(object sender, EventArgs e)
        {
            
            editorMain.Zoom = (((double)tkbZoom.Value) / 100);
            int n = (int)decimal.Round(((decimal)editorMain.Zoom /*/ (decimal)2*/) * (decimal)200);
            //int n = tkbZoom.Value;
            //n *= 2;
            lblPercent.Text = n.ToString() + "%";


            btnZoomIn.Enabled = (tkbZoom.Value > tkbZoom.Minimum);
            btnZoomOut.Enabled = (tkbZoom.Value < tkbZoom.Maximum);
        }

        private void editorMain_OnEditEntityKeyContent(DiagramEditor sender, DiagramEntity entity)
        {
            //string strType = entity.TypeName;

            //switch (strType)
            //{
            //    case DiagramKey.KEY_BUTTON:
            //        {
            //            string strInit = ((DiagramKeyArea)entity).KeyCode.ToString();

            //            frmKeyCode frm = new frmKeyCode();
            //            string inputed = frm.InputKeyCode(strInit);
            //            if (inputed != string.Empty)
            //                ((DiagramKeyArea)entity).KeyCode.SetKeyCodeString(inputed);
            //        }
            //        break;
            //    case DiagramKey.KEY_SLIDEH:
            //    case DiagramKey.KEY_SLIDEV:
            //        {
            //            frmSlideKeyContent frm = new frmSlideKeyContent();
            //            frm.edit_slide_button((DiagramKey)entity);
            //        }
            //        break;
                
            //}
            System.Diagnostics.Debug.Print("editorMain_OnEditEntityKeyContent");

            editorMain_OnShowDiagramEntityProperties(sender, entity);
            lblKeyCode0.Focus();
            lblKeyCode0.moveCaretEnd();
           
        }

        private void editMacro_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            frmMacro frm = new frmMacro();
            frm.ForEditMacro = true;
            frm.ShowDialog();
            this.updateStatusBar();
        }

        private void tsbMacro_Click(object sender, EventArgs e)
        {
            editMacro_Click(null, null);
        }

        private void txtEntityTitle_TextChanged(object sender, EventArgs e)
        {
            //if (this.PauseTextBoxChangedEvent)
            //    return;
            //DiagramEntity entity = editorMain.GetSelectedObject();
            //if (entity == null)
            //    return;
            //entity.PauseEvent = true;
            //entity.Title = txtEntityTitle.Text;
            //entity.PauseEvent = false;
            //editorMain.Refresh();
        }

        private void KB9TextBox_TextChanged(KB9TextBox t)
        {
            if (this.PauseTextBoxChangedEvent)
                return;

            DiagramEntity entity = GetCurrentEntity();
            //DiagramEntity entity = editorMain.GetSelectedObject();
            if (entity == null)
                return;
            ////for matrix Key
            //if (entity.TypeName == DiagramKeyMatrix.KEY_MATRIX)
            //{
            //    entity = ((DiagramKeyMatrix)entity).get_selected_entity();
            //    if (entity == null)
            //        return;
            //}


            entity.PauseEvent = true;
            if (entity.TypeName == DiagramKey.KEY_BUTTON)
            {
                if (t == lblKeyCode0)
                    ((DiagramKeyArea)entity).KeyCode = new KeyEditingType(t.Text);

            }
            else if (entity.TypeName == DiagramKey.KEY_SLIDEH)
            {
                if (t == lblKeyCode0)
                    ((DiagramKeySlideH)entity).SlideLeft = new KeyEditingType(t.Text);
                else if (t == lblKeyCode1)
                    ((DiagramKeySlideH)entity).SlideLeftHold = new KeyEditingType(t.Text);
                else if (t == lblKeyCode2)
                    ((DiagramKeySlideH)entity).SlideRight = new KeyEditingType(t.Text);
                else if (t == lblKeyCode3)
                    ((DiagramKeySlideH)entity).SlideRightHold = new KeyEditingType(t.Text);

            }
            else if (entity.TypeName == DiagramKey.KEY_SLIDEV)
            {
                if (t == lblKeyCode0)
                    ((DiagramKeySlideV)entity).SlideUp = new KeyEditingType(t.Text);
                else if (t == lblKeyCode1)
                    ((DiagramKeySlideV)entity).SlideUpHold = new KeyEditingType(t.Text);
                else if (t == lblKeyCode2)
                    ((DiagramKeySlideV)entity).SlideDown = new KeyEditingType(t.Text);
                else if (t == lblKeyCode3)
                    ((DiagramKeySlideV)entity).SlideDownHold = new KeyEditingType(t.Text);
            }

            entity.PauseEvent = false;
            string s = KB9Validation.ValidateKeyTextBoxMsg(this, t); ;
            //tsMsg.ForeColor = Color.Red;
            //tsMsg0.Text = s;
            setStatusBarText(s);
            UpdateGUI();
        }

        private void lblKeyCode0_OnTextChanged(object sender)
        {
            KB9TextBox_TextChanged((KB9TextBox)sender);
            //if (this.PauseTextBoxChangedEvent)
            //    return;
            
            //DiagramEntity entity = editorMain.GetSelectedObject();
            //if (entity == null)
            //    return;

            //entity.PauseEvent = true;
            //if (entity.TypeName == DiagramKey.KEY_BUTTON)
            //{
            //    ((DiagramKeyArea)entity).KeyCode = new KeyEditingType(lblKeyCode0.Text);
            //}
            //else if (entity.TypeName == DiagramKey.KEY_SLIDEH)
            //{
            //    ((DiagramKeySlideH)entity).SlideLeft = new KeyEditingType(lblKeyCode0.Text);
            //}
            //else if (entity.TypeName == DiagramKey.KEY_SLIDEV)
            //{
            //    ((DiagramKeySlideV)entity).SlideUp = new KeyEditingType(lblKeyCode0.Text);
            //}
            //entity.PauseEvent = false;

            //tsMsg.Text = KB9Validation.ValidateKeyTextBoxMsg(this, lblKeyCode0);
            //UpdateGUI();
        }

        //private void lblKeyCode1_OnTextChanged(object sender)
        //{
        //    if (this.PauseTextBoxChangedEvent)
        //        return;
        //    DiagramEntity entity = editorMain.GetSelectedObject();
        //    if (entity == null)
        //        return;
        //    entity.PauseEvent = true;
        //    if (entity.TypeName == DiagramKey.KEY_BUTTON)
        //    {
                
        //    }
        //    else if (entity.TypeName == DiagramKey.KEY_SLIDEH)
        //    {
        //        ((DiagramKeySlideH)entity).SlideLeftHold = new KeyEditingType(lblKeyCode1.Text);
        //    }
        //    else if (entity.TypeName == DiagramKey.KEY_SLIDEV)
        //    {
        //        ((DiagramKeySlideV)entity).SlideUpHold = new KeyEditingType(lblKeyCode1.Text);
        //    }
        //    entity.PauseEvent = false;
        //    tsMsg.Text = KB9Validation.ValidateKeyTextBoxMsg(this, lblKeyCode0);
        //    UpdateGUI();
        //}

        //private void lblKeyCode2_OnTextChanged(object sender)
        //{
        //    if (this.PauseTextBoxChangedEvent)
        //        return;
        //    DiagramEntity entity = editorMain.GetSelectedObject();
        //    if (entity == null)
        //        return;
        //    entity.PauseEvent = true;
        //    if (entity.TypeName == DiagramKey.KEY_BUTTON)
        //    {
        //        //((DiagramKeyArea)entity).KeyCode.SetKeyCodeString(lblKeyCode0.Text);
        //    }
        //    else if (entity.TypeName == DiagramKey.KEY_SLIDEH)
        //    {
        //        ((DiagramKeySlideH)entity).SlideRight = new KeyEditingType(lblKeyCode2.Text);
        //    }
        //    else if (entity.TypeName == DiagramKey.KEY_SLIDEV)
        //    {
        //        ((DiagramKeySlideV)entity).SlideDown = new KeyEditingType(lblKeyCode2.Text);
        //    }
        //    entity.PauseEvent = false;
        //    tsMsg.Text = KB9Validation.ValidateKeyTextBoxMsg(this, lblKeyCode0);
        //    UpdateGUI();
        //}

        //private void lblKeyCode3_OnTextChanged(object sender)
        //{
        //    if (this.PauseTextBoxChangedEvent)
        //        return;
        //    DiagramEntity entity = editorMain.GetSelectedObject();
        //    if (entity == null)
        //        return;

        //    entity.PauseEvent = true;
        //    if (entity.TypeName == DiagramKey.KEY_BUTTON)
        //    {
        //        //((DiagramKeyArea)entity).KeyCode.SetKeyCodeString(lblKeyCode0.Text);
        //    }
        //    else if (entity.TypeName == DiagramKey.KEY_SLIDEH)
        //    {
        //        ((DiagramKeySlideH)entity).SlideRightHold = new KeyEditingType(lblKeyCode3.Text);
        //    }
        //    else if (entity.TypeName == DiagramKey.KEY_SLIDEV)
        //    {
        //        ((DiagramKeySlideV)entity).SlideDownHold = new KeyEditingType(lblKeyCode3.Text);
        //    }
        //    entity.PauseEvent = false;
        //    tsMsg.Text = KB9Validation.ValidateKeyTextBoxMsg(this, lblKeyCode0);
        //    UpdateGUI();
        //}


        private KB9TextBox GetEditingTextBox()
        {
            if (lblKeyCode0.Focused)
                return lblKeyCode0;
            if (lblKeyCode1.Focused)
                return lblKeyCode1;
            if (lblKeyCode2.Focused)
                return lblKeyCode2;
            if (lblKeyCode3.Focused)
                return lblKeyCode3;
            return null;
        }

        private void btnCodeClear_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetEditingTextBox();
            if (t == null) return;
            t.Clear();
            
        }

       

        private void lblKeyCode0_Enter(object sender, EventArgs e)
        {
            this.EditingState = EDIT_STATE.CodeEditor_Focused;
            enable_code_editing_buttons(true);
            frmOnScreenKbd.Instance().FocusedTextBox = GetEditingTextBox();
            KB9TextBox t = (KB9TextBox)sender;
            if (t == null)
                return;
            //tsMsg0.Text = KB9Validation.ValidateKeyTextBoxMsg(this, t);
            setStatusBarText(KB9Validation.ValidateKeyTextBoxMsg(this, t));
            t.ClearUndo();
            //20140707
          //  editorMain.ClearRedo();
            editorMain.DiagramEntityContainer.PreSnapshot(); //preparation it for undo
            editUndo.Enabled = false;
            btnUndo.Enabled = false;
            btnRedo.Enabled = false;
            toolTips.SetToolTip(btnCodeMacro, "Insert Macro");
            toolTips.SetToolTipWhenDisabled(btnCodeMacro, "Insert Macro");
        }

        private int GetLastFocusedTextBoxIndex(object sender)
        {
            if (!(sender is KB9TextBox))
                return 0;
            KB9TextBox t = (KB9TextBox)sender;
            if (t.Name.Equals(this.lblKeyCode0.Name))
                return 0;
            if (t.Name.Equals(this.lblKeyCode1.Name))
                return 1;
            if (t.Name.Equals(this.lblKeyCode2.Name))
                return 2;
            if (t.Name.Equals(this.lblKeyCode3.Name))
                return 3;

            return 0;

        }
        public int GetFocusedTextBoxIndex()
        {
            if (tabCodes.SelectedTab.Name.Equals(tpCode0.Name))
                return 0;
            if (tabCodes.SelectedTab.Name.Equals(tpCode1.Name))
                return 1;
            if (tabCodes.SelectedTab.Name.Equals(tpCode2.Name))
                return 2;
            if (tabCodes.SelectedTab.Name.Equals(tpCode3.Name))
                return 3;
            
            return 0;
        }
        private UndoItem m_undoItemBeforeLeaveTextbox = null; //if validing false,use it to remove new undo items
        private bool m_bJustUndoFocus = false; //if validate error, we will just record last focus undo step. The original blank is recorded before
        private void lblKeyCode0_Leave(object sender, EventArgs e)
        {
            toolTips.SetToolTip(btnCodeMacro, "Edit Macro");
            toolTips.SetToolTipWhenDisabled(btnCodeMacro, "Edit Macro");

            
            if (m_bDisableTextBoxFocusLeaveEvent)
            {
            //    m_bDisableTextBoxFocusLeaveEvent = false;
                return;
            }

            //this.EditingState = EDIT_STATE.Unknown;
            enable_code_editing_buttons(false);
            close_onscreen_kbd();
            if (((KB9TextBox)sender).Modified)
            {
                int nTextBox = GetLastFocusedTextBoxIndex(sender);
                if (!m_bJustUndoFocus)
                {
                    editorMain.DiagramEntityContainer.SaveSnapshotAfterLostFocus(nTextBox);
                    
                }
                m_bJustUndoFocus = false;
                m_undoItemBeforeLeaveTextbox = editorMain.getLastUndo();
                //append a focus me snapshot, 20141127
                //if (this.editorMain.GetSelectedObject() == tabCodes.Tag)
                //    System.Diagnostics.Debug.Print(">>>>>>>>>>>it is not same");
                editorMain.DiagramEntityContainer.SnapshotAppendFocusTrace((DiagramEntity)tabCodes.Tag, nTextBox);
                System.Diagnostics.Debug.Print("Undo = " + editorMain.DiagramEntityContainer.UndoStack.Count.ToString());
                editorMain.ClearRedo();
            }
            //tsMsg.Text = "";
            //KB9TextBox t = (KB9TextBox)sender;
            //if (t == null)
            //    return;
            //if (KB9Validation.ValidateKeyTextBox2(this, t) != 0)
            //    editorMain.ResetMouseEvent();
            
        }

        private void close_onscreen_kbd()
        {
            frmOnScreenKbd.Instance().FocusedTextBox = null;
            frmOnScreenKbd.Instance().Hide();
        }

        private void btnCodeUndo_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetEditingTextBox();
            if (t == null) return;
            t.Undo();
        }

        static private void hide_onscreen_kbd()
        {
            frmOnScreenKbd frm = frmOnScreenKbd.Instance();
            frm.Hide();
        }

        private void btnCodePause_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            KB9TextBox t = GetEditingTextBox();
            if (t == null) return;

            KeyContent_InputPause(t);

        }

        static public void KeyContent_InputPause(KB9TextBox t)
        {
            
            if (t == null) return;
            hide_onscreen_kbd();
            // KB9TextBox t = GetFocusedTextBox();
            frmDelay frm = new frmDelay();
            decimal d = frm.InputDelay();
            if (d <= 0)
                return;
            //d *= 10; //delay time is nx0.1 format, so need to x10 to make string
            //int n = (int)d;
            //string s = n.ToString();
            //s = "Pause" + s;
            string s = frmDelay.makePauseString(d);
            t.AddKeyCode(s, true);
        }

        private void btnCodeList_Click(object sender, EventArgs e)
        {
            KeyContent_InputSpecialKey(GetEditingTextBox());
        }
        //frmSpecialKey frmKeyList = null;//new frmSpecialKey();
        public void KeyContent_InputSpecialKey(KB9TextBox t)
        {
            if (t == null)
                return;
            frmOnScreenKbd.show_special_list_modeless(this, t);
            /*

            frmSpecialKey frmKeyList = frmSpecialKey.Instance();
            if (frmKeyList == null)
                frmKeyList = new frmSpecialKey();
            frmKeyList.TopMost = true;
            //set it initial position
            Point point = t.Location;
            Point ppt = t.PointToScreen(point);
            point = this.PointToClient(ppt);

            int dx = point.X + t.Width;// -frmKeyList.Width;
            int dy = point.Y;// +t.Height;// -frmKeyList.Height;
            Point pt = this.Location;
            pt.Offset(dx, dy);
            frmKeyList.Location =pt;
            frmKeyList.FocusedTextBox = t;
            if (frmKeyList.FocusedTextBox == null)
            {
                frmKeyList.Hide();
                //frmKeyList = null;
            }
            else
            {
                //if (!frmKeyList.Visible)
                frmKeyList.Visible = false;
                    frmKeyList.Show(this);
            }

            //string s = frm.InputSpecialKey();
            //if (s.Length > 0)
            //{
            //    t.AddKeyCode(s);
            //}
            //  frm.ShowDialog();
             * */
        }

        private void btnCodeCombination_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            KeyContent_InputCombination(GetEditingTextBox());
        }

        static public void KeyContent_InputCombination(KB9TextBox t)
        {
            //KB9TextBox t = GetFocusedTextBox();
            if (t == null) return;
            hide_onscreen_kbd();
            frmCombination frm = new frmCombination();
            string s = frm.InputCombinationKeys();

            if (s.Length > 0)
            {
                t.AddKeyCode(s, true);
            }

        }

        private void btnCodeRepeat_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            KeyContent_InputRepeat(GetEditingTextBox());
        }

        static public void KeyContent_InputRepeat(KB9TextBox t)
        {
          //  KB9TextBox t = GetFocusedTextBox();
            if (t == null) return;
            hide_onscreen_kbd();
            frmRepeat frm = new frmRepeat();
            int n = frm.InputRepeat();
            if (n <= 0)
                return;


            string s = n.ToString();
            s = "Repeat" + s;
            t.AddKeyCode(s, true);

        }

        private void btnCodeMacro_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            KeyContent_InputMacro(GetEditingTextBox());
            this.updateStatusBar();
            showFreeSpace();
        }

        public static void KeyContent_InputMacro(KB9TextBox t)
        {
            //KB9TextBox t = GetFocusedTextBox();
            //if (t == null) return;
            hide_onscreen_kbd();
            frmMacro frm = new frmMacro();
            if (t == null)
            {
                frm.ForEditMacro = true;
                frm.ShowDialog();
            }
            else
            {
                frm.ForEditMacro = false;

                string s = frm.input_macro(null);
                if (s == string.Empty)
                    return;

                t.AddKeyCode(s, true);
            }
           

        }

        private void btnCodeLayer_Click(object sender, EventArgs e)
        {
            KB9TextBox t = GetEditingTextBox();
            if (t == null) return;
            frmLayers frm = new frmLayers();
            string s = frm.input_layer(t.Text);

            if (s == string.Empty)
                  return;

            t.AddKeyCode(s, false);
            
        }

        //private void btnSelectAll_Click(object sender, EventArgs e)
        //{

        //    editSelectAll_Click(sender, e);
        //}

        //private void btnUnselect_Click(object sender, EventArgs e)
        //{
        //    editUnselect_Click(sender, e);
        //}

        //private void btnRemoveAll_Click(object sender, EventArgs e)
        //{
        //    editRemoveAll_Click(sender, e);
        //}

        //private void btnSameWidth_Click(object sender, EventArgs e)
        //{
        //    formatMakeSameWidth_Click(sender, e);
        //}

        //private void btnSameHeight_Click(object sender, EventArgs e)
        //{
        //    formatMakeSameHeight_Click(sender, e);
        //}

        //private void btnSameBoth_Click(object sender, EventArgs e)
        //{
        //    formatMakeSameBoth_Click(sender, e);
        //}

        /// <summary>
        /// While we show entity contents, we don't want to fire the textbox_content_changed event.
        /// As in this event, it save its changes to entity, this will make dead loop.
        /// </summary>
        private bool _PauseTextBoxChangedEvent = false;
        public bool PauseTextBoxChangedEvent
        {
            get 
            {
                return _PauseTextBoxChangedEvent;
            }
            set
            {
                _PauseTextBoxChangedEvent = value;
            }
        }

        private void editorMain_Enter(object sender, EventArgs e)
        {
            this.EditingState = EDIT_STATE.Container_Focused;
        }

        private void editorMain_Leave(object sender, EventArgs e)
        {
            //this.EditingState = EDIT_STATE.Unknown;
        }

        private void txtEntityTitle_Leave(object sender, EventArgs e)
        {
           // this.EditingState = EDIT_STATE.Unknown;
        }

        private void txtEntityTitle_Enter(object sender, EventArgs e)
        {
            this.EditingState = EDIT_STATE.TitleEditor_Focused;
        }

        private void pgProperties_Enter(object sender, EventArgs e)
        {
            this.EditingState = EDIT_STATE.PropertyGrid_Focused;
        }

        private void pgProperties_Leave(object sender, EventArgs e)
        {
           // this.EditingState = EDIT_STATE.Unknown;
        }

#region _GUI_STATE_

        private void enable_code_editing_buttons(bool bEnable)
        {
            Control[] ar = new Control[]
            {
                //btnRemoveAll,
                btnUndo,
                btnCodeCombination,
                btnCodeList,
               // btnCodeMacro,
                //btnCodeLayer,
                btnCodePause,
                btnCodeRepeat,
                
            };

            for (int i = 0; i < ar.Length; i++)
                ar[i].Enabled = bEnable;
            if (GetEditingTextBox() != null)
                btnUndo.Enabled = GetEditingTextBox().CanUndo();

        }

        /// <summary>
        /// update the tab control for data viewer
        /// </summary>
        private void update_entity_viewer()
        {
            
            if (this.editorMain.GetSelectCount() != 1)
            {

                enable_tab_pages_viewer(false, 1, null);
                tabCodes.Tag = null;
                return;
            }
            Object obj = tabCodes.Tag;
            DiagramEntity entity = this.editorMain.GetSelectedObject();
            if (entity is DiagramKeyMatrix)
            {

                DiagramKeyMatrix m = (DiagramKeyMatrix)entity;
                entity = m.get_selected_entity();

            }
            if (entity == null)
            {
                tabCodes.Tag = entity;
                enable_tab_pages_viewer(false, 1, null);
                return;
            }
            if (obj != null && obj.Equals(entity))
            {
                if ((obj is DiagramKeySlideH) ||
                    (obj is DiagramKeySlideV))
                    tpLabel.Text = entity.Title;
                else
                    tpCode0.Text = entity.Title;
                return;
            }
            tabCodes.Tag = entity;
            if (entity is DiagramKeyArea)
            {
                enable_tab_pages_viewer(true, 1, entity);
                show_keycodes(entity);

            }
            else if (entity is DiagramKeyMatrix)
            {
                //DiagramKeyMatrix m = (DiagramKeyMatrix)entity;
                //DiagramEntity e = m.get_selected_entity();
                enable_tab_pages_viewer(false, 1, null);
                tabCodes.Tag = null;

            }
            else if (entity is DiagramKeySlideH)
            {
                enable_tab_pages_viewer(true, 4, entity);
                show_keycodes(entity);
            }
            else if (entity is DiagramKeySlideV)
            {
                enable_tab_pages_viewer(true, 4, entity);
                show_keycodes(entity);
            }

        }

        private void enable_tab_pages(bool benable, int nPages)
        {
            update_entity_viewer();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="benable"></param>
        /// <param name="nPages">I don't count the "Label" page in it.
        ///              If nPages == 1, it is for button key. don't show label page
        ///              If npages == 4, it is for showing slide key, show label page</param>
        private void enable_tab_pages_viewer(bool benable, int nPages, DiagramEntity entity)
        {
            
            tabCodes.SuspendLayout();
            enable_kb9textbox(lblKeyCode0, benable);

            if (nPages == 1)
            { //remove label
                if (entity != null)
                    tpCode0.Text = entity.Title;
                else
                    tpCode0.Text = "Key Contents";
                tabCodes.TabPages.Remove(tpLabel);
                tabCodes.TabPages.Remove(tpCode1);
                tabCodes.TabPages.Remove(tpCode2);
                tabCodes.TabPages.Remove(tpCode3);

            }
            else if (nPages == 4)
            {//show for slide key, show lable page too.
                if (tabCodes.TabPages.IndexOf(tpCode1) < 0)
                    tabCodes.TabPages.Add(tpCode1);
                if (tabCodes.TabPages.IndexOf(tpCode2) < 0)
                    tabCodes.TabPages.Add(tpCode2);
                if (tabCodes.TabPages.IndexOf(tpCode3) < 0)
                    tabCodes.TabPages.Add(tpCode3);
                if (tabCodes.TabPages.IndexOf(tpLabel) < 0)
                    tabCodes.TabPages.Insert(0, tpLabel);

                tabCodes.SelectedTab = tpCode0;
            }
            enable_kb9textbox(lblKeyCode1, benable);
            enable_kb9textbox(lblKeyCode2, benable);
            enable_kb9textbox(lblKeyCode3, benable);

            ///////////////////////////////////////////////////////////////////////////
            //if (this.editorMain.GetSelectCount() == 1)
            if (entity != null)
            {
                //DiagramEntity entity = this.editorMain.GetSelectedObject();

                switch (entity.TypeName)
                {
                    case DiagramKey.KEY_BUTTON:
                        {
                            tpCode0.Text = entity.Title;
                        }
                        break;
                    case DiagramKey.KEY_SLIDEV:
                        {
                            string title = entity.Title;
                            tpLabel.Text = title;
                            tpCode0.Text = "Up";
                            tpCode1.Text = "Up Hold";
                            tpCode2.Text = "Down";
                            tpCode3.Text = "Down Hold";
                        }
                        break;
                    case DiagramKey.KEY_SLIDEH:
                        {

                            string title = entity.Title;
                            tpLabel.Text = title;
                            tpCode0.Text = "Left";
                            tpCode1.Text = "Left Hold";
                            tpCode2.Text = "Right";
                            tpCode3.Text = "Right Hold";
                        }
                        break;
                    default:
                        break;

                }
            }

            ////////////////////////////////////////////////////////////////////////////
            //enable_kb9textbox(lblKeyCode2, benable);
            //if (nPages < 3)
            //    tabCodes.TabPages.Remove(tpCode2);
            //else
            //{
            //    if (tabCodes.TabPages.IndexOf(tpCode2) < 0)
            //        tabCodes.TabPages.Add(tpCode2);
            //}

            //enable_kb9textbox(lblKeyCode3, benable);
            //if (nPages < 4)
            //    tabCodes.TabPages.Remove(tpCode3);
            //else
            //{
            //    if (tabCodes.TabPages.IndexOf(tpCode3) < 0)
            //        tabCodes.TabPages.Add(tpCode3);
            //}
            tabCodes.ResumeLayout();
           // tabCodes.Visible = true;
        }
        private void enable_kb9textbox(KB9TextBox txt, bool benable)
        {
          //  if (txt.Visible == benable) return;

            if (!benable)
            {
                show_kb9textbox_text(txt, string.Empty);
                txt.ClearUndo();
            }
            if (txt.Visible != benable)
            {
                this.m_bDisableTextBoxFocusLeaveEvent = true;
                txt.Modified = false;
                txt.Visible = benable;
                
                this.m_bDisableTextBoxFocusLeaveEvent = false;
            }
        }

        private void enable_controls(Control[] ar, bool bEnable)
        {
            for (int i = 0; i < ar.Length; i++)
                ar[i].Enabled = bEnable;

        }

        private void enable_menu_items(ToolStripMenuItem[] ar, bool bEnable)
        {
            for (int i = 0; i < ar.Length; i++)
                ar[i].Enabled = bEnable;
        }
        /// <summary>
        /// Setup the "unknown" state GUI, 
        /// </summary>
        private void GUI_State_Unknown()
        {
            Control[] ar = new Control[]
            {
                btnKeyArea, btnSlideH,btnSlideV,
                btnCopy, btnCut, btnPaste, btnDel, btnUndo, /*btnRemoveAll,*/ btnSelectAll, btnUnselect,
                btnCodePause, btnCodeList, btnCodeCombination, btnCodeRepeat, //btnCodeLayer,
                //btnCodeMacro,
                btnAlignLeft, 
                btnAlignCenter,
                btnAlignRight,
                btnAlignTop,
                btnAlignMiddle,
                btnAlignBottom,

                btnSameHeight,
                btnSameWidth,
                btnSameBoth,
                pgProperties,
                //txtEntityTitle,

            };

            enable_controls(ar, false);
            enable_tab_pages(false, 1);

            ToolStripMenuItem[] items = new ToolStripMenuItem[]
            {
                //btnKeyArea, btnSlideH,btnSlideV,
                editCopy, editCut, editPaste, editDel, editUndo, /*editRemoveAll, editSelectAll,*/ editUnselect,
                contentsCodePause, contentsCodeList, contentsCodeCombination, contentsCodeRepeat, contentsInsertMacro, //contentsCodeLayer,
                
                formatAlignLeft, 
                formatAlignCenter,
                formatAlignRight,
                formatAlignTop,
                formatAlignMiddle,
                formatAlignBottom,

                formatSameHeight,
                formatSameWidth,
                formatSameBoth,
                
                

            };
            enable_menu_items(items, false);

        }

        /// <summary>
        /// While the entities container focused, setup the GUI.
        /// 
        /// </summary>
        private void GUI_State_Container(bool bResetTab)
        {
            //-----------------------------------
            Control[] disables = new Control[]
            {
                
                btnCodePause, btnCodeList, btnCodeCombination, btnCodeRepeat, //btnCodeLayer,
                //txtEntityTitle,
               
            };

            enable_controls(disables, false);
            if (bResetTab)
                enable_tab_pages(false, 1);

            ToolStripMenuItem[] menudisables = new ToolStripMenuItem[]
            {
                 contentsCodePause, contentsCodeList, contentsCodeCombination, contentsCodeRepeat,contentsInsertMacro, //contentsCodeLayer,
            };
            enable_menu_items(menudisables, false);

            //-----------------------------------
            Control[] enables = new Control[]
            {
                btnKeyArea, btnSlideH,btnSlideV,
                pgProperties,btnCodeMacro,/*btnRemoveAll,*/ btnSelectAll,
            };

            enable_controls(enables, true);

            //ToolStripMenuItem[] menuenables = new ToolStripMenuItem[]
            //{
            //     //editRemoveAll, editRemoveAll
            //};
            //enable_menu_items(menuenables, true);

            //-----------------------------------
            int n = editorMain.GetSelectCount();
            //System.Diagnostics.Debug.Print("selected count=" + n.ToString());
            //-----------------------------------
            Control[] enable_selected = new Control[]
            {
                btnUnselect,btnCopy, btnCut,btnDel,
            };
            enable_controls(enable_selected, (n > 0));
            //
            ToolStripMenuItem[] menuselected = new ToolStripMenuItem[]
            {
                editUnselect,editCopy, editCut,editDel,
            };
            enable_menu_items(menuselected, (n > 0));

            //-----------------------------------
            Control[] enable_multiple_selected = new Control[]
            {
                btnAlignLeft, 
                btnAlignCenter,
                btnAlignRight,
                btnAlignTop,
                btnAlignMiddle,
                btnAlignBottom,

                btnSameHeight,
                btnSameWidth,
                btnSameBoth,
            };

            enable_controls(enable_multiple_selected, (n > 1));
            //
            ToolStripMenuItem[] menumultiple = new ToolStripMenuItem[]
            {
                formatAlignLeft, formatAlignCenter,formatAlignRight,
                formatAlignTop, formatAlignMiddle,  formatAlignBottom,
                formatSameHeight, formatSameWidth,formatSameBoth,
            };
            enable_menu_items(menumultiple, (n > 1));

            //-----------------------------------
            Control[] enable_copied = new Control[]
            {
                btnPaste
            };


            enable_controls(enable_copied, editorMain.GetCopyCount() > 0);
            editPaste.Enabled = (editorMain.GetCopyCount() > 0);

            //-----------------------------------
            Control[] enable_changed = new Control[]
            {
                btnUndo
            };

            enable_controls(enable_changed, editorMain.GetUndoCount() > 0);

            editUndo.Enabled = (editorMain.GetUndoCount() > 0);

            editRedo.Enabled = (editorMain.CanRedo());
            btnRedo.Enabled = editRedo.Enabled;// (editorMain.CanRedo());
        }

        private void GUI_State_Button()
        {
            GUI_State_Container(false);
            
            if (editorMain.GetSelectCount() == 1)
            {
                enable_tab_pages(true, 1);
            }
            else if (editorMain.GetSelectCount() <= 0)
            {
                pgProperties.SelectedObject = null;
                enable_tab_pages(false, 1);
            }
            else
            {

                show_mulitple_selected_entities_properties();
                enable_tab_pages(false, 1);
            }
            //Control[] enables = new Control[]
            //{
            //    txtEntityTitle,  
            //};
            //enable_controls(enables, true);
        }

        private void GUI_State_SlideV()
        {
            GUI_State_Container(false);
            if (editorMain.GetSelectCount() == 1)
                enable_tab_pages(true, 4);
            else if (editorMain.GetSelectCount() <= 0)
            {
                pgProperties.SelectedObject = null;
                enable_tab_pages(false, 1);
            }
            else
            {
                show_mulitple_selected_entities_properties();
                enable_tab_pages(false, 1);
            }
            //Control[] enables = new Control[]
            //{
            //    txtEntityTitle,  
            //};
            //enable_controls(enables, true);
        }

        private void GUI_State_SlideH()
        {
            GUI_State_Container(false);
            if (editorMain.GetSelectCount() == 1)
                enable_tab_pages(true, 4);
            else if (editorMain.GetSelectCount() <= 0)
            {
                pgProperties.SelectedObject = null;
                enable_tab_pages(false, 1);
            }
             else
            {
                show_mulitple_selected_entities_properties();
                enable_tab_pages(false, 1);
            }
            //Control[] enables = new Control[]
            //{
            //    //txtEntityTitle,  
            //};
            //enable_controls(enables, true);
        }

        private void GUI_State_Matrix()
        {
            GUI_State_Container(false);
            enable_tab_pages(false, 1);
           
            if (editorMain.GetSelectCount() <= 0)
            {
                pgProperties.SelectedObject = null;
            
            }
            else if (editorMain.GetSelectCount() >1)
            {
                show_mulitple_selected_entities_properties();
            
            }
            
        }

        private void GUI_State_CodeTextBox()
        {
           // GUI_State_Container();
           // enable_tab_pages(true, 4);
            Control[] enables = new Control[]
            {
                //txtEntityTitle,  
                 btnCodePause, btnCodeList, btnCodeCombination, btnCodeRepeat, //btnCodeLayer,
            };
            enable_controls(enables, true);

            ToolStripMenuItem[] menuenables = new ToolStripMenuItem[]
            {
                contentsCodePause, contentsCodeList, contentsCodeCombination, contentsCodeRepeat, contentsInsertMacro,//contentsCodeLayer,
            };
            enable_menu_items(menuenables, true);


            KB9TextBox t = GetEditingTextBox();
            if (t == null)
                return;
            //-----------------------------------
            bool bselected = (t.HasSelection());
            Control[] enables_selected = new Control[]
            {
                btnCopy, btnCut, btnUnselect
            };
            enable_controls(enables_selected, bselected);

            ToolStripMenuItem[] menuselected = new ToolStripMenuItem[]
            {
                 editCopy, editCut, editUnselect
            };
            enable_menu_items(menuselected, true);
            //-----------------------------------

            bool bUndo = t.CanUndo();
            btnUndo.Enabled = bUndo;
            editUndo.Enabled = bUndo;

            bool bCopied = Clipboard.ContainsText();
            btnPaste.Enabled = bCopied;
            editPaste.Enabled = bCopied;

            btnRedo.Enabled = false;
        }

        private void GUI_State_TitleTextBox()
        {
            //// GUI_State_Container();
            //// enable_tab_pages(true, 4);
            //Control[] disables = new Control[]
            //{
            //  //  txtEntityTitle,  
            //     btnCodePause, btnCodeList, btnCodeCombination, btnCodeRepeat, btnCodeLayer,
            //};
            //enable_controls(disables, false);

            //ToolStripMenuItem[] menudisables = new ToolStripMenuItem[]
            //{
            //    contentsCodePause, contentsCodeList, contentsCodeCombination, contentsCodeRepeat, contentsCodeLayer,
            //};
            //enable_menu_items(menudisables, false);
            ////----------------------------------------------
            //Control[] enables = new Control[]
            //{
            //   btnRemoveAll, btnSelectAll
            //};
            //enable_controls(enables, true);
            //ToolStripMenuItem[] menuenables = new ToolStripMenuItem[]
            //{
            //    editRemoveAll, editSelectAll
            //};
            //enable_menu_items(menuenables, true);


            //TextBox t = txtEntityTitle;

            //bool bselected = (t.SelectionLength > 0);
            //Control[] enables_selected = new Control[]
            //{
            //    btnCopy, btnCut, btnUnselect
            //};
            //enable_controls(enables_selected, bselected);
            //ToolStripMenuItem[] menu_selected = new ToolStripMenuItem[]
            //{
            //     editCopy, editCut, editUnselect
            //};

            //enable_menu_items(menu_selected, bselected);


            //bool bUndo = t.CanUndo;
            //btnUndo.Enabled = bUndo;
            //editUndo.Enabled = bUndo;

            //bool bCopied = Clipboard.ContainsText();
            //btnPaste.Enabled = bCopied;
            //editPaste.Enabled = bCopied;
   

        }
        private void UpdateGUIState()
        {
            switch (this.EditingState)
            {
                case EDIT_STATE.Unknown:
                    {
                        GUI_State_Unknown();
                    }
                    break;
                case EDIT_STATE.Container_Focused:
                    {
                        GUI_State_Container(true);
                    }
                    break;
                case EDIT_STATE.EntityButton_Focused:
                    {
                        GUI_State_Button();
                    }
                    break;
                case EDIT_STATE.EntitySlideV_Focused:
                    {
                        GUI_State_SlideV();
                    }
                    break;
                case EDIT_STATE.EntitySlideH_Focused:
                    {
                        GUI_State_SlideH();
                    }
                    break;
                case EDIT_STATE.EntityMatrix_Focused:
                    {
                        GUI_State_Matrix();
                    }
                    break;

               case EDIT_STATE.CodeEditor_Focused:
                    {
                        GUI_State_CodeTextBox();
                    }
                    break;

                case EDIT_STATE.PropertyGrid_Focused:
                    {
                        GUI_State_Container(true);
                    }
                    break;
                case EDIT_STATE.TitleEditor_Focused:
                    {
                        GUI_State_TitleTextBox();
                    }
                    break;
            }

            int n = (int)decimal.Round( ((decimal)editorMain.Zoom * (decimal)100));
            if (n > tkbZoom.Maximum) n = tkbZoom.Maximum;
            if (n < tkbZoom.Minimum) n = tkbZoom.Minimum;
            tkbZoom.Value = n;
            updateStatusBar();
        }
#endregion  //_GUI_STATE_

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null )
                    t.Clear();
            }
            else
            {
                editRemoveAll_Click(sender, e);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                KB9TextBox t = GetEditingTextBox();
                if (t != null)
                    t.OnCopy(null, null);
            }
            else
            {
                editRemoveAll_Click(sender, e);
            }
        }

        private void contentsCodePause_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            btnCodePause_Click(sender, e);
        }

        private void contentsCodeCombination_Click(object sender, EventArgs e)
        {
            btnCodeCombination_Click(sender, e);
        }

        private void contentsCodeRepeat_Click(object sender, EventArgs e)
        {
            btnCodeRepeat_Click(sender, e);
        }

        private void contentsCodeLayer_Click(object sender, EventArgs e)
        {
            btnCodeLayer_Click(sender, e);
        }

        private void contentsCodeList_Click(object sender, EventArgs e)
        {
            btnCodeList_Click(sender, e);
        }

        private void kb9000TestDataStruture_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            frmInputStructure frm = new frmInputStructure(editorMain);
            string s = frm.InputDataStructure();
            if (s == string.Empty)
                return;
            editRemoveAll_Click(null, null);
            editorMain.LoadCSV(s);
        }

        private void tabCodes_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tpLabel)
                e.Cancel = true;
        }

        private void btnActionsDetect_Click(object sender, EventArgs e)
        {
            kb9000Detect_Click(null, null);
        }

        private void btnActionsRead_Click(object sender, EventArgs e)
        {
            kb9000Read_Click(null, null);
        }

        private void btnActionsWrite_Click(object sender, EventArgs e)
        {
            kb9000Write_Click(null, null);
        }

        private void btnActionsTest_Click(object sender, EventArgs e)
        {
            kb9000Test_Click(null, null);
        }

        private void SetTextBoxFocus(TabPage tp)
        {
            if (tp == tpLabel)
                return;
            else if (tp == tpCode0)
            {
                lblKeyCode0.Focus();
                lblKeyCode0.moveCaretEnd();
            }
            else if (tp == tpCode1)
            {
                lblKeyCode1.Focus();
                lblKeyCode1.moveCaretEnd();
            }
            else if (tp == tpCode2) 
            {
                lblKeyCode2.Focus();
                lblKeyCode2.moveCaretEnd();
            }
            else if (tp == tpCode3)
            {
                lblKeyCode3.Focus();
                lblKeyCode3.moveCaretEnd();
            }
        }

        private void tabCodes_Selected(object sender, TabControlEventArgs e)
        {
            //SetTextBoxFocus(e.TabPage);

            
        }

        private void tabCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TabPage tp = tabCodes.SelectedTab;
            //SetTextBoxFocus(tp);
        }

        private void tabCodes_Click(object sender, EventArgs e)
        {
            //TabPage tp = tabCodes.SelectedTab;
            //SetTextBoxFocus(tp);
        }

        private void filePreview_Click(object sender, EventArgs e)
        {
            LegendPrinter prn = new LegendPrinter();
            editorMain.UnselectAll(); //prevent the selected mark!
            bool bEnable3X = Settings.Default.Enable3X;
            Image img = null;
            if (bEnable3X)
                img = editorMain.CreateImage3X();
            else
                img = editorMain.CreateImage();

            prn.PrintPreviewImage(img);

            //close_onscreen_kbd();
            //Image img = editorMain.CreateImage();
            //frmPreview.PreviewImage(img);
        }
        /// <summary>
        /// as I change other property while one propertye changed, 
        /// I use this function to refresh whole grid
        /// see properties round corner radius and border thickness in diagramkey.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="entity"></param>
        private void editorMain_OnRefreshPropertiesGrid(DiagramEditor sender, DiagramEntity entity)
        {
           // pgProperties.SelectedObject = null;
           // pgProperties.SelectedObject = entity;
            if (entity != null)
                pgProperties.Refresh();
            else
                this.UpdateGUI();
        }

        private void exportJpg_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string f = dlg.FileName;
            ExportToImage(f, ImageFormat.Jpeg);

            KB9Logger.Log("Export file: " + f);

            
        }

        private bool ExportToImage(string fileName, ImageFormat format)
        {
            editorMain.UnselectAll(); //prevent the selected mark!
            Image img = editorMain.CreateImage();
            string f = fileName;
            img.Save(f, format);
            
            editorMain.Refresh();
            UpdateGUI();
            return true;
        }

        private void exportBmp_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string f = dlg.FileName;
            ExportToImage(f, ImageFormat.Bmp);
            KB9Logger.Log("Export file: " + f);
        }

        private void exportPng_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Png files (*.png)|*.png|All files (*.*)|*.*";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string f = dlg.FileName;
            ExportToImage(f, ImageFormat.Png);
            KB9Logger.Log("Export file: " + f);
        }

        private void menuFile_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
        }

        private void menuEdit_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
        }

        private void formatMakeSame_Click(object sender, EventArgs e)
        {

        }

        private void lblKeyCode0_OnSelectionChanged(object sender)
        {
            UpdateGUI();
        }

        private void lblKeyCode0_Validating(object sender, CancelEventArgs e)
        {
            //tsMsg0.Text = "";
            setStatusBarText("");
            KB9TextBox t = (KB9TextBox)sender;
            if (t == null)
                return;
            int nres = KB9Validation.ValidateKeyTextBox2(this, t);
            if (nres == 0)
            {
                t.ClearUndo();
                return;
            }

          //  MessageBox.Show("lblKeyCode0_Validating");

            e.Cancel = true;
            pgProperties.Focus();
            editorMain.ResetMouseEvent();
            //for matrix key
            DiagramEntity entity = GetCurrentEntity();// editorMain.GetSelectedObject();
            //if (entity.TypeName == DiagramKeyMatrix.KEY_MATRIX)
            //{
            //    entity = ((DiagramKeyMatrix)entity).get_selected_entity();

            //}

            //editorMain.SetSelectedEntity(editorMain.GetSelectedObject());
            editorMain.SetSelectedEntity(entity);
            editorMain.removeNewUndo(m_undoItemBeforeLeaveTextbox);
            m_undoItemBeforeLeaveTextbox = null;
            m_bJustUndoFocus = true;
            if (nres == 3) //cancel changes
            {
                m_bJustUndoFocus = false;
                editorMain.Undo();
                
            }
           // t.Modified = false;
            System.Diagnostics.Debug.Print("Undo after valide = " + editorMain.DiagramEntityContainer.UndoStack.Count.ToString());
        }

        private void filePrint_Click(object sender, EventArgs e)
        {
            LegendPrinter prn = new LegendPrinter();
            editorMain.UnselectAll(); //prevent the selected mark!
            bool bEnable3X = Settings.Default.Enable3X;
            Image img = null;
            if (bEnable3X)
                img = editorMain.CreateImage3X();
            else

                img = editorMain.CreateImage();

            prn.PrintWithDialog(img);

            
        }

        private void fileExportImageViewer_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            Image img = editorMain.CreateImage();
            frmPreview.PreviewImage(img);
        }

        private void helpLog_Click(object sender, EventArgs e)
        {
            Settings.Default.EnableLog = helpLog.Checked;
            Settings.Default.Save();
            bool benable = helpLog.Checked;
            if (benable)
            {//current is enable, old is disabled.
                KB9Logger.EnableLog(helpLog.Checked);
                KB9Logger.Log("Log enabled: " + helpLog.Checked);
            }
            else
            {
                KB9Logger.Log("Log enabled: " + helpLog.Checked);
                KB9Logger.EnableLog(helpLog.Checked);
            }
            KB9API.EnableDisableLogFile(benable);//20150112, add dll log function.
            
           
            
        }

        private void helpClearLog_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Do you want to clear log file contents?",
                                            "Question",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                KB9Logger.Instance().Clear();
                KB9API.KB9API_ERROR  err = KB9API.ClearLogFile();//20140112, add dll log function.
                

                if (err != KB9API.KB9API_ERROR.FUNC_SUCCESSFUL)
                {
                    string strmsg = "";
                    strmsg = result_message(err);
                    MessageBox.Show(strmsg,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }


        }

        private void contentsDiableBeep_Click(object sender, EventArgs e)
        {
            editorMain.set_all_keys_beep_duration(KB9Const.Min_BeepDuration);
            pgProperties.Refresh();
        }

        private void contentsEnableBeep_Click(object sender, EventArgs e)
        {
            editorMain.set_all_keys_beep_duration(KB9Const.DEFAULT_BeepDuration);
            pgProperties.Refresh();
        }

        private void keyNewKey_Click(object sender, EventArgs e)
        {
            btnKeyArea_Click(sender, e);
        }

        private void keysNewSlideV_Click(object sender, EventArgs e)
        {
            btnSlideV_Click(sender, e);
        }

        private void keysNewSlideH_Click(object sender, EventArgs e)
        {
            btnSlideH_Click(sender, e);
        }

        private void keysClearBeep_Click(object sender, EventArgs e)
        {
            editorMain.set_all_keys_beep_duration(KB9Const.Min_BeepDuration);
            pgProperties.Refresh();
        }

        private void keysDefaultBeep_Click(object sender, EventArgs e)
        {
            editorMain.set_all_keys_beep_duration(KB9Const.DEFAULT_BeepDuration);
            pgProperties.Refresh();
        }

        private void keysNewMatrix_Click(object sender, EventArgs e)
        {
            frmKeysMatrix frm = new frmKeysMatrix();
            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DiagramKeyMatrix obj = new DiagramKeyMatrix();
            //obj.Matrix = frm.Matrix;
            obj.Rows = frm.Rows;
            obj.RowSpacing = frm.RowSpacing;
            obj.Cols = frm.Cols;
            obj.ColSpacing = frm.ColSpacing;

            obj.Title = "";// editorMain.CreateUniqueName(obj.TypeName);
            editorMain.DrawingObject = obj;
        }

        private void keyMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keysNewMatrix_Click(sender, e);
        }

        private void frmMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.KeyCode == Keys.Space)
            //    MessageBox.Show("space");
        }

        public void editRedo_Click(object sender, EventArgs e)
        {
            close_onscreen_kbd();
            if (this.EditingState == EDIT_STATE.CodeEditor_Focused)
            {
                //KB9TextBox t = GetEditingTextBox();
                //if (t != null)
                //    t(null, null);
            }
            else if (this.EditingState == EDIT_STATE.TitleEditor_Focused)
            {
                //if (!txtEntityTitle.Focused)
                //    return;
                //txtEntityTitle.Undo();
            }
            else
                editorMain.Redo();
            editorMain_OnShowDiagramEntityProperties(editorMain, editorMain.GetSelectedObject());
            UpdateGUI();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            editRedo_Click(sender, e);
        }

        private void showFreeSpace()
        { 
            try
            {
                if (editorMain.ErrorOverlapped())
                {
                    tsFreeMem.Text = "Overlapped key layout.";
                    return;
                }
                int nBytes = DetectKB9000FreeSpace();
                
                if (nBytes >= 0)
                {

                    tsFreeMem.Text = "Free space: " + nBytes.ToString("n0") + " bytes      ";
                }
                else
                {
                    tsFreeMem.Text = "Error #" +Math.Abs(nBytes).ToString() + " in space calculation.     ";// +nBytes.ToString() + " Bytes ";
                }
            }
            catch (Exception err)
            {
                string strmsg = "Error in space calculation.";
                tsFreeMem.Text = strmsg;
                
            }
        }

        private void updateStatusBar()
        {
            if (lblKeyCode0.Focused ||
                lblKeyCode1.Focused ||
                lblKeyCode2.Focused ||
                lblKeyCode3.Focused)
                return;
            String s = "";
            if (editorMain.GetSelectCount() == 1)
            {
                s = "Click on content box to edit";
            }
            else if (editorMain.GetObjectCount() >0)
            {
                showFreeSpace();
                //s = "z - Key content is empty   ¢- Key content modified, not saved yet";
               // setStatusBarText("z", " -  Key content is empty", "     ¢", " -  Key content modified, not saved yet");
                setStatusBarText("{", "- Key content is empty       ", "¢", "- Key content modified, not saved yet");
                return;
//Legend Triangles: Contents is empty. Legend Rectangles: Contents changed, but don't save. ";
            }
            showFreeSpace();

            setStatusBarText(s);
            //free space
            if (editorMain.GetSelectCount() == 0)
            {
              

            }

            //tsMsg0.Text = s;
        }
        private void setStatusBarText(string strMsg)
        {
            setStatusBarText("", strMsg, "", "");
        }
        /// <summary>
        /// Use it to show "Windings3 fonts" information
        /// 
        /// </summary>
        /// <param name="strMsg0">"Windings3 fonts" message</param>
        /// <param name="strMsg1"></param>
        /// <param name="strMsg2">"Windings3 fonts" message</param>
        /// <param name="strMsg3"></param>
        private void setStatusBarText(string strMsg0, string strMsg1, string strMsg2, string strMsg3)
        {
            tsMsg0.Text = "";// strMsg0;
            if (!strMsg0.Equals(""))
                tsMsg0.Image = KB9Utility.Properties.Resources.triangle;//.del;// Bitmap.FromResource(
            else
                tsMsg0.Image = null;

            tsMsg.Text = strMsg1;
            tsMsg2.Text = "";// strMsg2;
            if (!strMsg2.Equals(""))
                tsMsg2.Image = KB9Utility.Properties.Resources.block;//.del;// Bitmap.FromResource(
            else
                tsMsg2.Image = null;

            tsMsg3.Text = strMsg3;

        }
        private bool m_bDisableTextBoxFocusLeaveEvent = false;
        private void editorMain_OnEditorUndo(DiagramEditor sender, UndoItem undo)
        {
            m_bDisableTextBoxFocusLeaveEvent = true;
            editorMain_OnShowDiagramEntityProperties(editorMain, editorMain.GetSelectedObject());
           // return;
            UpdateGUI();
            lblKeyCode0.Modified = false;
            lblKeyCode1.Modified = false;
            lblKeyCode2.Modified = false;
            lblKeyCode3.Modified = false;
            //m_bDisableTextBoxFocusLeaveEvent = false;
            if (undo.LastFocusedEditBoxIndex >= 0)
            {
                if (undo.LastFocusedEditBoxIndex == 0)
                {
                    tabCodes.SelectedTab = tpCode0;
                    //tpCode1..Focused = false;
                    editorMain.Focus();
                }
                //m_bDisableTextBoxFocusLeaveEvent = true;
                if (undo.LastFocusedEditBoxIndex == 1)
                {
                    tabCodes.SelectedTab = tpCode1;
                    //tpCode1..Focused = false;
                    editorMain.Focus();
                }

                    //tpCode1.Focus();
                if (undo.LastFocusedEditBoxIndex == 2)
                {
                    tabCodes.SelectedTab = tpCode2;
                    //tabCodes.SelectedIndex = 3;
                    //tpCode2.Focus();
                    editorMain.Focus();
                }
                if (undo.LastFocusedEditBoxIndex == 3)
                {
                    tabCodes.SelectedTab = tpCode3;
                    editorMain.Focus();
                    //tabCodes.SelectedIndex = ;
                    //tpCode3.Focus();
                }
               

            }
            m_bDisableTextBoxFocusLeaveEvent = false;
            
        }

        private void contentsInsertMacro_Click(object sender, EventArgs e)
        {
            btnCodeMacro_Click(null, null);
        }

        private void fileEnablePrint3X_Click(object sender, EventArgs e)
        {
            Settings.Default.Enable3X = fileEnablePrint3X.Checked;
            Settings.Default.Save();
            bool benable = fileEnablePrint3X.Checked;
            
        }
        //private void menuMain_KeyDown(object sender, KeyEventArgs e)
        //{
        //    e.Handled = true;
        //}
        //protected override bool ProcessDialogKey(Keys keyData)
        //{
        //    //return base.ProcessDialogKey(keyData);
        //    return true;
        //}

        //private void frmMain_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //if (e.KeyCode == Keys.F &&
        //    //    e.Modifiers == Keys.Alt)
        //    //    e.SuppressKeyPress = true;
        //}
    }
}
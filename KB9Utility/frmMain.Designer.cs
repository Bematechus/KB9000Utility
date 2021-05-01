using System.Drawing;
namespace KB9Utility
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            KB9Utility.KeyEditingType keyEditingType1 = new KB9Utility.KeyEditingType();
            KB9Utility.KeyEditingType keyEditingType2 = new KB9Utility.KeyEditingType();
            KB9Utility.KeyEditingType keyEditingType3 = new KB9Utility.KeyEditingType();
            KB9Utility.KeyEditingType keyEditingType4 = new KB9Utility.KeyEditingType();
            KB9Utility.KeyEditingType keyEditingType5 = new KB9Utility.KeyEditingType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.fileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.fileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.exportJpg = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBmp = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPng = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.fileExportImageViewer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.filePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.filePreview = new System.Windows.Forms.ToolStripMenuItem();
            this.fileEnablePrint3X = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fileRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.fileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.keysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyNewKey = new System.Windows.Forms.ToolStripMenuItem();
            this.keysNewSlideV = new System.Windows.Forms.ToolStripMenuItem();
            this.keysNewSlideH = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.keysNewMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            this.keysClearBeep = new System.Windows.Forms.ToolStripMenuItem();
            this.keysDefaultBeep = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.editCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.editCut = new System.Windows.Forms.ToolStripMenuItem();
            this.editPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.editDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.editUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.editRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.editSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.editUnselect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.editMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.formatAlign = new System.Windows.Forms.ToolStripMenuItem();
            this.formatAlignLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.formatAlignCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.formatAlignRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.formatAlignTop = new System.Windows.Forms.ToolStripMenuItem();
            this.formatAlignMiddle = new System.Windows.Forms.ToolStripMenuItem();
            this.formatAlignBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.formatMakeSame = new System.Windows.Forms.ToolStripMenuItem();
            this.formatSameWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.formatSameHeight = new System.Windows.Forms.ToolStripMenuItem();
            this.formatSameBoth = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.formatHEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.formatVEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContents = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsCodePause = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsCodeCombination = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsCodeRepeat = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsInsertMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.contentsCodeList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKB9000 = new System.Windows.Forms.ToolStripMenuItem();
            this.kb9000Detect = new System.Windows.Forms.ToolStripMenuItem();
            this.kb9000Write = new System.Windows.Forms.ToolStripMenuItem();
            this.kb9000Read = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.kb9000Test = new System.Windows.Forms.ToolStripMenuItem();
            this.kb9000TestDataStruture = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.helpLog = new System.Windows.Forms.ToolStripMenuItem();
            this.helpClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.helpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.tsFreeMem = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMsg0 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMsg2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMsg3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tkbZoom = new System.Windows.Forms.TrackBar();
            this.lblPercent = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabCodes = new System.Windows.Forms.TabControl();
            this.tpLabel = new System.Windows.Forms.TabPage();
            this.tpCode0 = new System.Windows.Forms.TabPage();
            this.lblKeyCode0 = new KB9Utility.KB9TextBox();
            this.tpCode1 = new System.Windows.Forms.TabPage();
            this.lblKeyCode1 = new KB9Utility.KB9TextBox();
            this.tpCode2 = new System.Windows.Forms.TabPage();
            this.lblKeyCode2 = new KB9Utility.KB9TextBox();
            this.tpCode3 = new System.Windows.Forms.TabPage();
            this.lblKeyCode3 = new KB9Utility.KB9TextBox();
            this.panelButton = new KB9Utility.OfficePanel();
            this.btnSlideH = new KB9Utility.NSButton();
            this.btnSlideV = new KB9Utility.NSButton();
            this.btnKeyArea = new KB9Utility.NSButton();
            this.panelTools = new KB9Utility.KB9Panel();
            this.panelActions = new KB9Utility.OfficePanel();
            this.btnActionsTest = new KB9Utility.NSButton();
            this.btnActionsWrite = new KB9Utility.NSButton();
            this.btnActionsRead = new KB9Utility.NSButton();
            this.btnActionsDetect = new KB9Utility.NSButton();
            this.panelKeyEdit = new KB9Utility.OfficePanel();
            this.btnCodeMacro = new KB9Utility.NSButton();
            this.btnCodeRepeat = new KB9Utility.NSButton();
            this.btnCodeCombination = new KB9Utility.NSButton();
            this.btnCodeList = new KB9Utility.NSButton();
            this.btnCodePause = new KB9Utility.NSButton();
            this.panelFormat = new KB9Utility.OfficePanel();
            this.btnSameBoth = new KB9Utility.NSButton();
            this.btnAlignMiddle = new KB9Utility.NSButton();
            this.btnAlignTop = new KB9Utility.NSButton();
            this.btnSameHeight = new KB9Utility.NSButton();
            this.btnSameWidth = new KB9Utility.NSButton();
            this.btnAlignBottom = new KB9Utility.NSButton();
            this.btnAlignRight = new KB9Utility.NSButton();
            this.btnAlignCenter = new KB9Utility.NSButton();
            this.btnAlignLeft = new KB9Utility.NSButton();
            this.panelEdit = new KB9Utility.OfficePanel();
            this.btnRedo = new KB9Utility.NSButton();
            this.btnUnselect = new KB9Utility.NSButton();
            this.btnUndo = new KB9Utility.NSButton();
            this.btnSelectAll = new KB9Utility.NSButton();
            this.btnDel = new KB9Utility.NSButton();
            this.btnPaste = new KB9Utility.NSButton();
            this.btnCut = new KB9Utility.NSButton();
            this.btnCopy = new KB9Utility.NSButton();
            this.panelFile = new KB9Utility.OfficePanel();
            this.btnSaveAs = new KB9Utility.NSButton();
            this.btnSave = new KB9Utility.NSButton();
            this.btnOpen = new KB9Utility.NSButton();
            this.btnNew = new KB9Utility.NSButton();
            this.pgProperties = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.editorMain = new KB9Utility.DEditor();
            this.btnZoomOut = new KB9Utility.NSButton();
            this.btnZoomIn = new KB9Utility.NSButton();
            this.toolTips = new KB9Utility.EnhancedToolTip(this.components);
            this.menuMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkbZoom)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabCodes.SuspendLayout();
            this.tpCode0.SuspendLayout();
            this.tpCode1.SuspendLayout();
            this.tpCode2.SuspendLayout();
            this.tpCode3.SuspendLayout();
            this.panelButton.SuspendLayout();
            this.panelTools.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.panelKeyEdit.SuspendLayout();
            this.panelFormat.SuspendLayout();
            this.panelEdit.SuspendLayout();
            this.panelFile.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.keysToolStripMenuItem,
            this.menuEdit,
            this.menuFormat,
            this.menuContents,
            this.menuKB9000,
            this.menuHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(948, 25);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNew,
            this.fileOpen,
            this.fileSave,
            this.fileSaveAs,
            this.fileExport,
            this.toolStripMenuItem11,
            this.filePrint,
            this.filePreview,
            this.fileEnablePrint3X,
            this.toolStripMenuItem1,
            this.fileRecent,
            this.toolStripMenuItem8,
            this.fileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(39, 21);
            this.menuFile.Text = "&File";
            this.menuFile.Click += new System.EventHandler(this.menuFile_Click);
            // 
            // fileNew
            // 
            this.fileNew.Image = global::KB9Utility.Properties.Resources.NewFile;
            this.fileNew.Name = "fileNew";
            this.fileNew.Size = new System.Drawing.Size(164, 22);
            this.fileNew.Text = "&New";
            this.fileNew.Click += new System.EventHandler(this.fileNew_Click);
            // 
            // fileOpen
            // 
            this.fileOpen.Image = global::KB9Utility.Properties.Resources.open;
            this.fileOpen.Name = "fileOpen";
            this.fileOpen.Size = new System.Drawing.Size(164, 22);
            this.fileOpen.Text = "&Open";
            this.fileOpen.Click += new System.EventHandler(this.fileOpen_Click);
            // 
            // fileSave
            // 
            this.fileSave.Image = global::KB9Utility.Properties.Resources.save;
            this.fileSave.Name = "fileSave";
            this.fileSave.Size = new System.Drawing.Size(164, 22);
            this.fileSave.Text = "&Save";
            this.fileSave.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // fileSaveAs
            // 
            this.fileSaveAs.Image = global::KB9Utility.Properties.Resources.saveas;
            this.fileSaveAs.Name = "fileSaveAs";
            this.fileSaveAs.Size = new System.Drawing.Size(164, 22);
            this.fileSaveAs.Text = "Save &As";
            this.fileSaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // fileExport
            // 
            this.fileExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportJpg,
            this.exportBmp,
            this.exportPng,
            this.toolStripMenuItem12,
            this.fileExportImageViewer});
            this.fileExport.Name = "fileExport";
            this.fileExport.Size = new System.Drawing.Size(164, 22);
            this.fileExport.Text = "&Export To";
            // 
            // exportJpg
            // 
            this.exportJpg.Name = "exportJpg";
            this.exportJpg.Size = new System.Drawing.Size(161, 22);
            this.exportJpg.Text = "JPG";
            this.exportJpg.Click += new System.EventHandler(this.exportJpg_Click);
            // 
            // exportBmp
            // 
            this.exportBmp.Name = "exportBmp";
            this.exportBmp.Size = new System.Drawing.Size(161, 22);
            this.exportBmp.Text = "Bitmap";
            this.exportBmp.Click += new System.EventHandler(this.exportBmp_Click);
            // 
            // exportPng
            // 
            this.exportPng.Name = "exportPng";
            this.exportPng.Size = new System.Drawing.Size(161, 22);
            this.exportPng.Text = "PNG";
            this.exportPng.Click += new System.EventHandler(this.exportPng_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(158, 6);
            // 
            // fileExportImageViewer
            // 
            this.fileExportImageViewer.Name = "fileExportImageViewer";
            this.fileExportImageViewer.Size = new System.Drawing.Size(161, 22);
            this.fileExportImageViewer.Text = "Image Preview";
            this.fileExportImageViewer.Click += new System.EventHandler(this.fileExportImageViewer_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(161, 6);
            // 
            // filePrint
            // 
            this.filePrint.Name = "filePrint";
            this.filePrint.Size = new System.Drawing.Size(164, 22);
            this.filePrint.Text = "&Print";
            this.filePrint.Click += new System.EventHandler(this.filePrint_Click);
            // 
            // filePreview
            // 
            this.filePreview.Name = "filePreview";
            this.filePreview.Size = new System.Drawing.Size(164, 22);
            this.filePreview.Text = "Print P&review";
            this.filePreview.Click += new System.EventHandler(this.filePreview_Click);
            // 
            // fileEnablePrint3X
            // 
            this.fileEnablePrint3X.CheckOnClick = true;
            this.fileEnablePrint3X.Name = "fileEnablePrint3X";
            this.fileEnablePrint3X.Size = new System.Drawing.Size(164, 22);
            this.fileEnablePrint3X.Text = "Enable Print 3X";
            this.fileEnablePrint3X.Click += new System.EventHandler(this.fileEnablePrint3X_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // fileRecent
            // 
            this.fileRecent.Name = "fileRecent";
            this.fileRecent.Size = new System.Drawing.Size(164, 22);
            this.fileRecent.Text = "&Recent Files";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(161, 6);
            // 
            // fileExit
            // 
            this.fileExit.Name = "fileExit";
            this.fileExit.Size = new System.Drawing.Size(164, 22);
            this.fileExit.Text = "E&xit";
            this.fileExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // keysToolStripMenuItem
            // 
            this.keysToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyNewKey,
            this.keysNewSlideV,
            this.keysNewSlideH,
            this.toolStripMenuItem15,
            this.keysNewMatrix,
            this.toolStripMenuItem14,
            this.keysClearBeep,
            this.keysDefaultBeep});
            this.keysToolStripMenuItem.Name = "keysToolStripMenuItem";
            this.keysToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.keysToolStripMenuItem.Text = "&Keys";
            // 
            // keyNewKey
            // 
            this.keyNewKey.Image = global::KB9Utility.Properties.Resources.button32;
            this.keyNewKey.Name = "keyNewKey";
            this.keyNewKey.Size = new System.Drawing.Size(299, 22);
            this.keyNewKey.Text = "Add New K&ey";
            this.keyNewKey.Click += new System.EventHandler(this.keyNewKey_Click);
            // 
            // keysNewSlideV
            // 
            this.keysNewSlideV.Image = global::KB9Utility.Properties.Resources.slidev32;
            this.keysNewSlideV.Name = "keysNewSlideV";
            this.keysNewSlideV.Size = new System.Drawing.Size(299, 22);
            this.keysNewSlideV.Text = "Add New Slide &V";
            this.keysNewSlideV.Click += new System.EventHandler(this.keysNewSlideV_Click);
            // 
            // keysNewSlideH
            // 
            this.keysNewSlideH.Image = global::KB9Utility.Properties.Resources.slideh32;
            this.keysNewSlideH.Name = "keysNewSlideH";
            this.keysNewSlideH.Size = new System.Drawing.Size(299, 22);
            this.keysNewSlideH.Text = "Add New Slide &H";
            this.keysNewSlideH.Click += new System.EventHandler(this.keysNewSlideH_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(296, 6);
            // 
            // keysNewMatrix
            // 
            this.keysNewMatrix.Name = "keysNewMatrix";
            this.keysNewMatrix.Size = new System.Drawing.Size(299, 22);
            this.keysNewMatrix.Text = "Add New Keys Matrix";
            this.keysNewMatrix.Click += new System.EventHandler(this.keysNewMatrix_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(296, 6);
            // 
            // keysClearBeep
            // 
            this.keysClearBeep.Name = "keysClearBeep";
            this.keysClearBeep.Size = new System.Drawing.Size(299, 22);
            this.keysClearBeep.Text = "&Clear Beep Duration For All Keys";
            this.keysClearBeep.Click += new System.EventHandler(this.keysClearBeep_Click);
            // 
            // keysDefaultBeep
            // 
            this.keysDefaultBeep.Name = "keysDefaultBeep";
            this.keysDefaultBeep.Size = new System.Drawing.Size(299, 22);
            this.keysDefaultBeep.Text = "&Set Default Beep Duration For All Keys";
            this.keysDefaultBeep.Click += new System.EventHandler(this.keysDefaultBeep_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCopy,
            this.editCut,
            this.editPaste,
            this.editDel,
            this.toolStripMenuItem2,
            this.editUndo,
            this.editRedo,
            this.toolStripMenuItem3,
            this.editSelectAll,
            this.editUnselect,
            this.toolStripMenuItem4,
            this.editMacro});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(42, 21);
            this.menuEdit.Text = "&Edit";
            this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // editCopy
            // 
            this.editCopy.Image = global::KB9Utility.Properties.Resources.copy;
            this.editCopy.Name = "editCopy";
            this.editCopy.Size = new System.Drawing.Size(140, 22);
            this.editCopy.Text = "&Copy";
            this.editCopy.Click += new System.EventHandler(this.editCopy_Click);
            // 
            // editCut
            // 
            this.editCut.Image = global::KB9Utility.Properties.Resources.cut;
            this.editCut.Name = "editCut";
            this.editCut.Size = new System.Drawing.Size(140, 22);
            this.editCut.Text = "Cu&t";
            this.editCut.Click += new System.EventHandler(this.editCut_Click);
            // 
            // editPaste
            // 
            this.editPaste.Image = global::KB9Utility.Properties.Resources.paste;
            this.editPaste.Name = "editPaste";
            this.editPaste.Size = new System.Drawing.Size(140, 22);
            this.editPaste.Text = "&Paste";
            this.editPaste.Click += new System.EventHandler(this.editPaste_Click);
            // 
            // editDel
            // 
            this.editDel.Image = global::KB9Utility.Properties.Resources.del;
            this.editDel.Name = "editDel";
            this.editDel.Size = new System.Drawing.Size(140, 22);
            this.editDel.Text = "&Delete";
            this.editDel.Click += new System.EventHandler(this.editDel_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(137, 6);
            // 
            // editUndo
            // 
            this.editUndo.Image = global::KB9Utility.Properties.Resources.undo;
            this.editUndo.Name = "editUndo";
            this.editUndo.Size = new System.Drawing.Size(140, 22);
            this.editUndo.Text = "&Undo";
            this.editUndo.Click += new System.EventHandler(this.editUndo_Click);
            // 
            // editRedo
            // 
            this.editRedo.Image = global::KB9Utility.Properties.Resources.redo;
            this.editRedo.Name = "editRedo";
            this.editRedo.Size = new System.Drawing.Size(140, 22);
            this.editRedo.Text = "&Redo";
            this.editRedo.Click += new System.EventHandler(this.editRedo_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(137, 6);
            // 
            // editSelectAll
            // 
            this.editSelectAll.Image = global::KB9Utility.Properties.Resources.selectall;
            this.editSelectAll.Name = "editSelectAll";
            this.editSelectAll.Size = new System.Drawing.Size(140, 22);
            this.editSelectAll.Text = "Select &all";
            this.editSelectAll.Click += new System.EventHandler(this.editSelectAll_Click);
            // 
            // editUnselect
            // 
            this.editUnselect.Image = global::KB9Utility.Properties.Resources.unselect;
            this.editUnselect.Name = "editUnselect";
            this.editUnselect.Size = new System.Drawing.Size(140, 22);
            this.editUnselect.Text = "U&nselect";
            this.editUnselect.Click += new System.EventHandler(this.editUnselect_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(137, 6);
            // 
            // editMacro
            // 
            this.editMacro.Image = global::KB9Utility.Properties.Resources.macro;
            this.editMacro.Name = "editMacro";
            this.editMacro.Size = new System.Drawing.Size(140, 22);
            this.editMacro.Text = "Edit &Macro";
            this.editMacro.Click += new System.EventHandler(this.editMacro_Click);
            // 
            // menuFormat
            // 
            this.menuFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatAlign,
            this.formatMakeSame,
            this.toolStripMenuItem7,
            this.formatHEqual,
            this.formatVEqual});
            this.menuFormat.Name = "menuFormat";
            this.menuFormat.Size = new System.Drawing.Size(49, 21);
            this.menuFormat.Text = "&Align";
            // 
            // formatAlign
            // 
            this.formatAlign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatAlignLeft,
            this.formatAlignCenter,
            this.formatAlignRight,
            this.toolStripMenuItem6,
            this.formatAlignTop,
            this.formatAlignMiddle,
            this.formatAlignBottom});
            this.formatAlign.Name = "formatAlign";
            this.formatAlign.Size = new System.Drawing.Size(222, 22);
            this.formatAlign.Text = "&Align";
            // 
            // formatAlignLeft
            // 
            this.formatAlignLeft.Image = global::KB9Utility.Properties.Resources.alignleft;
            this.formatAlignLeft.Name = "formatAlignLeft";
            this.formatAlignLeft.Size = new System.Drawing.Size(125, 22);
            this.formatAlignLeft.Text = "&Lefts";
            this.formatAlignLeft.Click += new System.EventHandler(this.formatAlignLeft_Click);
            // 
            // formatAlignCenter
            // 
            this.formatAlignCenter.Image = global::KB9Utility.Properties.Resources.aligncenter;
            this.formatAlignCenter.Name = "formatAlignCenter";
            this.formatAlignCenter.Size = new System.Drawing.Size(125, 22);
            this.formatAlignCenter.Text = "&Centers";
            this.formatAlignCenter.Click += new System.EventHandler(this.formatAlignCenters_Click);
            // 
            // formatAlignRight
            // 
            this.formatAlignRight.Image = global::KB9Utility.Properties.Resources.alignright;
            this.formatAlignRight.Name = "formatAlignRight";
            this.formatAlignRight.Size = new System.Drawing.Size(125, 22);
            this.formatAlignRight.Text = "&Rights";
            this.formatAlignRight.Click += new System.EventHandler(this.formatAlignRight_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(122, 6);
            // 
            // formatAlignTop
            // 
            this.formatAlignTop.Image = global::KB9Utility.Properties.Resources.aligntop;
            this.formatAlignTop.Name = "formatAlignTop";
            this.formatAlignTop.Size = new System.Drawing.Size(125, 22);
            this.formatAlignTop.Text = "&Tops";
            this.formatAlignTop.Click += new System.EventHandler(this.formatAlignTop_Click);
            // 
            // formatAlignMiddle
            // 
            this.formatAlignMiddle.Image = global::KB9Utility.Properties.Resources.alignmiddle;
            this.formatAlignMiddle.Name = "formatAlignMiddle";
            this.formatAlignMiddle.Size = new System.Drawing.Size(125, 22);
            this.formatAlignMiddle.Text = "&Middles";
            this.formatAlignMiddle.Click += new System.EventHandler(this.formatAlignMiddles_Click);
            // 
            // formatAlignBottom
            // 
            this.formatAlignBottom.Image = global::KB9Utility.Properties.Resources.alignbottom;
            this.formatAlignBottom.Name = "formatAlignBottom";
            this.formatAlignBottom.Size = new System.Drawing.Size(125, 22);
            this.formatAlignBottom.Text = "&Bottoms";
            this.formatAlignBottom.Click += new System.EventHandler(this.formatAlignBottom_Click);
            // 
            // formatMakeSame
            // 
            this.formatMakeSame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatSameWidth,
            this.formatSameHeight,
            this.formatSameBoth});
            this.formatMakeSame.Name = "formatMakeSame";
            this.formatMakeSame.Size = new System.Drawing.Size(222, 22);
            this.formatMakeSame.Text = "&Make Same Size";
            this.formatMakeSame.Click += new System.EventHandler(this.formatMakeSame_Click);
            // 
            // formatSameWidth
            // 
            this.formatSameWidth.Image = global::KB9Utility.Properties.Resources.samewidth;
            this.formatSameWidth.Name = "formatSameWidth";
            this.formatSameWidth.Size = new System.Drawing.Size(114, 22);
            this.formatSameWidth.Text = "&Width";
            this.formatSameWidth.Click += new System.EventHandler(this.formatMakeSameWidth_Click);
            // 
            // formatSameHeight
            // 
            this.formatSameHeight.Image = global::KB9Utility.Properties.Resources.sameheight;
            this.formatSameHeight.Name = "formatSameHeight";
            this.formatSameHeight.Size = new System.Drawing.Size(114, 22);
            this.formatSameHeight.Text = "&Height";
            this.formatSameHeight.Click += new System.EventHandler(this.formatMakeSameHeight_Click);
            // 
            // formatSameBoth
            // 
            this.formatSameBoth.Image = global::KB9Utility.Properties.Resources.sameboth;
            this.formatSameBoth.Name = "formatSameBoth";
            this.formatSameBoth.Size = new System.Drawing.Size(114, 22);
            this.formatSameBoth.Text = "&Both";
            this.formatSameBoth.Click += new System.EventHandler(this.formatMakeSameBoth_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(219, 6);
            // 
            // formatHEqual
            // 
            this.formatHEqual.Name = "formatHEqual";
            this.formatHEqual.Size = new System.Drawing.Size(222, 22);
            this.formatHEqual.Text = "&Horizontal Spacing Equal";
            this.formatHEqual.Click += new System.EventHandler(this.formatHEqual_Click);
            // 
            // formatVEqual
            // 
            this.formatVEqual.Name = "formatVEqual";
            this.formatVEqual.Size = new System.Drawing.Size(222, 22);
            this.formatVEqual.Text = "&Vertical Spacing Equal";
            this.formatVEqual.Click += new System.EventHandler(this.formatVEqual_Click);
            // 
            // menuContents
            // 
            this.menuContents.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsCodePause,
            this.contentsCodeCombination,
            this.contentsCodeRepeat,
            this.contentsInsertMacro,
            this.toolStripMenuItem9,
            this.contentsCodeList});
            this.menuContents.Name = "menuContents";
            this.menuContents.Size = new System.Drawing.Size(71, 21);
            this.menuContents.Text = "&Contents";
            // 
            // contentsCodePause
            // 
            this.contentsCodePause.Image = global::KB9Utility.Properties.Resources.delay;
            this.contentsCodePause.Name = "contentsCodePause";
            this.contentsCodePause.Size = new System.Drawing.Size(212, 22);
            this.contentsCodePause.Text = "Insert &Pause";
            this.contentsCodePause.Click += new System.EventHandler(this.contentsCodePause_Click);
            // 
            // contentsCodeCombination
            // 
            this.contentsCodeCombination.Image = global::KB9Utility.Properties.Resources.combination;
            this.contentsCodeCombination.Name = "contentsCodeCombination";
            this.contentsCodeCombination.Size = new System.Drawing.Size(212, 22);
            this.contentsCodeCombination.Text = "Insert Key &Combination";
            this.contentsCodeCombination.Click += new System.EventHandler(this.contentsCodeCombination_Click);
            // 
            // contentsCodeRepeat
            // 
            this.contentsCodeRepeat.Image = global::KB9Utility.Properties.Resources.repeat;
            this.contentsCodeRepeat.Name = "contentsCodeRepeat";
            this.contentsCodeRepeat.Size = new System.Drawing.Size(212, 22);
            this.contentsCodeRepeat.Text = "Insert &Repeat";
            this.contentsCodeRepeat.Click += new System.EventHandler(this.contentsCodeRepeat_Click);
            // 
            // contentsInsertMacro
            // 
            this.contentsInsertMacro.Image = global::KB9Utility.Properties.Resources.macro;
            this.contentsInsertMacro.Name = "contentsInsertMacro";
            this.contentsInsertMacro.Size = new System.Drawing.Size(212, 22);
            this.contentsInsertMacro.Text = "Insert &Macro";
            this.contentsInsertMacro.Click += new System.EventHandler(this.contentsInsertMacro_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(209, 6);
            // 
            // contentsCodeList
            // 
            this.contentsCodeList.Image = global::KB9Utility.Properties.Resources.specialkey;
            this.contentsCodeList.Name = "contentsCodeList";
            this.contentsCodeList.Size = new System.Drawing.Size(212, 22);
            this.contentsCodeList.Text = "&On-screen keyboard";
            this.contentsCodeList.Click += new System.EventHandler(this.contentsCodeList_Click);
            // 
            // menuKB9000
            // 
            this.menuKB9000.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kb9000Detect,
            this.kb9000Write,
            this.kb9000Read,
            this.toolStripMenuItem5,
            this.kb9000Test,
            this.kb9000TestDataStruture});
            this.menuKB9000.Name = "menuKB9000";
            this.menuKB9000.Size = new System.Drawing.Size(62, 21);
            this.menuKB9000.Text = "&Actions";
            // 
            // kb9000Detect
            // 
            this.kb9000Detect.Image = global::KB9Utility.Properties.Resources.detect32;
            this.kb9000Detect.Name = "kb9000Detect";
            this.kb9000Detect.Size = new System.Drawing.Size(192, 22);
            this.kb9000Detect.Text = "&Detect KB9000";
            this.kb9000Detect.Click += new System.EventHandler(this.kb9000Detect_Click);
            // 
            // kb9000Write
            // 
            this.kb9000Write.Image = global::KB9Utility.Properties.Resources.write32;
            this.kb9000Write.Name = "kb9000Write";
            this.kb9000Write.Size = new System.Drawing.Size(192, 22);
            this.kb9000Write.Text = "&Write to KB9000";
            this.kb9000Write.Click += new System.EventHandler(this.kb9000Write_Click);
            // 
            // kb9000Read
            // 
            this.kb9000Read.Image = global::KB9Utility.Properties.Resources.read32;
            this.kb9000Read.Name = "kb9000Read";
            this.kb9000Read.Size = new System.Drawing.Size(192, 22);
            this.kb9000Read.Text = "&Read from KB9000";
            this.kb9000Read.Click += new System.EventHandler(this.kb9000Read_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(189, 6);
            // 
            // kb9000Test
            // 
            this.kb9000Test.Image = global::KB9Utility.Properties.Resources.testkb32;
            this.kb9000Test.Name = "kb9000Test";
            this.kb9000Test.Size = new System.Drawing.Size(192, 22);
            this.kb9000Test.Text = "&Test KB9000 Output";
            this.kb9000Test.Click += new System.EventHandler(this.kb9000Test_Click);
            // 
            // kb9000TestDataStruture
            // 
            this.kb9000TestDataStruture.Name = "kb9000TestDataStruture";
            this.kb9000TestDataStruture.Size = new System.Drawing.Size(192, 22);
            this.kb9000TestDataStruture.Text = "Test Data Structure";
            this.kb9000TestDataStruture.Visible = false;
            this.kb9000TestDataStruture.Click += new System.EventHandler(this.kb9000TestDataStruture_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpLog,
            this.helpClearLog,
            this.toolStripMenuItem13,
            this.helpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(47, 21);
            this.menuHelp.Text = "&Help";
            // 
            // helpLog
            // 
            this.helpLog.CheckOnClick = true;
            this.helpLog.Name = "helpLog";
            this.helpLog.Size = new System.Drawing.Size(155, 22);
            this.helpLog.Text = "Enable Log";
            this.helpLog.Click += new System.EventHandler(this.helpLog_Click);
            // 
            // helpClearLog
            // 
            this.helpClearLog.Name = "helpClearLog";
            this.helpClearLog.Size = new System.Drawing.Size(155, 22);
            this.helpClearLog.Text = "Clear Log File";
            this.helpClearLog.Click += new System.EventHandler(this.helpClearLog_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(152, 6);
            // 
            // helpAbout
            // 
            this.helpAbout.Name = "helpAbout";
            this.helpAbout.Size = new System.Drawing.Size(155, 22);
            this.helpAbout.Text = "&About";
            this.helpAbout.Click += new System.EventHandler(this.helpAbout_Click);
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsFreeMem,
            this.toolStripStatusLabel2,
            this.tsMsg0,
            this.tsMsg,
            this.tsMsg2,
            this.tsMsg3});
            this.statusMain.Location = new System.Drawing.Point(0, 509);
            this.statusMain.Name = "statusMain";
            this.statusMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusMain.Size = new System.Drawing.Size(948, 22);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "statusStrip1";
            // 
            // tsFreeMem
            // 
            this.tsFreeMem.Name = "tsFreeMem";
            this.tsFreeMem.Size = new System.Drawing.Size(74, 17);
            this.tsFreeMem.Text = "Free space:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(20, 17);
            this.toolStripStatusLabel2.Text = "   ";
            // 
            // tsMsg0
            // 
            this.tsMsg0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.tsMsg0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tsMsg0.LinkColor = System.Drawing.SystemColors.ControlText;
            this.tsMsg0.Name = "tsMsg0";
            this.tsMsg0.Size = new System.Drawing.Size(93, 17);
            this.tsMsg0.Text = "Message0";
            // 
            // tsMsg
            // 
            this.tsMsg.Name = "tsMsg";
            this.tsMsg.Size = new System.Drawing.Size(61, 17);
            this.tsMsg.Text = "Message";
            // 
            // tsMsg2
            // 
            this.tsMsg2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.tsMsg2.Name = "tsMsg2";
            this.tsMsg2.Size = new System.Drawing.Size(88, 17);
            this.tsMsg2.Text = "Message2";
            // 
            // tsMsg3
            // 
            this.tsMsg3.Name = "tsMsg3";
            this.tsMsg3.Size = new System.Drawing.Size(68, 17);
            this.tsMsg3.Text = "Message3";
            // 
            // tkbZoom
            // 
            this.tkbZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tkbZoom.AutoSize = false;
            this.tkbZoom.Location = new System.Drawing.Point(683, 512);
            this.tkbZoom.Maximum = 100;
            this.tkbZoom.Minimum = 10;
            this.tkbZoom.Name = "tkbZoom";
            this.tkbZoom.Size = new System.Drawing.Size(176, 18);
            this.tkbZoom.TabIndex = 4;
            this.tkbZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tkbZoom.Value = 10;
            this.tkbZoom.ValueChanged += new System.EventHandler(this.tkbZoom_ValueChanged);
            // 
            // lblPercent
            // 
            this.lblPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(619, 512);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(40, 14);
            this.lblPercent.TabIndex = 7;
            this.lblPercent.Text = "100%";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(50, 24);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 24);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.splitContainer3.Panel1.Controls.Add(this.panel2);
            this.splitContainer3.Panel1.Controls.Add(this.panelButton);
            this.splitContainer3.Panel1.Controls.Add(this.panelTools);
            this.splitContainer3.Panel1.Controls.Add(this.pgProperties);
            this.splitContainer3.Panel1MinSize = 170;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(948, 482);
            this.splitContainer3.SplitterDistance = 174;
            this.splitContainer3.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.tabCodes);
            this.panel2.Location = new System.Drawing.Point(176, 88);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(518, 81);
            this.panel2.TabIndex = 9;
            // 
            // tabCodes
            // 
            this.tabCodes.Controls.Add(this.tpLabel);
            this.tabCodes.Controls.Add(this.tpCode0);
            this.tabCodes.Controls.Add(this.tpCode1);
            this.tabCodes.Controls.Add(this.tpCode2);
            this.tabCodes.Controls.Add(this.tpCode3);
            this.tabCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCodes.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCodes.Location = new System.Drawing.Point(0, 0);
            this.tabCodes.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.tabCodes.Name = "tabCodes";
            this.tabCodes.Padding = new System.Drawing.Point(3, 3);
            this.tabCodes.SelectedIndex = 0;
            this.tabCodes.Size = new System.Drawing.Size(514, 77);
            this.tabCodes.TabIndex = 0;
            this.tabCodes.SelectedIndexChanged += new System.EventHandler(this.tabCodes_SelectedIndexChanged);
            this.tabCodes.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabCodes_Selecting);
            this.tabCodes.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabCodes_Selected);
            this.tabCodes.Click += new System.EventHandler(this.tabCodes_Click);
            // 
            // tpLabel
            // 
            this.tpLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.tpLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.tpLabel.Location = new System.Drawing.Point(4, 24);
            this.tpLabel.Name = "tpLabel";
            this.tpLabel.Size = new System.Drawing.Size(506, 49);
            this.tpLabel.TabIndex = 4;
            this.tpLabel.Text = "Label";
            // 
            // tpCode0
            // 
            this.tpCode0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.tpCode0.Controls.Add(this.lblKeyCode0);
            this.tpCode0.Location = new System.Drawing.Point(4, 24);
            this.tpCode0.Name = "tpCode0";
            this.tpCode0.Padding = new System.Windows.Forms.Padding(3);
            this.tpCode0.Size = new System.Drawing.Size(506, 49);
            this.tpCode0.TabIndex = 0;
            this.tpCode0.Text = "Up";
            // 
            // lblKeyCode0
            // 
            this.lblKeyCode0.AllowDrop = true;
            this.lblKeyCode0.BackColor = System.Drawing.Color.White;
            this.lblKeyCode0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKeyCode0.Caret = -1;
            this.lblKeyCode0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKeyCode0.EnableTextChangedEvent = true;
            this.lblKeyCode0.Font = new System.Drawing.Font("Arial", 11F);
            this.lblKeyCode0.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.lblKeyCode0.Location = new System.Drawing.Point(3, 3);
            this.lblKeyCode0.LowerKey = false;
            this.lblKeyCode0.Modified = false;
            this.lblKeyCode0.Name = "lblKeyCode0";
            this.lblKeyCode0.RestrictMacro = false;
            this.lblKeyCode0.ScrollBarVisible = true;
            this.lblKeyCode0.ShowCaret = false;
            this.lblKeyCode0.ShowGripper = true;
            this.lblKeyCode0.SingleKey = false;
            this.lblKeyCode0.Size = new System.Drawing.Size(500, 43);
            this.lblKeyCode0.TabIndex = 1;
            this.lblKeyCode0.OnTextContentChanged += new KB9Utility.KB9TextBox.EventOnTextChanged(this.lblKeyCode0_OnTextChanged);
            this.lblKeyCode0.OnSelectionChanged += new KB9Utility.KB9TextBox.EventOnSelectionChanged(this.lblKeyCode0_OnSelectionChanged);
            this.lblKeyCode0.Enter += new System.EventHandler(this.lblKeyCode0_Enter);
            this.lblKeyCode0.Leave += new System.EventHandler(this.lblKeyCode0_Leave);
            this.lblKeyCode0.Validating += new System.ComponentModel.CancelEventHandler(this.lblKeyCode0_Validating);
            // 
            // tpCode1
            // 
            this.tpCode1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.tpCode1.Controls.Add(this.lblKeyCode1);
            this.tpCode1.Location = new System.Drawing.Point(4, 24);
            this.tpCode1.Name = "tpCode1";
            this.tpCode1.Padding = new System.Windows.Forms.Padding(3);
            this.tpCode1.Size = new System.Drawing.Size(506, 49);
            this.tpCode1.TabIndex = 1;
            this.tpCode1.Text = "Up Hold";
            // 
            // lblKeyCode1
            // 
            this.lblKeyCode1.AllowDrop = true;
            this.lblKeyCode1.BackColor = System.Drawing.Color.White;
            this.lblKeyCode1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKeyCode1.Caret = -1;
            this.lblKeyCode1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKeyCode1.EnableTextChangedEvent = true;
            this.lblKeyCode1.Font = new System.Drawing.Font("Arial", 11F);
            this.lblKeyCode1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.lblKeyCode1.Location = new System.Drawing.Point(3, 3);
            this.lblKeyCode1.LowerKey = false;
            this.lblKeyCode1.Modified = false;
            this.lblKeyCode1.Name = "lblKeyCode1";
            this.lblKeyCode1.RestrictMacro = false;
            this.lblKeyCode1.ScrollBarVisible = true;
            this.lblKeyCode1.ShowCaret = false;
            this.lblKeyCode1.ShowGripper = true;
            this.lblKeyCode1.SingleKey = false;
            this.lblKeyCode1.Size = new System.Drawing.Size(500, 43);
            this.lblKeyCode1.TabIndex = 2;
            this.lblKeyCode1.OnTextContentChanged += new KB9Utility.KB9TextBox.EventOnTextChanged(this.lblKeyCode0_OnTextChanged);
            this.lblKeyCode1.OnSelectionChanged += new KB9Utility.KB9TextBox.EventOnSelectionChanged(this.lblKeyCode0_OnSelectionChanged);
            this.lblKeyCode1.Enter += new System.EventHandler(this.lblKeyCode0_Enter);
            this.lblKeyCode1.Leave += new System.EventHandler(this.lblKeyCode0_Leave);
            this.lblKeyCode1.Validating += new System.ComponentModel.CancelEventHandler(this.lblKeyCode0_Validating);
            // 
            // tpCode2
            // 
            this.tpCode2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.tpCode2.Controls.Add(this.lblKeyCode2);
            this.tpCode2.Location = new System.Drawing.Point(4, 24);
            this.tpCode2.Name = "tpCode2";
            this.tpCode2.Padding = new System.Windows.Forms.Padding(3);
            this.tpCode2.Size = new System.Drawing.Size(506, 49);
            this.tpCode2.TabIndex = 2;
            this.tpCode2.Text = "Down";
            // 
            // lblKeyCode2
            // 
            this.lblKeyCode2.AllowDrop = true;
            this.lblKeyCode2.BackColor = System.Drawing.Color.White;
            this.lblKeyCode2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKeyCode2.Caret = -1;
            this.lblKeyCode2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKeyCode2.EnableTextChangedEvent = true;
            this.lblKeyCode2.Font = new System.Drawing.Font("Arial", 11F);
            this.lblKeyCode2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.lblKeyCode2.Location = new System.Drawing.Point(3, 3);
            this.lblKeyCode2.LowerKey = false;
            this.lblKeyCode2.Modified = false;
            this.lblKeyCode2.Name = "lblKeyCode2";
            this.lblKeyCode2.RestrictMacro = false;
            this.lblKeyCode2.ScrollBarVisible = true;
            this.lblKeyCode2.ShowCaret = false;
            this.lblKeyCode2.ShowGripper = true;
            this.lblKeyCode2.SingleKey = false;
            this.lblKeyCode2.Size = new System.Drawing.Size(500, 43);
            this.lblKeyCode2.TabIndex = 3;
            this.lblKeyCode2.OnTextContentChanged += new KB9Utility.KB9TextBox.EventOnTextChanged(this.lblKeyCode0_OnTextChanged);
            this.lblKeyCode2.OnSelectionChanged += new KB9Utility.KB9TextBox.EventOnSelectionChanged(this.lblKeyCode0_OnSelectionChanged);
            this.lblKeyCode2.Enter += new System.EventHandler(this.lblKeyCode0_Enter);
            this.lblKeyCode2.Leave += new System.EventHandler(this.lblKeyCode0_Leave);
            this.lblKeyCode2.Validating += new System.ComponentModel.CancelEventHandler(this.lblKeyCode0_Validating);
            // 
            // tpCode3
            // 
            this.tpCode3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.tpCode3.Controls.Add(this.lblKeyCode3);
            this.tpCode3.Location = new System.Drawing.Point(4, 24);
            this.tpCode3.Name = "tpCode3";
            this.tpCode3.Padding = new System.Windows.Forms.Padding(3);
            this.tpCode3.Size = new System.Drawing.Size(506, 49);
            this.tpCode3.TabIndex = 3;
            this.tpCode3.Text = "Down Hold";
            // 
            // lblKeyCode3
            // 
            this.lblKeyCode3.AllowDrop = true;
            this.lblKeyCode3.BackColor = System.Drawing.Color.White;
            this.lblKeyCode3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKeyCode3.Caret = -1;
            this.lblKeyCode3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKeyCode3.EnableTextChangedEvent = true;
            this.lblKeyCode3.Font = new System.Drawing.Font("Arial", 11F);
            this.lblKeyCode3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.lblKeyCode3.Location = new System.Drawing.Point(3, 3);
            this.lblKeyCode3.LowerKey = false;
            this.lblKeyCode3.Modified = false;
            this.lblKeyCode3.Name = "lblKeyCode3";
            this.lblKeyCode3.RestrictMacro = false;
            this.lblKeyCode3.ScrollBarVisible = true;
            this.lblKeyCode3.ShowCaret = false;
            this.lblKeyCode3.ShowGripper = true;
            this.lblKeyCode3.SingleKey = false;
            this.lblKeyCode3.Size = new System.Drawing.Size(500, 43);
            this.lblKeyCode3.TabIndex = 4;
            this.lblKeyCode3.OnTextContentChanged += new KB9Utility.KB9TextBox.EventOnTextChanged(this.lblKeyCode0_OnTextChanged);
            this.lblKeyCode3.OnSelectionChanged += new KB9Utility.KB9TextBox.EventOnSelectionChanged(this.lblKeyCode0_OnSelectionChanged);
            this.lblKeyCode3.Enter += new System.EventHandler(this.lblKeyCode0_Enter);
            this.lblKeyCode3.Leave += new System.EventHandler(this.lblKeyCode0_Leave);
            this.lblKeyCode3.Validating += new System.ComponentModel.CancelEventHandler(this.lblKeyCode0_Validating);
            // 
            // panelButton
            // 
            this.panelButton.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelButton.BaseColorOn = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelButton.Caption = "Add New Key";
            this.panelButton.Controls.Add(this.btnSlideH);
            this.panelButton.Controls.Add(this.btnSlideV);
            this.panelButton.Controls.Add(this.btnKeyArea);
            this.panelButton.ForeColor = System.Drawing.Color.SteelBlue;
            this.panelButton.Location = new System.Drawing.Point(2, 88);
            this.panelButton.Name = "panelButton";
            this.panelButton.Opacity = 255;
            this.panelButton.Size = new System.Drawing.Size(169, 81);
            this.panelButton.TabIndex = 3;
            // 
            // btnSlideH
            // 
            this.btnSlideH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSlideH.BackColor = System.Drawing.Color.Transparent;
            this.btnSlideH.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSlideH.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSlideH.HottrackImage = null;
            this.btnSlideH.Location = new System.Drawing.Point(105, 7);
            this.btnSlideH.Margin = new System.Windows.Forms.Padding(0);
            this.btnSlideH.Name = "btnSlideH";
            this.btnSlideH.NormalImage = global::KB9Utility.Properties.Resources.slideh32;
            this.btnSlideH.OnlyShowBitmap = false;
            this.btnSlideH.PressedImage = null;
            this.btnSlideH.Size = new System.Drawing.Size(51, 55);
            this.btnSlideH.TabIndex = 2;
            this.btnSlideH.TabStop = false;
            this.btnSlideH.Text = "Slide H";
            this.btnSlideH.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnSlideH, "New horizontal slide key");
            this.btnSlideH.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSlideH, "New horizontal slide key");
            this.btnSlideH.Click += new System.EventHandler(this.btnSlideH_Click);
            // 
            // btnSlideV
            // 
            this.btnSlideV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSlideV.BackColor = System.Drawing.Color.Transparent;
            this.btnSlideV.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSlideV.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSlideV.HottrackImage = null;
            this.btnSlideV.Location = new System.Drawing.Point(53, 7);
            this.btnSlideV.Margin = new System.Windows.Forms.Padding(0);
            this.btnSlideV.Name = "btnSlideV";
            this.btnSlideV.NormalImage = global::KB9Utility.Properties.Resources.slidev32;
            this.btnSlideV.OnlyShowBitmap = false;
            this.btnSlideV.PressedImage = null;
            this.btnSlideV.Size = new System.Drawing.Size(51, 55);
            this.btnSlideV.TabIndex = 1;
            this.btnSlideV.TabStop = false;
            this.btnSlideV.Text = "Slide V";
            this.btnSlideV.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnSlideV, "New vertical slide key");
            this.btnSlideV.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSlideV, "New vertical slide key");
            this.btnSlideV.Click += new System.EventHandler(this.btnSlideV_Click);
            // 
            // btnKeyArea
            // 
            this.btnKeyArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKeyArea.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyArea.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnKeyArea.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnKeyArea.HottrackImage = null;
            this.btnKeyArea.Location = new System.Drawing.Point(1, 7);
            this.btnKeyArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnKeyArea.Name = "btnKeyArea";
            this.btnKeyArea.NormalImage = global::KB9Utility.Properties.Resources.button32;
            this.btnKeyArea.OnlyShowBitmap = false;
            this.btnKeyArea.PressedImage = null;
            this.btnKeyArea.Size = new System.Drawing.Size(51, 55);
            this.btnKeyArea.TabIndex = 0;
            this.btnKeyArea.TabStop = false;
            this.btnKeyArea.Text = "Key";
            this.btnKeyArea.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnKeyArea, "New Key");
            this.btnKeyArea.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnKeyArea, "New Key");
            this.btnKeyArea.Click += new System.EventHandler(this.btnKeyArea_Click);
            // 
            // panelTools
            // 
            this.panelTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.panelTools.BarHeight = 18;
            this.panelTools.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.panelTools.BottomBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(216)))), ((int)(((byte)(240)))));
            this.panelTools.ColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(217)))), ((int)(((byte)(237)))));
            this.panelTools.ColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.panelTools.Controls.Add(this.panelActions);
            this.panelTools.Controls.Add(this.panelKeyEdit);
            this.panelTools.Controls.Add(this.panelFormat);
            this.panelTools.Controls.Add(this.panelEdit);
            this.panelTools.Controls.Add(this.panelFile);
            this.panelTools.Location = new System.Drawing.Point(2, 2);
            this.panelTools.Name = "panelTools";
            this.panelTools.Size = new System.Drawing.Size(695, 84);
            this.panelTools.TabIndex = 8;
            this.panelTools.Title = "";
            this.panelTools.TopBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            // 
            // panelActions
            // 
            this.panelActions.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelActions.BaseColorOn = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelActions.Caption = "Actions";
            this.panelActions.Controls.Add(this.btnActionsTest);
            this.panelActions.Controls.Add(this.btnActionsWrite);
            this.panelActions.Controls.Add(this.btnActionsRead);
            this.panelActions.Controls.Add(this.btnActionsDetect);
            this.panelActions.ForeColor = System.Drawing.Color.SteelBlue;
            this.panelActions.Location = new System.Drawing.Point(510, 2);
            this.panelActions.Name = "panelActions";
            this.panelActions.Opacity = 255;
            this.panelActions.Size = new System.Drawing.Size(182, 81);
            this.panelActions.TabIndex = 5;
            // 
            // btnActionsTest
            // 
            this.btnActionsTest.BackColor = System.Drawing.Color.Transparent;
            this.btnActionsTest.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnActionsTest.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnActionsTest.HottrackImage = null;
            this.btnActionsTest.Location = new System.Drawing.Point(138, 10);
            this.btnActionsTest.Name = "btnActionsTest";
            this.btnActionsTest.NormalImage = global::KB9Utility.Properties.Resources.testkb32;
            this.btnActionsTest.OnlyShowBitmap = false;
            this.btnActionsTest.PressedImage = null;
            this.btnActionsTest.Size = new System.Drawing.Size(40, 55);
            this.btnActionsTest.TabIndex = 3;
            this.btnActionsTest.TabStop = false;
            this.btnActionsTest.Text = "Test";
            this.btnActionsTest.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnActionsTest, "Test KB9000 output");
            this.btnActionsTest.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnActionsTest, "Test KB9000 output");
            this.btnActionsTest.Click += new System.EventHandler(this.btnActionsTest_Click);
            // 
            // btnActionsWrite
            // 
            this.btnActionsWrite.BackColor = System.Drawing.Color.Transparent;
            this.btnActionsWrite.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnActionsWrite.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnActionsWrite.HottrackImage = null;
            this.btnActionsWrite.Location = new System.Drawing.Point(93, 10);
            this.btnActionsWrite.Name = "btnActionsWrite";
            this.btnActionsWrite.NormalImage = global::KB9Utility.Properties.Resources.write32;
            this.btnActionsWrite.OnlyShowBitmap = false;
            this.btnActionsWrite.PressedImage = null;
            this.btnActionsWrite.Size = new System.Drawing.Size(40, 55);
            this.btnActionsWrite.TabIndex = 2;
            this.btnActionsWrite.TabStop = false;
            this.btnActionsWrite.Text = "Write";
            this.btnActionsWrite.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnActionsWrite, "Write to KB9000");
            this.btnActionsWrite.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnActionsWrite, "Write to KB9000");
            this.btnActionsWrite.Click += new System.EventHandler(this.btnActionsWrite_Click);
            // 
            // btnActionsRead
            // 
            this.btnActionsRead.BackColor = System.Drawing.Color.Transparent;
            this.btnActionsRead.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnActionsRead.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnActionsRead.HottrackImage = null;
            this.btnActionsRead.Location = new System.Drawing.Point(48, 10);
            this.btnActionsRead.Name = "btnActionsRead";
            this.btnActionsRead.NormalImage = global::KB9Utility.Properties.Resources.read32;
            this.btnActionsRead.OnlyShowBitmap = false;
            this.btnActionsRead.PressedImage = null;
            this.btnActionsRead.Size = new System.Drawing.Size(40, 55);
            this.btnActionsRead.TabIndex = 1;
            this.btnActionsRead.TabStop = false;
            this.btnActionsRead.Text = "Read";
            this.btnActionsRead.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnActionsRead, "Read From KB9000");
            this.btnActionsRead.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnActionsRead, "Read From KB9000");
            this.btnActionsRead.Click += new System.EventHandler(this.btnActionsRead_Click);
            // 
            // btnActionsDetect
            // 
            this.btnActionsDetect.BackColor = System.Drawing.Color.Transparent;
            this.btnActionsDetect.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnActionsDetect.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnActionsDetect.HottrackImage = null;
            this.btnActionsDetect.Location = new System.Drawing.Point(3, 10);
            this.btnActionsDetect.Name = "btnActionsDetect";
            this.btnActionsDetect.NormalImage = global::KB9Utility.Properties.Resources.detect32;
            this.btnActionsDetect.OnlyShowBitmap = false;
            this.btnActionsDetect.PressedImage = null;
            this.btnActionsDetect.Size = new System.Drawing.Size(40, 55);
            this.btnActionsDetect.TabIndex = 0;
            this.btnActionsDetect.TabStop = false;
            this.btnActionsDetect.Text = "Detect";
            this.btnActionsDetect.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnActionsDetect, "Detect KB9000");
            this.btnActionsDetect.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnActionsDetect, "Detect KB9000");
            this.btnActionsDetect.Click += new System.EventHandler(this.btnActionsDetect_Click);
            // 
            // panelKeyEdit
            // 
            this.panelKeyEdit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelKeyEdit.BaseColorOn = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelKeyEdit.Caption = "Key Content";
            this.panelKeyEdit.Controls.Add(this.btnCodeMacro);
            this.panelKeyEdit.Controls.Add(this.btnCodeRepeat);
            this.panelKeyEdit.Controls.Add(this.btnCodeCombination);
            this.panelKeyEdit.Controls.Add(this.btnCodeList);
            this.panelKeyEdit.Controls.Add(this.btnCodePause);
            this.panelKeyEdit.ForeColor = System.Drawing.Color.SteelBlue;
            this.panelKeyEdit.Location = new System.Drawing.Point(409, 2);
            this.panelKeyEdit.Name = "panelKeyEdit";
            this.panelKeyEdit.Opacity = 255;
            this.panelKeyEdit.Size = new System.Drawing.Size(99, 81);
            this.panelKeyEdit.TabIndex = 4;
            // 
            // btnCodeMacro
            // 
            this.btnCodeMacro.BackColor = System.Drawing.Color.Transparent;
            this.btnCodeMacro.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnCodeMacro.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnCodeMacro.HottrackImage = null;
            this.btnCodeMacro.Location = new System.Drawing.Point(34, 41);
            this.btnCodeMacro.Name = "btnCodeMacro";
            this.btnCodeMacro.NormalImage = global::KB9Utility.Properties.Resources.macro;
            this.btnCodeMacro.OnlyShowBitmap = false;
            this.btnCodeMacro.PressedImage = null;
            this.btnCodeMacro.Size = new System.Drawing.Size(25, 22);
            this.btnCodeMacro.TabIndex = 6;
            this.btnCodeMacro.TabStop = false;
            this.btnCodeMacro.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnCodeMacro, "Edit Macro");
            this.btnCodeMacro.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnCodeMacro, "Edit Macro");
            this.btnCodeMacro.Click += new System.EventHandler(this.btnCodeMacro_Click);
            // 
            // btnCodeRepeat
            // 
            this.btnCodeRepeat.BackColor = System.Drawing.Color.Transparent;
            this.btnCodeRepeat.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnCodeRepeat.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnCodeRepeat.HottrackImage = null;
            this.btnCodeRepeat.Location = new System.Drawing.Point(3, 41);
            this.btnCodeRepeat.Name = "btnCodeRepeat";
            this.btnCodeRepeat.NormalImage = global::KB9Utility.Properties.Resources.repeat;
            this.btnCodeRepeat.OnlyShowBitmap = false;
            this.btnCodeRepeat.PressedImage = null;
            this.btnCodeRepeat.Size = new System.Drawing.Size(25, 22);
            this.btnCodeRepeat.TabIndex = 5;
            this.btnCodeRepeat.TabStop = false;
            this.btnCodeRepeat.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnCodeRepeat, "Insert Repeat");
            this.btnCodeRepeat.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnCodeRepeat, "Insert Repeat");
            this.btnCodeRepeat.Click += new System.EventHandler(this.btnCodeRepeat_Click);
            // 
            // btnCodeCombination
            // 
            this.btnCodeCombination.BackColor = System.Drawing.Color.Transparent;
            this.btnCodeCombination.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnCodeCombination.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnCodeCombination.HottrackImage = null;
            this.btnCodeCombination.Location = new System.Drawing.Point(3, 18);
            this.btnCodeCombination.Name = "btnCodeCombination";
            this.btnCodeCombination.NormalImage = global::KB9Utility.Properties.Resources.combination;
            this.btnCodeCombination.OnlyShowBitmap = false;
            this.btnCodeCombination.PressedImage = null;
            this.btnCodeCombination.Size = new System.Drawing.Size(25, 22);
            this.btnCodeCombination.TabIndex = 4;
            this.btnCodeCombination.TabStop = false;
            this.btnCodeCombination.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnCodeCombination, "Insert key combination");
            this.btnCodeCombination.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnCodeCombination, "Insert key combination");
            this.btnCodeCombination.Click += new System.EventHandler(this.btnCodeCombination_Click);
            // 
            // btnCodeList
            // 
            this.btnCodeList.BackColor = System.Drawing.Color.Transparent;
            this.btnCodeList.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnCodeList.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnCodeList.HottrackImage = null;
            this.btnCodeList.Location = new System.Drawing.Point(65, 18);
            this.btnCodeList.Name = "btnCodeList";
            this.btnCodeList.NormalImage = global::KB9Utility.Properties.Resources.specialkey;
            this.btnCodeList.OnlyShowBitmap = false;
            this.btnCodeList.PressedImage = null;
            this.btnCodeList.Size = new System.Drawing.Size(25, 22);
            this.btnCodeList.TabIndex = 3;
            this.btnCodeList.TabStop = false;
            this.btnCodeList.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnCodeList, " On-screen keyboard");
            this.btnCodeList.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnCodeList, " On-screen keyboard");
            this.btnCodeList.Click += new System.EventHandler(this.btnCodeList_Click);
            // 
            // btnCodePause
            // 
            this.btnCodePause.BackColor = System.Drawing.Color.Transparent;
            this.btnCodePause.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnCodePause.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnCodePause.HottrackImage = null;
            this.btnCodePause.Location = new System.Drawing.Point(34, 18);
            this.btnCodePause.Name = "btnCodePause";
            this.btnCodePause.NormalImage = global::KB9Utility.Properties.Resources.delay;
            this.btnCodePause.OnlyShowBitmap = false;
            this.btnCodePause.PressedImage = null;
            this.btnCodePause.Size = new System.Drawing.Size(25, 22);
            this.btnCodePause.TabIndex = 2;
            this.btnCodePause.TabStop = false;
            this.btnCodePause.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnCodePause, "Insert Pause");
            this.btnCodePause.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnCodePause, "Insert Pause");
            this.btnCodePause.Click += new System.EventHandler(this.btnCodePause_Click);
            // 
            // panelFormat
            // 
            this.panelFormat.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelFormat.BaseColorOn = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelFormat.Caption = "Align";
            this.panelFormat.Controls.Add(this.btnSameBoth);
            this.panelFormat.Controls.Add(this.btnAlignMiddle);
            this.panelFormat.Controls.Add(this.btnAlignTop);
            this.panelFormat.Controls.Add(this.btnSameHeight);
            this.panelFormat.Controls.Add(this.btnSameWidth);
            this.panelFormat.Controls.Add(this.btnAlignBottom);
            this.panelFormat.Controls.Add(this.btnAlignRight);
            this.panelFormat.Controls.Add(this.btnAlignCenter);
            this.panelFormat.Controls.Add(this.btnAlignLeft);
            this.panelFormat.ForeColor = System.Drawing.Color.SteelBlue;
            this.panelFormat.Location = new System.Drawing.Point(243, 2);
            this.panelFormat.Name = "panelFormat";
            this.panelFormat.Opacity = 255;
            this.panelFormat.Size = new System.Drawing.Size(164, 81);
            this.panelFormat.TabIndex = 2;
            // 
            // btnSameBoth
            // 
            this.btnSameBoth.BackColor = System.Drawing.Color.Transparent;
            this.btnSameBoth.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSameBoth.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSameBoth.HottrackImage = null;
            this.btnSameBoth.Location = new System.Drawing.Point(134, 18);
            this.btnSameBoth.Name = "btnSameBoth";
            this.btnSameBoth.NormalImage = global::KB9Utility.Properties.Resources.sameboth;
            this.btnSameBoth.OnlyShowBitmap = false;
            this.btnSameBoth.PressedImage = null;
            this.btnSameBoth.Size = new System.Drawing.Size(25, 22);
            this.btnSameBoth.TabIndex = 8;
            this.btnSameBoth.TabStop = false;
            this.btnSameBoth.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnSameBoth, "Same both");
            this.btnSameBoth.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSameBoth, "Same both");
            this.btnSameBoth.Click += new System.EventHandler(this.formatMakeSameBoth_Click);
            // 
            // btnAlignMiddle
            // 
            this.btnAlignMiddle.BackColor = System.Drawing.Color.Transparent;
            this.btnAlignMiddle.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnAlignMiddle.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnAlignMiddle.HottrackImage = null;
            this.btnAlignMiddle.Location = new System.Drawing.Point(41, 18);
            this.btnAlignMiddle.Name = "btnAlignMiddle";
            this.btnAlignMiddle.NormalImage = global::KB9Utility.Properties.Resources.alignmiddle;
            this.btnAlignMiddle.OnlyShowBitmap = false;
            this.btnAlignMiddle.PressedImage = null;
            this.btnAlignMiddle.Size = new System.Drawing.Size(25, 22);
            this.btnAlignMiddle.TabIndex = 7;
            this.btnAlignMiddle.TabStop = false;
            this.btnAlignMiddle.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnAlignMiddle, "Align middle");
            this.btnAlignMiddle.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnAlignMiddle, "Align middle");
            this.btnAlignMiddle.Click += new System.EventHandler(this.formatAlignMiddles_Click);
            // 
            // btnAlignTop
            // 
            this.btnAlignTop.BackColor = System.Drawing.Color.Transparent;
            this.btnAlignTop.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnAlignTop.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnAlignTop.HottrackImage = null;
            this.btnAlignTop.Location = new System.Drawing.Point(10, 17);
            this.btnAlignTop.Name = "btnAlignTop";
            this.btnAlignTop.NormalImage = global::KB9Utility.Properties.Resources.aligntop;
            this.btnAlignTop.OnlyShowBitmap = false;
            this.btnAlignTop.PressedImage = null;
            this.btnAlignTop.Size = new System.Drawing.Size(25, 22);
            this.btnAlignTop.TabIndex = 6;
            this.btnAlignTop.TabStop = false;
            this.btnAlignTop.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnAlignTop, "Align top");
            this.btnAlignTop.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnAlignTop, "Align top");
            this.btnAlignTop.Click += new System.EventHandler(this.formatAlignTop_Click);
            // 
            // btnSameHeight
            // 
            this.btnSameHeight.BackColor = System.Drawing.Color.Transparent;
            this.btnSameHeight.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSameHeight.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSameHeight.HottrackImage = null;
            this.btnSameHeight.Location = new System.Drawing.Point(103, 18);
            this.btnSameHeight.Name = "btnSameHeight";
            this.btnSameHeight.NormalImage = global::KB9Utility.Properties.Resources.sameheight;
            this.btnSameHeight.OnlyShowBitmap = false;
            this.btnSameHeight.PressedImage = null;
            this.btnSameHeight.Size = new System.Drawing.Size(25, 22);
            this.btnSameHeight.TabIndex = 5;
            this.btnSameHeight.TabStop = false;
            this.btnSameHeight.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnSameHeight, "Same height");
            this.btnSameHeight.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSameHeight, "Same height");
            this.btnSameHeight.Click += new System.EventHandler(this.formatMakeSameHeight_Click);
            // 
            // btnSameWidth
            // 
            this.btnSameWidth.BackColor = System.Drawing.Color.Transparent;
            this.btnSameWidth.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSameWidth.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSameWidth.HottrackImage = null;
            this.btnSameWidth.Location = new System.Drawing.Point(103, 41);
            this.btnSameWidth.Name = "btnSameWidth";
            this.btnSameWidth.NormalImage = global::KB9Utility.Properties.Resources.samewidth;
            this.btnSameWidth.OnlyShowBitmap = false;
            this.btnSameWidth.PressedImage = null;
            this.btnSameWidth.Size = new System.Drawing.Size(25, 22);
            this.btnSameWidth.TabIndex = 4;
            this.btnSameWidth.TabStop = false;
            this.btnSameWidth.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnSameWidth, "Same width");
            this.btnSameWidth.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSameWidth, "Same width");
            this.btnSameWidth.Click += new System.EventHandler(this.formatMakeSameWidth_Click);
            // 
            // btnAlignBottom
            // 
            this.btnAlignBottom.BackColor = System.Drawing.Color.Transparent;
            this.btnAlignBottom.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnAlignBottom.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnAlignBottom.HottrackImage = null;
            this.btnAlignBottom.Location = new System.Drawing.Point(72, 18);
            this.btnAlignBottom.Name = "btnAlignBottom";
            this.btnAlignBottom.NormalImage = global::KB9Utility.Properties.Resources.alignbottom;
            this.btnAlignBottom.OnlyShowBitmap = false;
            this.btnAlignBottom.PressedImage = null;
            this.btnAlignBottom.Size = new System.Drawing.Size(25, 22);
            this.btnAlignBottom.TabIndex = 3;
            this.btnAlignBottom.TabStop = false;
            this.btnAlignBottom.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnAlignBottom, "Align bottom");
            this.btnAlignBottom.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnAlignBottom, "Align bottom");
            this.btnAlignBottom.Click += new System.EventHandler(this.formatAlignBottom_Click);
            // 
            // btnAlignRight
            // 
            this.btnAlignRight.BackColor = System.Drawing.Color.Transparent;
            this.btnAlignRight.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnAlignRight.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnAlignRight.HottrackImage = null;
            this.btnAlignRight.Location = new System.Drawing.Point(72, 40);
            this.btnAlignRight.Name = "btnAlignRight";
            this.btnAlignRight.NormalImage = global::KB9Utility.Properties.Resources.alignright;
            this.btnAlignRight.OnlyShowBitmap = false;
            this.btnAlignRight.PressedImage = null;
            this.btnAlignRight.Size = new System.Drawing.Size(25, 22);
            this.btnAlignRight.TabIndex = 2;
            this.btnAlignRight.TabStop = false;
            this.btnAlignRight.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnAlignRight, "Align right");
            this.btnAlignRight.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnAlignRight, "Align right");
            this.btnAlignRight.Click += new System.EventHandler(this.formatAlignRight_Click);
            // 
            // btnAlignCenter
            // 
            this.btnAlignCenter.BackColor = System.Drawing.Color.Transparent;
            this.btnAlignCenter.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnAlignCenter.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnAlignCenter.HottrackImage = null;
            this.btnAlignCenter.Location = new System.Drawing.Point(41, 40);
            this.btnAlignCenter.Name = "btnAlignCenter";
            this.btnAlignCenter.NormalImage = global::KB9Utility.Properties.Resources.aligncenter;
            this.btnAlignCenter.OnlyShowBitmap = false;
            this.btnAlignCenter.PressedImage = null;
            this.btnAlignCenter.Size = new System.Drawing.Size(25, 22);
            this.btnAlignCenter.TabIndex = 1;
            this.btnAlignCenter.TabStop = false;
            this.btnAlignCenter.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnAlignCenter, "Align center");
            this.btnAlignCenter.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnAlignCenter, "Align center");
            this.btnAlignCenter.Click += new System.EventHandler(this.tsbAlignCenter_Click);
            // 
            // btnAlignLeft
            // 
            this.btnAlignLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnAlignLeft.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnAlignLeft.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnAlignLeft.HottrackImage = null;
            this.btnAlignLeft.Location = new System.Drawing.Point(10, 40);
            this.btnAlignLeft.Name = "btnAlignLeft";
            this.btnAlignLeft.NormalImage = global::KB9Utility.Properties.Resources.alignleft;
            this.btnAlignLeft.OnlyShowBitmap = false;
            this.btnAlignLeft.PressedImage = null;
            this.btnAlignLeft.Size = new System.Drawing.Size(25, 22);
            this.btnAlignLeft.TabIndex = 0;
            this.btnAlignLeft.TabStop = false;
            this.btnAlignLeft.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnAlignLeft, "Align left");
            this.btnAlignLeft.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnAlignLeft, "Align left");
            this.btnAlignLeft.Click += new System.EventHandler(this.formatAlignLeft_Click);
            // 
            // panelEdit
            // 
            this.panelEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.panelEdit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelEdit.BaseColorOn = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelEdit.Caption = "Edit";
            this.panelEdit.Controls.Add(this.btnRedo);
            this.panelEdit.Controls.Add(this.btnUnselect);
            this.panelEdit.Controls.Add(this.btnUndo);
            this.panelEdit.Controls.Add(this.btnSelectAll);
            this.panelEdit.Controls.Add(this.btnDel);
            this.panelEdit.Controls.Add(this.btnPaste);
            this.panelEdit.Controls.Add(this.btnCut);
            this.panelEdit.Controls.Add(this.btnCopy);
            this.panelEdit.ForeColor = System.Drawing.Color.SteelBlue;
            this.panelEdit.Location = new System.Drawing.Point(69, 1);
            this.panelEdit.Name = "panelEdit";
            this.panelEdit.Opacity = 255;
            this.panelEdit.Size = new System.Drawing.Size(172, 81);
            this.panelEdit.TabIndex = 1;
            // 
            // btnRedo
            // 
            this.btnRedo.BackColor = System.Drawing.Color.Transparent;
            this.btnRedo.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnRedo.HighlightColor = System.Drawing.SystemColors.HotTrack;
            this.btnRedo.HottrackImage = null;
            this.btnRedo.Location = new System.Drawing.Point(138, 14);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.NormalImage = global::KB9Utility.Properties.Resources.redo;
            this.btnRedo.OnlyShowBitmap = false;
            this.btnRedo.PressedImage = null;
            this.btnRedo.Size = new System.Drawing.Size(25, 22);
            this.btnRedo.TabIndex = 8;
            this.btnRedo.TabStop = false;
            this.btnRedo.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnRedo, "Redo");
            this.btnRedo.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnRedo, "Redo");
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnUnselect
            // 
            this.btnUnselect.BackColor = System.Drawing.Color.Transparent;
            this.btnUnselect.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnUnselect.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnUnselect.HottrackImage = null;
            this.btnUnselect.Location = new System.Drawing.Point(106, 41);
            this.btnUnselect.Name = "btnUnselect";
            this.btnUnselect.NormalImage = global::KB9Utility.Properties.Resources.unselect;
            this.btnUnselect.OnlyShowBitmap = false;
            this.btnUnselect.PressedImage = null;
            this.btnUnselect.Size = new System.Drawing.Size(25, 22);
            this.btnUnselect.TabIndex = 7;
            this.btnUnselect.TabStop = false;
            this.btnUnselect.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnUnselect, "Unselect");
            this.btnUnselect.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnUnselect, "Unselect");
            this.btnUnselect.Click += new System.EventHandler(this.editUnselect_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.BackColor = System.Drawing.Color.Transparent;
            this.btnUndo.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnUndo.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnUndo.HottrackImage = null;
            this.btnUndo.Location = new System.Drawing.Point(106, 14);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.NormalImage = global::KB9Utility.Properties.Resources.undo;
            this.btnUndo.OnlyShowBitmap = false;
            this.btnUndo.PressedImage = null;
            this.btnUndo.Size = new System.Drawing.Size(25, 22);
            this.btnUndo.TabIndex = 6;
            this.btnUndo.TabStop = false;
            this.btnUndo.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnUndo, "Undo");
            this.btnUndo.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnUndo, "Undo");
            this.btnUndo.Click += new System.EventHandler(this.editUndo_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectAll.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSelectAll.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSelectAll.HottrackImage = null;
            this.btnSelectAll.Location = new System.Drawing.Point(76, 41);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.NormalImage = global::KB9Utility.Properties.Resources.selectall;
            this.btnSelectAll.OnlyShowBitmap = false;
            this.btnSelectAll.PressedImage = null;
            this.btnSelectAll.Size = new System.Drawing.Size(25, 22);
            this.btnSelectAll.TabIndex = 4;
            this.btnSelectAll.TabStop = false;
            this.btnSelectAll.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnSelectAll, "Select all");
            this.btnSelectAll.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSelectAll, "Select all");
            this.btnSelectAll.Click += new System.EventHandler(this.editSelectAll_Click);
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnDel.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnDel.HottrackImage = null;
            this.btnDel.Location = new System.Drawing.Point(45, 41);
            this.btnDel.Name = "btnDel";
            this.btnDel.NormalImage = global::KB9Utility.Properties.Resources.del;
            this.btnDel.OnlyShowBitmap = false;
            this.btnDel.PressedImage = null;
            this.btnDel.Size = new System.Drawing.Size(25, 22);
            this.btnDel.TabIndex = 3;
            this.btnDel.TabStop = false;
            this.btnDel.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnDel, "Delete");
            this.btnDel.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnDel, "Delete");
            this.btnDel.Click += new System.EventHandler(this.editDel_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.BackColor = System.Drawing.Color.Transparent;
            this.btnPaste.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnPaste.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnPaste.HottrackImage = null;
            this.btnPaste.Location = new System.Drawing.Point(4, 12);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.NormalImage = global::KB9Utility.Properties.Resources.paste32;
            this.btnPaste.OnlyShowBitmap = false;
            this.btnPaste.PressedImage = null;
            this.btnPaste.Size = new System.Drawing.Size(35, 55);
            this.btnPaste.TabIndex = 2;
            this.btnPaste.TabStop = false;
            this.btnPaste.Text = "Paste";
            this.btnPaste.TextAlign = KB9Utility.eTextAlign.Bottom;
            this.toolTips.SetToolTip(this.btnPaste, "Paste");
            this.btnPaste.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnPaste, "Paste");
            this.btnPaste.Click += new System.EventHandler(this.editPaste_Click);
            // 
            // btnCut
            // 
            this.btnCut.BackColor = System.Drawing.Color.Transparent;
            this.btnCut.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnCut.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnCut.HottrackImage = null;
            this.btnCut.Location = new System.Drawing.Point(43, 14);
            this.btnCut.Name = "btnCut";
            this.btnCut.NormalImage = global::KB9Utility.Properties.Resources.cut;
            this.btnCut.OnlyShowBitmap = false;
            this.btnCut.PressedImage = null;
            this.btnCut.Size = new System.Drawing.Size(25, 22);
            this.btnCut.TabIndex = 1;
            this.btnCut.TabStop = false;
            this.btnCut.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnCut, "Cut");
            this.btnCut.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnCut, "Cut");
            this.btnCut.Click += new System.EventHandler(this.editCut_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnCopy.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnCopy.HottrackImage = null;
            this.btnCopy.Location = new System.Drawing.Point(73, 14);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.NormalImage = global::KB9Utility.Properties.Resources.copy;
            this.btnCopy.OnlyShowBitmap = false;
            this.btnCopy.PressedImage = null;
            this.btnCopy.Size = new System.Drawing.Size(25, 22);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.TabStop = false;
            this.btnCopy.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnCopy, "Copy");
            this.btnCopy.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnCopy, "Copy");
            this.btnCopy.Click += new System.EventHandler(this.editCopy_Click);
            // 
            // panelFile
            // 
            this.panelFile.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelFile.BaseColorOn = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(227)))), ((int)(((byte)(242)))));
            this.panelFile.Caption = "File";
            this.panelFile.Controls.Add(this.btnSaveAs);
            this.panelFile.Controls.Add(this.btnSave);
            this.panelFile.Controls.Add(this.btnOpen);
            this.panelFile.Controls.Add(this.btnNew);
            this.panelFile.ForeColor = System.Drawing.Color.SteelBlue;
            this.panelFile.Location = new System.Drawing.Point(2, 2);
            this.panelFile.Name = "panelFile";
            this.panelFile.Opacity = 255;
            this.panelFile.Size = new System.Drawing.Size(65, 81);
            this.panelFile.TabIndex = 0;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveAs.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSaveAs.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSaveAs.HottrackImage = null;
            this.btnSaveAs.Location = new System.Drawing.Point(29, 41);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.NormalImage = global::KB9Utility.Properties.Resources.saveas;
            this.btnSaveAs.OnlyShowBitmap = false;
            this.btnSaveAs.PressedImage = null;
            this.btnSaveAs.Size = new System.Drawing.Size(25, 22);
            this.btnSaveAs.TabIndex = 3;
            this.btnSaveAs.TabStop = false;
            this.btnSaveAs.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnSaveAs, "Save as");
            this.btnSaveAs.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSaveAs, "Save as");
            this.btnSaveAs.Click += new System.EventHandler(this.tsbSaveAs_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnSave.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnSave.HottrackImage = null;
            this.btnSave.Location = new System.Drawing.Point(2, 41);
            this.btnSave.Name = "btnSave";
            this.btnSave.NormalImage = global::KB9Utility.Properties.Resources.save;
            this.btnSave.OnlyShowBitmap = false;
            this.btnSave.PressedImage = null;
            this.btnSave.Size = new System.Drawing.Size(25, 22);
            this.btnSave.TabIndex = 2;
            this.btnSave.TabStop = false;
            this.btnSave.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnSave, "Save");
            this.btnSave.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnSave, "Save");
            this.btnSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnOpen.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnOpen.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnOpen.HottrackImage = null;
            this.btnOpen.Location = new System.Drawing.Point(29, 17);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.NormalImage = global::KB9Utility.Properties.Resources.open;
            this.btnOpen.OnlyShowBitmap = false;
            this.btnOpen.PressedImage = null;
            this.btnOpen.Size = new System.Drawing.Size(25, 22);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.TabStop = false;
            this.btnOpen.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnOpen, "Open");
            this.btnOpen.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnOpen, "Open");
            this.btnOpen.Click += new System.EventHandler(this.fileOpen_Click);
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnNew.HighlightColor = System.Drawing.Color.DarkOrange;
            this.btnNew.HottrackImage = null;
            this.btnNew.Location = new System.Drawing.Point(2, 17);
            this.btnNew.Name = "btnNew";
            this.btnNew.NormalImage = global::KB9Utility.Properties.Resources.NewFile;
            this.btnNew.OnlyShowBitmap = false;
            this.btnNew.PressedImage = null;
            this.btnNew.Size = new System.Drawing.Size(25, 22);
            this.btnNew.TabIndex = 0;
            this.btnNew.TabStop = false;
            this.btnNew.TextAlign = KB9Utility.eTextAlign.None;
            this.toolTips.SetToolTip(this.btnNew, "New");
            this.btnNew.ToolTip = "";
            this.toolTips.SetToolTipWhenDisabled(this.btnNew, "New");
            this.btnNew.Click += new System.EventHandler(this.fileNew_Click);
            // 
            // pgProperties
            // 
            this.pgProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgProperties.HelpVisible = false;
            this.pgProperties.Location = new System.Drawing.Point(700, 4);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgProperties.Size = new System.Drawing.Size(248, 166);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.ToolbarVisible = false;
            this.pgProperties.Enter += new System.EventHandler(this.pgProperties_Enter);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.editorMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(948, 304);
            this.panel1.TabIndex = 1;
            // 
            // editorMain
            // 
            this.editorMain.AutoScroll = true;
            this.editorMain.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.editorMain.AutoScrollMinSize = new System.Drawing.Size(1152, 458);
            this.editorMain.BackColor = System.Drawing.SystemColors.Window;
            this.editorMain.BackgroundResizeZone = 10;
            this.editorMain.BackgroundSizable = false;
            this.editorMain.BorderColor = System.Drawing.SystemColors.Window;
            this.editorMain.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.editorMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorMain.DrawingObject = null;
            this.editorMain.GridLinesColor = System.Drawing.Color.Gray;
            this.editorMain.GridSize = new System.Drawing.Size(6, 6);
            this.editorMain.GridStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.editorMain.InterCharDelay = 2;
            this.editorMain.Location = new System.Drawing.Point(0, 0);
            this.editorMain.Macro1 = keyEditingType1;
            this.editorMain.Macro2 = keyEditingType2;
            this.editorMain.Macro3 = keyEditingType3;
            this.editorMain.Macro4 = keyEditingType4;
            this.editorMain.Macro5 = keyEditingType5;
            this.editorMain.MarginColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.editorMain.MarkerSize = new System.Drawing.Size(6, 6);
            this.editorMain.MiddleButtonPanningScrollBarOrigin = new System.Drawing.Point(0, 0);
            this.editorMain.MiddleButtonPanningVirtualOrigin = new System.Drawing.Point(0, 0);
            this.editorMain.Modified = true;
            this.editorMain.MultipleDrawOneObject = false;
            this.editorMain.Name = "editorMain";
            this.editorMain.NoneClientBackgourndColor = System.Drawing.SystemColors.ButtonShadow;
            this.editorMain.ParentForm = null;
            this.editorMain.Restraints = KB9Utility.DiagramEditor.RESTRAIN.RESTRAINT_MARGIN;
            this.editorMain.ScrollWheelMode = KB9Utility.DiagramEditor.WheelMode.WHEEL_ZOOM;
            this.editorMain.Sensitivity = 5;
            this.editorMain.ShowGridLines = true;
            this.editorMain.ShowVirtualScreenMargin = true;
            this.editorMain.Size = new System.Drawing.Size(944, 300);
            this.editorMain.SnapToGrid = true;
            this.editorMain.TabIndex = 0;
            this.editorMain.Text = "label1";
            this.editorMain.TouchDelay = 5;
            this.editorMain.VirtualScreenMargin = new System.Windows.Forms.Padding(59);
            this.editorMain.VirtualSize = new System.Drawing.Size(2232, 888);
            this.editorMain.Zoom = 0.5161290322580645D;
            this.editorMain.ZoomMax = 1D;
            this.editorMain.ZoomMin = 0.5161290322580645D;
            this.editorMain.ZoomStep = 0.01D;
            this.editorMain.OnShowDiagramEntityProperties += new KB9Utility.DiagramEditor.EventShowDiagramEntityProperties(this.editorMain_OnShowDiagramEntityProperties);
            this.editorMain.OnChanged += new KB9Utility.DiagramEditor.EventChanged(this.editorMain_OnChanged);
            this.editorMain.OnEditEntityKeyContent += new KB9Utility.DiagramEditor.EventEditEntityKeyContent(this.editorMain_OnEditEntityKeyContent);
            this.editorMain.OnRefreshPropertiesGrid += new KB9Utility.DiagramEditor.EventRefreshPropertiesGrid(this.editorMain_OnRefreshPropertiesGrid);
            this.editorMain.OnEditorUndo += new KB9Utility.DiagramEditor.EventEditorUndo(this.editorMain_OnEditorUndo);
            this.editorMain.Enter += new System.EventHandler(this.editorMain_Enter);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomOut.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnZoomOut.HighlightColor = System.Drawing.SystemColors.ButtonFace;
            this.btnZoomOut.HottrackImage = null;
            this.btnZoomOut.Location = new System.Drawing.Point(861, 512);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.NormalImage = global::KB9Utility.Properties.Resources.plus;
            this.btnZoomOut.OnlyShowBitmap = false;
            this.btnZoomOut.PressedImage = null;
            this.btnZoomOut.Size = new System.Drawing.Size(19, 16);
            this.btnZoomOut.TabIndex = 6;
            this.btnZoomOut.TabStop = false;
            this.btnZoomOut.TextAlign = KB9Utility.eTextAlign.None;
            this.btnZoomOut.ToolTip = "";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.ButtonForm = KB9Utility.eButtonForm.Rectangle;
            this.btnZoomIn.HighlightColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.HottrackImage = null;
            this.btnZoomIn.Location = new System.Drawing.Point(660, 512);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.NormalImage = global::KB9Utility.Properties.Resources.substract;
            this.btnZoomIn.OnlyShowBitmap = false;
            this.btnZoomIn.PressedImage = null;
            this.btnZoomIn.Size = new System.Drawing.Size(19, 16);
            this.btnZoomIn.TabIndex = 5;
            this.btnZoomIn.TabStop = false;
            this.btnZoomIn.TextAlign = KB9Utility.eTextAlign.None;
            this.btnZoomIn.ToolTip = "";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 531);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.tkbZoom);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "frmMain";
            this.Text = "KB9000 Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmMain_PreviewKeyDown);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkbZoom)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabCodes.ResumeLayout(false);
            this.tpCode0.ResumeLayout(false);
            this.tpCode1.ResumeLayout(false);
            this.tpCode2.ResumeLayout(false);
            this.tpCode3.ResumeLayout(false);
            this.panelButton.ResumeLayout(false);
            this.panelTools.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.panelKeyEdit.ResumeLayout(false);
            this.panelFormat.ResumeLayout(false);
            this.panelEdit.ResumeLayout(false);
            this.panelFile.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem fileNew;
        private System.Windows.Forms.ToolStripMenuItem fileOpen;
        private System.Windows.Forms.ToolStripMenuItem fileSave;
        private System.Windows.Forms.ToolStripMenuItem fileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fileExit;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        public System.Windows.Forms.ToolStripMenuItem editCopy;
        public System.Windows.Forms.ToolStripMenuItem editCut;
        public System.Windows.Forms.ToolStripMenuItem editPaste;
        public System.Windows.Forms.ToolStripMenuItem editDel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        public System.Windows.Forms.ToolStripMenuItem editUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        public System.Windows.Forms.ToolStripMenuItem editSelectAll;
        public System.Windows.Forms.ToolStripMenuItem editUnselect;
        private System.Windows.Forms.ToolStripMenuItem menuFormat;
        public System.Windows.Forms.ToolStripMenuItem formatAlign;
        public System.Windows.Forms.ToolStripMenuItem formatAlignLeft;
        public System.Windows.Forms.ToolStripMenuItem formatAlignRight;
        public System.Windows.Forms.ToolStripMenuItem formatAlignTop;
        public System.Windows.Forms.ToolStripMenuItem formatAlignBottom;
        public System.Windows.Forms.ToolStripMenuItem formatMakeSame;
        public System.Windows.Forms.ToolStripMenuItem formatSameWidth;
        public System.Windows.Forms.ToolStripMenuItem formatSameHeight;
        public System.Windows.Forms.ToolStripMenuItem formatSameBoth;
        public System.Windows.Forms.ToolStripMenuItem formatHEqual;
        public System.Windows.Forms.ToolStripMenuItem formatVEqual;
        private System.Windows.Forms.ToolStripMenuItem menuKB9000;
        private System.Windows.Forms.ToolStripMenuItem kb9000Detect;
        private System.Windows.Forms.ToolStripMenuItem kb9000Write;
        private System.Windows.Forms.ToolStripMenuItem kb9000Read;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem kb9000Test;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem helpAbout;
        private System.Windows.Forms.StatusStrip statusMain;
        public DEditor editorMain;
        private NSButton btnKeyArea;
        private System.Windows.Forms.PropertyGrid pgProperties;
        public System.Windows.Forms.ToolStripMenuItem formatAlignCenter;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        public System.Windows.Forms.ToolStripMenuItem formatAlignMiddle;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private NSButton btnSlideV;
        private NSButton btnSlideH;
        private KB9TextBox lblKeyCode0;
        private KB9TextBox lblKeyCode1;
        private KB9TextBox lblKeyCode2;
        private KB9TextBox lblKeyCode3;
        private System.Windows.Forms.TrackBar tkbZoom;
        private NSButton btnZoomIn;
        private NSButton btnZoomOut;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.ToolStripMenuItem editMacro;
        private System.Windows.Forms.ToolStripMenuItem fileRecent;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private OfficePanel panelFile;
        private NSButton btnSaveAs;
        private NSButton btnSave;
        private NSButton btnOpen;
        private NSButton btnNew;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl tabCodes;
        private System.Windows.Forms.TabPage tpCode0;
        private System.Windows.Forms.TabPage tpCode1;
        private System.Windows.Forms.TabPage tpCode2;
        private System.Windows.Forms.TabPage tpCode3;
        private NSButton btnUnselect;
        private NSButton btnUndo;
        private NSButton btnSelectAll;
        private NSButton btnDel;
        private NSButton btnPaste;
        private NSButton btnCut;
        private NSButton btnCopy;
        private KB9Panel panelTools;
        private OfficePanel panelEdit;
        private OfficePanel panelButton;
        private OfficePanel panelFormat;
        private System.Windows.Forms.Button button2;
        private NSButton btnSameBoth;
        private NSButton btnAlignMiddle;
        private NSButton btnAlignTop;
        private NSButton btnSameHeight;
        private NSButton btnSameWidth;
        private NSButton btnAlignBottom;
        private NSButton btnAlignRight;
        private NSButton btnAlignCenter;
        private NSButton btnAlignLeft;
        private OfficePanel panelKeyEdit;
        private NSButton btnCodePause;
        private NSButton btnCodeMacro;
        private NSButton btnCodeRepeat;
        private NSButton btnCodeCombination;
        private NSButton btnCodeList;
        private System.Windows.Forms.ToolStripMenuItem menuContents;
        private System.Windows.Forms.ToolStripMenuItem contentsCodePause;
        private System.Windows.Forms.ToolStripMenuItem contentsCodeList;
        private System.Windows.Forms.ToolStripMenuItem contentsCodeCombination;
        private System.Windows.Forms.ToolStripMenuItem contentsCodeRepeat;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem kb9000TestDataStruture;
        private EnhancedToolTip toolTips;
        private System.Windows.Forms.TabPage tpLabel;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg0;
        private OfficePanel panelActions;
        private NSButton btnActionsTest;
        private NSButton btnActionsWrite;
        private NSButton btnActionsRead;
        private NSButton btnActionsDetect;
        private System.Windows.Forms.ToolStripMenuItem filePreview;
        private System.Windows.Forms.ToolStripMenuItem fileExport;
        private System.Windows.Forms.ToolStripMenuItem exportJpg;
        private System.Windows.Forms.ToolStripMenuItem exportBmp;
        private System.Windows.Forms.ToolStripMenuItem exportPng;
        private System.Windows.Forms.ToolStripMenuItem filePrint;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem fileExportImageViewer;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem helpLog;
        private System.Windows.Forms.ToolStripMenuItem helpClearLog;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem keysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyNewKey;
        private System.Windows.Forms.ToolStripMenuItem keysNewSlideV;
        private System.Windows.Forms.ToolStripMenuItem keysNewSlideH;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem keysClearBeep;
        private System.Windows.Forms.ToolStripMenuItem keysDefaultBeep;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem keysNewMatrix;
        public System.Windows.Forms.ToolStripMenuItem editRedo;
        private NSButton btnRedo;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg2;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg3;
        private System.Windows.Forms.ToolStripStatusLabel tsFreeMem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem contentsInsertMacro;
        private System.Windows.Forms.ToolStripMenuItem fileEnablePrint3X;
    }
}


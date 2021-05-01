namespace KB9Utility
{
    partial class frmKeyCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKeyCode));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.tsbUndo = new System.Windows.Forms.ToolStripButton();
            this.tsbDelay = new System.Windows.Forms.ToolStripButton();
            this.tsbSpecial = new System.Windows.Forms.ToolStripButton();
            this.tsbCombinatioin = new System.Windows.Forms.ToolStripButton();
            this.tsbRepeat = new System.Windows.Forms.ToolStripButton();
            this.tsbMacro = new System.Windows.Forms.ToolStripButton();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDelay = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRepeat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmSpecial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCombination = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtKeyCode = new KB9Utility.KB9TextBox();
            this.toolStripMain.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClear,
            this.tsbUndo,
            this.tsbDelay,
            this.tsbSpecial,
            this.tsbCombinatioin,
            this.tsbRepeat,
            this.tsbMacro});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(630, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // tsbClear
            // 
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(23, 22);
            this.tsbClear.Text = "Clear";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // tsbUndo
            // 
            this.tsbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUndo.Image = ((System.Drawing.Image)(resources.GetObject("tsbUndo.Image")));
            this.tsbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.Size = new System.Drawing.Size(23, 22);
            this.tsbUndo.Text = "Undo";
            this.tsbUndo.Click += new System.EventHandler(this.tsbUndo_Click);
            // 
            // tsbDelay
            // 
            this.tsbDelay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelay.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelay.Image")));
            this.tsbDelay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelay.Name = "tsbDelay";
            this.tsbDelay.Size = new System.Drawing.Size(23, 22);
            this.tsbDelay.Text = "Pause";
            this.tsbDelay.Click += new System.EventHandler(this.tsbDelay_Click);
            // 
            // tsbSpecial
            // 
            this.tsbSpecial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSpecial.Image = ((System.Drawing.Image)(resources.GetObject("tsbSpecial.Image")));
            this.tsbSpecial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSpecial.Name = "tsbSpecial";
            this.tsbSpecial.Size = new System.Drawing.Size(23, 22);
            this.tsbSpecial.Text = "Key List";
            this.tsbSpecial.Click += new System.EventHandler(this.tsbSpecial_Click);
            // 
            // tsbCombinatioin
            // 
            this.tsbCombinatioin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCombinatioin.Image = ((System.Drawing.Image)(resources.GetObject("tsbCombinatioin.Image")));
            this.tsbCombinatioin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCombinatioin.Name = "tsbCombinatioin";
            this.tsbCombinatioin.Size = new System.Drawing.Size(23, 22);
            this.tsbCombinatioin.Text = "Combination";
            this.tsbCombinatioin.Click += new System.EventHandler(this.tsbCombinatioin_Click);
            // 
            // tsbRepeat
            // 
            this.tsbRepeat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRepeat.Image = global::KB9Utility.Properties.Resources.repeat;
            this.tsbRepeat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRepeat.Name = "tsbRepeat";
            this.tsbRepeat.Size = new System.Drawing.Size(23, 22);
            this.tsbRepeat.Text = "Input Repeat";
            this.tsbRepeat.Click += new System.EventHandler(this.tsbRepeat_Click);
            // 
            // tsbMacro
            // 
            this.tsbMacro.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMacro.Image = ((System.Drawing.Image)(resources.GetObject("tsbMacro.Image")));
            this.tsbMacro.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMacro.Name = "tsbMacro";
            this.tsbMacro.Size = new System.Drawing.Size(23, 22);
            this.tsbMacro.Text = "Input Macro";
            this.tsbMacro.Click += new System.EventHandler(this.tsbMacro_Click);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.keyToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(630, 24);
            this.menuMain.TabIndex = 2;
            this.menuMain.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmClear,
            this.toolStripMenuItem1,
            this.tsmUndo});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // tsmClear
            // 
            this.tsmClear.Name = "tsmClear";
            this.tsmClear.Size = new System.Drawing.Size(100, 22);
            this.tsmClear.Text = "&Clear";
            this.tsmClear.Click += new System.EventHandler(this.tsmClear_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 6);
            // 
            // tsmUndo
            // 
            this.tsmUndo.Name = "tsmUndo";
            this.tsmUndo.Size = new System.Drawing.Size(100, 22);
            this.tsmUndo.Text = "&Undo";
            this.tsmUndo.Click += new System.EventHandler(this.tsmUndo_Click);
            // 
            // keyToolStripMenuItem
            // 
            this.keyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmDelay,
            this.tsmRepeat,
            this.toolStripSeparator1,
            this.tsmSpecial,
            this.tsmCombination,
            this.tsmMacro});
            this.keyToolStripMenuItem.Name = "keyToolStripMenuItem";
            this.keyToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.keyToolStripMenuItem.Text = "&Key";
            // 
            // tsmDelay
            // 
            this.tsmDelay.Name = "tsmDelay";
            this.tsmDelay.Size = new System.Drawing.Size(172, 22);
            this.tsmDelay.Text = "Input &Pause";
            this.tsmDelay.Click += new System.EventHandler(this.tsmDelay_Click);
            // 
            // tsmRepeat
            // 
            this.tsmRepeat.Name = "tsmRepeat";
            this.tsmRepeat.Size = new System.Drawing.Size(172, 22);
            this.tsmRepeat.Text = "Input &Repeat";
            this.tsmRepeat.Click += new System.EventHandler(this.tsmRepeat_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // tsmSpecial
            // 
            this.tsmSpecial.Name = "tsmSpecial";
            this.tsmSpecial.Size = new System.Drawing.Size(172, 22);
            this.tsmSpecial.Text = "Key &List";
            this.tsmSpecial.Click += new System.EventHandler(this.tsmSpecial_Click);
            // 
            // tsmCombination
            // 
            this.tsmCombination.Name = "tsmCombination";
            this.tsmCombination.Size = new System.Drawing.Size(172, 22);
            this.tsmCombination.Text = "Input &Combination";
            this.tsmCombination.Click += new System.EventHandler(this.tsmCombination_Click);
            // 
            // tsmMacro
            // 
            this.tsmMacro.Name = "tsmMacro";
            this.tsmMacro.Size = new System.Drawing.Size(172, 22);
            this.tsmMacro.Text = "Input &Macro";
            this.tsmMacro.Click += new System.EventHandler(this.tstMacro_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(423, 127);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 27);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(531, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtKeyCode
            // 
            this.txtKeyCode.AllowDrop = true;
            this.txtKeyCode.BackColor = System.Drawing.Color.White;
            this.txtKeyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKeyCode.Caret = -1;
            this.txtKeyCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtKeyCode.EnableTextChangedEvent = true;
            this.txtKeyCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeyCode.Location = new System.Drawing.Point(12, 52);
            this.txtKeyCode.MinimumSize = new System.Drawing.Size(2, 2);
            this.txtKeyCode.Name = "txtKeyCode";
            this.txtKeyCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtKeyCode.RestrictMacro = false;
            this.txtKeyCode.ShowCaret = false;
            this.txtKeyCode.ShowGripper = true;
            this.txtKeyCode.SingleKey = false;
            this.txtKeyCode.Size = new System.Drawing.Size(606, 66);
            this.txtKeyCode.TabIndex = 0;
            this.txtKeyCode.OnTextContentChanged += new KB9Utility.KB9TextBox.EventOnTextChanged(this.txtKeyCode_OnTextChanged);
            this.txtKeyCode.Leave += new System.EventHandler(this.txtKeyCode_Leave);
            this.txtKeyCode.Resize += new System.EventHandler(this.txtKeyCode_Resize);
            this.txtKeyCode.Enter += new System.EventHandler(this.txtKeyCode_Enter);
            // 
            // frmKeyCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 163);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtKeyCode);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmKeyCode";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit key code";
            this.Load += new System.EventHandler(this.frmKeyCode_Load);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private KB9TextBox txtKeyCode;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmClear;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmDelay;
      //  private System.Windows.Forms.ToolStripButton tsClear;
       // private System.Windows.Forms.ToolStripButton tsbUndo;
       // private System.Windows.Forms.ToolStripSplitButton tsbLayer;
       // private System.Windows.Forms.ToolStripMenuItem tsmSingle;
       // private System.Windows.Forms.ToolStripMenuItem tsmMultiple;
        //private System.Windows.Forms.ToolStripButton tsbDelay;
        private System.Windows.Forms.ToolStripMenuItem tsmSpecial;
       // private System.Windows.Forms.ToolStripButton tsbSpecial;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripButton tsbUndo;
        private System.Windows.Forms.ToolStripButton tsbDelay;
        private System.Windows.Forms.ToolStripButton tsbSpecial;
        private System.Windows.Forms.ToolStripButton tsbCombinatioin;
        private System.Windows.Forms.ToolStripMenuItem tsmCombination;
        private System.Windows.Forms.ToolStripMenuItem tsmRepeat;
        private System.Windows.Forms.ToolStripMenuItem tsmMacro;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbRepeat;
        private System.Windows.Forms.ToolStripButton tsbMacro;
    }
}
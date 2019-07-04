namespace KB9Utility
{
    partial class frmCombination
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCombination));
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbSpecial = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKey2 = new KB9Utility.KB9TextBox();
            this.txtKey1 = new KB9Utility.KB9TextBox();
            this.txtKey0 = new KB9Utility.KB9TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "+";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(84, 83);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 27);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(187, 83);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSpecial});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(358, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbSpecial
            // 
            this.tsbSpecial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSpecial.Image = ((System.Drawing.Image)(resources.GetObject("tsbSpecial.Image")));
            this.tsbSpecial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSpecial.Name = "tsbSpecial";
            this.tsbSpecial.Size = new System.Drawing.Size(23, 22);
            this.tsbSpecial.Text = "Keyboard";
            this.tsbSpecial.Click += new System.EventHandler(this.tsbSpecial_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "+";
            // 
            // txtKey2
            // 
            this.txtKey2.AllowDrop = true;
            this.txtKey2.BackColor = System.Drawing.Color.White;
            this.txtKey2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKey2.Caret = -1;
            this.txtKey2.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtKey2.EnableTextChangedEvent = true;
            this.txtKey2.Location = new System.Drawing.Point(255, 43);
            this.txtKey2.MinimumSize = new System.Drawing.Size(2, 2);
            this.txtKey2.Name = "txtKey2";
            this.txtKey2.Padding = new System.Windows.Forms.Padding(2);
            this.txtKey2.RestrictMacro = true;
            this.txtKey2.ScrollBarVisible = false;
            this.txtKey2.ShowCaret = false;
            this.txtKey2.ShowGripper = false;
            this.txtKey2.SingleKey = true;
            this.txtKey2.Size = new System.Drawing.Size(89, 24);
            this.txtKey2.TabIndex = 6;
            this.txtKey2.Leave += new System.EventHandler(this.txtKey0_Leave);
            this.txtKey2.Enter += new System.EventHandler(this.txtKey0_Enter);
            // 
            // txtKey1
            // 
            this.txtKey1.AllowDrop = true;
            this.txtKey1.BackColor = System.Drawing.Color.White;
            this.txtKey1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKey1.Caret = -1;
            this.txtKey1.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtKey1.EnableTextChangedEvent = true;
            this.txtKey1.Location = new System.Drawing.Point(137, 43);
            this.txtKey1.MinimumSize = new System.Drawing.Size(2, 2);
            this.txtKey1.Name = "txtKey1";
            this.txtKey1.Padding = new System.Windows.Forms.Padding(2);
            this.txtKey1.RestrictMacro = true;
            this.txtKey1.ScrollBarVisible = false;
            this.txtKey1.ShowCaret = false;
            this.txtKey1.ShowGripper = false;
            this.txtKey1.SingleKey = false;
            this.txtKey1.Size = new System.Drawing.Size(89, 24);
            this.txtKey1.TabIndex = 1;
            this.txtKey1.Leave += new System.EventHandler(this.txtKey0_Leave);
            this.txtKey1.Enter += new System.EventHandler(this.txtKey0_Enter);
            // 
            // txtKey0
            // 
            this.txtKey0.AllowDrop = true;
            this.txtKey0.BackColor = System.Drawing.Color.White;
            this.txtKey0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKey0.Caret = -1;
            this.txtKey0.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtKey0.EnableTextChangedEvent = true;
            this.txtKey0.Location = new System.Drawing.Point(20, 43);
            this.txtKey0.MinimumSize = new System.Drawing.Size(2, 2);
            this.txtKey0.Name = "txtKey0";
            this.txtKey0.Padding = new System.Windows.Forms.Padding(2);
            this.txtKey0.RestrictMacro = true;
            this.txtKey0.ScrollBarVisible = false;
            this.txtKey0.ShowCaret = false;
            this.txtKey0.ShowGripper = false;
            this.txtKey0.SingleKey = false;
            this.txtKey0.Size = new System.Drawing.Size(89, 24);
            this.txtKey0.TabIndex = 0;
            this.txtKey0.Leave += new System.EventHandler(this.txtKey0_Leave);
            this.txtKey0.Enter += new System.EventHandler(this.txtKey0_Enter);
            // 
            // frmCombination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 124);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtKey2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKey1);
            this.Controls.Add(this.txtKey0);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCombination";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Combination Keys";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KB9TextBox txtKey0;
        private KB9TextBox txtKey1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbSpecial;
        private KB9TextBox txtKey2;
        private System.Windows.Forms.Label label2;
    }
}
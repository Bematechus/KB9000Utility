namespace KB9Utility
{
    partial class frmProgress
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
            this.timerOperation = new System.Windows.Forms.Timer(this.components);
            this.lkCancel = new System.Windows.Forms.LinkLabel();
            this.pbProgress = new KB9Utility.CMyProgressBar();
            this.SuspendLayout();
            // 
            // timerOperation
            // 
            this.timerOperation.Interval = 500;
            this.timerOperation.Tick += new System.EventHandler(this.timerOperation_Tick);
            // 
            // lkCancel
            // 
            this.lkCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lkCancel.AutoSize = true;
            this.lkCancel.Location = new System.Drawing.Point(666, 18);
            this.lkCancel.Name = "lkCancel";
            this.lkCancel.Size = new System.Drawing.Size(42, 14);
            this.lkCancel.TabIndex = 1;
            this.lkCancel.TabStop = true;
            this.lkCancel.Text = "Cancel";
            this.lkCancel.Visible = false;
            this.lkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkCancel_LinkClicked);
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.BarColor = System.Drawing.Color.Lime;
            this.pbProgress.BorderColor = System.Drawing.Color.Black;
            this.pbProgress.FillStyle = KB9Utility.CMyProgressBar.FillStyles.Solid;
            this.pbProgress.ForeColor = System.Drawing.SystemColors.Control;
            this.pbProgress.Location = new System.Drawing.Point(14, 18);
            this.pbProgress.Maximum = 100;
            this.pbProgress.Minimum = 0;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(681, 31);
            this.pbProgress.Step = 10;
            this.pbProgress.TabIndex = 0;
            this.pbProgress.Value = 0;
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 69);
            this.ControlBox = false;
            this.Controls.Add(this.lkCancel);
            this.Controls.Add(this.pbProgress);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progress";
            this.Load += new System.EventHandler(this.frmProgress_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmProgress_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CMyProgressBar pbProgress;
        private System.Windows.Forms.Timer timerOperation;
        private System.Windows.Forms.LinkLabel lkCancel;
    }
}
namespace KB9Utility
{
    partial class frmPreview
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
            this.picViewer = new KB9Utility.ImagePreviewPanel();
            this.SuspendLayout();
            // 
            // picViewer
            // 
            this.picViewer.AutoScroll = true;
            this.picViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picViewer.Location = new System.Drawing.Point(0, 0);
            this.picViewer.Name = "picViewer";
            this.picViewer.Size = new System.Drawing.Size(839, 446);
            this.picViewer.TabIndex = 2;
            this.picViewer.Paint += new System.Windows.Forms.PaintEventHandler(this.picPreview_Paint);
            this.picViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picViewer_MouseMove);
            this.picViewer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.picViewer_Scroll);
            this.picViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picViewer_MouseDown);
            this.picViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picViewer_MouseUp);
            // 
            // frmPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 446);
            this.Controls.Add(this.picViewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "frmPreview";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preview";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPreview_Load);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.Panel picPreview;
        private ImagePreviewPanel picViewer;
    }
}
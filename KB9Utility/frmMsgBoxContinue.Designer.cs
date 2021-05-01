namespace KB9Utility
{
    partial class frmMsgBoxContinue
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
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            this.picIcon.Location = new System.Drawing.Point(7, 11);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(37, 37);
            this.picIcon.TabIndex = 9;
            this.picIcon.TabStop = false;
            // 
            // btnNo
            // 
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.Location = new System.Drawing.Point(247, 65);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(142, 27);
            this.btnNo.TabIndex = 7;
            this.btnNo.Text = "Cancel Download";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(99, 65);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(142, 27);
            this.btnYes.TabIndex = 6;
            this.btnYes.Text = "Continue Download";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.Location = new System.Drawing.Point(68, 9);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(423, 39);
            this.lblMsg.TabIndex = 5;
            this.lblMsg.Text = "All template data in keyboard will erased.  Do you want to continue?";
            // 
            // frmMsgBoxContinue
            // 
            this.AcceptButton = this.btnYes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size(489, 104);
            this.ControlBox = false;
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblMsg);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMsgBoxContinue";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Question";
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Label lblMsg;
    }
}
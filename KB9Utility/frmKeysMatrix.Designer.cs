namespace KB9Utility
{
    partial class frmKeysMatrix
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
            this.nmRows = new System.Windows.Forms.NumericUpDown();
            this.nmRowSpacing = new System.Windows.Forms.NumericUpDown();
            this.nmCols = new System.Windows.Forms.NumericUpDown();
            this.nmColSpacing = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmRowSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmCols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmColSpacing)).BeginInit();
            this.SuspendLayout();
            // 
            // nmRows
            // 
            this.nmRows.Location = new System.Drawing.Point(71, 17);
            this.nmRows.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmRows.Name = "nmRows";
            this.nmRows.Size = new System.Drawing.Size(48, 21);
            this.nmRows.TabIndex = 3;
            this.nmRows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmRowSpacing
            // 
            this.nmRowSpacing.Location = new System.Drawing.Point(187, 17);
            this.nmRowSpacing.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmRowSpacing.Name = "nmRowSpacing";
            this.nmRowSpacing.Size = new System.Drawing.Size(48, 21);
            this.nmRowSpacing.TabIndex = 4;
            this.nmRowSpacing.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmCols
            // 
            this.nmCols.Location = new System.Drawing.Point(71, 44);
            this.nmCols.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nmCols.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmCols.Name = "nmCols";
            this.nmCols.Size = new System.Drawing.Size(48, 21);
            this.nmCols.TabIndex = 5;
            this.nmCols.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmColSpacing
            // 
            this.nmColSpacing.Location = new System.Drawing.Point(187, 44);
            this.nmColSpacing.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmColSpacing.Name = "nmColSpacing";
            this.nmColSpacing.Size = new System.Drawing.Size(48, 21);
            this.nmColSpacing.TabIndex = 6;
            this.nmColSpacing.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "Rows:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Cols:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Spacing:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Spacing:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(57, 82);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(138, 82);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmKeysMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 119);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nmColSpacing);
            this.Controls.Add(this.nmCols);
            this.Controls.Add(this.nmRowSpacing);
            this.Controls.Add(this.nmRows);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmKeysMatrix";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keys Matrix";
            ((System.ComponentModel.ISupportInitialize)(this.nmRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmRowSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmCols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmColSpacing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nmRows;
        private System.Windows.Forms.NumericUpDown nmRowSpacing;
        private System.Windows.Forms.NumericUpDown nmCols;
        private System.Windows.Forms.NumericUpDown nmColSpacing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
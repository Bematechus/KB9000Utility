using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KB9Utility
{
    public partial class frmKeysMatrix : Form
    {

        //private DiagramKeyMatrix.MatrixType _Matrix = DiagramKeyMatrix.MatrixType.Keys_2x2;
        //public DiagramKeyMatrix.MatrixType Matrix
        //{
        //    get
        //    {
        //        return _Matrix;
        //    }
        //    set
        //    {
        //        _Matrix = value;
        //    }
        //}

        private int _Rows = 1;
        public int Rows
        {
            get 
            {
                return _Rows;
            }
            set
            {
                _Rows = value;
            }
        }

        private int _Cols = 1;
        public int Cols
        {
            get
            {
                return _Cols;
            }
            set
            {
                _Cols = value;
            }
        }

        private int _RowSpacing = 1;
        public int RowSpacing
        {
            get
            {
                return _RowSpacing;
            }
            set
            {
                _RowSpacing = value;;
            }
        }

        private int _ColSpacing = 1;
        public int ColSpacing
        {
            get
            {
                return _ColSpacing;
            }
            set
            {
                _ColSpacing = value;
            }
        }

        public frmKeysMatrix()
        {
            InitializeComponent();
           // Init_Combo_Drawing(cmbPredefined);
           // Init_Predefined_Matrix();
        }
        /*
        private void Init_Predefined_Matrix()
        {
            
            
            CKeyMatrixItem[] ar = new CKeyMatrixItem[]{
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x1,  "Keys 1x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x2,  "Keys 1x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x3,  "Keys 1x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x4,  "Keys 1x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x5,  "Keys 1x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x6,  "Keys 1x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x7,  "Keys 1x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_1x8,  "Keys 1x8", Util.get_image("alignbottom")),

                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x1,  "Keys 2x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x2,  "Keys 2x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x3,  "Keys 2x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x4,  "Keys 2x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x5,  "Keys 2x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x6,  "Keys 2x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x7,  "Keys 2x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_2x8,  "Keys 2x8", Util.get_image("alignbottom")),

                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x1,  "Keys 3x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x2,  "Keys 3x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x3,  "Keys 3x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x4,  "Keys 3x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x5,  "Keys 3x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x6,  "Keys 3x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x7,  "Keys 3x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_3x8,  "Keys 3x8", Util.get_image("alignbottom")),


                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x1,  "Keys 4x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x2,  "Keys 4x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x3,  "Keys 4x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x4,  "Keys 4x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x5,  "Keys 4x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x6,  "Keys 4x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x7,  "Keys 4x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_4x8,  "Keys 4x8", Util.get_image("alignbottom")),

                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x1,  "Keys 5x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x2,  "Keys 5x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x3,  "Keys 5x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x4,  "Keys 5x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x5,  "Keys 5x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x6,  "Keys 5x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x7,  "Keys 5x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_5x8,  "Keys 5x8", Util.get_image("alignbottom")),


                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x1,  "Keys 6x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x2,  "Keys 6x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x3,  "Keys 6x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x4,  "Keys 6x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x5,  "Keys 6x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x6,  "Keys 6x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x7,  "Keys 6x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_6x8,  "Keys 6x8", Util.get_image("alignbottom")),


                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x1,  "Keys 7x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x2,  "Keys 7x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x3,  "Keys 7x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x4,  "Keys 7x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x5,  "Keys 7x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x6,  "Keys 7x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x7,  "Keys 7x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_7x8,  "Keys 7x8", Util.get_image("alignbottom")),


                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x1,  "Keys 8x1", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x2,  "Keys 8x2", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x3,  "Keys 8x3", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x4,  "Keys 8x4", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x5,  "Keys 8x5", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x6,  "Keys 8x6", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x7,  "Keys 8x7", Util.get_image("alignbottom")),
                new CKeyMatrixItem(DiagramKeyMatrix.MatrixType.Keys_8x8,  "Keys 8x8", Util.get_image("alignbottom")),

            };
            cmbPredefined.Items.AddRange(ar);
           // cmbPredefined.DisplayMember = "DemoImage";
            cmbPredefined.ValueMember = "Matrix";
            cmbPredefined.SelectedIndex = 0;

        }*/

        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (rbPredefined.Checked)
            //{
            //    if (cmbPredefined.SelectedItem == null)
            //        return;
            //    CKeyMatrixItem item = (CKeyMatrixItem)cmbPredefined.SelectedItem;

            //    this.Matrix = item.Matrix;
            //}
            //else
            //{
               // this.Matrix = DiagramKeyMatrix.MatrixType.Custom;
                this.Rows =(int) nmRows.Value;
                this.Cols = (int)nmCols.Value;
                this.RowSpacing = (int)nmRowSpacing.Value;
                this.ColSpacing = (int)nmColSpacing.Value;

           // }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /*
        public void Init_Combo_Drawing(ComboBox cmb)
        {
            cmb.DrawMode = DrawMode.OwnerDrawVariable;
            cmb.DrawItem += new DrawItemEventHandler(PredefinedCombo_DrawItem);
            cmb.Items.Clear();

        }
        private const int IMG_W = 15;
        private const int IMG_H = 15;

        private void PredefinedCombo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox cmb = sender as ComboBox;
                CKeyMatrixItem item = (CKeyMatrixItem)(cmb.Items[e.Index]);

                //string text = cmbPriority.Items[e.Index].ToString();
                e.DrawBackground();

                //if (text.Length > 0)
                //{
                    string text = item.Text;
                    Image img = item.DemoImage;// GetTaskPriorityImage(priority);

                    if (img != null)
                    {
                        e.Graphics.DrawImage(img, e.Bounds.X, e.Bounds.Y, IMG_W, IMG_H);
                    }
                //}

                e.Graphics.DrawString(text, cmb.Font, System.Drawing.Brushes.Black,
                                            new RectangleF(e.Bounds.X + IMG_W, e.Bounds.Y,
                                            e.Bounds.Width, e.Bounds.Height));

                e.DrawFocusRectangle();
            }
        }
        
        private void rbPredefined_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPredefined.Checked)
            {
                cmbPredefined.Enabled = true;

                nmRows.Enabled = false;
                nmCols.Enabled = false;
                nmRowSpacing.Enabled = false;
                nmColSpacing.Enabled = false;
            }
            else
            {
                cmbPredefined.Enabled = false;
                nmRows.Enabled = true;
                nmCols.Enabled = true;
                nmRowSpacing.Enabled = true;
                nmColSpacing.Enabled = true;

            }
        }
         * */
    }
}
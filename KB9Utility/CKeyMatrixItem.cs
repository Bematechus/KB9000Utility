using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace KB9Utility
{
    public class CKeyMatrixItem
    {

        //private DiagramKeyMatrix.MatrixType _MatrixType = DiagramKeyMatrix.MatrixType.Keys_2x4;
        //public DiagramKeyMatrix.MatrixType Matrix
        //{
        //    get
        //    {
        //        return _MatrixType;
        //    }
        //    set
        //    {
        //        _MatrixType = value;
        //    }
        //}

        private string _Text = "";
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
            }
        }

        private Image _DemoImage = null;
        public Image DemoImage
        {
            get
            {
                return _DemoImage;
            }
            set
            {
                _DemoImage = value;
            }
        }

        public CKeyMatrixItem()
        {

        }
        //public CKeyMatrixItem(DiagramKeyMatrix.MatrixType matrix, string text, Image img)
        //{
        //    SetItem(matrix, text, img);
        //}

        //public void SetItem(DiagramKeyMatrix.MatrixType matrix, string text, Image img)
        //{
        //    this.Matrix = matrix;
        //    this.Text = text;
        //    this.DemoImage = img;
        //}
    }
}

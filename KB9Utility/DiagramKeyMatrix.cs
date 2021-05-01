using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;

namespace KB9Utility
{
    /************************************************************************/
    /* 
     * A new feature to KB9000, add key matrix. Idea will be if you use this
        function to draw key(s), it will draw multiple keys.

        1.       Put it under keys-> Key Matrix->  Add New Key Matrix. When select
        this option, pop up a windows which like selecting KDS panel display mode
        with available modes(To be define how many ¡°mode¡±we need). 

        2.       In the Windows, user also be able to define the their own matrix (M
        x N) keys.

        3.       In the windows, user also be able to define the keys¡¯ row spacing
        and keys¡¯ column spacing. 

        4.       So if user choose 2 X 3 with, row space 1, column spacing 2, it
        will be like the picture below. And the generated keys will be ¡°auto-fit¡±
        the max box draw by the mouse.
     */
    /************************************************************************/
    public class DiagramKeyMatrix: DiagramEntity
    {

        //public delegate void EventSubEntityPropertiesChanged(DiagramEntity entity, bool bPausedEvent, bool bJustRefreshPropertiesGrid);
        //public EventSubEntityPropertiesChanged OnSubEntityPropertiesChanged;
        /*
        public enum MatrixType
        {
            //Custom = -1,
            Keys_1x1,
            Keys_1x2,
            Keys_1x3,
            Keys_1x4,
            Keys_1x5,
            Keys_1x6,
            Keys_1x7,
            Keys_1x8,

            Keys_2x1,
            Keys_2x2,
            Keys_2x3,
            Keys_2x4,
            Keys_2x5,
            Keys_2x6,
            Keys_2x7,
            Keys_2x8,

            Keys_3x1,
            Keys_3x2,
            Keys_3x3,
            Keys_3x4,
            Keys_3x5,
            Keys_3x6,
            Keys_3x7,
            Keys_3x8,

            Keys_4x1,
            Keys_4x2,
            Keys_4x3,
            Keys_4x4,
            Keys_4x5,
            Keys_4x6,
            Keys_4x7,
            Keys_4x8,

            Keys_5x1,
            Keys_5x2,
            Keys_5x3,
            Keys_5x4,
            Keys_5x5,
            Keys_5x6,
            Keys_5x7,
            Keys_5x8,

            Keys_6x1,
            Keys_6x2,
            Keys_6x3,
            Keys_6x4,
            Keys_6x5,
            Keys_6x6,
            Keys_6x7,
            Keys_6x8,

            Keys_7x1,
            Keys_7x2,
            Keys_7x3,
            Keys_7x4,
            Keys_7x5,
            Keys_7x6,
            Keys_7x7,
            Keys_7x8,

            Keys_8x1,
            Keys_8x2,
            Keys_8x3,
            Keys_8x4,
            Keys_8x5,
            Keys_8x6,
            Keys_8x7,
            Keys_8x8,
            
        }
        */
        //private MatrixType _Matrix = MatrixType.Keys_2x2;
        //[Browsable(false)]
        //public MatrixType Matrix
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

        //[DisplayName("Key bounds(mm)")]
        //[Browsable(true)]
        //[Category("Key")]
        //[ReadOnly(false)]
        //[TypeConverter(typeof(RectangleFConverter))]
        //public new RectangleF MMBounds
        //{
        //    get
        //    {
        //        return base.MMBounds;

        //        //Rectangle rt = this.PhyBounds;
        //        //rt.X -= KB9Const.UNITS_MARGIN;
        //        //rt.Y -= KB9Const.UNITS_MARGIN;
        //        //return ConvertUnit2MM(rt);


        //    }
        //    set
        //    {
        //        base.MMBounds = value;

        //        //RectangleF rt = value;
        //        //rt.X += KB9Const.MM_MARGIN;
        //        //rt.Y += KB9Const.MM_MARGIN;
        //        //this.PhyBounds = ConvertMM2Unit(rt);


        //    }
        //}

        protected void OnSubEntityPropertiesChanged(DiagramEntity entity, bool bPausedEvent, bool bJustRefreshPropertiesGrid)
        {
            if (this.ParentContainer != null)
            {
                if (this.ParentContainer.ParentEditor!= null)
                {
                    if (this.ParentContainer.ParentEditor.GetInteractMode() != DiagramEditor.MouseMode.MODE_NONE)
                        return;
                }
            }
            if (this.OnEntityPropertiesChanged != null)
                this.OnEntityPropertiesChanged(entity, bPausedEvent, bJustRefreshPropertiesGrid);
        }

        protected void OnSubEntityPropertiesWillChange(DiagramEntity entity, bool bPausedEvent, bool bJustRefreshPropertiesGrid)
        {
            if (this.ParentContainer != null)
            {
                if (this.ParentContainer.ParentEditor != null)
                {
                    if (this.ParentContainer.ParentEditor.GetInteractMode() != DiagramEditor.MouseMode.MODE_NONE)
                        return;
                }
            }
            if (this.OnEntityPropertiesBeforeChange != null)
                this.OnEntityPropertiesBeforeChange(entity, bPausedEvent, bJustRefreshPropertiesGrid);
        }

        public const string KEY_MATRIX = "Matrix";
        public DiagramKeyMatrix()
        {
            this.MinimumSize = new Size(100, 100);
            this.TypeName = KEY_MATRIX;
            build_all_entities();
        }

        private int _Rows = 2;
        [Browsable(false)]
        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                if (_Rows != value)
                {


                    _Rows = value;
                    build_all_entities();
                }
            }
        }

        private int _Cols = 2;
        [Browsable(false)]
        public int Cols
        {
            get
            {
                return _Cols;
            }
            set
            {
                if (_Cols != value)
                {


                    _Cols = value;
                    build_all_entities();
                }
            }
        }
        private const int DEFAULT_ROW_SPACING = 1;

        private int _RowSpacing = DEFAULT_ROW_SPACING;
        [Browsable(false)]
        public int RowSpacing
        {
            get
            {
                return _RowSpacing;
            }
            set
            {
                _RowSpacing = value;
            }
        }


        private const int DEFAULT_COL_SPACING = 1;
        private int _ColSpacing = DEFAULT_COL_SPACING;
        [Browsable(false)]
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
        /*
        private void redefine_rows_cols()
        {
            
            switch (this.Matrix)
            {

            
            case MatrixType.Custom:
                {
                    
                }
                break;
            case MatrixType.Keys_1x1:
                {
                    this.Rows = 1;
                    this.Cols = 1;
                    this.RowSpacing = 0;
                    this.ColSpacing = 0;
                } 
                break;
            case MatrixType.Keys_1x2:
                {
                    this.Rows = 1;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                } 
                break;
            case MatrixType.Keys_1x3:
                {
                    this.Rows = 1;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
           case MatrixType.Keys_1x4:
                {
                    this.Rows = 1;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                } 
                break;
            case MatrixType.Keys_1x5:
                {
                    this.Rows = 1;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_1x6:
                {
                    this.Rows = 1;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_1x7:
                {
                    this.Rows = 1;
                    this.Cols =7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_1x8:
                {
                    this.Rows = 1;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;

            case MatrixType.Keys_2x1:
                {
                    this.Rows = 2;
                    this.Cols = 1;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_2x2:
                {
                    this.Rows = 2;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_2x3:
                {
                    this.Rows = 2;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_2x4:
                {
                    this.Rows = 2;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_2x5:
                {
                    this.Rows = 2;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_2x6:
                {
                    this.Rows = 2;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_2x7:
                {
                    this.Rows = 2;
                    this.Cols = 7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_2x8:
                {
                    this.Rows = 2;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;

            case MatrixType.Keys_3x1:
                {
                    this.Rows = 3;
                    this.Cols = 1;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_3x2:
                {
                    this.Rows = 3;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_3x3:
                {
                    this.Rows = 3;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_3x4:
                {
                    this.Rows = 3;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_3x5:
                {
                    this.Rows = 3;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_3x6:
                {
                    this.Rows = 3;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_3x7:
                {
                    this.Rows = 3;
                    this.Cols = 7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_3x8:
                {
                    this.Rows = 3;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;

            case MatrixType.Keys_4x1:
                {
                    this.Rows = 4;
                    this.Cols = 1;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_4x2:
                {
                    this.Rows = 4;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_4x3:
                {
                    this.Rows = 4;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_4x4:
                {
                    this.Rows = 4;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_4x5:
                {
                    this.Rows = 4;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_4x6:
                {
                    this.Rows = 4;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_4x7:
                {
                    this.Rows = 4;
                    this.Cols = 7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_4x8:
                {
                    this.Rows = 4;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;

            case MatrixType.Keys_5x1:
                {
                    this.Rows = 5;
                    this.Cols = 1;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_5x2:
                {
                    this.Rows = 5;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_5x3:
                {
                    this.Rows = 5;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_5x4:
                {
                    this.Rows = 5;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_5x5:
                {
                    this.Rows = 5;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_5x6:
                {
                    this.Rows = 5;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_5x7:
                {
                    this.Rows = 5;
                    this.Cols = 7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_5x8:
                {
                    this.Rows = 5;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;

            case MatrixType.Keys_6x1:
                {
                    this.Rows = 6;
                    this.Cols = 1;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_6x2:
                {
                    this.Rows = 6;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_6x3:
                {
                    this.Rows = 6;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_6x4:
                {
                    this.Rows = 6;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_6x5:
                {
                    this.Rows = 6;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_6x6:
                {
                    this.Rows = 6;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_6x7:
                {
                    this.Rows = 6;
                    this.Cols = 7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_6x8:
                {
                    this.Rows = 6;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }

                break;
            case MatrixType.Keys_7x1:
                {
                    this.Rows = 7;
                    this.Cols = 1;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_7x2:
                {
                    this.Rows = 7;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_7x3:
                {
                    this.Rows = 7;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_7x4:
                {
                    this.Rows = 7;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_7x5:
                {
                    this.Rows = 7;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_7x6:
                {
                    this.Rows = 7;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_7x7:
                {
                    this.Rows = 7;
                    this.Cols = 7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_7x8:
                {
                    this.Rows = 7;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;

            case MatrixType.Keys_8x1:
                {
                    this.Rows = 8;
                    this.Cols = 1;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_8x2:
                {
                    this.Rows = 8;
                    this.Cols = 2;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_8x3:
                {
                    this.Rows = 8;
                    this.Cols = 3;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_8x4:
                {
                    this.Rows = 8;
                    this.Cols = 4;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_8x5:
                {
                    this.Rows = 8;
                    this.Cols = 5;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_8x6:
                {
                    this.Rows = 8;
                    this.Cols = 6;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_8x7:
                {
                    this.Rows = 8;
                    this.Cols = 7;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            case MatrixType.Keys_8x8:
                {
                    this.Rows = 8;
                    this.Cols = 8;
                    this.RowSpacing = DEFAULT_ROW_SPACING;
                    this.ColSpacing = DEFAULT_COL_SPACING;
                }
                break;
            default:
                    {
                        this.Rows = 2;
                        this.Cols = 2;
                        this.RowSpacing = DEFAULT_ROW_SPACING;
                        this.ColSpacing = DEFAULT_COL_SPACING;
                    }
                    break;

            }
        }*/

        private void build_all_entities()
        {
           // redefine_rows_cols();

            
            build_all_keys_matrix();

        }

        private void build_all_keys_matrix()
        {
            int ToCount = this.Rows * this.Cols;
            int FromCount = this.Entities.Count;

            //this.Entities.Clear();
            int ncount =ToCount - FromCount;
            if (ncount == 0)
                return;
            if (ncount >= 0)
            {


                for (int i = 0; i < ncount; i++)
                {
                    DiagramEntity entity = new DiagramKeyArea();
                    entity.OnEntityPropertiesChanged += new DiagramEntity.EventEntityPropertiesChanged(OnSubEntityPropertiesChanged);
                    entity.OnEntityPropertiesBeforeChange += new DiagramEntity.EventEntityPropertiesBeforeChange(OnSubEntityPropertiesWillChange);

                    this.Entities.Add(entity);
                }
            }
            else
            {
                ncount = Math.Abs(ncount);
                this.Entities.RemoveRange(0, ncount);
            }
        }

        /************************************************************************/
        /* 
         * it contains entities, save to this list
         */
        /************************************************************************/
        private List<DiagramEntity> _Entities = new List<DiagramEntity>();
        [Browsable(false)]
        public List<DiagramEntity> Entities
        {
            get
            {
                return _Entities;
            }
        }
        

        public override bool isEqual(DiagramEntity key)
        {
            if (!(key is DiagramKeyMatrix))
                return false;
            if (!base.isEqual(key)) return false;

            DiagramKeyMatrix k = (DiagramKeyMatrix)key;
            List<DiagramEntity> entities = this.Entities;
            List<DiagramEntity> items = k.Entities;

            List<object> mineCopies = new List<object>();
            for (int i = 0; i < entities.Count; i++)
            {
                mineCopies.Add(entities[i]);
            }

            for (int i = 0; i < items.Count; i++)
            {
                object obj = findSameItem(items[i], mineCopies);
                if (obj != null)
                {
                    mineCopies.Remove(obj);
                }

            }
            if (mineCopies.Count > 0)
                return false;
            return true;

        }

        private object findSameItem(object obj, List<object> items)
        {
            if (obj is DiagramKeyArea)
            {
                DiagramKeyArea c = (DiagramKeyArea)obj;
                for (int i = 0; i < items.Count; i++)
                {
                    if (!(items[i] is DiagramKeyArea)) continue;
                    DiagramKeyArea d = (DiagramKeyArea)items[i];
                    if (c.isEqual(d))
                        return d;
                }
            }
            else if (obj is DiagramKeySlideH)
            {
                DiagramKeySlideH c = (DiagramKeySlideH)obj;
                for (int i = 0; i < items.Count; i++)
                {
                    if (!(items[i] is DiagramKeySlideH)) continue;
                    DiagramKeySlideH d = (DiagramKeySlideH)items[i];
                    if (c.isEqual(d))
                        return d;
                }
            }
            else if (obj is DiagramKeySlideV)
            {
                DiagramKeySlideV c = (DiagramKeySlideV)obj;
                for (int i = 0; i < items.Count; i++)
                {
                    if (!(items[i] is DiagramKeySlideV)) continue;
                    DiagramKeySlideV d = (DiagramKeySlideV)items[i];
                    if (c.isEqual(d))
                        return d;
                }
            }

            return null;
        }

        public override DiagramEntity Clone()
        {


            DiagramKeyMatrix obj = new DiagramKeyMatrix();
            obj.Copy(this);
            return obj;

        }
        public override void Copy(DiagramEntity obj)
        {
            base.Copy(obj);
            DiagramKeyMatrix k = (DiagramKeyMatrix)obj;
            this.ParentContainer = (obj.ParentContainer);
            this.ContentModified = k.ContentModified;
            this.Rows = k.Rows;
            this.Cols = k.Cols;
            this.RowSpacing = k.RowSpacing;
            this.ColSpacing = k.ColSpacing;
           // this.Matrix = k.Matrix;

            this.Entities.Clear();
            for (int i=0; i< k.Entities.Count; i++)
            {
                //k.Entities[i].Clone()
                DiagramEntity entity = k.Entities[i].Clone();
                entity.OnEntityPropertiesChanged += new EventEntityPropertiesChanged(OnSubEntityPropertiesChanged);
                entity.OnEntityPropertiesBeforeChange += new DiagramEntity.EventEntityPropertiesBeforeChange(OnSubEntityPropertiesWillChange);
                this.Entities.Add(entity);

            }


        }

        /// <summary>
        /// ungroup the matrix entities to main editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        override public void OnMatrixUngroup_Click(object sender, EventArgs e)
        {
            for (int i=0; i< this.Entities.Count; i++)
            {
                
                DiagramEntity entity = this.Entities[i].Clone();
                
                this.ParentContainer.Add(entity);

            }
            this.Entities.Clear();
            this.ParentContainer.Remove(this);
            
        }

        protected void DrawBackground(Graphics g, Rectangle rect, double zoom)
        {
            SolidBrush br = new SolidBrush(this.ParentContainer.ParentEditor.BackColor);

            Rectangle rt = rect;
            g.FillRectangle(br, rt);
            
        }

        protected void DrawOverlappedArea2(Graphics g, Rectangle rectEntityClient)
        {
            List<Rectangle> lst = new List<Rectangle>();
            int ncount = this.ParentContainer.GetOverlappedRect2(this, lst);
            if (ncount <= 0)
                return;
            Rectangle rtBounds = this.Bounds;
            rtBounds = ZoomRectangle(rtBounds, this.Zoom);

            for (int i = 0; i < ncount; i++)
            {
                Rectangle rt = lst[i];
                rt = ZoomRectangle(rt, this.Zoom);
                if (rt.IsEmpty) return;
                Point pt = new Point(rectEntityClient.X - rtBounds.X, rectEntityClient.Y - rtBounds.Y);
                rt.Offset(pt);
                //SolidBrush br = new SolidBrush(Color.Red);
                HatchBrush br = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Red, Color.Yellow);
                g.FillRectangle(br, rt);
            }

        }

        private void DrawEntities(Graphics g, Rectangle rectTotalVirtualWithOffsetAndZoom, double zoom, bool bToImage)
        {
           
           // int count = 0;
            DiagramEntity obj;
            for (int i = 0; i < _Entities.Count; i++)
            {
             //   while ((obj = _Entities[count++]) != null)
             //   {
                obj = _Entities[i];
                    obj.ParentContainer = this.ParentContainer;
                    obj.DrawObject(g, rectTotalVirtualWithOffsetAndZoom, zoom, bToImage);
             //   }
            }
           
        }
        
        public override void DrawObject(Graphics g, Rectangle rectTotalVirtualWithOffsetAndZoom, double zoom, bool bToImage)
        {

            this.Zoom = (zoom);

            Rectangle rect ;//= null;
            if (!bToImage)
            {

                //the old zoomrectangle can not fit the border to grid, 
                //so I write a zoomdrawingrectangle to fix this bug.
                //rect = ZoomRectangle(this.Bounds, zoom);
                rect = ZoomDrawingRectangle2(zoom);
            }
            else
                rect = ZoomRectangle(this.ImgBounds, zoom);

            rect.Offset(rectTotalVirtualWithOffsetAndZoom.Location);
            Region region = g.Clip;
            g.Clip = new Region(rect);

	        Draw( g, rect, zoom );

            DrawEntities(g, rectTotalVirtualWithOffsetAndZoom, zoom , bToImage);
            g.Clip = region;
	        if( this.Selected )
		        DrawSelectionMarkers( g, rect );

         

        }
        protected override void Draw(Graphics g, Rectangle rectVirtualEntityWithOffsetAndZoom, double zoom)
        {

            //ControlPaint.DrawButton(g, rect, ButtonState.Normal);
            DrawBackground(g, rectVirtualEntityWithOffsetAndZoom, zoom);
            
          //  DrawOverlappedArea2(g, rectVirtualEntityWithOffsetAndZoom);
          


        }

         override public Rectangle PhyBounds
         {
             get
             {
                 return base.PhyBounds;
             }
             set
             {
                 if (base.PhyBounds != value)
                 {


                     base.PhyBounds = value;
                     resize_all_entities();
                     FirePropertiesChangedEvent();
                 }
             }
         }
        /************************************************************************/
        /* 
         * resize them according to the new container's rectangle.
         * 
         */
        /************************************************************************/
        private void resize_all_entities()
        {
            if (this.Rows <= 0 ||
                this.Cols <= 0)
                return;
            if (this.Entities.Count != (this.Rows * this.Cols) )
            {
                build_all_entities();
            }
            //for (int i=0; i< this.Entities.Count ; i++)
            //{
            //    this.Entities[i].ParentContainer = this.ParentContainer;
            //}
            Rectangle rtContainer = this.PhyBounds;
            //get all rows height,
            int nrows = this.Rows;
            int[] rows_h = new int[nrows];

            if (!calculate_average(rtContainer.Height, nrows, this.RowSpacing,  rows_h))
                return;

            
            //get all cols width,

            int ncols = this.Cols;
            int[] cols_w = new int[ncols];
            if (!calculate_average(rtContainer.Width, ncols,this.ColSpacing,  cols_w))
                return;
            for (int row = 0; row<nrows; row++)
            {
                for (int col = 0; col < ncols; col++)
                {
                    int index = row * ncols + col;
                    DiagramEntity entity = this.Entities[index];

                    Rectangle rtEntity = calculate_entity_rect(rtContainer, row, col, rows_h, cols_w);
                    entity.PhyBounds = rtEntity;
                }
            }


        }

        private Rectangle calculate_entity_rect(Rectangle rtContainer, int row, int col, int[] arRowHeight, int[] arColWidth)
        {
            int x = 0;
            for (int i = 0; i < col; i++)
            {
                x += arColWidth[i];
                x += this.ColSpacing;
            }
            int y = 0;
            for (int i = 0; i < row; i++)
            {
                y += arRowHeight[i];
                y += this.RowSpacing;
            }
            int w = arColWidth[col];
            int h = arRowHeight[row];

            return new Rectangle(rtContainer.X + x, rtContainer.Y + y, w, h);

        }

        private bool calculate_average(int nTotalSize, int nCount,int nSpacing, int[] arAverageSize)
        {
            if (nCount <= 0)
                return false;
            int nrows = nCount;
            //int[] rows_h = new int[nrows];
            int nTotal = (nTotalSize - nSpacing * (nrows - 1));
            int average = nTotal / nrows;
            int nleft = nTotal % nrows;
            for (int i = 0; i < nrows; i++)
            {
                if (i < nleft)
                    arAverageSize[i] = average + 1;
                else
                    arAverageSize[i] = average;
            }
            return true;
        }

        override public bool Selected
        {

            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                if (!value)
                {
                    unselect_all();
                }
                
            }
        }
        private void unselect_all()
        {
            for (int i = 0; i < this.Entities.Count; i++)
            {
                DiagramEntity entity = this.Entities[i];
                entity.Selected = false;
            }
        }
        //override public DEHT GetHitCode(Point point, Rectangle rect)
        //{

        //    DEHT result = base.GetHitCode(point, rect);
        //    if (result == DEHT.DEHT_NONE)
        //        return result;
        //    for (int i = 0; i < this.Entities.Count; i++)
        //    {
        //        DiagramEntity entity = this.Entities[i];
        //        entity.Selected = false;
        //    }
        //    for (int i=0; i< this.Entities.Count; i++)
        //    {
        //        DiagramEntity entity = this.Entities[i];
        //        if (entity.GetHitCode(point) == DiagramEntity.DEHT.DEHT_BODY)
        //        {
        //            entity.Selected = true;
        //        }
        //    }

        //    return result;
        //}

        public DiagramEntity hit_child(Point point)
        {
            unselect_all();
            for (int i=0; i< this.Entities.Count; i++)
            {
                DiagramEntity entity = this.Entities[i];
                if (entity.GetHitCode(point) == DiagramEntity.DEHT.DEHT_BODY)
                {
                    entity.Selected = true;
                    //MessageBox.Show(i.ToString());
                    return entity;
                }
            }
            return null;
        }

        public DiagramEntity get_selected_entity()
        {
            for (int i = 0; i < this.Entities.Count; i++)
            {
                DiagramEntity entity = this.Entities[i];
                if (entity.Selected)
                    return entity;
                
            }
            return null;
        }
        /// <summary>
        /// append new overlapped rect to list
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstOverlapped"></param>
        /// <returns></returns>

        public int GetOverlappedRect2(DiagramEntity entity, List<Rectangle> lstOverlapped)
        {
            if (entity == this)
                return 0;

            Rectangle rect = entity.Bounds;
            Rectangle rtReturn = new Rectangle(0, 0, 0, 0);
            for (int i = 0; i < this.Entities.Count; i++)
            {
                DiagramEntity d = this.Entities[i];// GetAt(i);
                if (d == entity) //its self, skip
                    continue;
                
                Rectangle rt = d.Bounds;
                if (rect.IntersectsWith(rt))
                {
                    Rectangle r = rect;
                    r.Intersect(rt);
                    lstOverlapped.Add(r);



                }
            }
            return lstOverlapped.Count;
        }

         public bool  CheckInternalOverlapped()
         {

             

             List<Rectangle> ar = new List<Rectangle>();

             for (int i = 0; i < this.Entities.Count; i++)
             {
                 DiagramEntity d = this.Entities[i];// GetAt(i);
               
                 ar.Clear();
                 if (GetOverlappedRect2(d, ar) > 0)
                     return true;
             }
             return false;
         }


        /************************************************************************/
        /* 
         * export data to xml file.
         */
        /************************************************************************/
        public override void Export(int nLayerIndex, CLCIXML xml)
        {
            //create this matrix
            base.Export(nLayerIndex, xml);

            //export its entities
            xml.set_attribute("rows", this.Rows.ToString());
            xml.set_attribute("rowspacing", this.RowSpacing.ToString());
            xml.set_attribute("cols", this.Cols.ToString());
            xml.set_attribute("colspacing", this.ColSpacing.ToString());

            xml.set_attribute("count", this.Entities.Count.ToString());

            for (int i = 0; i < this.Entities.Count; i++ )
            {
                DiagramEntity entity = this.Entities[i];
                entity.Export(i, xml);
            }
            xml.back_to_parent();

        }
        /// <summary>
        /// Load data from xml file.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public override bool FromXml(CLCIXML xml)
        {
            //load my entity properites
            if (!base.FromXml(xml))
                return false;
            //load sub entities
            string s = "";

            xml.get_attribute("rows", ref s);
            this.Rows = Util.string2int(s,0);

            xml.get_attribute("rowspacing",ref s);
            this.RowSpacing = Util.string2int(s, 0);

            xml.get_attribute("cols",ref s);
            this.Cols = Util.string2int(s, 0);

            xml.get_attribute("colspacing",ref s);
            this.ColSpacing = Util.string2int(s, 0);


            xml.get_attribute("count", ref s);
            int ncount = Util.string2int(s, 0);

            this.Entities.Clear();
            for (int i=0; i< ncount; i++)
            {
                
                s = "k" + i.ToString();
                if (!xml.get_first_group(s))
                    continue;
                s = "";
                xml.get_attribute("keytype", ref s);
                DiagramEntity k = null;
                switch (s)
                {
                    case DiagramKey.KEY_BUTTON:// "BUTTON":
                        {


                            k = new DiagramKeyArea();
                            k.FromXml(xml);
                        }
                        break;
                    case DiagramKey.KEY_SLIDEV:
                        {


                            k = new DiagramKeySlideV();
                            k.FromXml(xml);
                        }
                        break;
                    case DiagramKey.KEY_SLIDEH:
                        {


                            k = new DiagramKeySlideH();
                            k.FromXml(xml);
                        }
                        break;
                    default:
                        continue;
                }

                xml.back_to_parent();
                k.OnEntityPropertiesChanged += new EventEntityPropertiesChanged(OnSubEntityPropertiesChanged);
                k.OnEntityPropertiesBeforeChange += new DiagramEntity.EventEntityPropertiesBeforeChange(OnSubEntityPropertiesWillChange);
                this.Entities.Add(k);
            }
          
         
            return true;
        }

        public override string ExportCVS()
        {

            string strReturn = base.ExportCVS();
            for (int i = 0; i < this.Entities.Count; i++)
            {
                strReturn += ((DiagramKey )(this.Entities[i])).CreateCVS();

            }
            return strReturn;
            //    strReturn += this.KeyCode.ToCSV(this.CapsEffect);


            //strReturn += KB9Const.LF;
            //return strReturn;
        }

        public void set_all_child_beep_duration(int nDuration)
        {
            for (int i = 0; i < this.Entities.Count; i++)
            {
                ((DiagramKey)this.Entities[i]).BeepDuration = nDuration;
            }
        }

        override public string getContentText()
        {
            DiagramEntity entity = this.get_selected_entity();
            if (entity == null) return "";
            return entity.getContentText();

        }

        public DiagramEntity findSamePositionEntity(DiagramEntity entity)
        {
            for (int i = 0; i < this.Entities.Count; i++)
            {
                if (((DiagramKey)this.Entities[i]).Bounds.Equals(entity.Bounds))
                {
                    return this.Entities[i];
                }

            }
            return null;
        }
    }
}

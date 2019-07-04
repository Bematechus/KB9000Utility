using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace KB9Utility
{
    public class UndoItem
    {
        private List<object> _SnapshotItems = new List<object>();	// Object array
        public List<object> SnapshotItems
        {
            get { return _SnapshotItems; }
            set { _SnapshotItems = value; }
        }

        private int _LastFocusedEditBoxIndex = 0;//for undo the SlideV/SlideH
        public int LastFocusedEditBoxIndex
        {
            get { return _LastFocusedEditBoxIndex; }
            set { _LastFocusedEditBoxIndex = value; }
        }
        //private Size _EditorSize;		// Virtual editor size
        //public Size EditorSize
        //{
        //    get { return _EditorSize; }
        //    set { _EditorSize = value;}
        //}
        private bool _EditorPropertiesEnabled = false;
        public bool EditorPropertiesEnabled
        {
            get
            {
                return _EditorPropertiesEnabled;
            }
            set
            {
                _EditorPropertiesEnabled = value;
            }
        }
        private Color _EditorBackColor  =Color.White;
        public Color EditorBackColor
        {
            get
            {
                return _EditorBackColor;
            }
            set
            {
                _EditorBackColor = value;
            }
        }

        private Color _EditorBorderColor = Color.White;
        public Color EditorBorderColor
        {
            get { return _EditorBorderColor; }
            set
            {
                _EditorBorderColor = value;
                
            }
        }

        private int _EditorSensitivity = KB9Const.DEFAULT_Sensitivity;
        public int EditorSensitivity
        {
            get { return _EditorSensitivity; }
            set
            {
                
                    _EditorSensitivity = value;
                
            }
        }

        private int _EditorInterCharDelay = KB9Const.DEFAULT_InterCharDelay;
        public int EditorInterCharDelay
        {
            get { return _EditorInterCharDelay; }
            set
            {

                _EditorInterCharDelay = value;
               
            }
        }

        private int _EditorTouchDelay = KB9Const.DEFAULT_TouchDelay;
        public int EditorTouchDelay
        {
            get { return _EditorTouchDelay; }
            set
            {

                _EditorTouchDelay = value;
                
            }
        }

        private bool _RecordForFocus = false; //for undo focus
        public bool RecordForFocus
        {
            get { return _RecordForFocus; }
            set
            {
                _RecordForFocus = value;
            }
        }


        /// <summary>
        /// check if give item is same as mine
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool isEqualWithoutFunctionCheck(UndoItem item)
        {
            //if (this.EditorPropertiesEnabled != item.EditorPropertiesEnabled)
           //     return false;
            if (this.EditorBackColor != item.EditorBackColor)
                return false;
            if (this.EditorBorderColor != item.EditorBorderColor)
                return false;
            if (this.EditorInterCharDelay != item.EditorInterCharDelay)
                return false;
            if (this.EditorSensitivity != item.EditorSensitivity)
                return false;
            if (this.EditorTouchDelay != item.EditorTouchDelay)
                return false;
            //if (this.RecordForFocus != item.RecordForFocus)
            //    return false;

            List<object> mine = this.SnapshotItems;


            List<object> items = item.SnapshotItems;

            if (mine.Count != items.Count)
                return false;

            List<object> entries = new List<object>();
            for (int i = 0; i < mine.Count; i++)
            {
                entries.Add(mine[i]);
            }

            for (int i = 0; i < items.Count; i++)
            {
                object obj = findSameItem(items[i], entries);
                if (obj != null)
                {
                    entries.Remove(obj);
                }

            }
            if (entries.Count > 0)
                return false;
            return true;



        }

        /// <summary>
        /// check if give item is same as mine
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool isEqual(UndoItem item)
        {
            if (!isEqualWithoutFunctionCheck(item))
                return false;

            if (this.RecordForFocus != item.RecordForFocus)
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
            else if (obj is DiagramKeyMatrix)
            {
                DiagramKeyMatrix c = (DiagramKeyMatrix)obj;
                for (int i = 0; i < items.Count; i++)
                {
                    if (!(items[i] is DiagramKeyMatrix)) continue;
                    DiagramKeyMatrix d = (DiagramKeyMatrix)items[i];
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>hightlight entities count</returns>
        public int getHightlightCount()
        {
            List<object> items = this.SnapshotItems;

            int ncount = 0;
            for (int i=0; i< items.Count; i++)
            {
                DiagramEntity entity =(DiagramEntity) items[i];
                if (entity.Selected) ncount++;
            }
            return ncount;

            
        }

        public DiagramEntity getHightlightEntity()
        {
            List<object> items = this.SnapshotItems;

            
            for (int i = 0; i < items.Count; i++)
            {
                DiagramEntity entity = (DiagramEntity)items[i];
                if (entity.Selected)
                {
                    if (entity is DiagramKeyMatrix)
                    {
                        return ((DiagramKeyMatrix)entity).get_selected_entity();
                    }
                    else
                    {
                        return entity;
                    }
                    
                }
            }
            return null;
        }

        public DiagramEntity findSamePositionEntity(DiagramEntity entity)
        {
            List<object> items = this.SnapshotItems;

            for (int i = 0; i < items.Count; i++)
            {
                DiagramEntity item = (DiagramEntity)items[i];
                if (item is DiagramKeyMatrix)
                {
                    DiagramEntity d = ((DiagramKeyMatrix)item).findSamePositionEntity(entity);
                    if (d != null) return d;
                }
                else
                {
                    if (item.Bounds.Equals(entity.Bounds)) return item;
                }
            }
            return null;
        }

        public UndoItem clone()
        {
            UndoItem undo = new UndoItem();
            undo.EditorBackColor = this.EditorBackColor;
            undo.EditorBorderColor = this.EditorBorderColor;
            undo.EditorInterCharDelay = this.EditorInterCharDelay;
            undo.EditorPropertiesEnabled = this.EditorPropertiesEnabled;
            undo.EditorSensitivity = this.EditorSensitivity;
            undo.EditorTouchDelay = this.EditorTouchDelay;
            undo.RecordForFocus = this.RecordForFocus;

             List<object> items = this.SnapshotItems;

             for (int i = 0; i < items.Count; i++)
             {
                 undo.SnapshotItems.Add(((DiagramEntity)items[i]).Clone());
             }
             return undo;
        }
        public void hightlightNothing()
        {
            List<object> items = this.SnapshotItems;

            for (int i = 0; i < items.Count; i++)
            {
                ((DiagramEntity)items[i]).Selected = false;
            }
            
        }

         public void focusSamePositionEntity(DiagramEntity entity)
         {
             List<object> items = this.SnapshotItems;

             for (int i = 0; i < items.Count; i++)
             {
                 DiagramEntity item = (DiagramEntity)items[i];
                 if (item is DiagramKeyMatrix)
                 {
                     DiagramEntity d = ((DiagramKeyMatrix)item).findSamePositionEntity(entity);
                     if (d != null)
                     {
                         item.Selected = true;
                         d.Selected = true;
                     }
                 }
                 else
                 {
                     if (item.Bounds.Equals(entity.Bounds))
                     {
                         item.Selected = true;
                     }
                 }
             }
             
         }

        /// <summary>
        /// check anything, include focus, ....
        /// </summary>
        /// <param name="undo"></param>
        /// <returns></returns>
         public bool isAnythingSame(UndoItem undo)
         {
             if (this.getHightlightCount() != undo.getHightlightCount())
                 return false;

             if (this.SnapshotItems.Count != undo.SnapshotItems.Count)
                 return false;
             DiagramEntity entity = this.getHightlightEntity();
             DiagramEntity item = undo.getHightlightEntity();
             if (entity == item) //all null
             {
                 return isEqual(undo);
                 
             }
             if (entity != null && item != null)
             {
                if (!entity.isEqual(item))
                     return false;
             }
             return isEqual(undo);


         }

    }
}

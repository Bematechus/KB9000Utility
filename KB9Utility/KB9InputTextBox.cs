using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
/************************************************************************/
/* 
 * We need a text box to test KB9000 inputed key code.
 * I don't want to disturb the KB9Textbox, that was for setting.
 * This one is just for testing input.
 */
/************************************************************************/
namespace KB9Utility
{
    public partial class KB9InputTextBox : UserControl
    {
      
        private enum MOVE_TYPE
        {
            None=0,
            Select,
            Resize,
        }

        List<string> m_Data = new List<string>();
              
        Point _Selection = new Point(0, 0); //cx: from, cy: to
        Label _CaretControl = new Label();
        Point _Offset = new Point(0, 0);

        List<List<string>> m_Undo = new List<List<string>>();

        protected int _oldCaret;     // Backup of the previous caret if it changes
        
        protected Timer caretTimer;             // Timer used for caret blinking

        public delegate void EventOnTextChanged(object sender);
        public event EventOnTextChanged OnTextChanged;

        public delegate void EventOnSelectionChanged(object sender);
        public event EventOnSelectionChanged OnSelectionChanged;


        VScrollBar m_verticalScrollBar = new VScrollBar();


        private  void Fire_TextChangedEvent()
        {
            if (!this.EnableTextChangedEvent)
                return;
            if (OnTextChanged != null)
                OnTextChanged(this);
            update_vertical_scrollbar();
        }

        private void Fire_SelectionChangedEvent()
        {
           
            if (OnSelectionChanged != null)
                OnSelectionChanged(this);
            
        }


        private bool _EnableTextChangedEvent = true;
        public bool EnableTextChangedEvent
        {
            get 
            {
                return _EnableTextChangedEvent;
            }
            set
            {
                _EnableTextChangedEvent = value;
            }
        }

        public KB9InputTextBox()
        {
            InitializeComponent();
            m_Data.Clear();

            this.TabStop = true;

            this.SetStyle(ControlStyles.UserPaint
                | ControlStyles.UserMouse
                | ControlStyles.ResizeRedraw
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.Opaque
                | ControlStyles.CacheText
                | ControlStyles.EnableNotifyMessage
                
                //| ControlStyles.OptimizedDoubleBuffer
                , true);
            this.SetStyle(ControlStyles.ContainerControl
                | ControlStyles.OptimizedDoubleBuffer
                , false);
            this.DoubleBuffered = true;
            this.UpdateStyles();

           // this.Font = ColorTextBox.DefaultFont;
            //this.lnFont = ColorTextBox.DefaultLineNumberFont;
            // reset default Control values:
           // this.BorderStyle = BorderStyle.FixedSingle;
            this.AllowDrop = true;
            this.Text = String.Empty;

            _CaretControl.Padding = new Padding(0, 0, 0, 0);
            _CaretControl.Margin = new Padding(0, 0, 0, 0);
            _CaretControl.Text = "";
            _CaretControl.BackColor = Color.Black;
            _CaretControl.Size = new Size(1, this.LineHeight);
            this.Controls.Add(_CaretControl);

            this.ShowCaret = false;
            this.caretTimer = new Timer();
            this.caretTimer.Interval = 500;
            this.caretTimer.Tick += new EventHandler(caretTimer_Tick);
            this.caretTimer.Start();

            this._Offset.X = 0;
            this._Offset.Y = 0;

            this.Controls.Add(m_verticalScrollBar);
            m_verticalScrollBar.ValueChanged += new EventHandler(m_verticalScrollBar_ValueChanged);
            //if (!this.ScrollBarVisible)
                m_verticalScrollBar.Visible = false;
            located_vertical_scrollbar();
            update_vertical_scrollbar();
            
        }

        void m_verticalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            int ncurrentrow = Math.Abs(_Offset.Y) / this.LineHeight;
            int nval = Math.Abs(m_verticalScrollBar.Value);
            if (nval > ncurrentrow)
                do_scroll_up();
            else
                do_scroll_down();
        }

        //private const int VERTICAL_BAR_WIDTH = 15;
        private void located_vertical_scrollbar()
        {
            Point pt = new Point();
            int nBarWidth = System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            pt.X = this.Width - nBarWidth;// VERTICAL_BAR_WIDTH;// m_verticalScrollBar.Width;
            pt.Y = 0;
            m_verticalScrollBar.Location = pt;
            m_verticalScrollBar.Size = new Size(nBarWidth, this.Height);
          

        }

        private void update_vertical_scrollbar()
        {
           // if (!m_verticalScrollBar.Visible)
            if (!this.ScrollBarVisible)
                return;
            int textlines = measureLines(CreateGraphics());
            
            int nTextHeight = (textlines) * this.LineHeight;
            Rectangle controlRt = this.ClientRectangle;

            
            m_verticalScrollBar.Visible =(nTextHeight > controlRt.Height);
            if (textlines <= 0)
                m_verticalScrollBar.Visible = false;

            int nControlRows = controlRt.Height / this.LineHeight - 1;
            m_verticalScrollBar.Minimum = 0;
            int n = textlines;// -nControlRows;
            if (n <=0) n=1;
            m_verticalScrollBar.Maximum = n;
            m_verticalScrollBar.SmallChange = 1;
            m_verticalScrollBar.LargeChange = 1;


        }

         private void update_vertical_scrollbar_value()
         {
             int ncurrentrow = Math.Abs(_Offset.Y) / this.LineHeight;
             m_verticalScrollBar.Value = ncurrentrow;
         }

        private bool _ScrollBarVisible = false;
        public bool ScrollBarVisible
        {
            get
            {
                return _ScrollBarVisible;
            }
            set
            {
                _ScrollBarVisible = value;
                
                 m_verticalScrollBar.Visible = _ScrollBarVisible;
                
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            located_vertical_scrollbar();
        }
        protected void caretTimer_Tick(object sender, EventArgs e)
        {
            this.invertCaret();
        }

        int _Caret = -1; //-1: last one.
        public int Caret
        {
            get 
            {
                return _Caret;
            }
            set 
            {
                //if (_Caret != value)
                {
                    _Caret = value;
                    MakeCaretLineVisible();
                    
                    invertCaret();
                    CaretStart();
                    refresh_window();
                }
                
            }
        }
        private void CaretStop()
        {
            caretTimer.Stop();
            _CaretControl.Visible = false;
        }
        private void CaretStart()
        {
            caretTimer.Start();
            _CaretControl.Visible = false;
            invertCaret();

        }

        //protected virtual void setCaret(int nPos)
        //{
        //    if (_Caret == nPos) return;
        //    _Caret = nPos;

           
        //    if (!caretTimer.Enabled) caretTimer.Start();

        //}

        ///// <summary>
        ///// This method removes the caret by setting it to null and stops the caret timer. If there is a caret 
        ///// visible this method first removes the caret.
        ///// </summary>
        //protected void removeCaret()
        //{
        //    _oldCaret = _Caret;
        //    caretTimer.Stop();
        //    _CaretControl.Visible = false;
        //}

        //private bool _ShowGripper = true;
        //public bool ShowGripper
        //{
        //    get 
        //    {
        //        return _ShowGripper;
        //    }
        //    set
        //    {
        //        _ShowGripper = value;
        //        if (!_ShowGripper)
        //        {
        //            this.Cursor = Cursors.Default;
        //        }
        //    }
        //}


        public void DrawGripper(Graphics g)
        {
            //if (!this.Focused) return;
            //if (!this.ShowGripper) return;
            //if (VisualStyleRenderer.IsElementDefined(
            //    VisualStyleElement.Status.Gripper.Normal))
            //{
            //    VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
            //    //Rectangle rectangle1 = new Rectangle((Width) - 18, (Height) - 20, 20, 20);
            //    //int nsize = 20;
            //    Rectangle rectangle1 = GetGripperRect();// new Rectangle((Width) - nsize, (Height) - nsize, nsize, nsize);
            //    renderer.DrawBackground(g, rectangle1);
            //}
        }
        private Rectangle GetGripperRect()
        {
            int nsize = 20;
            Rectangle rectangle1 = new Rectangle((this.Width) - nsize, (this.Height) - nsize, nsize, nsize);
            return rectangle1;
        }
        protected bool drawBackground(Graphics g)
        {
            //g.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
            g.Clear(this.BackColor);
            return true;
        }
        protected Rectangle GetVisibleArea()
        {
            Rectangle rt = this.ClientRectangle;
            rt.X += this.Padding.Left;
            rt.Y += this.Padding.Top;
            rt.Width -= (this.Padding.Right+this.Padding.Left);
            if (m_verticalScrollBar.Visible)
                rt.Width -= m_verticalScrollBar.Width;
            rt.Height -= (this.Padding.Bottom+this.Padding.Top);
            return rt;// this.ClientRectangle;
        }
        protected bool drawText(Graphics g)
        {
           
            Font f = this.Font;
            int nLineHeight = LineHeight;
            List<RectangleF> lstCell = new List<RectangleF>();
            GetAllCellRectangle(lstCell, get_xoffset(), get_yoffset());

            //draw all selected text bg first
            //As the measurestring is not accurated, the background overlapped the 
            //text, I have to draw background first.
            if (HasSelection())
            {
                for (int i = 0; i < m_Data.Count; i++)
                {
                    
                    if ( is_selected(i))
                    {
                        RectangleF rt = lstCell[i];
                        SolidBrush brBG = new SolidBrush(SystemColors.Highlight);
                        g.FillRectangle(brBG, rt);//
                    }
                }
            }
            //draw text
            for (int i = 0; i < m_Data.Count; i++)
            {
                string s = m_Data[i];
               
                RectangleF rt = lstCell[i];
                SolidBrush brFG = null;
                if (HasSelection() && is_selected(i))
                {
                    brFG = new SolidBrush(SystemColors.HighlightText);   
                }
                else
                {
                    brFG = new SolidBrush(this.ForeColor);
                }

                g.DrawString(s, f, brFG, rt.Location);//
                
            }
            
            return true;
        }

        private void get_selected_from_to(ref int nfrom, ref int nto)
        {
            int from = _Selection.X;
            if (from < 0) from = m_Data.Count;
            int to = _Selection.Y;
            if (to < 0) to = m_Data.Count ;

            int start = (from > to ? to : from);
            int end = (from > to ? from : to);
            
            nfrom = start;
            nto = end;

        }
        bool is_selected(int nIndex)
        {
            int start=-1, end=-1;

            get_selected_from_to(ref start, ref end);

            int nmax = end;

            if (nmax < 0) 
                nmax = m_Data.Count ;
            if (nIndex >= start && nIndex < nmax)
                return true;
            return false;
        }

        private void redraw()
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            redraw(g);
        }
        private void redraw(Graphics g)
        {
            drawBackground(g);
            drawText(g);
            DrawGripper(g);
        }
        
        protected override void OnPaint(PaintEventArgs pe)
        {
           // System.Diagnostics.Debug.Print("OnPaint");
            // TODO: Add custom paint code here
            base.OnPaint(pe);
            redraw(pe.Graphics);
            //drawBackground(pe.Graphics);
            //drawText(pe.Graphics);
            //DrawGripper(pe.Graphics);
        }

        protected int measureLines(Graphics g)
        {

            Rectangle rect = GetVisibleArea();// this.ClientRectangle;
            Font f = this.Font;

            int nLines = 0;
            float fltW = 0;

            for (int i = 0; i < m_Data.Count; i++)
            {
                SizeF sz = measureString(g, m_Data[i], f);

                if (fltW + sz.Width > rect.Width)
                {
                    nLines ++;
                    fltW = 0; 
                    
                }
                fltW += sz.Width;
                
            }
            
            return nLines+1;
        }

        protected int GetAllCellRectangle(List<RectangleF> lst, int xoffset, int yoffset)
        {
            lst.Clear();
            SizeF szResult = new Size(0, 0);
            Rectangle rect = GetVisibleArea();// this.ClientRectangle;
            Font f = this.Font;
            float x = rect.Left + xoffset, y = rect.Top + yoffset;
            Graphics g = CreateGraphics();
            int nLineHeight = this.LineHeight;//

            RectangleF rectResult = new RectangleF(); //it will been return

            for (int i = 0; i < m_Data.Count; i++)
            {
                SizeF sz = measureString(g, m_Data[i], f);

                if (x + sz.Width > rect.Width)
                {
                    x = xoffset;
                    y += nLineHeight;

                }
                rectResult.X = x;
                rectResult.Y = y;
                rectResult.Width = sz.Width;
                rectResult.Height = sz.Height;
                lst.Add(new RectangleF(rectResult.Location, rectResult.Size));

                x += sz.Width;

            }
            return lst.Count;

        }

        //protected int GetAllCellRectangle(List<RectangleF> lst, int xoffset, int yoffset)
        //{
        //    lst.Clear();
        //    SizeF szResult = new Size(0, 0);
        //    Rectangle rect = GetVisibleArea();// this.ClientRectangle;
        //    Font f = this.Font;
        //    float x = rect.Left + xoffset, y = rect.Top + yoffset;
        //    Graphics g =  CreateGraphics();
        //    int nLineHeight = this.LineHeight;//

        //    RectangleF rectResult = new RectangleF(); //it will been return

        //    string s = string.Empty;
        //    string strPrev = string.Empty;
        //    for (int i = 0; i < m_Data.Count; i++)
        //    {
        //        s += m_Data[i];
        //        SizeF sz = measureString(g, s, f);

        //        if ( sz.Width > rect.Width)
        //        {
        //            x = xoffset;
        //            y += nLineHeight;
        //            s = m_Data[i];
        //            sz = measureString(g, s, f);

        //        }
        //        rectResult.X = x;
        //        rectResult.Y = y;
        //        rectResult.Width = sz.Width - x + xoffset;
        //        rectResult.Height = sz.Height;
        //        lst.Add(new RectangleF(rectResult.Location, rectResult.Size));

        //        x = (sz.Width + xoffset);

        //    }
        //    return lst.Count;

        //}


        protected RectangleF GetStringRectangle(int nIndex, int xoffset, int yoffset)
        {
           

            RectangleF rectResult = new RectangleF(); //it will been return

            List<RectangleF> lst = new List<RectangleF>();
            GetAllCellRectangle(lst, xoffset, yoffset);
            if (nIndex >= 0 && nIndex < lst.Count)
                return lst[nIndex];

            if (lst.Count > 0)
                rectResult = lst[lst.Count - 1];
            if (nIndex <0 || nIndex >= m_Data.Count)
            {
                rectResult.X = xoffset + rectResult.Right+1;
                //rectResult.Y = yoffset;
                //rectResult.Height = this.LineHeight;
                rectResult.Width = 2;
            }
            
            return rectResult;
        }

        protected SizeF measureString(Graphics g, string s, Font f)
        {
            g.PageUnit = GraphicsUnit.Pixel;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            SizeF sz =  g.MeasureString(s, f, 99999, StringFormat.GenericTypographic);
            if (s.Length > 1) //as with [] chars, they are too closed
                sz.Width += 1.2F;// 0.3F;
            //else
            //    sz.Width += 0.1F;
            //sz.Height += .0F;

            //return TextRenderer.MeasureText(s, f, new Size(0,0), TextFormatFlags.NoPadding);

            return sz;
        }
        protected override bool IsInputKey(Keys keyData)
        {
            return Focused;//base.IsInputKey(keyData);
        }
        protected override bool IsInputChar(char charCode)
        {
            return this.Focused;// base.IsInputChar(charCode);
        }

        

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //if (_RestrictMacro)
            //{
            //    if (!AllowForMacro(e.KeyCode))
            //    {
            //        e.Handled = true;
            //        return;
            //    }
            //}

            CaretStop();
            handleKey(e);

            if (!e.Handled)
                base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PrintScreen)
            {//the printscreen just fire keyup event!!!!
                AddKeyCode(e.KeyCode);
            }
            else
                append_keyup(e.KeyCode);
            //base.OnKeyUp(e);
            CaretStart();
            //if (!caretTimer.Enabled) caretTimer.Start();
          
        }

        private bool m_bRightAltDown = false;
        private bool m_bRightCtrlDown = false;
        private bool m_bRightShiftDown = false;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            string s = string.Empty;
            //if (msg.Msg == WinAPI.WM_SYSKEYDOWN)
            //    System.Diagnostics.Debug.Print(msg.Msg.ToString());
            //else if (msg.Msg == WinAPI.WM_SYSKEYUP)
            //{
            //    System.Diagnostics.Debug.Print(msg.Msg.ToString());
            //}
            //if (msg.Msg == WinAPI.WM_KEYDOWN)
            //{

            //}
            switch ((int)msg.LParam)
            {
                //case 0x20380001 : s = "LAlt"    ; break;
                case 0x21380001:  //ralt
                    {
                        //s = "RAlt";
                        //if (this.RestrictMacro)
                        //{
                        //    if (!AllowForCombination(Keys.RMenu))
                        //        return true;
                        //}
                       // System.Diagnostics.Debug.Print(msg.Msg.ToString());
                        //if (msg.Msg == WinAPI.WM_SYSKEYDOWN)
                            AddKeyCode(Keys.RMenu);
                            m_bRightAltDown = true;
                        //else if (msg.Msg == WinAPI.WM_SYSKEYUP)
                        //    AddKeyCode("[#RightAlt]");
                        return true;
                    }
                //  break;
                //case 0x001d0001 : s = "LControl"; break;
                case 0x011d0001:
                    {
                        //if (this.RestrictMacro)
                        //{
                        //    if (!AllowForCombination(Keys.RControlKey))
                        //        return true;
                        //}
                        AddKeyCode(Keys.RControlKey);
                        m_bRightCtrlDown = true; 
                     //   AddKeyCode("[#RightCtrl]");
                        return true;
                        //s = "RControl";
                    }
                case 0x00360001 : 
                    {

                        AddKeyCode(Keys.RShiftKey);
                        m_bRightShiftDown = true;
                        //s = "RightShift";
                        return true;
                    }
                    
                //break;
                //case 0x002a0001 : s = "LShift"  ; break;
                //case 0x00360001 : s = "RShift"  ; break;
                //case 0x001c0001 : s = "Enter"   ; break;
                case 0x011c0001:
                    {

                        AddKeyCode(KB9Const.PAD_Enter); //a special key
                        return true;
                        //    s = "PadEnter";
                    }
                // break;

            }
          
          //  System.Diagnostics.Debug.Print(keyData.ToString());
            return base.ProcessCmdKey(ref msg, keyData);

            //  return base.ProcessCmdKey(ref msg, keyData);
        }


        private int _LineHeight = 0;
        public int LineHeight
        {
            get
            {
                if (this._LineHeight <= 0)
                {
                    this._LineHeight = (int)measureString(CreateGraphics(), "MpgP", Font).Height+1;
                }
                return this._LineHeight+4;
            }
        }

        

        int get_xoffset()
        {
            return _Offset.X;
            
        }
        int get_yoffset()
        {
            return _Offset.Y;
            
        }
        /// <summary>
        /// Draws the caret at the current position. To be called from the caret timer as repeated calls to
        /// this method make the caret blink.
        /// </summary>
        protected virtual void invertCaret()
        {

            if (!this.ShowCaret)
            {
                _CaretControl.Visible = false;
                return;
            }
            RectangleF rect = GetStringRectangle(_Caret, get_xoffset(),get_yoffset());
            _CaretControl.Location = new Point((int)rect.Left+2, (int)rect.Top);
            _CaretControl.Visible = (!_CaretControl.Visible);
        }
        private bool _ShowCaret = false;
        public bool ShowCaret
        {
            get
            {
                return this._ShowCaret;
            }
            set
            {
               
                this._ShowCaret = value;
                if (!_ShowCaret)
                    _CaretControl.Visible = false;
            }
        }
        public bool HasSelection()
        {
            return (_Selection.X != _Selection.Y);
        }
        public void Unselect()
        {
            if (HasSelection())
            {
                _Selection.X = -1;
                _Selection.Y = -1;
                Fire_SelectionChangedEvent();
                this.refresh_window();
            }
        }

        public void SelectAll()
        {
            _Selection.X = 0;
            _Selection.Y = -1;
            Fire_SelectionChangedEvent();
            this.refresh_window();
        }


        private void refresh_window()
        {
           // this.Invalidate();
            this.Refresh();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            CaretStop();
            
            this.ShowCaret = false;
            //this.Caret = 0;
            this.refresh_window();
            
        }

        //protected override void OnEnter(EventArgs e)
        //{
        //    base.OnEnter(e);
        //    this.ShowCaret = true;
        //    CaretStart();
        //}
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.ShowCaret = true;
            CaretStart();
        }
#region _Caret_Position

        private void  moveCaretLeft()
        {
            int n = this.Caret;

            if (n < 0) n = m_Data.Count;
            n--;
            if (n < 0)
                n = 0;
            this.Caret = n;
            //MakeCaretLineVisible();
            //invertCaret();
        }

        private void moveCaretRight()
        {
            int n = this.Caret;
            n++;
            if (n >=m_Data.Count)
                n = m_Data.Count;
            this.Caret = n;
            //MakeCaretLineVisible();
            //invertCaret();
        }

        public void moveCaretEnd()
        {
            this.Caret = -1;
            //_Caret = -1;
            //MakeCaretLineVisible();
            //invertCaret();

        }
        private void moveCaretHome()
        {
            this.Caret = 0;
            //_Caret = 0;
            //MakeCaretLineVisible();
            //invertCaret();

        }

        private void moveCaretUp()
        {
            Point pt = _CaretControl.Location;
            pt.Y -= (this.LineHeight/2);
            int n = get_click_on_index(pt);
            if (n >=0)
            {
                this.Caret = n;

                //_Caret = n;
                //MakeCaretLineVisible();
                //invertCaret();
                //this.Invalidate();
            }


        }
        private void moveCaretDown()
        {
            Point pt = _CaretControl.Location;
            pt.Y += (this.LineHeight*3/2);
            int n = get_click_on_index(pt);
            if (n != _Caret)
            {
                this.Caret = n;
                //MakeCaretLineVisible();
                //invertCaret();
                //this.Invalidate();
            }
        }

#endregion
        private void deleteCellAfterCaret()
        {
            if (HasSelection())
            {
                delete_selection();
                return;
            }
            if (this.Caret < 0) return;
            undo_snapshot();
            int n = this.Caret;
            if (n >= 0 && n < m_Data.Count)
                m_Data.RemoveAt(n);

            refresh_window();
            Fire_TextChangedEvent();
        }

        public void deleteCellBeforeCaret()
        {
            if (HasSelection())
            {
                delete_selection();
                return;
            }
            int n = this.Caret;// _Caret - 1;
            undo_snapshot();
            if (n < 0)
                n = m_Data.Count;
            n--;
            if (n >= 0 && n < m_Data.Count)
                m_Data.RemoveAt(n);
            else
                n = 0;
            this.Caret = n;
            Fire_TextChangedEvent();
           // refresh_window();

        }
        protected virtual void handleKey(KeyEventArgs e)
        {

          /*  switch (e.KeyCode)
            {
                // Move caret to the left
                case Keys.Left:
                    {

                        if (HasSelection())
                        {
                            this.Caret = _Selection.X;
                            
                            Unselect();
                        }
                        else
                            moveCaretLeft();
                    } break;
                // Move caret to the right
                case Keys.Right:
                    {
                        if (HasSelection())
                        {
                            
                            this.Caret = _Selection.Y;
                            Unselect();
                        }
                        else 
                            moveCaretRight();
                    } break;
                case Keys.Up:
                    {
                        moveCaretUp();
                        if (HasSelection())
                            Unselect();
                        
                    }break;
                case Keys.Down:
                    {
                        moveCaretDown();
                        if (HasSelection())
                            Unselect();
                    }break;
                case Keys.Delete:
                    {
                        
                        deleteCellAfterCaret();
                        
                    } break;
                // Delete current selection or char before current caret position
                case Keys.Back:
                    {
                        deleteCellBeforeCaret();
                        
                    } break;

                default:*/
                    handleKeycodeKey(e);
            /*
                    break;
            }
             * */
        }

        public void Clear()
        {
            m_Data.Clear();
            m_Undo.Clear();
            this.Caret = 0;
            Fire_TextChangedEvent();
            //this.Invalidate();
        }
        public void ClearUndo()
        {
            m_Undo.Clear();
        }


        public virtual void AddKeyCode(Keys key)
        {
            AddKeyCode(KB9Utility.KB9KeyDefinition.KeyName(key), true);

          
        }

        public virtual void AddKeyCode(string strKey)
        {
            AddKeyCode(strKey, false);
          
        }
        public virtual void AddKeyCode(string strKey, bool bAppendBracket)
        {
            Unselect();
            if (this.SingleKey)
                this.Clear();

            undo_snapshot();
            if (bAppendBracket)
                strKey = "[" + strKey + "]";

            int n = this.Caret;
            if (n >= m_Data.Count)
                n = -1;
            if (n < 0)
                m_Data.Add(strKey);
            else
                m_Data.Insert(n, strKey);
            if (n >= 0)
            {
                n++;
                if (n > m_Data.Count)
                    n = m_Data.Count;
            }
            this.Caret = n;

            Fire_TextChangedEvent();
           // MakeCaretLineVisible();
            //this.Invalidate();
        }
        /************************************************************************/
        /* 
         * use this function to correct the validation error,
         * see KB9Validation.cs
         */
        /************************************************************************/
        public virtual void AppendKeyCode(string strKey)
        {
            
            if (this.SingleKey)
                this.Clear();
                                            
            m_Data.Add(strKey);
            Fire_TextChangedEvent();      
        }
        /************************************************************************/
        /* 
         * remove last error code, it was called in KB9Validation class.
         * If the Down is less then Up, remove last UP.
         */
        /************************************************************************/
        public void RemoveLastKeyCode(string strKey)
        {
            int ncount = m_Data.Count;
            for (int i = ncount - 1; i >= 0; i--)
            {
                string s = m_Data[i];
                if (s == strKey)
                {
                    m_Data.RemoveAt(i);
                    return;
                }
            }
             
        }

        private void append_keyup(Keys key)
        {
            
            string s = "";
            switch (key)
            {
                case Keys.ShiftKey:
                    {
                        if (m_bRightShiftDown)
                        {
                            s = "#RightShift";
                            m_bRightShiftDown = false;
                        }
                        else
                            s = "#Shift";
                    }
                    break;
                case Keys.ControlKey:
                case Keys.Control:
                    {

                        if (m_bRightCtrlDown)
                        {
                            s = "#RightCtrl";
                            m_bRightCtrlDown = false;
                        }
                        else
                            s = "#Ctrl";
                    }
                    break;
                case Keys.RControlKey:
                    s =  "#RightCtrl";
                    break;
                case Keys.LMenu:
                case Keys.Menu:
                    {

                        if (m_bRightAltDown)
                        {
                            s = "#RightAlt";
                            m_bRightAltDown = false;
                        }
                        else
                            s = "#Alt";
                    }
                    break;
                case Keys.RMenu:
                    s = "#RightAlt";
                    break;
                case Keys.LWin:
                    s = "#Win";
                    break;
                case Keys.RWin:
                    s = "#RightWin";
                    break;
            }
            if (s != string.Empty)
                AddKeyCode(s, true);
        }
        protected virtual void handleKeycodeKey(KeyEventArgs e)
        {

            Keys key = e.KeyCode;

            //System.Diagnostics.Debug.Print(key.ToString());
            //System.Diagnostics.Debug.Print(e.KeyValue.ToString());
            //System.Diagnostics.Debug.Print("--------------------------"+Keys.PrintScreen.ToString());
            // if (IsFunctionsKey(key) ||
            //     IsPadNumberKey(key))
            if (KB9Utility.KB9KeyDefinition.IsTestKbHandledKey(key))
            {
                //if (_RestrictMacro)
                //{
                //    if (!AllowForCombination(e.KeyCode))
                //    {
                //        e.Handled = true;
                //        return;
                //    }
                //}
               

                AddKeyCode(key);
                //append_keyup(key);
                e.Handled = true;
                return;
            }
            // if (handlePadKeys(e))
            //     return;
            char ch = KB9Utility.KB9KeyDefinition.ConvertKey2Char(e);

            //if (_RestrictMacro)
            //{
            //    if (!AllowForCombination(ch))
            //    {
            //        e.Handled = true;
            //        return;
            //    }
            //}

            if (ch >= '!' && ch <= '~' &&
                ch != '[' && ch != ']' &&
                ch !='\\')
            //check ASCII table    
            {

                string strKey = ch.ToString();
                AddKeyCode(strKey);


            }
            else if (ch == '[' || ch == ']')
            {
                string strKey = ch.ToString();
                //strKey ="\\"+ strKey; //double them
                AddKeyCode(strKey);
            }
            else if (ch == '\\')
            {
                //AddKeyCode("\\\\");
                AddKeyCode("\\");
            }

        }



        int get_click_on_index(Point ptMouse)
        {
            List<RectangleF> lst = new List<RectangleF>();
            GetAllCellRectangle(lst, get_xoffset(), get_yoffset());
            
            for (int i=0; i< lst.Count; i++)
            {
                if (lst[i].Contains(ptMouse))
                    return i;
            }
            return -1;
        }

        private void do_scroll_up()
        {
            this._Offset.Y -= this.LineHeight;

            int lines = measureLines(CreateGraphics());
            int nmax = (lines - 1) * this.LineHeight;
            if (_Offset.Y < (-1) * nmax)
                _Offset.Y = (-1) * nmax;
            refresh_window();
        }
        //arrow down
        private void scroll_up()
        {
            do_scroll_up();
            update_vertical_scrollbar_value();
            //this._Offset.Y -= this.LineHeight;

            //int lines = measureLines(CreateGraphics());
            //int nmax = (lines - 1) * this.LineHeight;
            //if (_Offset.Y < (-1) * nmax)
            //    _Offset.Y = (-1) * nmax;
            //refresh_window();
        }
        //arrow up
        private void do_scroll_down()
        {
            this._Offset.Y += this.LineHeight;
            if (this._Offset.Y > 0)
                this._Offset.Y = 0;
            refresh_window();
        }
        private void scroll_down()
        {
            do_scroll_down();
            update_vertical_scrollbar_value();
            //this._Offset.Y += this.LineHeight;
            //if (this._Offset.Y > 0)
            //    this._Offset.Y = 0;
            //refresh_window();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            this._Selection.X = 0;
            this._Selection.Y = -1;
            Fire_SelectionChangedEvent();
            this.Caret = -1;
            //this.invertCaret();
            //this.Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            this.ShowCaret = false;
            if (e.Delta > 0)
                scroll_down();
            else
                scroll_up();
            this.ShowCaret = true;

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
                OnLButtonDown(e);
            else if (e.Button == MouseButtons.Right)
                OnRButtonDown(e);
            
        }

        protected void OnRButtonDown(MouseEventArgs e)
        {
            ContextMenuStrip context = new ContextMenuStrip();
            ToolStripMenuItem menu;

            Image img = null;
            img = Util.get_image("undo");
            menu = new ToolStripMenuItem("Undo", img, new EventHandler(OnUndo));
            context.Items.Add(menu);
            menu.Enabled = (m_Undo.Count > 0);
            context.Items.Add(new ToolStripSeparator());

            img = Util.get_image("cut");
            menu = new ToolStripMenuItem("Cut", img, new EventHandler( OnCut ));
            context.Items.Add(menu);
            menu.Enabled = HasSelection();

            img = Util.get_image("copy");
            menu = new ToolStripMenuItem("Copy", img, new EventHandler(OnCopy));
            context.Items.Add(menu);
            menu.Enabled = HasSelection();

            img = Util.get_image("paste");
            menu = new ToolStripMenuItem("Paste", img, new EventHandler(OnPaste));
            context.Items.Add(menu);
            menu.Enabled = Clipboard.ContainsText();

            img = Util.get_image("del");
            menu = new ToolStripMenuItem("Delete", img, new EventHandler(OnDelete));
            context.Items.Add(menu);
            menu.Enabled = HasSelection();

            context.Show(this, e.Location);
        }

        //private void reset_selection()
        //{
        //    _Selection.X = -1;
        //    _Selection.Y = -1;
        //}

        private void delete_selection()
        {
            if (!HasSelection()) return;
            int from = -1, to = -1;
            get_selected_from_to(ref from, ref to);
            string s = string.Empty;
            for (int i = from; i < to; i++)
            {
                m_Data.RemoveAt(from);
            }
            Unselect();
            this.Caret = from;
            Fire_TextChangedEvent();

            //invertCaret();
            
            //this.Invalidate();
        }

        public void OnCut(System.Object sender, System.EventArgs e)
        {
            if (!HasSelection()) return;
            int from = -1, to = -1;
            get_selected_from_to(ref from , ref to);
            string s = string.Empty;
            for (int i = from ;i< to; i++ )
            {
                s += m_Data[from];
                m_Data.RemoveAt(from);
            }
            Unselect();
            this.Caret = from;
            


            if (s != string.Empty)
                Clipboard.SetText(s);

        }

        public void OnCopy(System.Object sender, System.EventArgs e)
        {
            if (!HasSelection()) return;
            int from = -1, to = -1;
            get_selected_from_to(ref from, ref to);
            string s = string.Empty;
            for (int i = from; i < to; i++)
            {
                s += m_Data[i];
                
            }
            this.Caret = from;
          
            if (s != string.Empty)
                Clipboard.SetText(s);
          
        }

        public void OnPaste(System.Object sender, System.EventArgs e)
        {
            if (!Clipboard.ContainsText())
                return;

            undo_snapshot();
            int nCaret = this.Caret;
            if (HasSelection()) 
            {
                delete_selection();
            }
            string s = Clipboard.GetText();
            s = s.Replace("\r", "");
            s = s.Replace("\n", "");

            ParseString(s);
            this.Caret = nCaret;
            refresh_window();
            //this.Caret = -1;
            
        }

        public void OnUndo(System.Object sender, System.EventArgs e)
        {
            undo_pop();
            invertCaret();
            Fire_TextChangedEvent();
            //this.Invalidate();
            refresh_window();
        }

        public void OnDelete(System.Object sender, System.EventArgs e)
        {
            if (!HasSelection())
                return;
            undo_snapshot();
            delete_selection();
        }

        private bool click_on_resize(Point pt)
        {
            return false;// (GetGripperRect().Contains(pt));
        }
        MOVE_TYPE _MoveType = MOVE_TYPE.None;
        Point _PointMouseDown = new Point(-1, -1);
        Size _SizeMouseDown = new Size(-1,-1);
        protected void OnLButtonDown(MouseEventArgs e)
        {
            this.Focus();
            _PointMouseDown = e.Location;
            _SizeMouseDown = this.Size;
            //if (click_on_resize(e.Location) && (this.ShowGripper))
            //{
            //    _MoveType = MOVE_TYPE.Resize;
            //}
            //else
            {

                _MoveType = MOVE_TYPE.Select;

                _Caret = get_click_on_index(e.Location);
                invertCaret();
                _Selection.X = _Caret; //set same
                _Selection.Y = _Caret;
                Fire_SelectionChangedEvent();
            }
            refresh_window();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
                OnLButtonMove(e);
            else if ( e.Button == MouseButtons.None)
            {
                OnNoneButtonMove(e);

            }
        }

        private void OnNoneButtonMove(MouseEventArgs e)
        {
            //if (!this.ShowGripper) return;

            //Rectangle rt = GetGripperRect();
            //if (rt.Contains(e.Location))
            //{
            //    this.Cursor = Cursors.SizeNWSE;
            //}
            //else
            //    this.Cursor = Cursors.Default;
        }

        protected void OnLButtonMove(MouseEventArgs e)
        {
            if (_MoveType == MOVE_TYPE.Select)
            {


                int n = get_click_on_index(e.Location);
                if (e.Location.Y < 0 || e.Location.X < 0)
                    n = 0;
                this.Caret = n;
                _Selection.X = n;
                Fire_SelectionChangedEvent();
                //System.Diagnostics.Debug.Print(n.ToString());
                this.Refresh();
                //this.invertCaret();
                //this.Invalidate();
            }
            //else if (_MoveType == MOVE_TYPE.Resize)
            //{
            //    if (!this.ShowGripper) return;
            //    if (_PointMouseDown.X == -1 && _PointMouseDown.Y == -1)
            //        return;
            //    //int w = e.X - _PointMouseDown.X;
            //    int h = e.Y - _PointMouseDown.Y;
            //    //w = _SizeMouseDown.Width + w;
            //    h = _SizeMouseDown.Height + h;
            //    int nmin = this.LineHeight * 2;
            //    int nmax = this.LineHeight * 5;
            //    if (h < nmin) h = nmin;
            //    if (h > nmax) h = nmax;
            //    this.Height = h;
            //}
            //MakeCaretLineVisible();
            //refresh_window();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
                OnLButtonUp(e);

        }

        protected void OnLButtonUp(MouseEventArgs e)
        {
            _PointMouseDown.X = -1;
            _PointMouseDown.Y = -1;
            _SizeMouseDown.Width = -1;
            _SizeMouseDown.Height = -1;
            _MoveType = MOVE_TYPE.None;
            //ajust_selection();
            Invalidate();
           // System.Diagnostics.Debug.Print(_Selection.ToString());
        }

        protected void MakeCaretLineVisible()
        {
             RectangleF rectString = GetStringRectangle(_Caret, get_xoffset(), get_yoffset());
            Rectangle rectVisible = GetVisibleArea();
            Rectangle rt = new Rectangle((int)rectString.Left, (int)rectString.Top, (int)rectString.Width, (int)rectString.Height);

            if (rectVisible.Contains(rt))
                return; //it is visible
            if (rt.Bottom > rectVisible.Bottom)
            {
                int nlines = (rt.Bottom - rectVisible.Bottom) / this.LineHeight + 1;
                this._Offset.Y -= (nlines * this.LineHeight); //move up
                
            }
            if (rt.Top < rectVisible.Top)
            {
                int nlines = (rectVisible.Top - rt.Top) / this.LineHeight + 1;
                this._Offset.Y += (nlines * this.LineHeight); //move down
                if (this._Offset.Y > 0)
                    this._Offset.Y = 0;
            }
            this.Invalidate();

        }

        #region _Undo_
        private const int MAX_UNDO = 10;
        private void undo_snapshot()
        {
            //List<string> ar = new List<string>();

            //for (int i=0; i< m_Data.Count ; i++)
            //    ar.Add(m_Data[i]);
            //m_Undo.Add(ar);
            //if (m_Undo.Count > MAX_UNDO)
            //{
            //    int ncount = MAX_UNDO - m_Undo.Count;
            //    for (int i = 0; i < ncount; i++)
            //    {
            //        if (m_Undo.Count >0)
            //            m_Undo.RemoveAt(0);
            //    }
            //}
        }
        private void undo_pop()
        {
            if (m_Undo.Count <= 0) return;
            List<string> ar = m_Undo[m_Undo.Count - 1];
            m_Undo.RemoveAt(m_Undo.Count - 1);

            m_Data.Clear();
            for (int i = 0; i < ar.Count; i++)
                m_Data.Add(ar[i]);
            _Caret = -1;
            this.Invalidate();
        }

        #endregion


        /// <summary>
        /// parse the text to token.
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void ParseString(string text)
        {
            List<string> lst = new List<string>();

            int ncount = Util.ParseKeyContents(text, lst);
            for (int i=0; i< ncount; i++)
            {
                AddKeyCode(lst[i]);
            }
            

            if (this.m_Data.Count > 0)
                this.Caret = 0;
        }

      

        //properties
        // \\1234\[[Space]\]
        public override string Text
        {
            get 
            {
                string s = string.Empty;
                for (int i=0; i< m_Data.Count; i++)
                {
                    s += m_Data[i];
                }
                return s;
            }
            set
            {
                update_vertical_scrollbar();
                if (this.Text == value)
                    return;
                m_Data.Clear();
                string s = value;
                ParseString(s);
                update_vertical_scrollbar();
                if (m_Data.Count > 0)
                    this.Caret = 0; //move cursor to first
                else
                    Refresh(); 
                //this._Offset = new Point(0, 0);
            }
        }

        private bool _SingleKey = false;
        public bool SingleKey
        {
            get 
            {
                return _SingleKey;
            }
            set 
            {
                _SingleKey = value;
            }
        }

        public void Undo()
        {
            undo_pop();
            invertCaret();
            Fire_TextChangedEvent();
        }

        public bool CanUndo()
        {
            return (m_Undo.Count > 0);
        }

        private bool _RestrictMacro = false;
        public bool RestrictMacro
        {
            get 
            {
                return _RestrictMacro;
            }
            set
            {
                _RestrictMacro = value;
            }
        }

        bool AllowForCombination(Keys key)
        {
           

            Keys[] keys = new Keys[]{
                Keys.Alt, Keys.LMenu, Keys.Menu,Keys.RMenu, //alt, ralt
                Keys.ShiftKey, Keys.Shift, Keys.RShiftKey, //shift, rshift
                Keys.ControlKey, Keys.Control,Keys.RControlKey, //control, rcontrol
                Keys.LWin, Keys.RWin //win, RWin
            };

            for (int i=0; i< keys.Length ; i++)
            {
                if (keys[i] == key)
                    return false;
            }
            System.Diagnostics.Debug.Print(key.ToString());
           

            return true;

        }
        bool AllowForCombination(char ch)
        {
            char[] ar = new char[]{
                '~','!','@','#','$','%','^','&','*','(',')','_','+','\\','|',
                '{','}',
                ':','\"','<','>','?'
            }; 
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i] == ch)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

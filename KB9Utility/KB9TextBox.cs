using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace KB9Utility
{
    public partial class KB9TextBox : UserControl// Must use UserControl as its parent, we need IsInputChar/IsInputKey function.
    {


        private enum MOVE_TYPE
        {
            None=0,
            Select,
            Resize,
        }

        KbdState _KbdState = new KbdState();


        List<string> m_Data = new List<string>();
              
        Point _Selection = new Point(0, 0); //cx: from, cy: to
        Label _CaretControl = new Label();
        Point _Offset = new Point(0, 0);

        List<List<string>> m_Undo = new List<List<string>>();

        protected int _oldCaret;     // Backup of the previous caret if it changes
        
        protected Timer caretTimer;             // Timer used for caret blinking

        public delegate void EventOnTextChanged(object sender);
        public event EventOnTextChanged OnTextContentChanged;

        public delegate void EventOnSelectionChanged(object sender);
        public event EventOnSelectionChanged OnSelectionChanged;


        VScrollBar m_verticalScrollBar = new VScrollBar();
        private bool _Modified = false;

        public bool Modified
        {
            get
            {
                return _Modified;
            }
            set
            {
                _Modified = value;
            }
        }

        private  void Fire_TextChangedEvent()
        {
            if (!this.EnableTextChangedEvent)
                return;
            if (OnTextContentChanged != null)
                OnTextContentChanged(this);
            update_vertical_scrollbar();
            this.Modified = true;
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

        public KB9TextBox()
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
            reset_timer();
            //this.caretTimer = new Timer();
            //this.caretTimer.Interval = 500;
            //this.caretTimer.Tick += new EventHandler(caretTimer_Tick);
            //this.caretTimer.Start();

            this._Offset.X = 0;
            this._Offset.Y = 0;

            this.Controls.Add(m_verticalScrollBar);
            m_verticalScrollBar.ValueChanged += new EventHandler(m_verticalScrollBar_ValueChanged);
            //if (!this.ScrollBarVisible)
                m_verticalScrollBar.Visible = false;
            located_vertical_scrollbar();
            update_vertical_scrollbar();

            this.ImeMode = ImeMode.Disable;
           
        }

        private void reset_timer()
        {
            this.caretTimer = new Timer();
            this.caretTimer.Interval = 500;
            this.caretTimer.Tick += new EventHandler(caretTimer_Tick);
            this.caretTimer.Start();
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
        /// <summary>
        /// 
        /// </summary>
        private void check_single_function_key_append_release()
        {
            if (_Caret == 0) //start position, and single key
            {

                if ((!this.SingleKey) && (m_Data.Count == 1))
                {
                    string key = m_Data[0];
                    if (is_function_key_string(key))
                    { //add release key code
                        key = key.Replace("[", "");
                        key = "[#" + key;
                        m_Data.Add(key);
                        refresh_window();
                    }
                }
            }
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
                    //if (_Caret == 0) //start position, and single key
                    //{

                    //    if ((!this.SingleKey) && (m_Data.Count == 1))
                    //    {
                    //        string key = m_Data[0];
                    //        if (is_function_key_string(key))
                    //        { //add release key code
                    //            key = key.Replace("[", "");
                    //            //key = key.Replace("]", "");
                    //            key = "[#" + key;
                    //            m_Data.Add(key);
                    //            //this.AddKeyCode(key);
                    //        }
                    //    }
                    //}
                    MakeCaretLineVisible();
                    
                    invertCaret();
                    System.Diagnostics.Debug.Print("Caret:CaretStart();" + this.Name);
                    //this.Parent.Focus();
                    //this.Focus();
                    CaretStart();
                    refresh_window();
                   // _CaretControl.BringToFront();
                }
                
            }
        }
        private void CaretStop()
        {
            System.Diagnostics.Debug.Print("CaretStop:" + this.Name);
            caretTimer.Stop();
            _CaretControl.Visible = false;
        }
        private void CaretStart()
        {
            caretTimer.Stop();
            System.Diagnostics.Debug.Print("CaretStart:" + this.Name);
            caretTimer.Start();
            _CaretControl.Visible =  false;
            invertCaret();
            _CaretControl.BringToFront();
            this.Refresh();
            caretTimer.Start();
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

        private bool _ShowGripper = true;
        public bool ShowGripper
        {
            get 
            {
                return _ShowGripper;
            }
            set
            {
                _ShowGripper = value;
                if (!_ShowGripper)
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }


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
           // return true;
            return Focused;//base.IsInputKey(keyData);
        }
        protected override bool IsInputChar(char charCode)
        {
           // return true;
            return this.Focused;// base.IsInputChar(charCode);
        }

        

        protected override void OnKeyDown(KeyEventArgs e)
        {
            System.Diagnostics.Debug.Print("OnKeyDown=" + e.KeyCode.ToString());
            //if (e.KeyCode == Keys.Space)
            //    MessageBox.Show("space");
            //if (_RestrictMacro)
            //{
            //    if (!AllowForMacro(e.KeyCode))
            //    {
            //        e.Handled = true;
            //        return;
            //    }
            //}

            if (_KbdState.is_down(e.KeyCode))
                return;
            //check right alt
            if (m_bRightAltDown)
            { //the lalt key was fired here.
                if (e.KeyCode == Keys.LMenu ||
                    e.KeyCode == Keys.Menu)
                    return;
            }

            //check right alt
            if (m_bRightCtrlDown)
            { //the lalt key was fired here.
                if (e.KeyCode == Keys.LControlKey ||
                    e.KeyCode == Keys.ControlKey)
                    return;
            }

            CaretStop();
            handleKey(e);
            if (e.Handled)
                _KbdState.set_down(e.KeyCode);
            e.SuppressKeyPress = true;
            if (!e.Handled)
            {
                base.OnKeyDown(e);
               
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            System.Diagnostics.Debug.Print("OnKeyUp="+e.KeyCode.ToString());
            //if (e.KeyCode == Keys.RControlKey ||
            //    e.KeyCode == Keys.R
            if (e.KeyCode == Keys.PrintScreen)
            {//the printscreen just fire keyup event!!!!
                AddKeyCode(e.KeyCode);
            }
            
            append_keyup(e.KeyCode);
            //base.OnKeyUp(e);
            CaretStart();
            _KbdState.set_up(e.KeyCode);
            //if (!caretTimer.Enabled) caretTimer.Start();
          
        }
        private bool m_bRightAltDown = false;
        private bool m_bRightCtrlDown = false;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            System.Diagnostics.Debug.Print("ProcessCmdKey");
           string s = string.Empty;
            switch ((int)msg.LParam)
            {
                //case 0x20380001 : s = "LAlt"    ; break;
                case 0x21380001:  //ralt
                    {
                        //s = "RAlt";
                        if (this.RestrictMacro)
                        {
                            if (!AllowForCombination(Keys.RMenu))
                                return true;
                        }
                        KeyEventArgs e ;
                        if (_KbdState.is_down(Keys.RControlKey))
                            e = new KeyEventArgs(Keys.RMenu | Keys.Alt|Keys.Control);
                        else
                            e = new KeyEventArgs(Keys.RMenu|Keys.Alt);
                        //e.Modifiers = Keys.Alt;
                        fix_function_keys_up(e);

                        AddKeyCode(Keys.RMenu);
                        //if (m_Data.Count > 1)
                        //    AddKeyCode("[#RightAlt]");
                        m_bRightAltDown = true;
                        _KbdState.set_down(Keys.Menu);
                       
                        return true;
                    }
                //  break;
                //case 0x001d0001 : s = "LControl"; break;
                case 0x011d0001:
                    {

                        if (this.RestrictMacro)
                        {
                            if (!AllowForCombination(Keys.RControlKey))
                                return true;
                        }

                        KeyEventArgs e;
                        if (_KbdState.is_down(Keys.RMenu))
                            e = new KeyEventArgs(Keys.RControlKey|Keys.Control | Keys.Alt);
                        else
                            e = new KeyEventArgs(Keys.RControlKey|Keys.Control);
                        //e.Control = true;
                        //e.Modifiers = Keys.Control;
                        fix_function_keys_up(e);

                        AddKeyCode(Keys.RControlKey);
                        if (m_Data.Count > 1)
                        {
                            //System.Diagnostics.Debug.Print("AddKeyCode([#RightCtrl])");
                            //AddKeyCode("[#RightCtrl]");
                        }
                        m_bRightCtrlDown = true;
                        _KbdState.set_down(Keys.RControlKey);
                        return true;
                        //s = "RControl";
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
            //System.Diagnostics.Debug.Print("invertCaret");
            if (!this.ShowCaret)
            {
                
                _CaretControl.Visible = false;
                return;
            }
           // System.Diagnostics.Debug.Print("invertCaret-> " + this.Name);
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
           
            System.Diagnostics.Debug.Print("lost focus:" + this.Name);
  
            CaretStop();

           // this.ShowCaret = false; 
           //return;
           //this.Caret = 0;
            this.refresh_window();
            _KbdState.reset();
           // this.ClearUndo();
                     
        }

        //protected override void OnEnter(EventArgs e)
        //{
        //    base.OnEnter(e);
        //    this.ShowCaret = true;
        //    CaretStart();
        //}
        protected override void OnGotFocus(EventArgs e)
        { 
           
           // this.ClearUndo();
            _KbdState.reset();
            base.OnGotFocus(e);
            this.ShowCaret = true;
            
           // reset_timer();
            CaretStart();
            System.Diagnostics.Debug.Print("get focus:" + this.Name + "  ,"+ this.ShowCaret.ToString());
        }
#region _Caret_Position

        private void  moveCaretLeft()
        {
            int n = this.Caret;

            if (n < 0) n = m_Data.Count;
            n--;
            if (n < 0)
                n = 0;
            if (this.Caret != n)
                undo_snapshot();
            this.Caret = n;
            check_single_function_key_append_release();
            //MakeCaretLineVisible();
            //invertCaret();
        }

        private void moveCaretRight()
        {
            int n = this.Caret;
            n++;
            if (n >=m_Data.Count)
                n = m_Data.Count;
            if (this.Caret != n)
                undo_snapshot();
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
            check_single_function_key_append_release();
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

            switch (e.KeyCode)
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
                case Keys.CapsLock:
                    {//just on-screen keyboard can input this key
                        
                    } break;
                default:
                    {


                        if (HasSelection())
                        {//20140429, delete selection for normal input key code.
                            delete_selection();
                        }
                        fix_function_keys_up(e);
                        handleKeycodeKey(e);
                    }
                    break;
            }
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
            //
            this.Modified = true;
            if (this.LowerKey)
            {
                if (strKey.Length == 1)
                    strKey = strKey.ToLower();
            }


            if (m_Undo.Count <=0)
              undo_snapshot(); //20141125, comment it.

           
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

        /// <summary>
        /// these key used by textbox itself.
        /// 20140203
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool is_reserved_key(KeyEventArgs e)
        {
             switch (e.KeyCode)
            {
                // Move caret to the left
                case Keys.Left:
                // Move caret to the right
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Delete:
                // Delete current selection or char before current caret position
                case Keys.Back:
                 case Keys.ShiftKey:
                 case Keys.LShiftKey:
                 case Keys.RShiftKey:

                   return true;
                default:
                    return false;
            }
        
        }
        /// <summary>
        /// Make Ctrl, shift, win and alt pair
        /// </summary>
        /// <param name="e"></param>
        public void fix_function_keys_up(KeyEventArgs e)
        {
           // if (m_Data.Count >1) //just one function key need this.
           //     return; 
            //get previous code
            if (is_reserved_key(e))
                return;
            int n = this.Caret;// _Caret - 1;
            string strPrevious = "";

            if (n < 0)
                n = m_Data.Count;
            n--;
            if (n >= 0 && n < m_Data.Count)
            {
                strPrevious = m_Data[n];
            }
            
            if (strPrevious.Length <= 0)
                return;

            if (!is_function_key_string(strPrevious))
                return;

            string strNext = "";
            n++;
            if (n < m_Data.Count)
            {
                strNext = m_Data[n];
                //if previous and next is pair function keys, 
                // do nothing
                strNext = strNext.Replace("#", "");
                if (strNext.Equals(strPrevious))
                    return;
            }
            
            //if ( (e.Modifiers == Keys.Control) &&
            if (e.Control &&
                ( _KbdState.is_down(Keys.ControlKey) ||
                 _KbdState.is_down(Keys.LControlKey) ||
                 _KbdState.is_down(Keys.RControlKey) ) &&
                 (e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.RControlKey))
                return;
            //    (strPrevious == KB9Const.CTRL_DOWN || strPrevious == KB9Const.RCTRL_DOWN) &&
            //    (e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.RControlKey) )
            //    return;

           // if (e.Modifiers == Keys.Alt && 
            if (e.Alt &&
                (_KbdState.is_down(Keys.Menu) ||
                _KbdState.is_down(Keys.LMenu) ||
                _KbdState.is_down(Keys.RMenu)) &&
                (e.KeyCode != Keys.LMenu && e.KeyCode != Keys.RMenu && e.KeyCode != Keys.Menu))
            {
                System.Diagnostics.Debug.Print(" e.Alt return =========" + e.KeyCode.ToString());
                return;
            }
            //   (strPrevious == KB9Const.ALT_DOWN || strPrevious == KB9Const.RALT_DOWN) &&
            //   (e.KeyCode != Keys.LMenu && e.KeyCode != Keys.RMenu))
            //    return;

            //if (e.Modifiers == Keys.Shift &&
            //   (strPrevious == KB9Const.SHIFT_DOWN || strPrevious == KB9Const.RSHIFT_DOWN) &&
            //   (e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.RShiftKey))
            //    return;
            //if (e.Modifiers == Keys.Shift &&
            if (e.Shift &&
              (_KbdState.is_down(Keys.ShiftKey) ||
              _KbdState.is_down(Keys.LShiftKey) ||
              _KbdState.is_down(Keys.RShiftKey)))
                return;

            if ( (strPrevious == KB9Const.WIN_DOWN || strPrevious == KB9Const.RWIN_DOWN) &&
                 (WinAPI.IsKeyDown(Keys.LWin) || WinAPI.IsKeyDown(Keys.RWin) ) &&
                 (e.KeyCode != Keys.LWin && e.KeyCode != Keys.RWin ))
            
                return;
            
            
            string s = strPrevious;
            s = s.Insert(1, "#");
            AddKeyCode(s);

            
            /*

            
            bool bIsLastKey = false;
            if (IsFunctionKeyDown(Keys.ControlKey, ref bIsLastKey))
                m_Data.Add("[#Ctrl]");
            else if (IsFunctionKeyDown(Keys.RControlKey, ref bIsLastKey))
                m_Data.Add("[#RightCtrl]");
            else if (IsFunctionKeyDown(Keys.LMenu, ref bIsLastKey))
                m_Data.Add("[#Alt]");
            else if (IsFunctionKeyDown(Keys.RMenu, ref bIsLastKey))
                m_Data.Add("[#RightAlt]");
            else if (IsFunctionKeyDown(Keys.LWin, ref bIsLastKey))
                m_Data.Add("[#Win]");
            else if (IsFunctionKeyDown(Keys.RWin, ref bIsLastKey))
                m_Data.Add("[#RightWin]");
            else if (IsFunctionKeyDown(Keys.ShiftKey, ref bIsLastKey))
                m_Data.Add("[#Shift]");
            else if (IsFunctionKeyDown(Keys.RShiftKey, ref bIsLastKey))
                m_Data.Add("[#RightShift]");
             * */
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
                    Fire_TextChangedEvent();      
                    return;
                }
            }
             
        }

        public void RemoveLastKey()
        {
            int nindex = m_Data.Count-1;
            if (nindex < 0)
                return;
            m_Data.RemoveAt(nindex);
            Fire_TextChangedEvent(); 

        }

        private void append_keyup(Keys key)
        {
            if (this.SingleKey)
                return;
            bool bIsLastKey = false;
            bool bKeyDown = IsFunctionKeyDown(key, ref bIsLastKey);

            if (m_Data.Count > 1)
                bIsLastKey = false;

            string s = "";
            switch (key)
            {
                case Keys.ControlKey:
                case Keys.Control:
                    if (m_bRightCtrlDown)
                    {
                        IsFunctionKeyDown(Keys.RControlKey, ref bIsLastKey);
                        if (m_Data.Count > 1) bIsLastKey = false;
                        s = "#RightCtrl";
                        m_bRightCtrlDown = false;
                    }
                    else
                        s = "#Ctrl";
                    if (bIsLastKey)
                        s = "";
                    break;
                case Keys.RControlKey:
                    {

                        s = "#RightCtrl";
                        if (bIsLastKey)
                            s = "";
                    }
                    break;
                case Keys.LMenu:
                case Keys.Menu:
                    if (m_bRightAltDown)
                    {
                        IsFunctionKeyDown(Keys.RMenu, ref bIsLastKey);
                        if (m_Data.Count > 1) bIsLastKey = false;
                        s = "#RightAlt";
                        m_bRightAltDown = false;
                    }
                    else
                        s = "#Alt";
                    if (bIsLastKey)
                        s = "";
                    break;
                case Keys.RMenu:
                    s = "#RightAlt";
                    if (bIsLastKey)
                        s = "";
                    break;
                case Keys.LWin:
                    s = "#Win";
                    if (bIsLastKey)
                        s = "";
                    break;
                case Keys.RWin:
                    s = "#RightWin";
                    if (bIsLastKey)
                        s = "";
                    break;
                default:
                    {

                    }
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
            if (KB9Utility.KB9KeyDefinition.IsHandledKey(key))
            {
                if (_RestrictMacro)
                {//this text box is for input Macro
                    if (!AllowForCombination(e.KeyCode))
                    {
                        e.Handled = true;
                        return;
                    }
                }
               
                
                AddKeyCode(key);
               // append_keyup(key);
                e.Handled = true;
                return;
            }
            // if (handlePadKeys(e))
            //     return;
            char ch = KB9Utility.KB9KeyDefinition.ConvertKey2Char(e);

            if (_RestrictMacro)
            {
                if (!AllowForCombination(ch))
                {
                    e.Handled = true;
                    return;
                }
            }

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
                strKey ="\\"+ strKey; //double them
                AddKeyCode(strKey);
            }
            else if (ch == '\\')
            {
                AddKeyCode("\\\\");
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
            if (click_on_resize(e.Location) && (this.ShowGripper))
            {
                _MoveType = MOVE_TYPE.Resize;
            }
            else
            {

                _MoveType = MOVE_TYPE.Select;
                int nOldCaret = _Caret;

                _Caret = get_click_on_index(e.Location);
                if (nOldCaret != _Caret)
                    undo_snapshot();
                invertCaret();
                _Selection.X = _Caret; //set same
                _Selection.Y = _Caret;
                Fire_SelectionChangedEvent();
            }
            check_single_function_key_append_release();
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
            else if (_MoveType == MOVE_TYPE.Resize)
            {
                if (!this.ShowGripper) return;
                if (_PointMouseDown.X == -1 && _PointMouseDown.Y == -1)
                    return;
                //int w = e.X - _PointMouseDown.X;
                int h = e.Y - _PointMouseDown.Y;
                //w = _SizeMouseDown.Width + w;
                h = _SizeMouseDown.Height + h;
                int nmin = this.LineHeight * 2;
                int nmax = this.LineHeight * 5;
                if (h < nmin) h = nmin;
                if (h > nmax) h = nmax;
                this.Height = h;
            }
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
        private const int MAX_UNDO = 50;
        private void undo_snapshot()
        {
            List<string> ar = new List<string>();

            for (int i=0; i< m_Data.Count ; i++)
                ar.Add(m_Data[i]);

            if (lastUndoIsSame(ar))
                return;
            if (!this.Modified) return;
            m_Undo.Add(ar);
            if (m_Undo.Count > MAX_UNDO)
            {
                int ncount = MAX_UNDO - m_Undo.Count;
                for (int i = 0; i < ncount; i++)
                {
                    if (m_Undo.Count >0)
                        m_Undo.RemoveAt(0);
                }
            }
        }
        private bool lastUndoIsSame(List<string> ar)
        {
            if (m_Undo.Count <= 0)
                return false;
            List<string> arLast = m_Undo[m_Undo.Count - 1];
            if (ar.Count != arLast.Count)
                return false;
            for (int i = 0; i< arLast.Count ; i++)
            {
                if (!arLast[i].Equals(ar[i]) )
                    return false;
            }
            return true;
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

        
        private bool _LowerKey = false;
        /// <summary>
        /// Just accept lower key, 
        /// If input a upper, convert it to lower
        /// </summary>
        public bool LowerKey
        {
            get
            {
                return _LowerKey;
            }
            set
            {
                _LowerKey = value;
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
        /// <summary>
        /// 20140421: disable this function, allow all keys for combination
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool AllowForCombination(Keys key)
        {
           // return true;

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
        /// <summary>
        /// check if given function key is down(without the up)
        /// For this feature:
        /// 
        ///  Re-define the four function keys(Ctrl,  Shfit, Win, Alt).
        /// Currently, whether it¡¯s keyboard or On screen keyboard, when one of the
        /// function keys above pressed, it automatically output the break
        /// code([#Ctrl]). We want to make a change here. When Ctrl key is press, it
        /// will only output [Ctrl]. If it is a single Ctrl key, there is NO need of
        /// break code[#Ctrl]. This means if user only defines(press) Ctrl once, the key
        /// content box will ONLY show [Ctrl]. When there are more keys define with the
        /// four function key, let¡¯s say Ctrl+A, the output will be the same as before:
        /// [Ctrl]a[#Ctrl]. (User press Ctrl + A at the same time)There is another
        /// output here, Ctrl, A(Press Ctrl first, release it, then press A. Two
        /// different keys. E.G.: The short cut in KB9000, Alt, F). In this situation,
        /// the output will be [Ctrl][#Ctrl]a. (User Press Ctrl, release it, then a)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsFunctionKeyDown(Keys key, ref bool bIsLastKey)
        {
            string down = "";
            string up = "";
            bIsLastKey = false;
            switch(key)
            {
                case Keys.ControlKey:
                    {
                        down = KB9Const.CTRL_DOWN;// "[Ctrl]";
                        up = KB9Const.CTRL_UP;// "[#Ctrl]";
                    }
                    break;
                case Keys.RControlKey:
                    {
                        down = KB9Const.RCTRL_DOWN;// "[RightCtrl]";
                        up = KB9Const.RCTRL_UP;// "[#RightCtrl]";
                    }
                    break;
                case Keys.ShiftKey:
                    {
                        down = KB9Const.SHIFT_DOWN;// "[Shift]";
                        up = KB9Const.SHIFT_UP;// "[#Shift]";
                    }
                    break;
                case Keys.RShiftKey:
                    {
                        down = "[RightShift]";
                        up = "[#RightShift]";
                    }
                    break;
                case Keys.Menu:
                    {
                        down = "[Alt]";
                        up = "[#Alt]";
                    }
                    break;
                case Keys.RMenu:
                    {
                        down = "[RightAlt]";
                        up = "[#RightAlt]";
                    }
                    break;
                case Keys.LWin:
                    {
                        down = "[Win]";
                        up = "[#Win]";
                    }
                    break;
                case Keys.RWin:
                    {
                        down = "[RightWin]";
                        up = "[#RightWin]";
                    }
                    break;
                default:
                    return false;
                    

            }

            string strCode = this.Text;
            int ndown = strCode.LastIndexOf(down);
            int nup = strCode.LastIndexOf(up);
            if (ndown < 0)
                return false;
            if (nup >=0) //there is up code
            {
                if (nup > ndown)
                    return false;
            }
            if (ndown + down.Length >= strCode.Length)
                bIsLastKey = true;
            return true;
        }

        private bool is_function_key_string(string strKey)
        {
            if (strKey == "[Ctrl]" ||
                strKey == "[RightCtrl]" ||
                strKey == "[Shift]" ||
                strKey == "[RightShift]" ||
                strKey == "[Alt]" ||
                strKey == "[RightAlt]" ||
                strKey == "[Win]" ||
                strKey == "[RightWin]"
                )
                return true;
            return false;
        }
        //
        //private bool _AutoClearUndo = false;
        //public bool AutoClearUndo
        //{
        //    get
        //    {
                
        //        return _AutoClearUndo;
        //    }
        //    set
        //    {
        //        _AutoClearUndo = value;
        //    }
        //}

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;


namespace KB9Utility
{
    public class DEditor: DiagramEditor
    {
        static public List<int> m_lstX = new List<int>();
        static public List<int> m_lstY = new List<int>();


        public void ClearRedo()
        {
            m_objs.ClearRedo();
        }

        public void ClearUndo()
        {
            m_objs.ClearUndo();
        }
        public UndoItem getLastUndo()
        {
            int i = m_objs.UndoStack.Count - 1;
            if (i <0)
                return null;
            return (UndoItem)m_objs.UndoStack[i];


        }
        /// <summary>
        /// remove all new undo after give undoitem
        /// </summary>
        /// <param name="lastUndo">The last undo</param>
        public void removeNewUndo(UndoItem lastUndo)
        {
            if (lastUndo == null)
            {
                m_objs.UndoStack.Clear();
                return;
            }
            int n = m_objs.UndoStack.IndexOf(lastUndo);
            if (n < 0) return;
            int ncount = m_objs.UndoStack.Count;
            for (int i = n + 1; i < ncount; i++)
            {
                m_objs.UndoStack.RemoveAt(n + 1);
            }
        }
        protected override void DrawBackground(Graphics g, Rectangle rect, double zoom) 
        {
            base.DrawBackground(g, rect, zoom);
          
        }

        private bool GetRowsColsCoordinate(Rectangle rectButtons, Size gridSize, double zoom, List<int> lstX, List<int> lstY, int marginX, int  marginY)
        {
            lstX.Clear();
            lstY.Clear();
            
            m_lstX.Clear();
            m_lstY.Clear();
          
            
            int stepx = rectButtons.Width / gridSize.Width;
            int stepy = rectButtons.Height / gridSize.Height;

            int unitsZoomed =(int) decimal.Round(((decimal)KB9Const.UNIT_PIXELS * (decimal)zoom));


            for (int x = 0; x <= stepx; x++)
            {
                int xx = (int)decimal.Round( ((gridSize.Width * x) * (decimal)zoom));
                
                lstX.Add(xx);
                m_lstX.Add(xx + marginX);
                m_lstX.Add(xx + marginX + unitsZoomed);
            }
            for (int y = 0; y <= stepy; y++)
            {
         
                int yy = (int)decimal.Round( ((gridSize.Height * y) * (decimal)zoom));
               // if (y == stepy)
                 //   yy = (int)decimal.Round( (rectButtons.Height-1) * (decimal)zoom);
                lstY.Add(yy);
                m_lstY.Add(yy + marginY);
                m_lstY.Add(yy + marginY + unitsZoomed);
            
            }
            return true;
        }


        //private bool GetRowsColsCoordinate2(Rectangle rectButtons, Size gridSize, double zoom, List<int> lstX, List<int> lstY)
        //{
        //    lstX.Clear();
        //    lstY.Clear();

        //    //m_lstX.Clear();
        //    //m_lstY.Clear();


        //    int stepx = rectButtons.Width / gridSize.Width;
        //    int stepy = rectButtons.Height / gridSize.Height;

        //    int zoomedGridWith =(int) decimal.Round((decimal)gridSize.Width * (decimal)zoom);
        //    int zoomedGridHeight = (int)decimal.Round((decimal)gridSize.Height * (decimal)zoom);

        //    for (int x = 0; x <= stepx; x++)
        //    {
        //        //int xx = (int)decimal.Round(((gridSize.Width * x) * (decimal)zoom));
        //        int xx = x * zoomedGridWith;
        //        lstX.Add(xx);
        //       // m_lstX.Add(gridSize.Width * x);

        //    }
        //    for (int y = 0; y <= stepy; y++)
        //    {

        //        //int yy = (int)decimal.Round(((gridSize.Height * y) * (decimal)zoom));
        //        int yy = y * zoomedGridHeight;
        //        lstY.Add(yy);
        //        //m_lstY.Add(gridSize.Height * y);

        //    }
        //    return true;
        //}

        private Bitmap CreateLineDotImage(int nRectWidth, List<int> lstX, Color bg, Color fg, double zoom)
        {
            Bitmap bm = new Bitmap(nRectWidth, 1);    
            Graphics g = Graphics.FromImage(bm);
            //int nmargin = this.VirtualScreenMargin.Left;
            //nmargin = (int)((double)nmargin * zoom);
            g.Clear(bg);

            for (int i = 0; i < lstX.Count; i++)
            {
                int xx = lstX[i];// -nmargin;
                if (xx >=0 && xx <bm.Width)
                    bm.SetPixel(xx, 0, fg);      //
            }
            return bm;
           
        }

        private Bitmap CreateDotImage(Color bg)
        {
            Bitmap bm = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bm);


            g.Clear(bg);

            return bm;

        }

        protected override void DrawGrid(Graphics g, Rectangle rectAfterZoom, double zoom, bool bPrepartion)
        {

            if (this.GridStyle != System.Drawing.Drawing2D.DashStyle.Dot)
            {
                base.DrawGrid(g, rectAfterZoom, zoom, bPrepartion);
                return;
            }

            Color gridColor = this.GridLinesColor;// Color.Gray;
            Padding padding = this.VirtualScreenMargin;
            Size virtualSize = this.VirtualSize;
            Rectangle rectButtons = new Rectangle(padding.Left,
                                                padding.Top,
                                                virtualSize.Width - padding.Left - padding.Right,
                                                virtualSize.Height - padding.Top - padding.Bottom);


            
            Size gridSize = new Size(KB9Const.UNIT_PIXELS * 2, KB9Const.UNIT_PIXELS * 2);

            List<int> lstX = new List<int>();
            List<int> lstY = new List<int>();

            //int xx = lstX[0] + (int)(rectButtons.Left*zoom);// rect.Left + (int)((rectButtons.Left) * (decimal)zoom);
            int marginX = (int)(decimal.Round((decimal)rectButtons.Left * (decimal)zoom));
            int marginY = (int)(decimal.Round((decimal)rectButtons.Top * (decimal)zoom));
            if (!bPrepartion)
            {
                marginX += rectAfterZoom.Left;
                marginY += rectAfterZoom.Top ;
            }
            
            //int marginX = rectAfterZoom.Left + (int)(decimal.Round( (decimal)rectButtons.Left * (decimal)zoom));
            //int marginY = rectAfterZoom.Top + (int)(decimal.Round((decimal)rectButtons.Top * (decimal)zoom));
            

            GetRowsColsCoordinate(rectButtons, gridSize, zoom, lstX, lstY, marginX, marginY);

            if (bPrepartion)
                return;
           // int stepx = rectButtons.Width / gridSize.Width;
           // int stepy = rectButtons.Height / gridSize.Height;

            Bitmap rowBmp = CreateLineDotImage((int)(rectButtons.Width * (decimal)zoom), lstX, this.BackColor, gridColor, zoom);

            /*
            int xx = lstX[0] + marginX;
            for (int y = 0; y < lstY.Count; y++)
            {
                if (y == 0) continue;
                int yy = lstY[y] + marginY;// rect.Top + (int)((rectButtons.Top + gridSize.Height * y) * (decimal)zoom);
                g.DrawImageUnscaled(rowBmp, xx, yy);

            }*/

            //GraphicsState transState = g.Save(); ///将你的Graphics到GraphicsState中 
            //                                            ///
            //Bitmap bm = new Bitmap(rectAfterZoom.Width, rectAfterZoom.Height);
            //Graphics gg = Graphics.FromImage(bm);
            //gg.Restore(transState);
            //bm.Save("c:\\t\\rrr.bmp", ImageFormat.Bmp);

            

            int xx, yy;
            Bitmap dotBmp = CreateDotImage(gridColor);
            Color clrOld = Color.Gray;
            for (int x = 0; x < lstX.Count; x++)
            {
                xx = lstX[x] + marginX;
                for (int y = 0; y < lstY.Count; y++)
                {
                    yy = lstY[y] +marginY;

                    /*
                    uint c = WinAPI.GetPixel(g.GetHdc(), xx, yy);
                    g.ReleaseHdc();
                    Color clr = Color.FromArgb((int)c);
                    clr = ControlPaint.Dark(clr, 0.4F);
                    if (clr != clrOld)
                    {
                        dotBmp.SetPixel(0, 0, clr);
                        clrOld = clr;
                    }*/
                    //dotBmp = CreateDotImage(clr);
                    g.DrawImageUnscaled(dotBmp, xx, yy);
                }
            }

             DrawGridScale(g, rectAfterZoom, rectButtons, lstX, lstY, zoom);
           
        }

        protected void DrawGridScale(Graphics g, Rectangle rectVirtualAfterZoom, Rectangle rectButtonsBeforeZoom, List<int> lstX, List<int> lstY,  double zoom)
        {


            Color gridColor = this.GridLinesColor;// Color.Gray;
          
            Pen pen = new Pen(gridColor);
            int marginX = rectVirtualAfterZoom.Left + (int)decimal.Round( ((decimal) rectButtonsBeforeZoom.Left * (decimal)zoom));
            int marginY = rectVirtualAfterZoom.Top + (int)decimal.Round( ((decimal)rectButtonsBeforeZoom.Top * (decimal)zoom));
            int yy = marginY;
            int xx = marginX;
            Font ft = new Font("Arial", 9);
            SolidBrush brFont = new SolidBrush(gridColor);

            SizeF sz = g.MeasureString("9", ft);

            int nMmLenght = GridSize.Height;
            int nFontMargin = nMmLenght + (int)sz.Height;// +3;
            int nCmLenght = nMmLenght * 3;
            int nHalfLenght = nMmLenght * 2;
            

            const int nCmCount = 10; //grid size is double, so 10grid is 1cm
            const int nHalfCount = 5;
            //x
            for (int x = 0; x <lstX.Count; x++)
            {

                xx = lstX[x] + marginX;// rectVirtualAfterZoom.Left + (int)((rectButtons.Left + gridSize.Width * x) * (decimal)zoom);
                if (x % nCmCount == 0) //1 cm
                {
                    g.DrawLine(pen, xx, yy, xx, yy - nCmLenght);
                    int nCM = x / nCmCount;
                    g.DrawString(nCM.ToString(), ft, brFont, xx, yy - nFontMargin);
                }
                else if (x % nHalfCount == 0) //5mm
                {
                    g.DrawLine(pen, xx, yy, xx, yy - nHalfLenght);
                }
                else
                {
                    g.DrawLine(pen, xx, yy, xx, yy - nMmLenght);
                }
               

            }
            //at last coordinate, show (mm)
            Font ftMM = new Font("Arial", 7);
            xx = lstX[lstX.Count - 1] + marginX;// +3;
            g.DrawString("(cm)", ftMM, brFont, xx, yy - nFontMargin);

            //y
            xx = marginX;
            for (int y = 0; y < lstY.Count; y++)
            {

                yy = lstY[y] + marginY;// rectVirtualAfterZoom.Left + (int)((rectButtons.Left + gridSize.Width * x) * (decimal)zoom);
                if (y % nCmCount == 0) //1 cm
                {
                    g.DrawLine(pen, xx, yy, xx - nCmLenght, yy);
                    int nCM = y / nCmCount;
                    g.DrawString(nCM.ToString(), ft, brFont, xx - nFontMargin, yy);
                }
                else if (y % nHalfCount == 0) //5mm
                {
                    g.DrawLine(pen, xx, yy, xx - nHalfLenght, yy);
                }
                else
                {
                    g.DrawLine(pen, xx, yy, xx - nMmLenght, yy);
                }


            }

            
            yy = lstY[lstY.Count - 1] + marginY +3;
            g.DrawString("(cm)", ftMM, brFont, xx - nFontMargin, yy);
            
        }

        /// <summary>
        /// unused
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="zoom"></param>
        protected void DrawGrid2(Graphics g, Rectangle rect, double zoom) 
        {

            if ( this.GridStyle != System.Drawing.Drawing2D.DashStyle.Dot)
            {
                base.DrawGrid(g, rect, zoom, false);
                return;
            }

            Color gridcol = this.GridLinesColor;// GetGridColor();
            Color gridColor = this.GridLinesColor;// Color.Gray;

            Bitmap bm = new Bitmap(1, 1);    //这里调整点的大小  
            bm.SetPixel(0, 0, gridColor);      //设置点的颜色  
           

            
            ///
            Padding padding = this.VirtualScreenMargin;

           
            Size virtualSize = this.VirtualSize;

            Rectangle rectButtons = new Rectangle( padding.Left,
                                                padding.Top,
                                                virtualSize.Width - padding.Left - padding.Right,
                                                virtualSize.Height - padding.Top - padding.Bottom);

            
            Pen pen = new Pen(gridColor, 1);
            SolidBrush br = new SolidBrush(gridColor);
          
            //Size gridSize = new Size(6, 6);
            Size gridSize = new Size(KB9Const.UNIT_PIXELS*2, KB9Const.UNIT_PIXELS*2);
            int stepx = rectButtons.Width / gridSize.Width;
            int stepy = rectButtons.Height / gridSize.Height;


            
	        for( int x = 0 ; x <= stepx ; x++ )
                for (int y = 0; y <= stepy; y++)
                {
                    int xx = rect.Left + (int) ((rectButtons.Left + gridSize.Width * x)* (decimal)zoom);
                    int yy = rect.Top +  (int)(( rectButtons.Top + gridSize.Height * y) * (decimal)zoom);
                    g.DrawImageUnscaled(bm, xx, yy); 
                   
                }

           
        }


        /// <summary>
        /// create cvs text, 
        /// See http://baike.baidu.com/subview/468993/5926031.htm?fromId=468993&from=rdtself
        /// for cvs text format
        /// For KB9000 format:
        /// 0	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro1
        //1	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro2
        //2	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro3
        //3	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro4
        //4	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro5
        //5	[Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>	Key definitions
        //...	  ...	  ...
        //...	  ...	  ...
        //Last	[PS2 code set][Inter-Code-Delay][Touch sensitivity][Touch delay]<0x0d><0x0a>	Properties definition

        /// </summary>
        /// <returns></returns>
        public string CreateCVS()
        {
            DiagramEntity key = null;
            string strReturn = "";

            strReturn = CreateMacroCVS();// 
            for (int i = 0; i < m_objs.GetSize(); i++)
            {
                key = (DiagramEntity)(m_objs.GetAt(i));
                string s = key.ExportCVS();// CreateCVS();

                if (s != string.Empty)
                    strReturn += s;

            }
            strReturn += CreateKB9000CVS();
            return strReturn;
        }
        /// <summary>
        /// create lines for macro
        /// </summary>
        /// <returns></returns>
        private string CreateMacroCVS()
        {
            string strLine = "";
            string strReturn = "";
            string strFlag = KB9Const.CSV_Macro_Flag;// "[0,0,1,1][0][0]";//macro flag
            string strMacro = "";
            string strLF = KB9Const.LF;

            strMacro = this.Macro1.ToCSV(false);
            strLine = strFlag + strMacro + strLF;
            strReturn += strLine;

            strMacro = this.Macro2.ToCSV(false);
            strLine = strFlag + strMacro + strLF;
            strReturn += strLine;

            strMacro = this.Macro3.ToCSV(false);
            strLine = strFlag + strMacro + strLF;
            strReturn += strLine;

            strMacro = this.Macro4.ToCSV(false);
            strLine = strFlag + strMacro + strLF;
            strReturn += strLine;

            strMacro = this.Macro5.ToCSV(false);
            strLine = strFlag + strMacro + strLF;
            strReturn += strLine;

            return strReturn;
        }
        /// <summary>
        /// create the cvs text for KB9000 hardware setting.
        ///[PS2 code set][Inter-Code-Delay][Touch sensitivity][Touch delay]<0x0d><0x0a>
        /// </summary>
        /// <returns></returns>
        private string CreateKB9000CVS()
        {
            string strResult = "";
            string strLF = KB9Const.LF;

            strResult = "[2]";//PS2, fixed value.
            strResult +=  Util.BracketString(this.InterCharDelay.ToString());
            strResult += Util.BracketString(this.Sensitivity.ToString() );
            strResult += Util.BracketString( this.TouchDelay.ToString() );
            strResult += strLF;
            return strResult;

        }
        public override void ExportToXml(CLCIXML xml)
        /* ============================================================
	        Function :		ExportToXml
	        Description :	Saves a string representation of the 
					        container (normally the virtual size) and 
					        all objects to "stra".
	        Access :		Public

	        Return :		void
	        Parameters :	CStringArray& stra	-	The array to fill
        					
	        Usage :			Call to save the data of the editor to a 
					        XML. 

           ============================================================*/
        {

            xml.new_group("screen", true);
            xml.new_attribute("image", DiagramKey.ImageToBase64(this.Image, ImageFormat.Png));
            xml.new_attribute("bg", ColorTranslator.ToWin32(this.BackColor).ToString());
            xml.new_attribute("borderbg", ColorTranslator.ToWin32(this.BorderColor).ToString());
            xml.new_attribute("interchardelay", this.InterCharDelay.ToString());
            xml.new_attribute("sensitivity", this.Sensitivity.ToString());
            xml.new_attribute("touchdelay", this.TouchDelay.ToString());
            xml.new_attribute("macro1", this.Macro1.ToString());
            xml.new_attribute("macro2", this.Macro2.ToString());
            xml.new_attribute("macro3", this.Macro3.ToString());
            xml.new_attribute("macro4", this.Macro4.ToString());
            xml.new_attribute("macro5", this.Macro5.ToString());
            xml.new_attribute("count", m_objs.GetSize().ToString());
            xml.back_to_parent();

            m_objs.ExportToXml(xml);

            this.Modified = false; ;// SetModified(FALSE);
        }

       
        public override bool ImportFromXml(CLCIXML xml)
        /* ============================================================
	        Function :		ImportFromXml
	        Description :	Load configuration from application self xml file.
	        Access :		Public

	        Return :		bool				-	"true" if "str" is a 
											        representation of a 
											        'paper'.
	        Parameters :	const string& str	-	String representation.
        					
	        Usage :			Call to set the size of the virtual paper 
					        from a string.

           ============================================================*/
        {

            //load background color
            xml.back_to_root();
            if (!xml.get_first_group("screen"))
                return false;
            string s = "";
            xml.get_attribute("image", ref s);
            if (s.Length > 0)
                this._Image =DiagramKey.Base64ToImage(s);

            string strVal = string.Empty;

            xml.get_attribute("bg", ref strVal);
            //if (strVal == string.Empty)
            //    strVal = "16777215";
            Color c = System.Drawing.ColorTranslator.FromWin32(Util.string2int(strVal,16777215));
            this.BackColor = c;

            strVal = string.Empty;
            xml.get_attribute("borderbg", ref strVal);
            //if (strVal == string.Empty)
            //    strVal = "16777215";
            Color clr = System.Drawing.ColorTranslator.FromWin32(Util.string2int(strVal, 16777215));
            this.BorderColor = clr;

            
            strVal = "";
            xml.get_attribute("interchardelay", ref strVal);
            this.InterCharDelay = Util.string2int(strVal, 50);//.ToString());

            strVal = "";
            xml.get_attribute("sensitivity", ref strVal);
            this.Sensitivity = Util.string2int(strVal, 50);//.ToString());

            strVal = "";
            xml.get_attribute("touchdelay", ref strVal);
            this.TouchDelay = Util.string2int(strVal, 50);//.ToString());


            strVal = "";
            xml.get_attribute("macro1", ref strVal);
            this.Macro1.SetKeyCodeString(strVal);

            strVal = "";
            xml.get_attribute("macro2", ref strVal);
            this.Macro2.SetKeyCodeString(strVal);

            strVal = "";
            xml.get_attribute("macro3", ref strVal);
            this.Macro3.SetKeyCodeString(strVal);

            strVal = "";
            xml.get_attribute("macro4", ref strVal);
            this.Macro4.SetKeyCodeString(strVal);

            strVal = "";
            xml.get_attribute("macro5", ref strVal);
            this.Macro5.SetKeyCodeString(strVal);
        
            //////////////

            xml.get_attribute("count", ref strVal);
            int nCount = int.Parse(strVal);
            xml.back_to_parent();
            bool result = m_objs.ImportFromXml(xml, nCount);
            m_objs.ClearRedo();
            m_objs.ClearUndo();
            Refresh();
       
            return result;

        }

        /// <summary>
        /// load setting from cvs text
        /// 0	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro1
        //1	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro2
        //2	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro3
        //3	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro4
        //4	[0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>	Macro5
        //5	[Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>	Key definitions
        //...	  ...	  ...
        //...	  ...	  ...
        //Last	[PS2 code set][Inter-Code-Delay][Touch sensitivity][Touch delay]<0x0d><0x0a>	Properties definition

        /// </summary>
        /// <param name="strCVS"></param>
        public void LoadCSV(string strCSV)
        {
            if (strCSV.Length == 0) return;
            List<string> ar = new List<string>();
            if (Util.string2lines(strCSV, ar) < 5)
                return;

            string s = "";
            //parse 5 macro
            s = ar[0];
            this.Macro1.FromCSV(parse_csv_macro(s));

            s = ar[1];
            this.Macro2.FromCSV(parse_csv_macro(s));

            s = ar[2];
            this.Macro3.FromCSV(parse_csv_macro(s));

            s = ar[3];
            this.Macro4.FromCSV(parse_csv_macro(s));

            s = ar[4];
            this.Macro5.FromCSV(parse_csv_macro(s));




            //others is the buttons setting, except last line
            int i = 5;
            //for (int i=5; i<ar.Count-1; i++)
            while (i < ar.Count - 1)
            {
                DiagramKey key = null;
                s = ar[i];
                if (!is_key_csv(s))
                {
                    i++;
                    continue;
                }
                if (s.IndexOf(KB9Const.CSV_SlideL) >= 0)
                {
                    //if (i + 3 < ar.Count - 1)
                    //{
                        key = parse_csv_key_slideh(s);
                        //i += 4;
                        
                    //}
                    //else
                        i++;
                }
                else if (s.IndexOf( KB9Const.CSV_SlideU) >= 0)
                {
                   // if (i + 3 < ar.Count - 1)
                    //{
                        key = parse_csv_key_slidev(s);//, ar[i + 1], ar[i + 2], ar[i + 3]);
                      //  i += 4;
                    //}
                    //else
                        i++;
                }
                else
                {
                    key = parse_csv_key_area(s);
                    i++;
                }

                if (key != null)
                    m_objs.Add(key);
            }

            s = ar[ar.Count - 1];
            parse_csv_kb9000(s);

            this.Refresh();
            FireShowPropertiesEvent(null);
        }
        /// <summary>
        /// check if give string is for button.
        /// detect the ",".
        /// </summary>
        /// <param name="strCSV"></param>
        /// <returns></returns>
        private bool is_key_csv(string strCSV)
        {
            return (strCSV.IndexOf(",") >= 0);

        }
        /// <summary>
        /// [0,0,1,1][0][0][Macro key contents] <0x0d> <0x0a>
        /// </summary>
        /// <param name="strCSV"></param>
        /// <returns></returns>
        private string parse_csv_macro(string strCSV)
        {
            string strFlag = KB9Const.CSV_Macro_Flag;// "[0,0,1,1][0][0]";
            if (strCSV.IndexOf(strFlag) < 0)
                return "";
            string s = strCSV;
            s = s.Replace(strFlag, "");
            return s;


        }
        /// <summary>
        /// /// create the cvs text for KB9000 hardware setting.
        /// 
        /// [PS2 code set][Inter-Code-Delay][Touch sensitivity][Touch delay]<0x0d><0x0a>
        /// </summary>
        /// <param name="s"></param>
        private void parse_csv_kb9000(string strCSV)
        {
            List<string> ar = new List<string>();
            if (Util.string2array(strCSV, "]", ar, false) <= 0)
                return;

            for (int i = 0; i < ar.Count; i++)
            {
                ar[i] = ar[i].Replace("[", "");
            }
            //now just properties data in array.
            string s = "";
            int n = 0;

            //ps2 set
            //s = ar[0];
            //n = Util.string2int(s, (int)PS2Code.XT);
            //this.PS2Set = (PS2Code)n;

            //oposjpos
            //s = ar[0];
            //this.OPOSJPOS = (s.IndexOf("E") >=0);
            if (ar.Count < 4)
            {
                //default values
                this.InterCharDelay =KB9Const.DEFAULT_InterCharDelay;
                this.Sensitivity = KB9Const.DEFAULT_Sensitivity;
                this.TouchDelay = KB9Const.DEFAULT_TouchDelay;
            }
            else
            {



                s = ar[1];
                n = Util.string2int(s, KB9Const.DEFAULT_InterCharDelay);
                this.InterCharDelay = n;


                //s = ar[2];
                //this.Typematic = (s.IndexOf("E")>=0);

                s = ar[2];
                n = Util.string2int(s, KB9Const.DEFAULT_Sensitivity);
                this.Sensitivity = n;

                s = ar[3];
                n = Util.string2int(s, KB9Const.DEFAULT_TouchDelay);
                this.TouchDelay = n;
            }

        }

        /// <summary>
        /// 
        /// [Key rectangle][Beep pitch][Beep duration][Key content 0] ... [Key content n]<0x0d><0x0a>
        /// [126,26,154,42][0][0][S][u][m][m][a][r][y]<0x0d><0x0a>
        /// </summary>
        /// <param name="str"></param>
        private DiagramKeyArea parse_csv_key_area(string str)
        {

            DiagramKeyArea key = new DiagramKeyArea();
            if (!key.FromCSV(str))
                return null;
            key.Title = CreateUniqueName(key.TypeName);

            return key;
        }

        private DiagramKeySlideH parse_csv_key_slideh(string strCSV)
        {
            DiagramKeySlideH key = new DiagramKeySlideH();
            if (!key.FromCSV(strCSV))
                return null;
            key.Title = CreateUniqueName(key.TypeName);

            return key;
        }
        private DiagramKeySlideH parse_csv_key_slideh_removed(string strCSV0, string strCSV1, string strCSV2, string strCSV3)
        {
            DiagramKeySlideH key = new DiagramKeySlideH();
            if (!key.FromCSV(strCSV0, strCSV1, strCSV2, strCSV3))
                return null;
            key.Title = CreateUniqueName(key.TypeName);

            return key;
        }
        private DiagramKeySlideV parse_csv_key_slidev(string strCSV)
        {
            DiagramKeySlideV key = new DiagramKeySlideV();
            if (!key.FromCSV(strCSV))
                return null;
            key.Title = CreateUniqueName(key.TypeName);

            return key;
        }

        private DiagramKeySlideV parse_csv_key_slidev_removed(string strCSV0, string strCSV1, string strCSV2, string strCSV3)
        {
            DiagramKeySlideV key = new DiagramKeySlideV();
            if (!key.FromCSV(strCSV0, strCSV1, strCSV2, strCSV3))
                return null;
            key.Title = CreateUniqueName(key.TypeName);

            return key;
        }

        public Image CreateImage()
        {
            //Bitmap bmp = new Bitmap(this.VirtualSize.Width, this.VirtualSize.Height);
            Size szImg = new Size(KB9Const.IMAGE_CX, KB9Const.IMAGE_CY);

            Bitmap bmp = new Bitmap(szImg.Width, szImg.Height);
            bmp.SetResolution(KB9Const.IMAGE_DPI, KB9Const.IMAGE_DPI);// 300, 300);
            Rectangle rect = new Rectangle(new Point(0, 0), szImg);
            Graphics g = Graphics.FromImage(bmp);
            
            bool bGrid = this.ShowGridLines;
            bool bShowMargin = this.ShowVirtualScreenMargin;
            double dblZoom = this.Zoom;

            //

            //this.ShowGridLines = false;
           // this.ShowVirtualScreenMargin = false;
            //this.Zoom = 1.0;
            this.SetPropertiesForExportImage(1.0, false, false );
            this.m_objs.show_entity_icon(false);
            EraseBackground(g, rect);
           
            //dc.SetWindowOrg(sih.nPos, siv.nPos);

            Draw(g, rect, true);

            Rectangle rt = rect;
            rt.X = 0;
            rt.Y = 0;
            rt.Width -= 1;
            rt.Height -= 1;
            //rt.Inflate(1, 1);
            g.DrawRectangle(new Pen(Color.Black, 1.0F), rt);
            this.SetPropertiesForExportImage(dblZoom, bGrid, bShowMargin);
            //this.Zoom = dblZoom;
            //this.ShowGridLines = bGrid;
            //this.ShowVirtualScreenMargin = bShowMargin;
            this.m_objs.show_entity_icon(true);
            this.Refresh();
            return bmp;
        }


        public Image CreateImage3X()
        {
            int nGap = 100;
            Size szImg = new Size(KB9Const.IMAGE_CX, KB9Const.IMAGE_CY);

            Bitmap bmp = new Bitmap(szImg.Width, szImg.Height*3+nGap*2);
            Image image = CreateImage();

            bmp.SetResolution(KB9Const.IMAGE_DPI, KB9Const.IMAGE_DPI);// 300, 300);
            Rectangle rect = new Rectangle(new Point(0, 0), szImg);
            Graphics g = Graphics.FromImage(bmp);
            EraseBackground(g, rect);
            g.DrawImage(image, new Point(0, 0));
            g.DrawImage(image, new Point(0, szImg.Height+nGap));
            g.DrawImage(image, new Point(0, (szImg.Height+nGap)*2));

           
            this.Refresh();
            return bmp;
        }
        /************************************************************************/
        /* 
         * check overlapped error
         */
        /************************************************************************/
        public bool ErrorOverlapped()
        {
            int ncount = m_objs.GetSize();
            List<Rectangle> ar = new List<Rectangle>();
            for (int i=0; i< ncount; i++)
            {
                ar.Clear();
                if (m_objs.GetOverlappedRect2(m_objs.GetAt(i), ar) > 0)
                    return true;
            }
            //check the matrix again
            for (int i = 0; i < ncount; i++)
            {
                //ar.Clear();
                DiagramEntity entity = m_objs.GetAt(i);
                if (entity.TypeName == DiagramKeyMatrix.KEY_MATRIX)
                {
                    if (((DiagramKeyMatrix)entity).CheckInternalOverlapped())
                        return true;
                }
                //if (m_objs.GetOverlappedRect2(m_objs.GetAt(i), ar) > 0)
                //    return true;
            }
                return false;
        }


        /************************************************************************/
        /* 
         * check key code error
         */
        /************************************************************************/
        public DiagramKey ErrorKeyUp()
        {
            int ncount = m_objs.GetSize();
            KB9Validation checker = new KB9Validation();
            for (int i = 0; i < ncount; i++)
            {
                DiagramEntity entity = m_objs.GetAt(i);
                if (entity.TypeName == DiagramKey.KEY_BUTTON )
                {
                    DiagramKeyArea key = (DiagramKeyArea)entity;
                    string code = key.KeyCode.GetKeyCodeString();
                    if (checker.ValidateKB9Keycodes(code) != KB9Validation.ERR_VALIDATION.OK)
                         return key;
                }
                else if (entity.TypeName == DiagramKey.KEY_SLIDEH)
                {
                    DiagramKeySlideH key = (DiagramKeySlideH)entity;

                    string slideLeft = key.SlideLeft.GetKeyCodeString();
                     if (checker.ValidateKB9Keycodes(slideLeft) != KB9Validation.ERR_VALIDATION.OK)
                         return key;
                    string slideLeftHold = key.SlideLeftHold.GetKeyCodeString();
                     if (checker.ValidateKB9Keycodes(slideLeftHold) != KB9Validation.ERR_VALIDATION.OK)
                         return key;
                    string slideRight = key.SlideRight.GetKeyCodeString();
                    if (checker.ValidateKB9Keycodes(slideRight) != KB9Validation.ERR_VALIDATION.OK)
                        return key;
                    string slideRightHold = key.SlideRightHold.GetKeyCodeString();
                    if (checker.ValidateKB9Keycodes(slideRightHold) != KB9Validation.ERR_VALIDATION.OK)
                        return key;

                   
                }
                else if (entity.TypeName == DiagramKey.KEY_SLIDEV)
                {
                    DiagramKeySlideV key = (DiagramKeySlideV)entity;

                    string slideUp = key.SlideUp.GetKeyCodeString();
                    if (checker.ValidateKB9Keycodes(slideUp) != KB9Validation.ERR_VALIDATION.OK)
                        return key;
                     string slideUpHold = key.SlideUpHold.GetKeyCodeString();
                     if (checker.ValidateKB9Keycodes(slideUpHold) != KB9Validation.ERR_VALIDATION.OK)
                         return key;
                     string slideDown = key.SlideDown.GetKeyCodeString();
                     if (checker.ValidateKB9Keycodes(slideDown) != KB9Validation.ERR_VALIDATION.OK)
                         return key;
                     string slideDownHold = key.SlideDownHold.GetKeyCodeString();
                     if (checker.ValidateKB9Keycodes(slideDownHold) != KB9Validation.ERR_VALIDATION.OK)
                         return key;

                }
                
            }
            return null;
        }



        #region _KB9000_Properties_


        //private bool _Typematic = false;
        ///// <summary>
        ///// enable typematic or no typematic = t, nt
        ///// </summary>
        ///// 
        //[Description("Typematic or not")]
        //public bool Typematic
        //{
        //    get { return _Typematic; }
        //    set
        //    {
        //        _Typematic = value;
        //        this.Modified = true;
        //    }
        //}
        ///// <summary>
        ///// range 2 -- 255
        ///// 20140408, change range to 1--100
        ///// </summary>
        //private int _InterCharDelay = KB9Const.DEFAULT_InterCharDelay;
        //[Description("Inter-Character delay time. Value Range: [1, 100]")]
        //[Category("Keyboard")]
        //[DisplayName("Inter char delay")]
        ////[TypeConverter(typeof(NumConverter))]
        //public int InterCharDelay
        //{
        //    get { return _InterCharDelay; }
        //    set
        //    {
        //        int val = value;
        //        if (val >= KB9Const.Min_InterCharDelay && val <= KB9Const.Max_InterCharDelay)
        //        {
        //           // this.m_objs.Snapshot();
        //            _InterCharDelay = value;
        //            this.Modified = true;
        //        }
        //        else if (val > KB9Const.Max_InterCharDelay)
        //        {//20140429, set to max, min
        //          //  this.m_objs.Snapshot();
        //            _InterCharDelay = KB9Const.Max_InterCharDelay;
        //            this.Modified = true;
        //        }
        //        else if (val <  KB9Const.Min_InterCharDelay)
        //        {
        //           // this.m_objs.Snapshot();
        //            _InterCharDelay = KB9Const.Min_InterCharDelay;
        //            this.Modified = true;
        //        }
        //    }
        //}

     

        //private int _TouchDelay = KB9Const.DEFAULT_TouchDelay;
        //[Description("Touch Delay Time. Value Range: [1, 10]")] //20140408, change from 1--100, to 1--10
        //[DisplayName("Touch delay")]
        //[Category("Keyboard")]
        ////[TypeConverter(typeof(NumConverter))]
        //public int TouchDelay
        //{
        //    get { return _TouchDelay; }
        //    set
        //    {
        //        int val = value;
        //        if (val >= KB9Const.Min_TouchDelay && val <= KB9Const.Max_TouchDelay)
        //        {
        //           // this.m_objs.Snapshot();
        //            _TouchDelay = value;
        //            this.Modified = true;
        //        }
        //        //20140429, set to max, min
        //        else if (val < KB9Const.Min_TouchDelay)
        //        {
        //            //this.m_objs.Snapshot();
        //            _TouchDelay = KB9Const.Min_TouchDelay;
        //            this.Modified = true;
        //        }
        //        else if (val > KB9Const.Max_TouchDelay)
        //        {
        //            //this.m_objs.Snapshot();
        //            _TouchDelay = KB9Const.Max_TouchDelay;
        //            this.Modified = true;
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// 
        /// </summary>
        //private int _Sensitivity = KB9Const.DEFAULT_Sensitivity;
        //[Description("Sensitivity. Value Range: [1, 10]")]//20140408, change from 1--100, to 1--10
        //[DisplayName("Touch sensitivity")]
        //[Category("Keyboard")]
        ////[TypeConverter(typeof(NumConverter))]
        //public int Sensitivity
        //{
        //    get { return _Sensitivity; }
        //    set
        //    {
        //        int val = value;
        //        if (val >= KB9Const.Min_Sensitivity && val <= KB9Const.Max_Sensitivity)
        //        {
        //           // this.m_objs.Snapshot();
        //            _Sensitivity = value;
        //            this.Modified = true;
        //        }
        //        //20140429, set to max, min
        //        else if (val < KB9Const.Min_Sensitivity)
        //        {
        //            //this.m_objs.Snapshot();
        //            _Sensitivity = KB9Const.Min_Sensitivity;
        //            this.Modified = true;
        //        }
        //        else if (val > KB9Const.Max_Sensitivity)
        //        {
        //            //this.m_objs.Snapshot();
        //            _Sensitivity = KB9Const.Max_Sensitivity;
        //            this.Modified = true;
        //        }
        //    }
        //}
        //private bool _OPOSJPOS = false;
        //[DisplayName("Enable OPOS/JPOS")]
        //[Description("Enable internal OPOS/JPOS command")]
        //public bool OPOSJPOS
        //{
        //    get { return _OPOSJPOS; }
        //    set
        //    {
        //        if (_OPOSJPOS != value)
        //        {

        //            _OPOSJPOS = value;
        //            this.Modified = true;
        //        }
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        private KeyEditingType _Macro1 = new KeyEditingType();// = "";
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Define the Macro #1")]
        public KeyEditingType Macro1
        {
            get { return _Macro1; }
            set
            {
                if (_Macro1 != value)
                {

                    _Macro1 = value;
                    this.Modified = true;
                    //FirePropertiesChangedEvent();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private KeyEditingType _Macro2 = new KeyEditingType();// = "";
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Define the Macro #2")]
        public KeyEditingType Macro2
        {
            get { return _Macro2; }
            set
            {
                if (_Macro2 != value)
                {

                    _Macro2 = value;
                    this.Modified = true;
                    //FirePropertiesChangedEvent();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private KeyEditingType _Macro3 = new KeyEditingType();// = "";
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Define the Macro #3")]
        public KeyEditingType Macro3
        {
            get { return _Macro3; }
            set
            {
                if (_Macro3 != value)
                {

                    _Macro3 = value;
                    this.Modified = true;
                    //FirePropertiesChangedEvent();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private KeyEditingType _Macro4 = new KeyEditingType();// = "";
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Define the Macro #4")]
        public KeyEditingType Macro4
        {
            get { return _Macro4; }
            set
            {
                if (_Macro4 != value)
                {

                    _Macro4 = value;
                    this.Modified = true;
                    //FirePropertiesChangedEvent();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private KeyEditingType _Macro5 = new KeyEditingType();// = "";
        [Browsable(false)]
        [Editor(typeof(KeyCodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Define the Macro #5")]
        public KeyEditingType Macro5
        {
            get { return _Macro5; }
            set
            {
                if (_Macro5 != value)
                {

                    _Macro5 = value;
                    this.Modified = true;
                    //FirePropertiesChangedEvent();
                }
            }
        }

        //[Category("Legend Sheet")]
        //[DisplayName("Background color")]
        //[Editor(typeof(ColorEditorUI), typeof(UITypeEditor))]
        //[TypeConverter(typeof(ColorConverterUI))]
        //public new Color BackColor
        //{
        //    get { return base.BackColor; }
        //    set
        //    {
        //        if (base.BackColor != value)
        //        {
        //            if (m_objs != null)
        //                m_objs.Snapshot();
        //            base.BackColor = value;
        //            this.Modified = true;
        //            if (this.OnRefreshPropertiesGrid != null)
        //                this.OnRefreshPropertiesGrid(this, null);
        //            //Refresh();
        //        }
        //    }
        //}
        //public enum PS2Code
        //{
        //    AT = 1, //AT,XT,PS2
        //    XT,
        //    PS2,


        //}
        //private PS2Code _PS2Set = PS2Code.XT ;
        //[DisplayName("PS/2 Set")]
        //[Description("PS/2 code set")]
        //public PS2Code PS2Set
        //{
        //    get { return _PS2Set; }
        //    set
        //    {
        //        if (_PS2Set != value)
        //        {

        //            _PS2Set = value;
        //            this.Modified = true;
        //        }
        //    }
        //}
        #endregion

        public void set_all_keys_beep_duration(int nDuration)
        {
            int ncount = m_objs.GetSize();
            for (int i=0; i< ncount; i++)
            {
                DiagramEntity entity = (DiagramEntity)m_objs.GetAt(i);
                if (entity.TypeName == DiagramKeyMatrix.KEY_MATRIX)
                {
                    ((DiagramKeyMatrix)entity).set_all_child_beep_duration(nDuration);
                }
                else
                {


                    DiagramKey key = (DiagramKey)m_objs.GetAt(i);
                    key.BeepDuration = nDuration;
                }
            }
        }

        public void reset()
        {
            this._Macro1.SetKeyCodeString("");
            this._Macro2.SetKeyCodeString("");
            this._Macro3.SetKeyCodeString("");
            this._Macro4.SetKeyCodeString("");
            this._Macro5.SetKeyCodeString("");
            this._Sensitivity = KB9Const.DEFAULT_Sensitivity;
            this._InterCharDelay = KB9Const.DEFAULT_InterCharDelay;
            this._TouchDelay = KB9Const.DEFAULT_TouchDelay;
            base.BackColor = SystemColors.Window;
            this.ShowGridLines = true;
        }

    }
}

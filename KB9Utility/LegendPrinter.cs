using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace KB9Utility
{
    class LegendPrinter
    {
        // class to hold settings for the PrintDialog presented to the user during
        // the print process
        public class PrintDialogSettingsClass
        {
            public bool AllowSelection = true;
            public bool AllowSomePages = true;
            public bool AllowCurrentPage = true;
            public bool AllowPrintToFile = false;
            public bool ShowHelp = true;
            public bool ShowNetwork = true;
            public bool UseEXDialog = true;
        };
        // Identify the reason for a new page when tracking rows
        enum paging { keepgoing, outofroom, datachange };

        private Image m_Image;
        // print document
        PrintDocument printDoc = null;
       // private Margins m_PrintMargins = new Margins(60, 60, 40, 40);

        /// <summary>
        /// Page margins override. Default is (60, 60, 40, 40)
        /// </summary>
        public Margins PrintMargins
        {
            get { return PageSettings.Margins; }
            set { PageSettings.Margins = value; }
        }


        /// <summary>
        /// Set Printer Name
        /// </summary>
        private String printerName;
        public String PrinterName
        {
            get { return printerName; }
            set { printerName = value; }
        }
        /// <summary>
        /// Expose the printdocument default page settings to the caller
        /// </summary>
        public PageSettings PageSettings
        {
            get { return printDoc.DefaultPageSettings; }
        }


        /// <summary>
        /// Allow caller to set print preview dialog
        /// </summary>
        private KB9Utility.CoolPrintPreviewDialog previewdialog = null;
        public KB9Utility.CoolPrintPreviewDialog PreviewDialog
        {
            get { return previewdialog; }
            set { previewdialog = value; }
        }

        /// <summary>
        /// provide an override for the print preview dialog "owner" field
        /// Note: Changed style for VS2005 compatibility
        /// </summary>
        //public Form Owner
        //{ get; set; }
        protected Form _Owner = null;
        public Form Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        public LegendPrinter()
        {
            printDoc = new PrintDocument();
            PrintMargins = new Margins(60, 60, 40, 40);
        }
        /// <summary>
        /// Start the printing process, print to a print preview dialog
        /// </summary>
        /// <param name="dgv">The DataGridView to print</param>
        /// NOTE: Any changes to this method also need to be done in PrintDataGridView
        public void PrintPreviewImage(Image img)
        {
          
          m_Image = img;

          //if (DialogResult.OK == DisplayPrintDialog()) //DAVID commented it.
          {
              PrintPreviewNoDisplay(img);
          }
          

           
        }

        private PrintDialogSettingsClass printDialogSettings = new PrintDialogSettingsClass();
        public PrintDialogSettingsClass PrintDialogSettings
        {
            get { return printDialogSettings; }
        }


        /// <summary>
        /// Display a printdialog and return the result. Either this method or 
        /// the equivalent must be done prior to calling either of the PrintNoDisplay
        /// or PrintPreviewNoDisplay methods.
        /// </summary>
        /// <returns></returns>
        public DialogResult DisplayPrintDialog()
        {
           
            // create new print dialog and set options
            PrintDialog pd = new PrintDialog();
            pd.UseEXDialog = printDialogSettings.UseEXDialog;
            pd.AllowSelection = printDialogSettings.AllowSelection;
            pd.AllowSomePages = printDialogSettings.AllowSomePages;
            pd.AllowCurrentPage = printDialogSettings.AllowCurrentPage;
            pd.AllowPrintToFile = printDialogSettings.AllowPrintToFile;
            pd.ShowHelp = printDialogSettings.ShowHelp;
            pd.ShowNetwork = printDialogSettings.ShowNetwork;

            //// setup print dialog with internal setttings
            pd.Document = printDoc;
            if (!String.IsNullOrEmpty(printerName))
                printDoc.PrinterSettings.PrinterName = printerName;

            // show the dialog and display the result
            return pd.ShowDialog();
        }

        
        public void PrintPreviewNoDisplay(Image img)
        {
            
            m_Image = img;

            printDoc.PrintPage += new PrintPageEventHandler(PrintPageEventHandler);
            printDoc.BeginPrint += new PrintEventHandler(BeginPrintEventHandler);

            // display the preview dialog
            SetupPrint();

            // if the caller hasn't provided a print preview dialog, then create one
            if (null == PreviewDialog)
                //PreviewDialog = new PrintPreviewDialog();
                PreviewDialog = new KB9Utility.CoolPrintPreviewDialog();

            // set up dialog for preview
            PreviewDialog.Document = printDoc;
            //PreviewDialog.UseAntiAlias = true;
            PreviewDialog.Owner = Owner;
            //PreviewDialog.PrintPreviewControl.Zoom = PrintPreviewZoom;
            PreviewDialog.Width = PreviewDisplayWidth();
            PreviewDialog.Height = PreviewDisplayHeight();

            //if (null != ppvIcon)
            //    PreviewDialog.Icon = ppvIcon;

            // show the dialog
            PreviewDialog.ShowDialog();
        }


        /// <summary>
        /// PrintPage event handler. This routine prints one page. It will
        /// skip non-printable pages if the user selected the "some pages" option
        /// on the print dialog.
        /// </summary>
        /// <param name="sender">default object from windows</param>
        /// <param name="e">Event info from Windows about the printing</param>
        public void PrintPageEventHandler(object sender, PrintPageEventArgs e)
        {
           // if (EnableLogging) Logger.LogInfoMsg("PrintPageEventHandler called. Printing a page.");
            e.HasMorePages = PrintPage(e.Graphics);
        }

        //int currentpageset = 0;
        //int lastrowprinted = -1;
        //int CurrentPage = 0;
        /// <summary>
        /// BeginPrint Event Handler
        /// Set values at start of print run
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BeginPrintEventHandler(object sender, PrintEventArgs e)
        {
           
            // reset counters since we'll go through this twice if we print from preview
            //currentpageset = 0;
            //lastrowprinted = -1;
            //CurrentPage = 0;
        }


        int pageHeight = 0;
        //float staticheight = 0;
        //float rowstartlocation = 0;
        int pageWidth = 0;
        int printWidth = 0;
        //float rowheaderwidth = 0;
        
        
        

        PrintRange printRange;
        int fromPage = 0;
        int toPage = -1;
        const int maxPages = 2147483647;
         int totalpages = 0;


        /// <summary>
        /// Set up the print job. Save information from print dialog
        /// and print document for easy access. Also sets up the rows
        /// and columns that will be printed. At this point, we're 
        /// collecting all columns in colstoprint. This will be broken
        /// up into pagesets later on 
        /// </summary>
        void SetupPrint()
        {
            
            
            //-----------------------------------------------------------------
            // get info on the limits of the printer's actual print area available. Convert
            // to int's to work with margins.
            //
            // note: do this only if we're not doing embedded printing.
            //-----------------------------------------------------------------

            printDoc.DefaultPageSettings.Landscape = false;// true;
            printDoc.DefaultPageSettings.PrinterResolution.X = KB9Const.IMAGE_DPI;//
            printDoc.DefaultPageSettings.PrinterResolution.Y = KB9Const.IMAGE_DPI;//
            //= printDoc.PrinterSettings.PrinterResolutions.
            int printareawidth;
            int hardx = (int)Math.Round(printDoc.DefaultPageSettings.HardMarginX);
            int hardy = (int)Math.Round(printDoc.DefaultPageSettings.HardMarginY);
            if (printDoc.DefaultPageSettings.Landscape)
                printareawidth = (int)Math.Round(printDoc.DefaultPageSettings.PrintableArea.Height);
            else
                printareawidth = (int)Math.Round(printDoc.DefaultPageSettings.PrintableArea.Width);

            //-----------------------------------------------------------------
            // set the print area we're working within
            //-----------------------------------------------------------------

            pageHeight = printDoc.DefaultPageSettings.Bounds.Height;
            pageWidth = printDoc.DefaultPageSettings.Bounds.Width;

            //-----------------------------------------------------------------
            // Set the printable area: margins and pagewidth
            //-----------------------------------------------------------------

            // Set initial printer margins 
            PrintMargins = printDoc.DefaultPageSettings.Margins;
            PrintMargins.Left = 50; //Set printing default to portrait with left margin = 0.5 inch.

            // adjust for when the margins are less than the printer's hard x/y limits
            PrintMargins.Right = (hardx > PrintMargins.Right) ? hardx : PrintMargins.Right;
            PrintMargins.Left = (hardx > PrintMargins.Left) ? hardx : PrintMargins.Left;
            PrintMargins.Top = (hardy > PrintMargins.Top) ? hardy : PrintMargins.Top;
            PrintMargins.Bottom = (hardy > PrintMargins.Bottom) ? hardy : PrintMargins.Bottom;

            // Now, we can calc default print width, again, respecting the printer's limitations
            printWidth = pageWidth - PrintMargins.Left - PrintMargins.Right;
            printWidth = (printWidth > printareawidth) ? printareawidth : printWidth;
           
            //-----------------------------------------------------------------
            // Figure out which pages / rows to print
            //-----------------------------------------------------------------

            // save print range 
            printRange = printDoc.PrinterSettings.PrintRange;
            

            // pages to print handles "some pages" option
            if (PrintRange.SomePages == printRange)
            {
                // set limits to only print some pages
                fromPage = printDoc.PrinterSettings.FromPage;
                toPage = printDoc.PrinterSettings.ToPage;
            }
            else
            {
                // set extremes so that we'll print all pages
                fromPage = 0;
                toPage = maxPages;
            }

            //-----------------------------------------------------------------
            // Determine what's going to be printed
            //-----------------------------------------------------------------
            //SetupPrintRange();

            //-----------------------------------------------------------------
            // Set up width overrides and fixed columns
            //-----------------------------------------------------------------
            //SetupColumns();

            //-----------------------------------------------------------------
            // Now that we know what we're printing, measure the print area and
            // count the pages.
            //-----------------------------------------------------------------

            // Measure the print area
           // measureprintarea(printDoc.PrinterSettings.CreateMeasurementGraphics());

            // Count the pages
            totalpages = Pagination();

        }

        /// <summary>
        /// calculate the print preview window width to show the entire page
        /// </summary>
        /// <returns></returns>
        private int PreviewDisplayWidth()
        {
            double displayWidth = printDoc.DefaultPageSettings.Bounds.Width
                + 3 * printDoc.DefaultPageSettings.HardMarginY;
            return (int)(displayWidth * PrintPreviewZoom);
        }

        /// <summary>
        /// calculate the print preview window height to show the entire page
        /// </summary>
        /// <returns></returns>
        private int PreviewDisplayHeight()
        {
            double displayHeight = printDoc.DefaultPageSettings.Bounds.Height
                + 3 * printDoc.DefaultPageSettings.HardMarginX;

            return (int)(displayHeight * PrintPreviewZoom);
        }
        /// <summary>
        /// provide an override for the print preview zoom setting
        /// Note: Changed style for VS2005 compatibility
        /// </summary>
        //public Double PrintPreviewZoom
        //{ get; set; }
        protected Double _PrintPreviewZoom = 1.0;
        public Double PrintPreviewZoom
        {
            get { return _PrintPreviewZoom; }
            set { _PrintPreviewZoom = value; }
        }


        /// <summary>
        /// This routine prints one page. It will skip non-printable pages if the user 
        /// selected the "some pages" option on the print dialog. This is called during 
        /// the Print event.
        /// </summary>
        /// <param name="g">Graphics object to print to</param>
        private bool PrintPage(Graphics g)
        {
            
            

            //// flag for continuing or ending print process
            //bool HasMorePages = false;

            //// flag for handling printing some pages rather than all
            //bool printthispage = false;

            //// current printing position within one page
            //float printpos = PrintMargins.Top;// pagesets[currentpageset].margins.Top;

            //// increment page number & check page range
            //CurrentPage++;
            
            //if ((CurrentPage >= fromPage) && (CurrentPage <= toPage))
            //    printthispage = true;

            //// calculate the static vertical space available - this is where we stop printing rows
            //staticheight = pageHeight - PrintMargins.Top;// FooterHeight - pagesets[currentpageset].margins.Bottom;

            //// count space used as we work our way down the page
            //float used = 0;

            Margins m = printDoc.DefaultPageSettings.Margins;

            g.DrawImageUnscaled(m_Image, new Point(m.Left, m.Top));
         //   g.DrawImageUnscaled(m_Image, new Point(PrintMargins.Left, PrintMargins.Top));
            //g.DrawImage(m_Image,
            //            new Rectangle( new Point( PrintMargins.Left,PrintMargins.Top),
            //                            new Size(m_Image.Width, m_Image.Height)));

            
            //-----------------------------------------------------------------
            // print headers
            //-----------------------------------------------------------------


            return false;
        }


        /// <summary>
        /// Determine the print range based on dialog selections and user input. The rows
        /// and columns are sorted to ensure that the rows appear in their correct index 
        /// order and the columns appear in DisplayIndex order to account for added columns
        /// and re-ordered columns.
        ///// </summary>
        //private void SetupPrintRange()
        //{
           
        //}

        ///// <summary>
        ///// Scan all the rows and columns to be printed and calculate the 
        ///// overall individual column width (based on largest column value), 
        ///// the header sizes, and determine all the row heights.
        ///// </summary>
        ///// <param name="g">The graphics context for all measurements</param>
        //private void measureprintarea(Graphics g)
        //{
           
        //}

             /// <summary>
        /// Set page breaks for the rows to be printed, and count total pages
        /// </summary>
        private int Pagination()
        {
            return 1;
        }

        public void PrintWithDialog(Image img)
        {


            // display dialog and print
            if (DialogResult.OK == DisplayPrintDialog())
                PrintNoDisplay(img);
            
        }

        /// <summary>
        /// Print the provided grid view. Either DisplayPrintDialog() or it's equivalent
        /// setup must be completed prior to calling this routine
        /// </summary>
        /// <param name="dgv"></param>
        public void PrintNoDisplay(Image img)
        {
            

            // save the grid we're printing
            //this.dgv = dgv;
            m_Image  = img;

            printDoc.PrintPage += new PrintPageEventHandler(PrintPageEventHandler);
            printDoc.BeginPrint += new PrintEventHandler(BeginPrintEventHandler);

            // setup and do printing
            SetupPrint();
            printDoc.Print();
        }

    }
}

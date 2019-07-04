using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;

using System.ComponentModel.Design;


namespace KB9Utility
{
	public enum eButtonForm
	{
		Rectangle,
		Ellipse
	}

	public enum eTextAlign
	{
		Bottom,
		Right,
		None,
        TopLeft, //no image
        Center, //no image
	}

	/// <summary>
	/// Summary description for NSButton.
	/// </summary>
   // [Designer(typeof(KB9Utility.NSButtonDesigner))]
	public class NSButton : System.Windows.Forms.Control
	{
		private enum MouseState
		{
			Normal = 0,
			Hoover,
			Pressed
		}
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private eButtonForm m_ButtonForm = eButtonForm.Rectangle;
		private MouseState m_MouseState = MouseState.Normal;
		private Color m_HighlightColor = SystemColors.HotTrack;
		private Image m_HottrackImage = null;
		private Image m_NormalImage = null;
		private Image m_PressedImage = null;
		private eTextAlign m_TextAlign = eTextAlign.Bottom;
        //private EnhancedToolTip m_ToolTip = null;//new EnhancedToolTip(this.components);
		private string m_ToolTipText="";
		private bool m_OnlyShowBitmap = false;

		public NSButton()
		{
			InitializeComponent();
           // m_ToolTip = new EnhancedToolTip(this.components);

			SetStyle(ControlStyles.DoubleBuffer | 
				ControlStyles.UserPaint | 
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.StandardDoubleClick |
				ControlStyles.UserMouse |
				ControlStyles.StandardClick |
                
				ControlStyles.SupportsTransparentBackColor,
				true);
            SetStyle(ControlStyles.Selectable, false);
			Size = new Size(23, 23);
			TabStop = false;
            //m_ToolTip.SetToolTip(this, this.ToolTip);

			//m_ToolTip.Active = true;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // NSButton
            // 
            this.TabStop = false;
            this.MouseLeave += new System.EventHandler(this.NSButton_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NSButton_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NSButton_MouseDown);
            this.MouseHover += new System.EventHandler(this.NSButton_MouseHover);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NSButton_MouseUp);
            this.MouseEnter += new System.EventHandler(this.NSButton_MouseEnter);
            this.ResumeLayout(false);

		}
		#endregion

		private void DrawBorder(Graphics g)
		{
			if (m_OnlyShowBitmap)
			{
				return;
			}
			if (m_MouseState == MouseState.Normal)
			{
				switch (m_ButtonForm)
				{
				case eButtonForm.Rectangle:
					break;
				case eButtonForm.Ellipse:
					break;
				}
				return;
			}

			if (m_MouseState == MouseState.Hoover || m_MouseState == MouseState.Pressed)
			{
				switch (m_ButtonForm)
				{
				case eButtonForm.Rectangle:
					//g.FillRectangle(new SolidBrush(m_HighlightColor), 0, 0, Width - 1, Height - 1);
                    LinearGradientBrush br = new LinearGradientBrush(this.ClientRectangle, Color.White, this.m_HighlightColor, LinearGradientMode.Vertical);
                    g.FillRectangle(br, this.ClientRectangle);

					g.DrawRectangle(SystemPens.ControlDarkDark, 0, 0, Width-1, Height-1 );
					break;
				case eButtonForm.Ellipse:
					g.FillEllipse(new SolidBrush(m_HighlightColor), 0, 0, Width - 1, Height - 1);
					g.DrawEllipse(SystemPens.ControlDarkDark, 0, 0, Width - 1, Height - 1);
					break;
				}
				return;
			}
		}

		protected void DrawImage(Graphics g)
		{
			if (m_NormalImage == null)
			{
				return;
			}

			int x = 3, y = 3;
			if (m_TextAlign == eTextAlign.Right)
			{
				y = (Height - m_NormalImage.Height) / 2;
			}
			if (m_TextAlign == eTextAlign.Bottom)
			{
				x = (Width - m_NormalImage.Width) / 2;
			}
			if (m_TextAlign == eTextAlign.None)
			{
				x = (Width - m_NormalImage.Width) / 2;
				y = (Height - m_NormalImage.Height) / 2;
			}

			if (m_MouseState == MouseState.Normal)
			{
				if (Enabled)
				{
					if (m_NormalImage != null)
					{
						g.DrawImage(m_NormalImage, x, y, m_NormalImage.Width, m_NormalImage.Height);
					}
				}
				else
				{
					if (m_NormalImage != null)
					{
						ControlPaint.DrawImageDisabled(g, m_NormalImage, x, y, BackColor);
					}
				}
				return;
			}
			if (m_MouseState == MouseState.Hoover)
			{
				if (m_HottrackImage != null)
				{
					g.DrawImage(m_HottrackImage, x, y, m_HottrackImage.Width, m_HottrackImage.Height);
				}
				else if (m_NormalImage != null)
				{
					g.DrawImage(m_NormalImage, x, y, m_NormalImage.Width, m_NormalImage.Height);
				}
				return;
			}
			if (m_MouseState == MouseState.Pressed)
			{
				if (m_PressedImage != null)
				{
					g.DrawImage(m_PressedImage, x + 1, y + 1, m_PressedImage.Width, m_PressedImage.Height);
				}
				else if (m_NormalImage != null)
				{
					g.DrawImage(m_NormalImage, x + 1, y + 1, m_NormalImage.Width, m_NormalImage.Height);
				}
				return;
			}
		}

		public void DrawText(Graphics g)
		{
			if (m_OnlyShowBitmap)
			{
				return;
			}
			SizeF w = g.MeasureString(Text, Font);
			SizeF h = g.MeasureString("X", Font);

			int x = 0, y = 0;

			if (m_TextAlign == eTextAlign.Bottom)
			{
				x = (Width - (int)w.Width) / 2;
				y = Height - (int)h.Height - 2;
			}
            else if (m_TextAlign == eTextAlign.Right)
            {
                if (m_NormalImage != null)
                {
                    x = m_NormalImage.Width + 5;
                }
                else
                {
                    x = 5;
                }
                y = (Height - (int)h.Height) / 2;
            }
            else if (m_TextAlign == eTextAlign.TopLeft)
            {
                x = 5;
                y = 5;
            }
            else if (m_TextAlign == eTextAlign.Center)
            {
                x = (Width - (int)w.Width) / 2;
                y = (Height - (int)h.Height)/2 ;
            }


			if (m_MouseState == MouseState.Pressed)
			{
				x++;
				y++;
			}

			Brush b;
			if (Enabled)
			{
                b = new SolidBrush(this.ForeColor);// SystemBrushes.ControlText;
			}
			else
			{
				b = SystemBrushes.ControlDark;
			}

            if (m_TextAlign == eTextAlign.None)
                return;

			//if (m_TextAlign == eTextAlign.Bottom)
			//{
				g.DrawString(Text, Font, b, x, y);
			//	return;
			//}
			//else if (m_TextAlign == eTextAlign.Right)
			//{
			//	g.DrawString(Text, Font, b, x, y);
			//	return;
			//}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
            //LinearGradientBrush br = new LinearGradientBrush(this.ClientRectangle, Color.White, this.BackColor, LinearGradientMode.Vertical);
            //pevent.Graphics.FillRectangle(br, this.ClientRectangle);

		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			if (m_ButtonForm == eButtonForm.Ellipse)
			{
				System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
				buttonPath.AddEllipse(0, 0, Width, Height);
				Region = new System.Drawing.Region(buttonPath);
			}
			Graphics g = pevent.Graphics;
			DrawBorder(g);
			DrawImage(g);
			DrawText(g);
		}

		private void NSButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_MouseState = MouseState.Pressed;
			Invalidate();
          
		}

		private void NSButton_MouseEnter(object sender, System.EventArgs e)
		{
			m_MouseState = MouseState.Hoover;
			Invalidate();
            //m_ToolTip.Active = true;
		}

		private void NSButton_MouseHover(object sender, System.EventArgs e)
		{
		
		}

		private void NSButton_MouseLeave(object sender, System.EventArgs e)
		{
			m_MouseState = MouseState.Normal;
			Invalidate();
            
		}

		private void NSButton_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (m_MouseState == MouseState.Normal)
			{
			    m_MouseState = MouseState.Hoover;
			    Invalidate();
			}
		}

		private void NSButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_MouseState = MouseState.Normal;
			Invalidate();
            //m_ToolTip.SetToolTip(this, this.ToolTip);
            //m_ToolTip.SetToolTipWhenDisabled(this, this.ToolTip);
            
            //m_ToolTip.Active = false;
            //m_ToolTip.Active = true;
		}

		[
		Category("Appearance"),
		Description("The form of the button")
		]
		public eButtonForm ButtonForm
		{
			get
			{
				return m_ButtonForm;
			}
			set
			{
				m_ButtonForm = value;
				Invalidate();
			}
		}

		[
		Category("Appearance"),
		Description("The \"mouse over\" color of the button")
		]
		public Color HighlightColor
		{
			get
			{
				return m_HighlightColor;
			}
			set
			{
				m_HighlightColor = value;
			}
		}

		[
		Category("Appearance"),
		Description("Where the text is showed...")
		]
		public eTextAlign TextAlign
		{
			get
			{
				return m_TextAlign;
			}
			set
			{
				m_TextAlign = value;
				Invalidate();
			}
		}

		[
		Category("Appearance"),
		Description("Where the text is showed...")
		]
		public bool OnlyShowBitmap
		{
			get
			{
				return m_OnlyShowBitmap;
			}
			set
			{
				m_OnlyShowBitmap = value;
				Invalidate();
			}
		}

        [
        Category("Appearance"),
        Description("The tooltip showed")
        ]
        public string ToolTip
        {
            get
            {
                return m_ToolTipText;
            }
            set
            {
                //m_ToolTipText = value;
                //m_ToolTip.SetToolTip(this, m_ToolTipText);
                //m_ToolTip.SetToolTipWhenDisabled(this, m_ToolTipText);
            }
        }

		[
		Category("Images"),
		Description("The image shown when the button is \"idle\"")
		]
		public Image NormalImage
		{
			get
			{
				return m_NormalImage;
			}
			set
			{
				m_NormalImage = value;
				Invalidate();
			}
		}

		[
		Category("Images"),
		Description("The image shown when the mouse is over the button")
		]
		public Image HottrackImage
		{
			get
			{
				return m_HottrackImage;
			}
			set
			{
				m_HottrackImage = value;
				Invalidate();
			}
		}

		[
		Category("Images"),
		Description("The image shown when the mouse button is pressed")
		]
		public Image PressedImage
		{
			get
			{
				return m_PressedImage;
			}
			set
			{
				m_PressedImage = value;
				Invalidate();
			}
		}

	}

}



























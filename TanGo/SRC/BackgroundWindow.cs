using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TanGo
{
	public partial class BackgroundWindow:Form
	{		
		public BackgroundWindow()
		{	InitializeComponent();
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		//сокрытие из Alt+Tab и панели задач
		protected override CreateParams CreateParams
		{	get
			{	CreateParams cp = base.CreateParams;
				//скрытие из Alt+Tab и панели задач
				cp.ExStyle |= 0x80;
				return cp;
			}
		}
	
		public float Radius, AbsRadius;
		public Color Color;
		protected override void OnPaint(PaintEventArgs e)
		{	base.OnPaint(e);
    		e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; //сглаживание
    		Rectangle Rectangle = new Rectangle(0, 0, this.Bounds.Width-1, this.Bounds.Height);
			//радиус не должен превышать половину меньшей стороны
			AbsRadius = Math.Min(Rectangle.Width / 2, Rectangle.Height / 2) * this.Radius * 2;
			using (GraphicsPath Path = new GraphicsPath())
			{	Path.AddArc(Rectangle.X				 , Rectangle.Y				 , AbsRadius, AbsRadius, 180, 90);
			    Path.AddArc(Rectangle.Right-AbsRadius, Rectangle.Y				 , AbsRadius, AbsRadius, 270, 90);
			   	Path.AddArc(Rectangle.Right-AbsRadius, Rectangle.Bottom-AbsRadius, AbsRadius, AbsRadius,   0, 90);
			   	Path.AddArc(Rectangle.X				 , Rectangle.Bottom-AbsRadius, AbsRadius, AbsRadius,  90, 90);
				Path.CloseFigure();
				using (Brush brush = new SolidBrush(Color))
					e.Graphics.FillPath(brush, Path);
			}
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		}
}

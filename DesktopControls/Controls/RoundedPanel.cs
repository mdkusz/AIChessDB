using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Panel con las esquinas redondeadas y borde negro / 
    /// Panel with rounded corners and black border
    /// </summary>
    public class RoundedPanel : Panel
    {
        private int _cornerRadius = 15;

        public int CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;


            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, _cornerRadius, _cornerRadius), 180, 90);
            path.AddArc(new Rectangle(Width - _cornerRadius - 1, 0, _cornerRadius, _cornerRadius), 270, 90);
            path.AddArc(new Rectangle(Width - _cornerRadius - 1, Height - _cornerRadius - 1, _cornerRadius, _cornerRadius), 0, 90);
            path.AddArc(new Rectangle(0, Height - _cornerRadius - 1, _cornerRadius, _cornerRadius), 90, 90);
            path.CloseAllFigures();

            GraphicsPath borderpath = new GraphicsPath();
            borderpath.AddArc(new Rectangle(0, 0, _cornerRadius, _cornerRadius), 180, 90);
            borderpath.AddArc(new Rectangle(Width - (_cornerRadius + 4), 0, _cornerRadius, _cornerRadius), 270, 90);
            borderpath.AddArc(new Rectangle(Width - (_cornerRadius + 4), Height - (_cornerRadius + 4), _cornerRadius, _cornerRadius), 0, 90);
            borderpath.AddArc(new Rectangle(0, Height - (_cornerRadius + 4), _cornerRadius, _cornerRadius), 90, 90);
            borderpath.CloseAllFigures();

            Region = new Region(path);

            using (Pen pen = new Pen(Color.Black, 1))
            {
                g.DrawPath(pen, borderpath);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}


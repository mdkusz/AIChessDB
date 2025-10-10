using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Panel with ToolTip support
    /// </summary>
    /// <remarks>
    /// This Panel is used in property editors to show a ToolTip with the property or method description when the mouse is over it.
    /// Decorate the property or method with the Description attrute and put there the tooltip text.
    /// </remarks>
    /// <descendant>InputEditorBase</descendant>
    public class ToolTipPanel : Panel
    {
        private ToolTip _toolTip;
        private Timer _toolTipTimer;
        private string _toolTipText;
        private string _toolTipTitle;

        public ToolTipPanel()
        {
            _toolTip = new ToolTip()
            {
                ShowAlways = true,
                IsBalloon = true,
                ToolTipIcon = ToolTipIcon.Info,
                Active = false
            };
            _toolTipTimer = new Timer()
            {
                Interval = 10000,
                Enabled = false
            };
            _toolTipTimer.Tick += (s, e) =>
            {
                if (_toolTip.Active)
                {
                    _toolTip.Active = false;
                    _toolTip.Hide(this);
                }
                _toolTipTimer.Stop();
            };
        }
        /// <summary>
        /// ToolTip title
        /// </summary>
        public string Title
        {
            get => _toolTipTitle;
            set => _toolTipTitle = value;
        }
        /// <summary>
        /// Text to show in the ToolTip
        /// </summary>
        public string Description
        {
            get => _toolTipText;
            set => _toolTipText = value;
        }
        /// <summary>
        /// Horizontal offset for the ToolTip
        /// </summary>
        public int ToolTipHorizontalOffset { get; set; } = 30;
        /// <summary>
        /// Border size. Each side can have a different size. 0 means no border.
        /// </summary>
        public Padding BorderSize { get; set; } = new Padding(0, 0, 0, 0);
        /// <summary>
        /// Border colors
        /// </summary>
        public Color LeftBorderColor { get; set; }
        public Color TopBorderColor { get; set; }
        public Color RightBorderColor { get; set; }
        public Color BottomBorderColor { get; set; }
        protected override void OnMouseEnter(EventArgs e)
        {
            // Show ToolTip when mouse enter the panel
            base.OnMouseEnter(e);
            string spmessage = ConfigureTooltipText(out int hl);
            BeginInvoke((Action)(() => SetToolTip(new Point(Right - 40, Height < 50 ? -(35 + hl * 15) : ((Height / 2) - (35 + hl * 15))), spmessage)));
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            // Hide ToolTip when mouse leave the panel
            BeginInvoke((Action)(() =>
            {
                var p = PointToClient(Cursor.Position);
                if (!ClientRectangle.Contains(p))
                {
                    _toolTipTimer.Stop();
                    _toolTip.Active = false;
                    _toolTip.Hide(this);
                }
                // Avoid hidding the tooltip if the mouse is still over the panel
            }));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            string spmessage = ConfigureTooltipText(out int hl);
            BeginInvoke((Action)(() => SetToolTip(new Point(Right - 40, Height < 50 ? -(35 + hl * 15) : ((Height / 2) - (35 + hl * 15))), spmessage)));
        }
        protected virtual void SetToolTip(Point at, string msg)
        {
            if (!_toolTip.Active)
            {
                _toolTip.Active = true;
                if (!string.IsNullOrEmpty(msg))
                {
                    Point pt = PointToScreen(new Point(at.X - 40, at.Y + 10));
                    Point oldPos = Cursor.Position;
                    Cursor.Position = pt;
                    Application.DoEvents();
                    _toolTip.Show(msg, this, at, 10000);
                    Cursor.Position = oldPos;
                    _toolTipTimer.Start();
                }
            }
        }
        /// <summary>
        /// Break the tooltip text in lines of max 32 characters
        /// </summary>
        /// <param name="hl">
        /// Text lines count
        /// </param>
        /// <returns>
        /// Tooltip text with line breaks
        /// </returns>
        protected virtual string ConfigureTooltipText(out int hl)
        {
            _toolTip.ToolTipTitle = _toolTipTitle;
            string spmessage = _toolTipText;
            hl = 1;
            if (spmessage?.Length > 32)
            {
                spmessage = spmessage.Replace("\r", "").Replace("\n", " ").Replace("  ", " ");
                List<string> lsm = new List<string>();
                while (!string.IsNullOrEmpty(spmessage))
                {
                    if (spmessage.Length <= 32)
                    {
                        lsm.Add(spmessage);
                        spmessage = null;
                    }
                    else
                    {
                        int ix = spmessage.IndexOf(' ', 32);
                        if (ix < 0)
                        {
                            lsm.Add(spmessage);
                            spmessage = null;
                        }
                        else
                        {
                            lsm.Add(spmessage.Substring(0, ix));
                            spmessage = spmessage.Substring(ix + 1);
                        }
                    }
                }
                hl = lsm.Count;
                spmessage = string.Join("\r\n", lsm);
            }
            return spmessage;
        }
        /// <summary>
        /// Overriden to draw borders
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (BorderSize.Top > 0)
            {
                using (Pen p = new Pen(TopBorderColor, BorderSize.Top))
                {
                    e.Graphics.DrawLine(p, 0, 0, Width, 0);
                }
            }
            if (BorderSize.Left > 0)
            {
                using (Pen p = new Pen(LeftBorderColor, BorderSize.Left))
                {
                    e.Graphics.DrawLine(p, 0, 0, 0, Height);
                }
            }
            if (BorderSize.Right > 0)
            {
                using (Pen p = new Pen(RightBorderColor, BorderSize.Right))
                {
                    e.Graphics.DrawLine(p, Width - 1, 0, Width - 1, Height);
                }
            }
            if (BorderSize.Bottom > 0)
            {
                using (Pen p = new Pen(BottomBorderColor, BorderSize.Bottom))
                {
                    e.Graphics.DrawLine(p, 0, Height - 1, Width, Height - 1);
                }
            }
        }
    }
}


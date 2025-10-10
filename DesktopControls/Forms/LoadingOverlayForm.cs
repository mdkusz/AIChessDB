using DesktopControls.Forms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Forms
{
    /// <summary>
    /// Overlay form to show a loading animation over a target form
    /// </summary>
    public sealed class LoadingOverlayForm : Panel
    {
        private readonly Form _target;
        private readonly Timer _timer;
        private Control _fillControl;
        private int _angle = 0;
        private void OnTargetFormClosed(object sender, FormClosedEventArgs e) => Close();
        public string OverlayMessage { get; set; }

        public LoadingOverlayForm(Form target, string message = null) : base()
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            if (!string.IsNullOrEmpty(message)) OverlayMessage = message;

            // Enable proper transparent painting and smooth redraws
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BorderStyle = BorderStyle.None;
            Dock = DockStyle.Fill;
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Visible = false;
            Enabled = true;

            // Timer animation
            _timer = new Timer { Interval = 75 };
            _timer.Tick += (_, __) => { _angle = (_angle + 30) % 360; Invalidate(); };

            // Refit and hook events
            HookTarget();
        }
        // Make Windows paint siblings first, then us (enables real-looking transparency)
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                UnhookTarget();
            base.Dispose(disposing);
        }
        private void HookTarget()
        {
            _fillControl = _target.Controls
                        .Cast<Control>()
                        .FirstOrDefault(c => c.Dock == DockStyle.Fill);
            if (_fillControl == null)
            {
                throw new InvalidOperationException(ERR_NOFILLCONTROL);
            }
            if (!(_fillControl is MdiClient))
            {
                Bitmap bmp = new Bitmap(_fillControl.Width, _fillControl.Height);
                _fillControl.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                BackgroundImage = bmp;
            }
            _target.FormClosed += OnTargetFormClosed;
            _target.Controls.Add(this);
            Visible = true;
            BringToFront();
            _timer.Start();
        }
        private void UnhookTarget()
        {
            _target.FormClosed -= OnTargetFormClosed;
            _target.Controls.Remove(this);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
                BackgroundImage = null;
                Bitmap bmp = new Bitmap(_fillControl.Width, _fillControl.Height);
                _fillControl.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                BackgroundImage = bmp;
            }
        }
        public void Close()
        {
            _timer.Stop();
            _timer.Dispose();
            UnhookTarget();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var sb = new SolidBrush(Color.FromArgb(80, Color.LightGray)))
            {
                g.FillRectangle(sb, ClientRectangle);
            }

            // Center
            int cx = ClientSize.Width / 2;
            int cy = ClientSize.Height / 2;

            // Spinner: 12 spokes with decreasing alpha
            int ExtSpinner = Math.Max(24, Math.Min(ClientSize.Width, ClientSize.Height) / 10);
            int InnSpinner = ExtSpinner - 8;
            int Spinners = 12;

            for (int i = 0; i < Spinners; i++)
            {
                int a = (_angle + i * (360 / Spinners)) % 360;
                double rad = a * Math.PI / 180.0;
                int x1 = cx + (int)(Math.Cos(rad) * InnSpinner);
                int y1 = cy + (int)(Math.Sin(rad) * InnSpinner);
                int x2 = cx + (int)(Math.Cos(rad) * ExtSpinner);
                int y2 = cy + (int)(Math.Sin(rad) * ExtSpinner);

                int alpha = (int)(255 * (i + 1) / (double)Spinners); // more transparent for the smallest
                using (var pen = new Pen(Color.FromArgb(alpha, 255, 0, 0), 3f))
                {
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }

            // Message
            using (var f = new Font(SystemFonts.DefaultFont.FontFamily, 20f, FontStyle.Bold))
            {
                var sz = g.MeasureString(OverlayMessage, f);
                var msgRect = new RectangleF(cx - sz.Width / 2, cy + ExtSpinner + 12, sz.Width, sz.Height);
                using (var sbText = new SolidBrush(Color.Red))
                {
                    g.DrawString(OverlayMessage, f, sbText, msgRect);
                }
            }
        }
    }
}
/// <summary>
/// Overlay scope to use in a using statement
/// </summary>
public sealed class OverlayScope : IDisposable
{
    private readonly LoadingOverlayForm _f;
    public OverlayScope(Form target, string msg = null)
    {
        _f = new LoadingOverlayForm(target, msg);
    }
    /// <summary>
    /// Allows to change the message in a thread-safe way
    /// </summary>
    public string Message
    {
        get => _f.OverlayMessage;
        set
        {
            if (_f.InvokeRequired)
            {
                _f.Invoke(new Action(() => _f.OverlayMessage = value));
            }
            else
            {
                _f.OverlayMessage = value;
            }
        }
    }
    public void Dispose() => _f.Close();
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Ventana para resaltar un control de la interfaz de usuario / 
    /// Window to highlight a control in the user interface
    /// </summary>
    public partial class fHighlight : Form
    {
        [Flags]
        public enum HighlightMode
        {
            Tooltip = 1,
            Circle = 2,
            Rectangle = 4,
            Flash = 8
        }
        private ConcurrentQueue<HighlightRequest> _highlightQueue = new ConcurrentQueue<HighlightRequest>();
        private bool _working = false;
        private Color _backColor;
        private Color _foreColor;
        private string _title;
        private string _message;
        private Size szCircle;
        private int _miliseconds;
        private ToolTip _bubbleMessage = null;
        public fHighlight()
        {
            InitializeComponent();
            Show();
            Hide();
        }
        /// <summary>
        /// Modo de resaltado / 
        /// Highlight mode
        /// </summary>
        public HighlightMode Mode { get; set; }
        /// <summary>
        /// Título del globo de ayuda / 
        /// Tooltip title
        /// </summary>
        public string ToolTipCaption
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        /// <summary>
        /// Mensaje del globo de ayuda / 
        /// Tooltip message
        /// </summary>
        public string ToolTipMessage
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
        /// <summary>
        /// Control a resaltar / 
        /// Control to highlight
        /// </summary>
        [Browsable(false)]
        public object CtlToHighlight { get; set; }
        /// <summary>
        /// Milisegundos antes de cerrar la ventana / 
        /// Milliseconds before closing the window
        /// </summary>
        public int CloseInMs
        {
            get
            {
                return _miliseconds;
            }
            set
            {
                _miliseconds = value;
                fTimer.Interval = value;
            }
        }
        /// <summary>
        /// Add a highlight request to the queue. If the window is not visible, it will be shown.
        /// </summary>
        /// <param name="request">
        /// Request to highlight a control in the user interface.
        /// </param>
        public void AddHighlightRequest(HighlightRequest request)
        {
            if (request != null)
            {
                if (_working || _highlightQueue.Count > 0)
                {
                    _highlightQueue.Enqueue(request);
                }
                else
                {
                    if (SetRequest(request))
                    {
                        _working = true;
                        if (!Visible)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                Show();
                            }));
                        }
                        BeginInvoke(new Action(() =>
                        {
                            PositionForm();
                        }));
                        BeginInvoke(new Action(() =>
                        {
                            BeginHighlight();
                        }));
                    }
                }
            }
        }
        /// <summary>
        /// Process the next highlight request in the queue.
        /// </summary>
        private void NextRequest()
        {
            while (_highlightQueue.TryDequeue(out HighlightRequest request))
            {
                if (SetRequest(request))
                {
                    _working = true;
                    if (!Visible)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            Show();
                        }));
                    }
                    BeginInvoke(new Action(() =>
                    {
                        PositionForm();
                    }));
                    BeginInvoke(new Action(() =>
                    {
                        BeginHighlight();
                    }));
                    return;  // Process only one request at a time
                }
            }
        }
        /// <summary>
        /// Set the request properties from the HighlightRequest object.
        /// </summary>
        /// <param name="request">
        /// Request to highlight a control in the user interface.
        /// </param>
        /// <returns>
        /// True if the request is valid.
        /// </returns>
        private bool SetRequest(HighlightRequest request)
        {
            Mode = request.Mode;
            ToolTipCaption = request.ToolTipCaption;
            ToolTipMessage = request.ToolTipMessage;
            CtlToHighlight = request.CtlToHighlight;
            CloseInMs = Math.Max(2000, Math.Min(request.CloseInMs, 10000));
            if (CtlToHighlight is Control ctl)
            {
                if (ctl.IsDisposed)
                {
                    return false;  // Object is disposed, skip it
                }
            }
            else if (CtlToHighlight is ToolStripItem tsi)
            {
                if (tsi.Owner == null)
                {
                    return false; // Owner is null, skip it
                }
            }
            return true;
        }
        /// <summary>
        /// Establecer el contenido de la ventana / 
        /// Set the window content
        /// </summary>
        /// <param name="sz">
        /// Tamaño del contenido / 
        /// Content size
        /// </param>
        private void SetContent(Size sz)
        {
            szCircle = sz;
            if ((Mode & HighlightMode.Tooltip) != 0)
            {
                _bubbleMessage = new ToolTip()
                {
                    ToolTipTitle = ToolTipCaption,
                    ToolTipIcon = ToolTipIcon.Info,
                    IsBalloon = true,
                    ShowAlways = true
                };
            }
        }
        private void PositionForm()
        {
            try
            {
                Control ctl = CtlToHighlight as Control;
                if (ctl != null)
                {
                    Size sz = new Size(ctl.Width + 20, ctl.Height + 20);
                    Location = ctl.PointToScreen(new Point(-10, -10));
                    Size = sz;
                    SetContent(sz);
                }
                else
                {
                    ToolStripComboBox cb = CtlToHighlight as ToolStripComboBox;
                    if (cb != null)
                    {
                        Size sz = new Size(cb.Control.Width + 20, cb.Control.Height + 20);
                        ctl = cb.Control;
                        Location = ctl.PointToScreen(new Point(-10, -10));
                        Size = sz;
                        SetContent(sz);
                    }
                    else
                    {
                        ToolStripProgressBar pg = CtlToHighlight as ToolStripProgressBar;
                        if (pg != null)
                        {
                            ctl = pg.ProgressBar;
                            Size sz = new Size(ctl.Width + 20, ctl.Height + 20);
                            Location = ctl.PointToScreen(new Point(-10, -10));
                            Size = sz;
                            SetContent(sz);
                        }
                        else
                        {
                            ToolStripButton tsb = CtlToHighlight as ToolStripButton;
                            if (tsb != null)
                            {
                                Rectangle rc = tsb.Bounds;
                                ctl = tsb.Owner;
                                Location = ctl.PointToScreen(new Point(rc.X - 10, rc.Y - 10));
                                Size sz = new Size(rc.Width + 20, rc.Height + 20);
                                Size = sz;
                                SetContent(sz);
                            }
                        }
                    }
                }
            }
            catch { }
        }
        private void BeginHighlight()
        {
            BringToFront();
            try
            {
                Control ctl = CtlToHighlight as Control;
                if (ctl != null)
                {
                    if ((Mode & HighlightMode.Flash) != 0)
                    {
                        _backColor = ctl.BackColor;
                        _foreColor = ctl.ForeColor;
                        ctl.BackColor = _foreColor;
                        ctl.ForeColor = _backColor;
                    }
                }
                else
                {
                    ToolStripComboBox cb = CtlToHighlight as ToolStripComboBox;
                    if (cb != null)
                    {
                        if ((Mode & HighlightMode.Flash) != 0)
                        {
                            _backColor = cb.BackColor;
                            _foreColor = cb.ForeColor;
                            cb.BackColor = _foreColor;
                            cb.ForeColor = _backColor;
                        }
                    }
                    else
                    {
                        ToolStripProgressBar pg = CtlToHighlight as ToolStripProgressBar;
                        if (pg != null)
                        {
                            if ((Mode & HighlightMode.Flash) != 0)
                            {
                                _backColor = pg.BackColor;
                                _foreColor = pg.ForeColor;
                                pg.BackColor = _foreColor;
                                pg.ForeColor = _backColor;
                            }
                        }
                        else
                        {
                            ToolStripButton tsb = CtlToHighlight as ToolStripButton;
                            if (tsb != null)
                            {
                                if ((Mode & HighlightMode.Flash) != 0)
                                {
                                    _backColor = tsb.BackColor;
                                    _foreColor = tsb.ForeColor;
                                    tsb.BackColor = _foreColor;
                                    tsb.ForeColor = _backColor;
                                }
                            }
                        }
                    }
                }
                if (((Mode & HighlightMode.Tooltip) != 0) && (_bubbleMessage != null))
                {
                    string spmessage = ToolTipMessage;
                    int hl = 1;
                    if (spmessage.Length > 32)
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
                    Point pc = new Point(szCircle.Width < 50 ? 0 : (Width / 2), szCircle.Height < 50 ? -(35 + hl * 15) : ((Height / 2) - (35 + hl * 15)));
                    Point pt = PointToScreen(new Point(pc.X, pc.Y));
                    int offset = 10;
                    if (pt.Y < 0)
                    {
                        pc.Y = Height;
                        Point oldPos = Cursor.Position;
                        Cursor.Position = new Point(pc.X, 0);
                        Application.DoEvents();
                        _bubbleMessage.Show(spmessage, this, pc, CloseInMs);
                        Cursor.Position = oldPos;
                    }
                    else
                    {
                        pt = PointToScreen(new Point(pc.X, pc.Y + offset));
                        Point oldPos = Cursor.Position;
                        Cursor.Position = pt;
                        Application.DoEvents();
                        _bubbleMessage.Show(spmessage, this, pc, CloseInMs);
                        Cursor.Position = oldPos;
                    }
                }
            }
            catch { }
            fTimer.Start();
        }
        private void fTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                fTimer.Stop();
                if ((Mode & HighlightMode.Flash) != 0)
                {
                    Control ctl = CtlToHighlight as Control;
                    if (ctl != null)
                    {
                        ctl.BackColor = _backColor;
                        ctl.ForeColor = _foreColor;
                    }
                    else
                    {
                        ToolStripItem tsb = CtlToHighlight as ToolStripItem;
                        if (tsb != null)
                        {
                            tsb.BackColor = _backColor;
                            tsb.ForeColor = _foreColor;
                        }
                    }
                }
            }
            finally
            {
                _working = false;
                Hide();
                BeginInvoke(new Action(() =>
                {
                    NextRequest();
                }));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if ((Mode & HighlightMode.Circle) != 0)
            {
                using (Pen pen = new Pen(ForeColor, 3))
                {
                    e.Graphics.DrawEllipse(pen, 3, 3, szCircle.Width - 6, szCircle.Height - 6);
                }
            }
            else if ((Mode & HighlightMode.Rectangle) != 0)
            {
                using (Pen pen = new Pen(ForeColor, 3))
                {
                    e.Graphics.DrawRectangle(pen, 3, 3, szCircle.Width - 6, szCircle.Height - 6);
                }
            }
        }
    }
    /// <summary>
    /// Request to highlight a control in the user interface.
    /// </summary>
    public class HighlightRequest
    {
        /// <summary>
        /// Highlight mode to use.
        /// </summary>
        public fHighlight.HighlightMode Mode { get; set; }
        /// <summary>
        /// ToolTip caption to show, in case of ToolTip mode.
        /// </summary>
        public string ToolTipCaption { get; set; }
        /// <summary>
        /// ToolTip message to show, in case of ToolTip mode.
        /// </summary>
        public string ToolTipMessage { get; set; }
        /// <summary>
        /// UI element to highlight.
        /// </summary>
        public object CtlToHighlight { get; set; }
        /// <summary>
        /// Gets or sets the highight duration, in milliseconds.
        /// </summary>
        public int CloseInMs { get; set; }
    }
}

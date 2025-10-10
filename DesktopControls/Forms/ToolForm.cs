using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Forms
{
    public partial class ToolForm : Form
    {
        private Form _host;
        private Point _initialLocation;

        public ToolForm()
        {
            InitializeComponent();
        }
        public void AttachToHost(Form host)
        {
            _host = host;
            // Close tool when host closes
            _host.FormClosed += CloseForm;

            // Hide when host is minimized; show when restored/maximized
            _host.Resize += SyncVisibility;
        }
        public void CloseForm(object sender = null, EventArgs e = null)
        {
            if (Modal)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                Close();
            }
        }
        /// <summary>
        /// Show or hide the tool form based on the host form's state.
        /// </summary>
        private void SyncVisibility(object sender, EventArgs e)
        {
            if (_host == null)
            {
                return;
            }
            if (Visible && _host.WindowState == FormWindowState.Minimized)
            {
                Close();
            }
        }
        private void ToolForm_Load(object sender, EventArgs e)
        {
            if (Icon != null)
            {
                Icon = Icon.Clone() as Icon;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if ((_host != null) && !_host.IsDisposed)
            {
                _host.Resize -= SyncVisibility;
                _host.FormClosed -= CloseForm;
            }
        }
    }
}

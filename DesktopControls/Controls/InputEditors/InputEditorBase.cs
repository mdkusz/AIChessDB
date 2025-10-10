using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Generic panel to edit UIDataSheet objects
    /// </summary>
    /// <remarks>
    /// This is the abstract base class for all Windows Desktop property editors.
    /// It is derived from ToolTipPanel, and uses the DescriptionAttribute attribute from the property or method to show a ToolTip when the mouse is over it.
    /// The header label shows the property or method DisplayNameAttribute value or the property name if there is not DisplaynameAttribute.
    /// </remarks>
    /// <seealso cref="ToolTipPanel"/>
    /// <seealso cref="IUIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public abstract class InputEditorBase : ToolTipPanel, IInputEditorBase
    {
        protected PropertyEditorInfo _pInfo;
        protected object _instance;
        protected Color _titleBC = SystemColors.ActiveCaption;
        protected Color _titleFC = SystemColors.ActiveCaptionText;

        protected InputEditorBase() : base()
        {
            AccessibleDescription = ".";
        }
        /// <summary>
        /// InputEditorBase Constructor
        /// </summary>
        /// <param name="pinfo">
        /// Information to edit the property
        /// </param>
        /// <param name="instance">
        /// Object instance to edit
        /// </param>
        /// <param name="container">
        /// Container control
        /// </param>
        public InputEditorBase(PropertyEditorInfo pinfo, object instance, Control container) : base()
        {
            AccessibleDescription = ".";
            Margin = new Padding(0);
            _pInfo = pinfo;
            _instance = instance;
            Padding = new Padding(3, 3, 3, 3);
            BackColor = SystemColors.Control;
            ForeColor = SystemColors.ControlText;
            BorderSize = pinfo.BorderSize;
            LeftBorderColor = pinfo.LeftBorderColor;
            TopBorderColor = pinfo.TopBorderColor;
            RightBorderColor = pinfo.RightBorderColor;
            BottomBorderColor = pinfo.BottomBorderColor;
            if (pinfo.BackColor.HasValue)
            {
                BackColor = Color.FromArgb(pinfo.BackColor.Value);
            }
            if (pinfo.ForeColor.HasValue)
            {
                ForeColor = Color.FromArgb(pinfo.ForeColor.Value);
            }
        }
        /// <summary>
        /// IInputEditorBase: Editor configuration
        /// </summary>
        [Browsable(false)]
        public PropertyEditorInfo EditorInfo
        {
            get
            {
                return _pInfo;
            }
        }
        /// <summary>
        /// IInputEditorBase: IUIDataSheet instance
        /// </summary>
        [Browsable(false)]
        public IUIDataSheet Instance
        {
            get
            {
                return _instance as IUIDataSheet;
            }
        }
        /// <summary>
        /// IInputEditorBase: Background color if this is a title panel
        /// </summary>
        public Color TitleBackColor
        {
            get
            {
                return _titleBC;
            }
            set
            {
                _titleBC = value;
            }
        }
        /// <summary>
        /// IInputEditorBase: Foreground color if this is a title panel
        /// </summary>
        public Color TitleForeColor
        {
            get
            {
                return _titleFC;
            }
            set
            {
                _titleFC = value;
            }
        }
        /// <summary>
        /// IInputEditorBase: Refresh the editor value and selection data when applicable
        /// </summary>
        public virtual void RefreshEditorValue() { }
        /// <summary>
        /// Update property with the current editor value when applicable
        /// </summary>
        public virtual void UpdatePropertyValue() { }
        /// <summary>
        /// Create editing controls
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected abstract void CreateControls(Control container);
        /// <summary>
        /// Resize editing controls
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected virtual void ResizeControls(Control container)
        {
            Width = container.ClientSize.Width - container.Padding.Horizontal;
            Control lbl = Controls.Find(NAME_lbTitle, false).FirstOrDefault();
            Height = container.Font.Height + Padding.Vertical + (lbl != null ? lbl.Margin.Vertical : 0);
            ResizeControl(container);
        }
        /// <summary>
        /// Add the control to edit the property value, based on the property editor type
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        /// <param name="name">
        /// Force control text if not null
        /// </param>
        protected virtual void AddControl(Control container, string text = null) { }
        /// <summary>
        /// Resize the control to edit the property value, based on the property editor type
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected virtual void ResizeControl(Control container) { }
        /// <summary>
        /// Update the control size and position
        /// </summary>
        protected virtual void UpdateControl()
        {
            Control ctl = Controls.Find(NAME_ctlEditor, false).FirstOrDefault();
            if (ctl != null)
            {
                ResizeControl(ctl, true);
            }
            else
            {
                ctl = Controls.Find(NAME_ctlEditorContainer, false).FirstOrDefault();
                if (ctl != null)
                {
                    ResizeControl(ctl, true);
                    ctl = ctl.Controls.Find(NAME_ctlEditor, false).FirstOrDefault();
                    if (ctl != null)
                    {
                        ResizeControl(ctl, false);
                    }
                }
            }
        }
        /// <summary>
        /// Resize the control to fit the panel
        /// </summary>
        /// <param name="control">
        /// Control to resize
        /// </param>
        /// <param name="topleft">
        /// Update Top and Left properties
        /// </param>
        protected virtual void ResizeControl(Control control, bool topleft)
        {
            if (topleft)
            {
                control.Top = Height - Padding.Bottom - control.Height - 1;
                control.Left = Padding.Left;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateControl();
        }
        protected override void OnParentFontChanged(EventArgs e)
        {
            base.OnParentFontChanged(e);
            Control lbl = Controls.Find(NAME_lbTitle, false).FirstOrDefault();
            if (lbl != null)
            {
                Font oldf = lbl.Font;
                lbl.Font = new Font(Parent.Font, FontStyle.Bold);
                try { oldf.Dispose(); } catch { }
            }
            ResizeControls(Parent);
        }
    }
}

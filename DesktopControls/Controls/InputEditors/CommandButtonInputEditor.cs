using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to implement command buttons for void methods without parameters
    /// </summary>
    /// <remarks>
    /// This editor uses a Button to execute the method without parameters indicated in the PropertyName property of the PropertyEditorInfo object.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not CommandButton, or the method is not found
    /// </exception>
    /// <seealso cref="MethodInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class CommandButtonInputEditor : MethodInputEditorBase
    {
        public CommandButtonInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.CommandButton)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (_method == null)
            {
                throw new ArgumentException(ERR_UnknownMethod);
            }
            CreateControls(container);
        }
        /// <summary>
        /// Create editing controls
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void CreateControls(Control container)
        {
            DescriptionAttribute description = _method.GetCustomAttribute<DescriptionAttribute>();
            Description = description?.Description;
            Width = container.ClientSize.Width - container.Padding.Horizontal;
            AddControl(container);
        }
        /// <summary>
        /// Resize editing controls
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void ResizeControls(Control container)
        {
            Width = container.ClientSize.Width - container.Padding.Horizontal;
            Height = Padding.Vertical;
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
        protected override void AddControl(Control container, string text = null)
        {
            DisplayNameAttribute displayName = _method.GetCustomAttribute<DisplayNameAttribute>();
            string dname = displayName != null ? displayName.DisplayName : _pInfo.PropertyName;
            Title = dname;
            Button btn = new Button()
            {
                AccessibleDescription = Description,
                AccessibleName = text ?? dname,
                AccessibleRole = AccessibleRole.PushButton,
                Text = dname,
                Name = NAME_ctlEditor,
                Font = container.Font,
                AutoSize = true
            };
            btn.Click += (s, e) =>
            {
                string message = _method.Invoke(_instance, null) as string;
                if (!string.IsNullOrEmpty(message))
                {
                    MessageBox.Show(this, message);
                }
            };
            Height = btn.Height + Padding.Vertical;
            Controls.Add(btn);
            ResizeControl(btn, true);
            btn.Font = null;
        }
        /// <summary>
        /// Resize the control to edit the property value, based on the property editor type
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void ResizeControl(Control container)
        {
            Control tb = Controls.Find(NAME_ctlEditor, false).FirstOrDefault();
            Height += tb.Height;
            ResizeControl(tb, true);
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
        protected override void ResizeControl(Control control, bool topleft)
        {
            using (Graphics gr = control.CreateGraphics())
            {
                int maxw = string.IsNullOrEmpty(control.Text) ? Width - Padding.Horizontal : (int)Math.Ceiling(gr.MeasureString(control.Text, control.Font).Width);
                control.Width = Math.Min(maxw, Width - Padding.Horizontal);
            }
            base.ResizeControl(control, topleft);
        }
    }
}

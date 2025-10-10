using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to edit text in a multiline control
    /// </summary>
    /// <remarks>
    /// This editor uses a multiline TextBox to edit the property value.
    /// The number of text lines is defined in the PropertyEditorInfo.MaxUnits property.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not MultiLineText, or the property is not found
    /// </exception>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class MultiLineTextInputEditor : PropertyInputEditorBase
    {
        public MultiLineTextInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.MultilineText)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (_property == null)
            {
                throw new ArgumentException(ERR_UnknownProperty);
            }
            CreateControls(container);
        }
        /// <summary>
        /// Refresh the editor value and selection data when applicable
        /// </summary>
        public override void RefreshEditorValue()
        {
            TextBox tb = Controls.Find(NAME_ctlEditor, false).FirstOrDefault() as TextBox;
            if (tb != null)
            {
                tb.Text = _property.GetValue(_instance)?.ToString() ?? "";
                ResizeControl(tb, false);
            }
        }
        /// <summary>
        /// Update property with the current editor value when applicable
        /// </summary>
        public override void UpdatePropertyValue()
        {
            TextBox tb = Controls.Find(NAME_ctlEditor, false).FirstOrDefault() as TextBox;
            if ((tb != null) && !_pInfo.ReadOnly)
            {
                _property.SetValue(_instance, tb.Text);
            }
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
            TextBox tb = new TextBox()
            {
                AccessibleDescription = Description,
                AccessibleName = text,
                AccessibleRole = AccessibleRole.Text,
                Multiline = true,
                AcceptsReturn = true,
                AcceptsTab = true,
                ScrollBars = ScrollBars.Both,
                Font = container.Font,
                WordWrap = false,
                Name = NAME_ctlEditor,
                ReadOnly = _pInfo.ReadOnly
            };
            tb.LostFocus += TextBoxLostFocus;
            if (_pInfo.InitialValue != null)
            {
                tb.Text = _pInfo.InitialValue.ToString();
            }
            else
            {
                tb.Text = _property.GetValue(_instance)?.ToString() ?? "";
            }
            int lines = _pInfo.MaxUnits;
            if (lines < 0)
            {
                if (tb.Lines != null)
                {
                    lines = 1 + tb.Lines.Length;
                }
            }
            tb.Height = SystemInformation.HorizontalScrollBarHeight + container.Font.Height * Math.Max(lines, 2);
            Height += tb.Height;
            Controls.Add(tb);
            ResizeControl(tb, true);
            tb.Font = null;
        }
        /// <summary>
        /// Resize the control to edit the property value, based on the property editor type
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void ResizeControl(Control container)
        {
            Control tb = tb = Controls.Find(NAME_ctlEditor, false).FirstOrDefault();
            int lines = _pInfo.MaxUnits;
            if (lines < 0)
            {
                if (((TextBox)tb).Lines != null)
                {
                    lines = 1 + ((TextBox)tb).Lines.Length;
                }
            }
            using (Graphics g = container.CreateGraphics())
            {
                tb.Height = SystemInformation.HorizontalScrollBarHeight + container.Font.Height * Math.Max(lines, 2);
                Height += tb.Height;
            }
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
                if (control.MaximumSize.Width > 0)
                {
                    control.Width = Math.Min(control.MaximumSize.Width, Width - Padding.Horizontal);
                }
                else
                {
                    int maxw = Width - Padding.Horizontal;
                    control.Width = Math.Min(maxw, Width - Padding.Horizontal);
                }
            }
            base.ResizeControl(control, topleft);
        }
        protected virtual void TextBoxLostFocus(object sender, EventArgs e)
        {
            UpdatePropertyValue();
        }
    }
}

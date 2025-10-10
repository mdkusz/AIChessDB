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
    /// Editor to edit properties from a list of values in a combobox
    /// </summary>
    /// <remarks>
    /// This editor uses a ComboBox with DropDownList style to select a single value for the property from a fixed list.
    /// The list of values is defined in the PropertyEditorInfo.Values property.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not FixedComboBox, or the property is not found
    /// </exception>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class FixedComboBoxInputEditor : PropertyInputEditorBase
    {
        public FixedComboBoxInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.FixedComboBox)
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
            ComboBox cb = Controls.Find(NAME_ctlEditor, false).FirstOrDefault() as ComboBox;
            if (cb != null)
            {
                UpdateCBData(cb);
                ResizeControl(cb, false);
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
            ComboBox cb = new ComboBox()
            {
                AccessibleDescription = Description,
                AccessibleName = text,
                AccessibleRole = AccessibleRole.ComboBox,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = NAME_ctlEditor,
                Font = container.Font,
                Sorted = true
            };
            cb.SelectedIndexChanged += ComboBoxChanged;
            if (_pInfo.MaxUnits > 0)
            {
                cb.MaxDropDownItems = _pInfo.MaxUnits;
            }
            UpdateCBData(cb);
            Height += cb.Height;
            Controls.Add(cb);
            ResizeControl(cb, true);
            cb.Font = null;
        }
        /// <summary>
        /// Update ComboBox data
        /// </summary>
        /// <param name="cb">
        /// ComboBox to update
        /// </param>
        private void UpdateCBData(ComboBox cb)
        {
            if (_pInfo.MaxUnits > 0)
            {
                cb.MaxDropDownItems = _pInfo.MaxUnits;
            }
            if (_pInfo.Values != null)
            {
                cb.Items.Clear();
                cb.Items.AddRange(_pInfo.Values.ToArray());
                int maxwidth = 0;
                using (Graphics gr = cb.CreateGraphics())
                {
                    foreach (object item in cb.Items)
                    {
                        maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(item.ToString(), cb.Font).Width));
                    }
                }
                cb.MaximumSize = new Size(Math.Max(maxwidth, Math.Min(100, Width / 2)) + SystemInformation.VerticalScrollBarWidth, cb.MaximumSize.Height);
            }
            object ival = _pInfo.InitialValue ?? _property.GetValue(_instance);
            if (ival != null)
            {
                if (cb.Items.Count == 0)
                {
                    cb.Items.Add(ival);
                }
                cb.SelectedItem = ival;
            }
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
                if (control.MaximumSize.Width > 0)
                {
                    control.Width = Math.Min(control.MaximumSize.Width, Width - Padding.Horizontal);
                }
                else
                {
                    ComboBox cb = control as ComboBox;
                    int maxwidth = 0;
                    foreach (object item in cb.Items)
                    {
                        maxwidth = Math.Max(maxwidth, 2 * (int)Math.Ceiling(gr.MeasureString(item.ToString(), cb.Font).Width));
                    }
                    cb.Width = Math.Min(Width - Padding.Horizontal, maxwidth > 0 ? maxwidth + SystemInformation.VerticalScrollBarWidth : (Width - Padding.Horizontal));
                }
            }
            base.ResizeControl(control, topleft);
            ToolTipHorizontalOffset = control.Right;
        }
        protected virtual void ComboBoxChanged(object sender, EventArgs e)
        {
            ComboBox cbBox = sender as ComboBox;
            if (cbBox != null)
            {
                _property.SetValue(_instance, cbBox.SelectedItem);
            }
        }
    }
}

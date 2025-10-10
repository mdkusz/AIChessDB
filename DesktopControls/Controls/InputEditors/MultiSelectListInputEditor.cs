using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to edit properties that are a list of objects with multiple selection options
    /// </summary>
    /// <remarks>
    /// This editor uses a multiselect ListBox to add elements to a list.
    /// The ListBox items show the string representation of the list elements, so they must override ToString to show a readable name for users.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not MultiSelectList, the property is not found, or the property type is not a list
    /// </exception>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class MultiSelectListInputEditor : PropertyInputEditorBase
    {
        public MultiSelectListInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.MultiSelectList)
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
            ListBox lb = Controls.Find(NAME_ctlEditor, false).FirstOrDefault() as ListBox;
            if (lb != null)
            {
                UpdateLBData(lb);
                ResizeControl(lb, false);
            }
        }
        /// <summary>
        /// Update property with the current editor value when applicable
        /// </summary>
        public override void UpdatePropertyValue()
        {
            ListBox lBox = Controls.Find(NAME_ctlEditor, false).FirstOrDefault() as ListBox;
            if (lBox != null)
            {
                IList l = _property.GetValue(_instance) as IList;
                l.Clear();
                foreach (object item in lBox.SelectedItems)
                {
                    l.Add(item);
                }
                _property.SetValue(_instance, l);
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
            ListBox lb = new ListBox()
            {
                AccessibleDescription = Description,
                AccessibleName = text,
                AccessibleRole = AccessibleRole.List,
                SelectionMode = SelectionMode.MultiExtended,
                Name = NAME_ctlEditor,
                Font = container.Font,
                Sorted = true
            };
            lb.LostFocus += ExitControl;
            UpdateLBData(lb);
            Height += lb.Height;
            Controls.Add(lb);
            ResizeControl(lb, true);
            lb.Font = null;
        }
        /// <summary>
        /// Update ListBox data
        /// </summary>
        /// <param name="lb">
        /// ListBox to update
        /// </param>
        private void UpdateLBData(ListBox lb)
        {
            if (_pInfo.MaxUnits > 0)
            {
                lb.MinimumSize = new Size(lb.MinimumSize.Width, _pInfo.MaxUnits * lb.ItemHeight);
            }
            if (_pInfo.Values != null)
            {
                lb.Items.Clear();
                lb.Items.AddRange(_pInfo.Values.ToArray());
                int maxwidth = 0;
                using (Graphics gr = lb.CreateGraphics())
                {
                    foreach (object item in lb.Items)
                    {
                        maxwidth = Math.Max(maxwidth, (int)Math.Ceiling(gr.MeasureString(item.ToString(), lb.Font).Width));
                    }
                }
                lb.MaximumSize = new Size(Math.Max(maxwidth, Math.Min(100, Width / 2)) + SystemInformation.VerticalScrollBarWidth, lb.Height);
            }
            IList value = _property.GetValue(_instance) as IList;
            if (value != null)
            {
                foreach (object item in value)
                {
                    int index = lb.Items.IndexOf(item);
                    if (index >= 0)
                    {
                        lb.SetSelected(index, true);
                    }
                }
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
                    ListBox lb = control as ListBox;
                    int maxwidth = 0;
                    foreach (object item in lb.Items)
                    {
                        maxwidth = Math.Max(maxwidth, 2 * (int)Math.Ceiling(gr.MeasureString(item.ToString(), lb.Font).Width));
                    }
                    lb.Width = Math.Min(Width - Padding.Horizontal, maxwidth > 0 ? maxwidth + SystemInformation.VerticalScrollBarWidth : (Width - Padding.Horizontal));
                }
            }
            base.ResizeControl(control, topleft);
        }
        protected virtual void ExitControl(object sender, EventArgs e)
        {
            UpdatePropertyValue();
        }
    }
}

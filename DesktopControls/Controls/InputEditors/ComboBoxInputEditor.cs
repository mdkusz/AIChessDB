using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to edit properties that are a list of objects
    /// </summary>
    /// <remarks>
    /// This editor uses a ComboBox to add elements to a list.
    /// Users can add new elements or delete existing ones.
    /// The ComboBox items show the string representation of the list elements, so they must override ToString to show a readable name for users.
    /// The Tag property of the ComboBox is used to store the current selected item. If you delete the combobox editor and press Enter, the item is removed from the list.
    /// Use Escape to cancel item modifications and unselect it.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not ComboBox, the property is not found, or the property type is not a list
    /// </exception>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class ComboBoxInputEditor : PropertyInputEditorBase
    {
        public ComboBoxInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.ComboBox)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (_property == null)
            {
                throw new ArgumentException(ERR_UnknownProperty);
            }
            if (_property.PropertyType.GetInterface(nameof(IList)) == null)
            {
                throw new ArgumentException(ERR_BadPropertyType);
            }
            CreateControls(container);
        }
        /// <summary>
        /// Add the control to edit the property value, based on the property editor type.
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
                DropDownStyle = ComboBoxStyle.DropDown,
                Name = NAME_ctlEditor,
                Font = container.Font,
                Sorted = true
            };
            cb.KeyDown += ComboBoxKeyDown;
            cb.SelectedIndexChanged += ComboBoxSelectionChanged;
            if (_pInfo.MaxUnits > 0)
            {
                cb.MaxDropDownItems = _pInfo.MaxUnits;
            }
            IList lval = _property.GetValue(_instance) as IList;
            if (lval != null)
            {
                foreach (object item in lval)
                {
                    cb.Items.Add(item);
                }
                int maxwidth = 0;
                using (Graphics gr = cb.CreateGraphics())
                {
                    foreach (object item in cb.Items)
                    {
                        maxwidth = Math.Max(maxwidth, 2 * (int)Math.Ceiling(gr.MeasureString(item.ToString(), cb.Font).Width));
                    }
                }
                cb.Width = Math.Min(Width - Padding.Horizontal, maxwidth > 0 ? maxwidth + SystemInformation.VerticalScrollBarWidth : (Width - Padding.Horizontal));
            }
            Height += cb.Height;
            Controls.Add(cb);
            ResizeControl(cb, true);
            cb.Font = null;
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
        protected virtual void ComboBoxKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ComboBox cbBox = sender as ComboBox;
                if (cbBox != null)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        IList list = _property.GetValue(_instance) as IList;
                        if (list == null)
                        {
                            // Get the property type
                            Type propertyType = _property.PropertyType;

                            // Verify whether the property is a list
                            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                // Get the list elements type
                                Type itemType = propertyType.GetGenericArguments()[0];

                                // Create a properly typed list instance
                                Type listType = typeof(List<>).MakeGenericType(itemType);
                                list = Activator.CreateInstance(listType) as IList;

                                _property.SetValue(_instance, list);
                            }
                        }
                        string newItemText = cbBox.Text;
                        object newItem = null;
                        if (!string.IsNullOrEmpty(newItemText))
                        {
                            Type targetType = _property.PropertyType.GetGenericArguments()[0];
                            newItem = Convert.ChangeType(newItemText, targetType);
                        }

                        if (cbBox.Tag != null)
                        {
                            if (newItem == null)
                            {
                                cbBox.Items.Remove(cbBox.Tag);
                                list.Remove(cbBox.Tag);
                            }
                            else if (cbBox.Items.Contains(cbBox.Tag))
                            {
                                cbBox.Items[cbBox.Items.IndexOf(cbBox.Tag)] = newItem;
                                list[list.IndexOf(cbBox.Tag)] = newItem;
                            }
                        }
                        else if ((newItem != null) && !cbBox.Items.Contains(newItem))
                        {
                            cbBox.Items.Add(newItem);
                            list.Add(newItem);
                        }
                        cbBox.Tag = null;
                        cbBox.Text = "";
                        cbBox.SelectedItem = null;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    else if (e.KeyCode == Keys.Escape)
                    {
                        cbBox.Tag = null;
                        cbBox.Text = "";
                        cbBox.SelectedItem = null;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected virtual void ComboBoxSelectionChanged(object sender, EventArgs e)
        {
            ComboBox cbBox = sender as ComboBox;
            if (cbBox != null)
            {
                object cobj = cbBox.SelectedItem;
                if (cbBox.Tag != null)
                {
                    ComboBoxKeyDown(sender, new KeyEventArgs(Keys.Enter));
                    if (cobj != null)
                    {
                        cbBox.Text = cobj.ToString();
                    }
                }
                cbBox.Tag = cobj;
            }
        }
    }
}

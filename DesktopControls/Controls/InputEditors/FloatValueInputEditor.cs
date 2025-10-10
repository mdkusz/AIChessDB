using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to edit floating point values with a numeric updown control
    /// </summary>
    /// <remarks>
    /// This editor uses a numeric updown control to edit the property value. The maximum and minimum values are defined in the PropertyEditorInfo.Values property.
    /// If the property is nullable, there is an additional CheckBox to select whether or not to set the property value.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not FloatValue, the property is not found, or the property is not a numeric floating or fixed point value
    /// </exception>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class FloatValueInputEditor : PropertyInputEditorBase
    {
        private CheckBox _nullCB = null;
        public FloatValueInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.FloatValue)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (_property == null)
            {
                throw new ArgumentException(ERR_UnknownProperty);
            }
            if (!new List<Type> { typeof(decimal), typeof(float), typeof(double),
                typeof(decimal?), typeof(float?), typeof(double?) }.Contains(_property.PropertyType))
            {
                throw new ArgumentException(ERR_BadPropertyType);
            }
            CreateControls(container);
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
            Control cedit = null;
            NumericUpDown nup = new NumericUpDown()
            {
                AccessibleDescription = Description,
                AccessibleName = text,
                AccessibleRole = AccessibleRole.SpinButton,
                DecimalPlaces = 1,
                Font = container.Font,
                Name = NAME_ctlEditor
            };
            if (_pInfo.Values != null)
            {
                nup.Minimum = Convert.ToDecimal(_pInfo.Values[0]);
                nup.Maximum = Convert.ToDecimal(_pInfo.Values[1]);
                nup.Increment = Math.Max((nup.Maximum - nup.Minimum) / 10M, 0.1M);
            }
            nup.Value = Convert.ToDecimal(_pInfo.InitialValue ?? _property.GetValue(_instance) ?? nup.Minimum);
            nup.ValueChanged += NumericUDChanged;
            using (Graphics g = nup.CreateGraphics())
            {
                nup.Width = (int)Math.Ceiling(g.MeasureString(nup.Value.ToString("0." + new string('0', nup.DecimalPlaces)), container.Font).Width) + SystemInformation.VerticalScrollBarWidth + 8;
            }
            if (_property.PropertyType.IsGenericType && _property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Panel p = new Panel()
                {
                    Height = nup.Height,
                    Width = Width - Padding.Horizontal,
                    Name = NAME_ctlEditorContainer,
                };
                _nullCB = new CheckBox()
                {
                    AccessibleDescription = DESC_NullCheckBox,
                    AccessibleName = NAME_NullCheckBox,
                    AccessibleRole = AccessibleRole.CheckButton,
                    Text = "",
                    Name = NAME_cbNulls,
                    AutoSize = true,
                    Font = container.Font,
                    Left = 0,
                    Checked = _pInfo.InitialValue != null || _property.GetValue(_instance) != null
                };
                _nullCB.Top = (p.Height - (_nullCB.Height / 2)) / 2;
                _nullCB.Font = null;
                _nullCB.CheckedChanged += NullCBChanged;
                p.Controls.Add(_nullCB);
                nup.Left = _nullCB.Width + 4;
                nup.Top = 0;
                p.Controls.Add(nup);
                cedit = p;
            }
            else
            {
                _nullCB = null;
                nup.Name = NAME_ctlEditor;
                cedit = nup;
            }
            Height += nup.Height;
            Controls.Add(cedit);
            ResizeControl(cedit, true);
            nup.Font = null;
        }
        /// <summary>
        /// Resize the control to edit the property value, based on the property editor type
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void ResizeControl(Control container)
        {
            Control cedit = null;
            NumericUpDown nup = Controls.Find(NAME_ctlEditor, false).FirstOrDefault() as NumericUpDown;
            if (nup == null)
            {
                Panel p = Controls.Find(NAME_ctlEditorContainer, false).FirstOrDefault() as Panel;
                nup = p.Controls.Find(NAME_ctlEditor, false).FirstOrDefault() as NumericUpDown;
                CheckBox cbx = p.Controls.Find(NAME_cbNulls, false).FirstOrDefault() as CheckBox;
                p.Height = nup.Height;
                cbx.Top = (p.Height - (cbx.Height / 2)) / 2;
                nup.Left = cbx.Width + 4;
                cedit = p;
            }
            else
            {
                cedit = nup;
            }
            using (Graphics g = nup.CreateGraphics())
            {
                nup.Width = (int)Math.Ceiling(g.MeasureString(nup.Value.ToString("0." + new string('0', nup.DecimalPlaces)), container.Font).Width) + SystemInformation.VerticalScrollBarWidth + 8;
            }
            Height += nup.Height;
            ResizeControl(cedit, true);
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
                    if (control is Panel)
                    {
                        control.Width = Width - Padding.Horizontal;
                    }
                    else
                    {
                        NumericUpDown nup = (NumericUpDown)control;
                        int maxw = (int)Math.Ceiling(gr.MeasureString(nup.Value.ToString("0." + new string('0', nup.DecimalPlaces)) + SystemInformation.VerticalScrollBarWidth + 8, control.Font).Width);
                        control.Width = Math.Min(maxw, Width - Padding.Horizontal);
                    }
                }
            }
            base.ResizeControl(control, topleft);
        }
        protected virtual void NumericUDChanged(object sender, EventArgs e)
        {
            NumericUpDown nup = sender as NumericUpDown;
            if ((nup != null) && ((_nullCB == null) || (_nullCB.CheckState == CheckState.Checked)))
            {
                Type targetType = _property.PropertyType;

                // Verify Nullable type
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // Get the underlying type
                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                // Convert the value to the target type
                object convertedValue = Convert.ChangeType(nup.Value, targetType);
                _property.SetValue(_instance, convertedValue);
            }
        }
        protected void NullCBChanged(object sender, EventArgs e)
        {
            CheckBox cbx = sender as CheckBox;
            if (cbx != null)
            {
                if (cbx.CheckState != CheckState.Checked)
                {
                    _property.SetValue(_instance, null);
                }
            }
        }
    }
}

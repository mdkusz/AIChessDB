using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to edit boolean properties
    /// </summary>
    /// <remarks>
    /// This editor uses a CheckBox control to edit boolean properties.
    /// If the property is nullable, the CheckBox control is set to ThreeState mode.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not BoolValue, the property is not found, or the property type is not bool or bool?
    /// </exception>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class BoolValueInputEditor : PropertyInputEditorBase
    {
        public BoolValueInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.BoolValue)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (_property == null)
            {
                throw new ArgumentException(ERR_UnknownProperty);
            }
            if (!new List<Type> { typeof(bool), typeof(bool?) }.Contains(_property.PropertyType))
            {
                throw new ArgumentException(ERR_BadPropertyType);
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
            DisplayNameAttribute displayName = _property.GetCustomAttribute<DisplayNameAttribute>();
            string pname = displayName != null ? displayName.DisplayName : _pInfo.PropertyName;
            DescriptionAttribute description = _property.GetCustomAttribute<DescriptionAttribute>();
            Description = description?.Description;
            Title = pname;
            Width = container.ClientSize.Width - container.Padding.Horizontal;
            Height = container.Font.Height + Padding.Vertical;
            AddControl(container, pname);
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
            CheckBox chb = new CheckBox()
            {
                AccessibleDescription = Description,
                AccessibleName = text,
                AccessibleRole = AccessibleRole.CheckButton,
                Text = text ?? _pInfo.PropertyName,
                Name = NAME_ctlEditor,
                Font = container.Font,
                ThreeState = Nullable.GetUnderlyingType(_property.PropertyType) != null,
                AutoSize = true
            };
            bool? pval = (bool?)(_pInfo.InitialValue ?? _property.GetValue(_instance));
            if (pval.HasValue)
            {
                chb.Checked = pval.Value;
            }
            else
            {
                chb.CheckState = CheckState.Indeterminate;
            }
            chb.CheckedChanged += CheckBoxChanged;
            Height = Math.Max(Height, chb.Height + Padding.Vertical);
            Controls.Add(chb);
            ResizeControl(chb, true);
            chb.Font = null;
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
        protected virtual void CheckBoxChanged(object sender, EventArgs e)
        {
            CheckBox cbBox = sender as CheckBox;
            if (cbBox != null)
            {
                if (cbBox.CheckState == CheckState.Indeterminate)
                {
                    _property.SetValue(_instance, null);
                }
                else
                {
                    _property.SetValue(_instance, cbBox.Checked);
                }
            }
        }
    }
}

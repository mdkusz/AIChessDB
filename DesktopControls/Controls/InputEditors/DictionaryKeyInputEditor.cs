using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to edit dictionary keys
    /// </summary>
    /// <example>
    /// The class FunctionParameters in the OpenAIAPI project has a property of type Dictionary<string, ParameterItem>: ParamProperties
    /// </example>
    /// <remarks>
    /// The property assigned to this editor must be of type IDictionary. This editor is assigned to one key in the dictionary.
    /// The dictionary Keys must be of type string.
    /// The properties of the dictionary values have a PropertyName composed by the dictionary property name, the key value, and the property name of the value object.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not SingleLineText, the property is not found, or the property is not IDictionary
    /// </exception>
    /// <seealso cref="FunctionParameters.ParamProperties"/>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class DictionaryKeyInputEditor : PropertyInputEditorBase
    {
        public DictionaryKeyInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.SingleLineText)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (_property == null)
            {
                throw new ArgumentException(ERR_UnknownProperty);
            }
            if (_property.PropertyType.GetInterface(nameof(IDictionary)) == null)
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
            TextBox tb = new TextBox()
            {
                AccessibleDescription = Description,
                AccessibleName = text,
                AccessibleRole = AccessibleRole.Text,
                Name = NAME_ctlEditor,
                Font = container.Font,
                ReadOnly = _pInfo.ReadOnly
            };
            tb.KeyDown += DictionaryKeyChanged;
            if (_pInfo.InitialValue != null)
            {
                tb.Text = _pInfo.InitialValue.ToString();
            }
            else
            {
                tb.Text = _property.GetValue(_instance)?.ToString() ?? "";
            }
            Height += tb.Height;
            Controls.Add(tb);
            ResizeControl(tb, true);
            tb.Font = null;
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
                    int maxw = string.IsNullOrEmpty(control.Text) ? Width - Padding.Horizontal : (int)Math.Ceiling(gr.MeasureString(control.Text, control.Font).Width);
                    control.Width = Math.Min(maxw, Width - Padding.Horizontal);
                }
            }
            base.ResizeControl(control, topleft);
        }

        protected virtual void DictionaryKeyChanged(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox txtBox = sender as TextBox;
                if (txtBox != null)
                {
                    string newKey = txtBox.Text;
                    IDictionary dict = _property.GetValue(_instance) as IDictionary;
                    object value = dict[_pInfo.InitialValue];
                    dict.Remove(_pInfo.InitialValue);
                    if (string.IsNullOrEmpty(newKey))
                    {
                        ((UIDataSheet)_instance).RemoveDictionaryEntryProperties(_property.Name, _pInfo.InitialValue.ToString());
                    }
                    else
                    {
                        ((UIDataSheet)_instance).ChangeDictionaryEntryPropertyKey(_property.Name, _pInfo.InitialValue.ToString(), newKey);
                        dict[newKey] = value;
                        _pInfo.InitialValue = newKey;
                    }
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}

using DesktopControls.Controls.PropertyTable.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Etiqueta que actualiza el valor de la propiedad al hacer clic sobre ella para poder generar eventos / 
    /// Label that updates the property value when clicked on to allow generate events
    /// </summary>
    public class InteractiveLabelPropertyEditor : PropertyEditorBase
    {
        protected Label _editor = null;
        protected Color _txtForeColor;
        public InteractiveLabelPropertyEditor() : base()
        {
        }
        /// <summary>
        /// Obtener el valor de la propiedad / 
        /// Get property value
        /// </summary>
        protected override void GetValue()
        {
            base.GetValue();
            if ((_property != null) && (_instance != null))
            {
                object pvalue;
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                {
                    pvalue = vmgr.GetValue(_property.Name, ValueIndex);
                }
                else
                {
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    pvalue = Property.GetValue(_instance, index);
                }
                _editor.Text = pvalue == null ? "" : pvalue.ToString();
                _dirty = false;
            }
        }
        /// <summary>
        /// Establecer el valor de la propiedad / 
        /// Set property value
        /// </summary>
        protected override void SetValue()
        {
            if (_dirty)
            {
                base.SetValue();
                if ((_property != null) && _property.CanWrite && (_instance != null))
                {
                    IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                    if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                    {
                        object value = vmgr.GetValue(_property.Name, ValueIndex);
                        vmgr.SetValue(_property.Name, value, ValueIndex);
                        OnPropertyChanged(value);
                    }
                    else
                    {
                        object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                        object value = Property.GetValue(_instance, index);
                        Property.SetValue(_instance, value, index);
                        OnPropertyChanged(value);
                    }
                }
                _dirty = false;
            }
        }
        /// <summary>
        /// Configuración de los controles de la interfaz de usuario / 
        /// User interface controls configuration
        /// </summary>
        protected override void ConfigureUI()
        {
            base.ConfigureUI();
            ClearControls();
            if (_property != null)
            {
                int maxheight = 0;
                IPropertyConfigurationProvider cfgprovider = _instance as IPropertyConfigurationProvider;
                _editor = new Label()
                {
                    Name = "lText",
                    Left = Padding.Left,
                    Top = Padding.Top,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    AutoSize = true
                };
                _editor.Enter += Editor_Enter;
                _editor.Leave += Editor_Leave;
                _editor.Click += Editor_Click;
                AddControl(_editor);
                maxheight = _editor.Height;
                SetHeight(maxheight);
            }
        }
        protected void Editor_Enter(object sender, EventArgs e)
        {
            OnEnter(e);
        }
        protected void Editor_Leave(object sender, EventArgs e)
        {
            OnLeave(e);
        }
        protected void Editor_Click(object sender, EventArgs e)
        {
            _dirty = true;
            SetValue();
        }
    }
}

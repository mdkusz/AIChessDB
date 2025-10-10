using DesktopControls.Controls.PropertyTable.Attributes;
using DesktopControls.Controls.PropertyTable.Interfaces;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedades con un control check box / 
    /// Check box property editor
    /// </summary>
    public class CheckPropertyEditor : PropertyEditorBase
    {
        /// <summary>
        /// Control de edición / 
        /// Editor control
        /// </summary>
        protected CheckBox _editor;
        public CheckPropertyEditor() : base()
        {
        }
        public override Color CaptionColor
        {
            get
            {
                return base.CaptionColor;
            }
            set
            {
                if (_title != null)
                {
                    _title.ForeColor = value;
                }
                else
                {
                    ForeColor = value;
                }
            }
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
                    // Obtener el valor a través del objeto instancia
                    // Get the value from the instance
                    pvalue = vmgr.GetValue(_property.Name, ValueIndex);
                }
                else
                {
                    // Obtener el valor a través del descriptor de la propiedad
                    // Get the value from the property descriptor
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    pvalue = Property.GetValue(_instance, index);
                }
                _editor.CheckState = pvalue == null ? CheckState.Indeterminate :
                    (Convert.ToBoolean(pvalue) ? CheckState.Checked : CheckState.Unchecked);
            }
        }
        /// <summary>
        /// Establecer el valor de la propiedad / 
        /// Set the property value
        /// </summary>
        protected override void SetValue()
        {
            base.SetValue();
            if ((_property != null) && _property.CanWrite && (_instance != null))
            {
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                {
                    // Obtener el valor a través del objeto instancia
                    // Get the value from the instance
                    object oldval = vmgr.GetValue(_property.Name, ValueIndex);
                    CheckState cvalue = oldval == null ? CheckState.Indeterminate :
                        (Convert.ToBoolean(oldval) ? CheckState.Checked : CheckState.Unchecked);
                    if (cvalue != _editor.CheckState)
                    {
                        // Establecer el valor a través del objeto instancia
                        // Set the value using the instance
                        vmgr.SetValue(_property.Name, _editor.Checked, ValueIndex);
                        OnPropertyChanged(oldval);
                    }
                }
                else
                {
                    // Obtener el valor a través del descriptor de la propiedad
                    // Get the value from the property descriptor
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    object oldval = Property.GetValue(_instance, index);
                    CheckState cvalue = oldval == null ? CheckState.Indeterminate :
                        (Convert.ToBoolean(oldval) ? CheckState.Checked : CheckState.Unchecked);
                    if (cvalue != _editor.CheckState)
                    {
                        // Establecer el valor a través del descriptor de la propiedad
                        // Set the value using the property descriptor
                        Property.SetValue(_instance, _editor.Checked, index);
                        OnPropertyChanged(oldval);
                    }
                }
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
                IPropertyConfigurationProvider cfgprovider = _instance as IPropertyConfigurationProvider;
                ValueEditorCaptionAttribute vc = _property.GetCustomAttribute<ValueEditorCaptionAttribute>();
                string caption = "";
                if (cfgprovider != null)
                {
                    // Obtener el texto del control a través de la instancia
                    // Get the control caption from the instance
                    caption = cfgprovider.Caption(_property.Name, ValueIndex);
                }
                if (string.IsNullOrEmpty(caption) && (vc != null))
                {
                    // Obtener el texto del control a través del atributo
                    // Get the control caption from the attribute
                    caption = vc.Caption;
                }
                _editor = new CheckBox()
                {
                    Name = "cbValue",
                    Top = 0,
                    Left = Padding.Left,
                    AutoSize = true,
                    Enabled = _property.CanWrite,
                    Text = string.IsNullOrEmpty(caption) ? PanelBaseText : caption
                };
                if (string.IsNullOrEmpty(caption))
                {
                    Text = null;
                }
                AddControl(_editor);
                int maxheight = _editor.Height;
                // Establecer la altura total del editor
                // Set editor total height
                SetHeight(maxheight);
            }
        }
    }
}

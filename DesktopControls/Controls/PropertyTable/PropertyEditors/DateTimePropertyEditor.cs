using DesktopControls.Controls.PropertyTable.Interfaces;
using DesktopControls.Controls.PropertyTable.PropertyEditors;
using System;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable
{
    /// <summary>
    /// Editor de propiedades de fecha y hora / 
    /// Date and time property editor
    /// </summary>
    public class DateTimePropertyEditor : PropertyEditorBase
    {
        protected DateAndTimePicker _editor = null;
        /// <summary>
        /// Las propiedades de solo lectura se muestran como texto / 
        /// Read only properties are shown as text
        /// </summary>
        protected Label _roLabel = null;
        public DateTimePropertyEditor()
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
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                {
                    // Obtener el valor a través del objeto instancia
                    // Get the value from the instance
                    _editor.NullableDateAndTime = (DateTime?)vmgr.GetValue(_property.Name, ValueIndex);
                }
                else
                {
                    // Obtener el valor a través del descriptor de la propiedad
                    // Get the value from the property descriptor
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    if (_property.CanWrite)
                    {
                        _editor.NullableDateAndTime = (DateTime?)Property.GetValue(_instance, index);
                    }
                    else
                    {
                        DateTime? dt = (DateTime?)Property.GetValue(_instance, index);
                        if (dt != null)
                        {
                            _roLabel.Text = dt.Value.ToShortDateString() + " " + dt.Value.ToShortTimeString();
                        }
                        else
                        {
                            _roLabel.Text = "null";
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Establecer el valor de la propiedad / 
        /// Set the property value
        /// </summary>
        protected override void SetValue()
        {
            base.SetValue();
            if ((_property != null) && (_instance != null) && _property.CanWrite)
            {
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                {
                    // Obtener el valor a través del objeto instancia
                    // Get the value from the instance
                    DateTime? cvalue = (DateTime?)vmgr.GetValue(_property.Name, ValueIndex);
                    if (((cvalue != null) &&
                        (_editor.NullableDateAndTime != null) &&
                        (cvalue.Value.CompareTo(_editor.DateAndTime)) != 0) ||
                        (cvalue != _editor.NullableDateAndTime))
                    {
                        // Establecer el valor a través del objeto instancia
                        // Set the value using the instance
                        vmgr.SetValue(_property.Name, _editor.NullableDateAndTime, ValueIndex);
                        OnPropertyChanged(cvalue);
                    }
                }
                else
                {
                    // Obtener el valor a través del descriptor de la propiedad
                    // Get the value from the property descriptor
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    DateTime? cvalue = (DateTime?)Property.GetValue(_instance, index);
                    if (((cvalue != null) &&
                        (_editor.NullableDateAndTime != null) &&
                        (cvalue.Value.CompareTo(_editor.DateAndTime)) != 0) ||
                        (cvalue != _editor.NullableDateAndTime))
                    {
                        // Establecer el valor a través del descriptor de la propiedad
                        // Set the value using the property descriptor
                        Property.SetValue(_instance, _editor.NullableDateAndTime, index);
                        OnPropertyChanged(cvalue);
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
                int maxheight;
                if (_property.CanWrite)
                {
                    _editor = new DateAndTimePicker()
                    {
                        Name = "dtPicker",
                        Left = Padding.Left,
                        Margin = new Padding(0),
                        Padding = new Padding(0)
                    };
                    maxheight = _editor.Height;
                    AddControl(_editor);
                }
                else
                {
                    _roLabel = new Label()
                    {
                        Name = "lDate",
                        Left = Padding.Left,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        AutoSize = true
                    };
                    maxheight = _roLabel.Height;
                    AddControl(_roLabel);
                }
                // Establecer la altura total del editor
                // Set editor total height
                SetHeight(maxheight);
            }
        }
    }
}

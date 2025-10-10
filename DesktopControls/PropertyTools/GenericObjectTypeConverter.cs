using System;
using System.ComponentModel;
using System.Globalization;

namespace DesktopControls.PropertyTools
{
    /// <summary>
    /// Conversor de objetos que sobrecargan ToString a string
    /// Converter from objects overriding ToString to string
    /// </summary>
    public class GenericObjectTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
            Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                // Devolver el valor actual de la propiedad. No se realiza conversión
                // Get current property value. No conversion performed
                return context.PropertyDescriptor.GetValue(context.Instance);
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return "";
                }
                // Devolver el nombre del objeto
                // Return the object name
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

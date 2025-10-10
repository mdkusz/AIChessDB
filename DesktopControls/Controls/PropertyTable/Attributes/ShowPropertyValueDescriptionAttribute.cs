using System;

namespace DesktopControls.Controls.PropertyTable.Attributes
{
    public class ShowPropertyValueDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Atributo para mostrar la descripción del valor de una propiedad en el panel de información
        /// Attribute to show the property value description in the information panel
        /// </summary>
        /// <param name="show"></param>
        public ShowPropertyValueDescriptionAttribute(bool show)
        {
            ShowPropertyValueDescription = show;
        }
        public bool ShowPropertyValueDescription { get; set; }
    }
}

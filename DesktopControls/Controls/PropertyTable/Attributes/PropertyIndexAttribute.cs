using System;

namespace DesktopControls.Controls.PropertyTable.Attributes
{
    /// <summary>
    /// Atributo para ordenar las propiedades dentro de un bloque
    /// Attribute to order properties in a block
    /// </summary>
    public class PropertyIndexAttribute : Attribute
    {
        public PropertyIndexAttribute(int index)
        {
            Index = index;
        }
        /// <summary>
        /// Índice de la propiedad
        /// Property index
        /// </summary>
        public int Index { get; private set; }
    }
}

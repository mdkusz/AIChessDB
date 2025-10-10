using System;
using System.Drawing;

namespace DesktopControls.Controls.PropertyTable.Attributes
{
    /// <summary>
    /// Atributo para configurar un bloque de editores de propiedades
    /// Attribute to configure a property editor block
    /// </summary>
    public class PropertyBlockAttribute : Attribute
    {
        public PropertyBlockAttribute(string category, int column, int row, ContentAlignment headerAlign)
        {
            Category = category;
            Column = column;
            Row = row;
            HeaderAlign = headerAlign;
        }
        /// <summary>
        /// Categoría de las propiedades del bloque
        /// Block properties category
        /// </summary>
        public virtual string Category { get; set; }
        /// <summary>
        /// Índice del bloque 
        /// Block index
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// Ancho del bloque
        /// Block width
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Posición del texto en la cabecera del bloque
        /// Block header text alignment
        /// </summary>
        public ContentAlignment HeaderAlign { get; set; }
    }
}

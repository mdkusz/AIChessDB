using System;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.Attributes
{
    /// <summary>
    /// Atributo para establecer el caso de los caracteres en un editor de texto
    /// Attribute to set the character casing in a text editor
    /// </summary>
    public class EditorCharCaseAttribute : Attribute
    {
        public EditorCharCaseAttribute(CharacterCasing casing)
        {
            Case = casing;
        }
        public CharacterCasing Case { get; set; }
    }
}

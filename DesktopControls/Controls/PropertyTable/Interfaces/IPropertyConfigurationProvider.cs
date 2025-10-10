using DesktopControls.Controls.PropertyTable.PropertyEditors;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.Interfaces
{
    /// <summary>
    /// Configuración dinámica de los editores de propiedades / 
    /// Property editors dynamic configuration
    /// </summary>
    public interface IPropertyConfigurationProvider
    {
        /// <summary>
        /// La propiedad se puede mostrar / 
        /// The propery can be shown
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// true para mostrar la propiedad / 
        /// true to show the property
        /// </returns>
        bool? Browsable(string name, int index = -1);
        /// <summary>
        /// El valor de la propiedad se puede cambiar / 
        /// The propery value can be changed
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// true para editar la propiedad / 
        /// true to allow editing the property
        /// </returns>
        bool? ReadOnly(string name, int index = -1);
        /// <summary>
        /// Nombre de la porpiedad en la interfaz de usuario / 
        /// User interface property name
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Nombre formateado de la propiedad / 
        /// Formatted property name
        /// </returns>
        string DisplayName(string name, int index = -1);
        /// <summary>
        /// Nombre genérico del valor de la porpiedad / 
        /// Property value generic name
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Nombre formateado del valor / 
        /// Formatted value name
        /// </returns>
        string Caption(string name, int index = -1);
        /// <summary>
        /// Descripción de la porpiedad en la interfaz de usuario / 
        /// User interface property description
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Descripción de la propiedad / 
        /// Property description
        /// </returns>
        string Description(string name, int index = -1);
        /// <summary>
        /// Editor de la propiedad / 
        /// Property editor
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="font">
        /// Tipo de letra del editor / 
        /// Editor font type
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Objeto derivado de PropertyEditorBase / 
        /// PropertyEditorBase derived object
        /// </returns>
        PropertyEditorBase Editor(string name, Font font, int index = -1);
        /// <summary>
        /// El valor de la propiedad es un texto / 
        /// The property value is a string
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// true si el valor es un texto / 
        /// true if the value is a text
        /// </returns>
        bool? IsText(string name, int index = -1);
        /// <summary>
        /// Forzar edición en mayúsculas o minúsculas / 
        /// Force character casing
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="ccdefault">
        /// Valor por defecto del caso / 
        /// Default character casing        
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Configuración del control de texto / 
        /// Text editor character casing
        /// </returns>        
        CharacterCasing CharCase(string name, CharacterCasing ccdefault = CharacterCasing.Normal, int index = -1);
        /// <summary>
        /// Restricción de longitud de caracteres / 
        /// Character length restriction
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Máxima longitud o 0 / 
        /// Max length or 0
        /// </returns>
        int? MaxLength(string name, int index = -1);
        /// <summary>
        /// El valor de la propiedad es un número / 
        /// The property value is a number
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// true si el valor es númerico / 
        /// true if the value is numeric
        /// </returns>
        bool? IsNumeric(string name, int index = -1);
        /// <summary>
        /// Valor numérico mínimo de la propiedad / 
        /// Numeric property min value
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Valor mínimo permitido / 
        /// Min value allowed
        /// </returns>
        double? MinValue(string name, int index = -1);
        /// <summary>
        /// Valor numérico máximo de la propiedad / 
        /// Numeric property max value
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento para propiedades de colección / 
        /// Collection properties element index
        /// </param>
        /// <returns>
        /// Valor máimo permitido / 
        /// Max value allowed
        /// </returns>
        double? MaxValue(string name, int index = -1);
    }
}

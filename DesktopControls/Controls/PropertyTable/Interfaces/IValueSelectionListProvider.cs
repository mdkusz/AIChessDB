using GlobalCommonEntities.DependencyInjection;
using System.Collections.Generic;

namespace DesktopControls.Controls.PropertyTable.Interfaces
{
    /// <summary>
    /// Proporcionar listas de posibles valores para una propiedad / 
    /// Provide lists of possible values for a property
    /// </summary>
    public interface IValueSelectionListProvider
    {
        /// <summary>
        /// Lista de valores permitidos para una propiedad / 
        /// List of allowed values for a property
        /// </summary>
        /// <param name="property">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <returns>
        /// Enumeración de valores en objetos ObjectWrapper / 
        /// ObjectWrapper values enumeration
        /// </returns>
        IEnumerable<ObjectWrapper> GetAllowedValues(string property);
        /// <summary>
        /// Actualizar el valor de uno de los elementos de la lista / 
        /// Update the value of one list element
        /// </summary>
        /// <param name="property">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <param name="item">
        /// Elemento a actualizar / 
        /// Updated element
        /// </param>
        void SetItemValue(string property, ObjectWrapper item);
        /// <summary>
        /// Encapsula un objeto en un ObjectWrapper / 
        /// Embed an object in an ObcectWrapper
        /// </summary>
        /// <param name="value">
        /// Objeto a encapsular / 
        /// Embedded object
        /// </param>
        /// <param name="property">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        /// <returns>
        /// ObjectWrapper configurado / 
        /// Configured ObjectWrapper 
        /// </returns>
        ObjectWrapper WrapObject(object value, string property);
    }
}

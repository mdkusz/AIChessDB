namespace DesktopControls.Controls.PropertyTable.Interfaces
{
    /// <summary>
    /// Obtener y actualizar valores de propiedades indexadas no homogéneas
    /// Get and set non-homogeneous indexed property values
    /// </summary>
    public interface IndexedPropertyValueManager
    {
        /// <summary>
        /// Indica si la colección ha cambiado
        /// Collection has changed
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad
        /// Property name
        /// </param>
        /// <returns>
        /// true si la colección ha cambiado
        /// true if collection has changed
        /// </returns>
        bool CollectionChanged(string name);
        /// <summary>
        /// Número de valores de una propiedad
        /// Property values count
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad
        /// Property name
        /// </param>
        /// <returns>
        /// Número de valores de la colección
        /// Collection value number
        /// </returns>
        int ValueCount(string name);
        /// <summary>
        /// Preguntar si una propiedad se va a gestionar por medio de este interfaz
        /// Ask whether this interface manages a property
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad
        /// Property name
        /// </param>
        /// <returns>
        /// true si el interfaz gestiona la propiedad
        /// true when this interface manages the property
        /// </returns>
        bool Managed(string name);
        /// <summary>
        /// Obtener el valor de un elemento de la propiedad
        /// Get property element value
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad
        /// Property name
        /// </param>
        /// <param name="index">
        /// Índice del elemento
        /// Element index
        /// </param>
        /// <returns>
        /// Valor del elemento
        /// Element value
        /// </returns>
        object GetValue(string name, int index);
        /// <summary>
        /// Establecer el valor de un elemento de la propiedad
        /// Set property element value
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad
        /// Property name
        /// </param>
        /// <param name="value">
        /// Valor del elemento
        /// Element value
        /// </param>
        /// <param name="index">
        /// Índice del elemento
        /// Element index
        /// </param>
        void SetValue(string name, object value, int index);
    }
}

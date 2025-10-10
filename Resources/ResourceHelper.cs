using System;
using GlobalCommonEntities.DependencyInjection;
using static Resources.Properties.Resources;

namespace Resources
{
    /// <summary>
    /// Clase auxiliar para tratar rescursos / 
    /// Resource management auxiliary class
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Obtener un nombre traducido para un elemento de enumeración / 
        /// Get a translated name for an enum element
        /// </summary>
        /// <param name="etype">
        /// Tipo de la enumeración / 
        /// Enum data type
        /// </param>
        /// <param name="value">
        /// Elemento de la enumeración / 
        /// Enum member
        /// </param>
        /// <param name="repository">
        /// Tipo del contenedro de recursos / 
        /// Resource container type
        /// </param>
        /// <returns>
        /// Texto que reprenta al miembro de la enumeración en la interfaz de usuario / 
        /// User interface enum member name
        /// </returns>
        public static string TranslateEnumMember(Type etype, object value, ResourcesRepository repository = null)
        {
            if (repository == null)
            {
                return Properties.UIResources.ResourceManager.GetString(PRE_ENUM + string.Join(".", etype.Name, Enum.GetName(etype, value)));
            }
            return repository.GetString(PRE_ENUM + string.Join(".", etype.Name, Enum.GetName(etype, value)));
        }
    }
}

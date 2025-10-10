using GlobalCommonEntities.DependencyInjection;
using System.Collections.Generic;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Class or interface dependencies container
    /// </summary>
    public interface IDependencyProvider
    {
        /// <summary>
        /// Central repository of embedded resources
        /// </summary>
        ResourcesRepository AllResources { get; set; }
        /// <summary>
        /// Check if a given class or interface is supported
        /// </summary>
        /// <param type="services">
        /// Type names separated by semicolon
        /// </param>
        /// <returns>
        /// True if can instantiante objets of at least one of the given types
        /// </returns>
        bool QueryClassOrInterface(string services);
        /// <summary>
        /// Get all the instances of all the rquested types or interfaces 
        /// </summary>
        /// <param type="services">
        /// Semicolon separated type type list
        /// </param>
        /// <param type="filter">
        /// Object containing an application-defined filter to select elements
        /// </param>
        /// <returns>
        /// Enumeration of objects implementing the types as IUIIdentifier objects
        /// </returns>
        IEnumerable<IUIIdentifier> GetObjects(string services, object filter = null);
    }
}

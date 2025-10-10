using GlobalCommonEntities.Interfaces;
using System.Collections.Generic;

namespace GlobalCommonEntities.DependencyInjection
{
    /// <summary>
    /// Dependency provider container
    /// </summary>
    public class DependencyRepository : IDependencyProvider
    {
        protected List<IDependencyProvider> _providers = new List<IDependencyProvider>();
        public DependencyRepository()
        {
        }
        /// <summary>
        /// Add a new dependency provider
        /// </summary>
        /// <param name="provider">
        /// Dependency provider
        /// </param>
        public void AddProvider(IDependencyProvider provider)
        {
            if (!_providers.Contains(provider))
            {
                if ((AllResources != null) && (provider.AllResources == null))
                {
                    provider.AllResources = AllResources;
                }
                _providers.Add(provider);
            }
        }
        /// <summary>
        /// Remove a dependency provider
        /// </summary>
        /// <param name="provider">
        /// Dependency provider
        /// </param>
        public void RemoveProvider(IDependencyProvider provider)
        {
            _providers.Remove(provider);
        }
        /// <summary>
        /// Remove all providers
        /// </summary>
        public void Clear()
        {
            _providers.Clear();
        }
        /// <summary>
        /// IDependencyProvider: Central repository of embedded resources
        /// </summary>
        public ResourcesRepository AllResources { get; set; }
        /// <summary>
        /// IDependencyProvider: Check if a given class or interface is supported
        /// </summary>
        /// <param type="services">
        /// Type names separated by semicolon
        /// </param>
        /// <returns>
        /// True if can instantiante objets of at least one of the given types
        /// </returns>
        public bool QueryClassOrInterface(string services)
        {
            foreach (IDependencyProvider provider in _providers)
            {
                if (provider.QueryClassOrInterface(services))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// IDependencyProvider: Get all the instances of all the rquested types or interfaces
        /// </summary>
        /// <param type="services">
        /// Semicolon separated type type list
        /// </param>
        /// <param type="filter">
        /// Object containing a filter to select elements
        /// </param>
        /// <returns>
        /// Enumeration of objects implementing the types as IUIIdentifier objects
        /// </returns>
        public IEnumerable<IUIIdentifier> GetObjects(string services, object filter = null)
        {
            foreach (IDependencyProvider provider in _providers)
            {
                if (provider.QueryClassOrInterface(services))
                {
                    foreach (IUIIdentifier uid in provider.GetObjects(services, filter))
                    {
                        yield return uid;
                    }
                }
            }
        }
    }
}

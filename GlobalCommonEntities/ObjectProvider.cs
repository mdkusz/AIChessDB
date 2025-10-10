using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using static GlobalCommonEntities.Properties.UIResources;

namespace GlobalCommonEntities
{
    /// <summary>
    /// Dependency provider for GlobalCommonEntities
    /// </summary>
    public class ObjectProvider : IDependencyProvider
    {
        private List<string> _services = new List<string> { nameof(ICredentialStore) };
        public ObjectProvider() { }
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
            List<string> lservices = new List<string>(services.Split(';'));
            return lservices.Any(s => _services.Contains(s));
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
            List<string> lservices = new List<string>(services.Split(';'));
            foreach (string service in lservices)
            {
                if (service == nameof(ICredentialStore))
                {
                    List<IUIIdentifier> lst = new List<IUIIdentifier>()
                    {
                        new ObjectWrapper(new AzureKeyVaultStore(), NAME_AzureKeyVaultStore, ""),
                        new ObjectWrapper(new ConfigFileCredentialStore(), NAME_ConfigFileCredentialStore, ""),
                        new ObjectWrapper(new EnvironmentVariableCredential(), NAME_EnvironmentVariable, ""),
                        new ObjectWrapper(new WindowsCredentialManager(), NAME_WindowsCredentialManager, "")
                    };
                    Type tkey = filter as Type;
                    foreach (IUIIdentifier uiid in lst)
                    {
                        ICredentialStore cs = uiid.Implementation() as ICredentialStore;
                        if (tkey == null || (cs != null && cs.KeyType == tkey))
                        {
                            yield return uiid;
                        }
                    }
                }
            }
        }
    }
}

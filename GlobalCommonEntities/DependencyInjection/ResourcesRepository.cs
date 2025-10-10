using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;

namespace GlobalCommonEntities.DependencyInjection
{
    /// <summary>
    /// Repository to centralize resources
    /// </summary>
    public class ResourcesRepository
    {
        protected CultureInfo _forceCulture = null;
        protected Dictionary<string, int> _resources = new Dictionary<string, int>();
        protected List<ResourceManager> _resourcemanagers = new List<ResourceManager>();
        public ResourcesRepository()
        {
        }
        /// <summary>
        /// Ignore resource names case
        /// </summary>
        public bool IgnoreCase { get; set; }
        public string ForceLanguage
        {
            get
            {
                if (_forceCulture != null)
                {
                    return _forceCulture.Name;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    _forceCulture = null;
                }
                _forceCulture = new CultureInfo(value);
            }
        }
        /// <summary>
        /// Index resources from a resource manager
        /// </summary>
        /// <param name="resource">
        /// ResourceManager to index
        /// </param>
        public void IndexResourcemanager(ResourceManager resource)
        {
            if (!_resourcemanagers.Contains(resource))
            {
                _resourcemanagers.Add(resource);
                foreach (string name in resource.GetResourceSet(CultureInfo.InvariantCulture, true, true).Cast<DictionaryEntry>().Select(e => e.Key.ToString()))
                {
                    _resources[name] = _resourcemanagers.Count - 1;
                }
            }
        }
        /// <summary>
        /// Add a resource manager
        /// </summary>
        /// <param name="resource">
        /// ResourceManager object to add
        /// </param>
        public void AddResourceManager(ResourceManager resource)
        {
            if (!_resourcemanagers.Contains(resource))
            {
                _resourcemanagers.Add(resource);
            }
        }
        /// <summary>
        /// Remove a resource manager
        /// </summary>
        /// <param name="resource">
        /// ResourceManager to remove
        /// </param>
        public void RemoveResourceManager(ResourceManager resource)
        {
            int ix = _resourcemanagers.IndexOf(resource);
            if (ix >= 0)
            {
                _resourcemanagers.Remove(resource);
                Dictionary<string, int> newresources = new Dictionary<string, int>();
                foreach (string key in _resources.Keys)
                {
                    if (_resources[key] > ix)
                    {
                        newresources[key] = _resources[key] - 1;
                    }
                    else if (_resources[key] < ix)
                    {
                        newresources[key] = _resources[key];
                    }
                }
                _resources = newresources;
            }
        }
        /// <summary>
        /// Clear all resources
        /// </summary>
        public void Clear()
        {
            _resources.Clear();
            _resourcemanagers.Clear();
        }
        /// <summary>
        /// Get a text string
        /// </summary>
        /// <param name="name">
        /// String name
        /// </param>
        /// <param name="defvalue">
        /// Default value
        /// </param>
        /// <returns>
        /// Text string or null
        /// </returns>
        public string GetString(string name, string defvalue = null)
        {
            if (_resources.ContainsKey(name))
            {
                return _resourcemanagers[_resources[name]].GetString(name);
            }
            foreach (ResourceManager resource in _resourcemanagers)
            {
                resource.IgnoreCase = IgnoreCase;
                string value = _forceCulture != null ? resource.GetString(name, _forceCulture) : resource.GetString(name);
                if (value != null)
                {
                    return value;
                }
            }
            return defvalue;
        }
        /// <summary>
        /// Get a generic object
        /// </summary>
        /// <param name="name">
        /// Object name
        /// </param>
        /// <returns>
        /// Object or null
        /// </returns>
        public object GetObject(string name)
        {
            if (_resources.ContainsKey(name))
            {
                return _resourcemanagers[_resources[name]].GetObject(name);
            }
            foreach (ResourceManager resource in _resourcemanagers)
            {
                resource.IgnoreCase = IgnoreCase;
                object value = _forceCulture == null ? resource.GetObject(name) : resource.GetObject(name, _forceCulture);
                if (value != null)
                {
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// Get a stream to read a resource
        /// </summary>
        /// <param name="name">
        /// Resource name
        /// </param>
        /// <returns>
        /// Stream or null
        /// </returns>
        public Stream GetStream(string name)
        {
            if (_resources.ContainsKey(name))
            {
                return _resourcemanagers[_resources[name]].GetStream(name);
            }
            foreach (ResourceManager resource in _resourcemanagers)
            {
                resource.IgnoreCase = IgnoreCase;
                Stream value = _forceCulture == null ? resource.GetStream(name) : resource.GetStream(name, _forceCulture);
                if (value != null)
                {
                    return value;
                }
            }
            return null;
        }
    }
}

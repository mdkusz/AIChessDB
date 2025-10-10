using BaseClassesAndInterfaces.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using System.Collections.Generic;
using AIChessDatabase.Properties;

namespace AIChessDatabase.Query
{
    /// <summary>
    /// Specialized value provider for keywords.
    /// </summary>
    /// <remarks>
    /// This class is used to provide a list of values to select in filter editors.
    /// </remarks>
    public class KeywordNameProvider : IValueListProvider
    {
        public List<string> _keywords;
        public KeywordNameProvider(List<string> keywords)
        {
            _keywords = keywords;
        }
        /// <summary>
        /// IValueListProvider: List of allowed values for a value selection editor
        /// </summary>
        /// <param name="editor">
        /// Element editor
        /// </param>
        /// <returns>
        /// ObjectWrapper values enumeration
        /// </returns>
        public IEnumerable<ObjectWrapper> GetAllowedValues(IValueListConsumer editor)
        {
            foreach (var keyword in _keywords)
            {
                yield return new ObjectWrapper(keyword, UIResources.ResourceManager.GetString(keyword) ?? keyword, "");
            }
        }
        /// <summary>
        /// IValueListProvider: Object identifier list to load all the auxiliary value selection lists
        /// </summary>
        /// <returns>
        /// Unique identifier enumeration
        /// </returns>
        public IEnumerable<string> GetSecondarySources() { yield break; }
        /// <summary>
        /// IValueListProvider: Set an auxiliary value list implementation
        /// </summary>
        /// <param name="uid">
        /// Object unique identifier
        /// </param>
        /// <param name="value">
        /// Object that implements the secondary list
        /// </param>
        public void SetSecondarySource(string uid, object value) { }
    }
}

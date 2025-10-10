using BaseClassesAndInterfaces.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using System.Collections.Generic;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Query
{
    /// <summary>
    /// Provides enum values for player color.
    /// </summary>
    /// <remarks>
    /// This class is used to provide a list of values to select in filter editors.
    /// </remarks>
    public class PlayerValueProvider : IValueListProvider
    {
        public PlayerValueProvider()
        {
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
            yield return new ObjectWrapper(0, LAB_WHITE, "");
            yield return new ObjectWrapper(1, LAB_BLACK, "");
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

using System;
using System.Collections.Generic;

namespace GlobalCommonEntities.UI
{
    /// <summary>
    /// EventArgs to notify that a dictionary key has been deleted
    /// </summary>
    public class DictionaryKeyDeletedEventHandler : EventArgs
    {
        /// <summary>
        /// Dictionary property name
        /// </summary>
        public string PropertyName { get; private set; }
        /// <summary>
        /// List of removed PropertyEditorInfo objects
        /// </summary>
        public List<PropertyEditorInfo> RemovedProperties { get; private set; }
        public DictionaryKeyDeletedEventHandler(string propertyName, List<PropertyEditorInfo> removedProperties)
        {
            PropertyName = propertyName;
            RemovedProperties = removedProperties;
        }
    }
    /// <summary>
    /// Delegate to notify that a dictionary key has been deleted
    /// </summary>
    /// <param name="sender">
    /// Object sending the event
    /// </param>
    /// <param name="e">
    /// Event arguments
    /// </param>
    public delegate void DictionaryKeyDeletedHandler(object sender, DictionaryKeyDeletedEventHandler e);

    /// <summary>
    /// EventArgs to notify that a dictionary key has been added
    /// </summary>
    public class NewDictionaryEntryEventArgs : EventArgs
    {
        /// <summary>
        /// Dictionary property name
        /// </summary>
        public string PropertyName { get; private set; }
        /// <summary>
        /// List of new PropertyEditorInfo objects
        /// </summary>
        public List<PropertyEditorInfo> NewProperties { get; private set; }
        /// <summary>
        /// Insert new controls at position
        /// </summary>
        public int InsertAt { get; private set; }
        public NewDictionaryEntryEventArgs(string propertyName, List<PropertyEditorInfo> newProperties, int insertat)
        {
            PropertyName = propertyName;
            NewProperties = newProperties;
            InsertAt = insertat;
        }
    }
    /// <summary>
    /// Operation to perform on the property editor container when refreshing editors
    /// </summary>
    public enum EditorContainerOperation
    {
        Add,
        Remove,
        Refresh,
        Rebuild,
        Update
    }
    /// <summary>
    /// Delegate to notify that a dictionary key has been added
    /// </summary>
    /// <param name="sender">
    /// Object sending the event
    /// </param>
    /// <param name="e">
    /// Event arguments
    /// </param>
    public delegate void NewDictionaryEntryHandler(object sender, NewDictionaryEntryEventArgs e);
    /// <summary>
    /// Refresh or create the property editor at the given index
    /// </summary>
    public class RefreshEditorEventArgs : EventArgs
    {
        /// <summary>
        /// New property editor information. Can be null when removing the editor at the given index.
        /// </summary>
        public PropertyEditorInfo Property { get; private set; }
        /// <summary>
        /// Index of the property editor to refresh
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// Create the whole control instead of just refreshing
        /// </summary>
        public EditorContainerOperation Operation { get; private set; }
        public RefreshEditorEventArgs(PropertyEditorInfo property,
            int index,
            EditorContainerOperation operation = EditorContainerOperation.Refresh)
        {
            Property = property;
            Index = index;
            Operation = operation;
        }
    }
    /// <summary>
    /// Delegate to notify that a property editor needs to be refreshed
    /// </summary>
    /// <param name="sender">
    /// Object sending the event
    /// </param>
    /// <param name="e">
    /// Event arguments
    /// </param>
    public delegate void RefreshEditorHandler(object sender, RefreshEditorEventArgs e);
    /// <summary>
    /// EventArgs to notify that a property needs objets
    /// </summary>
    public class SelectObjectsEventArgs : EventArgs
    {
        public PropertyEditorInfo Property { get; private set; }
        public SelectObjectsEventArgs(PropertyEditorInfo property)
        {
            Property = property;
        }
    }
    /// <summary>
    /// Delegate to notify that a property needs objets
    /// </summary>
    /// <param name="sender">
    /// object sending the event
    /// </param>
    /// <param name="e">
    /// Event arguments
    /// </param>
    public delegate void SelectObjectsHandler(object sender, SelectObjectsEventArgs e);
}


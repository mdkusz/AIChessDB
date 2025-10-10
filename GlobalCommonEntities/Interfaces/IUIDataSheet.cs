using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Enumeration to select a proper control to get a property value
    /// </summary>
    public enum InputEditorType
    {
        SingleLineText, MultilineText, Image, Audio, Video, FixedComboBox, ComboBox,
        TextFile, BinaryFile, ImageFile, AudioFile, VideoFile, Date, Time, DateTime, FileName, FileCollection,
        TextFileName, BinaryFileName, ImageFileName, AudioFileName, VideoFileName, MultiSelectList,
        Directory, IntValue, FloatValue, BoolValue, BlockTitle, CommandButton, ObjectSelector,
        Color, EnumValue, Password, ConnectionString, DataSheet, Custom
    }
    /// <summary>
    /// Interface for all data sheets
    /// </summary>
    /// <remarks>
    /// Use this interface for objects that need to expose properties to be edited in a user interface in a standardized way.
    /// </remarks>
    public interface IUIDataSheet : INotifyPropertyChanged, IEquatable<IUIDataSheet>
    {
        /// <summary>
        /// Use this event to know that a dictionary entry property editor set must be removed
        /// </summary>
        event DictionaryKeyDeletedHandler RemoveDictionaryEntry;
        /// <summary>
        /// Use this event to notify that a command has been executed
        /// </summary>
        event NewDictionaryEntryHandler DictionaryEntryAdded;
        /// <summary>
        /// Use this event to force creation or update of a new editor for a property
        /// </summary>
        event RefreshEditorHandler RefreshEditor;
        /// <summary>
        /// Check if an event is already subscribed
        /// </summary>
        /// <param name="eventName">
        /// Evnet name (case sensitive)
        /// </param>
        /// <returns>
        /// True if the event is subscribed
        /// </returns>
        bool EventSubscribed(string eventName);
        /// <summary>
        /// Release all sbscriptions for an event
        /// </summary>
        /// <param name="eventName">
        /// Evnet name (case sensitive)
        /// </param>
        void ReleaseEvent(string eventName);
        /// <summary>
        /// Editor information for all the editable Properties in this class
        /// </summary>
        List<PropertyEditorInfo> Properties { get; set; }
        /// <summary>
        /// Read only versión of the Properties list
        /// </summary>
        /// <remarks>
        /// This is used to show property values, but don't allow users to change its values.
        /// The editor types in this list usually are just BlockTitle, SingleLineText or MultilineText.
        /// Editors don't know from which PropertyEditorInfo list come the properties, so they don't show the properties as read only unless the ReadOnly property is set to true.
        /// </remarks>
        List<PropertyEditorInfo> ROProperties { get; }
        /// <summary>
        /// Interface to the object that manages ObjectSelector editors
        /// </summary>
        ISelectionObjectProvider SelectionManager { get; set; }
        /// <summary>
        /// JSON representation of the object
        /// </summary>
        /// <example>
        /// An example of use of this property is to provide users visual feedback of the object Json structure.
        /// </example>
        string AsJson { get; set; }
        /// <summary>
        /// Subscribe to events to get feedback
        /// </summary>
        bool NeedsFeedback { get; }
        /// <summary>
        /// Paent UIDataDheet
        /// </summary>
        /// <example>
        /// This is used for instance for lists and dictionaries entries. The parent object is that object that contains the list or dictionary.
        /// </example>
        /// <remarks>
        /// UIDataSheet objects can contain other UIDataSheet objects.
        /// </remarks>
        /// <seealso cref="ChildDataSheets"/>
        IUIDataSheet Parent { get; set; }
        /// <summary>
        /// Check completion status
        /// </summary>
        bool Completed { get; }
        /// <summary>
        /// List of child data sheets
        /// </summary>
        /// <seealso cref="Parent"/>
        List<IUIDataSheet> ChildDataSheets { get; }
        /// <summary>
        /// Apply changes made to the Properties list to the actual object properties
        /// </summary>
        void ApplyChanges();
        /// <summary>
        /// Apply changes made to the Properties list to the actual object properties
        /// </summary>
        /// <returns>
        /// Error message if the changes cannot be applied, otherwise empty string.
        /// </returns>
        Task<string> ApplyChangesAsync();
        /// <summary>
        /// Get an editor information by property name
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property
        /// </param>
        /// <param name="index">
        /// Index of the property in the Properties list
        /// </param>
        /// <returns>
        /// Property editor information corresponding to the property name
        /// </returns>
        PropertyEditorInfo GetEditorInfo(string propertyName, out int index);
        /// <summary>
        /// Get a command-type editor information by command label
        /// </summary>
        /// <param name="commandLabel">
        /// CommandLabel of the command property
        /// </param>
        /// <param name="index">
        /// Index of the property in the Properties list
        /// </param>
        /// <returns>
        /// Property editor information corresponding to the command
        /// </returns>
        PropertyEditorInfo GetCommandInfo(string commandlabel, out int index);
        /// <summary>
        /// Set the MethodParameters property of a PropertyEditorInfo object for a given method
        /// </summary>
        /// <param name="MethodName">
        /// Method name
        /// </param>
        /// <param name="pinfo">
        /// PropertyEditorInfo object to configure
        /// </param>
        void SetMethodParameterTypes(string methodName, PropertyEditorInfo pinfo);
        /// <summary>
        /// Create a copy of this object
        /// </summary>
        /// <remarks>
        /// There are two methods to replicate objects: Copy and Clone. 
        /// You can override both methods to provide the proper behavior.
        /// Clone can be a literal copy of the object, while Copy can be a new object with the same properties but some (like unique identifier) with different values.
        /// </remarks>
        /// <seealso cref="Clone"/>
        IUIDataSheet Copy();
        /// <summary>
        /// Create a clone of this object
        /// </summary>
        /// <seealso cref="Copy"/>
        IUIDataSheet Clone();
        /// <summary>
        /// Properties that are part of a dictionary entry have the PropertyName in the form "DictionaryPropertyName".Key."EntryPropertyName"
        /// This function removes all properties that are part of a dictionary entry
        /// </summary>
        /// <param name="dictionary">
        /// Dictionary property name
        /// </param>
        /// <param name="key">
        /// Key name
        /// </param>
        void RemoveDictionaryEntryProperties(string dictionary, string key);
        /// <summary>
        /// Properties that are part of a dictionary entry have the PropertyName in the form "DictionaryPropertyName".Key."EntryPropertyName"
        /// This function changes the key for all properties that are part of a dictionary entry
        /// </summary>
        /// <param name="dictionary">
        /// Dictionary property name
        /// </param>
        /// <param name="key">
        /// Key name
        /// </param>
        /// <param name="newkey">
        /// New key name
        /// </param>
        void ChangeDictionaryEntryPropertyKey(string dictionary, string key, string newkey);
    }
}

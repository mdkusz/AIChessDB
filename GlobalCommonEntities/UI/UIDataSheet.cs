using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GlobalCommonEntities.UI
{
    /// <summary>
    /// Base class for all data sheets
    /// </summary>
    /// <remarks>
    /// This is a default implementation of the IUIDataSheet interface.
    /// Use this base class for objects that need to expose properties to be edited in a user interface.
    /// </remarks>
    /// <seealso cref="IUIDataSheet"/>
    public abstract class UIDataSheet : IUIDataSheet, IEquatable<UIDataSheet>
    {
        protected string _uid = Guid.NewGuid().ToString();
        /// <summary>
        /// INotifyPropertyChanged: Property changed notification event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// IUIDataSheet: Use this event to know that a dictionary entry property editor set must be removed
        /// </summary>
        public event DictionaryKeyDeletedHandler RemoveDictionaryEntry;
        /// <summary>
        /// IUIDataSheet: Use this event to notify that a command has been executed
        /// </summary>
        public event NewDictionaryEntryHandler DictionaryEntryAdded;
        /// <summary>
        /// IUIDataSheet: Use this event to force creation or update of a new editor for a property
        /// </summary>
        public event RefreshEditorHandler RefreshEditor;
        /// <summary>
        /// IUIDataSheet: Check if an event is already subscribed
        /// </summary>
        /// <param name="eventName">
        /// Evnet name (case sensitive)
        /// </param>
        /// <returns>
        /// True if the event is subscribed
        /// </returns>
        public virtual bool EventSubscribed(string eventName)
        {
            switch (eventName)
            {
                case nameof(PropertyChanged):
                    return PropertyChanged != null;
                case nameof(RemoveDictionaryEntry):
                    return RemoveDictionaryEntry != null;
                case nameof(DictionaryEntryAdded):
                    return DictionaryEntryAdded != null;
                case nameof(RefreshEditor):
                    return RefreshEditor != null;
                default:
                    return false;
            }
        }
        /// <summary>
        /// IUIDataSheet: Release all sbscriptions for an event
        /// </summary>
        /// <param name="eventName">
        /// Evnet name (case sensitive)
        /// </param>
        public virtual void ReleaseEvent(string eventName)
        {
            switch (eventName)
            {
                case nameof(PropertyChanged):
                    if (PropertyChanged != null)
                    {
                        foreach (var d in PropertyChanged.GetInvocationList())
                        {
                            PropertyChanged -= (PropertyChangedEventHandler)d;
                        }
                    }
                    PropertyChanged = null;
                    break;
                case nameof(RemoveDictionaryEntry):
                    if (RemoveDictionaryEntry != null)
                    {
                        foreach (var d in RemoveDictionaryEntry.GetInvocationList())
                        {
                            RemoveDictionaryEntry -= (DictionaryKeyDeletedHandler)d;
                        }
                    }
                    RemoveDictionaryEntry = null;
                    break;
                case nameof(DictionaryEntryAdded):
                    if (DictionaryEntryAdded != null)
                    {
                        foreach (var d in DictionaryEntryAdded.GetInvocationList())
                        {
                            DictionaryEntryAdded -= (NewDictionaryEntryHandler)d;
                        }
                    }
                    DictionaryEntryAdded = null;
                    break;
                case nameof(RefreshEditor):
                    if (RefreshEditor != null)
                    {
                        foreach (var d in RefreshEditor.GetInvocationList())
                        {
                            RefreshEditor -= (RefreshEditorHandler)d;
                        }
                    }
                    RefreshEditor = null;
                    break;
            }
        }
        /// <summary>
        /// IUIDataSheet: Editor information for all the editable Properties in this class
        /// </summary>
        [JsonIgnore]
        public abstract List<PropertyEditorInfo> Properties { get; set; }
        /// <summary>
        /// IUIDataSheet: Read only versión of the Properties list
        /// </summary>
        /// <remarks>
        /// This is used to show property values, but don't allow users to change its values.
        /// The editor types in this list usually are just BlockTitle, SingleLineText or MultilineText.
        /// Editors don't know from which PropertyEditorInfo list come the properties, so they don't show the properties as read only unless the ReadOnly property is set to true.
        /// </remarks>
        [JsonIgnore]
        [Browsable(false)]
        public virtual List<PropertyEditorInfo> ROProperties { get { return null; } }
        /// <summary>
        /// IUIDataSheet: Interface to the object that manages ObjectSelector editors
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public virtual ISelectionObjectProvider SelectionManager { get; set; }
        /// <summary>
        /// IUIDataSheet: JSON representation of the object
        /// </summary>
        /// <example>
        /// An example of use of this property is to provide users visual feedback of the object Json structure.
        /// </example>
        [JsonIgnore]
        [Browsable(false)]
        public virtual string AsJson { get; set; }
        /// <summary>
        /// IUIDataSheet: Subscribe to events to get feedback
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public virtual bool NeedsFeedback { get { return false; } }
        /// <summary>
        /// IUIDataSheet: Parent IUIDataDheet object
        /// </summary>
        /// <example>
        /// This is used for instance for lists and dictionaries entries. The parent object is that object that contains the list or dictionary.
        /// </example>
        /// <remarks>
        /// UIDataSheet objects can contain other UIDataSheet objects.
        /// </remarks>
        /// <seealso cref="ChildDataSheets"/>
        [JsonIgnore]
        [Browsable(false)]
        public IUIDataSheet Parent { get; set; }
        /// <summary>
        /// IUIDataSheet: Check completion status
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public virtual bool Completed
        {
            get
            {
                foreach (PropertyEditorInfo p in Properties)
                {
                    if (p.Required)
                    {
                        if (GetType().GetProperty(p.PropertyName).GetValue(this) == null)
                        {
                            return false;
                        }
                    }
                }
                if (ChildDataSheets != null)
                {
                    foreach (UIDataSheet child in ChildDataSheets)
                    {
                        if (!child.Completed)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// IUIDataSheet: List of child data sheets
        /// </summary>
        /// <seealso cref="Parent"/>
        [JsonIgnore]
        [Browsable(false)]
        public virtual List<IUIDataSheet> ChildDataSheets { get { return null; } }
        /// <summary>
        /// IUIDataSheet: Apply changes made to the Properties list to the actual object properties
        /// </summary>
        public virtual void ApplyChanges() { }
        /// <summary>
        /// IUIDataSheet: Apply changes made to the Properties list to the actual object properties
        /// </summary>
        /// <returns>
        /// Error message if the changes cannot be applied, otherwise empty string.
        /// </returns>
        public virtual async Task<string> ApplyChangesAsync() { await Task.Yield(); return ""; }
        /// <summary>
        /// IUIDataSheet: Get an editor information by property name
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property
        /// </param>
        /// <param name="index">
        /// Index of the property editor in the Properties list
        /// </param>
        /// <returns>
        /// Property editor information corresponding to the property name
        /// </returns>
        public virtual PropertyEditorInfo GetEditorInfo(string propertyName, out int index)
        {
            index = -1;
            for (int ix = 0; ix < Properties.Count; ix++)
            {
                if (Properties[ix].PropertyName == propertyName)
                {
                    index = ix;
                    return Properties[ix];
                }
            }
            return null;
        }
        /// <summary>
        /// IUIDataSheet: Get a command-type editor information by command label
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
        public virtual PropertyEditorInfo GetCommandInfo(string commandlabel, out int index)
        {
            index = -1;
            for (int ix = 0; ix < Properties.Count; ix++)
            {
                if (Properties[ix].CommandLabel == commandlabel)
                {
                    index = ix;
                    return Properties[ix];
                }
            }
            return null;
        }
        /// <summary>
        /// IUIDataSheet: Set the MethodParameters property of a PropertyEditorInfo object for a given method
        /// </summary>
        /// <param name="MethodName">
        /// Method name
        /// </param>
        /// <param name="pinfo">
        /// PropertyEditorInfo object to configure
        /// </param>
        public virtual void SetMethodParameterTypes(string methodName, PropertyEditorInfo pinfo)
        {
            pinfo.MethodParameters = Type.EmptyTypes;
            pinfo.MethodParameterValues = null;
        }
        /// <summary>
        /// IUIDataSheet: Create a copy of this object
        /// </summary>
        /// <remarks>
        /// There are two methods to replicate objects: Copy and Clone. 
        /// You can override both methods to provide the proper behavior.
        /// Clone can be a literal copy of the object, while Copy can be a new object with the same properties but some (like unique identifier) with different values.
        /// </remarks>
        /// <seealso cref="Clone"/>
        public virtual IUIDataSheet Copy()
        {
            return null;
        }
        /// <summary>
        /// IUIDataSheet: Create a clone of this object
        /// </summary>
        /// <remarks>
        /// There are two methods to replicate objects: Copy and Clone. 
        /// You can override both methods to provide the proper behavior.
        /// Clone can be a literal copy of the object, while Copy can be a new object with the same properties but some (like unique identifier) with different values.
        /// </remarks>
        /// <seealso cref="Copy"/>
        public virtual IUIDataSheet Clone()
        {
            return null;
        }
        /// <summary>
        /// IEquatable interface member
        /// </summary>
        /// <param name="other">
        /// Other instance
        /// </param>
        /// <returns>
        /// True if both instances are the same
        /// </returns>
        public virtual bool Equals(UIDataSheet other)
        {
            if (other == null)
            {
                return false;
            }
            return _uid == other._uid;
        }
        public virtual bool Equals(IUIDataSheet other)
        {
            return Equals(other as UIDataSheet);
        }
        /// <summary>
        /// IUIDataSheet: Properties that are part of a dictionary entry have the PropertyName in the form "DictionaryPropertyName".Key."EntryPropertyName"
        /// This function removes all properties that are part of a dictionary entry
        /// </summary>
        /// <param name="dictionary">
        /// Dictionary property name
        /// </param>
        /// <param name="key">
        /// Key name
        /// </param>
        public virtual void RemoveDictionaryEntryProperties(string dictionary, string key)
        {
            List<PropertyEditorInfo> removed = new List<PropertyEditorInfo>();
            for (int ix = Properties.Count - 1; ix >= 0; ix--)
            {
                string[] pname = Properties[ix].PropertyName.Split('.');
                if ((pname.Length > 1) && (pname[0] == dictionary) && (pname[1] == key))
                {
                    removed.Add(Properties[ix]);
                    Properties.RemoveAt(ix);
                }
                else if ((pname.Length == 1) && (pname[0] == dictionary) &&
                    (Properties[ix].InitialValue?.ToString() == key))
                {
                    removed.Add(Properties[ix]);
                    Properties.RemoveAt(ix);
                }
            }
            if (removed.Count > 0)
            {
                RemoveDictionaryEntry?.Invoke(this, new DictionaryKeyDeletedEventHandler(dictionary, removed));
            }
        }
        /// <summary>
        /// IUIDataSheet: Properties that are part of a dictionary entry have the PropertyName in the form "DictionaryPropertyName".Key."EntryPropertyName"
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
        public virtual void ChangeDictionaryEntryPropertyKey(string dictionary, string key, string newkey)
        {
            for (int ix = Properties.Count - 1; ix >= 0; ix--)
            {
                string[] pname = Properties[ix].PropertyName.Split('.');
                if ((pname.Length > 1) && (pname[0] == dictionary) && (pname[1] == key))
                {
                    Properties[ix].PropertyName = string.Join(".", pname[0], newkey, pname[2]);
                }
                else if ((pname.Length == 1) && (pname[0] == dictionary))
                {
                    Properties[ix].InitialValue = newkey;
                }
            }
        }
        /// <summary>
        /// Method to invoke the RemoveDictionaryEntry event
        /// </summary>
        /// <param name="e">
        /// Event arguments
        /// </param>
        protected void InvokeNewDictionaryEntry(NewDictionaryEntryEventArgs e)
        {
            DictionaryEntryAdded?.Invoke(this, e);
        }
        /// <summary>
        /// Notify that a property has changed
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property. Default is the caller member name
        /// </param>
        protected void InvokePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Invoke the RefreshEditor event
        /// </summary>
        /// <param name="e">
        /// Event arguments
        /// </param>
        protected void InvokeRefreshEditor(RefreshEditorEventArgs e)
        {
            RefreshEditor?.Invoke(this, e);
        }
    }
}

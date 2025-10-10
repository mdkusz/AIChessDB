using System;
using System.ComponentModel;
using System.Reflection;

namespace DesktopControls.Tools
{
    /// <summary>
    /// Class to access event handlers
    /// </summary>
    public static class EventHelper
    {
        /// <summary>
        /// Get the event handler of a control
        /// </summary>
        /// <param name="control">
        /// Object that contains the event
        /// </param>
        /// <param name="eventName">
        /// Event to find
        /// </param>
        /// <returns>
        /// Delegeate to call teh event or null if not found
        /// </returns>
        public static Delegate GetEventHandler(object control, string eventName)
        {
            // Get control type
            Type controlType = control.GetType();

            EventInfo eventInfo = null;
            while (eventInfo == null)
            {
                // Get the specific event
                eventInfo = controlType.GetEvent(eventName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (eventInfo == null)
                {
                    controlType = controlType.BaseType;
                    if (controlType == null)
                    {
                        return null;
                    }
                }
            }
            // Get the field that store the events
            FieldInfo eventField = null;
            while (eventField == null)
            {
                eventField = controlType.GetField("Events", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (eventField == null)
                {
                    controlType = controlType.BaseType;
                    if (controlType == null)
                    {
                        return null;
                    }
                }
            }
            // Get the key for the event
            FieldInfo keyField = null;
            controlType = control.GetType();
            while (keyField == null)
            {
                keyField = controlType.GetField("Event" + eventName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                if (keyField == null)
                {
                    controlType = controlType.BaseType;
                    if (controlType == null)
                    {
                        return null;
                    }
                }
            }
            object key = keyField.GetValue(null);
            // Get the event in the control instance
            EventHandlerList events = eventField.GetValue(control) as EventHandlerList;
            if (events == null)
            {
                return null;
            }
            // Get the event delegate
            Delegate eventDelegate = events[key];
            return eventDelegate;
        }
    }
}

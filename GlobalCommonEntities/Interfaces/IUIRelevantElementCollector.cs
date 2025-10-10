using GlobalCommonEntities.UI;
using System.Collections.Generic;

namespace GlobalCommonEntities.Interfaces
{
    public interface IUIRelevantElementCollector
    {
        /// <summary>
        /// Top-level UI elements container.
        /// </summary>
        IUIRemoteControlElement BaseInstance { get; set; }
        /// <summary>
        /// Get first-level UI elements.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <returns>
        /// List of UI elements. Can be empty or null if there are no UI elements.
        /// </returns>
        List<UIRelevantElement> GetUIElements(IUIRemoteControlElement instance);
        /// <summary>
        /// Get all-level UI elements.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <returns>
        /// List of UI elements with all children included. Can be empty or null if there are no UI elements.
        /// </returns>
        List<UIRelevantElement> GetAllUIElements(IUIRemoteControlElement instance);
        /// <summary>
        /// Get the child UI elements of that on a specific path.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        List<UIRelevantElement> GetUIElementChildren(IUIRemoteControlElement instance, string path);
        /// <summary>
        /// Get the child UI elements of that on a specific path and all its children.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        List<UIRelevantElement> GetAllUIElementChildren(IUIRemoteControlElement instance, string path);
        /// <summary>
        /// Retrieve a UI element by its path.
        /// </summary>
        /// <param name="instance">
        /// Root instance to start searching from.
        /// </param>
        /// <param name="path">
        /// Dot-separated path to the control, starting with the root control's name.
        /// </param>
        /// <returns>
        /// UI element if found, otherwise null.
        /// </returns>
        object GetUIElementByPath(IUIRemoteControlElement instance, string path);
    }
}

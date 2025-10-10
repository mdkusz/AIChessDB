using GlobalCommonEntities.UI;
using System.Collections.Generic;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Interface to remote control UI components.
    /// </summary>
    public interface IUIRemoteControlElement
    {
        /// <summary>
        /// Unique identifier for the UI container.
        /// </summary>
        string UID { get; }
        /// <summary>
        /// Form friendly name.
        /// </summary>
        string FriendlyName { get; }
        /// <summary>
        /// Get first-level UI elements.
        /// </summary>
        /// <returns>
        /// List of first-level UI elements. Can be empty or null if there are no elements.
        /// </returns>
        List<UIRelevantElement> GetUIElements();
        /// <summary>
        /// Get all-level UI elements.
        /// </summary>
        /// <returns>
        /// List of all UI elements. Can be empty or null if there are no elements.
        /// </returns>
        List<UIRelevantElement> GetAllUIElements();
        /// <summary>
        /// Get the child UI elements of that on a specific path.
        /// </summary>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        List<UIRelevantElement> GetUIElementChildren(string path);
        /// <summary>
        /// Highlight a UI element for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to highlight.
        /// </param>
        /// <param name="seconds">
        /// Duration in seconds to highlight the element.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        void HighlightUIElement(string path, int seconds, string mode);
        /// <summary>
        /// Show a tooltip for a UI element with a comment for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to comment.
        /// </param>
        /// <param name="title">
        /// The title of the notification balloon.
        /// </param>
        /// <param name="comment">
        /// Comment to display in the tooltip.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        /// <param name="seconds">
        /// Seconds to display the tooltip.
        /// </param>
        void CommentUIElement(string path, string title, string comment, string mode, int seconds);
        /// <summary>
        /// Invoke an action on a UI element at the specified path.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to invoke the action on.
        /// </param>
        /// <param name="action">
        /// Action name to invoke on the UI element, such as "click", "double-click", etc.
        /// </param>
        /// <returns>
        /// String value or null
        /// </returns>
        /// <remarks>
        /// The format for action is: action[:param]
        /// </remarks>
        object InvokeElementAction(string path, string action);
    }
}

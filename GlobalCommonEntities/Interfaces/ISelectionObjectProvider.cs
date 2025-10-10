using GlobalCommonEntities.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Interface to provide an object list for ObjectSelector editors
    /// </summary>
    /// <remarks>
    /// Some InputEditorTypes imply the selection of objects from a list or dialog box.
    /// Objects that need this functionality, must implement this interface to provide selection data to the property editors.
    /// </remarks>
    public interface ISelectionObjectProvider
    {
        /// <summary>
        /// Title for the selection user interface
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Event to notify that the user wants to select an object
        /// </summary>
        event SelectObjectsHandler SelectionUIInvoked;
        /// <summary>
        /// Release SelectObjectsHandler event subscriptions
        /// </summary>
        void ReleaseEvent();
        /// <summary>
        /// Check if there are more objects to select from
        /// </summary>
        /// <param name="property">
        /// Property editor invoking the selection
        /// </param>
        bool HasMore(PropertyEditorInfo property);
        /// <summary>
        /// Get a list of objects to select from
        /// </summary>
        /// <returns>
        /// List of objects
        /// </returns>
        /// <param name="property">
        /// Property to get the object
        /// </param>
        Task<List<object>> GetSelectionObjects(PropertyEditorInfo property);
        /// <summary>
        /// Get a list of objects already selected
        /// </summary>
        /// <returns>
        /// List of objects
        /// </returns>
        /// <param name="property">
        /// Property to get the object list
        /// </param>
        Task<List<object>> GetSelectedObjects(PropertyEditorInfo property);
        /// <summary>
        /// Set the list of selected objects
        /// </summary>
        /// <param name="objects">
        /// Selected object list
        /// </param>
        /// <param name="property">
        /// Property to set the selection
        /// </param>
        /// <returns>
        /// List of error messages, if any
        /// </returns>
        Task<List<string>> SetSelection(List<object> objects, PropertyEditorInfo property);
        /// <summary>
        /// Remove a list of objects
        /// </summary>
        /// <param name="objects">
        /// Object to remove list
        /// </param>
        /// <param name="property">
        /// Property to remove the selection
        /// </param>
        /// <returns>
        /// List of error messages, if any
        /// </returns>
        Task<List<string>> RemoveSelection(List<object> objects, PropertyEditorInfo property);
        /// <summary>
        /// Remove the object container
        /// </summary>
        /// <param name="property">
        /// Property editor invoking the selection
        /// </param>
        /// <returns>
        /// Error messages, if any
        /// </returns>
        Task<string> RemoveContainer(PropertyEditorInfo property);
        /// <summary>
        /// Fire event to select an object
        /// </summary>
        /// <param name="sender">
        /// Object invoking the selection
        /// </param>
        /// <param name="property">
        /// Property editor invoking the selection
        /// </param>
        void InvokeAction(object sender, PropertyEditorInfo property);
    }
}

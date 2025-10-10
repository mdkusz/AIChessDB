using GlobalCommonEntities.UI;
using System.Drawing;
using System.Windows.Forms;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// IUIDataSheet input editor factory interface.
    /// </summary>
    public interface IInputEditorFactory
    {
        /// <summary>
        /// Editor border size. Each side can have a different size. 0 means no border.
        /// </summary>
        Padding BorderSize { get; set; }
        /// <summary>
        /// Editor border colors
        /// </summary>
        Color LeftBorderColor { get; set; }
        Color TopBorderColor { get; set; }
        Color RightBorderColor { get; set; }
        Color BottomBorderColor { get; set; }
        /// <summary>
        /// Editor background and foreground colors
        /// </summary>
        Color EditorBackColor { get; set; }
        Color EditorForeColor { get; set; }
        /// <summary>
        /// Block header background and foreground colors
        /// </summary>
        Color BlockHeaderForeColor { get; set; }
        Color BlockHeaderBackColor { get; set; }
        /// <summary>
        /// Check whether an editor type is supported
        /// </summary>
        /// <param name="etype">
        /// EditorType
        /// </param>
        /// <returns>
        /// True if the editor type is suppoerted
        /// </returns>
        bool AcceptsEditorType(InputEditorType etype);
        /// <summary>
        /// Create an editor for a property
        /// </summary>
        /// <param name="pinfo">
        /// Porperty creation and configuration information
        /// </param>
        /// <param name="instance">
        /// IUIDataSheet object to edit
        /// </param>
        /// <param name="container">
        /// Editor container control
        /// </param>
        /// <returns>
        /// IInputEditorBase control
        /// </returns>
        /// <seealso cref="InputEditorType"/>
        /// <seealso cref="PropertyEditorInfo"/>
        IInputEditorBase CreateEditor(PropertyEditorInfo pinfo, object instance, Control container);
        /// <summary>
        /// Populate a list of proerty editors in a container
        /// </summary>
        /// <param name="ds">
        /// IUIDataSheet object with properties
        /// </param>
        /// <param name="container">
        /// Container control to add property editors
        /// </param>
        /// <param name="readOnly">
        /// Populate read only or regular properties
        /// </param>
        void SetPropertyEditors(IUIDataSheet ds, Control container, bool readOnly = false);
    }
}

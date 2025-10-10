using GlobalCommonEntities.UI;
using System.Drawing;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Base interface for IUIDataSheet input editors.
    /// </summary>
    public interface IInputEditorBase
    {
        /// <summary>
        /// Editor configuration
        /// </summary>
        PropertyEditorInfo EditorInfo { get; }
        /// <summary>
        /// IUIDataSheet instance
        /// </summary>
        IUIDataSheet Instance { get; }
        /// <summary>
        /// Background color if this is a title panel
        /// </summary>
        Color TitleBackColor { get; set; }
        /// <summary>
        /// Foreground color if this is a title panel
        /// </summary>
        Color TitleForeColor { get; set; }
        /// <summary>
        /// Refresh the editor value and selection data when applicable
        /// </summary>
        void RefreshEditorValue();
    }
}

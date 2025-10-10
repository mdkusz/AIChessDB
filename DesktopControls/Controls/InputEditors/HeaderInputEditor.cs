using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor for property block headers
    /// </summary>
    /// <remarks>
    /// This editor is to define property block headers.
    /// It is not associated with any property. The Propertyname is used as the header text.
    /// You can use the BackColor and ForeColor properties of PropertyEditorInfo to change the header colors. 
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not BlockTitle
    /// </exception>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class HeaderInputEditor : InputEditorBase
    {
        public HeaderInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.BlockTitle)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (!pinfo.BackColor.HasValue)
            {
                BackColor = _titleBC;
            }
            if (!pinfo.ForeColor.HasValue)
            {
                ForeColor = _titleFC;
            }
            Padding = new Padding(3, 0, 3, 0);
            CreateControls(container);
        }
        /// <summary>
        /// Create editing controls
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void CreateControls(Control container)
        {
            Width = container.ClientSize.Width - container.Padding.Horizontal;
            Label lbl = new Label()
            {
                AccessibleDescription = _pInfo.PropertyName + SFX_HEADER,
                AccessibleRole = AccessibleRole.StaticText,
                AutoSize = true,
                Name = NAME_lbTitle,
                Text = _pInfo.PropertyName,
                Top = Padding.Top,
                Left = Padding.Left,
                Font = new Font(container.Font, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 3)
            };
            Height = container.Font.Height + Padding.Vertical + lbl.Margin.Vertical;
            Controls.Add(lbl);
            lbl.Top = (Height - lbl.Height) / 2;
        }
        /// <summary>
        /// Resize editing controls
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void ResizeControls(Control container)
        {
            Width = container.ClientSize.Width - container.Padding.Horizontal;
            Control lbl = Controls.Find(NAME_lbTitle, false).FirstOrDefault();
            Height = container.Font.Height + Padding.Vertical + (lbl != null ? lbl.Margin.Vertical : 0);
            lbl.Top = (Height - lbl.Height) / 2;
        }
    }
}

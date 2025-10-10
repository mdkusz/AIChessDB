using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor for Color properties
    /// </summary>
    /// <remarks>
    /// This editor uses a ColorDialog to select the color, and a colored Panel to show the selected color.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not Color or the property is not found
    /// </exception>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    /// <seealso cref="DIalogBoxInputEditor"/>
    public class ColorInputEditor : DIalogBoxInputEditor
    {
        private Panel _colorPanel;
        public ColorInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.Color)
            {
                throw new ArgumentException(ERR_BadEditorType);
            }
            if (_property == null)
            {
                throw new ArgumentException(ERR_UnknownProperty);
            }
            CreateControls(container);
        }
        /// <summary>
        /// Add the control to edit the property value, based on the property editor type
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        /// <param name="name">
        /// Force control text if not null
        /// </param>
        protected override void AddControl(Control container, string text = null)
        {
            base.AddControl(container, text);
            _colorPanel = new Panel()
            {
                BackColor = (Color)_property.GetValue(_instance),
                BorderStyle = BorderStyle.FixedSingle,
                Height = _btnDialog.Height,
                Width = 50,
                Left = _btnDialog.Right + 8,
                Top = _btnDialog.Top
            };
            Controls.Add(_colorPanel);
            ResizeControl(_colorPanel, true);
        }
        /// <summary>
        /// Resize the control to fit the panel
        /// </summary>
        /// <param name="control">
        /// Control to resize
        /// </param>
        /// <param name="topleft">
        /// Update Top and Left properties
        /// </param>
        protected override void ResizeControl(Control control, bool topleft)
        {
            base.ResizeControl(control, topleft);
            if (control == _colorPanel)
            {
                if (topleft)
                {
                    control.Top = Height - Padding.Bottom - control.Height - 1;
                    control.Left = _btnDialog.Right + 8;
                }
                control.Width = Width - control.Left - Padding.Right;
            }
        }
        protected override void ShowDialog(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                _property.SetValue(_instance, cd.Color);
                _colorPanel.BackColor = cd.Color;
            }
        }
    }
}

using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor for properties needing a dialog box
    /// </summary>
    /// <remarks>
    /// This is an abstract base calss for editors using a dialog box to select the property value.
    /// Uses a Button control with a magnifying glass icon to show the dialog box
    /// </remarks>
    /// <seealso cref="PropertyInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public abstract class DIalogBoxInputEditor : PropertyInputEditorBase
    {
        protected Button _btnDialog;
        public DIalogBoxInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
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
            _btnDialog = new Button()
            {
                AccessibleDescription = Description,
                AccessibleName = text,
                AccessibleRole = AccessibleRole.PushButton,
                Image = ICO_Find.ToBitmap(),
                ImageAlign = ContentAlignment.MiddleCenter,
                Text = "",
                Padding = new Padding(4)
            };
            _btnDialog.Width = _btnDialog.Image.Width + _btnDialog.Padding.Horizontal;
            _btnDialog.Height = _btnDialog.Image.Height + _btnDialog.Padding.Vertical;
            _btnDialog.Click += ShowDialog;
            Height += _btnDialog.Height;
            Controls.Add(_btnDialog);
            ResizeControl(_btnDialog, true);
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
            if (topleft && (control == _btnDialog))
            {
                control.Top = Height - Padding.Bottom - control.Height - 1;
                control.Left = Padding.Left;
            }
        }
        protected abstract void ShowDialog(object sender, EventArgs e);
    }
}

using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Editor to select a directory as the value of a property.
    /// </summary>
    public class DirectoryInputEditor : DIalogBoxInputEditor
    {
        private Label _dirLabel;

        public DirectoryInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.Directory)
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
            _dirLabel = new Label()
            {
                AutoSize = true,
                AutoEllipsis = true,
                Left = _btnDialog.Right + 8,
                Top = _btnDialog.Top,
                Font = container.Font,
                Text = _pInfo.InitialValue?.ToString() ?? ""
            };
            Controls.Add(_dirLabel);
            ResizeControl(_dirLabel, true);
            _dirLabel.Font = null;
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
            if (control == _dirLabel)
            {
                if (topleft)
                {
                    control.Top = _btnDialog.Top;
                    control.Left = _btnDialog.Right + 8;
                }
                control.Width = Width - control.Left - Padding.Right;
            }
        }
        protected override void ShowDialog(object sender, EventArgs e)
        {
            FolderBrowserDialog cd = new FolderBrowserDialog();
            cd.SelectedPath = _property.GetValue(_instance)?.ToString() ?? string.Empty;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                _property.SetValue(_instance, cd.SelectedPath);
                _dirLabel.Text = cd.SelectedPath;
            }
        }
    }
}

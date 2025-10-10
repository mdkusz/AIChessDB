using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    public class MultipleFileNameInputEditor : DIalogBoxInputEditor
    {
        private Label _fileLabel;
        public MultipleFileNameInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.FileCollection)
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
            _fileLabel = new Label()
            {
                AutoSize = true,
                AutoEllipsis = true,
                Left = _btnDialog.Right + 8,
                Top = _btnDialog.Top,
                Font = container.Font,
                Text = _pInfo.InitialValue?.ToString() ?? ""
            };
            Controls.Add(_fileLabel);
            ResizeControl(_fileLabel, true);
            _fileLabel.Font = null;
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
            if (control == _fileLabel)
            {
                if (topleft)
                {
                    control.Top = _btnDialog.Top + (_btnDialog.Height - control.Height) / 2;
                    control.Left = _btnDialog.Right + 8;
                }
                control.Width = Width - control.Left - Padding.Right;
            }
        }
        protected override void ShowDialog(object sender, EventArgs e)
        {
            OpenFileDialog cd = new OpenFileDialog();
            cd.CheckFileExists = false;
            cd.CheckPathExists = false;
            cd.ValidateNames = false;
            cd.Multiselect = true;
            cd.FileName = _property.GetValue(_instance)?.ToString() ?? string.Empty;
            cd.Filter = string.Join("|", _pInfo.Values ?? new List<object> { "All Files (*.*)", "*.*" });
            if (cd.ShowDialog() == DialogResult.OK)
            {
                _property.SetValue(_instance, cd.FileNames);
                _fileLabel.Text = $"{cd.FileNames?.Length} " + LAB_FIles;
            }
        }
    }
}

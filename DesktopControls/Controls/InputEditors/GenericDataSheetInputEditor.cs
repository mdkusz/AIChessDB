using DesktopControls.Dialogs;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// SHows a dialog box to edit a property of type IUIDataSheet
    /// </summary>
    public class GenericDataSheetInputEditor : DIalogBoxInputEditor
    {
        private Label _dsLabel;
        public GenericDataSheetInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.DataSheet)
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
            _btnDialog.Image = ICO_DataSheet.ToBitmap();
            _dsLabel = new Label()
            {
                AutoSize = true,
                Left = _btnDialog.Right + 8,
                Top = _btnDialog.Top,
                Font = container.Font,
                Text = _pInfo.InitialValue?.ToString() ?? ""
            };
            Controls.Add(_dsLabel);
            ResizeControl(_dsLabel, true);
            _dsLabel.Font = null;
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
            if (control == _dsLabel)
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
            DataSheetDialog cd = new DataSheetDialog();
            cd.Text = Title;
            cd.StartPosition = FormStartPosition.CenterParent;
            cd.DataSheet = _property.GetValue(_instance) as UIDataSheet;
            cd.EditorFactory = _pInfo.Service as IInputEditorFactory ?? new InputEditorFactory();
            if (cd.ShowDialog(this) == DialogResult.OK)
            {
                _property.SetValue(_instance, cd.DataSheet);
                _dsLabel.Text = cd.DataSheet.ToString();
            }
        }
    }
}

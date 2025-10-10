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
    /// Editor to select objects from a list
    /// </summary>
    /// <example>
    /// The Assistant object in the OpenAIAPI project uses this editor to select tools like File Search or Code Interpreter.
    /// </example>
    /// <remarks>
    /// This editor uses a dialog box to edit an arbitrary typed object from a list.
    /// The PropertyName property must be the name of a method that returns an object of the desired type, and has a single parameter of PropertyEditorInfo type.
    /// There is a Button that calls the method to open the selection dialog box.
    /// It is the shown dialog box which is responsible for setting the property value.
    /// The generic dialog box ObjectSelectionDialog can be used as a default dialog box
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// When EditorType is not ObjectSelector, the method is not found, or the method does not have a parameter of PropertyEditorInfo type
    /// </exception>
    /// <seealso cref="MethodInputEditorBase"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    public class ObjectSelectorInputEditor : MethodInputEditorBase
    {
        public ObjectSelectorInputEditor(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            if (pinfo.EditorType != InputEditorType.ObjectSelector)
            {
                throw new ArgumentException(ERR_BadEditorType);
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
            Button btn = new Button()
            {
                AccessibleDescription = Description,
                AccessibleName = _pInfo?.CommandCaptions[0]?.ToString() ?? NAME_command,
                AccessibleRole = AccessibleRole.PushButton,
                Text = _pInfo?.CommandCaptions[0]?.ToString() ?? NAME_command,
                Name = NAME_ctlEditor,
                Font = container.Font,
                AutoSize = true
            };
            btn.Click += (s, e) =>
            {
                try
                {
                    _method.Invoke(_instance, _pInfo.MethodParameterValues);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            Height += btn.Height + Padding.Bottom;
            Controls.Add(btn);
            ResizeControl(btn, true);
            btn.Font = null;
        }
        /// <summary>
        /// Resize the control to edit the property value, based on the property editor type
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void ResizeControl(Control container)
        {
            Control tb = Controls.Find(NAME_ctlEditor, false).FirstOrDefault();
            Height += tb.Height;
            ResizeControl(tb, true);
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
            using (Graphics gr = control.CreateGraphics())
            {
                int maxw = string.IsNullOrEmpty(control.Text) ? Width - Padding.Horizontal : (int)Math.Ceiling(gr.MeasureString(control.Text, control.Font).Width);
                control.Width = Math.Min(maxw, Width - Padding.Horizontal);
            }
            base.ResizeControl(control, topleft);
        }
    }
}

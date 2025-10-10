using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Base class for editors based on methods
    /// </summary>
    /// <remarks>
    /// This editor is the abstract base class for all method based editors.
    /// </remarks>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    /// <seealso cref="InputEditorType"/>
    /// <descendant>CommandButtonInputEditor</descendant>
    /// <descendant>ObjectSelectorInputEditor</descendant>
    public abstract class MethodInputEditorBase : InputEditorBase
    {
        protected MethodInfo _method;

        public MethodInputEditorBase(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            _method = pinfo.ResolveMethod(ref _instance);
        }

        protected override void CreateControls(Control container)
        {
            DisplayNameAttribute displayName = _method.GetCustomAttribute<DisplayNameAttribute>();
            string pname = displayName != null ? displayName.DisplayName : (_pInfo.CommandLabel ?? _pInfo.PropertyName);
            DescriptionAttribute description = _method.GetCustomAttribute<DescriptionAttribute>();
            Description = description?.Description;
            Width = container.ClientSize.Width - container.Padding.Horizontal;
            Label lbl = new Label()
            {
                AccessibleDescription = Description,
                AccessibleName = pname,
                AccessibleRole = AccessibleRole.StaticText,
                AutoSize = true,
                Name = NAME_lbTitle + (_pInfo.Required ? "*" : ""),
                Text = pname,
                Top = Padding.Top,
                Left = Padding.Left,
                Font = new Font(container.Font, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 3)
            };
            Height = container.Font.Height + Padding.Vertical + lbl.Margin.Vertical;
            Controls.Add(lbl);
            AddControl(container);
        }
    }
}

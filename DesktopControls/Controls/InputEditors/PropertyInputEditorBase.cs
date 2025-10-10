using GlobalCommonEntities.UI;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Generic panel to edit a property of an UIDataSheet object
    /// </summary>
    /// <remarks>
    /// This editor is the base class for all property editors.
    /// Property names can be simple, or it can be a chain of property names separated by dots, so you can reference properties of objects not just in the root object.
    /// </remarks>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="PropertyEditorInfo"/>
    public abstract class PropertyInputEditorBase : InputEditorBase
    {
        protected PropertyInfo _property;

        public PropertyInputEditorBase(PropertyEditorInfo pinfo, object instance, Control container) : base(pinfo, instance, container)
        {
            _property = pinfo.ResolveProperty(ref _instance);
        }
        /// <summary>
        /// Create editing controls
        /// </summary>
        /// <param name="container">
        /// Editor container to extract font, color, and sizes information
        /// </param>
        protected override void CreateControls(Control container)
        {
            DisplayNameAttribute displayName = _property.GetCustomAttribute(typeof(DisplayNameAttribute), false) as DisplayNameAttribute;
            string pname = displayName != null ? displayName.DisplayName : _pInfo.PropertyName;
            DescriptionAttribute description = _property.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
            Description = description?.Description;
            Title = pname;
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

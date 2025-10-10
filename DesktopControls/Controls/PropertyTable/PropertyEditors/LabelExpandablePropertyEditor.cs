using DesktopControls.Controls.PropertyTable.Interfaces;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedades compuestas con valor no editable / 
    /// Composite properties editor with read only value
    /// </summary>
    public class LabelExpandablePropertyEditor : ExpandablePropertyEditor
    {
        protected Label _roLabel;
        public LabelExpandablePropertyEditor()
        {
        }
        /// <summary>
        /// Realizar configuración extra de controles / 
        /// Set extra control configuration
        /// </summary>
        protected override void SetExtraControls()
        {
            base.SetExtraControls();
            if (_roLabel == null)
            {
                using (Graphics g = Graphics.FromHwnd(pEditor.Handle))
                {
                    _roLabel = new Label()
                    {
                        Name = "lText",
                        Left = 0,
                        Top = (HeadersHeight - (int)g.MeasureString(DisplayName, HeadersFont).Height) / 2,
                        Text = DisplayName,
                        Margin = new Padding(0),
                        AutoSize = true
                    };
                }
            }
            pEditor.Controls.Add(_roLabel);
        }
        /// <summary>
        /// Obtener el valor de la propiedad / 
        /// Get property value
        /// </summary>
        protected override void GetValue()
        {
            base.GetValue();
            if ((_property != null) && (_instance != null))
            {
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((vmgr != null) && vmgr.Managed(_property.Name))
                {
                    // Obtener el valor a través del objeto instancia
                    // Get the value from the instance
                    _value = vmgr.GetValue(_property.Name, ValueIndex);
                }
                else
                {
                    // Obtener el valor a través del descriptor de la propiedad
                    // Get the value from the property descriptor
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    _value = Property.GetValue(_instance, index);
                }
                if ((_value == null) || string.IsNullOrEmpty(_value.ToString()))
                {
                    _roLabel.Text = DisplayName;
                    Text = null;
                }
                else
                {
                    _roLabel.Text = _value.ToString();
                    Text = null;
                }
            }
        }
    }
}

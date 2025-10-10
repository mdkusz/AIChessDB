using DesktopControls.Controls.PropertyTable.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedad ObjectWrapper con lista de posibles valores y selección múltiple / 
    /// ObjectWrapper property editor with a multiselect value list
    /// </summary>
    public class ObjectWrapperCheckedListBoxPropertyEditor : ObjectWrapperEditorBase
    {
        protected FlowLayoutPanel _editor;
        protected Label _roLabel;
        protected IList _valueList;
        public ObjectWrapperCheckedListBoxPropertyEditor() : base()
        {
        }
        /// <summary>
        /// Configuración de los controles de la interfaz de usuario / 
        /// User interface controls configuration
        /// </summary>
        protected override void ConfigureUI()
        {
            base.ConfigureUI();
            ClearControls();
            if (_property != null)
            {
                if (_property.CanWrite)
                {
                    _editor = new FlowLayoutPanel()
                    {
                        Name = "cbEditor",
                        Left = Padding.Horizontal,
                        Width = _client.ClientSize.Width - Padding.Horizontal,
                        Height = _client.ClientSize.Height - Padding.Vertical,
                        FlowDirection = FlowDirection.TopDown,
                        WrapContents = false,
                        AutoScroll = true,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                        BorderStyle = BorderStyle.None
                    };
                    AddControl(_editor);
                }
                else
                {
                    _roLabel = new Label()
                    {
                        Name = "lText",
                        Left = Padding.Horizontal,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        AutoSize = true
                    };
                    AddControl(_roLabel);
                }
                // Establecer la altura total del editor
                // Set editor total height
                SetHeight(0);
            }
        }
        /// <summary>
        /// Color de texto de los controles sin color de fondo / 
        /// Text color of the controls without background color
        /// </summary>
        public override Color CaptionColor
        {
            get
            {
                return base.CaptionColor;
            }
            set
            {
                base.CaptionColor = value;
                if (_editor != null)
                {
                    _editor.ForeColor = value;
                }
            }
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
                if (_property.CanWrite)
                {
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    _valueList = Property.GetValue(_instance, index) as IList;
                    if (_vProvider != null)
                    {
                        GetAvailableValues();
                        _dirty = false;
                    }
                }
                else
                {
                    // Obtener el texto del valor de solo lectura
                    // Get read only value text
                    IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                    if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                    {
                        // Obtener el valor a través del objeto instancia
                        // Get the value from the instance
                        object pvalue = vmgr.GetValue(_property.Name, ValueIndex);
                        _roLabel.Text = pvalue == null ? "" : pvalue.ToString();
                    }
                    else
                    {
                        // Obtener el valor a través del descriptor de la propiedad
                        // Get the value from the property descriptor
                        object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                        object pvalue = Property.GetValue(_instance, index);
                        _roLabel.Text = pvalue == null ? "" : pvalue.ToString();
                    }
                    _dirty = false;
                }
            }
        }
        /// <summary>
        /// Establecer el valor de la propiedad / 
        /// Set the property value
        /// </summary>
        protected override void SetValue()
        {
            base.SetValue();
            if ((_property != null) && _property.CanWrite && (_vProvider != null))
            {
                IList oldval;
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                {
                    // Obtener el valor a través del objeto instancia
                    // Get the value from the instance
                    oldval = vmgr.GetValue(_property.Name, ValueIndex) as IList;
                }
                else
                {
                    // Obtener el valor a través del descriptor de la propiedad
                    // Get the value from the property descriptor
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    oldval = Property.GetValue(_instance, index) as IList;
                }
                Property.SetValue(_instance, _valueList);
                _dirty = true;
                OnPropertyChanged(oldval);
            }
        }
        /// <summary>
        /// Lista de valores disponibles para la propiedad / 
        /// Property available values list
        /// </summary>
        protected override void GetAvailableValues()
        {
            _editor.Controls.Clear();
            foreach (ObjectWrapper owr in _vProvider.GetAllowedValues(_property.Name))
            {
                CheckBox cb = new CheckBox()
                {
                    Name = "cb" + (_editor.Controls.Count + 1).ToString(),
                    Text = owr.FriendlyName,
                    AutoSize = true,
                    AutoCheck = true,
                    Checked = (_valueList != null) && _valueList.Contains(owr.Implementation()),
                    Tag = owr.Implementation()
                };
                cb.CheckedChanged += new EventHandler(cbEditor_ItemCheck);
                _editor.Controls.Add(cb);
            }
            if ((_editor != null) && (_editor.Controls.Count > 0))
            {
                _editor.Height = Math.Min(8, _editor.Controls.Count + 1) * _editor.Controls[0].Height;
            }
        }
        /// <summary>
        /// Cambiar estado de selección de uno de los posibles valores / 
        /// Change value selection state
        /// </summary>
        protected virtual void cbEditor_ItemCheck(object sender, EventArgs e)
        {
            if (_valueList != null)
            {
                CheckBox cb = sender as CheckBox;
                if (cb.Checked)
                {
                    if (!_valueList.Contains(cb.Tag))
                    {
                        _valueList.Add(cb.Tag);
                    }
                }
                else
                {
                    if (_valueList.Contains(cb.Tag))
                    {
                        _valueList.Remove(cb.Tag);
                    }
                }
                SetValue();
            }
        }
    }
}

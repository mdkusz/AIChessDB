using DesktopControls.Controls.PropertyTable.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedades de texto / 
    /// Text property editor
    /// </summary>
    public class TextPropertyEditor : PropertyEditorBase
    {
        protected TextBox _editor = null;
        protected Label _roLabel = null;
        protected Color _txtForeColor;
        /// <summary>
        /// Gestión de números / 
        /// Number management
        /// </summary>
        protected bool _isNumber = false;
        protected double _minValue = double.MinValue;
        protected double _maxValue = double.MaxValue;
        public TextPropertyEditor() : base()
        {
        }
        /// <summary>
        /// Caso gramatical del texto / 
        /// Text character casing
        /// </summary>
        public override CharacterCasing Case
        {
            get => base.Case;
            set
            {
                base.Case = value;
                if (_editor != null)
                {
                    _editor.CharacterCasing = value;
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
                object pvalue;
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                {
                    pvalue = vmgr.GetValue(_property.Name, ValueIndex);
                }
                else
                {
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    pvalue = Property.GetValue(_instance, index);
                }
                if (_property.CanWrite)
                {
                    _editor.Text = pvalue == null ? "" : pvalue.ToString();
                }
                else
                {
                    _roLabel.Text = pvalue == null ? "" : pvalue.ToString();
                }
                _dirty = false;
            }
        }
        /// <summary>
        /// Establecer el valor de la propiedad / 
        /// Set property value
        /// </summary>
        protected override void SetValue()
        {
            if (_dirty)
            {
                base.SetValue();
                if ((_property != null) && _property.CanWrite && (_instance != null))
                {
                    IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                    if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                    {
                        object value = vmgr.GetValue(_property.Name, ValueIndex);
                        string cvalue = value == null ? "" : value.ToString();
                        if (string.Compare(_editor.Text, cvalue, false) != 0)
                        {
                            vmgr.SetValue(_property.Name, Convert.ChangeType(_editor.Text, Property.PropertyType), ValueIndex);
                            OnPropertyChanged(value);
                        }
                    }
                    else
                    {
                        object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                        object value = Property.GetValue(_instance, index);
                        string cvalue = value == null ? "" : value.ToString();
                        if (string.Compare(_editor.Text, cvalue, false) != 0)
                        {
                            Property.SetValue(_instance, Convert.ChangeType(_editor.Text, Property.PropertyType), index);
                            OnPropertyChanged(value);
                        }
                    }
                }
                _dirty = false;
            }
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
                int maxheight = 0;
                IPropertyConfigurationProvider cfgprovider = _instance as IPropertyConfigurationProvider;
                if (_property.CanWrite && ((cfgprovider == null) ||
                    !cfgprovider.ReadOnly(_property.Name, ValueIndex).HasValue ||
                    !cfgprovider.ReadOnly(_property.Name, ValueIndex).Value))
                {
                    CreateEditor(Padding.Left, _client.ClientSize.Width - Padding.Horizontal);
                    maxheight = _editor.Height;
                    SetStyle();
                }
                else
                {
                    _roLabel = new Label()
                    {
                        Name = "lText",
                        Left = Padding.Left,
                        Top = Padding.Top,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left,
                        AutoSize = true
                    };
                    AddControl(_roLabel);
                    maxheight = _roLabel.Height;
                }
                SetHeight(maxheight);
                if ((cfgprovider != null) && (_editor != null))
                {
                    bool? isnum = cfgprovider.IsNumeric(_property.Name, ValueIndex);
                    _isNumber = isnum.HasValue ? isnum.Value : false;
                    if (_isNumber)
                    {
                        double? mvalue = cfgprovider.MinValue(_property.Name, ValueIndex);
                        _minValue = mvalue.HasValue ? mvalue.Value : double.MinValue;
                        mvalue = cfgprovider.MaxValue(_property.Name, ValueIndex);
                        _maxValue = mvalue.HasValue ? mvalue.Value : double.MaxValue;
                    }
                    else
                    {
                        int? maxl = cfgprovider.MaxLength(_property.Name, ValueIndex);
                        _editor.MaxLength = maxl.HasValue ? maxl.Value : 0;
                    }
                }
            }
        }
        /// <summary>
        /// Crear o posicionar el editor de texto / 
        /// Create or reposition the TextBox
        /// </summary>
        /// <param name="left">
        /// Posición izquierda / 
        /// Left position
        /// </param>
        /// <param name="width">
        /// Ancho del editor / 
        /// TextBox width
        /// </param>
        /// <param name="add">
        /// Añadir el control a la lista de controles / 
        /// Add the control to control list
        /// </param>
        protected void CreateEditor(int left, int width, bool add = true)
        {
            if (_editor == null)
            {
                _editor = new TextBox()
                {
                    Name = "txtEditor",
                    CharacterCasing = Case,
                    Multiline = false,
                    Left = left,
                    Width = width,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    PasswordChar = IsPassword ? '*' : '\0'
                };
                _txtForeColor = _editor.ForeColor;
                _editor.TextChanged += new EventHandler(txtEditor_TextChanged);
                _editor.KeyDown += new KeyEventHandler(txtEditor_KeyDown);
                _editor.Enter += new EventHandler(txtEditor_Enter);
                _editor.Leave += new EventHandler(txtEditor_Leave);
                if (add)
                {
                    AddControl(_editor);
                }
            }
            else
            {
                _editor.Left = left;
                _editor.Width = width;
            }
        }
        protected void txtEditor_TextChanged(object sender, EventArgs e)
        {
            _editor.ForeColor = _txtForeColor;
            _dirty = true;
            if (_isNumber)
            {
                double value;
                if (!double.TryParse(_editor.Text, out value))
                {
                    _editor.ForeColor = Color.Red;
                }
                else if ((value < _minValue) || (value > _maxValue))
                {
                    _editor.ForeColor = Color.Red;
                }
            }
        }

        protected void txtEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetValue();
                ParentContainer.PropertyCommited(this);
            }
        }
        protected void txtEditor_Enter(object sender, EventArgs e)
        {
            OnEnter(e);
        }
        protected void txtEditor_Leave(object sender, EventArgs e)
        {
            OnLeave(e);
        }
    }
}

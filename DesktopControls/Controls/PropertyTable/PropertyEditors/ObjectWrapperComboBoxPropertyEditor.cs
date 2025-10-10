using DesktopControls.Controls.PropertyTable.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using System;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedad ObjectWrapper con lista desplegable de posibles valores / 
    /// ObjectWrapper property editor with a dropdown value list
    /// </summary>
    public class ObjectWrapperComboBoxPropertyEditor : ObjectWrapperEditorBase
    {
        protected ComboBox _editor;
        protected bool _disableSetItem = false;
        protected Label _roLabel;
        public ObjectWrapperComboBoxPropertyEditor() : base()
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
                int maxheight = 0;
                if (_property.CanWrite)
                {
                    _editor = new ComboBox()
                    {
                        Name = "cbEditor",
                        Left = Padding.Left,
                        Width = _client.ClientSize.Width - Padding.Horizontal,
                        Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    _editor.SelectedIndexChanged += new EventHandler(cbEditor_SelectedItemChanged);
                    AddControl(_editor);
                    maxheight = _editor.Height;
                    SetStyle();
                }
                else
                {
                    _roLabel = new Label()
                    {
                        Name = "lText",
                        Margin = new Padding(0),
                        AutoSize = true,
                        Left = Padding.Left
                    };
                    AddControl(_roLabel);
                    maxheight = _roLabel.Height;
                    SetStyle();
                }
                // Establecer la altura total del editor
                // Set editor total height
                SetHeight(maxheight);
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
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                object pvalue;
                if (_property.CanWrite)
                {
                    if ((_editor.Items.Count == 0) && (_vProvider != null))
                    {
                        GetAvailableValues();
                    }
                    if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                    {
                        pvalue = vmgr.GetValue(_property.Name, ValueIndex);
                    }
                    else
                    {
                        object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                        pvalue = Property.GetValue(_instance, index);
                    }
                    ObjectWrapper ovalue = pvalue as ObjectWrapper;
                    if ((ovalue == null) && (pvalue != null))
                    {
                        ovalue = _vProvider.WrapObject(pvalue, _property.Name);
                    }
                    if (ovalue != null)
                    {
                        for (int ix = 0; ix < _editor.Items.Count; ix++)
                        {
                            if (((ObjectWrapper)_editor.Items[ix]).Equals(ovalue))
                            {
                                if (_editor.SelectedIndex != ix)
                                {
                                    _disableSetItem = true;
                                    _editor.SelectedIndex = ix;
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                    {
                        pvalue = vmgr.GetValue(_property.Name, ValueIndex);
                    }
                    else
                    {
                        pvalue = Property.GetValue(_instance);
                    }
                    _roLabel.Text = pvalue == null ? "" : pvalue.ToString();
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
            _dirty = false;
            if ((_property != null) && _property.CanWrite && (_instance != null))
            {
                object oldval;
                IndexedPropertyValueManager vmgr = _instance as IndexedPropertyValueManager;
                if ((ValueIndex >= 0) && (vmgr != null) && vmgr.Managed(_property.Name))
                {
                    oldval = vmgr.GetValue(_property.Name, ValueIndex);
                    ObjectWrapper wovalue = oldval as ObjectWrapper;
                    if ((wovalue == null) && (oldval != null))
                    {
                        wovalue = _vProvider.WrapObject(oldval, _property.Name);
                    }
                    if ((_editor.SelectedItem == null) || !_editor.SelectedItem.Equals(wovalue))
                    {
                        if (_editor.SelectedItem != null)
                        {
                            ObjectWrapper owp = _editor.SelectedItem as ObjectWrapper;
                            if (_property.PropertyType == typeof(ObjectWrapper))
                            {
                                vmgr.SetValue(_property.Name, owp, ValueIndex);
                            }
                            else
                            {
                                vmgr.SetValue(_property.Name, owp.Implementation(), ValueIndex);
                            }
                        }
                        else
                        {
                            vmgr.SetValue(_property.Name, null, ValueIndex);
                        }
                        OnPropertyChanged(oldval);
                    }
                }
                else
                {
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    oldval = Property.GetValue(_instance, index);
                    ObjectWrapper wovalue = oldval as ObjectWrapper;
                    if ((wovalue == null) && (oldval != null))
                    {
                        wovalue = _vProvider.WrapObject(oldval, _property.Name);
                    }
                    if ((_editor.SelectedItem == null) || !_editor.SelectedItem.Equals(wovalue))
                    {
                        if (_editor.SelectedItem != null)
                        {
                            ObjectWrapper owp = _editor.SelectedItem as ObjectWrapper;
                            if (_property.PropertyType == typeof(ObjectWrapper))
                            {
                                _property.SetValue(_instance, owp, index);
                            }
                            else
                            {
                                _property.SetValue(_instance, owp.Implementation(), index);
                            }
                        }
                        else
                        {
                            _property.SetValue(_instance, null, index);
                        }
                        OnPropertyChanged(oldval);
                    }
                }
            }
        }
        /// <summary>
        /// Lista de valores disponibles para la propiedad / 
        /// Property available values list
        /// </summary>
        protected override void GetAvailableValues()
        {
            if (_property.CanWrite)
            {
                _editor.Items.Clear();
                foreach (ObjectWrapper owr in _vProvider.GetAllowedValues(_property.Name))
                {
                    _editor.Items.Add(owr);
                }
            }
        }
        /// <summary>
        /// Cambio del valor seleccionado / 
        /// Change selected value
        /// </summary>
        protected virtual void cbEditor_SelectedItemChanged(object sender, EventArgs e)
        {
            _selectedItem = _editor.SelectedItem as ObjectWrapper;
            if (!_disableSetItem)
            {
                SetValue();
            }
            _disableSetItem = false;
        }
    }
}

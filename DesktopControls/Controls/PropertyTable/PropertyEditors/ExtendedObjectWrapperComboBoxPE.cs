using DesktopControls.Controls.PropertyTable.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de objetos ObjectWrapper con botones de nuevo y eliminar  / 
    /// ObjectWrapper editor with new and delete buttons
    /// </summary>
    public class ExtendedObjectWrapperComboBoxPE : ObjectWrapperComboBoxPropertyEditor
    {
        protected Button _btnNew = null;
        protected Button _btnDelete = null;
        public ExtendedObjectWrapperComboBoxPE()
        {
            _style = PropertyEditionStyle.Button;
        }
        /// <summary>
        /// Establecer el estilo de uso del editor
        /// Set the editor use style
        /// </summary>
        protected override void SetStyle()
        {
            base.SetStyle();
            if (_property != null)
            {
                int maxheight = 0;
                if (_property.CanWrite)
                {
                    if (_editor == null)
                    {
                        _editor = new ComboBox()
                        {
                            Name = "cbEditor",
                            Left = 54 + Padding.Left,
                            Width = _client.ClientSize.Width - (Padding.Horizontal + 54),
                            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left,
                            Margin = new Padding(0),
                            Padding = new Padding(0),
                            DropDownStyle = ComboBoxStyle.DropDownList
                        };
                        _editor.SelectedIndexChanged += new EventHandler(cbEditor_SelectedItemChanged);
                        AddControl(_editor);
                    }
                    else
                    {
                        _editor.Left = 54 + Padding.Left;
                        _editor.Top = Top = Math.Max(0, (Height - 24) / 2);
                        _editor.Width = _client.ClientSize.Width - (54 + Padding.Horizontal);
                    }
                    maxheight = _editor.Height;
                }
                else
                {
                    if (_roLabel == null)
                    {
                        _roLabel = new Label()
                        {
                            Name = "lText",
                            Margin = new Padding(0),
                            AutoSize = true,
                            Left = 54 + Padding.Left,
                            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left
                        };
                        AddControl(_roLabel);
                    }
                    else
                    {
                        _roLabel.Left = 54 + Padding.Left;
                        _roLabel.Anchor = Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
                    }
                    maxheight = _roLabel.Height;
                }
                switch (EditionStyle & (PropertyEditionStyle.Button | PropertyEditionStyle.Left))
                {
                    case PropertyEditionStyle.Button | PropertyEditionStyle.Left:
                        if (_btnNew == null)
                        {
                            _btnNew = new Button()
                            {
                                Name = "btnNew",
                                Image = ICO_New.ToBitmap(),
                                Left = Padding.Left,
                                Height = Math.Max(24, maxheight),
                                Width = 24,
                                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                                Margin = new Padding(0),
                                Padding = new Padding(0),
                                ImageAlign = ContentAlignment.MiddleCenter
                            };
                            _btnNew.Click += new EventHandler(btnNew_Click);
                        }
                        else
                        {
                            _btnNew.Left = Padding.Left;
                            _btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        }
                        if (_btnDelete == null)
                        {
                            _btnDelete = new Button()
                            {
                                Name = "btnDelete",
                                Image = ICO_Delete.ToBitmap(),
                                Left = Padding.Left + 26,
                                Height = Math.Max(24, maxheight),
                                Width = 24,
                                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                                Margin = new Padding(0),
                                Padding = new Padding(0),
                                ImageAlign = ContentAlignment.MiddleCenter,
                            };
                            _btnDelete.Click += new EventHandler(btnDelete_Click);
                        }
                        else
                        {
                            _btnDelete.Left = Padding.Left + 26;
                            _btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        }
                        if (!ContainsControl(_editor))
                        {
                            AddControl(_editor);
                        }
                        if (!ContainsControl(_btnDelete))
                        {
                            AddControl(_btnDelete);
                        }
                        if (!ContainsControl(_btnNew))
                        {
                            AddControl(_btnNew);
                        }
                        maxheight = Math.Max(_btnNew.Height, maxheight);
                        break;
                    case PropertyEditionStyle.Button:
                        if (_btnNew == null)
                        {
                            _btnNew = new Button()
                            {
                                Name = "btnNew",
                                Image = ICO_New.ToBitmap(),
                                Left = Width - (50 + Padding.Right),
                                Height = 24,
                                Width = 24,
                                ImageAlign = ContentAlignment.MiddleCenter,
                                Anchor = AnchorStyles.Top | AnchorStyles.Right
                            };
                            _btnNew.Click += new EventHandler(btnNew_Click);
                        }
                        else
                        {
                            _btnNew.Left = Width - (50 + Padding.Right);
                            _btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        }
                        if (_btnDelete == null)
                        {
                            _btnDelete = new Button()
                            {
                                Name = "btnDelete",
                                Image = ICO_Delete.ToBitmap(),
                                Enabled = _editor.SelectedItem != null,
                                Left = Width - (24 + Padding.Right),
                                Height = 24,
                                Width = 24,
                                ImageAlign = ContentAlignment.MiddleCenter,
                                Anchor = AnchorStyles.Top | AnchorStyles.Right
                            };
                            _btnDelete.Click += new EventHandler(btnDelete_Click);
                        }
                        else
                        {
                            _btnDelete.Left = Width - (24 + Padding.Right);
                            _btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        }
                        if (!ContainsControl(_editor))
                        {
                            AddControl(_editor);
                        }
                        if (!ContainsControl(_btnDelete))
                        {
                            AddControl(_btnDelete);
                        }
                        if (!ContainsControl(_btnNew))
                        {
                            AddControl(_btnNew);
                        }
                        maxheight = Math.Max(_btnNew.Height, maxheight);
                        break;
                    default:
                        if ((_btnNew != null) && ContainsControl(_btnNew))
                        {
                            RemoveControl(_btnNew);
                        }
                        if ((_btnDelete != null) && ContainsControl(_btnDelete))
                        {
                            RemoveControl(_btnNew);
                        }
                        if (!ContainsControl(_editor))
                        {
                            AddControl(_editor);
                        }
                        break;
                }
                SetHeight(maxheight);
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (_instance is IPropertyCommandManager)
            {
                PropertyCommandEventArgs args = new PropertyCommandEventArgs()
                {
                    PropertyName = _property.Name,
                    CommandIndex = 0
                };
                ((IPropertyCommandManager)_instance).PropertyCommand(this, args);
                if (!args.Cancel)
                {
                    GetAvailableValues();
                    ObjectWrapper ow = args.CommandResult as ObjectWrapper;
                    if (ow == null)
                    {
                        ow = ((IValueSelectionListProvider)Instance).WrapObject(args.CommandResult, _property.Name);
                    }
                    if (_editor != null)
                    {
                        _editor.SelectedItem = ow;
                    }
                    else if (_roLabel != null)
                    {
                        _roLabel.Text = ow.FriendlyName;
                    }
                }
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (_instance is IPropertyCommandManager)
            {
                PropertyCommandEventArgs args = new PropertyCommandEventArgs()
                {
                    PropertyName = _property.Name,
                    CommandIndex = 1
                };
                ((IPropertyCommandManager)_instance).PropertyCommand(this, args);
                if (!args.Cancel)
                {
                    GetAvailableValues();
                    GetValue();
                }
            }
        }
        /// <summary>
        /// Cambio del valor seleccionado / 
        /// Change selected value
        /// </summary>
        protected override void cbEditor_SelectedItemChanged(object sender, EventArgs e)
        {
            base.cbEditor_SelectedItemChanged(sender, e);
            if (_btnDelete != null)
            {
                _btnDelete.Enabled = _editor.SelectedItem != null;
            }
        }
    }
}

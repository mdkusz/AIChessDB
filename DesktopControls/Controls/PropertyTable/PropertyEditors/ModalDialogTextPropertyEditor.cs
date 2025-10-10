using GlobalCommonEntities.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de cadenas de texto con selección de usuario / 
    /// text string editor with user selection
    /// </summary>
    public class ModalDialogTextPropertyEditor : TextPropertyEditor
    {
        protected Button _btnEdit = null;
        protected string _filter;

        public ModalDialogTextPropertyEditor() : base()
        {
            _style = PropertyEditionStyle.Button;
        }
        /// <summary>
        /// Establecer el estilo de uso del editor / 
        /// Set the editor use style
        /// </summary>
        protected override void SetStyle()
        {
            base.SetStyle();
            bool ronly = false;
            if ((_property != null) && (_instance is IPropertyCommandManager))
            {
                ronly = !((IPropertyCommandManager)_instance).TextReadOnly(_property.Name);
            }
            if ((_property == null) || _property.CanWrite)
            {
                switch (EditionStyle & (PropertyEditionStyle.Button | PropertyEditionStyle.Left))
                {
                    case PropertyEditionStyle.Button | PropertyEditionStyle.Left:
                        CreateEditor(26 + Padding.Left, _client.ClientSize.Width - (26 + Padding.Horizontal));
                        _editor.Top = Top = Math.Max(0, (24 - Height) / 2);
                        CreateButton(Padding.Left, 24, AnchorStyles.Top | AnchorStyles.Left);
                        break;
                    case PropertyEditionStyle.Button:
                        CreateEditor(Padding.Left, _client.ClientSize.Width - (26 + Padding.Horizontal));
                        _editor.Top = Top = Math.Max(0, (24 - Height) / 2);
                        CreateButton(Width - (24 + Padding.Horizontal), 24, AnchorStyles.Top | AnchorStyles.Right);
                        break;
                    default:
                        if ((_btnEdit != null) && ContainsControl(_btnEdit))
                        {
                            RemoveControl(_btnEdit);
                        }
                        CreateEditor(Padding.Left, _client.ClientSize.Width - Padding.Horizontal);
                        break;
                }
                _editor.ReadOnly = ronly;
                SetHeight(Math.Max(_editor.Height, _btnEdit.Height));
            }
        }
        /// <summary>
        /// Crear o posicionar el botón de comando / 
        /// Create or reposition the command button
        /// </summary>
        /// <param name="left">
        /// Posición izquierda o derecha / 
        /// Left or right position
        /// </param>
        /// <param name="width">
        /// Ancho del editor / 
        /// TextBox width
        /// </param>
        /// <param name="anchor">
        /// Anclado del botón / 
        /// Button anchor style
        /// </param>
        /// <param name="add">
        /// Añadir el control a la lista de controles / 
        /// Add the control to control list
        /// </param>
        protected void CreateButton(int left, int width, AnchorStyles anchor, bool add = true)
        {
            if (_btnEdit == null)
            {
                _btnEdit = new Button()
                {
                    Name = "btnEdit",
                    Image = ICO_FindFile.ToBitmap(),
                    Left = left,
                    Height = Math.Max(24, _editor.Height),
                    Width = width,
                    Anchor = anchor,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    ImageAlign = ContentAlignment.MiddleCenter
                };
                _btnEdit.Click += new EventHandler(btnEdit_Click);
                if (add)
                {
                    AddControl(_btnEdit);
                }
            }
            else
            {
                _btnEdit.Anchor = anchor;
                _btnEdit.Left = left;
            }
        }
        /// <summary>
        /// Invocación al cuadro de diálogo para selección del usuario / 
        /// Invoke user selection dialog box
        /// </summary>
        protected override void EditorInvoked()
        {
            base.EditorInvoked();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
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
                    object obj = _property.GetValue(_instance);
                    string vstr = "";
                    if (obj != null)
                    {
                        vstr = obj.ToString();
                    }
                    if (vstr != _editor.Text)
                    {
                        _editor.Text = vstr;
                    }
                }
            }
        }
    }
}

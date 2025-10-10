using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedades que representan nombres de recursos incrustados / 
    /// Property editor for properties representing embedded resources names
    /// </summary>
    public class ResourcePropertyEditor : TextPropertyEditor, IAppResourcesConsumer
    {
        protected PictureBox _resourceImage;
        protected Label _resourceText;
        protected Panel _panel;
        public ResourcePropertyEditor() : base()
        {
            _style = PropertyEditionStyle.Text;
        }
        /// <summary>
        /// IAppResourcesConsumer: Repositorio de recursos incrustados de la aplicación / 
        /// IAppResourcesConsumer: Application embedded resources repository
        /// </summary>
        public ResourcesRepository AllResources
        {
            get
            {
                return ParentContainer?.AllResources;
            }
            set { }
        }
        /// <summary>
        /// Establecer el estilo de uso del editor / 
        /// Set the editor use style
        /// </summary>
        protected override void SetStyle()
        {
            base.SetStyle();
            bool ronly = false;
            if ((_property == null) || _property.CanWrite)
            {
                switch (EditionStyle & (PropertyEditionStyle.Text | PropertyEditionStyle.Image | PropertyEditionStyle.Left))
                {
                    case PropertyEditionStyle.Text | PropertyEditionStyle.Left:
                    case PropertyEditionStyle.Text:
                        CreateEditor(Padding.Left, _client.ClientSize.Width - Padding.Horizontal);
                        _editor.Top = Top = 0;
                        CreateLabel(Padding.Left);
                        SetHeight(_resourceText.Bottom);
                        break;
                    case PropertyEditionStyle.Image | PropertyEditionStyle.Left:
                        CreateEditor(26 + Padding.Left, _client.ClientSize.Width - (26 + Padding.Horizontal));
                        _editor.Top = Top = Math.Max(0, (24 - Height) / 2);
                        CreateImage(Padding.Left, 24, AnchorStyles.Top | AnchorStyles.Left);
                        SetHeight(Math.Max(_resourceImage.Height, _editor.Height));
                        break;
                    case PropertyEditionStyle.Image:
                        CreateEditor(Padding.Left, _client.ClientSize.Width - (26 + Padding.Horizontal));
                        _editor.Top = Top = Math.Max(0, (24 - Height) / 2);
                        CreateImage(Width - (24 + Padding.Horizontal), 24, AnchorStyles.Top | AnchorStyles.Right);
                        SetHeight(Math.Max(_resourceImage.Height, _editor.Height));
                        break;
                    default:
                        if ((_resourceImage != null) && ContainsControl(_resourceImage))
                        {
                            RemoveControl(_resourceImage);
                            _resourceImage = null;
                        }
                        if ((_resourceText != null) && ContainsControl(_resourceText))
                        {
                            RemoveControl(_resourceText);
                            _resourceText = null;
                        }
                        CreateEditor(Padding.Left, _client.ClientSize.Width - Padding.Horizontal);
                        SetHeight(_editor.Height);
                        break;
                }
                _editor.ReadOnly = ronly;
            }
        }
        /// <summary>
        /// Obtener el valor de la propiedad / 
        /// Get property value
        /// </summary>
        protected override void GetValue()
        {
            base.GetValue();
            if (AllResources != null)
            {
                string text = "";
                if (_property.CanWrite)
                {
                    text = _editor.Text;
                }
                else
                {
                    text = _roLabel.Text;
                }
                if ((EditionStyle & PropertyEditionStyle.Text) != PropertyEditionStyle.None)
                {
                    if (_resourceText != null)
                    {
                        _resourceText.Text = AllResources.GetString(text);
                    }
                }
                else if ((EditionStyle & PropertyEditionStyle.Image) != PropertyEditionStyle.None)
                {
                    if (_resourceImage != null)
                    {
                        object img = AllResources.GetObject(text);
                        Bitmap bmp = new Bitmap(16, 16);
                        using (Graphics gr = Graphics.FromImage(bmp))
                        {
                            if (img is Icon)
                            {
                                gr.DrawIcon((Icon)img, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                _resourceImage.Image = bmp;
                            }
                            else if (img is Bitmap)
                            {
                                gr.DrawImage((Bitmap)img, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                _resourceImage.Image = bmp;
                            }
                            else
                            {
                                _resourceImage.Image = null;
                            }
                        }
                    }
                }
                else
                {
                    if (_resourceImage != null)
                    {
                        _resourceImage.Image = null;
                    }
                    if (_resourceText != null)
                    {
                        _resourceText.Text = "";
                    }
                }
            }
        }
        /// <summary>
        /// Crear o posicionar la etiqueta con el recurso traducido / 
        /// Create or reposition the translated resource label
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
        /// Anclado de la etiqueta / 
        /// Label anchor style
        /// </param>
        /// <param name="add">
        /// Añadir el control a la lista de controles / 
        /// Add the control to control list
        /// </param>
        protected void CreateLabel(int left, bool add = true)
        {
            if (_resourceText == null)
            {
                _resourceText = new Label()
                {
                    Name = "resourceText",
                    Left = left,
                    AutoSize = true,
                    Top = _editor.Bottom + 2,
                    Font = new Font(Font.Name, Font.Size - 1, FontStyle.Bold | FontStyle.Italic),
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                if (add)
                {
                    AddControl(_resourceText);
                }
            }
            else
            {
                _resourceText.Left = left;
            }
        }
        /// <summary>
        /// Crear o posicionar la imagen que representa el recurso / 
        /// Create or reposition the resource image
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
        /// Anclado de la imagen / 
        /// Image anchor style
        /// </param>
        /// <param name="add">
        /// Añadir el control a la lista de controles / 
        /// Add the control to control list
        /// </param>
        protected void CreateImage(int left, int width, AnchorStyles anchor, bool add = true)
        {
            if (_resourceImage == null)
            {
                _resourceImage = new PictureBox()
                {
                    Name = "resourceImage",
                    Left = left,
                    Height = Math.Max(24, _editor.Height),
                    Width = width,
                    Anchor = anchor,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    SizeMode = PictureBoxSizeMode.CenterImage
                };
                if (add)
                {
                    AddControl(_resourceImage);
                }
            }
            else
            {
                _resourceImage.Anchor = anchor;
                _resourceImage.Left = left;
            }
        }
    }
}

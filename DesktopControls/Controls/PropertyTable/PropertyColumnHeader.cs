using DesktopControls.Controls.PropertyTable.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls.PropertyTable
{
    /// <summary>
    /// Encabezado de lista de editores de propiedades
    /// Property editor list header
    /// </summary>
    public class PropertyColumnHeader : Panel, IPropertyBlockHeader
    {
        protected Label lHeader = new Label();
        protected Button bCollapse = new Button();
        protected ImageList bImageList = new ImageList();
        public PropertyColumnHeader()
        {
            Dock = DockStyle.Left;
            lHeader.Dock = DockStyle.Fill;
            lHeader.Location = new Point(0, 0);
            lHeader.Name = "lHeader";
            lHeader.Height = 20;
            lHeader.TabIndex = 0;
            lHeader.Text = "Header";
            lHeader.TextAlign = ContentAlignment.MiddleCenter;
            bImageList.TransparentColor = Color.Magenta;
            bImageList.Images.Add("Collapse_small.bmp", Collapse_small);
            bImageList.Images.Add("Collapsed.bmp", Collapsed_small);
            bCollapse.BackColor = Color.Transparent;
            bCollapse.Dock = DockStyle.Left;
            bCollapse.FlatStyle = FlatStyle.Flat;
            bCollapse.FlatAppearance.BorderSize = 0;
            bCollapse.ImageIndex = 0;
            bCollapse.ImageList = bImageList;
            bCollapse.Location = new Point(0, 0);
            bCollapse.Name = "bCollapse";
            bCollapse.Size = new Size(20, 20);
            bCollapse.TabIndex = 1;
            bCollapse.UseVisualStyleBackColor = false;
            Controls.Add(lHeader);
            Controls.Add(bCollapse);
            bCollapse.Click += new EventHandler(bCollapse_Click);
        }
        /// <summary>
        /// Posición del texto
        /// Caption aligment
        /// </summary>
        public ContentAlignment Alignment
        {
            get
            {
                return lHeader.TextAlign;
            }
            set
            {
                lHeader.TextAlign = value;
            }
        }
        /// <summary>
        /// IPropertyBlockHeader: Título de la cabecera
        /// IPropertyBlockHeader: Header title
        /// </summary>
        public string Title
        {
            get
            {
                return lHeader.Text;
            }
            set
            {
                lHeader.Text = value;
            }
        }
        /// <summary>
        /// IPropertyBlockHeader: Colapsar o expandir el contenido del bloque
        /// IPropertyBlockHeader: Collapse or expand block contents
        /// </summary>
        public bool Collapsed
        {
            get
            {
                return bCollapse.ImageIndex != 0;
            }
            set
            {
                bCollapse.ImageIndex = value ? 1 : 0;
            }
        }
        /// <summary>
        /// IPropertyBlockHeader: Bloque contenedor de la cabecera
        /// IPropertyBlockHeader: Header block container
        /// </summary>
        public IPropertyBlockContainer ParentContainer { get; set; }

        private void bCollapse_Click(object sender, EventArgs e)
        {
            Collapsed = !Collapsed;
            ParentContainer?.Collapse(Collapsed);
        }
    }
}

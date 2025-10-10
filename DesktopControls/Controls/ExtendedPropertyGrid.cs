using DesktopControls.PropertyTools;
using GlobalCommonEntities.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Extensión del control property grid / 
    /// Property grid control extension
    /// </summary>
    public partial class ExtendedPropertyGrid : UserControl, IPropertyEditorContainer
    {
        public ExtendedPropertyGrid()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Se dispara al aignar un nuevo objeto al control / 
        /// Fires when a new object is assigned
        /// </summary>
        public event EventHandler SelectedObjectChanged;
        /// <summary>
        /// La propiedad activa actual en el PropertyGrid ha cambiado / 
        /// The current active property in the PropertyGrid has changed
        /// </summary>
        public event SelectedGridItemChangedEventHandler SelectedGridItemChanged;
        /// <summary>
        /// Se dispara cuando cambia el contenido o tamaño del control / 
        /// Fires when the control content or size changes
        /// </summary>
        public event PropertyEditorChangedEventHandler EditorSizeChanged;
        /// <summary>
        /// Evento que se dispara cuando se cambia el tamaó del panel de description / 
        /// Event fired when the description panel size changes
        /// </summary>
        public event EventHandler CommentSizeChanged;
        /// <summary>
        /// El valor de la propiedad actual ha cambiado / 
        /// The current property value has changed
        /// </summary>
        public event PropertyValueChangedEventHandler PropertyValueChanged;
        /// <summary>
        /// Se dispara al cambiar el contenido o tamaño del control / 
        /// Fires when the control content or size changes
        /// </summary>
        public event EventHandler ContentChanged;
        public override void Refresh()
        {
            base.Refresh();
            PropertyEditor.Refresh();
        }
        /// <summary>
        /// Control PropertyGrid / 
        /// ProperyGrid control
        /// </summary>
        public TabPropertyGrid PropertyEditor
        {
            get
            {
                return pgProperties;
            }
        }
        /// <summary>
        /// Acceso directo a la propiedad SelectedObject de PropertyGrid / 
        /// PropertyGrid SelectedObject property direct access
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return pgProperties.SelectedObject;
            }
            set
            {
                pgProperties.SelectedObject = value;
            }
        }
        /// <summary>
        /// Acceso directo a la propiedad SelectedObjects de PropertyGrid / 
        /// PropertyGrid SelectedObjects property direct access
        /// </summary>
        public object[] SelectedObjects
        {
            get
            {
                return pgProperties.SelectedObjects;
            }
            set
            {
                pgProperties.SelectedObjects = value;
            }
        }
        /// <summary>
        /// Superpone un panel de descripción a medida para la propiedad seleccionada / 
        /// Overlays a custom description panel for the selected property
        /// </summary>
        /// <param name="gi">
        /// Item que contiene el descriptor de la propiedad  / 
        /// Item containing the property descriptor
        /// </param>
        private void ShowExtendedComment(GridItem gi)
        {
            if (gi != null)
            {
                IUIIdentifier uid = gi.Value as IUIIdentifier;
                if ((uid != null) && !string.IsNullOrEmpty(uid.FriendlyDescription))
                {
                    Rectangle rc = pgProperties.GetCommentRectangle();
                    pExtendedDoc.Location = rc.Location;
                    pExtendedDoc.Size = rc.Size;
                    lTitle.Text = gi.Label + ": " + uid.FriendlyName;
                    lComment.Text = uid.FriendlyDescription;
                    pExtendedDoc.Visible = true;
                }
                else
                {
                    pExtendedDoc.Visible = false;
                }
            }
        }

        private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ShowExtendedComment(e.ChangedItem);
            PropertyValueChanged?.Invoke(this, e);
        }

        private void pgProperties_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            ShowExtendedComment(e.NewSelection);
            SelectedGridItemChanged?.Invoke(this, e);
        }

        private void pgProperties_SizeChanged(object sender, EventArgs e)
        {
            ShowExtendedComment(pgProperties.SelectedGridItem);
        }

        private void pgProperties_Layout(object sender, LayoutEventArgs e)
        {
            if ((e.AffectedComponent != null) && e.AffectedComponent.GetType().Name.EndsWith("DocComment"))
            {
                ShowExtendedComment(pgProperties.SelectedGridItem);
            }
            ContentChanged?.Invoke(this, e);
        }

        private void pgProperties_SelectedObjectsChanged(object sender, EventArgs e)
        {
            SelectedObjectChanged?.Invoke(this, e);
        }

        private void pgProperties_EditorSizeChanged(object sender, PropertyEditorChangedEventArgs e)
        {
            EditorSizeChanged?.Invoke(this, e);
        }

        private void pgProperties_CommentSizeChanged(object sender, EventArgs e)
        {
            CommentSizeChanged?.Invoke(this, e);
        }
    }
}

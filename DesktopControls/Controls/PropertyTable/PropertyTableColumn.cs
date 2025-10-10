using DesktopControls.Controls.PropertyTable.Attributes;
using DesktopControls.Controls.PropertyTable.Interfaces;
using DesktopControls.Controls.PropertyTable.PropertyEditors;
using DesktopControls.PropertyTools;
using GlobalCommonEntities.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable
{
    /// <summary>
    /// Bloque de editores de propiedades
    /// Property editor block
    /// </summary>
    public class PropertyTableColumn : PropertyContainerPanel, IPropertyBlockContainer, IComparable<PropertyTableColumn>, IEquatable<PropertyTableColumn>
    {
        protected IPropertyBlockHeader _header;
        protected IPropertyBlockContainer _parent;
        protected List<PropertyEditorBase> _properties;
        public PropertyTableColumn()
        {
            AutoScroll = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Width = 100;
        }
        /// <summary>
        /// Cuenta de propiedades
        /// Property count
        /// </summary>
        public int Count
        {
            get
            {
                return _properties == null ? 0 : _properties.Count;
            }
        }
        /// <summary>
        /// Altura necesaria para visualizar todos los editores
        /// Height needed to view all the editors
        /// </summary>
        public int ContentHeight
        {
            get
            {
                int height = headerPanel.Controls.Count > 0 ? headerPanel.Height : 0;
                foreach (Control c in propertyCollectionPanel.Controls)
                {
                    Size sz = c.GetPreferredSize(c.Size);
                    height += sz.Height;
                }
                return height;
            }
        }
        /// <summary>
        /// Cabecera de la columna
        /// Column header
        /// </summary>
        public IPropertyBlockHeader Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                headerPanel.SuspendLayout();
                headerPanel.Controls.Clear();
                if (_header != null)
                {
                    headerPanel.Visible = true;
                    headerPanel.Height = (_header as Control).Height;
                    (_header as Control).Dock = DockStyle.Fill;
                    headerPanel.Controls.Add(_header as Control);
                    _header.ParentContainer = this;
                }
                else
                {
                    headerPanel.Visible = false;
                }
                headerPanel.TabStop = false;
                headerPanel.ResumeLayout(false);
            }
        }
        /// <summary>
        /// Índice de orden horizontal del bloque de propiedades
        /// Horizontal order index of the property block
        /// </summary>
        public int ColumnIndex { get; set; }
        /// <summary>
        /// Índice de la fila a la que pertenece el bloque de propiedades
        /// Property block row index
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// Título del bloque de propiedades
        /// Property block title
        /// </summary>
        public string Title
        {
            get
            {
                if (_header != null)
                {
                    return _header.Title;
                }
                return null;
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Pasar el foco a la siguiente propiedad al terminar la edición / 
        /// IPropertyBlockContainer: Move focus to the next property when editing the edition
        /// </summary>
        public bool AutoTab { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Configuración de encabezados de columna
        /// IPropertyBlockContainer: Column headers settings
        /// </summary>
        public int HeadersHeight { get; set; }
        public Font HeadersFont { get; set; }
        public Color HeadersBackColor { get; set; }
        public Color HeadersForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Fuente para los editores de propiedades
        /// IPropertyBlockContainer: Property editors font
        /// </summary>
        public Font EditorsFont { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo de los editores de propiedades
        /// IPropertyBlockContainer: Property editors background color
        /// </summary>
        public Color EditorsBackColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de primer plano de los editores de propiedades
        /// IPropertyBlockContainer: Property editors foreground color
        /// </summary>
        public Color EditorsForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo de las propiedades desplegables
        /// IPropertyBlockContainer: Expandable properties background color
        /// </summary>
        public Color ExpandableBackColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de primer plano de las propiedades desplegables
        /// IPropertyBlockContainer: Expandable properties foreground color
        /// </summary>
        public Color ExpandableForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo del editor cuando está seleccionado
        /// IPropertyBlockContainer: Selected property editor background color
        /// </summary>
        public Color EditorsSelectedBackColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de texto del editor cuando está seleccionado
        /// IPropertyBlockContainer: Selected property editor text color
        /// </summary>
        public Color EditorsSelectedForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de los bordes de los editores de propiedades
        /// IPropertyBlockContainer: Property editors border colors
        /// </summary>
        public Color EditorsTopBorderColor { get; set; }
        public Color EditorsLeftBorderColor { get; set; }
        public Color EditorsBottomBorderColor { get; set; }
        public Color EditorsRightBorderColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Tamaño del contorno de los editores de propiedades
        /// IPropertyBlockContainer: Property editors surround size
        /// </summary>
        public Padding EditorsDefaultPadding { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Tamaño del borde de los editores de propiedades
        /// IPropertyBlockContainer: Property editors border size
        /// </summary>
        public Padding EditorsBorderSize { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Estilo por defecto de los editores de propiedades
        /// IPropertyBlockContainer: Property editors default style
        /// </summary>
        public PropertyEditionStyle EditionStyle { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Respositorio de recursos incrustados / 
        /// IPropertyBlockContainer: Embedded resources repository
        /// </summary>
        public ResourcesRepository AllResources
        {
            get
            {
                return _parent?.AllResources;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Contenedor de nivel superior
        /// IPropertyBlockContainer: High level container
        /// </summary>
        public IPropertyBlockContainer ParentContainer
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                if (value != null)
                {
                    AutoTab = _parent.AutoTab;
                    EditionStyle = _parent.EditionStyle;
                    EditorsFont = _parent.EditorsFont;
                    EditorsBackColor = _parent.EditorsBackColor;
                    EditorsForeColor = _parent.EditorsForeColor;
                    ExpandableBackColor = _parent.ExpandableBackColor;
                    ExpandableForeColor = _parent.ExpandableForeColor;
                    EditorsTopBorderColor = _parent.EditorsTopBorderColor;
                    EditorsLeftBorderColor = _parent.EditorsLeftBorderColor;
                    EditorsBottomBorderColor = _parent.EditorsBottomBorderColor;
                    EditorsRightBorderColor = _parent.EditorsRightBorderColor;
                    EditorsSelectedBackColor = _parent.EditorsSelectedBackColor;
                    EditorsSelectedForeColor = _parent.EditorsSelectedForeColor;
                    EditorsDefaultPadding = _parent.EditorsDefaultPadding;
                    EditorsBorderSize = _parent.EditorsBorderSize;
                    HeadersHeight = _parent.HeadersHeight;
                    HeadersFont = _parent.HeadersFont;
                    HeadersBackColor = _parent.HeadersBackColor;
                    HeadersForeColor = _parent.HeadersForeColor;
                    headerPanel.Height = _parent.HeadersHeight;
                    headerPanel.Font = _parent.HeadersFont;
                    headerPanel.BackColor = _parent.HeadersBackColor;
                    headerPanel.ForeColor = _parent.HeadersForeColor;
                }
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Contenedor de nivel inferior
        /// IPropertyBlockContainer: Low level container
        /// </summary>
        /// <param name="pname">
        /// Nombre de la propiedad buscada
        /// Property searched name 
        /// </param>
        /// <param name="pi">
        /// Información de la propiedad
        /// Property information
        /// </param>
        public IPropertyBlockContainer GetChildContainer(string pname, out PropertyInfo pi)
        {
            pi = null;
            foreach (PropertyEditorBase editor in _properties)
            {
                if (editor.Property.Name == pname)
                {
                    pi = editor.Property;
                    return editor as IPropertyBlockContainer;
                }
            }
            return null;
        }
        /// <summary>
        /// IPropertyBlockContainer: Columna del contenedor
        /// IPropertyBlockContainer: FOContainer column
        /// </summary>
        public PropertyTableColumn AsColumn
        {
            get
            {
                return this;
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguna de las propiedades ha cambiado
        /// IPropertyBlockContainer: Some property has changed
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad
        /// Property editor
        /// </param>
        /// <param name="oldvalue">
        /// Valor de la propiedad antes del cambio
        /// Property value before change
        /// </param>
        public void PropertyChanged(PropertyEditorBase editor, object oldvalue)
        {
            ParentContainer?.PropertyChanged(editor, oldvalue);
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguna de las propiedades ha recibido el foco
        /// IPropertyBlockContainer: Some property has got focus
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad
        /// Property editor
        /// </param>
        public void PropertyGotFocus(PropertyEditorBase editor)
        {
            ParentContainer?.PropertyGotFocus(editor);
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguna de las propiedades ha perdido el foco
        /// IPropertyBlockContainer: Some property has lost focus
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad
        /// Property editor
        /// </param>
        public void PropertyLostFocus(PropertyEditorBase editor)
        {
            ParentContainer?.PropertyLostFocus(editor);
        }
        /// <summary>
        /// IPropertyBlockContainer: La edición de la propiedad ha finalizado. Pasar el foco a la siguiente
        /// IPropertyBlockContainer: Property edition has finished. Move focus to the next property
        /// </summary>
        /// <param name="editor">
        /// Editor que ha finalizado la edición
        /// Finioshed editor
        /// </param>
        /// <param name="colindex">
        /// Índice de la columna
        /// Column index
        /// </param>
        /// <param name="rowindex">
        /// Índice de la fila
        /// Row index
        /// </param>
        public void PropertyCommited(PropertyEditorBase editor, int colindex = 0, int rowindex = 0)
        {
            if (AutoTab)
            {
                if (editor.Index == _properties.Count - 1)
                {
                    _parent.PropertyCommited(editor, ColumnIndex, RowIndex);
                }
                else
                {
                    _properties[editor.Index + 1].Focus();
                }
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: El contenedor debe colapsar o expandir su contenido
        /// IPropertyBlockContainer: The container must collapse or expand its content
        /// </summary>
        /// <param name="collapse">
        /// true para colapsar contenido
        /// true to collapse content
        /// </param>
        public void Collapse(bool collapse)
        {
            ArrangeProperties();
            ContentSizeChanged(this, collapse ? PropertyEditorChangedReason.Collapsed : PropertyEditorChangedReason.Expanded);
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguno de los elementos ha cambiado de tamaño
        /// IPropertyBlockContainer: Some element size has changed
        /// </summary>
        /// <param name="changedItem">
        /// Item que ha cambiado de tamaño / 
        /// Resized item
        /// </param>
        /// <param name="reason">
        /// Razón del cambio de tamaño / 
        /// Size change reason
        /// </param>
        public void ContentSizeChanged(object changedItem, PropertyEditorChangedReason reason)
        {
            ParentContainer?.ContentSizeChanged(changedItem, reason);
        }
        /// <summary>
        /// Cambia el foco a una propiedad
        /// Change the focus to a given property
        /// </summary>
        /// <param name="index">
        /// Índice de la propiedad
        /// Property index
        /// </param>
        public virtual void FocusProperty(int index)
        {
            try
            {
                _properties[index].Focus();
            }
            catch { }
        }
        /// <summary>
        /// Añadir una nueva propiedad al bloque
        /// Add a new property to the block
        /// </summary>
        /// <param name="pi">
        /// Objecto PropertyInfo de acceso a la propiedad
        /// PropertyInfo object to access the property
        /// </param>
        /// <param name="instance">
        /// Instancia del objeto propietario de la propiedad
        /// Property owner instance
        /// </param>
        /// <param name="index">
        /// Índice del valor de la propiedad si se trata a una colección
        /// Property value index when it is a collection
        /// </param>
        /// <returns>
        /// Control editor de la propiedad
        /// Property editor control
        /// </returns>
        public PropertyEditorBase AddProperty(PropertyInfo pi, object instance, int index = -1)
        {
            PropertyEditorAttribute pe = pi.GetCustomAttribute<PropertyEditorAttribute>();
            IPropertyConfigurationProvider cfgprovider = instance as IPropertyConfigurationProvider;
            PropertyEditorBase editor = null;
            if (cfgprovider != null)
            {
                editor = cfgprovider.Editor(pi.Name, EditorsFont, index);
            }
            if ((editor == null) && (pe != null))
            {
                editor = pe.Editor;
            }
            if (editor == null)
            {
                List<Type> texttypes = new List<Type>()
                {
                    typeof(string), typeof(byte), typeof(char), typeof(int), typeof(uint), typeof(short), typeof(ushort),
                    typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)
                };
                if (pi.PropertyType == typeof(DateTime))
                {
                    editor = new DateTimePropertyEditor();
                }
                else if (texttypes.Contains(pi.PropertyType))
                {
                    editor = new TextPropertyEditor();
                }
                else if (pi.PropertyType == typeof(bool))
                {
                    editor = new CheckPropertyEditor();
                }
                else
                {
                    editor = new ObjectWrapperComboBoxPropertyEditor();
                }
            }
            editor.SuspendLayout();
            editor.IsPassword = (pe == null) ? false : pe.IsPassword;
            editor.Font = EditorsFont;
            editor.ForeColor = ((pe == null) || pe.EditorForeColor == Color.Empty) ? EditorsForeColor : pe.EditorForeColor;
            editor.BackColor = ((pe == null) || pe.EditorBackColor == Color.Empty) ? EditorsBackColor : pe.EditorBackColor;
            editor.EditionStyle = ((pe == null) || pe.EditorStyle == PropertyEditionStyle.None) ? EditionStyle : pe.EditorStyle;
            editor.ParentContainer = this;
            editor.Instance = instance;
            editor.Property = pi;
            editor.Padding = ((pe == null) || pe.EditorPadding == Padding.Empty) ? EditorsDefaultPadding : pe.EditorPadding;
            editor.BorderSize = ((pe == null) || pe.EditorBorder == Padding.Empty) ? EditorsBorderSize : pe.EditorBorder;
            editor.TopBorderColor = ((pe == null) || pe.TopBorderColor == Color.Empty) ? EditorsTopBorderColor : pe.TopBorderColor;
            editor.LeftBorderColor = ((pe == null) || pe.LeftBorderColor == Color.Empty) ? EditorsLeftBorderColor : pe.LeftBorderColor;
            editor.RightBorderColor = ((pe == null) || pe.RightBorderColor == Color.Empty) ? EditorsRightBorderColor : pe.RightBorderColor;
            editor.BottomBorderColor = ((pe == null) || pe.BottomBorderColor == Color.Empty) ? EditorsBottomBorderColor : pe.BottomBorderColor;
            editor.CaptionColor = ((pe == null) || pe.EditorForeColor == Color.Empty) ? EditorsForeColor : pe.EditorForeColor;
            if (_properties == null)
            {
                _properties = new List<PropertyEditorBase>();
            }
            _properties.Add(editor);
            editor.TabStop = true;
            editor.ResumeLayout(false);
            return editor;
        }
        /// <summary>
        /// Comprueba si el bloque contiene una propiedad
        /// Check whether the block contains a property
        /// </summary>
        /// <param name="pi">
        /// Objecto PropertyInfo de acceso a la propiedad
        /// PropertyInfo object to access the property
        /// </param>
        /// <returns>
        /// True si el bloque contiene la propiedad
        /// True if the block contains the property
        /// </returns>
        public bool ContainsProperty(PropertyInfo pi)
        {
            foreach (PropertyEditorBase editor in _properties)
            {
                if (editor.Property.Name == pi.Name)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Elimina una propiedad
        /// Remove a property
        /// </summary>
        /// <param name="pi">
        /// Objecto PropertyInfo de acceso a la propiedad
        /// PropertyInfo object to access the property
        /// </param>
        public object RemoveProperty(PropertyInfo pi)
        {
            for (int ix = 0; ix < _properties.Count; ix++)
            {
                if (_properties[ix].Property.Name == pi.Name)
                {
                    propertyCollectionPanel.Controls.Remove(_properties[ix]);
                    object instance = _properties[ix].Instance;
                    _properties.RemoveAt(ix);
                    return instance;
                }
            }
            return null;
        }
        /// <summary>
        /// Refresca una propiedad
        /// Refresh a property
        /// </summary>
        /// <param name="pi">
        /// Objecto PropertyInfo de acceso a la propiedad
        /// PropertyInfo object to access the property
        /// </param>
        public void RefreshProperty(PropertyInfo pi)
        {
            for (int ix = 0; ix < _properties.Count; ix++)
            {
                if (_properties[ix].Property.Name == pi.Name)
                {
                    _properties[ix].RefreshValue();
                    return;
                }
            }
        }
        /// <summary>
        /// Refresca la altura de un editor de propiedad
        /// Refresh a property editor height
        /// </summary>
        /// <param name="pi">
        /// Objecto PropertyInfo de acceso a la propiedad
        /// PropertyInfo object to access the property
        /// </param>
        public void RefreshPropertyEditorHeight(PropertyInfo pi)
        {
            for (int ix = 0; ix < _properties.Count; ix++)
            {
                if (_properties[ix].Property.Name == pi.Name)
                {
                    _properties[ix].RefreshHeight();
                    return;
                }
            }
        }
        /// <summary>
        /// Reposiciona las propiedades del bloque
        /// Recalculate the block layout
        /// </summary>
        public void ArrangeProperties()
        {
            if (_properties != null)
            {
                propertyCollectionPanel.SuspendLayout();
                int ixc = 0;
                if ((_header == null) || !_header.Collapsed)
                {
                    _properties.Sort();
                    int ix = 0;
                    while (ix < _properties.Count)
                    {
                        _properties[ix].TabIndex = ix + 1;
                        if (ix == _properties.Count - 1)
                        {
                            // Incrementar el Padding.Bottom para el último editor
                            // Increment last editor Padding.Bottom
                            _properties[ix].Padding = new Padding(EditorsDefaultPadding.Left,
                                EditorsDefaultPadding.Top,
                                EditorsDefaultPadding.Right,
                                EditorsDefaultPadding.Bottom + 7);
                        }
                        if (ixc >= propertyCollectionPanel.Controls.Count)
                        {
                            // Nueva propiedad
                            // New property
                            propertyCollectionPanel.Controls.Add(_properties[ix]);
                            ix++;
                            ixc++;
                        }
                        else
                        {
                            int cmp = string.Compare(((PropertyEditorBase)propertyCollectionPanel.Controls[ixc]).Name.Substring(2), _properties[ix].Property.Name, false);
                            if (cmp > 0)
                            {
                                // La propiedad no existía antes
                                // The property doesn't existed before
                                propertyCollectionPanel.Controls.Add(_properties[ix]);
                                propertyCollectionPanel.Controls.SetChildIndex(_properties[ix], ix);
                                ixc++;
                                ix++;
                            }
                            else if (cmp == 0)
                            {
                                // La propiedad ya existe
                                // The property already exists
                                ixc++;
                                ix++;
                            }
                            else
                            {
                                // La propiedad ya no existe
                                // The property no longer exists
                                propertyCollectionPanel.Controls.RemoveAt(ixc);
                            }
                        }
                    }
                }
                while (ixc < propertyCollectionPanel.Controls.Count)
                {
                    // Eliminar los controles restantes
                    // Remove the remainder controls
                    propertyCollectionPanel.Controls.RemoveAt(ixc);
                }
                propertyCollectionPanel.ResumeLayout(false);
            }
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (Parent != null)
            {
                return new Size(Parent.ClientSize.Width, ContentHeight);
            }
            return base.GetPreferredSize(proposedSize);
        }
        /// <summary>
        /// Recalcula la altura de los editores de propiedades
        /// Refresh property editors height
        /// </summary>
        public void ResizeProperties()
        {
            if (_properties != null)
            {
                if ((_header == null) || !_header.Collapsed)
                {
                    for (int ix = 0; ix < _properties.Count; ix++)
                    {
                        _properties[ix].RefreshHeight();
                    }
                }
            }
        }
        /// <summary>
        /// Actualiza los valores mostrados en todos los editores de propiedades
        /// Update the shown values for all the property editors
        /// </summary>
        /// <param name="except">
        /// Editor que no se debe actualizar
        /// Do not update this editor
        /// </param>
        public void RefreshProperties(PropertyEditorBase except)
        {
            if (_properties != null)
            {
                foreach (PropertyEditorBase pe in _properties)
                {
                    if (pe != except)
                    {
                        pe.RefreshValue();
                    }
                }
            }
        }
        /// <summary>
        /// Actualiza la instancia del objeto actual en los editores / 
        /// Update the current object instance in the editors
        /// </summary>
        /// <param name="instance">
        /// Nueva instancia del tipo editado / 
        /// New instance of the edited type
        /// </param>
        public void RefreshInstance(object instance)
        {
            if (_properties != null)
            {
                foreach (PropertyEditorBase pe in _properties)
                {
                    pe.Instance = instance;
                }
            }
        }
        public int CompareTo(PropertyTableColumn other)
        {
            return ColumnIndex.CompareTo(other.ColumnIndex);
        }
        public bool Equals(PropertyTableColumn other)
        {
            return Title.Equals(other.Title) &&
                (RowIndex == other.RowIndex) &&
                (ColumnIndex == other.ColumnIndex);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _properties[0].Focus();
        }
    }
}

using DesktopControls.Controls.PropertyTable.Interfaces;
using DesktopControls.PropertyTools;
using GlobalCommonEntities.DependencyInjection;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedades compuestas / 
    /// Composite properties editor
    /// </summary>
    public class ExpandablePropertyEditor : PropertyEditorBase, IPropertyBlockContainer
    {
        protected ImageList bImageList = new ImageList();
        protected Panel pTop = new Panel();
        protected Panel pEditor = new Panel();
        protected Button bCollapse = new Button();
        protected object _value = null;
        /// <summary>
        /// Columna para contener las propiedades del objeto compuesto / 
        /// Column to list the composite object properties
        /// </summary>
        protected PropertyTableColumn _properties;
        public ExpandablePropertyEditor()
        {
            bImageList.TransparentColor = Color.Magenta;
            bImageList.Images.Add("Collapse_small.bmp", Collapse_small);
            bImageList.Images.Add("Collapsed.bmp", Collapsed_small);
            pTop.BackColor = SystemColors.Control;
            pTop.Controls.Add(pEditor);
            pTop.Controls.Add(bCollapse);
            pTop.Dock = DockStyle.Top;
            pTop.Margin = new Padding(0, 0, 0, 0);
            pTop.Name = "pTop";
            pTop.Height = 20;
            pTop.TabIndex = 0;
            pEditor.Dock = DockStyle.Fill;
            pEditor.Name = "pEditor";
            pEditor.Height = 20;
            pEditor.TabIndex = 2;
            bCollapse.BackColor = Color.Transparent;
            bCollapse.Dock = DockStyle.Left;
            bCollapse.FlatStyle = FlatStyle.Flat;
            bCollapse.FlatAppearance.BorderSize = 0;
            bCollapse.ImageIndex = 1;
            bCollapse.ImageList = bImageList;
            bCollapse.Name = "bCollapse";
            bCollapse.Size = new Size(20, 20);
            bCollapse.TabIndex = 1;
            bCollapse.UseVisualStyleBackColor = false;
            bCollapse.Click += new EventHandler(bCollapse_Click);
        }
        /// <summary>
        /// Estado de la lista de expansión / 
        /// Expansion list state
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
        /// IPropertyBlockContainer: Configuración de encabezados de columna / 
        /// IPropertyBlockContainer: Column headers settings
        /// </summary>
        public int HeadersHeight
        {
            get
            {
                return ParentContainer == null ? 20 : ParentContainer.HeadersHeight;
            }
            set { }
        }
        public Font HeadersFont
        {
            get
            {
                return ParentContainer == null ? null : ParentContainer.HeadersFont;
            }
            set { }
        }
        public Color HeadersBackColor
        {
            get
            {
                return ExpandableBackColor;
            }
            set
            {
            }
        }
        public Color HeadersForeColor
        {
            get
            {
                return ExpandableForeColor;
            }
            set { }
        }
        /// <summary>
        /// IPropertyBlockContainer: Pasar el foco a la siguiente propiedad al terminar la edición / 
        /// IPropertyBlockContainer: Move focus to the next property when editing the edition
        /// </summary>
        public bool AutoTab
        {
            get
            {
                return ParentContainer == null ? false : ParentContainer.AutoTab;
            }
            set { }
        }
        /// <summary>
        /// IPropertyBlockContainer: Fuente para los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors font
        /// </summary>
        public Font EditorsFont
        {
            get
            {
                return ParentContainer == null ? null : ParentContainer.EditorsFont;
            }
            set { }
        }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors background color
        /// </summary>
        public Color EditorsBackColor
        {
            get
            {
                return ParentContainer == null ? BackColor : ParentContainer.EditorsBackColor;
            }
            set { }
        }
        /// <summary>
        /// IPropertyBlockContainer: Color de primer plano de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors foreground color
        /// </summary>
        public Color EditorsForeColor
        {
            get
            {
                return ParentContainer == null ? ForeColor : ParentContainer.EditorsForeColor;
            }
            set { }
        }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo de las propiedades desplegables / 
        /// IPropertyBlockContainer: Expandable properties background color
        /// </summary>
        public Color ExpandableBackColor
        {
            get
            {
                return ParentContainer == null ? ExpandableBackColor : ParentContainer.ExpandableBackColor;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Color de primer plano de las propiedades desplegables / 
        /// IPropertyBlockContainer: Expandable properties foreground color
        /// </summary>
        public Color ExpandableForeColor
        {
            get
            {
                return ParentContainer == null ? ForeColor : ParentContainer.ExpandableForeColor;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo del editor cuando está seleccionado / 
        /// IPropertyBlockContainer: Selected property editor background color
        /// </summary>
        public Color EditorsSelectedBackColor
        {
            get
            {
                return ParentContainer == null ? ForeColor : ParentContainer.EditorsSelectedBackColor;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Color de texto del editor cuando está seleccionado / 
        /// IPropertyBlockContainer: Selected property editor foreground color
        /// </summary>
        public Color EditorsSelectedForeColor
        {
            get
            {
                return ParentContainer == null ? ForeColor : ParentContainer.EditorsSelectedForeColor;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Color de los bordes de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors border colors
        /// </summary>
        public Color EditorsTopBorderColor
        {
            get
            {
                return ParentContainer == null ? Color.Black : ParentContainer.EditorsTopBorderColor;
            }
            set
            {
            }
        }
        public Color EditorsLeftBorderColor
        {
            get
            {
                return ParentContainer == null ? Color.Black : ParentContainer.EditorsLeftBorderColor;
            }
            set
            {
            }
        }
        public Color EditorsBottomBorderColor
        {
            get
            {
                return ParentContainer == null ? Color.Black : ParentContainer.EditorsBottomBorderColor;
            }
            set
            {
            }
        }
        public Color EditorsRightBorderColor
        {
            get
            {
                return ParentContainer == null ? Color.Black : ParentContainer.EditorsRightBorderColor;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Tamaño del contorno de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors surround size
        /// </summary>
        public Padding EditorsDefaultPadding
        {
            get
            {
                return ParentContainer == null ? Padding : ParentContainer.EditorsDefaultPadding;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Tamaño del borde de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors border size
        /// </summary>
        public Padding EditorsBorderSize
        {
            get
            {
                return ParentContainer == null ? new Padding(0) : ParentContainer.EditorsBorderSize;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Respositorio de recursos incrustados / 
        /// IPropertyBlockContainer: Embedded resources repository
        /// </summary>
        public ResourcesRepository AllResources
        {
            get
            {
                return ParentContainer?.AllResources;
            }
            set
            {
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Contenedor de nivel inferior / 
        /// IPropertyBlockContainer: Low level container
        /// </summary>
        /// <param name="pname">
        /// Nombre de la propiedad buscada / 
        /// Property searched name 
        /// </param>
        /// <param name="pi">
        /// Información de la propiedad / 
        /// Property information
        /// </param>
        public IPropertyBlockContainer GetChildContainer(string pname, out PropertyInfo pi)
        {
            pi = null;
            if (_properties != null)
            {
                return _properties.GetChildContainer(pname, out pi);
            }
            return null;
        }
        /// <summary>
        /// IPropertyBlockContainer: Columna del contenedor / 
        /// IPropertyBlockContainer: FOContainer column
        /// </summary>
        public PropertyTableColumn AsColumn
        {
            get
            {
                return _properties;
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguna de las propiedades ha cambiado / 
        /// IPropertyBlockContainer: Some property has changed
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad / 
        /// Property editor
        /// </param>
        /// <param name="oldvalue">
        /// Valor de la propiedad antes del cambio / 
        /// Property value before change
        /// </param>
        public virtual void PropertyChanged(PropertyEditorBase editor, object oldvalue)
        {
            GetValue();
            ParentContainer?.PropertyChanged(editor, oldvalue);
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguna de las propiedades ha recibido el foco / 
        /// IPropertyBlockContainer: Some property has got focus
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad / 
        /// Property editor
        /// </param>
        public void PropertyGotFocus(PropertyEditorBase editor)
        {
            ParentContainer?.PropertyGotFocus(editor);
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguna de las propiedades ha perdido el foco / 
        /// IPropertyBlockContainer: Some property has lost focus
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad / 
        /// Property editor
        /// </param>
        public void PropertyLostFocus(PropertyEditorBase editor)
        {
            ParentContainer?.PropertyLostFocus(editor);
        }
        /// <summary>
        /// IPropertyBlockContainer: La edición de la propiedad ha finalizado. Pasar el foco a la siguiente / 
        /// IPropertyBlockContainer: Property edition has finished. Move focus to the next property
        /// </summary>
        /// <param name="editor">
        /// Editor que ha finalizado la edición / 
        /// Finioshed editor
        /// </param>
        /// <param name="colindex">
        /// Índice de la columna / 
        /// Column index
        /// </param>
        /// <param name="rowindex">
        /// Índice de la fila / 
        /// Row index
        /// </param>
        public void PropertyCommited(PropertyEditorBase editor, int colindex = 0, int rowindex = 0)
        {
            if (colindex == _properties.Count - 1)
            {
                ParentContainer.PropertyCommited(this);
            }
            else
            {
                _properties.FocusProperty(editor.Index + 1);
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: El contenedor debe colapsar o expandir su contenido / 
        /// IPropertyBlockContainer: The container must collapse or expand its content
        /// </summary>
        /// <param name="collapse">
        /// true para colapsar contenido
        /// true to collapse content
        /// </param>
        public void Collapse(bool collapse)
        {
            Collapsed = collapse;
        }
        /// <summary>
        /// IPropertyBlockContainer: Alguno de los elementos ha cambiado de tamaño / 
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
        /// Establece la altura óptima del editor en función de su contenido / 
        /// Set optimum editor height based on its content
        /// </summary>
        public override int CalcHeight(int minheight)
        {
            int hcnt = Math.Max(minheight, pTop.Height + Padding.Vertical);
            if (_properties != null)
            {
                if (Collapsed)
                {
                    if (hcnt > _properties.ContentHeight)
                    {
                        hcnt -= _properties.ContentHeight;
                    }
                }
                else
                {
                    hcnt += _properties.ContentHeight;
                }
            }
            return hcnt;
        }
        /// <summary>
        /// Instancia del objeto al que pertenece la propiedad / 
        /// Instance of the property owner object
        /// </summary>
        [Browsable(false)]
        public override object Instance
        {
            get
            {
                return base.Instance;
            }
            set
            {
                if ((_properties != null) && (base.Instance != null))
                {
                    base.Instance = value;
                    _properties.RefreshInstance(_value);
                }
                else
                {
                    base.Instance = value;
                }
            }
        }
        /// <summary>
        /// Refrescar los datos mostrados al usuario / 
        /// Refresh data shown to the user
        /// </summary>
        public override void RefreshValue()
        {
            base.RefreshValue();
            _properties.RefreshProperties(null);
        }
        /// <summary>
        /// Configuración de los controles de la interfaz de usuario / 
        /// User interface controls configuration
        /// </summary>
        protected override void ConfigureUI()
        {
            base.ConfigureUI();
            pEditor.Controls.Clear();
            ClearControls();
            if (_property != null)
            {
                if (!_property.CanWrite)
                {
                    throw new Exception(ERROR_NonExpandable);
                }
                _properties = new PropertyTableColumn()
                {
                    Header = null,
                    Name = "tc" + _property.Name,
                    Left = 8,
                    Width = _client.Width - 8,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left,
                    ParentContainer = this
                };
                ListProperties();
                _properties.ArrangeProperties();
                if (!Collapsed)
                {
                    AddControl(_properties);
                }
                AddControl(pTop);
                pTop.BackColor = ExpandableBackColor;
                pTop.ForeColor = ExpandableForeColor;
                pTop.Font = HeadersFont;
                pTop.Height = HeadersHeight;
                SetExtraControls();
                SetHeight(Height);
            }
        }
        /// <summary>
        /// Realizar configuración extra de controles / 
        /// Set extra control configuration
        /// </summary>
        protected virtual void SetExtraControls()
        {
        }
        /// <summary>
        /// Volver a calcular la altura del editor / 
        /// Recalculate editor's height
        /// </summary>
        public override void RefreshHeight()
        {
            SetHeight(pTop.Height);
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(proposedSize.Width, CalcHeight(pTop.Height));
        }
        /// <summary>
        /// Listar las propiedades visibles del objeto / 
        /// List browsable object properties
        /// </summary>
        protected virtual void ListProperties()
        {
            IPropertyConfigurationProvider cfgprovider = _instance as IPropertyConfigurationProvider;
            object value = _property.GetValue(_instance);
            Type vtype = value == null ? _property.PropertyType : value.GetType();
            foreach (PropertyInfo pi in vtype.GetProperties())
            {
                bool browsable = ((cfgprovider != null) && cfgprovider.Browsable(pi.Name, ValueIndex).HasValue) ?
                    cfgprovider.Browsable(pi.Name, ValueIndex).Value : true;
                BrowsableAttribute ba = pi.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute;
                if (pi.CanRead && browsable && ((ba == null) || ba.Browsable))
                {
                    object[] index = ValueIndex < 0 ? null : new object[] { ValueIndex };
                    _properties.AddProperty(pi, _instance == null ? null : _property.GetValue(_instance, index));
                }
            }
        }
        /// <summary>
        /// Colapsar o expandir la lista de propiedades / 
        /// Collapse or Expand property list
        /// </summary>
        private void bCollapse_Click(object sender, EventArgs e)
        {
            Collapsed = !Collapsed;
            if (Collapsed)
            {
                RemoveControl(_properties);
                SetHeight(pTop.Height);
            }
            else
            {
                ClearControls();
                AddControl(_properties);
                _properties.ResizeProperties();
                AddControl(pTop);
                SetHeight(pTop.Height);
            }
            ContentSizeChanged(this, Collapsed ? PropertyEditorChangedReason.Collapsed : PropertyEditorChangedReason.Expanded);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_properties != null)
            {
                _properties.Top = pTop.Bottom;
                _properties.Width = ClientSize.Width - _properties.Left;
                _properties.Height = _properties.ContentHeight;
            }
        }
    }
}

using DesktopControls.Controls.PropertyTable.Attributes;
using DesktopControls.Controls.PropertyTable.Interfaces;
using DesktopControls.Controls.PropertyTable.PropertyEditors;
using DesktopControls.PropertyTools;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable
{
    /// <summary>
    /// Estilo de los editores de propiedades / 
    /// Property editors style
    /// </summary>
    [Flags]
    public enum PropertyEditionStyle
    {
        None = 0,
        /// <summary>
        /// Editar usando botones / 
        /// Edit using button
        /// </summary>
        Button = 1,
        /// <summary>
        /// Editar usando clic del ratón / 
        /// Edit using mouse click
        /// </summary>
        Click = 2,
        /// <summary>
        /// Editar usando doble clic del ratón / 
        /// Edit using mouse double click
        /// </summary>
        DoubleClick = 4,
        /// <summary>
        /// Situar el elemento de edición a la izquierda o usar botón izquierdo / 
        /// Place the edition element to the left or use left button
        /// </summary>
        Left = 8,
        /// <summary>
        /// Usar texto para el elemento de edición / 
        /// Use text for the edition element
        /// </summary>
        Text = 16,
        /// <summary>
        /// Usar una imagen para el elemento de edición / 
        /// Use an image for the edition element
        /// </summary>
        Image = 32,
        ButtonLeft = Button | Left,
        ClickLeft = Click | Left,
        DoubleClickLeft = DoubleClick | Left,
        TextLeft = Text | Left,
        ImageLeft = Image | Left
    }
    /// <summary>
    /// Tabla de editores de propiedades / 
    /// Property editors table
    /// </summary>
    public partial class PropertyTable : UserControl, IPropertyBlockContainer, IPropertyEditorContainer
    {
        protected object _editableObject;
        protected PropertyEditorBase _currentPropertyEditor;
        protected List<List<PropertyTableColumn>> _columns;
        protected Dictionary<string, PropertyTableColumn> _categories;
        protected IPropertyConfigurationProvider _cfgProvider;
        protected bool _scrollVisible = false;
        public PropertyTable()
        {
            InitializeComponent();
            HeadersHeight = 20;
            HeadersBackColor = SystemColors.ActiveCaption;
            HeadersForeColor = SystemColors.ActiveCaptionText;
            EditorsBackColor = SystemColors.Window;
            EditorsForeColor = SystemColors.WindowText;
            ExpandableBackColor = SystemColors.Control;
            ExpandableForeColor = SystemColors.WindowText;
            EditorsDefaultPadding = new Padding(3);
            EditorsBorderSize = new Padding(0);
            EditorsBottomBorderColor = Color.Black;
            EditorsTopBorderColor = Color.Black;
            EditorsLeftBorderColor = Color.Black;
            EditorsRightBorderColor = Color.Black;
            EditorsSelectedBackColor = SystemColors.Highlight;
            EditorsSelectedForeColor = SystemColors.HighlightText;
        }
        /// <summary>
        /// Se dispara al asignar un nuevo objeto para editar / 
        /// Fires when a new object is assigned
        /// </summary>
        public event EventHandler SelectedObjectChanged;
        /// <summary>
        /// Se dispara cuando cambia el contenido o tamaño del control / 
        /// Fires when the control content or size changes
        /// </summary>
        public event EventHandler ContentChanged;
        /// <summary>
        /// Se dispara cuando cambia el contenido o tamaño del control / 
        /// Fires when the control content or size changes
        /// </summary>
        public event PropertyEditorChangedEventHandler EditorSizeChanged;
        /// <summary>
        /// Objeto con las propiedades a editar / 
        /// Object with editing properties
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return _editableObject;
            }
            set
            {
                if (_editableObject != value)
                {
                    Type currenttype = (_editableObject != null) ? _editableObject.GetType() : null;
                    if (value != null)
                    {
                        _editableObject = value;
                        _cfgProvider = _editableObject as IPropertyConfigurationProvider;
                        if ((_cfgProvider != null) || (currenttype == null) || (currenttype != _editableObject.GetType()))
                        {
                            pGrid.Controls.Clear();
                            if (_columns != null)
                            {
                                _columns.Clear();
                                _currentPropertyEditor = null;
                            }
                            BuildLayout();
                        }
                        else
                        {
                            RefreshInstance();
                        }
                    }
                    else
                    {
                        pGrid.Controls.Clear();
                        if (_columns != null)
                        {
                            _columns.Clear();
                            _currentPropertyEditor = null;
                        }
                        _editableObject = null;
                        _cfgProvider = null;
                    }
                    SelectedObjectChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// IPropertyBlockContainer: Configuración de encabezados de columna / 
        /// IPropertyBlockContainer: Column headers settings
        /// </summary>
        [DefaultValue(20)]
        public int HeadersHeight { get; set; }
        public Font HeadersFont { get; set; }
        public Color HeadersBackColor { get; set; }
        public Color HeadersForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo de las propiedades desplegables / 
        /// IPropertyBlockContainer: Expandable properties background color
        /// </summary>
        public Color ExpandableBackColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de primer plano de las propiedades desplegables / 
        /// IPropertyBlockContainer: Expandable properties foreground color
        /// </summary>
        public Color ExpandableForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo del editor cuando está seleccionado / 
        /// IPropertyBlockContainer: Selected property editor background color
        /// </summary>
        public Color EditorsSelectedBackColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de texto del editor cuando está seleccionado / 
        /// IPropertyBlockContainer: Selected property editor text color
        /// </summary>
        public Color EditorsSelectedForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de los bordes de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors border colors
        /// </summary>
        public Color EditorsTopBorderColor { get; set; }
        public Color EditorsLeftBorderColor { get; set; }
        public Color EditorsBottomBorderColor { get; set; }
        public Color EditorsRightBorderColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Tamaño del contorno de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors surround size
        /// </summary>
        public Padding EditorsDefaultPadding { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Tamaño del borde de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors border size
        /// </summary>
        public Padding EditorsBorderSize { get; set; }
        /// <summary>
        /// Configuración del panel de información / 
        /// Informatión panel settings
        /// </summary>
        public Font CommentsFont
        {
            get
            {
                return lComment.Font;
            }
            set
            {
                lComment.Font = value;
            }
        }
        public Font CommentsHeaderFont
        {
            get
            {
                return gbTitle.Font;
            }
            set
            {
                gbTitle.Font = value;
            }
        }
        public Color CommentsbackColor
        {
            get
            {
                return pExtendedDoc.BackColor;
            }
            set
            {
                pExtendedDoc.BackColor = value;
            }
        }
        public Color CommentsForeColor
        {
            get
            {
                return pExtendedDoc.ForeColor;
            }
            set
            {
                pExtendedDoc.ForeColor = value;
            }
        }
        /// <summary>
        /// Color de fondo de la tabla / 
        /// Table background color
        /// </summary>
        public Color GridBackColor
        {
            get
            {
                return pGrid.BackColor;
            }
            set
            {
                pGrid.BackColor = value;
            }
        }
        /// <summary>
        /// Redistribuir contenido al cambiar el tamaño / 
        /// Redistribute content when size changes
        /// </summary>
        public bool AutoResize { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Pasar el foco a la siguiente propiedad al terminar la edición / 
        /// IPropertyBlockContainer: Move focus to the next property when editing the edition
        /// </summary>
        public bool AutoTab { get; set; }
        /// <summary>
        /// El valor de una propiedad ha cambiado / 
        /// The value of a property has changed
        /// </summary>
        public event ValueChangedEventHandler PropertyValueChanged;
        /// <summary>
        /// Se ha seleccionado una nueva propiedad / 
        /// A nuew property has been selected
        /// </summary>
        public event SelectedPropertyChangedEventHandler SelectedPropertyChanged;
        /// <summary>
        /// IPropertyBlockContainer: Fuente para los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors font
        /// </summary>
        public Font EditorsFont { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de fondo de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors background color
        /// </summary>
        public Color EditorsBackColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Color de primer plano de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors foreground color
        /// </summary>
        public Color EditorsForeColor { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Estilo por defecto de los editores de propiedades / 
        /// IPropertyBlockContainer: Property editors default style
        /// </summary>
        public PropertyEditionStyle EditionStyle { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Respositorio de recursos incrustados / 
        /// IPropertyBlockContainer: Embedded resources repository
        /// </summary>
        public ResourcesRepository AllResources { get; set; }
        /// <summary>
        /// IPropertyBlockContainer: Contenedor de nivel superior / 
        /// IPropertyBlockContainer: High level container
        /// </summary>
        [Browsable(false)]
        public IPropertyBlockContainer ParentContainer { get; set; }
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
            pi = _editableObject.GetType().GetRuntimeProperty(pname);
            bool browsable;
            bool newcolumn;
            return FindPropertyColumn(pi, out browsable, out newcolumn);
        }
        /// <summary>
        /// IPropertyBlockContainer: Columna del contenedor / 
        /// IPropertyBlockContainer: FOContainer column
        /// </summary>
        public PropertyTableColumn AsColumn
        {
            get
            {
                return null;
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
        public void PropertyChanged(PropertyEditorBase editor, object oldvalue)
        {
            ValueChangedEventArgs e = new ValueChangedEventArgs(editor.Property.Name, oldvalue);
            PropertyValueChanged?.Invoke(this, e);
            _currentPropertyEditor = editor;
            gbTitle.Text = editor.DisplayName;
            lComment.Text = editor.Description;
            switch (editor.RefreshMode)
            {
                case RefreshProperties.All:
                    foreach (List<PropertyTableColumn> lcol in _columns)
                    {
                        foreach (PropertyTableColumn col in lcol)
                        {
                            col.RefreshProperties(editor);
                        }
                    }
                    break;
            }
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
            _currentPropertyEditor = editor;
            editor.Select(EditorsSelectedBackColor, EditorsSelectedForeColor);
            gbTitle.Text = editor.DisplayName;
            lComment.Text = editor.Description;
            SelectedPropertyChanged?.Invoke(this, new SelectedPropertyChangedEventArgs(editor.Property.Name));
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
            if (AutoTab)
            {
                if (rowindex == _columns.Count - 1)
                {
                    if (colindex == _columns[rowindex].Count - 1)
                    {
                        colindex = 0;
                        rowindex = 0;
                    }
                    else
                    {
                        colindex++;
                    }
                }
                else if (colindex == _columns[rowindex].Count - 1)
                {
                    colindex = 0;
                    rowindex++;
                }
                else
                {
                    colindex++;
                }
                _columns[rowindex][colindex].FocusProperty(0);
            }
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
            editor.UnSelect();
            _currentPropertyEditor = null;
            gbTitle.Text = "";
            lComment.Text = "";
        }
        /// <summary>
        /// IPropertyBlockContainer: El contenedor debe colapsar o expandir su contenido / 
        /// IPropertyBlockContainer: The container must collapse or expand its content
        /// </summary>
        /// <param name="collapse">
        /// true para colapsar contenido / 
        /// true to collapse content
        /// </param>
        public void Collapse(bool collapse)
        {
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
            ResizeContent();
            EditorSizeChanged?.Invoke(this, new PropertyEditorChangedEventArgs(reason, changedItem));
        }
        /// <summary>
        /// Refrescar contenido / 
        /// Refresh contents
        /// </summary>
        public void RefreshData()
        {
            if (_columns != null)
            {
                foreach (List<PropertyTableColumn> lcol in _columns)
                {
                    foreach (PropertyTableColumn col in lcol)
                    {
                        col.RefreshProperties(null);
                    }
                }
            }
        }
        /// <summary>
        /// Refrescar la instancia del objeto en los editores / 
        /// Refresh object instance in editors
        /// </summary>
        protected void RefreshInstance()
        {
            if (_columns != null)
            {
                foreach (List<PropertyTableColumn> lcol in _columns)
                {
                    foreach (PropertyTableColumn col in lcol)
                    {
                        col.RefreshInstance(_editableObject);
                    }
                }
            }
        }
        /// <summary>
        /// Refrescar una propiedad / 
        /// Refresh a property
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        public void Refresh(string name)
        {
            if (_editableObject != null)
            {
                string[] parts = name.Split('.');
                Type ptype = _editableObject.GetType();
                PropertyInfo pi = ptype.GetRuntimeProperty(parts[0]);
                if (pi != null)
                {
                    bool browsable;
                    bool newcolumn;
                    PropertyTableColumn col = FindPropertyColumn(pi, out browsable, out newcolumn);
                    if ((col != null) && browsable && !newcolumn)
                    {
                        if (parts.Length > 1)
                        {
                            for (int ix = 0; ix < parts.Length - 1; ix++)
                            {
                                IPropertyBlockContainer scol = col.GetChildContainer(parts[ix], out pi);
                                if (scol == null)
                                {
                                    return;
                                }
                                col = scol.AsColumn;
                            }
                            col.GetChildContainer(parts[parts.Length - 1], out pi);
                        }
                        col.RefreshProperty(pi);
                    }
                }
            }
        }
        /// <summary>
        /// Volver a procesar una determinada propiedad / 
        /// Reprocess a given property
        /// </summary>
        /// <param name="name">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        public void RescanProperty(string name)
        {
            if (_editableObject != null)
            {
                string[] parts = name.Split('.');
                PropertyTableColumn col = null;
                PropertyInfo pi = _editableObject.GetType().GetRuntimeProperty(parts[0]);
                bool browsable;
                bool newcolumn;
                col = FindPropertyColumn(pi, out browsable, out newcolumn);
                if (col != null)
                {
                    if (parts.Length > 1)
                    {
                        for (int ix = 0; ix < parts.Length - 1; ix++)
                        {
                            IPropertyBlockContainer scol = col.GetChildContainer(parts[ix], out pi);
                            if (scol == null)
                            {
                                return;
                            }
                            col = scol.AsColumn;
                        }
                        col.GetChildContainer(parts[parts.Length - 1], out pi);
                    }
                    object instance = col.RemoveProperty(pi);
                    if (browsable)
                    {
                        col.AddProperty(pi, instance);
                        col.RefreshPropertyEditorHeight(pi);
                        if (newcolumn)
                        {
                            ArrangeColumns();
                        }
                        else
                        {
                            col.ArrangeProperties();
                            RefreshColumns();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Pasar el foco al siguiente editor / 
        /// Activate the next editor
        /// </summary>
        public void FocusNextEditor()
        {
            if (_columns != null)
            {
                if (_currentPropertyEditor == null)
                {
                    _columns[0][0].FocusProperty(0);
                }
                else
                {
                    PropertyTableColumn col = _currentPropertyEditor.ParentContainer as PropertyTableColumn;
                    if (col != null)
                    {
                        PropertyCommited(null, col.ColumnIndex, col.RowIndex);
                    }
                }
            }
        }
        public void PostCurrentProperty()
        {
            if (_currentPropertyEditor != null)
            {
                _currentPropertyEditor.UnSelect();
                _currentPropertyEditor.UpdateValue();
            }
        }
        /// <summary>
        /// Altura mínima necesaria para mostrar todas las propiedades / 
        /// Minimum height to show all properties
        /// </summary>
        public int GetMinimumHeight()
        {
            int minheight = pExtendedDoc.Height + spContainter.SplitterWidth;
            if (SelectedObject != null)
            {
                for (int ir = 0; ir < _columns.Count; ir++)
                {
                    int maxrowheight = 0;
                    for (int ic = 0; ic < _columns[ir].Count; ic++)
                    {
                        PropertyTableColumn col = _columns[ir][ic];
                        if (col.Count > 0)
                        {
                            maxrowheight = Math.Max(maxrowheight, col.ContentHeight);
                        }
                    }
                    minheight += maxrowheight;
                }
            }
            return minheight;
        }
        /// <summary>
        /// Busca la columna a la que pertenece una propiedad o crea una nueva / 
        /// Find the column containing a given peorperty or create a new one
        /// </summary>
        /// <param name="pi">
        /// Descriptor de la propiedad / 
        /// Property descriptor
        /// </param>
        /// <param name="browsable">
        /// La propiedad es visible para el usuario / 
        /// The property is visible to the user
        /// </param>
        /// <param name="newcolumn">
        /// Se ha creado una nueva columna para la propiedad / 
        /// A new column has been created
        /// </param>
        /// <returns>
        /// Columna que contiene la propiedad / 
        /// Property container column
        /// </returns>
        protected PropertyTableColumn FindPropertyColumn(PropertyInfo pi, out bool browsable, out bool newcolumn)
        {
            string category = "General";
            ContentAlignment alignment = ContentAlignment.MiddleCenter;
            int column = 0;
            int row = 0;
            BrowsableAttribute ba = pi.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute;
            browsable = ((_cfgProvider != null) && _cfgProvider.Browsable(pi.Name).HasValue) ? _cfgProvider.Browsable(pi.Name).Value : true;
            browsable = pi.CanRead && browsable && ((ba == null) || ba.Browsable);
            newcolumn = false;
            if (browsable)
            {
                PropertyBlockAttribute ca = pi.GetCustomAttribute<PropertyBlockAttribute>();
                if (ca != null)
                {
                    category = ca.Category;
                    column = ca.Column;
                    row = ca.Row;
                    alignment = ca.HeaderAlign;
                }
                PropertyTableColumn col = null;
                if (!_categories.ContainsKey(category))
                {
                    PropertyColumnHeader header = new PropertyColumnHeader()
                    {
                        Title = category,
                        Alignment = alignment,
                        Name = string.Join("_", "h", category, row.ToString(), column.ToString()),
                        Height = HeadersHeight,
                        Font = HeadersFont,
                        BackColor = HeadersBackColor,
                        ForeColor = HeadersForeColor
                    };
                    col = new PropertyTableColumn()
                    {
                        Header = header,
                        Name = string.Join("_", "c", category, row.ToString(), column.ToString()),
                        ColumnIndex = column,
                        RowIndex = row,
                        TabStop = true,
                        ParentContainer = this
                    };
                    _categories[category] = col;
                    List<PropertyTableColumn> lcol = GetRow(row);
                    lcol.Add(col);
                    if (lcol.Count == 1)
                    {
                        col.Width = ClientSize.Width;
                        _columns.Sort(CompareRows);
                    }
                    else
                    {
                        lcol.Sort();
                        foreach (PropertyTableColumn ccol in lcol)
                        {
                            ccol.Width = ClientSize.Width / lcol.Count;
                        }
                    }
                    newcolumn = true;
                }
                else
                {
                    col = _categories[category];
                }
                return col;
            }
            else
            {
                foreach (List<PropertyTableColumn> lcol in _columns)
                {
                    foreach (PropertyTableColumn col in lcol)
                    {
                        if (col.ContainsProperty(pi))
                        {
                            return col;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Obtener una fila de columnas de propiedades a partir de su índice / 
        /// Get a property column row by row index
        /// </summary>
        /// <param name="row">
        /// Índice de la fila / 
        /// Row index
        /// </param>
        /// <returns>
        /// Fila existente o nueva fila vacía / 
        /// Row if exists or a new empty row
        /// </returns>
        protected List<PropertyTableColumn> GetRow(int row)
        {
            foreach (List<PropertyTableColumn> lcol in _columns)
            {
                if (lcol[0].RowIndex == row)
                {
                    return lcol;
                }
            }
            List<PropertyTableColumn> nrow = new List<PropertyTableColumn>();
            _columns.Add(nrow);
            return nrow;
        }
        /// <summary>
        /// Comparador de listas de columnas de propiedades, para ordenarlas por índice de fila / 
        /// Property column list comparer, by row index
        /// </summary>
        /// <param name="r1">
        /// Lista de columnas de propiedades / 
        /// Property column list
        /// </param>
        /// <param name="r2">
        /// Lista de columnas de propiedades / 
        /// Property column list
        /// </param>
        /// <returns>
        /// 0 si son iguales, 1 si r2 > r1 o -1 si r1 > r2 / 
        /// 0 if both are equal, 1 if r2 > r1 or -1 if r1 > r2
        /// </returns>
        protected int CompareRows(List<PropertyTableColumn> r1, List<PropertyTableColumn> r2)
        {
            int row1 = r1[0].RowIndex;
            int row2 = r2[0].RowIndex;
            return row1.CompareTo(row2);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (AutoResize)
            {
                RefreshColumns();
            }
        }
        /// <summary>
        /// Distibuye las columnas en paneles splitter / 
        /// Arrange columns in splitter containers
        /// </summary>
        protected void ArrangeColumns()
        {
            SplitContainer spmain = null;
            pGrid.Controls.Clear();
            //int y = 0;
            for (int ir = 0; ir < _columns.Count; ir++)
            {
                if (_columns[ir].Count > 1)
                {
                    int colwidth = pGrid.ClientSize.Width / _columns[ir].Count;
                    spmain = new SplitContainer()
                    {
                        Orientation = Orientation.Vertical,
                        Width = pGrid.ClientSize.Width,
                        Height = 20,
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        BorderStyle = BorderStyle.None,
                        Left = 0,
                        Tag = _columns[ir]
                    };
                    SplitContainer sp = spmain;
                    SplitterPanel spanel = sp.Panel1;
                    bool setsplitter = true;
                    for (int ic = 0; ic < _columns[ir].Count; ic++)
                    {
                        PropertyTableColumn col = _columns[ir][ic];
                        if (col.Count > 0)
                        {
                            col.ArrangeProperties();
                            spmain.Height = Math.Max(spmain.Height, col.ContentHeight);
                            if (setsplitter)
                            {
                                try
                                {
                                    if (colwidth > (sp.Width - sp.Panel2MinSize))
                                    {
                                        sp.SplitterDistance = sp.Width - sp.Panel2MinSize;
                                    }
                                    else
                                    {
                                        sp.SplitterDistance = Math.Max(sp.Panel1MinSize, colwidth);
                                    }
                                }
                                catch { }
                            }
                            col.Dock = DockStyle.Fill;
                            spanel.Controls.Add(col);
                            if (ic == _columns[ir].Count - 2)
                            {
                                spanel = sp.Panel2;
                                setsplitter = false;
                            }
                            else if (ic < _columns[ir].Count - 2)
                            {
                                spanel = sp.Panel2;
                                sp = new SplitContainer()
                                {
                                    Orientation = Orientation.Vertical,
                                    Margin = new Padding(0),
                                    Padding = new Padding(0),
                                    BorderStyle = BorderStyle.None,
                                    Dock = DockStyle.Fill
                                };
                                spanel.Controls.Add(sp);
                                spanel = sp.Panel1;
                                setsplitter = true;
                            }
                        }
                        col.TabIndex = ir + 1;
                        col.Left = _columns[ir][0].Margin.Left;
                        col.Width = colwidth - col.Margin.Horizontal;
                        col.Height = col.ContentHeight;
                    }
                    pGrid.Controls.Add(spmain);
                }
                else if (_columns[ir].Count > 0)
                {
                    _columns[ir][0].TabIndex = ir + 1;
                    _columns[ir][0].ArrangeProperties();
                    _columns[ir][0].Left = _columns[ir][0].Margin.Left;
                    _columns[ir][0].Width = pGrid.ClientSize.Width - _columns[ir][0].Margin.Horizontal;
                    int blockheight = _columns[ir][0].ContentHeight;
                    _columns[ir][0].Height = blockheight;
                    pGrid.Controls.Add(_columns[ir][0]);
                }
            }
        }
        protected int SplitContainerColumnCount(SplitContainer sp)
        {
            int cnt = 0;
            SplitContainer sp1 = sp.Panel1.Controls[0] as SplitContainer;
            if (sp1 != null)
            {
                cnt += SplitContainerColumnCount(sp1);
            }
            else if (sp.Panel1.Controls[0] is PropertyTableColumn)
            {
                cnt++;
            }
            SplitContainer sp2 = sp.Panel2.Controls[0] as SplitContainer;
            if (sp2 != null)
            {
                cnt += SplitContainerColumnCount(sp2);
            }
            else if (sp.Panel2.Controls[0] is PropertyTableColumn)
            {
                cnt++;
            }
            return cnt;
        }
        protected int RefreshSplitContainer(SplitContainer sp, int colwidth)
        {
            int colheight = 0;
            SplitContainer sp1 = sp.Panel1.Controls[0] as SplitContainer;
            if (sp1 != null)
            {
                colheight = Math.Max(RefreshSplitContainer(sp1, colwidth), colheight);
            }
            else if (sp.Panel1.Controls[0] is PropertyTableColumn)
            {
                PropertyTableColumn pc = sp.Panel1.Controls[0] as PropertyTableColumn;
                int pch = pc.ContentHeight;
                colheight = Math.Max(colheight, pch);
                pc.Height = pch;
                sp.SplitterDistance = colwidth;
            }
            SplitContainer sp2 = sp.Panel2.Controls[0] as SplitContainer;
            if (sp2 != null)
            {
                colheight = Math.Max(RefreshSplitContainer(sp2, colwidth), colheight);
            }
            else if (sp.Panel2.Controls[0] is PropertyTableColumn)
            {
                PropertyTableColumn pc = sp.Panel2.Controls[0] as PropertyTableColumn;
                int pch = pc.ContentHeight;
                colheight = Math.Max(colheight, pch);
                pc.Height = pch;
            }
            return colheight;
        }
        protected void RefreshColumns()
        {
            for (int ic = 0; ic < pGrid.Controls.Count; ic++)
            {
                SplitContainer spmain = pGrid.Controls[ic] as SplitContainer;
                PropertyTableColumn pc = pGrid.Controls[ic] as PropertyTableColumn;
                if (spmain != null)
                {
                    int colcnt = SplitContainerColumnCount(spmain);
                    int colwidth = (pGrid.ClientSize.Width - ((colcnt - 1) * spmain.SplitterWidth) - spmain.Margin.Horizontal) / colcnt;
                    spmain.Width = pGrid.ClientSize.Width;
                    spmain.Height = RefreshSplitContainer(spmain, colwidth);
                }
                else if (pc != null)
                {
                    pc.Width = pGrid.ClientSize.Width;
                    pc.Height = pc.ContentHeight;
                }
            }
        }
        /// <summary>
        /// Construye la tabla de editores / 
        /// Build editors table
        /// </summary>
        protected virtual void BuildLayout()
        {
            _categories = new Dictionary<string, PropertyTableColumn>();
            _columns = new List<List<PropertyTableColumn>>();
            SuspendLayout();
            foreach (PropertyInfo pi in _editableObject.GetType().GetProperties())
            {
                bool browsable;
                bool newcolumn;
                PropertyTableColumn col = FindPropertyColumn(pi, out browsable, out newcolumn);
                if (browsable && (col != null))
                {
                    col.AddProperty(pi, SelectedObject);
                }
            }
            ArrangeColumns();
            ResumeLayout(false);
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (_currentPropertyEditor != null)
            {
                _currentPropertyEditor.UpdateValue();
            }
        }
        /// <summary>
        /// Cambia la posición de los spliters en un control SplitContainer / 
        /// Change the splitter positions in a SplitContainer control
        /// </summary>
        /// <param name="sp">
        /// Control SplitContainer / 
        /// SplitContainer control
        /// </param>
        /// <param name="width">
        /// Ancho de cada una de las divisiones / 
        /// Desired width of all splits
        /// </param>
        /// <returns>
        /// Altura del control SplitControl / 
        /// SplitControl height
        /// </returns>
        protected virtual int ResizeSplitters(SplitContainer sp, int width)
        {
            int pih = (sp.Panel1.Controls[0] as PropertyTableColumn).ContentHeight;
            if (AutoResize)
            {
                if (width > (sp.ClientSize.Width - sp.Panel2MinSize))
                {
                    sp.SplitterDistance = sp.ClientSize.Width - sp.Panel2MinSize;
                }
                else
                {
                    sp.SplitterDistance = Math.Max(sp.Panel1MinSize, width);
                }
            }
            if (sp.Panel2.Controls[0] is SplitContainer)
            {
                return Math.Max(ResizeSplitters(sp.Panel2.Controls[0] as SplitContainer, width), pih);
            }
            return Math.Max((sp.Panel2.Controls[0] as PropertyTableColumn).ContentHeight, pih);
        }
        /// <summary>
        /// Cambia el tamaño y la posición del contenido / 
        /// Change the size and position of all the propery rows
        /// </summary>
        protected virtual void ResizeContent()
        {
            try
            {
                int y = pGrid.AutoScrollPosition.Y;
                foreach (Control c in pGrid.Controls)
                {
                    c.Top = y;
                    c.Width = pGrid.ClientSize.Width;
                    if (c is SplitContainer)
                    {
                        int sph = ResizeSplitters(c as SplitContainer, pGrid.ClientSize.Width / ((List<PropertyTableColumn>)c.Tag).Count);
                        c.Height = sph;
                        y += sph;
                    }
                    else
                    {
                        c.Height = (c as PropertyTableColumn).ContentHeight;
                        y += c.Height;
                    }
                }
            }
            catch
            {
            }
        }
        private void pGrid_Layout(object sender, LayoutEventArgs e)
        {
            if (pGrid.VerticalScroll.Visible != _scrollVisible)
            {
                _scrollVisible = pGrid.VerticalScroll.Visible;
                ContentSizeChanged(this, PropertyEditorChangedReason.Resized);
            }
        }

        private void PropertyTable_Load(object sender, EventArgs e)
        {
            pGrid.Layout += new LayoutEventHandler(pGrid_Layout);
        }
        // BORRAR
        /*private string ControlFromHwnd(IntPtr hwnd, Control parent)
        {
            if (parent.Handle == hwnd)
            {
                return (parent.Name ?? "") + ": " + parent.GetType().Name;
            }
            foreach (Control c in parent.Controls) 
            {
                string cn = ControlFromHwnd(hwnd, c);
                if (cn != null)
                {
                    return cn;
                }
            }
            return null;
        }
        
        protected Dictionary<int, string> wmsg = null;
        protected override void WndProc(ref Message m)
        { 
            base.WndProc(ref m);
            if (wmsg == null)
            {
                wmsg = new Dictionary<int, string>();
                wmsg[0x0000] = "WM_NULL";
                wmsg[0x0001] = "WM_CREATE";
                wmsg[0x0002] = "WM_DESTROY";
                wmsg[0x0003] = "WM_MOVE";
                wmsg[0x0005] = "WM_SIZE";
                wmsg[0x0006] = "WM_ACTIVATE";
                wmsg[0x0007] = "WM_SETFOCUS";
                wmsg[0x0008] = "WM_KILLFOCUS";
                wmsg[0x000A] = "WM_ENABLE";
                wmsg[0x000B] = "WM_SETREDRAW";
                wmsg[0x000C] = "WM_SETTEXT";
                wmsg[0x000D] = "WM_GETTEXT";
                wmsg[0x000E] = "WM_GETTEXTLENGTH";
                wmsg[0x000F] = "WM_PAINT";
                wmsg[0x0010] = "WM_CLOSE";
                wmsg[0x0011] = "WM_QUERYENDSESSION";
                wmsg[0x0012] = "WM_QUIT";
                wmsg[0x0013] = "WM_QUERYOPEN";
                wmsg[0x0014] = "WM_ERASEBKGND";
                wmsg[0x0015] = "WM_SYSCOLORCHANGE";
                wmsg[0x0016] = "WM_ENDSESSION";
                wmsg[0x0017] = "WM_SYSTEMERROR";
                wmsg[0x0018] = "WM_SHOWWINDOW";
                wmsg[0x0019] = "WM_CTLCOLOR";
                wmsg[0x001A] = "WM_WININICHANGE";
                wmsg[0x001A] = "WM_SETTINGCHANGE";
                wmsg[0x001B] = "WM_DEVMODECHANGE";
                wmsg[0x001C] = "WM_ACTIVATEAPP";
                wmsg[0x001D] = "WM_FONTCHANGE";
                wmsg[0x001E] = "WM_TIMECHANGE";
                wmsg[0x001F] = "WM_CANCELMODE";
                wmsg[0x0020] = "WM_SETCURSOR";
                wmsg[0x0021] = "WM_MOUSEACTIVATE";
                wmsg[0x0022] = "WM_CHILDACTIVATE";
                wmsg[0x0023] = "WM_QUEUESYNC";
                wmsg[0x0024] = "WM_GETMINMAXINFO";
                wmsg[0x0026] = "WM_PAINTICON";
                wmsg[0x0027] = "WM_ICONERASEBKGND";
                wmsg[0x0028] = "WM_NEXTDLGCTL";
                wmsg[0x002A] = "WM_SPOOLERSTATUS";
                wmsg[0x002B] = "WM_DRAWITEM";
                wmsg[0x002C] = "WM_MEASUREITEM";
                wmsg[0x002D] = "WM_DELETEITEM";
                wmsg[0x002E] = "WM_VKEYTOITEM";
                wmsg[0x002F] = "WM_CHARTOITEM";
                wmsg[0x0030] = "WM_SETFONT";
                wmsg[0x0031] = "WM_GETFONT";
                wmsg[0x0032] = "WM_SETHOTKEY";
                wmsg[0x0033] = "WM_GETHOTKEY";
                wmsg[0x0037] = "WM_QUERYDRAGICON";
                wmsg[0x0039] = "WM_COMPAREITEM";
                wmsg[0x0041] = "WM_COMPACTING";
                wmsg[0x0046] = "WM_WINDOWPOSCHANGING";
                wmsg[0x0047] = "WM_WINDOWPOSCHANGED";
                wmsg[0x0048] = "WM_POWER";
                wmsg[0x004A] = "WM_COPYDATA";
                wmsg[0x004B] = "WM_CANCELJOURNAL";
                wmsg[0x004E] = "WM_NOTIFY";
                wmsg[0x0050] = "WM_INPUTLANGCHANGEREQUEST";
                wmsg[0x0051] = "WM_INPUTLANGCHANGE";
                wmsg[0x0052] = "WM_TCARD";
                wmsg[0x0053] = "WM_HELP";
                wmsg[0x0054] = "WM_USERCHANGED";
                wmsg[0x0055] = "WM_NOTIFYFORMAT";
                wmsg[0x007B] = "WM_CONTEXTMENU";
                wmsg[0x007C] = "WM_STYLECHANGING";
                wmsg[0x007D] = "WM_STYLECHANGED";
                wmsg[0x007E] = "WM_DISPLAYCHANGE";
                wmsg[0x007F] = "WM_GETICON";
                wmsg[0x0080] = "WM_SETICON";
                wmsg[0x0081] = "WM_NCCREATE";
                wmsg[0x0082] = "WM_NCDESTROY";
                wmsg[0x0083] = "WM_NCCALCSIZE";
                wmsg[0x0084] = "WM_NCHITTEST";
                wmsg[0x0085] = "WM_NCPAINT";
                wmsg[0x0086] = "WM_NCACTIVATE";
                wmsg[0x0087] = "WM_GETDLGCODE";
                wmsg[0x00A0] = "WM_NCMOUSEMOVE";
                wmsg[0x00A1] = "WM_NCLBUTTONDOWN";
                wmsg[0x00A2] = "WM_NCLBUTTONUP";
                wmsg[0x00A3] = "WM_NCLBUTTONDBLCLK";
                wmsg[0x00A4] = "WM_NCRBUTTONDOWN";
                wmsg[0x00A5] = "WM_NCRBUTTONUP";
                wmsg[0x00A6] = "WM_NCRBUTTONDBLCLK";
                wmsg[0x00A7] = "WM_NCMBUTTONDOWN";
                wmsg[0x00A8] = "WM_NCMBUTTONUP";
                wmsg[0x00A9] = "WM_NCMBUTTONDBLCLK";
                wmsg[0x0100] = "WM_KEYFIRST";
                wmsg[0x0100] = "WM_KEYDOWN";
                wmsg[0x0101] = "WM_KEYUP";
                wmsg[0x0102] = "WM_CHAR";
                wmsg[0x0103] = "WM_DEADCHAR";
                wmsg[0x0104] = "WM_SYSKEYDOWN";
                wmsg[0x0105] = "WM_SYSKEYUP";
                wmsg[0x0106] = "WM_SYSCHAR";
                wmsg[0x0107] = "WM_SYSDEADCHAR";
                wmsg[0x0108] = "WM_KEYLAST";
                wmsg[0x010D] = "WM_IME_STARTCOMPOSITION";
                wmsg[0x010E] = "WM_IME_ENDCOMPOSITION";
                wmsg[0x010F] = "WM_IME_COMPOSITION";
                wmsg[0x010F] = "WM_IME_KEYLAST";
                wmsg[0x0110] = "WM_INITDIALOG";
                wmsg[0x0111] = "WM_COMMAND";
                wmsg[0x0112] = "WM_SYSCOMMAND";
                wmsg[0x0113] = "WM_TIMER";
                wmsg[0x0114] = "WM_HSCROLL";
                wmsg[0x0115] = "WM_VSCROLL";
                wmsg[0x0116] = "WM_INITMENU";
                wmsg[0x0117] = "WM_INITMENUPOPUP";
                wmsg[0x011F] = "WM_MENUSELECT";
                wmsg[0x0120] = "WM_MENUCHAR";
                wmsg[0x0121] = "WM_ENTERIDLE";
                wmsg[0x0132] = "WM_CTLCOLORMSGBOX";
                wmsg[0x0133] = "WM_CTLCOLOREDIT";
                wmsg[0x0134] = "WM_CTLCOLORLISTBOX";
                wmsg[0x0135] = "WM_CTLCOLORBTN";
                wmsg[0x0136] = "WM_CTLCOLORDLG";
                wmsg[0x0137] = "WM_CTLCOLORSCROLLBAR";
                wmsg[0x0138] = "WM_CTLCOLORSTATIC";
                wmsg[0x0200] = "WM_MOUSEFIRST";
                wmsg[0x0200] = "WM_MOUSEMOVE";
                wmsg[0x0201] = "WM_LBUTTONDOWN";
                wmsg[0x0202] = "WM_LBUTTONUP";
                wmsg[0x0203] = "WM_LBUTTONDBLCLK";
                wmsg[0x0204] = "WM_RBUTTONDOWN";
                wmsg[0x0205] = "WM_RBUTTONUP";
                wmsg[0x0206] = "WM_RBUTTONDBLCLK";
                wmsg[0x0207] = "WM_MBUTTONDOWN";
                wmsg[0x0208] = "WM_MBUTTONUP";
                wmsg[0x0209] = "WM_MBUTTONDBLCLK";
                wmsg[0x020A] = "WM_MOUSEWHEEL";
                wmsg[0x020E] = "WM_MOUSEHWHEEL";
                wmsg[0x0210] = "WM_PARENTNOTIFY";
                wmsg[0x0211] = "WM_ENTERMENULOOP";
                wmsg[0x0212] = "WM_EXITMENULOOP";
                wmsg[0x0213] = "WM_NEXTMENU";
                wmsg[0x0214] = "WM_SIZING";
                wmsg[0x0215] = "WM_CAPTURECHANGED";
                wmsg[0x0216] = "WM_MOVING";
                wmsg[0x0218] = "WM_POWERBROADCAST";
                wmsg[0x0219] = "WM_DEVICECHANGE";
                wmsg[0x0220] = "WM_MDICREATE";
                wmsg[0x0221] = "WM_MDIDESTROY";
                wmsg[0x0222] = "WM_MDIACTIVATE";
                wmsg[0x0223] = "WM_MDIRESTORE";
                wmsg[0x0224] = "WM_MDINEXT";
                wmsg[0x0225] = "WM_MDIMAXIMIZE";
                wmsg[0x0226] = "WM_MDITILE";
                wmsg[0x0227] = "WM_MDICASCADE";
                wmsg[0x0228] = "WM_MDIICONARRANGE";
                wmsg[0x0229] = "WM_MDIGETACTIVE";
                wmsg[0x0230] = "WM_MDISETMENU";
                wmsg[0x0231] = "WM_ENTERSIZEMOVE";
                wmsg[0x0232] = "WM_EXITSIZEMOVE";
                wmsg[0x0233] = "WM_DROPFILES";
                wmsg[0x0234] = "WM_MDIREFRESHMENU";
                wmsg[0x0281] = "WM_IME_SETCONTEXT";
                wmsg[0x0282] = "WM_IME_NOTIFY";
                wmsg[0x0283] = "WM_IME_CONTROL";
                wmsg[0x0284] = "WM_IME_COMPOSITIONFULL";
                wmsg[0x0285] = "WM_IME_SELECT";
                wmsg[0x0286] = "WM_IME_CHAR";
                wmsg[0x0290] = "WM_IME_KEYDOWN";
                wmsg[0x0291] = "WM_IME_KEYUP";
                wmsg[0x02A1] = "WM_MOUSEHOVER";
                wmsg[0x02A2] = "WM_NCMOUSELEAVE";
                wmsg[0x02A3] = "WM_MOUSELEAVE";
                wmsg[0x0300] = "WM_CUT";
                wmsg[0x0301] = "WM_COPY";
                wmsg[0x0302] = "WM_PASTE";
                wmsg[0x0303] = "WM_CLEAR";
                wmsg[0x0304] = "WM_UNDO";
                wmsg[0x0305] = "WM_RENDERFORMAT";
                wmsg[0x0306] = "WM_RENDERALLFORMATS";
                wmsg[0x0307] = "WM_DESTROYCLIPBOARD";
                wmsg[0x0308] = "WM_DRAWCLIPBOARD";
                wmsg[0x0309] = "WM_PAINTCLIPBOARD";
                wmsg[0x030A] = "WM_VSCROLLCLIPBOARD";
                wmsg[0x030B] = "WM_SIZECLIPBOARD";
                wmsg[0x030C] = "WM_ASKCBFORMATNAME";
                wmsg[0x030D] = "WM_CHANGECBCHAIN";
                wmsg[0x030E] = "WM_HSCROLLCLIPBOARD";
                wmsg[0x030F] = "WM_QUERYNEWPALETTE";
                wmsg[0x0310] = "WM_PALETTEISCHANGING";
                wmsg[0x0311] = "WM_PALETTECHANGED";
                wmsg[0x0312] = "WM_HOTKEY";
                wmsg[0x0317] = "WM_PRINT";
                wmsg[0x0318] = "WM_PRINTCLIENT";
                wmsg[0x0358] = "WM_HANDHELDFIRST";
                wmsg[0x035F] = "WM_HANDHELDLAST";
                wmsg[0x0380] = "WM_PENWINFIRST";
                wmsg[0x038F] = "WM_PENWINLAST";
                wmsg[0x0390] = "WM_COALESCE_FIRST";
                wmsg[0x039F] = "WM_COALESCE_LAST";
                wmsg[0x03E0] = "WM_DDE_FIRST";
                wmsg[0x03E0] = "WM_DDE_INITIATE";
                wmsg[0x03E1] = "WM_DDE_TERMINATE";
                wmsg[0x03E2] = "WM_DDE_ADVISE";
                wmsg[0x03E3] = "WM_DDE_UNADVISE";
                wmsg[0x03E4] = "WM_DDE_ACK";
                wmsg[0x03E5] = "WM_DDE_DATA";
                wmsg[0x03E6] = "WM_DDE_REQUEST";
                wmsg[0x03E7] = "WM_DDE_POKE";
                wmsg[0x03E8] = "WM_DDE_EXECUTE";
                wmsg[0x03E8] = "WM_DDE_LAST";
                wmsg[0x0400] = "WM_USER";
                wmsg[0x8000] = "WM_APP";
            }
            string mt = wmsg.ContainsKey(m.Msg) ? wmsg[m.Msg] : "";
            Debug.WriteLine(string.Format("0x{0:X4} 0x{1:X8} 0x{2:X4} 0x{3:X8} {4} {5}", m.Msg, m.HWnd.ToInt64(), m.WParam.ToInt64(), m.LParam.ToInt64(), ControlFromHwnd(m.HWnd, this) ?? "??", mt));
        }*/
    }
    public class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs(string name, object oldvalue)
        {
            PropertyName = name;
            oldValue = oldvalue;
        }
        public string PropertyName { get; set; }
        public object oldValue { get; set; }
    }
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public class SelectedPropertyChangedEventArgs : EventArgs
    {
        public SelectedPropertyChangedEventArgs(string name)
        {
            PropertyName = name;
        }
        public string PropertyName { get; set; }
    }
    public delegate void SelectedPropertyChangedEventHandler(object sender, SelectedPropertyChangedEventArgs e);
}

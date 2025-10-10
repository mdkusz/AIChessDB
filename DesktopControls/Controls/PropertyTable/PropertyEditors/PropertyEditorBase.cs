using DesktopControls.Controls.PropertyTable.Attributes;
using DesktopControls.Controls.PropertyTable.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Clase base para todos los demás editores de propiedades / 
    /// Base class for all property editors
    /// </summary>
    public class PropertyEditorBase : Panel, IComparable<PropertyEditorBase>, IEquatable<PropertyEditorBase>
    {
        protected Label _title;
        protected Panel _client;
        protected Padding _border;
        protected bool _dirty = true;
        /// <summary>
        /// Descriptor de la propiedad / 
        /// Property descriptor
        /// </summary>
        protected PropertyInfo _property;
        /// <summary>
        /// Instancia del objeto al que pertenece la propiedad / 
        /// Property owner object instance
        /// </summary>
        protected object _instance;
        /// <summary>
        /// Estilo de edición / 
        /// Edition style
        /// </summary>
        protected PropertyEditionStyle _style;
        /// <summary>
        /// Caso gramatical de los textos / 
        /// Text character case
        /// </summary>
        protected CharacterCasing _case;
        /// <summary>
        /// Activar la descripción de cada valor individual en el panel de información / 
        /// Show individual values description in the information panel
        /// </summary>
        protected bool _showValueDescription = false;
        /// <summary>
        /// Gestión de la selección / 
        /// Selection management
        /// </summary>
        protected bool _selected = false;
        protected Color _foreColor;
        protected Color _backColor;
        /// <summary>
        /// Nombre y descripción de la propiedad / 
        /// Property name and description
        /// </summary>
        protected string _description = "";
        protected string _displayname = "";
        public PropertyEditorBase() : base()
        {
            BorderStyle = BorderStyle.None;
            BorderSize = new Padding(0, 0, 0, 0);
            _client = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                TabStop = false
            };
            Controls.Add(_client);
            ValueIndex = -1;
            Size = new Size(100, 20);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(3, 3, 3, 3);
        }
        /// <summary>
        /// No mostrar el texto real / 
        /// Don't show the actual text
        /// </summary>
        public bool IsPassword { get; set; }
        /// <summary>
        /// Color de texto de los controles sin color de fondo / 
        /// Text color of the controls without background color
        /// </summary>
        public virtual Color CaptionColor
        {
            get
            {
                if (_title != null)
                {
                    return _title.ForeColor;
                }
                return ForeColor;
            }
            set
            {
                if (_title != null)
                {
                    _title.ForeColor = value;
                }
            }
        }
        /// <summary>
        /// Tamaño del borde / 
        /// Border size
        /// </summary>
        public Padding BorderSize
        {
            get
            {
                return _border;
            }
            set
            {
                _border = value;
            }
        }
        /// <summary>
        /// Título del panel / 
        /// Panel caption
        /// </summary>
        public override string Text
        {
            get
            {
                if (_title == null)
                {
                    return "";
                }
                else
                {
                    return _title.Text;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (_title != null)
                    {
                        Controls.Remove(_title);
                        _title = null;
                    }
                }
                else if (_title == null)
                {
                    _title = new Label()
                    {
                        Dock = DockStyle.Top,
                        AutoSize = true,
                        Text = value,
                        ForeColor = CaptionColor,
                        TabStop = false
                    };
                    Controls.Clear();
                    Controls.Add(_client);
                    Controls.Add(_title);
                }
                else
                {
                    _title.Text = value;
                }
            }
        }
        /// <summary>
        /// Color del borde / 
        /// Border color
        /// </summary>
        public Color LeftBorderColor { get; set; }
        public Color TopBorderColor { get; set; }
        public Color RightBorderColor { get; set; }
        public Color BottomBorderColor { get; set; }
        /// <summary>
        /// Índice para ordenar el editor de propiedades en una lista / 
        /// Index to order this property editor in a list
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Cuando la propiedad es una colección, índice del valor actual / 
        /// Current value index, when the property is a collection
        /// </summary>
        public int ValueIndex { get; set; }
        /// <summary>
        /// Nombre de la propiedad para mostrar al usuario / 
        /// User-friendly property name
        /// </summary>
        public virtual string DisplayName
        {
            get
            {
                if (_showValueDescription &&
                    (_property != null) &&
                    (_instance != null) &&
                    _property.GetValue(_instance) is ObjectWrapper)
                {
                    return (_property.GetValue(_instance) as ObjectWrapper).FriendlyName;
                }
                if (string.IsNullOrEmpty(Text))
                {
                    return _displayname;
                }
                return Text;
            }
            set
            {
                _displayname = value;
                Text = value;
            }
        }
        /// <summary>
        /// Descripción de la propiedad para mostrar al usuario / 
        /// User-friendly property description
        /// </summary>
        public virtual string Description
        {
            get
            {
                if (_showValueDescription &&
                    (_property != null) &&
                    (_instance != null) &&
                    _property.GetValue(_instance) is ObjectWrapper)
                {
                    return (_property.GetValue(_instance) as ObjectWrapper).FriendlyDescription;
                }
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        /// <summary>
        /// Modo de refresco del editor / 
        /// Editor refresh mode
        /// </summary>
        public RefreshProperties RefreshMode { get; set; }
        /// <summary>
        /// Instancia del objeto al que pertenece la propiedad / 
        /// Instance of the property owner object
        /// </summary>
        [Browsable(false)]
        public virtual object Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                if (_instance != value)
                {
                    _instance = value;
                    if (_property != null)
                    {
                        GetValue();
                    }
                }
            }
        }
        /// <summary>
        /// Dewscriptor de la propiedad / 
        /// Property descriptor
        /// </summary>
        [Browsable(false)]
        public virtual PropertyInfo Property
        {
            get
            {
                return _property;
            }
            set
            {
                if (_property != value)
                {
                    _property = value;
                    if (_property != null)
                    {
                        Name = "ed" + _property.Name;
                    }
                    ConfigureUI();
                    GetValue();
                }
            }
        }
        /// <summary>
        /// Estilo de edición / 
        /// Edition style
        /// </summary>
        public PropertyEditionStyle EditionStyle
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
            }
        }
        /// <summary>
        /// Caso gramatical del texto / 
        /// Text character casing
        /// </summary>
        public virtual CharacterCasing Case
        {
            get
            {
                return _case;
            }
            set
            {
                _case = value;
            }
        }
        /// <summary>
        /// Altura de la etiqueta de título / 
        /// Title label height
        /// </summary>
        public int TitleHeight
        {
            get
            {
                return _title == null ? 0 : _title.Height;
            }
        }
        /// <summary>
        /// Contenedor del editor / 
        /// Editor container
        /// </summary>
        public IPropertyBlockContainer ParentContainer { get; set; }
        /// <summary>
        /// Actualizar el valor de la propiedad / 
        /// Update property value
        /// </summary>
        public virtual void UpdateValue()
        {
            SetValue();
        }
        /// <summary>
        /// Refrescar el valor de la propiedad / 
        /// Refresh property value
        /// </summary>
        public virtual void RefreshValue()
        {
            GetValue();
        }
        /// <summary>
        /// Cálculo de la altura total del control / 
        /// Total control heicght calculation
        /// </summary>
        /// <param name="maxheight">
        /// Altura mínima del control / 
        /// Minimum control height
        /// </param>
        /// <returns>
        /// Altura adecuada para el control / 
        /// Suitable control height
        /// </returns>
        public virtual int CalcHeight(int minheight)
        {
            return minheight + TitleHeight + Padding.Vertical;
        }
        /// <summary>
        /// Volver a calcular la altura del editor / 
        /// Recalculate editor's height
        /// </summary>
        public virtual void RefreshHeight()
        {
            int maxheight = 0;
            foreach (Control c in _client.Controls)
            {
                maxheight = Math.Max(c.Bottom, maxheight);
            }
            SetHeight(maxheight);
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            int maxheight = 0;
            foreach (Control c in _client.Controls)
            {
                maxheight = Math.Max(c.Bottom, maxheight);
            }
            Control parent = ParentContainer as Control;
            if (parent != null)
            {
                return new Size(parent.ClientSize.Width -
                    (parent.Left + parent.Padding.Horizontal + parent.Margin.Horizontal),
                    CalcHeight(maxheight));
            }
            return new Size(proposedSize.Width, CalcHeight(maxheight));
        }
        /// <summary>
        /// Cambiar el estado a seleccionado / 
        /// Change the state to selected
        /// </summary>
        /// <param name="selBackColor">
        /// Color de fondo / 
        /// Background color
        /// </param>
        /// <param name="selForeColor">
        /// Color de texto / 
        /// Foregropund color
        /// </param>
        public virtual void Select(Color selBackColor, Color selForeColor)
        {
            if (!_selected)
            {
                _selected = true;
                _foreColor = ForeColor;
                _backColor = BackColor;
                BackColor = selBackColor;
                CaptionColor = selForeColor;
                ForeColor = selForeColor;
            }
        }
        /// <summary>
        /// Cambiar el estado a no seleccionado / 
        /// Change the state to not selected
        /// </summary>
        public virtual void UnSelect()
        {
            if (_selected)
            {
                _selected = false;
                ForeColor = _foreColor;
                CaptionColor = _foreColor;
                BackColor = _backColor;
            }
        }
        /// <summary>
        /// IComparable: Usado para ordenar en listas / 
        /// IComparable: Used to order in a list
        /// </summary>
        /// <param name="other">
        /// Editor con el que se compara / 
        /// Editor to compare with
        /// </param>
        /// <returns>
        /// 0: =, 1: >, -1: <
        /// </returns>
        public int CompareTo(PropertyEditorBase other)
        {
            return Index.CompareTo(other.Index);
        }
        /// <summary>
        /// IEquatable: Para comprobar si dos editores son iguales / 
        /// IEquatable: Check if thwo editors are equal
        /// </summary>
        /// <param name="other">
        /// Editor con el que se compara / 
        /// Editor to compare with
        /// </param>
        /// <returns>
        /// True si son iguales / 
        /// True if they are equal
        /// </returns>
        public bool Equals(PropertyEditorBase other)
        {
            return DisplayName.Equals(other.DisplayName);
        }
        /// <summary>
        /// Elimina todos los controles / 
        /// Remove all controls
        /// </summary>
        protected void ClearControls()
        {
            GetControls(true);
        }
        /// <summary>
        /// Texto del contenedor de controles / 
        /// Control container caption
        /// </summary>
        protected string PanelBaseText
        {
            get
            {
                return Text;
            }
        }
        /// <summary>
        /// Añadir un control al contenedor / 
        /// Add a control to the container
        /// </summary>
        /// <param name="control">
        /// Nuevo control / 
        /// New control
        /// </param>
        protected void AddControl(Control control)
        {
            _client.Controls.Add(control);
        }
        /// <summary>
        /// Eliminar un control del contenedor / 
        /// Remove a control from the container
        /// </summary>
        /// <param name="control">
        /// Control a eliminar / 
        /// Control to remove
        /// </param>
        protected void RemoveControl(Control control)
        {
            _client.Controls.Remove(control);
        }
        /// <summary>
        /// Comprobar si existe un control / 
        /// Check if a control exists
        /// </summary>
        /// <param name="control">
        /// Control a buscar / 
        /// Control to find
        /// </param>
        /// <returns>
        /// True si el contenedor contiene el control / 
        /// True if the container contains the control
        /// </returns>
        protected bool ContainsControl(Control control)
        {
            return _client.Controls.Contains(control);
        }
        /// <summary>
        /// Calcular la altura óptima del editor / 
        /// Calculate editor optimum height
        /// </summary>
        protected virtual void SetHeight(int maxheight)
        {
            Height = CalcHeight(maxheight);
        }
        /// <summary>
        /// Configuración de los controles de la interfaz de usuario / 
        /// User interface controls configuration
        /// </summary>
        protected virtual void ConfigureUI()
        {
            if (ParentContainer != null)
            {
                Font = ParentContainer.EditorsFont;
            }
            if (_property == null)
            {
                Text = "";
                Description = "";
            }
            else
            {
                IPropertyConfigurationProvider cfgprovider = _instance as IPropertyConfigurationProvider;
                PropertyIndexAttribute pi = _property.GetCustomAttribute(typeof(PropertyIndexAttribute)) as PropertyIndexAttribute;
                if (pi != null)
                {
                    Index = pi.Index;
                }
                string displayname = cfgprovider == null ? "" : cfgprovider.DisplayName(_property.Name, ValueIndex);
                DisplayNameAttribute dn = _property.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                if (string.IsNullOrEmpty(displayname) && (dn == null))
                {
                    displayname = _property.Name;
                }
                else if (string.IsNullOrEmpty(displayname))
                {
                    displayname = dn.DisplayName;
                }
                Text = displayname;
                if (!string.IsNullOrEmpty(displayname))
                {
                    _displayname = displayname;
                }
                Description = cfgprovider == null ? "" : cfgprovider.Description(_property.Name, ValueIndex);
                DescriptionAttribute da = _property.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (string.IsNullOrEmpty(Description) && (dn != null))
                {
                    Description = da.Description;
                }
                RefreshPropertiesAttribute rp = _property.GetCustomAttribute<RefreshPropertiesAttribute>();
                if (rp != null)
                {
                    RefreshMode = rp.RefreshProperties;
                }
                else
                {
                    RefreshMode = RefreshProperties.None;
                }
                EditorCharCaseAttribute cc = _property.GetCustomAttribute<EditorCharCaseAttribute>();
                if (cfgprovider != null)
                {
                    Case = cfgprovider.CharCase(_property.Name, cc != null ? cc.Case : CharacterCasing.Normal);
                }
                else if (cc != null)
                {
                    Case = cc.Case;
                }
                else
                {
                    Case = CharacterCasing.Normal;
                }
                ShowPropertyValueDescriptionAttribute spd = _property.GetCustomAttribute<ShowPropertyValueDescriptionAttribute>();
                if (spd != null)
                {
                    _showValueDescription = spd.ShowPropertyValueDescription;
                }
            }
        }
        /// <summary>
        /// El valor de la propiedad ha cambiado / 
        /// The property value has changed
        /// </summary>
        /// <param name="oldvalue">
        /// Anterior valor de la propiedad / 
        /// Property old value
        /// </param>
        protected void OnPropertyChanged(object oldvalue)
        {
            ParentContainer?.PropertyChanged(this, oldvalue);
        }
        /// <summary>
        /// Obtener el valor de la propiedad / 
        /// Get property value
        /// </summary>
        protected virtual void GetValue() { }
        /// <summary>
        /// Establecer el valor de la propiedad / 
        /// Set property value
        /// </summary>
        protected virtual void SetValue() { }
        /// <summary>
        /// Establecer el estilo de edición / 
        /// Set edition style
        /// </summary>
        protected virtual void SetStyle() { }
        /// <summary>
        /// Ejecutar el editor especial de valores de la propiedad / 
        /// Launch the special property values editor
        /// </summary>
        protected virtual void EditorInvoked() { }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (((_style & (PropertyEditionStyle.Click | PropertyEditionStyle.Left)) == PropertyEditionStyle.Click)
                && (e.Button == MouseButtons.Right))
            {
                EditorInvoked();
            }
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            PropertyEditionStyle ps = _style & (PropertyEditionStyle.DoubleClick | PropertyEditionStyle.Left);
            if (((ps == PropertyEditionStyle.DoubleClick) &&
                (e.Button == MouseButtons.Right)) ||
                ((ps == (PropertyEditionStyle.DoubleClick | PropertyEditionStyle.Left)) &&
                (e.Button == MouseButtons.Left)))
            {
                EditorInvoked();
            }
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (_dirty)
            {
                SetValue();
            }
            ParentContainer?.PropertyLostFocus(this);
        }
        protected override void OnEnter(EventArgs e)
        {
            if (_dirty)
            {
                GetValue();
            }
            base.OnEnter(e);
            foreach (Control c in _client.Controls)
            {
                if (c.TabStop)
                {
                    c.Focus();
                    break;
                }
            }
            ParentContainer?.PropertyGotFocus(this);
        }
        /// <summary>
        /// Lista de todos los controles / 
        /// All contained controls list
        /// </summary>
        /// <param name="remove">
        /// True para eliminar los controles / 
        /// True to remove the controls
        /// </param>
        /// <returns>
        /// Lista de controles / 
        /// Control list
        /// </returns>
        protected List<Control> GetControls(bool remove = false)
        {
            List<Control> lctl = new List<Control>();
            int ix = 0;
            while (ix < _client.Controls.Count)
            {
                lctl.Add(_client.Controls[ix]);
                if (remove)
                {
                    _client.Controls.Remove(_client.Controls[ix]);
                }
                else
                {
                    ix++;
                }
            }
            return lctl;
        }
        /// <summary>
        /// Sobrecargado para pintar los bordes / 
        /// Overriden to draw borders
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_border.Top > 0)
            {
                using (Pen p = new Pen(TopBorderColor, _border.Top))
                {
                    e.Graphics.DrawLine(p, 0, 0, Width, 0);
                }
            }
            if (_border.Left > 0)
            {
                using (Pen p = new Pen(LeftBorderColor, _border.Left))
                {
                    e.Graphics.DrawLine(p, 0, 0, 0, Height);
                }
            }
            if (_border.Right > 0)
            {
                using (Pen p = new Pen(RightBorderColor, _border.Right))
                {
                    e.Graphics.DrawLine(p, Width - 1, 0, Width - 1, Height);
                }
            }
            if (_border.Bottom > 0)
            {
                using (Pen p = new Pen(BottomBorderColor, _border.Bottom))
                {
                    e.Graphics.DrawLine(p, 0, Height - 1, Width, Height - 1);
                }
            }
        }
    }
}

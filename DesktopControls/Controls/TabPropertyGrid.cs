using DesktopControls.PropertyTools;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Extensión del control PropertyGrid / 
    /// PropertyGrid extension
    /// </summary>
    public class TabPropertyGrid : PropertyGrid, IPropertyEditorContainer
    {
        private int _noprocess = 0;
        public TabPropertyGrid() : base()
        {
            RefreshKey = Keys.Return;
            EditorDropDownKey = Keys.F1;
        }
        /// <summary>
        /// Evento que se dispara cuando se cambia el tamaó del panel de description / 
        /// Event fired when the description panel size changes
        /// </summary>
        public event EventHandler CommentSizeChanged;
        /// <summary>
        /// Se dispara cuando cambia el contenido o tamaño del control / 
        /// Fires when the control content or size changes
        /// </summary>
        public event PropertyEditorChangedEventHandler EditorSizeChanged;
        [Category("Control Keys")]
        [DisplayName("Refresh Key")]
        [Description("Use this key to refresh the property grid, even though the property value has not changed")]
        public Keys RefreshKey { get; set; }
        [Category("Control Keys")]
        [DisplayName("Dropdown Key")]
        [Description("Use this key to show the property editor, if any")]
        public Keys EditorDropDownKey { get; set; }

        /// <summary>
        /// Altura que necesitan las propiedades para que el control no muestre barras de scroll vertical / 
        /// Height needed for properties so there is no need of vertical scroll bars
        /// </summary>
        /// <returns>
        /// Altura mínima en píxels / 
        /// Minimum height in pixels
        /// </returns>
        public int GetMinimumHeight()
        {
            int height = Height;
            try
            {
                if (SelectedObject != null)
                {
                    Control grid = GetPropertyGrid();
                    var field = grid.GetType().GetField("allGridEntries",
                            BindingFlags.NonPublic |
                            BindingFlags.Instance);
                    List<GridItem> items = (field.GetValue(grid) as IEnumerable).Cast<GridItem>().ToList();
                    GridItem item = items[0];
                    int itemheight = 0;
                    using (Graphics oGraphics = Graphics.FromHwnd(Handle))
                    {
                        // Escalar según el tamaño de fuente del control
                        // Scale according to the control font size
                        itemheight = (int)oGraphics.MeasureString(item.Label, Font).Height + 3;
                    }
                    // Calcular la altura para que quepan todos los elementos sin barras de scroll
                    // Calculate height so that all elements can be shown without scroll bars
                    height = 8 + (itemheight * items.Count);
                    foreach (Control c in Controls)
                    {
                        // Añadir la altura del resto de componentes
                        // Add the height of other components
                        if (!c.GetType().Name.EndsWith("PropertyGridView"))
                        {
                            height += c.Height;
                        }
                    }
                }
            }
            catch { }
            return height;
        }
        /// <summary>
        /// Tamaño y posición del panel de descripción / 
        /// Size and position of the description panel
        /// </summary>
        /// <returns>
        /// Rectangle con posición y tamaño / 
        /// Size and location Rectangle
        /// </returns>
        public Rectangle GetCommentRectangle()
        {
            foreach (Control c in Controls)
            {
                if (c.GetType().Name.EndsWith("DocComment"))
                {
                    return new Rectangle(c.Location, c.Size);
                }
            }
            return new Rectangle();
        }
        /// <summary>
        /// Cambiar la posición del splitter que separa los nobres de las propiedades de los editores / 
        /// Change the position of the splitter separating property names from editors
        /// </summary>
        /// <param name="xpos">
        /// Nueva posición del splitter / 
        /// Splitter new position
        /// </param>
        public void MoveSplitterTo(int xpos)
        {
            try
            {
                Control grid = GetPropertyGrid();
                if (grid != null)
                {
                    MethodInfo methodInfo = grid.GetType().GetMethod("MoveSplitterTo",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance);
                    methodInfo.Invoke(grid, new object[] { xpos });
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Calcula el ancho mínimo necesario para mostrar todas las propiedades / 
        /// Calculate minimum width to show all the properties
        /// </summary>
        /// <returns>
        /// Ancho mínimo del panel de nombres de propiedades o -1 si no se puede calcular / 
        /// Minimum width of the property names panel or -1 if it cannot be calculated
        /// </returns>
        public int EstimatePropertyMaxWidth()
        {
            try
            {
                if (SelectedObject != null)
                {
                    Control grid = GetPropertyGrid();
                    if (grid != null)
                    {
                        var field = grid.GetType().GetField("allGridEntries",
                            BindingFlags.NonPublic |
                            BindingFlags.Instance);
                        var entries = (field.GetValue(grid) as IEnumerable).Cast<GridItem>().ToList();
                        int MaxWidth = 0;
                        using (Graphics oGraphics = Graphics.FromHwnd(Handle))
                        {
                            int CurWidth = 0;

                            foreach (GridItem oItem in entries)
                            {
                                if (oItem.GridItemType == GridItemType.Property)
                                {
                                    // estimar el ancho según el tamaño de fuente
                                    // Estimate witdth according font size
                                    CurWidth = (int)oGraphics.MeasureString(oItem.Label, Font).Width + 16;
                                    if (CurWidth > MaxWidth)
                                    {
                                        MaxWidth = CurWidth;
                                    }
                                }
                            }
                        }
                        return MaxWidth;
                    }
                }
            }
            catch { }
            return -1;
        }
        /// <summary>
        /// Busca el componente del control que contiene los editores de propiedades / 
        /// Find the control component containing the property editors
        /// </summary>
        /// <returns>
        /// Control PropertyGridView / 
        /// PropertyGridView control
        /// </returns>
        private Control GetPropertyGrid()
        {
            for (int ix = 0; ix < Controls.Count; ix++)
            {
                if (Controls[ix].GetType().Name == "PropertyGridView")
                {
                    return Controls[ix];
                }
            }
            return null;
        }
        /// <summary>
        /// Sobrecargado para que se pueda navegar por las propiedades con el tabulador / 
        /// Overriden to allow navigate properties using tab key
        /// </summary>
        /// <param name="forward">
        /// Dirección de tabulación / 
        /// Tab direction
        /// </param>
        /// <returns>
        /// true si se ha gestionado el evento / 
        /// true if event handled
        /// </returns>
        protected override bool ProcessTabKey(bool forward)
        {
            if (!ProcessTabKeyBody(forward, true))
            {
                return base.ProcessTabKey(forward);
            }
            return true;
        }
        protected bool ProcessTabKeyBody(bool forward, bool onlyproperties)
        {
            Control grid = GetPropertyGrid();
            if (grid != null)
            {
                // Todas las propiedades y títulos de categoría
                // All properties and category titles
                var field = grid.GetType().GetField("allGridEntries",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);
                List<GridItem> entries = (field.GetValue(grid) as IEnumerable).Cast<GridItem>().ToList();
                // Selección actual
                // Current selection
                int index = entries.IndexOf(SelectedGridItem);

                if (forward)
                {
                    GridItem next;
                    if (index < entries.Count - 1)
                    {
                        // Siguiente elemento
                        // Next element
                        next = entries[index + 1];
                    }
                    else
                    {
                        index = 0;
                        // Primer elemento
                        // First element
                        next = entries[0];
                    }
                    // Asegurarse de que es una propiedad
                    // Ensure it is a property
                    if (onlyproperties)
                    {
                        while ((index < entries.Count - 1) && (next.GridItemType != GridItemType.Property))
                        {
                            index++;
                            next = entries[index];
                        }
                    }
                    next.Select();
                    // Seleccionar el valor de la propiedad, no el nombre
                    // Select the property value, not the name
                    SendKeys.Send("{TAB}");
                    return true;
                }
                else if (!forward)
                {
                    GridItem prev;
                    if (index > 0)
                    {
                        // Elemento anterior
                        // Previous element
                        prev = entries[index - 1];
                    }
                    else
                    {
                        index = entries.Count - 1;
                        // Último elemento
                        // Last element
                        prev = entries[index];
                    }
                    // Asegurarse de que es una propiedad
                    // Ensure it is a property
                    if (onlyproperties)
                    {
                        while ((index > 0) && (prev.GridItemType != GridItemType.Property))
                        {
                            index--;
                            prev = entries[index];
                        }
                    }
                    prev.Select();
                    // Seleccionar el valor de la propiedad, no el nombre
                    // Select the property value, not the name
                    SendKeys.Send("+{TAB}");
                    return true;
                }
            }
            return false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            PropertyDescriptor pd = SelectedGridItem?.PropertyDescriptor;
            if (pd != null)
            {
                if (keyData == RefreshKey)
                {
                    try
                    {
                        // Refrescar el grid, aunque no se haya cambiado la propiedad
                        // Refresh grid, even though the property value has not changed
                        SelectedGridItem.PropertyDescriptor.SetValue(SelectedObject, SelectedGridItem.Value);
                        Refresh();
                    }
                    catch { }
                }
                else if (keyData == EditorDropDownKey)
                {
                    // Desplegar el editor de propiedades 
                    // Show the property editor
                    UITypeEditor ed = pd.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
                    if (ed != null)
                    {
                        object val = ed.EditValue((ITypeDescriptorContext)SelectedGridItem,
                            (IServiceProvider)SelectedGridItem,
                            pd.GetValue(SelectedObject));
                        pd.SetValue(SelectedObject, val);
                        Refresh();
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            if ((e.AffectedComponent != null)
                && e.AffectedComponent.GetType().Name.EndsWith("DocComment"))
            {
                CommentSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        protected override void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
        {
            base.OnSelectedGridItemChanged(e);
            if (e.NewSelection != null)
            {
                // Esto no me gusta nada, pero funciona
                // This sucks, but it works
                try
                {
                    if (e.NewSelection.Expandable)
                    {
                        switch (_noprocess)
                        {
                            case 0:
                                _noprocess++;
                                ProcessTabKeyBody(true, false);
                                break;
                            case 1:
                                _noprocess++;
                                ProcessTabKeyBody(false, false);
                                break;
                            case 2:
                                EditorSizeChanged?.Invoke(this,
                                    new PropertyEditorChangedEventArgs(
                                        e.NewSelection.Expanded ? PropertyEditorChangedReason.Expanded : PropertyEditorChangedReason.Collapsed,
                                        e.NewSelection));
                                _noprocess++;
                                ProcessTabKeyBody(true, e.NewSelection.Expanded);
                                break;
                            default:
                                _noprocess = 0;
                                break;

                        }
                    }
                    else
                    {
                        switch (_noprocess)
                        {
                            case 1:
                                _noprocess++;
                                ProcessTabKeyBody(false, false);
                                break;
                            default:
                                _noprocess = 0;
                                break;
                        }
                    }
                }
                catch
                {
                    _noprocess = 0;
                }
            }
        }
    }
}

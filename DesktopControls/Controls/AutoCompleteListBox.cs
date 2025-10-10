using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// ListBox para gestionar una lista de elementos de autocompletado
    /// Autocomplete manager listbox control
    /// </summary>
    public class AutoCompleteListBox : ListBox
    {
        /// <summary>
        /// Modo de selección de elementos de autocompletado / 
        /// Autocomplete item selection mode
        /// </summary>
        public enum ItemSelectionMode
        {
            StartsWith,
            Contains
        }
        public AutoCompleteListBox() : base()
        {
            MaxVisibleItems = 32;
            AutoCompleteElements = new List<string>();
        }
        /// <summary>
        /// Cantidad máxima de elementos visibles / 
        /// Maximum visible items
        /// </summary>
        public int MaxVisibleItems { get; set; }
        /// <summary>
        /// Lista de elementos de autocompletado / 
        /// AutoComplete elements list
        /// </summary>
        public List<string> AutoCompleteElements { get; set; }
        /// <summary>
        /// Modo de selección de elementos de autocompletado / 
        /// Autocomplete item selection mode
        /// </summary>
        public ItemSelectionMode Mode { get; set; }
        /// <summary>
        /// Mostrar la lista de autocompletado / 
        /// Show autocomplete list
        /// </summary>
        /// <param name="text">
        /// Texto usado como referencia para seleccionar elementos de la lista / 
        /// Text used as reference to select elements from the list
        /// </param>
        /// <param name="location">
        /// Coordenadas para mostrar la lista / 
        /// Coordinates to show the list
        /// </param>
        /// <param name="maxx">
        /// Coordenada X máxima de la zona en la que se mostrará la lista / 
        /// Maximum X coordinate of the area where the list will be shown
        /// </param>
        /// <returns>
        /// True si se muestra la lista / 
        /// True if the list is shown
        /// </returns>
        public bool ShowAutoComplete(string text, Point location, int maxx)
        {
            if (!string.IsNullOrEmpty(text) && (AutoCompleteElements != null))
            {
                Items.Clear();
                int maxw = 0;
                int maxh = ItemHeight;
                using (Graphics gr = Graphics.FromHwnd(Handle))
                {
                    foreach (string element in AutoCompleteElements)
                    {
                        if (((Mode == ItemSelectionMode.StartsWith) && element.ToLower().StartsWith(text.ToLower())) ||
                            ((Mode == ItemSelectionMode.Contains) && element.ToLower().Contains(text.ToLower())))
                        {
                            Items.Add(element);
                            SizeF ms = gr.MeasureString(element, Font);
                            maxw = Math.Max(maxw, (int)ms.Width + SystemInformation.VerticalScrollBarWidth);
                            maxh = Math.Max(maxh, (int)ms.Height);
                        }
                    }
                }
                if (Items.Count > 0)
                {
                    if (location.X + maxw > maxx)
                    {
                        location.X = maxx - maxw;
                    }
                    Location = location;
                    ItemHeight = maxh;
                    BringToFront();
                    if (!Visible)
                    {
                        Show();
                    }
                    if (SelectedIndex < 0)
                    {
                        SelectedIndex = 0;
                    }
                    Height = Math.Min(MaxVisibleItems, Items.Count + 1) * ItemHeight;
                    Width = maxw;
                    return true;
                }
                else
                {
                    Visible = false;
                }
            }
            return false;
        }
        /// <summary>
        /// Mostrar la lista de autocompletado / 
        /// Show autocomplete list
        /// </summary>
        /// <param name="location">
        /// Coordenadas para mostrar la lista / 
        /// Coordinates to show the list
        /// </param>
        /// <param name="maxx">
        /// Coordenada X máxima de la zona en la que se mostrará la lista / 
        /// Maximum X coordinate of the area where the list will be shown
        /// </param>
        /// <returns>
        /// True si se muestra la lista / 
        /// True if the list is shown
        /// </returns>
        public bool ShowAutoComplete(Point location, int maxx)
        {
            if (AutoCompleteElements != null)
            {
                Items.Clear();
                int maxw = 0;
                int maxh = ItemHeight;
                using (Graphics gr = Graphics.FromHwnd(Handle))
                {
                    foreach (string element in AutoCompleteElements)
                    {
                        Items.Add(element);
                        SizeF ms = gr.MeasureString(element, Font);
                        maxw = Math.Max(maxw, (int)ms.Width + SystemInformation.VerticalScrollBarWidth);
                        maxh = Math.Max(maxh, (int)ms.Height);
                    }
                }
                if (Items.Count > 0)
                {
                    if (location.X + maxw > maxx)
                    {
                        location.X = maxx - maxw;
                    }
                    Location = location;
                    ItemHeight = maxh;
                    BringToFront();
                    if (!Visible)
                    {
                        Show();
                    }
                    if (SelectedIndex < 0)
                    {
                        SelectedIndex = 0;
                    }
                    Height = Math.Min(MaxVisibleItems, Items.Count + 1) * ItemHeight;
                    Width = maxw;
                    return true;
                }
                else
                {
                    Visible = false;
                }
            }
            return false;
        }
        /// <summary>
        /// Actualizar y mostrar la lista de autocompletado / 
        /// Update items and show autocomplete list
        /// </summary>
        /// <param name="text">
        /// Texto usado como referencia para seleccionar elementos de la lista / 
        /// Text used as reference to select elements from the list
        /// </param>
        /// <returns>
        /// True si se muestra la lista / 
        /// True if the list is shown
        /// </returns>
        public bool UpdateAutoComplete(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                for (int ix = Items.Count - 1; ix >= 0; ix--)
                {
                    if (!(((Mode == ItemSelectionMode.StartsWith) && Items[ix].ToString().ToLower().StartsWith(text.ToLower())) ||
                        ((Mode == ItemSelectionMode.Contains) && Items[ix].ToString().ToLower().Contains(text.ToLower()))))
                    {
                        Items.RemoveAt(ix);
                    }
                }
                if (Items.Count > 0)
                {
                    BringToFront();
                    if (!Visible)
                    {
                        Show();
                        SelectedIndex = 0;
                    }
                    Height = Math.Min(MaxVisibleItems, Items.Count + 1) * ItemHeight;
                    return true;
                }
                else
                {
                    Visible = false;
                }
            }
            return false;
        }
        /// <summary>
        /// Seleccionar un item de la lista / 
        /// Select an item from the list
        /// </summary>
        /// <param name="text">
        /// Texto del itema a seleccionar / 
        /// Text of the item to select
        /// </param>
        /// <param name="defaultselectindex">
        /// Indice por defecto para seleccionar en caso de no encontrar el texto / 
        /// Default index to select if the text is not found
        /// </param>
        /// <returns>
        /// True si se ha encontrado el texto / 
        /// True if the text has been found
        /// </returns>
        public bool MatchText(string text, int defaultselectindex = -1)
        {
            for (int ix = 0; ix < Items.Count; ix++)
            {
                if (Items[ix].ToString().ToLower() == text.ToLower())
                {
                    SelectedIndex = ix;
                    return true;
                }
            }
            if (defaultselectindex >= 0)
            {
                SelectedIndex = 0;
            }
            return false;
        }
    }
}

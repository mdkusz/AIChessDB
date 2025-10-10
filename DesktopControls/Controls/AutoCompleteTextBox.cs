using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static DesktopControls.Properties.Resources;

namespace DesktopControls.Controls
{
    /// <summary>
    /// TextBox con capacidad de autocompletar en mono o multi línea / 
    /// TextBox with autocomplete in single or multiline
    /// Based on the article DIY Intellisense by yetanotherchris published in CodeProject
    /// </summary>
    public class AutoCompleteTextBox : TextBox
    {
        [DllImport("user32.dll")]
        protected static extern bool GetCaretPos(out Point lpPoint);
        protected AutoCompleteListBox _acList = new AutoCompleteListBox()
        {
            Visible = false
        };
        protected Control _mainContainer = null;
        protected List<string> _matches = new List<string>();
        protected int _longestMatch = 0;
        protected string _currentMatch = "";
        protected bool _processKeyUp = false;
        protected int _minHeight = 0;
        public AutoCompleteTextBox() : base()
        {
            _minHeight = Height;
        }
        public event SuggestionEventHandler ExpandSuggestion;
        /// <summary>
        /// Contenedor del control / 
        /// Control container
        /// </summary>
        [Browsable(false)]
        public Control ParentContainer
        {
            get
            {
                return _mainContainer;
            }
            set
            {
                _mainContainer = value;
                if (_mainContainer != null)
                {
                    if (!_mainContainer.Controls.Contains(_acList))
                    {
                        _mainContainer.Controls.Add(_acList);
                    }
                }
            }
        }
        /// <summary>
        /// Modo de selección de elementos de autocompletado / 
        /// Autocomplete item selection mode
        /// </summary>
        public AutoCompleteListBox.ItemSelectionMode Mode
        {
            get
            {
                return _acList.Mode;
            }
            set
            {
                _acList.Mode = value;
            }
        }
        /// <summary>
        /// Lista de cadenas para detener el autocompletado / 
        /// Stop autocomplete strings list
        /// </summary>
        [Browsable(false)]
        public List<string> StopStrings { get; set; }
        /// <summary>
        /// Lista de elementos de autocompletado / 
        /// AutoComplete elements list
        /// </summary>
        [Browsable(false)]
        public List<string> AutoCompleteElements
        {
            get
            {
                return _acList.AutoCompleteElements;
            }
            set
            {
                if (value != null)
                {
                    value.Sort();
                    _acList.AutoCompleteElements = value;
                    BuildMatches(value);
                }
                else
                {
                    _acList.AutoCompleteElements.Clear();
                }
            }
        }
        /// <summary>
        /// Comprobar si la lista de autocompletado está visible / 
        /// Check if the autocomplete list is visible
        /// </summary>
        [Browsable(false)]
        public bool AutocompleteVisible
        {
            get
            {
                return _acList.Visible;
            }
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
            return _acList.MatchText(text, defaultselectindex);
        }
        /// <summary>
        /// Forzar la aparición o desaparición de la lista de autocompletado / 
        /// Force the appearance or hidding of the autocomplete list
        /// </summary>
        public void ShowAutocompleteList()
        {
            if (AutoCompleteElements.Count == 0)
            {
                _acList.Hide();
            }
            else
            {
                int charpos = SelectionStart;
                Point point = GetCaretPos();
                point.Y += (int)Math.Ceiling(Font.GetHeight()) + 2;
                point.X += 2;
                point = _mainContainer.PointToClient(PointToScreen(point));

                _acList.ShowAutoComplete(point, _mainContainer.Width);
            }
        }
        /// <summary>
        /// Preparar la lista interna de elementos de autocompletado / 
        /// Prepare the internal autocomplete elements list
        /// </summary>
        /// <param name="autocompletelist">
        /// Lista original de elementos de autocompletado / 
        /// Original autocomplete elements list
        /// </param>
        protected virtual void BuildMatches(List<string> autocompletelist)
        {
            _longestMatch = 0;
            _matches = new List<string>();
            for (int ix = 0; ix < autocompletelist.Count; ix++)
            {
                _longestMatch = Math.Max(_longestMatch, autocompletelist[ix].Length - 1);
                _matches.Add(autocompletelist[ix].ToLower());
            }
        }
        /// <summary>
        /// Buscar la coincidencia más larga en la lista de autocompletado / 
        /// Find the longest match in the autocomplete list
        /// </summary>
        /// <param name="text">
        /// Texto de referencia / 
        /// Reference text
        /// </param>
        /// <returns>
        /// Fragmento del texto de referencia que se ajusta al elemento de autocompletado / 
        /// Reference text fragment that fits the autocomplete element
        /// </returns>
        protected virtual string GetLongestMatch(string text)
        {
            if ((StopStrings == null) || (SelectionStart < text.Length) || !StopStrings.Any(text.EndsWith))
            {
                int start = Math.Max(0, SelectionStart - _longestMatch);
                if (SelectionStart - start > 0)
                {
                    string txt = text.Substring(start, Math.Min(_longestMatch, SelectionStart - start));
                    while (!string.IsNullOrEmpty(txt))
                    {
                        if (((Mode == AutoCompleteListBox.ItemSelectionMode.StartsWith) && _matches.Any(m => m.StartsWith(txt.ToLower()))) ||
                            ((Mode == AutoCompleteListBox.ItemSelectionMode.Contains) && _matches.Any(m => m.Contains(txt.ToLower()))))
                        {
                            return txt;
                        }
                        if (txt.Length > 1)
                        {
                            txt = txt.Substring(1, txt.Length - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// Obtener el punto de inserción del cursor / 
        /// Get the caret insertion point
        /// </summary>
        /// <returns>
        /// Coordenadas del punto de inserción del cursor / 
        /// Caret insertion point coordinates
        /// </returns>
        protected Point GetCaretPos()
        {
            Point pt = Point.Empty;
            if (GetCaretPos(out pt))
            {
                return pt;
            }
            else
            {
                return Point.Empty;
            }
        }
        /// <summary>
        /// Ajustar la altura del control al texto multilinea / 
        /// Adjust the control height to multiline text
        /// </summary>
        protected void ChangeMultilineHeight()
        {
            if (Multiline)
            {
                if (Lines != null)
                {
                    int h = (Math.Max(_minHeight, Font.Height + 2) * (Lines.Length + 1)) + SystemInformation.HorizontalScrollBarHeight;
                    if (h != Height)
                    {
                        Height = h;
                    }
                }
                else
                {
                    int h = Math.Max(_minHeight, Font.Height + 2) + SystemInformation.HorizontalScrollBarHeight;
                    if (h != Height)
                    {
                        Height = h;
                    }
                }
            }
        }
        protected override void OnMultilineChanged(EventArgs e)
        {
            base.OnMultilineChanged(e);
            ChangeMultilineHeight();
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            ChangeMultilineHeight();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            _processKeyUp = false;
            // Find the position of the caret
            int charpos = SelectionStart;
            Point point = GetCaretPos();
            point.Y += (int)Math.Ceiling(Font.GetHeight()) + 2;
            point.X += 2; // for Courier, may need a better method
            point = _mainContainer.PointToClient(PointToScreen(point));

            if (e.KeyCode == Keys.Escape)
            {
                if (_acList.Visible)
                {
                    _acList.Hide();
                    e.Handled = true;
                }
                else
                {
                    base.OnKeyDown(e);
                }
            }
            else if (e.KeyData == Keys.Enter)
            {
                if (_acList.Visible)
                {
                    if (_acList.SelectedIndex >= 0)
                    {
                        string item = _acList.Items[_acList.SelectedIndex].ToString();
                        string text = item.Substring(_currentMatch.Length);
                        SuggestionEventArgs esg = null;
                        if (Multiline)
                        {
                            if (item.EndsWith(CTK_SUGGESTION))
                            {
                                esg = new SuggestionEventArgs(new List<string>() { item });
                                ExpandSuggestion?.Invoke(this, esg);
                                esg.Suggestion[0] = esg.Suggestion[0].Substring(_currentMatch.Length);
                            }
                            else
                            {
                                esg = new SuggestionEventArgs(new List<string>() { text });
                            }
                            int line = GetLineFromCharIndex(charpos);
                            int column = charpos - GetFirstCharIndexFromLine(line);
                            List<string> lines = new List<string>(Lines);
                            if (lines.Count == 0)
                            {
                                lines.AddRange(esg.Suggestion);
                            }
                            else if (lines[line].Length > column)
                            {
                                lines[line] = lines[line].Substring(0, column) + string.Join(" ", esg.Suggestion) + lines[line].Substring(column);
                            }
                            else
                            {
                                lines[line] = lines[line] + esg.Suggestion[0];
                                esg.Suggestion.RemoveAt(0);
                                if (esg.Suggestion.Count > 0)
                                {
                                    lines.AddRange(esg.Suggestion);
                                }
                            }
                            Lines = lines.ToArray();
                        }
                        else
                        {
                            if (item.EndsWith(CTK_SUGGESTION))
                            {
                                esg = new SuggestionEventArgs(new List<string>() { item });
                                ExpandSuggestion?.Invoke(this, esg);
                                esg.Suggestion[0] = esg.Suggestion[0].Substring(_currentMatch.Length);
                            }
                            else
                            {
                                esg = new SuggestionEventArgs(new List<string>() { text });
                            }
                            text = string.Join(" ", esg.Suggestion);
                            if (text.Length > charpos)
                            {
                                Text = Text.Substring(0, charpos) + text + Text.Substring(charpos);
                            }
                            else
                            {
                                Text = Text.Substring(0, charpos) + text;
                            }
                        }
                        SelectionStart = Text.Length;
                        _acList.Hide();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                }
                else
                {
                    _processKeyUp = true;
                    base.OnKeyDown(e);
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (_acList.Visible)
                {
                    if (_acList.SelectedIndex > 0)
                    {
                        _acList.SelectedIndex--;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else
                {
                    _processKeyUp = true;
                    base.OnKeyDown(e);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (_acList.Visible)
                {
                    if (_acList.SelectedIndex < _acList.Items.Count - 1)
                    {
                        _acList.SelectedIndex++;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else
                {
                    _processKeyUp = true;
                    base.OnKeyDown(e);
                }
            }
            else
            {
                _processKeyUp = true;
                base.OnKeyDown(e);
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (_processKeyUp)
            {
                _processKeyUp = false;
                int charpos = SelectionStart;
                Point point = GetCaretPos();
                point.Y += (int)Math.Ceiling(Font.GetHeight()) + 2;
                point.X += 2;
                point = _mainContainer.PointToClient(PointToScreen(point));

                _currentMatch = GetLongestMatch(Text);
                if (string.IsNullOrEmpty(_currentMatch))
                {
                    if ((SelectionStart >= Text.Length) && (AutoCompleteElements.Count > 0))
                    {
                        _acList.ShowAutoComplete(point, _mainContainer.Width);
                    }
                    else
                    {
                        _acList.Hide();
                    }
                }
                else if (!_acList.Visible)
                {
                    _acList.ShowAutoComplete(_currentMatch, point, _mainContainer.Width);
                }
                else
                {
                    if (!_acList.UpdateAutoComplete(_currentMatch))
                    {
                        _currentMatch = "";
                    }
                    else
                    {
                        _acList.MatchText(_currentMatch, 0);
                    }
                }
                base.OnKeyUp(e);
            }
            else
            {
                e.Handled = true;
            }
        }
    }
    public class SuggestionEventArgs : EventArgs
    {
        public SuggestionEventArgs(List<string> suggestion)
        {
            Suggestion = suggestion;
        }
        public List<string> Suggestion { get; set; }
    }
    public delegate void SuggestionEventHandler(object sender, SuggestionEventArgs e);
}

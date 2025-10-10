using DesktopControls.Controls.PropertyTable.PropertyEditors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.Attributes
{
    /// <summary>
    /// Atributo para establecer el tipo de editor de una propiedad
    /// Attribute to set the property editor type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyEditorAttribute : Attribute
    {
        public string backColor;
        public string foreColor;
        public string borderColor;
        public string editStyle;
        public string padding;
        public string border;
        protected Color[] _borderColors;
        public bool password;
        public PropertyEditorAttribute(Type type)
        {
            BuildEditor(type);
        }
        /// <summary>
        /// Obtiene o establece el editor
        /// Get or set the editor
        /// </summary>
        public PropertyEditorBase Editor { get; protected set; }
        /// <summary>
        /// Construye el editor
        /// Build the editor
        /// </summary>
        /// <param name="type">
        /// Tipo del editor
        /// Editor type
        /// </param>
        protected void BuildEditor(Type type)
        {
            Editor = type.GetConstructor(Type.EmptyTypes).Invoke(null) as PropertyEditorBase;
        }
        public bool IsPassword
        {
            get
            {
                return password;
            }
        }
        public Color EditorBackColor
        {
            get
            {
                if (!string.IsNullOrEmpty(backColor))
                {
                    return Color.FromName(backColor);
                }
                return Color.Empty;
            }
        }
        public Color EditorForeColor
        {
            get
            {
                if (!string.IsNullOrEmpty(foreColor))
                {
                    return Color.FromName(foreColor);
                }
                return Color.Empty;
            }
        }
        public PropertyEditionStyle EditorStyle
        {
            get
            {
                if (!string.IsNullOrEmpty(editStyle))
                {
                    PropertyEditionStyle pe;
                    if (Enum.TryParse(editStyle, true, out pe))
                    {
                        return pe;
                    }
                }
                return PropertyEditionStyle.None;
            }
        }
        public Padding EditorBorder
        {
            get
            {
                return ParsePadding(border);
            }
        }
        public Padding EditorPadding
        {
            get
            {
                return ParsePadding(padding);
            }
        }
        public Color LeftBorderColor
        {
            get
            {
                return GetBorderColor(0);
            }
        }
        public Color TopBorderColor
        {
            get
            {
                return GetBorderColor(1);
            }
        }
        public Color RightBorderColor
        {
            get
            {
                return GetBorderColor(2);
            }
        }
        public Color BottomBorderColor
        {
            get
            {
                return GetBorderColor(3);
            }
        }
        private Padding ParsePadding(string padding)
        {
            if (!string.IsNullOrEmpty(padding))
            {
                string[] pad = padding.Split(';');
                int[] pd = new int[pad.Length];
                for (int ix = 0; ix < pad.Length; ix++)
                {
                    if (!int.TryParse(pad[ix], out pd[ix]))
                    {
                        return Padding.Empty;
                    }
                }
                if (pd.Length >= 4)
                {
                    return new Padding(pd[0], pd[1], pd[2], pd[3]);
                }
                else
                {
                    return new Padding(pd[0]);
                }
            }
            return Padding.Empty;
        }
        private Color GetBorderColor(int cindex)
        {
            if (_borderColors == null)
            {
                if (string.IsNullOrEmpty(borderColor))
                {
                    return Color.Empty;
                }
                string[] colors = borderColor.Split(';');
                _borderColors = new Color[colors.Length];
                for (int ix = 0; ix < colors.Length; ix++)
                {
                    _borderColors[ix] = Color.FromName(colors[ix]);
                }
            }
            if ((cindex >= 0) && (cindex < _borderColors.Length))
            {
                return _borderColors[cindex];
            }
            return Color.Empty;
        }
    }
}

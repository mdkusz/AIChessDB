using System;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// RichTextBox que conserva el formato al cambiar de fuente.Trabaja con RTF.
    /// RichTextBox that keeps the format when changing the font. Works with RTF.
    /// </summary>
    public class RTFTextBox : RichTextBox
    {
        protected string _rtf;

        public string FixedRtf
        {
            get
            {
                return _rtf;
            }
            set
            {
                _rtf = value;
                Rtf = value;
            }
        }
        public int Bold { get; set; }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Rtf = _rtf;
        }
    }
}

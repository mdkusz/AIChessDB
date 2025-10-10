using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    /// <summary>
    /// Estilo de celda de tipo fecha y hora / 
    /// Date and time cell style
    /// </summary>
    public class DGVDateAndTimePickerCellStyle : DataGridViewCellStyle
    {
        public DGVDateAndTimePickerCellStyle() : base()
        {
            DateFormat = DateTimePickerFormat.Short;
        }
        /// <summary>
        /// Formato de fecha / 
        /// Date format
        /// </summary>
        public DateTimePickerFormat DateFormat { get; set; }
        /// <summary>
        /// Formato personalizado / 
        /// Curstom format
        /// </summary>
        public string CustomFormat { get; set; }
    }
}

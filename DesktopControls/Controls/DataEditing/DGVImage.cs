using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public class DGVImage : PictureBox, IDataGridViewEditingControl
    {
        public DGVImage() : base()
        {
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
        }

        public int EditingControlRowIndex { get; set; }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            return !dataGridViewWantsInputKey;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public DataGridView EditingControlDataGridView { get; set; }

        public bool EditingControlValueChanged { get; set; }

        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
    }
}

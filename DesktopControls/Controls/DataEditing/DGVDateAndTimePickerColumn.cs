using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public class DGVDateAndTimePickerColumn : DataGridViewColumn
    {
        protected DGVDateAndTimePickerCell _cellTemplate;
        public DGVDateAndTimePickerColumn() : base() { }
        public DGVDateAndTimePickerColumn(DataGridViewCell cellTemplate) : base(cellTemplate)
        {
            _cellTemplate = cellTemplate as DGVDateAndTimePickerCell;
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                if (_cellTemplate == null)
                {
                    _cellTemplate = new DGVDateAndTimePickerCell();
                }
                return _cellTemplate;
            }
            set
            {
                if (value is DGVDateAndTimePickerCell)
                {
                    _cellTemplate = value as DGVDateAndTimePickerCell;
                }
            }
        }
        public override DataGridViewTriState Resizable
        {
            get
            {
                return DataGridViewTriState.True;
            }
            set { }
        }
    }
}

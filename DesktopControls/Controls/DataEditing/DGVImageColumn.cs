using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public class DGVImageColumn : DataGridViewColumn
    {
        protected DGVImageCell _cellTemplate;
        public DGVImageColumn() : base()
        {
        }
        public DGVImageColumn(DataGridViewCell cellTemplate) : base(cellTemplate)
        {
            _cellTemplate = cellTemplate as DGVImageCell;
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                if (_cellTemplate == null)
                {
                    _cellTemplate = new DGVImageCell();
                }
                return _cellTemplate;
            }
            set
            {
                if (value is DGVImageCell)
                {
                    _cellTemplate = value as DGVImageCell;
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

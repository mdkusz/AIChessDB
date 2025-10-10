using GlobalCommonEntities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public class DGVComboBoxColumn : DataGridViewColumn
    {
        protected DGVComboBoxCell _cellTemplate;
        public DGVComboBoxColumn() : base()
        {
        }
        public DGVComboBoxColumn(DataGridViewCell cellTemplate) : base(cellTemplate)
        {
            _cellTemplate = cellTemplate as DGVComboBoxCell;
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                if (_cellTemplate == null)
                {
                    _cellTemplate = new DGVComboBoxCell();
                }
                return _cellTemplate;
            }
            set
            {
                if (value is DGVComboBoxCell)
                {
                    _cellTemplate = value as DGVComboBoxCell;
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
        [Browsable(false)]
        public List<IUIIdentifier> SelectionItems { get; set; }
    }
}

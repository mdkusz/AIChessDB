using GlobalCommonEntities.Interfaces;
using System;
using System.Windows.Forms;

namespace DesktopControls.PropertyTools
{
    public partial class PEListBoxWithCommandButtons : UserControl
    {
        public PEListBoxWithCommandButtons()
        {
            InitializeComponent();
        }
        public IPropertyCommandManager CommandManager { get; set; }
        public string PropertyName { get; set; }
        public PropertyEditorListBox ItemsList
        {
            get
            {
                return peListBox;
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            PropertyCommandEventArgs pev = new PropertyCommandEventArgs()
            {
                PropertyName = PropertyName,
                CommandIndex = 0
            };
            CommandManager.PropertyCommand(this, pev);
            if (!pev.Cancel)
            {
                peListBox.Selection = pev.CommandResult;
            }
            peListBox.WInSrv.CloseDropDown();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            PropertyCommandEventArgs pev = new PropertyCommandEventArgs()
            {
                PropertyName = PropertyName,
                CommandIndex = 1
            };
            CommandManager.PropertyCommand(this, pev);
            if (!pev.Cancel)
            {
                peListBox.Selection = null;
            }
            peListBox.WInSrv.CloseDropDown();
        }
    }
}

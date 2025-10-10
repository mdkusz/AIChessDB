using System;
using System.Windows.Forms;

namespace DesktopControls.Controls.DataEditing
{
    public partial class ComboBoxWithCommandButton : UserControl
    {
        public ComboBoxWithCommandButton()
        {
            InitializeComponent();
        }
        public ComboBox Values
        {
            get
            {
                return cbValues;
            }
        }
        public event EventHandler CommandClicked;

        private void bCommand_Click(object sender, EventArgs e)
        {
            CommandClicked?.Invoke(this, e);
        }
    }
}

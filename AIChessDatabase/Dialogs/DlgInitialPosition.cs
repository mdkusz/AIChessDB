using System;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    public partial class DlgInitialPosition : Form
    {
        private string _initial = "";
        public DlgInitialPosition()
        {
            InitializeComponent();
            Text = TTL_INITIALPOS;
        }
        public string Board
        {
            get
            {
                return cfBoard.Board;
            }
            set
            {
                cfBoard.Board = value;
                _initial = value;
            }
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            try
            {
                cfBoard.Board = new string('0', 64);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            try
            {
                cfBoard.Board = _initial;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

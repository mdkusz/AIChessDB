using System.ComponentModel;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Dialogs
{
    public partial class DlgInputText : Form
    {
        public DlgInputText()
        {
            InitializeComponent();
            bOK.Text = BTN_OK;
            bCancel.Text = BTN_Cancel;
        }
        [Browsable(false)]
        public string Prompt
        {
            get
            {
                return lPrompt.Text;
            }
            set
            {
                lPrompt.Text = value;
            }
        }
        public string InputText
        {
            get
            {
                return txtInput.Text;
            }
            set
            {
                txtInput.Text = value;
            }
        }
    }
}

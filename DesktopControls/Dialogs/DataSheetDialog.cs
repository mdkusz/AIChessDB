using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Dialogs
{
    /// <summary>
    /// Generic dialog box to edit IUIDataSheet objects
    /// </summary>
    public partial class DataSheetDialog : Form
    {
        private bool _allowclose = true;

        public DataSheetDialog()
        {
            InitializeComponent();
            bOK.Text = BTN_OK;
            bCancel.Text = BTN_Cancel;
        }
        /// <summary>
        /// Input editor control factory
        /// </summary>
        [Browsable(false)]
        public IInputEditorFactory EditorFactory
        {
            get
            {
                return flpSettings.EditorFactory;
            }
            set
            {
                flpSettings.EditorFactory = value;
            }
        }
        /// <summary>
        /// Data sheet being edited. Null for editing application configuration.
        /// </summary>
        [Browsable(false)]
        public IUIDataSheet DataSheet { get; set; }
        private void bOK_Click(object sender, EventArgs e)
        {
            // Ensure last changes are committed
            flpSettings.RefreshEditor(DataSheet, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
            if (DataSheet.Completed)
            {
                _allowclose = true;
                if (Modal)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    Close();
                }
            }
            else
            {
                MessageBox.Show(ERR_RequiredData);
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            _allowclose = true;
            if (Modal)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                Close();
            }
        }

        private void DataSheetDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !(_allowclose || DataSheet.Completed);
        }

        private void DataSheetDialog_Shown(object sender, EventArgs e)
        {
            try
            {
                _allowclose = DataSheet.Completed;
                EditorFactory.BorderSize = new Padding(0, 0, 0, 1);
                EditorFactory.BottomBorderColor = Color.LightGray;
                EditorFactory.EditorBackColor = SystemColors.Window;
                EditorFactory.EditorForeColor = SystemColors.WindowText;
                EditorFactory.BlockHeaderBackColor = SystemColors.ActiveCaption;
                EditorFactory.BlockHeaderForeColor = SystemColors.ActiveCaptionText;
                flpSettings.Controls.Clear();
                DataSheet.RefreshEditor += flpSettings.RefreshEditor;
                for (int ix = 0; ix < DataSheet.Properties.Count; ix++)
                {
                    PropertyEditorInfo pi = DataSheet.Properties[ix];
                    flpSettings.RefreshEditor(DataSheet, new RefreshEditorEventArgs(pi, ix, EditorContainerOperation.Add));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void flpSettings_ResizeParent(object sender, EventArgs e)
        {
            ClientSize = new Size(ClientSize.Width, flpSettings.PreferredSize.Height + panel1.Height);
        }
    }
}

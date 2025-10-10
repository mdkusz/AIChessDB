using AIChessDatabase.Setup;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    /// <summary>
    /// Dialog to configure application settings.
    /// </summary>
    public partial class DlgAppConfiguration : Form, IUIRemoteControlElement
    {
        private IInputEditorFactory _editorFactory = null;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;
        private bool _allowclose = true;
        public DlgAppConfiguration()
        {
            InitializeComponent();
            Text = TTL_APPCONFIGURATION;
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
        }
        /// <summary>
        /// IUIRemoteControlElement: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return Text; } }
        /// <summary>
        /// IUIRemoteControlElement: Get first-level UI elements.
        /// </summary>
        /// <returns>
        /// List of first-level UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElements()
        {
            return _collector.GetUIElements(null);
        }
        /// <summary>
        /// IUIRemoteControlElement: Get all-level UI elements.
        /// </summary>
        /// <returns>
        /// List of all UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetAllUIElements()
        {
            return _collector.GetAllUIElements(null);
        }
        /// <summary>
        /// IUIRemoteControlElement: Get the child UI elements of that on a specific path.
        /// </summary>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElementChildren(string path)
        {
            return _collector.GetUIElementChildren(null, path);
        }
        /// <summary>
        /// IUIRemoteControlElement: Highlight a UI element for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to highlight.
        /// </param>
        /// <param name="seconds">
        /// Duration in seconds to highlight the element.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        public void HighlightUIElement(string path, int seconds, string mode)
        {
            _interactor.HighLight(path, seconds, mode);
        }
        /// <summary>
        /// IUIRemoteControlElement: Show a tooltip for a UI element with a comment for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to comment.
        /// </param>
        /// <param name="title">
        /// The title of the notification balloon.
        /// </param>
        /// <param name="comment">
        /// Comment to display in the tooltip.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        /// <param name="seconds">
        /// Seconds to display the tooltip.
        /// </param>
        public void CommentUIElement(string path, string title, string comment, string mode, int seconds)
        {
            _interactor.ShowBalloon(path, title, comment, mode, seconds);
        }
        /// <summary>
        /// IUIRemoteControlElement: Invoke an action on a UI element at the specified path.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to invoke the action on.
        /// </param>
        /// <param name="action">
        /// Action name to invoke on the UI element, such as "click", "double-click", etc.
        /// </param>
        /// <returns>
        /// String value or null
        /// </returns>
        /// <remarks>
        /// The format for action is: action[:param]
        /// </remarks>
        public object InvokeElementAction(string path, string action)
        {
            return _interactor.Invoke(path, action);
        }
        /// <summary>
        /// Apply changes asynchronously to the data sheet.
        /// </summary>
        public bool AsyncDataSheet { get; set; }
        /// <summary>
        /// Dependecy provider for creating objects.
        /// </summary>
        [Browsable(false)]
        public IDependencyProvider DependencyProvider { get; set; }
        /// <summary>
        /// Data sheet being edited. Null for editing application configuration.
        /// </summary>
        [Browsable(false)]
        public UIDataSheet DataSheet { get; set; }
        /// <summary>
        /// Flag to indicate if the dialog was cancelled when non-modal.
        /// </summary>
        public bool Cancelled { get; private set; }
        private async void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure last changes are committed
                flpSettings.RefreshEditor(DataSheet, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                if (DataSheet.Completed)
                {
                    _allowclose = true;
                    Cancelled = false;
                    if (AsyncDataSheet)
                    {
                        string error = await DataSheet.ApplyChangesAsync();
                        if (!string.IsNullOrEmpty(error))
                        {
                            throw new Exception(error);
                        }
                    }
                    else
                    {
                        DataSheet.ApplyChanges();
                    }
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
                    MessageBox.Show(ERR_CompleteAllFields, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            _allowclose = true;
            Cancelled = true;
            if (Modal)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                Close();
            }
        }

        private void DlgAppConfiguration_Shown(object sender, EventArgs e)
        {
            try
            {
                if (DataSheet == null)
                {
                    ConfigurationSettingsDataSheet settings = new ConfigurationSettingsDataSheet(DependencyProvider);
                    DataSheet = settings;
                }
                _allowclose = DataSheet.Completed;
                _editorFactory = DependencyProvider.GetObjects(nameof(IInputEditorFactory), DataSheet).FirstOrDefault()?.Implementation() as IInputEditorFactory;
                _editorFactory.BorderSize = new Padding(0, 0, 0, 1);
                _editorFactory.BottomBorderColor = Color.LightGray;
                _editorFactory.EditorBackColor = SystemColors.Window;
                _editorFactory.EditorForeColor = SystemColors.WindowText;
                _editorFactory.BlockHeaderBackColor = SystemColors.ActiveCaption;
                _editorFactory.BlockHeaderForeColor = SystemColors.ActiveCaptionText;
                flpSettings.EditorFactory = _editorFactory;
                flpSettings.Controls.Clear();
                DataSheet.RefreshEditor += flpSettings.RefreshEditor;
                SuspendLayout();
                for (int ix = 0; ix < DataSheet.Properties.Count; ix++)
                {
                    PropertyEditorInfo pi = DataSheet.Properties[ix];
                    flpSettings.RefreshEditor(DataSheet, new RefreshEditorEventArgs(pi, ix, EditorContainerOperation.Add));
                }
                ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DlgAppConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_allowclose && !DataSheet.Completed;
        }

        private void flpSettings_ResizeParent(object sender, EventArgs e)
        {
            ClientSize = new Size(ClientSize.Width, flpSettings.PreferredSize.Height + panel1.Height);
        }
    }
}

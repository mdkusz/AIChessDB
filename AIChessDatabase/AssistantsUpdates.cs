using AIAssistants.Interfaces;
using AIChessDatabase.AI;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase
{
    public partial class AssistantsUpdates : Form, IUIRemoteControlElement, IProgressMonitor
    {
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;
        private IDependencyProvider _provider = null;
        private IInputEditorFactory _editorFactory = null;
        private UpdateAssistantsConfiguration _config = null;
        private IUIDataSheet _currentDS = null;
        public AssistantsUpdates()
        {
            InitializeComponent();
            Text = TTL_AssistantUpdates;
            bFont.ToolTipText = TTP_Font;
            bEditDoc.ToolTipText = TTP_EditDocument;
            bAddDoc.ToolTipText = TTP_AddDocuments;
            bApplyUpdates.ToolTipText = TTP_ApplyUpdates;
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
        /// Application automation manager
        /// </summary>
        [Browsable(false)]
        public IAppAutomation automationManager { get; set; }
        /// <summary>
        /// Dependency provider for the application
        /// </summary>
        [Browsable(false)]
        public IDependencyProvider DependencyProvider
        {
            get
            {
                return _provider;
            }
            set
            {
                _provider = value;
                _editorFactory = _provider?.GetObjects(nameof(IInputEditorFactory))?.FirstOrDefault()?.Implementation() as IInputEditorFactory;
                _editorFactory.BorderSize = new Padding(0, 0, 0, 1);
                _editorFactory.BottomBorderColor = Color.LightGray;
                _editorFactory.EditorBackColor = SystemColors.Window;
                _editorFactory.EditorForeColor = SystemColors.WindowText;
                _editorFactory.BlockHeaderBackColor = SystemColors.ActiveCaption;
                _editorFactory.BlockHeaderForeColor = SystemColors.ActiveCaptionText;
                flpEditor.EditorFactory = _editorFactory;
            }
        }
        /// <summary>
        /// IProgressMonitor: Sets the total step number of the operation.
        /// </summary>
        /// <param name="steps">
        /// Number of steps to complete the operation.
        /// </param>
        public void SetTotalSteps(int steps)
        {
            BeginInvoke(new Action(() =>
            {
                pgUpdates.Step = 1;
                pgUpdates.Maximum = steps;
                pgUpdates.Value = 0;
            }));
        }
        /// <summary>
        /// IProgressMonitor: Restets the progress monitor to its initial state.
        /// </summary>
        /// <param name="sender">
        /// Object that is resetting the progress monitor, typically the operation initiator.
        /// </param>
        public void Reset(object sender)
        {
            BeginInvoke(new Action(() =>
            {
                pgUpdates.Value = 0;
            }));
        }
        /// <summary>
        /// IProgressMonitor: Advances the progress monitor.
        /// </summary>
        /// <param name="c">
        /// Number of steps to advance the progress monitor.
        /// </param>
        public void Step(int c = 1)
        {
            BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < c; i++)
                {
                    pgUpdates.PerformStep();
                }
            }));
        }
        /// <summary>
        /// IProgressMonitor: Process finished event.
        /// </summary>
        /// <param name="sender">
        /// Finishing object, typically the operation initiator.
        /// </param>
        public void Stop(object sender)
        {
            BeginInvoke(new Action(() =>
            {
                pgUpdates.Value = 0;
            }));
        }
        private void ManageButtons()
        {
            bFont.Enabled = true;
            bEditDoc.Enabled = (rtvStructure.SelectedNode.Tag is UpdateDocument) || (rtvStructure.SelectedNode.Tag is AssistantUpdates);
            bAddDoc.Enabled = rtvStructure.SelectedNode.Tag is List<UpdateDocument>;
            bApplyUpdates.Enabled = true;
        }
        private void AssistantsUpdates_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = Icon.Clone() as Icon;
                _config = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                    ConfigurationManager.AppSettings[SETTING_updateConfiguration])));
                _config.AppAutomation = automationManager;
                _config.DataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
                _config.ProgressMonitor = this;
                rtvStructure.TreeObject = _config;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bFont_Click(object sender, EventArgs e)
        {
            try
            {
                fontdlg.Font = flpEditor.Font;
                if (fontdlg.ShowDialog() == DialogResult.OK)
                {
                    flpEditor.Font = fontdlg.Font;
                    rtvStructure.Font = fontdlg.Font;
                    if (_currentDS != null)
                    {
                        _currentDS.RefreshEditor -= flpEditor.RefreshEditor;
                    }
                    _currentDS = null;
                    rtvStructure_AfterSelect(sender, new TreeViewEventArgs(rtvStructure.SelectedNode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rtvStructure_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node?.Tag is IUIDataSheet)
                {
                    IUIDataSheet ds = e.Node.Tag as IUIDataSheet;
                    if ((ds != null) && (ds != _currentDS))
                    {
                        if (_currentDS != null)
                        {
                            // Ensure last changes are committed
                            flpEditor.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                            _currentDS.RefreshEditor -= flpEditor.RefreshEditor;
                        }
                        _currentDS = ds;
                        flpEditor.Controls.Clear();
                        for (int ix = 0; ix < ds.Properties.Count; ix++)
                        {
                            PropertyEditorInfo pi = ds.Properties[ix];
                            flpEditor.RefreshEditor(ds, new RefreshEditorEventArgs(pi, ix, EditorContainerOperation.Add));
                        }
                        ds.RefreshEditor += flpEditor.RefreshEditor;
                    }
                }
                else
                {
                    if (_currentDS != null)
                    {
                        // Ensure last changes are committed
                        flpEditor.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                        _currentDS.RefreshEditor -= flpEditor.RefreshEditor;
                    }
                    _currentDS = null;
                    flpEditor.Controls.Clear();
                }
                ManageButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bApplyUpdates_Click(object sender, EventArgs e)
        {
            try
            {
                bFont.Enabled = false;
                bAddDoc.Enabled = false;
                bEditDoc.Enabled = false;
                bApplyUpdates.Enabled = false;
                rtvStructure.Enabled = false;
                flpEditor.Enabled = false;
                if (_currentDS != null)
                {
                    // Ensure last changes are committed
                    flpEditor.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                    _currentDS.RefreshEditor -= flpEditor.RefreshEditor;
                }
                _currentDS = null;
                List<string> errors = await _config.PerformUpdates();
                if ((errors != null) && (errors.Count > 0))
                {
                    throw new Exception(string.Join(Environment.NewLine, errors));
                }
                File.WriteAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                    ConfigurationManager.AppSettings[SETTING_updateConfiguration]),
                    JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true }));
                MessageBox.Show(MSG_AllUpdated);
                rtvStructure_AfterSelect(sender, new TreeViewEventArgs(rtvStructure.SelectedNode));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                rtvStructure.Enabled = true;
                flpEditor.Enabled = true;
                ManageButtons();
            }
        }

        private void bEditDoc_Click(object sender, EventArgs e)
        {
            try
            {
                string path = rtvStructure.SelectedNode.Tag is UpdateDocument ?
                    Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath],
                    (rtvStructure.SelectedNode.Tag as UpdateDocument).FileName) :
                    Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath],
                    (rtvStructure.SelectedNode.Tag as AssistantUpdates).Instructions);
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true
                    });
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 1155)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "rundll32.exe",
                            Arguments = $"shell32.dll,OpenAs_RunDLL \"{path}\"",
                            UseShellExecute = false
                        });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bAddDoc_Click(object sender, EventArgs e)
        {
            try
            {
                List<UpdateDocument> docs = rtvStructure.SelectedNode.Tag as List<UpdateDocument>;
                if (docs != null)
                {
                    ofDlg.InitialDirectory = ConfigurationManager.AppSettings[SETTING_dataPath];
                    ofDlg.Filter = FILTER_AllFiles;
                    if (ofDlg.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string file in ofDlg.FileNames)
                        {
                            if (string.Compare(Path.GetDirectoryName(file), Path.GetFullPath(ConfigurationManager.AppSettings[SETTING_dataPath]), true) != 0)
                            {
                                File.Copy(file, Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], Path.GetFileName(file)), true);
                            }
                            UpdateDocument doc = new UpdateDocument()
                            {
                                FileName = Path.GetFileName(file),
                                Update = true
                            };
                            if (!docs.Contains(doc))
                            {
                                docs.Add(doc);
                                rtvStructure.SelectedNode.Nodes.Add(rtvStructure.CreateNodeForObject(doc));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

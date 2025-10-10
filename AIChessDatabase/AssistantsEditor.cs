using AIAssistants.Interfaces;
using AIAssistants.JSON;
using AIChessDatabase.AI;
using DesktopControls.Controls;
using DesktopControls.Dialogs;
using DesktopControls.Tools;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase
{
    /// <summary>
    /// Editor to create, modify and remove API objects like assistants or assistant tools.
    /// </summary>
    public partial class AssistantsEditor : Form, IUIRemoteControlElement
    {
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;
        private IDependencyProvider _provider = null;
        private IInputEditorFactory _editorFactory = null;
        private IUIDataSheet _currentDS = null;
        private List<IUIDataSheet> _modifiedDS = new List<IUIDataSheet>();
        private bool _allowClose = true;
        public AssistantsEditor()
        {
            InitializeComponent();
            Text = TTL_Assistants;
            bFont.ToolTipText = TTP_Font;
            bNewItem.ToolTipText = TTP_NewItem;
            bDelItem.ToolTipText = TTP_DeleteElement;
            bSaveItem.ToolTipText = TTP_SaveChanges;
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
        public string FriendlyName { get { return FNAME_AIAssistantsEditor; } }
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
            }
        }
        private void ManageButtons()
        {
            bNewItem.Enabled = cbItemType.SelectedIndex >= 0;
            bDelItem.Enabled = rtvStructure.TreeObject != null;
            bSaveItem.Enabled = _modifiedDS.Count > 0;
        }
        /// <summary>
        /// Create a suitable UIDataSheet to edit a given IAPIElement.
        /// </summary>
        /// <param name="element">
        /// IAPIElement to edit.
        /// </param>
        /// <param name="bnew">
        /// The element is a new one.
        /// </param>
        /// <returns>
        /// UIDataSheet to edit the element, or null if no suitable sheet is found.
        /// </returns>
        private async Task<UIDataSheet> GetDataSheetForElement(IAPIElement element, bool bnew)
        {
            UIDataSheet ds = null;
            IAPIPlayer player = element as IAPIPlayer;
            if (player != null)
            {
                if (player is IStorableElement)
                {
                    await ((IStorableElement)player).ModifyStoredElement();
                }
                ds = new APIPlayerData(player);
            }
            if (ds == null)
            {
                IFilePackageManager pack = element as IFilePackageManager;
                if (pack != null)
                {
                    if (pack is IStorableElement)
                    {
                        await ((IStorableElement)pack).ModifyStoredElement();
                    }
                    ds = new FilePackageManagerData(pack);
                }
            }
            if (ds == null)
            {
                ISpeechManager speech = element as ISpeechManager;
                if (speech != null)
                {
                    if (speech is IStorableElement)
                    {
                        await ((IStorableElement)speech).ModifyStoredElement();
                    }
                    ds = new SpeechManagerData(speech);
                }
            }
            return ds;
        }
        private void AssistantsEditor_Load(object sender, EventArgs e)
        {
            Icon = Icon.Clone() as Icon;
            cbAPIManager.Items.Clear();
            cbInstance.Items.Clear();
            cbOtherInstances.Items.Clear();
            cbItemType.Items.Clear();
            if (automationManager != null)
            {
                foreach (ObjectWrapper<IAPIManager> api in automationManager.ApiManagers)
                {
                    cbAPIManager.Items.Add(api);
                }
                ControlHelpers.SetToolBarComboBoxWidth(cbAPIManager);
                if (cbAPIManager.Items.Count == 1)
                {
                    cbAPIManager.SelectedIndex = 0;
                }
            }
        }
        private void SelectObjects(object sender, SelectObjectsEventArgs e)
        {
            try
            {
                ObjectSelectionDialog dlg = new ObjectSelectionDialog()
                {
                    SelectionTarget = sender as ISelectionObjectProvider,
                    AllowRemoveContainer = false,
                    EditorInfo = e.Property
                };
                dlg.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (sender is IUIDataSheet ds)
                {
                    if (!_modifiedDS.Contains(ds))
                    {
                        _modifiedDS.Add(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ManageButtons();
            }
        }
        private void UnHookDataSheet(IUIDataSheet ds)
        {
            if (ds != null)
            {
                // Ensure last changes are committed
                flpEditor.RefreshEditor(ds, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                ds.RefreshEditor -= flpEditor.RefreshEditor;
                ds.PropertyChanged -= PropertyChanged;
                if (ds is ISelectionObjectProvider)
                {
                    (ds as ISelectionObjectProvider).SelectionUIInvoked -= SelectObjects;
                }
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
                    UnHookDataSheet(_currentDS);
                    _currentDS = null;
                    rtvStructure_AfterSelect(sender, new TreeViewEventArgs(rtvStructure.SelectedNode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbAPIManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cbInstance.Items.Clear();
                cbOtherInstances.Items.Clear();
                cbItemType.Items.Clear();
                if (cbAPIManager.SelectedIndex >= 0)
                {
                    IAPIManager api = (cbAPIManager.SelectedItem as ObjectWrapper<IAPIManager>).TypedImplementation;
                    foreach (ObjectWrapper<string> player in api.AvailablePLayerTypes)
                    {
                        string type = player.TypedImplementation;
                        Type itype = api.NameToTYpe(type);
                        if (typeof(IDocumentAnalyzer).IsAssignableFrom(itype))
                        {
                            cbItemType.Items.Add(player);
                        }
                    }
                    foreach (ObjectWrapper<string> tool in api.AvailableElementTypes)
                    {
                        string type = tool.TypedImplementation;
                        if ((type == nameof(ISpeechManager)) || (type == nameof(IFilePackageManager)))
                        {
                            cbItemType.Items.Add(tool);
                        }
                    }
                    ControlHelpers.SetToolBarComboBoxWidth(cbItemType);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ManageButtons();
            }
        }

        private async void cbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cbInstance.Items.Clear();
                cbOtherInstances.Items.Clear();
                UnHookDataSheet(_currentDS);
                _currentDS = null;
                rtvStructure.TreeObject = null;
                flpEditor.Controls.Clear();
                toolStrip1.Enabled = false;
                using (OverlayScope ov = new OverlayScope(this, MSG_LoadingInstances))
                {
                    if ((cbAPIManager.SelectedIndex >= 0) && (cbItemType.SelectedIndex >= 0))
                    {
                        IAPIManager api = (cbAPIManager.SelectedItem as ObjectWrapper<IAPIManager>).TypedImplementation;
                        string type = (cbItemType.SelectedItem as ObjectWrapper<string>).TypedImplementation;
                        List<ObjectWrapper<IAPIElement>> list = null;
                        foreach (ObjectWrapper<IAPIElement> ow in await api.GetCurrentElements(type))
                        {
                            if (list == null)
                            {
                                list = new List<ObjectWrapper<IAPIElement>>();
                            }
                            list.Add(ow);
                        }
                        if (list != null)
                        {
                            cbInstance.Items.AddRange(list.ToArray());
                        }
                        list = null;
                        foreach (ObjectWrapper<IAPIElement> ow in await api.GetOtherElements(type))
                        {
                            if (list == null)
                            {
                                list = new List<ObjectWrapper<IAPIElement>>();
                            }
                            list.Add(ow);
                        }
                        if (list != null)
                        {
                            cbOtherInstances.Items.AddRange(list.ToArray());
                        }
                        ControlHelpers.SetToolBarComboBoxWidth(cbInstance);
                        ControlHelpers.SetToolBarComboBoxWidth(cbOtherInstances);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                toolStrip1.Enabled = true;
                ManageButtons();
            }
        }

        private async void cbInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbInstance.SelectedIndex >= 0)
                {
                    UIDataSheet ds = await GetDataSheetForElement((cbInstance.SelectedItem as ObjectWrapper<IAPIElement>).TypedImplementation, false);
                    if ((ds != null) && (ds != _currentDS))
                    {
                        UnHookDataSheet(_currentDS);
                        ds.RefreshEditor += flpEditor.RefreshEditor;
                        ds.PropertyChanged += PropertyChanged;
                        if (ds is ISelectionObjectProvider)
                        {
                            (ds as ISelectionObjectProvider).SelectionUIInvoked += SelectObjects;
                        }
                        rtvStructure.TreeObject = ds;
                        rtvStructure.SelectedNode = rtvStructure.Nodes[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ManageButtons();
            }
        }
        private async void cbOtherInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbOtherInstances.SelectedIndex >= 0)
                {
                    UIDataSheet ds = await GetDataSheetForElement((cbOtherInstances.SelectedItem as ObjectWrapper<IAPIElement>).TypedImplementation, false);
                    if ((ds != null) && (_currentDS != ds))
                    {
                        UnHookDataSheet(_currentDS);
                        ds.RefreshEditor += flpEditor.RefreshEditor;
                        ds.PropertyChanged += PropertyChanged;
                        if (ds is ISelectionObjectProvider)
                        {
                            (ds as ISelectionObjectProvider).SelectionUIInvoked += SelectObjects;
                        }
                        if (!_modifiedDS.Contains(ds))
                        {
                            _modifiedDS.Add(ds);
                        }
                        rtvStructure.TreeObject = ds;
                        rtvStructure.SelectedNode = rtvStructure.Nodes[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ManageButtons();
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
                        UnHookDataSheet(_currentDS);
                        _currentDS = ds;
                        flpEditor.Controls.Clear();
                        foreach (PropertyEditorInfo pi in ds.Properties)
                        {
                            IInputEditorBase dsp = _editorFactory.CreateEditor(pi, ds, flpEditor);
                            flpEditor.Controls.Add(dsp as Control);
                        }
                    }
                }
                else
                {
                    UnHookDataSheet(_currentDS);
                    _currentDS = null;
                    flpEditor.Controls.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bNewItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((cbAPIManager.SelectedIndex >= 0) && (cbItemType.SelectedIndex >= 0))
                {
                    IAPIManager api = (cbAPIManager.SelectedItem as ObjectWrapper<IAPIManager>).TypedImplementation;
                    string type = (cbItemType.SelectedItem as ObjectWrapper<string>).TypedImplementation;
                    IAPIElement newElement = await api.CreateElement(type, null);
                    UIDataSheet ds = await GetDataSheetForElement(newElement, true);
                    if ((ds != null) && (ds != _currentDS))
                    {
                        UnHookDataSheet(_currentDS);
                        ds.RefreshEditor += flpEditor.RefreshEditor;
                        ds.PropertyChanged += PropertyChanged;
                        if (ds is ISelectionObjectProvider)
                        {
                            (ds as ISelectionObjectProvider).SelectionUIInvoked += SelectObjects;
                        }
                        _modifiedDS.Add(ds);
                        rtvStructure.TreeObject = ds;
                        rtvStructure.SelectedNode = rtvStructure.Nodes[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ManageButtons();
            }
        }

        private async void bDelItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((cbAPIManager.SelectedIndex >= 0) && (rtvStructure.TreeObject is UIDataSheet))
                {
                    if (MessageBox.Show(MSG_DeleteObjectFromCfgFiles, CAP_DelObject, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        IAPIManager api = (cbAPIManager.SelectedItem as ObjectWrapper<IAPIManager>).TypedImplementation;
                        FilePackageManagerData ds = rtvStructure.TreeObject as FilePackageManagerData;
                        if (ds != null)
                        {
                            // Remove files from the package manager
                            List<object> files = await (ds as ISelectionObjectProvider).GetSelectedObjects(null);
                            await (ds as ISelectionObjectProvider).RemoveSelection(files, null);
                            // Remove the package manager from the API configuration file
                            api.RemoveConfiguration(ds.PackageManager);
                            // Remove the package manager from the list of modified data sheets
                            _modifiedDS.Remove(ds);
                            UnHookDataSheet(_currentDS);
                            _currentDS = null;
                            // Remove the package manager from the play collection
                            bool updplays = false;
                            foreach (PlaySchema ps in automationManager.Plays.Plays)
                            {
                                foreach (PlayPlayer pp in ps.Players)
                                {
                                    if (pp.Assets != null)
                                    {
                                        for (int i = pp.Assets.Count - 1; i >= 0; i--)
                                        {
                                            if (pp.Assets[i].Id == ds.PackageManager.Identifier)
                                            {
                                                pp.Assets.RemoveAt(i);
                                                updplays = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (updplays)
                            {
                                automationManager.SavePlayConfiguration();
                            }
                            rtvStructure.TreeObject = null;
                            // Refresh the instance list
                            cbItemType_SelectedIndexChanged(this, EventArgs.Empty);
                        }
                        else
                        {
                            SpeechManagerData sds = rtvStructure.TreeObject as SpeechManagerData;
                            if (sds != null)
                            {
                                // Remove the speech manager from the API configuration file
                                api.RemoveConfiguration(sds.Speech);
                                // Remove the speech manager from the list of modified data sheets
                                _modifiedDS.Remove(ds);
                                UnHookDataSheet(_currentDS);
                                _currentDS = null;
                                // Remove the speech manager from the play collection
                                bool updplays = false;
                                foreach (PlaySchema ps in automationManager.Plays.Plays)
                                {
                                    foreach (PlayPlayer pp in ps.Players)
                                    {
                                        if (pp.Assets != null)
                                        {
                                            for (int i = pp.Assets.Count - 1; i >= 0; i--)
                                            {
                                                if (pp.Assets[i].Id == sds.Speech.Identifier)
                                                {
                                                    pp.Assets.RemoveAt(i);
                                                    updplays = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (updplays)
                                {
                                    automationManager.SavePlayConfiguration();
                                }
                                rtvStructure.TreeObject = null;
                                // Refresh the instance list
                                cbItemType_SelectedIndexChanged(this, EventArgs.Empty);
                            }
                            else
                            {
                                APIPlayerData pds = rtvStructure.TreeObject as APIPlayerData;
                                if (pds != null)
                                {
                                    // Remove the player from the API configuration file
                                    api.RemoveConfiguration(pds.Player);
                                    // Remove the player from the list of modified data sheets
                                    _modifiedDS.Remove(ds);
                                    UnHookDataSheet(_currentDS);
                                    _currentDS = null;
                                    // Remove the player from the play collection
                                    bool updplays = false;
                                    foreach (PlaySchema ps in automationManager.Plays.Plays)
                                    {
                                        for (int i = ps.Players.Count - 1; i >= 0; i--)
                                        {
                                            if (ps.Players[i].Id == pds.Player.Identifier)
                                            {
                                                ps.Players.RemoveAt(i);
                                                updplays = true;
                                            }
                                        }
                                    }
                                    if (updplays)
                                    {
                                        automationManager.SavePlayConfiguration();
                                    }
                                    automationManager.CurrentConsole.RemovePlayer(pds.Player, false);
                                    // Remove update configuration for the assistant
                                    string updfile = Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                                        ConfigurationManager.AppSettings[SETTING_updateConfiguration]);
                                    UpdateAssistantsConfiguration upcfg = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(updfile));
                                    upcfg.AppAutomation = automationManager;
                                    upcfg.DataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
                                    AssistantUpdates au = upcfg.AssistantUpdates.Find(a => a.Identifier == pds.Player.Identifier);
                                    if (au != null)
                                    {
                                        upcfg.AssistantUpdates.Remove(au);
                                        File.WriteAllText(updfile,
                                            JsonSerializer.Serialize(upcfg, new JsonSerializerOptions { WriteIndented = true }));
                                    }
                                    rtvStructure.TreeObject = null;
                                    flpEditor.Controls.Clear();
                                    // Refresh the instance list
                                    cbItemType_SelectedIndexChanged(this, EventArgs.Empty);
                                }
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

        private async void bSaveItem_Click(object sender, EventArgs e)
        {
            _allowClose = false;
            try
            {
                if (_currentDS != null)
                {
                    // Ensure last changes are committed
                    flpEditor.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                }
                using (OverlayScope ov = new OverlayScope(this, MSG_SavingAIObjects))
                {
                    IAPIManager api = (cbAPIManager.SelectedItem as ObjectWrapper<IAPIManager>).TypedImplementation;
                    for (int i = _modifiedDS.Count - 1; i >= 0; i--)
                    {
                        string name = "";
                        try
                        {
                            if (_modifiedDS[i] is FilePackageManagerData fpkm)
                            {
                                name = fpkm.PackageManager.Name;
                                api.UpdateConfiguration(fpkm.PackageManager);
                                if (fpkm is IStorableElement)
                                {
                                    await ((IStorableElement)fpkm).ModifyStoredElement();
                                }
                                _modifiedDS.RemoveAt(i);
                            }
                            else if (_modifiedDS[i] is SpeechManagerData spkm)
                            {
                                name = spkm.Speech.Name;
                                api.UpdateConfiguration(spkm.Speech);
                                if (spkm is IStorableElement)
                                {
                                    await ((IStorableElement)spkm).ModifyStoredElement();
                                }
                                _modifiedDS.RemoveAt(i);
                            }
                            else if (_modifiedDS[i] is APIPlayerData apd)
                            {
                                name = apd.Player.Name;
                                api.UpdateConfiguration(apd.Player);
                                if (apd is IStorableElement)
                                {
                                    await ((IStorableElement)apd).ModifyStoredElement();
                                }
                                _modifiedDS.RemoveAt(i);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{name}: {ex.Message}");
                        }
                    }
                    // This saves the data in the configuration file
                    api.UpdateConfiguration(null);
                    ManageButtons();
                }
                cbItemType_SelectedIndexChanged(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _allowClose = true;
            }
        }

        private void AssistantsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_allowClose)
            {
                e.Cancel = true;
            }
            else if (_modifiedDS.Count > 0)
            {
                if (MessageBox.Show(MSG_ChangesNotSaved, CAP_QUESTION, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

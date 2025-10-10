using AIAssistants.Data;
using AIAssistants.Interfaces;
using AIAssistants.JSON;
using AIChessDatabase.AI;
using DesktopControls.Controls;
using DesktopControls.Tools;
using GlobalCommonEntities.Config;
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
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase
{
    /// <summary>
    /// Play editor form
    /// </summary>
    public partial class PlayEditor : Form, IUIRemoteControlElement
    {
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;
        private IDependencyProvider _provider = null;
        private IInputEditorFactory _editorFactory = null;
        private IUIDataSheet _currentDS = null;
        private ConsolePlay _consolePlay = null;
        private Dictionary<string, IAPIManager> _apiObjectUIDs = new Dictionary<string, IAPIManager>();
        private Dictionary<string, List<PlayPlayer>> _newPlayers = new Dictionary<string, List<PlayPlayer>>();
        private Dictionary<string, List<PlayPlayer>> _delPlayers = new Dictionary<string, List<PlayPlayer>>();

        public PlayEditor()
        {
            InitializeComponent();
            Text = TTL_Play;
            bFont.ToolTipText = TTP_Font;
            lPlayList.Text = LAB_Plays;
            bPlayFromList.ToolTipText = TTP_LoadPlay;
            bAddPlay.ToolTipText = TTP_AddPlay;
            bRemovePlay.ToolTipText = TTP_DelPlay;
            bSaveConfiguration.ToolTipText = TTP_SaveConfiguration;
            bDefaultPlay.ToolTipText = TTP_SetAsDefaultPlay;
            bConsole.ToolTipText = TTP_ChangeToPlay;
            lAdd.Text = LAB_AddItem;
            tsbAddItem.ToolTipText = TTP_AddItem;
            tsbRemoveItem.ToolTipText = TTP_RemoveItem;
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
                flpPlay.EditorFactory = _editorFactory;
            }
        }
        /// <summary>
        /// Current editing play
        /// </summary>
        [Browsable(false)]
        public PlaySchema Play { get; set; }
        private void PlayIdChanged(object sener, PlayIdentifierChangedEventArgs e)
        {
            // Check if the new identifier is already in use
            foreach (PlaySchema psch in automationManager.Plays.Plays)
            {
                if ((psch.Id != e.OldId) && (psch.Id == e.NewId))
                {
                    // Stop the change and notify the user
                    throw new Exception(string.Format(ERR_PlayIdInUse, e.NewId));
                }
            }
            // Change identifier in the combobox item
            for (int i = 0; i < cbPlays.Items.Count; i++)
            {
                ObjectWrapper<PlaySchema> ps = cbPlays.Items[i] as ObjectWrapper<PlaySchema>;
                if (ps != null)
                {
                    if (ps.UID == e.OldId)
                    {
                        ps.UID = e.NewId;
                        break;
                    }
                    if (_newPlayers.ContainsKey(e.OldId))
                    {
                        _newPlayers[e.NewId] = _newPlayers[e.OldId];
                        _newPlayers.Remove(e.OldId);
                    }
                    if (_delPlayers.ContainsKey(e.OldId))
                    {
                        _delPlayers[e.NewId] = _delPlayers[e.OldId];
                        _delPlayers.Remove(e.OldId);
                    }
                }
            }
            try
            {
                // If the play is the default play, update the config file
                if (ConfigurationManager.AppSettings[SETTING_defaultPlay] == e.OldId)
                {
                    AppConfigWriter.SetAppSetting(SETTING_defaultPlay, e.NewId);
                }
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
                if (sender is PlaySchema)
                {
                    if (e.PropertyName == nameof(PlaySchema.Name))
                    {
                        PlaySchema play = sender as PlaySchema;
                        if (rtvPlay.SelectedNode?.Tag == sender)
                        {
                            rtvPlay.SelectedNode.Text = play.Name;
                        }
                        // Update the combobox item
                        for (int i = 0; i < cbPlays.Items.Count; i++)
                        {
                            ObjectWrapper<PlaySchema> ps = cbPlays.Items[i] as ObjectWrapper<PlaySchema>;
                            if (ps != null)
                            {
                                if (ps.UID == play.Id)
                                {
                                    ps.FriendlyName = play.Name;
                                    cbPlays.Items[i] = ps;
                                    break;
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
        private void cfPlay_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = Icon.Clone() as Icon;
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
                fontdlg.Font = flpPlay.Font;
                if (fontdlg.ShowDialog() == DialogResult.OK)
                {
                    flpPlay.Font = fontdlg.Font;
                    rtvPlay.Font = fontdlg.Font;
                    if (_currentDS != null)
                    {
                        flpPlay.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                        _currentDS.RefreshEditor -= flpPlay.RefreshEditor;
                    }
                    _currentDS = null;
                    rtvPlay_AfterSelect(sender, new TreeViewEventArgs(rtvPlay.SelectedNode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bSaveConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentDS != null)
                {
                    flpPlay.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                }
                string updfile = Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                    ConfigurationManager.AppSettings[SETTING_updateConfiguration]);
                foreach (string key in _newPlayers.Keys)
                {
                    foreach (PlayPlayer pp in _newPlayers[key])
                    {
                        automationManager.Plays.Plays.Find(p => p.Id == key).Players.Add(pp);
                        if (key == automationManager.CurrentPlay.StdUID)
                        {
                            IAPIPlayer player = await automationManager.CurrentPlay.GetPlayerFromSchema(pp, null);
                            await automationManager.CurrentConsole.AddPlayer(player, true);
                        }
                        // Create update configuration for the assistant
                        UpdateAssistantsConfiguration upcfg = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(updfile));
                        upcfg.AppAutomation = automationManager;
                        upcfg.DataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
                        string ifname = null;
                        if (!string.IsNullOrEmpty(pp.Instructions))
                        {
                            string instructions = string.Format(pp.Instructions, pp.Name, pp.Name.Replace(" ", ""));
                            if (!instructions.StartsWith("file:"))
                            {
                                ifname = string.Format(IFILE_Assistant, pp.Name.Replace(" ", ""));
                                // Save assistant instructions file
                                File.WriteAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], ifname), instructions);
                                pp.Instructions = "file:" + Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], ifname);
                            }
                            else
                            {
                                ifname = Path.GetFileName(instructions.Substring(5));
                            }
                        }
                        else
                        {
                            ifname = string.Format(IFILE_Assistant, pp.Name.Replace(" ", ""));
                        }
                        AssistantUpdates au = new AssistantUpdates()
                        {
                            DataPath = upcfg.DataPath,
                            Identifier = pp.Id,
                            Name = pp.Name,
                            Instructions = ifname,
                            UpdateInstructions = false,
                        };
                        if (!upcfg.AssistantUpdates.Contains(au))
                        {
                            upcfg.AssistantUpdates.Add(au);
                            File.WriteAllText(updfile,
                                JsonSerializer.Serialize(upcfg, new JsonSerializerOptions { WriteIndented = true }));
                        }
                    }
                }
                foreach (string key in _delPlayers.Keys)
                {
                    foreach (PlayPlayer pp in _delPlayers[key])
                    {
                        if (key == automationManager.CurrentPlay.StdUID)
                        {
                            IAPIPlayer player = await automationManager.CurrentPlay.GetPlayerFromSchema(pp, null);
                            automationManager.CurrentConsole.RemovePlayer(player, true);
                        }
                        automationManager.Plays.Plays.Find(p => p.Id == key).Players.Remove(pp);
                        int cnt = 0;
                        // Check whether the player is used in other plays
                        foreach (PlaySchema ps in automationManager.Plays.Plays)
                        {
                            if (ps.Players.Contains(pp))
                            {
                                cnt++;
                            }
                        }
                        if (cnt == 0)
                        {
                            // Remove update configuration for the assistant
                            UpdateAssistantsConfiguration upcfg = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(updfile));
                            upcfg.AppAutomation = automationManager;
                            upcfg.DataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
                            AssistantUpdates au = upcfg.AssistantUpdates.Find(a => a.Identifier == pp.Id);
                            if (au != null)
                            {
                                upcfg.AssistantUpdates.Remove(au);
                                File.WriteAllText(updfile,
                                    JsonSerializer.Serialize(upcfg, new JsonSerializerOptions { WriteIndented = true }));
                            }
                        }
                    }
                }
                _newPlayers.Clear();
                _delPlayers.Clear();
                automationManager.SavePlayConfiguration();
                MessageBox.Show(MSG_ConfigurationSaved, CAP_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbPlays_SelectedIndexChanged(object sender, EventArgs e)
        {
            bPlayFromList.Enabled = cbPlays.SelectedItem != null;
            ObjectWrapper<PlaySchema> ps = cbPlays.SelectedItem as ObjectWrapper<PlaySchema>;
            string defid = ConfigurationManager.AppSettings[SETTING_defaultPlay];
            bRemovePlay.Enabled = (ps != null) && (ps.UID != defid);
            bConsole.Enabled = (ps != null) && (ps.UID != automationManager.CurrentPlay.StdUID) && (ps.TypedImplementation.Players?.Count > 0);
            bDefaultPlay.Enabled = (ps != null) && (ps.UID != defid) && (ps.TypedImplementation.Players?.Count > 0);
            Text = string.Format(TTL_Play, (cbPlays.SelectedItem ?? "").ToString());
        }

        private async void bPlayFromList_Click(object sender, EventArgs e)
        {
            try
            {
                if ((_newPlayers.Count > 0) || (_delPlayers.Count > 0))
                {
                    if (MessageBox.Show(MSG_ChangesNotSaved2, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                }
                _newPlayers.Clear();
                _delPlayers.Clear();
                Play = (cbPlays.SelectedItem as ObjectWrapper<PlaySchema>).TypedImplementation;
                Play.AppAutomation = automationManager;
                _consolePlay = new ConsolePlay()
                {
                    AppAutomation = automationManager
                };
                await _consolePlay.LoadFromSchema(Play);
                rtvPlay.TreeObject = Play;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(MSG_DeleteObject, CAP_DelObject, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ObjectWrapper<PlaySchema> splay = cbPlays.SelectedItem as ObjectWrapper<PlaySchema>;
                    foreach (PlayPlayer pp in splay.TypedImplementation.Players)
                    {
                        int cnt = 0;
                        foreach (PlaySchema ps in automationManager.Plays.Plays)
                        {
                            if ((ps.Id != splay.TypedImplementation.Id) && ps.Players.Contains(pp))
                            {
                                cnt++;
                            }
                        }
                        if (cnt == 0)
                        {
                            string updfile = Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                                ConfigurationManager.AppSettings[SETTING_updateConfiguration]);
                            // Remove update configuration for the assistant
                            UpdateAssistantsConfiguration upcfg = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(updfile));
                            upcfg.AppAutomation = automationManager;
                            upcfg.DataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
                            AssistantUpdates au = upcfg.AssistantUpdates.Find(a => a.Identifier == pp.Id);
                            if (au != null)
                            {
                                upcfg.AssistantUpdates.Remove(au);
                                File.WriteAllText(updfile,
                                    JsonSerializer.Serialize(upcfg, new JsonSerializerOptions { WriteIndented = true }));
                            }
                        }
                    }
                    automationManager.Plays.Plays.Remove(splay.TypedImplementation);
                    automationManager.SavePlayConfiguration();
                    cbPlays.Items.RemoveAt(cbPlays.SelectedIndex);
                    Play = null;
                    _consolePlay = null;
                    rtvPlay.TreeObject = null;
                    flpPlay.Controls.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bConsole_Click(object sender, EventArgs e)
        {
            try
            {
                ConsolePlay play = null;
                if (cbPlays.SelectedItem is ObjectWrapper<PlaySchema>)
                {
                    play = new ConsolePlay()
                    {
                        AppAutomation = automationManager
                    };
                    await play.LoadFromSchema((cbPlays.SelectedItem as ObjectWrapper<PlaySchema>).TypedImplementation);
                    automationManager.SetCurrentPlay(play, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rtvPlay_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                cbAddItem.Items.Clear();
                tsbAddItem.Enabled = false;
                tsbRemoveItem.Enabled = false;
                if ((rtvPlay.SelectedNode != null) && (rtvPlay.SelectedNode.Tag is UIDataSheet))
                {
                    UIDataSheet ds = rtvPlay.SelectedNode.Tag as UIDataSheet;
                    if ((ds != null) && (ds != _currentDS))
                    {
                        if (_currentDS != null)
                        {
                            flpPlay.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                            _currentDS.RefreshEditor -= flpPlay.RefreshEditor;
                        }
                        _currentDS = ds;
                        flpPlay.Controls.Clear();
                        ds.RefreshEditor += flpPlay.RefreshEditor;
                        for (int ix = 0; ix < ds.Properties.Count; ix++)
                        {
                            PropertyEditorInfo pi = ds.Properties[ix];
                            flpPlay.RefreshEditor(ds, new RefreshEditorEventArgs(pi, ix, EditorContainerOperation.Add));
                        }
                    }
                    if ((rtvPlay.SelectedNode != null) &&
                        ((rtvPlay.SelectedNode.Tag is PlayPlayer) || (rtvPlay.SelectedNode.Tag is CastElement)))
                    {
                        // Player or Asset selected
                        tsbRemoveItem.Enabled = true;
                    }
                }
                else
                {
                    if (_currentDS != null)
                    {
                        flpPlay.RefreshEditor(_currentDS, new RefreshEditorEventArgs(null, -1, EditorContainerOperation.Update));
                        _currentDS.RefreshEditor -= flpPlay.RefreshEditor;
                    }
                    _currentDS = null;
                    flpPlay.Controls.Clear();
                    if ((rtvPlay.SelectedNode != null) && (rtvPlay.SelectedNode.Tag is List<PlayPlayer>))
                    {
                        // Play Cast
                        foreach (ObjectWrapper<IAPIManager> apim in automationManager.ApiManagers)
                        {
                            foreach (ObjectWrapper<string> plt in apim.TypedImplementation.AvailableInstances(nameof(IAPIPlayer)))
                            {
                                if (!_apiObjectUIDs.ContainsKey(plt.UID))
                                {
                                    _apiObjectUIDs[plt.UID] = apim.TypedImplementation;
                                }
                                if (!_consolePlay.Cast.Any(p => p.Identifier == plt.UID))
                                {
                                    cbAddItem.Items.Add(plt);
                                }
                            }
                        }
                    }
                    else if ((rtvPlay.SelectedNode != null) && (rtvPlay.SelectedNode.Tag is List<CastElement>))
                    {
                        // Player Assets
                        foreach (ObjectWrapper<IAPIManager> apim in automationManager.ApiManagers)
                        {
                            foreach (ObjectWrapper<string> plt in apim.TypedImplementation.AvailableInstances(nameof(IPlayerAsset)))
                            {
                                if (!_apiObjectUIDs.ContainsKey(plt.UID))
                                {
                                    _apiObjectUIDs[plt.UID] = apim.TypedImplementation;
                                }
                                if (!_consolePlay.Cast.Any(p => p.Identifier == plt.UID))
                                {
                                    cbAddItem.Items.Add(plt);
                                }
                            }
                        }
                    }
                    ControlHelpers.SetToolBarComboBoxWidth(cbAddItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbAddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsbAddItem.Enabled = cbAddItem.SelectedItem != null;
        }

        private async void tsbAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                ObjectWrapper<string> plt = cbAddItem.SelectedItem as ObjectWrapper<string>;
                if ((rtvPlay.SelectedNode != null) &&
                    (plt != null))
                {
                    if (_apiObjectUIDs.ContainsKey(plt.UID))
                    {
                        if (rtvPlay.SelectedNode.Tag is List<PlayPlayer>)
                        {
                            IAPIPlayer player = await _apiObjectUIDs[plt.UID].CreateElement(plt.TypedImplementation, plt.UID) as IAPIPlayer;
                            if (player != null)
                            {
                                PlayPlayer psch = _consolePlay.GetPlayerSchema(player);

                                if (_newPlayers.ContainsKey(_consolePlay.StdUID))
                                {
                                    _newPlayers[_consolePlay.StdUID].Add(psch);
                                }
                                else
                                {
                                    _newPlayers[_consolePlay.StdUID] = new List<PlayPlayer>() { psch };
                                }
                                rtvPlay.SelectedNode.Nodes.Add(rtvPlay.CreateNodeForObject(psch));
                                rtvPlay.SelectedNode.Expand();
                                cbAddItem.Items.RemoveAt(cbAddItem.SelectedIndex);
                                cbAddItem.SelectedItem = null;
                            }
                        }
                        else if (rtvPlay.SelectedNode.Tag is List<CastElement>)
                        {
                            IPlayerAsset asset = await _apiObjectUIDs[plt.UID].CreateElement(plt.TypedImplementation, plt.UID) as IPlayerAsset;
                            PlayPlayer player = rtvPlay.SelectedNode.Parent.Tag as PlayPlayer;
                            if (player != null)
                            {
                                IAPIPlayer aplayer = _consolePlay.GetPlayerByID(player.Id);
                                if (aplayer != null)
                                {
                                    aplayer.Assets.Add(asset);
                                }
                                CastElement ce = _consolePlay.GetCastElement(asset);
                                player.Assets.Add(ce);
                                rtvPlay.SelectedNode.Nodes.Add(rtvPlay.CreateNodeForObject(ce));
                                rtvPlay.SelectedNode.Expand();
                                cbAddItem.Items.RemoveAt(cbAddItem.SelectedIndex);
                                cbAddItem.SelectedItem = null;
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

        private void tsbRemoveItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (rtvPlay.SelectedNode != null)
                {
                    if (rtvPlay.SelectedNode.Tag is PlayPlayer)
                    {
                        PlayPlayer psch = rtvPlay.SelectedNode.Tag as PlayPlayer;
                        rtvPlay.SelectedNode.Remove();
                        if (psch != null)
                        {
                            if (_newPlayers.ContainsKey(_consolePlay.StdUID))
                            {
                                _newPlayers[_consolePlay.StdUID].Remove(psch);
                            }
                            if (_delPlayers.ContainsKey(_consolePlay.StdUID))
                            {
                                _delPlayers[_consolePlay.StdUID].Add(psch);
                            }
                            else
                            {
                                _delPlayers[_consolePlay.StdUID] = new List<PlayPlayer>() { psch };
                            }
                        }
                        tsbRemoveItem.Enabled = false;
                    }
                    else if (rtvPlay.SelectedNode.Tag is CastElement)
                    {
                        IAPIPlayer player = _consolePlay.GetPlayerByID((rtvPlay.SelectedNode.Parent.Parent.Tag as PlayPlayer).Id);
                        IPlayerAsset asset = player.Assets.FirstOrDefault(a => a.Identifier == (rtvPlay.SelectedNode.Tag as CastElement).Id);
                        (rtvPlay.SelectedNode.Parent.Parent.Tag as PlayPlayer).Assets.Remove(rtvPlay.SelectedNode.Tag as CastElement);
                        if (player != null)
                        {
                            player.Assets.Remove(asset);
                        }
                        rtvPlay.SelectedNode.Remove();
                        tsbRemoveItem.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bAddPlay_Click(object sender, EventArgs e)
        {
            try
            {
                PlaySchema nplay = new PlaySchema()
                {
                    Name = string.Format(NAME_NewPlay, automationManager.Plays.Plays.Count + 1),
                    Id = string.Format(NAME_NewPlay, automationManager.Plays.Plays.Count + 1),
                    Players = new List<PlayPlayer>(),
                    AppAutomation = automationManager,
                    User = new PlayUser(),
                    Applications = new List<PlayUser>() { new PlayUser() { Name = "AIChessDB" } }
                };
                nplay.PropertyChanged += PropertyChanged;
                nplay.PlayIdentifierChanged += PlayIdChanged;
                nplay.RefreshEditor += flpPlay.RefreshEditor;
                automationManager.Plays.Plays.Add(nplay);
                cbPlays.SelectedIndex = cbPlays.Items.Add(new ObjectWrapper<PlaySchema>(nplay, nplay.Name, nplay.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bDefaultPlay_Click(object sender, EventArgs e)
        {
            try
            {
                ObjectWrapper<PlaySchema> splay = cbPlays.SelectedItem as ObjectWrapper<PlaySchema>;
                if (splay != null)
                {
                    AppConfigWriter.SetAppSetting(SETTING_defaultPlay, splay.TypedImplementation.Id);
                    bSaveConfiguration_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void PlayEditor_Shown(object sender, EventArgs e)
        {
            try
            {
                if (automationManager != null)
                {
                    List<ObjectWrapper<PlaySchema>> plays = ObjectWrapper<PlaySchema>.ConvertList(automationManager.Plays.Plays);
                    foreach (ObjectWrapper<PlaySchema> play in plays)
                    {
                        cbPlays.Items.Add(play);
                        play.TypedImplementation.PlayIdentifierChanged -= PlayIdChanged;
                        play.TypedImplementation.PlayIdentifierChanged += PlayIdChanged;
                        play.TypedImplementation.PropertyChanged -= PropertyChanged;
                        play.TypedImplementation.PropertyChanged += PropertyChanged;
                        play.TypedImplementation.RefreshEditor -= flpPlay.RefreshEditor;
                        play.TypedImplementation.RefreshEditor += flpPlay.RefreshEditor;
                    }
                    ControlHelpers.SetToolBarComboBoxWidth(cbPlays);
                }
                if (Play != null)
                {
                    toolStrip1.Enabled = false;
                    using (new OverlayScope(this, MSG_LoadingPlay))
                    {
                        cbPlays.SelectedItem = new ObjectWrapper<PlaySchema>(Play, Play.Name, Play.Id);
                        _consolePlay = new ConsolePlay()
                        {
                            AppAutomation = automationManager
                        };
                        await _consolePlay.LoadFromSchema(Play);
                    }
                }
                bPlayFromList.Enabled = cbPlays.SelectedItem != null;
                ObjectWrapper<PlaySchema> ps = cbPlays.SelectedItem as ObjectWrapper<PlaySchema>;
                bRemovePlay.Enabled = (ps != null) && (ps.UID != Play.Id);
                rtvPlay.TreeObject = Play;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                toolStrip1.Enabled = true;
            }
        }

        private void PlayEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((_newPlayers.Count > 0) || (_delPlayers.Count > 0))
            {
                if (MessageBox.Show(MSG_ChangesNotSaved, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

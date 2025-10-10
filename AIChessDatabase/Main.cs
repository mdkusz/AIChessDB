using AIAssistants.Data;
using AIAssistants.Interfaces;
using AIAssistants.JSON;
using AIAssistants.Tools;
using AIChessDatabase.AI;
using AIChessDatabase.Controls;
using AIChessDatabase.Data;
using AIChessDatabase.Dialogs;
using AIChessDatabase.Interfaces;
using AIChessDatabase.PGNParser;
using AIChessDatabase.Properties;
using AIChessDatabase.Query;
using AIChessDatabase.Setup;
using BaseClassesAndInterfaces.Application;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using DesktopAIAssistants.Console;
using DesktopAIAssistants.Interfaces;
using DesktopControls.Controls;
using GlobalCommonEntities.Config;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using QueryDesktopControls.Forms;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;
using static Resources.Properties.Resources;

namespace AIChessDatabase
{
    /// <summary>
    /// Application main form.
    /// </summary>
    public partial class Main : MainForm, IAppAutomation, IAppServiceProvider
    {
        /// <summary>
        /// One line for a log file in CSV style
        /// </summary>
        public class LogRecord
        {
            /// <summary>
            /// Log file path
            /// </summary>
            public string Path { get; set; }
            /// <summary>
            /// Column separator
            /// </summary>
            public string Sepator { get; set; } = ";";
            /// <summary>
            /// Log column data
            /// </summary>
            public List<string> Data { get; set; } = new List<string>();
        }
        private IObjectRepository _repository;
        private Dictionary<string, IObjectRepository> _otherRepositories = new Dictionary<string, IObjectRepository>();
        private APIManagerList _apiManagersCfgs;
        private List<ObjectWrapper<IAPIManager>> _apiManagers;
        private PlayCollection _playCollection;
        private AppDocumentRepository _documents;
        private APIConsole _hConsole;
        private IPlay _play;
        private List<FunctionDef> _functions;
        private string _fcallLog = null;
        private CancellationTokenSource _tokenSource;
        private string _usagelog = null;
        private List<ObjectWrapper<IApplicationService>> _services;
        private Dictionary<string, int> _callOrder = new Dictionary<string, int>();
        private Dictionary<string, string> _callBatches = new Dictionary<string, string>();
        private ConcurrentQueue<LogRecord> _logQueue = new ConcurrentQueue<LogRecord>();
        private AIAssistantConsole _frmAIConsole;
        private ICredentialStore _credStore;

        public Main()
        {
            InitializeComponent();
            _resources.IndexResourcemanager(Properties.Resources.ResourceManager);
            _resources.IndexResourcemanager(UIResources.ResourceManager);
            _resources.IndexResourcemanager(UIElements.ResourceManager);
            List<string> lcalls = new List<string>
                    {
                        "get_database_connections",
                        "get_available_database_providers",
                        "draw_board",
                        "import_pgn_file",
                        "import_multiple_pgn_files",
                        "copy_database",
                        "create_new_match",
                        "open_new_querywindow",
                        "normalize_match_labels",
                        "arrange_windows",
                        "list_open_automation_forms",
                        "get_form_controls",
                        "get_children_controls",
                        "highlight_control",
                        "highlight_and_comment_control",
                        "click_control",
                        "send_text_to_editor",
                        "read_editor_text",
                        "dropdown_or_expand",
                        "get_list_items",
                        "select_list_item",
                        "get_windows_with_open_matches",
                        "send_complete_pgn_match",
                        "get_current_match_info",
                        "get_all_match_moves",
                        "get_current_match_move",
                        "go_to_next_match_move",
                        "go_to_previous_match_move",
                        "add_new_match_move",
                        "show_application_configuration",
                        "new_connection_string",
                        "edit_connection_string",
                        "show_play_editor",
                        "show_assistants_editor",
                        "show_updates_editor",
                        "copy_text_to_clipboard",
                        "add_new_note",
                        "rewrite_all_notes",
                        "list_open_query_forms",
                        "open_new_querywindow",
                        "set_query_view",
                        "add_filter_expression",
                        "get_current_filters",
                        "remove_filter_expressions",
                        "get_order_by_list",
                        "set_order_by_list",
                        "refresh_query_data",
                    };
            for (int ix = 0; ix < lcalls.Count; ix++)
            {
                // Order to execute functions in parallel calls
                _callOrder[lcalls[ix]] = ix;
            }
            // In parallel calls, order function execution using those parameters
            _callBatches["go_to_next_match_move"] = "execution_order";
        }
        /// <summary>
        /// IAppServiceProvider: Shared database repository for common operations.
        /// </summary>
        public IObjectRepository SharedRepository { get { return _repository; } }
        /// <summary>
        /// IAppServiceProvider: New instance of the current shared database repository for operations that require a fresh context.
        /// </summary>
        public IObjectRepository NewRepository { get { return _repository?.GetType().GetConstructor(new Type[] { _repository.Provider.GetType() }).Invoke(new[] { _repository.Provider }) as IObjectRepository; } }
        /// <summary>
        /// IAppServiceProvider: List of additional database repositories available in the application.
        /// </summary>
        public List<string> OtherRepositories(string otherthan)
        {
            List<string> cnames = new List<string>();
            foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
            {
                if (cs.Name != otherthan)
                {
                    cnames.Add(cs.Name);
                }
            }
            return cnames;
        }
        /// <summary>
        /// IAppServiceProvider: Create an instance of a database repository with the specified name.
        /// </summary>
        /// <param name="name">
        /// Connection string name.
        /// </param>
        /// <returns>
        /// New database repository instance.
        /// </returns>
        /// <remarks>
        /// By default, there is only a connection available in the new repository. Use MaxConnctions to set the maximum number of connections available in the repository.
        /// </remarks>
        public IObjectRepository GetRepository(string name)
        {
            if (_otherRepositories.ContainsKey(name))
            {
                return _otherRepositories[name];
            }
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[name];
            string dbprov = GetDependencyValue(cs.ProviderName);
            Type providertype = Type.GetType(dbprov);
            AIChessObjectRepository repository = new AIChessObjectRepository(providertype.GetConstructor(Type.EmptyTypes).Invoke(null) as IDatabaseDependencyProvider);
            repository.DatabaseConnectionString = GetCredentialString(string.Join(";", cs.ConnectionString, ConfigurationManager.AppSettings[SETTING_CredentialStoreKeyData]));
            repository.ConnectionName = name;
            int.TryParse(ConfigurationManager.AppSettings[SETTING_dbconnections], out int maxconnections);
            repository.MaxConnections = Math.Max(maxconnections, 2);
            if (_repository?.Queries?.Queries != null)
            {
                repository.Queries.Queries = new List<NonStandardQuery>(_repository.Queries.Queries);
            }
            else
            {
                repository.Queries.Queries = LoadConfiguration(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                    ConfigurationManager.AppSettings[SETTING_nsQueries]),
                    typeof(List<NonStandardQuery>)) as List<NonStandardQuery>;
            }
            _otherRepositories[name] = repository;
            return repository;
        }
        /// <summary>
        /// IAppServiceProvider: Launch user interface to edit a match.
        /// </summary>
        /// <param name="match">
        /// Match to edit.
        /// </param>
        public void EditMatch(Match match)
        {
            EditMatchWindow emw = FindWindow(ID_MATCHWINDOW + match.IdMatch.ToString()) as EditMatchWindow;
            if (emw == null)
            {
                emw = new EditMatchWindow();
                CenterMdiChild(emw);
                emw.Provider = this;
            }
            emw.CurrentMatch = match;
            emw.Show();
            emw.BringToFront();
        }
        /// <summary>
        /// IAppServiceProvider: Launch user interface to play a match.
        /// </summary>
        /// <param name="match">
        /// Match to play.
        /// </param>
        public void PlayMatch(Match match)
        {
            PlayMatchWindow emw = FindWindow(ID_PLAYWINDOW + match.IdMatch.ToString()) as PlayMatchWindow;
            if (emw == null)
            {
                emw = new PlayMatchWindow();
                CenterMdiChild(emw);
                emw.Provider = this;
            }
            emw.CurrentMatch = match;
            emw.Show();
            emw.BringToFront();
        }
        /// <summary>
        /// IDependencyProvider: Check if a given class or interface is supported
        /// </summary>
        /// <param type="services">
        /// Type names separated by semicolon
        /// </param>
        /// <returns>
        /// True if can instantiante objets of at least one of the given types
        /// </returns>
        public override bool QueryClassOrInterface(string services)
        {
            List<string> lservices = new List<string>(services.Split(';'));
            return lservices.Contains(nameof(IDataExportFormatter)) || base.QueryClassOrInterface(services);
        }
        /// <summary>
        /// IDependencyProvider: Get all the instances of all the rquested types or interfaces
        /// </summary>
        /// <param type="services">
        /// Semicolon separated type type list
        /// </param>
        /// <param type="sourcefilter">
        /// Object containing a sourcefilter to select elements
        /// </param>
        /// <returns>
        /// Enumeration of objects implementing the types as IUIIdentifier objects
        /// </returns>
        public override IEnumerable<IUIIdentifier> GetObjects(string services, object filter = null)
        {
            List<string> lservices = new List<string>(services.Split(';'));
            foreach (string service in lservices)
            {
                if (service == nameof(IDataExportFormatter))
                {
                    yield return new PGNFormatter();
                    yield return new MatchFormatter();
                }
            }
            foreach (IUIIdentifier uid in base.GetObjects(services, filter))
            {
                yield return uid;
            }
        }
        /// <summary>
        /// IAppAutomation: List of supported API Managers
        /// </summary>
        public List<ObjectWrapper<IAPIManager>> ApiManagers
        {
            get
            {
                if (_apiManagers == null)
                {
                    _apiManagers = new List<ObjectWrapper<IAPIManager>>();
                    string cfgpath = ConfigurationManager.AppSettings[SETTING_ConfigPath];
                    if (string.IsNullOrEmpty(cfgpath))
                    {
                        cfgpath = Path.GetDirectoryName(Application.ExecutablePath);
                    }
                    foreach (APIManagerCfg cfg in _apiManagersCfgs.Managers)
                    {
                        IAPIManager manager = _apiManagersCfgs.GetAPIManager(cfg.Type,
                            cfgpath,
                            ConfigurationManager.AppSettings[SETTING_LogPathBase]);
                        if (manager != null)
                        {
                            manager.AppAutomation = this;
                            _apiManagers.Add(new ObjectWrapper<IAPIManager>(manager, manager.Name, manager.Identifier));
                        }
                    }
                }
                return _apiManagers;
            }
        }
        /// <summary>
        /// IAppAutomation: Central repository for application documents
        /// </summary>
        public AppDocumentRepository Documents { get { return _documents; } }
        /// <summary>
        /// IAppAutomation: List of supported integration data readers
        /// </summary>
        public List<ObjectWrapper<IIntegrationDataReader>> IntegrationObjects
        {
            get
            {
                return new List<ObjectWrapper<IIntegrationDataReader>> { new ObjectWrapper<IIntegrationDataReader>(new ExcelDataReader(), "Excel", "") };
            }
        }
        /// <summary>
        /// IAppAutomation: Get an API Manager by type
        /// </summary>
        /// <param name="type">
        /// API Manager type
        /// </param>
        /// <param name="id">
        /// API Manager unique identifier
        /// </param>
        /// <returns>
        /// API Manager instance or null if not found
        /// </returns>
        public async Task<IAPIManager> GetAPIManager(Type type, string id)
        {
            string cfgpath = ConfigurationManager.AppSettings[SETTING_ConfigPath];
            if (string.IsNullOrEmpty(cfgpath))
            {
                cfgpath = Path.GetDirectoryName(Application.ExecutablePath);
            }
            IAPIManager manager = _apiManagersCfgs.GetAPIManager(type,
                cfgpath,
                ConfigurationManager.AppSettings[SETTING_LogPathBase]);
            if (manager != null)
            {
                manager.AppAutomation = this;
                manager = await manager.CreateElement(nameof(IAPIManager), id) as IAPIManager;
                if (manager != null)
                {
                    return manager;
                }
            }
            return null;
        }
        /// <summary>
        /// IAppAutomation: Collection of play configurations
        /// </summary>
        public PlayCollection Plays
        {
            get
            {
                return _playCollection;
            }
        }
        /// <summary>
        /// IAppAutomation: Get the current active play
        /// </summary>
        public IPlay CurrentPlay { get { return _play; } }
        /// <summary>
        /// IAppAutomation: Get the current active console
        /// </summary>
        public IHelpConsole CurrentConsole { get { return _hConsole; } }
        /// <summary>
        /// IAppAutomation: Name of the application role for application responses to function calls
        /// </summary>
        public string ApplicationRole { get { return "application"; } }
        /// <summary>
        /// IAppAutomation: Base path for log files
        /// </summary>
        public string LogPathBase
        {
            get
            {
                return ConfigurationManager.AppSettings[SETTING_LogPathBase];
            }
        }
        /// <summary>
        /// IAppAutomation: This is a property intended to get assembly attribute names used to recognize assemblies related to the application or application ecosystem.
        /// </summary>
        /// <remarks>
        /// Each string is a full name of an assembly attribute and the name of the value property name, sepparated by a comma.
        /// </remarks>
        public List<string> AssemblySignatures
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// IAppAutomation: Check whether an assembly is related to the application or belongs to the application ecosystem.
        /// </summary>
        /// <param name="signatures">
        /// Dictionary of asembly attributes and values
        /// </param>
        /// <param name="whatfor">
        /// Reason to check the assembly
        /// </param>
        /// <remarks>
        /// Each key is the full name of an assembly attribute and the name of the value property name, sepparated by a comma, and the value is the value for that attribute in the assembly being checked.
        /// When you need to process an assembly, you can use this method to check if the assembly has the required attributes with the expected values.
        /// It's on the consumer to decide whether check only one or a set of attributes.
        /// </remarks>
        /// <returns>
        /// True if the assembly is related to the application or belongs to the application ecosystem.
        /// </returns>
        public bool CheckAssemblySignature(Dictionary<string, string> signatures, string whatfor)
        {
            return false;
        }
        /// <summary>
        /// IAppAutomation: Get a credential string from the application credential repository
        /// </summary>
        /// <param name="key">
        /// Credential key string. Semi-colon separated key name and other components.
        /// </param>
        /// <returns>
        /// Credential string
        /// </returns>
        public string GetCredentialString(string key)
        {
            string error;
            if (_credStore == null)
            {
                _credStore = Type.GetType(ConfigurationManager.AppSettings[SETTING_CredentialStore]).GetConstructor(Type.EmptyTypes).Invoke(null) as ICredentialStore;
            }
            CredentialStoreKey stkey = _credStore.KeyType.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { key }) as CredentialStoreKey;
            NetworkCredential credential = _credStore.GetCredentialFromStore(stkey, out error);
            if (credential != null)
            {
                return credential.Password;
            }
            throw new Exception(error);
        }
        /// <summary>
        /// IAppAutomation: List of application automation services
        /// </summary>
        public List<ObjectWrapper<IApplicationService>> AppServices
        {
            get
            {
                return _services;
            }
        }
        /// <summary>
        /// IAppAutomation: List of available service names
        /// </summary>
        public List<string> ServiceNames
        {
            get
            {
                List<string> names = new List<string>();
                if (_services != null)
                {
                    foreach (ObjectWrapper<IApplicationService> svc in _services)
                    {
                        names.Add(svc.TypedImplementation.Identifier);
                    }
                }
                return names;
            }
        }
        /// <summary>
        /// IAppAutomation: List of functions available for the application
        /// </summary>
        public List<FunctionDef> ApplicationFunctions { get { return _functions; } }
        /// <summary>
        /// IAppAutomation: Add service extensions to the main application
        /// </summary>
        /// <param name="extension">
        /// New service extension
        /// </param>
        public void AddAppExtension(IAppAutomationExtension extension)
        {
        }
        /// <summary>
        /// IAppAutomation: Show help console with a given play
        /// </summary>
        /// <param name="play">
        /// Play with the cast of console players
        /// </param>
        /// <param name="showConsole">
        /// Show help console
        /// </param>
        public void SetCurrentPlay(IPlay play, bool showConsole)
        {
            _play = play;
            if ((_play != null) && !_play.Equals(_hConsole.Play))
            {
                _hConsole.Play = _play;
                _hConsole.UpdateAppServices();
            }
        }
        /// <summary>
        /// IAppAutomation: Send a message to a player
        /// </summary>
        /// <param name="player">
        /// Player to receive the message
        /// </param>
        /// <param name="msg">
        /// Message to send
        /// </param>
        public async Task SendMessageToPlayer(IAPIPlayer player, ContextMessage msg)
        {
            await _hConsole?.SendMessageToPlayerAsync(player, msg);
        }
        /// <summary>
        /// IAppAutomation: Give a chance to the application to edit player configuration.
        /// </summary>
        /// <param name="player">
        /// Player to edit
        /// </param>
        /// <returns>
        /// True if the application managed to edit the player configuration.
        /// </returns>
        public async Task<bool> EditPlayerConfiguration(IAPIPlayer player)
        {
            await Task.Yield();
            EditPlayerData data = new EditPlayerData(player);
            DlgAppConfiguration dlg = new DlgAppConfiguration()
            {
                DependencyProvider = this,
                DataSheet = data
            };
            dlg.Text = string.Format(TTL_EditPlayer, player.Name);
            CenterDialogBox(dlg);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                player.Name = data.Name;
                player.SystemInstructions = data.Instructions;
                player.Model = data.Model;
                player.Voice = data.Voice;
                player.BackColor = data.BackColor;
                player.ForeColor = data.ForeColor;
                player.Temperature = data.Temperature;
                player.TopP = data.TopP;
                player.ParallelTools = data.ParallelCalls;
                string rl = data.ReasoningLevel;
                player.ReasoningEffort(ref rl);
                if (data.WebSearch)
                {
                    player.Tools |= ToolTypes.WebSearch;
                }
                else if (player.Tools.HasFlag(ToolTypes.WebSearch))
                {
                    player.Tools = player.Tools - (int)ToolTypes.WebSearch;
                }
                SetCurrentPlayer(player);
            }
            return true;
        }
        /// <summary>
        /// IAppAutomation: Current player in the play
        /// </summary>
        /// <param name="player">
        /// IAPIPlayer assistant
        /// </param>
        /// <remarks>
        /// IHelpConsole sets this porperty for the application to set console menu commands as needed.
        /// </remarks>
        public void SetCurrentPlayer(IAPIPlayer player)
        {
            List<CommandMenuItem> appcommands = new List<CommandMenuItem>();
            List<CommandMenuItem> acommands = new List<CommandMenuItem>();
            _hConsole.AddAppMenuItems(appcommands);
            List<ToolUI> tui = new List<ToolUI>();
            tui.Add(new ToolUI()
            {
                Type = ToolUIType.ModelList,
                Items = new List<ObjectWrapper>()
            });
            player.APIManager.LLMModels.ForEach(m =>
            {
                tui[0].Items.Add(new ObjectWrapper(m, m, m));
            });
            tui[0].SelectedItem = new ObjectWrapper(player.Model, player.Model, player.Model);
            ToolTypes tools = player.Tools;
            if (tools != ToolTypes.None)
            {
                string desc;
                if ((tools & ToolTypes.ReadDocuments) == ToolTypes.ReadDocuments)
                {
                    string name = player.ToolName(ToolTypes.ReadDocuments, out desc);
                    ToolUI t = new ToolUI()
                    {
                        ToolTip = desc,
                        UID = name,
                        Icon = ICO_FileSearch,
                        Type = ToolUIType.Check,
                        ToolType = ToolTypes.ReadDocuments
                    };
                    tui.Add(t);
                }
                if ((tools & ToolTypes.ExecuteCode) == ToolTypes.ExecuteCode)
                {
                    string name = player.ToolName(ToolTypes.ExecuteCode, out desc);
                    ToolUI t = new ToolUI()
                    {
                        ToolTip = desc,
                        UID = name,
                        Icon = ICO_CodeInterpreter,
                        Type = ToolUIType.Check,
                        ToolType = ToolTypes.ExecuteCode
                    };
                    tui.Add(t);
                }
                if ((tools & ToolTypes.WebSearch) == ToolTypes.WebSearch)
                {
                    string name = player.ToolName(ToolTypes.WebSearch, out desc);
                    ToolUI t = new ToolUI()
                    {
                        ToolTip = desc,
                        UID = name,
                        Icon = ICO_WebSearch.ToBitmap(),
                        Type = ToolUIType.Check,
                        ToolType = ToolTypes.WebSearch
                    };
                    tui.Add(t);
                }
                if ((tools & ToolTypes.FunctionCall) == ToolTypes.FunctionCall)
                {
                    string name = player.ToolName(ToolTypes.FunctionCall, out desc);
                    List<FunctionDef> functions = player.AllFunctions;
                    if ((functions != null) && (functions.Count > 0))
                    {
                        ToolUI t = new ToolUI()
                        {
                            ToolTip = desc,
                            UID = name,
                            Icon = FunctionCalling,
                            Type = ToolUIType.DropDown,
                            ToolType = ToolTypes.FunctionCall,
                            Items = new List<ObjectWrapper>()
                        };
                        t.Items.Add(new ObjectWrapper(null, "None", ""));
                        foreach (FunctionDef f in functions)
                        {

                            if (!string.IsNullOrEmpty(f.DisplayName))
                            {
                                string translatedName = Properties.UIResources.ResourceManager.GetString(f.DisplayName) ?? f.DisplayName;
                                t.Items.Add(new ObjectWrapper(f, translatedName, f.Name)
                                {
                                    FriendlyDescription = f.Description
                                });
                                if ((f.AllowUserCalling != null) &&
                                    f.AllowUserCalling.Value &&
                                    (f.Parameters.MandatoryParameters == 0))
                                {
                                    acommands.Add(new CommandMenuItem(f.Name,
                                        translatedName,
                                        f.Description,
                                        ExecuteConsoleCommandAsync));
                                }
                            }
                            else
                            {
                                t.Items.Add(new ObjectWrapper(f, f.Name, f.Name)
                                {
                                    FriendlyDescription = f.Description
                                });
                            }
                        }
                        tui.Add(t);
                    }
                }
                if (tui.Count == 0)
                {
                    tui = null;
                }
            }
            _hConsole.AddAssistantMenuItems(acommands);
            _hConsole.AddForceToolSelection(tui);
        }
        /// <summary>
        /// IAppAutomation: Find a player in the current play by its name
        /// </summary>
        /// <param name="playerName">
        /// Player name
        /// </param>
        /// <returns>
        /// Found player or null
        /// </returns>
        public IAPIPlayer GetPlayerByName(string playerName)
        {
            if (_play != null)
            {
                return _play.GetPlayerByName(playerName);
            }
            return null;
        }
        /// <summary>
        /// IAppAutomation: Find a player in all plays by its unique identifier
        /// </summary>
        /// <param name="playerId">
        /// Player unique identifier
        /// </param>
        /// <returns>
        /// Found player or null
        /// </returns>
        public async Task<IAPIPlayer> GetPlayerByID(string playerId)
        {
            if (_play != null)
            {
                IAPIPlayer player = _play.GetPlayerByID(playerId);
                if (player != null)
                {
                    return player;
                }
                foreach (PlaySchema p in _playCollection.Plays)
                {
                    PlayPlayer pp = p.Players.Find(pl => pl.Id == playerId);
                    if (pp != null)
                    {
                        IAPIManager api = await GetAPIManager(pp.API.Type, pp.API.Id);
                        player = await api.CreateElement(pp.Type, pp.Id) as IAPIPlayer;
                        if ((player != null) && (pp.Assets != null) && (pp.Assets.Count > 0))
                        {
                            player.AllowedServices = pp.Services;
                            player.BackColor = pp.BackColor;
                            player.ForeColor = pp.ForeColor;
                            if (ApplicationFunctions != null)
                            {
                                player.AddFunctions(ApplicationFunctions);
                            }
                            if (player.AllowedServices != null)
                            {
                                List<IApplicationService> lps = null;
                                foreach (ObjectWrapper<IApplicationService> srv in AppServices)
                                {
                                    if (player.AllowedServices.Contains(srv.TypedImplementation.Identifier))
                                    {
                                        if (lps == null)
                                        {
                                            lps = new List<IApplicationService>();
                                        }
                                        lps.Add(srv.TypedImplementation);
                                    }
                                }
                                player.AddServices(lps);
                            }
                            List<IPlayerAsset> assets = new List<IPlayerAsset>();
                            if (player.Assets != null)
                            {
                                assets.AddRange(player.Assets);
                            }
                            foreach (CastElement ce in pp.Assets)
                            {
                                if ((api?.GetType() != ce.API.Type) || (api.Identifier != ce.API.Id))
                                {
                                    api = await GetAPIManager(ce.API.Type, ce.API.Id);
                                }
                                IPlayerAsset asset = await api.CreateElement(ce.Type, ce.Id) as IPlayerAsset;
                                if ((asset != null) && !assets.Contains(asset))
                                {
                                    assets.Add(asset);
                                }
                            }
                            player.Assets = assets;
                        }
                        return player;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// IAppAutomation: Save play configuration
        /// </summary>
        public void SavePlayConfiguration()
        {
            SaveConfiguration(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath], ConfigurationManager.AppSettings[SETTING_playCollection]), _playCollection);
        }
        /// <summary>
        /// IAppAutomation: Get a list of contacts for external applications
        /// </summary>
        /// <param name="appid">
        /// Extenal application identifier
        /// </param>
        /// <returns>
        /// List of contacts
        /// </returns>
        public List<ContactAddress> ContactsForApplication(string appid)
        {
            return null;
        }
        /// <summary>
        /// IAppAutomation: Make a call to a function
        /// </summary>
        /// <param name="caller">
        /// Function caller
        /// </param>
        /// <param name="calls">
        /// Block of function calls
        /// </param>
        /// <returns>
        /// FunctionBatch with the call results
        /// </returns>
        public async Task<FunctionBatch> CallFunction(IPlayer caller, FunctionBatch calls)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            IAPIPlayer api = caller as IAPIPlayer;
            IDocumentAnalyzer pdoc = caller as IDocumentAnalyzer;
            IDocumentStoreManager extvs = pdoc != null ? await pdoc.GetDocumentStoreManager() : null;
            int servedcalls = 0;
            // First, order functions by name using the predefined order
            foreach (FunctionBatchItem item in calls.Calls)
            {
                if (_callOrder.ContainsKey(item.FunctionName.ToLower()))
                {
                    // Step 1000 to allow inserting order indexes in between if needed
                    item.OrderIndex = 1000 * _callOrder[item.FunctionName.ToLower()];
                }
            }
            calls.Calls.Sort();
            // Then, if any function has a batch order parameter, group those functions by that parameter value
            int ic = 0;
            while (ic < calls.Calls.Count)
            {
                FunctionBatchItem item = calls.Calls[ic];
                if (_callBatches.ContainsKey(item.FunctionName.ToLower()))
                {
                    int ic0 = ic;
                    string fname = item.FunctionName.ToLower();
                    List<int> ordervalues = new List<int>();
                    while (ic < calls.Calls.Count)
                    {
                        item = calls.Calls[ic];
                        using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                        {
                            bool reject = true;
                            if (doc.RootElement.TryGetProperty(_callBatches[item.FunctionName.ToLower()], out JsonElement orderparam))
                            {
                                if (orderparam.TryGetInt32(out int ordervalue) && (ordervalue >= 0) && (ordervalue < 1000))
                                {
                                    item.OrderIndex += ordervalue;
                                    if (!ordervalues.Contains(ordervalue))
                                    {
                                        ordervalues.Add(ordervalue);
                                        reject = false;
                                    }
                                }
                            }
                            if (reject)
                            {
                                ic = ic0;
                                if (!string.IsNullOrEmpty(_fcallLog))
                                {
                                    using (StreamWriter wrt = new StreamWriter(_fcallLog, true))
                                    {
                                        wrt.WriteLine(string.Format("{0}; {1} => {2}.{3}({4})", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), caller.PlayerName, "Batch cancelled", item.FunctionName, item.Parameters));
                                    }
                                }
                                while (ic < calls.Calls.Count)
                                {
                                    if (fname != item.FunctionName.ToLower())
                                    {
                                        break;
                                    }
                                    item = calls.Calls[ic];
                                    // Reject the whole batch if any function is missing the order parameter or there are duplicated order values
                                    result.Clear();
                                    result["status"] = "error";
                                    result["message"] = $"Function batch item without proper {_callBatches[item.FunctionName.ToLower()]} order parameter.";
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                    item.Response.Result = JsonSerializer.Serialize(result);
                                    ic++;
                                }
                            }
                            else
                            {
                                ic++;
                            }
                        }
                    }
                }
                else
                {
                    ic++;
                }
            }
            // Finally, sort again by order index
            calls.Calls.Sort();
            int ixc = 0;
            while (ixc < calls.Calls.Count)
            {
                FunctionBatchItem item = calls.Calls[ixc++];
                if (item.Response != null)
                {
                    continue;
                }
                result.Clear();
                if (!string.IsNullOrEmpty(_fcallLog))
                {
                    using (StreamWriter wrt = new StreamWriter(_fcallLog, true))
                    {
                        wrt.WriteLine(string.Format("{0}; {1} => {2}.{3}({4})", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), caller.PlayerName, calls.Identifier, item.FunctionName, item.Parameters));
                    }
                }
                try
                {
                    switch (item.FunctionName.ToLower())
                    {
                        #region userinterface service
                        case "get_database_connections":
                            List<RepositoryInfo> repos = new List<RepositoryInfo>();
                            foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
                            {
                                string deptype;
                                if (!string.IsNullOrEmpty(css.ProviderName) &&
                                    !string.IsNullOrEmpty(deptype = GetDependencyValue(css.ProviderName)))
                                {
                                    Type providertype = Type.GetType(deptype);
                                    if (providertype != null)
                                    {
                                        ConnectionString str = new ConnectionString()
                                        {
                                            ConnectionData = css.ConnectionString,
                                            Name = css.Name,
                                            ProviderType = providertype,
                                        };
                                        repos.Add(new RepositoryInfo()
                                        {
                                            ConnectionStringName = css.Name,
                                            ProviderName = css.ProviderName,
                                            Default = css.Name == ConfigurationManager.AppSettings[SETTING_dbconstring]
                                        });
                                    }
                                }
                                servedcalls++;
                                result["status"] = "success";
                                result["databases"] = repos;
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                            }
                            break;
                        case "get_available_database_providers":
                            List<string> providers = new List<string>();
                            IDictionary dbp = ConfigurationManager.GetSection(CONFIG_dbProviders) as IDictionary;
                            if (dbp != null)
                            {
                                providers.AddRange(dbp.Keys.Cast<string>().ToList());
                            }
                            servedcalls++;
                            result["status"] = "success";
                            result["server_types"] = providers;
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            break;
                        case "draw_board":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("board", out JsonElement board))
                                {
                                    Bitmap boardimg = null;
                                    if (_hConsole.Width > 550)
                                    {
                                        using (Board b = new Board())
                                        {
                                            boardimg = b.BoardFromString(board.GetString());
                                        }
                                    }
                                    else
                                    {
                                        using (TinyBoard b = new TinyBoard())
                                        {
                                            boardimg = b.BoardFromString(board.GetString());
                                        }
                                    }
                                    await _hConsole.SendImageToUserAsync(caller, boardimg);
                                    servedcalls++;
                                    result["status"] = "success";
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "board"));
                                }
                            }
                            break;
                        case "import_pgn_file":
                            WindowID wid = null;
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                AIChessObjectRepository repo = null;
                                if (doc.RootElement.TryGetProperty("connection_name", out JsonElement connection_name))
                                {
                                    repo = GetRepository(connection_name.GetString()) as AIChessObjectRepository;
                                }
                                EndInvoke(BeginInvoke((Action)(() =>
                                {
                                    wid = opImportPGN_Click(repo);
                                })));
                                await _hConsole.ShowActivityFeedback(MSG_import_pgn_file);
                                servedcalls++;
                                if (wid == null)
                                {
                                    result["status"] = "information";
                                    result["message"] = MSG_OPerationCancelled;
                                }
                                else
                                {
                                    result["status"] = "success";
                                    IUIRemoteControlElement frc = FindController(wid.UID);
                                    if ((caller.AllowedServices != null) &&
                                        (extvs != null) &&
                                        (frc != null) &&
                                        caller.AllowedServices.Contains("automation"))
                                    {
                                        string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                        string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                        result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                    }
                                    else
                                    {
                                        result["message"] = MSG_UILaunched;
                                    }
                                }
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                                if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                                {
                                    item.Response.ChainedCall = true;
                                    item.Response.Message = new ContextMessage()
                                    {
                                        PlayerName = _play.User.PlayerName,
                                        PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                        Message = result["message"].ToString()
                                    };
                                }
                            }
                            break;
                        case "import_multiple_pgn_files":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                AIChessObjectRepository repo = null;
                                if (doc.RootElement.TryGetProperty("connection_name", out JsonElement connection_name))
                                {
                                    repo = GetRepository(connection_name.GetString()) as AIChessObjectRepository;
                                }
                                wid = null;
                                EndInvoke(BeginInvoke((Action)(() =>
                                {
                                    wid = opImportMultiPGN_Click(repo);
                                })));
                                await _hConsole.ShowActivityFeedback(MSG_import_multiple_pgn_files);
                                servedcalls++;
                                if (wid == null)
                                {
                                    result["status"] = "information";
                                    result["message"] = MSG_OPerationCancelled;
                                }
                                else
                                {
                                    result["status"] = "success";
                                    IUIRemoteControlElement frc = FindController(wid.UID);
                                    if ((caller.AllowedServices != null) &&
                                        (extvs != null) &&
                                        (frc != null) &&
                                        caller.AllowedServices.Contains("automation"))
                                    {
                                        string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                        string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                        result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                    }
                                    else
                                    {
                                        result["message"] = MSG_UILaunched;
                                    }
                                }
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                                if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                                {
                                    item.Response.ChainedCall = true;
                                    item.Response.Message = new ContextMessage()
                                    {
                                        PlayerName = _play.User.PlayerName,
                                        PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                        Message = result["message"].ToString()
                                    };
                                }
                            }
                            break;
                        case "copy_database":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                AIChessObjectRepository repo = null;
                                if (doc.RootElement.TryGetProperty("connection_name", out JsonElement connection_name))
                                {
                                    repo = GetRepository(connection_name.GetString()) as AIChessObjectRepository;
                                }
                                BeginInvoke((Action)(() =>
                                {
                                    optExport_Click(repo);
                                }));
                                await _hConsole.ShowActivityFeedback(MSG_copy_database);
                                servedcalls++;
                                result["status"] = "success";
                                result["message"] = MSG_UILaunched;
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                            }
                            break;
                        case "create_new_match":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                AIChessObjectRepository repo = null;
                                if (doc.RootElement.TryGetProperty("connection_name", out JsonElement connection_name))
                                {
                                    repo = GetRepository(connection_name.GetString()) as AIChessObjectRepository;
                                }
                                wid = null;
                                EndInvoke(BeginInvoke((Action)(() =>
                                {
                                    wid = opNewMatch_Click(repo);
                                })));
                                await _hConsole.ShowActivityFeedback(MSG_create_new_match);
                                servedcalls++;
                                if (wid == null)
                                {
                                    result["status"] = "information";
                                    result["message"] = MSG_OPerationCancelled;
                                }
                                else
                                {
                                    result["status"] = "success";
                                    IUIRemoteControlElement frc = FindController(wid.UID);
                                    if ((caller.AllowedServices != null) &&
                                        (extvs != null) &&
                                        (frc != null) &&
                                        caller.AllowedServices.Contains("automation"))
                                    {
                                        string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                        string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                        result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                    }
                                    else
                                    {
                                        result["message"] = MSG_UILaunched;
                                    }
                                }
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                                if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                                {
                                    item.Response.ChainedCall = true;
                                    item.Response.Message = new ContextMessage()
                                    {
                                        PlayerName = _play.User.PlayerName,
                                        PlayerRole = api?.APIManager.GetPlatformRole(GenericRole.User, api),
                                        Message = result["message"].ToString()
                                    };
                                }
                            }
                            break;
                        case "open_new_querywindow":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                AIChessObjectRepository repo = null;
                                if (doc.RootElement.TryGetProperty("connection_name", out JsonElement connection_name))
                                {
                                    repo = GetRepository(connection_name.GetString()) as AIChessObjectRepository;
                                }
                                wid = null;
                                EndInvoke(BeginInvoke((Action)(() =>
                                {
                                    wid = opNewQuery_Click(repo);
                                })));
                                await _hConsole.ShowActivityFeedback(MSG_open_new_querywindow);
                                servedcalls++;
                                if (wid == null)
                                {
                                    result["status"] = "information";
                                    result["message"] = MSG_OPerationCancelled;
                                }
                                else
                                {
                                    result["status"] = "success";
                                    IUIRemoteControlElement frc = FindController(wid.UID);
                                    if ((caller.AllowedServices != null) &&
                                        (extvs != null) &&
                                        (frc != null) &&
                                        caller.AllowedServices.Contains("automation"))
                                    {
                                        string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                        string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                        result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                        result["number_of_matches_in_database"] = ((QueryWindow)frc).TotalMatches;
                                    }
                                    else
                                    {
                                        result["message"] = MSG_UILaunched;
                                    }
                                }
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                                if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                                {
                                    item.Response.ChainedCall = true;
                                    item.Response.Message = new ContextMessage()
                                    {
                                        PlayerName = _play.User.PlayerName,
                                        PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                        Message = result["message"].ToString()
                                    };
                                }
                            }
                            break;
                        case "normalize_match_labels":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                AIChessObjectRepository repo = null;
                                if (doc.RootElement.TryGetProperty("connection_name", out JsonElement connection_name))
                                {
                                    repo = GetRepository(connection_name.GetString()) as AIChessObjectRepository;
                                }
                                wid = new WindowID();
                                EndInvoke(BeginInvoke((Action)(() =>
                                {
                                    optConsolidate_Click(wid, repo);
                                })));
                                await _hConsole.ShowActivityFeedback(MSG_normalize_match_labels);
                                servedcalls++;
                                if (!string.IsNullOrEmpty(wid.UID))
                                {
                                    result["status"] = "information";
                                    result["message"] = MSG_OPerationCancelled;
                                }
                                else
                                {
                                    result["status"] = "success";
                                    IUIRemoteControlElement frc = FindController(wid.UID);
                                    if ((caller.AllowedServices != null) &&
                                        (extvs != null) &&
                                        (frc != null) &&
                                        caller.AllowedServices.Contains("automation"))
                                    {
                                        string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                        string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                        result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                    }
                                    else
                                    {
                                        result["message"] = MSG_UILaunched;
                                    }
                                }
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                                if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                                {
                                    item.Response.ChainedCall = true;
                                    item.Response.Message = new ContextMessage()
                                    {
                                        PlayerName = _play.User.PlayerName,
                                        PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                        Message = result["message"].ToString()
                                    };
                                }
                            }
                            break;
                        case "arrange_windows":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("mode", out JsonElement mode))
                                {
                                    MdiLayout lmodel;
                                    switch (mode.GetString().ToLower())
                                    {
                                        case "tile_vertical":
                                            lmodel = MdiLayout.TileVertical;
                                            break;
                                        case "tile_horizontal":
                                            lmodel = MdiLayout.TileHorizontal;
                                            break;
                                        case "cascade":
                                            lmodel = MdiLayout.Cascade;
                                            break;
                                        default:
                                            throw new Exception(string.Format(ERR_INVALIDPARAMVALUE, "mode", mode.GetString()));
                                    }
                                    BeginInvoke((Action)(() =>
                                    {
                                        LayoutMdi(lmodel);
                                    }));
                                    await _hConsole.ShowActivityFeedback(MSG_arrange_windows);
                                    servedcalls++;
                                    result["status"] = "success";
                                    result["message"] = MSG_UILaunched;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "mode"));
                                }
                            }
                            break;
                        #endregion
                        #region automation service
                        case "list_open_automation_forms":
                            await _hConsole.ShowActivityFeedback(MSG_list_open_automation_forms);
                            List<WindowID> formnames = new List<WindowID>();
                            foreach (Form chf in MdiChildren)
                            {
                                if (chf is IUIRemoteControlElement uichf)
                                {
                                    formnames.Add(new WindowID()
                                    {
                                        UID = string.Join("-", chf.Name, uichf.UID),
                                        FriendlyName = uichf.FriendlyName,
                                        ConnectionName = (chf as IChessDBWindow)?.Repository.ConnectionName,
                                        ServerType = (chf as IChessDBWindow)?.Repository.ServerName
                                    });
                                }
                            }
                            foreach (Form fown in OwnedForms)
                            {
                                if (fown is IUIRemoteControlElement uifown)
                                {
                                    WindowID id = new WindowID()
                                    {
                                        UID = string.Join("-", fown.Name, uifown.UID),
                                        FriendlyName = uifown.FriendlyName,
                                        ConnectionName = (fown as IChessDBWindow)?.Repository.ConnectionName,
                                        ServerType = (fown as IChessDBWindow)?.Repository.ServerName
                                    };
                                    if (!formnames.Contains(id))
                                    {
                                        formnames.Add(id);
                                    }
                                }
                            }
                            servedcalls++;
                            result["status"] = "success";
                            result["form_list"] = formnames;
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            break;
                        case "get_form_controls":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid))
                                {
                                    await _hConsole.ShowActivityFeedback(MSG_get_form_controls);
                                    IUIRemoteControlElement frc = FindController(form_uid.GetString());
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        result["status"] = "success";
                                        result["controls"] = frc.GetUIElements();
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid"));
                                }
                            }
                            break;
                        case "get_children_controls":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path))
                                {
                                    await _hConsole.ShowActivityFeedback(MSG_get_children_controls);
                                    IUIRemoteControlElement frc = ControllerFromPath(path.GetString());
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        result["status"] = "success";
                                        result["controls"] = frc.GetUIElementChildren(path.GetString());
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path"));
                                }
                            }
                            break;
                        case "highlight_control":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path))
                                {
                                    string ctlpath = path.GetString();
                                    int secs = 2;
                                    if (doc.RootElement.TryGetProperty("seconds", out JsonElement seconds))
                                    {
                                        secs = seconds.GetInt32();
                                    }
                                    fHighlight.HighlightMode mode = fHighlight.HighlightMode.Flash;
                                    if (doc.RootElement.TryGetProperty("highlight_mode", out JsonElement highlight_mode))
                                    {
                                        switch (highlight_mode.GetString())
                                        {
                                            case "circle":
                                                mode = fHighlight.HighlightMode.Circle;
                                                break;
                                            case "rectangle":
                                                mode = fHighlight.HighlightMode.Rectangle;
                                                break;
                                        }
                                    }
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        result["status"] = "success";
                                        BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            frc.HighlightUIElement(ctlpath, secs, mode.ToString());
                                        }));
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path"));
                                }
                            }
                            break;
                        case "highlight_and_comment_control":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path) &&
                                    doc.RootElement.TryGetProperty("message", out JsonElement message))
                                {
                                    string ctlpath = path.GetString();
                                    string msg = message.GetString();
                                    int secs = 5;
                                    if (doc.RootElement.TryGetProperty("seconds", out JsonElement seconds))
                                    {
                                        secs = seconds.GetInt32();
                                    }
                                    string ttl = "";
                                    if (doc.RootElement.TryGetProperty("title", out JsonElement title))
                                    {
                                        ttl = title.GetString();
                                    }
                                    fHighlight.HighlightMode mode = fHighlight.HighlightMode.Tooltip;
                                    if (doc.RootElement.TryGetProperty("highlight_mode", out JsonElement highlight_mode))
                                    {
                                        switch (highlight_mode.GetString())
                                        {
                                            case "flash":
                                                mode |= fHighlight.HighlightMode.Flash;
                                                break;
                                            case "circle":
                                                mode |= fHighlight.HighlightMode.Circle;
                                                break;
                                            case "rectangle":
                                                mode |= fHighlight.HighlightMode.Rectangle;
                                                break;
                                        }
                                    }
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    if (frc != null)
                                    {
                                        result["status"] = "success";
                                        BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            frc.CommentUIElement(ctlpath, ttl, msg, mode.ToString(), secs);
                                        }));
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path, message"));
                                }
                            }
                            break;
                        case "send_text_to_editor":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path) &&
                                    doc.RootElement.TryGetProperty("text", out JsonElement text))
                                {
                                    string ctlpath = path.GetString();
                                    string msg = text.GetString();
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    if (frc != null)
                                    {
                                        object resultmsg = "";
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            resultmsg = frc.InvokeElementAction(ctlpath, string.Join(":", "write", msg));
                                        })));
                                        if (resultmsg == null)
                                        {
                                            result["status"] = "success";
                                        }
                                        else
                                        {
                                            result["status"] = "error";
                                            result["message"] = resultmsg;
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path, text"));
                                }
                            }
                            break;
                        case "read_editor_text":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path))
                                {
                                    string ctlpath = path.GetString();
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    if (frc != null)
                                    {
                                        object resultmsg = "";
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            resultmsg = frc.InvokeElementAction(ctlpath, "read");
                                        })));
                                        result["status"] = "success";
                                        result["text"] = resultmsg;
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path"));
                                }
                            }
                            break;
                        case "click_control":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path))
                                {
                                    string ctlpath = path.GetString();
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    if (frc != null)
                                    {
                                        BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            frc.InvokeElementAction(ctlpath, "click");
                                        }));
                                        result["status"] = "success";
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path"));
                                }
                            }
                            break;
                        case "dropdown_or_expand":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path))
                                {
                                    string ctlpath = path.GetString();
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    if (frc != null)
                                    {
                                        BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            frc.InvokeElementAction(ctlpath, "expand");
                                        }));
                                        result["status"] = "success";
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path"));
                                }
                            }
                            break;
                        case "get_list_items":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path))
                                {
                                    string ctlpath = path.GetString();
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    if (frc != null)
                                    {
                                        object resultmsg = "";
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            resultmsg = frc.InvokeElementAction(ctlpath, "items");
                                        })));
                                        result["status"] = "success";
                                        result["text"] = resultmsg;
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path"));
                                }
                            }
                            break;
                        case "select_list_item":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("path", out JsonElement path) &&
                                    doc.RootElement.TryGetProperty("index", out JsonElement index))
                                {
                                    string ctlpath = path.GetString();
                                    int ix = index.GetInt32();
                                    IUIRemoteControlElement frc = ControllerFromPath(ctlpath);
                                    if (frc != null)
                                    {
                                        object resultmsg = "";
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)frc).Activate();
                                            resultmsg = frc.InvokeElementAction(ctlpath, string.Join(":", "select", ix));
                                        })));
                                        if (resultmsg == null)
                                        {
                                            result["status"] = "success";
                                        }
                                        else
                                        {
                                            result["status"] = "error";
                                            result["message"] = resultmsg;
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Owner window not found";
                                        await _hConsole.ShowActivityFeedback("Owner window not found");
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "path, index"));
                                }
                            }
                            break;
                        #endregion
                        #region chessplayer
                        case "get_windows_with_open_matches":
                            await _hConsole.ShowActivityFeedback(MSG_get_open_match_windows);
                            formnames = new List<WindowID>();
                            foreach (Form chf in MdiChildren)
                            {
                                if ((chf is IAIChessPlayer cpchf) && (cpchf.MatchInfo != null))
                                {
                                    formnames.Add(new WindowID()
                                    {
                                        UID = string.Join("-", chf.Name, cpchf.UID),
                                        FriendlyName = cpchf.FriendlyName,
                                        ConnectionName = (chf as IChessDBWindow)?.Repository.ConnectionName,
                                        ServerType = (chf as IChessDBWindow)?.Repository.ServerName
                                    });
                                }
                            }
                            foreach (Form fown in OwnedForms)
                            {
                                if ((fown is IAIChessPlayer cpfown) && (cpfown.MatchInfo != null))
                                {
                                    WindowID id = new WindowID()
                                    {
                                        UID = string.Join("-", fown.Name, cpfown.UID),
                                        FriendlyName = cpfown.FriendlyName,
                                        ConnectionName = (fown as IChessDBWindow)?.Repository.ConnectionName,
                                        ServerType = (fown as IChessDBWindow)?.Repository.ServerName
                                    };
                                    if (!formnames.Contains(id))
                                    {
                                        formnames.Add(id);
                                    }
                                }
                            }
                            servedcalls++;
                            if (formnames.Count == 0)
                            {
                                result["status"] = "info";
                                result["message"] = "No windows with open matches found";
                                await _hConsole.ShowActivityFeedback("No windows with open matches found");
                            }
                            else
                            {
                                result["status"] = "success";
                                result["form_list"] = formnames;
                            }
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            break;
                        case "get_current_match_info":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("play_window_id", out JsonElement play_window_id))
                                {
                                    IAIChessPlayer player = ChessPlayerFromPath(play_window_id.GetString());
                                    if (player == null)
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Chess player window not found";
                                        await _hConsole.ShowActivityFeedback("Chess player window not found");
                                    }
                                    else
                                    {
                                        result["status"] = "success";
                                        result["match_info"] = player.MatchInfo;
                                        result["editable"] = player.Editable;
                                        await _hConsole.ShowActivityFeedback(MSG_get_current_match_info);
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "play_window_id"));
                                }
                            }
                            break;
                        case "get_all_match_moves":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("play_window_id", out JsonElement play_window_id))
                                {
                                    IAIChessPlayer player = ChessPlayerFromPath(play_window_id.GetString());
                                    if (player == null)
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Chess player window not found";
                                        await _hConsole.ShowActivityFeedback("Chess player window not found");
                                    }
                                    else
                                    {
                                        result["status"] = "success";
                                        result["match_moves"] = player.Moves;
                                        await _hConsole.ShowActivityFeedback(MSG_get_current_match_moves);
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "play_window_id"));
                                }
                            }
                            break;
                        case "get_current_match_move":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("play_window_id", out JsonElement play_window_id))
                                {
                                    IAIChessPlayer player = ChessPlayerFromPath(play_window_id.GetString());
                                    if (player == null)
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Chess player window not found";
                                        await _hConsole.ShowActivityFeedback("Chess player window not found");
                                    }
                                    else
                                    {
                                        result["status"] = "success";
                                        result["current_move"] = player.CurrentMove;
                                        await _hConsole.ShowActivityFeedback(MSG_get_last_match_move);
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "play_window_id"));
                                }
                            }
                            break;
                        case "go_to_next_match_move":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("play_window_id", out JsonElement play_window_id))
                                {
                                    IAIChessPlayer player = ChessPlayerFromPath(play_window_id.GetString());
                                    string mcomment = null;
                                    if (doc.RootElement.TryGetProperty("comment", out JsonElement comment))
                                    {
                                        mcomment = comment.GetString();
                                    }
                                    if (player == null)
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Chess player window not found";
                                        await _hConsole.ShowActivityFeedback("Chess player window not found");
                                    }
                                    else
                                    {
                                        result["status"] = "success";
                                        List<MatchMove> mm = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)player).Activate();
                                            mm = player.NextMove(mcomment);
                                        })));
                                        string txtmove = "";
                                        if (mm[1].IdMatch != 0)
                                        {
                                            txtmove = string.Format(MSG_CurrentMove, mm[1].Player == 0 ? LAB_WHITE : LAB_BLACK, mm[1].ANText);
                                            result["current_board"] = mm[1].FinalPosition.Board.Board;
                                        }
                                        if (mm[2].IdMatch != 0)
                                        {
                                            txtmove += string.Format(MSG_NextMove, mm[2].Player == 0 ? LAB_WHITE : LAB_BLACK, mm[2].ANText);
                                        }
                                        else
                                        {
                                            txtmove += MSG_NoMoreMoves;
                                        }
                                        result["summary"] = txtmove;
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "play_window_id"));
                                }
                            }
                            break;
                        case "go_to_previous_match_move":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("play_window_id", out JsonElement play_window_id))
                                {
                                    IAIChessPlayer player = ChessPlayerFromPath(play_window_id.GetString());
                                    if (player == null)
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Chess player window not found";
                                        await _hConsole.ShowActivityFeedback("Chess player window not found");
                                    }
                                    else
                                    {
                                        result["status"] = "success";
                                        MatchMove mm = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)player).Activate();
                                            mm = player.PrevMove;
                                        })));
                                        result["previous_move"] = mm;
                                        await _hConsole.ShowActivityFeedback(MSG_go_to_previous_match_move);
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "play_window_id"));
                                }
                            }
                            break;
                        case "add_new_match_move":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("play_window_id", out JsonElement play_window_id) &&
                                    doc.RootElement.TryGetProperty("move_text", out JsonElement move_text))
                                {
                                    string comment = null;
                                    if (doc.RootElement.TryGetProperty("move_comment", out JsonElement move_comment))
                                    {
                                        comment = move_comment.GetString();
                                    }
                                    IAIChessPlayer player = ChessPlayerFromPath(play_window_id.GetString());
                                    if (player == null)
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Chess player window not found";
                                        await _hConsole.ShowActivityFeedback("Chess player window not found");
                                    }
                                    else
                                    {
                                        await _hConsole.ShowActivityFeedback(MSG_add_new_match_move);
                                        string error = null;
                                        string board = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)player).Activate();
                                            error = player.AddMove(move_text.GetString(), comment);
                                            board = player.CurrentBoard;
                                        })));
                                        if (string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "success";
                                            result["board_position"] = board;
                                        }
                                        else
                                        {
                                            result["status"] = "error";
                                            result["error_message"] = error;
                                        }
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "play_window_id, move_text"));
                                }
                            }
                            break;
                        case "send_complete_pgn_match":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("play_window_id", out JsonElement play_window_id) &&
                                    doc.RootElement.TryGetProperty("pgn_match", out JsonElement pgn_match))
                                {
                                    IAIChessPlayer player = ChessPlayerFromPath(play_window_id.GetString());
                                    if (player == null)
                                    {
                                        result["status"] = "error";
                                        result["message"] = "Chess player window not found";
                                        await _hConsole.ShowActivityFeedback("Chess player window not found");
                                    }
                                    else
                                    {
                                        await _hConsole.ShowActivityFeedback(MSG_send_complete_pgn_match);
                                        string resultmsg = "";
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            ((Form)player).Activate();
                                            resultmsg = player.AddMatch(pgn_match.GetString());
                                        })));
                                        result["status"] = "success";
                                        result["message"] = resultmsg;
                                    }
                                    servedcalls++;
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "play_window_id, move_text"));
                                }
                            }
                            break;
                        #endregion
                        #region configuration
                        case "show_application_configuration":
                            wid = null;
                            EndInvoke(BeginInvoke((Action)(() =>
                            {
                                wid = opAppConfiguration_Click(this, EventArgs.Empty);
                            })));
                            await _hConsole.ShowActivityFeedback(MSG_show_application_configuration);
                            servedcalls++;
                            if (wid == null)
                            {
                                result["status"] = "information";
                                result["message"] = MSG_OPerationCancelled;
                            }
                            else
                            {
                                result["status"] = "success";
                                IUIRemoteControlElement frc = FindController(wid.UID);
                                if ((caller.AllowedServices != null) &&
                                    (extvs != null) &&
                                    (frc != null) &&
                                    caller.AllowedServices.Contains("automation"))
                                {
                                    string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                    string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                    result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                }
                                else
                                {
                                    result["message"] = MSG_UILaunched;
                                }
                            }
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                            {
                                item.Response.ChainedCall = true;
                                item.Response.Message = new ContextMessage()
                                {
                                    PlayerName = _play.User.PlayerName,
                                    PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                    Message = result["message"].ToString()
                                };
                            }
                            break;
                        case "new_connection_string":
                        case "edit_connection_string":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                string conname = null;
                                if (doc.RootElement.TryGetProperty("connection_name", out JsonElement connection_name))
                                {
                                    conname = connection_name.GetString();
                                }
                                else if (item.FunctionName.ToLower() == "edit_connection_string")
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "connection_name"));
                                }
                                wid = null;
                                EndInvoke(BeginInvoke((Action)(() =>
                                {
                                    wid = opCreateConnectionString(conname);
                                })));
                                servedcalls++;
                                if (wid == null)
                                {
                                    result["status"] = "information";
                                    result["message"] = MSG_OPerationCancelled;
                                }
                                else
                                {
                                    result["status"] = "success";
                                    IUIRemoteControlElement frc = FindController(wid.UID);
                                    if ((caller.AllowedServices != null) &&
                                        (extvs != null) &&
                                        (frc != null) &&
                                        caller.AllowedServices.Contains("automation"))
                                    {
                                        string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                        string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                        result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                    }
                                    else
                                    {
                                        result["message"] = MSG_UILaunched;
                                    }
                                }
                                item.Response = new FunctionResponse() { NeedsInteraction = false };
                                if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                                {
                                    item.Response.ChainedCall = true;
                                    item.Response.Message = new ContextMessage()
                                    {
                                        PlayerName = _play.User.PlayerName,
                                        PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                        Message = result["message"].ToString()
                                    };
                                }
                            }
                            break;
                        case "show_play_editor":
                            wid = null;
                            EndInvoke(BeginInvoke((Action)(() =>
                            {
                                wid = opEditPlay_Click(this, EventArgs.Empty);
                            })));
                            await _hConsole.ShowActivityFeedback(MSG_show_play_editor);
                            servedcalls++;
                            if (wid == null)
                            {
                                result["status"] = "information";
                                result["message"] = MSG_OPerationCancelled;
                            }
                            else
                            {
                                result["status"] = "success";
                                IUIRemoteControlElement frc = FindController(wid.UID);
                                if ((caller.AllowedServices != null) &&
                                    (extvs != null) &&
                                    (frc != null) &&
                                    caller.AllowedServices.Contains("automation"))
                                {
                                    string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                    string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                    result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                }
                                else
                                {
                                    result["message"] = MSG_UILaunched;
                                }
                            }
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                            {
                                item.Response.ChainedCall = true;
                                item.Response.Message = new ContextMessage()
                                {
                                    PlayerName = _play.User.PlayerName,
                                    PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                    Message = result["message"].ToString()
                                };
                            }
                            break;
                        case "show_assistants_editor":
                            wid = null;
                            EndInvoke(BeginInvoke((Action)(() =>
                            {
                                wid = opEditAssistants_Click(this, EventArgs.Empty);
                            })));
                            await _hConsole.ShowActivityFeedback(MSG_show_assistants_editor);
                            servedcalls++;
                            if (wid == null)
                            {
                                result["status"] = "information";
                                result["message"] = MSG_OPerationCancelled;
                            }
                            else
                            {
                                result["status"] = "success";
                                IUIRemoteControlElement frc = FindController(wid.UID);
                                if ((caller.AllowedServices != null) &&
                                    (extvs != null) &&
                                    (frc != null) &&
                                    caller.AllowedServices.Contains("automation"))
                                {
                                    string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                    string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                    result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                }
                                else
                                {
                                    result["message"] = MSG_UILaunched;
                                }
                            }
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                            {
                                item.Response.ChainedCall = true;
                                item.Response.Message = new ContextMessage()
                                {
                                    PlayerName = _play.User.PlayerName,
                                    PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                    Message = result["message"].ToString()
                                };
                            }
                            break;
                        case "show_updates_editor":
                            wid = null;
                            EndInvoke(BeginInvoke((Action)(() =>
                            {
                                wid = opEditUpdates_Click(this, EventArgs.Empty);
                            })));
                            await _hConsole.ShowActivityFeedback(MSG_show_updates_editor);
                            servedcalls++;
                            if (wid == null)
                            {
                                result["status"] = "information";
                                result["message"] = MSG_OPerationCancelled;
                            }
                            else
                            {
                                result["status"] = "success";
                                IUIRemoteControlElement frc = FindController(wid.UID);
                                if ((caller.AllowedServices != null) &&
                                    (extvs != null) &&
                                    (frc != null) &&
                                    caller.AllowedServices.Contains("automation"))
                                {
                                    string controls = JsonSerializer.Serialize(frc.GetAllUIElements());
                                    string fname = string.Format("{0}_{1}_controls.json", caller.PlayerName.Replace(" ", ""), frc.GetType().Name);
                                    result["message"] = await UploadFileToAssistant(fname, controls, extvs, new TimeSpan(2, 0, 0), string.Format(MSG_ControlsUploaded, fname, wid.UID), MSG_UILaunched);
                                }
                                else
                                {
                                    result["message"] = MSG_UILaunched;
                                }
                            }
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            if ((api != null) && (result["message"].ToString() != MSG_UILaunched))
                            {
                                item.Response.ChainedCall = true;
                                item.Response.Message = new ContextMessage()
                                {
                                    PlayerName = _play.User.PlayerName,
                                    PlayerRole = api.APIManager.GetPlatformRole(GenericRole.User, api),
                                    Message = result["message"].ToString()
                                };
                            }
                            break;
                        case "copy_text_to_clipboard":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("text", out JsonElement text))
                                {
                                    Clipboard.SetText(text.GetString());
                                    servedcalls++;
                                    result["status"] = "success";
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "text"));
                                }
                            }
                            break;
                        #endregion
                        #region memoryandnotes
                        case "add_new_note":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("text", out JsonElement text))
                                {
                                    await _hConsole.ShowActivityFeedback(MSG_add_new_note);
                                    string notesfile = Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], string.Join("_", api?.Name ?? caller.PlayerName, "notes.txt"));
                                    string notes = "";
                                    if (File.Exists(notesfile))
                                    {
                                        notes = File.ReadAllText(notesfile);
                                    }
                                    string.Join("\n", text.GetString(), notes);
                                    File.WriteAllText(notesfile, notes);
                                    if (extvs != null)
                                    {
                                        string id = await extvs.FileManager.GetFileIdByName(Path.GetFileName(notesfile));
                                        if (!string.IsNullOrEmpty(id))
                                        {
                                            await _hConsole.ShowActivityFeedback(MSG_add_new_note2);
                                            await extvs.RemoveDocument(id, true);
                                            await Task.Delay(1000); // Wait for the file to be removed
                                            await extvs.FileManager.DeleteFile(id, null);
                                            await Task.Delay(1000); // Wait for the file to be removed
                                            ObjectWrapper ow = await extvs.FileManager.UploadFile(Path.GetFileName(notesfile));
                                            await Task.Delay(1000); // Wait for the file to be uploaded
                                            await extvs.StoreFromFileManager(ow.UID);
                                        }
                                    }
                                    servedcalls++;
                                    result["status"] = "success";
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "text"));
                                }
                            }
                            break;
                        case "rewrite_all_notes":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("text", out JsonElement text))
                                {
                                    await _hConsole.ShowActivityFeedback(MSG_rewrite_all_notes);
                                    string notesfile = Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], string.Join("_", api?.Name ?? caller.PlayerName, "notes.txt"));
                                    if (File.Exists(notesfile))
                                    {
                                        File.Delete(notesfile);
                                    }
                                    File.WriteAllText(notesfile, text.GetString());
                                    if (extvs != null)
                                    {
                                        string id = await extvs.FileManager.GetFileIdByName(Path.GetFileName(notesfile));
                                        if (!string.IsNullOrEmpty(id))
                                        {
                                            await _hConsole.ShowActivityFeedback(MSG_add_new_note2);
                                            await extvs.RemoveDocument(id, true);
                                            await Task.Delay(1000); // Wait for the file to be removed
                                            await extvs.FileManager.DeleteFile(id, null);
                                            await Task.Delay(1000); // Wait for the file to be removed
                                            ObjectWrapper ow = await extvs.FileManager.UploadFile(Path.GetFileName(notesfile));
                                            await Task.Delay(1000); // Wait for the file to be uploaded
                                            await extvs.StoreFromFileManager(ow.UID);
                                        }
                                    }
                                    servedcalls++;
                                    result["status"] = "success";
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "text"));
                                }
                            }
                            break;
                        #endregion
                        #region querydatabase
                        case "list_open_query_forms":
                            await _hConsole.ShowActivityFeedback(MSG_list_open_query_forms);
                            formnames = new List<WindowID>();
                            foreach (Form chf in MdiChildren)
                            {
                                if ((chf is IUIRemoteControlElement uichf) && (chf is QueryWindow))
                                {
                                    formnames.Add(new WindowID()
                                    {
                                        UID = string.Join("-", chf.Name, uichf.UID),
                                        FriendlyName = uichf.FriendlyName,
                                        ConnectionName = (chf as IChessDBWindow)?.Repository.ConnectionName,
                                        ServerType = (chf as IChessDBWindow)?.Repository.ServerName
                                    });
                                }
                            }
                            servedcalls++;
                            result["status"] = "success";
                            result["form_list"] = formnames;
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            break;
                        case "set_query_view":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid) &&
                                    doc.RootElement.TryGetProperty("view_name", out JsonElement view_name))
                                {
                                    string vname = view_name.GetString().ToLower();
                                    if ((vname != "vw_match_keywords") &&
                                        (vname != "vw_match_moves") &&
                                        (vname != "vw_match_positions"))
                                    {
                                        throw new Exception(string.Format(ERR_UNKNOWNVIEWNAME, vname));
                                    }
                                    await _hConsole.ShowActivityFeedback(MSG_set_query_view);
                                    QueryWindow frc = FindController(form_uid.GetString()) as QueryWindow;
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        List<DatabaseFieldInfo> fields = new List<DatabaseFieldInfo>();
                                        string error = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            try
                                            {
                                                ISQLUIQuery qf = frc.ChangeView(vname);
                                                // Get field information from the first table (the view)
                                                Table t = qf.Tables[0] as Table;
                                                foreach (TableColumn tc in t.Columns)
                                                {
                                                    DatabaseFieldInfo fi = new DatabaseFieldInfo()
                                                    {
                                                        Name = tc.Name,
                                                        DataType = tc.NativeType.GetSQL(SQLScope.Console)
                                                    };
                                                    fields.Add(fi);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                error = ex.Message;
                                            }
                                        })));
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "error";
                                            result["message"] = error;
                                        }
                                        else
                                        {
                                            result["status"] = "success";
                                            result["total_matches_in_database"] = frc.TotalMatches;
                                            result["field_information"] = fields;
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid, view_name"));
                                }
                            }
                            break;
                        case "add_filter_expression":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid) &&
                                    doc.RootElement.TryGetProperty("filter_expression", out JsonElement filter_expression))
                                {
                                    string fexpr = filter_expression.GetString();
                                    await _hConsole.ShowActivityFeedback(string.Format(MSG_add_filter_expression, fexpr));
                                    QueryWindow frc = FindController(form_uid.GetString()) as QueryWindow;
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        string error = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            try
                                            {
                                                ISQLUIQuery qf = frc.GetCurrentQuery();
                                                // Build a SQL expression from the filter expression
                                                SQLExpression sexpr = ((ISQLElementBuilder)qf).BuildExpression(ref fexpr) as SQLExpression;
                                                UIFilterExpression uif = new UIFilterExpression();
                                                uif.SetElement(sexpr);
                                                frc.AddFilter(uif);
                                            }
                                            catch (Exception ex)
                                            {
                                                error = ex.Message;
                                            }
                                        })));
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "error";
                                            result["message"] = error;
                                        }
                                        else
                                        {
                                            result["status"] = "success";
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid, filter_expression"));
                                }
                            }
                            break;
                        case "get_current_filters":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid))
                                {
                                    await _hConsole.ShowActivityFeedback(MSG_get_current_filters);
                                    QueryWindow frc = FindController(form_uid.GetString()) as QueryWindow;
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        List<DatabaseFilterInfo> filters = new List<DatabaseFilterInfo>();
                                        string error = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            try
                                            {
                                                ISQLUIQuery qf = frc.GetCurrentQuery();
                                                if (qf.UserWhereFilters != null)
                                                {
                                                    for (int ix = 0; ix < qf.UserWhereFilters.Count; ix++)
                                                    {
                                                        UIFilterExpression fex = new UIFilterExpression();
                                                        fex.SetElement(qf.UserWhereFilters[ix].Element);
                                                        DatabaseFilterInfo dfi = new DatabaseFilterInfo()
                                                        {
                                                            FilterIndex = ix,
                                                            FriendlyText = fex.FriendlyName,
                                                            SQLText = qf.UserWhereFilters[ix].Element.GetSQL(SQLScope.Where)
                                                        };
                                                        filters.Add(dfi);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                error = ex.Message;
                                            }
                                        })));
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "error";
                                            result["message"] = error;
                                        }
                                        else
                                        {
                                            result["status"] = "success";
                                            result["filters"] = filters;
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid"));
                                }
                            }
                            break;
                        case "remove_filter_expressions":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid))
                                {
                                    List<int> indexes = null;
                                    if (doc.RootElement.TryGetProperty("expression_indexes", out JsonElement expression_indexes))
                                    {
                                        indexes = new List<int>();
                                        List<JsonElement> jvalues = new List<JsonElement>(expression_indexes.EnumerateArray());
                                        foreach (JsonElement findex in jvalues)
                                        {
                                            indexes.Add(findex.GetInt32());
                                        }
                                    }
                                    await _hConsole.ShowActivityFeedback(MSG_remove_filter_expressions);
                                    QueryWindow frc = FindController(form_uid.GetString()) as QueryWindow;
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        string error = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            try
                                            {
                                                frc.RemoveFilters(indexes);
                                            }
                                            catch (Exception ex)
                                            {
                                                error = ex.Message;
                                            }
                                        })));
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "error";
                                            result["message"] = error;
                                        }
                                        else
                                        {
                                            result["status"] = "success";
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid"));
                                }
                            }
                            break;
                        case "get_order_by_list":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid))
                                {
                                    await _hConsole.ShowActivityFeedback(MSG_get_current_filters);
                                    QueryWindow frc = FindController(form_uid.GetString()) as QueryWindow;
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        List<string> orderby = new List<string>();
                                        string error = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            try
                                            {
                                                ISQLUIQuery qf = frc.GetCurrentQuery();
                                                if (qf.UserOrderBy != null)
                                                {
                                                    for (int ix = 0; ix < qf.UserOrderBy.Elements.Count; ix++)
                                                    {
                                                        orderby.Add(qf.UserOrderBy.Elements[ix].GetSQL(SQLScope.OrderBy));
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                error = ex.Message;
                                            }
                                        })));
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "error";
                                            result["message"] = error;
                                        }
                                        else
                                        {
                                            result["status"] = "success";
                                            result["filters"] = orderby;
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid"));
                                }
                            }
                            break;
                        case "set_order_by_list":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid))
                                {
                                    List<string> fields = null;
                                    if (doc.RootElement.TryGetProperty("field_list", out JsonElement field_list))
                                    {
                                        fields = new List<string>();
                                        List<JsonElement> jvalues = new List<JsonElement>(field_list.EnumerateArray());
                                        foreach (JsonElement findex in jvalues)
                                        {
                                            fields.Add(findex.GetString());
                                        }
                                    }
                                    await _hConsole.ShowActivityFeedback(MSG_remove_filter_expressions);
                                    QueryWindow frc = FindController(form_uid.GetString()) as QueryWindow;
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        string error = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            try
                                            {
                                                frc.SetOrderBy(fields);
                                            }
                                            catch (Exception ex)
                                            {
                                                error = ex.Message;
                                            }
                                        })));
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "error";
                                            result["message"] = error;
                                        }
                                        else
                                        {
                                            result["status"] = "success";
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid"));
                                }
                            }
                            break;
                        case "refresh_query_data":
                            using (JsonDocument doc = JsonDocument.Parse(item.Parameters))
                            {
                                if (doc.RootElement.TryGetProperty("form_uid", out JsonElement form_uid))
                                {
                                    await _hConsole.ShowActivityFeedback(MSG_refresh_query_data);
                                    QueryWindow frc = FindController(form_uid.GetString()) as QueryWindow;
                                    servedcalls++;
                                    if (frc != null)
                                    {
                                        string error = null;
                                        EndInvoke(BeginInvoke((Action)(() =>
                                        {
                                            try
                                            {
                                                frc.RefreshQueryData();
                                            }
                                            catch (Exception ex)
                                            {
                                                error = ex.Message;
                                            }
                                        })));
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                            result["status"] = "error";
                                            result["message"] = error;
                                        }
                                        else
                                        {
                                            result["status"] = "success";
                                            result["result_count"] = frc.TotalResults;
                                            result["total_matches_in_database"] = frc.TotalMatches;
                                        }
                                    }
                                    else
                                    {
                                        result["status"] = "error";
                                        result["message"] = "No such window found";
                                        await _hConsole.ShowActivityFeedback("No such window found");
                                    }
                                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                                }
                                else
                                {
                                    throw new Exception(string.Format(ERR_PARAMMANDATORY, "form_uid"));
                                }
                            }
                            break;
                            #endregion
                    }
                }
                catch (Exception ex)
                {
                    result.Clear();
                    result["status"] = "error";
                    result["message"] = ex.Message;
                    servedcalls++;
                    item.Response = new FunctionResponse() { NeedsInteraction = false };
                    await _hConsole.ShowActivityFeedback(ex.Message);
                }
                if (item.Response != null)
                {
                    item.Response.Result = JsonSerializer.Serialize(result);
                }
            }
            if (!string.IsNullOrEmpty(_fcallLog))
            {
                using (StreamWriter wrt = new StreamWriter(_fcallLog, true))
                {
                    foreach (FunctionBatchItem item in calls.Calls)
                    {
                        if (item.Response != null)
                        {
                            wrt.WriteLine(string.Format("{0}; {1} => {2}, {3}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "App", item.Response.Result, item.Response.Message?.Message));
                        }
                    }
                }
            }
            if (calls.Calls.Count > servedcalls)
            {
                if (calls.Calls.Count > servedcalls)
                {
                    foreach (FunctionBatchItem item in calls.Calls)
                    {
                        if (item.Response == null)
                        {
                            result.Clear();
                            result["status"] = "error";
                            result["message"] = "Unknown function name.";
                            item.Response = new FunctionResponse() { NeedsInteraction = false };
                            item.Response.Result = JsonSerializer.Serialize(result);
                            if (!string.IsNullOrEmpty(_fcallLog))
                            {
                                using (StreamWriter wrt = new StreamWriter(_fcallLog, true))
                                {

                                    wrt.WriteLine(string.Format("{0}; {1} => {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), item.FunctionName, "Unknown function name."));
                                }
                            }
                        }
                    }
                }
            }
            return calls;
        }
        /// <summary>
        /// IAppAutomation: Capture an image from the active form, upload it to a server, and set the image URL in the message
        /// </summary>
        /// <param name="msg">
        /// Message to set the image URL
        /// </param>
        /// <param name="format">
        /// Data format
        /// </param>
        /// <param name="filename">
        /// Name to save the image in the server. Null to use a default name.
        /// </param>
        /// <returns>
        /// Key to delete the image from the server
        /// </returns>
        public async Task<string> CaptureActiveForm(ContextMessage msg, ImageFormat format, string filename)
        {
            await Task.Yield();
            return null;
        }
        /// <summary>
        /// Find a MDI child window by its identifier
        /// </summary>
        /// <param name="id">
        /// Window identifier
        /// </param>
        /// <returns>
        /// Window foud or null if not found
        /// </returns>
        private Form FindWindow(string id)
        {
            foreach (Form chf in MdiChildren)
            {
                IChessDBWindow pw = chf as IChessDBWindow;
                if (pw != null)
                {
                    if (id == pw.WINDOW_ID)
                    {
                        return chf;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Center an MDI child inside the visible MDI client area (accounts for toolbars/statusbars and scroll).
        /// </summary>
        /// <param name="child">
        /// MDI child form to center
        /// </param>
        public void CenterMdiChild(Form child)
        {
            if (child == null) return;

            // Ensure this will be an MDI child
            child.MdiParent = this;
            child.StartPosition = FormStartPosition.Manual;

            // Find the MDI client area where children live
            var mdiClient = Controls.OfType<MdiClient>().FirstOrDefault();
            int offset = 0;
            if (mdiClient == null)
            {
                // Fallback: center on parent client rectangle
                var x = (ClientSize.Width - child.Width) / 2;
                var y = (ClientSize.Height - child.Height) / 2;
                // Do not cover the console when possible
                if (_hConsole.Parent.Left < (x + child.Width))
                {
                    offset = (_hConsole.Parent.Left - (x + child.Width)) - 4;
                }
                child.Location = new Point(Math.Max(0, x + offset), Math.Max(0, y));
                return;
            }

            // Account for current scroll (AutoScrollPosition is negative when scrolled)
            var scrollOrigin = new Point(-mdiClient.AutoScrollOffset.X, -mdiClient.AutoScrollOffset.Y);

            int cx = scrollOrigin.X + (mdiClient.ClientSize.Width - child.Width) / 2;
            int cy = scrollOrigin.Y + (mdiClient.ClientSize.Height - child.Height) / 2;
            // Do not cover the console when possible
            if (_hConsole.Parent.Left < (cx + child.Width))
            {
                offset = (_hConsole.Parent.Left - (cx + child.Width)) - 4;
            }
            child.Location = new Point(Math.Max(0, cx + offset), Math.Max(0, cy));
        }
        /// <summary>
        /// Center an MDI child inside the visible MDI client area (accounts for toolbars/statusbars and scroll).
        /// </summary>
        /// <param name="dlg">
        /// Dialog box to center
        /// </param>
        public void CenterDialogBox(Form dlg)
        {
            if (dlg == null) return;

            dlg.StartPosition = FormStartPosition.Manual;

            // Find the MDI client area where children live
            var mdiClient = Controls.OfType<MdiClient>().FirstOrDefault();
            if (mdiClient == null)
            {
                // Fallback: center on parent client rectangle
                var x = (ClientSize.Width - dlg.Width) / 2;
                var y = (ClientSize.Height - dlg.Height) / 2;
                // Do not cover the console when possible
                int offset = 0;
                if (_hConsole.Parent.Left < (x + dlg.Width))
                {
                    offset = (_hConsole.Parent.Left - (x + dlg.Width)) - 4;
                }
                dlg.Location = new Point(Math.Max(0, x + offset), Math.Max(0, y));
                return;
            }

            // Account for current scroll (AutoScrollPosition is negative when scrolled)
            var scrollOrigin = new Point(-mdiClient.AutoScrollOffset.X, -mdiClient.AutoScrollOffset.Y);

            int cx = scrollOrigin.X + (mdiClient.ClientSize.Width - dlg.Width) / 2;
            int cy = scrollOrigin.Y + (mdiClient.ClientSize.Height - dlg.Height) / 2;

            dlg.Location = new Point(Math.Max(0, cx), Math.Max(0, cy));
        }
        /// <summary>
        /// Find an IUIRemoteControlElement controller by its identifier
        /// </summary>
        /// <param name="identifier">
        /// Window name-IUIRemoteControlElement.UID
        /// </param>
        /// <returns>
        /// Form found or null if not found
        /// </returns>
        private IUIRemoteControlElement FindController(string identifier)
        {
            List<string> nameparts = identifier.Split('-').ToList();
            string fname = nameparts[0];
            nameparts.RemoveAt(0);
            string uid = string.Join("-", nameparts);
            foreach (Form chf in MdiChildren)
            {
                IUIRemoteControlElement frct = chf as IUIRemoteControlElement;
                if ((frct != null) &&
                    (chf.Name == fname) &&
                    (frct.UID == uid))
                {
                    return frct;
                }
            }
            foreach (Form fown in OwnedForms)
            {
                IUIRemoteControlElement frct = fown as IUIRemoteControlElement;
                if ((frct != null) &&
                    (fown.Name == fname) &&
                    (frct.UID == uid))
                {
                    return frct;
                }
            }
            return null;
        }
        /// <summary>
        /// Find the Controller that owns a given control path
        /// </summary>
        /// <param name="path">
        /// Control path
        /// </param>
        /// <returns>
        /// Form found or null if not found
        /// </returns>
        private IUIRemoteControlElement ControllerFromPath(string path)
        {
            string[] pathparts = path.Split('.');
            string uid = pathparts[0];
            if (uid.Contains('-'))
            {
                List<string> nameparts = uid.Split('-').ToList();
                string fname = nameparts[0];
                nameparts.RemoveAt(0);
                uid = string.Join("-", nameparts);
            }
            foreach (Form chf in MdiChildren)
            {
                IUIRemoteControlElement frct = chf as IUIRemoteControlElement;
                if ((frct != null) &&
                    (frct.UID == uid))
                {
                    return frct;
                }
            }
            foreach (Form fown in OwnedForms)
            {
                IUIRemoteControlElement frct = fown as IUIRemoteControlElement;
                if ((frct != null) &&
                    (frct.UID == uid))
                {
                    return frct;
                }
            }
            return null;
        }
        /// <summary>
        /// Find a chess player window from its path
        /// </summary>
        /// <param name="path">
        /// Window path
        /// </param>
        /// <returns>
        /// Form found or null if not found
        /// </returns>
        private IAIChessPlayer ChessPlayerFromPath(string path)
        {
            string[] pathparts = path.Split('.');
            string uid = pathparts[0];
            if (uid.Contains('-'))
            {
                List<string> nameparts = uid.Split('-').ToList();
                string fname = nameparts[0];
                nameparts.RemoveAt(0);
                uid = string.Join("-", nameparts);
            }
            foreach (Form chf in MdiChildren)
            {
                IAIChessPlayer frct = chf as IAIChessPlayer;
                if ((frct != null) &&
                    (frct.UID == uid))
                {
                    return frct;
                }
            }
            foreach (Form fown in OwnedForms)
            {
                IAIChessPlayer frct = fown as IAIChessPlayer;
                if ((frct != null) &&
                    (frct.UID == uid))
                {
                    return frct;
                }
            }
            return null;
        }
        /// <summary>
        /// IAppAutomation: Upload an image to the server
        /// </summary>
        /// <param name="msg">
        /// Message to set the image URL. It can be null.
        /// </param>
        /// <param name="image">
        /// Data to upload or null to use filename
        /// </param>
        /// <param name="format">
        /// Data format
        /// </param>
        /// <param name="filename">
        /// Data path
        /// </param>
        /// <returns>
        /// Key to delete the image from the server
        /// </returns>
        public async Task<string> UploadImage(ContextMessage msg, Bitmap image, ImageFormat format, string filename)
        {
            await Task.Yield();
            return "";
        }
        /// <summary>
        /// IAppAutomation: Remove an image from the server
        /// </summary>
        /// <param name="deletekey">
        /// Data deletion key
        /// </param>
        /// <returns>
        /// Error message or null if successful
        /// </returns>
        public async Task<string> RemoveImageFromServer(string deletekey)
        {
            await Task.Yield();
            return "";
        }
        /// <summary>
        /// IAppAutomation: Log token usage for a given player
        /// </summary>
        /// <param name="playername">
        /// Assistant name
        /// </param>
        /// <param name="model">
        /// Model used for the request
        /// </param>
        /// <param name="temperature">
        /// Value for the temperature parameter used in the request
        /// </param>
        /// <param name="topp">
        /// Value for the topp parameter used in the request
        /// </param>
        /// <param name="input_tokens">
        /// Tokens used for input (prompt)
        /// </param>
        /// <param name="output_tokens">
        /// Tokens generated by the AI assistant (response)
        /// </param>
        /// <param name="audio_input_tokens">
        /// Audio input tokens used for the request, if any.
        /// </param>
        /// <param name="audio_output_tokens">
        /// Audio output tokens generated by the AI assistant, if any.
        /// </param>
        /// <param name="reasoning_tokens">
        /// Reasoning tokens used by the AI assistant, if any.
        /// </param>
        /// <param name="input_cached_tokens">
        /// Tokens used for cached input, if any.
        /// </param>
        /// <param name="input_image_tokens">
        /// Tokens used for input images, if any.
        /// </param>
        /// <param name="input_text_tokens">
        /// Tokens used for input text, if any.
        /// </param>
        public async Task LogUsageData(string playername, string model, double temperature, double topp, int input_tokens, int output_tokens,
            int audio_input_tokens = 0, int audio_output_tokens = 0, int reasoning_tokens = 0, int input_cached_tokens = 0, int input_image_tokens = 0, int input_text_tokens = 0)
        {
            try
            {
                await Task.Yield();
                if (!string.IsNullOrEmpty(_usagelog))
                {
                    LogRecord usage = new LogRecord()
                    {
                        Path = _usagelog,
                        Data = new List<string> { DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm:ss"),
                        playername, model, temperature.ToString(), topp.ToString(),
                        input_tokens.ToString(), output_tokens.ToString(),
                        audio_input_tokens.ToString(), audio_output_tokens.ToString(),
                        reasoning_tokens.ToString(), input_cached_tokens.ToString(),
                        input_image_tokens.ToString(), input_text_tokens.ToString()
                    }
                    };
                    if (!File.Exists(_usagelog))
                    {
                        using (StreamWriter wrt = new StreamWriter(_usagelog))
                        {
                            wrt.WriteLine(string.Join(usage.Sepator, new string[] {"USAGE_DATE", "PLAYER_NAME", "MODEL_NAME","TEMPERATURE",
                                "TOP_P","INPUT_TOKENS","OUTPUT_TOKENS","AUDIO_INPUT_TOKENS","AUDIO_OUTPUT_TOKENS","REASONING_TOKENS",
                                "INPUT_CACHED_TOKENS","INPUT_IMAGE_TOKENS","INPUT_TEXT_TOKENS" }));
                        }
                    }
                    _logQueue.Enqueue(usage);
                }
            }
            catch { }
        }
        /// <summary>
        /// Execute a direct command from the console menus
        /// </summary>
        /// <param name="sender">
        /// Console object
        /// </param>
        /// <param name="cmdid">
        /// Command identifier
        /// </param>
        /// <param name="parameters">
        /// Serialized JSON string with parameter values.
        /// </param>
        private async Task<FunctionBatchItem> ExecuteConsoleCommandAsync(object sender, string cmdid, string parameters = null)
        {
            FunctionBatch batch = new FunctionBatch()
            {
                Identifier = Guid.NewGuid().ToString(),
                Calls = new List<FunctionBatchItem>()
            };
            batch.Calls.Add(new FunctionBatchItem()
            {
                id = Guid.NewGuid().ToString(),
                FunctionName = cmdid,
                Parameters = string.IsNullOrEmpty(parameters) ? "{}" : parameters
            });
            _hConsole.EnableUserInput = false;
            batch = await CallFunction(_play.User, batch);
            _hConsole.EnableUserInput = true;
            return batch.Calls[0];
        }
        /// <summary>
        /// Load a configuration object from a Json file
        /// </summary>
        /// <param name="path">
        /// Path to the configuration file
        /// </param>
        /// <param name="objtype">
        /// GenericType of object to create
        /// </param>
        /// <returns>
        /// New object instance with or without configuration (if the file doesn't exists)
        /// </returns>
        private object LoadConfiguration(string path, Type objtype)
        {
            if (File.Exists(path))
            {
                using (StreamReader rdr = new StreamReader(path))
                {
                    string json = rdr.ReadToEnd();
                    return JsonSerializer.Deserialize(json, objtype);
                }
            }
            return objtype.GetConstructor(Type.EmptyTypes).Invoke(null);
        }
        /// <summary>
        /// Save a configuration object to a Json file
        /// </summary>
        /// <param name="path">
        /// Path to the configuration file
        /// </param>
        /// <param name="obj">
        /// Object to save
        /// </param>
        private void SaveConfiguration(string path, object obj)
        {
            using (StreamWriter wrt = new StreamWriter(path))
            {
                wrt.Write(JsonSerializer.Serialize(obj, obj.GetType(), new JsonSerializerOptions { WriteIndented = true }));
            }
        }
        /// <summary>
        /// Initialize the application services
        /// </summary>
        private async Task InitializeServices()
        {
            await Task.Yield();
            List<FunctionDef> functions = LoadConfiguration(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath], ConfigurationManager.AppSettings[SETTING_appFunctions]),
                typeof(List<FunctionDef>)) as List<FunctionDef>;
            _functions = new List<FunctionDef>();
            foreach (FunctionDef function in functions)
            {
                if (!function.HasPrefix())
                {
                    _functions.Add(new FunctionDef(function));
                }
            }
            _services = new List<ObjectWrapper<IApplicationService>>()
            {
                new ObjectWrapper<IApplicationService>(new BaseAppService(this, "userinterface", userinterface, functions, "userinterface"), SRV_userinterface, "userinterface"),
                new ObjectWrapper<IApplicationService>(new BaseAppService(this, "automation", automation, functions, "automation"), SRV_automation, "automation"),
                new ObjectWrapper<IApplicationService>(new BaseAppService(this, "chessplayer", chessplayer, functions, "chessplayer"), SRV_chessplayer, "chessplayer"),
                new ObjectWrapper<IApplicationService>(new BaseAppService(this, "configuration", configuration, functions, "configuration"), SRV_configuration, "configuration"),
                new ObjectWrapper<IApplicationService>(new BaseAppService(this, "memoryandnotes", memoryandnotes, functions, "memoryandnotes"), SRV_memoryandnotes, "memoryandnotes"),
                new ObjectWrapper<IApplicationService>(new BaseAppService(this, "querydatabase", querydatabase, functions, "querydatabase"), SRV_querydatabase, "querydatabase")
            };
        }
        /// <summary>
        /// Import a single PGN file
        /// </summary>
        /// <param name="filename">
        /// File path to import
        /// </param>
        private WindowID ImportPGN(string filename, AIChessObjectRepository repo)
        {
            PGNWindow wpgn = new PGNWindow();
            wpgn.Provider = this;
            CenterMdiChild(wpgn);
            wpgn.Repository = repo ?? _repository;
            wpgn.PGNFilename = filename;
            wpgn.Show();
            wpgn.BringToFront();
            return new WindowID()
            {
                UID = string.Join("-", wpgn.GetType().Name, wpgn.UID),
                FriendlyName = wpgn.FriendlyName,
                ConnectionName = wpgn.Repository.ConnectionName,
                ServerType = wpgn.Repository.ServerName
            };
        }
        /// <summary>
        /// Import a BMF file (Binary Match File)
        /// </summary>
        /// <param name="filename">
        /// File path to import
        /// </param>
        private WindowID ImportBMF(string filename, AIChessObjectRepository repo)
        {
            PGNWindow wpgn = new PGNWindow();
            CenterMdiChild(wpgn);
            wpgn.Provider = this;
            wpgn.Repository = repo ?? _repository;
            wpgn.BMFFilename = filename;
            wpgn.Show();
            wpgn.BringToFront();
            return new WindowID()
            {
                UID = string.Join("-", wpgn.GetType().Name, wpgn.UID),
                FriendlyName = wpgn.FriendlyName,
                ConnectionName = wpgn.Repository.ConnectionName,
                ServerType = wpgn.Repository.ServerName
            };
        }
        /// <summary>
        /// Upload a text file to the assistant document store
        /// </summary>
        /// <param name="fname">
        /// File name
        /// </param>
        /// <param name="content">
        /// File content
        /// </param>
        /// <param name="extvs">
        /// Vector store to use
        /// </param>
        /// <param name="success">
        /// Success message
        /// </param>
        /// <param name="error">
        /// Error message
        /// </param>
        /// <returns>
        /// Success or error message
        /// </returns>
        private async Task<string> UploadFileToAssistant(string fname, string content, IDocumentStoreManager extvs, TimeSpan? expiration, string success, string error)
        {
            fname = Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], fname);
            try
            {
                using (StreamWriter wr = new StreamWriter(fname))
                {
                    wr.WriteLine(content);
                }
                TimeSpan cts = extvs.FileManager.DefaultExpirationTime;
                if ((expiration != null) && expiration.HasValue)
                {
                    extvs.FileManager.DefaultExpirationTime = expiration.Value;
                }
                await extvs.StoreFile(fname, false);
                await Task.Delay(1000);
                extvs.FileManager.DefaultExpirationTime = cts;
#if DEBUG
#else
                try { File.Delete(fname); } catch { }
#endif
                return success;
            }
            catch
            {
                return error;
            }
        }
        /// <summary>
        /// Setup the application if not properly configured
        /// </summary>
        /// <param name="psch">
        /// Default play schema to create players if needed
        /// </param>
        /// <param name="asetup">
        /// Assistants data to create default play if needed
        /// </param>
        /// <returns>
        /// True wehen successful, false if the user cancels the configuration or an error occurs
        /// </returns>
        private bool CheckSetup(PlaySchema psch, out AssistantsSetup asetup)
        {
            try
            {
                bool asked = false;
                asetup = null;
                // Check for credential store
                if (ConfigurationManager.AppSettings[SETTING_CredentialStore] == null)
                {
                    MessageBox.Show(MSG_StartConfiguration, CAP_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    asked = true;
                    CredentialStoreSelector cssel = new CredentialStoreSelector(this);
                    DlgAppConfiguration dlg = new DlgAppConfiguration()
                    {
                        DependencyProvider = this,
                        DataSheet = cssel
                    };
                    CenterDialogBox(dlg);
                    if ((dlg.ShowDialog(this) == DialogResult.Cancel) || (ConfigurationManager.AppSettings[SETTING_CredentialStore] == null))
                    {
                        MessageBox.Show(ERR_AppNotProperlyConfigured, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return false;
                    }
                }
                _credStore = Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings[SETTING_CredentialStore])) as ICredentialStore;
                foreach (ObjectWrapper<IAPIManager> api in ApiManagers)
                {
                    IAPIManager manager = api.TypedImplementation;
                    if ((manager != null) && string.IsNullOrEmpty(manager.AccountId))
                    {
                        if (!asked)
                        {
                            MessageBox.Show(MSG_StartConfiguration, CAP_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            asked = true;
                        }
                        manager.AppAutomation = this;
                        APIKeySetup ak = new APIKeySetup(api.FriendlyName, api.FriendlyDescription, manager, _credStore);
                        DlgAppConfiguration dlg = new DlgAppConfiguration()
                        {
                            DependencyProvider = this,
                            DataSheet = ak
                        };
                        CenterDialogBox(dlg);
                        if ((dlg.ShowDialog(this) == DialogResult.Cancel) || string.IsNullOrEmpty(ak.KeyValue) || string.IsNullOrEmpty(ak.KeyName))
                        {
                            MessageBox.Show(ERR_AppNotProperlyConfigured, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                            return false;
                        }
                        Task.Run(async () => await manager.AvailableLLMModels())
                            .GetAwaiter()
                            .GetResult();
                    }
                }
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[SETTING_dbconstring]) ||
                    !int.TryParse(ConfigurationManager.AppSettings[SETTING_dbconnections], out int ncon))
                {
                    if (!asked)
                    {
                        MessageBox.Show(MSG_StartConfiguration, CAP_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        asked = true;
                    }
                    ConnectionStringSetup cssetup = new ConnectionStringSetup(this, _credStore);
                    DlgAppConfiguration dlg = new DlgAppConfiguration()
                    {
                        DependencyProvider = this,
                        DataSheet = cssetup
                    };
                    CenterDialogBox(dlg);
                    if (dlg.ShowDialog(this) == DialogResult.Cancel)
                    {
                        MessageBox.Show(ERR_AppNotProperlyConfigured, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return false;
                    }
                    string credname = "CS_" + cssetup.ST_dbconstring;
                    _credStore.SaveCredentialToStore(new CredentialStoreKey(credname), new NetworkCredential("admin", cssetup.ConnectionString), out string error);
                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }
                    AppConfigWriter.SetAppSetting(SETTING_dbconstring, cssetup.ST_dbconstring);
                    AppConfigWriter.SetAppSetting(SETTING_dbconnections, cssetup.ST_dbconnections.ToString());
                    AppConfigWriter.SetConnectionString(cssetup.ST_dbconstring, credname, GetCSProviderName(cssetup.DBConnector.UID));
                }
                if ((psch.Players == null) || (psch.Players.Count == 0))
                {
                    if (!asked)
                    {
                        MessageBox.Show(MSG_StartConfiguration, CAP_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        asked = true;
                    }
                    psch.Description = DESC_DefPlay;
                    psch.Name = NAME_DefPlay;
                    asetup = new AssistantsSetup(this);
                    asetup.UserName = psch.User.Name;
                    DlgAppConfiguration dlg = new DlgAppConfiguration()
                    {
                        DependencyProvider = this,
                        DataSheet = asetup
                    };
                    CenterDialogBox(dlg);
                    if (dlg.ShowDialog(this) == DialogResult.Cancel)
                    {
                        MessageBox.Show(ERR_AppNotProperlyConfigured, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                asetup = null;
                MessageBox.Show(string.Join("\n", ERR_AppNotProperlyConfigured, ex.Message), CAP_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return false;
            }
        }
        private async Task CreateAssistantConfiguration(UpdateAssistantsConfiguration upcfg,
            PlayerSetupDataSheet pds,
            PlaySchema psch,
            List<string> services,
            string description,
            string sysmsg,
            bool defaultast = false)
        {
            // Create the player
            IAPIPlayer aplayer = await pds.APIManager.CreateElement(pds.Type.TypedImplementation, "") as IAPIPlayer;
            aplayer.Name = pds.Name;
            aplayer.Model = pds.Model;
            aplayer.Description = description;
            aplayer.BackColor = pds.BackColor;
            aplayer.ForeColor = pds.ForeColor;
            aplayer.AllowedServices = services;
            string instructions = string.Format(sysmsg, aplayer.Name, aplayer.Name.Replace(" ", ""));
            string ifname = string.Format(IFILE_Assistant, aplayer.Name.Replace(" ", ""));
            string nfname = string.Format(NFILE_Assistant, aplayer.Name.Replace(" ", ""));
            File.WriteAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], ifname), instructions);
            aplayer.SystemInstructions = instructions;
            // Create a default vector store for the assistant
            IFilePackageManager pvs = await pds.APIManager.CreateElement(nameof(IFilePackageManager), "") as IFilePackageManager;
            pvs.Name = string.Format(NAME_VectorStore, pds.Name);
            // Store it in the cloud to get an identifier
            await ((IStorableElement)pvs).ModifyStoredElement();
            // Save the player to the cloud to get an identifier
            await ((IStorableElement)aplayer).ModifyStoredElement();
            aplayer.Assets = new List<IPlayerAsset> { pvs };
            // Store the player configuration in the API configuration file
            aplayer.APIManager.UpdateConfiguration(aplayer);
            // Store the vector store configuration in the API configuration file
            aplayer.APIManager.UpdateConfiguration(pvs);
            // Commit changes to disk too
            aplayer.APIManager.UpdateConfiguration();
            // Store the player and the vector store in the play schema
            PlayPlayer pp = _play.GetPlayerSchema(aplayer);
            pp.BackColor = pds.BackColor;
            pp.ForeColor = pds.ForeColor;
            pp.Services = services;
            pp.Default = defaultast;
            pp.Instructions = "file:" + Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], ifname);
            CastElement ce = _play.GetCastElement(pvs);
            pp.Assets = new List<CastElement>() { ce };
            psch.Players.Add(pp);
            _play.Cast.Add(aplayer);
            if (defaultast)
            {
                _play.DefaultPlayer = aplayer;
            }
            // Create update assistant settings
            upcfg.AssistantUpdates.Add(new AssistantUpdates()
            {
                DataPath = upcfg.DataPath,
                Identifier = aplayer.Identifier,
                Name = aplayer.Name,
                Instructions = ifname,
                UpdateInstructions = true,
                Documents = new List<UpdateDocument>()
                            {
                                new UpdateDocument()
                                {
                                    Update = false,
                                    FileName = nfname
                                }
                            }
            });
        }
        /// <summary>
        /// Add a service update to the update configuration
        /// </summary>
        /// <param name="upcfg">
        /// Configuration updates collection object
        /// </param>
        /// <param name="id">
        /// Service identifier
        /// </param>
        /// <param name="docs">
        /// Documents to update
        /// </param>
        private void AddServiceUpdate(UpdateAssistantsConfiguration upcfg, string id, List<UpdateDocument> docs)
        {
            ServiceUpdates srvu = new ServiceUpdates()
            {
                DataPath = upcfg.DataPath,
                ServiceId = id,
                Documents = docs
            };
            if (!upcfg.ServiceUpdates.Contains(srvu))
            {
                upcfg.ServiceUpdates.Add(srvu);
            }
            else if (docs != null)
            {
                ServiceUpdates asrv = upcfg.ServiceUpdates.First(s => s.ServiceId == srvu.ServiceId);
                if (asrv.Documents == null)
                {
                    asrv.Documents = new List<UpdateDocument>();
                }
                foreach (UpdateDocument doc in srvu.Documents)
                {
                    if (!asrv.Documents.Contains(doc))
                    {
                        asrv.Documents.Add(doc);
                    }
                    else
                    {
                        asrv.Documents.First(d => d.FileName == doc.FileName).Update = doc.Update;
                    }
                }
            }
        }
        private async void Main_Load(object sender, EventArgs e)
        {
            try
            {
                // Load API manager list
                _apiManagersCfgs = new APIManagerList()
                {
                    Managers = new List<APIManagerCfg>()
                };
                _apiManagers = new List<ObjectWrapper<IAPIManager>>();
                string cfgpath = ConfigurationManager.AppSettings[SETTING_ConfigPath];
                if (string.IsNullOrEmpty(cfgpath))
                {
                    cfgpath = Path.GetDirectoryName(Application.ExecutablePath);
                }
                foreach (IUIIdentifier apims in GetObjects(nameof(APIManagerCfg)))
                {
                    APIManagerCfg cfg = apims.Implementation() as APIManagerCfg;
                    if (!_apiManagersCfgs.Managers.Contains(cfg))
                    {
                        _apiManagersCfgs.Managers.Add(cfg);
                    }
                    IAPIManager manager = _apiManagersCfgs.GetAPIManager(cfg.Type,
                        cfgpath,
                        ConfigurationManager.AppSettings[SETTING_LogPathBase]);
                    if (manager != null)
                    {
                        manager.AppAutomation = this;
                        if (!string.IsNullOrEmpty(manager.AccountId))
                        {
                            // Load assistant usable models to use them in the configuration
                            _ = await manager.AvailableLLMModels();
                        }
                        _apiManagers.Add(new ObjectWrapper<IAPIManager>(manager, manager.Name, manager.Identifier)
                        {
                            FriendlyDescription = cfg.Description
                        });
                    }
                }
                // Create function calling log file.
                _fcallLog = Path.Combine(ConfigurationManager.AppSettings[SETTING_LogPathBase], ConfigurationManager.AppSettings[SETTING_FunctionCallLog]);
                if (!string.IsNullOrEmpty(_fcallLog) && !Directory.Exists(Path.GetDirectoryName(_fcallLog)))
                {
                    _fcallLog = null;
                }
                // Create token usage log file.
                _usagelog = Path.Combine(ConfigurationManager.AppSettings[SETTING_LogPathBase], ConfigurationManager.AppSettings[SETTING_tokenUsageLog]);
                if (!string.IsNullOrEmpty(_usagelog) && !Directory.Exists(Path.GetDirectoryName(_usagelog)))
                {
                    _usagelog = null;
                }
                // Enable timer to save token usage data in a centeralized way, to avoid multiple threads writing to the same file.
                LogTimer.Enabled = !string.IsNullOrEmpty(_usagelog);
                // Application local document repository. This is a working place to store files that can be later uploaded to an external vector store.
                _documents = new AppDocumentRepository()
                {
                    RepositoryPath = Path.Combine(ConfigurationManager.AppSettings[SETTING_dataPath], FILE_DocumentRepository)
                };
                await _documents.LoadRepository();
                // Initialize services. Services are set of functions that can be called from the assistants.
                await InitializeServices();
                // Load available play list
                _playCollection = LoadConfiguration(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                    ConfigurationManager.AppSettings[SETTING_playCollection]),
                    typeof(PlayCollection)) as PlayCollection;
                AssistantsSetup asetup;
                // Load default play schema
                PlaySchema psch = _playCollection.Plays.FirstOrDefault(p => p.Id == ConfigurationManager.AppSettings[SETTING_defaultPlay]);
                if (!CheckSetup(psch, out asetup))
                {
                    return;
                }
                if (asetup != null)
                {
                    _play = new ConsolePlay()
                    {
                        AppAutomation = this
                    };
                    List<string> errors;
                    using (OverlayScope ovs = new OverlayScope(this, MSG_CreateingAssistants))
                    {
                        // Object to update assistants configuration files
                        UpdateAssistantsConfiguration upcfg = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                            ConfigurationManager.AppSettings[SETTING_updateConfiguration])));
                        upcfg.AppAutomation = this;
                        upcfg.DataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
                        psch.User.Name = asetup.UserName;
                        // Initialize a new play with the basic configuration
                        await _play.LoadFromSchema(psch);
                        psch.Players = new List<PlayPlayer>();
                        psch.AppAutomation = this;
                        // Create the ChessMate (or equivalent) player
                        string p0 = asetup.ChessMate.Name;
                        ovs.Message = string.Format(MSG_CreatingAssistant, p0);
                        await CreateAssistantConfiguration(upcfg, asetup.ChessMate, psch, new List<string> { "automation", "memoryandnotes", "userinterface", "querydatabase" }, DESC_ChessMate, ChessMate_instructions, true);
                        // Create the Caspov (or equivalent) player
                        string p1 = asetup.Caspov.Name;
                        ovs.Message = string.Format(MSG_CreatingAssistant, p1);
                        await CreateAssistantConfiguration(upcfg, asetup.Caspov, psch, new List<string> { "automation", "memoryandnotes", "chessplayer", "querydatabase" }, DESC_Caspov, Caspov_instructions);
                        // Create the MorFix (or equivalent) player
                        string p2 = asetup.MorFix.Name;
                        ovs.Message = string.Format(MSG_CreatingAssistant, p2);
                        await CreateAssistantConfiguration(upcfg, asetup.MorFix, psch, new List<string> { "automation", "memoryandnotes", "configuration" }, DESC_MorFix, MorFix_instructions);
                        psch.Presentation = string.Format(Play_presentation, p0, p1, p2);
                        // Save play schema changes
                        SavePlayConfiguration();
                        // Create service document updates                        
                        AddServiceUpdate(upcfg, "userinterface",
                            new List<UpdateDocument>()
                            {
                                new UpdateDocument()
                                {
                                    Update = true,
                                    FileName = AFILE_Application
                                }
                            });
                        AddServiceUpdate(upcfg, "querydatabase",
                            new List<UpdateDocument>()
                            {
                                new UpdateDocument()
                                {
                                    Update = true,
                                    FileName = AFILE_database
                                }
                            });
                        AddServiceUpdate(upcfg, "automation",
                            new List<UpdateDocument>()
                            {
                                new UpdateDocument()
                                {
                                    Update = true,
                                    FileName = AFILE_Automation
                                }
                            });
                        AddServiceUpdate(upcfg, "chessplayer", null);
                        AddServiceUpdate(upcfg, "memoryandnotes", null);
                        AddServiceUpdate(upcfg, "configuration",
                            new List<UpdateDocument>()
                            {
                                new UpdateDocument()
                                {
                                    Update = true,
                                    FileName = AFILE_Configuration
                                },
                                new UpdateDocument()
                                {
                                    Update = true,
                                    FileName = SFILE_openai_api_config_schema
                                },
                                new UpdateDocument()
                                {
                                    Update = true,
                                    FileName = SFILE_assistant_updates_schema
                                },
                                new UpdateDocument()
                                {
                                    Update = true,
                                    FileName = SFILE_play_config_schema
                                }
                            });
                        // Save update configuration and execute updates
                        SaveConfiguration(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                            ConfigurationManager.AppSettings[SETTING_updateConfiguration]), upcfg);
                        ovs.Message = MSG_UploadingDocuments;
                        errors = await upcfg.PerformUpdates();
                        // Save update configuration again to store changes in document status
                        SaveConfiguration(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                            ConfigurationManager.AppSettings[SETTING_updateConfiguration]), upcfg);
                    }
                    if ((errors != null) && (errors.Count > 0))
                    {
                        errors.Add(ERR_UploadingDocuments);
                        MessageBox.Show(string.Join(Environment.NewLine, errors));
                    }
                }
                _repository = GetRepository(ConfigurationManager.AppSettings[SETTING_dbconstring]);
                Text = TTL_AIChessDB + " (" + _repository.ConnectionName + ")";
                Application.DoEvents();
                if ((psch != null) && (_play == null))
                {
                    _play = new ConsolePlay()
                    {
                        AppAutomation = this
                    };
                    using (OverlayScope ovs = new OverlayScope(this, MSG_LoadingPlay))
                    {
                        await _play.LoadFromSchema(psch);
                        // Clean old working documents
                        ovs.Message = MSG_CleaningDocuments;
                        UpdateAssistantsConfiguration upcfg = JsonSerializer.Deserialize<UpdateAssistantsConfiguration>(File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings[SETTING_ConfigPath],
                            ConfigurationManager.AppSettings[SETTING_updateConfiguration])));
                        upcfg.AppAutomation = this;
                        upcfg.DataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
                        await upcfg.CleanStorage();
                    }
                }
                // Get a console from dependency provider
                IUIIdentifier console = GetObjects(nameof(IHelpConsole), typeof(APIConsole)).FirstOrDefault();
                if (console == null)
                {
                    throw new Exception(ERR_NoConsoleFound);
                }
                _hConsole = console.Implementation() as APIConsole;
                // Get chat bubble controls from dependency provider
                IUIIdentifier interaction = GetObjects(nameof(IPlayerInteraction), typeof(IDesktopPlayerInteraction)).FirstOrDefault();
                if (interaction != null)
                {
                    _hConsole.PlayerInteraction = interaction.Implementation() as IPlayerInteraction;
                }
                // Get user prompt bubble control from dependency provider
                IUIIdentifier userbubble = GetObjects(nameof(IUserPrompt), typeof(IDesktopUserPrompt)).FirstOrDefault();
                if (userbubble != null)
                {
                    _hConsole.UserPromptUI = userbubble.Implementation() as IUserPrompt;
                }
                _hConsole.Dock = DockStyle.Fill;
                _hConsole.AppAutomation = this;
                _hConsole.Options = ConsoleOptions.ConfigurePlayer | ConsoleOptions.FileUpload | ConsoleOptions.ImageUpload | ConsoleOptions.AssistantCommands |
                        ConsoleOptions.Speech | ConsoleOptions.LogUsage | ConsoleOptions.ResetThread | ConsoleOptions.StopThread;
                _hConsole.LogPath = ConfigurationManager.AppSettings[SETTING_LogPathBase];
                _hConsole.Play = _play;
                _hConsole.UpdateAppServices();
                _frmAIConsole = new AIAssistantConsole()
                {
                    StartPosition = FormStartPosition.Manual,
                };
                _frmAIConsole.MdiParent = this;
                _frmAIConsole.Console = _hConsole;
                _frmAIConsole.Show();
                ApplicationAPIElement apiapp = new ApplicationAPIElement(_play.Applications[0]);
                apiapp.APIManager = _apiManagers.First()?.TypedImplementation;
                string ast0 = "";
                string ast1 = "";
                string ast2 = "";
                foreach (IAPIPlayer player in _play.Cast)
                {
                    if (player.AllowedServices != null)
                    {
                        if (player.AllowedServices.Contains("chessplayer") &&
                            string.IsNullOrEmpty(ast2))
                        {
                            ast2 = player.Name;
                        }
                        else if (player.AllowedServices.Contains("configuration") &&
                            string.IsNullOrEmpty(ast1))
                        {
                            ast1 = player.Name;
                        }
                        else if (player.AllowedServices.Contains("userinterface") &&
                            string.IsNullOrEmpty(ast0))
                        {
                            ast0 = player.Name;
                        }
                    }
                }
                Application.DoEvents();
                _hConsole.SetInitialLayout();
                await _hConsole.ApplicationMessageAsync(apiapp, new ContextMessage() { Message = string.Format(MSG_SystemToUser, ast0, ast1, ast2) });
                _frmAIConsole.Height = DisplayRectangle.Height - 4;
                _frmAIConsole.Location = new Point(DisplayRectangle.Right - (_frmAIConsole.Width + 4), 0);
                // Set focus to the console input box
                _hConsole.EnableUserInput = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private WindowID opImportPGN_Click(AIChessObjectRepository repo)
        {
            try
            {
                ofDlg.Filter = FIL_PGN;
                ofDlg.InitialDirectory = Path.GetFullPath(ConfigurationManager.AppSettings[SETTING_PGNdir]);
                if (ofDlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (string.Compare(Path.GetExtension(ofDlg.FileName), ".pgn", true) == 0)
                    {
                        return ImportPGN(ofDlg.FileName, repo);
                    }
                    else
                    {
                        return ImportBMF(ofDlg.FileName, repo);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return null;
        }
        private WindowID opAppConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                // Check whether the configuration dialog is already open
                DlgAppConfiguration dlg = OwnedForms.FirstOrDefault(f =>
                    (f is DlgAppConfiguration dc) &&
                    (dc.DataSheet is ConfigurationSettingsDataSheet)) as DlgAppConfiguration;
                if (dlg == null)
                {
                    dlg = new DlgAppConfiguration();
                    dlg.DependencyProvider = this;
                    CenterDialogBox(dlg);
                    dlg.FormClosed += (_, __) =>
                    {
                        if (!dlg.Cancelled)
                        {
                            string nwcs = ConfigurationManager.AppSettings[SETTING_dbconstring];
                            if (nwcs != _repository.ConnectionName)
                            {
                                _repository = GetRepository(nwcs);
                                Text = TTL_AIChessDB + " (" + _repository.ConnectionName + ")";
                            }
                        }
                    };
                    dlg.Show(this);
                }
                dlg.BringToFront();
                return new WindowID() { UID = string.Join("-", dlg.GetType().Name, dlg.UID), FriendlyName = dlg.FriendlyName };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        private WindowID opCreateConnectionString(string connection_name = null)
        {
            try
            {
                // Check whether the configuration dialog is already open
                DlgAppConfiguration dlg = OwnedForms.FirstOrDefault(f =>
                    (f is DlgAppConfiguration dc) &&
                    (dc.DataSheet is ConnectionStringEdition cs) &&
                    (cs.ST_dbconstring == connection_name)) as DlgAppConfiguration;
                if (dlg == null)
                {
                    ConnectionStringEdition cssetup = new ConnectionStringEdition(this, connection_name);
                    dlg = new DlgAppConfiguration()
                    {
                        DependencyProvider = this,
                        DataSheet = cssetup
                    };
                    CenterDialogBox(dlg);
                    dlg.FormClosed += new FormClosedEventHandler((s, e) =>
                    {
                        dlg = s as DlgAppConfiguration;
                        if (!dlg.Cancelled)
                        {
                            cssetup = dlg.DataSheet as ConnectionStringEdition;
                            string credname = "CS_" + cssetup.ST_dbconstring;
                            _credStore.SaveCredentialToStore(new CredentialStoreKey(credname), new NetworkCredential("admin", cssetup.ConnectionString), out string error);
                            if (!string.IsNullOrEmpty(error))
                            {
                                MessageBox.Show(error);
                            }
                            if (!string.IsNullOrEmpty(connection_name))
                            {
                                if (connection_name != cssetup.ST_dbconstring)
                                {
                                    AppConfigWriter.RemoveConnectionString(connection_name);
                                    if (ConfigurationManager.AppSettings[SETTING_dbconstring] == connection_name)
                                    {
                                        AppConfigWriter.SetAppSetting(SETTING_dbconstring, cssetup.ST_dbconstring);
                                    }
                                }
                            }
                            AppConfigWriter.SetConnectionString(cssetup.ST_dbconstring, credname, GetCSProviderName(cssetup.DBConnector.UID));
                        }
                    });
                    dlg.Show(this);
                }
                dlg.BringToFront();
                return new WindowID() { UID = string.Join("-", dlg.GetType().Name, dlg.UID), FriendlyName = dlg.FriendlyName };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private WindowID opEditPlay_Click(object sender, EventArgs e)
        {
            try
            {
                PlayEditor wp = MdiChildren.FirstOrDefault(f => f is PlayEditor) as PlayEditor;
                if (wp == null)
                {
                    wp = new PlayEditor();
                    CenterMdiChild(wp);
                    wp.automationManager = this;
                    wp.DependencyProvider = this;
                    wp.Play = _playCollection.Plays.FirstOrDefault(p => p.Id == ConfigurationManager.AppSettings[SETTING_defaultPlay]);
                    wp.Show();
                }
                wp.BringToFront();
                return new WindowID() { UID = string.Join("-", wp.GetType().Name, wp.UID), FriendlyName = wp.FriendlyName };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return null;
        }
        private WindowID opEditAssistants_Click(object sender, EventArgs e)
        {
            try
            {
                AssistantsEditor wae = MdiChildren.FirstOrDefault(f => f is AssistantsEditor) as AssistantsEditor;
                if (wae == null)
                {
                    wae = new AssistantsEditor();
                    CenterMdiChild(wae);
                    wae.automationManager = this;
                    wae.DependencyProvider = this;
                    wae.Show();
                }
                wae.BringToFront();
                return new WindowID() { UID = string.Join("-", wae.GetType().Name, wae.UID), FriendlyName = wae.FriendlyName };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return null;
        }
        private WindowID opEditUpdates_Click(object sender, EventArgs e)
        {
            try
            {
                AssistantsUpdates wae = MdiChildren.FirstOrDefault(f => f is AssistantsUpdates) as AssistantsUpdates;
                if (wae == null)
                {
                    wae = new AssistantsUpdates();
                    CenterMdiChild(wae);
                    wae.automationManager = this;
                    wae.DependencyProvider = this;
                    wae.Show();
                }
                wae.BringToFront();
                return new WindowID() { UID = string.Join("-", wae.GetType().Name, wae.UID), FriendlyName = wae.FriendlyName };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return null;
        }
        private WindowID opNewQuery_Click(IObjectRepository repo)
        {
            try
            {
                QueryWindow wq = new QueryWindow();
                CenterMdiChild(wq);
                wq.Provider = this;
                wq.Repository = repo ?? _repository;
                wq.Show();
                wq.BringToFront();
                return new WindowID()
                {
                    UID = string.Join("-", wq.GetType().Name, wq.UID),
                    FriendlyName = wq.FriendlyName,
                    ConnectionName = wq.Repository.ConnectionName,
                    ServerType = wq.Repository.ServerName
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return null;
        }
        private void optExport_Click(IObjectRepository repo)
        {
            try
            {
                DlgBulkCopyDB dlg = new DlgBulkCopyDB()
                {
                    Provider = this,
                    Repository = repo ?? _repository
                };
                CenterDialogBox(dlg);
                dlg.Show(this);
                dlg.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private WindowID opNewMatch_Click(IObjectRepository repo)
        {
            try
            {
                PlayMatchWindow wpm = new PlayMatchWindow();
                CenterMdiChild(wpm);
                wpm.Provider = this;
                wpm.Repository = repo ?? _repository;
                wpm.CurrentMatch = null;
                wpm.Show();
                wpm.BringToFront();
                return new WindowID()
                {
                    UID = string.Join("-", wpm.GetType().Name, wpm.UID),
                    FriendlyName = wpm.FriendlyName,
                    ConnectionName = wpm.Repository.ConnectionName,
                    ServerType = wpm.Repository.ServerName
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                else
                    MessageBox.Show(ex.Message);
            }
            return null;
        }

        private WindowID opImportMultiPGN_Click(IObjectRepository repo)
        {
            try
            {
                DlgBulkImportPGN dlg = new DlgBulkImportPGN();
                dlg.Repository = repo ?? _repository;
                CenterDialogBox(dlg);
                dlg.Show(this);
                dlg.BringToFront();
                return new WindowID()
                {
                    UID = string.Join("-", dlg.GetType().Name, dlg.UID),
                    FriendlyName = dlg.FriendlyName,
                    ConnectionName = dlg.Repository.ConnectionName,
                    ServerType = dlg.Repository.ServerName
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        private void optConsolidate_Click(WindowID wid, IObjectRepository repo)
        {
            try
            {
                DlgKeywordConsolidation dlg = new DlgKeywordConsolidation();
                wid.UID = string.Join("-", dlg.GetType().Name, dlg.UID);
                wid.FriendlyName = dlg.FriendlyName;
                dlg.Repository = repo ?? _repository;
                wid.ConnectionName = dlg.Repository.ConnectionName;
                wid.ServerType = dlg.Repository.ServerName;
                CenterDialogBox(dlg);
                dlg.Show(this);
                dlg.BringToFront();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                else
                    MessageBox.Show(ex.Message);
            }
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_logQueue.Count > 0)
                {
                    LogRecord rec;
                    while (_logQueue.TryDequeue(out rec))
                    {
                        using (StreamWriter wrt = new StreamWriter(rec.Path, true))
                        {
                            wrt.WriteLine(string.Join(";", rec.Data));
                        }
                    }
                }
            }
            catch { }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_frmAIConsole != null)
            {
                _frmAIConsole.AllowClose = true;
            }
            LogTimer_Tick(sender, e);
        }

        private void Main_Shown(object sender, EventArgs e)
        {
        }
    }
}

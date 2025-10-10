using AIAssistants.Interfaces;
using AIAssistants.JSON;
using AIChessDatabase.Properties;
using GlobalCommonEntities.Config;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Setup
{
    /// <summary>
    /// Class to hold configuration settings for the application.
    /// </summary>
    public class ConfigurationSettingsDataSheet : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private Dictionary<string, string> _changes = new Dictionary<string, string>();
        private string _ST_dbconstring;
        private int _ST_dbconnections;
        private string _ST_dataPath;
        private string _ST_playCollection;
        private string _ST_ConfigPath;
        private string _ST_PGNPath;
        private string _ST_appFunctions;
        private string _ST_nsQueries;
        private string _ST_updateConfiguration;
        private string _ST_defaultPlay;
        private string _ST_LogPathBase;
        private string _ST_FunctionCallLog;
        private string _ST_tokenUsageLog;
        private ObjectWrapper _ST_CredentialStore;
        private string _ST_CredentialStoreType;
        private UIDataSheet _ST_CredentialStoreKey;
        private string _ST_CredentialStoreKeyData;
        private List<IUIIdentifier> _credmgrs;

        public ConfigurationSettingsDataSheet(IDependencyProvider provider)
        {
            AppAutomation = provider as IAppAutomation;
            _ST_dbconstring = ConfigurationManager.AppSettings[SETTING_dbconstring];
            _ST_dbconnections = int.Parse(ConfigurationManager.AppSettings[SETTING_dbconnections]);
            _ST_dataPath = ConfigurationManager.AppSettings[SETTING_dataPath];
            _ST_ConfigPath = ConfigurationManager.AppSettings[SETTING_ConfigPath];
            _ST_PGNPath = ConfigurationManager.AppSettings[SETTING_PGNdir];
            _ST_playCollection = ConfigurationManager.AppSettings[SETTING_playCollection];
            _ST_appFunctions = ConfigurationManager.AppSettings[SETTING_appFunctions];
            _ST_updateConfiguration = ConfigurationManager.AppSettings[SETTING_updateConfiguration];
            _ST_nsQueries = ConfigurationManager.AppSettings[SETTING_nsQueries];
            _ST_LogPathBase = ConfigurationManager.AppSettings[SETTING_LogPathBase];
            _ST_FunctionCallLog = ConfigurationManager.AppSettings[SETTING_FunctionCallLog];
            _ST_tokenUsageLog = ConfigurationManager.AppSettings[SETTING_tokenUsageLog];
            _credmgrs = new List<IUIIdentifier>(provider.GetObjects(nameof(ICredentialStore)));
            _ST_CredentialStoreType = ConfigurationManager.AppSettings[SETTING_CredentialStore];
            foreach (IUIIdentifier mgr in _credmgrs)
            {
                if (mgr.Implementation().GetType() == Type.GetType(_ST_CredentialStoreType))
                {
                    _ST_CredentialStore = new ObjectWrapper(mgr.Implementation(), mgr.FriendlyName, "");
                    break;
                }
            }
            _ST_defaultPlay = ConfigurationManager.AppSettings[SETTING_defaultPlay];
            _ST_CredentialStoreKeyData = ConfigurationManager.AppSettings[SETTING_CredentialStoreKeyData];
            PropertyChanged += OnPropertyChanged;
        }
        [JsonIgnore]
        [Browsable(false)]
        public IAppAutomation AppAutomation { get; set; }
        [JsonIgnore]
        [Browsable(false)]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                if (_info == null)
                {
                    _info = new List<PropertyEditorInfo>();
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_DBConnection });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(ST_dbconstring), InitialValue = _ST_dbconstring });
                    _info[_info.Count - 1].Values = new List<object>();
                    foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
                    {
                        _info[_info.Count - 1].Values.Add(cs.Name);
                    }
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.IntValue, PropertyName = nameof(ST_dbconnections), InitialValue = _ST_dbconnections, Values = new List<object> { 1, 30 } });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_Paths });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.Directory, PropertyName = nameof(ST_dataPath), InitialValue = _ST_dataPath });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.Directory, PropertyName = nameof(ST_ConfigPath), InitialValue = _ST_ConfigPath });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.Directory, PropertyName = nameof(ST_PGNPath), InitialValue = _ST_PGNPath });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.Directory, PropertyName = nameof(ST_LogPathBase), InitialValue = _ST_LogPathBase });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_Files });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(ST_appFunctions), InitialValue = _ST_appFunctions, Values = new List<object> { FILTER_Json, FILTER_AllFiles } });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(ST_nsQueries), InitialValue = _ST_nsQueries, Values = new List<object> { FILTER_Json, FILTER_AllFiles } });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(ST_playCollection), InitialValue = _ST_playCollection, Values = new List<object> { FILTER_Json, FILTER_AllFiles } });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(ST_updateConfiguration), InitialValue = _ST_updateConfiguration, Values = new List<object> { FILTER_Json, FILTER_AllFiles } });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(ST_FunctionCallLog), InitialValue = _ST_FunctionCallLog, Values = new List<object> { FILTER_LogFiles, FILTER_AllFiles } });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FileName, PropertyName = nameof(ST_tokenUsageLog), InitialValue = _ST_tokenUsageLog, Values = new List<object> { FILTER_LogFiles, FILTER_AllFiles } });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_DefValues });
                    PropertyEditorInfo pi = new PropertyEditorInfo() { EditorType = AppAutomation != null ? InputEditorType.FixedComboBox : InputEditorType.SingleLineText, PropertyName = nameof(ST_defaultPlay), InitialValue = _ST_defaultPlay };
                    if (AppAutomation != null)
                    {
                        pi.Values = new List<object>();
                        foreach (PlaySchema play in AppAutomation.Plays.Plays)
                        {
                            pi.Values.Add(play.Id);
                        }
                    }
                    _info.Add(pi);
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_CredentialMgr });
                    _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(ST_CredentialStoreType), InitialValue = _ST_CredentialStore, Values = new List<object>(_credmgrs) });
                    if (_ST_CredentialStore != null)
                    {
                        _ST_CredentialStoreKey = ((ICredentialStore)_ST_CredentialStore.Implementation()).EmptyKey;
                        if (_ST_CredentialStoreKey.Properties != null)
                        {
                            ((CredentialStoreKey)_ST_CredentialStoreKey).SetData(_ST_CredentialStoreKeyData);
                            _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.DataSheet, PropertyName = nameof(ST_CredentialStoreKey) });
                        }
                    }
                }
                return _info;
            }
            set
            {
                _info = value;
            }
        }
        /// <summary>
        /// Connection string for the database.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_dbconstring), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_dbconstring), typeof(UIResources))]
        public string ST_dbconstring
        {
            get
            {
                return _ST_dbconstring;
            }
            set
            {
                if (_ST_dbconstring != value)
                {
                    _ST_dbconstring = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Number of concurrent database connections to open.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_dbconnections), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_dbconnections), typeof(UIResources))]
        public int ST_dbconnections
        {
            get
            {
                return _ST_dbconnections;
            }
            set
            {
                if (_ST_dbconnections != value)
                {
                    _ST_dbconnections = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Directory path where the application data is stored.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_dataPath), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_dataPath), typeof(UIResources))]
        public string ST_dataPath
        {
            get
            {
                return _ST_dataPath;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    string oldpath = _ST_dataPath;
                    _ST_dataPath = RelativePath(value);
                    if (string.Compare(oldpath, _ST_dataPath, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        if (MessageBox.Show(MSG_MoveFiles, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            MoveFiles(oldpath, _ST_dataPath);
                        }
                        InvokePropertyChanged();
                    }
                }
            }
        }
        /// <summary>
        /// Directory path where the application configuration files are stored.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_ConfigPath), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_ConfigPath), typeof(UIResources))]
        public string ST_ConfigPath
        {
            get
            {
                return _ST_ConfigPath;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    string oldpath = _ST_ConfigPath;
                    _ST_ConfigPath = RelativePath(value);
                    if (string.Compare(oldpath, _ST_ConfigPath, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        if (MessageBox.Show(MSG_MoveFiles, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            MoveFiles(oldpath, _ST_ConfigPath);
                        }
                        InvokePropertyChanged();
                    }
                }
            }
        }
        /// <summary>
        /// Directory path where the application configuration files are stored.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_PGNPath), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_PGNPath), typeof(UIResources))]
        public string ST_PGNPath
        {
            get
            {
                return _ST_PGNPath;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    string oldpath = _ST_PGNPath;
                    _ST_PGNPath = RelativePath(value);
                    if (string.Compare(oldpath, _ST_PGNPath, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        if (MessageBox.Show(MSG_MoveFiles, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            MoveFiles(oldpath, _ST_PGNPath);
                        }
                        InvokePropertyChanged();
                    }
                }
            }
        }
        /// <summary>
        /// Log path base for the application.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_LogPathBase), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_LogPathBase), typeof(UIResources))]
        public string ST_LogPathBase
        {
            get
            {
                return _ST_LogPathBase;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    string oldpath = _ST_LogPathBase;
                    _ST_LogPathBase = RelativePath(value);
                    if (string.Compare(oldpath, _ST_LogPathBase, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        if (MessageBox.Show(MSG_MoveFiles, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            MoveFiles(oldpath, _ST_LogPathBase);
                        }
                        InvokePropertyChanged();
                    }
                }
            }
        }
        /// <summary>
        /// Play collection file name.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_playCollection), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_playCollection), typeof(UIResources))]
        public string ST_playCollection
        {
            get
            {
                return _ST_playCollection;
            }
            set
            {
                if (File.Exists(value))
                {
                    if (string.Compare(RelativePath(Path.GetDirectoryName(value)), _ST_ConfigPath, true) != 0)
                    {
                        if (MessageBox.Show(MSG_CopyFile, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            File.Copy(value, Path.Combine(_ST_ConfigPath, Path.GetFileName(value)), true);
                        }
                    }
                    _ST_playCollection = Path.GetFileName(value);
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Update configuration file name.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_updateConfiguration), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_updateConfiguration), typeof(UIResources))]
        public string ST_updateConfiguration
        {
            get
            {
                return _ST_updateConfiguration;
            }
            set
            {
                if (File.Exists(value))
                {
                    if (string.Compare(RelativePath(Path.GetDirectoryName(value)), _ST_ConfigPath, true) != 0)
                    {
                        if (MessageBox.Show(MSG_CopyFile, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            File.Copy(value, Path.Combine(_ST_ConfigPath, Path.GetFileName(value)), true);
                        }
                    }
                    _ST_updateConfiguration = Path.GetFileName(value);
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Assistants functions file name.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_appFunctions), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_appFunctions), typeof(UIResources))]
        public string ST_appFunctions
        {
            get
            {
                return _ST_appFunctions;
            }
            set
            {
                if (File.Exists(value))
                {
                    if (string.Compare(RelativePath(Path.GetDirectoryName(value)), _ST_ConfigPath, true) != 0)
                    {
                        if (MessageBox.Show(MSG_CopyFile, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            File.Copy(value, Path.Combine(_ST_ConfigPath, Path.GetFileName(value)), true);
                        }
                    }
                    _ST_appFunctions = Path.GetFileName(value);
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Non-standard queries file name.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_nsQueries), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_nsQueries), typeof(UIResources))]
        public string ST_nsQueries
        {
            get
            {
                return _ST_nsQueries;
            }
            set
            {
                if (File.Exists(value))
                {
                    if (string.Compare(RelativePath(Path.GetDirectoryName(value)), _ST_ConfigPath, true) != 0)
                    {
                        if (MessageBox.Show(MSG_CopyFile, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            File.Copy(value, Path.Combine(_ST_ConfigPath, Path.GetFileName(value)), true);
                        }
                    }
                    _ST_nsQueries = Path.GetFileName(value);
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Default play to use when no play is selected.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_defaultPlay), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_defaultPlay), typeof(UIResources))]
        public string ST_defaultPlay
        {
            get
            {
                return _ST_defaultPlay;
            }
            set
            {
                if (_ST_defaultPlay != value)
                {
                    _ST_defaultPlay = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// FUnction call log file name.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_FunctionCallLog), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_FunctionCallLog), typeof(UIResources))]
        public string ST_FunctionCallLog
        {
            get
            {
                return _ST_FunctionCallLog;
            }
            set
            {
                if (File.Exists(value))
                {
                    if (string.Compare(RelativePath(Path.GetDirectoryName(value)), _ST_LogPathBase, true) != 0)
                    {
                        if (MessageBox.Show(MSG_CopyFile, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            File.Copy(value, Path.Combine(_ST_LogPathBase, Path.GetFileName(value)), true);
                        }
                    }
                    _ST_FunctionCallLog = Path.GetFileName(value);
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Token usage log file name.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_tokenUsageLog), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_tokenUsageLog), typeof(UIResources))]
        public string ST_tokenUsageLog
        {
            get
            {
                return _ST_tokenUsageLog;
            }
            set
            {
                if (File.Exists(value))
                {
                    if (string.Compare(RelativePath(Path.GetDirectoryName(value)), _ST_LogPathBase, true) != 0)
                    {
                        if (MessageBox.Show(MSG_CopyFile, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            File.Copy(value, Path.Combine(_ST_LogPathBase, Path.GetFileName(value)), true);
                        }
                    }
                    _ST_tokenUsageLog = Path.GetFileName(value);
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Credential store to use for storing and retrieving credentials.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_CredentialStore), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_CredentialStore), typeof(UIResources))]
        public ObjectWrapper ST_CredentialStoreType
        {
            get
            {
                return _ST_CredentialStore;
            }
            set
            {
                if (_ST_CredentialStore != value)
                {
                    _ST_CredentialStore = value;
                    if (value != null)
                    {
                        ICredentialStore store = value.Implementation() as ICredentialStore;
                        ST_CredentialStore = store.GetType().AssemblyQualifiedName;
                        _ST_CredentialStoreKey = store.EmptyKey;
                        if (_ST_CredentialStoreKey.Properties != null)
                        {
                            ((CredentialStoreKey)_ST_CredentialStoreKey).SetData(_ST_CredentialStoreKeyData);
                            if (Properties[Properties.Count - 1].PropertyName != nameof(ST_CredentialStoreKey))
                            {
                                _info.Add(new PropertyEditorInfo() { EditorType = InputEditorType.DataSheet, PropertyName = nameof(ST_CredentialStoreKey) });
                                InvokeRefreshEditor(new RefreshEditorEventArgs(Properties[Properties.Count - 1],
                                    Properties.Count - 1,
                                    EditorContainerOperation.Add));
                            }
                        }
                        else
                        {
                            if (Properties[Properties.Count - 1].PropertyName == nameof(ST_CredentialStoreKey))
                            {
                                InvokeRefreshEditor(new RefreshEditorEventArgs(Properties[Properties.Count - 1],
                                    Properties.Count - 1,
                                    EditorContainerOperation.Remove));
                                Properties.RemoveAt(Properties.Count - 1);
                            }
                        }
                    }
                    else
                    {
                        ST_CredentialStore = string.Empty;
                        if (Properties[Properties.Count - 1].PropertyName == nameof(ST_CredentialStoreKey))
                        {
                            InvokeRefreshEditor(new RefreshEditorEventArgs(Properties[Properties.Count - 1],
                                Properties.Count - 1,
                                EditorContainerOperation.Remove));
                            Properties.RemoveAt(Properties.Count - 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Credential store key extra data.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_CredentialStoreKeyData), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_CredentialStoreKeyData), typeof(UIResources))]
        public UIDataSheet ST_CredentialStoreKey
        {
            get
            {
                return _ST_CredentialStoreKey;
            }
            set
            {
                if (_ST_CredentialStoreKey != value)
                {
                    _ST_CredentialStoreKey = value;
                    if (value != null)
                    {
                        _ST_CredentialStoreKeyData = ((CredentialStoreKey)value).ToString();
                    }
                    else
                    {
                        _ST_CredentialStoreKeyData = string.Empty;
                    }
                }
            }
        }
        [Browsable(false)]
        public string ST_CredentialStoreKeyData
        {
            get
            {
                return _ST_CredentialStoreKeyData;
            }
            set
            {
                if (_ST_CredentialStoreKeyData != value)
                {
                    _ST_CredentialStoreKeyData = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Credential store type as string
        /// </summary>
        public string ST_CredentialStore
        {
            get
            {
                return _ST_CredentialStoreType;
            }
            set
            {
                if (_ST_CredentialStoreType != value)
                {
                    _ST_CredentialStoreType = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Apply canges to the application configuration.
        /// </summary>
        public override void ApplyChanges()
        {
            foreach (string key in _changes.Keys)
            {
                AppConfigWriter.SetAppSetting(key, _changes[key]);
            }
        }
        /// <summary>
        /// Convert a path to a relative path based on the executable path.
        /// </summary>
        /// <param name="path">
        /// Full path to convert to relative path.
        /// </param>
        /// <returns>
        /// Original path without the data path prefix if it starts with it, otherwise the original path.
        /// </returns>
        private string RelativePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            string exepath = Path.GetDirectoryName(Application.ExecutablePath);
            if (path.StartsWith(exepath, StringComparison.OrdinalIgnoreCase))
            {
                return path.Substring(exepath.Length).TrimStart('\\', '/');
            }
            return path;
        }
        /// <summary>
        /// Move files from the old path to the new path.
        /// </summary>
        /// <param name="oldPath">
        /// Old path where the files are located.
        /// </param>
        /// <param name="newPath">
        /// New path where the files should be moved to.
        /// </param>
        private void MoveFiles(string oldPath, string newPath)
        {
            try
            {
                foreach (string file in Directory.GetFiles(oldPath))
                {
                    string destFile = Path.Combine(newPath, Path.GetFileName(file));
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                    File.Move(file, destFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                PropertyInfo pi = GetType().GetProperty(e.PropertyName);
                object value = pi?.GetValue(this, null);
                string sval = value?.ToString();
                _changes[e.PropertyName.Substring(3)] = sval ?? string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using AIChessDatabase.Properties;
using BaseClassesAndInterfaces.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text.Json.Serialization;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Setup
{
    /// <summary>
    /// Setup data sheet to define a database connection string
    /// </summary>
    public class ConnectionStringSetup : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private List<ObjectWrapper<ISQLDatabaseConnector>> _connectors = new List<ObjectWrapper<ISQLDatabaseConnector>>();
        private ICredentialStore _cstore;
        private string _ST_dbconstring;
        private int _ST_dbconnections;
        private string _connectionString;
        private ObjectWrapper<ISQLDatabaseConnector> _connector;

        public ConnectionStringSetup(IDependencyProvider provider, ICredentialStore cstore)
        {
            _cstore = cstore;
            _ST_dbconstring = ConfigurationManager.AppSettings[SETTING_dbconstring];
            if (!int.TryParse(ConfigurationManager.AppSettings[SETTING_dbconnections], out _ST_dbconnections))
            {
                _ST_dbconnections = 10;
            }
            foreach (IUIIdentifier ui in provider.GetObjects(nameof(IDatabaseDependencyProvider)))
            {
                IDatabaseDependencyProvider dbprovider = ui.Implementation() as IDatabaseDependencyProvider;
                foreach (IUIIdentifier uic in dbprovider.GetObjects(nameof(ISQLDatabaseConnector)))
                {
                    ISQLDatabaseConnector conn = uic.Implementation() as ISQLDatabaseConnector;
                    _connectors.Add(new ObjectWrapper<ISQLDatabaseConnector>(conn, uic.FriendlyName, dbprovider.RDBMSName));
                }
            }
        }
        /// <summary>
        /// Data sheet properties
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public override List<PropertyEditorInfo> Properties
        {
            get
            {
                if (_info == null)
                {
                    _info = new List<PropertyEditorInfo>()
                    {
                        new PropertyEditorInfo() { EditorType = InputEditorType.BlockTitle, PropertyName = BTTL_DBConnection },
                        new PropertyEditorInfo() { EditorType = InputEditorType.FixedComboBox, PropertyName = nameof(DBConnector), Values = new List<object>(_connectors), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.SingleLineText, PropertyName = nameof(ST_dbconstring), Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.IntValue, PropertyName = nameof(ST_dbconnections), Values = new List<object> { 1, 30 }, Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.ConnectionString, PropertyName = nameof(ConnectionString), Service = null, Required = true },
                        new PropertyEditorInfo() { EditorType = InputEditorType.MultilineText, PropertyName = nameof(Instructions), ReadOnly = true, MaxUnits = -1 }
                    };
                }
                return _info;
            }
            set
            {
                _info = value;
            }
        }
        /// <summary>
        /// Database connector to configure
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_dbconnector), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_dbconnector), typeof(UIResources))]
        public ObjectWrapper<ISQLDatabaseConnector> DBConnector
        {
            get
            {
                return _connector;
            }
            set
            {
                if (_connector != value)
                {
                    _connector = value;
                    Properties[4].Service = _connector?.TypedImplementation;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Connection string for the database.
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_dbconstringname), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_dbconstringname), typeof(UIResources))]
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
        /// Connection string built from the data provided
        /// </summary>
        [DILocalizedDisplayName(nameof(NAME_dbconstring), typeof(UIResources))]
        [DILocalizedDescription(nameof(DESC_connstring), typeof(UIResources))]
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    InvokePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Setup instructions
        /// </summary>
        [DILocalizedDisplayName(nameof(CAP_Instructions), typeof(UIResources))]
        public string Instructions { get { return SETUP_ConnectionString; } set { } }
    }
}

using AIChessDatabase.Properties;
using BaseClassesAndInterfaces.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Setup
{
    /// <summary>
    /// Setup data sheet to define a database connection string
    /// </summary>
    public class ConnectionStringEdition : UIDataSheet
    {
        private List<PropertyEditorInfo> _info;
        private List<ObjectWrapper<ISQLDatabaseConnector>> _connectors = new List<ObjectWrapper<ISQLDatabaseConnector>>();
        private string _ST_dbconstring;
        private string _connectionString;
        private ObjectWrapper<ISQLDatabaseConnector> _connector;

        public ConnectionStringEdition(IDependencyProvider provider, string connection_name = null)
        {
            string currentdbconnector = "";
            if (!string.IsNullOrEmpty(connection_name))
            {
                _ST_dbconstring = connection_name;
                currentdbconnector = ConfigurationManager.ConnectionStrings[connection_name].ProviderName;
            }
            foreach (IUIIdentifier ui in provider.GetObjects(nameof(IDatabaseDependencyProvider)))
            {
                IDatabaseDependencyProvider dbprovider = ui.Implementation() as IDatabaseDependencyProvider;
                foreach (IUIIdentifier uic in dbprovider.GetObjects(nameof(ISQLDatabaseConnector)))
                {
                    ISQLDatabaseConnector conn = uic.Implementation() as ISQLDatabaseConnector;
                    _connectors.Add(new ObjectWrapper<ISQLDatabaseConnector>(conn, uic.FriendlyName, dbprovider.RDBMSName));
                    if (dbprovider.RDBMSName == currentdbconnector)
                    {
                        _connector = new ObjectWrapper<ISQLDatabaseConnector>(conn, uic.FriendlyName, dbprovider.RDBMSName);
                    }
                }
            }
        }
        /// <summary>
        /// Data sheet properties
        /// </summary>
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
                        new PropertyEditorInfo() { EditorType = InputEditorType.ConnectionString, PropertyName = nameof(ConnectionString), Service = DBConnector?.TypedImplementation, Required = true }
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
                    Properties[3].Service = _connector?.TypedImplementation;
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
    }
}

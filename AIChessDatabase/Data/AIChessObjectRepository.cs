using AIChessDatabase.Interfaces;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Default database repository implementation.
    /// </summary>
    public class AIChessObjectRepository : IObjectRepository
    {
        private ConcurrentQueue<int> _availableConnections = new ConcurrentQueue<int>();
        private ISQLDatabaseConnector _connector = null;
        private int _maxConnections = 0;
        private Dictionary<string, ISQLUIQuery> _typeQueries = new Dictionary<string, ISQLUIQuery>();
        private string _parameterPrefix = null;
        public AIChessObjectRepository(IDatabaseDependencyProvider provider)
        {
            Provider = provider;
            ServerName = provider.RDBMSName;
        }
        /// <summary>
        /// IGenericObjectRepository: Checks if the repository can handle the specified type.
        /// </summary>
        /// <param name="t">
        /// Type to check.
        /// </param>
        /// <returns>
        /// True if the repository can handle the specified type, false otherwise.
        /// </returns>
        public bool CanHandle(Type t) => t == typeof(ObjectBase);
        /// <summary>
        /// IGenericObjectRepository: Safely tries to cast this repository to a typed repository.
        /// </summary>
        /// <typeparam name="T">
        /// Needed object type.
        /// </typeparam>
        /// <param name="typed">
        /// Casted repository or null if the cast failed.
        /// </param>
        /// <returns>
        /// True if the cast was successful, false otherwise.
        /// </returns>
        public bool TryAs<TWanted>(out IGenericObjectRepository<TWanted> typed)
        {
            if (this is IGenericObjectRepository<TWanted> ok)
            {
                typed = ok;
                return true;
            }
            typed = null;
            return false;
        }
        /// <summary>
        /// IGenericObjectRepository: Database connector for executing queries.
        /// </summary>
        public ISQLDatabaseConnector Connector
        {
            get
            {
                return _connector;
            }
        }
        /// <summary>
        /// IGenericObjectRepository: Database services provider.
        /// </summary>
        public IDatabaseDependencyProvider Provider { get; }
        /// <summary>
        /// IGenericObjectRepository: Database connection name
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// IGenericObjectRepository: Server identifier name.
        /// </summary>
        public string ServerName { get; }
        /// <summary>
        /// IGenericObjectRepository: List of non-standard queries, database dependant, for this repository.
        /// </summary>
        public NonStandardQueries Queries { get; set; } = new NonStandardQueries();
        /// <summary>
        /// IGenericObjectRepository: Database connection string.
        /// </summary>
        public string DatabaseConnectionString
        {
            get
            {
                return _connector?.ConnectionString;
            }
            set
            {
                _connector = Provider.GetObjects(nameof(ISQLDatabaseConnector))
                    .FirstOrDefault()?.Implementation() as ISQLDatabaseConnector;
                _connector.ConnectionString = value;
            }
        }
        /// <summary>
        /// IGenericObjectRepository: Maximum number of simultaneous connections to the database.
        /// </summary>
        public int MaxConnections
        {
            get
            {
                return _maxConnections;
            }
            set
            {
                if (_connector == null)
                {
                    throw new InvalidOperationException(ERR_NOCONNECTOR);
                }
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(value.ToString(), ERR_MAXCONNEGATIVE);
                }
                if (_availableConnections.Count != _maxConnections)
                {
                    throw new InvalidOperationException(ERR_CONNECTIONSINUSE);
                }
                _maxConnections = value;
                _availableConnections = new ConcurrentQueue<int>();
                // Connection 0 is reserved to the system.
                for (int i = 1; i <= _maxConnections; i++)
                {
                    _availableConnections.Enqueue(i);
                }
                _connector.NewConnection(_maxConnections + 1);
            }
        }
        /// <summary>
        /// IGenericObjectRepository: Prefix to use for parameters in SQL queries.
        /// </summary>
        public string ParameterPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_parameterPrefix) && (Provider != null))
                {
                    ISQLParser parser = Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
                    _parameterPrefix = parser.ParameterPrefix;
                }
                return _parameterPrefix;
            }
        }
        /// <summary>
        /// IGenericObjectRepository: Provides the index of a free connection to the database.
        /// </summary>
        /// <returns>
        /// Free connection index.
        /// </returns>
        public int GetFreeConnection()
        {
            if (_availableConnections.TryDequeue(out int connection))
            {
                return connection;
            }
            throw new InvalidOperationException(ERR_NOFREECONNECTIONS);
        }
        /// <summary>
        /// IGenericObjectRepository: Releases the specified index of the connection to the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to release.
        /// </param>
        public void ReleaseConnection(int connection)
        {
            if ((connection > 0) &&
                (connection <= _maxConnections) &&
                !_availableConnections.Contains(connection))
            {
                _availableConnections.Enqueue(connection);
            }
        }
        /// <summary>
        /// IGenericObjectRepository: Query to apply to allobjects of a given type
        /// </summary>
        /// <param name="idtype">
        /// Type identifier to set the query for.
        /// </param>
        /// <param name="query">
        /// SQL code to use as query for the specified type.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for build the query.
        /// </param>
        /// <returns>
        /// ISQLUIQuery instance for all objects.
        /// </returns>
        public ISQLUIQuery SetQueryForType(string idtype, string query, int connection)
        {
            string tokenset = "";
            string token = "";
            ISQLParser parser = Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.Connector = Connector;
            parser.ConnectionIndex = connection;
            ISQLUIQuery uiquery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            foreach (Table bdtabla in uiquery.Tables)
            {
                if (bdtabla != null)
                {
                    List<string> fields = bdtabla.Columns.Select(f => f.Name).ToList();
                    List<string> dml = null;
                    dml = bdtabla.DefaultDMLSentence(UpdateMode.Insert, false, fields);
                    ((IUpdateManager)uiquery).ParseUpdateCommand(bdtabla.FullName, dml, UpdateMode.Insert);
                    dml = bdtabla.DefaultDMLSentence(UpdateMode.Update, false, fields);
                    ((IUpdateManager)uiquery).ParseUpdateCommand(bdtabla.FullName, dml, UpdateMode.Update);
                    dml = bdtabla.DefaultDMLSentence(UpdateMode.Delete, false);
                    ((IUpdateManager)uiquery).ParseUpdateCommand(bdtabla.FullName, dml, UpdateMode.Delete);
                }
            }
            string sql = uiquery.SQL;
            _typeQueries[idtype] = uiquery;
            return uiquery;
        }
        /// <summary>
        /// IGenericObjectRepository: Return the query to apply to all objects of a given type.
        /// </summary>
        /// <param name="idtype">
        /// Type identifier to get the query for.
        /// </param>
        /// <param name="clone">
        /// Return a clone of the query (true) or the original instance (false).
        /// </param>
        /// <returns>
        /// ISQLUIQuery instance for all objects.
        /// </returns>
        /// <remarks>
        /// Use cloned versions of the query for multithreaded applications.
        /// You can safely modify the cloned version of the query without affecting other tasks using the same query.
        /// </remarks>
        public ISQLUIQuery GetQueryForType(string idtype, bool clone = true)
        {
            if (_typeQueries.ContainsKey(idtype))
            {
                ISQLUIQuery query = _typeQueries[idtype];
                if (clone)
                {
                    query = ((SQLElement)query).Clone(query.Parser.Provider) as ISQLUIQuery;
                    query.Parser = Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
                    query.Parser.Connector = Connector;
                }
                return query;
            }
            return null;
        }
        /// <summary>
        /// IGenericObjectRepository: Object creation factory.
        /// </summary>
        /// <param name="type">
        /// Object type to create.
        /// </param>
        /// <returns>
        /// Object instance as ObjectBase.
        /// </returns>
        public ObjectBase CreateObject(Type type)
        {
            if (type == typeof(Keyword))
            {
                Keyword obj = new Keyword(this);
                return obj;
            }
            if (type == typeof(MatchEvent))
            {
                MatchEvent obj = new MatchEvent(this);
                return obj;
            }
            if (type == typeof(Position))
            {
                Position obj = new Position(this);
                return obj;
            }
            if (type == typeof(Match))
            {
                Match obj = new Match(this);
                return obj;
            }
            if (type == typeof(MatchKeyword))
            {
                MatchKeyword obj = new MatchKeyword(this);
                return obj;
            }
            if (type == typeof(MatchStatistic))
            {
                MatchStatistic obj = new MatchStatistic(this);
                return obj;
            }
            if (type == typeof(PositionStatistic))
            {
                PositionStatistic obj = new PositionStatistic(this);
                return obj;
            }
            if (type == typeof(MatchPosition))
            {
                MatchPosition obj = new MatchPosition(this);
                return obj;
            }
            if (type == typeof(MoveComment))
            {
                MoveComment obj = new MoveComment(this);
                return obj;
            }
            if (type == typeof(MatchMove))
            {
                MatchMove obj = new MatchMove(this);
                return obj;
            }
            return null;
        }
        /// <summary>
        /// IObjectRepository: Object creation factory.
        /// </summary>
        /// <param name="copy">
        /// Object to provide data and type to create a copy (excepting keys).
        /// </param>
        /// <returns>
        /// Object instance as ObjectBase.
        /// </returns>
        public ObjectBase CreateObject(ObjectBase copy)
        {
            if (copy is Keyword)
            {
                Keyword obj = new Keyword(this, copy);
                return obj;
            }
            if (copy is MatchEvent)
            {
                MatchEvent obj = new MatchEvent(this, copy);
                return obj;
            }
            if (copy is Position)
            {
                Position obj = new Position(this, copy);
                return obj;
            }
            if (copy is Match)
            {
                Match obj = new Match(this, copy);
                return obj;
            }
            if (copy is MatchKeyword)
            {
                MatchKeyword obj = new MatchKeyword(this, copy);
                return obj;
            }
            if (copy is MatchStatistic)
            {
                MatchStatistic obj = new MatchStatistic(this, copy);
                return obj;
            }
            if (copy is PositionStatistic)
            {
                PositionStatistic obj = new PositionStatistic(this, copy);
                return obj;
            }
            if (copy is MatchPosition)
            {
                MatchPosition obj = new MatchPosition(this, copy);
                return obj;
            }
            if (copy is MoveComment)
            {
                MoveComment obj = new MoveComment(this, copy);
                return obj;
            }
            if (copy is MatchMove)
            {
                MatchMove obj = new MatchMove(this, copy);
                return obj;
            }
            return null;
        }
        /// <summary>
        /// IObjectRepository: Run a query with a specific timeout.
        /// </summary>
        /// <param name="query">
        /// Compiled query to execute.
        /// </param>
        /// <param name="connection">
        /// Connection index to execute the query.
        /// </param>
        /// <returns>
        /// DataTable with the results of the query.
        /// </returns>
        public async Task<DataTable> GlobalQuery(ISQLUIQuery query, int connection)
        {
            return await Connector.ExecuteTableAsync(query, null, null, connection);
        }
    }
}

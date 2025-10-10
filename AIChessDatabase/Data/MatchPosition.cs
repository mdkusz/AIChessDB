using AIChessDatabase.Interfaces;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.UserInterface;
using GlobalCommonEntities.Attributes;
using Resources;
using System;
using System.ComponentModel;
using System.Data;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// MatchPosition POCO class represents a position in a chess match, including its order, events, and score.
    /// </summary>
    [Serializable]
    [TableName("match_positions")]
    public class MatchPosition : ObjectBase, IComparable<MatchPosition>
    {
        private const string _querysql = "select mp.cod_match,mp.position_order,mp.position_events,mp.score,p.* from match_positions mp join positions p on mp.cod_position = p.cod_position";
        private const string _querycntsql = "select count(*) from match_positions mp join positions p on mp.cod_position = p.cod_position";
        public MatchPosition()
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
        }
        public MatchPosition(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
        }
        public MatchPosition(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            MatchPosition pos = copy as MatchPosition;
            if (pos != null)
            {
                _querySQL = _querysql;
                _queryCountSQL = _querycntsql;
                Board = rep.CreateObject(pos.Board) as Position;
                Order = pos.Order;
                Events = pos.Events;
                Score = pos.Score;
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// Primary key values for MatchPosition, composed of IdMatch, Board.IdPosition, and Order.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdMatch, Board.IdPosition, (ulong)Order };
            }
        }
        /// <summary>
        /// Set the primary key values of the object.
        /// </summary>
        /// <param name="key">
        /// Array of primary key values. The order of the values must match the order of the primary key components.
        /// </param>
        public override void SetKey(ulong[] key)
        {
            IdMatch = key[0];
            Board.IdPosition = key[1];
            Order = (int)key[2];
        }
        /// <summary>
        /// Database repository for MatchPosition.
        /// </summary>
        [JsonIgnore]
        public override IObjectRepository Repository
        {
            get
            {
                return base.Repository;
            }
            set
            {
                base.Repository = value;
                if (Board != null)
                {
                    Board.Repository = value;
                }
            }
        }
        /// <summary>
        /// Match primary key value.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_match")]
        [TableColumnName("cod_match")]
        public ulong IdMatch { get; set; }
        /// <summary>
        /// Board object representing the position in the match.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("board")]
        [TableColumnName("cod_position")]
        public Position Board { get; set; }
        /// <summary>
        /// Position order in the match.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_pos_order), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_move_order), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("position_order")]
        [TableColumnName("position_order")]
        public int Order { get; set; }
        /// <summary>
        /// Match event flags for the position, such as captures, checks, etc.
        /// </summary>
        /// <see cref="MatchEvent"/>
        [HiddenField(true)]
        [JsonPropertyName("position_events")]
        [TableColumnName("position_events")]
        public ulong Events { get; set; }
        /// <summary>
        /// Score of the position in the match, representing its evaluation.
        /// </summary>
        [TableColumnName("score")]
        [JsonIgnore]
        [DILocalizedDisplayName(nameof(DN_score), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_pos_score), typeof(Properties.UIResources))]
        public double Score { get; set; }
        /// <summary>
        /// Retrieve position data from the database using the primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key values for MatchPosition, which include IdMatch, Board.IdPosition, and Order.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"mp.cod_match = {_parameterPrefix}cod_match and mp.cod_position = {_parameterPrefix}cod_position and mp.position_order = {_parameterPrefix}position_order";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        /// Retrieve position data from the database using the primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key values for MatchPosition, which include IdMatch, Board.IdPosition, and Order.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"mp.cod_match = {_parameterPrefix}cod_match and mp.cod_position = {_parameterPrefix}cod_position and mp.position_order = {_parameterPrefix}position_order";
            InternalGet(key, text, connection);
        }
        /// <summary>
        /// Use a DataRow to fill the object properties without retrieving from the database.
        /// </summary>
        /// <param name="row">
        /// DataRow to use to fill the object properties. The row must have been executed with a query that returns the object properties.
        /// </param>
        public override void Get(DataRow row)
        {
            IdMatch = Convert.ToUInt64(row["cod_match"]);
            Order = Convert.ToInt32(row["position_order"]);
            Events = Convert.ToUInt64(row["position_events"]);
            Score = Convert.ToDouble(row["score"]);
            Board = Repository.CreateObject(typeof(Position)) as Position;
            Board.Get(row);
        }
        /// <summary>
        /// Query to optimize loop operations (insert, update, delete) of descendant objects.
        /// </summary>
        [JsonIgnore]
        public ISQLUIQuery PositionsLoopQuery { get; set; }
        /// <summary>
        /// Insert for descendant objects that need to re-use the cloned query object.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="query">
        /// Cloned query to re-use. If null, a new query will be retrieved from the repository.
        /// </param>
        /// <returns>
        /// The query used to insert the object to re-use in other objects.
        /// </returns>
        public override async Task<ISQLUIQuery> LoopInsert(int connection, ISQLUIQuery query)
        {
            PositionsLoopQuery = await Board.LoopInsert(connection, PositionsLoopQuery);
            int cnt = Convert.ToInt32(await _repository.Connector.ExecuteScalarAsync($"select count(*) from match_positions where cod_position = {Board.IdPosition} " +
                $"and cod_match = {IdMatch} and position_order = {Order}", null, connection));
            if (cnt == 0)
            {
                query = await base.LoopInsert(connection, query);
            }
            return query;
        }
        /// <summary>
        /// Update for descendant objects that need to re-use the cloned query object.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="query">
        /// Cloned query to re-use. If null, a new query will be retrieved from the repository.
        /// </param>
        /// <returns>
        /// The query used to insert the object to re-use in other objects.
        /// </returns>
        public override async Task<ISQLUIQuery> LoopUpdate(int connection, ISQLUIQuery query)
        {
            query = await base.LoopUpdate(connection, query);
            await Board.Update(connection);
            return query;
        }
        /// <summary>
        /// Delete for descendant objects that need to re-use the cloned query object.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="query">
        /// Cloned query to re-use. If null, a new query will be retrieved from the repository.
        /// </param>
        /// <returns>
        /// The query used to insert the object to re-use in other objects.
        /// </returns>
        public override async Task<ISQLUIQuery> LoopDelete(int connection, ISQLUIQuery query)
        {
            int cnt = Convert.ToInt32(await _repository.Connector.ExecuteScalarAsync($"select count(*) from match_positions where cod_position = {Board.IdPosition} " +
                $"and cod_match = {IdMatch} and position_order = {Order}", null, connection));
            if (cnt == 1)
            {
                query = await base.LoopDelete(connection, query);
            }
            await Board.Delete(connection);
            return query;
        }
        /// <summary>
        /// Get the inner object that corresponds to a given field name and its value in the database.
        /// </summary>
        /// <param name="key">
        /// Key value.
        /// </param>
        /// <param name="pname">
        /// Field name in the IDataReader that contains the primary key value of the inner object.
        /// </param>
        protected override void GetInnerObject(ulong key, string pname, int connection = 0)
        {
            if (pname == "initial_position")
            {
                Board = Repository.CreateObject(typeof(Position)) as Position;
                Board.Get(new ulong[] { key }, connection);
            }
            else if ((pname == "from_position") || (pname == "to_position"))
            {
                Board = Repository.CreateObject(typeof(Position)) as Position;
                Board.IdPosition = key;
            }
        }
        public override string ToString()
        {
            return Order.ToString() + ": " + Board;
        }
        public int CompareTo(MatchPosition other)
        {
            if (Order == other.Order)
            {
                return 0;
            }
            else if (Order > other.Order)
            {
                return 1;
            }
            return -1;
        }
    }
}

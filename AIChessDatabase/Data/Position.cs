using AIChessDatabase.Interfaces;
using AIChessDatabase.Query;
using BaseClassesAndInterfaces.DML;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using GlobalCommonEntities.Attributes;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Position POCO class representing a chess position in the database.
    /// </summary>
    [Serializable]
    [TableName("positions")]
    public class Position : ObjectBase
    {
        protected string _board = null;
        protected List<PositionStatistic> _statistics = new List<PositionStatistic>();
        private const string _querysql = "select * from positions";
        private const string _querycntsql = "select count(*) cnt from positions";

        public Position()
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
            StsDate = DateTime.MinValue;
        }
        public Position(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
            StsDate = DateTime.MinValue;
        }
        public Position(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            Position p = copy as Position;
            if (p != null)
            {
                _querySQL = _querysql;
                _queryCountSQL = _querycntsql;
                Board = p.Board;
                WhitePawnCount = p.WhitePawnCount;
                BlackPawnCount = p.BlackPawnCount;
                WhiteRookCount = p.WhiteRookCount;
                BlackRookCount = p.BlackRookCount;
                WhiteKnightCount = p.WhiteKnightCount;
                BlackKnightCount = p.BlackKnightCount;
                WhiteBishopCount = p.WhiteBishopCount;
                BlackBishopCount = p.BlackBishopCount;
                WhiteQueenCount = p.WhiteQueenCount;
                BlackQueenCount = p.BlackQueenCount;
                StsDate = DateTime.MinValue;
                foreach (PositionStatistic st in p.Statistics)
                {
                    _statistics.Add(rep.CreateObject(st) as PositionStatistic);
                }
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// Repository to access the database.
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
                foreach (PositionStatistic sts in _statistics)
                {
                    sts.Repository = value;
                }
            }
        }
        /// <summary>
        /// Primary key value for the position, used to uniquely identify it in the database.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdPosition };
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
            IdPosition = key[0];
        }
        /// <summary>
        /// Primary key value for the position, used to uniquely identify it in the database.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_position")]
        [TableColumnName("cod_position")]
        public ulong IdPosition { get; set; }
        /// <summary>
        /// String representation of the chess position in custom format.
        /// </summary>
        /// <remarks>
        /// This is a 64 character string representing the board position.
        /// 0: are for empty squares:
        /// b or B are for black and white bishops respectively,
        /// r or R are for black and white rooks respectively,
        /// n or N are for black and white knights respectively,
        /// q or Q are for black and white queens respectively,
        /// k or K are for black and white kings respectively,
        /// p or P are for black and white pawns respectively.
        /// </remarks>
        [DILocalizedDescription(nameof(DES_board), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_board), typeof(Properties.UIResources))]
        [JsonPropertyName("board")]
        [TableColumnName("board")]
        public string Board
        {
            get
            {
                return _board;
            }
            set
            {
                _board = value;
                WhitePawnCount = PzCount("P");
                BlackPawnCount = PzCount("p");
                WhiteRookCount = PzCount("R");
                BlackRookCount = PzCount("r");
                WhiteKnightCount = PzCount("N");
                BlackKnightCount = PzCount("n");
                WhiteBishopCount = PzCount("B");
                BlackBishopCount = PzCount("b");
                WhiteQueenCount = PzCount("Q");
                BlackQueenCount = PzCount("q");
            }
        }
        /// <summary>
        /// WHite pawn count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_w_pawns), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_w_pawns), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("white_pawns")]
        [TableColumnName("white_pawns")]
        public byte WhitePawnCount { get; set; }
        /// <summary>
        /// Black pawn count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_b_pawns), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_b_pawns), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("black_pawns")]
        [TableColumnName("black_pawns")]
        public byte BlackPawnCount { get; set; }
        /// <summary>
        /// White rook count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_w_rooks), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_w_rooks), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("white_rooks")]
        [TableColumnName("white_rooks")]
        public byte WhiteRookCount { get; set; }
        /// <summary>
        /// Black rook count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_b_rooks), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_b_rooks), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("black_rooks")]
        [TableColumnName("black_rooks")]
        public byte BlackRookCount { get; set; }
        /// <summary>
        /// White knight count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_w_knights), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_w_knights), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("white_knights")]
        [TableColumnName("white_knights")]
        public byte WhiteKnightCount { get; set; }
        /// <summary>
        /// Black knight count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_b_knights), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_b_knights), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("black_knights")]
        [TableColumnName("black_knights")]
        public byte BlackKnightCount { get; set; }
        /// <summary>
        /// White bishop count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_w_bishops), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_w_bishops), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("white_bishops")]
        [TableColumnName("white_bishops")]
        public byte WhiteBishopCount { get; set; }
        /// <summary>
        /// Black bishop count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_b_bishops), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_b_bishops), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("black_bishops")]
        [TableColumnName("black_bishops")]
        public byte BlackBishopCount { get; set; }
        /// <summary>
        /// White queen count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_w_queens), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_w_queens), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("white_queens")]
        [TableColumnName("white_queens")]
        public byte WhiteQueenCount { get; set; }
        /// <summary>
        /// Black queen count in the position.
        /// </summary>
        [DILocalizedDescription(nameof(DES_b_queens), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_b_queens), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("black_queens")]
        [TableColumnName("black_queens")]
        public byte BlackQueenCount { get; set; }
        /// <summary>
        /// Date of the last statistics update for this position.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("sts_date")]
        [TableColumnName("sts_date")]
        public DateTime StsDate { get; set; }
        /// <summary>
        /// Retrieves the position from the database using its primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key value for the position, which is an array containing the position ID.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation. Default is 0.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"cod_position = {_parameterPrefix}cod_position";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        /// Retrieves the position from the database using its primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key value for the position, which is an array containing the position ID.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation. Default is 0.
        /// </param>
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"cod_position = {_parameterPrefix}cod_position";
            InternalGet(key, text, connection);
        }
        /// <summary>
        /// Use a DataRow to fill the object properties without retrieving it from the database.
        /// </summary>
        /// <param name="row">
        /// DataRow to use to fill the object properties. The row must have been executed with a query that returns the object properties.
        /// </param>
        public override void Get(DataRow row)
        {
            IdPosition = Convert.ToUInt64(row["cod_position"]);
            WhiteBishopCount = Convert.ToByte(row["white_bishops"]);
            BlackBishopCount = Convert.ToByte(row["black_bishops"]);
            WhiteKnightCount = Convert.ToByte(row["white_knights"]);
            BlackKnightCount = Convert.ToByte(row["black_knights"]);
            WhitePawnCount = Convert.ToByte(row["white_pawns"]);
            BlackPawnCount = Convert.ToByte(row["black_pawns"]);
            WhiteQueenCount = Convert.ToByte(row["white_queens"]);
            BlackQueenCount = Convert.ToByte(row["black_queens"]);
            WhiteRookCount = Convert.ToByte(row["white_rooks"]);
            BlackRookCount = Convert.ToByte(row["black_rooks"]);
            Board = Convert.ToString(row["board"]);
            if (row["sts_date"] == DBNull.Value)
            {
                StsDate = DateTime.MinValue;
            }
            else
            {
                StsDate = Convert.ToDateTime(row["sts_date"]);
            }
        }
        /// <summary>
        /// Statistics for the position.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<PositionStatistic> Statistics
        {
            get
            {
                foreach (PositionStatistic sts in _statistics)
                {
                    yield return sts;
                }
            }
        }
        /// <summary>
        /// Add statistic for fast loading.
        /// </summary>
        /// <param name="statistic"></param>
        public void AddStatistic(PositionStatistic statistic)
        {
            _statistics.Add(statistic);
        }
        /// <summary>
        /// Get a list of all children of a specific type for this position.
        /// </summary>
        /// <param name="ChildType">
        /// Child type to retrieve, such as PositionStatistic.
        /// </param>
        /// <param name="filter">
        /// Data filter to apply when retrieving children.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation. Default is 0.
        /// </param>
        /// <returns>
        /// List of ObjectBase instances representing the children of the specified type.
        /// </returns>
        public override async Task<List<ObjectBase>> GetAllChildren(Type ChildType, DataFilter filter, int connection = 0)
        {
            List<ObjectBase> result = new List<ObjectBase>();
            switch (ChildType.Name)
            {
                case nameof(PositionStatistic):
                    PositionStatistic sts = _repository.CreateObject(typeof(PositionStatistic)) as PositionStatistic;
                    foreach (ObjectBase obj in await sts.GetAllAsync(filter, connection))
                    {
                        result.Add(obj);
                    }
                    break;
            }
            return result;
        }
        /// <summary>
        /// Retrieves the children of this position, specifically PositionStatistic objects.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the database operation. Default is 0.
        /// </param>
        public override async Task GetChildrenAsync(int connection = 0)
        {
            _statistics.Clear();
            try
            {
                PositionStatistic ms = _repository.CreateObject(typeof(PositionStatistic)) as PositionStatistic;
                ISQLUIQuery query = ms.ObjectQuery(connection);
                ISQLElementProvider esql = query.Parser.Provider;
                SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_position"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdPosition.ToString() }));
                DataFilter filter = new DataFilter();
                filter.WFilter = new UIFilterExpression();
                filter.WFilter.SetElement(expr);
                filter.OExpr = new UIOrderByExpression();
                filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "keyword"));
                foreach (PositionStatistic k in await ms.GetAllAsync(filter, connection))
                {
                    _statistics.Add(k);
                }
            }
            finally
            {
            }
        }
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
            object idpos = await _repository.Connector.ExecuteScalarAsync($"select cod_position from positions where board = '{Board}'", null, connection);
            if (idpos == null)
            {
                query = await base.LoopInsert(connection, query);
                foreach (PositionStatistic st in _statistics)
                {
                    st.IdPosition = IdPosition;
                    await st.Insert(connection);
                }
            }
            else
            {
                IdPosition = Convert.ToUInt64(idpos);
                ResetStatistics();
                StsDate = DateTime.MinValue;
                query = await LoopUpdate(connection, query);
            }
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
            string check = $"select count(*) from positions p where p.cod_position = {IdPosition} " +
                "and (exists (select 1 from match_positions mp where mp.cod_position = p.cod_position) " +
                "or exists (select 1 from match_moves mm where mm.from_position = p.cod_position) " +
                "or exists(select 1 from matches m where m.initial_position = p.cod_position))";
            ulong inuse = Convert.ToUInt64(await _repository.Connector.ExecuteScalarAsync(check, null, connection));
            if (inuse == 0)
            {
                PositionStatistic ps = _repository.CreateObject(typeof(PositionStatistic)) as PositionStatistic;
                ISQLUIQuery pquery = ps.ObjectQuery(connection);
                Table tcom = pquery.Tables[0] as Table;
                ((IUpdateManager)pquery).ParseUpdateCommand(tcom.FullName, new List<string> {
                        $"delete from position_statistics where cod_position = {_parameterPrefix}cod_position"
                    }, UpdateMode.Delete);
                DMLCommand cmd = tcom.DeleteCommand;
                cmd.Parameters[0].DefaultValue = IdPosition;
                await cmd.ExecuteAsync(_repository.Connector, connection);
                query = await base.LoopDelete(connection, query);
            }
            else
            {
                ResetStatistics();
                StsDate = DateTime.MinValue;
                query = await LoopUpdate(connection, query);
            }
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
            if (pname == "cod_position")
            {
                Get(new ulong[] { key }, connection);
            }
        }
        /// <summary>
        /// Retrieves the statistics for this position from the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the database operation. Default is 0.
        /// </param>
        public virtual async Task GetStatistics(int connection = 0)
        {
            _statistics.Clear();
            foreach (ObjectBase s in await GetAllChildren(typeof(PositionStatistic), null, connection))
            {
                _statistics.Add(s as PositionStatistic);
            }
        }
        /// <summary>
        /// Adds a child object to the position, specifically a PositionStatistic.
        /// </summary>
        /// <param name="child">
        /// Child object to add, which must be of type PositionStatistic.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation. Default is 0.
        /// </param>
        public override async Task AddChildAsync(ObjectBase child, int connection = 0)
        {
            PositionStatistic sts = child as PositionStatistic;
            if (sts != null)
            {
                sts.IdPosition = IdPosition;
                sts.Repository = Repository;
                _statistics.Add(sts);
                if (IdPosition > 0)
                {
                    await sts.Insert(connection);
                }
                return;
            }
        }
        /// <summary>
        /// Removes a child object from the position, specifically a PositionStatistic.
        /// </summary>
        /// <param name="child">
        /// Child object to remove, which must be of type PositionStatistic.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation. Default is 0.
        /// </param>
        public override async Task RemoveChildAsync(ObjectBase child, int connection = 0)
        {
            PositionStatistic sts = child as PositionStatistic;
            if (sts != null)
            {
                if (_statistics.Contains(sts))
                {
                    _statistics.Remove(sts);
                    if (IdPosition > 0)
                    {
                        await sts.Delete(connection);
                    }
                }
                return;
            }
        }
        /// <summary>
        /// Mark the position statistics as reset, to force recalculation on next access.
        /// </summary>
        protected void ResetStatistics()
        {
            StsDate = DateTime.MinValue;
        }
        /// <summary>
        /// Counts the occurrences of a specified string within the board.
        /// </summary>
        /// <param name="pz">
        /// The string to search for within the board. Cannot be null.
        /// </param>
        /// <returns>
        /// The number of times the specified string appears in the board, as a byte.
        /// </returns>
        protected byte PzCount(string pz)
        {
            return (byte)(_board.Length - _board.Replace(pz, "").Length);
        }
        public override string ToString()
        {
            return Board;
        }
    }
}

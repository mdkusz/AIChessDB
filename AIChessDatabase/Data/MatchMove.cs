using AIChessDatabase.Interfaces;
using AIChessDatabase.Properties;
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
    /// MatchMove POCO class representing a move in a chess match.
    /// </summary>
    [Serializable]
    [TableName("match_moves")]
    public class MatchMove : ObjectBase, IComparable<MatchMove>
    {
        protected byte _player = 0;  // 0 = white 1 = black
        protected List<MoveComment> _comments = new List<MoveComment>();
        protected bool _hasComments = false;
        private const string _sqlquery = "select * from match_moves";
        private const string _sqlcntquery = "select count(*) cnt from match_moves";

        public MatchMove()
        {
            _querySQL = _sqlquery;
            _queryCountSQL = _sqlcntquery;
        }
        public MatchMove(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _sqlquery;
            _queryCountSQL = _sqlcntquery;
        }
        public MatchMove(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            MatchMove mm = copy as MatchMove;
            if (mm != null)
            {
                _querySQL = _sqlquery;
                _queryCountSQL = _sqlcntquery;
                InitialPosition = rep.CreateObject(mm.InitialPosition) as MatchPosition;
                FinalPosition = rep.CreateObject(mm.FinalPosition) as MatchPosition;
                MoveNumber = mm.MoveNumber;
                Order = mm.Order;
                Player = mm._player;
                Events = mm.Events;
                From = mm.From;
                To = mm.To;
                ANText = mm.ANText;
                Score = mm.Score;
                foreach (MoveComment mc in mm.Comments)
                {
                    AddChild(rep.CreateObject(mc));
                }
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// Database repository for this object.
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
                if (InitialPosition != null)
                {
                    InitialPosition.Repository = value;
                }
                if (FinalPosition != null)
                {
                    FinalPosition.Repository = value;
                }
                foreach (MoveComment c in _comments)
                {
                    c.Repository = value;
                }
            }
        }
        /// <summary>
        /// Primary key for this object, used to uniquely identify it in the database.
        /// It is a combination of the match primary key value and the move order.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdMatch, (ulong)Order };
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
            Order = (int)key[1];
        }
        /// <summary>
        /// Match primary key value
        /// </summary>
        [TableColumnName("cod_match")]
        [JsonIgnore]
        [HiddenField(true)]
        public ulong IdMatch { get; set; }
        /// <summary>
        ///  Match position at the start of the move.
        /// </summary>
        [JsonPropertyName("from_position")]
        [JsonConverter(typeof(MatchPositionConverter))]
        [HiddenField(true)]
        [TableColumnName("from_position")]
        public MatchPosition InitialPosition { get; set; }
        /// <summary>
        /// Match position at the end of the move.
        /// </summary>
        [JsonPropertyName("to_position")]
        [JsonConverter(typeof(MatchPositionConverter))]
        [HiddenField(true)]
        [TableColumnName("to_position")]
        public MatchPosition FinalPosition { get; set; }
        /// <summary>
        /// Move number of the move in the match.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_move_number), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_move_number), typeof(UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("move_number")]
        [TableColumnName("move_number")]
        public int MoveNumber { get; set; }
        /// <summary>
        /// Ply order of the move in the match.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_move_order), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_move_order), typeof(UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("move_order")]
        [TableColumnName("move_order")]
        public int Order { get; set; }
        /// <summary>
        /// Player number of the move: 0 for white, 1 for black.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_move_player), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_move_player), typeof(UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("move_player")]
        [TableColumnName("move_player")]
        public byte Player
        {
            get
            {
                return _player;
            }
            set
            {
                if ((value == 0) || (value == 1))
                {
                    _player = value;
                }
                else
                {
                    throw new Exception(ERR_BADPLAYERNUMBER);
                }
            }
        }
        /// <summary>
        /// Match event flags for the move, represented as a bitmask.
        /// </summary>
        /// <see cref="MatchEvent"/>
        [HiddenField(true)]
        [JsonPropertyName("move_events")]
        [TableColumnName("move_events")]
        public ulong Events { get; set; }
        /// <summary>
        /// Square from which the piece was moved. 1 to 64 for a1 to h8.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("move_from")]
        [TableColumnName("move_from")]
        public byte From { get; set; }
        /// <summary>
        /// Square to which the piece was moved. 1 to 64 for a1 to h8.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("move_to")]
        [TableColumnName("move_to")]
        public byte To { get; set; }
        /// <summary>
        /// Algebraic notation of the move, including any special annotations (e.g., check, mate, etc.).
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_move_an_text), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_move_an_text), typeof(UIResources))]
        [JsonPropertyName("move_an_text")]
        [TableColumnName("move_an_text")]
        public string ANText { get; set; }
        /// <summary>
        /// Score of the move, which can be used to evaluate the position after the move.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_score), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_move_score), typeof(UIResources))]
        [TableColumnName("score")]
        [JsonIgnore]
        public double Score { get; set; }
        /// <summary>
        /// Flag indicating whether the move has associated comments.
        /// </summary>
        [HiddenField(true)]
        [TableColumnName("comments")]
        [JsonIgnore]
        public bool HasComments
        {
            get
            {
                return _hasComments;
            }
            set
            {
                _hasComments = value;
            }
        }
        /// <summary>
        /// Get move data from the database using the primary key value.
        /// </summary>
        /// <param name="key">
        /// Move primary key value, which is a combination of the match primary key and the move order.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"cod_match = {_parameterPrefix}cod_match and move_order = {_parameterPrefix}move_order";
            await InternalGetAsync(key, text, connection);
            await InitialPosition.GetAsync(new ulong[] { IdMatch, InitialPosition.Board.IdPosition, (ulong)Order - 1 }, connection);
            await FinalPosition.GetAsync(new ulong[] { IdMatch, FinalPosition.Board.IdPosition, (ulong)Order }, connection);
            if (_hasComments)
            {
                await GetCommentsAsync(connection);
            }
        }
        /// <summary>
        /// Get move data from the database using the primary key value.
        /// </summary>
        /// <param name="key">
        /// Move primary key value, which is a combination of the match primary key and the move order.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"cod_match = {_parameterPrefix}cod_match and move_order = {_parameterPrefix}move_order";
            InternalGet(key, text, connection);
            InitialPosition.Get(new ulong[] { IdMatch, InitialPosition.Board.IdPosition, (ulong)Order - 1 }, connection);
            FinalPosition.Get(new ulong[] { IdMatch, FinalPosition.Board.IdPosition, (ulong)Order }, connection);
            if (_hasComments)
            {
                GetComments(connection);
            }
        }
        /// <summary>
        /// Use a DataRow to fill the object properties without querying the database.
        /// </summary>
        /// <param name="row">
        /// DataRow to use to fill the object properties. The row must have been executed with a query that returns the object properties.
        /// </param>
        public override void Get(DataRow row)
        {
            IdMatch = Convert.ToUInt64(row["cod_match"]);
            Order = Convert.ToInt32(row["move_order"]);
            Events = Convert.ToUInt64(row["move_events"]);
            From = Convert.ToByte(row["move_from"]);
            To = Convert.ToByte(row["move_to"]);
            MoveNumber = Convert.ToInt32(row["move_number"]);
            Player = Convert.ToByte(row["move_player"]);
            ANText = row["move_an_text"] as string;
            Score = Convert.ToDouble(row["score"]);
            HasComments = Convert.ToBoolean(row["comments"]);
        }
        /// <summary>
        /// Move comments associated with this move.
        /// </summary>
        public IEnumerable<MoveComment> Comments
        {
            get
            {
                foreach (MoveComment c in _comments)
                {
                    yield return c;
                }
            }
        }
        /// <summary>
        /// Get the move as a PGN string, optionally including comments.
        /// </summary>
        /// <param name="comment">
        /// Input parameter indicating whether to include comments in the PGN string.
        /// </param>
        /// <returns>
        /// PGN string representation of the move, including comments if specified.
        /// </returns>
        public string GetPGN(ref bool comment)
        {
            string pgn = ANText;
            if (comment && (_comments != null))
            {
                string sep = " ";
                comment = false;
                foreach (MoveComment c in _comments)
                {
                    comment = true;
                    if (c.Comment.StartsWith("$") || c.Comment.StartsWith("("))
                    {
                        pgn += sep + c.Comment;
                    }
                    else
                    {
                        pgn += sep + "{" + c.Comment + "}";
                    }
                }
            }
            else
            {
                comment = false;
            }
            return pgn;
        }
        /// <summary>
        /// Retrieve the value of a field by its databse field name.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>
        /// Object with the value of the field. If the field is a child object, the value is the primary key of the child object. If the field is a DateTime, it returns DBNull.Value if the date is DateTime.MinValue.
        /// </returns>
        protected override object GetFieldValue(string fieldName)
        {
            if (fieldName == "from_position")
            {
                return InitialPosition.Board.IdPosition;
            }
            if (fieldName == "to_position")
            {
                return FinalPosition.Board.IdPosition;
            }
            return base.GetFieldValue(fieldName);
        }
        /// <summary>
        /// Query to optimize loop operations (insert, update, delete) of descendant objects.
        /// </summary>
        [JsonIgnore]
        public ISQLUIQuery PositionsLoopQuery { get; set; }
        /// <summary>
        /// Query to optimize loop operations (insert, update, delete) of descendant objects.
        /// </summary>
        [JsonIgnore]
        public ISQLUIQuery MatchPositionsLoopQuery { get; set; }
        /// <summary>
        /// Query to optimize loop operations (insert, update, delete) of descendant objects.
        /// </summary>
        [JsonIgnore]
        public ISQLUIQuery CommentsLoopQuery { get; set; }
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
            InitialPosition.PositionsLoopQuery = PositionsLoopQuery;
            InitialPosition.IdMatch = IdMatch;
            MatchPositionsLoopQuery = await InitialPosition.LoopInsert(connection, MatchPositionsLoopQuery);
            FinalPosition.PositionsLoopQuery = InitialPosition.PositionsLoopQuery;
            FinalPosition.IdMatch = IdMatch;
            MatchPositionsLoopQuery = await FinalPosition.LoopInsert(connection, MatchPositionsLoopQuery);
            PositionsLoopQuery = FinalPosition.PositionsLoopQuery;
            query = await base.LoopInsert(connection, query);
            foreach (MoveComment com in _comments)
            {
                com.IdMatch = IdMatch;
                CommentsLoopQuery = await com.LoopInsert(connection, CommentsLoopQuery);
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
            InitialPosition.IdMatch = IdMatch;
            await InitialPosition.Update(connection);
            FinalPosition.IdMatch = IdMatch;
            await FinalPosition.Update(connection);
            ISQLUIQuery loopquery = null;
            foreach (MoveComment com in _comments)
            {
                if (com.IdMatch != IdMatch)
                {
                    com.IdMatch = IdMatch;
                    loopquery = await com.LoopInsert(connection, query);
                }
                else
                {
                    loopquery = await com.LoopUpdate(connection, query);
                }
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
            MoveComment mc = _repository.CreateObject(typeof(MoveComment)) as MoveComment;
            ISQLUIQuery cquery = mc.ObjectQuery(connection);
            Table tcom = cquery.Tables[0] as Table;
            ((IUpdateManager)cquery).ParseUpdateCommand(tcom.FullName, new List<string> {
                    $"delete from move_comments where cod_match = {_parameterPrefix}cod_match and move_order = {_parameterPrefix}move_order"
                }, UpdateMode.Delete);
            DMLCommand cmd = tcom.DeleteCommand;
            foreach (SQLParameter p in cmd.Parameters)
            {
                switch (p.Name)
                {
                    case "cod_match":
                        p.DefaultValue = IdMatch;
                        break;
                    case "move_order":
                        p.DefaultValue = Order;
                        break;
                }
            }
            await cmd.ExecuteAsync(_repository.Connector, connection);
            query = await base.LoopDelete(connection, query);
            await InitialPosition.Delete(connection);
            await FinalPosition.Delete(connection);
            return query;
        }
        /// <summary>
        /// Get all child objects of a specific type associated with this move.
        /// </summary>
        /// <param name="ChildType">
        /// Type of child objects to retrieve, such as MoveComment.
        /// </param>
        /// <param name="filter">
        /// DataFilter to apply to the query, allowing for filtering of results based on specific criteria.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        /// <returns>
        /// List of ObjectBase instances representing the child objects of the specified type.
        /// </returns>
        public override async Task<List<ObjectBase>> GetAllChildren(Type ChildType, DataFilter filter, int connection = 0)
        {
            List<ObjectBase> result = new List<ObjectBase>();
            switch (ChildType.Name)
            {
                case nameof(MoveComment):
                    MoveComment com = _repository.CreateObject(typeof(MoveComment)) as MoveComment;
                    foreach (ObjectBase obj in await com.GetAllAsync(filter, connection))
                    {
                        result.Add(obj);
                    }
                    break;
            }
            return result;
        }
        /// <summary>
        /// Retrieve all comments associated with this move from the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        public virtual async Task GetCommentsAsync(int connection = 0)
        {
            _comments.Clear();
            MoveComment mc = _repository.CreateObject(typeof(MoveComment)) as MoveComment;
            ISQLUIQuery query = mc.ObjectQuery(connection);
            ISQLElementProvider esql = query.Parser.Provider;
            SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
            expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
            expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "and" }));
            expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "move_order"));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
            expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { Order.ToString() }));
            DataFilter filter = new DataFilter();
            filter.Query = query;
            filter.WFilter = new UIFilterExpression();
            filter.WFilter.SetElement(expr);
            filter.OExpr = new UIOrderByExpression();
            filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "comment_order"));
            foreach (MoveComment c in await mc.GetAllAsync(filter, connection))
            {
                _comments.Add(c);
            }
        }
        /// <summary>
        /// Retrieve all comments associated with this move from the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        public virtual void GetComments(int connection = 0)
        {
            _comments.Clear();
            MoveComment mc = _repository.CreateObject(typeof(MoveComment)) as MoveComment;
            ISQLUIQuery query = mc.ObjectQuery(connection);
            ISQLElementProvider esql = query.Parser.Provider;
            SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
            expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
            expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "and" }));
            expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "move_order"));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
            expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { Order.ToString() }));
            DataFilter filter = new DataFilter();
            filter.Query = query;
            filter.WFilter = new UIFilterExpression();
            filter.WFilter.SetElement(expr);
            filter.OExpr = new UIOrderByExpression();
            filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "comment_order"));
            foreach (MoveComment c in mc.GetAll(filter, connection))
            {
                _comments.Add(c);
            }
        }
        /// <summary>
        /// Add a child object to this move, specifically a MoveComment.
        /// </summary>
        /// <param name="child">
        /// Child object to add, which must be of type MoveComment.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        public override void AddChild(ObjectBase child, int connection = 0)
        {
            MoveComment c = child as MoveComment;
            if (c != null)
            {
                c.IdMatch = IdMatch;
                c.Repository = Repository;
                c.MoveOrder = Order;
                c.Order = _comments.Count;
                _comments.Add(c);
                _hasComments = true;
            }
        }
        /// <summary>
        /// Add a child object asynchronously to this move, specifically a MoveComment.
        /// </summary>
        /// <param name="child">
        /// Child object to add, which must be of type MoveComment.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        public override async Task AddChildAsync(ObjectBase child, int connection = 0)
        {
            MoveComment c = child as MoveComment;
            if (c != null)
            {
                c.IdMatch = IdMatch;
                c.Repository = Repository;
                c.MoveOrder = Order;
                c.Order = _comments.Count;
                _comments.Add(c);
                _hasComments = true;
            }
            await Task.Yield();
        }
        /// <summary>
        /// Remove a child object from this move, specifically a MoveComment.
        /// </summary>
        /// <param name="child">
        /// Child object to remove, which must be of type MoveComment.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database query, default is 0.
        /// </param>
        public override async Task RemoveChildAsync(ObjectBase child, int connection = 0)
        {
            MoveComment c = child as MoveComment;
            if (c != null)
            {
                if (_comments.Contains(c))
                {
                    _comments.Remove(c);
                    _hasComments = _comments.Count > 0;
                }
            }
            await Task.Yield();
        }
        public override string ToString()
        {
            return MoveNumber.ToString();
        }
        public int CompareTo(MatchMove other)
        {
            if (Equals(other))
            {
                return 0;
            }
            if (MoveNumber > other.MoveNumber)
            {
                return 1;
            }
            if (MoveNumber < other.MoveNumber)
            {
                return -1;
            }
            if (Player > other.Player)
            {
                return 1;
            }
            return -1;
        }
    }
}

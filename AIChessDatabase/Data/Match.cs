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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Match POCO class, representing a chess match with its metadata, moves, keywords, and statistics.
    /// </summary>
    [Serializable]
    [TableName("matches")]
    public class Match : ObjectBase
    {
        protected List<MatchKeyword> _keywords = new List<MatchKeyword>();
        protected List<MatchStatistic> _statistics = new List<MatchStatistic>();
        protected List<MatchMove> _moves = new List<MatchMove>();
        protected string _resultText = "*";
        private const string _querystr = "select * from matches";
        private const string _querycntstr = "select count(*) cnt from matches";

        public Match()
        {
            StsDate = DateTime.MinValue;
            _querySQL = _querystr;
            _queryCountSQL = _querycntstr;
        }
        public Match(IObjectRepository rep)
            : base(rep)
        {
            StsDate = DateTime.MinValue;
            _querySQL = _querystr;
            _queryCountSQL = _querycntstr;
        }
        public Match(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            _querySQL = _querystr;
            _queryCountSQL = _querycntstr;
            Match m = copy as Match;
            if (m != null)
            {
                Description = m.Description;
                InitialPosition = rep.CreateObject(m.InitialPosition) as MatchPosition;
                Date = m.Date;
                White = m.White;
                Black = m.Black;
                ResultText = m.ResultText;
                FullMoveCount = m.FullMoveCount;
                StsDate = DateTime.MinValue;
                MatchPosition current = InitialPosition;
                foreach (MatchKeyword mk in m.Keywords)
                {
                    AddChild(rep.CreateObject(mk));
                }
                foreach (MatchStatistic ms in m.Statistics)
                {
                    AddChild(rep.CreateObject(ms));
                }
                foreach (MatchMove mm in m.Moves)
                {
                    MatchMove newmm = rep.CreateObject(mm) as MatchMove;
                    newmm.InitialPosition = current;
                    current = newmm.FinalPosition;
                    AddChild(newmm);
                }
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// Database and object repository for the match.
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
                foreach (MatchKeyword key in _keywords)
                {
                    key.Repository = value;
                }
                foreach (MatchStatistic sts in _statistics)
                {
                    sts.Repository = value;
                }
                foreach (MatchMove mov in _moves)
                {
                    mov.Repository = value;
                }
            }
        }
        /// <summary>
        /// Match primary key value.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdMatch };
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
        }
        /// <summary>
        /// Match primary key value.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_code), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_code), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("cod_match")]
        [TableColumnName("cod_match")]
        public ulong IdMatch { get; protected set; }
        /// <summary>
        /// Match description.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_description), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_match_description), typeof(Properties.UIResources))]
        [JsonPropertyName("match_description")]
        [TableColumnName("match_description")]
        public string Description { get; set; }
        /// <summary>
        /// Board position at the start of the stored match.
        /// </summary>
        /// <remarks>
        /// Matches can be stored starting from any position, not necessarily the initial position.
        /// </remarks>
        [HiddenField(true)]
        [TableColumnName("initial_position")]
        [JsonPropertyName("initial_position")]
        [JsonConverter(typeof(MatchPositionConverter))]
        public MatchPosition InitialPosition { get; set; }
        /// <summary>
        /// Match result
        /// </summary>
        /// <example>
        /// 0 = *, 1 = 1-0, 2 = 0-1, 3 = 1/2-1/2
        /// </example>
        [DILocalizedDisplayName(nameof(DN_result), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_result), typeof(Properties.UIResources))]
        [VisibleField(false)]
        [TableColumnName("result")]
        [JsonIgnore]
        public byte Result { get; set; }
        /// <summary>
        /// Date of the match.
        /// </summary>
        /// <remarks>
        /// PGN files don’t guarantee a valid date format, so the date is stored as a string.
        /// </remarks>
        [DILocalizedDisplayName(nameof(DN_date), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_match_date), typeof(Properties.UIResources))]
        [JsonPropertyName("match_date")]
        [TableColumnName("match_date")]
        public string Date { get; set; }
        /// <summary>
        /// White player name.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_white), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_white), typeof(Properties.UIResources))]
        [JsonPropertyName("white")]
        [TableColumnName("white")]
        public string White { get; set; }
        /// <summary>
        /// Black player name.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_black), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_black), typeof(Properties.UIResources))]
        [JsonPropertyName("black")]
        [TableColumnName("black")]
        public string Black { get; set; }
        /// <summary>
        /// Match result as a string.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_result_text), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_result_text), typeof(Properties.UIResources))]
        [JsonPropertyName("result_text")]
        [TableColumnName("result_text")]
        public string ResultText
        {
            get
            {
                return _resultText;
            }
            set
            {
                switch (value)
                {
                    case "1-0":
                        _resultText = value;
                        Result = 1;
                        break;
                    case "0-1":
                        _resultText = value;
                        Result = 2;
                        break;
                    case "1/2-1/2":
                        _resultText = value;
                        Result = 3;
                        break;
                    default:
                        _resultText = "*";
                        Result = 0;
                        break;
                }
            }
        }
        /// <summary>
        /// Date of the last statistics update.
        /// </summary>
        [HiddenField(false)]
        [TableColumnName("sts_date")]
        [JsonIgnore]
        public DateTime StsDate { get; set; }
        /// <summary>
        /// Match plies count.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_move_count), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_move_count), typeof(Properties.UIResources))]
        [JsonPropertyName("move_count")]
        [TableColumnName("move_count")]
        public int MoveCount
        {
            get
            {
                return _moves.Count;
            }
            set { }
        }
        /// <summary>
        /// Match full move count.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_fullmove_count), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_fullmove_count), typeof(Properties.UIResources))]
        [JsonPropertyName("fullmove_count")]
        [TableColumnName("fullmove_count")]
        public int FullMoveCount { get; set; }
        /// <summary>
        /// sha256 hash of the match data to check for dupliactes.
        /// </summary>
        [JsonPropertyName("sha256")]
        [TableColumnName("sha256")]
        public byte[] Sha256 { get; set; }
        /// <summary>
        /// Enumeration of match positions, including the initial position and all final positions of each move.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<MatchPosition> Positions
        {
            get
            {
                yield return InitialPosition;
                foreach (MatchMove mov in _moves)
                {
                    yield return mov.FinalPosition;
                }
            }
        }
        /// <summary>
        /// Enumeration of match keywords, which are metadata associated with the match.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<MatchKeyword> Keywords
        {
            get
            {
                foreach (MatchKeyword key in _keywords)
                {
                    yield return key;
                }
            }
        }
        /// <summary>
        /// Enumeration of match statistics, which are numerical data about the match.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<MatchStatistic> Statistics
        {
            get
            {
                foreach (MatchStatistic sts in _statistics)
                {
                    yield return sts;
                }
            }
        }
        /// <summary>
        /// Enumeration of match moves, which are the individual actions taken during the match.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<MatchMove> Moves
        {
            get
            {
                foreach (MatchMove mov in _moves)
                {
                    yield return mov;
                }
            }
        }
        /// <summary>
        /// Calculate and set the SHA256 hash of the match data, based on the sequence of board positions.
        /// </summary>
        public void SetSha256()
        {
            // Concatenate boards: 64*N chars, no separators
            var sb = new StringBuilder((1 + _moves.Count) * 64);
            sb.Append(InitialPosition.Board.ToString()); // must be exactly 64 chars
            for (int i = 0; i < _moves.Count; i++)
                sb.Append(_moves[i].FinalPosition.Board.ToString()); // must be exactly 64 chars

            using (var sha = SHA256.Create())
            {
                Sha256 = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            }
        }
        /// <summary>
        /// Find a keyword by its name.
        /// </summary>
        /// <param name="k">
        /// Keyword name to search for.
        /// </param>
        /// <returns>
        /// Keyword object if found, otherwise null.
        /// </returns>
        public MatchKeyword GetKeyword(string k)
        {
            return _keywords?.FirstOrDefault(kw => kw.Name == k);
        }
        /// <summary>
        /// Get the PGN (Portable Game Notation) representation of the match.
        /// </summary>
        /// <param name="comments">
        /// Add comments to the PGN output if true.
        /// </param>
        /// <returns>
        /// PGN string representation of the match, including metadata and moves.
        /// </returns>
        public string GetPGN(bool comments)
        {
            string pgn = "";
            string[] pk = new string[] { KEY_EVENT, KEY_SITE, KEY_DATE,
                    KEY_ROUND, KEY_WHITE, KEY_BLACK, KEY_RESULT };
            List<MatchKeyword> lpk = new List<MatchKeyword>();
            for (int ix = 0; ix < pk.Length; ix++)
            {
                MatchKeyword mk = GetKeyword(pk[ix]);
                if (mk != null)
                {
                    pgn += string.Format("[{0} \"{1}\"]\n", mk.Name ?? "", mk.Value ?? "");
                    lpk.Add(mk);
                }
            }
            if (_keywords != null)
            {
                foreach (MatchKeyword mk in _keywords)
                {
                    if (!lpk.Contains(mk))
                    {
                        pgn += string.Format("[{0} \"{1}\"]\n", mk.Name ?? "", mk.Value ?? "");
                    }
                }
            }
            string moves = "\n";
            if ((_moves != null) && (_moves.Count > 0))
            {
                bool ellipsis = _moves[0].Player == 1;
                bool tnum = true;
                for (int ix = 0; ix < _moves.Count; ix++)
                {
                    if (tnum)
                    {
                        if (ellipsis)
                        {
                            moves += string.Format("{0}... ", _moves[ix].MoveNumber);
                        }
                        else
                        {
                            moves += string.Format("{0}.", _moves[ix]?.MoveNumber);
                        }
                        ellipsis = false;
                        tnum = false;
                    }
                    bool com = comments;
                    moves += _moves[ix].GetPGN(ref com);
                    if (_moves[ix].Player == 1)
                    {
                        tnum = true;
                    }
                    else if (com)
                    {
                        tnum = true;
                        ellipsis = true;
                    }
                    moves += " ";
                }
            }
            moves += (ResultText ?? "") + "\n\n";
            moves = SplitText(moves, 80);
            return pgn + moves;
        }
        /// <summary>
        /// Reset match statistics date. This causes statistics to be recalculated on the next update.
        /// </summary>
        public void ResetStatistics()
        {
            StsDate = DateTime.MinValue;
        }
        /// <summary>
        /// Load match data quickly from the database using the match primary key.
        /// </summary>
        /// <param name="idmatch">
        /// Match primary key value to load.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation.
        /// </param>
        public async Task FastLoad(ulong idmatch, int connection)
        {
            string tokenset = "";
            string token = "";
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            _parameterPrefix = parser.ParameterPrefix;
            parser.Connector = _repository.Connector;
            parser.ConnectionIndex = connection;
            string query = $"select * from matches where cod_match = {_parameterPrefix}cod_match";
            ISQLUIQuery squery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            squery.Parameters[0].DefaultValue = idmatch;
            DataTable tm = await parser.Connector.ExecuteTableAsync(squery, null, null, connection);
            if (tm == null)
            {
                throw new Exception(string.Format(ERR_MATCHNOTFOUND, idmatch));
            }
            query = $"select mk.*,k.keyword,k.keyword_type,kv.kw_value from match_keywords mk join keywords k on mk.cod_keyword = k.cod_keyword join keyword_values kv on kv.cod_value = mk.cod_value where mk.cod_match = {_parameterPrefix}cod_match";
            tokenset = "";
            token = "";
            squery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            squery.Parameters[0].DefaultValue = idmatch;
            DataTable tk = await parser.Connector.ExecuteTableAsync(squery, null, null, connection);
            query = $"select ms.*,k.keyword,k.keyword_type from match_statistics ms join keywords k on ms.cod_keyword = k.cod_keyword where ms.cod_match = {_parameterPrefix}cod_match";
            tokenset = "";
            token = "";
            squery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            squery.Parameters[0].DefaultValue = idmatch;
            DataTable ts = await parser.Connector.ExecuteTableAsync(squery, null, null, connection);
            query = $"select * from match_moves where cod_match = {_parameterPrefix}cod_match order by move_order";
            tokenset = "";
            token = "";
            squery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            squery.Parameters[0].DefaultValue = idmatch;
            DataTable mv = await parser.Connector.ExecuteTableAsync(squery, null, null, connection);
            if (mv == null)
            {
                throw new Exception(string.Format(ERR_MATCHWITHOUTMOVES, idmatch));
            }
            query = "select mp.cod_match,mp.cod_position,mp.position_order,mp.position_events,mp.score,p.board,p.black_pawns,p.white_pawns,p.black_rooks,p.white_rooks,p.black_bishops,p.white_bishops,p.black_knights,p.white_knights,p.black_queens,p.white_queens,p.sts_date " +
                $"from match_positions mp join positions p on mp.cod_position = p.cod_position where mp.cod_match = {_parameterPrefix}cod_match order by mp.position_order";
            tokenset = "";
            token = "";
            squery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            squery.Parameters[0].DefaultValue = idmatch;
            DataTable tp = await parser.Connector.ExecuteTableAsync(squery, null, null, connection);
            if (tp == null)
            {
                throw new Exception(string.Format(ERR_MATCHWITHOUTPOS, idmatch));
            }
            query = $"select * from move_comments where cod_match = {_parameterPrefix}cod_match order by move_order,comment_order";
            tokenset = "";
            token = "";
            squery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            squery.Parameters[0].DefaultValue = idmatch;
            DataTable cm = await parser.Connector.ExecuteTableAsync(squery, null, null, connection);
            query = "select mp.cod_position,mp.position_order,ps.cod_keyword,ps.st_value,k.keyword,k.keyword_type " +
                $"from match_positions mp join position_statistics ps on mp.cod_position = ps.cod_position join keywords k on ps.cod_keyword = k.cod_keyword where mp.cod_match = {_parameterPrefix}cod_match order by mp.position_order";
            tokenset = "";
            token = "";
            squery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            squery.Parameters[0].DefaultValue = idmatch;
            DataTable ps = await parser.Connector.ExecuteTableAsync(squery, null, null, connection);
            IdMatch = idmatch;
            Description = tm.Rows[0]["match_description"] as string;
            Date = tm.Rows[0]["match_date"] as string;
            White = tm.Rows[0]["white"] as string;
            Black = tm.Rows[0]["black"] as string;
            Result = Convert.ToByte(tm.Rows[0]["result"]);
            ResultText = tm.Rows[0]["result_text"] as string;
            MoveCount = Convert.ToInt32(tm.Rows[0]["move_count"]);
            FullMoveCount = Convert.ToInt32(tm.Rows[0]["fullmove_count"]);
            StsDate = Convert.IsDBNull(tm.Rows[0]["sts_date"]) ? DateTime.MinValue : Convert.ToDateTime(tm.Rows[0]["sts_date"]);
            InitialPosition = _repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
            InitialPosition.Get(tp.Rows[0]);
            _keywords.Clear();
            if (tk != null)
            {
                MatchKeyword[] mkeywords = new MatchKeyword[tk.Rows.Count];
                Parallel.For(0, tk.Rows.Count, ix =>
                {
                    MatchKeyword mk = _repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                    mk.Get(tk.Rows[ix]);
                    mkeywords[ix] = mk;
                });
                _keywords.AddRange(mkeywords);
            }
            _statistics.Clear();
            if (ts != null)
            {
                MatchStatistic[] mstatistics = new MatchStatistic[ts.Rows.Count];
                Parallel.For(0, ts.Rows.Count, ix =>
                {
                    MatchStatistic ms = _repository.CreateObject(typeof(MatchStatistic)) as MatchStatistic;
                    ms.Get(ts.Rows[ix]);
                    mstatistics[ix] = ms;
                });
                _statistics.AddRange(mstatistics);
            }
            _moves.Clear();
            MatchMove[] moves = new MatchMove[mv.Rows.Count];
            Parallel.For(0, mv.Rows.Count, ix =>
            {
                MatchMove mm = _repository.CreateObject(typeof(MatchMove)) as MatchMove;
                mm.Get(mv.Rows[ix]);
                mm.InitialPosition = Repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                mm.InitialPosition.Get(tp.Rows[ix]);
                mm.FinalPosition = Repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                mm.FinalPosition.Get(tp.Rows[ix + 1]);
                moves[ix] = mm;
            });
            _moves.AddRange(moves);
            if (cm != null)
            {
                for (int ix = 0; ix < cm.Rows.Count; ix++)
                {
                    MoveComment mc = _repository.CreateObject(typeof(MoveComment)) as MoveComment;
                    mc.Get(cm.Rows[ix]);
                    _moves[mc.MoveOrder - 1].AddChild(mc, connection);
                }
            }
            if (ps != null)
            {
                for (int ix = 0; ix < ps.Rows.Count; ix++)
                {
                    PositionStatistic psst = _repository.CreateObject(typeof(PositionStatistic)) as PositionStatistic;
                    psst.Get(ps.Rows[ix]);
                    int porder = Convert.ToInt32(ps.Rows[ix]["position_order"]);
                    if (porder == 0)
                    {
                        InitialPosition.AddChild(psst, connection);
                    }
                    else
                    {
                        _moves[porder - 1].FinalPosition.Board.AddStatistic(psst);
                    }
                }
            }
        }
        /// <summary>
        /// Get match data from the database using the match primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key value of the match to retrieve.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"cod_match = {_parameterPrefix}cod_match";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        /// Check if the match is duplicated in the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the database operation.
        /// </param>
        /// <param name="query">
        /// Query object to use for the database operation. If null, a new query will be created from the repository.
        /// </param>
        /// <returns>
        /// True if the match is duplicated, otherwise false.
        /// </returns>
        public virtual async Task<bool> Duplicated(int connection = 0, ISQLUIQuery query = null)
        {
            if (query == null)
            {
                query = GetQuery(connection);
            }
            if (Sha256 == null)
            {
                SetSha256();
            }
            Table bdtabla = query.Tables[0] as Table;
            DMLCommand cmd = bdtabla.InsertCommand;
            SQLParameter param = cmd.Parameters.GetParameterByName("sha256");
            SQLParameterList plist = new SQLParameterList()
            {
                param
            };
            param.DefaultValue = Sha256;
            string cnt = $"select count(*) from matches where sha256 = {_parameterPrefix}sha256";
            return Convert.ToInt32(await Repository.Connector.ExecuteScalarAsync(cnt, plist, connection)) > 0;
        }
        /// <summary>
        /// Retrieve the value of a field by its databse field name.
        /// </summary>
        /// <param name="fieldName">
        /// Name of the field to retrieve the value for.
        /// </param>
        /// <returns>
        /// Object with the value of the field. If the field is a child object, the value is the primary key of the child object. If the field is a DateTime, it returns DBNull.Value if the date is DateTime.MinValue.
        /// </returns>
        protected override object GetFieldValue(string fieldName)
        {
            if (fieldName == "initial_position")
            {
                return InitialPosition.Board.IdPosition;
            }
            return base.GetFieldValue(fieldName);
        }
        /// <summary>
        /// Insert the object into the database. If the object has a primary key with a surrogate, it will be automatically generated.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public override async Task Insert(int connection)
        {
            await LoopInsert(connection, null);
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
            if (Sha256 == null)
            {
                SetSha256();
            }
            ISQLUIQuery qboards = await InitialPosition.Board.LoopInsert(connection, null);
            query = await base.LoopInsert(connection, query);
            InitialPosition.IdMatch = IdMatch;
            ISQLUIQuery qmatchpos = await InitialPosition.LoopInsert(connection, null);
            ISQLUIQuery loopquery = null;
            ISQLUIQuery commentquery = null;
            foreach (MatchMove mov in _moves)
            {
                mov.CommentsLoopQuery = commentquery;
                mov.IdMatch = IdMatch;
                mov.PositionsLoopQuery = qboards;
                mov.MatchPositionsLoopQuery = qmatchpos;
                loopquery = await mov.LoopInsert(connection, loopquery);
                qboards = mov.PositionsLoopQuery;
                qmatchpos = mov.MatchPositionsLoopQuery;
                commentquery = mov.CommentsLoopQuery;
            }
            loopquery = null;
            foreach (MatchKeyword kw in _keywords)
            {
                kw.IdMatch = IdMatch;
                loopquery = await kw.LoopInsert(connection, loopquery);
            }
            loopquery = null;
            foreach (MatchStatistic st in _statistics)
            {
                st.IdMatch = IdMatch;
                loopquery = await st.LoopInsert(connection, loopquery);
            }
            return query;
        }
        /// <summary>
        /// Update the object in the database. The object must have been previously inserted, and its primary key must be set.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public override async Task Update(int connection)
        {
            await LoopUpdate(connection, null);
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
            SetSha256();
            await InitialPosition.Board.Update(connection);
            query = await base.LoopUpdate(connection, query);
            InitialPosition.IdMatch = IdMatch;
            await InitialPosition.Update(connection);
            ISQLUIQuery loopquery = null;
            foreach (MatchMove mov in _moves)
            {
                if (mov.IdMatch != IdMatch)
                {
                    mov.IdMatch = IdMatch;
                    loopquery = await mov.LoopInsert(connection, loopquery);
                }
                else
                {
                    loopquery = await mov.LoopUpdate(connection, loopquery);
                }
            }
            loopquery = null;
            foreach (MatchKeyword kw in _keywords)
            {
                if (kw.IdMatch != IdMatch)
                {
                    kw.IdMatch = IdMatch;
                    loopquery = await kw.LoopInsert(connection, loopquery);
                }
                else
                {
                    loopquery = await kw.LoopUpdate(connection, loopquery);
                }
            }
            loopquery = null;
            foreach (MatchStatistic st in _statistics)
            {
                if (st.IdMatch != IdMatch)
                {
                    st.IdMatch = IdMatch;
                    loopquery = await st.LoopInsert(connection, loopquery);
                }
                else
                {
                    loopquery = await st.LoopUpdate(connection, loopquery);
                }
            }
            return query;
        }
        /// <summary>
        /// Delete the object from the database. The object must have been previously inserted, and its primary key must be set.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public override async Task Delete(int connection)
        {
            await LoopDelete(connection, null);
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
            ISQLUIQuery loopquery = null;
            foreach (MatchKeyword k in _keywords)
            {
                loopquery = await k.LoopDelete(connection, loopquery);
            }
            loopquery = null;
            foreach (MatchMove m in _moves)
            {
                loopquery = await m.LoopDelete(connection, loopquery);
            }
            loopquery = null;
            foreach (MatchStatistic st in _statistics)
            {
                loopquery = await st.LoopDelete(connection, loopquery);
            }
            await InitialPosition.Delete(connection);
            await InitialPosition.Board.Delete(connection);
            return await base.LoopDelete(connection, query);
        }
        /// <summary>
        /// Bulk copy matches from the database to another repository.
        /// </summary>
        /// <param name="bindex">
        /// Block index for pagination.
        /// </param>
        /// <param name="bsize">
        /// Block size for pagination, how many matches to copy at once.
        /// </param>
        /// <param name="duplicates">
        /// Copy duplicates if true, otherwise skip matches that are already present in the destination repository.
        /// </param>
        /// <param name="rdest">
        /// Destination repository where matches will be copied to.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the source repository.
        /// </param>
        /// <param name="conndest">
        /// Connection index to use for the destination repository.
        /// </param>
        /// <returns>
        /// Number of matches copied.
        /// </returns>
        public virtual async Task<int> BulkCopy(int bindex, int bsize, bool duplicates, IObjectRepository rdest, int connection = 0, int conndest = 0)
        {
            string bcsql = _repository.Queries.GetNSQuery(_repository.ServerName, "MatchBulkCopy");
            SQLParameterList pl = BulkParameters(bsize, bindex);
            DataTable tm = await _repository.Connector.ExecuteTableAsync(bcsql, pl, null, null, connection);
            try
            {
                ISQLUIQuery loopquery = null;
                rdest.Connector.OpenConnection(conndest);
                for (int ix = 0; ix < tm.Rows.Count; ix++)
                {
                    Match m = new Match(_repository);
                    try
                    {
                        await m.FastLoad(Convert.ToUInt64(tm.Rows[ix]["cod_match"]), connection);
                        m.ResetStatistics();
                        Match copy = rdest.CreateObject(m) as Match;
                        rdest.Connector.BeginTransaction(conndest);
                        if (!duplicates)
                        {
                            if (!await copy.Duplicated(conndest, loopquery))
                            {
                                loopquery = await copy.LoopInsert(conndest, loopquery);
                                rdest.Connector.Commit(conndest);
                            }
                        }
                        else
                        {
                            loopquery = await copy.LoopInsert(conndest, loopquery);
                            rdest.Connector.Commit(conndest);
                        }
                    }
                    catch (Exception ex)
                    {
                        rdest.Connector.Rollback(conndest);
                        throw new Exception($"cod_match = {m.IdMatch}:\n" + ex.Message);
                    }
                }
            }
            finally
            {
                rdest.Connector.CloseConnection(conndest);
            }
            return tm.Rows.Count;
        }
        /// <summary>
        /// Bulk copy matches to a file.
        /// </summary>
        /// <param name="bindex">
        /// Block index for pagination, which block of matches to copy.
        /// </param>
        /// <param name="bsize">
        /// Block size for pagination, how many matches to copy at once.
        /// </param>
        /// <param name="filename">
        /// Path of the file to save the matches to.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation.
        /// </param>
        /// <returns>
        /// Number of matches copied to the file.
        /// </returns>
        public virtual async Task<int> BulkCopy(int bindex, int bsize, string filename, int connection = 0)
        {
            string bcsql = _repository.Queries.GetNSQuery(_repository.ServerName, "MatchBulkCopy");
            SQLParameterList pl = BulkParameters(bsize, bindex);
            DataTable tm = await _repository.Connector.ExecuteTableAsync(bcsql, pl, null, null, connection);
            List<Match> matches = new List<Match>();
            FileStream fs = null;
            try
            {
                for (int ix = 0; ix < tm.Rows.Count; ix++)
                {
                    Match m = new Match(_repository);
                    await m.FastLoad(Convert.ToUInt64(tm.Rows[ix]["cod_match"]), connection);
                    m.ResetStatistics();
                    matches.Add(m);
                }
                fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, matches);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return tm.Rows.Count;
        }
        /// <summary>
        /// Get a specific move by its move number and player color.
        /// </summary>
        /// <param name="n">
        /// Move number to search for.
        /// </param>
        /// <param name="color">
        /// Player color, true for white (0) and false for black (1).
        /// </param>
        /// <returns>
        /// MatchMove object if found, otherwise null.
        /// </returns>
        public MatchMove GetMove(int n, bool color)
        {
            for (int mv = 0; mv < _moves.Count; mv++)
            {
                if ((_moves[mv].MoveNumber == n) && (_moves[mv].Player == (color ? 0 : 1)))
                {
                    return _moves[mv];
                }
            }
            return null;
        }
        /// <summary>
        /// Get a specific move by its order in the match.
        /// </summary>
        /// <param name="n">
        /// Move order to search for, starting from 0.
        /// </param>
        /// <returns>
        /// MatchMove object if found, otherwise null.
        /// </returns>
        public MatchMove GetMove(int n)
        {
            for (int mv = 0; mv < _moves.Count; mv++)
            {
                if (_moves[mv].Order == n)
                {
                    return _moves[mv];
                }
            }
            return null;
        }
        /// <summary>
        /// Get a specific statistic by its name.
        /// </summary>
        /// <param name="name">
        /// Statistic name to search for.
        /// </param>
        /// <param name="create">
        /// Create a new statistic if it does not exist when true, otherwise return null.
        /// </param>
        /// <returns>
        /// MatchStatistic object if found or created, otherwise null.
        /// </returns>
        public MatchStatistic GetStatistic(string name, bool create)
        {
            MatchStatistic ms = _repository.CreateObject(typeof(MatchStatistic)) as MatchStatistic;
            ms.Name = name;
            ms.IdMatch = IdMatch;
            int ix = _statistics.IndexOf(ms);
            if (ix >= 0)
            {
                return _statistics[ix];
            }
            if (create)
            {
                return ms;
            }
            return null;
        }
        /// <summary>
        /// Add a child object to the match, which can be a keyword, statistic, or move.
        /// </summary>
        /// <param name="child">
        /// Child object to add, which can be a MatchKeyword, MatchStatistic, or MatchMove.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override void AddChild(ObjectBase child, int connection = 0)
        {
            MatchKeyword key = child as MatchKeyword;
            if (key != null)
            {
                key.IdMatch = IdMatch;
                key.Repository = Repository;
                if (!_keywords.Contains(key))
                {
                    _keywords.Add(key);
                }
                return;
            }
            MatchStatistic sts = child as MatchStatistic;
            if (sts != null)
            {
                sts.IdMatch = IdMatch;
                sts.Repository = Repository;
                if (!_statistics.Contains(sts))
                {
                    _statistics.Add(sts);
                }
                return;
            }
            MatchMove mov = child as MatchMove;
            if (mov != null)
            {
                mov.IdMatch = IdMatch;
                mov.Repository = Repository;
                if (!_moves.Contains(mov))
                {
                    _moves.Add(mov);
                    if (mov.MoveNumber > FullMoveCount)
                    {
                        FullMoveCount = mov.MoveNumber;
                    }
                    UpdateStatistics(mov, 1, connection);
                }
            }
        }
        /// <summary>
        /// Add a child object to the match asynchronously, which can be a keyword, statistic, or move.
        /// </summary>
        /// <param name="child">
        /// Child object to add, which can be a MatchKeyword, MatchStatistic, or MatchMove.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override async Task AddChildAsync(ObjectBase child, int connection = 0)
        {
            MatchKeyword key = child as MatchKeyword;
            if (key != null)
            {
                key.IdMatch = IdMatch;
                key.Repository = Repository;
                if (!_keywords.Contains(key))
                {
                    _keywords.Add(key);
                }
                return;
            }
            MatchStatistic sts = child as MatchStatistic;
            if (sts != null)
            {
                sts.IdMatch = IdMatch;
                sts.Repository = Repository;
                if (!_statistics.Contains(sts))
                {
                    _statistics.Add(sts);
                }
                return;
            }
            MatchMove mov = child as MatchMove;
            if (mov != null)
            {
                mov.IdMatch = IdMatch;
                mov.Repository = Repository;
                if (!_moves.Contains(mov))
                {
                    _moves.Add(mov);
                    if (mov.MoveNumber > FullMoveCount)
                    {
                        FullMoveCount = mov.MoveNumber;
                    }
                    await UpdateStatisticsAsync(mov, 1, connection);
                }
            }
        }
        /// <summary>
        /// Remove a child object from the match, which can be a keyword, statistic, or move.
        /// </summary>
        /// <param name="child">
        /// Child object to remove, which can be a MatchKeyword, MatchStatistic, or MatchMove.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override async Task RemoveChildAsync(ObjectBase child, int connection = 0)
        {
            MatchKeyword key = child as MatchKeyword;
            if (key != null)
            {
                if (_keywords.Contains(key))
                {
                    _keywords.Remove(key);
                }
                return;
            }
            MatchStatistic sts = child as MatchStatistic;
            if (sts != null)
            {
                if (_statistics.Contains(sts))
                {
                    _statistics.Remove(sts);
                }
                return;
            }
            MatchMove mov = child as MatchMove;
            if (mov != null)
            {
                if (_moves.Contains(mov))
                {
                    _moves.Remove(mov);
                    if (_moves.Count > 0)
                    {
                        FullMoveCount = _moves[_moves.Count - 1].MoveNumber;
                    }
                    await UpdateStatisticsAsync(mov, -1);
                }
            }
        }
        /// <summary>
        /// Remove a child object from the match, which can be a keyword, statistic, or move.
        /// </summary>
        /// <param name="child">
        /// Child object to remove, which can be a MatchKeyword, MatchStatistic, or MatchMove.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override void RemoveChild(ObjectBase child, int connection = 0)
        {
            MatchKeyword key = child as MatchKeyword;
            if (key != null)
            {
                if (_keywords.Contains(key))
                {
                    _keywords.Remove(key);
                }
                return;
            }
            MatchStatistic sts = child as MatchStatistic;
            if (sts != null)
            {
                if (_statistics.Contains(sts))
                {
                    _statistics.Remove(sts);
                }
                return;
            }
            MatchMove mov = child as MatchMove;
            if (mov != null)
            {
                if (_moves.Contains(mov))
                {
                    _moves.Remove(mov);
                    if (_moves.Count > 0)
                    {
                        FullMoveCount = _moves[_moves.Count - 1].MoveNumber;
                    }
                    UpdateStatistics(mov, -1);
                }
            }
        }
        /// <summary>
        /// Get all child objects of a specific type associated with the match.
        /// </summary>
        /// <param name="ChildType">
        /// Child type to retrieve, which can be MatchMove, MatchKeyword, or MatchStatistic.
        /// </param>
        /// <param name="filter">
        /// Data filter to apply when retrieving child objects.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        /// <returns>
        /// List of found child objects of the specified type.
        /// </returns>
        public override async Task<List<ObjectBase>> GetAllChildren(Type ChildType, DataFilter filter, int connection = 0)
        {
            List<ObjectBase> result = new List<ObjectBase>();
            switch (ChildType.Name)
            {
                case nameof(MatchMove):
                    MatchMove mov = _repository.CreateObject(typeof(MatchMove)) as MatchMove;
                    foreach (ObjectBase obj in await mov.GetAllAsync(filter, connection))
                    {
                        result.Add(obj);
                    }
                    break;
                case nameof(MatchKeyword):
                    MatchKeyword key = _repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                    foreach (ObjectBase obj in await key.GetAllAsync(filter, connection))
                    {
                        result.Add(obj);
                    }
                    break;
                case nameof(MatchStatistic):
                    MatchStatistic sts = _repository.CreateObject(typeof(MatchStatistic)) as MatchStatistic;
                    foreach (ObjectBase obj in await sts.GetAllAsync(filter, connection))
                    {
                        result.Add(obj);
                    }
                    break;
            }
            return result;
        }
        /// <summary>
        /// Retrieve all child objects associated with the match.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override async Task GetChildrenAsync(int connection = 0)
        {
            _moves.Clear();
            MatchMove mov = _repository.CreateObject(typeof(MatchMove)) as MatchMove;
            ISQLUIQuery query = mov.ObjectQuery(connection);
            ISQLElementProvider esql = query.Parser.Provider;
            SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
            expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
            expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
            DataFilter filter = new DataFilter();
            filter.Query = query;
            filter.WFilter = new UIFilterExpression();
            filter.WFilter.SetElement(expr);
            filter.OExpr = new UIOrderByExpression();
            filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "move_order"));
            try
            {
                foreach (MatchMove mm in await mov.GetAllAsync(filter, connection))
                {
                    _moves.Add(mm);
                }
                FullMoveCount = _moves[_moves.Count - 1].MoveNumber;
                for (int ix = 0; ix < _moves.Count; ix++)
                {
                    if (_moves[ix].HasComments)
                    {
                        await _moves[ix].GetCommentsAsync(connection);
                    }
                }
            }
            finally
            {
            }
            _keywords.Clear();
            try
            {
                MatchKeyword mk = _repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                query = mk.ObjectQuery(connection);
                expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
                filter = new DataFilter();
                filter.Query = query;
                filter.WFilter = new UIFilterExpression();
                filter.WFilter.SetElement(expr);
                filter.OExpr = new UIOrderByExpression();
                filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "keyword"));
                foreach (MatchKeyword k in await mk.GetAllAsync(filter, connection))
                {
                    _keywords.Add(k);
                }
            }
            finally
            {
            }
            _statistics.Clear();
            try
            {
                MatchStatistic ms = _repository.CreateObject(typeof(MatchStatistic)) as MatchStatistic;
                query = ms.ObjectQuery(connection);
                expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
                filter = new DataFilter();
                filter.Query = query;
                filter.WFilter = new UIFilterExpression();
                filter.WFilter.SetElement(expr);
                filter.OExpr = new UIOrderByExpression();
                filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "keyword"));
                foreach (MatchStatistic k in await ms.GetAllAsync(filter, connection))
                {
                    _statistics.Add(k);
                }
            }
            finally
            {
            }
        }
        /// <summary>
        /// Retrieve all child objects associated with the match.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        public override void GetChildren(int connection = 0)
        {
            _moves.Clear();
            MatchMove mov = _repository.CreateObject(typeof(MatchMove)) as MatchMove;
            ISQLUIQuery query = mov.ObjectQuery(connection);
            ISQLElementProvider esql = query.Parser.Provider;
            SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
            expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
            expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
            DataFilter filter = new DataFilter();
            filter.Query = query;
            filter.WFilter = new UIFilterExpression();
            filter.WFilter.SetElement(expr);
            filter.OExpr = new UIOrderByExpression();
            filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "move_order"));
            try
            {
                foreach (MatchMove mm in mov.GetAll(filter, connection))
                {
                    _moves.Add(mm);
                }
                FullMoveCount = _moves[_moves.Count - 1].MoveNumber;
                for (int ix = 0; ix < _moves.Count; ix++)
                {
                    if (_moves[ix].HasComments)
                    {
                        _moves[ix].GetComments(connection);
                    }
                }
            }
            finally
            {
            }
            _keywords.Clear();
            try
            {
                MatchKeyword mk = _repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                query = mk.ObjectQuery(connection);
                esql = query.Parser.Provider;
                expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
                filter = new DataFilter();
                filter.Query = query;
                filter.WFilter = new UIFilterExpression();
                filter.WFilter.SetElement(expr);
                filter.OExpr = new UIOrderByExpression();
                filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "keyword"));
                foreach (MatchKeyword k in mk.GetAll(filter, connection))
                {
                    _keywords.Add(k);
                }
            }
            finally
            {
            }
            _statistics.Clear();
            try
            {
                MatchStatistic ms = _repository.CreateObject(typeof(MatchStatistic)) as MatchStatistic;
                query = ms.ObjectQuery(connection);
                esql = query.Parser.Provider;
                expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
                filter = new DataFilter();
                filter.Query = query;
                filter.WFilter = new UIFilterExpression();
                filter.WFilter.SetElement(expr);
                filter.OExpr = new UIOrderByExpression();
                filter.OExpr.SetElement(query.QueryColumns.Find(q => q.Name == "keyword"));
                foreach (MatchStatistic k in ms.GetAll(filter, connection))
                {
                    _statistics.Add(k);
                }
            }
            finally
            {
            }
        }
        /// <summary>
        /// Auxiliary method to split text into lines of a specified size.
        /// </summary>
        /// <param name="txt">
        /// Text to split into lines.
        /// </param>
        /// <param name="lsize">
        /// Line size, maximum number of characters per line.
        /// </param>
        /// <returns>
        /// Text split into lines, each line not exceeding the specified size.
        /// </returns>
        /// <see cref="GetPGN(bool)"/>
        private string SplitText(string txt, int lsize)
        {
            string result = "";
            string sep = "";
            while (txt.Length > lsize)
            {
                int n = lsize - 1;
                while ((txt[n] != ' ') && (n < txt.Length - 1))
                {
                    n++;
                }
                while ((txt[n] == ' ') && (n < txt.Length - 1))
                {
                    n++;
                }
                result += sep + txt.Substring(0, n);
                txt = txt.Substring(n);
                sep = "\n";
            }
            result += sep + txt;
            return result;
        }
        /// <summary>
        /// Update match statistics based on the last move made in the match.
        /// </summary>
        /// <param name="move">
        /// MatchMove object representing the last move made in the match.
        /// </param>
        /// <param name="inc">
        /// Increment value to add to the statistics.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        private void UpdateStatistics(MatchMove move, int inc, int connection = 0)
        {
            MatchStatistic mst = GetStatistic(MatchStatistic.cTotalCheckCount, true);
            MatchStatistic ms = GetStatistic(MatchStatistic.cBlackCheckCount, true);
            if ((move.Player != 0) && ((move.Events & (uint)MatchEvent.Event.Check) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            AddChild(ms, connection);
            ms = GetStatistic(MatchStatistic.cWhiteCheckCount, true);
            if ((move.Player == 0) && ((move.Events & (uint)MatchEvent.Event.Check) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            AddChild(ms, connection);
            AddChild(mst, connection);
            mst = GetStatistic(MatchStatistic.cTotalDiscoveredCheckCount, true);
            ms = GetStatistic(MatchStatistic.cBlackDiscoveredCheckCount, true);
            if ((move.Player != 0) && ((move.Events & (uint)MatchEvent.Event.DiscovedCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            AddChild(ms, connection);
            ms = GetStatistic(MatchStatistic.cWhiteDiscoveredCheckCount, true);
            if ((move.Player == 0) && ((move.Events & (uint)MatchEvent.Event.DiscovedCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            AddChild(ms, connection);
            AddChild(mst, connection);
            mst = GetStatistic(MatchStatistic.cTotalMultipleCheckCoutn, true);
            ms = GetStatistic(MatchStatistic.cBlackMultipleCheckCoutn, true);
            if ((move.Player != 0) && ((move.Events & (uint)MatchEvent.Event.MultipleCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            AddChild(ms, connection);
            ms = GetStatistic(MatchStatistic.cWhiteMultipleCheckCoutn, true);
            if ((move.Player == 0) && ((move.Events & (uint)MatchEvent.Event.MultipleCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            AddChild(ms, connection);
            AddChild(mst, connection);
        }
        /// <summary>
        /// Update match statistics asynchronously based on the last move made in the match.
        /// </summary>
        /// <param name="move">
        /// MatchMove object representing the last move made in the match.
        /// </param>
        /// <param name="inc">
        /// Increment value to add to the statistics.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the database operation, default is 0.
        /// </param>
        private async Task UpdateStatisticsAsync(MatchMove move, int inc, int connection = 0)
        {
            MatchStatistic mst = GetStatistic(MatchStatistic.cTotalCheckCount, true);
            MatchStatistic ms = GetStatistic(MatchStatistic.cBlackCheckCount, true);
            if ((move.Player != 0) && ((move.Events & (uint)MatchEvent.Event.Check) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            await AddChildAsync(ms, connection);
            ms = GetStatistic(MatchStatistic.cWhiteCheckCount, true);
            if ((move.Player == 0) && ((move.Events & (uint)MatchEvent.Event.Check) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            await AddChildAsync(ms, connection);
            await AddChildAsync(mst, connection);
            mst = GetStatistic(MatchStatistic.cTotalDiscoveredCheckCount, true);
            ms = GetStatistic(MatchStatistic.cBlackDiscoveredCheckCount, true);
            if ((move.Player != 0) && ((move.Events & (uint)MatchEvent.Event.DiscovedCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            await AddChildAsync(ms, connection);
            ms = GetStatistic(MatchStatistic.cWhiteDiscoveredCheckCount, true);
            if ((move.Player == 0) && ((move.Events & (uint)MatchEvent.Event.DiscovedCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            await AddChildAsync(ms, connection);
            await AddChildAsync(mst, connection);
            mst = GetStatistic(MatchStatistic.cTotalMultipleCheckCoutn, true);
            ms = GetStatistic(MatchStatistic.cBlackMultipleCheckCoutn, true);
            if ((move.Player != 0) && ((move.Events & (uint)MatchEvent.Event.MultipleCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            await AddChildAsync(ms, connection);
            ms = GetStatistic(MatchStatistic.cWhiteMultipleCheckCoutn, true);
            if ((move.Player == 0) && ((move.Events & (uint)MatchEvent.Event.MultipleCheck) != 0))
            {
                ms.Value += inc;
                mst.Value += inc;
            }
            await AddChildAsync(ms, connection);
            await AddChildAsync(mst, connection);
        }
        public override string ToString()
        {
            return Description;
        }
        /// <summary>
        /// Generate a list of SQL parameters for bulk operations.
        /// </summary>
        /// <param name="bsize">
        /// Block size, how many matches to process at once.
        /// </param>
        /// <param name="bindex">
        /// Block index, which block of matches to process.
        /// </param>
        /// <returns>
        /// SQLParameterList containing parameters for bulk operations.
        /// </returns>
        protected SQLParameterList BulkParameters(int bsize, int bindex)
        {
            ISQLElementProvider esql = _repository.Provider.GetObjects(nameof(ISQLElementProvider)).First().Implementation() as ISQLElementProvider;
            SQLParameterList pl = new SQLParameterList();
            SQLParameter psize = esql.SQLElement(typeof(SQLParameter), new[] { _parameterPrefix }) as SQLParameter;
            psize.Name = "bsize";
            psize.Position = 1;
            psize.NativeType = esql.SQLElement(typeof(SQLDataType)) as SQLDataType;
            psize.NativeType.SetType(typeof(int));
            psize.DefaultValue = bsize;
            pl.Add(psize);
            SQLParameter pindex = esql.SQLElement(typeof(SQLParameter), new[] { _parameterPrefix }) as SQLParameter;
            psize.Name = "bindex";
            psize.Position = 2;
            psize.NativeType = esql.SQLElement(typeof(SQLDataType)) as SQLDataType;
            psize.NativeType.SetType(typeof(int));
            psize.DefaultValue = bindex;
            pl.Add(pindex);
            return pl;
        }
    }
}

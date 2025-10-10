using AIChessDatabase.Controls;
using AIChessDatabase.Interfaces;
using BaseClassesAndInterfaces.Application;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;
using static Resources.Properties.Resources;

namespace AIChessDatabase.Query
{
    /// <summary>
    /// Main query class for retrieving match information.
    /// </summary>
    public class ChessDBQuery
    {
        private IObjectRepository _repository = null;
        public ChessDBQuery()
        {
        }
        /// <summary>
        /// Connection index to the database.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// Database repository for executing queries and retrieving data.
        /// </summary>
        public IObjectRepository Repository
        {
            get
            {
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
        /// <summary>
        /// Match primary key filter. When set to 0, all matches are retrieved.
        /// </summary>
        public ulong MatchFilter { get; set; }
        /// <summary>
        /// Query category: mattches by keywords, matches by moves, or matches by positions.
        /// </summary>
        public string QueryType { get; private set; }
        /// <summary>
        /// Compiled query object with advanced filtering options for the current query.
        /// </summary>
        public ISQLUIQuery Query { get; private set; }
        /// <summary>
        /// Build a master query for matches and a detail query for moves.
        /// </summary>
        /// <param name="result">
        /// Result filter for matches.
        /// </param>
        /// <param name="nmoves">
        /// NUmber of moves to retrieve for each match.
        /// </param>
        /// <param name="board">
        /// Position board string to filter matches.
        /// </param>
        /// <param name="colmanager">
        /// Grid column manager to configure the query columns.
        /// </param>
        /// <returns>
        /// MasterDetailQuery object containing the master query and detail queries.
        /// </returns>
        public MasterDetailQuery BuildPositionQuery(string result, int nmoves, string board, IQueryGridColumnManager colmanager = null)
        {
            string tokenset = "";
            string token = "";
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.Connector = _repository.Connector;
            parser.ConnectionIndex = ConnectionIndex;
            string masterquery = $"select distinct cod_match,position_order mn0,match_description,match_date,white,black,result_text,move_count,fullmove_count from VW_MATCH_POSITIONS where board = {parser.ParameterPrefix}board";
            ISQLUIQuery mquery = parser.Parse(ref token, ref tokenset, ref masterquery) as ISQLUIQuery;
            mquery.Parameters[0].DefaultValue = board;
            string detailquery = $"select to_board,move_an_text,move_player,move_from,move_to from VW_MATCH_MOVES where cod_match = {parser.ParameterPrefix}cod_match and move_order between {parser.ParameterPrefix}position_order and {parser.ParameterPrefix}position_order + {nmoves - 1} order by move_number,move_player";
            ISQLUIQuery dquery = parser.Parse(ref token, ref tokenset, ref detailquery) as ISQLUIQuery;
            foreach (QueryColumn col in mquery.QueryColumns)
            {
                col.ColumnUIConfig = new GridColumnConfiguration()
                {
                    ColumnType = GridColumnType.Text
                };
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_SELECT, col, true);
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_ORDERBY, col, true);
                switch (col.ColumnID)
                {
                    case 1:
                        col.ColumnUIConfig.Caption = "Id";
                        col.ColumnUIConfig.Visible = false;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 2:
                        col.ColumnUIConfig.Caption = "mn0";
                        col.ColumnUIConfig.Visible = false;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 3:
                        col.ColumnUIConfig.Caption = COL_MATCH;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 4:
                        col.ColumnUIConfig.Caption = COL_DATE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 5:
                        col.ColumnUIConfig.Caption = COL_WHITE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 6:
                        col.ColumnUIConfig.Caption = COL_BLACK;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 7:
                        col.ColumnUIConfig.Caption = COL_RESULT;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    default:
                        col.ColumnUIConfig.Hidden = true;
                        break;
                }
            }
            MasterDetailQuery mdquery = new MasterDetailQuery()
            {
                MasterQuery = mquery,
                DetailQueries = new List<ISQLUIQuery>() { dquery }
            };
            return mdquery;
        }
        /// <summary>
        /// Configure a QueryColumn object to show it properly.
        /// </summary>
        /// <param name="mquery">
        /// Query containing the column.
        /// </param>
        /// <param name="kcols">
        /// List of columns to add the configured column to.
        /// </param>
        /// <param name="name">
        /// Column name to configure.
        /// </param>
        /// <param name="caption">
        /// Grid caption for the column.
        /// </param>
        /// <returns>
        /// Configured QueryColumn object.
        /// </returns>
        private QueryColumn ConfigureColumn(ISQLUIQuery mquery, List<QueryColumn> kcols, string name, string caption)
        {
            QueryColumn col = mquery.GetFieldByName(name);
            if (col == null)
            {
                foreach (QueryTable table in mquery.AllQueryTables)
                {
                    col = table.GetQueryColumnByName(null, name);
                    if (col != null)
                    {
                        break;
                    }
                }
            }
            col.ColumnUIConfig = new GridColumnConfiguration()
            {
                ColumnType = GridColumnType.Text,
                Caption = caption,
                ReadOnly = true
            };
            ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_SELECT, col, true);
            kcols.Add(col);
            return col;
        }
        /// <summary>
        /// Build a query based on the keywords view.
        /// </summary>
        /// <param name="colmanager">
        /// Grid column manager to configure the query columns.
        /// </param>
        /// <returns>
        /// Compiled query object with advanced filtering options for the current query.
        /// </returns>
        public async Task<ISQLUIQuery> BuildKeywordsQuery(IQueryGridColumnManager colmanager = null)
        {
            string tokenset = "";
            string token = "";
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.Connector = _repository.Connector;
            parser.ConnectionIndex = ConnectionIndex;
            string query = "select distinct m.cod_match,m.match_description,m.match_date,m.white,m.black,m.result_text,m.move_count,m.fullmove_count from VW_MATCH_KEYWORDS m" +
                    (MatchFilter > 0 ? $" where m.cod_match={MatchFilter}" : "");
            ISQLUIQuery mquery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            foreach (QueryColumn col in mquery.QueryColumns)
            {
                col.ColumnUIConfig = new GridColumnConfiguration()
                {
                    ColumnType = GridColumnType.Text
                };
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_SELECT, col, true);
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_ORDERBY, col, true);
                switch (col.ColumnID)
                {
                    case 1:
                        col.ColumnUIConfig.Caption = "Id";
                        col.ColumnUIConfig.Visible = true;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 2:
                        col.ColumnUIConfig.Caption = COL_MATCH;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 3:
                        col.ColumnUIConfig.Caption = COL_DATE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 4:
                        col.ColumnUIConfig.Caption = COL_WHITE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 5:
                        col.ColumnUIConfig.Caption = COL_BLACK;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 6:
                        col.ColumnUIConfig.Caption = COL_RESULT;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 7:
                        col.ColumnUIConfig.Caption = COL_MOVES;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 8:
                        col.ColumnUIConfig.Caption = COL_FMOVES;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    default:
                        col.ColumnUIConfig.Hidden = true;
                        break;
                }
            }
            if (colmanager != null)
            {
                token = "";
                tokenset = "";
                string sql = $"select keyword from keywords where keyword_type = {parser.ParameterPrefix}keyword_type";
                ISQLUIQuery kwquery = parser.Parse(ref token, ref tokenset, ref sql) as ISQLUIQuery;
                QueryTable view = mquery.AllQueryTables[0];
                List<QueryColumn> kcols = new List<QueryColumn>();
                QueryColumn qcol = ConfigureColumn(mquery, kcols, "keyword", COL_KEYWORD);
                qcol.ColumnUIConfig.ColumnType = GridColumnType.Enum;
                kwquery.Parameters[0].DefaultValue = "MKW";
                DataTable dt = await _repository.Connector.ExecuteTableAsync(kwquery, null, null, ConnectionIndex);
                colmanager.AddColumnValueProvider(qcol.UID, new KeywordNameProvider(KeywordList(dt, "keyword")));
                ConfigureColumn(mquery, kcols, "kw_value", COL_KEYWORDVALUE);
                colmanager.AddColumnsToCategory(CAT_KEYWORDS, kcols);
                kcols = new List<QueryColumn>();
                qcol = ConfigureColumn(mquery, kcols, "statistic", COL_MATCHSTS);
                kwquery.Parameters[0].DefaultValue = "MST";
                dt = await _repository.Connector.ExecuteTableAsync(kwquery, null, null, ConnectionIndex);
                colmanager.AddColumnValueProvider(qcol.UID, new KeywordNameProvider(KeywordList(dt, "keyword")));
                qcol.ColumnUIConfig.ColumnType = GridColumnType.Enum;
                ConfigureColumn(mquery, kcols, "st_value", COL_MATCHSTSVALUE);
                colmanager.AddColumnsToCategory(CAT_STATISTICS, kcols);
                colmanager.BuildUI();
            }
            QueryType = CAT_KEYWORDS;
            Query = mquery;
            return mquery;
        }
        /// <summary>
        /// Retrieve a list of keywords from a DataTable.
        /// </summary>
        /// <param name="dt">
        /// DataTable containing the keywords.
        /// </param>
        /// <param name="fname">
        /// Keyword field name to retrieve from the DataTable.
        /// </param>
        /// <returns>
        /// List of keyword names.
        /// </returns>
        private List<string> KeywordList(DataTable dt, string fname)
        {
            List<string> keywords = new List<string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string keyword = row[fname].ToString();
                    keywords.Add(keyword);
                }
            }
            return keywords;
        }
        /// <summary>
        /// Build a query to retrieve match moves based on the moves view.
        /// </summary>
        /// <param name="colmanager">
        /// Grid column manager to configure the query columns.
        /// </param>
        /// <returns>
        /// Compiled query object with advanced filtering options for the current query.
        /// </returns>
        public ISQLUIQuery BuildMoveQuery(IQueryGridColumnManager colmanager = null)
        {
            string tokenset = "";
            string token = "";
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.Connector = _repository.Connector;
            parser.ConnectionIndex = ConnectionIndex;
            string query = "select distinct m.cod_match,m.match_description,m.match_date,m.white,m.black,m.result_text,m.move_count,m.fullmove_count from VW_MATCH_MOVES m" +
                (MatchFilter > 0 ? $" where m.cod_match={MatchFilter}" : "");
            ISQLUIQuery mquery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            string mmquery = $"select m.move_order from VW_MATCH_MOVES m where m.cod_match = {parser.ParameterPrefix}cod_match";
            token = "";
            tokenset = "";
            ISQLUIQuery squery = parser.Parse(ref token, ref tokenset, ref mmquery) as ISQLUIQuery;
            QueryLink link = new QueryLink()
            {
                DetailQuery = squery
            };
            mquery.AddLink(link);
            foreach (QueryColumn col in mquery.QueryColumns)
            {
                col.ColumnUIConfig = new GridColumnConfiguration()
                {
                    ColumnType = GridColumnType.Text
                };
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_SELECT, col, true);
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_ORDERBY, col, true);
                switch (col.ColumnID)
                {
                    case 1:
                        col.ColumnUIConfig.Caption = "Id";
                        col.ColumnUIConfig.Visible = true;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 2:
                        col.ColumnUIConfig.Caption = COL_MATCH;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 3:
                        col.ColumnUIConfig.Caption = COL_DATE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 4:
                        col.ColumnUIConfig.Caption = COL_WHITE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 5:
                        col.ColumnUIConfig.Caption = COL_BLACK;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 6:
                        col.ColumnUIConfig.Caption = COL_RESULT;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 7:
                        col.ColumnUIConfig.Caption = COL_MOVES;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 8:
                        col.ColumnUIConfig.Caption = COL_FMOVES;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    default:
                        col.ColumnUIConfig.Hidden = true;
                        break;
                }
            }
            if (colmanager != null)
            {
                QueryTable view = mquery.AllQueryTables[0];
                List<QueryColumn> kcols = new List<QueryColumn>();
                QueryColumn coled = ConfigureColumn(mquery, kcols, "initial_board", COL_INITIALBOARD);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new PositionFilter());
                ConfigureColumn(mquery, kcols, "initial_black_pawns", COL_INITAIALBPAWNS);
                ConfigureColumn(mquery, kcols, "initial_white_pawns", COL_initial_white_pawns);
                ConfigureColumn(mquery, kcols, "initial_black_rooks", COL_initial_black_rooks);
                ConfigureColumn(mquery, kcols, "initial_white_rooks", COL_initial_white_rooks);
                ConfigureColumn(mquery, kcols, "initial_black_bishops", COL_initial_black_bishops);
                ConfigureColumn(mquery, kcols, "initial_white_bishops", COL_initial_white_bishops);
                ConfigureColumn(mquery, kcols, "initial_black_knights", COL_initial_black_knights);
                ConfigureColumn(mquery, kcols, "initial_white_knights", COL_initial_white_knights);
                ConfigureColumn(mquery, kcols, "initial_black_queens", COL_initial_black_queens);
                ConfigureColumn(mquery, kcols, "initial_white_queens", COL_initial_white_queens);
                colmanager.AddColumnsToCategory(CAT_INITIALPOS, kcols);
                kcols = new List<QueryColumn>();
                ConfigureColumn(mquery, kcols, "MOVE_ORDER", COL_MOVE_ORDER);
                ConfigureColumn(mquery, kcols, "MOVE_NUMBER", COL_MOVE_NUMBER);
                coled = ConfigureColumn(mquery, kcols, "MOVE_PLAYER", COL_MOVE_PLAYER);
                coled.ColumnUIConfig.ColumnType = GridColumnType.Enum;
                colmanager.AddColumnValueProvider(coled.UID, new PlayerValueProvider());
                coled = ConfigureColumn(mquery, kcols, "MOVE_EVENTS", COL_MOVE_EVENTS);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new EventsFilter());
                ConfigureColumn(mquery, kcols, "MOVE_AN_TEXT", COL_MOVE_AN_TEXT);
                ConfigureColumn(mquery, kcols, "COMMENTS", COL_COMMENTS);
                ConfigureColumn(mquery, kcols, "SCORE", COL_SCORE);
                colmanager.AddColumnsToCategory(CAT_GENMOVE, kcols);
                kcols = new List<QueryColumn>();
                coled = ConfigureColumn(mquery, kcols, "MOVE_FROM", COL_MOVE_FROM);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new SquareFilter());
                coled = ConfigureColumn(mquery, kcols, "from_board", COL_from_board);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new PositionFilter());
                ConfigureColumn(mquery, kcols, "from_black_pawns", COL_from_black_pawns);
                ConfigureColumn(mquery, kcols, "from_white_pawns", COL_from_white_pawns);
                ConfigureColumn(mquery, kcols, "from_black_rooks", COL_from_black_rooks);
                ConfigureColumn(mquery, kcols, "from_white_rooks", COL_from_white_rooks);
                ConfigureColumn(mquery, kcols, "from_black_bishops", COL_from_black_bishops);
                ConfigureColumn(mquery, kcols, "from_white_bishops", COL_from_white_bishops);
                ConfigureColumn(mquery, kcols, "from_black_knights", COL_from_black_knights);
                ConfigureColumn(mquery, kcols, "from_white_knights", COL_from_white_knights);
                ConfigureColumn(mquery, kcols, "from_black_queens", COL_from_black_queens);
                ConfigureColumn(mquery, kcols, "from_white_queens", COL_from_white_queens);
                colmanager.AddColumnsToCategory(CAT_MOVEFROM, kcols);
                kcols = new List<QueryColumn>();
                coled = ConfigureColumn(mquery, kcols, "MOVE_TO", COL_MOVE_TO);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new SquareFilter());
                coled = ConfigureColumn(mquery, kcols, "to_board", COL_to_board);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new PositionFilter());
                ConfigureColumn(mquery, kcols, "to_black_pawns", COL_to_black_pawns);
                ConfigureColumn(mquery, kcols, "to_white_pawns", COL_to_white_pawns);
                ConfigureColumn(mquery, kcols, "to_black_rooks", COL_to_black_rooks);
                ConfigureColumn(mquery, kcols, "to_white_rooks", COL_to_white_rooks);
                ConfigureColumn(mquery, kcols, "to_black_bishops", COL_to_black_bishops);
                ConfigureColumn(mquery, kcols, "to_white_bishops", COL_to_white_bishops);
                ConfigureColumn(mquery, kcols, "to_black_knights", COL_to_black_knights);
                ConfigureColumn(mquery, kcols, "to_white_knights", COL_to_white_knights);
                ConfigureColumn(mquery, kcols, "to_black_queens", COL_to_black_queens);
                ConfigureColumn(mquery, kcols, "to_white_queens", COL_to_white_queens);
                colmanager.AddColumnsToCategory(CAT_MOVETO, kcols);
                colmanager.BuildUI();
            }
            QueryType = CAT_MOVE;
            Query = mquery;
            return mquery;
        }
        /// <summary>
        /// Build a query to retrieve match positions based on the positions view.
        /// </summary>
        /// <param name="colmanager">
        /// Grid column manager to configure the query columns.
        /// </param>
        /// <returns>
        /// Compiled query object with advanced filtering options for the current query.
        /// </returns>
        public ISQLUIQuery BuildPositionQuery(IQueryGridColumnManager colmanager = null)
        {
            string tokenset = "";
            string token = "";
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.Connector = _repository.Connector;
            parser.ConnectionIndex = ConnectionIndex;
            string query = "select distinct m.cod_match,m.match_description,m.match_date,m.white,m.black,m.result_text,m.move_count,m.fullmove_count from VW_MATCH_POSITIONS m" +
                (MatchFilter > 0 ? $" where m.cod_match={MatchFilter}" : "");
            ISQLUIQuery mquery = parser.Parse(ref token, ref tokenset, ref query) as ISQLUIQuery;
            string mmquery = $"select m.position_order from VW_MATCH_POSITIONS m where m.cod_match = {parser.ParameterPrefix}cod_match";
            token = "";
            tokenset = "";
            ISQLUIQuery squery = parser.Parse(ref token, ref tokenset, ref mmquery) as ISQLUIQuery;
            QueryLink link = new QueryLink()
            {
                DetailQuery = squery
            };
            mquery.AddLink(link);
            foreach (QueryColumn col in mquery.QueryColumns)
            {
                col.ColumnUIConfig = new GridColumnConfiguration()
                {
                    ColumnType = GridColumnType.Text
                };
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_SELECT, col, true);
                ((IElementUIProvider)mquery).CreateElementUI(CATEGORY_ORDERBY, col, true);
                switch (col.ColumnID)
                {
                    case 1:
                        col.ColumnUIConfig.Caption = "Id";
                        col.ColumnUIConfig.Visible = true;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 2:
                        col.ColumnUIConfig.Caption = COL_MATCH;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 3:
                        col.ColumnUIConfig.Caption = COL_DATE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 4:
                        col.ColumnUIConfig.Caption = COL_WHITE;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 5:
                        col.ColumnUIConfig.Caption = COL_BLACK;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 6:
                        col.ColumnUIConfig.Caption = COL_RESULT;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 7:
                        col.ColumnUIConfig.Caption = COL_MOVES;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    case 8:
                        col.ColumnUIConfig.Caption = COL_FMOVES;
                        col.ColumnUIConfig.ReadOnly = true;
                        break;
                    default:
                        col.ColumnUIConfig.Hidden = true;
                        break;
                }
            }
            if (colmanager != null)
            {
                QueryTable view = mquery.AllQueryTables[0];
                List<QueryColumn> kcols = new List<QueryColumn>();
                QueryColumn coled = ConfigureColumn(mquery, kcols, "initial_board", COL_INITIALBOARD);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new PositionFilter());
                ConfigureColumn(mquery, kcols, "initial_black_pawns", COL_INITAIALBPAWNS);
                ConfigureColumn(mquery, kcols, "initial_white_pawns", COL_initial_white_pawns);
                ConfigureColumn(mquery, kcols, "initial_black_rooks", COL_initial_black_rooks);
                ConfigureColumn(mquery, kcols, "initial_white_rooks", COL_initial_white_rooks);
                ConfigureColumn(mquery, kcols, "initial_black_bishops", COL_initial_black_bishops);
                ConfigureColumn(mquery, kcols, "initial_white_bishops", COL_initial_white_bishops);
                ConfigureColumn(mquery, kcols, "initial_black_knights", COL_initial_black_knights);
                ConfigureColumn(mquery, kcols, "initial_white_knights", COL_initial_white_knights);
                ConfigureColumn(mquery, kcols, "initial_black_queens", COL_initial_black_queens);
                ConfigureColumn(mquery, kcols, "initial_white_queens", COL_initial_white_queens);
                colmanager.AddColumnsToCategory(CAT_INITIALPOS, kcols);
                kcols = new List<QueryColumn>();
                ConfigureColumn(mquery, kcols, "POSITION_ORDER", COL_POSITION_ORDER);
                coled = ConfigureColumn(mquery, kcols, "POSITION_EVENTS", COL_POSITION_EVENTS);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new EventsFilter());
                ConfigureColumn(mquery, kcols, "SCORE", COL_SCORE);
                coled = ConfigureColumn(mquery, kcols, "board", COL_board);
                colmanager.AddColumnFilterCustomEditor(coled.UID, new PositionFilter());
                ConfigureColumn(mquery, kcols, "black_pawns", COL_black_pawns);
                ConfigureColumn(mquery, kcols, "white_pawns", COL_white_pawns);
                ConfigureColumn(mquery, kcols, "black_rooks", COL_black_rooks);
                ConfigureColumn(mquery, kcols, "white_rooks", COL_white_rooks);
                ConfigureColumn(mquery, kcols, "black_bishops", COL_black_bishops);
                ConfigureColumn(mquery, kcols, "white_bishops", COL_white_bishops);
                ConfigureColumn(mquery, kcols, "black_knights", COL_black_knights);
                ConfigureColumn(mquery, kcols, "white_knights", COL_white_knights);
                ConfigureColumn(mquery, kcols, "black_queens", COL_black_queens);
                ConfigureColumn(mquery, kcols, "white_queens", COL_white_queens);
                colmanager.AddColumnsToCategory(CAT_POSITION, kcols);
                colmanager.BuildUI();
            }
            QueryType = CAT_POSITION;
            Query = mquery;
            return mquery;
        }
    }
}

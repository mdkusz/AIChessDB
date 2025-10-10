using AIChessDatabase.Interfaces;
using AIChessDatabase.Query;
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
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Match statistics POCO data class.
    /// </summary>
    [Serializable]
    [TableName("match_statistics")]
    public class MatchStatistic : ObjectBase
    {
        public const string cWhiteCheckCount = nameof(MST_WCHECKCOUNT);
        public const string cBlackCheckCount = nameof(MST_BCHECKCOUNT);
        public const string cTotalCheckCount = nameof(MST_TCHECKCOUNT);
        public const string cWhiteMultipleCheckCoutn = nameof(MST_WMULTICHECKCOUNT);
        public const string cBlackMultipleCheckCoutn = nameof(MST_BMULTICHECKCOUNT);
        public const string cTotalMultipleCheckCoutn = nameof(MST_TMULTICHECKCOUNT);
        public const string cWhiteDiscoveredCheckCount = nameof(MST_WDISCCHECKCOUNT);
        public const string cBlackDiscoveredCheckCount = nameof(MST_BDISCCHECKCOUNT);
        public const string cTotalDiscoveredCheckCount = nameof(MST_TDISCCHECKCOUNT);

        private const string _querysql = "select ms.*,k.keyword,k.keyword_type from match_statistics ms join keywords k on ms.cod_keyword = k.cod_keyword";
        private const string _querycntsql = "select count(*) cnt from match_statistics ms join keywords k on ms.cod_keyword = k.cod_keyword";

        public MatchStatistic()
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
        }
        public MatchStatistic(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
        }
        public MatchStatistic(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            MatchStatistic st = copy as MatchStatistic;
            if (st != null)
            {
                _querySQL = _querysql;
                _queryCountSQL = _querycntsql;
                Name = st.Name;
                Value = st.Value;
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// Primary Key values. IdKeyword and IdMatch are used to identify the statistic.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdKeyword, IdMatch };
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
            IdKeyword = key[0];
            IdMatch = key[1];
        }
        /// <summary>
        /// Keyword primary key value, used to identify the statistic type.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_keyword")]
        [TableColumnName("cod_keyword")]
        public ulong IdKeyword { get; set; }
        /// <summary>
        /// Match primary key value, used to identify the match this statistic belongs to.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_match")]
        [TableColumnName("cod_match")]
        public ulong IdMatch { get; set; }
        /// <summary>
        /// Statistic keyword name, used to identify the statistic type.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_keyword), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_statistic), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("keyword")]
        [TableColumnName("keyword")]
        public string Name { get; set; }
        /// <summary>
        /// Statistic value, used to store the value of the statistic.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_value), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_statistic_value), typeof(Properties.UIResources))]
        [JsonPropertyName("st_value")]
        [TableColumnName("st_value")]
        public double Value { get; set; }
        /// <summary>
        /// Retrieve the data from the database using the primary key values.
        /// </summary>
        /// <param name="key">
        /// Primary key values, which are IdKeyword and IdMatch.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, default is 0.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"ms.cod_keyword = {_parameterPrefix}cod_keyword and ms.cod_match = {_parameterPrefix}cod_match";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        /// Retrieve the data from the database using the primary key values.
        /// </summary>
        /// <param name="key">
        /// Primary key values, which are IdKeyword and IdMatch.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, default is 0.
        /// </param>
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"ms.cod_keyword = {_parameterPrefix}cod_keyword and ms.cod_match = {_parameterPrefix}cod_match";
            InternalGet(key, text, connection);
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
            IdKeyword = Convert.ToUInt64(row["cod_keyword"]);
            Name = row["keyword"].ToString();
            Value = Convert.ToDouble(row["st_value"]);
        }
        /// <summary>
        /// Translate a keyword name to its localized version.
        /// </summary>
        /// <param name="name">
        /// Original keyword name to translate. It must be defined in the UIResources.resx file.
        /// </param>
        /// <returns>
        /// Translated string if found, otherwise returns the original name.
        /// </returns>
        public static string Translate(string name)
        {
            string tr = Properties.UIResources.ResourceManager.GetString(name);
            return string.IsNullOrEmpty(tr) ? name : tr;
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
            if (IdKeyword == 0)
            {
                Keyword kw = Repository.CreateObject(typeof(Keyword)) as Keyword;
                ISQLUIQuery kquery = kw.ObjectQuery(connection);
                ISQLElementProvider esql = kquery.Parser.Provider;
                DataFilter filter = new DataFilter()
                {
                    Query = kquery
                };
                filter.WFilter = new UIFilterExpression();
                SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(kquery.QueryColumns.Find(q => q.Name == "keyword"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { Name }));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "and" }));
                expr.Elements.Add(kquery.QueryColumns.Find(q => q.Name == "keyword_type"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { TYPE_MATCHSTATISTIC }));
                filter.WFilter.SetElement(expr);
                List<ObjectBase> lkw = await kw.GetAllAsync(filter, connection);
                if (lkw.Count > 0)
                {
                    kw = lkw[0] as Keyword;
                }
                else
                {
                    kw.Name = Name;
                    kw.KeywordType = TYPE_MATCHSTATISTIC;
                    await kw.Insert(connection);
                }
                IdKeyword = kw.IdKeyword;
            }
            else
            {
                if (query == null)
                {
                    query = GetQuery(connection);
                }
                ISQLElementProvider esql = query.Parser.Provider;
                // If already defined, update value
                DataFilter filter = new DataFilter();
                filter.WFilter = new UIFilterExpression();
                SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_keyword"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdKeyword.ToString() }));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "and" }));
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_match"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdMatch.ToString() }));
                filter.WFilter.SetElement(expr);
                ulong count = await GetCount(filter, connection);
                if (count > 0)
                {
                    return await LoopUpdate(connection, query);
                }
            }
            return await base.LoopInsert(connection, query);
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
            query = await base.LoopDelete(connection, query);
            Keyword kw = Repository.CreateObject(typeof(Keyword)) as Keyword;
            await kw.GetAsync(new ulong[] { IdKeyword });
            await kw.Delete(connection);
            return query;
        }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(ObjectBase other)
        {
            MatchStatistic ok = other as MatchStatistic;
            if (ok == null)
            {
                return false;
            }
            return (ok.Name == Name) && (ok.IdMatch == IdMatch);
        }
        public int CompareTo(MatchStatistic other)
        {
            if (IdMatch == other.IdMatch)
            {
                return Name.CompareTo(other.Name);
            }
            return IdMatch.CompareTo(other.IdMatch);
        }
    }
}

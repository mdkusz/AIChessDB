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
    /// Position statistic POCO class.
    /// </summary>
    [Serializable]
    [TableName("position_statistics")]
    public class PositionStatistic : ObjectBase
    {
        private const string _querysql = "select ps.*,k.keyword,k.keyword_type from position_statistics ps join keywords k on ps.cod_keyword = k.cod_keyword";
        private const string _querycntsql = "select count(*) cnt from position_statistics ps join keywords k on ps.cod_keyword = k.cod_keyword";
        public PositionStatistic()
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
        }
        public PositionStatistic(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _querysql;
            _queryCountSQL = _querycntsql;
        }
        public PositionStatistic(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            PositionStatistic st = copy as PositionStatistic;
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
        /// Primary key values for the PositionStatistic object. Keyword and Position primary keys.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdKeyword, IdPosition };
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
            IdPosition = key[1];
        }
        /// <summary>
        /// Keyword primary key value.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_keyword")]
        [TableColumnName("cod_keyword")]
        public ulong IdKeyword { get; set; }
        /// <summary>
        /// Position primary key value.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_position")]
        [TableColumnName("cod_position")]
        public ulong IdPosition { get; set; }
        /// <summary>
        /// Statistic keyword name, used to identify the type of statistic.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_keyword), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_statistic), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("keyword")]
        [TableColumnName("keyword")]
        public string Name { get; set; }
        /// <summary>
        /// Statistic value.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_value), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_statistic_value), typeof(Properties.UIResources))]
        [JsonPropertyName("st_value")]
        [TableColumnName("st_value")]
        public double Value { get; set; }
        /// <summary>
        ///  Retrieves the object data from database by primary key values.
        /// </summary>
        /// <param name="key">
        /// Primary key values for the PositionStatistic object, which are Keyword and Position primary keys.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. If not specified, the default connection is used.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"ps.cod_keyword = {_parameterPrefix}cod_keyword and ps.cod_position = {_parameterPrefix}cod_position";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        ///  Retrieves the object data from database by primary key values.
        /// </summary>
        /// <param name="key">
        /// Primary key values for the PositionStatistic object, which are Keyword and Position primary keys.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. If not specified, the default connection is used.
        /// </param>
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"ps.cod_keyword = {_parameterPrefix}cod_keyword and ps.cod_position = {_parameterPrefix}cod_position";
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
            IdPosition = Convert.ToUInt64(row["cod_position"]);
            IdKeyword = Convert.ToUInt64(row["cod_keyword"]);
            Name = row["keyword"].ToString();
            Value = Convert.ToDouble(row["st_value"]);
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
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { TYPE_POSSTATISTIC }));
                filter.WFilter.SetElement(expr);
                List<ObjectBase> lkw = await kw.GetAllAsync(filter, connection);
                if (lkw.Count > 0)
                {
                    kw = lkw[0] as Keyword;
                }
                else
                {
                    kw.Name = Name;
                    kw.KeywordType = TYPE_POSSTATISTIC;
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
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "cod_position"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralNumber), new object[] { IdPosition.ToString() }));
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
        /// <summary>
        /// Translate the statistic name using the UI resources.
        /// </summary>
        /// <param name="name">
        /// Statistic name to translate.
        /// </param>
        /// <returns>
        /// Translated statistic name if found in resources, otherwise returns the original name.
        /// </returns>
        public static string Translate(string name)
        {
            string tr = Properties.UIResources.ResourceManager.GetString(name);
            return string.IsNullOrEmpty(tr) ? name : tr;
        }
        public override string ToString()
        {
            return Name;
        }
        public int CompareTo(PositionStatistic other)
        {
            int cname = Name.CompareTo(other.Name);
            if (cname == 0)
            {
                return Value.CompareTo(other.Value);
            }
            return cname;
        }
    }
}

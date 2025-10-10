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
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// MatchKeyword POCO represents a keyword used in a match and its value.
    /// </summary>
    [Serializable]
    [TableName("match_keywords")]
    public class MatchKeyword : ObjectBase, IComparable<MatchKeyword>
    {
        private const string _mksql = "select mk.*,k.keyword,k.keyword_type,kv.kw_value from match_keywords mk join keywords k on mk.cod_keyword = k.cod_keyword join keyword_values kv on kv.cod_value = mk.cod_value";
        private const string _mkcntsql = "select count(*) cnt from match_keywords mk join keywords k on mk.cod_keyword = k.cod_keyword join keyword_values kv on kv.cod_value = mk.cod_value";
        private const string _mksqlvalues = "select distinct mk.cod_keyword,k.keyword,k.keyword_type,kv.kw_value from match_keywords mk join keywords k on mk.cod_keyword = k.cod_keyword join keyword_values kv on kv.cod_value = mk.cod_value";

        public MatchKeyword()
        {
            _querySQL = _mksql;
            _queryCountSQL = _mkcntsql;
        }
        public MatchKeyword(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _mksql;
            _queryCountSQL = _mkcntsql;
        }
        public MatchKeyword(IObjectRepository rep, ObjectBase copy)
            : base(rep)
        {
            MatchKeyword mk = copy as MatchKeyword;
            if (mk != null)
            {
                _querySQL = _mksql;
                _queryCountSQL = _mkcntsql;
                Name = mk.Name;
                Value = mk.Value;
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// MatchKeyword primary key value. Three values are used to uniquely identify a match keyword:
        /// Value primery key (IdValue), keyword primary key (IdKeyword), and match primary key (IdMatch).
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdValue, IdKeyword, IdMatch };
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
            IdValue = Key[0];
            IdKeyword = key[1];
            IdMatch = key[2];
        }
        /// <summary>
        /// Value table Primary key value.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_value")]
        [TableColumnName("cod_value")]
        public ulong IdValue { get; set; }
        /// <summary>
        /// Keyword table Primary key value.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_keyword")]
        [TableColumnName("cod_keyword")]
        public ulong IdKeyword { get; set; }
        /// <summary>
        /// Match table Primary key value.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_match")]
        [TableColumnName("cod_match")]
        public ulong IdMatch { get; set; }
        /// <summary>
        /// Keyword name.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_keyword), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_keyword), typeof(Properties.UIResources))]
        [JsonPropertyName("keyword")]
        [TableColumnName("keyword")]
        public string Name { get; set; }
        /// <summary>
        /// Keyword value.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_value), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_keyword_value), typeof(Properties.UIResources))]
        [JsonPropertyName("kw_value")]
        [TableColumnName("kw_value")]
        public string Value { get; set; }
        /// <summary>
        /// Get the object values from its primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key value. It is an array with three values: Value primary key (IdValue), keyword primary key (IdKeyword), and match primary key (IdMatch).
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. Default is 0.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"mk.cod_value = {_parameterPrefix}cod_value and mk.cod_keyword = {_parameterPrefix}cod_keyword and mk.cod_match = {_parameterPrefix}cod_match";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        /// Get the object values from its primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key value. It is an array with three values: Value primary key (IdValue), keyword primary key (IdKeyword), and match primary key (IdMatch).
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. Default is 0.
        /// </param>
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"mk.cod_value = {_parameterPrefix}cod_value and mk.cod_keyword = {_parameterPrefix}cod_keyword and mk.cod_match = {_parameterPrefix}cod_match";
            InternalGet(key, text, connection);
        }
        /// <summary>
        /// Use a DataRow to fill the object properties.
        /// </summary>
        /// <param name="row">
        /// DataRow to use to fill the object properties. The row must have been executed with a query that returns the object properties.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public override void Get(DataRow row)
        {
            IdMatch = Convert.ToUInt64(row["cod_match"]);
            IdKeyword = Convert.ToUInt64(row["cod_keyword"]);
            IdValue = Convert.ToUInt64(row["cod_value"]);
            Name = row["keyword"].ToString();
            Value = row["kw_value"].ToString();
        }
        /// <summary>
        /// Get a quiery to retrieve the distinct values for the keywords.
        /// </summary>
        /// <param name="connection">
        /// Database connection index to use for the query. Default is 0.
        /// </param>
        /// <param name="clone">
        /// Return a cloned query object. Default is true.
        /// </param>
        /// <returns>
        /// QUery object to retrieve the distinct keyword values and apply filters.
        /// </returns>
        public ISQLUIQuery ValuesQuery(int connection = 0, bool clone = true)
        {
            string name = GetType().Name + ".Value";
            ISQLUIQuery query = Repository.GetQueryForType(name, clone);
            if (query == null)
            {
                string sql = _mksqlvalues;
                query = Repository.SetQueryForType(name, sql, connection);
                if (clone)
                {
                    query = Repository.GetQueryForType(name, true);
                    query.Parser.ConnectionIndex = connection;
                }
            }
            return query;
        }
        /// <summary>
        /// Get all the different values for a keyword in the database.
        /// </summary>
        /// <param name="filter">
        /// Filder conditions to apply to the query. It can include where, having, order by clauses, and set quantifier.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. Default is 0.
        /// </param>
        /// <returns>
        /// List of keyword values as string.
        /// </returns>
        public async Task<List<string>> GetAllValues(DataFilter filter, int connection = 0)
        {
            ISQLUIQuery query = GetQuery(connection);
            query.UserWhereFilters = null;
            query.UserHavingFilters = null;
            query.UserOrderBy = null;
            if (filter != null)
            {
                if (filter.WFilter != null)
                {
                    query.AddFilter(filter.WFilter);
                }
                if (filter.HFilter != null)
                {
                    query.AddFilter(filter.HFilter);
                }
                if (filter.OExpr != null)
                {
                    query.AddOrder(filter.OExpr);
                }
                if (filter.SetQ != null)
                {
                    query.SetQuantifier = filter.SetQ;
                }
            }
            List<string> list = new List<string>();
            using (DataTable dt = await _repository.Connector.ExecuteTableAsync(query, null, null, connection))
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(row["kw_value"].ToString());
                    }
                }
            }
            return list;
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
                Keyword kw = _repository.CreateObject(typeof(Keyword)) as Keyword;
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
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { TYPE_MATCHKEYWORD }));
                filter.WFilter.SetElement(expr);
                List<ObjectBase> lkw = await kw.GetAllAsync(filter, connection);
                if (lkw.Count > 0)
                {
                    kw = lkw[0] as Keyword;
                    IdKeyword = kw.IdKeyword;
                }
                else
                {
                    kw.Name = Name;
                    kw.KeywordType = TYPE_MATCHKEYWORD;
                    await kw.Insert(connection);
                    IdKeyword = kw.IdKeyword;
                }
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
            // Find a value for the same keyword with identical text
            string sql = $"select kv.cod_value from keyword_values kv join match_keywords mk on kv.cod_value = mk.cod_value where mk.cod_keyword = {_parameterPrefix}cod_keyword and kv.kw_value " +
                (string.IsNullOrEmpty(Value) ? $"is null or kv.kw_value = {_parameterPrefix}kw_value" : $"= {_parameterPrefix}kw_value");
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.ConnectionIndex = connection;
            parser.Connector = _repository.Connector;
            string token = "";
            string tokenset = "";
            ISQLUIQuery qvalue = parser.Parse(ref token, ref tokenset, ref sql) as ISQLUIQuery;
            qvalue.Parameters[0].DefaultValue = IdKeyword;
            qvalue.Parameters[1].DefaultValue = Value;
            object oval = await _repository.Connector.ExecuteScalarAsync(qvalue, connection);
            if (oval != null)
            {
                IdValue = Convert.ToUInt64(oval);
            }
            else
            {
                // If not found, then create new
                Table bdtable = qvalue.GetTable("keyword_values") as Table;
                List<string> dml = bdtable.DefaultDMLSentence(UpdateMode.Insert, false);
                ((IUpdateManager)qvalue).ParseUpdateCommand(bdtable.FullName, dml, UpdateMode.Insert);
                DMLCommand cmd = bdtable.InsertCommand;
                foreach (SQLParameter p in cmd.Parameters)
                {
                    p.DefaultValue = GetFieldValue(p.Name);
                }
                await cmd.ExecuteAsync(_repository.Connector, connection);
                IdValue = await cmd.AutogeneratedKey(_repository.Connector, connection);
            }
            // Finally, insert match keyword
            return await base.LoopInsert(connection, query);
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
            if (query == null)
            {
                query = GetQuery(connection);
            }
            // Check whether value changes
            string sql = $"select count(*) from keyword_values kv join match_keywords mk on kv.cod_value = mk.cod_value where mk.cod_keyword = {_parameterPrefix}cod_keyword and kv.kw_value " +
                (string.IsNullOrEmpty(Value) ? $"is null or kv.kw_value = {_parameterPrefix}kw_value" : $"= {_parameterPrefix}kw_value") +
                $" and mk.cod_match = {_parameterPrefix}cod_match and mk.cod_value = {_parameterPrefix}cod_value";
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.ConnectionIndex = connection;
            parser.Connector = _repository.Connector;
            string token = "";
            string tokenset = "";
            ISQLUIQuery qvalue = parser.Parse(ref token, ref tokenset, ref sql) as ISQLUIQuery;
            qvalue.Parameters[0].DefaultValue = IdKeyword;
            qvalue.Parameters[1].DefaultValue = Value;
            qvalue.Parameters[2].DefaultValue = IdMatch;
            qvalue.Parameters[3].DefaultValue = IdValue;
            if (Convert.ToUInt64(await _repository.Connector.ExecuteScalarAsync(qvalue, connection)) == 0)
            {
                // Is the value used in other matches?
                sql = $"select count(*) from match_keywords where cod_value = {_parameterPrefix}cod_value and cod_keyword = {_parameterPrefix}cod_keyword and cod_match <> {_parameterPrefix}cod_match";
                token = "";
                tokenset = "";
                qvalue = parser.Parse(ref token, ref tokenset, ref sql) as ISQLUIQuery;
                qvalue.Parameters[0].DefaultValue = IdValue;
                qvalue.Parameters[1].DefaultValue = IdKeyword;
                qvalue.Parameters[2].DefaultValue = IdMatch;
                bool bused = Convert.ToUInt64(await _repository.Connector.ExecuteScalarAsync(qvalue, connection)) > 0;
                ulong idoldvalue = IdValue;
                // If value changes, try to find another identical value already defined for the same keyword
                sql = $"select kv.cod_value from keyword_values kv join match_keywords mk on kv.cod_value = mk.cod_value where mk.cod_keyword = {_parameterPrefix}cod_keyword and kv.kw_value " +
                    (string.IsNullOrEmpty(Value) ? $"is null or kv.kw_value = {_parameterPrefix}kw_value" : $"= {_parameterPrefix}kw_value");
                token = "";
                tokenset = "";
                qvalue = parser.Parse(ref token, ref tokenset, ref sql) as ISQLUIQuery;
                qvalue.Parameters[0].DefaultValue = IdKeyword;
                qvalue.Parameters[1].DefaultValue = Value;
                object oval = await _repository.Connector.ExecuteScalarAsync(qvalue, connection);
                if (oval != null)
                {
                    // Update value reference
                    IdValue = Convert.ToUInt64(oval);
                    query = await base.LoopUpdate(connection, query);
                    if (!bused)
                    {
                        // If value not used in other matches, delete it
                        sql = $"delete from keyword_values where cod_value = {idoldvalue}";
                        await _repository.Connector.ExecuteNonQueryAsync(sql, null, connection);
                    }
                }
                else
                {
                    if (!bused)
                    {
                        // If not used in other matches, update the value
                        sql = $"update keyword_values set kw_value = '{Value}' where cod_value = {IdValue}";
                        await _repository.Connector.ExecuteNonQueryAsync(sql, null, connection);
                    }
                    else
                    {
                        // If used in other matches, create new value
                        ICodeBuilder cb = _repository.Provider.GetObjects(nameof(ICodeBuilder)).First().Implementation() as ICodeBuilder;
                        cb.ConnectionIndex = connection;
                        cb.Parser = parser;
                        List<object> pr = cb.CallProcedure("CreateKeywordValue", null, new object[] { Value });
                        IdValue = Convert.ToUInt64(pr[0]);
                        // And update match_keywords
                        return await base.LoopUpdate(connection, query);
                    }
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
            if (query == null)
            {
                query = GetQuery(connection);
            }
            // Is the value used in other matches?
            ISQLParser parser = _repository.Provider.GetObjects(nameof(ISQLParser), DependencyBehaviourModifier.NoSingleton).First().Implementation() as ISQLParser;
            parser.ConnectionIndex = connection;
            parser.Connector = _repository.Connector;
            string token = "";
            string tokenset = "";
            string sql = $"select count(*) from match_keywords where cod_value = {_parameterPrefix}cod_value and cod_keyword = {_parameterPrefix}cod_keyword and cod_match <> {_parameterPrefix}cod_match";
            ISQLUIQuery qvalue = parser.Parse(ref token, ref tokenset, ref sql) as ISQLUIQuery;
            qvalue.Parameters[0].DefaultValue = IdValue;
            qvalue.Parameters[1].DefaultValue = IdKeyword;
            qvalue.Parameters[2].DefaultValue = IdMatch;
            bool bused = Convert.ToUInt64(await _repository.Connector.ExecuteScalarAsync(qvalue, connection)) > 0;
            query = await base.LoopDelete(connection, query);
            if (!bused)
            {
                // If not used in other matches, delete value
                sql = $"update keyword_values set kw_value = '{Value}' where cod_value = {IdValue}";
                await _repository.Connector.ExecuteNonQueryAsync(sql, null, connection);
            }
            return query;
        }
        /// <summary>
        /// Consolidates multiple keyword values into a single value in the database.
        /// </summary>
        /// <remarks>
        /// This method updates the database to consolidate multiple keyword values into a single
        /// value.  The first identifier in <paramref name="oldvalues"/> is used as the primary value, and all other 
        /// identifiers are merged into it. The method also removes the redundant keyword values from the
        /// database.</remarks>
        /// <param name="newvalue">
        /// The new value to assign to the consolidated keyword.
        /// </param>
        /// <param name="oldvalues">
        /// An array of unique identifiers representing the keyword values to be consolidated. 
        /// The first element in the array is treated as the primary identifier to which the other values will be merged.
        /// </param>
        /// <param name="connection">
        /// The database connection identifier to use for the operation. Defaults to 0 if not specified.
        /// </param>
        public virtual async Task Consolidate(string newvalue, List<string> oldvalues, ulong idk, int connection = 0)
        {
            string sql = $"select min(kw.cod_value) cod_value from match_keywords mk join keyword_values kw on mk.cod_value = kw.cod_value where mk.cod_keyword = {idk} and kw.kw_value = '{oldvalues[0]}'";
            ulong oval = Convert.ToUInt64(await _repository.Connector.ExecuteScalarAsync(sql, null, connection));
            sql = $"update keyword_values set kw_value = '{newvalue ?? ""}' where cod_value = {oval}";
            await _repository.Connector.ExecuteNonQueryAsync(sql, null, connection);
            for (int iv = 1; iv < oldvalues.Count; iv++)
            {
                sql = $"select distinct kw.cod_value from match_keywords mk join keyword_values kw on mk.cod_value = kw.cod_value where mk.cod_keyword = {idk} and kw.kw_value = '{oldvalues[0]}'";
                DataTable dt = await _repository.Connector.ExecuteTableAsync(sql, null, null, null, connection);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ulong oldval = Convert.ToUInt64(row["cod_value"]);
                        sql = $"update match_keywords set cod_value = {oval} where cod_value = {oldval} and cod_keyword = {idk}";
                        await _repository.Connector.ExecuteNonQueryAsync(sql, null, connection);
                        sql = $"delete from keyword_values where cod_value = {oldval}";
                        await _repository.Connector.ExecuteNonQueryAsync(sql, null, connection);
                    }
                }
            }
        }
        public override bool Equals(ObjectBase other)
        {
            MatchKeyword ok = other as MatchKeyword;
            if (ok == null)
            {
                return false;
            }
            return (ok.Name == Name) && (ok.IdMatch == IdMatch);
        }
        public override string ToString()
        {
            return Name;
        }
        public int CompareTo(MatchKeyword other)
        {
            if (IdMatch == other.IdMatch)
            {
                return Name.CompareTo(other.Name);
            }
            return IdMatch.CompareTo(other.IdMatch);
        }
    }
}

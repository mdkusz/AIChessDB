using AIChessDatabase.Interfaces;
using BaseClassesAndInterfaces.Interfaces;
using GlobalCommonEntities.Attributes;
using Resources;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Database POCO for keywords.
    /// </summary>
    [Serializable]
    [TableName("keywords")]
    public class Keyword : ObjectBase, IComparable<Keyword>
    {
        private const string _querystr = "select * from keywords";
        private const string _querycntstr = "select count(*) cnt from keywords";
        public Keyword()
        {
            _querySQL = _querystr;
            _queryCountSQL = _querycntstr;
        }
        public Keyword(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _querystr;
            _queryCountSQL = _querycntstr;
        }
        public Keyword(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            Keyword k = copy as Keyword;
            if (k != null)
            {
                _querySQL = _querystr;
                _queryCountSQL = _querycntstr;
                Name = k.Name;
                KeywordType = k.KeywordType;
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// Primary key value for the keyword.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdKeyword };
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
        }
        /// <summary>
        /// Primary key for the keyword.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_code), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_code), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("cod_keyword")]
        [TableColumnName("cod_keyword")]
        public ulong IdKeyword { get; protected set; }
        /// <summary>
        /// Keyword name
        /// </summary>
        [JsonPropertyName("keyword")]
        [TableColumnName("keyword")]
        [DILocalizedDescription(nameof(DES_keyword), typeof(Properties.UIResources))]
        [DILocalizedDisplayName(nameof(DN_keyword), typeof(Properties.UIResources))]
        public string Name { get; set; }
        /// <summary>
        /// Keyword type. MKW = Match Keyword, PST = Position statistic, MST = Match statistic.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_keyword_type), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_keyword_type), typeof(Properties.UIResources))]
        [JsonPropertyName("keyword_type")]
        [TableColumnName("keyword_type")]
        public string KeywordType { get; set; }
        /// <summary>
        /// Load keyword data from the database.
        /// </summary>
        /// <param name="key">
        /// Primary key value of the keyword to retrieve.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. Default is 0.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"cod_keyword = {_parameterPrefix}cod_keyword";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        /// Delete the keyword from the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query. Default is 0.
        /// </param>
        public override async Task Delete(int connection = 0)
        {
            string check = $"select count(*) from keywords k where k.cod_keyword = {IdKeyword}" +
                " and (exists (select 1 from match_keywords mk where mk.cod_keyword = k.cod_keyword) " +
                "or exists (select 1 from position_statistics pk where pk.cod_keyword = k.cod_keyword) " +
                "or exists (select 1 from match_statistics ms where ms.cod_keyword = k.cod_keyword))";
            int count = Convert.ToInt32(await _repository.Connector.ExecuteScalarAsync(check, null, connection));
            if (count == 0)
            {
                await base.Delete(connection);
            }
        }
        public override async Task<ISQLUIQuery> LoopDelete(int connection, ISQLUIQuery query)
        {
            string check = $"select count(*) from keywords k where k.cod_keyword = {IdKeyword}" +
                " and (exists (select 1 from match_keywords mk where mk.cod_keyword = k.cod_keyword) " +
                "or exists (select 1 from position_statistics pk where pk.cod_keyword = k.cod_keyword) " +
                "or exists (select 1 from match_statistics ms where ms.cod_keyword = k.cod_keyword))";
            int count = Convert.ToInt32(await _repository.Connector.ExecuteScalarAsync(check, null, connection));
            if (count == 0)
            {
                return await base.LoopDelete(connection, query);
            }
            return query;
        }
        public override bool Equals(ObjectBase other)
        {
            Keyword ok = other as Keyword;
            if (ok == null)
            {
                return false;
            }
            return (ok.Name == Name) && (ok.KeywordType == KeywordType);
        }
        public int CompareTo(Keyword other)
        {
            return Name.CompareTo(other.Name);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

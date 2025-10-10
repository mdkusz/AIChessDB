using AIChessDatabase.Interfaces;
using AIChessDatabase.Properties;
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
    /// MoveComment POCO class represents a comment on a move in a chess match.
    /// </summary>
    [Serializable]
    [TableName("move_comments")]
    public class MoveComment : ObjectBase, IComparable<MoveComment>
    {
        private const string _sqlcomments = "select * from move_comments";
        private const string _sqlcntcomments = "select count(*) cnt from move_comments";
        public MoveComment()
        {
            _querySQL = _sqlcomments;
            _queryCountSQL = _sqlcntcomments;
        }
        public MoveComment(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _sqlcomments;
            _queryCountSQL = _sqlcntcomments;
        }
        public MoveComment(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            MoveComment com = copy as MoveComment;
            if (com != null)
            {
                _querySQL = _sqlcomments;
                _queryCountSQL = _sqlcntcomments;
                MoveOrder = com.MoveOrder;
                Order = com.Order;
                Comment = com.Comment;
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        /// <summary>
        /// Primary key value for the MoveComment object, which is the comment identifier.
        /// </summary>
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdComment };
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
            IdComment = key[0];
        }
        /// <summary>
        /// Comment primary key value.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_code), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_code), typeof(UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("cod_comment")]
        [TableColumnName("cod_comment")]
        public ulong IdComment { get; protected set; }
        /// <summary>
        /// Match Primary key value to which this comment belongs.
        /// </summary>
        [HiddenField(true)]
        [JsonPropertyName("cod_match")]
        [TableColumnName("cod_match")]
        public ulong IdMatch { get; set; }
        /// <summary>
        /// Move order in the match to which this comment belongs.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_move_order), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_move_order), typeof(UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("move_order")]
        [TableColumnName("move_order")]
        public int MoveOrder { get; set; }
        /// <summary>
        /// Comment order in the move comment list, used to sort comments.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_comment_order), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_comment_order), typeof(UIResources))]
        [JsonPropertyName("comment_order")]
        [TableColumnName("comment_order")]
        public int Order { get; set; }
        /// <summary>
        /// Comment text. There are some standard NAG comments that start with a dollar sign ($), and have a corresponding string in the UIResources file.
        /// </summary>
        [DILocalizedDisplayName(nameof(DN_comment_text), typeof(UIResources))]
        [DILocalizedDescription(nameof(DES_comment_text), typeof(UIResources))]
        [JsonPropertyName("comment_text")]
        [TableColumnName("comment_text")]
        public string Comment { get; set; }
        /// <summary>
        /// Retrieves the object data from the database by its primary key value.
        /// </summary>
        /// <param name="key">
        /// Primary key value.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. If not specified, the default connection is used.
        /// </param>
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"cod_comment = {_parameterPrefix}cod_comment";
            await InternalGetAsync(key, text, connection);
        }
        /// <summary>
        /// Retrieves the object data from the database by its primary key value.
        /// </summary>
        /// <param name="key">
        /// Primary key value.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query. If not specified, the default connection is used.
        /// </param>
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"cod_comment = {_parameterPrefix}cod_comment";
            InternalGet(key, text, connection);
        }
        /// <summary>
        /// Use a DataRow to fill the object properties without retrieving data from the database.
        /// </summary>
        /// <param name="row">
        /// DataRow to use to fill the object properties. The row must have been executed with a query that returns the object properties.
        /// </param>
        public override void Get(DataRow row)
        {
            IdMatch = Convert.ToUInt64(row["cod_match"]);
            MoveOrder = Convert.ToInt32(row["move_order"]);
            Order = Convert.ToInt32(row["comment_order"]);
            Comment = row["comment_text"].ToString();
            IdComment = Convert.ToUInt64(row["cod_comment"]);
        }
        /// <summary>
        /// Gets the comment text, replacing any NAG codes with their corresponding localized strings.
        /// </summary>
        /// <returns>
        /// Localized comment text, or the original comment if no NAG code is found.
        /// </returns>
        public override string ToString()
        {
            if (Comment.StartsWith("$"))
            {
                string nag = ResourceManager.GetString("NAG_" + Comment.Substring(1));
                if (nag != null)
                {
                    return nag;
                }
            }
            return Comment;
        }
        public int CompareTo(MoveComment other)
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

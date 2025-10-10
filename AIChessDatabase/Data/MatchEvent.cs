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

    [Serializable]
    [TableName("match_events")]
    public class MatchEvent : ObjectBase
    {
        public const string _sqlcode = "select * from match_events";
        public const string _countsql = "select count(*) cnt from match_events";
        [Flags]
        public enum Event
        {
            Check = 1, Checkmate = 2, MultipleCheck = 4, DiscovedCheck = 8, DrawOffer = 16, PawnPromoted = 32, EnPassant = 64, Capture = 128,
            KingCastle = 256, QueenCastle = 512, Pawn1 = 1024, LightBishop1 = 2048, Knight1 = 4096, Rook1 = 8192, Queen1 = 16384, King1 = 32768,
            Pawn2 = 65536, LightBishop2 = 131072, Knight2 = 262144, Rook2 = 524288, Queen2 = 1048576, Move = 2097152,
            DarkBishop1 = 4194304, DarkBishop2 = 8388608, Bishop3 = 16777216, Knight3 = 33554432, Rook3 = 67108864, Queen3 = 134217728,
            AllChecks = 13, AllPieces1 = 4226048, AllPieces1ButPawn = 4225024, AllPieces2 = 10420224, AllPieces3 = 251658240, AllCastling = 768
        }
        public MatchEvent()
        {
            _querySQL = _sqlcode;
            _queryCountSQL = _countsql;
        }
        public MatchEvent(IObjectRepository rep)
            : base(rep)
        {
            _querySQL = _sqlcode;
            _queryCountSQL = _countsql;
        }
        public MatchEvent(IObjectRepository rep, ObjectBase copy)
            : base(rep, copy)
        {
            MatchEvent ev = copy as MatchEvent;
            if (ev != null)
            {
                _querySQL = _sqlcode;
                _queryCountSQL = _countsql;
                IdEvent = ev.IdEvent;
                Description = ev.Description;
            }
            else
            {
                throw new Exception(ERR_INCOMPATIBLETYPES);
            }
        }
        [JsonIgnore]
        public override ulong[] Key
        {
            get
            {
                return new ulong[] { IdEvent };
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
            IdEvent = (uint)key[0];
        }
        [DILocalizedDisplayName(nameof(DN_code), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_code), typeof(Properties.UIResources))]
        [ReadOnly(true)]
        [JsonPropertyName("cod_event")]
        [TableColumnName("cod_event")]
        public uint IdEvent { get; protected set; }
        [DILocalizedDisplayName(nameof(DN_description), typeof(Properties.UIResources))]
        [DILocalizedDescription(nameof(DES_event_description), typeof(Properties.UIResources))]
        [JsonPropertyName("description")]
        [TableColumnName("description")]
        public string Description { get; set; }
        public override async Task GetAsync(ulong[] key, int connection = 0)
        {
            string text = $"cod_event = {_parameterPrefix}cod_event";
            await InternalGetAsync(key, text, connection);
        }
        public override void Get(ulong[] key, int connection = 0)
        {
            string text = $"cod_event = {_parameterPrefix}cod_event";
            InternalGet(key, text, connection);
        }
        public override async Task Insert(int connection = 0)
        {
            await Task.Yield();
            throw new Exception(ERR_OPERATION_NOTALLOWED);
        }
        public override async Task<ISQLUIQuery> LoopInsert(int connection, ISQLUIQuery query)
        {
            await Task.Yield();
            throw new Exception(ERR_OPERATION_NOTALLOWED);
        }
        public override async Task Update(int connection = 0)
        {
            await Task.Yield();
            throw new Exception(ERR_OPERATION_NOTALLOWED);
        }
        public override async Task<ISQLUIQuery> LoopUpdate(int connection, ISQLUIQuery query)
        {
            await Task.Yield();
            throw new Exception(ERR_OPERATION_NOTALLOWED);
        }
        public override async Task Delete(int connection = 0)
        {
            await Task.Yield();
            throw new Exception(ERR_OPERATION_NOTALLOWED);
        }
        public override async Task<ISQLUIQuery> LoopDelete(int connection, ISQLUIQuery query)
        {
            await Task.Yield();
            throw new Exception(ERR_OPERATION_NOTALLOWED);
        }
        public override string ToString()
        {
            return ResourceManager.GetString(Description);
        }
    }
}

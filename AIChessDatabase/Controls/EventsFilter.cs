using AIChessDatabase.Data;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using GlobalCommonEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;
using static BaseClassesAndInterfaces.Properties.Resources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Specilized control for filtering matches by match move events.
    /// </summary>
    public partial class EventsFilter : UserControl, ISubqueryFilterEditor
    {
        private UIFilterExpression _efilter = null;
        private ISQLQuery _mainQuery = null;
        public EventsFilter()
        {
            InitializeComponent();
            gbMove.Text = GRP_PZMOVE;
            gbCaptured.Text = GRP_PIECECAPTURED;
            gbPromoted.Text = GRP_PAWNPROMOTED;
            label1.Text = LAB_CHECK;
            label2.Text = LAB_CASTLE;
            Text = TTL_MOVEPIECESED;
            cbAllTogether.Text = LAB_AllTogether;
        }
        /// <summary>
        /// IFilterEditor: Wrong values color
        /// </summary>
        public Color ValueErrorColor { get; set; }
        /// <summary>
        /// IFilterEditor: Correct values color
        /// </summary>
        public Color ValueOKColor { get; set; }
        /// <summary>
        /// IFilterEditor: Object to apply a filter
        /// </summary>
        public SQLElement Element { get; set; }
        /// <summary>
        /// IFilterEditor: Field name in the user interface
        /// </summary>
        public string FieldUIName { get; set; }
        /// <summary>
        /// IFilterEditor: Filter operator list
        /// </summary>
        public List<IUIIdentifier> FilterOperators { get; set; }
        /// <summary>
        /// IFilterEditor: Filter expression
        /// </summary>
        public UIFilterExpression FilterCondition
        {
            get
            {
                return _efilter;
            }
            set
            {
                _efilter = value;
                Clear();
                if ((_efilter != null) &&
                    !string.IsNullOrEmpty(_efilter.CustomFormattedValue))
                {
                    ulong ev = 0;
                    cbNever.Checked = _efilter.CustomFormattedValue.StartsWith("!");
                    cbAllTogether.Checked = _efilter.CustomFormattedValue.StartsWith("&") || _efilter.CustomFormattedValue.StartsWith("!&");
                    if (ulong.TryParse(_efilter.CustomFormattedValue.Substring(2), out ev))
                    {
                        SetMoves(ev);
                        SetCapture(ev);
                        SetPromote(ev);
                        SetCheck(ev);
                        SetCastle(ev);
                    }
                }
            }
        }
        /// <summary>
        /// IFilterEditor: The editor is visible to the user
        /// </summary>
        public bool FilterEditorVisible { get; set; }
        /// <summary>
        /// IFilterEditor: Filter operators provider
        /// </summary>
        public ISQLElementProvider ElementProvider { get; set; }
        /// <summary>
        /// IFilterEditor: Allowed values list provider
        /// </summary>
        public IValueListProvider ValueListProvider { get; set; }
        /// <summary>
        /// IFilterEditor: Event to infom that the user has accepted the filter
        /// </summary>
        public event EventHandler FilterAccepted;
        /// <summary>
        /// IFilterEditor: Event to inform that the user has cancelled the filter
        /// </summary>
        public event EventHandler FilterCancelled;
        /// <summary>
        /// ISubqueryFilterEditor: Main query to apply the filter to
        /// </summary>
        public ISQLQuery MainQuery
        {
            get
            {
                return _mainQuery;
            }
            set
            {
                _mainQuery = value;
                cbNever.Visible = _mainQuery != null;
            }
        }
        /// <summary>
        /// Clear the filter values in the user interface.
        /// </summary>
        private void Clear()
        {
            foreach (Control c in gbMove.Controls)
            {
                CheckBox cb = c as CheckBox;
                if (cb != null)
                {
                    cb.Checked = false;
                }
            }
            foreach (Control c in gbCaptured.Controls)
            {
                CheckBox cb = c as CheckBox;
                if (cb != null)
                {
                    cb.Checked = false;
                }
            }
            foreach (Control c in gbPromoted.Controls)
            {
                CheckBox cb = c as CheckBox;
                if (cb != null)
                {
                    cb.Checked = false;
                }
            }
            cbDraw.Checked = false;
            cbCastling.SelectedIndex = -1;
            cbCheck.SelectedIndex = -1;
        }
        /// <summary>
        /// Gets a string representing the selected promotion options for a pawn.
        /// </summary>
        private string Promoted
        {
            get
            {
                string p = "";
                if (cbBBishopP.Checked)
                {
                    p += "b";
                }
                if (cbKnightP.Checked)
                {
                    p += "n";
                }
                if (cbRookP.Checked)
                {
                    p += "r";
                }
                if (cbQueenP.Checked)
                {
                    p += "q";
                }
                return p;
            }
        }
        /// <summary>
        /// Move events.
        /// </summary>
        /// <remarks>
        /// This is a bitmask representing the events that occurred during a move.
        /// 1 ('EVENT_CHECK'): This event represents a check.
        /// 2 ('EVENT_CHECK_MATE'): This event represents a checkmate.
        /// 4 ('EVENT_MULTIPLE_CHECK'): This event represents a multiple check.
        /// 8 ('EVENT_DISCOVERED_CHECK'): This event represents a discovered check.
        /// 16 ('EVENT_DRAW_OFFER'): This event represents a draw offer.
        /// 32 ('EVENT_PAWN_PROMOTED'): This event represents a pawn promotion.
        /// 64 ('EVENT_PAWN_PASSANT'): This event represents an en passant pawn capture.
        /// 128 ('EVENT_CAPTURE'): This event represents a capture.
        /// 256 ('EVENT_CASTLEK'): This event represents a kingside castling.
        /// 512 ('EVENT_CASTLEQ'): This event represents a queenside castling.
        /// 1024 ('EVENT_PAWN1'): This event represents a pawn involved in a move (the piece that moves, captures, or gives check).
        /// 2048 ('EVENT_WBISHOP1'): This event represents a white-square bishop involved in a move (the piece that moves, captures, or gives check).
        /// 4096 ('EVENT_KNIGHT1'): This event represents a knight involved in a move (the piece that moves, captures, or gives check).
        /// 8192 ('EVENT_ROOK1'): This event represents a rook involved in a move (the piece that moves, captures, or gives check).
        /// 16384 ('EVENT_QUEEN1'): This event represents a queen involved in a move (the piece that moves, captures, or gives check).
        /// 32768 ('EVENT_KING1'): This event represents a king involved in a move (the piece that moves or captures).
        /// 65536 ('EVENT_PAWN2'): This event represents a pawn involved in a move (the piece that is captured).
        /// 131072 ('EVENT_WBISHOP2'): This event represents a white-square bishop involved in a move (the piece that is captured).
        /// 262144 ('EVENT_KNIGHT2'): This event represents a knight involved in a move (the piece that is captured).
        /// 524288 ('EVENT_ROOK2'): This event represents a rook involved in a move (the piece that is captured).
        /// 1048576 ('EVENT_QUEEN2'): This event represents a queen involved in a move (the piece that is captured).
        /// 2097152 ('EVENT_MOVE'): This event represents a movement.
        /// 4194304 ('EVENT_BBISHOP1'): This event represents a black-square bishop involved in a move (the piece that moves, captures, or gives check).
        /// 8388608 ('EVENT_BBISHOP2'): This event represents a black-square bishop involved in a move (the piece that is captured).
        /// 16777216 ('EVENT_BISHOP3'): This event represents a generic bishop (e.g., the piece a pawn promotes to).
        /// 33554432 ('EVENT_KNIGHT3'): This event represents a generic knight (e.g., the piece a pawn promotes to).
        /// 134217728 ('EVENT_QUEEN3'): This event represents a generic queen (e.g., the piece a pawn promotes to).
        /// </remarks>      
        /// <example>
        /// 2113538 = EVENT_CHECK_MATE + EVENT_QUEEN1 + EVENT_MOVE (The queen moves and gives checkmate).
        /// 2098176 = EVENT_PAWN1 + EVENT_MOVE(a pawn moves).
        /// 4259968 = EVENT_CAPTURE + EVENT_PAWN2 + EVENT_BBISHOP1(a bishop captures a pawn).
        /// 136315937 = EVENT_CHECK + EVENT_PAWN_PROMOTED + EVENT_PAWN1 + EVENT_MOVE + EVENT_QUEEN3(a pawn moves, promotes to a queen, and gives check).
        /// 136315938 = EVENT_CHECK_MATE + EVENT_PAWN_PROMOTED + EVENT_PAWN1 + EVENT_MOVE + EVENT_QUEEN3(a pawn moves, promotes to queen, and gives checkmate).
        /// </example>
        private ulong EventFilter
        {
            get
            {
                ulong ev = 0;
                ulong evm = (ulong)MatchEvent.Event.Move;
                ulong evc = 0;
                if (cbPawnM.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        evm = (ulong)MatchEvent.Event.Pawn1;
                    }
                    else
                    {
                        ev |= (ulong)MatchEvent.Event.Pawn1;
                    }
                }
                if (cbBBishopM.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev = (ulong)MatchEvent.Event.DarkBishop1;
                    }
                    else
                    {
                        ev |= (ulong)MatchEvent.Event.DarkBishop1;
                    }
                }
                if (cbWBishopM.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev = (ulong)MatchEvent.Event.LightBishop1;
                    }
                    else
                    {
                        ev |= (ulong)MatchEvent.Event.LightBishop1;
                    }
                }
                if (cbKnightM.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev = (ulong)MatchEvent.Event.Knight1;
                    }
                    else
                    {
                        ev |= (ulong)MatchEvent.Event.Knight1;
                    }
                }
                if (cbRookM.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev = (ulong)MatchEvent.Event.Rook1;
                    }
                    else
                    {
                        ev |= (ulong)MatchEvent.Event.Rook1;
                    }
                }
                if (cbQueenM.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev = (ulong)MatchEvent.Event.Queen1;
                    }
                    else
                    {
                        ev |= (ulong)MatchEvent.Event.Queen1;
                    }
                }
                if (cbKingM.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev = (ulong)MatchEvent.Event.King1;
                    }
                    else
                    {
                        ev |= (ulong)MatchEvent.Event.King1;
                    }
                }
                if ((ev == 0) && !cbAllTogether.Checked)
                {
                    ev = (ulong)MatchEvent.Event.AllPieces1;
                }
                ulong ev2 = 0;
                if (cbPawnC.Checked)
                {
                    evm = 0;
                    evc = (ulong)MatchEvent.Event.Capture;
                    if (cbAllTogether.Checked)
                    {
                        ev2 = (ulong)MatchEvent.Event.Pawn2;
                    }
                    else
                    {
                        ev2 |= (ulong)MatchEvent.Event.Pawn2;
                    }
                }
                if (cbBBishopC.Checked)
                {
                    evm = 0;
                    evc = (ulong)MatchEvent.Event.Capture;
                    if (cbAllTogether.Checked)
                    {
                        ev2 = (ulong)MatchEvent.Event.DarkBishop2;
                    }
                    else
                    {
                        ev2 |= (ulong)MatchEvent.Event.DarkBishop2;
                    }
                }
                if (cbWBishopC.Checked)
                {
                    evm = 0;
                    evc = (ulong)MatchEvent.Event.Capture;
                    if (cbAllTogether.Checked)
                    {
                        ev2 = (ulong)MatchEvent.Event.LightBishop2;
                    }
                    else
                    {
                        ev2 |= (ulong)MatchEvent.Event.LightBishop2;
                    }
                }
                if (cbKnightC.Checked)
                {
                    evm = 0;
                    evc = (ulong)MatchEvent.Event.Capture;
                    if (cbAllTogether.Checked)
                    {
                        ev2 = (ulong)MatchEvent.Event.Knight2;
                    }
                    else
                    {
                        ev2 |= (ulong)MatchEvent.Event.Knight2;
                    }
                }
                if (cbRookC.Checked)
                {
                    evm = 0;
                    evc = (ulong)MatchEvent.Event.Capture;
                    if (cbAllTogether.Checked)
                    {
                        ev2 = (ulong)MatchEvent.Event.Rook2;
                    }
                    else
                    {
                        ev2 |= (ulong)MatchEvent.Event.Rook2;
                    }
                }
                if (cbQueenC.Checked)
                {
                    evm = 0;
                    evc = (ulong)MatchEvent.Event.Capture;
                    if (cbAllTogether.Checked)
                    {
                        ev2 = (ulong)MatchEvent.Event.Queen2;
                    }
                    else
                    {
                        ev2 |= (ulong)MatchEvent.Event.Queen2;
                    }
                }
                ev |= evc | evm | ev2;
                ulong ev3 = 0;
                if (cbBBishopP.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev3 = (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Bishop3);
                    }
                    else
                    {
                        ev3 |= (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Bishop3);
                    }
                }
                if (cbKnightP.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev3 = (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Knight3);
                    }
                    else
                    {
                        ev3 |= (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Knight3);
                    }
                }
                if (cbRookP.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev3 = (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Rook3);
                    }
                    else
                    {
                        ev3 |= (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Rook3);
                    }
                }
                if (cbQueenP.Checked)
                {
                    if (cbAllTogether.Checked)
                    {
                        ev3 = (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Queen3);
                    }
                    else
                    {
                        ev3 |= (ulong)(MatchEvent.Event.PawnPromoted | MatchEvent.Event.Queen3);
                    }
                }
                ev |= ev3;
                if (cbDraw.Checked)
                {
                    ev |= (ulong)MatchEvent.Event.DrawOffer;
                }
                if (cbCastling.SelectedIndex >= 0)
                {
                    if (cbCastling.SelectedItem.ToString() == EVENT_CASTLEK)
                    {
                        if (cbAllTogether.Checked)
                        {
                            ev = (ulong)MatchEvent.Event.KingCastle;
                        }
                        else
                        {
                            ev |= (ulong)MatchEvent.Event.KingCastle;
                        }
                    }
                    else if (cbCastling.SelectedItem.ToString() == EVENT_CASTLEQ)
                    {
                        if (cbAllTogether.Checked)
                        {
                            ev = (ulong)MatchEvent.Event.QueenCastle;
                        }
                        else
                        {
                            ev |= (ulong)MatchEvent.Event.QueenCastle;
                        }
                    }
                    else if (cbCastling.SelectedItem.ToString() == EVENT_CASTLE)
                    {
                        if (cbAllTogether.Checked)
                        {
                            ev = (ulong)(MatchEvent.Event.QueenCastle | MatchEvent.Event.KingCastle);
                        }
                        else
                        {
                            ev |= (ulong)(MatchEvent.Event.QueenCastle | MatchEvent.Event.KingCastle);
                        }
                    }
                }
                if (cbCheck.SelectedIndex >= 0)
                {
                    if (cbCheck.SelectedItem.ToString() == EVENT_CHECK)
                    {
                        ev |= (ulong)MatchEvent.Event.Check;
                    }
                    else if (cbCheck.SelectedItem.ToString() == EVENT_CHECK_MATE)
                    {
                        ev |= (ulong)MatchEvent.Event.Checkmate;
                    }
                    else if (cbCheck.SelectedItem.ToString() == EVENT_MULTIPLE_CHECK)
                    {
                        ev |= (ulong)MatchEvent.Event.MultipleCheck;
                    }
                    else if (cbCheck.SelectedItem.ToString() == EVENT_DISCOVERED_CHECK)
                    {
                        ev |= (ulong)MatchEvent.Event.DiscovedCheck;
                    }
                }
                if (cbEnPassant.Checked)
                {
                    ev |= (ulong)MatchEvent.Event.EnPassant;
                }
                return ev;
            }
        }
        /// <summary>
        /// Set piece moved checkboxes based on the event value.
        /// </summary>
        /// <param name="ev">
        /// Event mask value representing the move events.
        /// </param>
        private void SetMoves(ulong ev)
        {
            cbPawnM.Checked = (ev & (ulong)MatchEvent.Event.Pawn1) != 0;
            cbBBishopM.Checked = (ev & (ulong)MatchEvent.Event.DarkBishop1) != 0;
            cbWBishopM.Checked = (ev & (ulong)MatchEvent.Event.LightBishop1) != 0;
            cbKnightM.Checked = (ev & (ulong)MatchEvent.Event.Knight1) != 0;
            cbRookM.Checked = (ev & (ulong)MatchEvent.Event.Rook1) != 0;
            cbQueenM.Checked = (ev & (ulong)MatchEvent.Event.Queen1) != 0;
            cbKingM.Checked = (ev & (ulong)MatchEvent.Event.King1) != 0;
        }
        /// <summary>
        /// Set piece captured checkboxes based on the event value.
        /// </summary>
        /// <param name="ev">
        /// Event mask value representing the capture events.
        /// </param>
        private void SetCapture(ulong ev)
        {
            cbPawnC.Checked = (ev & (ulong)MatchEvent.Event.Pawn2) != 0;
            cbBBishopC.Checked = (ev & (ulong)MatchEvent.Event.DarkBishop2) != 0;
            cbWBishopC.Checked = (ev & (ulong)MatchEvent.Event.LightBishop2) != 0;
            cbKnightC.Checked = (ev & (ulong)MatchEvent.Event.Knight2) != 0;
            cbRookC.Checked = (ev & (ulong)MatchEvent.Event.Rook2) != 0;
            cbQueenC.Checked = (ev & (ulong)MatchEvent.Event.Queen2) != 0;
            cbEnPassant.Checked = (ev & (ulong)MatchEvent.Event.EnPassant) != 0;
        }
        /// <summary>
        /// Set pawn promopted to checkboxes based on the event value.
        /// </summary>
        /// <param name="ev">
        /// Event mask value representing the pawn promotion events.
        /// </param>
        private void SetPromote(ulong ev)
        {
            cbBBishopP.Checked = (ev & (ulong)MatchEvent.Event.Bishop3) != 0;
            cbKnightP.Checked = (ev & (ulong)MatchEvent.Event.Knight3) != 0;
            cbRookP.Checked = (ev & (ulong)MatchEvent.Event.Rook3) != 0;
            cbQueenP.Checked = (ev & (ulong)MatchEvent.Event.Queen3) != 0;
        }
        /// <summary>
        /// Set check selection based on the event value.
        /// </summary>
        /// <param name="ev">
        /// Event mask value representing the check events.
        /// </param>
        private void SetCheck(ulong ev)
        {
            if ((ev & (ulong)MatchEvent.Event.Check) != 0)
            {
                cbCheck.SelectedItem = EVENT_CHECK;
            }
            else if ((ev & (ulong)MatchEvent.Event.Checkmate) != 0)
            {
                cbCheck.SelectedItem = EVENT_CHECK_MATE;
            }
            else if ((ev & (ulong)MatchEvent.Event.MultipleCheck) != 0)
            {
                cbCheck.SelectedItem = EVENT_MULTIPLE_CHECK;
            }
            else if ((ev & (ulong)MatchEvent.Event.DiscovedCheck) != 0)
            {
                cbCheck.SelectedItem = EVENT_DISCOVERED_CHECK;
            }
            if ((ev & (ulong)MatchEvent.Event.DrawOffer) != 0)
            {
                cbDraw.Checked = true;
            }
        }
        /// <summary>
        /// Set castling selection based on the event value.
        /// </summary>
        /// <param name="ev">
        /// Event mask value representing the castling events.
        /// </param>
        private void SetCastle(ulong ev)
        {
            if ((ev & (ulong)(MatchEvent.Event.QueenCastle | MatchEvent.Event.KingCastle)) ==
                (ulong)(MatchEvent.Event.QueenCastle | MatchEvent.Event.KingCastle))
            {
                cbCastling.SelectedItem = EVENT_CASTLE;
            }
            else if ((ev & (ulong)MatchEvent.Event.KingCastle) != 0)
            {
                cbCastling.SelectedItem = EVENT_CASTLEK;
            }
            else if ((ev & (ulong)MatchEvent.Event.QueenCastle) != 0)
            {
                cbCastling.SelectedItem = EVENT_CASTLEQ;
            }
        }
        /// <summary>
        /// Builds the filter expression based on the selected events.
        /// </summary>
        /// <remarks>
        /// This is a combination of BITAND operations on the event flags.
        /// </remarks>
        private void BuildFilter()
        {
            UIFilterExpressionList filters = new UIFilterExpressionList();
            ulong ev = EventFilter;
            LogicOperator opband = ElementProvider.SQLElement(typeof(LogicOperator), new[] { PLOP_bit_and }) as LogicOperator;
            LogicOperator opand = ElementProvider.SQLElement(typeof(LogicOperator), new[] { TK_and }) as LogicOperator;
            QueryColumn cevents = Element as QueryColumn;
            if (cbNever.Checked)
            {
                cevents = cevents.Clone(ElementProvider) as QueryColumn;
                cevents.OwnerName = "mm";
            }
            if (cbAllTogether.Checked)
            {
                filters.Add(opband.BuildExpression(cevents, new List<object> { ev, ev }, ElementProvider));
            }
            else
            {
                filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.AllPieces1 }, ElementProvider));
                if ((ev & (ulong)MatchEvent.Event.AllPieces2) != 0)
                {
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.Capture }, ElementProvider));
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.AllPieces2 }, ElementProvider));
                }
                else
                {
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.Move }, ElementProvider));
                }
                if ((ev & (ulong)MatchEvent.Event.AllPieces3) != 0)
                {
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.AllPieces3 }, ElementProvider));
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.PawnPromoted }, ElementProvider));
                }
                if ((ev & (ulong)MatchEvent.Event.AllCastling) != 0)
                {
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.AllCastling }, ElementProvider));
                }
                if ((ev & (ulong)MatchEvent.Event.AllChecks) != 0)
                {
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.AllChecks }, ElementProvider));
                }
                if ((ev & (ulong)MatchEvent.Event.DrawOffer) != 0)
                {
                    filters.Add(opband.BuildExpression(cevents, new List<object> { ev & (ulong)MatchEvent.Event.DrawOffer }, ElementProvider));
                }
            }
            SQLExpression expr = ElementProvider.SQLElement(typeof(SQLExpression)) as SQLExpression;
            expr.Parentheses = filters.Count > 1;
            expr.Elements.Add(filters[0].GetElement());
            for (int ix = 1; ix < filters.Count; ix++)
            {
                expr.Elements.Add(opand);
                expr.Elements.Add(filters[ix].GetElement());
            }
            if (cbNever.Checked)
            {
                Table tv = MainQuery.Tables[0] as Table;
                string sql = $"(select 1 from {tv.Name} mm where mm.cod_match = m.cod_match and {expr.GetSQL(SQLScope.Where)})";
                string tokenset = "";
                string token = "";
                SQLElement squery = MainQuery.Parser.Parse(ref token, ref tokenset, ref sql, MainQuery);
                expr.Elements.Clear();
                LogicOperator opnotexists = ElementProvider.SQLElement(typeof(LogicOperator), new[] { TK_not_exists }) as LogicOperator;
                expr.Elements.Add(opnotexists);
                expr.Elements.Add(squery);
            }
            FilterCondition = new UIFilterExpression();
            FilterCondition.SetElement(expr);
            FilterCondition.FriendlyName = BuildFriendlyName(ev);
            string prefix = (cbNever.Checked ? "!" : "") + (cbAllTogether.Checked ? "&" : "");
            if (prefix.Length < 2)
            {
                prefix += new string(' ', 2 - prefix.Length);
            }
            FilterCondition.CustomFormattedValue = prefix + ev.ToString();
        }
        /// <summary>
        /// Builds a description for the event based on the selected options.
        /// </summary>
        /// <param name="ev">
        /// Event mask value representing the move events.
        /// </param>
        /// <returns>
        /// Friendly name for the event combination, which is a string that describes the move in a human-readable format.
        /// </returns>
        private string BuildFriendlyName(ulong ev)
        {
            string castling = "";
            string pzmove = "";
            string promo = "";
            string check = "";
            string draw = "";
            string sep = "";
            if (((ev & (ulong)MatchEvent.Event.QueenCastle) != 0) || ((ev & (ulong)MatchEvent.Event.KingCastle) != 0))
            {
                if (((ev & (ulong)MatchEvent.Event.QueenCastle) == 0))
                {
                    castling = EVENT_CASTLEK;
                }
                else if ((ev & (ulong)MatchEvent.Event.KingCastle) == 0)
                {
                    castling = EVENT_CASTLEQ;
                }
                else
                {
                    castling = EVENT_CASTLE;
                }
            }
            if (((ev & (ulong)MatchEvent.Event.Pawn1) != 0) || !string.IsNullOrEmpty(Promoted))
            {
                pzmove = EVENT_PAWN1;
                if (!string.IsNullOrEmpty(Promoted))
                {
                    promo = EVENT_PAWN_PROMOTED + SEP_TO;
                    if ((ev & (ulong)MatchEvent.Event.Bishop3) != 0)
                    {
                        promo += PZ_BISHOP;
                    }
                    if ((ev & (ulong)MatchEvent.Event.Knight3) != 0)
                    {
                        promo += EVENT_KNIGHT2;
                    }
                    if ((ev & (ulong)MatchEvent.Event.Rook3) != 0)
                    {
                        promo += EVENT_ROOK2;
                    }
                    if ((ev & (ulong)MatchEvent.Event.Queen3) != 0)
                    {
                        promo += EVENT_QUEEN2;
                    }
                }
                sep = SEP_AND;
            }
            if ((ev & (ulong)MatchEvent.Event.DarkBishop1) != 0)
            {
                if ((ev & (ulong)MatchEvent.Event.LightBishop1) != 0)
                {
                    pzmove += sep + PZ_BISHOP;
                    sep = SEP_OR;
                }
                else
                {
                    pzmove += sep + EVENT_BBISHOP1;
                    sep = SEP_OR;
                }
            }
            else if ((ev & (ulong)MatchEvent.Event.LightBishop1) != 0)
            {
                pzmove += sep + EVENT_WBISHOP1;
                sep = SEP_OR;
            }
            if ((ev & (ulong)MatchEvent.Event.Knight1) != 0)
            {
                pzmove += sep + EVENT_KNIGHT1;
                sep = SEP_OR;
            }
            if ((ev & (ulong)MatchEvent.Event.Rook1) != 0)
            {
                pzmove += sep + EVENT_ROOK1;
                sep = SEP_OR;
            }
            if ((ev & (ulong)MatchEvent.Event.Queen1) != 0)
            {
                pzmove += sep + EVENT_QUEEN1;
                sep = SEP_OR;
            }
            if ((ev & (ulong)MatchEvent.Event.King1) != 0)
            {
                pzmove += sep + EVENT_KING1;
                sep = SEP_AND;
            }
            sep = " ";
            if ((ev & (ulong)MatchEvent.Event.Capture) != 0)
            {
                if (string.IsNullOrEmpty(pzmove))
                {
                    pzmove = PZ_ANY;
                }
                if ((ev & (ulong)MatchEvent.Event.Move) != 0)
                {
                    pzmove += sep + EVENT_MOVE + SEP_AND;
                }
                else
                {
                    pzmove += sep;
                }
                pzmove += EVENT_CAPTURE;
                if ((ev & (ulong)MatchEvent.Event.EnPassant) != 0)
                {
                    pzmove += sep + EVENT_PAWN_PASSANT;
                }
                if ((ev & (ulong)MatchEvent.Event.DarkBishop2) != 0)
                {
                    if ((ev & (ulong)MatchEvent.Event.LightBishop2) != 0)
                    {
                        pzmove += sep + PZ_BISHOP;
                        sep = SEP_OR;
                    }
                    else
                    {
                        pzmove += sep + EVENT_BBISHOP2;
                        sep = SEP_OR;
                    }
                }
                else if ((ev & (ulong)MatchEvent.Event.LightBishop2) != 0)
                {
                    pzmove += sep + EVENT_WBISHOP2;
                    sep = SEP_OR;
                }
                if ((ev & (ulong)MatchEvent.Event.Knight2) != 0)
                {
                    pzmove += sep + EVENT_KNIGHT2;
                    sep = SEP_OR;
                }
                if ((ev & (ulong)MatchEvent.Event.Rook2) != 0)
                {
                    pzmove += sep + EVENT_ROOK2;
                    sep = SEP_OR;
                }
                if ((ev & (ulong)MatchEvent.Event.Queen2) != 0)
                {
                    pzmove += sep + EVENT_QUEEN2;
                    sep = SEP_AND;
                }
            }
            else
            {
                pzmove += sep + EVENT_MOVE;
            }
            sep = "";
            if ((ev & (ulong)MatchEvent.Event.Check) != 0)
            {
                check = EVENT_CHECK;
                sep = SEP_AND;
            }
            else
            {
                if ((ev & (ulong)MatchEvent.Event.MultipleCheck) != 0)
                {
                    check = EVENT_MULTIPLE_CHECK;
                    sep = SEP_AND;
                }
                if ((ev & (ulong)MatchEvent.Event.DiscovedCheck) != 0)
                {
                    check += sep + EVENT_DISCOVERED_CHECK;
                    sep = SEP_AND;
                }
            }
            if ((ev & (ulong)MatchEvent.Event.Checkmate) != 0)
            {
                check += sep + EVENT_CHECKMATE;
            }
            if ((ev & (ulong)MatchEvent.Event.DrawOffer) != 0)
            {
                draw = EVENT_DRAW_OFFER;
            }
            string result = castling;
            if (!string.IsNullOrEmpty(result))
            {
                result += SEP_AND;
            }
            result += pzmove;
            if (!string.IsNullOrEmpty(result))
            {
                result += "; ";
            }
            result += promo;
            if (!string.IsNullOrEmpty(result))
            {
                result += "; ";
            }
            result += check;
            if (!string.IsNullOrEmpty(result))
            {
                result += "; ";
            }
            result += draw;
            if (cbNever.Checked)
            {
                result = CB_NEVER + " " + result;
            }
            return result;
        }
        private void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                BuildFilter();
                Form frmc = Parent as Form;
                if (frmc != null)
                {
                    frmc.Controls.Remove(this);
                    if (frmc.Modal)
                    {
                        frmc.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        FilterAccepted?.Invoke(this, EventArgs.Empty);
                        frmc.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbPawnM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPawnM.Checked)
            {
                cbEnPassant.Enabled = true;
            }
            else
            {
                cbEnPassant.Enabled = false;
                cbEnPassant.Checked = false;
            }
            if (cbAllTogether.Checked && cbPawnM.Checked)
            {
                cbBBishopM.Checked = false;
                cbWBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Form frmc = Parent as Form;
            if (frmc != null)
            {
                frmc.Controls.Remove(this);
                if (frmc.Modal)
                {
                    frmc.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    FilterCancelled?.Invoke(this, EventArgs.Empty);
                    frmc.Close();
                }
            }
        }

        private void cbCastling_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && (cbCastling.SelectedIndex >= 0))
            {
                cbBBishopC.Checked = false;
                cbBBishopM.Checked = false;
                cbWBishopC.Checked = false;
                cbWBishopM.Checked = false;
                cbKnightC.Checked = false;
                cbKnightM.Checked = false;
                cbRookC.Checked = false;
                cbRookM.Checked = false;
                cbQueenC.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
                cbPawnC.Checked = false;
                cbEnPassant.Checked = false;
                cbPawnM.Checked = false;
            }
        }

        private void cbWBishopM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbWBishopM.Checked)
            {
                cbPawnM.Checked = false;
                cbEnPassant.Checked = false;
                cbKnightP.Checked = false;
                cbRookP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbBBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbBBishopM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbBBishopM.Checked)
            {
                cbPawnM.Checked = false;
                cbEnPassant.Checked = false;
                cbKnightP.Checked = false;
                cbRookP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbWBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbKnightM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbKnightM.Checked)
            {
                cbPawnM.Checked = false;
                cbEnPassant.Checked = false;
                cbKnightP.Checked = false;
                cbRookP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbWBishopM.Checked = false;
                cbBBishopM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbRookM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbRookM.Checked)
            {
                cbPawnM.Checked = false;
                cbEnPassant.Checked = false;
                cbKnightP.Checked = false;
                cbRookP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbWBishopM.Checked = false;
                cbBBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbQueenM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbQueenM.Checked)
            {
                cbPawnM.Checked = false;
                cbEnPassant.Checked = false;
                cbKnightP.Checked = false;
                cbRookP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbWBishopM.Checked = false;
                cbBBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbKingM_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbKingM.Checked)
            {
                cbPawnM.Checked = false;
                cbEnPassant.Checked = false;
                cbKnightP.Checked = false;
                cbRookP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbWBishopM.Checked = false;
                cbBBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
            }
        }

        private void cbPawnC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbPawnC.Checked)
            {
                cbBBishopC.Checked = false;
                cbWBishopC.Checked = false;
                cbKnightC.Checked = false;
                cbRookC.Checked = false;
                cbQueenC.Checked = false;
            }
        }

        private void cbWBishopC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbWBishopC.Checked)
            {
                cbBBishopC.Checked = false;
                cbPawnC.Checked = false;
                cbKnightC.Checked = false;
                cbRookC.Checked = false;
                cbQueenC.Checked = false;
            }
        }

        private void cbBBishopC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbBBishopC.Checked)
            {
                cbWBishopC.Checked = false;
                cbPawnC.Checked = false;
                cbKnightC.Checked = false;
                cbRookC.Checked = false;
                cbQueenC.Checked = false;
            }
        }

        private void cbKnightC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbKnightC.Checked)
            {
                cbWBishopC.Checked = false;
                cbPawnC.Checked = false;
                cbBBishopC.Checked = false;
                cbRookC.Checked = false;
                cbQueenC.Checked = false;
            }
        }

        private void cbRookC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbRookC.Checked)
            {
                cbWBishopC.Checked = false;
                cbPawnC.Checked = false;
                cbBBishopC.Checked = false;
                cbKnightC.Checked = false;
                cbQueenC.Checked = false;
            }
        }

        private void cbQueenC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbQueenC.Checked)
            {
                cbWBishopC.Checked = false;
                cbPawnC.Checked = false;
                cbBBishopC.Checked = false;
                cbKnightC.Checked = false;
                cbRookC.Checked = false;
            }
        }

        private void cbQueenP_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbQueenP.Checked)
            {
                cbRookP.Checked = false;
                cbKnightP.Checked = false;
                cbBBishopP.Checked = false;
                cbBBishopM.Checked = false;
                cbWBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbBBishopP_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbBBishopP.Checked)
            {
                cbRookP.Checked = false;
                cbKnightP.Checked = false;
                cbQueenP.Checked = false;
                cbBBishopM.Checked = false;
                cbWBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbKnightP_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbKnightP.Checked)
            {
                cbRookP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbBBishopM.Checked = false;
                cbWBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbRookP_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked && cbRookP.Checked)
            {
                cbKnightP.Checked = false;
                cbBBishopP.Checked = false;
                cbQueenP.Checked = false;
                cbBBishopM.Checked = false;
                cbWBishopM.Checked = false;
                cbKnightM.Checked = false;
                cbRookM.Checked = false;
                cbQueenM.Checked = false;
                cbKingM.Checked = false;
            }
        }

        private void cbAllTogether_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllTogether.Checked)
            {
                cbPawnM_CheckedChanged(this, EventArgs.Empty);
                cbWBishopM_CheckedChanged(this, EventArgs.Empty);
                cbBBishopM_CheckedChanged(this, EventArgs.Empty);
                cbKnightM_CheckedChanged(this, EventArgs.Empty);
                cbRookM_CheckedChanged(this, EventArgs.Empty);
                cbQueenM_CheckedChanged(this, EventArgs.Empty);
                cbKingM_CheckedChanged(this, EventArgs.Empty);
                cbPawnC_CheckedChanged(this, EventArgs.Empty);
                cbWBishopC_CheckedChanged(this, EventArgs.Empty);
                cbBBishopC_CheckedChanged(this, EventArgs.Empty);
                cbKnightC_CheckedChanged(this, EventArgs.Empty);
                cbRookC_CheckedChanged(this, EventArgs.Empty);
                cbQueenC_CheckedChanged(this, EventArgs.Empty);
                cbQueenP_CheckedChanged(this, EventArgs.Empty);
                cbRookP_CheckedChanged(this, EventArgs.Empty);
                cbKnightP_CheckedChanged(this, EventArgs.Empty);
                cbBBishopP_CheckedChanged(this, EventArgs.Empty);
            }
        }
    }
}

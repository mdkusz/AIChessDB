using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    /// <summary>
    /// Dialog box to display a keyboard for a chess match with their moves, commets and positions.
    /// Users can step forward and backward through the moves on the board and view the corresponding positions and comments.
    /// </summary>
    public partial class DlgMatch : Form, IUIRemoteControlElement, IAIChessPlayer, IChessDBWindow
    {
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;
        public DlgMatch()
        {
            InitializeComponent();
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
        }
        /// <summary>
        /// IChessDBWindow: Window identifier, used to uniquely identify the window in the application.
        /// </summary>
        public string WINDOW_ID { get { return UID; } }
        /// <summary>
        /// IChessDBWindow: Application services provider.
        /// </summary>

        public IAppServiceProvider Provider { get; set; }
        /// <summary>
        /// IChessDBWindow: Connection index to use for database operations.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// IChessDBWindow: Database repository for data operations.
        /// </summary>
        public IObjectRepository Repository { get { return cPlayer.CurrentMatch?.Repository; } set { } }
        /// <summary>
        /// IUIRemoteControlElement, IAIChessPlayer: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement, IAIChesPlayer: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return FNAME_DlgMatch; } }
        /// <summary>
        /// IUIRemoteControlElement: Get first-level UI elements.
        /// </summary>
        /// <returns>
        /// List of first-level UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElements()
        {
            return _collector.GetUIElements(null);
        }
        /// <summary>
        /// IUIRemoteControlElement: Get all-level UI elements.
        /// </summary>
        /// <returns>
        /// List of all UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetAllUIElements()
        {
            return _collector.GetAllUIElements(null);
        }
        /// <summary>
        /// IUIRemoteControlElement: Get the child UI elements of that on a specific path.
        /// </summary>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElementChildren(string path)
        {
            return _collector.GetUIElementChildren(null, path);
        }
        /// <summary>
        /// IUIRemoteControlElement: Highlight a UI element for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to highlight.
        /// </param>
        /// <param name="seconds">
        /// Duration in seconds to highlight the element.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        public void HighlightUIElement(string path, int seconds, string mode)
        {
            _interactor.HighLight(path, seconds, mode);
        }
        /// <summary>
        /// IUIRemoteControlElement: Show a tooltip for a UI element with a comment for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to comment.
        /// </param>
        /// <param name="title">
        /// The title of the notification balloon.
        /// </param>
        /// <param name="comment">
        /// Comment to display in the tooltip.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        /// <param name="seconds">
        /// Seconds to display the tooltip.
        /// </param>
        public void CommentUIElement(string path, string title, string comment, string mode, int seconds)
        {
            _interactor.ShowBalloon(path, title, comment, mode, seconds);
        }
        /// <summary>
        /// IUIRemoteControlElement: Invoke an action on a UI element at the specified path.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to invoke the action on.
        /// </param>
        /// <param name="action">
        /// Action name to invoke on the UI element, such as "click", "double-click", etc.
        /// </param>
        /// <returns>
        /// String value or null
        /// </returns>
        /// <remarks>
        /// The format for action is: action[:param]
        /// </remarks>
        public object InvokeElementAction(string path, string action)
        {
            return _interactor.Invoke(path, action);
        }
        /// <summary>
        /// IAIChessPlayer: Match is editable. New moves can be added, and existing moves can be canceled.
        /// </summary>
        public bool Editable { get { return false; } }
        /// <summary>
        /// IAIChessPlayer: Json object with match information.
        /// </summary>
        public Match MatchInfo { get { return cPlayer.CurrentMatch; } }
        /// <summary>
        /// IAIChessPlayer: List of moves in the match. Each move is a Json object.
        /// </summary>
        public List<MatchMove> Moves { get { return new List<MatchMove>(cPlayer.CurrentMatch.Moves); } }
        /// <summary>
        /// IAIChessPlayer: Current move in the match.
        /// </summary>
        public MatchMove CurrentMove { get { return cPlayer.CurrentMove; } }
        /// <summary>
        /// IAIChessPlayer: Current board position.
        /// </summary>
        public string CurrentBoard { get { return cPlayer.CurrentBoard; } }
        /// <summary>
        /// IAIChessPlayer: Advance to the next move in the match.
        /// </summary>
        /// <param name="comment">
        /// Show a comment on the move being made.
        /// </param>
        /// <returns>
        /// A list with three moves: the previous move, the current move, and the next move.
        /// </returns>
        public List<MatchMove> NextMove(string comment) { return cPlayer.EnqueueMove(comment); }
        /// <summary>
        /// IAIChessPlayer: Forward to the previous move in the match.
        /// </summary>
        public MatchMove PrevMove { get { return cPlayer.MoveBack(); } }
        /// <summary>
        /// IAIChessPlayer: Add a move to the match.
        /// </summary>
        /// <param name="move">
        /// Textual representation of the move in Algebraic Notation (AN).
        /// </param>
        /// <param name="comment">
        /// Optional move comment
        /// </param>
        public string AddMove(string move, string comment = null)
        {
            return ERR_NOTEDITABLEMATCH;
        }
        /// <summary>
        /// IAIChessPlayer: Add a whole match in PGN format.
        /// </summary>
        /// <param name="pgn">
        /// Match in PGN format.
        /// </param>
        /// <returns>
        /// Message indicating success or failure.
        /// </returns>
        public string AddMatch(string pgn)
        {
            return ERR_NOTIMPLEMENTED;
        }
        /// <summary>
        /// IAIChessPlayer: Cancel the last move made in the match.
        /// </summary>
        public void CancelLastMove()
        {
            throw new Exception(ERR_NOTEDITABLEMATCH);
        }
        /// <summary>
        /// Set the current match to interact with
        /// </summary>
        /// <param name="match">
        /// Match with all its properties, such as players, moves, psositions, statistics and keywords, such date and description.
        /// </param>
        /// <param name="moves">
        /// Datatable with all the moves of the match, as required by the MatchMoveDisplay control.
        /// </param>
        public void SetMatch(Match match, DataTable moves)
        {
            cPlayer.ConnectionIndex = ConnectionIndex;
            cPlayer.Keywords = true;
            cPlayer.CurrentMatch = match;
            mmDisplay.SetMoves(moves);
            Text = match.White + " - " + match.Black + " (" + match.Date + " " + match.Description + ")";
        }
    }
}

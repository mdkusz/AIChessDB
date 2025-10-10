using AIChessDatabase.Data;
using AIChessDatabase.Dialogs;
using AIChessDatabase.Interfaces;
using AIChessDatabase.PGNParser;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase
{
    /// <summary>
    /// Form to play chess matches.
    /// </summary>
    public partial class PlayMatchWindow : Form, IChessDBWindow, IUIRemoteControlElement, IAIChessPlayer
    {
        private IObjectRepository _repository = null;
        private Match _match = null;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;

        public PlayMatchWindow()
        {
            InitializeComponent();
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
            cPlayer.AddComments = true;
        }
        /// <summary>
        /// IChessDBWindow: Window identifier, used to uniquely identify the window in the application.
        /// </summary>
        [Browsable(false)]
        public string WINDOW_ID
        {
            get
            {
                return ID_PLAYWINDOW + _match == null ? "0" : _match.IdMatch.ToString();
            }
        }
        /// <summary>
        /// IChessDBWindow: Database connection index, used to identify the connection in multi-connection scenarios.
        /// </summary>
        public int ConnectionIndex
        {
            get
            {
                return edMatch.ConnectionIndex;
            }
            set
            {
                edMatch.ConnectionIndex = value;
                cPlayer.ConnectionIndex = value;
                pViewer.ConnectionIndex = value;
            }
        }
        /// <summary>
        /// IChessDBWindow: Application services provider.
        /// </summary>
        [Browsable(false)]
        public IAppServiceProvider Provider { get; set; }
        /// <summary>
        /// IChessDBWindow: Gets or sets the database repository used to manage and persist data objects.
        /// </summary>
        [Browsable(false)]
        public IObjectRepository Repository
        {
            get
            {
                return _repository ?? Provider?.SharedRepository;
            }
            set
            {
                _repository = value;
            }
        }
        /// <summary>
        /// IUIRemoteControlElement, IAIChessPlayer: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement, IAIChessPlayer: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return FNAME_PLayMatchWindow; } }
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
        public bool Editable { get { return mmDisplay.AllowAddMoves; } }
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
            try
            {
                if (mmDisplay.AllowAddMoves)
                {
                    mmDisplay.AddNewMove(move, comment);
                    return null;
                }
                else
                {
                    throw new Exception(ERR_NOTEDITABLEMATCH);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
            try
            {
                PGNFile pf = new PGNFile();
                int cnt = pf.ParseString(pgn);
                if (cnt == 0)
                {
                    throw new Exception(ERR_NOMATCHADDED);
                }
                pf.ConvertAllMatches(Repository);
                CurrentMatch = pf.GetMatch(0, Repository);
                return cnt == 1 ? MSG_MATCHADDED : string.Format(MSG_MATCHESADDED, cnt);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// IAIChessPlayer: Cancel the last move made in the match.
        /// </summary>
        public void CancelLastMove()
        {
            if (mmDisplay.AllowAddMoves)
            {
                mmDisplay.DeleteLastMove();
            }
            else
            {
                throw new Exception(ERR_NOTEDITABLEMATCH);
            }
        }
        /// <summary>
        /// Current match to play in the window.
        /// </summary>
        [Browsable(false)]
        public Match CurrentMatch
        {
            get
            {
                return _match;
            }
            set
            {
                _match = value;
                if (_match != null)
                {
                    _repository = _match.Repository;
                    ConnectionIndex = _repository?.GetFreeConnection() ?? -1;
                    if (ConnectionIndex == -1)
                    {
                        throw new InvalidOperationException(ERR_NOAVAILABLECONNECTIONS);
                    }
                    pViewer.Repository = _repository;
                    if (_match.IdMatch != 0)
                    {
                        cPlayer.Keywords = true;
                    }
                    cPlayer.CurrentMatch = _match;
                    edMatch.CurrentMatch = _match;
                    edMatch.AllowSave = true;
                    Text = _match.White + " - " + _match.Black + " (" + _match.Date + " " + _match.Description + ") - " + _repository.ServerName + " (" + _repository.ConnectionName + ")";
                }
            }
        }

        private void mmDisplay_MoveChanged(object sender, EventArgs e)
        {
            try
            {
                edMatch.SetMove(cPlayer.CurrentMove);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void edMatch_CommentsChanged(object sender, EventArgs e)
        {
            cPlayer.ResetComments();
        }

        private void mmDisplay_MatchFinished(object sender, EventArgs e)
        {
            edMatch.AllowSave = true;
        }

        private void PlayMatchWindow_Load(object sender, EventArgs e)
        {
            Icon = Icon.Clone() as Icon;
            splitContainer3.SplitterDistance = splitContainer3.Height / 2;
            splitContainer2.SplitterDistance = 3 * splitContainer2.Height / 4;
        }
        private void PlayMatchWindow_Shown(object sender, EventArgs e)
        {
            try
            {
                if (CurrentMatch == null)
                {
                    DlgNewMatch dlg = new DlgNewMatch()
                    {
                        Repository = Repository,
                    };
                    dlg.ShowDialog(this);
                    if (dlg.Match != null)
                    {
                        CurrentMatch = dlg.Match;
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }
        private void PlayMatchWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _repository?.ReleaseConnection(ConnectionIndex);
        }
    }
}

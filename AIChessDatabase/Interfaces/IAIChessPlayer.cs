using AIChessDatabase.Data;
using System.Collections.Generic;

namespace AIChessDatabase.Interfaces
{
    /// <summary>
    /// Interface for objects that can interact with AI agents to play and comment on chess games.
    /// </summary>
    public interface IAIChessPlayer
    {
        /// <summary>
        /// Chess player unique identifier.
        /// </summary>
        string UID { get; }
        /// <summary>
        /// Form friendly name.
        /// </summary>
        string FriendlyName { get; }
        /// <summary>
        /// Match is editable. New moves can be added, and existing moves can be canceled.
        /// </summary>
        bool Editable { get; }
        /// <summary>
        /// Json object with match information.
        /// </summary>
        Match MatchInfo { get; }
        /// <summary>
        /// List of moves in the match. Each move is a Json object.
        /// </summary>
        List<MatchMove> Moves { get; }
        /// <summary>
        /// Current move in the match.
        /// </summary>
        MatchMove CurrentMove { get; }
        /// <summary>
        /// Current board position.
        /// </summary>
        string CurrentBoard { get; }
        /// <summary>
        /// Advance to the next move in the match.
        /// </summary>
        /// <param name="comment">
        /// Show a comment on the move being made.
        /// </param>
        /// <returns>
        /// A list with three moves: the previous move, the current move, and the next move.
        /// </returns>
        List<MatchMove> NextMove(string comment);
        /// <summary>
        /// Forward to the previous move in the match.
        /// </summary>
        MatchMove PrevMove { get; }
        /// <summary>
        /// Add a move to the match.
        /// </summary>
        /// <param name="move">
        /// Textual representation of the move in Algebraic Notation (AN).
        /// </param>
        /// <param name="comment">
        /// Optional move comment
        /// </param>
        /// <returns>
        /// Error message if the move could not be added, or null if the move was added successfully.
        /// </returns>
        string AddMove(string move, string comment = null);
        /// <summary>
        /// Add a whole match in PGN format.
        /// </summary>
        /// <param name="pgn">
        /// Match in PGN format.
        /// </param>
        /// <returns>
        /// Message indicating success or failure.
        /// </returns>
        string AddMatch(string pgn);
        /// <summary>
        /// Cancel the last move made in the match.
        /// </summary>
        void CancelLastMove();
    }
}

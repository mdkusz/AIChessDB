using AIChessDatabase.Data;
using GlobalCommonEntities.Interfaces;
using System.Collections.Generic;

namespace AIChessDatabase.Interfaces
{
    /// <summary>
    /// Application service provider interface for managing repositories and match operations.
    /// </summary>
    public interface IAppServiceProvider : IDependencyProvider
    {
        /// <summary>
        /// Shared database repository for common operations.
        /// </summary>
        IObjectRepository SharedRepository { get; }
        /// <summary>
        /// New instance of the current shared database repository for operations that require a fresh context.
        /// </summary>
        IObjectRepository NewRepository { get; }
        /// <summary>
        /// List of additional database repositories available in the application.
        /// </summary>
        /// <param name="otherthan">
        /// Repository name to exclude from the list.
        /// </param>
        List<string> OtherRepositories(string otherthan);
        /// <summary>
        /// Create an instance of a database repository with the specified name.
        /// </summary>
        /// <param name="name">
        /// Connection string name.
        /// </param>
        /// <returns>
        /// New database repository instance.
        /// </returns>
        IObjectRepository GetRepository(string name);
        /// <summary>
        /// Launch user interface to edit a match.
        /// </summary>
        /// <param name="match">
        /// Match to edit.
        /// </param>
        void EditMatch(Match match);
        /// <summary>
        /// Launch user interface to play a match.
        /// </summary>
        /// <param name="match">
        /// Match to play.
        /// </param>
        void PlayMatch(Match match);
    }
}

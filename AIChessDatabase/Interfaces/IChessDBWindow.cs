namespace AIChessDatabase.Interfaces
{
    /// <summary>
    /// Window interface for chess database forms.
    /// </summary>
    public interface IChessDBWindow
    {
        /// <summary>
        /// Window identifier, used to uniquely identify the window in the application.
        /// </summary>
        string WINDOW_ID { get; }
        /// <summary>
        /// Database connection index, used to identify the connection in multi-connection scenarios.
        /// </summary>
        int ConnectionIndex { get; set; }
        /// <summary>
        /// Application services provider.
        /// </summary>
        IAppServiceProvider Provider { get; set; }
        /// <summary>
        /// Database repository for data operations.
        /// </summary>
        IObjectRepository Repository { get; set; }
    }
}

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Objects implementing this interface can trace and log Json requests and responses to/from APIs.
    /// </summary>
    public interface IApiJsonLogger
    {
        /// <summary>
        /// Json object as returned by the last API call
        /// </summary>
        string LastAPICallJsonResult { get; }
        /// <summary>
        /// Set a path to a log file to write Json requests and responses. Null or empty to disable logging (default).
        /// </summary>
        string JsonLogPath { get; set; }
    }
}

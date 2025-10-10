using System.Text.Json.Serialization;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Information about a database filter.
    /// </summary>
    public class DatabaseFilterInfo
    {
        /// <summary>
        /// Index in the filter list
        /// </summary>
        [JsonPropertyName("index")]
        public int FilterIndex { get; set; }
        /// <summary>
        /// Human readable text for the filter
        /// </summary>
        [JsonPropertyName("friendly_text")]
        public string FriendlyText { get; set; }
        /// <summary>
        /// Actual sql text for the filter
        /// </summary>
        [JsonPropertyName("sql_text")]
        public string SQLText { get; set; }
    }
}

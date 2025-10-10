using System.Text.Json.Serialization;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Information about a database repository
    /// </summary>
    public class RepositoryInfo
    {
        /// <summary>
        /// Connections tring name
        /// </summary>
        [JsonPropertyName("connection_name")]
        public string ConnectionStringName { get; set; }
        /// <summary>
        /// Database provider name
        /// </summary>
        [JsonPropertyName("provider_name")]
        public string ProviderName { get; set; }
        /// <summary>
        /// Default database connection
        /// </summary>
        [JsonPropertyName("default_databse")]
        public bool Default { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace GlobalCommonEntities.API
{
    /// <summary>
    /// Container for API call error information
    /// </summary>
    public class APICallError
    {
        /// <summary>
        /// Endpoint where the error occurred
        /// </summary>
        [JsonPropertyName("endpoint")]
        public string Enpoint { get; set; }
        /// <summary>
        /// Http method or equivalent used for the request
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; }
        /// <summary>
        /// Request data that caused the error, if available
        /// </summary>
        [JsonPropertyName("request")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Request { get; set; }
        /// <summary>
        /// Message describing the error
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
        /// <summary>
        /// Error code, if available
        /// </summary>
        [JsonPropertyName("code")]
        public int? Code { get; set; }
        /// <summary>
        /// Additional details about the error
        /// </summary>
        [JsonPropertyName("details")]
        public string Details { get; set; }
    }
}

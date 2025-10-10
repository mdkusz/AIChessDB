using System.Text.Json.Serialization;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Information about a database field.
    /// </summary>
    public class DatabaseFieldInfo
    {
        /// <summary>
        /// Field name
        /// </summary>
        [JsonPropertyName("field_name")]
        public string Name { get; set; }
        /// <summary>
        /// Field data type
        /// </summary>
        [JsonPropertyName("data_type")]
        public string DataType { get; set; }
    }
}

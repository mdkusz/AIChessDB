using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// List of non-ISO or server dependant queries.
    /// </summary>
    public class NonStandardQueries
    {
        /// <summary>
        /// Find a non-standard query by server and name.
        /// </summary>
        /// <param name="server">
        /// Server identifier name.
        /// </param>
        /// <param name="name">
        /// Query identifier name.
        /// </param>
        /// <returns>
        /// Query SQL string or null if not found.
        /// </returns>
        public string GetNSQuery(string server, string name)
        {
            return Queries.FirstOrDefault(q => q.Server == server && q.Name == name)?.Query;
        }
        public List<NonStandardQuery> Queries { get; set; }
    }
    /// <summary>
    /// A single non-standard query.
    /// </summary>
    public class NonStandardQuery
    {
        /// <summary>
        /// Server identifier
        /// </summary>
        [JsonPropertyName("server")]
        public string Server { get; set; }
        /// <summary>
        /// Query name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// Query SQL statement
        /// </summary>
        [JsonPropertyName("query")]
        public string Query { get; set; }
    }
}

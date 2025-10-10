using System;
using System.Text.Json.Serialization;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// Identifier for a window in the AI system. Used to return window information to the AI function calls.
    /// </summary>
    public class WindowID : IEquatable<WindowID>
    {
        /// <summary>
        /// Friendly name of the window, for display and user communication purposes.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string FriendlyName { get; set; }
        /// <summary>
        /// Unique identifier for the window, used internally by the AI system to call functions.
        /// </summary>
        [JsonPropertyName("uid")]
        public string UID { get; set; }
        /// <summary>
        /// Name of the current database connection used in this Window, if any.
        /// </summary>
        [JsonPropertyName("connection_name")]
        public string ConnectionName { get; set; }
        /// <summary>
        /// Name of the dabatase server type for the current database connection, if any.
        /// </summary>
        [JsonPropertyName("server_type")]
        public string ServerType { get; set; }
        public bool Equals(WindowID other)
        {
            return other != null && UID == other.UID;
        }
    }
}

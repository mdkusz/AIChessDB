using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.UI
{
    public enum RelevantElementClass
    {
        Control, UserControl, ToolStripItem, ToolStripDropDownItem
    }
    /// <summary>
    /// Class to encapsulate information about a UI element.
    /// </summary>
    public class UIRelevantElement
    {
        /// <summary>
        /// Path to locate the element in the UI tree.
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; }
        /// <summary>
        /// Horizontal position of the element in the UI.
        /// </summary>
        [JsonPropertyName("bounds")]
        public CtlBounds Bounds { get; set; }
        /// <summary>
        /// Role of the element in the UI, such as button, text box, etc.
        /// </summary>
        [JsonPropertyName("role")]
        public string Role { get; set; }
        /// <summary>
        /// Friendly name of the element, which is a human-readable identifier.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string FriendlyName { get; set; }
        /// <summary>
        /// Description of the element, providing additional context or information.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        /// <summary>
        /// Element current state
        /// </summary>
        [JsonPropertyName("state")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string State { get; set; }
        /// <summary>
        /// Current value of the element, if any.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
        /// <summary>
        /// True if the element has child elements, false otherwise.
        /// </summary>
        [JsonPropertyName("has_children")]
        public bool HasChildren { get; set; }
        /// <summary>
        /// Generic class of the element.
        /// </summary>
        [JsonPropertyName("element_class")]
        [JsonConverter(typeof(JsonStringEnumConverter<RelevantElementClass>))]
        public RelevantElementClass ElementClass { get; set; }
        /// <summary>
        /// Children controls list
        /// </summary>
        [JsonPropertyName("children")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<UIRelevantElement> Children { get; set; }
    }
    /// <summary>
    /// Control location and size
    /// </summary>
    public class CtlBounds
    {
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        [JsonPropertyName("width")]
        public int Width { get; set; }
        [JsonPropertyName("height")]
        public int Height { get; set; }
    }
}

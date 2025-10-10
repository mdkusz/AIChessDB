using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// JsonConverter for MatchPosition objects.
    /// </summary>
    /// <remarks>
    /// Only serializes and deserializes the Board property as a string.
    /// </remarks>
    public class MatchPositionConverter : JsonConverter<MatchPosition>
    {
        public override MatchPosition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var boardString = reader.GetString();

            return new MatchPosition
            {
                Board = new Position { Board = boardString }
            };
        }

        public override void Write(Utf8JsonWriter writer, MatchPosition value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.Board?.Board);
        }
    }
}

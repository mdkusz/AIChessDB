using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Json.Converters
{
    /// <summary>
    /// Converts DateTime to and from Unix time (seconds since 1970-01-01T00:00:00Z).
    /// </summary>
    public class UnixDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long unixTime = reader.GetInt64();
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            long unixTime = ((DateTimeOffset)value).ToUnixTimeSeconds();
            writer.WriteNumberValue(unixTime);
        }
    }
}

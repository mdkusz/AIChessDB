using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Json.Converters
{
    /// <summary>
    /// Converts a string to/from Base64 for JSON serialization.
    /// </summary>
    public class Base64StringJsonConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var base64 = reader.GetString() ?? "";
            try
            {
                var bytes = Convert.FromBase64String(base64);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return base64;
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(value ?? "");
            string base64 = Convert.ToBase64String(bytes);
            writer.WriteStringValue(base64);
        }
    }
}

using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Json.Converters
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            uint uintColor = reader.GetUInt32();
            int intColor = unchecked((int)uintColor);
            return Color.FromArgb(intColor);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            uint intColor = (uint)value.ToArgb();
            writer.WriteNumberValue(intColor);
        }
    }
}

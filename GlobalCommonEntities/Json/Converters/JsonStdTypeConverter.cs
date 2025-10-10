using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Json.Converters
{
    public class JsonStdTypeConverter : JsonConverter<Type>
    {
        private static readonly Dictionary<string, Type> PrimitiveTypes = new Dictionary<string, Type>
        {
            { "bool", typeof(bool) },
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) },
            { "char", typeof(char) },
            { "decimal", typeof(decimal) },
            { "double", typeof(double) },
            { "float", typeof(float) },
            { "int", typeof(int) },
            { "uint", typeof(uint) },
            { "long", typeof(long) },
            { "ulong", typeof(ulong) },
            { "short", typeof(short) },
            { "ushort", typeof(ushort) },
            { "string", typeof(string) },
            { "object", typeof(object) },
            { "DateTime", typeof(DateTime) }
        };

        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string typeName = reader.GetString();
            if (PrimitiveTypes.TryGetValue(typeName, out Type type))
            {
                return type;
            }
            return Type.GetType(typeName);
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Json.Converters
{
    /// <summary>
    /// Use this converter when a GenericType can be an array or dictionary
    /// </summary>
    public class ArrayJsonConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string typeName = reader.GetString();
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            return ResolveType(typeName);
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(UnResolveType(value));
        }

        private Type ResolveType(string typeName)
        {
            switch (typeName)
            {
                case "byte[]":
                    return typeof(byte[]);
                case "int[]":
                    return typeof(int[]);
                case "string[]":
                    return typeof(string[]);
                case "List<string>":
                    return typeof(List<string>);
                case "Dictionary<string, int>":
                    return typeof(Dictionary<string, int>);
                case "Dictionary<string, object>":
                    return typeof(Dictionary<string, object>);
                default:
                    return Type.GetType(typeName);
            }
        }
        private string UnResolveType(Type type)
        {
            if (type == null)
            {
                return null;
            }
            if (type == typeof(byte[]))
            {
                return "byte[]";
            }
            if (type == typeof(int[]))
            {
                return "int[]";
            }
            if (type == typeof(string[]))
            {
                return "string[]";
            }
            if (type == typeof(List<string>))
            {
                return "List<string>";
            }
            if (type == typeof(Dictionary<string, int>))
            {
                return "Dictionary<string, int>";
            }
            if (type == typeof(Dictionary<string, object>))
            {
                return "Dictionary<string, object>";
            }
            return type.AssemblyQualifiedName;
        }
    }
}

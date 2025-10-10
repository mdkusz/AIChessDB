using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Json.Converters
{
    /// <summary>
    /// Use this converter to convet GenericType properties from and to string
    /// </summary>
    public class JsonTypeAssemblyNameConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string typeName = reader.GetString();
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            Type type = Type.GetType(typeName);
            return type;
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteStringValue(null as string);
            }
            else
            {
                writer.WriteStringValue(value.AssemblyQualifiedName);
            }
        }
    }
    /// <summary>
    /// Convert a GenericType from and to a JSON string
    /// </summary>
    public class JsonTypeAssemblyLoadConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string assemblyQualifiedName = reader.GetString();
            Type type = Type.GetType(assemblyQualifiedName);

            if (type == null)
            {
                // Try load the assembly
                string assemblyName = new AssemblyName(assemblyQualifiedName).Name;
                Assembly assembly = Assembly.Load(assemblyName);

                if (assembly != null)
                {
                    type = assembly.GetType(assemblyQualifiedName);
                }
            }

            return type;
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.AssemblyQualifiedName);
        }
    }
}

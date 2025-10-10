using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.Json
{
    /// <summary>
    /// Provides functionality to flatten any object (with JsonPropertyName‑decorated properties)
    /// into a flat dictionary where nested properties become dotted keys.
    /// </summary>
    public static class JsonFlattener
    {
        /// <summary>
        /// Serializes <paramref name="obj"/> to JSON (honoring your
        /// <see cref="JsonPropertyNameAttribute"/>s), parses it as a JsonDocument tree,
        /// and produces a <see cref="Dictionary{TKey, TValue}"/> whose keys are paths like
        /// "parent.child[0].grandchild" and whose values are the corresponding CLR values.
        /// </summary>
        /// <param name="obj">
        /// Any instance whose properties are decorated with
        /// <see cref="JsonPropertyNameAttribute"/> to control JSON names.
        /// </param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> mapping flattened
        /// JSON‑property paths to boxed values (<c>string</c>, <c>long</c>,
        /// <c>double</c>, <c>bool</c>, <c>DateTime</c>, or <c>null</c>).
        /// </returns>
        public static Dictionary<string, object> Flatten(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            // 1) Serialize respecting JsonPropertyName attributes
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                DictionaryKeyPolicy = null
            };
            string json = JsonSerializer.Serialize(obj, options);

            // 2) Parse into JsonDocument using classic using-statement
            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                FlattenElement(doc.RootElement, prefix: "", result);
            }

            return result;
        }

        /// <summary>
        /// Recursively descends a <see cref="JsonElement"/>, building up
        /// dotted keys and filling the <paramref name="dict"/> with
        /// primitive values at each leaf.
        /// </summary>
        private static void FlattenElement(JsonElement element, string prefix, Dictionary<string, object> dict)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        string key = string.IsNullOrEmpty(prefix)
                            ? prop.Name
                            : $"{prefix}.{prop.Name}";
                        FlattenElement(prop.Value, key, dict);
                    }
                    break;

                case JsonValueKind.Array:
                    int index = 0;
                    foreach (var item in element.EnumerateArray())
                    {
                        string key = $"{prefix}[{index++}]";
                        FlattenElement(item, key, dict);
                    }
                    break;

                case JsonValueKind.String:
                    if (element.TryGetDateTime(out DateTime dt))
                        dict[prefix] = dt;
                    else
                        dict[prefix] = element.GetString();
                    break;

                case JsonValueKind.Number:
                    if (element.TryGetInt64(out long l))
                        dict[prefix] = l;
                    else
                        dict[prefix] = element.GetDouble();
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    dict[prefix] = element.GetBoolean();
                    break;

                case JsonValueKind.Null:
                    dict[prefix] = null;
                    break;

                default:
                    dict[prefix] = element.GetRawText();
                    break;
            }
        }
    }
}

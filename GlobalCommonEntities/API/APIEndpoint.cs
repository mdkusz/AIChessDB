using GlobalCommonEntities.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GlobalCommonEntities.API
{
    /// <summary>
    /// API endpoint settings
    /// </summary>
    /// <remarks>
    /// This is the definition of enpoints for all AI APIs.
    /// </remarks>
    public class APIEndpoint
    {
        /// <summary>
        /// Endpoint friendly name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// Endporint friendly description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        /// <summary>
        /// Endpoint section. Internal use to classify and find the proper endpoint 
        /// </summary>
        [JsonPropertyName("section")]
        public string Section { get; set; }
        /// <summary>
        /// HTTP method or equivalent
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; }
        /// <summary>
        /// Timeout in seconds
        /// </summary>
        [JsonPropertyName("timeout")]
        public int TimeOut { get; set; } = 0;
        /// <summary>
        /// Endpoint url
        /// </summary>
        [JsonPropertyName("url")]
        public string URL { get; set; }
        /// <summary>
        /// Path parameters
        /// </summary>
        [JsonPropertyName("pathParams")]
        public List<UrlParameter> PathParameters { get; set; }
        /// <summary>
        /// URL parameters
        /// </summary>
        [JsonPropertyName("queryParams")]
        public List<UrlParameter> QueryParameters { get; set; }
        /// <summary>
        /// Request object datatype
        /// </summary>
        [JsonPropertyName("requestType")]
        [JsonConverter(typeof(ArrayJsonConverter))]
        public Type requestType { get; set; }
        /// <summary>
        /// Response object datatype
        /// </summary>
        [JsonPropertyName("returnType")]
        [JsonConverter(typeof(ArrayJsonConverter))]
        public Type returnType { get; set; }

        /// <summary>
        /// Get a list of possible url arguments
        /// </summary>
        /// <returns></returns>
        public List<ExtraArgs> GetUrlArgs()
        {
            if (QueryParameters == null)
            {
                return null;
            }
            List<ExtraArgs> args = new List<ExtraArgs>();
            for (int ix = 0; ix < QueryParameters.Count; ix++)
            {
                UrlParameter p = QueryParameters[ix];
                args.Add(new ExtraArgs() { Position = ix, Name = p.Name, type = Type.GetType(p.Type), Description = p.Description, Optional = p.Optional ?? true, Values = p.Values });
            }
            return args;
        }
    }
    /// <summary>
    /// Url parameter
    /// </summary>
    public class UrlParameter
    {
        /// <summary>
        /// Parameter name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// Parameter datatype
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
        /// <summary>
        /// Parameter description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        /// <summary>
        /// Optional parameter
        /// </summary>
        [JsonPropertyName("optional")]
        public bool? Optional { get; set; }
        /// <summary>
        /// List of possible values
        /// </summary>
        [JsonPropertyName("values")]
        public List<object> Values { get; set; }
    }
}

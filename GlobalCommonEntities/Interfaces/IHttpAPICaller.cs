using GlobalCommonEntities.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Generic interface for all classes that make API calls using HttpClient
    /// </summary>
    public interface IHttpAPICaller
    {
        /// <summary>
        /// Get a HttpClient to perform API calls
        /// </summary>
        /// <returns>
        /// Authorized HttpClient with all necessary headers
        /// </returns>
        HttpClient GetHttpClient();
        /// <summary>
        /// Get a HttpClient to perform API calls
        /// </summary>
        /// <returns>
        /// Authorized HttpClient with all necessary headers
        /// </returns>
        Task<HttpClient> GetHttpClientAsync();
        /// <summary>
        /// Release a HttpClient
        /// </summary>
        /// <param name="client">
        /// HttpClient to realease
        /// </param>
        void ReleaseHttpClient(HttpClient client);
        /// <summary>
        /// Make an API call to a given endpoint
        /// </summary>
        /// <param name="requestBody">
        /// Object to send to the API
        /// </param>
        /// <param name="endpoint">
        /// Endpoint url to send the request to
        /// </param>
        /// <param name="method">
        /// HTTP method
        /// </param>
        /// <param name="responseType">
        /// Response type for serialization
        /// </param>
        /// <returns>
        /// Response object
        /// </returns>
        /// <remarks>
        /// This API call version uses the default HttpClient singleton.
        /// </remarks>
        Task<ObjectWrapper> APICallAsync(object requestBody, string endpoint, HttpMethod method, Type responseType);
        /// <summary>
        /// Make an API call to a given endpoint using a given HttpClient
        /// </summary>
        /// <param name="client">
        /// Authorized HttpClient with all necessary headers
        /// </param>
        /// <param name="requestBody">
        /// Object to send to the API
        /// </param>
        /// <param name="endpoint">
        /// Endpoint url to send the request to
        /// </param>
        /// <param name="method">
        /// HTTP method
        /// </param>
        /// <param name="responseType">
        /// Response type for serialization
        /// </param>
        /// <returns>
        /// Response object
        /// </returns>
        /// <remarks>
        /// HttpClient is a singleton by default, but you can use this method if you need to perform parallel calls or for some other reasons.
        /// </remarks>
        Task<ObjectWrapper> APICallAsync(HttpClient client, object requestBody, string endpoint, HttpMethod method, Type responseType);
    }
}

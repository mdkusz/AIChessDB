using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalCommonEntities.API
{
    /// <summary>
    /// Base class to create simple API cllers
    /// </summary>
    /// <remarks>
    /// This class provides a simple way to make API calls using HttpClient.
    /// Derived classes must implement GetHttpClient method to provide an authorized HttpClient.
    /// </remarks>
    /// <seealso cref="ObjectWrapper"/>
    /// <seealso cref="IHttpAPICaller"/>
    public abstract class GenericHttpAPICaller : IHttpAPICaller, IAppResourcesConsumer, IApiJsonLogger
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        protected bool _staticHTTPClient = false;
        protected string _lastJson = null;
        /// <summary>
        /// IApiJsonLogger: Json object as returned by the last API call
        /// </summary>
        public string LastAPICallJsonResult { get { return _lastJson; } }
        /// <summary>
        /// IApiJsonLogger: Set a path to a log file to write Json requests and responses. Null or empty to disable logging (default).
        /// </summary>
        public string JsonLogPath { get; set; }
        /// <summary>
        /// IHttpAPICaller: Make an API call to a given endpoint
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
        public virtual async Task<ObjectWrapper> APICallAsync(object requestBody, string endpoint, HttpMethod method, Type responseType)
        {
            if (_staticHTTPClient)
            {
                return await APICallAsync(await GetHttpClientAsync(), requestBody, endpoint, method, responseType);
            }
            else
            {
                using (HttpClient client = await GetHttpClientAsync())
                {
                    return await APICallAsync(client, requestBody, endpoint, method, responseType);
                }
            }
        }
        /// <summary>
        /// IAppRespurcesConsumer: Resources repository
        /// </summary>
        public ResourcesRepository AllResources { get; set; }
        /// <summary>
        /// IHttpAPICaller: Get a HttpClient to perform API calls
        /// </summary>
        /// <returns>
        /// Authorized HttpClient with all necessary headers
        /// </returns>
        public abstract HttpClient GetHttpClient();
        /// <summary>
        /// IHttpAPICaller: Get a HttpClient to perform API calls
        /// </summary>
        /// <returns>
        /// Authorized HttpClient with all necessary headers
        /// </returns>
        public virtual async Task<HttpClient> GetHttpClientAsync()
        {
            return await Task.Run(() => { return GetHttpClient(); });
        }
        /// <summary>
        /// IHttpAPICaller: Release a HttpClient
        /// </summary>
        /// <param name="client">
        /// Client to release
        /// </param>
        public virtual void ReleaseHttpClient(HttpClient client)
        {
            if (!_staticHTTPClient)
            {
                client.Dispose();
            }
        }
        /// <summary>
        /// IHttpAPICaller: Make an API call to a given endpoint using a given HttpClient
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
        public virtual async Task<ObjectWrapper> APICallAsync(HttpClient client, object requestBody, string endpoint, HttpMethod method, Type responseType)
        {
            try
            {
                HttpResponseMessage response;
                string jsonString = "";
                _lastJson = "";
                if (method == HttpMethod.Post)
                {
                    if (requestBody == null)
                    {
                        response = await client.PostAsync(endpoint, null);
                    }
                    else if (requestBody is MultipartFormDataContent)
                    {
                        response = await client.PostAsync(endpoint, requestBody as MultipartFormDataContent);
                    }
                    else
                    {
#if DEBUG
                        jsonString = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
#else
                    jsonString = JsonSerializer.Serialize(requestBody);
#endif
                        if (!string.IsNullOrEmpty(JsonLogPath))
                        {
                            await LogJsonAsync(endpoint, jsonString);
                        }

                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(endpoint, content);
                    }
                }
                else if (method == HttpMethod.Get)
                {
                    response = await client.GetAsync(endpoint);
                }
                else if (method == HttpMethod.Put)
                {
                    if (requestBody == null)
                    {
                        response = await client.PutAsync(endpoint, null);
                    }
                    else if (requestBody is MultipartFormDataContent)
                    {
                        response = await client.PutAsync(endpoint, requestBody as MultipartFormDataContent);
                    }
                    else
                    {
#if DEBUG
                        jsonString = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
#else
                    jsonString = JsonSerializer.Serialize(requestBody);
#endif
                        if (!string.IsNullOrEmpty(JsonLogPath))
                        {
                            await LogJsonAsync(endpoint, jsonString);
                        }
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        response = await client.PutAsync(endpoint, content);
                    }
                }
                else if (method == HttpMethod.Delete)
                {
                    response = await client.DeleteAsync(endpoint);
                }
                else
                {
                    string error = AllResources?.GetString("ERR_HTTPMethod");
                    if (string.IsNullOrEmpty(error))
                    {
                        error = "Unsupported HTTP method.";
                    }
                    throw new Exception(error);
                }

                object apiResponse = null;

                if (response.IsSuccessStatusCode)
                {
                    if (responseType != null)
                    {
                        if (responseType == typeof(byte[]))
                        {
                            apiResponse = await response.Content.ReadAsByteArrayAsync();
                        }
                        else if (responseType == typeof(string))
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            _lastJson = await response.Content.ReadAsStringAsync();
                            apiResponse = JsonSerializer.Deserialize(_lastJson, responseType);

                            if (!string.IsNullOrEmpty(JsonLogPath))
                            {
                                await LogJsonAsync(endpoint, _lastJson);
                            }
                        }
                    }
                    else
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                    }
                    return new ObjectWrapper(apiResponse);
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    string error = AllResources?.GetString("ERR_Request");
                    if (string.IsNullOrEmpty(error))
                    {
                        error = "Request failed with status code {0}: {1}";
                    }
                    throw new Exception(string.Format(error, response.StatusCode, responseContent));
                }
            }
            catch (Exception ex)
            {
                APICallError error = new APICallError()
                {
                    Enpoint = endpoint,
                    Method = method.Method,
                    Request = requestBody,
                    Message = ex.Message,
                    Details = ex.StackTrace
                };
                return new ObjectWrapper(error);
            }
        }
        /// <summary>
        /// Log a Json object to the log file
        /// </summary>
        /// <param name="endpoint">
        /// Endpoint that consume or produced the json
        /// </param>
        /// <param name="json">
        /// Json string to log
        /// </param>
        private async Task LogJsonAsync(string endpoint, string json)
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                using (StreamWriter sw = new StreamWriter(JsonLogPath, true))
                {
                    await sw.WriteLineAsync(endpoint).ConfigureAwait(false);
                    await sw.WriteLineAsync(json).ConfigureAwait(false);
                }
            }
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}

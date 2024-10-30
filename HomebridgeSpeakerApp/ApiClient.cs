using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HomebridgeSpeakerApp
{
    public class ApiClient : IDisposable
    {
        private readonly HttpClient httpClient;

        public ApiClient()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Sends a GET request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>The response as a string.</returns>
        public async Task<string> GetAsync(string url, Dictionary<string, string> headers)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    // Add headers to the request
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }

                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Sends a PUT request to the specified URL with the given JSON content.
        /// </summary>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="jsonContent">The JSON content to send in the PUT request.</param>
        /// <returns>The response as a string.</returns>
        public async Task<HttpResponseMessage> PutAsync(string url, string jsonContent, Dictionary<string, string> headers)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                // Set the JSON content
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Add headers to the request
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                // Send the request and return the full response
                return await httpClient.SendAsync(request);
            }
        }

        public async Task<string> PostAsync(string url, string jsonContent)
        {
            try
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Disposes the HttpClient instance.
        /// </summary>
        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}

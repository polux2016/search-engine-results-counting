using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SearchEngineResultsCounting.Services.Contract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchEngineResultsCounting.Services
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly ILogger<HttpClientWrapper> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private JsonSerializerSettings _jsonSerializerSettings;
        private readonly Dictionary<string, string[]> _headerValues;

        public HttpClientWrapper(
            ILogger<HttpClientWrapper> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;

            _headerValues = new Dictionary<string, string[]>();

            SetDefaultJsonSettings();
        }

        public void AddValueToHeader(string key, string[] values)
        {
            _headerValues.Add(key, values);
        }

        public async Task<T> GetJsonAsync<T>(string url) where T : new()
        {
            _logger.LogDebug($"GetJsonAsync for url = {url}");
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();

                AddsHeadersToHttpClient(httpClient); 
                
                var jsonString = await httpClient.GetStringAsync(url);

                return JsonConvert.DeserializeObject<T>(jsonString, _jsonSerializerSettings);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Not possible to process url: {url}.");
                return new T();
            }
        }

        private void AddsHeadersToHttpClient(HttpClient httpClient)
        {
            foreach (var headerValue in _headerValues)
            {
                httpClient.DefaultRequestHeaders.Add(headerValue.Key, headerValue.Value);
            }
        }

        private void SetDefaultJsonSettings()
        {
            SetJsonSerializerSettings(new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }

        private void SetJsonSerializerSettings(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
        }
    }
}

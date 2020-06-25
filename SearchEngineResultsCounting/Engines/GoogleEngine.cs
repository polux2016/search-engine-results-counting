using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using System;
using System.Net.Http;
using System.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SearchEngineResultsCounting.Engines
{
    public class GoogleEngine : ISearchEngine
    {
        const string urlFormat = "https://www.googleapis.com/customsearch/v1?key={0}&cx=017576662512468239146:omuauf_lfve&q={1}";
        private readonly string _apiKey;
        private readonly ILogger<GoogleEngine> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public string Name { get; } = "Google Engine";

        public GoogleEngine(ILogger<GoogleEngine> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _apiKey = config.GetSection("GoogleEngineConfig:ApiKey").Get<string>();
        }

        public async Task<long> GetResultsCount(string text)
        {
            var resultsCount = await GetGoogleCount(text);
            _logger.LogDebug($"{Name} get {resultsCount} for {text}");
            return resultsCount;
        }

        private async Task<long> GetGoogleCount(string text)
        {
            string responseStr = await GetString(GetUrl(text));
            JsonValue response = null;
            try
            {
                response = JsonObject.Parse(responseStr);
                return long.Parse(response["searchInformation"]["totalResults"]);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't parse the response. Msg: {ex.Message}. response string {responseStr}");
                throw ex;
            }
        }

        protected virtual async Task<string> GetString(string url)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {   
                _logger.LogDebug($"GetString for url = {url}");
                return await httpClient.GetStringAsync(url);
            }
        }

        private string GetUrl(string text)
        {
            return string.Format(urlFormat, _apiKey, Uri.EscapeDataString(text));
        }

        public class Config
        {
            public String ApiKey;
        }
    }
}
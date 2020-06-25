using System;
using System.Net.Http;
using System.Json;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SearchEngineResultsCounting.Engines
{
    public class MsnEngine : ISearchEngine
    {
        const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";
        private readonly string _accessKey;
        private readonly ILogger<MsnEngine> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public string Name { get; } = "MSN";
        public MsnEngine(ILogger<MsnEngine> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _accessKey = config["MsnEngineConfig:AccessKey"];
        }

        public async Task<long> GetResultsCount(string text)
        {
            var resultsCount = await GetMsnCount(text);
            _logger.LogDebug($"{Name} get {resultsCount} for {text}");
            return resultsCount;
        }

        private async Task<long> GetMsnCount(string text)
        {
            string responseStr = await GetString(GetUrl(text));
            JsonValue response = null;
            try
            {
                response = JsonObject.Parse(responseStr);
                return response["totalEstimatedMatches"];
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
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", new string[] { _accessKey });
                return await httpClient.GetStringAsync(url);
            }
        }

        private string GetUrl(string text)
        {
            return uriBase + "?q=" + Uri.EscapeDataString(text);
        }

        public class Config
        {
            public string AccessKey;
        }
    }
}
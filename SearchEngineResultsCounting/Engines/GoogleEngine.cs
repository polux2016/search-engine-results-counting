using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using System;
using System.Net.Http;
using System.Json;
using System.Threading.Tasks;

namespace SearchEngineResultsCounting.Engines
{
    public class GoogleEngine : ISearchEngine
    {
        const string urlFormat = "https://www.googleapis.com/customsearch/v1?key={0}&cx=017576662512468239146:omuauf_lfve&q={1}";
        const string apiKey = "AIzaSyCp3LWWCVszsF_ES1JXA5OsRA27If3CGHU";
        private readonly ILogger<GoogleEngine> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public string Name { get; } = "Google Engine";

        public GoogleEngine(ILogger<GoogleEngine> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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
            return string.Format(urlFormat, apiKey, Uri.EscapeDataString(text));
        }
    }
}
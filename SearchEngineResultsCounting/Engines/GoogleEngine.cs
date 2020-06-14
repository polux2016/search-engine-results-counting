using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Engines.Contract;
using System;
using System.Net.Http;
using System.Text.Json;

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

        public long GetResultsCount(string text)
        {
            var resultsCount = GetGoogleCount(text);
            _logger.LogDebug($"{Name} get {resultsCount} for {text}");
            return resultsCount;
        }

        private long GetGoogleCount(string text)
        {
            string responseStr = GetString(GetUrl(text));
            GoogleEngineContract.Root response = null;
            try
            {
                response = JsonSerializer.Deserialize<GoogleEngineContract.Root>(responseStr);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't parse the response. Msg: {ex.Message}. response string {responseStr}");
            }
            return response == null ? -1 : long.Parse(response.searchInformation.totalResults);
        }

        protected virtual string GetString(string url)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {   
                _logger.LogDebug($"GetString for url = {url}");
                return httpClient.GetStringAsync(url).GetAwaiter().GetResult();
            }
        }

        private string GetUrl(string text)
        {
            return string.Format(urlFormat, apiKey, Uri.EscapeDataString(text));
        }
    }
}
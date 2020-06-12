using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Engines.Contract;

namespace SearchEngineResultsCounting.Engines
{
    public class MsnEngine : ISearchEngine
    {
        const string accessKey = "7947afa74a6647eaa3838c2cfc394ebf";
        const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";
        private readonly ILogger<MsnEngine> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public string Name { get; } = "MSN";
        public MsnEngine(ILogger<MsnEngine> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        
        public long GetResultsCount(string text)
        {
            var resultsCount = 0;//GetMsnCount(text);
            _logger.LogDebug($"{Name} get {resultsCount} for {text}");
            return resultsCount;
        }

        private long GetMsnCount(string text)
        {
            using(var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", new string[] {accessKey});
                var responseStr = httpClient.GetStringAsync(GetUrl(text)).GetAwaiter().GetResult();
                MsnEngineContract.Root response = null;
                try {
                    response = JsonSerializer.Deserialize<MsnEngineContract.Root>(responseStr);
                } catch (Exception ex)
                {
                    _logger.LogError($"Can't parse the response. Msg: {ex.Message}. response string {responseStr}");
                }
                return response == null ? -1 : response.totalEstimatedMatches;
            }
        }

        private string GetUrl(string text)
        {
            return uriBase + "?q=" + Uri.EscapeDataString(text);
        }
    }
}
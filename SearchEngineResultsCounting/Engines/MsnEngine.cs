using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Contracts.Configurations;
using SearchEngineResultsCounting.Services.Contract;
using System;
using System.Threading.Tasks;

namespace SearchEngineResultsCounting.Engines
{
    public class MsnEngine : ISearchEngine
    {
        private const string UriBase = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";

        private const string OcpApimSubscriptionKey = "Ocp-Apim-Subscription-Key";
        
        private readonly ILogger<MsnEngine> _logger;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly MsnEngineConfiguration _msnEngineConfiguration;

        public string Name => nameof(MsnEngine);

        public MsnEngine(ILogger<MsnEngine> logger,
            IHttpClientWrapper httpClientWrapper,
            IOptions<MsnEngineConfiguration> msnEngineConfiguration)
        {
            _logger = logger;
            _httpClientWrapper = httpClientWrapper;
            _msnEngineConfiguration = msnEngineConfiguration.Value;
        }

        public async Task<long> GetResultsCount(string text)
        {
            var resultsCount = await GetMsnCount(text);
            _logger.LogDebug($"{Name} get {resultsCount} for {text}");
            return resultsCount;
        }

        private async Task<long> GetMsnCount(string text)
        {
            if (!_msnEngineConfiguration.Enabeld)
            {
                _logger.LogInformation($"{nameof(MsnEngine)} disabled.");

                return 0;
            }

            var url = GetUrl(text);

            _httpClientWrapper.AddValueToHeader(OcpApimSubscriptionKey, new[]
            {
                _msnEngineConfiguration.AccessKey
            });

            var response = await _httpClientWrapper.GetJsonAsync<MsnEngineResponse>(url);

            return response.TotalEstimatedMatches;
        }

        private string GetUrl(string text)
        {
            return UriBase + "?q=" + Uri.EscapeDataString(text);
        }
    }
}
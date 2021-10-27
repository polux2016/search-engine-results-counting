using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Contracts.Configurations;
using SearchEngineResultsCounting.Services.Contract;
using System;
using System.Threading.Tasks;

namespace SearchEngineResultsCounting.Engines
{
    public class GoogleEngine : ISearchEngine
    {
        private const string UrlFormat = "https://www.googleapis.com/customsearch/v1?key={0}&cx=017576662512468239146:omuauf_lfve&q={1}";

        private readonly ILogger<GoogleEngine> _logger;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly GoogleEngineConfiguration _googleEngineConfiguration;

        public string Name => nameof(GoogleEngine);

        public GoogleEngine(ILogger<GoogleEngine> logger,
            IHttpClientWrapper httpClientWrapper,
            IOptions<GoogleEngineConfiguration> googleEngineConfiguration)
        {
            _logger = logger;
            _httpClientWrapper = httpClientWrapper;
            _googleEngineConfiguration = googleEngineConfiguration.Value;
        }

        public async Task<long> GetResultsCount(string text)
        {
            var resultsCount = await GetGoogleCount(text);

            _logger.LogDebug($"{Name} get {resultsCount} for {text}");

            return resultsCount;
        }

        private async Task<long> GetGoogleCount(string text)
        {
            if (!_googleEngineConfiguration.Enabeld)
            {
                _logger.LogInformation($"{nameof(GoogleEngine)} disabled.");

                return 0;
            }

            var url = GetUrl(text);

            var googleEngineResponse = await _httpClientWrapper.GetJsonAsync<GoogleEngineResponse>(url);

            return googleEngineResponse.SearchInformation.TotalResults;
        }

        private string GetUrl(string text)
        {
            return string.Format(UrlFormat, _googleEngineConfiguration.ApiKey, Uri.EscapeDataString(text));
        }
    }
}
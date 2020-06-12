using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.Engines
{
    public class GoogleEngine : ISearchEngine
    {
        private readonly ILogger<GoogleEngine> _logger;

        public string Name { get; } = "Google Engine";

        public GoogleEngine(ILogger<GoogleEngine> logger)
        {
            _logger = logger;
        }

        public int GetResultsCount(string text)
        {
            var resultsCount = text.Length;
            _logger.LogDebug($"{Name} get {resultsCount} for {text}");
            return resultsCount;
        }
    }
}
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.Engines
{
    public class MsnEngine : ISearchEngine
    {
        private readonly ILogger<MsnEngine> _logger;

        public string Name { get; } = "MSN";
        public MsnEngine(ILogger<MsnEngine> logger)
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
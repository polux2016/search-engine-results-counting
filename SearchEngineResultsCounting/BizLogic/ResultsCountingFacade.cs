using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.BizLogic
{
    public class ResultsCountingFacade : IResultsCountingFacade
    {
        private readonly IEnumerable<ISearchEngine> _engines;

        private readonly ILogger<ResultsCountingFacade> _logger;

        public ResultsCountingFacade(IEnumerable<ISearchEngine> engines,
            ILogger<ResultsCountingFacade> logger)
        {
            _engines = engines;
            _logger = logger;
        }

        public string FindAndCompareResults(string text)
        {
            return FormatResults(GetResults(text));
        }

        private ConcurrentDictionary<string, int> GetResults(string text)
        {
            var results = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(_engines, engine =>
            {
                if (!results.TryAdd(engine.Name, engine.GetResultsCount(text)))
                {
                    _logger.LogError($"Can't add {engine.Name}. It already exists in results.");
                }
            });

            return results;
        }

        private string FormatResults(ConcurrentDictionary<string, int> results)
        {
            return $"Results.Count = {results.Count}";
        }
    }
}
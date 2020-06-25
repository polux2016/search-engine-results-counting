using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.BizLogic.Contract;
using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.BizLogic
{
    public class ResultsCountingFacade : IResultsCountingFacade
    {
        private readonly IEnumerable<ISearchEngine> _engines;

        private readonly ILogger<ResultsCountingFacade> _logger;

        private readonly IEnumerable<IAggregator> _aggregators;

        public ResultsCountingFacade(IEnumerable<ISearchEngine> engines,
            ILogger<ResultsCountingFacade> logger,
            IEnumerable<IAggregator> aggregators)
        {
            _engines = engines;
            _logger = logger;
            _aggregators = aggregators;
        }

        public string FindAndCompareResults(string[] texts)
        {
            if (!Validation(texts)) { return ""; }

            var summaryResult = new StringBuilder();
            var textResults = AppendResults(texts, summaryResult);

            foreach(var aggregator in _aggregators)
            {
                aggregator.Append(textResults, summaryResult);
            }

            return summaryResult.ToString();
        }

        private bool Validation(string[] texts)
        {
            if (texts is null)
            {
                throw new ArgumentNullException();
            }

            if (texts.Length == 0)
            {
                _logger.LogInformation("No text to find");
                return false;
            }

            if (_engines.Count() == 0)
            {
                _logger.LogInformation("No engines to search");
                return false;
            }

            return true;
        }

        private List<EngineResult> AppendResults(string[] texts, StringBuilder summaryResult)
        {
            var textResults = new List<EngineResult>();
            object sync = new Object();

            var promises = texts.Select(async text =>
                {
                    var currentResults = await GetResults(text);
                    foreach (var item in currentResults)
                    {
                        textResults.Add(item);
                    }
                }
            );

            try
            {
                Task.WhenAll(promises).GetAwaiter().GetResult();
            }
            catch (AggregateException)
            {
                _logger.LogError("Not all text results success.");
                throw;
            }

            return textResults.ToList();
        }

        private async Task<List<EngineResult>> GetResults(string text)
        {
            _logger.LogDebug($"Processing {text}");

            var results = new ConcurrentBag<EngineResult>();
            var promises = _engines.Select(async engine =>
                {
                    var engineResult = new EngineResult()
                    {
                        EngineName = engine.Name,
                        Text = text,
                        Count = await engine.GetResultsCount(text)
                    };
                    results.Add(engineResult);
                }
            );

            try
            {
                await Task.WhenAll(promises);
            }
            catch (AggregateException)
            {
                _logger.LogError("Not all engines success.");
                throw;
            }

            return results.OrderBy(r => r.Count).ToList();
        }
    }
}
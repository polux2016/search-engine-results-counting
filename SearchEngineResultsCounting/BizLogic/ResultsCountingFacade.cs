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

        public ResultsCountingFacade(IEnumerable<ISearchEngine> engines,
            ILogger<ResultsCountingFacade> logger)
        {
            _engines = engines;
            _logger = logger;
        }

        public string FindAndCompareResults(string[] texts)
        {
            if (!Validation(texts)) { return ""; }

            var summaryResult = new StringBuilder();
            var textResults = AppendResults(texts, summaryResult);

            AppendEnginesWinner(textResults, summaryResult);

            AppendTotalWinner(textResults, summaryResult);

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
                    summaryResult.AppendLine($"{text}: {FormatResults(currentResults)}");
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

        private void AppendTotalWinner(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (textResults.Count == 0)
            {
                _logger.LogInformation("No text results to append total winner.");
                return;
            }

            var groupResults = textResults.GroupBy(engineResult => engineResult.Text)
                .Select(g => new
                {
                    Text = g.First().Text,
                    Sum = g.Sum(er => er.Count)
                });
            var maxSum = groupResults.Max(gr => gr.Sum);
            var winners = string.Join(", ", groupResults.OrderBy(gr => gr.Text)
                .Where(gr => gr.Sum == maxSum)
                .Select(gr => gr.Text));
            summaryResult.AppendLine($"Total winner(s): {winners}");
        }

        private void AppendEnginesWinner(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (textResults.Count == 0)
            {
                _logger.LogInformation("No text results to append engines winner.");
                return;
            }

            foreach (var group in textResults.GroupBy(engineResult => engineResult.EngineName))
            {
                if (group is null) continue;

                var maxCount = group.Max(er => er.Count);
                var resultLine = string.Join(", ",
                    group.Where(engineResult => engineResult.Count == maxCount)
                        .OrderBy(engineResult => engineResult.Text)
                        .Select(engineResult => engineResult.Text)
                );
                summaryResult.AppendLine($"{group.First().EngineName} winner(s): {resultLine} ");
            }
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

            _logger.LogDebug(results.Count.ToString());

            return results.OrderBy(r => r.Count).ToList();
        }

        private string FormatResults(List<EngineResult> results)
        {
            return string
                .Join(" ", results.OrderBy(engineResult => engineResult.EngineName)
                    .Select(engineResult => $"{engineResult.EngineName}: {engineResult.Count} "));
        }
    }
}
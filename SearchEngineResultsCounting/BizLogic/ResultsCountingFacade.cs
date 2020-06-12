using System;
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
            var summaryResult = new StringBuilder();
            var textResults = AppendResults(texts, summaryResult);

            AppendEnginesWinner(textResults, summaryResult);

            AppendTotalWinner(textResults, summaryResult);

            return summaryResult.ToString();
        }

        private List<EngineResult> AppendResults(string[] texts, StringBuilder summaryResult)
        {
            var textResults = new List<EngineResult>(texts.Length);
            object sync = new Object();
            Parallel.ForEach(texts, text =>
            {
                var currentResults = GetResults(text);
                lock(sync) 
                {
                    textResults.AddRange(currentResults);
                    summaryResult.AppendLine($"{text}: {FormatResults(currentResults)}");
                }
            });
            return textResults;
        }

        private void AppendTotalWinner(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            var groupResults = textResults.GroupBy(engineResult => engineResult.Text)
                .Select(g => new { 
                    Text = g.First().Text,
                    Sum = g.Sum(er => er.Count)
                });
            int maxSum = groupResults.Max(gr => gr.Sum);
            var winners = string.Join(", ", groupResults.Where(gr => gr.Sum == maxSum)
                .Select(gr => gr.Text));
            summaryResult.AppendLine($"Total winner(s): {winners}");
        }

        private void AppendEnginesWinner(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            foreach (var group in textResults.GroupBy(engineResult => engineResult.EngineName))
            {
                if(group == null) continue;
                var maxCount = group.Max(er => er.Count);
                var resultLine = string.Join(", ",
                    group.Where(engineResult => engineResult.Count == maxCount)
                        .Select(engineResult => engineResult.Text)
                );
                summaryResult.AppendLine($"{group.First().EngineName} winner(s): {resultLine} ");
            }
        }

        private List<EngineResult> GetResults(string text)
        {
            var results = new List<EngineResult>();

            Parallel.ForEach(_engines, engine =>
            {
                var engineResult = new EngineResult() {
                    EngineName = engine.Name,
                    Text = text,
                    Count = engine.GetResultsCount(text)
                };
                results.Add(engineResult);
            });

            return results;
        }

        private string FormatResults(List<EngineResult> results)
        {   
            return string
                .Join(" ", results.OrderBy(engineResult => engineResult.EngineName)
                    .Select(engineResult => $"{engineResult.EngineName}: {engineResult.Count} "));
        }
    }
}
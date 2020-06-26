using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.BizLogic.Contract;

namespace SearchEngineResultsCounting.BizLogic.Aggregators
{
    public class ResultListAggregator : BaseAggregator, IAggregator
    {
        private readonly ILogger<ResultListAggregator> _logger;

        public ResultListAggregator(ILogger<ResultListAggregator> logger)
        {
            _logger = logger;
        }

        public override void Append(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (!Validate(textResults, summaryResult)) return;

            foreach (var textResult in textResults.GroupBy(tr => tr.Text))
            {
                var groupItems = textResult.ToList();
                summaryResult.AppendLine($"{textResult.Key}: {FormatResults(groupItems)}");
            }
        }

        private string FormatResults(List<EngineResult> results)
        {
            return string
                .Join(" ", results.OrderBy(engineResult => engineResult.EngineName)
                    .Select(engineResult => $"{engineResult.EngineName}: {engineResult.Count} "));
        }

        protected override bool Validate(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (textResults is null)
            {
                throw new ArgumentNullException("textResults");
            }

            if (summaryResult is null)
            {
                throw new ArgumentNullException("summaryResult");
            }

            if (textResults.Count == 0)
            {
                _logger.LogInformation("No text results to append result list.");
                return false;
            }

            return true;
        }
    }
}
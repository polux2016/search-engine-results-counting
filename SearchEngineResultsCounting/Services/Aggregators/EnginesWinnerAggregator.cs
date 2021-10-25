using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Services.Contract;

namespace SearchEngineResultsCounting.Services.Aggregators
{
    public class EnginesWinnerAggregator : BaseAggregator
    {
        private readonly ILogger<EnginesWinnerAggregator> _logger;

        public EnginesWinnerAggregator(ILogger<EnginesWinnerAggregator> logger)
        {
            _logger = logger;
        }

        public override void Append(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (!Validate(textResults, summaryResult)) return;

            foreach (var group in textResults.GroupBy(engineResult => engineResult.EngineName))
            {
                if (!group.Any())
                {
                    continue;
                }

                var maxCount = group.Max(er => er.Count);
                var resultLine = string.Join(", ",
                    group.Where(engineResult => engineResult.Count == maxCount)
                        .OrderBy(engineResult => engineResult.Text)
                        .Select(engineResult => engineResult.Text)
                );
                summaryResult.AppendLine($"{group.First().EngineName} winner(s): {resultLine} ");
            }
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
                _logger.LogInformation("No text results to append engines winner.");
                return false;
            }

            return true;
        }
    }
}
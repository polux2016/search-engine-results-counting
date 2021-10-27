using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Services.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchEngineResultsCounting.Services.Aggregators
{
    public class TotalWinnerAggregator : BaseAggregator
    {
        private readonly ILogger<TotalWinnerAggregator> _logger;

        public TotalWinnerAggregator(ILogger<TotalWinnerAggregator> logger)
        {
            _logger = logger;
        }

        public override void Append(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (!Validate(textResults, summaryResult)) return;

            var groupResults = textResults.GroupBy(engineResult => engineResult.Text)
                .Select(g => new
                {
                    g.First().Text,
                    Sum = g.Sum(er => er.Count)
                });
            var groupResultsList = groupResults.ToList();
            var maxSum = groupResultsList.Max(gr => gr.Sum);
            var winners = string.Join(", ", groupResultsList.OrderBy(gr => gr.Text)
                .Where(gr => gr.Sum == maxSum)
                .Select(gr => gr.Text));
            summaryResult.AppendLine($"Total winner(s): {winners}");
        }

        protected override bool Validate(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (!base.Validate(textResults, summaryResult))
            {
                return false;
            }

            if (textResults.Count == 0)
            {
                _logger.LogInformation("No text results to append total winner.");
                return false;
            }

            return true;
        }
    }
}
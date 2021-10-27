using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SearchEngineResultsCounting.Services.Aggregators;
using SearchEngineResultsCounting.Services.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Services.Aggregators
{
    public class TotalWinnerAggregatorTest
    {
        private readonly string _nl = Environment.NewLine;

        private TotalWinnerAggregator _aggregator;

        public TotalWinnerAggregatorTest()
        {
            var logger = Substitute.For<ILogger<TotalWinnerAggregator>>();

            _aggregator = new TotalWinnerAggregator(logger);
        }

        [Fact]
        public void BaseTest()
        {
            var textResult = new List<EngineResult>()
            {
                new EngineResult() {
                    EngineName = "Test 1",
                    Count = 1,
                    Text = "Text 1"
                }
            };
            var sb = new StringBuilder();

            _aggregator.Append(textResult, sb);

            Assert.Equal("Total winner(s): Text 1" + _nl, sb.ToString());
        }
    }
}
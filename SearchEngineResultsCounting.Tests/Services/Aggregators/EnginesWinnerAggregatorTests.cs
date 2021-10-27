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
    public class EnginesWinnerAggregatorTests
    {
        private readonly string _nl = Environment.NewLine;

        private readonly EnginesWinnerAggregator _aggregator;

        public EnginesWinnerAggregatorTests()
        {
            var logger = Substitute.For<ILogger<EnginesWinnerAggregator>>();

            _aggregator = new EnginesWinnerAggregator(logger);
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

            Assert.Equal("Test 1 winner(s): Text 1 " + _nl, sb.ToString());
        }
    }
}
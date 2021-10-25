using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using SearchEngineResultsCounting.Services.Aggregators;
using SearchEngineResultsCounting.Services.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Services.Aggregators
{
    public class TotalWinnerAggregatorTest
    {
        private readonly string nl = Environment.NewLine;

        private Mock<ILogger<TotalWinnerAggregator>> _loggerMock;

        private TotalWinnerAggregator _aggregator;

        public TotalWinnerAggregatorTest()
        {
            _loggerMock = new Mock<ILogger<TotalWinnerAggregator>>();

            _aggregator = new TotalWinnerAggregator(_loggerMock.Object);
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

            Assert.Equal("Total winner(s): Text 1" + nl, sb.ToString());
        }
    }
}
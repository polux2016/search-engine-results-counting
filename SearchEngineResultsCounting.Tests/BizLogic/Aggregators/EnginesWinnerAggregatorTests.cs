using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using SearchEngineResultsCounting.BizLogic.Aggregators;
using SearchEngineResultsCounting.BizLogic.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.BizLogic.Aggregators
{
    public class EnginesWinnerAggregatorTests
    {
        private readonly string nl = Environment.NewLine;

        private Mock<ILogger<EnginesWinnerAggregator>> _loggerMock;

        private EnginesWinnerAggregator _aggregator;

        public EnginesWinnerAggregatorTests()
        {
            _loggerMock = new Mock<ILogger<EnginesWinnerAggregator>>();

            _aggregator = new EnginesWinnerAggregator(_loggerMock.Object);
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

            Assert.Equal("Test 1 winner(s): Text 1 " + nl, sb.ToString());
        }
    }
}
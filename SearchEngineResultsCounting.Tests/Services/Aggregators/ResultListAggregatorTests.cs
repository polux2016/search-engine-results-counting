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
    public class ResultListAggregatorTests
    {
        private readonly string _nl = Environment.NewLine;

        private ResultListAggregator _aggregator;

        public ResultListAggregatorTests()
        {
            var loggerMock = new Mock<ILogger<ResultListAggregator>>();

            _aggregator = new ResultListAggregator(loggerMock.Object);
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

            Assert.Equal("Text 1: Test 1: 1 " + _nl, sb.ToString());
        }
    }
}
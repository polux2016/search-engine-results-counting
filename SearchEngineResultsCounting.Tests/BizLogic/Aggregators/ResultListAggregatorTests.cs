using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using SearchEngineResultsCounting.BizLogic.Aggregators;
using SearchEngineResultsCounting.BizLogic.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.BizLogic.Aggregators
{
    public class ResultListAggregatorTests
    {
        private Mock<ILogger<ResultListAggregator>> _loggerMock;
        private ResultListAggregator _aggregator;

        public ResultListAggregatorTests()
        {
            _loggerMock = new Mock<ILogger<ResultListAggregator>>();

            _aggregator = new ResultListAggregator(_loggerMock.Object);
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

            Assert.Equal("Text 1: Test 1: 1 \n", sb.ToString());
        }
    }
}
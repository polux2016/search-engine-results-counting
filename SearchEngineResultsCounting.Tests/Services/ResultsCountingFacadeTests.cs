using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Engines;
using SearchEngineResultsCounting.Services;
using SearchEngineResultsCounting.Services.Aggregators;
using SearchEngineResultsCounting.Services.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Services
{
    public class ResultsCountingFacadeTests
    {
        private readonly Mock<ILogger<ResultsCountingFacade>> _loggerMock;
        private readonly Mock<ILogger<ResultListAggregator>> _loggerResultListAggregatorMock;
        private readonly Mock<ILogger<EnginesWinnerAggregator>> _loggerEnginesWinnerAggregatorMock;
        private readonly Mock<ILogger<TotalWinnerAggregator>> _loggerTotalWinnerAggregatorMock;
        private readonly Mock<ISearchEngine> _searchEngineFirst;
        private readonly Mock<ISearchEngine> _searchEngineSecond;
        private readonly Mock<ISearchEngine> _searchEngineThird;

        public ResultsCountingFacadeTests()
        {
            _loggerMock = new Mock<ILogger<ResultsCountingFacade>>();
            _loggerResultListAggregatorMock = new Mock<ILogger<ResultListAggregator>>();
            _loggerEnginesWinnerAggregatorMock = new Mock<ILogger<EnginesWinnerAggregator>>();
            _loggerTotalWinnerAggregatorMock = new Mock<ILogger<TotalWinnerAggregator>>();

            _searchEngineFirst = new Mock<ISearchEngine>().As<ISearchEngine>();
            _searchEngineSecond = new Mock<ISearchEngine>().As<ISearchEngine>();
            _searchEngineThird = new Mock<ISearchEngine>().As<ISearchEngine>();
        }

        [Theory]
        [InlineData("phrase one", 11, 22, 33, 
            "phrase one: : 11  : 22  : 33 \n winner(s): phrase one \nTotal winner(s): phrase one\n")]
        [InlineData("phrase one", 11, 22, null, 
            "phrase one: : 11  : 22 \n winner(s): phrase one \nTotal winner(s): phrase one\n")]
        [InlineData("phrase one", 11, null, null, 
            "phrase one: : 11 \n winner(s): phrase one \nTotal winner(s): phrase one\n")]
        [InlineData("phrase one", null, null, null, "")]
        public void BaseFindAndCompareResultsTests(string text = null, int? count1 = null, int? count2 = null, int? count3 = null, 
            string expectedResult= null)
        {
            var resultsCountingFacade = GetResultsCountingFacade(count1, count2, count3);

            var actualResult = resultsCountingFacade.FindAndCompareResults(new string[] {text});

            Assert.Equal(expectedResult.Replace("\n", Environment.NewLine), actualResult);
        }

        private ResultsCountingFacade GetResultsCountingFacade(long? count1 = null, long? count2 = null, long? count3 = null)
        {
            var engines = new List<ISearchEngine>();
            
            AddEngineIfNeeded(count1, _searchEngineFirst, engines);
            
            AddEngineIfNeeded(count2, _searchEngineSecond, engines);

            AddEngineIfNeeded(count3, _searchEngineThird, engines);

            var aggregators = new List<IAggregator>() {
                new ResultListAggregator(_loggerResultListAggregatorMock.Object),
                new EnginesWinnerAggregator(_loggerEnginesWinnerAggregatorMock.Object),
                new TotalWinnerAggregator(_loggerTotalWinnerAggregatorMock.Object)
            };

            return new ResultsCountingFacade(engines, _loggerMock.Object, aggregators);
        }

        private void AddEngineIfNeeded(long? count, Mock<ISearchEngine> engine, List<ISearchEngine> engines)
        {
            if (count.HasValue)
            {
                engine.Setup(se => se.GetResultsCount(It.IsAny<string>()))
                    .ReturnsAsync(count.Value);
                engines.Add(engine.Object);
            }
        }

        private void SetTheResponse(long totalResultsCount, Mock<GoogleEngine> googleEngineMock)
        {
            var responceStr = "{'searchInformation': {'totalResultsCount': '{" + totalResultsCount.ToString() + "}'}}";

            googleEngineMock.Reset();
            googleEngineMock.CallBase = true;

            googleEngineMock.Protected()
                .Setup<string>("GetString", ItExpr.IsAny<string>())
                .Returns(responceStr);
        }
    }
}
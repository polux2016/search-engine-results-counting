using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections;
using Moq;
using Xunit;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.BizLogic;
using SearchEngineResultsCounting.Engines;
using System.Net.Http;
using Moq.Protected;
using System.Text.Json;

namespace SearchEngineResultsCounting.Tests.BizLogic
{
    public class ResultsCountingFacadeTests
    {
        private readonly Mock<ILogger<ResultsCountingFacade>> _loggerMock;
        private readonly Mock<ILogger<GoogleEngine>> _loggerMockGoogleEngine;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<ISearchEngine> _searchEngineFirst;
        private readonly Mock<ISearchEngine> _searchEngineSecond;
        private readonly Mock<ISearchEngine> _searchEngineThird;

        public ResultsCountingFacadeTests()
        {
            _loggerMock = new Mock<ILogger<ResultsCountingFacade>>();

            _searchEngineFirst = new Mock<ISearchEngine>().As<ISearchEngine>();
            _searchEngineSecond = new Mock<ISearchEngine>().As<ISearchEngine>();
            _searchEngineThird = new Mock<ISearchEngine>().As<ISearchEngine>();
        }

        [Theory]
        [InlineData("phrase one", 11, 22, 33, 
            "phrase one: : 11  : 22  : 33  winner(s): phrase one \nTotal winner(s): phrase one\n")]
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

            Assert.Equal(expectedResult, actualResult);
        }

        private ResultsCountingFacade GetResultsCountingFacade(long? count1 = null, long? count2 = null, long? count3 = null)
        {
            var engines = new List<ISearchEngine>();
            
            AddEngineIfNeeded(count1, _searchEngineFirst, engines);
            
            AddEngineIfNeeded(count2, _searchEngineSecond, engines);

            AddEngineIfNeeded(count3, _searchEngineThird, engines);

            return new ResultsCountingFacade(engines, _loggerMock.Object);
        }

        private void AddEngineIfNeeded(long? count, Mock<ISearchEngine> engine, List<ISearchEngine> engines)
        {
            if (count.HasValue)
            {
                engine.Setup(se => se.GetResultsCount(It.IsAny<string>()))
                    .Returns(count.Value);
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
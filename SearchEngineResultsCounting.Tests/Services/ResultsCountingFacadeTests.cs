using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Services;
using SearchEngineResultsCounting.Services.Aggregators;
using SearchEngineResultsCounting.Services.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Services
{
    public class ResultsCountingFacadeTests
    {
        private readonly ILogger<ResultsCountingFacade> _logger;
        private readonly ILogger<ResultListAggregator> _loggerResultListAggregator;
        private readonly ILogger<EnginesWinnerAggregator> _loggerEnginesWinnerAggregator;
        private readonly ILogger<TotalWinnerAggregator> _loggerTotalWinnerAggregator;
        private readonly ISearchEngine _searchEngineFirst;
        private readonly ISearchEngine _searchEngineSecond;
        private readonly ISearchEngine _searchEngineThird;

        public ResultsCountingFacadeTests()
        {
            _logger = Substitute.For<ILogger<ResultsCountingFacade>>();
            _loggerResultListAggregator = Substitute.For<ILogger<ResultListAggregator>>();
            _loggerEnginesWinnerAggregator = Substitute.For<ILogger<EnginesWinnerAggregator>>();
            _loggerTotalWinnerAggregator = Substitute.For<ILogger<TotalWinnerAggregator>>();

            _searchEngineFirst = Substitute.For<ISearchEngine>();
            _searchEngineSecond = Substitute.For<ISearchEngine>();
            _searchEngineThird = Substitute.For<ISearchEngine>();
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

            var actualResult = resultsCountingFacade.FindAndCompareResults(new[] {text});

            Assert.NotNull(expectedResult);
            Assert.Equal(expectedResult.Replace("\n", Environment.NewLine), actualResult);
        }

        private ResultsCountingFacade GetResultsCountingFacade(long? count1 = null, long? count2 = null, long? count3 = null)
        {
            var engines = new List<ISearchEngine>();
            
            AddEngineIfNeeded(count1, _searchEngineFirst, engines);
            
            AddEngineIfNeeded(count2, _searchEngineSecond, engines);

            AddEngineIfNeeded(count3, _searchEngineThird, engines);

            var aggregators = new List<IAggregator>() {
                new ResultListAggregator(_loggerResultListAggregator),
                new EnginesWinnerAggregator(_loggerEnginesWinnerAggregator),
                new TotalWinnerAggregator(_loggerTotalWinnerAggregator)
            };

            return new ResultsCountingFacade(engines, _logger, aggregators);
        }

        private void AddEngineIfNeeded(long? count, ISearchEngine engine, List<ISearchEngine> engines)
        {
            if (count.HasValue)
            {
                engine.GetResultsCount(Arg.Any<string>())
                    .Returns(count.Value);
                engines.Add(engine);
            }
        }
    }
}
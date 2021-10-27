using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using SearchEngineResultsCounting.Contracts.Configurations;
using SearchEngineResultsCounting.Engines;
using SearchEngineResultsCounting.Services.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Engines
{
    public class GoogleEngineTests
    {
        private readonly GoogleEngine _googleEngine;
        private readonly GoogleEngineConfiguration _googleEngineConfiguration;
        private readonly IHttpClientWrapper _httpClientWrapper;

        public GoogleEngineTests()
        {
            var logger = Substitute.For<ILogger<GoogleEngine>>();

            _httpClientWrapper = Substitute.For<IHttpClientWrapper>();
            _httpClientWrapper.GetJsonAsync<GoogleEngineResponse>(Arg.Any<string>())
                .Returns(new GoogleEngineResponse());

            var options = Substitute.For<IOptions<GoogleEngineConfiguration>>();
            _googleEngineConfiguration = new GoogleEngineConfiguration();
            options.Value.Returns(_googleEngineConfiguration);

            _googleEngine = new GoogleEngine(logger, _httpClientWrapper, options);
        }

        [Fact]
        public async void GetResultsCount_EngineDisabled_NoHttpCalls()
        {
            //arrange
            _googleEngineConfiguration.Enabeld = false;

            //act
            await _googleEngine.GetResultsCount(string.Empty);

            //assert
            await _httpClientWrapper.DidNotReceive().GetJsonAsync<GoogleEngineResponse>(Arg.Any<string>());
        }


        [Fact]
        public async void GetResultsCount_EngineEnabled_CallHttpWrapper()
        {
            //arrange
            _googleEngineConfiguration.Enabeld = true;

            //act
            await _googleEngine.GetResultsCount(string.Empty);

            //assert
            await _httpClientWrapper.Received(1).GetJsonAsync<GoogleEngineResponse>(Arg.Any<string>());
        }
    }
}
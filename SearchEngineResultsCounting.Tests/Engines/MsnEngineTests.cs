using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using SearchEngineResultsCounting.Contracts.Configurations;
using SearchEngineResultsCounting.Engines;
using SearchEngineResultsCounting.Services.Contract;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Engines
{
    public class MsnEngineTests
    {
        private readonly MsnEngine _msnEngine;
        private readonly MsnEngineConfiguration _msnEngineConfiguration;
        private readonly IHttpClientWrapper _httpClientWrapper;

        public MsnEngineTests()
        {
            var logger = Substitute.For<ILogger<MsnEngine>>();

            _httpClientWrapper = Substitute.For<IHttpClientWrapper>();
            _httpClientWrapper.GetJsonAsync<MsnEngineResponse>(Arg.Any<string>())
                .Returns(new MsnEngineResponse());

            var options = Substitute.For<IOptions<MsnEngineConfiguration>>();
            _msnEngineConfiguration = new MsnEngineConfiguration();
            options.Value.Returns(_msnEngineConfiguration);

            _msnEngine = new MsnEngine(logger, _httpClientWrapper, options);
        }

        [Fact]
        public async void GetResultsCount_EngineDisabled_NoHttpCalls()
        {
            //arrange
            _msnEngineConfiguration.Enabeld = false;

            //act
            await _msnEngine.GetResultsCount(string.Empty);

            //assert
            await _httpClientWrapper.DidNotReceive().GetJsonAsync<MsnEngineResponse>(Arg.Any<string>());
        }


        [Fact]
        public async void GetResultsCount_EngineEnabled_CallHttpWrapper()
        {
            //arrange
            _msnEngineConfiguration.Enabeld = true;

            //act
            await _msnEngine.GetResultsCount(string.Empty);

            //assert
            await _httpClientWrapper.Received(1).GetJsonAsync<MsnEngineResponse>(Arg.Any<string>());
        }
    }
}
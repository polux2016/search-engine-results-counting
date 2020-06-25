using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using SearchEngineResultsCounting.Engines;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Engines
{
    public class GoogleEngineTests
    {
        private readonly Mock<GoogleEngine> _googleEngineMock;
        private readonly Mock<ILogger<GoogleEngine>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configMock;

        public GoogleEngineTests()
        {
            _loggerMock = new Mock<ILogger<GoogleEngine>>();

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            _configMock = new Mock<IConfiguration>();
            _configMock.SetupGet(x => x[It.IsAny<string>()]).Returns(string.Empty);

            _googleEngineMock = new Mock<GoogleEngine>(_loggerMock.Object, 
                _httpClientFactoryMock.Object,
                _configMock.Object);
        }

        private void SetTheResponse(long totalResultsCount)
        {
            var responceStr = "{\"searchInformation\": {\"totalResults\": \"" + totalResultsCount.ToString() + "\"}}";

            _googleEngineMock.Reset();
            _googleEngineMock.CallBase = true;

            _googleEngineMock.Protected()
                .Setup<Task<string>>("GetString", ItExpr.IsAny<string>())
                .Returns(Task.FromResult(responceStr));
        }

        [Theory]
        [InlineData("", 1000)]
        [InlineData("test", 1000)]
        public async void BaseGetCountTest(string text, long count)
        {
            SetTheResponse(count);

            var result = await _googleEngineMock.Object.GetResultsCount(text);

            Assert.Equal(count, result);
        }
    }
}
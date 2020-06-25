using System.Net.Http;
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

        public GoogleEngineTests()
        {
            _loggerMock = new Mock<ILogger<GoogleEngine>>();

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            _googleEngineMock = new Mock<GoogleEngine>(_loggerMock.Object, _httpClientFactoryMock.Object);
        }

        private void SetTheResponse(long totalResultsCount)
        {
            var responceStr = "{\"searchInformation\": {\"totalResults\": \"" + totalResultsCount.ToString() + "\"}}";

            _googleEngineMock.Reset();
            _googleEngineMock.CallBase = true;

            _googleEngineMock.Protected()
                .Setup<string>("GetString", ItExpr.IsAny<string>())
                .Returns(responceStr);
        }

        [Theory]
        [InlineData("", 1000)]
        [InlineData("test", 1000)]
        public void BaseGetCountTest(string text, long count)
        {
            SetTheResponse(count);

            var result = _googleEngineMock.Object.GetResultsCount(text);

            Assert.Equal(count, result);
        }
    }
}
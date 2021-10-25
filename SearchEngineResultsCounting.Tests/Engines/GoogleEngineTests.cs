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

        public GoogleEngineTests()
        {
            var loggerMock = new Mock<ILogger<GoogleEngine>>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x[It.IsAny<string>()]).Returns(string.Empty);

            _googleEngineMock = new Mock<GoogleEngine>(loggerMock.Object, 
                httpClientFactoryMock.Object,
                configMock.Object);
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
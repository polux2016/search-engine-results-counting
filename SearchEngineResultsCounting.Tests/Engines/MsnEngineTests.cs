using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using SearchEngineResultsCounting.Engines;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Engines
{
    public class MsnEngineTests
    {
        private readonly Mock<MsnEngine> _MsnEngineMock;
        private readonly Mock<ILogger<MsnEngine>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;

        public MsnEngineTests()
        {
            _loggerMock = new Mock<ILogger<MsnEngine>>();

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            _MsnEngineMock = new Mock<MsnEngine>(_loggerMock.Object, _httpClientFactoryMock.Object);
        }

        private void SetTheResponse(long totalResultsCount)
        {
            var responceStr = "{\"totalEstimatedMatches\": " + totalResultsCount.ToString() + "}";

            _MsnEngineMock.Reset();
            _MsnEngineMock.CallBase = true;

            _MsnEngineMock.Protected()
                .Setup<Task<string>>("GetString", ItExpr.IsAny<string>())
                .Returns(Task.FromResult(responceStr));
        }

        [Theory]
        [InlineData("", 1000)]
        [InlineData("test", 1000)]
        public async void BaseGetCountTest(string text, long count)
        {
            SetTheResponse(count);

            var result = await _MsnEngineMock.Object.GetResultsCount(text);

            Assert.Equal(count, result);
        }
    }
}
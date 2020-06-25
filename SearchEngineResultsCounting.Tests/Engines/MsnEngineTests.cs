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
    public class MsnEngineTests
    {
        private readonly Mock<MsnEngine> _msnEngineMock;
        private readonly Mock<ILogger<MsnEngine>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configMock;

        public MsnEngineTests()
        {
            _loggerMock = new Mock<ILogger<MsnEngine>>();

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            _configMock = new Mock<IConfiguration>();
            _configMock.SetupGet(x => x[It.IsAny<string>()]).Returns(string.Empty);

            _msnEngineMock = new Mock<MsnEngine>(_loggerMock.Object, 
                _httpClientFactoryMock.Object,
                _configMock.Object
            );
        }

        private void SetTheResponse(long totalResultsCount)
        {
            var responceStr = "{\"totalEstimatedMatches\": " + totalResultsCount.ToString() + "}";

            _msnEngineMock.Reset();
            _msnEngineMock.CallBase = true;

            _msnEngineMock.Protected()
                .Setup<Task<string>>("GetString", ItExpr.IsAny<string>())
                .Returns(Task.FromResult(responceStr));
        }

        [Theory]
        [InlineData("", 1000)]
        [InlineData("test", 1000)]
        public async void BaseGetCountTest(string text, long count)
        {
            SetTheResponse(count);

            var result = await _msnEngineMock.Object.GetResultsCount(text);

            Assert.Equal(count, result);
        }
    }
}
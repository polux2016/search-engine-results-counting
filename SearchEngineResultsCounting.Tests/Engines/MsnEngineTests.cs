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

        public MsnEngineTests()
        {
            var loggerMock = new Mock<ILogger<MsnEngine>>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x[It.IsAny<string>()]).Returns(string.Empty);

            _msnEngineMock = new Mock<MsnEngine>(loggerMock.Object, 
                httpClientFactoryMock.Object,
                configMock.Object
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
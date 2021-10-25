using Microsoft.Extensions.Logging;
using Moq;
using SearchEngineResultsCounting.Services;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Services
{
    public class ArgumentsValidatorTests
    {
        private ArgumentsValidator _argumentsValidator;

        public ArgumentsValidatorTests()
        {
            var loggerMock = new Mock<ILogger<ArgumentsValidator>>();
            _argumentsValidator = new ArgumentsValidator(loggerMock.Object);
        }

        [Theory]
        [InlineData(new[] {"first", "second"}, new[] {"first", "second"})]
        [InlineData(new[] {"first"}, new[] {"first"})]
        public void ArgumentsValidation(string[] args, string[] texts)
        {
            _argumentsValidator.Validate(args);

            Assert.Equal(texts, _argumentsValidator.Texts);
        }

        [Fact]
        public void NoExceptionIfNoArgs()
        {
            var exception = Record.Exception(() => _argumentsValidator.Validate(new string[0]));
            Assert.Null(exception);
        }
    }
}
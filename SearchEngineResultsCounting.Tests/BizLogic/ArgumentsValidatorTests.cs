using Xunit;
using Moq;
using SearchEngineResultsCounting.BizLogic;
using Microsoft.Extensions.Logging;

namespace SearchEngineResultsCounting.Tests.BizLogic
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
        [InlineData(new string[] {"first", "second"}, new string[] {"first", "second"})]
        [InlineData(new string[] {"first"}, new string[] {"first"})]
        public void ArgumentsValidation(string[] args, string[] texts)
        {
            _argumentsValidator.Validate(args);

            Assert.Equal(_argumentsValidator.Texts, texts);
        }

        [Fact]
        public void NoExceptionIfNoArgs()
        {
            var exception = Record.Exception(() => _argumentsValidator.Validate(new string[0]));
            Assert.Null(exception);
        }
    }
}
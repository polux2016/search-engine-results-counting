using Microsoft.Extensions.Logging;
using NSubstitute;
using SearchEngineResultsCounting.Services;
using Xunit;

namespace SearchEngineResultsCounting.Tests.Services
{
    public class ArgumentsValidatorTests
    {
        private readonly ArgumentsValidator _argumentsValidator;

        public ArgumentsValidatorTests()
        {
            var logger = Substitute.For<ILogger<ArgumentsValidator>>();
            _argumentsValidator = new ArgumentsValidator(logger);
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
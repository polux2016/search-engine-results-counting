using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.BizLogic
{
    public class ArgumentsValidator : IArgumentsValidator
    {
        private readonly ILogger<ArgumentsValidator> _logger;

        private string text;

        string IArgumentsValidator.Text
        {
            get
            {
                return text;
            }
        }

        public ArgumentsValidator(ILogger<ArgumentsValidator> logger)
        {
            _logger = logger;
        }

        public bool Validate(string[] args)
        {
            if (args.Length < 1)
            {
                _logger.LogError($"There is not text to search. Arguments Count {args.Length}");
            }
            else
            {
                text = args[args.Length - 1];
                _logger.LogInformation($"Text to search will be '{text}'");
            }
            return true;
        }
    }
}
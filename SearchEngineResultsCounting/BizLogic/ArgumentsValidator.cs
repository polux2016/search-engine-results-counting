using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.BizLogic
{
    public class ArgumentsValidator : IArgumentsValidator
    {
        private readonly ILogger<ArgumentsValidator> _logger;

        private string[] texts;

        public string[] Texts
        {
            get
            {
                return texts;
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
                _logger.LogError($"There are no texts to search. Arguments Count {args.Length}");
                return false;
            }
            else
            {
                texts = args;
                _logger.LogInformation($"{args.Length} texts to search was found.");
            }
            return true;
        }
    }
}
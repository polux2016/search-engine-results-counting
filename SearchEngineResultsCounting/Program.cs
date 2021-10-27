using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using System;

namespace SearchEngineResultsCounting
{
    internal class Program
    {
        private static ILogger _logger;

        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            var startup = new Startup();
            _serviceProvider = startup.ConfigureServiceProvider();
            _logger = _serviceProvider.GetService<ILogger<Program>>();

            try
            {
                _logger.LogDebug("Starting application");

                RunApp(args);

                _logger.LogDebug("Application finished");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled error appear. Msg: '{ex.Message}'", ex);
            }
        }

        private static void RunApp(string[] args)
        {
            var argumentsValidator = _serviceProvider.GetService<IArgumentsValidator>();
            if (argumentsValidator.Validate(args))
            {
                var separator = new string('=', 50);
                var lineSeparator = $"{separator} Result report {separator}{Environment.NewLine}";
                var manager = _serviceProvider.GetService<IResultsCountingFacade>();
                var result = manager.FindAndCompareResults(argumentsValidator.Texts);
                Console.WriteLine($"{lineSeparator}{result}{lineSeparator}");
                Console.ReadKey();
            }
            else
            {
                _logger.LogError("Arguments not valid. Check it and try one more time.");
            }
        }
    }
}

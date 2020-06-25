using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.BizLogic;
using SearchEngineResultsCounting.Engines;

namespace SearchEngineResultsCounting
{
    class Program
    {
        static ILogger _logger;

        static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            ConfigureDI();
            ConfigureLogging();

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
                var manager = _serviceProvider.GetService<IResultsCountingFacade>();
                var result = manager.FindAndCompareResults(argumentsValidator.Texts);
                _logger.LogInformation(result);
            }
            else
            {
                _logger.LogError("Arguments not valid. Check it and try one more time.");
            }
        }

        private static void ConfigureDI()
        {
            _serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddLogging(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Debug))
                .AddTransient<ISearchEngine, GoogleEngine>()
                .AddTransient<ISearchEngine, MsnEngine>()
                .AddTransient<IResultsCountingFacade, ResultsCountingFacade>()
                .AddTransient<IArgumentsValidator, ArgumentsValidator>()
                .BuildServiceProvider();
        }

        private static void ConfigureLogging()
        {
            _logger = _serviceProvider.GetService<ILogger<Program>>();
        }
    }
}

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

            _logger.LogDebug("Starting application");

            RunApp(args);

            _logger.LogDebug("Application finished");

            Console.ReadLine();
        }

        private static void RunApp(string[] args)
        {
            var argumentsValidator = _serviceProvider.GetService<IArgumentsValidator>();
            if (argumentsValidator.Validate(args))
            {
                var manager = _serviceProvider.GetService<IResultsCountingFacade>();
                var result = manager.FindAndCompareResults(argumentsValidator.Text);
                _logger.LogInformation(result);
            } 
            else 
            {
                _logger.LogError("Arguments not valid. Check it and try one more.");
            }
        }

        private static void ConfigureDI()
        {
            _serviceProvider = new ServiceCollection()
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

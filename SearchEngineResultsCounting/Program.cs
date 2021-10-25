using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Engines;
using Microsoft.Extensions.Configuration;
using SearchEngineResultsCounting.Services;
using SearchEngineResultsCounting.Services.Aggregators;
using SearchEngineResultsCounting.Services.Contract;

namespace SearchEngineResultsCounting
{
    class Program
    {
        static ILogger _logger;

        static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            var configuration = ConfigureAppSettings();
            ConfigureDi(configuration);
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

        private static IConfiguration ConfigureAppSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            return config;
        }

        private static void RunApp(string[] args)
        {
            var argumentsValidator = _serviceProvider.GetService<IArgumentsValidator>();
            if (argumentsValidator.Validate(args))
            {
                var sepparator = new string('=', 50);
                var lineSepparator = $"{sepparator} Result report {sepparator}{Environment.NewLine}";
                var manager = _serviceProvider.GetService<IResultsCountingFacade>();
                var result = manager.FindAndCompareResults(argumentsValidator.Texts);
                Console.WriteLine($"{lineSepparator}{result}{lineSepparator}");
                Console.ReadKey();
            }
            else
            {
                _logger.LogError("Arguments not valid. Check it and try one more time.");
            }
        }

        private static void ConfigureDi(IConfiguration config)
        {
            _serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddLogging(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(config.GetSection("LogLevel").Get<LogLevel>()))
                .AddTransient<ISearchEngine, GoogleEngine>()
                .AddTransient<ISearchEngine, MsnEngine>()
                .AddTransient<IResultsCountingFacade, ResultsCountingFacade>()
                .AddTransient<IArgumentsValidator, ArgumentsValidator>()
                .AddTransient<IAggregator, ResultListAggregator>()
                .AddTransient<IAggregator, EnginesWinnerAggregator>()
                .AddTransient<IAggregator, TotalWinnerAggregator>()
                .AddTransient<IArgumentsValidator, ArgumentsValidator>()
                .AddSingleton(config)
                .BuildServiceProvider();
        }

        private static void ConfigureLogging()
        {
            _logger = _serviceProvider.GetService<ILogger<Program>>();
        }
    }
}

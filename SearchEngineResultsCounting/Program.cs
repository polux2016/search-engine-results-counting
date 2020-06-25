using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.BizLogic;
using SearchEngineResultsCounting.Engines;
using Microsoft.Extensions.Configuration;

namespace SearchEngineResultsCounting
{
    class Program
    {
        static ILogger _logger;

        static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            var configuration = ConfigureAppSettings();
            ConfigureDI(configuration);
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

        private static void ConfigureDI(IConfiguration config)
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
                .AddSingleton<IConfiguration>(config)
                .BuildServiceProvider();
        }

        private static void ConfigureLogging()
        {
            _logger = _serviceProvider.GetService<ILogger<Program>>();
        }
    }
}

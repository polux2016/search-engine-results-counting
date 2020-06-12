using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

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

            //Do something 

            _logger.LogDebug("Application finished");
        }

        private static void ConfigureDI()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.None)
                .BuildServiceProvider();
        }

        private static void ConfigureLogging()
        {
            _logger = _serviceProvider.GetService<ILogger<Program>>();
        }
    }
}

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SearchEngineResultsCounting.Contracts;
using SearchEngineResultsCounting.Contracts.Configurations;
using SearchEngineResultsCounting.Engines;
using SearchEngineResultsCounting.Services;
using SearchEngineResultsCounting.Services.Aggregators;
using SearchEngineResultsCounting.Services.Contract;

namespace SearchEngineResultsCounting
{
    public class Startup
    {
        private const string AppSettingsFileName = "appsettings.json";

        public IServiceProvider ConfigureServiceProvider()
        {
            var configuration = ConfigureAppSettings();

            return ConfigureDi(configuration);
        }

        private IConfiguration ConfigureAppSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(AppSettingsFileName, false, true)
                .Build();
            return config;
        }

        private ServiceProvider ConfigureDi(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection()
                .AddHttpClient()
                .AddLogging(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(configuration.GetSection("LogLevel").Get<LogLevel>()))
                .AddTransient<ISearchEngine, GoogleEngine>()
                .AddTransient<ISearchEngine, MsnEngine>()
                .AddTransient<IResultsCountingFacade, ResultsCountingFacade>()
                .AddTransient<IArgumentsValidator, ArgumentsValidator>()
                .AddTransient<IAggregator, ResultListAggregator>()
                .AddTransient<IAggregator, EnginesWinnerAggregator>()
                .AddTransient<IAggregator, TotalWinnerAggregator>()
                .AddTransient<IArgumentsValidator, ArgumentsValidator>()
                .AddTransient<IHttpClientWrapper, HttpClientWrapper>()
                .AddSingleton(configuration);

            serviceCollection.AddOptions<GoogleEngineConfiguration>(GoogleEngineConfiguration.SectionName);
            serviceCollection.AddOptions<MsnEngineConfiguration>(MsnEngineConfiguration.SectionName);

            return serviceCollection.BuildServiceProvider();
        }
    }
}

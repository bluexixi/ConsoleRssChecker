namespace ConsoleRssChecker
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using ConsoleRssChecker.Service;
    using ConsoleRssChecker.Settings;
    using ConsoleRssChecker.Repository;
    using ConsoleRssChecker.Utility;

    public class Program
    {
        public static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
              .AddConsole()
              .AddDebug());
            serviceCollection.AddLogging();

            // build configuration
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", false)
              .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            // add services
            serviceCollection.AddTransient<IRssChecker, RssChecker>();
            serviceCollection.AddTransient<IRssParser, RssParser>();
            serviceCollection.AddSingleton<ICompanyRssRepository, CsvCompanyRssRepository>();
            serviceCollection.AddSingleton<IInactiveCompanyRepository, TextFileInactiveCompanyRepository>();

            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}

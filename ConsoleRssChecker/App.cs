namespace ConsoleRssChecker
{
    using ConsoleRssChecker.Service;
    using ConsoleRssChecker.Settings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;

    public class App
    {
        private readonly IRssChecker _rssChecker;
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;

        public App(IRssChecker rssChecker, ILogger<App> logger, IOptions<AppSettings> config)
        {
            _rssChecker = rssChecker;
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            _logger.LogInformation($"Checking for inactive companies for {_config.InactiveDays} day(s).");
            _rssChecker.Run();
            _logger.LogInformation($"Finished. Check the Outputs folder for results.");
            Console.ReadKey();
        }
    }
}

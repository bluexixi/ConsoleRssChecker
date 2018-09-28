namespace ConsoleRssChecker.Service
{
    using ConsoleRssChecker.Repository;
    using ConsoleRssChecker.Settings;
    using ConsoleRssChecker.Utility;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RssChecker : IRssChecker
    {
        private readonly ILogger<RssChecker> _logger;
        private readonly AppSettings _config;
        private readonly IRssParser _rssParser;
        private readonly ICompanyRssRepository _companyRssRepository;

        public RssChecker(ILogger<RssChecker> logger, 
                          IOptions<AppSettings> config,
                          IRssParser rssParser,
                          ICompanyRssRepository companyRssRepository)
        {
            _logger = logger;
            _config = config.Value;
            _rssParser = rssParser;
            _companyRssRepository = companyRssRepository;
        }


        public IEnumerable<string> CreateInactiveCompanyList()
        {
            var companyRssTuples = _companyRssRepository.GetAllCompanyRss();
            // Thread-safe, only Add method is used.
            var inactiveCompanyList = new List<string>();

            Parallel.ForEach(companyRssTuples, companyRssTuple =>
            {
                var rssUri = companyRssTuple.Item2;
                var date = _rssParser.GetLastBuildDate(rssUri);
                if (date.Equals(DateTime.MinValue)) return;

                var days = (DateTime.Now - date).TotalDays;
                if (days >= _config.InactiveDays) inactiveCompanyList.Add(companyRssTuple.Item1);
            });

            return inactiveCompanyList;
        }

        public void Run()
        {
            var inactiveCompanyList = CreateInactiveCompanyList();

            if (inactiveCompanyList.Count() > 0)
            {
                Console.WriteLine($"The following companies have been inactive for {_config.InactiveDays} days:");
                foreach (var name in inactiveCompanyList)
                {
                    Console.WriteLine($"{name}");
                }
            }
            else
            {
                Console.WriteLine($"No company has been inactive for {_config.InactiveDays} days.");
            }
        }
    }
}

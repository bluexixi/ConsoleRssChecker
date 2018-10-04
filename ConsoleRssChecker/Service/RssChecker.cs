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
        private readonly IInactiveCompanyRepository _inactiveCompanyRepository;

        public RssChecker(ILogger<RssChecker> logger, 
                          IOptions<AppSettings> config,
                          IRssParser rssParser,
                          ICompanyRssRepository companyRssRepository,
                          IInactiveCompanyRepository inactiveCompanyRepository)
        {
            _logger = logger;
            _config = config.Value;
            _rssParser = rssParser;
            _companyRssRepository = companyRssRepository;
            _inactiveCompanyRepository = inactiveCompanyRepository;
        }

        public async Task<IEnumerable<string>> CreateInactiveCompanyList()
        {
            var companyRssTuples = await _companyRssRepository.GetAllCompanyRss();
            // Thread-safe, only Add method is used.
            var inactiveCompanyList = new List<string>();

            Parallel.ForEach(companyRssTuples, async companyRssTuple =>
            {
                var rssUri = companyRssTuple.Item2;
                var date = await _rssParser.GetLastBuildDate(rssUri);
                if (date.Equals(DateTime.MinValue)) return;

                var days = (DateTime.Now - date).TotalDays;
                if (days >= _config.InactiveDays) inactiveCompanyList.Add(companyRssTuple.Item1);
            });

            return inactiveCompanyList;
        }

        public void Run()
        {
            var inactiveCompanyList = CreateInactiveCompanyList().Result;

            _inactiveCompanyRepository.Save(inactiveCompanyList);
        }
    }
}

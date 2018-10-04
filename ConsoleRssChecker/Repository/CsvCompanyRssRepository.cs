namespace ConsoleRssChecker.Repository
{
    using ConsoleRssChecker.Settings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class CsvCompanyRssRepository : ICompanyRssRepository
    {
        private readonly ILogger<CsvCompanyRssRepository> _logger;
        private readonly AppSettings _config;
        private string _filepath;

        public CsvCompanyRssRepository(ILogger<CsvCompanyRssRepository> logger, IOptions<AppSettings> config)
        {
            _logger = logger;
            _config = config.Value;
            try
            {
                _filepath = Path.Combine(Directory.GetCurrentDirectory(), _config.CSVFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get the file path for {_config.CSVFileName}. The error message is {ex.Message}");
                _filepath = string.Empty;
            }
        }

        public async Task<IEnumerable<Tuple<string, string>>> GetAllCompanyRss()
        {
            return await Task.Run(() => {
                var companyRssTuples = new List<Tuple<string, string>>();

                if (!File.Exists(_filepath))
                {
                    _logger.LogError($"The file at location: {_filepath} does not exist.");
                    return companyRssTuples;
                }

                try
                {
                    using (var sr = new StreamReader(_filepath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            var elems = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            var tuple = new Tuple<string, string>(elems[0], elems[1]);
                            companyRssTuples.Add(tuple);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"The file at location: {_filepath} can not be read with an error message {ex.Message}.");
                }
                return companyRssTuples;
            });
        }
    }
}

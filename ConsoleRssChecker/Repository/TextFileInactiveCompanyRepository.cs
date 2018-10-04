namespace ConsoleRssChecker.Repository
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class TextFileInactiveCompanyRepository : IInactiveCompanyRepository
    {
        private readonly ILogger<TextFileInactiveCompanyRepository> _logger;

        public TextFileInactiveCompanyRepository(ILogger<TextFileInactiveCompanyRepository> logger)
        {
            _logger = logger;
        }
        public Task Save(IEnumerable<string> inactiveCompanyList)
        {
            return Task.Run(() => 
            {
                try
                {
                    if (!Directory.Exists("Outputs"))
                    {
                        Directory.CreateDirectory("Outputs");
                    }
                    var fileName = $"inactiveCompanyList_{DateTime.Now.ToString("yyyy_MM_dd")}";
                    File.WriteAllLines($@"Outputs\{fileName}.txt", inactiveCompanyList);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unable to create result file with an erro message {ex.Message}");
                }
            });
        }
    }
}

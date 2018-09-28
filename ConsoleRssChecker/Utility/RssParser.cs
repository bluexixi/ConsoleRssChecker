namespace ConsoleRssChecker.Utility
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class RssParser : IRssParser
    {
        private readonly ILogger<RssParser> _logger;

        public RssParser(ILogger<RssParser> logger)
        {
            _logger = logger;
        }

        public DateTime GetLastBuildDate(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                // XPath: rss/channel/lastBuildDate
                var element = doc.Root.Descendants().FirstOrDefault(i => i.Name.LocalName == "lastBuildDate");
                DateTime.TryParse(element?.Value, out DateTime lastBuildDate);
                return lastBuildDate;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Unable to get the last build date for {url}. The error message is: {ex.Message}");
                return DateTime.MinValue;
            }

        }
    }
}

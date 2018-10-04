namespace ConsoleRssChecker.Utility
{
    using System;
    using System.Threading.Tasks;

    public interface IRssParser
    {
        Task<DateTime> GetLastBuildDate(string url);
    }
}

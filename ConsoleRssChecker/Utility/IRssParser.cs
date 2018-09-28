namespace ConsoleRssChecker.Utility
{
    using System;

    public interface IRssParser
    {
        DateTime GetLastBuildDate(string url);
    }
}

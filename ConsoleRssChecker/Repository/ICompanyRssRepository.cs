namespace ConsoleRssChecker.Repository
{
    using System;
    using System.Collections.Generic;

    public interface ICompanyRssRepository
    {
        IEnumerable<Tuple<string, string>> GetAllCompanyRss();
    }
}

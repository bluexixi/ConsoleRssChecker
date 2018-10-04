﻿namespace ConsoleRssChecker.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICompanyRssRepository
    {
        Task<IEnumerable<Tuple<string, string>>> GetAllCompanyRss();
    }
}

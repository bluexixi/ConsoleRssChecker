namespace ConsoleRssChecker.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRssChecker
    {
        Task<IEnumerable<string>> CreateInactiveCompanyList();

        void Run();
    }
}

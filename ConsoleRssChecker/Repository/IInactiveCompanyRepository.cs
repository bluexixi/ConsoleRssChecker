namespace ConsoleRssChecker.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInactiveCompanyRepository
    {
        Task Save(IEnumerable<string> inactiveCompanyList);
    }
}

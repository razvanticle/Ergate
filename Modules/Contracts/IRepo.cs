namespace Contracts
{
    using System.Collections.Generic;

    public interface IRepo
    {
        IEnumerable<string> Get();

        IEnumerable<string> GetCompanies();
    }
}
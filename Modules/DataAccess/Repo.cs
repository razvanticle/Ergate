namespace DataAccess
{
    using System.Collections.Generic;

    using Contracts;

    using iQuarc.AppBoot;

    [Service(typeof(IRepo))]
    public class Repo : IRepo
    {
        public IEnumerable<string> Get()
        {
            return new[] { "Value 1", "Value 2" };
        }

        public IEnumerable<string> GetCompanies()
        {
            return new[] { "Company 1", "Company 2" };
        }
    }
}
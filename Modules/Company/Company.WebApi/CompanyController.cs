namespace Company.WebApi
{
    using System.Web.Http;

    using Contracts;

    using iQuarc.DataAccess;

    public class CompanyController : ApiController
    {
        private readonly IRepo repo;

        public CompanyController(IRepo repo)
        {
            this.repo = repo;
        }

        [Route("companies")]
        public IHttpActionResult Get()
        {
            return this.Ok(this.repo.GetCompanies());
        }
    }
}
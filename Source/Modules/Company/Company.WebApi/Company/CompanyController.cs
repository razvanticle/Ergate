namespace Company.WebApi.Company
{
    using System.Linq;
    using System.Web.Http;

    using DataAccess;

    using iQuarc.DataAccess;

    public class CompanyController : ApiController
    {
        private readonly IRepository repository;

        public CompanyController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("companies")]
        public IHttpActionResult Get()
        {
            var companies =
                this.repository.GetEntities<Company>()
                    .Select(c => new CompanyDto { Id = c.Id, Name = c.Name })
                    .ToList();

            return this.Ok(companies);
        }
    }
}
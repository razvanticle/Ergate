namespace Company.WebApi.Company
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using DataAccess;

    using iQuarc.AppBoot;
    using iQuarc.DataAccess;

    public class CompanyController : ApiController
    {
        private readonly IRepository repository;

        private readonly IMyInterface myInterface;

        public CompanyController(IRepository repository, IMyInterface myInterface)
        {
            this.repository = repository;
            this.myInterface = myInterface;
        }

        [Route("companies")]
        public IHttpActionResult Get()
        {
            this.myInterface.DoStuff();

            var companies =
                this.repository.GetEntities<Company>().Select(c => new CompanyDto { Id = c.Id, Name = c.Name }).ToList();

            return this.Ok(companies);
        }
    }

    [Service(typeof(IMyInterface))]
    public class MyService : IMyInterface
    {
        public void DoStuff()
        {
            throw new Exception("Some error!");
        }
    }

    public interface IMyInterface
    {
        void DoStuff();
    }
}
namespace Company.WebApi
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using DataModel;

    using Ergate.DataAccess;
    using Ergate.Infrastructure.AppBoot.Container;

    public class CompanyController : ApiController
    {
        private readonly IMyInterface myInterface;

        private readonly IRepository repository;

        public CompanyController(IRepository repository, IMyInterface myInterface)
        {
            this.repository = repository;
            this.myInterface = myInterface;
        }

        [Route("companies")]
        public IHttpActionResult Get()
        {
            //this.myInterface.DoStuff();

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
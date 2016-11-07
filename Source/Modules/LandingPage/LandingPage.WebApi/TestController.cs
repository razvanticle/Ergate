namespace LandingPage.WebApi
{
    using System.Linq;
    using System.Web.Http;

    using DataModel;

    using Ergate.DataAccess;
    
    public class TestController : ApiController
    {
        private readonly IRepository repository;

        public TestController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("users")]
        public IHttpActionResult Get()
        {
            var users =
                this.repository.GetEntities<User>()
                    .Select(u => new UserDto { Id = u.Id, Name = u.FirstName + " " + u.LastName })
                    .ToList();

            return this.Ok(users);
        }
    }
}
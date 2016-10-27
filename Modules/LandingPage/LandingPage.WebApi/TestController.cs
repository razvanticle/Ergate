namespace LandingPage.WebApi
{
    using System.Web.Http;

    using Contracts;

    public class TestController : ApiController
    {
        private readonly IRepo repo;

        public TestController(IRepo repo)
        {
            this.repo = repo;
        }

        [Route("test")]
        public IHttpActionResult Get()
        {
            var strings = this.repo.Get();
            return this.Ok(strings);
        }
    }
}
namespace Ergate.DataAccess.UnitTests
{
    using Ergate.DataAccess;
    using Ergate.DataAccess.UnitTests.TestDoubles;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public  class RepositoryTests : RepositoryBaseTests
    {
        protected override IRepository GetTarget(IDbContextFactory factory, IInterceptorsResolver resolver)
        {
            return new Repository(factory, resolver, new ContextUtilitiesDouble());
        }
    }
}
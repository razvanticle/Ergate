namespace Ergate.DataAccess.UnitTests
{
    using Ergate.DataAccess;
    using Ergate.DataAccess.ExceptionHandling;
    using Ergate.DataAccess.UnitTests.TestDoubles;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class UnitOfWorkAsRepositoryTests : RepositoryBaseTests
    {
        protected override IRepository GetTarget(IDbContextFactory factory, IInterceptorsResolver resolver)
        {
            IExceptionHandler handler = new Mock<IExceptionHandler>().Object;
            return new UnitOfWork(factory, resolver, new ContextUtilitiesDouble(), handler);
        }
    }
}
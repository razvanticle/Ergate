namespace Ergate.DataAccess.UnitTests
{
    using System.Data.Entity;

    using Ergate.DataAccess;
    using Ergate.DataAccess.UnitTests.TestDoubles;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class UnitOfWorkAddTests
    {
        [TestMethod]
        public void Add_ValidEntity_EntityAddedToDbContext()
        {
            FakeSet<User> setStub = new FakeSet<User>();
            Mock<DbContext> contextStub = new Mock<DbContext>();
            contextStub.Setup(c => c.Set<User>()).Returns(setStub);

            IInterceptorsResolver interceptorsResolver =  new Mock<IInterceptorsResolver>().Object;
            UnitOfWork uof = new UnitOfWork(contextStub.BuildFactoryStub(), interceptorsResolver);

            User u = new User();
            uof.Add(u);

            Assert.IsTrue(setStub.Values.Contains(u));
        }

        private class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
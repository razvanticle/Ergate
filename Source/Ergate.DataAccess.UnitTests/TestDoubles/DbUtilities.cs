namespace Ergate.DataAccess.UnitTests.TestDoubles
{
    using System.Collections.Generic;
    using System.Data.Entity;

    using Ergate.DataAccess;

    using Moq;

    static class DbUtilities
    {
        public static DbSet<T> MockDbSet<T>(this IEnumerable<T> values) where T : class
        {
            return new FakeSet<T>(values);
        }

        public static DbSet<T> MockDbSet<T>(this IEnumerable<T> values, DbContextFakeWrapper wrapper) where T : class
        {
            return new FakeSet<T>(values, wrapper);
        }

        //TODO: write an overload for DbContextFakeWrapper
        public static IDbContextFactory BuildFactoryStub(this Mock<DbContext> contextStub)
        {
            Mock<IDbContextFactory> contextFactoryStub = new Mock<IDbContextFactory>();
            contextFactoryStub.Setup(s => s.CreateContext())
                              .Returns(new DbContextFakeWrapper(contextStub));
            return contextFactoryStub.Object;
        }
    }
}
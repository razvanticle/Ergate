namespace Ergate.DataAccess.UnitTests.TestDoubles
{
    using System.Data.Entity;

    using Ergate.DataAccess;

    using Moq;

    sealed class DbContextFakeWrapper : IDbContextWrapper
    {
        private readonly Mock<DbContext> contextDouble;

        public DbContextFakeWrapper()
        {
            this.contextDouble = new Mock<DbContext>();
        }

        public DbContextFakeWrapper(Mock<DbContext> dbContext)
        {
            this.contextDouble = dbContext;
        }

        public Mock<DbContext> ContextDouble
        {
            get { return this.contextDouble; }
        }

        public DbContext Context
        {
            get { return this.contextDouble.Object; }
        }

        public event EntityLoadedEventHandler EntityLoaded;

        public void RaiseEntityLoaded(EntityLoadedEventHandlerArgs args)
        {
            if (this.EntityLoaded != null)
                this.EntityLoaded(this, args);
        }

        public void Dispose()
        {
            this.WasDisposed = true;
        }

        public bool WasDisposed { get; private set; }
    }
}
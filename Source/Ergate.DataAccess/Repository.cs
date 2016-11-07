namespace Ergate.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    ///     Implements a repository for reading data with Entity Framework
    ///     The entities retrieved through this repository are not meant to be modified and persisted back.
    ///     This implementation is optimized for read-only operations. For reading data for edit, or delete create and use an IUnitOfWork
    /// </summary>
    public class Repository : IRepository, IDisposable
    {
        private readonly IInterceptorsResolver interceptorsResolver;
        private readonly IDbContextFactory contextFactory;

        private readonly DbContextBuilder contextBuilder;

        public Repository(IDbContextFactory contextFactory, IInterceptorsResolver interceptorsResolver)
            : this(contextFactory, interceptorsResolver, new DbContextUtilities())
        {
        }

        internal Repository(IDbContextFactory contextFactory, IInterceptorsResolver interceptorsResolver, IDbContextUtilities contextUtilities)
        {
            this.interceptorsResolver = interceptorsResolver;
            this.contextFactory = contextFactory;

            this.contextBuilder = new DbContextBuilder(contextFactory, interceptorsResolver, this, contextUtilities);
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return this.Context.Set<T>().AsNoTracking();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(this.contextFactory, this.interceptorsResolver);
        }

        protected DbContext Context
        {
            get { return this.contextBuilder.Context; }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.contextBuilder != null)
                    this.contextBuilder.Dispose();
            }
        }
    }
}
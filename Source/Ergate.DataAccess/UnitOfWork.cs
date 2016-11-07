namespace Ergate.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Threading.Tasks;

    using Ergate.DataAccess.ExceptionHandling;

    /// <summary>
    ///     Implements an unit of work for modifying, deleting or adding new data with Entity Framework.
    ///     An instance of this class should have a well defined and short scope. It should be disposed once the changes were
    ///     saved into the database
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEnumerable<IEntityInterceptor> globalInterceptors;
        private readonly IInterceptorsResolver interceptorsResolver;
        private readonly IDbContextUtilities contextUtilities;

        private readonly DbContextBuilder contextBuilder;
        private DbContextTransaction transactionScope;

        private readonly IExceptionHandler exceptionHandler;

        internal UnitOfWork(IDbContextFactory contextFactory, IInterceptorsResolver interceptorsResolver)
            : this(contextFactory, interceptorsResolver, new DbContextUtilities(), new ExceptionHandler())
        {
        }

        internal UnitOfWork(IDbContextFactory contextFactory, IInterceptorsResolver interceptorsResolver, IDbContextUtilities contextUtilities, IExceptionHandler exceptionHandler)
        {
            this.interceptorsResolver = interceptorsResolver;
            this.contextUtilities = contextUtilities;
            this.globalInterceptors = interceptorsResolver.GetGlobalInterceptors();
            this.exceptionHandler = exceptionHandler;

            this.contextBuilder = new DbContextBuilder(contextFactory, interceptorsResolver, this, contextUtilities);
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return this.contextBuilder.Context.Set<T>();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return this;
        }

        public void SaveChanges()
        {
            try
            {
                this.InterceptSave(new List<object>());
                this.contextBuilder.Context.SaveChanges();
                if (this.transactionScope != null)
                    this.transactionScope.Commit();
            }
            catch (Exception e)
            {
                this.Handle(e);
            }
        }

        private void Handle(Exception exception)
        {
            this.exceptionHandler.Handle(exception);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                this.InterceptSave(new List<object>());

                await this.contextBuilder.Context.SaveChangesAsync();

                if (this.transactionScope != null)
                {
                    this.transactionScope.Commit();
                }
            }
            catch (Exception e)
            {
                this.Handle(e);
            }
        }

        private void InterceptSave(List<object> interceptedEntities)
        {
            List<IEntityEntry> modifiedAndNotIntercepted = this.GetModifiedEntities(this.contextBuilder.Context)
                .Where(e => !interceptedEntities.Contains(e.Entity)).ToList();

            if (modifiedAndNotIntercepted.Count == 0)
                return;

            foreach (IEntityEntry entry in modifiedAndNotIntercepted)
            {
                object entity = entry.Entity;

                Type entityType = ObjectContext.GetObjectType(entity.GetType());
                IEnumerable<IEntityInterceptor> entityInterceptors = this.interceptorsResolver.GetEntityInterceptors(entityType);

                if (entry.State == EntityEntryState.Deleted)
                {
                    this.Intercept(this.globalInterceptors, entity, (i, e) => i.OnDelete(e, this));
                    this.Intercept(entityInterceptors, entity, (i, e) => i.OnDelete(e, this));
                }
                else
                {
                    this.Intercept(this.globalInterceptors, entity, (i, e) => i.OnSave(e, this));
                    this.Intercept(entityInterceptors, entity, (i, e) => i.OnSave(e, this));
                }

                interceptedEntities.AddIfNotExists(entity);
            }

            this.InterceptSave(interceptedEntities);
        }

        private IEnumerable<IEntityEntry> GetModifiedEntities(DbContext context)
        {
            IEnumerable<IEntityEntry> modifiedEntities = this.contextUtilities.GetChangedEntities(context,
                s => s == EntityState.Added || s == EntityState.Modified || s == EntityState.Deleted);
            return modifiedEntities;
        }

        private void Intercept<T>(IEnumerable<T> interceptors, object entity, Action<T, IEntityEntry> intercept)
        {
            IEntityEntry entry = this.contextUtilities.GetEntry(entity, this.contextBuilder.Context);
            foreach (var interceptor in interceptors)
            {
                intercept(interceptor, entry);
            }
        }

        public void Add<T>(T entity) where T : class
        {
            this.contextBuilder.Context.Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this.contextBuilder.Context.Set<T>().Remove(entity);
        }

        public void BeginTransactionScope(SimplifiedIsolationLevel isolationLevel)
        {
            if (this.transactionScope != null)
                throw new InvalidOperationException("Cannot begin another transaction scope");

            this.transactionScope = this.contextBuilder.Context.Database.BeginTransaction((System.Data.IsolationLevel)isolationLevel);
        }

        public IEntityEntry<T> GetEntityEntry<T>(T entity) where T : class
        {
            return this.contextUtilities.GetEntry(entity, this.contextBuilder.Context);
        }

        public IEnumerable<IEntityEntry> GetEntityEntries()
        {
            return this.contextUtilities.GetEntries(this.contextBuilder.Context);
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
                if (this.transactionScope != null)
                    this.transactionScope.Dispose();

                if (this.contextBuilder != null)
                    this.contextBuilder.Dispose();
            }
        }
    }
}
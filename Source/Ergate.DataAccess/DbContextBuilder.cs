namespace Ergate.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;

    sealed class DbContextBuilder : IDisposable
    {
        private readonly IDbContextFactory factory;
        private readonly IInterceptorsResolver interceptorsResolver;
        private readonly IRepository repository;
        private readonly IDbContextUtilities contextUtilities;

        private IDbContextWrapper contextWrapper;
        private IEnumerable<IEntityInterceptor> globalInterceptors;

        public DbContextBuilder(IDbContextFactory factory, IInterceptorsResolver interceptorsResolver, IRepository repository, IDbContextUtilities contextUtilities)
        {
            this.factory = factory;
            this.interceptorsResolver = interceptorsResolver;
            this.repository = repository;
            this.contextUtilities = contextUtilities;
        }

        public DbContext Context
        {
            get
            {
                if (this.contextWrapper == null)
                    this.Init();

                return this.contextWrapper.Context;
            }
        }

        private void Init()
        {
            this.globalInterceptors = this.interceptorsResolver.GetGlobalInterceptors();

            this.contextWrapper = this.factory.CreateContext();
            this.contextWrapper.EntityLoaded += this.OnEntityLoaded;
        }

        private void OnEntityLoaded(object sender, EntityLoadedEventHandlerArgs e)
        {
            this.InterceptLoad(this.globalInterceptors, e.Entity);

            Type entityType = ObjectContext.GetObjectType(e.Entity.GetType());
            IEnumerable<IEntityInterceptor> entityInterceptors = this.interceptorsResolver.GetEntityInterceptors(entityType);
            this.InterceptLoad(entityInterceptors, e.Entity);
        }

        private void InterceptLoad(IEnumerable<IEntityInterceptor> interceptors, object entity)
        {
			IEntityEntry entry = this.contextUtilities.GetEntry(entity, this.Context);
            foreach (var interceptor in interceptors)
            {
                interceptor.OnLoad(entry, this.repository);
            }
        }

        public void Dispose()
        {
            if (this.contextWrapper != null)
            {
                this.contextWrapper.EntityLoaded -= this.OnEntityLoaded;
                this.contextWrapper.Dispose();
            }
        }
    }
}
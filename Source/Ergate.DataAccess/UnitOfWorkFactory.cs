namespace Ergate.DataAccess
{
    using Microsoft.Practices.ServiceLocation;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory contextFactory;
        private readonly IInterceptorsResolver interceptorsResolver;

        public UnitOfWorkFactory(IServiceLocator serviceLocator)
        {
            this.contextFactory = serviceLocator.GetInstance<IDbContextFactory>();
            this.interceptorsResolver = serviceLocator.GetInstance<IInterceptorsResolver>();
        }

        public UnitOfWorkFactory(IDbContextFactory contextFactory, IInterceptorsResolver interceptorsResolver)
        {
            this.contextFactory = contextFactory;
            this.interceptorsResolver = interceptorsResolver;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(this.contextFactory, this.interceptorsResolver);
        }
    }
}
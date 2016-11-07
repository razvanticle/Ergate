namespace iQuarc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ergate.Common.Extensions.Priority;

    using Microsoft.Practices.ServiceLocation;

    public class InterceptorsResolver : IInterceptorsResolver
    {
        private static readonly Type interceptorGenericType = typeof(IEntityInterceptor<>);

        private readonly IServiceLocator servicelocator;

        public InterceptorsResolver(IServiceLocator servicelocator)
        {
            this.servicelocator = servicelocator;
        }

        public IEnumerable<IEntityInterceptor> GetEntityInterceptors(Type entityType)
        {
            var interceptorType = interceptorGenericType.MakeGenericType(entityType);
            return this.servicelocator.GetAllInstances(interceptorType).Cast<IEntityInterceptor>();
        }

        public IEnumerable<IEntityInterceptor> GetGlobalInterceptors()
        {
            return this.servicelocator.GetAllInstances<IEntityInterceptor>().OrderByPriority();
        }
    }
}
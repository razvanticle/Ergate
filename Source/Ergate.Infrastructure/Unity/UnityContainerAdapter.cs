namespace Ergate.Infrastructure.Unity
{
    using System;
    using System.Collections.Generic;

    using Ergate.Infrastructure.AppBoot;
    using Ergate.Infrastructure.AppBoot.Container;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    internal sealed class UnityContainerAdapter : IDependencyContainer, IDisposable
    {
        private static readonly Dictionary<Lifetime, Func<ServiceInfo, LifetimeManager>> lifetimeManagers =
            new Dictionary<Lifetime, Func<ServiceInfo, LifetimeManager>>
                {
                    {
                        Lifetime.Instance, 
                        s => new PerResolveLifetimeManager()
                    }, 
                    {
                        Lifetime.AlwaysNew, 
                        s => new TransientLifetimeManager()
                    }, 
                    {
                        Lifetime.Application, 
                        s =>
                        new ContainerControlledLifetimeManager()
                    }
                };

        private readonly IUnityContainer container;

        public UnityContainerAdapter()
        {
            this.container = new UnityContainer();
            this.AsServiceLocator = new UnityServiceLocator(this.container);
        }

        private UnityContainerAdapter(IUnityContainer child)
        {
            this.container = child;
            this.AsServiceLocator = new UnityServiceLocator(child);
        }

        public IServiceLocator AsServiceLocator { get; }

        public IDependencyContainer CreateChildContainer()
        {
            var child = this.container.CreateChildContainer();
            return new UnityContainerAdapter(child);
        }

        public void Dispose()
        {
            this.container.Dispose();

            var serviceLocatorAsDisposable = this.AsServiceLocator as IDisposable;
            serviceLocatorAsDisposable?.Dispose();
        }

        public void RegisterInstance<T>(T instance)
        {
            this.container.RegisterInstance(instance);
        }

        public void RegisterService(ServiceInfo service)
        {
            var lifetime = GetLifetime(service);
            this.container.RegisterType(service.From, service.To, service.ContractName, lifetime);
        }

        private static LifetimeManager GetLifetime(ServiceInfo srv)
        {
            var factory = lifetimeManagers[srv.InstanceLifetime];
            return factory(srv);
        }
    }
}
namespace Ergate.Infrastructure.AppBoot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Ergate.Infrastructure.AppBoot.Container;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///     A class that starts the application and initializes it.
    /// </summary>
    public sealed class Bootstrapper : IBootstrapper, IDisposable
    {
        private IDependencyContainer container;
        private IServiceLocator serviceLocator;

        private readonly IEnumerable<Assembly> applicationAssemblies;
        private readonly List<IRegistrationBehavior> behaviors = new List<IRegistrationBehavior>();

        public Bootstrapper(IEnumerable<Assembly> applicationAssemblies)
        {
            this.applicationAssemblies = applicationAssemblies;
            this.Configuration = new BootstrapperConfig();
        }

        public IEnumerable<Assembly> ApplicationAssemblies
        {
            get { return this.applicationAssemblies; }
        }

        public IServiceLocator ServiceLocator
        {
            get { return this.serviceLocator; }
        }

        public BootstrapperConfig Configuration { get; private set; }

        public void AddRegistrationBehavior(IRegistrationBehavior behavior)
        {
            this.behaviors.Add(behavior);
        }

        public void Run()
        {
            this.ConfigureDependencyContainer();
            this.ConfigureContextManager();

            this.RegisterServices();

            this.InitApplication();
        }

        private void ConfigureDependencyContainer()
        {
            this.container = this.Configuration.GetSetting<IDependencyContainer>();
            this.serviceLocator = this.container.AsServiceLocator;
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(this.GetServiceLocator);
        }

        private IServiceLocator GetServiceLocator()
        {
            if (OperationContext.Current != null)
                return OperationContext.Current.ServiceLocator;

            return this.serviceLocator;
        }

        private void ConfigureContextManager()
        {
            IContextStore contextStore = this.Configuration.GetSetting<IContextStore>();

            ContextManager.GlobalContainer = this.container;
            ContextManager.SetContextStore(contextStore);
        }

        private void RegisterServices()
        {
            RegistrationsCatalog catalog = new RegistrationsCatalog();

            IEnumerable<Type> types = this.applicationAssemblies.SelectMany(a => a.GetTypes());

            foreach (Type type in types)
            {
                for (int i = 0; i < this.behaviors.Count; i++)
                {
                    IRegistrationBehavior behavior = this.behaviors[i];

                    IEnumerable<ServiceInfo> registrations = behavior.GetServicesFrom(type);
                    foreach (ServiceInfo reg in registrations)
                        catalog.Add(reg, i);
                }
            }

            foreach (ServiceInfo registration in catalog)
                this.container.RegisterService(registration);
        }

        private void InitApplication()
        {
            Application application = this.serviceLocator.GetInstance<Application>();
            application.Initialize();
        }

        public void Dispose()
        {
            IDisposable disposable = this.container as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    }
}
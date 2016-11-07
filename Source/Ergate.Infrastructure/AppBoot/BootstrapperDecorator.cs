namespace Ergate.Infrastructure.AppBoot
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Ergate.Infrastructure.AppBoot.Container;

    public abstract class BootstrapperDecorator : IBootstrapper, IDisposable
    {
        private readonly IBootstrapper bootstrapper;

        protected BootstrapperDecorator(IBootstrapper bootstrapper)
        {
            this.bootstrapper = bootstrapper;
        }

        public IEnumerable<Assembly> ApplicationAssemblies
        {
            get { return this.bootstrapper.ApplicationAssemblies; }
        }

        public BootstrapperConfig Configuration
        {
            get { return this.bootstrapper.Configuration; }
        }

        public virtual void AddRegistrationBehavior(IRegistrationBehavior behavior)
        {
            this.bootstrapper.AddRegistrationBehavior(behavior);
        }

        public virtual void Run()
        {
            this.bootstrapper.Run();
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
                IDisposable disposable = this.bootstrapper as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }
    }
}
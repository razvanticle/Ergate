namespace Ergate.Infrastructure.AppBoot
{
    using System.Collections.Generic;
    using System.Reflection;

    using Ergate.Infrastructure.AppBoot.Container;

    public interface IBootstrapper
    {
        IEnumerable<Assembly> ApplicationAssemblies { get; }
        BootstrapperConfig Configuration { get; }
        void AddRegistrationBehavior(IRegistrationBehavior behavior);
        void Run();
    }
}
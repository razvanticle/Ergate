namespace Ergate.Infrastructure.Unity
{
    using Ergate.Infrastructure.AppBoot;

    public static class BootstrapperConfigureExtensions
    {
        public static IBootstrapper ConfigureWithUnity(this IBootstrapper bootstrapper)
        {
            return bootstrapper.ConfigureWith(new UnityContainerAdapter());
        }
    }
}
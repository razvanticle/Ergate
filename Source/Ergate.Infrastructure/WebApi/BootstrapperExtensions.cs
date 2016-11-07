namespace Ergate.Infrastructure.WebApi
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    using Ergate.Infrastructure.AppBoot;

    public static class BootstrapperExtensions
    {
        public static Bootstrapper ConfigureWebApi(this Bootstrapper bootstrapper, HttpConfiguration config)
        {
            bootstrapper.ConfigureWith(new HttpRequestContextStore());

            var serviceLocator = bootstrapper.ServiceLocator;
            config.DependencyResolver = new DependencyContainerResolver(serviceLocator);

            // todo move this to a nicer place
            config.Services.Replace(typeof(IExceptionHandler), new WebApiExceptionHandler());

            return bootstrapper;
        }
    }
}
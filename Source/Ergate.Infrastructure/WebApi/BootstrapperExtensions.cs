namespace Ergate.Infrastructure.WebApi
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    using Ergate.Infrastructure.AppBoot;

    using Microsoft.Practices.ServiceLocation;

    public static class BootstrapperExtensions
	{
		public static Bootstrapper ConfigureWebApi(this Bootstrapper bootstrapper, HttpConfiguration config)
		{
		    bootstrapper.ConfigureWith(new HttpRequestContextStore());

			IServiceLocator serviceLocator = bootstrapper.ServiceLocator;
			config.DependencyResolver = new DependencyContainerResolver(serviceLocator);

            //todo move this to a nicer place
		    IExceptionHandler exceptionHandler = serviceLocator.GetInstance<IExceptionHandler>();
            config.Services.Replace(typeof(IExceptionHandler), exceptionHandler);

            return bootstrapper;
		}
	}
}
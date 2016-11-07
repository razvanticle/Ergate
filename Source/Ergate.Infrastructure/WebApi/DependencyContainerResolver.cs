namespace Ergate.Infrastructure.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;

    using Ergate.Infrastructure.AppBoot;

    using Microsoft.Practices.ServiceLocation;

    public sealed class DependencyContainerResolver : IDependencyResolver
	{
		private readonly DependencyScope rootResolver;

		public DependencyContainerResolver(IServiceLocator serviceLocator)
		{
			this.rootResolver = new DependencyScope(serviceLocator);
		}

		public IExceptionLogger Logger
		{
			get { return this.rootResolver.Logger; }
			set { this.rootResolver.Logger = value; }
		}

		public object GetService(Type serviceType)
		{
			return this.rootResolver.GetService(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return this.rootResolver.GetServices(serviceType);
		}

		public void Dispose()
		{
			this.rootResolver.Dispose();
		}

		public IDependencyScope BeginScope()
		{
			OperationContext context = OperationContext.CreateNew();
			return new DependencyScope(context) {Logger = this.Logger};	
		}

		private class DependencyScope : IDependencyScope
		{
			private readonly IServiceLocator serviceLocator;
			private readonly OperationContext context;

			public DependencyScope(IServiceLocator serviceLocator)
			{
				this.serviceLocator = serviceLocator;
			}

			public DependencyScope(OperationContext context)
			{
				this.context = context;
				this.serviceLocator = context.ServiceLocator;
			}

			public void Dispose()
			{
				if (this.context != null)
					this.context.Dispose();

				IDisposable disposable = this.serviceLocator as IDisposable;
				if (disposable != null)
					disposable.Dispose();
			}

			public IExceptionLogger Logger { get; set; }

			public object GetService(Type serviceType)
			{
				try
				{
					return this.serviceLocator.GetInstance(serviceType);
				}
				catch (ActivationException ex)
				{
					this.Log(ex);
					return null;
				}
			}

			public IEnumerable<object> GetServices(Type serviceType)
			{
				try
				{
					return this.serviceLocator.GetAllInstances(serviceType);
				}
				catch (ActivationException ex)
				{
					this.Log(ex);
					return Enumerable.Empty<object>();
				}
			}

			private void Log(Exception exception)
			{
				if (this.Logger != null)
					this.Logger.Log(exception);
			}
		}
	}
}
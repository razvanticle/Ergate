namespace Ergate.Infrastructure.UnitTests.Unity
{
    using Microsoft.Practices.Unity;

    using Xunit;

    public class ConstructorInjectionTests
	{
		[Fact]
		public void ConstructorInjectionGivesSameInstancesAndServiceLocatorInjectionGivesNewInstance()
		{
			IUnityContainer container = new UnityContainer()
				.RegisterType<IController, Controller>(new PerResolveLifetimeManager())
				.RegisterType<IService1, Service1>(new PerResolveLifetimeManager())
				.RegisterType<IService2, Service2>(new PerResolveLifetimeManager())
				.RegisterType<IService3, Service3>(new PerResolveLifetimeManager())
				.RegisterType<IRepository, Repository>(new PerResolveLifetimeManager());

			Controller controller = (Controller) container.Resolve<IController>();

			Assert.Same(controller.S1.Repository, controller.S2.Repository);
			Assert.NotSame(controller.S1.Repository, controller.S3.Repository);
		}

		private interface IController
		{
		}

		private class Controller : IController
		{
			public IService1 S1 { get; set; }
			public IService2 S2 { get; set; }
			public IService3 S3 { get; set; }

			public Controller(IService1 s1, IService2 s2, IService3 s3)
			{
				this.S1 = s1;
				this.S2 = s2;
				this.S3 = s3;
			}
		}

		internal interface IService2
		{
			IRepository Repository { get; }
		}

		private class Service2 : IService2
		{
			public IRepository Repository { get; set; }

			public Service2(IRepository repository)
			{
				this.Repository = repository;
			}
		}

		internal interface IService1
		{
			IRepository Repository { get; }
		}

		private class Service1 : IService1
		{
			public IRepository Repository { get; set; }

			public Service1(IRepository repository)
			{
				this.Repository = repository;
			}
		}

		internal interface IService3
		{
			IRepository Repository { get; }
		}

		private class Service3 : IService3
		{
			public IRepository Repository { get; set; }

			public Service3(IUnityContainer container)
			{
				//serviceLocator.GetInstance<IRepository>()
				this.Repository = container.Resolve<IRepository>();
			}
		}

		internal interface IRepository
		{
		}

		private class Repository : IRepository
		{
		}
	}
}
namespace Ergate.Infrastructure.UnitTests.Unity
{
    using Microsoft.Practices.Unity;

    using Xunit;

    public class PerResolveLifetimeManagerTests
	{
		[Fact]
		public void PerResolveUsedForTheView_SameViewInstanceInjected()
		{
			IUnityContainer container = new UnityContainer()
				.RegisterType<IPresenter, Presenter>(new TransientLifetimeManager())
				.RegisterType<IView, View>(new PerResolveLifetimeManager());

			IView view = container.Resolve<IView>();
			IView viewOfThePresenter = ((Presenter) view.Presenter).View;

			Assert.Same(view, viewOfThePresenter);
		}

		[Fact]
		public void TransientUsedForPresenterAndPerResolveForTheView_DifferentInstancesForThePresenter()
		{
			IUnityContainer container = new UnityContainer()
				.RegisterType<IPresenter, Presenter>(new TransientLifetimeManager())
				.RegisterType<IView, View>(new PerResolveLifetimeManager());

			IPresenter p = container.Resolve<IPresenter>();
			IPresenter presenterOfTheView = ((Presenter) p).View.Presenter;

			Assert.NotSame(p, presenterOfTheView);
		}

		public interface IPresenter
		{
		}

		public class Presenter : IPresenter
		{
			public Presenter(IView view)
			{
				this.View = view;
			}

			public IView View { get; set; }
		}

		public interface IView
		{
			IPresenter Presenter { get; set; }
		}

		public class View : IView
		{
			[Dependency]
			public IPresenter Presenter { get; set; }
		}
	}
}
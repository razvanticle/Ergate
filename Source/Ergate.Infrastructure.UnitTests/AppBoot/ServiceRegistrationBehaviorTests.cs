namespace Ergate.Infrastructure.UnitTests.AppBoot
{
    using System;
    using System.Collections.Generic;

    using Ergate.Infrastructure.AppBoot.Container;

    using iQuarc.xUnitEx;

    using Moq;

    using Xunit;

    public class ServiceRegistrationBehaviorTests
	{
		[Fact]
		public void GetServicesFrom_NotDecoratedWithServiceAttribute_NoServicesReturned()
		{
			ServiceRegistrationBehavior behavior = new ServiceRegistrationBehavior();

			IEnumerable<ServiceInfo> services = behavior.GetServicesFrom(typeof (MyNonService));

			Assert.Empty(services);
		}

		[Fact]
		public void GetServicesFrom_DecoratedWithAttribute_ServiceInfoWithAttributeParameters()
		{
			ServiceRegistrationBehavior behavior = new ServiceRegistrationBehavior();

			IEnumerable<ServiceInfo> services = behavior.GetServicesFrom(typeof (MyService));

			ServiceInfo expected = new ServiceInfo(typeof (IMyService), typeof (MyService),
				"SomeContractName", Lifetime.Application);
			AssertEx.AreEquivalent(services, this.ServiceInfoEquals, expected);
		}

		[Fact]
		public void GetServicesFrom_DecoratedWithTwoAttributes_TwoServiceInfoReturned()
		{
			ServiceInfo expected1 = new ServiceInfo(typeof (IMyService), typeof (MyDoubleService), "SomeContractName",
				Lifetime.Application);
			ServiceInfo expected2 = new ServiceInfo(typeof (IMyService), typeof (MyDoubleService),
				"SomeOtherContractName", Lifetime.Instance);

			ServiceRegistrationBehavior behavior = new ServiceRegistrationBehavior();

			IEnumerable<ServiceInfo> services = behavior.GetServicesFrom(typeof (MyDoubleService));

			AssertEx.AreEquivalent(services, this.ServiceInfoEquals, expected1, expected2);
		}

		[Fact]
		public void GetServicesFrom_ServiceAttributeDoesNotHaveExportType_DecoratedTypeUsedAsFromType()
		{
			ServiceAttribute service = new ServiceAttribute();
			Type fakeType = this.GetFakeType(service);

			ServiceRegistrationBehavior behavior = this.GetTarget();

			IEnumerable<ServiceInfo> services = behavior.GetServicesFrom(fakeType);

			ServiceInfo[] expected = {new ServiceInfo(fakeType, fakeType, Lifetime.Instance)};
			AssertEx.AreEquivalent(services, this.ServiceInfoEquals, expected);
		}

		private ServiceRegistrationBehavior GetTarget()
		{
			return new ServiceRegistrationBehavior();
		}

		private Type GetFakeType(ServiceAttribute serviceAttribute)
		{
			Mock<Type> type = new Mock<Type>();
			type.Setup(t => t.GetCustomAttributes(It.IsAny<Type>(), It.IsAny<bool>()))
				.Returns(new object[] {serviceAttribute});

			return type.Object;
		}

		private bool ServiceInfoEquals(ServiceInfo s1, ServiceInfo s2)
		{
			return s1.From == s2.From &&
			       s1.To == s2.To &&
			       s1.ContractName == s2.ContractName &&
			       s1.InstanceLifetime == s2.InstanceLifetime;
		}

		[Service("SomeContractName", typeof (IMyService), Lifetime.Application)]
		private class MyService : IMyService
		{
		}

		internal interface IMyService
		{
		}

		private class MyNonService
		{
		}

		[Service("SomeContractName", typeof (IMyService), Lifetime.Application)]
		[Service("SomeOtherContractName", typeof (IMyService), Lifetime.Instance)]
		private class MyDoubleService : IMyService
		{
		}
	}
}
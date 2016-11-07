namespace Ergate.Infrastructure.AppBoot.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ergate.Common.Extensions;

    /// <summary>
	///     Gives service information to be registered to Dependency Injection Container, based on the ServiceAttribute
	///     If more service attribute decorations are on current type more ServiceInfo are returned, one for each attribute
	/// </summary>
	public sealed class ServiceRegistrationBehavior : IRegistrationBehavior
	{
		public IEnumerable<ServiceInfo> GetServicesFrom(Type type)
		{
			IEnumerable<ServiceAttribute> attributes = type.GetAttributes<ServiceAttribute>(false);
			return attributes.Select(a =>
				new ServiceInfo(a.ExportType ?? type, type, a.ContractName, a.Lifetime));
		}
	}
}
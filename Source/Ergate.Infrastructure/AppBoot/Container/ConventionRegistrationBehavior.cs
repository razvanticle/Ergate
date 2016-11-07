namespace Ergate.Infrastructure.AppBoot.Container
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class ConventionRegistrationBehavior : IRegistrationBehavior
	{
		private readonly IList<ServiceBuilder> builders = new List<ServiceBuilder>();

		public IEnumerable<ServiceInfo> GetServicesFrom(Type type)
		{
			IEnumerable<ServiceInfo> services = this.builders.SelectMany(x => x.GetServicesFrom(type));
			return services;
		}

		public ServiceBuilder ForType(Type type)
		{
			ServiceBuilder builder = CreateServiceBuilder(x => x == type);
			this.builders.Add(builder);
			return builder;
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience call")]
        public ServiceBuilder ForType<T>()
		{
			return this.ForType(typeof (T));
		}

		public ServiceBuilder ForTypesDerivedFrom(Type type)
		{
			ServiceBuilder builder = CreateServiceBuilder(x => type.IsAssignableFrom(x));
			this.builders.Add(builder);
			return builder;
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience call")]
        public ServiceBuilder ForTypesDerivedFrom<T>()
		{
			return this.ForTypesDerivedFrom(typeof (T));
		}

		public ServiceBuilder ForTypesMatching(Predicate<Type> typeFilter)
		{
			ServiceBuilder builder = CreateServiceBuilder(typeFilter);
			this.builders.Add(builder);
			return builder;
		}


		private static ServiceBuilder CreateServiceBuilder(Predicate<Type> typeFilter)
		{
			return new ServiceBuilder(typeFilter);
		}
	}
}
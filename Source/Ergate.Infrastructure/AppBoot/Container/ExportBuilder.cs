namespace Ergate.Infrastructure.AppBoot.Container
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class ExportBuilder
	{
		private Type contractType;
		private string contractName;
		private Lifetime life = Lifetime.Instance;

		internal ExportBuilder(Type fromType)
		{
			this.FromType = fromType;
		}

		public Type FromType { get; private set; }

		public ExportBuilder AsContractName(string name)
		{
			this.contractName = name;
			return this;
		}

		public ExportBuilder AsContractType(Type type)
		{
			this.contractType = type;
			return this;
		}
        
        public ExportBuilder AsContractType<T>()
		{
			return this.AsContractType(typeof (T));
		}

		public ExportBuilder WithLifetime(Lifetime lifetime)
		{
			this.life = lifetime;
			return this;
		}

		internal ServiceInfo GetServiceInfo(Type type)
		{
			return new ServiceInfo(this.contractType ?? type, type, this.contractName, this.life);
		}
	}
}
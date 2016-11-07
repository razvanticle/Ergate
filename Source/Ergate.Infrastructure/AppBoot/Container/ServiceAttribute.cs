namespace Ergate.Infrastructure.AppBoot.Container
{
    using System;

    /// <summary>
    ///     Declares a service implementation, by decorating the class that implements it.
    ///     It may also specify the lifetime of the service instance by using the Lifetime enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute()
        {
        }

        public ServiceAttribute(Type exportType)
            : this(null, exportType)
        {
        }

        public ServiceAttribute(string contractName)
            : this(contractName, null)
        {
        }

        public ServiceAttribute(Lifetime lifetime)
            : this(null, null, lifetime)
        {
        }

        public ServiceAttribute(string contractName, Lifetime lifetime)
            : this(contractName, null, lifetime)
        {
        }

        public ServiceAttribute(string contractName, Type exportType)
            : this(contractName, exportType, Lifetime.Instance)
        {
        }

        public ServiceAttribute(Type exportType, Lifetime lifetime)
            : this(null, exportType, lifetime)
        {
        }

        public ServiceAttribute(string contractName, Type exportType, Lifetime lifetime)
        {
            this.ContractName = contractName;
            this.ExportType = exportType;
            this.Lifetime = lifetime;
        }

        public string ContractName { get; private set; }

        public Type ExportType { get; private set; }

        public Lifetime Lifetime { get; private set; }
    }
}
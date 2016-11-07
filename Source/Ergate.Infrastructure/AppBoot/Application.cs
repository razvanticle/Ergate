namespace Ergate.Infrastructure.AppBoot
{
    using System.Collections.Generic;

    using Ergate.Common.Extensions.Priority;

    internal sealed class Application
    {
        public Application(IModule[] modules)
        {
            this.Modules = modules.OrderByPriority();
        }

        public IEnumerable<IModule> Modules { get; }

        public void Initialize()
        {
            if (this.Modules != null)
            {
                foreach (var module in this.Modules)
                {
                    module.Initialize();
                }
            }
        }
    }
}
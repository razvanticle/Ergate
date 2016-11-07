namespace Ergate.Infrastructure.AppBoot
{
    using System;
    using System.Collections.Generic;

    public class BootstrapperConfig
    {
        private readonly Dictionary<Type, object> settings = new Dictionary<Type, object>()
        {
            {typeof (IContextStore), new CallContextStore()}
        };

        public void SetSettingInstance<T>(T instance)
        {
            this.settings[typeof (T)] = instance;
        }

        public T GetSetting<T>()
        {
            object setting;
            if (this.settings.TryGetValue(typeof (T), out setting))
                return (T) setting;

            throw new InvalidOperationException(string.Format("Setting for {0} not found", typeof (T).Name));
        }
    }
}
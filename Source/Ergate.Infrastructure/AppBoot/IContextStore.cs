﻿namespace Ergate.Infrastructure.AppBoot
{
    public interface IContextStore
    {
        object GetContext(string key);
        void SetContext(object context, string key);
    }
}
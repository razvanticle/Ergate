namespace Ergate.Infrastructure.AppBoot
{
    using System.Runtime.Remoting.Messaging;

    public class CallContextStore : IContextStore
    {
        public object GetContext(string key)
        {
            return CallContext.LogicalGetData(key);
        }

        public void SetContext(object context, string key)
        {
            CallContext.LogicalSetData(key, context);
        }
    }
}
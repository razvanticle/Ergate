namespace Ergate.Infrastructure.AppBoot
{
    using System;
    using System.Collections;

    using Microsoft.Practices.ServiceLocation;

    public sealed class OperationContext : IDisposable
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IDependencyContainer container;

        private Hashtable items;

        private bool isDisposed;

        private OperationContext()
        {
            this.container = ContextManager.GlobalContainer.CreateChildContainer();
            this.serviceLocator = this.container.AsServiceLocator;
        }

        public IDictionary Items
        {
            get
            {
                if (this.items == null)
                    this.items = new Hashtable();
                return this.items;
            }
        }

        public IServiceLocator ServiceLocator
        {
            get { return this.serviceLocator; }
        }

        public void Dispose()
        {
            if (this.isDisposed)
                return;

            this.DisposeItems();

            IDisposable c = this.container as IDisposable;
            if (c != null)
                c.Dispose();

            this.isDisposed = true;
        }

        private void DisposeItems()
        {
            foreach (var key in this.Items.Keys)
            {
                IDisposable disposableKey = key as IDisposable;
                if (disposableKey != null)
                    disposableKey.Dispose();

                IDisposable disposableValue = this.Items[key] as IDisposable;
                if (disposableValue != null)
                    disposableValue.Dispose();
            }
        }

        public static OperationContext Current
        {
            get { return ContextManager.Current; }
        }

        public static OperationContext CreateNew()
        {
            OperationContext operationContext = new OperationContext();
            ContextManager.SwitchContext(operationContext);
            return operationContext;
        }
    }
}
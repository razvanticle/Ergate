namespace Ergate.DataAccess
{
    using System;
    using System.Data.Entity;

    public interface IDbContextWrapper : IDisposable
    {
        DbContext Context { get; }
        event EntityLoadedEventHandler EntityLoaded;
    }

    public delegate void EntityLoadedEventHandler(object sender, EntityLoadedEventHandlerArgs e);

    public class EntityLoadedEventHandlerArgs : EventArgs
    {
        public object Entity { get; private set; }

        public EntityLoadedEventHandlerArgs(object entity)
        {
            this.Entity = entity;
        }
    }
}
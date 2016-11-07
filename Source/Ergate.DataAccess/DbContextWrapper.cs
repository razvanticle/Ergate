namespace Ergate.DataAccess
{
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public sealed class DbContextWrapper : IDbContextWrapper
    {
        private readonly ObjectContext objectContext;

        public DbContextWrapper(DbContext context)
        {
            this.Context = context;

            this.objectContext = ((IObjectContextAdapter) context).ObjectContext;
            this.objectContext.ObjectMaterialized += this.ObjectMaterializedHandler;
        }

        private void ObjectMaterializedHandler(object sender, ObjectMaterializedEventArgs e)
        {
            EntityLoadedEventHandler handler = this.EntityLoaded;
            if (handler != null)
                handler(this, new EntityLoadedEventHandlerArgs(e.Entity));
        }

        public DbContext Context { get; private set; }

        public event EntityLoadedEventHandler EntityLoaded;

        public void Dispose()
        {
            this.objectContext.ObjectMaterialized -= this.ObjectMaterializedHandler;
            this.Context.Dispose();
        }
    }
}
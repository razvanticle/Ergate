namespace DataModel
{
    using iQuarc.DataAccess;
    using Ergate.Infrastructure.AppBoot.Container;

    [Service(typeof(IDbContextFactory))]
    public class DbContextFactory : DbContextFactory<ErgateEntities>
    {
    }
}
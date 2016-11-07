namespace DataModel
{
    using iQuarc.AppBoot;
    using iQuarc.DataAccess;

    [Service(typeof(IDbContextFactory))]
    public class DbContextFactory : DbContextFactory<ErgateEntities>
    {
    }
}
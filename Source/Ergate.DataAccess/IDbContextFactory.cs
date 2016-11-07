namespace Ergate.DataAccess
{
    public interface IDbContextFactory
    {
        IDbContextWrapper CreateContext();
    }
}
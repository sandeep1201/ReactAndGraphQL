namespace Dcf.Wwp.DataAccess.Interfaces
{
    public interface IDbContextFactory
    {
        IDbContext Create(string connectionString);
    }
}
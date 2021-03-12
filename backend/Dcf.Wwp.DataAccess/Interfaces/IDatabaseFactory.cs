using System;

namespace Dcf.Wwp.DataAccess.Interfaces
{
    public interface IDatabaseFactory : IDisposable
    {
        IDbContext GetContext();
        //BaseDbContext GetContext();
    }
}

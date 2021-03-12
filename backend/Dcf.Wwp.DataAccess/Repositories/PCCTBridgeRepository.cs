using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PCCTBridgeRepository : RepositoryBase<PCCTBridge>, IPCCTBridgeRepository
    {
        public PCCTBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}

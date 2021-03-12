﻿using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JRWorkPreferenceShiftBridgeRepository : RepositoryBase<JRWorkPreferenceShiftBridge>, IJRWorkPreferenceShiftBridgeRepository
    {
        public JRWorkPreferenceShiftBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
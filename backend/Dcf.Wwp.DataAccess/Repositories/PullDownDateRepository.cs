﻿using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PullDownDateRepository : RepositoryBase<PullDownDate>, IPullDownDateRepository
    {
        public PullDownDateRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}

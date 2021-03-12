﻿using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAIPVOccurrenceRepository : RepositoryBase<EAIPVOccurrence>, IEAIPVOccurrenceRepository
    {
        public EAIPVOccurrenceRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}

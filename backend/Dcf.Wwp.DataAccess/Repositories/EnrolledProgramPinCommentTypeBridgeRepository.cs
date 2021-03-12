﻿using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EnrolledProgramPinCommentTypeBridgeRepository : RepositoryBase<EnrolledProgramPinCommentTypeBridge>, IEnrolledProgramPinCommentTypeBridgeRepository
    {
        public EnrolledProgramPinCommentTypeBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockParticipantPlacementRepository : MockRepositoryBase<ParticipantPlacement>, IParticipantPlacementRepository
    {
        #region Properties

        public bool IsPOPClaim = false;

        #endregion

        #region Methods

        public new IEnumerable<ParticipantPlacement> GetMany(Expression<Func<ParticipantPlacement, bool>> clause)
        {
            var placements = new List<ParticipantPlacement>();

            if (IsPOPClaim)
            {
                var placement = new ParticipantPlacement
                                           {
                                               PlacementStartDate = new DateTime(2020, 08, 20)
                                           };

                placements.Add(placement);
            }

            return placements;
        }

        #endregion
    }
}

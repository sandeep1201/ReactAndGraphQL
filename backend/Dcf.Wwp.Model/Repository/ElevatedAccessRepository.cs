using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IElevatedAccessRepository
    {
        public IElevatedAccess NewElevatedAccess(string user, int workerId, int participantId,  int? earId, string details)
        {
            var ea = new ElevatedAccess
                     {
                         WorkerId               = workerId,
                         ParticipantId          = participantId,
                         AccessCreateDate       = DateTime.Now,
                         ElevatedAccessReasonId = earId,
                         Details                = details,
                         ModifiedBy             = user,
                         ModifiedDate           = DateTime.Now
                     };

            _db.ElevatedAccesses.Add(ea);

            return ea;
        }
    }
}

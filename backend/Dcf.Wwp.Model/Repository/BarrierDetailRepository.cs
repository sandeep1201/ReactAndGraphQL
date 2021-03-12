using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IBarrierDetailRepository
    {
        public IBarrierDetail BarrierDetailById(int? barrierId)
        {
            //return (from x in _db.BarrierDetails where x.Id == barrierId where x.IsDeleted == false select x).SingleOrDefault();

            var barrierDetail = _db.BarrierDetails.FirstOrDefault(i => i.Id == barrierId && i.IsDeleted == false);

            return (barrierDetail);
        }

        public IBarrierDetail NewBarrierDetailInfo(IParticipant participant, string user)
        {
            IBarrierDetail bd = new BarrierDetail
                                {
                                    ParticipantId = participant.Id,
                                    IsDeleted     = false,
                                    ModifiedBy    = _authUser.Username,
                                    ModifiedDate  = DateTime.Now
                                };

            _db.BarrierDetails.Add((BarrierDetail) bd);

            return (bd);
        }

        public IEnumerable<IBarrierDetail> BarrierDetailsByParticipantId(int participantId)
        {
            //return  (from x in _db.BarrierDetails where x.ParticipantId == participantId && !x.EndDate.HasValue && x.IsDeleted != true select x);

            var barrierDetails = _db.BarrierDetails.Where(i => i.ParticipantId == participantId && !i.EndDate.HasValue && !i.IsDeleted).AsEnumerable();

            return (barrierDetails);
        }
    }
}

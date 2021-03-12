using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialParentReferralSectionRepository
    {
        public INonCustodialParentsReferralSection NewNonCustodialParentsReferralSection(int participantId, string user)
        {
            var section = new NonCustodialParentsReferralSection
            {
                ParticipantId = participantId,
                ModifiedBy = user,
                ModifiedDate = DateTime.Now
            };

            _db.NonCustodialParentsReferralSections.Add(section);

            return section;
        }
    }
}
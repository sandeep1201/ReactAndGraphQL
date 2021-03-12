using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialParentSectionRepository
    {
        public INonCustodialParentsSection NewNonCustodialParentsSection(int participantId, string user)
        {
            var section = new NonCustodialParentsSection
            {
                ParticipantId = participantId,
                ModifiedBy = user,
                ModifiedDate = DateTime.Now
            };

            _db.NonCustodialParentsSections.Add(section);

            return section;
        }
    }
}
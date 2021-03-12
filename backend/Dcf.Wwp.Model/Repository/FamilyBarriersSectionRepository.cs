using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public  partial class Repository: IFamilyBarriersSectionRepository
    {
        public IFamilyBarriersSection NewFamilyBarriersSection(IParticipant participant, string user)
        {
            var section = new FamilyBarriersSection()
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user,
                ParticipantId = participant.Id
            };
            _db.FamilyBarriersSections.Add(section);

            return section;
        }
    }
}

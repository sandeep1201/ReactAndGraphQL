using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IParticipantContactInfoRepository
    {
        public IParticipantContactInfo NewParticipantContactInfo(IParticipant parent)
        {
            var newParticipantContactInfo = new ParticipantContactInfo() as IParticipantContactInfo;

            newParticipantContactInfo.Participant  = parent;
            newParticipantContactInfo.ModifiedBy   = _authUser.Username;
            newParticipantContactInfo.ModifiedDate = DateTime.Now;
            newParticipantContactInfo.CreatedDate  = _authUser.CDODate ?? DateTime.Now;

            _db.ParticipantContactInfoes.Add((ParticipantContactInfo) newParticipantContactInfo);

            return (newParticipantContactInfo);
        }
    }
}

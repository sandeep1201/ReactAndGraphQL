using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEARequestParticipantBridgeRepository
    {
        public IEARequestParticipantBridge NewEaRequestParticipantBridge(IParticipant participant)
        {
            var newEaRequestParticipantBridge = new EARequestParticipantBridge() as IEARequestParticipantBridge;

            newEaRequestParticipantBridge.Participant  = participant;
            newEaRequestParticipantBridge.ModifiedBy   = _authUser.WIUID;
            newEaRequestParticipantBridge.ModifiedDate = DateTime.Now;

            _db.EaRequestParticipantBridges.Add((EARequestParticipantBridge) newEaRequestParticipantBridge);

            return (newEaRequestParticipantBridge);
        }
    }
}

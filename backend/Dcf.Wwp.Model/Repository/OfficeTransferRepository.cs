using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IOfficeTransferRepository
    {
        public IOfficeTransfer NewOfficeTransfer(IParticipantEnrolledProgram participantEnrolledProgram,
                                                 IOffice                     sourceOffice,
                                                 IOffice                     destinationOffice,
                                                 int?                        workerId,
                                                 string                      user)
        {
            if (participantEnrolledProgram == null)
                throw new ArgumentNullException(nameof(participantEnrolledProgram));

            IOfficeTransfer officeTransfer = new OfficeTransfer
            {
                ParticipantId                = participantEnrolledProgram.ParticipantId,
                ParticipantEnrolledProgramId = participantEnrolledProgram.Id,
                SourceAssignedWorkerId       = participantEnrolledProgram.WorkerId,
                SourceOfficeId               = sourceOffice?.Id,
                DestinationOfficeId          = destinationOffice?.Id,
                DestinationAssignedWorkerId  = workerId,
                TransferDate                 = _authUser.CDODate ?? DateTime.Now,
                ModifiedDate                 = DateTime.Now,
                ModifiedBy                   = user,
                IsDeleted                    = false
            };

            _db.OfficeTransfers.Add((OfficeTransfer) officeTransfer);

            return officeTransfer;
        }

        public bool hasOfficeTransfer (int? participantId)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var officeTransfer = _db.OfficeTransfers.Any(i => i.ParticipantId == participantId && EntityFunctions.TruncateTime(i.TransferDate) == currentDate);

            return officeTransfer;

        }
    }
}

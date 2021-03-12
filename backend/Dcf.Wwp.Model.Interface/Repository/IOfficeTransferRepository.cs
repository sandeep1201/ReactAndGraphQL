using System;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IOfficeTransferRepository
    {
        IOfficeTransfer NewOfficeTransfer(IParticipantEnrolledProgram participantEnrolledProgram, IOffice sourceOffice, IOffice destinationOffice, int? workerId, string user);

        bool hasOfficeTransfer(int? participantId);
    }
}
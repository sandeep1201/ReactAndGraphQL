using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IOfficeTransfer : ICommonDelModel
    {
        int           ParticipantEnrolledProgramId { get; set; }
        int           ParticipantId                { get; set; }
        Nullable<int> SourceOfficeId               { get; set; }
        Nullable<int> SourceAssignedWorkerId       { get; set; }
        Nullable<int> DestinationOfficeId          { get; set; }
        Nullable<int> DestinationAssignedWorkerId  { get; set; }
        DateTime      TransferDate                 { get; set; }
    }
}

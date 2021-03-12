using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWorker
    {
        int       Id                     { get; set; }
        string    WAMSId                 { get; set; }
        string    MFUserId               { get; set; }
        string    FirstName              { get; set; }
        string    LastName               { get; set; }
        string    MiddleInitial          { get; set; }
        string    SuffixName             { get; set; }
        string    Roles                  { get; set; }
        string    WorkerActiveStatusCode { get; set; }
        DateTime? LastLogin              { get; set; }
        int?      OrganizationId         { get; set; }
        bool      IsDeleted              { get; set; }
        string    ModifiedBy             { get; set; }
        DateTime? ModifiedDate           { get; set; }
        byte[]    RowVersion             { get; set; }
        string    WIUID                  { get; set; }

        ICollection<IParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }
        ICollection<IRecentParticipant>          RecentParticipants          { get; set; }
        IOrganization                            Organization                { get; set; }
        ICollection<IConfidentialPinInformation> ConfidentialPinInformations { get; set; }
        ICollection<IWorkerTaskList>             WorkerTaskLists             { get; set; }
    }
}

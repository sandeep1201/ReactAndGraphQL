using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Worker : BaseEntity
    {
        #region Properties

        public string    WAMSId                 { get; set; }
        public string    MFUserId               { get; set; }
        public string    WIUID                  { get; set; }
        public string    FirstName              { get; set; }
        public string    LastName               { get; set; }
        public string    MiddleInitial          { get; set; }
        public string    SuffixName             { get; set; }
        public string    Roles                  { get; set; }
        public string    WorkerActiveStatusCode { get; set; }
        public DateTime? LastLogin              { get; set; }
        public int?      OrganizationId         { get; set; }
        public bool      IsDeleted              { get; set; }
        public string    ModifiedBy             { get; set; }
        public DateTime? ModifiedDate           { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ConfidentialPinInformation> ConfidentialPinInformations { get; set; }

        public virtual ICollection<ElevatedAccess> ElevatedAccesses { get; set; }

        // public virtual ICollection<OfficeTransfer>             OfficeTransfers              { get; set; }    //TODO: not used
        // public virtual ICollection<OfficeTransfer>             OfficeTransfers1             { get; set; }    //TODO: not used
        public virtual Organization                            Organization                { get; set; }
        public virtual ICollection<ParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }

        [JsonIgnore]
        public virtual ICollection<ParticipantEnrolledProgram> ParticipantEnrolledPrograms1 { get; set; } //TODO: rename to LFFeps

        public virtual ICollection<RecentParticipant>       RecentParticipants       { get; set; }
        public virtual ICollection<WorkerParticipantBridge> WorkerParticipantBridges { get; set; }
        public virtual ICollection<WorkerTaskList>          WorkerTaskLists          { get; set; }

        #endregion
    }
}

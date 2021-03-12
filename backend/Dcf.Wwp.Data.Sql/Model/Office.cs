using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Office
    {
        #region Properties

        public short?    OfficeNumber              { get; set; }
        public string    OfficeName                { get; set; }
        public short?    MFWPOfficeNumber          { get; set; }
        public short?    MFEligibilityOfficeNumber { get; set; }
        public short?    MFLocationNumber          { get; set; }
        public int?      CountyandTribeId          { get; set; }
        public int?      ContractAreaId            { get; set; }
        public DateTime? ActiviatedDate            { get; set; }
        public DateTime? InactivatedDate           { get; set; }
        public bool      IsDeleted                 { get; set; }
        public string    ModifiedBy                { get; set; }
        public DateTime? ModifiedDate              { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ContractArea   ContractArea   { get; set; }
        public virtual CountyAndTribe CountyAndTribe { get; set; }

        public virtual ICollection<ParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }

        public virtual ICollection<OfficeTransfer> OfficeTransfers { get; set; }

        public virtual ICollection<OfficeTransfer> OfficeTransfers1 { get; set; }

        public virtual ICollection<RequestForAssistance> RequestForAssistances { get; set; }

        #endregion
    }
}

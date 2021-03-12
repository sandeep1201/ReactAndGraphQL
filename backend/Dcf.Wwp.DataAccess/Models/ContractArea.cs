using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ContractArea : BaseEntity
    {
        #region Properties

        public string    Name              { get; set; }
        public int?      OrganizationId    { get; set; }
        public int?      EnrolledProgramId { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime  ModifiedDate      { get; set; }
        public DateTime? ActivatedDate     { get; set; }
        public DateTime? InActivatedDate   { get; set; }

        #endregion

        #region Methods

        public virtual Organization    Organization    { get; set; }
        public virtual EnrolledProgram EnrolledProgram { get; set; }

        #endregion
    }
}

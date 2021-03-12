using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Office : BaseEntity
    {
        #region Properties

        public short?    OfficeNumber              { get; set; }
        public string    OfficeName                { get; set; }
        public short?    MFWPOfficeNumber          { get; set; }
        public short?    MFEligibilityOfficeNumber { get; set; }
        public int?      CountyandTribeId          { get; set; }
        public int?      ContractAreaId            { get; set; }
        public short?    MFLocationNumber          { get; set; }
        public DateTime? ActiviatedDate            { get; set; }
        public DateTime? InActivatedDate           { get; set; }
        public bool      IsDeleted                 { get; set; }
        public string    ModifiedBy                { get; set; }
        public DateTime? ModifiedDate              { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ContractArea   ContractArea   { get; set; }
        public virtual CountyAndTribe CountyAndTribe { get; set; }

        #endregion
    }
}

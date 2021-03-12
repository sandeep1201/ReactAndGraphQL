using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DeleteReason
    {
        #region Properties

        public string    Name         { get; set; }
        public DateTime? CreatedDate  { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<TimeLimitExtension>         TimeLimitExtensions         { get; set; }
        public virtual ICollection<DeleteReasonByRepeater>     DeleteReasonByRepeaters     { get; set; }
        [JsonIgnore]
        public virtual ICollection<Conviction>                 Convictions                 { get; set; }
        [JsonIgnore]
        public virtual ICollection<ChildYouthSectionChild>     ChildYouthSectionChilds     { get; set; }
        [JsonIgnore]
        public virtual ICollection<BarrierAccommodation>       BarrierAccommodations       { get; set; }
        [JsonIgnore]
        public virtual ICollection<FamilyMember>               FamilyMembers               { get; set; }
        [JsonIgnore]
        public virtual ICollection<FormalAssessment>           FormalAssessments           { get; set; }
        [JsonIgnore]
        public virtual ICollection<NonCustodialCaretaker>      NonCustodialCaretakers      { get; set; }
        [JsonIgnore]
        public virtual ICollection<NonCustodialChild>          NonCustodialChilds          { get; set; }
        [JsonIgnore]
        public virtual ICollection<NonCustodialReferralChild>  NonCustodialReferralChilds  { get; set; }
        [JsonIgnore]
        public virtual ICollection<NonCustodialReferralParent> NonCustodialReferralParents { get; set; }
        [JsonIgnore]
        public virtual ICollection<EmploymentInformation>      EmploymentInformations      { get; set; }

        #endregion
    }
}

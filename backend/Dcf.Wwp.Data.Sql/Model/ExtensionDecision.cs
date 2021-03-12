using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExtensionDecision : BaseEntity
    {
        #region Properties

        public string    Name         { get; set; }
        public DateTime? CreatedDate  { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        //public virtual ICollection<TimeLimitExtension> TimeLimitExtensions { get; set; }    //TODO: relationship

        #endregion
    }
}

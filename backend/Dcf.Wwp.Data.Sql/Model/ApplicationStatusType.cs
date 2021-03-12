using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ApplicationStatusType
    {
        #region Properties

        public string ApplicationStatusName { get; set; }
        public bool?  IsRequired            { get; set; }

        public int SortOrder { get; set; }

        //public bool      IsDeleted             { get; set; } //TODO: already defined
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<FamilyBarriersSection> FamilyBarriersSections { get; set; }

        #endregion
    }
}

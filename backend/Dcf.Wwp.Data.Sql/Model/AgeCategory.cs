using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AgeCategory
    {
        #region Properties

        // this entity does not have a 'RowVersion', so it does not inherit from 'BaseEntity'.cs

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string    AgeRange        { get; set; }
        public string    DescriptionText { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ChildYouthSectionChild> ChildYouthSectionChilds { get; set; }

        #endregion
    }
}

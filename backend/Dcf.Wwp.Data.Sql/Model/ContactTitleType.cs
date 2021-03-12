using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContactTitleType
    {
        #region Properties

        public string    Name         { get; set; }
        public int?      SortOrder    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Nav Props

        public virtual ICollection<Contact> Contacts { get; set; }

        #endregion

        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}

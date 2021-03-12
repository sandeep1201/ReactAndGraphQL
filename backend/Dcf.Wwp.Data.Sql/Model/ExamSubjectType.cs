using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExamSubjectType
    {
        #region Properties

        public string    Name         { get; set; }
        public int?      ExamTypeId   { get; set; }
        public int?      SortOrder    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ExamType                           ExamType               { get; set; }
        public virtual ICollection<ExamResult>            ExamResults            { get; set; }
        public virtual ICollection<ExamSubjectTypeBridge> ExamSubjectTypeBridges { get; set; }

        #endregion
    }
}

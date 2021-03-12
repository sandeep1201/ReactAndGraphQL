using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Absence : BaseEntity
    {
        #region Properties

        public int       Id                      { get; set; }
        public int?      EmploymentInformationId { get; set; }
        public DateTime? BeginDate               { get; set; }
        public DateTime? EndDate                 { get; set; }
        public int?      AbsenceReasonId         { get; set; }
        public string    Details                 { get; set; }
        public int?      SortOrder               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }
        public bool      IsDeleted               { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}

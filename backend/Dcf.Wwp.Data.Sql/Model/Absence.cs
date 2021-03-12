using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Absence
    {
        #region Properties

        public int?      EmploymentInformationId { get; set; }
        public DateTime? BeginDate               { get; set; }
        public DateTime? EndDate                 { get; set; }
        public int?      AbsenceReasonId         { get; set; }
        public string    Details                 { get; set; }
        public int?      SortOrder               { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual AbsenceReason         AbsenceReason         { get; set; }
        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}

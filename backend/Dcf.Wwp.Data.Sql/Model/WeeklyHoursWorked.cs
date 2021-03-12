using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WeeklyHoursWorked
    {
        #region Properties

        public int      EmploymentInformationId { get; set; }
        public DateTime StartDate               { get; set; }
        public decimal  Hours                   { get; set; }
        public string   Details                 { get; set; }
        public decimal  TotalSubsidyAmount      { get; set; }
        public decimal? TotalWorkSiteAmount     { get; set; }
        public bool     IsDeleted               { get; set; }
        public string   ModifiedBy              { get; set; }
        public DateTime ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}

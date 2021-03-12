using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class WeeklyHoursWorked : BaseEntity
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

        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion

        #region ICloneable

        public object Clone()
        {
            var clone = new WeeklyHoursWorked
                        {
                            Id                      = Id,
                            EmploymentInformationId = EmploymentInformationId,
                            StartDate               = StartDate,
                            Hours                   = Hours,
                            Details                 = Details,
                            TotalSubsidyAmount      = TotalSubsidyAmount,
                            TotalWorkSiteAmount     = TotalWorkSiteAmount,
                            IsDeleted               = IsDeleted
                        };

            return clone;
        }

        #endregion ICloneable
    }
}

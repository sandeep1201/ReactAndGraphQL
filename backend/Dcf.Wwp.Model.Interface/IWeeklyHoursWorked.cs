using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWeeklyHoursWorked : ICommonModelFinal, ICloneable
    {
        int      EmploymentInformationId { get; set; }
        DateTime StartDate               { get; set; }
        decimal  Hours                   { get; set; }
        string   Details                 { get; set; }
        decimal  TotalSubsidyAmount      { get; set; }
        decimal? TotalWorkSiteAmount     { get; set; }
    }
}

using System;
using Dcf.Wwp.Api.Library.Contracts;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IWeeklyHoursWorkedDomain
    {
        #region Properties

        #endregion

        #region Methods

        IEnumerable<WeeklyHoursWorked>  GetWeeklyHoursWorkedEntries(int                        employmentInformationId);
        List<WeeklyHoursWorkedContract> GetWeeklyHoursWorkedContracts(int                      employmentInformationId);
        WeeklyHoursWorkedContract       GetWeeklyHoursWorkedEntry(int                          id);
        List<WeeklyHoursWorkedContract> UpsertWeeklyHoursWorkedEntry(WeeklyHoursWorkedContract contract);
        List<WeeklyHoursWorkedContract> DeleteWeeklyHoursWorkedEntry(int                       id);

        dynamic UpdateTotalSubsidyAmount(DateTime               modifiedDate,                   WeeklyHoursWorked     weeklyHoursWorked   = null,
                                         IEmploymentInformation employmentInfoInterface = null, EmploymentInformation employmentInfoModel = null);

        #endregion
    }
}

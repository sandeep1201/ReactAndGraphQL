using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api;

namespace Dcf.Wwp.TelerikReport.Library.Interface
{
    public interface IReportDomain
    {
        #region Properties

        #endregion

        #region Methods

        ReportContract GetWorkHistoryPdf(string       pin, List<EmploymentInfoContract> contracts);
        ReportContract GenerateBatchDetailsPdf(string pin, string                       participationPeriod, short periodYear, IEnumerable<decimal> caseNumberList);

        #endregion
    }

    public interface IEmploymentPlanDomain
    {
        ReportContract GetEmploymentPlanPdf(string pin, PrintedEmployabilityPlanContract epRContract,         PrintedEPStockTextConfig stockText);
        bool           AppendPdf(string            pin, int                              employabilityPlanId, string                   firstName, string middleInitial, string lastName, DateTime dateSigned, string resultFile);
    }
}

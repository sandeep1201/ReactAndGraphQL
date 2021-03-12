using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Telerik.Reporting;

namespace Dcf.Wwp.TelerikReport.Library
{
    public partial class BatchDetailsReport : Report
    {
        public BatchDetailsReport(IReadOnlyCollection<BatchDetailsReportContract> batchDetailsReportContracts)
        {
            InitializeComponent();

            CaseList.DataSource = batchDetailsReportContracts;

            var participationDateSorting = new Sorting
                                           {
                                               Expression = "= Fields.ParticipationDate",
                                               Direction  = SortDirection.Desc
                                           };

            var codeSorting = new Sorting
                              {
                                  Expression = "= Fields.Code",
                                  Direction  = SortDirection.Asc
                              };

            ActionsTakenList.Sortings.Add(participationDateSorting);
            ActionsTakenList.Sortings.Add(codeSorting);

            var nullFormattingRule = new Telerik.Reporting.Drawing.FormattingRule
                                     {
                                         Filters = { new Filter("= Parameters.IncludesUnAppliedHours", FilterOperator.Equal, "false") },
                                         Style   = { Visible = false }
                                     };

            UnAppliedTextValue.ConditionalFormatting.Add(nullFormattingRule);
        }
    }
}

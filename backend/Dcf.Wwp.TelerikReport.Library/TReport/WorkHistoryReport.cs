using System.Collections.Generic;
using Telerik.Reporting;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.TelerikReport.Library
{
    /// <summary>
    /// Summary description for WorkHistoryReport
    /// </summary>
    public partial class WorkHistoryReport : Report
    {
        #region Properties

        #endregion

        #region Methods

        public WorkHistoryReport(IEnumerable<EmploymentReportContract> contracts)
        {
            // Required for telerik Reporting designer support

            var nullFormattingRule = new Telerik.Reporting.Drawing.FormattingRule();

            InitializeComponent();

            EmploymentList.DataSource = contracts;

            nullFormattingRule.Filters.Add(new Filter("= Fields.ContactName", FilterOperator.Equal, ""));
            nullFormattingRule.Style.Visible = false;
            SpaceBox.ConditionalFormatting.AddRange(new[] { nullFormattingRule });

            nullFormattingRule.Filters.Add(new Filter("= Fields.ContactName", FilterOperator.Equal, ""));
            nullFormattingRule.Style.Visible = false;
            ContactLabel.ConditionalFormatting.AddRange(new [] { nullFormattingRule });

            nullFormattingRule.Filters.Add(new Filter("= Fields.ContactName", FilterOperator.Equal, ""));
            nullFormattingRule.Style.Visible = false;
            ContactNameValue.ConditionalFormatting.AddRange(new[] { nullFormattingRule });

            nullFormattingRule.Filters.Add(new Filter("= Fields.ContactName", FilterOperator.Equal, ""));
            nullFormattingRule.Style.Visible = false;
            ContactNumberValue.ConditionalFormatting.AddRange(new[] { nullFormattingRule });
        }

        #endregion
    }
}

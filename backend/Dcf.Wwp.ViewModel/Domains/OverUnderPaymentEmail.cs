using System;
using System.Collections.Generic;
using System.IO;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class OverUnderPaymentEmail : IOverUnderPaymentEmail
    {
        #region Properties

        private readonly ISmtpEmail _smtpEmail;

        public OverUnderPaymentEmail(ISmtpEmail smtpEmail)
        {
            _smtpEmail = smtpEmail;
        }

        #endregion

        #region Methods

        public void SendEmail(string dbCatalog, List<OverUnderPaymentResult> overUnderPaymentsWithoutWorker)
        {
            const string jobName = "Update Placement Web Service";

            var subject           = $"Over Payments w/o assigned Worker from {jobName}";
            var runTime           = DateTime.Now;
            var subGuid           = Guid.NewGuid().ToString().Substring(24);
            var outputPackage     = new ExcelPackage();
            var outputWorksheet   = outputPackage.Workbook.Worksheets.Add(jobName);
            var row               = 1;
            var reportHeaderValue = $"Update Placement Web Service at {runTime}";
            var reportHeader      = outputWorksheet.Cells[row, 1];

            reportHeader.Value           = reportHeaderValue;
            reportHeader.Merge           = true;
            reportHeader.Style.Font.Bold = true;
            reportHeader.Style.Font.Size = 16;
            reportHeader.Style.Font.Color.SetColor(red: 68, green: 84, blue: 106, alpha: 0);
            reportHeader.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            reportHeader.Style.Border.Bottom.Color.SetColor(red: 68, green: 114, blue: 196, alpha: 0);
            row += 1;

            overUnderPaymentsWithoutWorker.ForEach(i =>
                                                   {
                                                       var messageCell = outputWorksheet.Cells[row, 1];
                                                       var beginMonth  = i.BeginDate.ToString("MMMM");
                                                       var endMonth    = i.EndDate.ToString("MMMM");
                                                       messageCell.Value = $"For Participant {i.PinNumber} in Case {i.CaseNumber} placement change calculated an "   +
                                                                           $"{(i.RevisedPaymentAmount < 0 ? "auxiliary" : "overpayment")} for participation period {beginMonth} 16th - {endMonth} 15th " +
                                                                           $"{i.EndDate.Year} in the amount of ${Math.Abs(i.RevisedPaymentAmount)}.";

                                                       row += 1;
                                                   });

            outputWorksheet.Cells.AutoFitColumns();

            var fileName = $"{jobName}_Results_{runTime:MM-dd-yyyy-hh-mm-sstt}_{subGuid}.xlsx";
            var stream   = new MemoryStream(outputPackage.GetAsByteArray());

            _smtpEmail.SendEmail(jobName, stream, fileName, null, dbCatalog, subject,
                                 $"Report for {subject} as of {DateTime.Today:MM/dd/yyyy}.\nManual action maybe needed.\n\nThanks,\nWWP BITS Support");

            stream.Flush();
            stream.Dispose();
        }

        #endregion
    }
}

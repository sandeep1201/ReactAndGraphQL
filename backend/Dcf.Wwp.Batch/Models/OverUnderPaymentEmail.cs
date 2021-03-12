using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Dcf.Wwp.Batch.Constants;
using Dcf.Wwp.Batch.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Dcf.Wwp.Batch.Models
{
    public class OverUnderPaymentEmail : IOverUnderPaymentEmail
    {
        #region Properties

        private readonly ISmtpEmail _smtpEmail;
        private readonly string     _jobName;
        private readonly string     _outPath;
        private readonly DateTime   _runTime;
        private readonly string     _subGuid;

        public OverUnderPaymentEmail(ISmtpEmail smtpEmail, IBatchJobOptions batchOptions)
        {
            _smtpEmail = smtpEmail;
            _jobName   = batchOptions.JobName;
            _runTime   = batchOptions.RunTime;
            _subGuid   = batchOptions.SubGuid;
            _outPath   = batchOptions.OutPath;
        }

        #endregion

        #region Methods

        public void SendEmail(string dbCatalog, List<OverUnderPaymentResult> overUnderPaymentsWithoutWorker)
        {
            var outputPackage   = new ExcelPackage();
            var outputWorksheet = outputPackage.Workbook.Worksheets.Add(_jobName);
            var row             = 1;
            var subject         = string.Empty;
            var batchName       = string.Empty;
            var textInfo        = new CultureInfo("en-US", false).TextInfo;

            switch (_jobName)
            {
                case JobNames.JWWWP10:
                    batchName = "pull down";
                    subject   = $"Over Payments w/o assigned Worker from {textInfo.ToTitleCase(batchName)} Batch";
                    break;
                case JobNames.JWWWP11:
                    batchName = "weekly";
                    subject   = $"Over or Under Payments w/o assigned Worker from {textInfo.ToTitleCase(batchName)} Batch";
                    break;
                case JobNames.JWWWP12:
                    batchName = "auxiliary check trigger";
                    subject   = $"Auxiliaries w/o assigned Worker from {textInfo.ToTitleCase(batchName)} Batch";
                    break;
            }

            var reportHeader = outputWorksheet.Cells[row, 1];
            reportHeader.Value           = $"{textInfo.ToTitleCase(batchName)} Batch as of {DateTime.Today:D}";
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

                                                       var overOrUnderMsg = _jobName == JobNames.JWWWP10
                                                                                ? "auxiliary"
                                                                                : i.RevisedPaymentAmount > 0
                                                                                    ? "overpayment"
                                                                                    : "auxiliary";

                                                       messageCell.Value = $"For Participant {i.PinNumber} in Case {i.CaseNumber} {batchName} batch calculated an " +
                                                                           $"{overOrUnderMsg} for participation period {beginMonth} 16th - {endMonth} 15th "        +
                                                                           $"{i.EndDate.Year} in the amount of {Math.Abs(i.RevisedPaymentAmount)}.";

                                                       row += 1;
                                                   });

            outputWorksheet.Cells.AutoFitColumns();

            var fileName = $"{_jobName}_Results_{_runTime:MM-dd-yyyy-hh-mm-sstt}_{_subGuid}.xlsx";
            var fullName = $"{AppDomain.CurrentDomain.BaseDirectory}{_outPath}{Path.DirectorySeparatorChar}{_jobName}{Path.DirectorySeparatorChar}{fileName}";
            var dirName  = Path.GetDirectoryName(fullName);
            if (dirName != null && !Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
            var stream = new FileInfo(fullName).Create();

            outputPackage.SaveAs(stream);
            _smtpEmail.SendEmail(_jobName, stream, fileName, null, dbCatalog, subject,
                                 $"Report for {subject} as of {DateTime.Today:MM/dd/yyyy}.\nManual action maybe needed.\n\nThanks,\nWWP BITS Support");
            stream.Flush();
        }

        #endregion
    }
}

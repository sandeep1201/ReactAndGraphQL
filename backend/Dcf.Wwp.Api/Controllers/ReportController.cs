using System.Collections.Generic;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.TelerikReport.Library.Interface;
using System;
using System.IO;
using System.Linq;
using Dcf.Wwp.Api.Library.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/reports")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class ReportController : BaseController
    {
        #region Properties

        private readonly IReportDomain         _reportDomain;
        private readonly IEmploymentPlanDomain _employmentPlanDomain;
        private readonly IFileUploadDomain     _fileUploadDomain;

        #endregion

        #region MyRegion

        public ReportController(IRepository           repository,
                                IReportDomain         reportDomain,
                                IEmploymentPlanDomain employmentPlanDomain,
                                IFileUploadDomain     fileUploadDomain) : base(repository)
        {
            _reportDomain         = reportDomain;
            _employmentPlanDomain = employmentPlanDomain;
            _fileUploadDomain     = fileUploadDomain;
        }

        [HttpPost("{pin}/work-history")]
        public IActionResult GenerateWorkHistoryPdf(string pin, [FromBody] List<EmploymentInfoContract> contracts)
        {
            var contract = _reportDomain.GetWorkHistoryPdf(pin, contracts);
            var result = new FileContentResult(contract.FileStream, contract.MimeType)
                         {
                             FileDownloadName = $"WorkHistory_{pin}.pdf"
                         };

            return result;
        }

        [HttpPost("{pin}/ep")]
        public IActionResult GenerateEPPdf(string pin, [FromBody] PrintedEmployabilityPlanContract model)
        {
            var cb = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddEnvironmentVariables()
                     .AddJsonFile("EmployabilityPlanStockText.json");
            var cbb = cb.Build();

            var stockText = cbb.Get<PrintedEPStockTextConfig>();

            var contract = _employmentPlanDomain.GetEmploymentPlanPdf(pin, model, stockText);
            var result = new FileContentResult(contract.FileStream, contract.MimeType)
                         {
                             FileDownloadName = $"EmployabilityPlan_{pin}.pdf"
                         };

            return result;
        }

        [HttpGet("{pin}/batch-details/{participationPeriod}/{periodYear}/{caseNumbers}")]
        public IActionResult GenerateBatchDetailsPdf(string pin, string participationPeriod, short periodYear, string caseNumbers)
        {
            var caseNumberList = caseNumbers.Split(',').Select(decimal.Parse);
            var contract       = _reportDomain.GenerateBatchDetailsPdf(pin, participationPeriod, periodYear, caseNumberList);
            var result = new FileContentResult(contract.FileStream, contract.MimeType)
                         {
                             FileDownloadName = $"BatchDetails_{pin}.pdf"
                         };

            return result;
        }

        [HttpGet("retrieve/{docId}")]
        public IActionResult RetrieveDocument(string docId)
        {
            var workStream = new MemoryStream();
            var contract   = _fileUploadDomain.RetrieveDocument(docId);
            workStream.Write(contract, 0, contract.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.TelerikReport.Library.Interface;
using Telerik.Reporting.Processing;

namespace Dcf.Wwp.TelerikReport.Library.Domain
{
    public partial class ReportDomain : IReportDomain
    {
        #region Properties

        private readonly IParticipantRepository _participantRepo;
        private readonly IContactRepository     _contactRepo;

        #endregion

        #region Methods

        public ReportDomain(IParticipantRepository                     participantRepo,
                            IContactRepository                         contactRepo,
                            IAfterPullDownWeeklyBatchDetailsRepository afterPullDownWeeklyBatchDetailsRepository,
                            IParticipationEntryHistoryRepository       participationEntryHistoryRepository,
                            IParticipationEntryRepository              participationEntryRepository,
                            IParticipantPlacementRepository            participantPlacementRepository,
                            IPullDownDateRepository                    pullDownDateRepository)
        {
            _participantRepo                           = participantRepo;
            _contactRepo                               = contactRepo;
            _pullDownDateRepository                    = pullDownDateRepository;
            _participantPlacementRepository            = participantPlacementRepository;
            _participationEntryRepository              = participationEntryRepository;
            _participationEntryHistoryRepository       = participationEntryHistoryRepository;
            _afterPullDownWeeklyBatchDetailsRepository = afterPullDownWeeklyBatchDetailsRepository;
        }

        public ReportContract GetWorkHistoryPdf(string pin, List<EmploymentInfoContract> contracts)
        {
            var decPin          = decimal.Parse(pin);
            var participant     = _participantRepo.Get(i => i.PinNumber == decPin);
            var reportContracts = new List<EmploymentReportContract>();
            contracts.ForEach(c =>
                              {
                                  var address       = !string.IsNullOrEmpty(c.StreetAddress) ? $"{c.StreetAddress}, {c.Location.Description} {c.ZipAddress}" : c.Location.Description;
                                  var jobEndDate    = c.JobEndDate ?? "Currently Employed";
                                  var contact       = _contactRepo.Get(i => i.Id == c.ContactId && i.IsDeleted == false);
                                  var contactName   = contact?.Name;
                                  var contactNumber = contact?.Phone;
                                  var extension     = contact?.ExtensionNo;
                                  contactNumber = !string.IsNullOrEmpty(contactNumber) && contactNumber.Length == 10
                                                      ? $"{decimal.Parse(contactNumber):(###) ###-####}"
                                                      : contactNumber;
                                  contactNumber = !string.IsNullOrEmpty(contactNumber) && !string.IsNullOrEmpty(extension) ? $"{contactNumber} x{extension}" : contactNumber;
                                  var    wageHourHistory = c.WageHour?.WageHourHistories.OrderBy(i => DateTime.Parse(i.EffectiveDate)).FirstOrDefault();
                                  string beginPayRate;
                                  string endPayRate;

                                  if (c.WageHour != null)
                                  {
                                      if (wageHourHistory != null)
                                          beginPayRate = $"${wageHourHistory.PayRate} per {wageHourHistory.PayRateIntervalName}";
                                      else
                                          beginPayRate = c.WageHour.PastBeginPayRate != null
                                                             ? $"${c.WageHour.PastBeginPayRate} per {c.WageHour.PastBeginPayRateIntervalName}"
                                                             : $"${c.WageHour.CurrentPayRate} per {c.WageHour.CurrentPayRateIntervalName}";

                                      if (c.WageHour.CurrentPayRate != null)
                                      {
                                          endPayRate = $"${c.WageHour.CurrentPayRate} per {c.WageHour.CurrentPayRateIntervalName}";
                                      }
                                      else
                                          endPayRate = c.WageHour.PastEndPayRate != null
                                                           ? $"${c.WageHour.PastEndPayRate} per {c.WageHour.PastEndRateIntervalName}"
                                                           : $"${c.WageHour.PastBeginPayRate} per {c.WageHour.PastBeginPayRateIntervalName}";
                                  }
                                  else
                                  {
                                      beginPayRate = "No Pay";
                                      endPayRate   = "No Pay";
                                  }

                                  beginPayRate = !string.IsNullOrWhiteSpace(beginPayRate) && beginPayRate.Trim() != "$ per" ? beginPayRate : "No Pay";
                                  endPayRate   = !string.IsNullOrWhiteSpace(endPayRate)   && endPayRate.Trim()   != "$ per" ? endPayRate : "No Pay";

                                  reportContracts.Add(new EmploymentReportContract
                                                      {
                                                          JobTypeName   = c.JobTypeName,
                                                          CompanyName   = c.CompanyName,
                                                          Address       = address,
                                                          ContactName   = contactName ?? "",
                                                          ContactNumber = contactNumber,
                                                          JobBeginDate  = c.JobBeginDate,
                                                          JobEndDate    = jobEndDate,
                                                          BeginPayRate  = beginPayRate,
                                                          EndPayRate    = endPayRate,
                                                          Hours         = c.WageHour?.CurrentAverageWeeklyHours,
                                                          JobPosition   = c.JobPosition,
                                                          JobDuties     = $"{string.Join("\n", c.JobDuties.Select(i => i.Details))}"
                                                      });
                              });

            var sortedContract = reportContracts.OrderByDescending(i => DateTime.Parse(i.JobBeginDate))
                                                .ThenByDescending(i => i.JobEndDate == null)
                                                .ToList();
            var report               = new WorkHistoryReport(sortedContract);
            var instanceReportSource = new Telerik.Reporting.InstanceReportSource { ReportDocument = report };

            report.ReportParameters["Participant"].Value = $"{participant.FirstName.SafeTrim().ToUpper()} {participant.LastName.SafeTrim().ToUpper()}";

            var reportProcessor = new ReportProcessor();
            var renderingResult = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            var ms = new MemoryStream();
            ms.Write(renderingResult.DocumentBytes, 0, renderingResult.DocumentBytes.Length);
            ms.Flush();

            var contract = new ReportContract
                           {
                               FileStream = ms.ToArray(),
                               MimeType   = renderingResult.MimeType
                           };
            return contract;
        }

        #endregion
    }
}

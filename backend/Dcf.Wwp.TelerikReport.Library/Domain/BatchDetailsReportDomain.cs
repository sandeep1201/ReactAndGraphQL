// ReSharper disable InterpolatedStringExpressionIsNotIFormattable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.TelerikReport.Library.Interface;
using Telerik.Reporting.Processing;

namespace Dcf.Wwp.TelerikReport.Library.Domain
{
    public partial class ReportDomain
    {
        #region Properties

        private readonly IAfterPullDownWeeklyBatchDetailsRepository _afterPullDownWeeklyBatchDetailsRepository;
        private readonly IParticipationEntryHistoryRepository       _participationEntryHistoryRepository;
        private readonly IParticipationEntryRepository              _participationEntryRepository;
        private readonly IParticipantPlacementRepository            _participantPlacementRepository;
        private readonly IPullDownDateRepository                    _pullDownDateRepository;

        #endregion

        #region Methods

        public ReportContract GenerateBatchDetailsPdf(string pin, string participationPeriod, short periodYear, IEnumerable<decimal> caseNumberList)
        {
            var splitPeriod        = participationPeriod.SplitStringToDate(periodYear);
            var beginDate          = DateTime.Parse(splitPeriod[0]);
            var endDate            = DateTime.Parse(splitPeriod[1]);
            var decimalPin         = decimal.Parse(pin);
            var reportDate         = DateTime.Now;
            var pullDownDate       = _pullDownDateRepository.Get(i => i.BenefitMonth == endDate.Month && i.BenefitYear == endDate.Year).PullDownDates;
            var participant        = _participantRepo.Get(i => i.PinNumber == decimalPin);
            var placementInPeriod  = _participantPlacementRepository.GetMany(i => caseNumberList.Contains(i.CaseNumber)            && i.PlacementStartDate     <= endDate   && i.CreatedDate          > pullDownDate && !i.IsDeleted).ToList();
            var batchDetails       = _afterPullDownWeeklyBatchDetailsRepository.GetMany(i => caseNumberList.Contains(i.CaseNumber) && i.ParticipationBeginDate >= beginDate && i.ParticipationEndDate <= endDate     && !i.IsDeleted).ToList();
            var entryHistories     = _participationEntryHistoryRepository.GetMany(i => caseNumberList.Contains(i.CaseNumber ?? 0)  && i.ParticipationDate      >= beginDate && i.ParticipationDate    <= endDate     && i.IsProcessed  && !i.IsDeleted).ToList();
            var unProcessedEntries = _participationEntryRepository.GetMany(i => caseNumberList.Contains(i.CaseNumber        ?? 0)  && i.ParticipationDate      >= beginDate && i.ParticipationDate    <= endDate     && !i.IsProcessed && !i.IsDeleted && i.ProcessedDate != null).ToList();
            var firstBatchDetail   = batchDetails.OrderByDescending(j => j.WeeklyBatchDate).FirstOrDefault();

            batchDetails.AddRange(unProcessedEntries.Select(i => new AfterPullDownWeeklyBatchDetails
                                                                 {
                                                                     ParticipantId          = i.ParticipantId,
                                                                     CaseNumber             = i.CaseNumber.GetValueOrDefault(),
                                                                     ParticipationBeginDate = beginDate,
                                                                     ParticipationEndDate   = endDate,
                                                                     WeeklyBatchDate        = DateTime.MinValue,
                                                                     PreviousNPHours        = firstBatchDetail?.CurrentNPHours ?? 0,
                                                                     PreviousGCHours        = firstBatchDetail?.CurrentGCHours ?? 0,
                                                                     CurrentNPHours         = i.NonParticipatedHours.GetValueOrDefault(),
                                                                     CurrentGCHours         = i.GoodCausedHours.GetValueOrDefault(),
                                                                     PreviousUnAppliedHours = 0,
                                                                     CurrentUnAppliedHours  = 0,
                                                                     Calculation            = 0,
                                                                     OverPaymentOrAux       = "-"
                                                                 }).ToList());

            var batchDetailsReportContracts = batchDetails.GroupBy(i => i.CaseNumber)
                                                          .Select(caseNumber =>
                                                                  {
                                                                      var batchDetailsContract          = new List<BatchDetails>();
                                                                      var previousBatchDate             = pullDownDate;
                                                                      var index                         = 1;
                                                                      var previousNPHoursForUnProcessed = 0.0m;
                                                                      var previousGCHoursForUnProcessed = 0.0m;

                                                                      caseNumber.GroupBy(i => i.WeeklyBatchDate)
                                                                                .ToList()
                                                                                .ForEach(processed =>
                                                                                         {
                                                                                             var isUnProcessedEntry             = processed.Key == DateTime.MinValue;
                                                                                             var processedDate                  = isUnProcessedEntry ? DateTime.Today : processed.Key;
                                                                                             var previousProcessed              = previousBatchDate;
                                                                                             var currentProcessedEntries        = entryHistories.Where(i => i.ProcessedDate == processedDate && i.CaseNumber == caseNumber.Key).ToList();
                                                                                             var previousProcessedEntries       = entryHistories.Where(i => i.ProcessedDate < processedDate  && i.CaseNumber == caseNumber.Key).GroupBy(i => i.Id).Select(i => i.OrderByDescending(j => j.ProcessedDate).First()).ToList();
                                                                                             var previousNPHours                = isUnProcessedEntry ? previousNPHoursForUnProcessed : processed.Sum(i => i.PreviousNPHours);
                                                                                             var previousGCHours                = isUnProcessedEntry ? previousGCHoursForUnProcessed : processed.Sum(i => i.PreviousGCHours);
                                                                                             var currentNPHours                 = processed.Sum(i => i.CurrentNPHours);
                                                                                             var currentGCHours                 = processed.Sum(i => i.CurrentGCHours);
                                                                                             var calculations                   = processed.First().Calculation;
                                                                                             var includesPreviousUnAppliedHours = processed.Sum(i => i.PreviousUnAppliedHours) > 0;
                                                                                             var includesCurrentUnAppliedHours  = processed.Sum(i => i.CurrentUnAppliedHours)  > 0;
                                                                                             var placementInBatchPeriod = placementInPeriod.Where(i => i.CreatedDate.Date > previousProcessed.Date &&
                                                                                                                                                       i.CreatedDate.Date <= (processed.Key.Date == DateTime.MinValue ? DateTime.MaxValue : processed.Key.Date))
                                                                                                                                           .ToList();
                                                                                             var batchDetailContract = new BatchDetails
                                                                                                                       {
                                                                                                                           Index                  = index,
                                                                                                                           WeeklyBatchDate        = isUnProcessedEntry ? "-" : processed.Key.ToString("d"),
                                                                                                                           PreviousNPHours        = $"{previousNPHours - previousGCHours}{(includesPreviousUnAppliedHours ? "*" : string.Empty)}",
                                                                                                                           ActionsTaken           = new List<ActionsTaken>(),
                                                                                                                           CurrentNPHours         = isUnProcessedEntry ? "-" : $"{currentNPHours - currentGCHours}{(includesCurrentUnAppliedHours ? "*" : string.Empty)}",
                                                                                                                           Calculation            = calculations == 0 ? "-" : $"$ {calculations:N2}",
                                                                                                                           OpOrAux                = processed.FirstOrDefault()?.OverPaymentOrAux,
                                                                                                                           IncludesUnAppliedHours = includesPreviousUnAppliedHours || includesCurrentUnAppliedHours
                                                                                                                       };

                                                                                             if (isUnProcessedEntry)
                                                                                                 unProcessedEntries.GroupBy(i => i.Activity.ActivityType)
                                                                                                                   .ToList()
                                                                                                                   .ForEach(activityType => activityType.GroupBy(i => i.ParticipationDate.ToString("d"))
                                                                                                                                                        .ToList()
                                                                                                                                                        .ForEach(participationDate =>
                                                                                                                                                                 {
                                                                                                                                                                     var currentProcessedNPHours         = participationDate.Where(i => i.HoursSanctionable == true).Sum(i => i.NonParticipatedHours);
                                                                                                                                                                     var previousProcessedNPHours        = previousProcessedEntries.Where(i => i.Activity.ActivityType.Code == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear() && i.HoursSanctionable == true).Sum(i => i.NonParticipatedHours);
                                                                                                                                                                     var currentProcessedGCHours         = participationDate.Where(i => i.HoursSanctionable == true).Sum(i => i.GoodCausedHours);
                                                                                                                                                                     var previousProcessedGCHours        = previousProcessedEntries.Where(i => i.Activity.ActivityType.Code == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear() && i.HoursSanctionable == true).Sum(i => i.GoodCausedHours);
                                                                                                                                                                     var isCurrentProcessedSanctionable  = currentProcessedEntries.Where(i => i.Activity.ActivityType.Code  == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear()).Any(i => i.HoursSanctionable == true);
                                                                                                                                                                     var isPreviousProcessedSanctionable = previousProcessedEntries.Where(i => i.Activity.ActivityType.Code == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear()).Any(i => i.HoursSanctionable == true);

                                                                                                                                                                     if (currentProcessedNPHours != previousProcessedNPHours)
                                                                                                                                                                         batchDetailContract.ActionsTaken
                                                                                                                                                                                            .Add(new ActionsTaken
                                                                                                                                                                                            {
                                                                                                                                                                                                ActionTaken = NPMessage(currentProcessedNPHours, previousProcessedNPHours, activityType.Key.Code, participationDate.Key),
                                                                                                                                                                                                ParticipationDate = participationDate.Key.ToDateMonthDayYear(),
                                                                                                                                                                                                Code = activityType.Key.Code
                                                                                                                                                                                            });

                                                                                                                                                                     if (currentProcessedGCHours != previousProcessedGCHours)
                                                                                                                                                                         batchDetailContract.ActionsTaken
                                                                                                                                                                                            .Add(new ActionsTaken
                                                                                                                                                                                            {
                                                                                                                                                                                                ActionTaken = GCMessage(currentProcessedGCHours, previousProcessedGCHours, activityType.Key.Code, participationDate.Key),
                                                                                                                                                                                                ParticipationDate = participationDate.Key.ToDateMonthDayYear(),
                                                                                                                                                                                                Code = activityType.Key.Code
                                                                                                                                                                                            });

                                                                                                                                                                     if (currentProcessedNPHours == previousProcessedNPHours && currentProcessedGCHours == previousProcessedGCHours && (isCurrentProcessedSanctionable || isPreviousProcessedSanctionable))
                                                                                                                                                                         batchDetailContract.ActionsTaken
                                                                                                                                                                                            .Add(new ActionsTaken
                                                                                                                                                                                            {
                                                                                                                                                                                                ActionTaken = NAMessage(activityType.Key.Code, participationDate.Key),
                                                                                                                                                                                                ParticipationDate = participationDate.Key.ToDateMonthDayYear(),
                                                                                                                                                                                                Code = activityType.Key.Code
                                                                                                                                                                                            });
                                                                                                                                                                 }));
                                                                                             else
                                                                                                 currentProcessedEntries.OrderByDescending(i => i.ParticipationDate)
                                                                                                                        .GroupBy(i => i.Activity.ActivityType)
                                                                                                                        .ToList()
                                                                                                                        .ForEach(activityType => activityType.GroupBy(i => i.ParticipationDate.ToString("d"))
                                                                                                                                                             .ToList()
                                                                                                                                                             .ForEach(participationDate =>
                                                                                                                                                                      {
                                                                                                                                                                          var currentProcessedNPHours         = participationDate.Where(i => i.HoursSanctionable == true).Sum(i => i.NonParticipatedHours);
                                                                                                                                                                          var previousProcessedNPHours        = previousProcessedEntries.Where(i => i.Activity.ActivityType.Code == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear() && i.HoursSanctionable == true).Sum(i => i.NonParticipatedHours);
                                                                                                                                                                          var currentProcessedGCHours         = participationDate.Where(i => i.HoursSanctionable == true).Sum(i => i.GoodCausedHours);
                                                                                                                                                                          var previousProcessedGCHours        = previousProcessedEntries.Where(i => i.Activity.ActivityType.Code == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear() && i.HoursSanctionable == true).Sum(i => i.GoodCausedHours);
                                                                                                                                                                          var isCurrentProcessedSanctionable  = currentProcessedEntries.Where(i => i.Activity.ActivityType.Code  == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear()).Any(i => i.HoursSanctionable == true);
                                                                                                                                                                          var isPreviousProcessedSanctionable = previousProcessedEntries.Where(i => i.Activity.ActivityType.Code == activityType.Key.Code && i.ParticipationDate == participationDate.Key.ToDateMonthDayYear()).Any(i => i.HoursSanctionable == true);

                                                                                                                                                                          if (currentProcessedNPHours != previousProcessedNPHours)
                                                                                                                                                                              batchDetailContract.ActionsTaken
                                                                                                                                                                                                 .Add(new ActionsTaken
                                                                                                                                                                                                      {
                                                                                                                                                                                                          ActionTaken       = NPMessage(currentProcessedNPHours, previousProcessedNPHours, activityType.Key.Code, participationDate.Key),
                                                                                                                                                                                                          ParticipationDate = participationDate.Key.ToDateMonthDayYear(),
                                                                                                                                                                                                          Code              = activityType.Key.Code
                                                                                                                                                                                                      });

                                                                                                                                                                          if (currentProcessedGCHours != previousProcessedGCHours)
                                                                                                                                                                              batchDetailContract.ActionsTaken
                                                                                                                                                                                                 .Add(new ActionsTaken
                                                                                                                                                                                                      {
                                                                                                                                                                                                          ActionTaken       = GCMessage(currentProcessedGCHours, previousProcessedGCHours, activityType.Key.Code, participationDate.Key),
                                                                                                                                                                                                          ParticipationDate = participationDate.Key.ToDateMonthDayYear(),
                                                                                                                                                                                                          Code              = activityType.Key.Code
                                                                                                                                                                                                      });

                                                                                                                                                                          if (currentProcessedNPHours == previousProcessedNPHours && currentProcessedGCHours == previousProcessedGCHours && (isCurrentProcessedSanctionable || isPreviousProcessedSanctionable))
                                                                                                                                                                              batchDetailContract.ActionsTaken
                                                                                                                                                                                                 .Add(new ActionsTaken
                                                                                                                                                                                                      {
                                                                                                                                                                                                          ActionTaken       = NAMessage(activityType.Key.Code, participationDate.Key),
                                                                                                                                                                                                          ParticipationDate = participationDate.Key.ToDateMonthDayYear(),
                                                                                                                                                                                                          Code              = activityType.Key.Code
                                                                                                                                                                                                      });
                                                                                                                                                                      }));

                                                                                             placementInBatchPeriod.ForEach(i => batchDetailContract.ActionsTaken
                                                                                                                                                    .Add(new ActionsTaken
                                                                                                                                                         {
                                                                                                                                                             ActionTaken       = $"{i.PlacementType.Code} placement start date backdated to {i.PlacementStartDate:d}",
                                                                                                                                                             ParticipationDate = i.PlacementStartDate.GetValueOrDefault(),
                                                                                                                                                             Code              = i.PlacementType.Code
                                                                                                                                                         }));

                                                                                             batchDetailsContract.Add(batchDetailContract);
                                                                                             previousBatchDate             =  processed.Key;
                                                                                             previousNPHoursForUnProcessed =  currentNPHours;
                                                                                             previousGCHoursForUnProcessed =  currentGCHours;
                                                                                             index                         += 1;
                                                                                         });

                                                                      return new BatchDetailsReportContract
                                                                             {
                                                                                 CaseNumber   = caseNumber.Key,
                                                                                 BatchDetails = batchDetailsContract.OrderBy(i => i.WeeklyBatchDate).ToList()
                                                                             };
                                                                  }).ToList();

            batchDetailsReportContracts.ForEach(i => i.BatchDetails = i.BatchDetails.OrderBy(j => j.Index).ToList());

            var report               = new BatchDetailsReport(batchDetailsReportContracts);
            var instanceReportSource = new Telerik.Reporting.InstanceReportSource { ReportDocument = report };

            report.ReportParameters["PinNumber"].Value              = pin;
            report.ReportParameters["Participant"].Value            = participant.DisplayName;
            report.ReportParameters["ParticipationPeriod"].Value    = $"{participationPeriod} {periodYear}";
            report.ReportParameters["ReportRunTimeStamp"].Value     = $"{reportDate.ToString("hh:mm tt").ToLower()} on {reportDate:d}";
            report.ReportParameters["IncludesUnAppliedHours"].Value = batchDetailsReportContracts.Any(i => i.BatchDetails.Any(j => j.IncludesUnAppliedHours)) ? "true" : "false";

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

        private string NPMessage(decimal? currentProcessedNPHours, decimal? previousProcessedNPHours, string activityCode, string participationDate)
        {
            var nPHoursDifference = currentProcessedNPHours.GetValueOrDefault() - previousProcessedNPHours.GetValueOrDefault();
            return $"{Math.Abs(nPHoursDifference)} hour(s) of NP {(nPHoursDifference < 0 ? "deducted" : "added")} for {activityCode} on {participationDate:d}";
        }

        private string GCMessage(decimal? currentProcessedGCHours, decimal? previousProcessedGCHours, string activityCode, string participationDate)
        {
            var gCHoursDifference = currentProcessedGCHours.GetValueOrDefault() - previousProcessedGCHours.GetValueOrDefault();
            return $"{Math.Abs(gCHoursDifference)} hour(s) of GC {(gCHoursDifference < 0 ? "removed" : "added")} for {activityCode} on {participationDate:d}";
        }

        private string NAMessage(string activityCode, string participationDate)
        {
            return "No action taken for " +
                   $"{activityCode} on {participationDate:d}";
        }

        #endregion
    }
}

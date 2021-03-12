using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Dcf.Wwp.Api;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.TelerikReport.Library.Interface;
using Dcf.Wwp.TelerikReport.Library.TReport;
using Telerik.Reporting.Processing;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;
using NonSelfDirectedActivity = Dcf.Wwp.Api.Library.Contracts.NonSelfDirectedActivity;

namespace Dcf.Wwp.TelerikReport.Library.Domain
{
    public class EmploymentPlanDomain : IEmploymentPlanDomain
    {
        #region Properties

        private readonly IEmployabilityPlanRepository       _epRepo;
        private readonly IContactRepository                 _contactRepo;
        private readonly INonSelfDirectedActivityRepository _nsdRepo;
        private readonly ILocations                         _locations;
        private readonly IFileUploadDomain                  _fileUploadDomain;
        private readonly IDocumentRepository                _documentRepository;

        #endregion

        #region Methods

        public EmploymentPlanDomain(IEmployabilityPlanRepository       epRepo,
                                    IContactRepository                 contactRepo,
                                    INonSelfDirectedActivityRepository nsdRepo,
                                    ILocations                         locations,
                                    IFileUploadDomain                  fileUploadDomain,
                                    IDocumentRepository                documentRepository)
        {
            _epRepo             = epRepo;
            _contactRepo        = contactRepo;
            _nsdRepo            = nsdRepo;
            _locations          = locations;
            _fileUploadDomain   = fileUploadDomain;
            _documentRepository = documentRepository;
        }

        public ReportContract GetEmploymentPlanPdf(string pin, PrintedEmployabilityPlanContract epRContract, PrintedEPStockTextConfig stockText)
        {
            //If this is Learnfare participant, display LF worker information, not FEP
            var mfUserId = epRContract.EmployabilityPlan.EnrolledProgramCd?.Trim() == EnrolledProgram.LFProgramCode
                               ? epRContract.Participant.Programs.FirstOrDefault(i => i.Id == epRContract.EmployabilityPlan.PEPId)?.LearnFareFEP.WorkerId
                               : epRContract.Participant.Programs.FirstOrDefault(i => i.Id == epRContract.EmployabilityPlan.PEPId)?.AssignedWorker.WorkerId;
            var wiuid = epRContract.EmployabilityPlan?.EnrolledProgramCd?.Trim() == EnrolledProgram.LFProgramCode
                            ? epRContract.Participant.Programs.FirstOrDefault(i => i.Id == epRContract.EmployabilityPlan.PEPId)?.LearnFareFEP.Wiuid
                            : epRContract.Participant.Programs.FirstOrDefault(i => i.Id == epRContract.EmployabilityPlan.PEPId)?.AssignedWorker.Wiuid;

            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]         = pin ?? (object) DBNull.Value,
                            ["ParticipantId"]     = epRContract.Participant.Id,
                            ["TargetDate"]        = epRContract.EmployabilityPlan.BeginDate.ToStringMonthDayYear(),
                            ["MFUserId"]          = mfUserId ?? (object) DBNull.Value,
                            ["WIUID"]             = wiuid    ?? (object) DBNull.Value,
                            ["EnrolledProgramId"] = epRContract.EmployabilityPlan.EnrolledProgramId,
                            ["OrganizationId"]    = epRContract.EmployabilityPlan.OrganizationId ?? (object) DBNull.Value
                        };

            var rs          = _epRepo.ExecStoredProc<USP_PrintEP_Info_Result>("USP_PrintEP_Info", parms).FirstOrDefault();
            var contactList = new List<ContactReportContract>();
            epRContract.Activities.ForEach(a =>
                                           {
                                               a.Contacts.ForEach(con =>
                                                                  {
                                                                      var contact = _contactRepo.GetAsQueryable()
                                                                                                .Where(i => i.Id == con)
                                                                                                .Select(c => new ContactReportContract
                                                                                                             {
                                                                                                                 ActivityId           = a.Id,
                                                                                                                 ContactTitleTypeName = !c.Title.Name.Equals("Other") ? c.Title.Name : "Other - " + c.CustomTitle,
                                                                                                                 ContactName          = c.Name        ?? "",
                                                                                                                 ContactAddress       = c.Address     ?? "",
                                                                                                                 ContactPhone         = c.Phone       ?? "",
                                                                                                                 ContactExt           = c.ExtensionNo ?? "",
                                                                                                                 ContactFax           = c.FaxNo       ?? "",
                                                                                                                 ContactEmail         = c.Email       ?? ""
                                                                                                             }).FirstOrDefault();
                                                                      if ( contact != null && contact.ContactPhone.Length > 0)
                                                                      {
                                                                          var phoneNum  = new string(contact.ContactPhone.Replace(" ", "").Where(char.IsDigit).ToArray());
                                                                          var phoneNumD = Convert.ToDouble(phoneNum);
                                                                          contact.ContactPhone = phoneNum.Length == 7 ? $"{phoneNumD:###-####}" : contact.ContactPhone;
                                                                          contact.ContactPhone = phoneNum.Length == 10 ? $"{phoneNumD:(###) ###-####}" : contact.ContactPhone;
                                                                          contact.ContactPhone = (!string.IsNullOrWhiteSpace(contact.ContactExt) ? $"{contact.ContactPhone} x{contact.ContactExt}" : contact.ContactPhone);
                                                                      }

                                                                      contactList.Add(contact);
                                                                  });
                                               //Query for the Phone number
                                               var isSelfDirected = a.ActivityLocationName == "Self-directed";
                                               if (!isSelfDirected)
                                               {
                                                   //if Self Directed Activity is found
                                                   var sda = _nsdRepo.Get(i => i.ActivityId == a.Id);
                                                   if (sda != null)
                                                   {
                                                       var sdaLocContract = _locations.GetLocationInfo(sda, sda.City);

                                                       a.NonSelfDirectedActivity = new NonSelfDirectedActivity
                                                                                   {
                                                                                       BusinessLocation    = sdaLocContract,
                                                                                       BusinessName        = sda.BusinessName,
                                                                                       BusinessPhoneNumber = sda.PhoneNumber,
                                                                                       BusinessZipAddress  = sda.ZipAddress
                                                                                   };
                                                   }
                                               }
                                           });

            //Schedule Events info from assigned activities
            var prms = new Dictionary<string, object>
                       {
                           ["EPId"]           = epRContract.EmployabilityPlan.Id,
                           ["SubsequentEPId"] = DBNull.Value,
                           ["ProgramCd"]      = epRContract.EmployabilityPlan.EnrolledProgramCd?.ToLower().SafeTrim(),
                           ["Consider15th"]   = false
                       };
            var EPschedule = _epRepo.ExecStoredProc<USP_Create_Events>("USP_Create_Events", prms);


            //Finding Current Enrolled Program
            EnrolledProgramContract enrolledProgramContract = null;
            rs.HomeLanguage = rs.HomeLanguage ?? "E";

            epRContract.Participant.Programs.ForEach(c =>
                                                     {
                                                         if ( c.EnrolledProgramId == epRContract.EmployabilityPlan.EnrolledProgramId)
                                                         {
                                                             enrolledProgramContract = c;
                                                         }
                                                     });

            //Goals
            var goalsContract = new List<GoalReportContract>();
            epRContract.Goals.ForEach(c =>
                                      {
                                          var goalSteps = new List<GoalStepReportContract>();
                                          if (c.GoalSteps != null)
                                              for (var i = 1; i <= c.GoalSteps.Count; i++)
                                                  goalSteps.Add(new GoalStepReportContract
                                                                {
                                                                    GoalStepDetails             = i + ".  " + c.GoalSteps[i - 1].Details,
                                                                    GoalStepIsGoalStepCompleted = c.GoalSteps[i             - 1].IsGoalStepCompleted ?? false,
                                                                });

                                          goalsContract.Add(new GoalReportContract
                                                            {
                                                                GoalTypeName = c.GoalTypeName,
                                                                GoalName     = c.Name,
                                                                GoalDetails  = c.Details,
                                                                GoalBegin    = c.BeginDate,
                                                                HasGoalStep  = goalSteps.Count != 0,
                                                                GoalStep     = goalSteps
                                                            });
                                      });

            //Employment
            var employmentContract = new List<EmploymentInfoReportContract>();
            epRContract.EmploymentInfo.ForEach(c =>
                                               {
                                                   var location = new [] { c.Location?.FullAddress, c.Location?.Description };
                                                   var addr     = string.Join(", ", location.Where(i => !string.IsNullOrWhiteSpace(i)));
                                                   employmentContract.Add(new EmploymentInfoReportContract
                                                                          {
                                                                              EmploymentJobTypeName               = c.JobTypeName,
                                                                              EmploymentJobPosition               = c.JobPosition,
                                                                              EmploymentCompanyName               = c.CompanyName,
                                                                              EmploymentLocation                  = $"{addr} {c.Location?.ZipAddress}".SafeTrim(),
                                                                              EmploymentJobBeginDate              = c.JobBeginDate,
                                                                              EmploymentCurrentAverageWeeklyHours = c.WageHour.ToString(),
                                                                              EmploymentPosition                  = c.JobPosition
                                                                          });
                                               });

            //Assigned Activities
            var activityContract = new List<ActivityReportContract>();
            epRContract.Activities.ForEach(c =>
                                           {
                                               var activitySchedulesContract = new List<ActivityScheduleReportContract>();
                                               for (var i = 0; i < c.ActivitySchedules.Count; i++)
                                               {
                                                   var endDateType = rs.HomeLanguage.Equals("E") ?  "Planned End Date:" : "Fecha de finalización planificada:";
                                                   if (c.ActivitySchedules[i].IsRecurring != null && c.ActivitySchedules[i].IsRecurring == false)
                                                       //If the schedule is non-recurring, display start date for planned end date
                                                       c.ActivitySchedules[i].ScheduleEndDate = c.ActivitySchedules[i].ScheduleStartDate;
                                                   else
                                                       //check if the activity has an ActualEndDate otherwise keep as planned end date
                                                       if (c.ActivitySchedules[i].ActualEndDate != null)
                                                       {
                                                           endDateType                            = rs.HomeLanguage.Equals("E") ? "Actual End Date" : "Fecha de finalización";
                                                           c.ActivitySchedules[i].ScheduleEndDate = c.ActivitySchedules[i].ActualEndDate;
                                                       }

                                                   if (c.ActivitySchedules != null && c.ActivitySchedules.Count > 0)
                                                   {
                                                       //create a string for working days each week
                                                       var wkFrequencyName = "";
                                                       c.ActivitySchedules[i].ActivityScheduleFrequencies.OrderByDescending(e => -e.WKFrequencyId).ToList().ForEach(d => { wkFrequencyName = wkFrequencyName + d.WKFrequencyName + ", "; });
                                                       wkFrequencyName                    = " - " + (wkFrequencyName.Equals("") ? "" : wkFrequencyName.Substring(0, wkFrequencyName.Length                   - 2));
                                                       c.ActivitySchedules[i].IsRecurring = c.ActivitySchedules[i].IsRecurring ?? false;
                                                       wkFrequencyName                    = wkFrequencyName.Equals(" - ") ? "" : wkFrequencyName;
                                                       //is spanish/English version of Schedule
                                                       var textSchedule = rs.HomeLanguage.Equals("E") ? "Schedule " : "El Horario ";
                                                       activitySchedulesContract.Add(new ActivityScheduleReportContract
                                                                                     {
                                                                                         ActivityScheduleNumber            = textSchedule + (i + 1) + ":",
                                                                                         ActivityScheduleStartDate         = c.ActivitySchedules[i].ScheduleStartDate,
                                                                                         ActivitySchedulePlannedEndDate    = c.ActivitySchedules[i].ScheduleEndDate,
                                                                                         ActivityScheduleFrequencyTypeName = (bool) c.ActivitySchedules[i].IsRecurring ? c.ActivitySchedules[i].FrequencyTypeName +  wkFrequencyName : "",
                                                                                         ActivityScheduleHoursPerDay       = c.ActivitySchedules[i].HoursPerDay,
                                                                                         ActivityScheduleEndTime           = c.ActivitySchedules[i].EndTime   != null ? c.ActivitySchedules[i].EndTime.GetShortHourMinuteAndAMPM() : "",
                                                                                         ActivityScheduleBeginTime         = c.ActivitySchedules[i].BeginTime != null ? c.ActivitySchedules[i].BeginTime.GetShortHourMinuteAndAMPM() : "",
                                                                                         ActivityScheduleEndDateType       = endDateType,
                                                                                     });
                                                   }
                                               }

                                               var phoneNum = "";
                                               if (c.NonSelfDirectedActivity?.BusinessPhoneNumber != null)
                                               {
                                                   var businessPhone = Convert.ToDouble(c.NonSelfDirectedActivity?.BusinessPhoneNumber.ToString());
                                                   phoneNum = businessPhone.ToString(CultureInfo.InvariantCulture).Length == 7 ? $"{businessPhone:###-####}" : businessPhone.ToString(CultureInfo.InvariantCulture);
                                                   phoneNum = businessPhone.ToString(CultureInfo.InvariantCulture).Length == 10 ? $"{businessPhone:(###) ###-####}" : phoneNum;
                                               }

                                               var location = new[] { c.NonSelfDirectedActivity?.BusinessLocation?.FullAddress, c.NonSelfDirectedActivity?.BusinessLocation?.Description };
                                               var addr     = string.Join(", ", location.Where(i => !string.IsNullOrWhiteSpace(i)));
                                               activityContract.Add(new ActivityReportContract
                                                                    {
                                                                        ActivityTypeName              = c.ActivityTypeCode + " - " + c.ActivityTypeName,
                                                                        ActivityDescription           = c.Description,
                                                                        IsSelfDirected                = c.NonSelfDirectedActivity == null,
                                                                        ActivityLocationName          = !string.IsNullOrWhiteSpace(c.NonSelfDirectedActivity?.BusinessZipAddress.SafeTrim()) ? $"{addr} {c.NonSelfDirectedActivity ?.BusinessZipAddress}".SafeTrim() : "Self-directed",
                                                                        hasContacts                   = (c.Contacts          != null && c.Contacts.Count          > 0),
                                                                        hasSchedules                  = (c.ActivitySchedules != null && c.ActivitySchedules.Count > 0),
                                                                        ActivityPhone                 = phoneNum.Equals("0") ? "" : phoneNum,
                                                                        ActivityAdditionalInformation = c.AdditionalInformation.NullStringToBlank().SafeTrim(),
                                                                        Contacts                      = contactList.Where(i => i.ActivityId == c.Id).ToList(),
                                                                        ActivitySchedules             = activitySchedulesContract,
                                                                        hasAdditionalInfo             = !c.AdditionalInformation.NullStringToBlank().SafeTrim().Equals("")
                                                                    });
                                           });

            //Supportive Services
            var supportContract = new List<SupportiveServiceReportContract>();
            epRContract.SupportiveServices?.ForEach(c => supportContract.Add(new SupportiveServiceReportContract
                                                                             {
                                                                                 SupportiveServiceTypeName = c.SupportiveServiceTypeName,
                                                                                 SupportDetails            = c.Details
                                                                             }));

            //Employment Schedule
            var scheduleContract = new List<ScheduleReportContract>();
            for (var i = 0; i < EPschedule.Count; i++)
            {
                //find correlated Activity/Schedule
                var scheduleEvent = epRContract.Activities.Where(j => j.Id == EPschedule[i].ActivityId).Select(c => new ScheduleReportContract
                                                                                                                    {
                                                                                                                        TimeWorked = c.ActivitySchedules.Where(s => s.Id == EPschedule[i].ScheduleId)
                                                                                                                                      .Select(sc => !string.IsNullOrEmpty(sc.EndTime.ToString()) ? sc.BeginTime.GetShortHourMinuteAndAMPM() + " - " + sc.EndTime.GetShortHourMinuteAndAMPM() : sc.HoursPerDay + " hours")
                                                                                                                                      .FirstOrDefault(),
                                                                                                                        ActivityTypeName = c.ActivityTypeCode + " - " + c.ActivityTypeName,
                                                                                                                        Description      = c.Description ?? "",
                                                                                                                        Business         = !string.IsNullOrWhiteSpace(c.NonSelfDirectedActivity?.BusinessName) ? c.NonSelfDirectedActivity?.BusinessName ?? "" : "Self-Directed",
                                                                                                                        Location         = string.Join(", ", new[] { c.NonSelfDirectedActivity?.BusinessLocation?.FullAddress, c.NonSelfDirectedActivity?.BusinessLocation?.Description + " " + c.NonSelfDirectedActivity?.BusinessZipAddress }.Where(k => !string.IsNullOrWhiteSpace(k))),
                                                                                                                    }).FirstOrDefault();

                if (scheduleEvent != null)
                {
                    //if first event or new day begins, save start date
                    if (i != 0 && (EPschedule[i]?.StartDate.ToStringMonthDayYear() ?? "").Equals((EPschedule[i - 1].StartDate.ToStringMonthDayYear() ?? "")))
                    {
                        scheduleEvent.Date = "";
                    }
                    else
                    {
                        DateTime startDate = EPschedule[i]?.StartDate != null ? (DateTime) EPschedule[i].StartDate : DateTime.MinValue;
                        scheduleEvent.Date = startDate != DateTime.MinValue ? startDate.ToString("dddd, MMMM dd") : "";
                    }

                    scheduleContract.Add(scheduleEvent);
                }
            }


            var ePStockText = new EPStockText();
            var placement   = rs?.Placement ?? "";

            if (rs != null)
            {
                rs.WkrFirstName = rs.WkrFirstName.TrimAndUpper();
                rs.WkrLastName  = rs.WkrLastName.TrimAndUpper();
                rs.WkrEmail     = rs.WkrEmail.TrimAndUpper();
            }

            // final Report
            var reportContracts = new PrintedEmployabilityPlanReportContract
                                  {
                                      HomeLanguageName = rs.HomeLanguage,
                                      Placement        = ePStockText.GetPlacement(enrolledProgramContract),
                                      Goals            = goalsContract,
                                      Activites        = activityContract,
                                      Support          = supportContract,
                                      Employment       = employmentContract,
                                      Schedule         = scheduleContract,
                                      WorkerPhone      = rs.WkrPhoneNumber.SafeTrim(),
                                      WorkerEmail      = rs.WkrEmail
                                  };


            //All Non-List objects text
            if (rs.HomeLanguage.Equals("S"))
            {
                var finalReport = new EmploymentPlanReportSpanish(reportContracts);

                //Organization
                finalReport.ReportParameters["WorkName"].Value    = (!string.IsNullOrWhiteSpace(rs.AgencyName) ? rs.AgencyName.SafeTrim() : " ").ToUpper();
                finalReport.ReportParameters["WorkAddress"].Value = (!string.IsNullOrWhiteSpace(rs.OrgAddressLine1) ? rs.OrgAddressLine1.SafeTrim() : " ").ToUpper();
                finalReport.ReportParameters["WorkZip"].Value     = !string.IsNullOrWhiteSpace(rs.OrgCity) ? rs.OrgCity.SafeTrim()?.ToUpper() + ", " + rs.OrgState.SafeTrim()?.ToUpper() + " " + rs.OrgZipCode.SafeTrim() : " ";

                //Worker
                finalReport.ReportParameters["ParPin"].Value     = $"{pin}";
                finalReport.ReportParameters["WorkerName"].Value = !string.IsNullOrWhiteSpace(rs.WkrFirstName) ? $"{rs.WkrFirstName} {rs.WkrLastName}" : " ";

                //Participant
                finalReport.ReportParameters["ParName"].Value = epRContract.Participant.FirstName.SafeTrim().ToUpper() + " " + epRContract.Participant.LastName.SafeTrim().ToUpper();
                //Address Line 1 is chosen with logic through the stored procedure
                //When both a Household Address and Aleternate address, print AlternateAddress For CF, TJ, and TMJ When both a Household Address and Mailing address, print Mailing Address.
                finalReport.ReportParameters["ParAddress"].Value = (!string.IsNullOrWhiteSpace(rs.PartAddressLine1) ? rs.PartAddressLine1.SafeTrim() : " ").ToUpper();
                finalReport.ReportParameters["ParZip"].Value     = !string.IsNullOrWhiteSpace(rs.PartCity) ? rs.PartCity.SafeTrim()?.ToUpper() + ", " + rs.PartState.SafeTrim()?.ToUpper() + " " + rs.PartZipCode.SafeTrim() : " ";

                //Plan
                finalReport.ReportParameters["PlanTitle"].Value = ePStockText.GetStockTextSpanish(reportContracts.Placement, "plan_title", null,                                                           null,                                                         placement, stockText);
                finalReport.ReportParameters["PlanParag"].Value = ePStockText.GetStockTextSpanish(reportContracts.Placement, "plan_parag", epRContract.EmployabilityPlan.BeginDate.ToString("MM/dd/yyyy"), epRContract.EmployabilityPlan.EndDate.ToString("MM/dd/yyyy"), placement, stockText);

                //Assigned Activities
                finalReport.ReportParameters["AssignmentTitle"].Value = "Assigned Activities";
                finalReport.ReportParameters["AssignmentParag"].Value = ePStockText.GetStockTextSpanish(reportContracts.Placement, "assign_parag", null, null, placement, stockText);

                //Notes
                finalReport.ReportParameters["EPNotes"].Value = epRContract.EmployabilityPlan.Notes ?? "";

                //Signature
                finalReport.ReportParameters["SignatureName"].Value   = ePStockText.GetStockTextSpanish(reportContracts.Placement, "sig_title",  null, null, placement, stockText);
                finalReport.ReportParameters["SignatureDetail"].Value = ePStockText.GetStockTextSpanish(reportContracts.Placement, "sig_detail", null, null, placement, stockText);

                //Build and return report
                var reportProcessor = new ReportProcessor();
                var renderingResult = reportProcessor.RenderReport("PDF", finalReport, null);

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
            else
            {
                var finalReport = new EmploymentPlanReport(reportContracts);

                //Organization
                finalReport.ReportParameters["WorkName"].Value    = (!string.IsNullOrWhiteSpace(rs.AgencyName) ? rs.AgencyName.SafeTrim() : " ").ToUpper();
                finalReport.ReportParameters["WorkAddress"].Value = (!string.IsNullOrWhiteSpace(rs.OrgAddressLine1) ? rs.OrgAddressLine1.SafeTrim() : " ").ToUpper();
                finalReport.ReportParameters["WorkZip"].Value     = !string.IsNullOrWhiteSpace(rs.OrgCity) ? rs.OrgCity.SafeTrim()?.ToUpper() + ", " + rs.OrgState.SafeTrim()?.ToUpper() + " " + rs.OrgZipCode.SafeTrim() : " ";

                //Worker
                finalReport.ReportParameters["ParPin"].Value       = $"{pin}";
                finalReport.ReportParameters["WorkerName"].Value   = !string.IsNullOrWhiteSpace(rs.WkrFirstName) ? $"{rs.WkrFirstName} {rs.WkrLastName}" : " ";

                //Participant
                finalReport.ReportParameters["ParName"].Value = epRContract.Participant.FirstName.SafeTrim().ToUpper() + " " + epRContract.Participant.LastName.SafeTrim().ToUpper();
                //Address Line 1 is chosen with logic through the stored procedure
                //When both a Household Address and Aleternate address, print AlternateAddress For CF, TJ, and TMJ When both a Household Address and Alternate address, print Alternate Address.
                finalReport.ReportParameters["ParAddress"].Value = (!string.IsNullOrWhiteSpace(rs.PartAddressLine1) ? rs.PartAddressLine1.SafeTrim() : " ").ToUpper();
                finalReport.ReportParameters["ParZip"].Value     = !string.IsNullOrWhiteSpace(rs.PartCity) ? rs.PartCity.SafeTrim()?.ToUpper() + ", " + rs.PartState.SafeTrim()?.ToUpper() + " " + rs.PartZipCode.SafeTrim() : " ";

                //Plan
                finalReport.ReportParameters["PlanTitle"].Value = ePStockText.GetStockText(reportContracts.Placement, "plan_title", null,                                                           null,                                                         placement, stockText);
                finalReport.ReportParameters["PlanParag"].Value = ePStockText.GetStockText(reportContracts.Placement, "plan_parag", epRContract.EmployabilityPlan.BeginDate.ToString("MM/dd/yyyy"), epRContract.EmployabilityPlan.EndDate.ToString("MM/dd/yyyy"), placement, stockText);

                //Assigned Activities
                finalReport.ReportParameters["AssignmentTitle"].Value = "Assigned Activities";
                finalReport.ReportParameters["AssignmentParag"].Value = ePStockText.GetStockText(reportContracts.Placement, "assign_parag", null, null, placement, stockText);

                //Notes
                finalReport.ReportParameters["EPNotes"].Value = epRContract.EmployabilityPlan.Notes ?? "";

                //Signature
                finalReport.ReportParameters["SignatureName"].Value   = ePStockText.GetStockText(reportContracts.Placement, "sig_title",  null, null, placement, stockText);
                finalReport.ReportParameters["SignatureDetail"].Value = ePStockText.GetStockText(reportContracts.Placement, "sig_detail", null, null, placement, stockText);

                //Build and return report
                var reportProcessor = new ReportProcessor();
                var renderingResult = reportProcessor.RenderReport("PDF", finalReport, null);

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
        }

        public bool AppendPdf(string pin, int employabilityPlanId, string firstName, string middleInitial, string lastName, DateTime dateSigned, string resultFile)
        {
            var ep = _epRepo.GetById(employabilityPlanId);

            if (ep == null) return false;

            ep.DateSigned   = dateSigned;
            ep.ModifiedBy   = "CWW";
            ep.ModifiedDate = DateTime.Now;

            var docId = _documentRepository.GetAsQueryable()
                                           .Where(i => i.EmployabilityPlanId == employabilityPlanId)
                                           .OrderByDescending(i => i.ModifiedDate).ThenByDescending(i => i.Id)
                                           .First().Id.ToString();
            var epDoc            = _fileUploadDomain.RetrieveDocument($"W{docId}");
            var epSignDoc        = GenerateSignedPdf(employabilityPlanId, firstName, middleInitial, lastName);
            var documentsToMerge = new[] { epDoc, epSignDoc };

            // ToDo: Check alternative to read byte stream instead of writing to a location and deleting it
            // Create a PdfStreamWriter instance, responsible to write the document into the specified file 
            using (var fileWriter = new PdfStreamWriter(File.OpenWrite(resultFile)))
            {
                // Iterate through the files you would like to merge
                foreach (var document in documentsToMerge)
                {
                    // Open each of the files 
                    using (var fileToMerge = new PdfFileSource(new MemoryStream(document)))
                    {
                        // Iterate through the pages of the current document 
                        foreach (var pageToMerge in fileToMerge.Pages)
                        {
                            // Append the current page to the fileWriter, which holds the stream of the result file 
                            fileWriter.WritePage(pageToMerge);
                        }
                    }
                }
            }

            // ToDO: Determine how to pass doc id and replace the existing file
            var isUploaded = _fileUploadDomain.UploadEPDoc(pin, employabilityPlanId, File.ReadAllBytes(resultFile));

            return isUploaded;
        }

        private byte[] GenerateSignedPdf(int epId, string firstName, string middleInitial, string lastName)
        {
            var isSpanish = _epRepo.GetById(epId).Participant.LanguageCode == "S";

            if (!isSpanish)
            {
                var report = new EPESignReport();
                report.ReportParameters["FirstName"].Value = $"{firstName.SafeTrim().ToUpper()}";
                report.ReportParameters["MI"].Value        = $"{middleInitial.SafeTrim().ToUpper()}";
                report.ReportParameters["LastName"].Value  = $"{lastName.SafeTrim().ToUpper()}";

                var reportProcessor = new ReportProcessor();
                var renderingResult = reportProcessor.RenderReport("PDF", report, null);

                var ms = new MemoryStream();
                ms.Write(renderingResult.DocumentBytes, 0, renderingResult.DocumentBytes.Length);
                ms.Flush();

                var contract = new ReportContract
                               {
                                   FileStream = ms.ToArray(),
                                   MimeType   = renderingResult.MimeType
                               };

                return contract.FileStream;
            }
            else
            {
                var report = new EPESignSpanishReport();
                report.ReportParameters["FirstName"].Value = $"{firstName.SafeTrim().ToUpper()}";
                report.ReportParameters["MI"].Value        = $"{middleInitial.SafeTrim().ToUpper()}";
                report.ReportParameters["LastName"].Value  = $"{lastName.SafeTrim().ToUpper()}";

                var reportProcessor = new ReportProcessor();
                var renderingResult = reportProcessor.RenderReport("PDF", report, null);

                var ms = new MemoryStream();
                ms.Write(renderingResult.DocumentBytes, 0, renderingResult.DocumentBytes.Length);
                ms.Flush();

                var contract = new ReportContract
                               {
                                   FileStream = ms.ToArray(),
                                   MimeType   = renderingResult.MimeType
                               };

                return contract.FileStream;
            }
        }

        #endregion
    }
}

using System;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Interface.Core;
using EnrolledProgramStatusCode = Dcf.Wwp.Model.Interface.Constants.EnrolledProgramStatusCode;

namespace Dcf.Wwp.Api.Library.Utils
{
    public static class ParticipantHelper
    {
        public static List<EnrolledProgramContract> GetMostRecentEnrolledPrograms(IAuthUser _authUser, IParticipant participant, IReadOnlyCollection<ICountyAndTribe> countiesAndTribes, List<IOrganization> organizations, List<IOffice> offices, IRepository repo)
        {
            ICountyAndTribe county   = null;
            var             progList = new List<EnrolledProgramContract>();

            if (participant == null) return progList;
            if (participant.ParticipantEnrolledPrograms == null || participant.ParticipantEnrolledPrograms.Count <= 0)
            {
                return (progList);
            }

            if (participant.PinNumber != null)
            {
                var peps = repo.GetPepRecordsForPin((decimal) participant.PinNumber).ToList();

                foreach (var peploop in peps)
                {
                    if (peploop == null) continue;

                    var pep                = repo.GetPepById(peploop.Id);
                    var agency             = pep.Office?.ContractArea?.Organization;
                    var associatedAgencies = pep.Office?.ContractArea?.AssociatedOrganizations?.Select(i => i.Organization).ToList();
                    var currentDate        = _authUser.CDODate ?? DateTime.Today;
                    var associatedAgencyCodes = associatedAgencies?.Where(i => (i.InActivatedDate == null || i.InActivatedDate >= currentDate) && i.ActivatedDate <= currentDate)
                                                                  .Select(i => i.EntsecAgencyCode).ToList();
                    var associatedAgencyNames = associatedAgencies?.Where(i => (i.InActivatedDate == null || i.InActivatedDate >= currentDate) && i.ActivatedDate <= currentDate)
                                                                  .Select(i => i.AgencyName).ToList();

                    // The business rule per US1347 that the transfer indicator will show up for 14 days.
                    var hasRecentTransfer = pep.OfficeTransfers?.FirstOrDefault(x => (DateTime.Now - x.TransferDate).TotalDays <= 14) != null;
                    var cfRfaDetail       = pep.RequestForAssistance?.CFRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId    == pep.RequestForAssistance?.Id);
                    var tjtmjRfaDetail    = pep.RequestForAssistance?.TJTMJRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId == pep.RequestForAssistance?.Id);
                    var fcdpRfaDetail     = pep.RequestForAssistance?.FCDPRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId  == pep.RequestForAssistance?.Id);

                    if (pep.IsCF)
                    {
                        county = countiesAndTribes.FirstOrDefault(i => i.CountyNumber == cfRfaDetail?.CountyAndTribe?.CountyNumber);
                    }
                    else
                        if (pep.IsFCDP)
                        {
                            county = countiesAndTribes.FirstOrDefault(i => i.CountyNumber == fcdpRfaDetail?.CountyAndTribe?.CountyNumber);
                        }

                    var enrolledProg = new EnrolledProgramContract()
                                       {
                                           Id                      = pep.Id,
                                           ParticipantId           = pep.ParticipantId,
                                           EnrolledProgramId       = pep.EnrolledProgramId,
                                           RfaNumber               = pep.RequestForAssistance?.RfaNumber,
                                           ReferralDate            = pep.ReferralDate,
                                           EnrollmentDate          = pep.EnrollmentDate,
                                           DisenrollmentDate       = pep.DisenrollmentDate,
                                           CompletionReasonId      = pep.CompletionReasonId,
                                           CompletionReasonDetails = pep.PEPOtherInformations?.FirstOrDefault(i => i.PEPId == pep.Id)?.CompletionReasonDetails,
                                           Status                  = pep.StatusCode?.StatusCode,
                                           IsTransfer              = (hasRecentTransfer || (pep.StatusCode?.StatusCode == "Enrolled" && pep.Worker == null)) && (pep.StatusCode?.StatusCode != "Disenrolled"),
                                           CanDisenroll            = (pep.DisenrollmentDate == null), //need to modify later - now returning all can disenroll
                                           StatusDate              = pep.DisenrollmentDate ?? (pep.EnrollmentDate ?? pep.ReferralDate),
                                           OfficeCounty            = pep.Office?.CountyAndTribe?.CountyName,
                                           OfficeNumber            = pep.Office?.OfficeNumber,
                                           ProgramCode             = (pep.EnrolledProgram?.ProgramCode?.Trim() == "WW") ? "W-2" : pep.EnrolledProgram?.DescriptionText,
                                           ProgramCd               = pep.EnrolledProgram?.ProgramCode?.Trim(), //HACK
                                           CourtOrderedDate        = pep.IsCF ? cfRfaDetail?.CourtOrderEffectiveDate : fcdpRfaDetail?.CourtOrderEffectiveDate,
                                           CourtOrderedCounty      = county?.CountyName,
                                           AgencyCode              = agency?.EntsecAgencyCode,
                                           AgencyName              = agency?.AgencyName,
                                           AssociatedAgencyCodes   = associatedAgencyCodes,
                                           AssociatedAgencyNames   = associatedAgencyNames,
                                           ContractorId            = tjtmjRfaDetail?.ContractorId, //pep.RequestForAssistance?.TjTmjContractorId,
                                           ContractorName          = pep.RequestForAssistance?.Office?.ContractArea?.Organization?.AgencyName,
                                           CaseNumber              = pep.CASENumber,
                                           IsVoluntary = pep.RequestForAssistance?.FCDPRfaDetails?
                                                             .FirstOrDefault(i => i.RequestForAssistanceId == pep.RequestForAssistance?.Id)?.IsVoluntary ?? false,
                                           AssignedWorker = (pep.Worker != null)
                                                                ? new WorkerContract
                                                                  {
                                                                      Id            = pep.Worker?.Id,
                                                                      WamsId        = pep.Worker?.WAMSId,
                                                                      WorkerId      = pep.Worker?.MFUserId,
                                                                      Wiuid         = pep.Worker?.WIUID,
                                                                      FirstName     = pep.Worker?.FirstName?.ToTitleCase(),
                                                                      MiddleInitial = pep.Worker?.MiddleInitial?.ToTitleCase(),
                                                                      LastName      = pep.Worker?.LastName?.ToTitleCase(),
                                                                      IsActive      = pep.Worker?.WorkerActiveStatusCode == "ACTIVE"
                                                                  }
                                                                : null,
                                           LearnFareFEP =    (pep.LFFEP != null)
                                                                 ? new WorkerContract
                                                                   {
                                                                       Id            = pep.LFFEP?.Id,
                                                                       WamsId        = pep?.LFFEP?.WAMSId,
                                                                       WorkerId      = pep?.LFFEP?.MFUserId,
                                                                       Wiuid         = pep.LFFEP?.WIUID,
                                                                       FirstName     = pep?.LFFEP?.FirstName?.ToTitleCase(),
                                                                       MiddleInitial = pep.LFFEP?.MiddleInitial?.ToTitleCase(),
                                                                       LastName      = pep?.LFFEP?.LastName?.ToTitleCase(),
                                                                       IsActive      = pep.LFFEP?.WorkerActiveStatusCode == "ACTIVE"
                                                                   }
                                                                 : null
                                       };

                    progList.Add(enrolledProg);
                }
            }

            return (progList);
        }

        public static IParticipantEnrolledProgram GetMostRecentEnrolledProgram(IParticipant participant, IAuthUser authUser, bool skipOrgCheck = false, bool includeDisenrolled = false)
        {
            return participant.ParticipantEnrolledPrograms
                              .Where(i => (i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.EnrolledId
                                           || (includeDisenrolled && i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.DisenrolledId))
                                          && authUser.Authorizations
                                                     .Where(j => j.StartsWith("canAccessProgram_"))
                                                     .Select(j => j.Trim().ToLower().Split('_')[1])
                                                     .Contains(i.EnrolledProgram.ProgramCode.Trim().ToLower())
                                          && (skipOrgCheck || i.Office.ContractArea.Organization.EntsecAgencyCode.Trim().ToLower() == authUser.AgencyCode.Trim().ToLower()))
                              .OrderByDescending(i => i.EnrollmentDate)
                              .First();
        }

        public static ParticipantEnrolledProgram GetMostRecentEnrolledProgram(Participant participant, IAuthUser authUser, bool skipOrgCheck = false, bool includeDisenrolled = false)
        {
            return participant.ParticipantEnrolledPrograms
                              .Where(i => (i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.EnrolledId
                                           || (includeDisenrolled && i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.DisenrolledId))
                                          && authUser.Authorizations
                                                     .Where(j => j.StartsWith("canAccessProgram_"))
                                                     .Select(j => j.Trim().ToLower().Split('_')[1])
                                                     .Contains(i.EnrolledProgram.ProgramCode.Trim().ToLower())
                                          && (skipOrgCheck || i.Office.ContractArea.Organization.EntsecAgencyCode.Trim().ToLower() == authUser.AgencyCode.Trim().ToLower()))
                              .OrderByDescending(i => i.EnrollmentDate)
                              .First();
        }
    }
}

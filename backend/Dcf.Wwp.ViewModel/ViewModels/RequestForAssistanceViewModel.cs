using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Api.Library.Rules;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Constants = Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.DataAccess.Contexts;
using NRules;
using NRules.Fluent;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class RequestForAssistanceViewModel : BasePinViewModel
    {
        #region Properties

        private readonly IAuthUser          _authUser;
        private readonly EPContext          _referenceDataContext;
        private readonly ITransactionDomain _transactionDomain;

        #endregion

        #region Methods

        public RequestForAssistanceViewModel(IRepository repository, IAuthUser authUser, EPContext referenceDataContext, ITransactionDomain transactionDomain) : base(repository, authUser)
        {
            _authUser             = authUser;
            _referenceDataContext = referenceDataContext;
            _transactionDomain    = transactionDomain;
        }

        public RequestForAssistanceContract GetRfa(string pin, int id)
        {
            var rfa = Repo.GetRfa(pin, id);

            var populationTypes = rfa.RequestForAssistancePopulationTypeBridges
                                     .Cast<RequestForAssistancePopulationTypeBridge>()
                                     .Select(i => new { Id = i.PopulationTypeId, i.PopulationType.Name })
                                     .ToArray();

            var pep            = rfa.ParticipantEnrolledPrograms.FirstOrDefault();
            var cfRfaDetail    = rfa.CFRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId    == rfa.Id);
            var tjtmjRfaDetail = rfa.TJTMJRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId == rfa.Id);
            var fcdpRfaDetail  = rfa.FCDPRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId  == rfa.Id);

            var data = new RequestForAssistanceContract
                       {
                           Id                                   = rfa.Id,
                           RfaNumber                            = rfa.RfaNumber,
                           ProgramId                            = rfa.EnrolledProgramId,
                           ProgramCode                          = rfa.EnrolledProgram?.ProgramCode.Trim(),
                           ProgramName                          = rfa.EnrolledProgram?.DescriptionText,
                           StatusId                             = rfa.RequestForAssistanceStatusId,
                           StatusName                           = rfa.RequestForAssistanceStatus.Name,
                           StatusDate                           = rfa.RequestForAssistanceStatusDate,
                           CountyOfResidenceId                  = rfa.CountyOfResidence?.Id,
                           CountyOfResidenceName                = rfa.CountyOfResidence?.CountyName?.Trim().ToTitleCase(),
                           ApplicationDate                      = tjtmjRfaDetail?.ApplicationDate,
                           ApplicationDueDate                   = tjtmjRfaDetail?.ApplicationDueDate,
                           EnrollmentDate                       = pep?.EnrollmentDate,
                           DisenrollmentDate                    = pep?.DisenrollmentDate,
                           CourtOrderCountyTribeId              = rfa.EnrolledProgramId == Constants.EnrolledProgram.ChildrenFirstId ? cfRfaDetail?.CourtOrderedCountyId : fcdpRfaDetail?.CourtOrderedCountyId,
                           CourtOrderCountyTribeName            = rfa.EnrolledProgramId == Constants.EnrolledProgram.ChildrenFirstId ? cfRfaDetail?.CountyAndTribe?.CountyName?.Trim().ToTitleCase() : fcdpRfaDetail?.CountyAndTribe?.CountyName?.Trim().ToTitleCase(),
                           CourtOrderCountyTribeNumber          = rfa.EnrolledProgramId == Constants.EnrolledProgram.ChildrenFirstId ? cfRfaDetail?.CountyAndTribe?.CountyNumber : fcdpRfaDetail?.CountyAndTribe?.CountyNumber,
                           WorkProgramOfficeId                  = rfa.Office?.Id,
                           WorkProgramOfficeName                = rfa.Office?.OfficeName?.Trim().ToTitleCase(),
                           WorkProgramOfficeNumber              = rfa.Office?.OfficeNumber,
                           AgencyName                           = rfa.Office?.ContractArea?.Organization?.AgencyName,
                           AgencyCountyName                     = rfa.Office?.CountyAndTribe?.CountyName?.Trim().ToTitleCase(),
                           CourtOrderEffectiveDate              = rfa.EnrolledProgramId == Constants.EnrolledProgram.ChildrenFirstId ? cfRfaDetail?.CourtOrderEffectiveDate : fcdpRfaDetail?.CourtOrderEffectiveDate,
                           ContractorId                         = tjtmjRfaDetail?.ContractorId,
                           ContractorName                       = tjtmjRfaDetail?.Organization?.AgencyName,
                           CompletionReasonId                   = pep?.CompletionReasonId,
                           CompletionReasonDetails              = pep?.PEPOtherInformations.FirstOrDefault(i => i.PEPId == pep.Id)?.CompletionReasonDetails,
                           AnnualHouseholdIncome                = tjtmjRfaDetail?.HouseholdIncome,
                           HouseholdSize                        = tjtmjRfaDetail?.HouseholdSizeId,
                           LastDateOfEmployment                 = tjtmjRfaDetail?.LastEmploymentDate,
                           HasWorked16HoursLess                 = tjtmjRfaDetail?.HasWorkedLessThan16Hours,
                           TjTmjIsEligibleForUnemployment       = tjtmjRfaDetail?.IsEligibleForUnemployment,
                           TjTmjIsReceivingW2Benefits           = tjtmjRfaDetail?.IsReceivingW2Benefits,
                           IsUSCitizen                          = tjtmjRfaDetail?.IsCitizen,
                           TjTmjHasNeverEmployed                = tjtmjRfaDetail?.HasNeverEmployed,
                           TjTmjHasWorked1040Hours              = tjtmjRfaDetail?.HasWorked1040Hours,
                           TjTmjIsAppCompleteAndSigned          = tjtmjRfaDetail?.IsAppCompleteAndSigned,
                           TjTmjHasEligibilityBeenVerified      = tjtmjRfaDetail?.HasEligibilityBeenVerified,
                           HasChildren                          = tjtmjRfaDetail?.IsUnder18,
                           TjTmjIsBenefitFromSubsidizedJob      = tjtmjRfaDetail?.IsBenefitFromSubsidizedJob,
                           TjTmjBenefitFromSubsidizedJobDetails = tjtmjRfaDetail?.BenefitFromSubsidizedJobDetails,
                           PopulationTypesIds                   = populationTypes.Select(i => (int) i.Id).ToArray(),
                           PopulationTypesNames                 = populationTypes.Select(i => i.Name).ToArray(),
                           PopulationTypeDetails                = tjtmjRfaDetail?.PopulationTypeDetails,
                           KIDSPin                              = fcdpRfaDetail?.KIDSPinNumber,
                           ReferralSource                       = fcdpRfaDetail?.ReferralSource,
                           IsVoluntary                          = fcdpRfaDetail?.IsVoluntary ?? false,
                           ModifiedBy                           = rfa.ModifiedBy,
                           ModifiedDate                         = rfa.ModifiedDate,
                           RowVersion                           = rfa.RowVersion
                       };

            // Add any children.
            foreach (var rfac in rfa.RequestForAssistanceChilds)
            {
                data.Children.Add(new ChildContract
                                  {
                                      DateOfBirth = rfac.Child.DateOfBirth.ToStringMonthDayYear(),
                                      Id          = rfac.Id,
                                      ChildId     = rfac.Child.Id,
                                      FirstName   = rfac.Child.FirstName,
                                      LastName    = rfac.Child.LastName,
                                      GenderId    = rfac.Child.GenderTypeId,
                                      Gender      = rfac.Child.GenderType?.Name
                                  });
            }

            foreach (var rfae in rfa.RequestForAssistanceRuleReasons)
            {
                data.EligibilityCodes.Add(rfae?.RuleReason?.Name);
            }

            return (data);
        }

        public bool PostRfa(RequestForAssistanceContract contract, ref int id)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant has not been initilized.");
            }

            var modDate = DateTime.Now;

            var rfa = Repo.GetRfa(Participant.PinNumber?.ToString(), id) ?? Repo.NewRfa(Participant, AuthUser.Username);

            if (contract.ProgramId.HasValue)
            {
                rfa.EnrolledProgramId = contract.ProgramId.Value;
            }

            rfa.CountyOfResidenceId = contract.CountyOfResidenceId;

            rfa.OfficeId = contract.WorkProgramOfficeId;

            if (contract.ProgramId == Constants.EnrolledProgram.TransitionalJobsId || contract.ProgramId == Constants.EnrolledProgram.TransformMilwaukeeJobsId)
            {
                PostTjTmjRfa(contract, rfa, modDate);

                if (string.IsNullOrWhiteSpace(rfa.RfaNumber.ToString()))
                {
                    rfa.RfaNumber = GenerateRfaNumber(rfa, AuthUser.Username);
                }
            }
            else
                if (contract.ProgramId == Constants.EnrolledProgram.ChildrenFirstId)
                {
                    PostCfRfa(contract, rfa, modDate);

                    if (string.IsNullOrWhiteSpace(rfa.RfaNumber.ToString()))
                    {
                        rfa.RfaNumber = GenerateRfaNumber(rfa, AuthUser.Username);
                    }
                }
                else
                    if (contract.ProgramId == Constants.EnrolledProgram.FCDPId)
                    {
                        PostFcdpRfa(contract, rfa, modDate);
                    }


            // TODO: Implement change tracking and concurrency check instead of hardcoding a change every time.
            rfa.ModifiedBy   = AuthUser.Username;
            rfa.ModifiedDate = modDate;

            Repo.Save();

            id = rfa.Id;

            return (true);
        }

        public bool ChangeRfaStatus(string pin, int id, string newStatus)
        {
            var result = false;
            var rfa    = Repo.GetRfa(Participant.PinNumber?.ToString(), id);

            if (null != rfa)
            {
                newStatus = newStatus.ToLower().UnKabob();

                var rfaStatus = Repo.GetRequestForAssistanceStatus(newStatus);

                if (rfaStatus != null)
                {
                    rfa.RequestForAssistanceStatusId   = rfaStatus.Id;
                    rfa.RequestForAssistanceStatusDate = _authUser.CDODate ?? DateTime.Now;
                    result                             = true;
                }

                if (newStatus == "referred")
                {
                    var pep = Repo.NewPep(rfa, AuthUser.Username);

                    pep.ReferralDate = _authUser.CDODate ?? DateTime.Today;

                    var referralRegCode = Repo.GetReferralRegCode(rfa.Participant.PinNumber, rfa.EnrolledProgram.ProgramCode);

                    pep.ReferralRegistrationCode = referralRegCode;

                    var transactionContract = new TransactionContract
                                              {
                                                  ParticipantId       = pep.ParticipantId,
                                                  WorkerId            = Repo.WorkerByWIUID(_authUser.WIUID).Id,
                                                  OfficeId            = pep.Office.Id,
                                                  EffectiveDate       = pep.ReferralDate.GetValueOrDefault(),
                                                  CreatedDate         = pep.ReferralDate.GetValueOrDefault(),
                                                  TransactionTypeCode = Constants.TransactionTypes.ParticipantReferred,
                                                  ModifiedBy          = _authUser.WIUID
                                              };
                    var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

                    if (transaction != null)
                        Repo.NewTransaction(transaction as ITransaction);
                }

                Repo.Save();

                if (newStatus == "referred" && rfa.EnrolledProgramId != Constants.EnrolledProgram.FCDPId)
                {
                    Repo.WriteBackReferralToDb2(rfa, AuthUser.CDODate ?? DateTime.Now, AuthUser.MainFrameId);
                }

                if (newStatus == "rfa denied")
                {
                    if (!string.IsNullOrWhiteSpace(rfa.RfaNumber.ToString()))
                    {
                        Repo.DenyRFAInDB2(rfa, AuthUser.MainFrameId);
                    }
                }
            }

            return result;
        }

        public List<RequestForAssistanceSummaryContract> RequestForAssistanceSummariesForParticipant(string pin)
        {
            var listOfRfas = new List<RequestForAssistanceSummaryContract>();

            if (Participant != null)
            {
                var rfas = Repo.GetRfasForPin(pin).ToList();

                foreach (var rfa in rfas)
                {
                    var pep            = rfa.ParticipantEnrolledPrograms.FirstOrDefault();
                    var cfRfaDetail    = rfa?.CFRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId    == rfa.Id);
                    var tjtmjRfaDetail = rfa?.TJTMJRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId == rfa.Id);
                    var fcdpRfaDetail  = rfa?.FCDPRfaDetails?.FirstOrDefault(i => i.RequestForAssistanceId  == rfa.Id);

                    var r = new RequestForAssistanceSummaryContract
                            {
                                Id                          = rfa.Id,
                                ProgramId                   = rfa.EnrolledProgramId,
                                ProgramCode                 = rfa.EnrolledProgram?.ProgramCode.Trim(),
                                ProgramName                 = rfa.EnrolledProgram?.DescriptionText,
                                RfaNumber                   = rfa.RfaNumber,
                                StatusId                    = rfa.RequestForAssistanceStatusId,
                                StatusName                  = rfa.RequestForAssistanceStatus?.Name,
                                StatusDate                  = rfa.RequestForAssistanceStatusDate,
                                AgencyName                  = $"{rfa.Office?.ContractArea?.Organization?.AgencyName}",
                                ApplicationDate             = tjtmjRfaDetail?.ApplicationDate,
                                ApplicationDueDate          = tjtmjRfaDetail?.ApplicationDueDate,
                                CourtOrderEffectiveDate     = rfa.EnrolledProgramId == Constants.EnrolledProgram.ChildrenFirstId ? cfRfaDetail?.CourtOrderEffectiveDate : fcdpRfaDetail?.CourtOrderEffectiveDate,
                                CountyOfResidenceName       = rfa.CountyOfResidence?.CountyName?.Trim().ToTitleCase(),
                                WorkProgramOfficeCountyName = $"{rfa.Office?.CountyAndTribe?.CountyName?.Trim().ToTitleCase()}",
                                EnrolledDate                = pep?.EnrollmentDate,
                                DisenrolledDate             = pep?.DisenrollmentDate,
                                ContractorId                = rfa.Office?.ContractArea?.OrganizationId,
                                ContractorName              = tjtmjRfaDetail?.Organization?.AgencyName,
                                ContractorCode              = tjtmjRfaDetail?.Organization?.EntsecAgencyCode,
                                WorkProgramOfficeNumber     = rfa.Office?.OfficeNumber,
                                IsVoluntary                 = fcdpRfaDetail?.IsVoluntary ?? false,
                                KIDSPin                     = fcdpRfaDetail?.KIDSPinNumber,
                                ReferralSource              = fcdpRfaDetail?.ReferralSource
                            };

                    listOfRfas.Add(r);
                }
            }

            return (listOfRfas);
        }

        public List<RequestForAssistanceSummaryContract> RequestForAssistanceOldSummariesForParticipant(string pin)
        {
            var listOfOldRfas = new List<RequestForAssistanceSummaryContract>();

            if (Participant != null)
            {
                var oldRfas = Repo.GetOldRfasForPin(pin).ToList();

                foreach (var oldRfa in oldRfas)
                {
                    var r = new RequestForAssistanceSummaryContract
                            {
                                ProgramName     = oldRfa.ProgramName,
                                RfaNumber       = oldRfa.RfaNumber,
                                StatusName      = oldRfa.RfaStatus,
                                ApplicationDate = oldRfa.ApplicationDate,
                                CountyName      = oldRfa.CountyName,
                                CountyNumber    = oldRfa.CountyNumber
                            };

                    listOfOldRfas.Add(r);
                }
            }

            return (listOfOldRfas);
        }

        public RequestForAssistanceEligiblityContract DetermineEligibility(int id, RequestForAssistanceContract contract, IAuthUser authUser)
        {
            var result = new RequestForAssistanceEligiblityContract();

            var w2Programs = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 11 }; // TODO: GET this from DS

            var fpl             = Repo.EligibilityByFPL(contract.HouseholdSize.Value);
            var counties        = Repo.GetCounties().ToList();
            var fd              = new ReferenceDataViewModel(Repo, authUser, _referenceDataContext);
            var populationTypes = fd.GetData2("population-types", "tmj", contract.ContractorId.ToString());

            var l = Repo.GetPepRecordsForPin((decimal) Participant.PinNumber).ToList();

            if (l.Any(i => w2Programs.Contains((int) i.EnrolledProgramId) && (i.EnrolledProgramStatusCodeId == Constants.EnrolledProgramStatusCode.ReferredId || i.EnrolledProgramStatusCodeId == Constants.EnrolledProgramStatusCode.EnrolledId)))
            {
                contract.AddAlert(Constants.RuleReason.W2);
            }

            // Getting the counties that serve TJ program.
            //   var tjContractAreas = Repo.GetContractAreasByProgramCode(Constants.EnrolledProgram.TjProgramCode);
            var tjContractAreas = Repo.GetContractAreasByProgramCodeAndOrganizationId(Constants.EnrolledProgram.TjProgramCode, contract.ContractorId ?? 0);
            var tjCounties      = new List<int>();

            foreach (var tca in tjContractAreas.AsNotNull())
            {
                var currentDate = _authUser.CDODate ?? DateTime.Today;
                foreach (var off in tca.Offices.Where(i => (i.InactivatedDate == null || i.InactivatedDate >= currentDate) && i.ActiviatedDate <= currentDate).ToList().AsNotNull())
                {
                    if (off.CountyandTribeId.HasValue)
                        tjCounties.Add(off.CountyandTribeId.Value);
                }
            }

            if (tjCounties.Count == 0 && contract.IsTJ)
            {
                throw new Exception("Bad Configuration. There should be TJ counties.");
            }

            var context = new RFARulesContext
                          {
                              TJCounties  = tjCounties.ToArray(),
                              Participant = Participant,
                              FieldData   = populationTypes
                          };

            var repository = new RuleRepository();

            repository.Load(x => x
                                 .From(Assembly.GetExecutingAssembly())
                                 .Where(rule => rule.IsTagged("RFA"))
                                 .To("RFA-RuleSet"));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            session.Insert(context);
            session.Insert(contract);
            session.Insert(fpl);
            session.Insert(counties);
            session.Fire();

            //contract = session.Query<RequestForAssistanceContract>().FirstOrDefault();
            //context  = session.Query<RFARulesContext>().FirstOrDefault();

            //result.HandleIneligibility("SUB", "Unable to obtain and benefit from a subsidized job.");
            //result.IsEligible = !result.ServerMessages.Any();

            if (contract.TjTmjHasNeverEmployed == true)
            {
                contract.LastDateOfEmployment = null;
                contract.HasWorked16HoursLess = null;
            }

            if (contract.LastDateOfEmployment.HasValue)
            {
                var currentDate = _authUser.CDODate ?? DateTime.Today;
                var ts          = currentDate - (DateTime) contract.LastDateOfEmployment; // Tim's checked, they want it 28 days straight out...

                if (ts.TotalDays <= 28)
                {
                    if (contract.HasWorked16HoursLess.HasValue && contract.HasWorked16HoursLess == false)
                    {
                        result.HandleIneligibility(Constants.RuleReason.FRWK, "Employed in the last four weeks.");
                    }
                }
            }

            result.IsEligible = !contract.Alerts.Any();

            //if (contract.Alerts.Any())
            if (!result.IsEligible)
            {
                var ruleReasons             = Repo.GetRuleReasonsWhere(i => i.Category == Constants.RuleReason.RFA && i.SubCategory == Constants.RuleReason.Eligibility);
                var inEligiblityReasonCodes = contract.Alerts.Select(i => i).Distinct().ToList();

                inEligiblityReasonCodes.ForEach(i => result.HandleIneligibility(i, ruleReasons.FirstOrDefault(j => j.Code == i).Name));
            }

            //result.IsEligible = !contract.Alerts.Any();
            result.IsEligible = !result.ServerMessages.Any();

            return (result);
        }

        public PreRfaResponseContract PreCheck(string pin, int programId)
        {
            var rfac = new PreRfaResponseContract { CanEnroll = true };

            var rfaStatuses = new[] { 1, 2, 3 };
            var rfasForPin  = Repo.GetRfasForPin(pin).ToList();
            var hasTMJ      = rfasForPin.Any(rfa => rfaStatuses.Contains(rfa.RequestForAssistanceStatusId) && rfa.EnrolledProgramId == 9);
            var hasTJ       = rfasForPin.Any(rfa => rfaStatuses.Contains(rfa.RequestForAssistanceStatusId) && rfa.EnrolledProgramId == 12);
            var hasCF       = rfasForPin.Any(rfa => rfaStatuses.Contains(rfa.RequestForAssistanceStatusId) && rfa.EnrolledProgramId == 10);

            switch (programId)
            {
                case 9:

                    if (hasTMJ && programId == 9)
                    {
                        rfac.Warnings.Add("An new RFA for TMJ cannot be created - TMJ already exists");
                        break;
                    }

                    if (hasTJ && programId == 9)
                    {
                        rfac.Warnings.Add("An new RFA for TMJ cannot be created - TJ already exists");
                    }

                    break;

                case 12:

                    if (hasTJ && programId == 12)
                    {
                        rfac.Warnings.Add("An new RFA for TJ cannot be created - TJ already exists");
                        break;
                    }

                    if (hasTMJ && programId == 12)
                    {
                        rfac.Warnings.Add("An new RFA for TJ cannot be created - TMJ already exists");
                    }

                    break;

                case 10:

                    if (hasCF)
                    {
                        rfac.Warnings.Add("An new RFA for CF cannot be created - CF already exists");
                    }

                    break;
            }

            rfac.CanEnroll = !(rfac.Warnings.Any() || rfac.Errors.Any());

            return (rfac);
        }

        public PreRfaResponseContract PreCheck(string pin, string newProgramCode)
        {
            var rfac         = new PreRfaResponseContract { CanEnroll = true };
            var reason       = string.Empty;
            var rfaStatuses  = new[] { 1, 2, 3 };
            var ruleReasons  = Repo.GetRuleReasonsWhere(i => i.Category == "RFA" && i.SubCategory == "PreCheck" && !i.IsDeleted).ToList();
            var newProgram   = Repo.WhereEnrolledPrograms(i => i.ProgramCode == newProgramCode).FirstOrDefault();
            var newProgramId = newProgram.Id;
            var rfasForPin   = Repo.GetRfasForPin(pin).ToList();
            var hasTMJ       = rfasForPin.Any(rfa => rfaStatuses.Contains(rfa.RequestForAssistanceStatusId) && rfa.EnrolledProgramId == 9);
            var hasTJ        = rfasForPin.Any(rfa => rfaStatuses.Contains(rfa.RequestForAssistanceStatusId) && rfa.EnrolledProgramId == 12);
            var hasCF        = rfasForPin.Any(rfa => rfaStatuses.Contains(rfa.RequestForAssistanceStatusId) && rfa.EnrolledProgramId == 10);
            //var tmjProgram   = rfasForPin.FirstOrDefault(i => i.EnrolledProgramId == 9);

            switch (newProgramCode.ToUpper())
            {
                case "TMJ":

                    if (hasTMJ && newProgramId == 9)
                    {
                        rfac.Warnings.Add("A new RFA for Transform Milwaukee Jobs cannot be created - Transform Milwaukee Jobs already exists");
                        break;
                    }

                    if (hasTJ && newProgramId == 9)
                    {
                        var tjProgram = rfasForPin.FirstOrDefault(i => i.EnrolledProgramId == 12 && rfaStatuses.Contains(i.RequestForAssistanceStatusId));

                        switch (tjProgram.RequestForAssistanceStatusId)
                        {
                            case 1:
                                reason = ruleReasons.FirstOrDefault(i => i.Code == "TJIP").Name;
                                rfac.Warnings.Add(reason);
                                break;

                            case 2:
                                reason = ruleReasons.FirstOrDefault(i => i.Code == "TJREF").Name;
                                rfac.Warnings.Add(reason);
                                break;
                            case 3:
                                reason = ruleReasons.FirstOrDefault(i => i.Code == "TJENR").Name;
                                rfac.Warnings.Add(reason);
                                break;
                            default:
                                break;
                        }
                    }

                    break;

                case "TJ":

                    if (hasTJ && newProgramId == 12)
                    {
                        rfac.Warnings.Add("A new RFA for Transitional Jobs cannot be created - Transitional Jobs already exists");
                        break;
                    }

                    if (hasTMJ && newProgramId == 12)
                    {
                        var tmjProgram = rfasForPin.FirstOrDefault(i => i.EnrolledProgramId == 9 && rfaStatuses.Contains(i.RequestForAssistanceStatusId));

                        switch (tmjProgram.RequestForAssistanceStatusId)
                        {
                            case 1:
                                reason = ruleReasons.FirstOrDefault(i => i.Code == "TMJIP").Name;
                                rfac.Warnings.Add(reason);
                                break;

                            case 2:
                                reason = ruleReasons.FirstOrDefault(i => i.Code == "TMJREF").Name;
                                rfac.Warnings.Add(reason);
                                break;
                            case 3:
                                reason = ruleReasons.FirstOrDefault(i => i.Code == "TMJENR").Name;
                                rfac.Warnings.Add(reason);
                                break;
                            default:
                                break;
                        }
                    }

                    break;

                case "CF":

                    if (hasCF)
                    {
                        rfac.Warnings.Add("A new RFA for Children First cannot be created - Children First already exists");
                    }

                    break;
            }

            rfac.CanEnroll = !(rfac.Warnings.Any() || rfac.Errors.Any());

            return (rfac);
        }

        public PreRfaResponseContract PreCheckRulesEngineBased(string pin, RequestForAssistanceContract model)
        {
            var rfac = new PreRfaResponseContract { CanEnroll = true };

            if (Participant == null)
                throw new Exception("Participant not loaded.");

            var rfasForPin = Participant.RequestsForAssistance?.ToList();

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category == Constants.RuleReason.RFA && i.SubCategory
                                                                                                 == Constants.RuleReason.PreCheckError).ToList()
                                         };

            var repository = new RuleRepository();

            repository.Load(x => x.From(Assembly.GetExecutingAssembly())
                                  .Where(rule => rule.IsTagged("RFA-PreCheck")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            session.Insert(messageCodeLevelResult);
            session.Insert(rfasForPin);
            session.Insert(Participant);
            session.Insert(model);

            session.Fire();

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        rfac.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        rfac.Warnings?.Add(cml.Message);
                        break;
                }
            }

            // Do not allow RFA to pass validation if there are any errors.
            rfac.CanEnroll = !(rfac.Warnings.Any() || rfac.Errors.Any());

            return (rfac);
        }

        public DisenrollCheckContract Validate(string pin, string ruleCategory, RequestForAssistanceContract contract)
        {
            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = Repo.GetRuleReasonsWhere(i => i.Category == Constants.RuleReason.RFA && i.SubCategory
                                                                                                 == Constants.RuleReason.CFValidate).ToList()
                                         };

            var repository = new RuleRepository();

            repository.Load(x => x.From(Assembly.GetExecutingAssembly())
                                  .Where(rule => rule.IsTagged("RFA-Validate")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            session.Insert(Participant.ParticipantEnrolledPrograms);
            session.Insert(messageCodeLevelResult);
            session.Insert(contract);

            session.Fire();

            var disenrollCheckContract = new DisenrollCheckContract();

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        disenrollCheckContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        disenrollCheckContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            // Do not allow RFA to pass validation if there are any errors.
            disenrollCheckContract.CanDisenroll = (disenrollCheckContract.Errors?.Count == 0);

            return disenrollCheckContract;
        }

        //private static string GenerateRfaNumber()
        //{
        //    const string pool    = "0123456789";
        //    var          builder = new StringBuilder();
        //    var          random  = new Random((int) DateTime.Now.Ticks);

        //    builder.Append("RFA0");

        //    for (var i = 0; i < 6; i++)
        //    {
        //        var c = pool[random.Next(0, pool.Length)];
        //        builder.Append(c);
        //    }

        //    return builder.ToString();
        //}

        private decimal GenerateRfaNumber(IRequestForAssistance rfa, string userId)
        {
            var r = Repo.GenerateRFANumberFromDB2(rfa, AuthUser.MainFrameId);

            return (r);
        }

        private void PostTjTmjRfa(RequestForAssistanceContract contract, IRequestForAssistance rfa, DateTime modDate)
        {
            var tjtmjRfaDetail = Repo.GetTjTmjRfaDetail(rfa.Id) ?? Repo.NewTjTmjRfaDetail(rfa, AuthUser.Username, modDate);

            if (contract.TjTmjIsBenefitFromSubsidizedJob == null || (bool) contract.TjTmjIsBenefitFromSubsidizedJob)
            {
                contract.TjTmjBenefitFromSubsidizedJobDetails = null;
                if (tjtmjRfaDetail != null) tjtmjRfaDetail.BenefitFromSubsidizedJobDetails = null; // do both just for good measure
            }

            if (contract.ApplicationDate.HasValue)
            {
                tjtmjRfaDetail.ApplicationDueDate = Repo.AddBusinessDays(contract.ApplicationDate);
            }

            bool? hasWorked16HrsOrLess = null;

            if (contract.TjTmjHasNeverEmployed == true)
            {
                contract.LastDateOfEmployment = null;
                contract.HasWorked16HoursLess = null;
            }

            // if contract.LastDateOfEmployment is 28 days out, we don't care about contract.HasWorked16HoursLess (zero it out)
            if (contract.LastDateOfEmployment != null)
            {
                var currentDate = _authUser.CDODate ?? DateTime.Today;
                var ts          = currentDate - (DateTime) contract.LastDateOfEmployment; // Tim's checked, they want it 28 days out...

                if (ts.TotalDays <= 28)
                {
                    if (contract.HasWorked16HoursLess.HasValue)
                    {
                        hasWorked16HrsOrLess = contract.HasWorked16HoursLess;
                    }
                }
            }

            // Update the TJ/TMJ data elements.
            tjtmjRfaDetail.ContractorId                    = contract.ContractorId;
            tjtmjRfaDetail.ApplicationDate                 = contract.ApplicationDate;
            tjtmjRfaDetail.IsCitizen                       = contract.IsUSCitizen;
            tjtmjRfaDetail.IsUnder18                       = contract.HasChildren;
            tjtmjRfaDetail.HouseholdSizeId                 = contract.HouseholdSize;
            tjtmjRfaDetail.HouseholdIncome                 = contract.AnnualHouseholdIncome;
            tjtmjRfaDetail.LastEmploymentDate              = contract.LastDateOfEmployment;
            tjtmjRfaDetail.HasWorkedLessThan16Hours        = hasWorked16HrsOrLess;
            tjtmjRfaDetail.IsAppCompleteAndSigned          = contract.TjTmjIsAppCompleteAndSigned;
            tjtmjRfaDetail.HasWorked1040Hours              = contract.TjTmjHasWorked1040Hours;
            tjtmjRfaDetail.IsReceivingW2Benefits           = contract.TjTmjIsReceivingW2Benefits;
            tjtmjRfaDetail.HasEligibilityBeenVerified      = contract.TjTmjHasEligibilityBeenVerified;
            tjtmjRfaDetail.IsBenefitFromSubsidizedJob      = contract.TjTmjIsBenefitFromSubsidizedJob;
            tjtmjRfaDetail.BenefitFromSubsidizedJobDetails = contract.TjTmjBenefitFromSubsidizedJobDetails;
            tjtmjRfaDetail.IsEligibleForUnemployment       = contract.TjTmjIsEligibleForUnemployment;
            rfa.OfficeId                                   = contract.WorkProgramOfficeId;
            tjtmjRfaDetail.IsEligible                      = contract.IsEligible;
            tjtmjRfaDetail.HasNeverEmployed                = contract.TjTmjHasNeverEmployed;


            if (contract.EligibilityCodes != null && contract.EligibilityCodes.Any())
            {
                Repo.DeleteAllRfaEligibilityRows(rfa.Id);

                foreach (var code in contract.EligibilityCodes)
                {
                    var rfae = Repo.NewRfaEligibility(rfa, code, AuthUser.Username);
                    rfa.RequestForAssistanceRuleReasons.Add(rfae);
                }
            }

            // Children repeater
            // TODO: Handle similar/duplicate children.

            // First, cleanse the incoming PreTeen data.  This means clearing out empty repeater items.
            contract.Children = contract.Children.WithoutEmpties();

            // Next map any new items that are similar to existing/deleted items.
            var allChildren = rfa.AllRequestForAssistanceChilds.ToList();
            contract.Children.UpdateNewItemsIfSimilarToExisting(allChildren, ChildContract.AdoptIfSimilarToModel);

            // Now we need to mark the Child records not being used as deleted.
            var childIds = contract.Children.Where(i => i.Id != 0).Select(i => i.Id).ToList();

            // Mark the unused items as deleted.
            allChildren.MarkUnusedItemsAsDeleted(childIds);

            // Now update the database items with the posted model data.
            if (contract.Children != null)
            {
                foreach (var cc in contract.Children)
                {
                    IRequestForAssistanceChild rfac;

                    if (cc.IsNew())
                    {
                        rfac              = Repo.NewRequestForAssistanceChild(rfa, modDate, AuthUser.Username);
                        rfac.ModifiedDate = modDate;
                        rfac.ModifiedBy   = AuthUser.Username;
                    }
                    else
                    {
                        rfac = allChildren.FirstOrDefault(x => x.Id == cc.Id);
                    }

                    // We may need to create a Child record in the database.
                    if (cc.ChildId == 0)
                    {
                        // Associating it this way will take care of the
                        // relationship in the database as in the foreign keys.
                        rfac.Child              = Repo.NewChild();
                        rfac.Child.ModifiedDate = modDate;
                        rfac.Child.ModifiedBy   = AuthUser.Username;
                    }

                    // We will always set the values of the child (even if they are empty).
                    rfac.Child.DateOfBirth  = cc.DateOfBirth.ToDateTimeMonthDayYear();
                    rfac.Child.FirstName    = cc.FirstName;
                    rfac.Child.LastName     = cc.LastName;
                    rfac.Child.GenderTypeId = cc.GenderId;
                    rfac.Child.IsDeleted    = false; // Need to set this in case we've un-soft-deleted an item up above.
                }
            }

            if (contract.IsEligible.HasValue)
            {
                tjtmjRfaDetail.IsEligible = contract.IsEligible;
            }

            if (!string.IsNullOrEmpty(contract.PopulationTypeDetails))
            {
                tjtmjRfaDetail.PopulationTypeDetails = contract.PopulationTypeDetails;
            }

            if (contract.PopulationTypesIds != null && contract.PopulationTypesIds.Any())
            {
                Repo.DeleteAllRfaPopulationTypeBridgeRows(rfa.Id);

                foreach (var populationTypeId in contract.PopulationTypesIds)
                {
                    Repo.NewRfaPopulationTypeBridge(rfa, populationTypeId, AuthUser.Username);
                }
            }

            tjtmjRfaDetail.ModifiedBy   = AuthUser.Username;
            tjtmjRfaDetail.ModifiedDate = modDate;
        }

        private void PostCfRfa(RequestForAssistanceContract contract, IRequestForAssistance rfa, DateTime modDate)
        {
            var cfRfaDetail = Repo.GetCfRfaDetail(rfa.Id) ?? Repo.NewCfRfaDetail(rfa, AuthUser.Username, modDate);

            cfRfaDetail.CourtOrderedCountyId    = contract.CourtOrderCountyTribeId;
            cfRfaDetail.CourtOrderEffectiveDate = contract.CourtOrderEffectiveDate;

            rfa.RequestForAssistanceChilds.ForEach(rfac => rfac.IsDeleted = true); // Clear out Children

            cfRfaDetail.ModifiedBy   = AuthUser.Username;
            cfRfaDetail.ModifiedDate = modDate;
        }

        private void PostFcdpRfa(RequestForAssistanceContract contract, IRequestForAssistance rfa, DateTime modDate)
        {
            var fcdpRfaDetail = Repo.GetFcdpRfaDetail(rfa.Id) ?? Repo.NewFcdpRfaDetail(rfa, AuthUser.Username, modDate);

            fcdpRfaDetail.IsVoluntary = contract.IsVoluntary;

            if (contract.IsVoluntary)
            {
                //clear court order values
                fcdpRfaDetail.CourtOrderedCountyId    = null;
                fcdpRfaDetail.CourtOrderEffectiveDate = null;
            }
            else
            {
                fcdpRfaDetail.CourtOrderedCountyId    = contract.CourtOrderCountyTribeId;
                fcdpRfaDetail.CourtOrderEffectiveDate = contract.CourtOrderEffectiveDate;
            }

            fcdpRfaDetail.KIDSPinNumber  = contract.KIDSPin;
            fcdpRfaDetail.ReferralSource = contract.ReferralSource;

            rfa.RequestForAssistanceChilds.ForEach(rfac => rfac.IsDeleted = true); // Clear out Children

            fcdpRfaDetail.ModifiedBy   = AuthUser.Username;
            fcdpRfaDetail.ModifiedDate = modDate;
        }

        #endregion
    }
}

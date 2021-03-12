using System;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class InformalAssessmentViewModel : BaseInformalAssessmentViewModel
    {
        private readonly ITransactionDomain _transactionDomain;

        public InformalAssessmentViewModel(IRepository repo, IAuthUser authUser, ITransactionDomain transactionDomain) : base(repo, authUser)
        {
            _transactionDomain = transactionDomain;
        }

        public InformalAssessmentContract NewAssessment()
        {
            // If this method is called repeatedly by the user, it causes issues becuase it will
            // create multiple new IA records.  We'll protect this by making sure there is no
            // existing IA record that hasn't yet been submitted.
            if (InformalAssessment != null && !InformalAssessment.EndDate.HasValue)
            {
                // We have an existing IA that doesn't have an end date.  We will return this
                // existing record.
                return GetContract();
            }

            // If we get here, we should create a new assessment.
            var ia = Repo.NewInformalAssessment(Participant.Id, InformalAssessment != null, AuthUser.Username);
            Repo.ResetNoActionNeededs(Participant.Id);
            Repo.Save();

            InformalAssessment = ia;
            return GetContract();
        }

        public InformalAssessmentContract SubmitAssessment()
        {
            var iac = new InformalAssessmentContract();

            if (InformalAssessment != null)
            {
                var updateDate = DateTime.Now;
                var auths      = AuthUser.Authorizations.Where(i => i.StartsWith("canAccessProgram_")).ToList();
                var isFCDP     = auths.Count == 1 && AuthUser.Authorizations.Contains("canAccessProgram_FCD");
                var pep = Participant.ParticipantEnrolledPrograms
                                     .OrderByDescending(i => i.EnrollmentDate)
                                     .FirstOrDefault(i => i.EnrolledProgramId != EnrolledProgram.FCDPId && i.EnrolledProgramStatusCodeId == EnrolledProgramStatusCode.EnrolledId);

                InformalAssessment.EndDate      = updateDate;
                InformalAssessment.ModifiedBy   = AuthUser.Username;
                InformalAssessment.ModifiedDate = updateDate;

                #region Transaction

                var officeId = Participant.EnrolledParticipantEnrolledPrograms
                                          .Where(i => auths.Select(j => j.Trim().ToLower().Split('_')[1])
                                                           .Contains(i.EnrolledProgram.ProgramCode.Trim().ToLower())
                                                      && i.Office.ContractArea.Organization.EntsecAgencyCode.Trim().ToLower() == AuthUser.AgencyCode.Trim().ToLower())
                                          .OrderByDescending(i => i.EnrollmentDate)
                                          .First().Office.Id;

                var transactionContract = new TransactionContract
                                          {
                                              ParticipantId       = Participant.Id,
                                              WorkerId            = Repo.WorkerByWIUID(AuthUser.WIUID).Id,
                                              OfficeId            = officeId,
                                              EffectiveDate       = updateDate,
                                              CreatedDate         = updateDate,
                                              TransactionTypeCode = TransactionTypes.InformalAssessmentSubmitted,
                                              ModifiedBy          = AuthUser.WIUID
                                          };

                var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

                if (transaction != null)
                    Repo.NewTransaction(transaction as ITransaction);

                #endregion

                try
                {
                    if (!isFCDP && pep != null)
                        Repo.SP_DB2_InformalAssessment_Update(InformalAssessment.Participant.PinNumber, AuthUser.MainFrameId);

                    Repo.Save();
                }
                catch (Exception ex)
                {
                    throw new DCFApplicationException("Submit failed due to mainframe issue.  Please try again.", ex);
                }

                iac = GetContract();
            }

            return iac;
        }

        public InformalAssessmentContract GetAssessment() => GetContract();

        private InformalAssessmentContract GetContract()
        {
            var iac = new InformalAssessmentContract();

            if (InformalAssessment == null)
            {
                return iac;
            }

            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null");
            }

            iac.Id                                 = InformalAssessment.Id;
            iac.Type                               = InformalAssessment.AssessmentType?.Code;
            iac.SubmitDate                         = InformalAssessment.EndDate;
            iac.LanguagesSection                   = LanguageSectionViewModel.GetContract(InformalAssessment, Participant);
            iac.WorkHistorySection                 = WorkHistorySectionViewModel.GetContract(InformalAssessment, Repo);
            iac.WorkProgramSection                 = WorkProgramSectionViewModel.GetContract(InformalAssessment, Participant, Repo);
            iac.EducationSection                   = EducationSectionViewModel.GetContract(InformalAssessment, Repo);
            iac.PostSecondarySection               = PostSecondarySectionViewModel.GetContract(InformalAssessment, Participant);
            iac.MilitarySection                    = MilitarySectionViewModel.GetContract(InformalAssessment);
            iac.HousingSection                     = HousingSectionViewModel.GetContract(InformalAssessment, Participant, Repo);
            iac.TransportationSection              = TransportationSectionViewModel.GetContract(InformalAssessment, Participant, Repo);
            iac.LegalIssuesSection                 = LegalIssueSectionViewModel.GetContract(InformalAssessment, Participant, Repo);
            iac.ParticipantBarriersSection         = ParticipantBarrierSectionViewModel.GetContract(InformalAssessment, Participant);
            iac.ChildYouthSupportsSection          = ChildYouthSupportsSectionViewModel.GetContract(InformalAssessment, Participant, Repo);
            iac.FamilyBarriersSection              = FamilyBarriersSectionViewModel.GetContract(InformalAssessment, Participant, Repo);
            iac.NonCustodialParentsSection         = NonCustodialParentsSectionViewModel.GetContract(InformalAssessment, Participant, Repo);
            iac.NonCustodialParentsReferralSection = NonCustodialParentsReferralSectionViewModel.GetContract(InformalAssessment, Participant, Repo);

            return iac;
        }
    }
}

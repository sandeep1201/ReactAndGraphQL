using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DCF.Common.Extensions;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class Participant : BaseEntity, IParticipant
    {
        public bool HasPrograms => ParticipantEnrolledPrograms?.Count > 0;

        /// <summary>
        ///     Returns Peps that are enrolled for this participant.
        /// </summary>
        ICollection<IParticipantEnrolledProgram> IParticipant.EnrolledParticipantEnrolledPrograms => (from x in ParticipantEnrolledPrograms
                                                                                                      where x.EnrolledProgramStatusCodeId ==
                                                                                                            Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId
                                                                                                      select x).Cast<IParticipantEnrolledProgram>().ToList();

        ICollection<IInformalAssessment> IParticipant.InformalAssessments
        {
            get => InformalAssessments.Cast<IInformalAssessment>().ToList();

            set => InformalAssessments = value.Cast<InformalAssessment>().ToList();
        }

        ICollection<IEARequestParticipantBridge > IParticipant.EaRequestParticipantBridges
        {
            get => EaRequestParticipantBridges.Cast<IEARequestParticipantBridge>().ToList();

            set => EaRequestParticipantBridges = value.Cast<EARequestParticipantBridge>().ToList();
        }

        ICollection<IContact> IParticipant.Contacts
        {
            get => (from x in Contacts where x.IsDeleted == false select x).Cast<IContact>().ToList();

            set => Contacts = value.Cast<Contact>().ToList();
        }

        ICollection<IAKA> IParticipant.AKAs
        {
            get => (from x in AKAs where x.IsDeleted == false select x).Cast<IAKA>().ToList();

            set => AKAs = value.Cast<AKA>().ToList();
        }

        /// <summary>
        ///     All Alt ssn data including deleted ones.
        /// </summary>
        ICollection<IAKA> IParticipant.AllAKAs
        {
            get => (from x in AKAs select x).Cast<IAKA>().ToList();

            set => AKAs = value.Cast<AKA>().ToList();
        }

        ICollection<IEducationExam> IParticipant.EducationExams
        {
            get => (from x in EducationExams where x.IsDeleted == false select x).Cast<IEducationExam>().ToList();

            set => EducationExams = value.Cast<EducationExam>().ToList();
        }

        ICollection<IEmploymentInformation> IParticipant.EmploymentInformations
        {
            get => (from x in EmploymentInformations where !x.IsDeleted select x).Cast<IEmploymentInformation>().ToList();
            set => EmploymentInformations = value.Cast<EmploymentInformation>().ToList();
        }

        ICollection<IEmploymentInformation> IParticipant.AllEmploymentInformations
        {
            get => (from x in EmploymentInformations select x).Cast<IEmploymentInformation>().ToList();
            set => EmploymentInformations = value.Cast<EmploymentInformation>().ToList();
        }

        ICollection<IParticipantEnrolledProgram> IParticipant.ParticipantEnrolledPrograms
        {
            get => ParticipantEnrolledPrograms.Cast<IParticipantEnrolledProgram>().ToList();
            set => ParticipantEnrolledPrograms = value.Cast<ParticipantEnrolledProgram>().ToList();
        }

        ICollection<IParticipantChildRelationshipBridge> IParticipant.ParticipantChildRelationshipBridges
        {
            get => ParticipantChildRelationshipBridges.Cast<IParticipantChildRelationshipBridge>().ToList();
            set => ParticipantChildRelationshipBridges = value.Cast<ParticipantChildRelationshipBridge>().ToList();
        }

        ICollection<IRequestForAssistance> IParticipant.RequestsForAssistance
        {
            get => RequestsForAssistance.Cast<IRequestForAssistance>().ToList();
            set => RequestsForAssistance = value.Cast<RequestForAssistance>().ToList();
        }

        ICollection<IParticipantContactInfo> IParticipant.ParticipantContactInfoes
        {
            get => ParticipantContactInfoes.Cast<IParticipantContactInfo>().ToList();

            set => ParticipantContactInfoes = value.Cast<ParticipantContactInfo>().ToList();
        }

        ICollection<IOtherDemographic> IParticipant.OtherDemographics
        {
            get => OtherDemographics.Cast<IOtherDemographic>().ToList();

            set => OtherDemographics = value.Cast<OtherDemographic>().ToList();
        }

        ICollection<IConfidentialPinInformation> IParticipant.ConfidentialPinInformations
        {
            get => ConfidentialPinInformations.Cast<IConfidentialPinInformation>().ToList();

            set => ConfidentialPinInformations = value.Cast<ConfidentialPinInformation>().ToList();
        }


        // Properties not stored in the database.

        ICollection<IBarrierDetail> IParticipant.BarrierDetails
        {
            get => (from x in BarrierDetails where x.IsDeleted == false select x).Cast<IBarrierDetail>().ToList();
            set => BarrierDetails = value.Cast<BarrierDetail>().ToList();
        }

        ICollection<IBarrierDetail> IParticipant.AllBarrierDetails
        {
            get => (from x in BarrierDetails select x).Cast<IBarrierDetail>().ToList();
            set => BarrierDetails = value.Cast<BarrierDetail>().ToList();
        }

        ICollection<IEmployabilityPlan> IParticipant.EmployabilityPlans
        {
            get => EmployabilityPlans.Cast<IEmployabilityPlan>().ToList();

            set => EmployabilityPlans = value.Cast<EmployabilityPlan>().ToList();
        }

        ICollection<IParticipationStatu> IParticipant.ParticipationStatus
        {
            get => ParticipationStatus.Cast<IParticipationStatu>().ToList();

            set => ParticipationStatus = value.Cast<ParticipationStatu>().ToList();
        }

        ICollection<IParticipantEnrolledProgramCutOverBridge> IParticipant.ParticipantEnrolledProgramCutOverBridges
        {
            get => ParticipantEnrolledProgramCutOverBridges.Cast<IParticipantEnrolledProgramCutOverBridge>().ToList();

            set => ParticipantEnrolledProgramCutOverBridges = value.Cast<ParticipantEnrolledProgramCutOverBridge>().ToList();
        }

        ICollection<ITransaction> IParticipant.Transactions
        {
            get => Transactions.Cast<ITransaction>().ToList();

            set => Transactions = value.Cast<Transaction>().ToList();
        }

        ICollection<IWorkerTaskList> IParticipant.WorkerTaskLists
        {
            get => WorkerTaskLists.Cast<IWorkerTaskList>().ToList();

            set => WorkerTaskLists = value.Cast<WorkerTaskList>().ToList();
        }

        public IInformalAssessment InitialInformalAssessment
        {
            get
            {
                if (InformalAssessments == null || InformalAssessments.Count < 1)
                    return null;

                return (from x in InformalAssessments orderby x.CreatedDate select x).FirstOrDefault();
            }
        }

        public IInformalAssessment InProgressInformalAssessment
        {
            get
            {
                if (InformalAssessments == null || InformalAssessments.Count < 1)
                    return null;

                return (from x in InformalAssessments where !x.IsDeleted && !x.EndDate.HasValue orderby x.CreatedDate select x).FirstOrDefault();
            }
        }

        public ILanguageSection LanguageSection
        {
            get
            {
                if (LanguageSections == null || LanguageSections.Count < 1)
                    return null;

                return (from x in LanguageSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public IWorkHistorySection WorkHistorySection
        {
            get
            {
                if (WorkHistorySections == null || WorkHistorySections.Count < 1)
                    return null;

                return (from x in WorkHistorySections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public IWorkProgramSection WorkProgramSection
        {
            get
            {
                if (WorkProgramSections == null || WorkProgramSections.Count < 1)
                    return null;

                return (from x in WorkProgramSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public IEducationSection EducationSection
        {
            get
            {
                if (EducationSections == null || EducationSections.Count < 1)
                    return null;

                return (from x in EducationSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public IPostSecondaryEducationSection PostSecondaryEducationSection
        {
            get
            {
                if (PostSecondaryEducationSections == null || PostSecondaryEducationSections.Count < 1)
                    return null;

                return (from x in PostSecondaryEducationSections orderby x.Id select x).FirstOrDefault();
            }
        }

        public IMilitaryTrainingSection MilitaryTrainingSection
        {
            get
            {
                if (MilitaryTrainingSections == null || MilitaryTrainingSections.Count < 1)
                    return null;

                return (from x in MilitaryTrainingSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public IHousingSection HousingSection
        {
            get
            {
                if (HousingSections == null || HousingSections.Count < 1)
                    return null;

                return (from x in HousingSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public ITransportationSection TransportationSection
        {
            get
            {
                if (TransportationSections == null || TransportationSections.Count < 1)
                    return null;

                return (from x in TransportationSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public ILegalIssuesSection LegalIssuesSection
        {
            get
            {
                if (LegalIssuesSections == null || LegalIssuesSections.Count < 1)
                    return null;

                return (from x in LegalIssuesSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        public IBarrierSection BarrierSection
        {
            get
            {
                if (BarrierSections == null || BarrierSections.Count < 1)
                    return null;

                return (from x in BarrierSections orderby x.Id select x).FirstOrDefault();
            }
        }

        public IChildYouthSection ChildYouthSection
        {
            get
            {
                if (ChildYouthSections == null || ChildYouthSections.Count < 1)
                    return null;

                return (from x in ChildYouthSections orderby x.Id select x).FirstOrDefault();
            }
        }

        public IFamilyBarriersSection FamilyBarriersSection
        {
            get
            {
                if (FamilyBarriersSections == null || FamilyBarriersSections.Count < 1)
                    return null;

                return (from x in FamilyBarriersSections orderby x.Id select x).FirstOrDefault();
            }
        }

        public INonCustodialParentsSection NonCustodialParentsSection
        {
            get
            {
                if (NonCustodialParentsSections == null || NonCustodialParentsSections.Count < 1)
                    return null;

                return (from x in NonCustodialParentsSections orderby x.Id select x).FirstOrDefault();
            }
        }

        public INonCustodialParentsReferralSection NonCustodialParentsReferralSection
        {
            get
            {
                if (NonCustodialParentsReferralSections == null || NonCustodialParentsReferralSections.Count < 1)
                    return null;

                return (from x in NonCustodialParentsReferralSections where !x.IsDeleted orderby x.Id select x).FirstOrDefault();
            }
        }

        // SQL stores Y/N for these because they are written back to DB2. DB2 here is the consumer so lets follow its datatype.

        [NotMapped]
        public bool? IsAmericanIndian
        {
            get => AmericanIndianIndicator.ToBool();

            set => AmericanIndianIndicator = value.ToYn();
        }

        [NotMapped]
        public bool? IsBlack
        {
            get => BlackIndicator.ToBool();

            set => BlackIndicator = value.ToYn();
        }

        [NotMapped]
        public bool? IsAsian
        {
            get => AsianIndicator.ToBool();

            set => AsianIndicator = value.ToYn();
        }

        [NotMapped]
        public bool? IsHispanic
        {
            get => HispanicIndicator.ToBool();

            set => HispanicIndicator = value.ToYn();
        }

        [NotMapped]
        public bool? IsWhite
        {
            get => WhiteIndicator.ToBool();

            set => WhiteIndicator = value.ToYn();
        }

        [NotMapped]
        public bool? IsPacificIslander
        {
            get => PacificIslanderIndicator.ToBool();

            set => PacificIslanderIndicator = value.ToYn();
        }

        [NotMapped]
        public bool? HasAlias
        {
            get => AliasResponse.ToBool();

            set => AliasResponse = value.ToYn();
        }

        [NotMapped]
        public int? AgeInYears
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;

                var today = DateTime.Today;
                var age   = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value > today.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}

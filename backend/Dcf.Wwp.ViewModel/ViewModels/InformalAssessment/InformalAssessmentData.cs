using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Cww;


namespace Dcf.Wwp.Api.Library.InformalAssessment
{
    #region Information Assessment Parent

    [DataContract]
    public class InformalAssessmentData
    {
        [DataMember(Name = "pin")]
        public string Pin { get; set; }

        [DataMember(Name = "languages")]
        public LanguagesData LanguagesData { get; set; }

        [DataMember(Name = "school")]
        public SchoolData SchoolData { get; set; }

        [DataMember(Name = "post-secondary")]
        public PostSecondaryData PostSecondaryData { get; set; }

        [DataMember(Name = "work-programs")]
        public WorkProgramData WorkProgramData { get; set; }

        [DataMember(Name = "military")]
        public MilitaryTrainingData MilitaryTrainingData { get; set; }

        [DataMember(Name = "childcare")]
        public ChildCareData ChildCareData { get; set; }

        [DataMember(Name = "housing")]
        public HousingData HousingData { get; set; }

        [DataMember(Name = "transportation")]
        public TransportationData TransportationData { get; set; }
        [DataMember(Name = "legal")]
        public LegalIssuesData LegalIssueData { get; set; }
        [DataMember(Name = "work")]
        public WorkHistoryData WorkHistoryData { get; set; }
    }


    #endregion

    #region Transportation Section

    [DataContract]
    public class TransportationData
    {
        [DataMember(Name = "trans-rowversion")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "trans-type")]
        public int? Type { get; set; }

        [DataMember(Name = "trans-type-details")]
        public string TypeDetails { get; set; }

        [DataMember(Name = "trans-dl[]")]
        public int? DlType { get; set; }

        [DataMember(Name = "trans-dl-details")]
        public string DlDetails { get; set; }

        [DataMember(Name = "trans-dl-exp")]
        public string DlExpire { get; set; }

        [DataMember(Name = "trans-dl-state")]
        public int? DlState { get; set; }
    }

    #endregion

    #region Child Care Section
    [DataContract]
    public class ChildCareData
    {
        [DataMember(Name = "childcare-version")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "childcare-lastEdit-author")]
        public string LastEditAuthor { get; set; }

        [DataMember(Name = "childcare-lastEdit-date")]
        public string LastEditDate { get; set; }

        [DataMember(Name = "cc-chldrn[]")]
        public bool? HasChildren { get; set; }

        [DataMember(Name = "cc-teen[]")]
        public bool? HasChildrenWithDisabilityInNeedOfChildCare { get; set; }

        [DataMember(Name = "cc-chldrn-list-xxx")]
        public List<ChildCareContract> Children { get; set; }

        [DataMember(Name = "cc-teen-list-xxx")]
        public List<TeenCareContract> Teen { get; set; }

        [DataMember(Name = "cc-change[]")]
        public bool? HasFutureChildCareNeeds { get; set; }

        [DataMember(Name = "cc-change-notes")]
        public string FutureChildCareNeedNotes { get; set; }

        //How can we assist you with child care?
        [DataMember(Name = "cc-need")]
        public string AssistDetails { get; set; }

        //New Radio Array

        [DataMember(Name = "cc-action[]")]
        public List<int> ActionNeededs { get; set; }


        //ChildCare Notes
        [DataMember(Name = "cc-chldrn-notes")]
        public string Notes { get; set; }

        [DataMember(Name = "childcare-cww")]
        public List<Child> CwwChildren { get; set; }
    }

    #endregion

    #region Housing Section

    [DataContract]
    public class HousingData
    {
        [DataMember(Name = "housing-version")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "housing-lastEdit-author")]
        public string LastEditAuthor { get; set; }

        [DataMember(Name = "housing-lastEdit-date")]
        public string LastEditDate { get; set; }

        [DataMember(Name = "housing-curr-type")]
        public int? HousingCurrentType { get; set; }

        [DataMember(Name = "housing-curr-duration")]
        public int? HousingCurrentDuration { get; set; }

        [DataMember(Name = "housing-curr-eviction[]")]
        public bool? HousingCurrentEviction { get; set; }

        [DataMember(Name = "housing-curr-notes")]
        public string CurrentHousingNotes { get; set; }

        [DataMember(Name = "housing-curr-rent")]
        public int? HousingCurrentRent { get; set; }

        [DataMember(Name = "housing-curr-utility[]")]
        public bool? HousingCurrentUtility { get; set; }

        [DataMember(Name = "housing-curr-utility-notes")]
        public string HousingActionUtilityNotes { get; set; }

        [DataMember(Name = "housing-curr-parti[]")]
        public bool? HousingCurrentParticipant { get; set; }

        [DataMember(Name = "housing-curr-parti-notes")]
        public string HousingActionParticipateNotes { get; set; }

        [DataMember(Name = "housing-action-notes")]
        public string HousingActionNotes { get; set; }

        [DataMember(Name = "housing-action[]")]
        public List<int> ActionNeededs { get; set; }

        [DataMember(Name = "housing-action-refer")]
        public string OtherAgencyDetails { get; set; }

        [DataMember(Name = "housing-history-xxx")]
        public List<HousingHistoryContract> Histories { get; set; }
        [DataMember(Name = "housing-notes")]
        public string HousingNotes { get; set; }

        [DataMember(Name = "housing-cww")]
        public List<CwwHousing> CwwHousing { get; set; }
    }

    #endregion

    #region Miltary Section

    [DataContract]
    public class MilitaryTrainingData
    {
		[DataMember(Name = "militaryVersion")]
		public byte[] RowVersion { get; set; }

		[DataMember(Name = "militaryLastEditAuthor")]
		public string LastEditAuthor { get; set; }

		[DataMember(Name = "militaryLastEditDate")]
		public string LastEditDate { get; set; }

		[DataMember(Name = "milTraining[]")]
		public bool? DoesHaveTraining { get; set; }
		[DataMember(Name = "milBranch")]
		public int? MilitaryBranch { get; set; }
		[DataMember(Name = "milRank")]
		public int? MilitaryRank { get; set; }

		[DataMember(Name = "milRate")]
		[StringLength(200)]
		public string MilitaryRate { get; set; }

		[DataMember(Name = "milYears")]
		public int? MilitaryYears { get; set; }
		[DataMember(Name = "milDischarge")]
		public int? DischargeType { get; set; }

		[DataMember(Name = "milSkills")]
		[StringLength(200)]
		public string SkillsAndTraining { get; set; }

		[DataMember(Name = "militaryNotes")]
		[StringLength(1000)]
		public string MilitaryNotes { get; set; }

	}

    #endregion

    #region Post Secondary Education Section

    [DataContract]
    public class PostSecondaryData
    {
        [DataMember(Name = "post-secondary-version")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "post-secondary-lastEdit-author")]
        public string ModifiedBy { get; set; }

        [DataMember(Name = "post-secondary-lastEdit-date")]
        public string ModifiedDate { get; set; }

        [DataMember(Name = "ps-attn-college[]")]
        public bool? DidAttendCollege { get; set; }

        [DataMember(Name = "ps-coll-list-xxx")]
        public List<PostSecondaryCollegeContract> PostSecondaryColleges { get; set; }

        [DataMember(Name = "ps-degree[]")]
        public bool? HasDegree { get; set; }

        [DataMember(Name = "ps-degrees-list-xxx")]
        public List<PostSecondaryDegreeContract> PostSecondaryDegrees { get; set; }

        [DataMember(Name = "ps-lic-cert[]")]
        public bool? IsWorkingOnLicensesOrCertificates { get; set; }

        [DataMember(Name = "ps-license-list-xxx")]
        public List<PostSecondaryLicenseContract> PostSecondaryLicenses { get; set; }


        [DataMember(Name = "ps-post-secondary-notes")]
        public string PostSecondaryNotes { get; set; }

    }

    #endregion

    #region Education History Section

    [DataContract]
    public class SchoolData
    {
        /*
		{
			"assessment":{
				"pin":"5007737046",
				"school":{
					"school-rowversion":[
					0,
					0,
					0,
					0,
					0,
					0,
					101,
					202
					],
					"school-lastEdit-author":"XCT266",
					"school-lastEdit-date":"2/11/16 12:27",
					"diploma[]":"4",
					"formal[]":"2",
					"hs-loc":"Madison, WI",
					"hs-state":"WI",
					"hs-last-year":"2013",
					"hs-completed":"3",
					"hs-enrolled[]":"Yes",
					"ged-year":"2010",
					"ged-hsed[]":"No",
					"education-notes":"some education"
				}
			}
		}
		*/

        [DataMember(Name = "school-version")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "school-lastEdit-author")]
        public string LastEditAuthor { get; set; }

        [DataMember(Name = "school-lastEdit-date")]
        public string LastEditDate { get; set; }


        [DataMember(Name = "school-dip[]")]
        public int? Diploma { get; set; }

        [DataMember(Name = "school-hs-loc")]
        public string SchoolLocation { get; set; }

        [DataMember(Name = "school-hs-lng")]
        public string Longitude { get; set; }

        [DataMember(Name = "school-hs-lat")]
        public string Latitude { get; set; }

        //[DataMember(Name = "school-hs-state")]
        //public string State { get; set; }

        [DataMember(Name = "school-hs-city")]
        public string City { get; set; }

        [DataMember(Name = "school-hs-country")]
        public string Country { get; set; }

        [DataMember(Name = "school-hs-address")]
        public string FullAddress { get; set; }

        [DataMember(Name = "school-hs-locid")]
        public string GooglePlaceId { get; set; }

        [DataMember(Name = "school-hs-name")]
        public string SchoolName { get; set; }

        [DataMember(Name = "school-notes")]
        public string Notes { get; set; }

        [DataMember(Name = "school-ged-year")] // new
        public int? CertificateYearAwarded { get; set; }

        [DataMember(Name = "school-hs-last-year")] //new
        public int? LastYearAttended { get; set; }

        [DataMember(Name = "school-hs-enrolled[]")] //new
        public bool? IsCurrentlyEnrolled { get; set; }


        [DataMember(Name = "school-hs-completed")]
        public int? LastGradeCompleted { get; set; }

        [DataMember(Name = "school-ged-hsed[]")] //new
        public bool? GedHsedStatus { get; set; }

        [DataMember(Name = "school-formal[]")] //new
        public bool? HasEverGoneToSchool { get; set; }

        [DataMember(Name = "school-ged-issuer")]
        public int? IssuingAuthorityCode { get; set; }


    }

    #endregion

    #region Languages Section

    [DataContract]
    public class LanguagesData
    {
        [DataMember(Name = "languages-version")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "languages-lastEdit-author")]
        public string LastEditAuthor { get; set; }

        [DataMember(Name = "languages-lastEdit-date")]
        public string LastEditDate { get; set; }

        [DataMember(Name = "lang-home-id")]
        public int HomeLangId { get; set; }

        [DataMember(Name = "lang-home")]
        public int HomeLang { get; set; }

        [DataMember(Name = "lang-home-read[]")]
        public bool? IsAbleToReadHomeLang { get; set; }

        [DataMember(Name = "lang-home-write[]")]
        public bool? IsAbleToWriteHomeLang { get; set; }

        [DataMember(Name = "lang-home-speak[]")]
        public bool? IsAbleToSpeakHomeLang { get; set; }

        [DataMember(Name = "lang-eng-id")]
        public int EnglishLangId { get; set; }

        [DataMember(Name = "lang-eng-read[]")]
        public bool? EnglishRead { get; set; }

        [DataMember(Name = "lang-eng-write[]")]
        public bool? EnglishWrite { get; set; }

        [DataMember(Name = "lang-eng-speak[]")]
        public bool? EnglishSpeak { get; set; }

        [DataMember(Name = "lang-list-xxx", IsRequired = false)]
        public List<KnownLanguageContract> KnownLanguages { get; set; }

        [DataMember(Name = "lang-notes")]
        public string LangNotes { get; set; }

        [DataMember(Name = "lang-interpreter[]")]
        public bool? IsNeedingInterpreter { get; set; }

        [DataMember(Name = "lang-interpreter-notes")]
        public string InterpreterNotes { get; set; }
    }

    #endregion

    #region Work Programs Section

    [DataContract]
    public class WorkProgramData
    {
        [DataMember(Name = "workprogramsversion")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "workprogramslastEditauthor")]
        public string LastEditAuthor { get; set; }

        [DataMember(Name = "workprogramslastEditdate")]
        public string LastEditDate { get; set; }

        [DataMember(Name = "workp[]")]
        public bool? IsInOtherPrograms { get; set; }

        [DataMember(Name = "workpnotes")]
        public string Notes { get; set; }

        [DataMember(Name = "workplistxxx")]
        public List<WorkProgramContract> WorkPrograms { get; set; }
    }

    #endregion

    #region Legal Issues Section

    [DataContract]
    public class LegalIssuesData
    {
        [DataMember(Name = "legal-version")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "legal-lastEdit-author")]
        public string LastEditAuthor { get; set; }

        [DataMember(Name = "legal-lastEdit-date")]
        public string LastEditDate { get; set; }

        [DataMember(Name = "legal-crime[]")]
        public bool? IsConvictedOfCrime { get; set; }

        [DataMember(Name = "legal-crime-list-xxx")]
        public List<ConvictionContract> Convictions { get; set; }

        [DataMember(Name = "legal-parole[]")]
        public bool? IsParoled { get; set; }

        [DataMember(Name = "legal-parole-details")]
        public string ParoledDetails { get; set; }

        // Supervision Contact.
        [DataMember(Name = "legal-parole-contact")]
        public int? SupervisionContactId { get; set; }

        [DataMember(Name = "legal-pending[]")]
        public bool? IsPending { get; set; }

        [DataMember(Name = "legal-pending-list-xxx")]
        public List<PendingContract> Pendings { get; set; }

        [DataMember(Name = "legal-family[]")]
        public bool? HasFamilyLegalIssues { get; set; }

        [DataMember(Name = "legal-family-details")]
        public string FamilyLegalIssueNotes { get; set; }

        [DataMember(Name = "legal-child[]")]
        public bool? HasChildWelfareWorker { get; set; }

        [DataMember(Name = "legal-child-details")]
        public string ChildWelfareNotes { get; set; }

        // Child Welfare Worker Contact.
        [DataMember(Name = "legal-child-welfare-contact")]
        public int? ChildWelfareWorkerContactId { get; set; }


        // Child Support.
        [DataMember(Name = "legal-child-support[]")]
        public bool? HasChildSupport { get; set; }

        [DataMember(Name = "legal-child-support-amount")]
        public string ChildSupportAmount { get; set; }

        [DataMember(Name = "legal-child-support-amount-non[]")]
        public bool?[] IsAmountUnknown { get; set; }

        [DataMember(Name = "legal-back-child-support[]")]
        public bool? HasBackChildSupport { get; set; }

        [DataMember(Name = "legal-child-support-details")]
        public string ChildSupportDetails { get; set; }


        // Upcoming court dates?
        [DataMember(Name = "legal-upcoming[]")]
        public bool? HasUpcomingCourtDates { get; set; }

        [DataMember(Name = "legal-upcoming-list-xxx")]
        public List<CourtContract> CourtDates { get; set; }


        [DataMember(Name = "legal-action[]")]
        public List<int> ActionNeededs { get; set; }

        [DataMember(Name = "legal-action-details")]
        public string ActionNeededDetails { get; set; }

        [DataMember(Name = "legal-notes")]
        public string Notes { get; set; }
    }


    #endregion

    #region WorkHistory Section
    [DataContract]
    public class WorkHistoryData
    {
        [DataMember(Name = "work-version")]
        public byte[] RowVersion { get; set; }

        [DataMember(Name = "work-lastEdit-author")]
        public string LastEditAuthor { get; set; }

        [DataMember(Name = "work-lastEdit-date")]
        public string LastEditDate { get; set; }

        [DataMember(Name = "emp-stat[]")]
        public int? EmploymentStatus { get; set; }

        [DataMember(Name = "emp-reason[]")]
        public List<int> PreventionFactors { get; set; }

        [DataMember(Name = "emp-reason-details")]
        public string Details { get; set; }

        [DataMember(Name = "emp-no-job[]")]
        public bool? HasVolunteered { get; set; }

    }
    #endregion

    #region Checklist Section

    [DataContract]
    public class ChecklistData
    {
        [DataMember(Name = "checklist-version")]
        public byte[] RowVersion { get; set; }
    }

    #endregion

    #region Work Skills Section

    [DataContract]
    public class SkillsData
    {
        [DataMember(Name = "skills-version")]
        public byte[] RowVersion { get; set; }
    }

    #endregion

}

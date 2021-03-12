using System;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class EducationSectionViewModel : BaseInformalAssessmentViewModel
    {
        #region Properties

        private readonly IGoogleApi         _googleApi;
        private readonly ITransactionDomain _transactionDomain;

        #endregion

        #region Methods

        public EducationSectionViewModel(IGoogleApi googleApi, IRepository repo, IAuthUser authUser, ITransactionDomain transactionDomain) : base(repo, authUser)
        {
            _googleApi         = googleApi;
            _transactionDomain = transactionDomain;
        }

        public EducationHistorySectionContract GetData()
        {
            return GetContract(InformalAssessment, Repo);
        }

        public bool PostData(EducationHistorySectionContract contract, string user)
        {
            var p = Participant;

            if (p == null)
                throw new InvalidOperationException("PIN not valid.");

            if (contract == null)
                throw new InvalidOperationException("Education data is missing.");

            IEducationSection           es  = null;
            IEducationAssessmentSection eas = null;

            var updateTime = DateTime.Now;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (p.InProgressInformalAssessment != null)
            {
                eas = p.InProgressInformalAssessment.EducationAssessmentSection ?? Repo.NewEducationAssessmentSection(p.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(eas);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                eas.ReviewCompleted = true;
                eas.ModifiedBy      = AuthUser.Username;
                eas.ModifiedDate    = updateTime;
            }

            es              = p.EducationSection ?? Repo.NewEducationSection(p, user);
            es.ModifiedBy   = AuthUser.Username;
            es.ModifiedDate = updateTime;

            Repo.StartChangeTracking(es);

            var userRowVersion       = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            ISchoolCollegeEstablishment sce = null;
            sce        = Repo.SchoolById(es.SchoolCollegeEstablishmentId) ?? Repo.NewSchoolByEducationSection(es, user);
            sce.Name   = contract.SchoolName.SafeTrim();
            sce.Street = contract.Location?.FullAddress.SafeTrim();

            sce.City = contract.Location != null ? Repo.GetOrCreateCity(contract.Location, _googleApi.GetPlaceDetails, _googleApi.GetLatLong, user) : null;

            if (contract.Location?.City == null)
            {
                sce.City = null;
            }

            sce.ModifiedDate              = updateTime;
            sce.ModifiedBy                = user;
            es.SchoolCollegeEstablishment = sce;

            // Graduation Statuses.
            const string d       = "Diploma";
            const string gd      = "GED";
            const string hsd     = "HSED";
            const string n       = "None";
            var          diploma = Repo.SchoolGraduationStatusByName(d);
            var          ged     = Repo.SchoolGraduationStatusByName(gd);
            var          hsed    = Repo.SchoolGraduationStatusByName(hsd);
            var          none    = Repo.SchoolGraduationStatusByName(n);

            // Here's the logic for the different Grad Status options.
            if (contract.Diploma == null || contract.Diploma == diploma.Id)
            {
                // Each option tracks the Last Year Attended
                es.LastYearAttended = contract.LastYearAttended;

                if (contract.Diploma == diploma.Id)
                {
                    es.SchoolGraduationStatusId = contract.Diploma;
                    // Clear out all values from other graduation statuses.
                    es.HasEverAttendedSchool       = null;
                    es.SchoolGradeLevel            = null;
                    es.IsCurrentlyEnrolled         = null;
                    es.HasEducationPlan            = null;
                    es.EducationPlanDetails        = null;
                    es.CertificateIssuingAuthority = null;
                    es.CertificateYearAwarded      = null;
                    es.IsWorkingOnCertificate      = null;
                }

                // Education Plans.
                es.HasEducationPlan     = null;
                es.EducationPlanDetails = null;
            }

            if (contract.Diploma == ged.Id || contract.Diploma == hsed.Id)
            {
                es.SchoolGraduationStatusId = contract.Diploma;
                es.LastYearAttended         = contract.LastYearAttended;
                // Clear out properties not available for GED & HSED.
                es.HasEverAttendedSchool  = null;
                es.IsWorkingOnCertificate = null;
                es.IsCurrentlyEnrolled    = null;
                es.HasEducationPlan       = null;
                es.EducationPlanDetails   = null;

                // Last Grade Completed
                es.LastGradeLevelCompletedId = contract.LastGradeCompleted;

                // Issuing Authority
                if (contract.IssuingAuthorityCode == null)
                {
                    es.CertificateIssuingAuthority = null;
                }
                else
                {
                    var exc = Repo.CertificateIssuerById(contract.IssuingAuthorityCode);
                    es.CertificateIssuingAuthority = exc;
                }

                // Year Awarded.
                es.CertificateYearAwarded = contract.CertificateYearAwarded;

                // Education Plans.
                es.HasEducationPlan     = null;
                es.EducationPlanDetails = null;
            }

            if (contract.Diploma == none.Id)
            {
                es.CertificateIssuingAuthority   = null;
                es.CertificateIssuingAuthorityId = null;
                es.CertificateYearAwarded        = null;
                es.SchoolGraduationStatusId      = none.Id;
                es.LastYearAttended              = contract.LastYearAttended;
                es.HasEverAttendedSchool         = contract.HasEverGoneToSchool;

                // Remove Any School Data if participant has never been to school.
                if (es.HasEverAttendedSchool == false)
                {
                    es.SchoolCollegeEstablishment   = null;
                    es.SchoolCollegeEstablishmentId = null;
                    es.IsCurrentlyEnrolled          = null;
                    es.SchoolGradeLevel             = null;
                    es.LastYearAttended             = null;
                    es.HasEducationPlan             = null;
                    es.EducationPlanDetails         = null;
                }
                else
                {
                    //Currently enrolled?
                    es.IsCurrentlyEnrolled = contract.IsCurrentlyEnrolled;

                    // Education Plans.
                    if (contract.IsCurrentlyEnrolled == true)
                    {
                        es.HasEducationPlan     = contract.HasEducationPlan;
                        es.EducationPlanDetails = contract.HasEducationPlan == true ? contract.EducationPlanDetails : null;
                    }
                    else
                    {
                        es.HasEducationPlan     = null;
                        es.EducationPlanDetails = null;
                    }

                    // Last Grade Completed
                    es.LastGradeLevelCompletedId = contract.LastGradeCompleted;
                }

                // Are you working towards a GED or HSED?
                es.IsWorkingOnCertificate = contract.GedHsedStatus;
            }

            es.Notes = contract.Notes;

            var currentIA = Repo.GetMostRecentAssessment(p);
            currentIA.ModifiedBy   = AuthUser.Username;
            currentIA.ModifiedDate = updateTime;

            #region Transaction

            if (p.InitialInformalAssessment.EndDate != null)
            {
                var officeId = Participant.EnrolledParticipantEnrolledPrograms
                                          .Where(i => AuthUser.Authorizations
                                                              .Where(j => j.StartsWith("canAccessProgram_"))
                                                              .Select(j => j.Trim().ToLower().Split('_')[1])
                                                              .Contains(i.EnrolledProgram.ProgramCode.Trim().ToLower())
                                                      && i.Office.ContractArea.Organization.EntsecAgencyCode.Trim().ToLower() == AuthUser.AgencyCode.Trim().ToLower())
                                          .OrderByDescending(i => i.EnrollmentDate)
                                          .First().Office.Id;

                var transactionContract = new TransactionContract
                                          {
                                              ParticipantId       = Participant.Id,
                                              WorkerId            = Repo.WorkerByWIUID(AuthUser.WIUID).Id,
                                              OfficeId            = officeId,
                                              EffectiveDate       = updateTime,
                                              CreatedDate         = updateTime,
                                              TransactionTypeCode = TransactionTypes.EducationHistory,
                                              ModifiedBy          = AuthUser.WIUID
                                          };

                var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

                if (transaction != null)
                    Repo.NewTransaction(transaction as ITransaction);
            }

            #endregion

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(es, userRowVersion))
                return false;

            if (!Repo.IsRowVersionStillCurrent(eas, userAssessRowVersion))
                return false;

            // If the first save completes, it actually has already saved the AssessmentSection
            // object as well since they are on the save repository context.  But if the Section didn't
            // need saving, we still need to SaveIfChangd on the AssessmentSection.
            if (!Repo.SaveIfChanged(es, user))
                Repo.SaveIfChanged(eas, user);

            return true;
        }

        public static EducationHistorySectionContract GetContract(IInformalAssessment ia, IRepository repo)
        {
            var contract = new EducationHistorySectionContract();

            if (ia != null)
            {
                if (ia.Participant?.EducationSection != null)
                {
                    var es = ia.Participant.EducationSection;

                    contract.RowVersion   = es.RowVersion;
                    contract.ModifiedBy   = es.ModifiedBy;
                    contract.ModifiedDate = es.ModifiedDate;

                    const string d       = "Diploma";
                    const string gd      = "GED";
                    const string hsd     = "HSED";
                    const string n       = "None";
                    var          diploma = repo.SchoolGraduationStatusByName(d);
                    var          ged     = repo.SchoolGraduationStatusByName(gd);
                    var          hsed    = repo.SchoolGraduationStatusByName(hsd);
                    var          none    = repo.SchoolGraduationStatusByName(n);

                    if (es.SchoolGraduationStatus == null || es.SchoolGraduationStatusId == diploma.Id)
                    {
                        if (es.SchoolGraduationStatusId == diploma.Id)
                        {
                            contract.Diploma     = es.SchoolGraduationStatusId;
                            contract.DiplomaName = es.SchoolGraduationStatus?.Name;
                        }

                        contract.SchoolName = es.SchoolCollegeEstablishment?.Name;

                        if (es.SchoolCollegeEstablishment != null)
                        {
                            contract.Location = LocationHelper.GetLocationInfo(es, es.SchoolCollegeEstablishment?.City); //refactored 

                            //if (es.SchoolCollegeEstablishment?.City != null)
                            //{
                            //    if (es.SchoolCollegeEstablishment?.City?.State != null)
                            //    {
                            //        var description =
                            //            PrettyLocation.CityStateCountry(es.SchoolCollegeEstablishment?.City?.Name,
                            //                es.SchoolCollegeEstablishment?.City?.State?.Code,
                            //                es.SchoolCollegeEstablishment?.City?.State?.Country?.Name);
                            //        var location = new LocationContract();
                            //        location.Description = description;
                            //        location.City = es.SchoolCollegeEstablishment?.City?.Name;
                            //        location.State = es.SchoolCollegeEstablishment?.City?.State?.Code;
                            //        location.Country = es.SchoolCollegeEstablishment?.City?.State?.Country?.Name;
                            //        location.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                            //        contract.Location = location;
                            //    }
                            //    else
                            //    {
                            //        var description1 =
                            //            PrettyLocation.CityCountry(es.SchoolCollegeEstablishment?.City?.Name,
                            //                es.SchoolCollegeEstablishment?.City?.Country?.Name);
                            //        var location1 = new LocationContract();
                            //        location1.Description = description1;
                            //        location1.City = es.SchoolCollegeEstablishment?.City?.Name;
                            //        location1.Country = es.SchoolCollegeEstablishment?.City?.Country?.Name;
                            //        location1.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                            //        contract.Location = location1;
                            //    }
                            //}
                            //else
                            //{
                            //    var location = new LocationContract();
                            //    contract.Location = location;
                            //}
                        }
                        else
                        {
                            var location = new LocationContract();
                            contract.Location = location;
                        }

                        contract.HasEducationPlan     = null;
                        contract.EducationPlanDetails = null;
                    }

                    if (es.SchoolGraduationStatusId == ged.Id || es.SchoolGraduationStatusId == hsed.Id)
                    {
                        if (es.SchoolGraduationStatusId == ged.Id)
                        {
                            contract.Diploma     = es.SchoolGraduationStatusId;
                            contract.DiplomaName = es.SchoolGraduationStatus?.Name;
                        }

                        if (es.SchoolGraduationStatusId == hsed.Id)
                        {
                            contract.Diploma     = es.SchoolGraduationStatusId;
                            contract.DiplomaName = es.SchoolGraduationStatus?.Name;
                        }

                        contract.SchoolName = es.SchoolCollegeEstablishment?.Name;

                        if (es.SchoolCollegeEstablishment != null)
                        {
                            contract.Location = LocationHelper.GetLocationInfo(es, es.SchoolCollegeEstablishment?.City); //refactored 

                            //if (es.SchoolCollegeEstablishment?.City?.State != null)
                            //{
                            //    var description =
                            //        PrettyLocation.CityStateCountry(es.SchoolCollegeEstablishment?.City?.Name,
                            //            es.SchoolCollegeEstablishment?.City?.State?.Code,
                            //            es.SchoolCollegeEstablishment?.City?.State?.Country?.Name);
                            //    var location = new LocationContract();
                            //    location.Description = description;
                            //    location.City = es.SchoolCollegeEstablishment?.City?.Name;
                            //    location.State = es.SchoolCollegeEstablishment?.City?.State?.Code;
                            //    location.Country = es.SchoolCollegeEstablishment?.City?.State?.Country?.Name;
                            //    location.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                            //    contract.Location = location;
                            //}
                            //else
                            //{
                            //    var description1 = PrettyLocation.CityCountry(
                            //        es.SchoolCollegeEstablishment?.City?.Name,
                            //        es.SchoolCollegeEstablishment?.City?.Country?.Name);
                            //    var location1 = new LocationContract();
                            //    location1.Description = description1;
                            //    location1.City = es.SchoolCollegeEstablishment?.City?.Name;
                            //    location1.Country = es.SchoolCollegeEstablishment?.City?.Country?.Name;
                            //    location1.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                            //    contract.Location = location1;
                            //}
                        }
                        else
                        {
                            var location = new LocationContract();
                            contract.Location = location;
                        }

                        contract.LastGradeCompleted     = es.SchoolGradeLevel?.Id;
                        contract.LastGradeCompletedName = es.SchoolGradeLevel?.Name;
                        contract.IssuingAuthorityCode   = es.CertificateIssuingAuthority?.Id;
                        contract.IssuingAuthorityName   = es.CertificateIssuingAuthority?.Name;
                        contract.HasEducationPlan       = null;
                        contract.EducationPlanDetails   = null;
                        contract.CertificateYearAwarded = es.CertificateYearAwarded;
                    }

                    if (es.SchoolGraduationStatusId == none.Id)
                    {
                        contract.Diploma             = es.SchoolGraduationStatusId;
                        contract.DiplomaName         = es.SchoolGraduationStatus?.Name;
                        contract.HasEverGoneToSchool = es.HasEverAttendedSchool;
                        contract.SchoolName          = es.SchoolCollegeEstablishment?.Name;

                        if (es.SchoolCollegeEstablishment != null)
                        {
                            contract.Location = LocationHelper.GetLocationInfo(es, es.SchoolCollegeEstablishment?.City); //refactored 

                            //if (es.SchoolCollegeEstablishment?.City?.State != null)
                            //{
                            //    var description =
                            //        PrettyLocation.CityStateCountry(es.SchoolCollegeEstablishment?.City?.Name,
                            //            es.SchoolCollegeEstablishment?.City?.State?.Code,
                            //            es.SchoolCollegeEstablishment?.City?.State?.Country?.Name);
                            //    var location = new LocationContract();
                            //    location.Description = description;
                            //    location.City = es.SchoolCollegeEstablishment?.City?.Name;
                            //    location.State = es.SchoolCollegeEstablishment?.City?.State?.Code;
                            //    location.Country = es.SchoolCollegeEstablishment?.City?.State?.Country?.Name;
                            //    location.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                            //    contract.Location = location;
                            //}
                            //else
                            //{
                            //    var description1 = PrettyLocation.CityCountry(
                            //        es.SchoolCollegeEstablishment?.City?.Name,
                            //        es.SchoolCollegeEstablishment?.City?.Country?.Name);
                            //    var location1 = new LocationContract();
                            //    location1.Description = description1;
                            //    location1.City = es.SchoolCollegeEstablishment?.City?.Name;
                            //    location1.Country = es.SchoolCollegeEstablishment?.City?.Country?.Name;
                            //    location1.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                            //    contract.Location = location1;
                            //}
                        }
                        else
                        {
                            var location = new LocationContract();
                            contract.Location = location;
                        }

                        contract.LastGradeCompleted     = es.LastGradeLevelCompletedId;
                        contract.LastGradeCompletedName = es.SchoolGradeLevel?.Name;
                        contract.IssuingAuthorityCode   = es.CertificateIssuingAuthority?.Id;
                        contract.IssuingAuthorityName   = es.CertificateIssuingAuthority?.Name;
                        contract.CertificateYearAwarded = es.CertificateYearAwarded;
                        contract.IsCurrentlyEnrolled    = es.IsCurrentlyEnrolled;

                        if (es.IsCurrentlyEnrolled == true)
                        {
                            contract.HasEducationPlan = es.HasEducationPlan;

                            if (es.HasEducationPlan == true)
                                contract.EducationPlanDetails = es.EducationPlanDetails;
                        }

                        contract.GedHsedStatus = es.IsWorkingOnCertificate;
                    }

                    contract.LastYearAttended = es.LastYearAttended;
                    contract.Notes            = es.Notes;
                }

                // We look at the assessment section now which at this point just
                // indicates it was submitted via the driver flow.
                if (ia.EducationAssessmentSection != null)
                {
                    contract.AssessmentRowVersion     = ia.EducationAssessmentSection.RowVersion;
                    contract.IsSubmittedViaDriverFlow = true;
                }
            }

            return contract;
        }

        #endregion
    }
}

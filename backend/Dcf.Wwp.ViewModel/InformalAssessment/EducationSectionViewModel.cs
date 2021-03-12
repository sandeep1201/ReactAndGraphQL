using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.ViewModel.Extensions;
using Dcf.Wwp.ViewModel.Helpers;

namespace Dcf.Wwp.ViewModel.InformalAssessment
{
    public  class EducationSectionViewModel:BaseInformalAssessmentViewModel
    {
        public EducationSectionViewModel(IRepository repo) : base(repo)
	    {

        }
        #region GetData

        public SchoolData GetData()
        {

            var ia = InformalAssessment;
            var esd = new SchoolData();

            if (ia?.EducationSection == null)
                return esd;

            var es = ia.EducationSection;

            esd.RowVersion = es.RowVersion;
            esd.LastEditAuthor = es.ModifiedBy;
            esd.LastEditDate = es.ModifiedDate?.ToString("M/d/yy h:mm tt");

            const int Diploma = 1;
            const int Ged = 2;
            const int Hsed = 3;
            const int None = 4;

            if (es.SchoolGraduationStatus == null || es.SchoolGraduationStatus.Id == Diploma)
            {
                if (es.SchoolGraduationStatus?.Id == Diploma)
                {
                    esd.Diploma = Diploma;
                }


                esd.SchoolName = es.SchoolCollegeEstablishment?.Name;


                if (es.SchoolCollegeEstablishment != null)
                {
                    esd.FullAddress = es.SchoolCollegeEstablishment.Street;


                    esd.SchoolLocation =
                        SchoolEstablishmentLocation.OutputFormattedForeignOrDomesticLocation(
                            es.SchoolCollegeEstablishment?.City?.Name, es.SchoolCollegeEstablishment?.City?.State?.Code,
                            es.SchoolCollegeEstablishment?.City?.State?.Country?.Name);


                    esd.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                }
            }

            if (es.SchoolGraduationStatus?.Id == Ged || es.SchoolGraduationStatus?.Id == Hsed)
            {
                if (es.SchoolGraduationStatus.Id == Ged)
                {

                    esd.Diploma = Ged;
                }

                if (es.SchoolGraduationStatus.Id == Hsed)
                {

                    esd.Diploma = Hsed;
                }

                esd.SchoolName = es.SchoolCollegeEstablishment?.Name;


                if (es.SchoolCollegeEstablishment != null)
                {
                    esd.SchoolLocation =
                        SchoolEstablishmentLocation.OutputFormattedForeignOrDomesticLocation(
                            es.SchoolCollegeEstablishment?.City?.Name, es.SchoolCollegeEstablishment?.City?.State?.Code,
                            es.SchoolCollegeEstablishment?.City?.State?.Country?.Name);

                    esd.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                }

                esd.LastGradeCompleted = es.SchoolGradeLevel?.Id;
                esd.IssuingAuthorityCode = es.CertificateIssuingAuthority?.Id;
                esd.CertificateYearAwarded = es.CertificateYearAwarded;
            }

            if (es.SchoolGraduationStatus?.Id == 13)
            {
                esd.Diploma = 4;
                esd.HasEverGoneToSchool = es.HasEverAttendedSchool;
                esd.SchoolName = es.SchoolCollegeEstablishment?.Name;

                if (es.SchoolCollegeEstablishment != null)
                {
                    esd.SchoolLocation =
                        SchoolEstablishmentLocation.OutputFormattedForeignOrDomesticLocation(
                            es.SchoolCollegeEstablishment?.City?.Name, es.SchoolCollegeEstablishment?.City?.State?.Code,
                            es.SchoolCollegeEstablishment?.City?.State?.Country?.Name);


                    esd.GooglePlaceId = es.SchoolCollegeEstablishment?.City?.GooglePlaceId;
                }

                esd.LastGradeCompleted = es.SchoolGradeLevel?.Id;
                esd.IssuingAuthorityCode = es.CertificateIssuingAuthority?.Id;
                esd.CertificateYearAwarded = es.CertificateYearAwarded;
                esd.IsCurrentlyEnrolled = es.IsCurrentlyEnrolled;
                esd.GedHsedStatus = es.IsWorkingOnCertificate;
            }

            esd.LastYearAttended = es.LastYearAttended;
            esd.LastYearAttended = es.LastYearAttended;
            esd.Notes = es.Notes;
            return esd;
        }

        #endregion


        #region UpsertData
        public static bool UpsertData(IRepository repo, string pin, SchoolData model, string user)
        {

            var p = repo.ParticipantByPin(pin);
            if (p == null)
                throw new InvalidOperationException("PIN not valid.");

            if (model == null)
                throw new InvalidOperationException("School data is missing.");

            // Validation of model.
            var lastYearAttended = model.LastYearAttended?.ToString();
            if (lastYearAttended != null && lastYearAttended.Length != 4)
            {
                throw new InvalidOperationException("Last Year Attended is in wrong format.");
            }

            var certificateYearAwarded = model.CertificateYearAwarded?.ToString();
            if (certificateYearAwarded != null && certificateYearAwarded.Length != 4)
            {
                throw new InvalidOperationException("Certificate Year Awarded is in wrong format.");
            }

            IInformalAssessment ia = null;
            IEducationSection es = null;


            if (p.InformalAssessments == null || p.InformalAssessments.Count == 0)
            {
                // we need to create a new one.
                ia = repo.NewInformalAssessment();
                ia.ParticipantId = p.Id;
                es = repo.NewEducationSection(ia);
            }
            else
            {
                // For now we take the first informal assessment.
                ia = p.InitialAssessment;
                es = ia.EducationSection ?? repo.NewEducationSection(ia);
            }

            repo.StartChangeTracking(es);
            var userRowVersion = model.RowVersion;


            // School Location string broken down into city state country.
            var commaNumber = new List<int>();

            // Check to see if the country contains a state.
            int i = 0;
            if (model.SchoolLocation != null)
                while ((i = model.SchoolLocation.IndexOf(',', i)) != -1)
                {
                    commaNumber.Add(1);
                    // Increment the index.
                    i++;
                }


            string cityName;
            string stateCode;
            string countryName;

            switch (commaNumber.Count)
            {
                case 2:
                    cityName = model.SchoolLocation.ToCity().SafeTrim();
                    stateCode = model.SchoolLocation.ToState().SafeTrim();
                    countryName = model.SchoolLocation.ToCountry().SafeTrim();
                    break;
                case 1:
                    cityName = model.SchoolLocation?.ToCity().SafeTrim();
                    stateCode = null;
                    countryName = model.SchoolLocation?.ToCountryShort().SafeTrim();
                    break;
                default:
                    cityName = null;
                    stateCode = null;
                    countryName = null;
                    break;
            }


            ISchoolCollegeEstablishment sce = null;

            if (cityName != null && countryName != null)
            {
                // Finding existing School. 
                // With a State.
                if (stateCode != null)
                    sce = repo.SchoolByNameStreetCityStateCodeCountry(model.SchoolName, model.FullAddress, cityName, stateCode,
                        countryName);
                // Finding existing School. 
                // Without a State.
                if (stateCode == null)
                    sce = repo.SchoolByNameStreetCityCountry(model.SchoolName, model.FullAddress, cityName, countryName);


                if (sce != null)
                {
                    es.SchoolCollegeEstablishment = sce;
                }
                else
                {
                    ICity ci = null;
                    IState st = null;
                    ICountry co = null;

                    sce = repo.NewSchoolByEducationSection(es, user);

                    ci = repo.CitybyCityStateCountry(cityName, stateCode, countryName) ?? repo.NewCity(sce, user);
                    st = repo.StateByStateAndCountryId(stateCode, countryName) ?? repo.NewState(ci, user);
                    co = repo.CountryByName(countryName) ?? repo.NewCountry(st, user);

                    es.SchoolCollegeEstablishment.Street = model.FullAddress;
                    es.SchoolCollegeEstablishment.City = ci;
                    es.SchoolCollegeEstablishment.City.State = st;
                    es.SchoolCollegeEstablishment.City.State.Country = co;

                    es.SchoolCollegeEstablishment.Street = model.FullAddress;

                    ci.Name = cityName;
                    ci.GooglePlaceId = model.GooglePlaceId;
                    var coors = ExternalAPIs.GoogleViewModel.GetLatLong(model.GooglePlaceId);
                    ci.LongitudeNumber = coors[1];
                    ci.LatitudeNumber = coors[0];
                    ci.Country = co;
                    st.Code = stateCode;
                    co.Name = countryName;
                    sce.Name = model.SchoolName.SafeTrim();
                    sce.Street = model.FullAddress.SafeTrim();

                    es.SchoolCollegeEstablishment = sce;
                }
            }
            else
            {
                sce = repo.SchoolByNameStreetCityStateCodeCountry(model.SchoolName, model.FullAddress, cityName, stateCode,
                      countryName);

                if (sce != null)
                {
                    es.SchoolCollegeEstablishment = sce;
                }
                else
                {
                    ICity ci = null;
                    IState st = null;
                    ICountry co = null;

                    sce = repo.NewSchoolByEducationSection(es, user);


                    ci = repo.CitybyName(String.Empty) ?? repo.NewCity(sce, user);
                    st = repo.StateByCode(String.Empty) ?? repo.NewState(ci, user);
                    co = repo.CountryByName(String.Empty) ?? repo.NewCountry(st, user);


                    sce.Name = model.SchoolName.SafeTrim();
                    sce.Street = model.FullAddress.SafeTrim();
                    ci.Name = String.Empty;

                    ci.Country = co;
                    st.Code = String.Empty;
                    co.Name = String.Empty;

                    sce.City = ci;
                    sce.City.State = st;
                    sce.City.State.Country = co;

                    es.SchoolCollegeEstablishment = sce;
                }
            }


            // Graduation Statuses.
            const int Diploma = 1;
            const int Ged = 2;
            const int Hsed = 3;
            const int None = 4;

            // Here's the logic for the different Grad Status options.
            if (model.Diploma == null || model.Diploma == Diploma)
            {
                // Each option tracks the Last Year Attended
                es.LastYearAttended = model.LastYearAttended;
                if (model.Diploma == Diploma)
                {
                    es.SchoolGraduationStatusId = model.Diploma;
                    // Clear out all values from other graduation statuses.
                    es.HasEverAttendedSchool = null;
                    es.SchoolGradeLevel = null;
                    es.IsCurrentlyEnrolled = null;
                    es.CertificateIssuingAuthority = null;
                    es.CertificateYearAwarded = null;
                    es.IsWorkingOnCertificate = null;
                }
            }
            if (model.Diploma == Ged || model.Diploma == Hsed)
            {
                es.SchoolGraduationStatusId = model.Diploma;
                es.LastYearAttended = model.LastYearAttended;
                // Clear out properties not available for GED & HSED.
                es.HasEverAttendedSchool = null;
                es.IsWorkingOnCertificate = null;
                es.IsCurrentlyEnrolled = null;
                // Last Grade Completed
                if (model.LastGradeCompleted.HasValue)
                {
                    var exg = repo.SchoolGradeLevelById(model.LastGradeCompleted.Value);
                    es.SchoolGradeLevel = exg;
                }
                else
                {
                    es.SchoolGradeLevel = null;
                    es.LastGradeLevelCompletedId = null;
                }

                // Issuing Authority
                if (model.IssuingAuthorityCode == null)
                {
                    es.CertificateIssuingAuthority = null;
                }
                else
                {
                    var exc = repo.CertificateIssuerById(model.IssuingAuthorityCode);
                    es.CertificateIssuingAuthority = exc;
                }

                // Year Awarded.
                es.CertificateYearAwarded = model.CertificateYearAwarded;

            }


            if (model.Diploma == None)
            {

                es.CertificateIssuingAuthority = null;
                es.CertificateIssuingAuthorityId = null;
                es.CertificateYearAwarded = null;
                es.SchoolGraduationStatusId = 13;

                es.LastYearAttended = model.LastYearAttended;
                es.HasEverAttendedSchool = model.HasEverGoneToSchool;

                // Remove Any School Data if participant has never been to school.
                if (es.HasEverAttendedSchool == false)
                {
                    es.SchoolCollegeEstablishment = null;
                    es.SchoolCollegeEstablishmentId = null;
                    es.IsCurrentlyEnrolled = null;
                    es.SchoolGradeLevel = null;
                    es.LastYearAttended = null;

                }
                else
                {
                    //Currently enrolled?
                    es.IsCurrentlyEnrolled = model.IsCurrentlyEnrolled;


                    // Last Grade Completed
                    if (model.LastGradeCompleted.HasValue)

                    {
                        var exg = repo.SchoolGradeLevelById(model.LastGradeCompleted.Value);
                        es.SchoolGradeLevel = exg;
                    }
                    else
                    {
                        es.SchoolGradeLevel = null;
                        es.LastGradeLevelCompletedId = null;
                    }
                }
                // Are you working towards a GED or HSED?
                es.IsWorkingOnCertificate = model.GedHsedStatus;
            }

            es.Notes = model.Notes;
            // Do a concurrency check.
            if (!repo.IsRowVersionStillCurrent(es, userRowVersion))
            {
                //UpsertError = "Version has changed.";
                return false;
            }
            repo.SaveIfChanged(es, user);
            //UpsertError = String.Empty;
            return true;
        }
        #endregion
    }
}

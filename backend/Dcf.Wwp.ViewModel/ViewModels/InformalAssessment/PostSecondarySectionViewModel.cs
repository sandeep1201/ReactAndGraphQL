using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class PostSecondarySectionViewModel : BaseInformalAssessmentViewModel
    {
        #region Properties

        private readonly IGoogleApi _googleApi;

        #endregion

        #region Methods

        public PostSecondarySectionViewModel(IGoogleApi googleApi, IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
            _googleApi = googleApi;
        }

        public PostSecondarySectionContract GetData() => GetContract(InformalAssessment, Participant);

        public bool PostData(PostSecondarySectionContract model, string user)
        {
            var ia = InformalAssessment;
            var p  = Participant;

            if (ia == null)
            {
                throw new InvalidOperationException("InformalAssessment record is missing.");
            }

            if (p == null)
            {
                throw new InvalidOperationException("PIN not valid.");
            }

            if (model == null)
            {
                throw new InvalidOperationException("Post Secondary data is missing.");
            }

            var updateTime = DateTime.Now;

            var pseSec = p.PostSecondaryEducationSection ?? Repo.NewPostSecondaryEducationSection(p, user);
            Repo.StartChangeTracking(pseSec);

            var pseAssesmtSec = ia.PostSecondaryEducationAssessmentSection ?? Repo.NewPostSecondaryEducationAssessmentSection(ia, user);
            Repo.StartChangeTracking(pseAssesmtSec);

            var userRowVersion    = model.RowVersion;
            var userAssRowVersion = model.AssessmentRowVersion;

            pseSec.DidAttendCollege = model.HasAttendedCollege;

            // Check if they answered yes for attending college.
            CheckCollegeAttendedInfo(pseSec, model, user, updateTime);

            // Apply Sort Order for colleges,degrees
            ApplySortOrderForDegrees(pseSec);
            // var i = 0;
            // pseSec.PostSecondaryColleges.ForEach(psc => psc.SortOrder = ++i); // simpler and no overhead of an untestable private method...

            // Check if they are working towards license or certificates.
            pseSec.IsWorkingOnLicensesOrCertificates = model.IsWorkingOnLicensesOrCertificates;
            CheckLicenseOrCertificatesInfo(pseSec, model, user, updateTime);

            pseSec.Notes = model.PostSecondaryNotes;

            var currentIA = Repo.GetMostRecentAssessment(p);
            currentIA.ModifiedBy   = AuthUser.Username;
            currentIA.ModifiedDate = updateTime;
            ia.ModifiedBy                               = AuthUser.Username; //very likely the exact same instance as 'p.InProgressInformalAssessment'
            ia.ModifiedDate                             = updateTime;

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(pseSec, userRowVersion))
            {
                return false;
            }

            if (!Repo.IsRowVersionStillCurrent(pseAssesmtSec, userAssRowVersion))
            {
                return false;
            }

            if (!Repo.SaveIfChanged(pseSec, user))
            {
                Repo.SaveIfChanged(pseAssesmtSec, user);
            }

            return true;
        }

        private void CheckCollegeAttendedInfo(IPostSecondaryEducationSection pse, PostSecondarySectionContract contract, string user, DateTime updateTime)
        {
            // For colleges, we will update the existing records, but we don't have any similarity
            // checking so we don't restore.

            if (pse.DidAttendCollege.HasValue && pse.DidAttendCollege.Value)
            {
                // Grab all the repeater records from the database which normally includes the soft
                // deleted items... But not this time.
                var allRepeaterItems = pse.PostSecondaryColleges.ToList();

                // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.
                contract.PostSecondaryColleges = contract.PostSecondaryColleges.WithoutEmpties();

                // Next map any new items that are similar to existing/deleted items.
                //contract.....UpdateNewItemsIfSimilarToExisting(..., ....AdoptIfSimilarToModel);

                // Get the Id's of the WorkPrograms that are not new.
                var ids = (from x in contract.PostSecondaryColleges where x.Id != 0 select x.Id).ToList();

                // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
                // to mark the unused items as deleted.
                allRepeaterItems.MarkUnusedItemsAsDeleted(ids);

                foreach (var college in contract.PostSecondaryColleges)
                {
                    var psc = college.IsNew() ? Repo.NewCollege(pse, user) : allRepeaterItems.SingleOrDefault(x => x.Id == college.Id);

                    if (psc == null)
                    {
                        throw new Exception("PostSecondaryCollege could not be created or found by its ID.");
                    }

                    var sce = Repo.SchoolById(psc.SchoolCollegeEstablishmentId) ?? Repo.NewSchoolByPostSecondaryEducation(psc, user);
                    sce.Name   = college.Name?.Trim();
                    sce.Street = college.Location?.FullAddress?.Trim();

                    // If the location is null the name should be nulled because the name list is generated from the Google API from the location
                    if (college.Location != null)
                    {
                        sce.City = Repo.GetOrCreateCity(college.Location, _googleApi.GetPlaceDetails, _googleApi.GetLatLong, user);
                    }
                    else
                    {
                        sce.City = null;
                        sce.Name = null;
                    }

                    sce.ModifiedDate               = updateTime;
                    sce.ModifiedBy                 = user;
                    psc.SchoolCollegeEstablishment = sce;
                    psc.HasGraduated               = college.HasGraduated;
                    psc.CurrentlyAttending         = college.IsCurrentlyAttending;

                    if (college.IsCurrentlyAttending.HasValue && college.IsCurrentlyAttending.Value)
                    {
                        psc.LastYearAttended = null;
                    }
                    else
                    {
                        psc.LastYearAttended = college.LastYearAttended;
                    }

                    psc.Semesters = college.Semesters;
                    psc.Credits   = college.Credits;
                    psc.Details   = college.Details;
                }

                pse.DoesHaveDegrees = contract.HasDegree;

                // Grab all the repeater records from the database which normally includes the soft
                // deleted items... But not this time.
                var allPostSecDegrees = pse.AllPostSecondaryDegrees.ToList();

                // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.
                contract.PostSecondaryDegrees = contract.PostSecondaryDegrees.WithoutEmpties();

                // Next map any new items that are similar to existing/deleted items.
                //contract.....UpdateNewItemsIfSimilarToExisting(..., ....AdoptIfSimilarToModel);
                contract.PostSecondaryDegrees.UpdateNewItemsIfSimilarToExisting(allPostSecDegrees, PostSecondaryDegreeContract.AdoptIfSimilarToModel);

                // Get the Id's of the WorkPrograms that are not new.
                var psdIds = (from x in contract.PostSecondaryDegrees where x.Id != 0 select x.Id).ToList();

                // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
                // to mark the unused items as deleted.
                allPostSecDegrees.MarkUnusedItemsAsDeleted(psdIds);

                if (pse.DoesHaveDegrees.HasValue && pse.DoesHaveDegrees.Value)
                {
                    foreach (var degree in contract.PostSecondaryDegrees)
                    {
                        var psd = degree.IsNew() ? Repo.NewDegree(pse, user) : allPostSecDegrees.SingleOrDefault(x => x.Id == degree.Id);

                        if (psd == null)
                        {
                            throw new Exception("PostSecondaryDegree could not be created or found by its ID.");
                        }

                        psd.Name         = degree.Name;
                        psd.DegreeTypeId = degree.Type;
                        psd.College      = degree.College;
                        psd.YearAttained = degree.YearAttained;
                        psd.ModifiedBy   = user;
                        psd.ModifiedDate = updateTime;
                    }
                }
            }
            else
            {
                // This else captures the case where they selected No they nvered attended, or are you currently 
                // attending a college or university, so in that case we delete everything.
                foreach (var college in pse.PostSecondaryColleges)
                {
                    college.IsDeleted = true;
                }

                foreach (var x in pse.PostSecondaryDegrees)
                {
                    x.IsDeleted = true;
                }

                pse.DoesHaveDegrees = null;
            }
        }

        private void ApplySortOrderForDegrees(IPostSecondaryEducationSection pse)
        {
            var i = 0;

            foreach (var psc in pse.PostSecondaryColleges)
            {
                i++;
                psc.SortOrder = i;
            }
        }

        private void CheckLicenseOrCertificatesInfo(IPostSecondaryEducationSection pse, PostSecondarySectionContract contract, string user, DateTime updateTime)
        {
            if (pse.IsWorkingOnLicensesOrCertificates.HasValue && pse.IsWorkingOnLicensesOrCertificates.Value)
            {
                // Grab all the repeater records from the database.
                var allPostSecLicenses = pse.AllPostSecondaryLicenses.ToList();

                // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.
                contract.PostSecondaryLicenses = contract.PostSecondaryLicenses.WithoutEmpties();

                // Next map any new items that are similar to existing/deleted items.
                contract.PostSecondaryLicenses.UpdateNewItemsIfSimilarToExisting(allPostSecLicenses, PostSecondaryLicenseContract.AdoptIfSimilarToModel);

                // Get the Id's of the WorkPrograms that are not new.
                var ids = (from x in contract.PostSecondaryLicenses where x.Id != 0 select x.Id).ToList();

                // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
                // to mark the unused items as deleted.
                allPostSecLicenses.MarkUnusedItemsAsDeleted(ids);

                foreach (var license in contract.PostSecondaryLicenses)
                {
                    var psl = license.IsNew() ? Repo.NewLicense(pse, user) : allPostSecLicenses.SingleOrDefault(x => x.Id == license.Id);

                    if (psl == null)
                    {
                        throw new Exception("PostSecondaryLicense could not be created or found by its ID.");
                    }

                    psl.LicenseTypeId          = license.LicenseType;
                    psl.Name                   = license.Name;
                    psl.ValidInWIPolarLookupId = license.PolarLookupId;
                    psl.Issuer                 = license.Issuer;
                    psl.ExpiredDate            = license.ExpiredDate?.ToDateTimeMonthYear();
                    psl.DoesNotExpire          = license.DoesNotExpire;
                    psl.AttainedDate           = license.AttainedDate?.ToDateTimeMonthYear();
                    psl.IsInProgress           = license.IsInProgress;
                }
            }
            else
            {
                foreach (var x in pse.PostSecondaryLicenses)
                {
                    x.IsDeleted = true;
                }
            }
        }

        public static PostSecondarySectionContract GetContract(IInformalAssessment informalAssessment, IParticipant participant)
        {
            var contract = new PostSecondarySectionContract();

            if (informalAssessment != null && participant != null)
            {
                if (participant.PostSecondaryEducationSection != null)
                {
                    var pse = participant.PostSecondaryEducationSection;

                    contract.RowVersion         = pse.RowVersion;
                    contract.ModifiedDate       = pse.ModifiedDate;
                    contract.ModifiedBy         = pse.ModifiedBy;
                    contract.HasAttendedCollege = pse.DidAttendCollege;

                    if (pse.DidAttendCollege.HasValue && pse.DidAttendCollege.Value)
                    {
                        var postsecondarycolleges = new List<PostSecondaryCollegeContract>();

                        foreach (var c in pse.PostSecondaryColleges)
                        {
                            var college = new PostSecondaryCollegeContract();

                            if (c.SchoolCollegeEstablishment != null)
                            {
                                college.Location = LocationHelper.GetLocationInfo(c.SchoolCollegeEstablishment, c.SchoolCollegeEstablishment?.City);
                            }
                            else
                            {
                                college.Location = new LocationContract();
                            }

                            college.Id                   = c.Id;
                            college.Name                 = c.SchoolCollegeEstablishment?.Name.SafeTrim();
                            college.HasGraduated         = c.HasGraduated;
                            college.LastYearAttended     = c.LastYearAttended;
                            college.Semesters            = c.Semesters;
                            college.Credits              = c.Credits;
                            college.Details              = c.Details;
                            college.IsCurrentlyAttending = c.CurrentlyAttending;

                            postsecondarycolleges.Add(college);
                        }

                        contract.PostSecondaryColleges = postsecondarycolleges;
                    }
                    else
                    {
                        // If Participant did not go to college; can't have a degree.
                        pse.DoesHaveDegrees = null;
                        var colleges = new List<PostSecondaryCollegeContract>();
                        contract.PostSecondaryColleges = colleges;
                    }

                    contract.HasDegree = pse.DoesHaveDegrees;

                    if (pse.DoesHaveDegrees.HasValue && pse.DoesHaveDegrees.Value)
                    {
                        var postsecondaydegrees = new List<PostSecondaryDegreeContract>();

                        foreach (var d in pse.PostSecondaryDegrees)
                        {
                            var degree = new PostSecondaryDegreeContract();
                            degree.Id   = d.Id;
                            degree.Name = d.Name;
                            //TODO: Database needs sort order for this lookup
                            degree.Type         = d.DegreeType?.Id;
                            degree.TypeName     = d.DegreeType?.Name?.SafeTrim();
                            degree.College      = d.College.SafeTrim();
                            degree.YearAttained = d.YearAttained;
                            postsecondaydegrees.Add(degree);
                        }

                        contract.PostSecondaryDegrees = postsecondaydegrees;
                    }
                    else
                    {
                        var degrees = new List<PostSecondaryDegreeContract>();
                        contract.PostSecondaryDegrees = degrees;
                    }

                    contract.IsWorkingOnLicensesOrCertificates = pse.IsWorkingOnLicensesOrCertificates;

                    if (pse.IsWorkingOnLicensesOrCertificates.HasValue && pse.IsWorkingOnLicensesOrCertificates.Value)
                    {
                        var postsecondarylicenses = new List<PostSecondaryLicenseContract>();

                        foreach (var l in pse.PostSecondaryLicenses)
                        {
                            var license = new PostSecondaryLicenseContract();
                            license.Id              = l.Id;
                            license.LicenseType     = l.LicenseTypeId;
                            license.LicenseTypeName = l.LicenseType?.Name?.SafeTrim();
                            license.Name            = l.Name;
                            license.PolarLookupId   = l.ValidInWIPolarLookupId;
                            license.ValidInWi       = l.PolarLookup?.Name?.SafeTrim();
                            license.Issuer          = l.Issuer;

                            // Radio Button for no expiration.
                            license.DoesNotExpire = l.DoesNotExpire;

                            // Radio Button for in progress.
                            license.IsInProgress = l.IsInProgress;

                            if (l.IsInProgress.HasValue && l.IsInProgress.Value)
                            {
                                license.AttainedDate = null;
                                license.IsInProgress = true;
                            }
                            else
                            {
                                license.AttainedDate = l.AttainedDate?.ToString("MM/yyyy");
                                license.IsInProgress = null;
                            }

                            if (l.DoesNotExpire.HasValue && l.DoesNotExpire.Value)
                            {
                                license.ExpiredDate   = null;
                                license.DoesNotExpire = true;
                            }
                            else
                            {
                                license.ExpiredDate   = l.ExpiredDate?.ToString("MM/yyyy");
                                license.DoesNotExpire = null;
                            }

                            postsecondarylicenses.Add(license);
                        }

                        contract.PostSecondaryLicenses = postsecondarylicenses;
                    }
                    else
                    {
                        var licenses = new List<PostSecondaryLicenseContract>();
                        contract.PostSecondaryLicenses = licenses;
                    }

                    contract.PostSecondaryNotes = pse.Notes;
                }
                else
                {
                    var colleges = new List<PostSecondaryCollegeContract>();
                    contract.PostSecondaryColleges = colleges;
                    var degrees = new List<PostSecondaryDegreeContract>();
                    contract.PostSecondaryDegrees = degrees;
                    var licenses = new List<PostSecondaryLicenseContract>();
                    contract.PostSecondaryLicenses = licenses;
                }

                if (informalAssessment.PostSecondaryEducationAssessmentSection == null)
                {
                    return contract;
                }

                contract.IsSubmittedViaDriverFlow = true;
                contract.AssessmentRowVersion     = informalAssessment.PostSecondaryEducationAssessmentSection.RowVersion;
            }

            return contract;
        }

        #endregion
    }
}

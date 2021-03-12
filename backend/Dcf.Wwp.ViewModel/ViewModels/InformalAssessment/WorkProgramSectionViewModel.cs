using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Cww;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class WorkProgramSectionViewModel : BaseInformalAssessmentViewModel
    {
        #region Properties

        private readonly IGoogleApi _googleApi;

        #endregion

        #region Methods

        public WorkProgramSectionViewModel(IGoogleApi googleApi, IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
            _googleApi = googleApi;
        }

        public WorkProgramSectionContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }

        public static WorkProgramSectionContract GetContract(IInformalAssessment ia, IParticipant participant, IRepository repo)
        {
            var contract = new WorkProgramSectionContract();

            if (participant == null) return contract;

            // CWW reference data display for FSET Status.
            contract.CwwFsetStatus = GetCwwFsetStatus(participant, repo);

            if (participant.WorkProgramSection != null)
            {
                var wp = participant.WorkProgramSection;

                contract.RowVersion   = wp.RowVersion;
                contract.ModifiedDate = wp.ModifiedDate;
                contract.ModifiedBy   = wp.ModifiedBy;

                contract.WorkPrograms = new List<WorkProgramContract>();

                wp.InvolvedWorkPrograms?.ForEach(i =>
                                                 {
                                                     var w = new WorkProgramContract
                                                             {
                                                                 Id              = i.Id,
                                                                 WorkStatus      = i.WorkProgramStatusId,
                                                                 WorkStatusName  = i.WorkProgramStatus?.Name,
                                                                 WorkProgram     = i.WorkProgramId,
                                                                 WorkProgramName = i.WorkProgram?.Name,
                                                                 StartDate       = i.StartMonth?.ToString("MM/yyyy"),
                                                                 EndDate         = i.EndMonth?.ToString("MM/yyyy"),
                                                                 ContactId       = i.ContactId,
                                                                 Details         = i.Details,
                                                                 RowVersion      = i.RowVersion,
                                                                 Location        = LocationHelper.GetLocationInfo(i, i.City)
                                                             };


                                                     contract.WorkPrograms.Add(w);
                                                 });

                contract.IsInOtherPrograms = wp.IsInOtherPrograms;
                contract.Notes             = wp.Notes;
            }

            // We look at the assessment section now which at this point just
            // indicates it was submitted via the driver flow.
            if (ia.WorkProgramAssessmentSection != null)
            {
                contract.AssessmentRowVersion     = ia.WorkProgramAssessmentSection.RowVersion;
                contract.IsSubmittedViaDriverFlow = true;
            }

            return contract;
        }

        public bool PostData(WorkProgramSectionContract contract, string user)
        {
            var p = Participant;

            if (p == null)
                throw new InvalidOperationException("PIN not valid.");

            if (contract == null)
                throw new InvalidOperationException("Work Program data is missing.");

            IWorkProgramSection           wps  = null;
            IWorkProgramAssessmentSection wpas = null;

            var updateTime = DateTime.Now;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (p.InProgressInformalAssessment != null)
            {
                wpas = p.InProgressInformalAssessment.WorkProgramAssessmentSection ?? Repo.NewWorkProgramAssessmentSection(p.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(wpas);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                wpas.ReviewCompleted = true;
                wpas.ModifiedBy      = AuthUser.Username;
                wpas.ModifiedDate    = updateTime;
            }

            wps              = p.WorkProgramSection ?? Repo.NewWorkProgramSection(p, user);
            wps.ModifiedBy   = AuthUser.Username;
            wps.ModifiedDate = updateTime;

            Repo.StartChangeTracking(wps);

            var userRowVersion       = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            // There are only two properties on the main section table.
            wps.Notes             = contract.Notes.SafeTrim();
            wps.IsInOtherPrograms = contract.IsInOtherPrograms;

            if (wps.IsInOtherPrograms.HasValue && wps.IsInOtherPrograms.Value == true)
            {
                // Grab all the repeater records from the database which includes the soft deleted item.
                var allWorkPrograms = wps.AllInvolvedWorkPrograms.ToList();

                // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.
                contract.WorkPrograms = contract.WorkPrograms.WithoutEmpties();

                // Next map any new items that are similar to existing/deleted items.
                contract.WorkPrograms.UpdateNewItemsIfSimilarToExisting(allWorkPrograms, WorkProgramContract.AdoptIfSimilarToModel);

                // Get the Id's of the WorkPrograms that are not new.
                var ids = (from x in contract.WorkPrograms where x.Id != 0 select x.Id).ToList();

                // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
                // to mark the unused items as deleted.
                allWorkPrograms.MarkUnusedItemsAsDeleted(ids);

                // Now update the database items with the posted model data.
                if (contract.WorkPrograms != null)
                {
                    foreach (var cwp in contract.WorkPrograms)
                    {
                        IInvolvedWorkProgram iwp;

                        if (cwp.IsNew())
                        {
                            iwp              = Repo.NewInvolvedWorkProgram(wps, user);
                            iwp.ModifiedDate = updateTime;
                            iwp.ModifiedBy   = user;
                        }
                        else
                        {
                            iwp = (from x in allWorkPrograms where x.Id == cwp.Id select x).SingleOrDefault();
                        }

                        Debug.Assert(iwp != null, "IInvolvedWorkProgram should not be null.");

                        iwp.WorkProgramStatusId = cwp.WorkStatus;
                        iwp.WorkProgramId       = cwp.WorkProgram;
                        iwp.StartMonth          = cwp.StartDate.ToDateTimeMonthYear();
                        iwp.EndMonth            = cwp.EndDate.ToDateTimeMonthYear();
                        iwp.ContactId           = cwp.ContactId;
                        iwp.Details             = cwp.Details;

                        // See if we have a City location.
                        if (cwp.Location != null && !cwp.Location.IsEmpty())
                        {
                            iwp.City = Repo.GetOrCreateCity(cwp.Location, _googleApi.GetPlaceDetails, _googleApi.GetLatLong, user);
                            // In case we have a city that was deleted, we need to restore it.
                            iwp.City.IsDeleted = false;
                        }
                        else
                        {
                            iwp.CityId = null;
                        }

                        // Always reset the soft delete.
                        iwp.IsDeleted = false;
                    }
                }
            }
            else
            {
                // Since the indication is there is no other programs, clear out any
                // previous values.
                foreach (var iwp in wps.InvolvedWorkPrograms)
                {
                    iwp.IsDeleted = true;
                }
            }

            /*
             
            /*
				When the Upsert is called, the section may be new or it
				may have already been created.
				
				We are going to have a set of properties on the section
				and (for some sections) multiple repeaters.

				To start off updating the section data, we need to start
				tracking if the section and it's data has changed.  We
				also need to grab the row version to assist in detecting
				a change.

				For each repeater, items will either have an Id or it will
				be set to 0 (indicating it is new).

				For new items in a repeater, they might actually need to
				be restored by clearing the IsDeleted property.  Otherwise
				they need to be newed up and added to the DB Context.
			 
            *
            if (contract.WorkPrograms != null && wps.InvolvedWorkPrograms != null)
            {
                foreach (var wps in contract.WorkPrograms)
                {
                    if (wps.Id != 0 && IsLocationNullOrEmpty(wps.Location) && string.IsNullOrEmpty(wps.Details) &&
                        string.IsNullOrEmpty(wps.StartDate) && string.IsNullOrEmpty(wps.EndDate) &&
                          wps.WorkProgram == null && wps.WorkStatus == null)
                    {
                        wps.Id = 0;
                    }
                }

                // Model list
                var involvedWorkProgramByIdModel = (from x in contract.WorkPrograms select x.Id).ToList();

                //Databaselist
                var involvedWorkProgramByIdDb = (from x in wps.InvolvedWorkPrograms select x.Id).ToList();

                // Here we soft delete WorkPrograms that have been deleted by
                // the user. First we create the list of workProgramsIDs that 
                // are missing from the upsert model. 
                DeleteFromList(wps.InvolvedWorkPrograms, involvedWorkProgramByIdModel, involvedWorkProgramByIdDb);

                #region Restoration of InvolvedWorkProgram
                var involvedWorkProgramsModel = contract.WorkPrograms;

                // List of deleted InvolvedWorkProgram lists from Database.   
                var deletedInvolvedWorkProgramDb = from x in wps.AllInvolvedWorkPrograms where x.IsDeleted == true select x;
                var involvedWorkProgramsRestoreList = new List<WorkProgramContract>();
                foreach (var d in deletedInvolvedWorkProgramDb)
                {
                    foreach (var m in involvedWorkProgramsModel)
                    {
                        if ((d.ContactId == m.ContactId || (!d.ContactId.HasValue && m.ContactId == 0)) &&      // We need to handle when values are null and 0
                            d.City?.Name == m.Location?.City &&
                            d.City?.State?.Code == m.Location?.State &&
                            d.City?.State?.Country?.Name == m.Location?.Country &&
                            d.Details == m.Details &&
                            d.WorkProgramStatusId == m.WorkStatus &&
                            d.WorkProgramId == m.WorkProgram &&
                            d.StartMonth == m.StartDate.ToDateTimeMonthYear() &&
                            d.EndMonth == m.EndDate.ToDateTimeMonthYear()
                            )
                        {
                            d.IsDeleted = false;
                            involvedWorkProgramsRestoreList.Add(m);
                        }
                    }
                }

                // We remove the items that have been restored so they aren't "processed"
                // later in the code.
                foreach (var r in involvedWorkProgramsRestoreList)
                {
                    contract.WorkPrograms.Remove(r);
                }

                #endregion
            }

            if (contract.IsInOtherPrograms == true && contract.WorkPrograms != null)
            {
                foreach (var w in contract.WorkPrograms)
                {
                    if (w.Id == 0 && IsLocationNullOrEmpty(w.Location) && string.IsNullOrEmpty(w.Details) &&
                        string.IsNullOrEmpty(w.StartDate) && string.IsNullOrEmpty(w.EndDate) && w.WorkProgram == null &&
                         w.WorkStatus == null)
                    {
                        continue;
                    }
                    //if (w.Details == null)
                    //{
                    //    var otherworkprogram = Repo.OtherProgram(w.WorkProgram);
                    //    if (otherworkprogram.Name == "Other")
                    //    {
                    //        throw new InvalidOperationException("If other is Selected Details cannot be Empty");
                    //    }
                    //}

                    // Validation of model.
                    //var startDate = w.StartDate;
                    //if (!String.IsNullOrEmpty(startDate) && startDate.Length != 7)
                    //{
                    //    throw new InvalidOperationException("start date is in wrong format.");
                    //}

                    //var dob = p.DateOfBirth;
                    //if (w.StartDate.ToDateTimeMonthYear() <= dob)
                    //{
                    //    throw new InvalidOperationException("Start date Attended cnnot be before date of birth.");
                    //}
                    //var endDate = w.EndDate;
                    //if (endDate != null && endDate.Length != 7)
                    //{
                    //    throw new InvalidOperationException("End date is in wrong format.");
                    //}

                    //if (w.EndDate.ToDateTimeMonthYear() <= dob)
                    //{
                    //    throw new InvalidOperationException("End date Attended cnnot be before date of birth.");
                    //}

                    IInvolvedWorkProgram iwp = null;
                    iwp = wps.InvolvedWorkPrograms?.SingleOrDefault(x => x.Id == w.Id && w.Id != 0);

                    if (iwp == null)
                    {
                        iwp = Repo.NewInvolvedWorkProgram(wps, user);
                    }
                    iwp.WorkProgramStatusId = w.WorkStatus;
                    iwp.WorkProgramId = w.WorkProgram;
                    iwp.StartMonth = w.StartDate.ToDateTimeMonthYear();
                    iwp.EndMonth = w.EndDate.ToDateTimeMonthYear();
                    iwp.ContactId = w.ContactId;
                    iwp.Details = w.Details;
                    //var workprogram = Repo.WorkProgramById(iwp.WorkProgramId);
                    //if (workprogram.Name
                    //== "Other")
                    //{

                    //}

                    iwp.ModifiedBy = user;
                    iwp.ModifiedDate = DateTime.Now;

                    if (w.Location?.City != null && w.Location?.Country == "United States")
                    {
                        var ci = Repo.CitybyName(w.Location?.City) ?? Repo.NewCity(iwp, user);
                        var st = Repo.StateByCode(w.Location.State) ?? Repo.NewState(ci, user);
                        var co = Repo.CountryByName(w.Location.Country) ?? Repo.NewCountry(st, user);
                        ci.Country = co;
                        st.Code = w.Location.State;
                        ci.Name = w.Location.City;
                        ci.Country.Name = w.Location.Country;
                        co.Name = w.Location.Country;
                        ci.GooglePlaceId = w.Location.GooglePlaceId;
                        if (w.Location.GooglePlaceId != null)
                        {
                            var coors = ExternalAPIs.GoogleViewModel.GetLatLong(w.Location.GooglePlaceId);
                            ci.LongitudeNumber = coors[1];
                            ci.LatitudeNumber = coors[0];
                        }
                        iwp.City = ci;

                        // Country is bound to city when there is no state.
                        if (w.Location.State != null)
                        {
                            iwp.City.State = st;
                            iwp.City.State.Country = co;
                        }
                        else
                        {
                            iwp.City.Country = co;
                        }
                    }
                    else
                    {
                        if (w.Location?.State == null && w.Location?.City != null)
                        {
                            var ci = Repo.CitybyName(w.Location?.City) ?? Repo.NewCity(iwp, user);
                            var co = Repo.CountryByName(w.Location?.Country) ?? Repo.NewCountry(ci, user);
                            co.Name = w.Location?.Country;
                            ci.Country = co;
                            ci.Name = w.Location?.City;
                            ci.GooglePlaceId = w.Location?.GooglePlaceId;
                            if (w.Location.GooglePlaceId != null)
                            {
                                var coors = ExternalAPIs.GoogleViewModel.GetLatLong(w.Location.GooglePlaceId);
                                ci.LongitudeNumber = coors[1];
                                ci.LatitudeNumber = coors[0];
                            }
                            iwp.City = ci;
                        }
                        else
                        {
                            if (w.Location?.City != null)
                            {
                                var ci = Repo.CitybyName(w.Location?.City) ?? Repo.NewCity(iwp, user);
                                var st = Repo.StateByCode(w.Location?.State) ?? Repo.NewState(ci, user);
                                var co = Repo.CountryByName(w.Location?.Country) ?? Repo.NewCountry(st, user);
                                st.Code = w.Location?.State;
                                co.Name = w.Location?.Country;
                                st.Country = co;
                                ci.Name = w.Location?.City;
                                ci.State = st;
                                ci.GooglePlaceId = w.Location?.GooglePlaceId;
                                if (w.Location?.GooglePlaceId != null)
                                {
                                    var coors = ExternalAPIs.GoogleViewModel.GetLatLong(w.Location?.GooglePlaceId);
                                    ci.LongitudeNumber = coors[1];
                                    ci.LatitudeNumber = coors[0];
                                }
                                iwp.City = ci;
                            }
                        }
                        if (w.Location?.City == null)
                        {
                            iwp.City = null;
                        }
                    }

                }
            }
            else
            {
                foreach (var i in wps.InvolvedWorkPrograms)
                {
                    i.IsDeleted = true;
                }
            }
            */

            // Set Sort Order of work programs.
            var o = 1;

            foreach (var w in wps.InvolvedWorkPrograms)
            {
                w.SortOrder = o++;
            }

            if (p.InProgressInformalAssessment != null)
            {
                var currentIA = Repo.GetMostRecentAssessment(p);
                currentIA.ModifiedBy   = AuthUser.Username;
                currentIA.ModifiedDate = updateTime;
            }

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(wps, userRowVersion))
                return false;

            if (!Repo.IsRowVersionStillCurrent(wpas, userAssessRowVersion))
                return false;

            // If the first save completes, it actually has already saved the AssessmentSection
            // object as well since they are on the save repository context.  But if the Section didn't
            // need saving, we still need to SaveIfChangd on the AssessmentSection.
            if (!Repo.SaveIfChanged(wps, user))
                Repo.SaveIfChanged(wpas, user);

            return true;
        }

        private static FsetStatus GetCwwFsetStatus(IParticipant participant, IRepository repo)
        {
            var fset = repo.CwwFsetStatus(participant.PinNumber.ToString());

            if (fset == null) return null;

            return new FsetStatus()
                   {
                       CurrentStatusCode       = fset.CurrentStatusCode,
                       EnrollmentDate          = fset.EnrollmentDate?.ToShortDateString(),
                       DisenrollmentDate       = fset.DisenrollmentDate?.ToShortDateString(),
                       DisenrollmentReasonCode = fset.DisenrollmentReasonCode
                   };
        }

        #endregion
    }
}

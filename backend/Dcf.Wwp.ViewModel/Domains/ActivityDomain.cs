using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DCF.Common;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using NRules;
using NRules.Fluent;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;
using IActivityContactBridgeRepository = Dcf.Wwp.DataAccess.Interfaces.IActivityContactBridgeRepository;
using IActivityRepository = Dcf.Wwp.DataAccess.Interfaces.IActivityRepository;
using IRuleReasonRepository = Dcf.Wwp.DataAccess.Interfaces.IRuleReasonRepository;
using IWorkerRepository = Dcf.Wwp.DataAccess.Interfaces.IWorkerRepository;
using NonSelfDirectedActivity = Dcf.Wwp.Api.Library.Contracts.NonSelfDirectedActivity;
using ParticipationStatus = Dcf.Wwp.Model.Interface.Constants.ParticipationStatus;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;
using RuleReason = Dcf.Wwp.Data.Sql.Model.RuleReason;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class ActivityDomain : IActivityDomain
    {
        #region Properties

        private readonly IActivityRepository                            _activityRepository;
        private readonly IActivityContactBridgeRepository               _activityContactBridgeRepository;
        private readonly IActivityLocationRepository                    _activityLocationRepository;
        private readonly IActivityScheduleRepository                    _activityScheduleRepository;
        private readonly IActivityScheduleFrequencyBridgeRepository     _activityScheduleFrequencyBridgeRepository;
        private readonly IEmployabilityPlanActivityBridgeRepository     _epaBridgeRepository;
        private readonly INonSelfDirectedActivityRepository             _nonSelfDirActivityRepo;
        private readonly IWorkerRepository                              _workerRepo;
        private readonly ILocations                                     _locations;
        private readonly IUnitOfWork                                    _unitOfWork;
        private readonly IAuthUser                                      _authUser;
        private readonly IGoogleApi                                     _googleApi;
        private readonly IRuleReasonRepository                          _ruleReasonRepository;
        private readonly IParticipationStatusRepository                 _participationStatusRepository;
        private readonly IEmployabilityPlanRepository                   _epRepository;
        private readonly IParticipationStatusRepository                 _psRepository;
        private readonly IEnrolledProgramEPActivityTypeBridgeRepository _epatBridgeRepository;
        private readonly IParticipantDomain                             _participantDomain;
        private readonly ITransactionDomain                             _transactionDomain;
        private readonly IPOPClaimDomain                                _popClaimDomain;
        private readonly ICityDomain                                    _cityDomain;

        private readonly Func<string, string> _convertWIUIdToName;

        #endregion

        #region Methods

        public ActivityDomain(IActivityRepository                            activityRepository,
                              IActivityContactBridgeRepository               activityContactBridgeRepository,
                              IActivityLocationRepository                    activityLocationRepository,
                              IActivityScheduleFrequencyBridgeRepository     activityScheduleFrequencyBridgeRepository,
                              IActivityScheduleRepository                    activityScheduleRepository,
                              IEmployabilityPlanActivityBridgeRepository     epaBridgeRepository,
                              INonSelfDirectedActivityRepository             nonSelfDirActivityRepo,
                              IWorkerRepository                              workerRepo,
                              IUnitOfWork                                    unitOfWork,
                              ILocations                                     locations,
                              IAuthUser                                      authUser,
                              IGoogleApi                                     googleApi,
                              IRuleReasonRepository                          ruleReasonRepository,
                              IParticipationStatusRepository                 participationStatusRepository,
                              IEmployabilityPlanRepository                   epRepository,
                              IParticipationStatusRepository                 psRepository,
                              IEnrolledProgramEPActivityTypeBridgeRepository epatBridgeRepository,
                              IParticipantDomain                             participantDomain,
                              ITransactionDomain                             transactionDomain,
                              IPOPClaimDomain                                popClaimDomain,
                              ICityDomain                                    cityDomain)
        {
            _activityRepository                        = activityRepository;
            _activityContactBridgeRepository           = activityContactBridgeRepository;
            _activityLocationRepository                = activityLocationRepository;
            _activityScheduleFrequencyBridgeRepository = activityScheduleFrequencyBridgeRepository;
            _activityScheduleRepository                = activityScheduleRepository;
            _epaBridgeRepository                       = epaBridgeRepository;
            _nonSelfDirActivityRepo                    = nonSelfDirActivityRepo;
            _workerRepo                                = workerRepo;
            _unitOfWork                                = unitOfWork;
            _locations                                 = locations;
            _authUser                                  = authUser;
            _googleApi                                 = googleApi;
            _ruleReasonRepository                      = ruleReasonRepository;
            _participationStatusRepository             = participationStatusRepository;
            _epRepository                              = epRepository;
            _psRepository                              = psRepository;
            _epatBridgeRepository                      = epatBridgeRepository;
            _participantDomain                         = participantDomain;
            _transactionDomain                         = transactionDomain;
            _popClaimDomain                            = popClaimDomain;
            _cityDomain                                = cityDomain;

            _convertWIUIdToName = (wiuId) =>
                                  {
                                      var wo = workerRepo.GetAsQueryable()
                                                         .Where(i => i.WIUId == wiuId)
                                                         .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                         .FirstOrDefault();

                                      var wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");

                                      return (wn);
                                  };
        }

        public EmployabilityPlan GetRecentSubmittedEP(string pin, int epId, int enrolledProgramId, bool considerEnded = false)
        {
            var pinNo      = decimal.Parse(pin);
            var epStatuses = new List<string> { EmployabilityPlanStatus.Submitted };

            if (considerEnded)
                epStatuses.Add(EmployabilityPlanStatus.Ended);

            var recentSubmittedEP = _epRepository.GetMany(ep => ep.Participant.PinNumber == pinNo && ep.Id != epId && !ep.IsDeleted
                                                                && (ep.EnrolledProgram.ProgramCode.Trim() == EnrolledProgram.W2ProgramCode
                                                                        ? EnrolledProgram.WW
                                                                        : ep.EnrolledProgramId) == (enrolledProgramId    >= EnrolledProgram.WWC
                                                                                                    && enrolledProgramId <= EnrolledProgram.WWZ
                                                                                                        ? EnrolledProgram.WW
                                                                                                        : enrolledProgramId)
                                                                && epStatuses.Contains(ep.EmployabilityPlanStatusType.Name))
                                                 .OrderByDescending(i => i.BeginDate)
                                                 .FirstOrDefault();

            return recentSubmittedEP;
        }

        public List<EventsContract> GetEvents(string pin, int epId, int programId, DateTime epBeginDate)
        {
            var subEp   = GetRecentSubmittedEP(pin, epId, programId, true);
            var subEpId = subEp?.BeginDate != epBeginDate ? subEp?.Id : null;
            var parms = new Dictionary<string, object>
                        {
                            ["EPId"]           = epId,
                            ["SubsequentEPId"] = subEpId ?? (object) DBNull.Value,
                            ["ProgramCd"]      = DBNull.Value,
                            ["Consider15th"]   = false
                        };

            var epSchedule = _activityRepository.ExecStoredProc<USP_Create_Events>("USP_Create_Events", parms);

            var contracts = epSchedule.Select(i =>
                                              {
                                                  var evStarTime = i.StartTime.GetHourMinuteAndAMPM();
                                                  var evEndTime  = i.EndTime.GetHourMinuteAndAMPM();

                                                  return new EventsContract
                                                         {
                                                             Id                  = i.Id,
                                                             Title               = i.Title,
                                                             Description         = i.Description,
                                                             Type                = i.Type,
                                                             EndDate             = i.IsRecurring == false ? i.PlannedEndDate : null,
                                                             StartDate           = i.StartDate,
                                                             Hours               = i.HoursPerDay?.ToString(),
                                                             EndTime             = evEndTime,
                                                             StartTime           = evStarTime,
                                                             EmployabilityPlanId = i.EmployabilityPlanId
                                                         };
                                              }).ToList();

            return contracts;
        }

        public List<ActivityContract> GetActivitiesForEp(int epId)
        {
            var contracts = new List<ActivityContract>();

            var activities = _epaBridgeRepository.GetAsQueryable()
                                                 .Where(i => i.EmployabilityPlanId == epId && i.IsDeleted == false)
                                                 .Select(i => i.Activity)
                                                 .Where(i => i.IsDeleted == false)
                                                 .ToList();

            activities.ForEach(a => contracts.Add(
                                                  new ActivityContract
                                                  {
                                                      Id                           = a.Id,
                                                      ActivityTypeId               = a.ActivityTypeId,
                                                      ActivityTypeName             = a.ActivityType.Name,
                                                      ActivityTypeCode             = a.ActivityType.Code,
                                                      ActivityLocationId           = a.ActivityLocationId,
                                                      ActivityLocationName         = a.ActivityLocation.Name,
                                                      Description                  = a.Description,
                                                      AdditionalInformation        = a.Details,
                                                      ModifiedBy                   = _convertWIUIdToName(a.ModifiedBy),
                                                      ModifiedDate                 = a.ModifiedDate,
                                                      EmployabilityPlanId          = epId,
                                                      ActivityCompletionReasonId   = a.ActivityCompletionReasonId,
                                                      ActivityCompletionReasonName = a.ActivityCompletionReason?.Name,
                                                      ActivityCompletionReasonCode = a.ActivityCompletionReason?.Code,
                                                      EndDate                      = a.EndDate?.ToString("MM/dd/yyyy"),
                                                      MinStartDate                 = a.StartDate?.ToString("MM/dd/yyyy"),
                                                      Contacts                     = a.ActivityContactBridges.Select(ac => ac.ContactId).ToList(),
                                                      IsCarriedOver                = _epaBridgeRepository.GetMany(i => i.ActivityId == a.Id).Count() > 1,
                                                      ActivitySchedules = a.ActivitySchedules.Where(i => i.EmployabilityPlanId == epId && !i.IsDeleted)
                                                                           .Select(activitySchedule => new ActivityScheduleContract
                                                                                                       {
                                                                                                           Id                  = activitySchedule.Id,
                                                                                                           ScheduleStartDate   = activitySchedule.StartDate?.ToString("MM/dd/yyyy"),
                                                                                                           IsRecurring         = activitySchedule.IsRecurring,
                                                                                                           FrequencyTypeId     = activitySchedule.FrequencyTypeId,
                                                                                                           FrequencyTypeName   = activitySchedule.FrequencyType?.Name,
                                                                                                           ScheduleEndDate     = activitySchedule.PlannedEndDate?.ToString("MM/dd/yyyy"),
                                                                                                           HoursPerDay         = activitySchedule.HoursPerDay?.ToString(),
                                                                                                           ActualEndDate       = activitySchedule.ActualEndDate?.ToString("MM/dd/yyyy"),
                                                                                                           BeginHour           = GetHour(activitySchedule.BeginTime   != null ? activitySchedule.BeginTime : null),
                                                                                                           BeginMinute         = GetMinute(activitySchedule.BeginTime != null ? activitySchedule.BeginTime : null),
                                                                                                           BeginAmPm           = GetAmPm(activitySchedule.BeginTime   != null ? activitySchedule.BeginTime : null),
                                                                                                           EndHour             = GetHour(activitySchedule.EndTime     != null ? activitySchedule.EndTime : null),
                                                                                                           EndMinute           = GetMinute(activitySchedule.EndTime   != null ? activitySchedule.EndTime : null),
                                                                                                           EndAmPm             = GetAmPm(activitySchedule.EndTime     != null ? activitySchedule.EndTime : null),
                                                                                                           EmployabilityPlanId = activitySchedule.EmployabilityPlanId,
                                                                                                           BeginTime           = activitySchedule.BeginTime,
                                                                                                           EndTime             = activitySchedule.EndTime,
                                                                                                           ActivityScheduleFrequencies = activitySchedule.ActivityScheduleFrequencyBridges
                                                                                                                                                         .Select(afb => new ActivityScheduleFrequencyContract
                                                                                                                                                                        {
                                                                                                                                                                            Id                 = afb.Id,
                                                                                                                                                                            ActivityScheduleId = afb.ActivityScheduleId,
                                                                                                                                                                            WKFrequencyId      = afb.WKFrequencyId,
                                                                                                                                                                            WKFrequencyName    = afb.WKFrequency?.Name,
                                                                                                                                                                            MRFrequencyId      = afb.MRFrequencyId,
                                                                                                                                                                            MRFrequencyName    = afb.MRFrequency?.Name,
                                                                                                                                                                            ShortName          = afb.WKFrequency?.ShortName
                                                                                                                                                                        }).ToList()
                                                                                                       }).ToList()
                                                  }
                                                 ));
            return (contracts);
        }

        public ActivityContract GetActivity(int id, int employabilityPlanId)
        {
            var activityContract = new ActivityContract();
            var sdaLocContract   = new LocationContract();

            var activity = _activityRepository.Get(a => a.Id == id && a.IsDeleted == false);
            var minEpId = activity.EmploybilityPlanActivityBridges
                                  .Where(epab => epab.ActivityId == id && epab.IsDeleted == false)
                                  .Select(epab => epab.EmployabilityPlanId).Min();

            var isCarriedOver = minEpId != employabilityPlanId && _epaBridgeRepository.GetMany(a => a.ActivityId == id).Count() > 1;

            if (activity != null)
            {
                var sda = activity.NonSelfDirectedActivities?.FirstOrDefault(asda => asda.ActivityId == id);
                if (sda != null)
                {
                    sdaLocContract = _locations.GetLocationInfo(sda, sda.City);
                }


                activityContract = new ActivityContract
                                   {
                                       Id                           = activity.Id,
                                       ActivityTypeId               = activity.ActivityTypeId,
                                       ActivityTypeName             = activity.ActivityType?.Name,
                                       ActivityTypeCode             = activity.ActivityType?.Code,
                                       ActivityLocationId           = activity.ActivityLocationId,
                                       ActivityLocationName         = activity.ActivityLocation?.Name,
                                       Description                  = activity.Description,
                                       ModifiedBy                   = _convertWIUIdToName(activity.ModifiedBy),
                                       ModifiedDate                 = activity.ModifiedDate,
                                       EmployabilityPlanId          = employabilityPlanId,
                                       AdditionalInformation        = activity.Details,
                                       ActivityCompletionReasonId   = activity.ActivityCompletionReasonId,
                                       ActivityCompletionReasonName = activity.ActivityCompletionReason?.Name,
                                       ActivityCompletionReasonCode = activity.ActivityCompletionReason?.Code,
                                       EndDate                      = activity.EndDate?.ToString("MM/dd/yyyy"),
                                       MinStartDate                 = activity.StartDate?.ToString("MM/dd/yyyy"),
                                       IsCarriedOver                = isCarriedOver,
                                       Contacts                     = new List<int?>(),
                                       NonSelfDirectedActivity = new NonSelfDirectedActivity
                                                                 {
                                                                     BusinessLocation      = sdaLocContract,
                                                                     BusinessName          = sda?.BusinessName,
                                                                     BusinessPhoneNumber   = sda?.PhoneNumber,
                                                                     BusinessStreetAddress = sda?.StreetAddress,
                                                                     BusinessZipAddress    = sda?.ZipAddress
                                                                 }
                                   };

                // We need to look at the ContactBridge table to get the list of contact
                // ID's that the user has previously chosen.
                if (activity.ActivityContactBridges != null)
                {
                    foreach (var contact in activity.ActivityContactBridges.Where(i => i.IsDeleted == false))
                    {
                        if (contact.ContactId.HasValue)
                        {
                            activityContract.Contacts.Add(contact.ContactId.Value);
                        }
                    }
                }

                activityContract.ActivitySchedules = new List<ActivityScheduleContract>();
                var ac = activity.ActivitySchedules?.AsQueryable();
                ac = ac?.Where(i => i.EmployabilityPlanId == employabilityPlanId && !i.IsDeleted);

                var activitySchedules = ac?.ToList();

                if (activitySchedules != null)
                {
                    foreach (var activitySchedule in activitySchedules)
                    {
                        var activityScheduleContract = new ActivityScheduleContract
                                                       {
                                                           Id                          = activitySchedule.Id,
                                                           ScheduleStartDate           = activitySchedule.StartDate?.ToString("MM/dd/yyyy"),
                                                           IsRecurring                 = activitySchedule.IsRecurring,
                                                           FrequencyTypeId             = activitySchedule.FrequencyTypeId,
                                                           FrequencyTypeName           = activitySchedule.FrequencyType?.Name,
                                                           ScheduleEndDate             = activitySchedule.PlannedEndDate?.ToString("MM/dd/yyyy"),
                                                           HoursPerDay                 = activitySchedule.HoursPerDay?.ToString(),
                                                           ActualEndDate               = activitySchedule.ActualEndDate?.ToString("MM/dd/yyyy"),
                                                           BeginHour                   = GetHour(activitySchedule.BeginTime   != null ? activitySchedule.BeginTime : null),
                                                           BeginMinute                 = GetMinute(activitySchedule.BeginTime != null ? activitySchedule.BeginTime : null),
                                                           BeginAmPm                   = GetAmPm(activitySchedule.BeginTime   != null ? activitySchedule.BeginTime : null),
                                                           EndHour                     = GetHour(activitySchedule.EndTime     != null ? activitySchedule.EndTime : null),
                                                           EndMinute                   = GetMinute(activitySchedule.EndTime   != null ? activitySchedule.EndTime : null),
                                                           EndAmPm                     = GetAmPm(activitySchedule.EndTime     != null ? activitySchedule.EndTime : null),
                                                           BeginTime                   = activitySchedule.BeginTime,
                                                           EndTime                     = activitySchedule.EndTime,
                                                           EmployabilityPlanId         = activitySchedule.EmployabilityPlanId,
                                                           ActivityScheduleFrequencies = new List<ActivityScheduleFrequencyContract>()
                                                       };

                        if (activityScheduleContract.IsRecurring == true)
                        {
                            var f = activitySchedule.ActivityScheduleFrequencyBridges.Where(i => !i.IsDeleted);

                            var frequencies = f.ToList();

                            foreach (var frequency in frequencies)
                            {
                                var frequencyContract = new ActivityScheduleFrequencyContract
                                                        {
                                                            Id                 = frequency.Id,
                                                            ActivityScheduleId = frequency.ActivityScheduleId,
                                                            WKFrequencyId      = frequency.WKFrequencyId,
                                                            WKFrequencyName    = frequency.WKFrequency?.Name,
                                                            MRFrequencyId      = frequency.MRFrequencyId,
                                                            MRFrequencyName    = frequency.MRFrequency?.Name,
                                                            ShortName          = frequency.WKFrequency.ShortName
                                                        };


                                activityScheduleContract.ActivityScheduleFrequencies?.Add(frequencyContract);
                            }
                        }

                        activityContract.ActivitySchedules?.Add(activityScheduleContract);
                    }
                }
            }
            else
            {
                return (null);
            }

            return (activityContract);
        }

        public List<ActivityContract> GetActivitiesForPin(string pin, bool fromEvents = false)
        {
            var decimalPin = decimal.Parse(pin);
            var activities = _epaBridgeRepository.GetAsQueryable()
                                                 .Where(i => i.EmployabilityPlan.Participant.PinNumber == decimalPin && i.IsDeleted == false)
                                                 .Select(i => i.Activity)
                                                 .Where(i => i.IsDeleted == false)
                                                 .Distinct()
                                                 .OrderByDescending(i => i.ActivityCompletionReasonId == null)
                                                 .ThenByDescending(i => i.EndDate)
                                                 .ToList();

            var contracts = (from a in activities
                             let epId = a.EmploybilityPlanActivityBridges.OrderByDescending(i => i.EmployabilityPlan.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                                         .ThenByDescending(i => i.EmployabilityPlan.BeginDate)
                                         .Where(i => i.EmployabilityPlan.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.SubmittedId || i.EmployabilityPlan.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.EndedId || i.EmployabilityPlan.EmployabilityPlanStatusTypeId == EmployabilityPlanStatus.InProgressId)
                                         .Select(i => i.EmployabilityPlanId)
                                         .FirstOrDefault()
                             select new ActivityContract
                                    {
                                        Id                           = a.Id,
                                        ActivityTypeId               = a.ActivityTypeId,
                                        ActivityTypeName             = a.ActivityType.Name,
                                        ActivityTypeCode             = a.ActivityType.Code,
                                        ActivityLocationId           = a.ActivityLocationId,
                                        ActivityLocationName         = a.ActivityLocation.Name,
                                        Description                  = a.Description,
                                        ModifiedBy                   = _convertWIUIdToName(a.ModifiedBy),
                                        ModifiedDate                 = a.ModifiedDate,
                                        ActivityCompletionReasonId   = a.ActivityCompletionReasonId,
                                        ActivityCompletionReasonName = a.ActivityCompletionReason?.Name,
                                        ActivityCompletionReasonCode = a.ActivityCompletionReason?.Code,
                                        EndDate                      = a.EndDate?.ToString("MM/dd/yyyy"),
                                        MinStartDate                 = a.StartDate?.ToString("MM/dd/yyyy"),
                                        IsCarriedOver                = _epaBridgeRepository.GetMany(i => i.ActivityId == a.Id).Count() > 1,
                                        Program                      = a.EmploybilityPlanActivityBridges.FirstOrDefault()?.EmployabilityPlan.EnrolledProgram.ShortName,
                                        EmployabilityPlanId          = epId,
                                        ActivitySchedules = a.ActivitySchedules.Where(i => fromEvents || (!fromEvents && i.EmployabilityPlanId == epId) && !i.IsDeleted)
                                                             .Select(activitySchedule => new ActivityScheduleContract
                                                                                         {
                                                                                             Id                  = activitySchedule.Id,
                                                                                             ScheduleStartDate   = activitySchedule.StartDate?.ToString("MM/dd/yyyy"),
                                                                                             IsRecurring         = activitySchedule.IsRecurring,
                                                                                             FrequencyTypeId     = activitySchedule.FrequencyTypeId,
                                                                                             FrequencyTypeName   = activitySchedule.FrequencyType?.Name,
                                                                                             ScheduleEndDate     = activitySchedule.PlannedEndDate?.ToString("MM/dd/yyyy"),
                                                                                             HoursPerDay         = activitySchedule.HoursPerDay?.ToString(),
                                                                                             ActualEndDate       = activitySchedule.ActualEndDate?.ToString("MM/dd/yyyy"),
                                                                                             BeginHour           = GetHour(activitySchedule.BeginTime   != null ? activitySchedule.BeginTime : null),
                                                                                             BeginMinute         = GetMinute(activitySchedule.BeginTime != null ? activitySchedule.BeginTime : null),
                                                                                             BeginAmPm           = GetAmPm(activitySchedule.BeginTime   != null ? activitySchedule.BeginTime : null),
                                                                                             EndHour             = GetHour(activitySchedule.EndTime     != null ? activitySchedule.EndTime : null),
                                                                                             EndMinute           = GetMinute(activitySchedule.EndTime   != null ? activitySchedule.EndTime : null),
                                                                                             EndAmPm             = GetAmPm(activitySchedule.EndTime     != null ? activitySchedule.EndTime : null),
                                                                                             BeginTime           = activitySchedule.BeginTime,
                                                                                             EndTime             = activitySchedule.EndTime,
                                                                                             EmployabilityPlanId = activitySchedule.EmployabilityPlanId,
                                                                                             ActivityScheduleFrequencies = activitySchedule.ActivityScheduleFrequencyBridges.Select(afb => new ActivityScheduleFrequencyContract
                                                                                                                                                                                           {
                                                                                                                                                                                               Id                 = afb.Id,
                                                                                                                                                                                               ActivityScheduleId = afb.ActivityScheduleId,
                                                                                                                                                                                               WKFrequencyId      = afb.WKFrequencyId,
                                                                                                                                                                                               WKFrequencyName    = afb.WKFrequency?.Name,
                                                                                                                                                                                               MRFrequencyId      = afb.MRFrequencyId,
                                                                                                                                                                                               MRFrequencyName    = afb.MRFrequency?.Name,
                                                                                                                                                                                               ShortName          = afb.WKFrequency?.ShortName
                                                                                                                                                                                           })
                                                                                                                                           .ToList()
                                                                                         })
                                                             .ToList()
                                    }).ToList();
            return contracts;
        }

        public List<ActivityContract> GetActivitiesForPep(string pin, int pepId)
        {
            var decimalPin = decimal.Parse(pin);
            var contracts  = new List<ActivityContract>();
            var activities = _epaBridgeRepository.GetAsQueryable()
                                                 .Where(i => i.EmployabilityPlan.Participant.PinNumber == decimalPin && i.EmployabilityPlan.ParticipantEnrolledProgramId == pepId && i.IsDeleted == false)
                                                 .Select(i => i.Activity)
                                                 .Where(i => i.IsDeleted == false)
                                                 .Distinct().ToList();

            activities.ForEach(a => contracts.Add(
                                                  new ActivityContract
                                                  {
                                                      Id                           = a.Id,
                                                      ActivityTypeId               = a.ActivityTypeId,
                                                      ActivityTypeName             = a.ActivityType.Name,
                                                      ActivityTypeCode             = a.ActivityType.Code,
                                                      Description                  = a.Description,
                                                      ActivityCompletionReasonId   = a.ActivityCompletionReasonId,
                                                      ActivityCompletionReasonName = a.ActivityCompletionReason?.Name,
                                                      ActivityCompletionReasonCode = a.ActivityCompletionReason?.Code,
                                                      EndDate                      = a.EndDate?.ToString("MM/dd/yyyy"),
                                                      Program                      = a.EmploybilityPlanActivityBridges.FirstOrDefault()?.EmployabilityPlan.EnrolledProgram.ShortName
                                                  }
                                                 ));
            return contracts;
        }

        public bool DeleteActivity(string pin, int id, int epId, bool fromEndEp)
        {
            var activity = _activityRepository.Get(a => a.Id == id);

            if (fromEndEp)
            {
                var epab = _epaBridgeRepository.GetMany(a => a.ActivityId == id);

                epab.ForEach(a =>
                             {
                                 a.IsDeleted = true;

                                 _epaBridgeRepository.Update(a);
                             });

                activity.IsDeleted = true;
                _activityRepository.Update(activity);
            }
            else
            {
                DeleteActivityAndSchedules(epId, activity);
            }

            var uow       = _unitOfWork.Commit();
            var isDeleted = (uow > 0);

            return (isDeleted);
        }

        public void DeleteActivityAndSchedules(int epId, Activity activity)
        {
            var id                  = activity.Id;
            var activityBridgeCount = _epaBridgeRepository.GetMany(a => a.ActivityId == id).Count();

            if (activityBridgeCount == 1)
                _activityRepository.Delete(a => a.Id == id);
            else
                if (activityBridgeCount > 1)
                    _epaBridgeRepository.Delete(i => i.EmployabilityPlanId == epId && i.ActivityId == id);

            var schedules = activity.ActivitySchedules.Where(i => i.EmployabilityPlanId == epId);
            _activityScheduleRepository.DeleteRange(schedules);
        }

        public PreSavingActivityContract PreSaveActivity(string pin, int epId, string activityTypeId)
        {
            var decimalPin = decimal.Parse(pin);
            var ep         = _epRepository.Get(i => i.Id == epId);
            var pep        = ep.ParticipantEnrolledProgram;
            var statuses   = _participationStatusRepository.GetMany(i => i.Participant.PinNumber == decimalPin && i.EnrolledProgramId == pep.EnrolledProgramId).ToList();
            var possibleRuleReasons = _ruleReasonRepository.GetMany(i => i.Category == Wwp.Model.Interface.Constants.RuleReason.Activity && i.SubCategory
                                                                         == Wwp.Model.Interface.Constants.RuleReason.PreCheckError).ToList();

            var ufActivityIds = _epatBridgeRepository.GetMany(i => i.IsUpfrontActivity == true && i.EnrolledProgram.ProgramCode == pep.EnrolledProgram.ProgramCode)
                                                     .Select(i => i.ActivityTypeId).ToList();
            var spPreCheck      = new USP_CheckDB2OpenPlacement_Result();
            var activityTypeIds = activityTypeId.Split(',').Select(int.Parse).ToList();
            var programCodes    = new List<string> { EnrolledProgram.W2ProgramCode, EnrolledProgram.LFProgramCode };

            if (!activityTypeId.IsNullOrEmpty() && activityTypeIds.Any(i => ufActivityIds.Contains(i)) && programCodes.Contains(ep.EnrolledProgram.ProgramCode.SafeTrim()))
            {
                var sqlParams = new Dictionary<string, object>
                                {
                                    ["CaseNumber"] = pep.CASENumber,
                                    ["PinNumber"]  = decimalPin
                                };

                spPreCheck = _activityRepository.ExecStoredProc<USP_CheckDB2OpenPlacement_Result>("USP_CheckDB2OpenPlacement", sqlParams).FirstOrDefault();
            }


            //ToDo: Better way to change RuleReason to IRuleReason
            var ruleReasons = new List<IRuleReason>();

            foreach (var possibleRuleReason in possibleRuleReasons)
            {
                var ruleReason = new RuleReason
                                 {
                                     Category    = possibleRuleReason.Category,
                                     SubCategory = possibleRuleReason.SubCategory,
                                     Name        = possibleRuleReason.Name,
                                     Code        = possibleRuleReason.Code
                                 };

                ruleReasons.Add(ruleReason);
            }

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = ruleReasons
                                         };

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(rule => rule.IsTagged("ACT")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            // Fire engine.
            session.Insert(messageCodeLevelResult);
            session.Insert(ep);
            session.Insert(statuses);
            session.Insert(spPreCheck);
            session.Fire();

            var preSavingActivityContract = new PreSavingActivityContract();

            foreach (var cml in messageCodeLevelResult.CodesAndMesssegesByLevel.AsNotNull())
            {
                switch (cml.Level)
                {
                    case CodeLevel.Error:
                        preSavingActivityContract.Errors?.Add(cml.Message);
                        break;
                    case CodeLevel.Warning:
                        preSavingActivityContract.Warnings?.Add(cml.Message);
                        break;
                }
            }

            // Do not allow save if there are any errors.
            preSavingActivityContract.CanSaveActivity = preSavingActivityContract.Errors?.Count == 0;

            return preSavingActivityContract;
        }

        public ActivityContract UpsertActivity(ActivityContract activityContract, string pin, int epId, int subepId)
        {
            if (activityContract == null)
            {
                throw new ArgumentNullException(nameof(activityContract));
            }

            var updateTime = DateTime.Now;
            var activity   = activityContract.Id != 0 ? _activityRepository.Get(g => g.Id == activityContract.Id && g.IsDeleted == false) : _activityRepository.New();

            if (activityContract.Id == 0)
            {
                var epab = new EmployabilityPlanActivityBridge
                           {
                               EmployabilityPlanId = epId,
                               IsDeleted           = false,
                               ModifiedBy          = _authUser.WIUID,
                               ModifiedDate        = updateTime
                           };
                activity.EmploybilityPlanActivityBridges.Add(epab);
            }

            var contractStartDate = activityContract.ActivitySchedules.Min(i => i.ScheduleStartDate).ToDateMonthDayYear();

            activity.ActivityTypeId     = activityContract.ActivityTypeId;
            activity.Description        = activityContract.Description;
            activity.ActivityLocationId = activityContract.ActivityLocationId;
            activity.StartDate          = activityContract.IsCarriedOver && activity.StartDate <= contractStartDate ? activity.StartDate : contractStartDate;

            var activityLocation = _activityLocationRepository.Get(i => i.Id == activityContract.ActivityLocationId);

            if (activityLocation.Name == "On-site" || activityLocation.Name == "Off-site")
            {
                var nonSelfDirectedActivity = _nonSelfDirActivityRepo.Get(sda => sda.ActivityId == activityContract.Id) ?? _nonSelfDirActivityRepo.New();
                nonSelfDirectedActivity.ActivityId   = activity.Id;
                nonSelfDirectedActivity.ModifiedDate = updateTime;
                nonSelfDirectedActivity.ModifiedBy   = _authUser.WIUID;
                nonSelfDirectedActivity.BusinessName = activityContract.NonSelfDirectedActivity?.BusinessName;

                if (activityContract.NonSelfDirectedActivity?.BusinessLocation?.City != null)
                {
                    nonSelfDirectedActivity.City = _cityDomain.GetOrCreateCity(activityContract.NonSelfDirectedActivity?.BusinessLocation, _googleApi.GetPlaceDetails, _googleApi.GetLatLong, _authUser.Username);
                }
                else
                {
                    nonSelfDirectedActivity.City   = null;
                    nonSelfDirectedActivity.CityId = null;
                }

                nonSelfDirectedActivity.PhoneNumber   = activityContract.NonSelfDirectedActivity?.BusinessPhoneNumber;
                nonSelfDirectedActivity.StreetAddress = activityContract.NonSelfDirectedActivity?.BusinessStreetAddress;
                nonSelfDirectedActivity.ZipAddress    = activityContract.NonSelfDirectedActivity?.BusinessZipAddress;

                activity.NonSelfDirectedActivities.Add(nonSelfDirectedActivity);
            }
            else
            {
                if (activity.NonSelfDirectedActivities.AsNotNull().Any())
                {
                    var sda = activity.NonSelfDirectedActivities.FirstOrDefault();
                    if (sda != null)
                    {
                        _nonSelfDirActivityRepo.Delete(i => i.Id == sda.Id);
                    }
                }
            }

            // This is a many to many relationship.
            activity.Details      = activityContract.AdditionalInformation;
            activity.ModifiedBy   = _authUser.WIUID;
            activity.ModifiedDate = updateTime;

            activity.ActivityContactBridges.ForEach(i => i.IsDeleted = true);

            if (activityContract.Contacts != null)
            {
                var contacts = activityContract.Contacts;

                if (contacts.Count != 0)
                {
                    foreach (var contact in contacts)
                    {
                        var restore = activity.ActivityContactBridges?.FirstOrDefault(i => i.ContactId == contact);

                        if (restore != null)
                        {
                            restore.ModifiedDate = DateTime.Now;
                            restore.ModifiedBy   = _authUser.WIUID;
                            restore.IsDeleted    = false;
                        }
                        else
                        {
                            ActivityContactBridge acb;
                            acb              = _activityContactBridgeRepository.New();
                            acb.ModifiedBy   = _authUser.WIUID;
                            acb.ModifiedDate = updateTime;
                            acb.ContactId    = contact;
                            activity.ActivityContactBridges?.Add(acb);
                        }
                    }
                }
            }

            var dbActivityScheduleIds       = _activityScheduleRepository.GetMany(i => i.ActivityId == activity.Id && i.EmployabilityPlanId == epId).Select(i => i.Id).ToList();
            var contractActivityScheduleIds = activityContract.ActivitySchedules?.Select(i => i.Id).ToList();

            List<int> deletedIds;

            if (contractActivityScheduleIds != null)
            {
                deletedIds = dbActivityScheduleIds.Except(contractActivityScheduleIds).ToList();
            }
            else
            {
                deletedIds = dbActivityScheduleIds.ToList();
            }

            foreach (var activityScheduleId in deletedIds)
            {
                _activityScheduleRepository.Delete(activitySchedule => activitySchedule.Id == activityScheduleId);
            }

            foreach (var activitySchedule in activityContract.ActivitySchedules.AsNotNull())
            {
                var dbActivityScheduleFrequencyIds       = _activityScheduleFrequencyBridgeRepository.GetMany(i => i.ActivityScheduleId == activitySchedule.Id).Select(i => i.Id).ToList();
                var contractActivityScheduleFrequencyIds = activitySchedule.ActivityScheduleFrequencies?.Select(i => i.Id).ToList();

                List<int> deletedFrequencyIds;

                if (contractActivityScheduleFrequencyIds != null)
                {
                    deletedFrequencyIds = dbActivityScheduleFrequencyIds.Except(contractActivityScheduleFrequencyIds).ToList();
                }
                else
                {
                    deletedFrequencyIds = dbActivityScheduleFrequencyIds.ToList();
                }

                foreach (var frequencyScheduleId in deletedFrequencyIds)
                {
                    _activityScheduleFrequencyBridgeRepository.Delete(asfb => asfb.Id == frequencyScheduleId);
                }

                var schedule = _activityScheduleRepository.Get(i => i.Id == activitySchedule.Id && i.EmployabilityPlanId == epId) ?? _activityScheduleRepository.New();

                schedule.ActivityId          = activity.Id;
                schedule.StartDate           = activitySchedule.ScheduleStartDate.ToDateTimeMonthDayYear();
                schedule.IsRecurring         = activitySchedule.IsRecurring;
                schedule.FrequencyTypeId     = activitySchedule.FrequencyTypeId;
                schedule.PlannedEndDate      = activitySchedule.ScheduleEndDate.ToDateTimeMonthDayYear();
                schedule.HoursPerDay         = (activitySchedule.HoursPerDay != null || activitySchedule.HoursPerDay != "") ? activitySchedule.HoursPerDay.ToDecimal() : null;
                schedule.ModifiedDate        = updateTime;
                schedule.ModifiedBy          = _authUser.WIUID;
                schedule.EmployabilityPlanId = epId;

                //Ending schedule in an In-Progress EP
                var currentDate = _authUser.CDODate ?? DateTime.Today;
                if (activityContract.ActivityCompletionReasonId != null && activityContract.EndDate?.ToDateTimeMonthDayYear() < currentDate)
                    schedule.ActualEndDate = activityContract.EndDate?.ToDateTimeMonthDayYear();

                //Unending schedule in an In-Progress EP
                if (activityContract.ActivityCompletionReasonId == null && schedule.ActualEndDate != null)
                    schedule.ActualEndDate = null;

                if (activitySchedule.BeginHour != null)
                {
                    string beginAmPm = null;
                    if (activitySchedule.BeginAmPm != null)
                        beginAmPm = activitySchedule.BeginAmPm == 1 ? "AM" : "PM";

                    var beginTime = GetHourMinute(activitySchedule.BeginHour.ToString(), activitySchedule.BeginMinute.ToString(), beginAmPm);
                    schedule.BeginTime = beginTime;
                }

                if (activitySchedule.EndHour != null)
                {
                    string endAmPm = null;
                    if (activitySchedule.EndAmPm != null)
                        endAmPm = activitySchedule.EndAmPm == 1 ? "AM" : "PM";

                    var endTime = GetHourMinute(activitySchedule.EndHour.ToString(), activitySchedule.EndMinute.ToString(), endAmPm);
                    schedule.EndTime = endTime;
                }

                if (activitySchedule.BeginHour == null && activitySchedule.EndHour == null)
                {
                    schedule.BeginTime = null;
                    schedule.EndTime   = null;
                }

                if (activitySchedule.IsRecurring == true && activitySchedule.FrequencyTypeName != "Daily – Every Weekday")
                {
                    foreach (var scheduleFrequency in activitySchedule.ActivityScheduleFrequencies)
                    {
                        ActivityScheduleFrequencyBridge frequencyBridge;

                        if (scheduleFrequency.Id == 0)
                        {
                            frequencyBridge                    = _activityScheduleFrequencyBridgeRepository.New();
                            frequencyBridge.ActivityScheduleId = schedule.Id;
                        }
                        else
                        {
                            frequencyBridge = _activityScheduleFrequencyBridgeRepository.Get(asfb => asfb.Id == scheduleFrequency.Id);
                        }

                        frequencyBridge.ModifiedBy   = _authUser.WIUID;
                        frequencyBridge.ModifiedDate = updateTime;

                        if (activitySchedule.FrequencyTypeName == "Weekly" || activitySchedule.FrequencyTypeName == "Biweekly")
                        {
                            frequencyBridge.WKFrequencyId = scheduleFrequency.WKFrequencyId;
                            frequencyBridge.ModifiedBy    = _authUser.WIUID;
                            frequencyBridge.ModifiedDate  = updateTime;
                            schedule.ActivityScheduleFrequencyBridges?.Add(frequencyBridge);
                        }
                        else
                        {
                            if (activitySchedule.FrequencyTypeName == "Monthly")
                            {
                                frequencyBridge.WKFrequencyId = scheduleFrequency.WKFrequencyId;
                                frequencyBridge.MRFrequencyId = scheduleFrequency.MRFrequencyId;
                                frequencyBridge.ModifiedBy    = _authUser.WIUID;
                                frequencyBridge.ModifiedDate  = updateTime;
                                schedule.ActivityScheduleFrequencyBridges?.Add(frequencyBridge);
                            }
                        }
                    }
                }

                if (activitySchedule.Id == 0)
                    activity.ActivitySchedules.Add(schedule);
            }

            //Ending Activity on In-Progress EP
            if (activityContract.ActivityCompletionReasonId != null)
                activity = EndActivity(activityContract, activity, epId, subepId, updateTime);

            //Unending Activity on In-Progress EP
            if (activityContract.ActivityCompletionReasonId == null)
                activity = UnEndActivity(activity);

            if (activityContract.Id == 0)
                _activityRepository.Add(activity);

            _unitOfWork.Commit();
            return activityContract;
        }


        public List<ActivityContract> UpsertElapsedActivity(List<ActivityContract> activityContracts, string pin, int epId, bool isDisenrollment, bool fromEpOverView)
        {
            if (activityContracts == null || !activityContracts.Any())
            {
                throw new ArgumentNullException(nameof(activityContracts));
            }

            var updateTime                   = DateTime.Now;
            var canInsertVocationalPopClaim  = true;
            var canInsertEducationalPopClaim = true;
            var workerId                     = _workerRepo.Get(x => x.WIUId == _authUser.WIUID).Id;
            var xml = new XElement("Activities", activityContracts.Select(a => new XElement("Activity", new XElement("EPId", epId),
                                                                                            new XElement("ActivityId",       a.Id),
                                                                                            new XElement("EndDate",          a.EndDate)))).ToString();

            foreach (var activityContract in activityContracts)
            {
                var activity    = _activityRepository.Get(g => g.Id == activityContract.Id && g.IsDeleted == false);
                var schedules   = activity.ActivitySchedules.AsNotNull().ToList();
                var delSchedule = schedules.FirstOrDefault(i => i.ActivityId == activity.Id && i.EmployabilityPlanId == epId);

                activity.ActivityCompletionReasonId = activityContract.ActivityCompletionReasonId;
                activity.EndDate                    = activityContract.EndDate.ToDateTimeMonthDayYear();

                if (activityContract.IsCarriedOver && activity.EmploybilityPlanActivityBridges.Count > 1 && (isDisenrollment || !fromEpOverView))
                {
                    _epaBridgeRepository.Delete(activity.EmploybilityPlanActivityBridges.FirstOrDefault(i => i.EmployabilityPlanId == epId));

                    if (delSchedule != null)
                        _activityScheduleRepository.Delete(delSchedule);
                }

                schedules.ForEach(i =>
                                  {
                                      var endDate = i.IsRecurring == true ? i.PlannedEndDate : i.StartDate;

                                      i.ActualEndDate = endDate;
                                      i.ModifiedDate  = updateTime;
                                      i.ModifiedBy    = _authUser.WIUID;
                                  });

                activity.ModifiedDate = updateTime;
                activity.ModifiedBy   = _authUser.WIUID;

                var employabilityPlan = _epRepository.Get(i => i.Id == epId);

                #region Transaction

                EndActivityTransaction(employabilityPlan, activity.ActivityType.Code, activity.EndDate.GetValueOrDefault(), updateTime);

                #endregion

                #region POPClaim

                if (canInsertVocationalPopClaim)
                    canInsertVocationalPopClaim = _popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, activityContract.ActivityTypeCode, activityContract.ActivityCompletionReasonName, activityContract.EndDate.ToDateMonthDayYear(), activityContract.Id, POPClaimType.VocationalTrainingCd);

                if (canInsertEducationalPopClaim)
                    canInsertEducationalPopClaim = _popClaimDomain.InsertSystemGeneratedPOPClaim(employabilityPlan, activityContract.ActivityTypeCode, activityContract.ActivityCompletionReasonName, activityContract.EndDate.ToDateMonthDayYear(), activityContract.Id, POPClaimType.EducationalAttainmentCd);

                #endregion
            }

            EndStatus(epId, decimal.Parse(pin), activityContracts, updateTime);

            if (!isDisenrollment)
                EndActivityTransactionalSave(xml, epId: epId);

            return activityContracts;
        }

        public void EndActivityTransactionalSave(string xml, int? epId = null, string programCd = null)
        {
            programCd = !string.IsNullOrWhiteSpace(programCd) ? programCd : _epRepository.Get(i => i.Id == epId).EnrolledProgram.ProgramCode.ToLower().SafeTrim();

            if (programCd == "ww" || programCd == "cf")
            {
                using (var tx = _epaBridgeRepository.GetDataBase().BeginTransaction())
                {
                    try
                    {
                        _unitOfWork.Commit();

                        var parms = new Dictionary<string, object>
                                    {
                                        ["XML"]       = xml,
                                        ["ProgramCd"] = programCd
                                    };

                        var rs = _epaBridgeRepository.GetStoredProcReturnValue("USP_Clear_Participation_Entries", parms);

                        if (rs == 0)
                            tx.Commit();
                        else
                            throw new DCFApplicationException("Failed due to SProc issue. Please try again.");
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw new DCFApplicationException("Failed due to SProc issue. Please try again.", ex);
                    }

                    tx.Dispose();
                }
            }
            else
            {
                _unitOfWork.Commit();
            }
        }

        private int? GetHour(TimeSpan? time)
        {
            int? hr = null;

            if (time.HasValue)
            {
                hr = int.Parse(DateTime.Today.Add((TimeSpan) time).ToString("hh"));
            }


            return hr;
        }

        private int? GetMinute(TimeSpan? time)
        {
            int? mm = null;

            if (time.HasValue)
                mm = int.Parse(DateTime.Today.Add((TimeSpan) time).ToString("mm"));

            return mm;
        }

        private int? GetAmPm(TimeSpan? time)
        {
            string ampm = null;

            if (time.HasValue)
                ampm = DateTime.Today.Add((TimeSpan) time).ToString("tt");

            if (ampm != null)
                return ampm?.ToLower() == "am" ? 1 : 2;
            return null;
        }

        private TimeSpan GetHourMinute(string hr, string mm, string tt)
        {
            var dateTime = DateTime.Parse(hr + ":" + mm + " " + tt);
            var time     = TimeSpan.Parse(dateTime.ToString("HH:mm"));

            return time;
        }

        private Activity EndActivity(ActivityContract contract, Activity activity, int epId, int subepId, DateTime updateTime)
        {
            activity.ActivityCompletionReasonId = contract.ActivityCompletionReasonId;
            activity.EndDate                    = contract.EndDate.ToDateTimeMonthDayYear();

            var ep          = _epRepository.Get(i => i.Id == epId);
            var epBeginDate = ep.BeginDate;
            if (contract.EndDate?.ToDateTimeMonthDayYear() < epBeginDate)
            {
                var epActivityBridge = _epaBridgeRepository.Get(i => i.ActivityId == activity.Id && i.EmployabilityPlanId == epId);
                _epaBridgeRepository.Delete(epActivityBridge);

                var currSchedules = _activityScheduleRepository.GetMany(i => i.EmployabilityPlanId == epId && i.ActivityId == activity.Id).ToList();
                currSchedules.ForEach(i => _activityScheduleRepository.Delete(i));

                var prevSchedules = _activityScheduleRepository.GetMany(i => i.EmployabilityPlanId == subepId && i.ActivityId == activity.Id).ToList();
                prevSchedules.ForEach(i =>
                                      {
                                          i.ActualEndDate = contract.EndDate.ToDateTimeMonthDayYear();
                                          i.ModifiedDate  = updateTime;
                                          i.ModifiedBy    = _authUser.WIUID;
                                      });

                #region Transaction

                if (contract.IsCarriedOver) EndActivityTransaction(ep, activity.ActivityType.Code, contract.EndDate.ToDateMonthDayYear(), updateTime);

                #endregion
            }
            else
            {
                var schedules = _activityScheduleRepository.GetMany(i => i.EmployabilityPlanId == epId && i.ActivityId == activity.Id).ToList();
                schedules.ForEach(i =>
                                  {
                                      i.ActualEndDate = contract.EndDate.ToDateTimeMonthDayYear();
                                      i.ModifiedDate  = updateTime;
                                      i.ModifiedBy    = _authUser.WIUID;
                                  });
            }

            return activity;
        }

        private void EndActivityTransaction(EmployabilityPlan ep, string statusCode, DateTime effectiveTime, DateTime modifiedDate)
        {
            var workerId = _workerRepo.Get(i => i.WIUId == _authUser.WIUID).Id;

            var transactionContract = new TransactionContract
                                      {
                                          ParticipantId       = ep.ParticipantId,
                                          WorkerId            = workerId,
                                          OfficeId            = ep.ParticipantEnrolledProgram.OfficeId.GetValueOrDefault(),
                                          EffectiveDate       = effectiveTime,
                                          CreatedDate         = modifiedDate,
                                          TransactionTypeCode = TransactionTypes.ActivityEnd,
                                          ModifiedBy          = _authUser.WIUID,
                                          StatusCode          = statusCode
            };

            _transactionDomain.InsertTransaction(transactionContract);
        }

        private Activity UnEndActivity(Activity activity)
        {
            activity.ActivityCompletionReasonId = null;
            activity.EndDate                    = null;

            return activity;
        }


        public void EndStatus(int epId, decimal pin, IReadOnlyCollection<ActivityContract> contracts, DateTime updateTime)
        {
            var assessmentActivities = _epaBridgeRepository.GetAsQueryable()
                                                           .Where(i => i.EmployabilityPlanId == epId)
                                                           .Select(i => i.Activity)
                                                           .Where(i => i.ActivityType.EnrolledProgramEPActivityTypeBridges
                                                                        .Any(j => j.IsAssessmentRelated == true))
                                                           .Distinct();
            ;

            if (!assessmentActivities.Any()) return;
            {
                var nonEndedActivities = assessmentActivities.Where(i => i.ActivityCompletionReasonId == null).ToList();

                if (contracts != null)
                    nonEndedActivities.RemoveAll(i => contracts.Select(j => j.Id).Contains(i.Id));

                if (nonEndedActivities.Any()) return;
                {
                    var status             = _psRepository.Get(i => i.Participant.PinNumber == pin && i.isCurrent == true && i.Status.Name == ParticipationStatus.FA);
                    var maxDbEndDate       = assessmentActivities.Max(i => i.EndDate);
                    var maxContractEndDate = contracts?.Max(i => DateTime.Parse(i.EndDate));
                    var endDate            = new[] { maxDbEndDate, maxContractEndDate }.Max();
                    if (status == null) return;
                    status.isCurrent    = false;
                    status.EndDate      = endDate;
                    status.ModifiedDate = updateTime;
                    status.ModifiedBy   = _authUser.WIUID;

                    var rs = _participantDomain.ExecSanctionableSP(_authUser.WIUID, updateTime, status.ParticipantId, status.BeginDate, endDate);

                    if (rs != 0)
                        throw new DCFApplicationException("Failed due to SProc issue. Please try again.");
                }
            }
        }

        #endregion
    }
}

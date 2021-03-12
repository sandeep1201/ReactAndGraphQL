using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class ParticipantActivityDomain : IParticipantActivityDomain
    {
        #region Properties

        private readonly IParticipantRepository                     _participantRepo;
        private readonly IEmployabilityPlanRepository               _epRepo;
        private readonly IEmployabilityPlanActivityBridgeRepository _epActivityBridgeRepo;
        private readonly IParticipationStatusRepository             _participationStatusRepo;

        #endregion

        #region Methods

        public ParticipantActivityDomain(IParticipantRepository                     participantRepo,
                                         IEmployabilityPlanRepository               epRepo,
                                         IEmployabilityPlanActivityBridgeRepository epActivityBridgeRepo,
                                         IParticipationStatusRepository             participationStatusRepo)
        {
            _participantRepo         = participantRepo;
            _epRepo                  = epRepo;
            _epActivityBridgeRepo    = epActivityBridgeRepo;
            _participationStatusRepo = participationStatusRepo;
        }

        public ParticipantActivitiesWebService GetParticipantActivitiesByPins(string pins)
        {
            var contract = new ParticipantActivitiesWebService
                           {
                               Participants = pins.Split(',')
                                                  .Select(pin =>
                                                          {
                                                              var pinNo       = decimal.Parse(pin);
                                                              var participant = _participantRepo.Get(i => i.PinNumber == pinNo);

                                                              return new WSSummary
                                                                     {
                                                                         PinNumber = decimal.Parse(pin),
                                                                         Programs = participant?.ParticipantEnrolledPrograms
                                                                                               .Select(pep =>
                                                                                                       {
                                                                                                           var submittedEp = _epRepo.Get(i => i.ParticipantId                    == participant.Id                    &&
                                                                                                                                              i.EnrolledProgram.ProgramCode      == pep.EnrolledProgram.ProgramCode   &&
                                                                                                                                              i.EmployabilityPlanStatusType.Name == EmployabilityPlanStatus.Submitted &&
                                                                                                                                              !i.IsDeleted);
                                                                                                           var participationStatuses = _participationStatusRepo.GetMany(i => i.ParticipantId == participant.Id &&
                                                                                                                                                                             !i.IsDeleted                      &&
                                                                                                                                                                             i.EnrolledProgram.ProgramCode == pep.EnrolledProgram.ProgramCode)
                                                                                                                                                               .ToList();
                                                                                                           return new WSPrograms
                                                                                                                  {
                                                                                                                      ProgramCode = pep.EnrolledProgram.ProgramCode,
                                                                                                                      Activities = submittedEp == null
                                                                                                                                       ? new List<WSActivity>()
                                                                                                                                       : _epActivityBridgeRepo.GetAsQueryable()
                                                                                                                                                              .Where(i => i.EmployabilityPlanId == submittedEp.Id && !i.IsDeleted)
                                                                                                                                                              .Select(i => i.Activity)
                                                                                                                                                              .Where(i => !i.IsDeleted)
                                                                                                                                                              .Select(i => new WSActivity
                                                                                                                                                                           {
                                                                                                                                                                               Activity    = i.ActivityType.Code + " - " + i.ActivityType.Name,
                                                                                                                                                                               Description = i.Description,
                                                                                                                                                                               StartDate   = i.StartDate,
                                                                                                                                                                               EndDate     = i.EndDate,
                                                                                                                                                                               ActivitySchedules = i.ActivitySchedules
                                                                                                                                                                                                    .Where(j => !j.IsDeleted)
                                                                                                                                                                                                    .Select(j => new WSActivitySchedule
                                                                                                                                                                                                                 {
                                                                                                                                                                                                                     BeginTime      = j.BeginTime,
                                                                                                                                                                                                                     EndTime        = j.EndTime,
                                                                                                                                                                                                                     HoursPerDay    = j.HoursPerDay,
                                                                                                                                                                                                                     PlannedEndDate = j.PlannedEndDate,
                                                                                                                                                                                                                     FrequencyType  = GetFrequencyNameForSchedule(j)
                                                                                                                                                                                                                 })
                                                                                                                                                                                                    .ToList()
                                                                                                                                                                           })
                                                                                                                                                              .ToList(),
                                                                                                                      ParticipationStatuses = participationStatuses.Select(i => new WSParticipantionStatus
                                                                                                                                                                                 {
                                                                                                                                                                                     ParticipantionStatus = i.Status.Code + " - " + i.Status.Name,
                                                                                                                                                                                     BeginDate            = i.BeginDate,
                                                                                                                                                                                     EndDate              = i.EndDate,
                                                                                                                                                                                     Details              = i.Details
                                                                                                                                                                                 })
                                                                                                                                                                   .ToList()
                                                                                                                  };
                                                                                                       })
                                                                                               .ToList(),
                                                                     };
                                                          })
                                                  .ToList()
                           };

            contract.Participants
                    .AsNotNull()
                    .ForEach(i => i.IsDateFound = i.Programs != null && i.Programs.Any() && i.Programs.Any(j => j.Activities.Any() || j.ParticipationStatuses.Any()));

            return contract;
        }

        private string GetFrequencyNameForSchedule(ActivitySchedule schedule)
        {
            var frequencyTypeName =  string.Empty;

            if (!schedule.IsRecurring.GetValueOrDefault() || schedule.FrequencyTypeId == null) return frequencyTypeName;

            frequencyTypeName = schedule.FrequencyType.Name;

            var frequencies = schedule.ActivityScheduleFrequencyBridges.Where(i => !i.IsDeleted).ToList();

            switch (frequencyTypeName)
            {
                case ActivityScheduleFrequencyName.Monthly:
                    frequencyTypeName = string.Concat(frequencyTypeName, " - ", frequencies[0].MRFrequency.Name, " - ", frequencies[0].WKFrequency.Name);
                    break;
                case ActivityScheduleFrequencyName.Weekly:
                case ActivityScheduleFrequencyName.Biweekly:
                    frequencyTypeName = string.Concat(frequencyTypeName, " - ", string.Join(", ", frequencies.Select(i => i.WKFrequency.Name)));
                    break;
            }

            return frequencyTypeName;
        }

        #endregion
    }
}

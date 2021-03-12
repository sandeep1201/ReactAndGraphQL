using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ParticipantActivitiesWebService
    {
        public List<WSSummary> Participants { get; set; }
        public string          MessageCode  { get; set; }
    }

    public class WSSummary
    {
        public decimal?         PinNumber   { get; set; }
        public bool             IsDateFound { get; set; }
        public List<WSPrograms> Programs    { get; set; }
    }

    public class WSPrograms
    {
        public string                       ProgramCode            { get; set; }
        public List<WSActivity>             Activities             { get; set; }
        public List<WSParticipantionStatus> ParticipationStatuses { get; set; }
    }

    public class WSActivity
    {
        public string                   Activity          { get; set; }
        public string                   Description       { get; set; }
        public DateTime?                StartDate         { get; set; }
        public DateTime?                EndDate           { get; set; }
        public List<WSActivitySchedule> ActivitySchedules { get; set; }
    }

    public class WSActivitySchedule
    {
        public DateTime? PlannedEndDate { get; set; }
        public string    FrequencyType  { get; set; }
        public decimal?  HoursPerDay    { get; set; }
        public TimeSpan? BeginTime      { get; set; }
        public TimeSpan? EndTime        { get; set; }
    }

    public class WSParticipantionStatus
    {
        public string    ParticipantionStatus { get; set; }
        public DateTime? BeginDate            { get; set; }
        public DateTime? EndDate              { get; set; }
        public string    Details              { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Tasks;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Domain;
using SpAlienStatusResult = Dcf.Wwp.Data.Sql.Model.SpAlienStatusResult;


namespace DCF.Timelimts.Service
{
    public interface ITimelimitService : IDisposable
    {
        //void DisableT0459_IN_W2_LIMITS_Triggers();
        //void EnableT0459_IN_W2_LIMITS_Triggers();
        //void DisableT0460_IN_W2_EXT_Triggers();
        //void EnableT0460_IN_W2_EXT_Triggers();

        Task<Int32?>                  GetClockMaxAsync(Decimal                                                         pinNumber,   ClockTypes        flags,     CancellationToken token = default(CancellationToken));
        Task<List<Decimal>>           GetTimelimitPinsToProcessAsync(DateTime                                          currentDate, Int32             partition, CancellationToken token = default(CancellationToken));
        Task<List<Decimal>>           GetExtensionPinsToProcessAsync(DateTime                                          currentDate, Int32             partition, CancellationToken token = default(CancellationToken));
        Task<List<Decimal>>           GetBatchEvaluatedPins(DateTime                                                   currentDate, Int32             partition, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Placement>>  GetPlacementsAsync(Decimal                                                       pinNumber,   CancellationToken token = default(CancellationToken));
        IEnumerable<Placement>        GetPlacements(Decimal                                                            pinNumber);
        Task<List<TimelineMonth>>     GetTimelineMonthsAsync(Decimal                                                   pinNumber, CancellationToken token = new CancellationToken());
        List<TimelineMonth>           GetTimelineMonths(Decimal                                                        pinNumber);
        Task<List<ExtensionSequence>> GetTimelineExtensionSequencesAsync(Decimal                                       pinNumber, CancellationToken token = new CancellationToken());
        List<ExtensionSequence>       GetTimelineExtensionSequences(Decimal                                            pinNumber);
        Task                          RefreshAuxPaymentsAsync(decimal                                                  pinNumber, CancellationToken token = default(CancellationToken));
        void                          RefreshAuxPayments(decimal                                                       pinNumber);
        Task                          RefreshParticipantsAsync(DateTime                                                month, CancellationToken token = default(CancellationToken));
        void                          RefreshParticipants(DateTime                                                     month);
        Task<Participant>             GetParticipantAsync(Decimal                                                      pinNumber, CancellationToken token = default(CancellationToken));
        Task<Participant>             GetParticipantByIdAsync(Int32                                                    id,        CancellationToken token = default(CancellationToken));
        Participant                   GetParticipant(Decimal                                                           pinNumber);
        Task<List<AlienStatus>>       GetParticipantAlienStatusAsync(Decimal                                           pinNumber, CancellationToken token = default(CancellationToken));
        List<AlienStatus>             GetParticipantAlienStatus(Decimal                                                pinNumber);
        List<AlienStatus>             Map_SpAlienStatusResults_To_SpAlienStatusResult(IEnumerable<SpAlienStatusResult> data);
        Task<List<AuxiliaryPayment>>  GetAuxillaryPaymentsAsync(Decimal                                                pinNumber, CancellationToken token = default(CancellationToken));
        List<AuxiliaryPayment>        GetAuxillaryPayments(Decimal                                                     pinNumber);
        List<Payment>                 GetPaymentInfo(Decimal                                                           pinNumber);
        Task<List<Payment>>           GetPaymentInfoAsync(Decimal                                                      pinNumber, CancellationToken token = default(CancellationToken));

        List<AssistanceGroupMember>       GetOtherAGMembers(Decimal      pinNumber, DateTime          beginDate, DateTime endDate);
        Task<List<AssistanceGroupMember>> GetOtherAGMembersAsync(Decimal pinNumber, DateTime          beginDate, DateTime endDate, CancellationToken token = default(CancellationToken));
        Task<List<TimeLimit>>             TimeLimitsByPinAsync(Decimal   pin,       CancellationToken token                         = default(CancellationToken));
        Task<TimeLimit>                   TimeLimitByIdAsync(Int32       id,        CancellationToken token                         = default(CancellationToken));
        Task<TimeLimit>                   TimeLimitByDateAsync(Decimal   pin,       DateTime          date, CancellationToken token = default(CancellationToken));
        List<TimeLimit>                   TimeLimitsByPin(Decimal        pin);
        TimeLimit                         TimeLimitById(Int32            id);
        TimeLimit                         TimeLimitByDate(Decimal        pin, DateTime date);
        TimeLimit                         NewTimeLimit();
        Task<List<TimeLimitState>>        TimeLimitStatesAsync(Boolean    excludeWisconsin                = true, CancellationToken token = default(CancellationToken));
        Task<List<ChangeReason>>          ChangeReasons(CancellationToken token                           = default(CancellationToken));
        Task<Int32>                       SaveEntityAsync<T>(T            entity, CancellationToken token = default(CancellationToken)) where T : class, ICommonDelCreatedModel;
        Int32                             SaveEntity<T>(T                 entity) where T : class, ICommonDelCreatedModel;

        /// <summary>
        /// Updateds a db2record.
        /// Current sproc impelementation will only change history code 0 record to history code 9 records and ignore all other fields! After "updating", insert a new record with the update values
        /// and incremented history sequence number
        /// </summary>
        /// <param name="db2Record"></param>
        void SPDB2T0459Update(IT0459_IN_W2_LIMITS db2Record);

        void                    SPDB2T0459Insert(IT0459_IN_W2_LIMITS       db2Record);
        ITimeline               GetTimeline(Decimal                        pinNumber);
        Task<ITimeline>         GetTimelineAsync(Decimal                   pinNumber, CancellationToken token = default(CancellationToken));
        ITimeLimitSummary       CreateTimelimitSummary(ITimeline           timeline,  Int32             participantId);
        TimeLimitWSSummary      CreateTimeLimitWebServiceSummary(ITimeline timeline,  string            pin, IQueryable<ITimeLimit> twentyFourFrom2009To2011);
        Task<ITimeLimitSummary> CreateTimelimitSummaryAsync(Decimal        pinNumber, CancellationToken token = default(CancellationToken));
        ITimeLimitSummary       GetTimelimitSummary(Decimal                pinNumber);
        Task<ITimeLimitSummary> GetTimelimitSummaryAsync(Decimal           pinNumber, CancellationToken token = default(CancellationToken));


        Task SpTimeLimitPlacementClosureAsync(Decimal caseNumber, DateTime databaseDate, string inputUserId, DateTime existingEpisodeBeginDate, Decimal pinNumber, string existingFepId, DateTime existingEpisodeEndDate, string existingPlacementCode, DateTime existingPlacementBeginDate, string newFepIdNumber, DateTime newEpisodeEndDate, string newPlacementCode, CancellationToken token = default(CancellationToken));
        void SpTimeLimitPlacementClosure(Decimal      caseNumber, DateTime databaseDate, string inputUserId, DateTime existingEpisodeBeginDate, Decimal pinNumber, string existingFepId, DateTime existingEpisodeEndDate, string existingPlacementCode, DateTime existingPlacementBeginDate, string newFepIdNumber, DateTime newEpisodeEndDate, string newPlacementCode);

        Task<List<IT0459_IN_W2_LIMITS>> GetLatestW2LimitsMonthsForEachClockTypeAsync(Decimal pinNum, CancellationToken token = default(CancellationToken));


        Task<IT0459_IN_W2_LIMITS> GetLatestW2LimitsByClockTypeAsync(Decimal pinNum, ClockTypes clockType, CancellationToken token = default(CancellationToken));
    }
}

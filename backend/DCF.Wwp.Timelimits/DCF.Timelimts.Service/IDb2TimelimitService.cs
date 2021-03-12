using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;

namespace Dcf.Wwp.Api.Library.Services
{
    public interface IDb2TimelimitService : IDisposable
    {
        #region Properties

        Lazy<List<IApprovalReason>> approvalReasons   { get; set; }
        Lazy<List<IChangeReason>>   changeReasons     { get; set; }
        Lazy<List<IDenialReason>>   denialReasons     { get; set; }
        IRepository                 _repo             { get; set; }
        ITimelimitService           _timelimitService { get; set; }
        bool                        IsSimulated       { get; set; }

        #endregion

        #region Methods

        IT0754_LTR_RQST           CreateExtensionNotice(ITimeLimitExtension           extension,      IParticipantInfo    participant,    ITimeline             timeline);
        ExtensionNotice           GetExtensionNotice(ITimeLimitExtension              extension,      IParticipantInfo    participant,    ITimeline             timeline,      ClockTypes clockType);
        IT0754_LTR_RQST           GetExtensionTriggerLetterRequest(ITimeline          timeline,       IParticipantInfo    participant,    ExtensionNoticeRecord noticeRecord,  GeoArea    wpGeoArea);
        IT0460_IN_W2_EXT          InsertExtension(ITimeLimitExtension                 extensionModel, IParticipantInfo    participant,    string                mainFrameUser, ITimeline  timeline);
        IT0754_LTR_RQST           InsertExtensionNotice(ITimeLimitExtension           extension,      IParticipantInfo    participant,    ITimeline             timeline);
        IT0459_IN_W2_LIMITS       InsertTick0459(ITimeLimit                           timeLimitModel, IT0459_IN_W2_LIMITS originalTick,   IParticipantInfo      participant, string mainFrameUser,   ITimeline timeline, DateTime? updatedDate = null);
        IT0459_IN_W2_LIMITS       TickRecords(ITimeLimit                              timeLimitModel, decimal             effectiveMonth, IParticipantInfo      participant, string mainFrameUserId, DateTime? updatedDate = null);
        void                      UpdateCountsFromExtensionUpsert(ITimeLimitExtension extensionModel, IParticipantInfo    participant,    ITimeline             timeline);
        void                      UpdateT0459ModelCounts(IT0459_IN_W2_LIMITS          clockTick,      ITimeline           timeline,       IT0459_IN_W2_LIMITS   originalTick);
        List<IT0459_IN_W2_LIMITS> UpdateTicks0459(IEnumerable<IT0459_IN_W2_LIMITS>    monthsToUpdate, ITimeline           timeline,       DateTime?             updatedDate = null, bool retainDate = false, string fixingCountsForWwpDb2WriteBack = null);
        IT0460_IN_W2_EXT          Upsert(ITimeLimitExtension                          extensionModel, IParticipantInfo    participant,    string                wamsId);
        IT0459_IN_W2_LIMITS       Upsert(ITimeLimit                                   timeLimitModel, IParticipantInfo    particpant,     string                wamsId, DateTime? updatedDate = null);
        void                      Save();
        Task                      SaveAsync(CancellationToken token = default(CancellationToken));

        #endregion
    }
}

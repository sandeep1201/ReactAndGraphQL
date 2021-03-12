using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITimeLimit : ICommonDelCreatedModel
    {
        Int32? ParticipantID { get; set; }
        DateTime? EffectiveMonth { get; set; }
        Int32? TimeLimitTypeId { get; set; }
        Boolean? TwentyFourMonthLimit { get; set; }
        Boolean? StateTimelimit { get; set; }
        Boolean? FederalTimeLimit { get; set; }
        Int32? StateId { get; set; }
        Int32? ChangeReasonId { get; set; }
        String ChangeReasonDetails { get; set; }
        String Notes { get; set; }
        IChangeReason ChangeReason { get; set; }
        IParticipant Participant { get; set; }
        ITimeLimitState TimeLimitState { get; set; }
        ITimeLimitType TimeLimitType { get; set; }
    }
}
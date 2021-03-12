using System;
using System.Net.PeerToPeer.Collaboration;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class TimelineMonthContract : BaseModelContract
    {
        [DataMember]
        public Int32 TimelimitTypeId { get; set; }

        [DataMember]
        public DateTimeOffset EffectiveMonth { get; set; }

        [DataMember]
        public Boolean IsEdited {get; set; }

        [DataMember]
        public Decimal? PaymentIssued { get; set; }
        
        [DataMember]
        public Int32 ReasonForChangeId { get; set; }

        [DataMember]
        public Boolean? IsPlacement { get; set; }
        [DataMember]
        public Boolean IsState { get; set; }

        [DataMember]
        public Boolean IsFederal { get; set; }

        [DataMember]
        public String ChangeReasonDetails { get; set; }
        [DataMember]
        public String Notes { get; set; }
        [DataMember]
        public Int32? StateId { get; set; }

        private static TimelineMonthContract Create(Int32 timeLimitTypeId, Boolean isFederal, Boolean IsState, Boolean? IsPlacement, DateTime? effectiveMonth, Boolean isEdited, Int32 paymentAmount, Int32? changeReasonId, String changeReasonDetails, String notes, Int32? stateId)
        {
            var tmContract = new TimelineMonthContract();
            tmContract.TimelimitTypeId = timeLimitTypeId;
            tmContract.IsFederal = isFederal;
            tmContract.IsState = IsState;
            tmContract.IsPlacement = IsPlacement;
            tmContract.EffectiveMonth = effectiveMonth.GetValueOrDefault();
            tmContract.IsEdited = isEdited;
            tmContract.ReasonForChangeId = changeReasonId.GetValueOrDefault();
            tmContract.ChangeReasonDetails = changeReasonDetails;
            tmContract.Notes = notes;
            tmContract.StateId = stateId;

            return tmContract;
        }

        public static TimelineMonthContract Create(ITimeLimit model)
        {
            var tmContract = TimelineMonthContract.Create(
                model.TimeLimitTypeId.GetValueOrDefault(),model.FederalTimeLimit.GetValueOrDefault(),model.StateTimelimit.GetValueOrDefault(),
                model.TwentyFourMonthLimit,model.EffectiveMonth,model.ModifiedDate.HasValue,/*model.paymentAmount*/0,model.ChangeReasonId,model.ChangeReasonDetails,model.Notes,model.StateId);
            BaseModelContract.SetBaseProperties(tmContract, model);

            return tmContract;
        }

    }
}
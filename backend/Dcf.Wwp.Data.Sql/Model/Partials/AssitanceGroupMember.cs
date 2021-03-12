using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class AssistanceGroup : IAssistanceGroup
    {
        public List<IAssistanceGroupMember> Parents { get; set; } = new List<IAssistanceGroupMember>();
        public List<IAssistanceGroupMember> Children { get; set; } = new List<IAssistanceGroupMember>();

    }

    
    public class AssistanceGroupMember : IAssistanceGroupMember
    {
        public Decimal? OTHER_PARTICIPANT { get; set; }
        public Decimal? PARTICIPANT { get; set; }
        public String FIRST_NAME { get; set; }
        public String LAST_NAME { get; set; }
        public String MIDDLE_INITIAL_NAME { get; set; }
        public DateTime? BIRTH_DATE { get; set; }
        public DateTime? DEATH_DATE { get; set; }
        public String GENDER { get; set; }
        public String RELATIONSHIP { get; set; }
        public String AGE { get; set; }
        public String ELIGIBILITY_PART_STATUS_CODE { get; set; }
        public String ISINPLACEMENTPLACED { get; set; }
        public List<AlienStatus> AlienStatuses { get; set; }
        public List<ExtensionSequence> ExtensionSequences { get; set; }

        public Boolean IsChild()
        {
            return this.RELATIONSHIP.ToLower().Equals("child");
        }
    }

    public class SPW2PaymentInfoResult
    {
        private DateTime? _effectivePaymentMonthDateTime;

        public Decimal CaseNumber { get; set; }

        public Decimal? EffectivePaymentMonth { get; set; }
        // Note: This is the payment effective month,
        // its "effect" should apply to the previous month.
        // I.E. July Effective Month is for pay period may-15th to june-15th or delayed payment June-16th(+) - June 30th. Count Toward June!
        public DateTime EffectivePaymentMonthDateTime
        {
            get
            {
                if (!this._effectivePaymentMonthDateTime.HasValue)
                {
                    var effMonthString = this.EffectivePaymentMonth?.ToString();
                    this._effectivePaymentMonthDateTime = DateTime.ParseExact(effMonthString, "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                }
                return this._effectivePaymentMonthDateTime.GetValueOrDefault();
            }
        }

        public DateTime PayPeriodBeginDate { get; set; }

        public DateTime PayPeriodEndDate { get; set; }

        public Decimal OriginalPaymentAmount { get; set; }

        public Decimal OrignalCheckAmount { get; set; }  
                                 
        public Decimal VendorPayment { get; set; }  
          
        public Decimal AdjustedNetAmount { get; set; }
    }


    public class SpAlienStatusResult
    {
        public Int32 ID { get; set; }
        public Decimal ParticipantId { get; set; }
        public Int32 ALIEN_STS_CD { get; set; } // TODO: I think this is a number
        public String AlienStatusCodeDescriptionText { get; set; }
        public DateTime? EffectiveBeginMonth { get; set; }
        public DateTime? EffectiveEndMonth { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public String CountryOfOrign { get; set; }
    }


}

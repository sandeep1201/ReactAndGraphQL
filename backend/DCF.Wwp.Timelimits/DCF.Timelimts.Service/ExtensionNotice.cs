using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Timelimits.Rules.Domain;


namespace DCF.Timelimts.Service
{

    public class PlacementDenialExtensionNotice : DenialExtensionNotice
    {
        public override String GEN1_INDICATOR_1 { get; set; } = "1";

        public PlacementDenialExtensionNotice(String pinNumber, ITimeLimitExtension extension, DateTime timelimitClosureDate, IDenialReason denialReason) 
            : base(pinNumber, extension, denialReason, timelimitClosureDate)
        {

        }
    }

    public class PlacementApprovalExtensionNotice : ApprovalExtensionNotice
    {
        public override String GEN1_INDICATOR_1 { get; set; } = "2";
        public PlacementApprovalExtensionNotice(String pinNumber, ITimeLimitExtension extension) : base(pinNumber, extension)
        {
        }
    }

    public class StateDenialExtensionNotice : DenialExtensionNotice
    {
        public override String GEN1_INDICATOR_1 { get; set; } = "3";

        public StateDenialExtensionNotice(String pinNumber, ITimeLimitExtension extension, DateTime timelimitClosureDate, IDenialReason denialReason) 
            : base(pinNumber, extension, denialReason, timelimitClosureDate)
        {
        }
    }

    public class StateApprovalExtensionNotice : ApprovalExtensionNotice
    {
        public override String GEN1_INDICATOR_1 { get; set; } = "4";

        public StateApprovalExtensionNotice(String pinNumber, ITimeLimitExtension extension) : base(pinNumber, extension)
        {
        }
    }

    public abstract class DenialExtensionNotice : ExtensionNotice
    {
        public override String GEN1_LOOKUP_KEY_2 { get; protected set; }

        protected DenialExtensionNotice(String pinNumber, 
            ITimeLimitExtension extension,
            IDenialReason denailReason,
            DateTime timelimitClosureDate
            ) : base(pinNumber, extension)
        {
            this.GEN1_LOOKUP_KEY_2 = "TERR_" + denailReason?.Code?.ToUpper();
            this.GEN1_DATE_FIELD_1 = timelimitClosureDate.EndOf(DateTimeUnit.Month);
            this.GEN1_NUMERIC_FIELD_1 = 0;

        }
    }

    public abstract class ApprovalExtensionNotice : ExtensionNotice
    {
        public override String GEN1_LOOKUP_KEY_2 { get; protected set; } = "TERR_";

        protected ApprovalExtensionNotice(String pinNumber, ITimeLimitExtension extension) : base(pinNumber, extension)
        {
            this.GEN1_DATE_FIELD_1 = extension.BeginMonth.GetValueOrDefault().StartOf(DateTimeUnit.Month);
            this.GEN1_NUMERIC_FIELD_1 = new DateTimeRange(extension.BeginMonth.GetValueOrDefault(),extension.EndMonth.GetValueOrDefault()).By(DateTimeUnits.Months).Count();
        }
    }

    public abstract class ExtensionNotice
    {

        protected ExtensionNotice(String pinNumber, ITimeLimitExtension extension)
        {
            this.GEN1_LOOKUP_KEY_1 = pinNumber.PadLeft(10, '0');

            var clockType = (ClockTypes) extension.TimeLimitTypeId;
            this.GEN1_LOOKUP_KEY_3 = $"SLOTCD_{(clockType == ClockTypes.State ? "60MO" : clockType.ToString())}".ToUpper();
            this.GEN1_SHORT_TEXT_1 = pinNumber.PadLeft(10, '0');
        }
        
        public virtual String GEN1_GENERIC_IND { get; set; } = "G";
        public virtual String GEN1_MAILING_ADDRESS_TYPE { get; set; } = "C";
        public virtual String GEN1_WW_RETURN_ADDRESS { get; set; } = "Y";
        public virtual String GEN1_CC_RETURN_ADDRESS { get; set; } = "N";
        public virtual Int32? GEN1_RETURN_ADDRESS_COUNTY { get; set; } = 0;
        public virtual Int32? GEN1_RETURN_ADDRESS_OFFICE { get; set; } = 0;
        public virtual String GEN1_WW_AGENCY_CONTACT { get; set; } = "Y";
        public virtual String GEN1_CC_AGENCY_CONTACT { get; set; } = "N";
        public virtual String GEN1_INCLUDE_HMONG_SW { get; set; } = "N";
        public virtual Int32? GEN1_INDICATOR_CNT { get; set; } = 1;
        public virtual Int32? GEN1_LOOKUP_CNT { get; set; } = 3;
        public virtual Int32? GEN1_DATE_CNT { get; set; } = 1;
        public virtual Int32? GEN1_NUMERIC_CNT { get; set; } = 1;
        public virtual Int32? GEN1_SHORT_TEXT_CNT { get; set; } = 1;
        public virtual Int32? GEN1_LONG_TEXT_CNT { get; set; } = 0;
        public abstract String GEN1_INDICATOR_1 { get; set; } 
        public virtual String GEN1_LOOKUP_TYPE_1 { get; set; } = "PIN_NAM";
        public String GEN1_LOOKUP_KEY_1 { get;} // PinNumber set in constructor
        public virtual String GEN1_LOOKUP_FIELD_TYPE_1 { get; set; } = "L";
        public virtual Int32? GEN1_LOOKUP_FIELD_NUM_1 { get; set; } = 1;
        public virtual String GEN1_LOOKUP_TYPE_2 { get; set; } = "T0764";
        public abstract String GEN1_LOOKUP_KEY_2 { get; protected set; } // Extension reason code, set in inherited constructor
        public virtual String GEN1_LOOKUP_FIELD_TYPE_2 { get; set; } = "L";
        public virtual Int32? GEN1_LOOKUP_FIELD_NUM_2 { get; set; } = 2;
        public virtual String GEN1_LOOKUP_TYPE_3 { get; set; } = "T0764";
        public virtual String GEN1_LOOKUP_KEY_3 { get; } // Clock-Type-CD, set in constructor
        public virtual String GEN1_LOOKUP_FIELD_TYPE_3 { get; set; } = "L";
        public virtual Int32? GEN1_LOOKUP_FIELD_NUM_3 { get; set; } = 3;
        public DateTime GEN1_DATE_FIELD_1 { get; protected set; } // last day of 24month clock/extension begin date, Set in inherited constructor
        public virtual Int32? GEN1_NUMERIC_FIELD_1 { get; set; } // TODO: Set in constructor
        public String GEN1_SHORT_TEXT_1 { get;  }


        public ExtensionNoticeRecord CreateRecord()
        {
            var record = new ExtensionNoticeRecord();
            record.GEN1_GENERIC_IND = this.GEN1_GENERIC_IND;
            record.GEN1_MAILING_ADDRESS_TYPE = this.GEN1_MAILING_ADDRESS_TYPE;
            record.GEN1_WW_RETURN_ADDRESS = this.GEN1_WW_RETURN_ADDRESS;
            record.GEN1_CC_RETURN_ADDRESS = this.GEN1_CC_RETURN_ADDRESS;
            record.GEN1_RETURN_ADDRESS_COUNTY = this.GEN1_RETURN_ADDRESS_COUNTY;
            record.GEN1_RETURN_ADDRESS_OFFICE = this.GEN1_RETURN_ADDRESS_OFFICE;
            record.GEN1_WW_AGENCY_CONTACT = this.GEN1_WW_AGENCY_CONTACT;
            record.GEN1_CC_AGENCY_CONTACT = this.GEN1_CC_AGENCY_CONTACT;
            record.GEN1_INCLUDE_HMONG_SW = this.GEN1_INCLUDE_HMONG_SW;
            record.GEN1_INDICATOR_CNT = this.GEN1_INDICATOR_CNT;
            record.GEN1_LOOKUP_CNT = this.GEN1_LOOKUP_CNT;
            record.GEN1_DATE_CNT = this.GEN1_DATE_CNT;
            record.GEN1_NUMERIC_CNT = this.GEN1_NUMERIC_CNT;
            record.GEN1_SHORT_TEXT_CNT = this.GEN1_SHORT_TEXT_CNT;
            record.GEN1_LONG_TEXT_CNT = this.GEN1_LONG_TEXT_CNT;
            record.GEN1_INDICATOR_1 = this.GEN1_INDICATOR_1;
            record.GEN1_LOOKUP_TYPE_1 = this.GEN1_LOOKUP_TYPE_1;
            record.GEN1_LOOKUP_KEY_1 = this.GEN1_LOOKUP_KEY_1; // PinNumber set in constructor
            record.GEN1_LOOKUP_FIELD_TYPE_1 = this.GEN1_LOOKUP_FIELD_TYPE_1;
            record.GEN1_LOOKUP_FIELD_NUM_1 = this.GEN1_LOOKUP_FIELD_NUM_1;
            record.GEN1_LOOKUP_TYPE_2 = this.GEN1_LOOKUP_TYPE_2;
            record.GEN1_LOOKUP_KEY_2 = this.GEN1_LOOKUP_KEY_2;
            record.GEN1_LOOKUP_FIELD_TYPE_2 = this.GEN1_LOOKUP_FIELD_TYPE_2;
            record.GEN1_LOOKUP_FIELD_NUM_2 = this.GEN1_LOOKUP_FIELD_NUM_2;
            record.GEN1_LOOKUP_TYPE_3 = this.GEN1_LOOKUP_TYPE_3;
            record.GEN1_LOOKUP_KEY_3 = this.GEN1_LOOKUP_KEY_3;
            record.GEN1_LOOKUP_FIELD_TYPE_3 = this.GEN1_LOOKUP_FIELD_TYPE_3;
            record.GEN1_LOOKUP_FIELD_NUM_3 = this.GEN1_LOOKUP_FIELD_NUM_3;
            record.GEN1_DATE_FIELD_1 = this.GEN1_DATE_FIELD_1; // last day of 24month clock/extension begin date, Set in inherited constructor
            record.GEN1_NUMERIC_FIELD_1 = this.GEN1_NUMERIC_FIELD_1; // TODO: Set in constructor
            record.GEN1_SHORT_TEXT_1 = this.GEN1_SHORT_TEXT_1;
            return record;
        }
}
}

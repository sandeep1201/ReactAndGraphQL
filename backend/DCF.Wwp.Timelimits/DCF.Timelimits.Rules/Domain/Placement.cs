using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DCF.Common.Dates;
using DCF.Core;
using EnumsNET;

namespace DCF.Timelimits.Rules.Domain
{
    public class Payment
    {
        public Decimal CaseNumber { get; set; }
        // Note: This is the payment effective month,
        // its "effect" should apply to the previous month.
        // I.E. July Effective Month is for pay period may-15th to june-15th or delayed payment June-16th(+) - June 30th. Count Toward June!
        public DateTime EffectivePaymentMonth { get; set; }
        public DateTime PayPeriodBeginDate { get; set; }
        public DateTime PayPeriodEndDate { get; set; }
        /// <summary>
        /// Payment amount normally paid for placement before sactions/adjustments
        /// </summary>
        public Decimal OriginalPaymentAmount { get; set; }

        /// <summary>
        /// The actualy payment made to the client
        /// </summary>
        public Decimal OrignalCheckAmount { get; set; }
        
        /// <summary>
        /// part of actual payment amount sent to vendor
        /// </summary>
        public Decimal VendorPayment { get; set; }

        /// <summary>
        /// Adjust payment amount after all adjustments (adjusted after pulldown)
        /// </summary>
        public Decimal AdjustedNetAmount { get; set; }

        public Boolean SanctionedToZero()
        {
            return this.OriginalPaymentAmount > 0 && (this.OrignalCheckAmount + this.VendorPayment == 0);
        }
    }

    public class Placement
    {
        public Decimal PinNumber { get; set; }
        public Boolean IsOpen
        {
            get
            {
                var dateTimeRange = this.DateRange;
                var end = dateTimeRange.End;
                return end.Year == 9999;
            }
        }

        public Placement(ClockTypes placementType, DateTimeRange dateRange)
            : this(placementType, placementType.ToString(), dateRange)
        {
            
        }

        public Placement(ClockTypes placementType, DateTime beginDate, DateTime endDate)
            : this(placementType, placementType.ToString(), new DateTimeRange(beginDate, endDate))
        {

        }

        public Placement(String placementCode, DateTime beginDate, DateTime endDate)
            :this(Placement.GetPlacementTypeFromCode(placementCode),placementCode, new DateTimeRange(beginDate,endDate))
        {
            
        }

        public Placement(String placementCode, DateTimeRange dateRange)
            : this(Placement.GetPlacementTypeFromCode(placementCode), placementCode, dateRange)
        {

        }


        internal Placement(ClockTypes placementType, String placementCode, DateTimeRange dateRange)
        {
            this.PlacementCode = placementCode?.ToUpper();
            this.PlacementType = placementType;
            this.DateRange = dateRange;
        }

        public DateTimeRange DateRange { get; set; }
        public String PlacementCode { get; set; }
        public ClockTypes? PlacementType { get;  }

        public static ClockTypes GetPlacementTypeFromCode(string code)
        {
            switch (code?.ToUpper())
            {
                
                case "CSJ":
                case "CS1":
                case "CS2":
                case "CS3":
                    return ClockTypes.CSJ;
                case "W-2 T":
                case "W2T":
                    return ClockTypes.W2T;
                case "TNP":
                    return ClockTypes.TNP;
                case "TMP":
                    return ClockTypes.TMP;
                case "CMC":
                    return ClockTypes.CMC;
                case "TJB":
                    return ClockTypes.TJB;
                case "JOBS":
                    return ClockTypes.JOBS;
                case "NON":
                case "NONE":
                    return ClockTypes.None;
                case "ARP":
                case "CMD":
                case "CMF":
                case "CMJ":
                case "CMM":
                case "CMN":
                case "CMP":
                case "CMS":
                case "CMU":
                case "END":
                case "TSP":
                default:
                    return ClockTypes.Other;
            }
        }
    }

    //public class Episode
    //{
    //    public List<Placement> Placements { get; set; }

    //    public DateTime? BeginDate()
    //    {
    //        return this.Placements.GetMin(x=>x.BeginDate)?.BeginDate;
    //    }

    //    public DateTime? EndDate()
    //    {
    //        return this.Placements.GetMax(x=>x.EndDate)?.EndDate;
    //    }
    //}
}
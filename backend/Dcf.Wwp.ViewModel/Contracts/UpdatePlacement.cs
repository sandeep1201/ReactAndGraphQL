using System;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class UpdatePlacement
    {
        [Required]
        public decimal CaseNumber { get; set; }

        [Required]
        public decimal PinNumber { get; set; }

        [Required]
        public string PlacementType { get; set; }

        [Required]
        public DateTime PlacementStartDate { get; set; }

        public DateTime? PlacementEndDate { get; set; }

        [Required]
        public string MFFEPId { get; set; }
    }

    public class OverUnderPaymentResult
    {
        public decimal  PinNumber            { get; set; }
        public decimal  CaseNumber           { get; set; }
        public int?     WorkerId             { get; set; }
        public DateTime BeginDate            { get; set; }
        public DateTime EndDate              { get; set; }
        public decimal  RevisedPaymentAmount { get; set; }
    }
}

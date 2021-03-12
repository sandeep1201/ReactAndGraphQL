using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAVehicles : BaseEntity
    {
        #region Properties

        public int      RequestId                      { get; set; }
        public string   VehicleType                    { get; set; }
        public decimal? VehicleValue                   { get; set; }
        public decimal? AmountOwed                     { get; set; }
        public decimal? VehicleEquity                  { get; set; }
        public int?     OwnershipVerificationTypeId    { get; set; }
        public int?     VehicleValueVerificationTypeId { get; set; }
        public int?     OwedVerificationTypeId         { get; set; }
        public int?     VehicleOwner                   { get; set; }
        public bool     IsDeleted                      { get; set; }
        public string   ModifiedBy                     { get; set; }
        public DateTime ModifiedDate                   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest          EaRequest                      { get; set; }
        public virtual EAVerificationType EaOwnershipVerificationType    { get; set; }
        public virtual EAVerificationType EaVehicleValueVerificationType { get; set; }
        public virtual EAVerificationType EaOwedVerificationType         { get; set; }
        public virtual Participant        Participant                    { get; set; }

        #endregion
    }
}

using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FCDPRfaDetail
    {
        #region Properties

        public int?      RequestForAssistanceId  { get; set; }
        public bool      IsVoluntary             { get; set; }
        public int?      CourtOrderedCountyId    { get; set; }
        public DateTime? CourtOrderEffectiveDate { get; set; }
        public decimal?  KIDSPinNumber           { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }
        public string    ReferralSource          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual RequestForAssistance RequestForAssistance { get; set; }
        public virtual CountyAndTribe       CountyAndTribe       { get; set; }

        #endregion
    }
}

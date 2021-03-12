using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class HousingHistory
    {
        #region Properties

        public int?      HousingSectionId   { get; set; }
        public int?      HousingSituationId { get; set; }
        public DateTime? BeginDate          { get; set; }
        public DateTime? EndDate            { get; set; }
        public bool?     HasEvicted         { get; set; }
        public decimal?  MonthlyAmount      { get; set; }
        public bool?     IsAmountUnknown    { get; set; }
        public string    Details            { get; set; }
        public int?      OriginId           { get; set; }
        public int?      SortOrder          { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual HousingSection   HousingSection   { get; set; }
        public virtual HousingSituation HousingSituation { get; set; }

        #endregion
    }
}

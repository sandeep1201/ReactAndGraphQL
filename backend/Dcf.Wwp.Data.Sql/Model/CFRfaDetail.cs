using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CFRfaDetail
    {
        #region Properties

        public int?      RequestForAssistanceId  { get; set; }
        public int?      CourtOrderedCountyId    { get; set; }
        public DateTime? CourtOrderEffectiveDate { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual CountyAndTribe       CountyAndTribe       { get; set; }
        public virtual RequestForAssistance RequestForAssistance { get; set; }

        #endregion
    }
}

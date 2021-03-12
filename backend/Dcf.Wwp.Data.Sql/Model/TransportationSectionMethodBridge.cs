using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TransportationSectionMethodBridge
    {
        #region Properties

        public int       TransportationSectionId { get; set; }
        public int?      TransporationTypeId     { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual TransportationSection TransportationSection { get; set; }
        public virtual TransportationType    TransportationType    { get; set; }

        #endregion
    }
}

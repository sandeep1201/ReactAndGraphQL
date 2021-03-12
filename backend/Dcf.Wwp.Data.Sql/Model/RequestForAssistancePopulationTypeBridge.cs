using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistancePopulationTypeBridge
    {
        #region Properties

        public int?      RequestForAssistanceId { get; set; }
        public int?      PopulationTypeId       { get; set; }
        public bool      IsDeleted              { get; set; }
        public string    ModifiedBy             { get; set; }
        public DateTime? ModifiedDate           { get; set; }

        #endregion

        #region Navigation Properties

        public virtual PopulationType       PopulationType       { get; set; }
        public virtual RequestForAssistance RequestForAssistance { get; set; }

        #endregion
    }
}

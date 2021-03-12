using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierAccommodation
    {
        #region Properties

        public int?      BarrierDetailsId { get; set; }
        public int?      AccommodationId  { get; set; }
        public DateTime? BeginDate        { get; set; }
        public DateTime? EndDate          { get; set; }
        public string    Details          { get; set; }
        public int?      DeleteReasonId   { get; set; }
        public string    ModifiedBy       { get; set; }
        public DateTime? ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual BarrierDetail BarrierDetail { get; set; }
        public virtual Accommodation Accommodation { get; set; }
        public virtual DeleteReason  DeleteReason  { get; set; }

        #endregion
    }
}

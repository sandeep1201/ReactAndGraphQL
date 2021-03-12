using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkerTaskStatus
    {
        #region Properties

        public string    Code            { get; set; }
        public string    Name            { get; set; }
        public int       SortOrder       { get; set; }
        public bool      IsSystemUseOnly { get; set; }
        public DateTime  EffectiveDate   { get; set; }
        public DateTime? EndDate         { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime  ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}

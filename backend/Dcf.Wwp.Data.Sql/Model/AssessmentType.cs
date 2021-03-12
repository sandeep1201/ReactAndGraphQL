using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AssessmentType
    {
        #region Properties

        public int      Id           { get; set; }
        public string   Code         { get; set; }
        public string   Name         { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}

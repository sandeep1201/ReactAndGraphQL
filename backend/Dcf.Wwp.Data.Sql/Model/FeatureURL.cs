using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FeatureURL
    {
        #region Properties

        public string   Feature      { get; set; }
        public string   URL          { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

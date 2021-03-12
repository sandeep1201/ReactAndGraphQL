using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SpecialInitiative
    {
        #region Properties

        public int       Id             { get; set; }
        public string    ParameterName  { get; set; }
        public string    ParameterValue { get; set; }
        public bool      IsDeleted      { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime? ModifiedDate   { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

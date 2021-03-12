using System;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContentModuleMeta
    {
        #region Properties

        public string    Name            { get; set; }
        public string    Data            { get; set; }
        public int?      ContentModuleId { get; set; }
        public DateTime? CreatedDate     { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual ContentModule ContentModule { get; set; }

        #endregion
    }
}

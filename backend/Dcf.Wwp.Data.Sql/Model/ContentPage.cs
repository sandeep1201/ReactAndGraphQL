using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContentPage
    {
        #region Properties

        public string    Title        { get; set; }
        public string    Description  { get; set; }
        public string    Slug         { get; set; }
        public int       SortOrder    { get; set; }
        public DateTime? CreatedDate  { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual ICollection<ContentModule> ContentModules { get; set; }

        #endregion
    }
}

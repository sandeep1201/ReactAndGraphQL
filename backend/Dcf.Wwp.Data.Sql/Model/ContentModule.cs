using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContentModule
    {
        #region Properties

        public string    Name            { get; set; }
        public string    Title           { get; set; }
        public bool      ShowTitle       { get; set; }
        public string    Description     { get; set; }
        public bool      ShowDescription { get; set; }
        public int       Status          { get; set; }
        public int       SortOrder       { get; set; }
        public int?      ContentPageId   { get; set; }
        public DateTime? CreatedDate     { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual ContentPage ContentPage { get; set; }

        [JsonIgnore]
        public virtual ICollection<ContentModuleMeta> ContentModuleMetas { get; set; }

        #endregion
    }
}

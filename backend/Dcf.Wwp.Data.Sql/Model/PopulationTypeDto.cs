using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PopulationTypeDto : IPopulationTypeDto
    {
        #region Properties

        public int              Id             { get; set; }
        public string           Name           { get; set; }
        public bool             DisablesOthers { get; set; }
        public IEnumerable<int> DisabledIds    { get; set; }

        #endregion
    }
}

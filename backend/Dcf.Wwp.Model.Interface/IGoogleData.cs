using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IGoogleData
    {
        #region Properties

        List<IGoogleCity>          Cities          { get; set; }
        List<IGooglePlace>         Schools         { get; set; }
        List<IGoogleStreetAddress> StreetAddresses { get; set; }
        IGoogleStreetAddress       Address         { get; set; }

        #endregion

        #region Methods
        
        #endregion
    }
}

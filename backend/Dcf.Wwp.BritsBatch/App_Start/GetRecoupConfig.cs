using Dcf.Wwp.BritsBatch.Interfaces;

namespace Dcf.Wwp.BritsBatch
{
    public class GetRecoupConfig : IGetRecoupConfig
    {
        #region Properties

        public string Env          { get; set; }
        public string Endpoint     { get; set; }
        public string PostEndpoint { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

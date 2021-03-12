using Dcf.Wwp.BritsBatch.Interfaces;

namespace Dcf.Wwp.BritsBatch
{
    public class EntSecConfig : IEntSecConfig
    {
        #region Properties

        public string Env            { get; set; }
        public string Endpoint       { get; set; }
        public string ApplicationKey { get; set; }
        public string Username       { get; set; }
        public string Password       { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

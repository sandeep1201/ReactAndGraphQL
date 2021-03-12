using DCF.Common;

namespace Dcf.Wwp.UnitTest.Infrastructure
{
    public class MockDatabaseConfiguration : IDatabaseConfiguration
    {
        #region Properties

        public string Server      { get; set; }
        public string Catalog     { get; set; }
        public string UserId      { get; set; }
        public string Password    { get; set; }
        public int    MaxPoolSize { get; set; }
        public int    Timeout     { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

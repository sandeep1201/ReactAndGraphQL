using System.Data.Entity;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EFConfig : DbConfiguration
    {
        #region Properties

        #endregion

        #region Methods

        public EFConfig()
        {
            AddInterceptor(new StringTrimInterceptor());
        }

        #endregion
    }
}

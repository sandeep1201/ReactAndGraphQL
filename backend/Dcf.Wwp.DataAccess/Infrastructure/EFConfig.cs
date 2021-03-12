using System.Data.Entity;

namespace Dcf.Wwp.DataAccess.Infrastructure
{
    /// <summary>
    /// Custom configs for EF behavior.
    /// Gets automatically loaded by the CLR, no
    /// need to invoke it.
    /// </summary>
    public class EFConfig : DbConfiguration
    {
        #region Properties
        
        #endregion

        #region Methods

        public EFConfig ()
        {
            AddInterceptor(new StringTrimInterceptor());    // Trims trailing blanks... in rows coming from SQLServer.
        }

        #endregion
    }
}

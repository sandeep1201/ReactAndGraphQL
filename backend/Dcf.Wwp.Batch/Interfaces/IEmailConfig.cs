using System.Collections.Generic;

namespace Dcf.Wwp.Batch.Interfaces
{
    public interface IEmailConfig
    {
        #region Properties

        string         SmtpServer { get; set; }
        int            SmtpPort   { get; set; }
        string         From       { get; set; }
        string         FromName   { get; set; }
        List<SendAddr> SendAddrs  { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

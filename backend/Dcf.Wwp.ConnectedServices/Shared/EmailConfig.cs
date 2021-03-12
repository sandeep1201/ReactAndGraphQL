using System.Collections.Generic;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    public class EmailConfig : IEmailConfig
    {
        #region Properties

        public string         SmtpServer { get; set; }
        public int            SmtpPort   { get; set; }
        public string         From       { get; set; }
        public string         FromName   { get; set; }
        public List<SendAddr> SendAddrs  { get; set; }

        #endregion

        #region Methods

        #endregion
    }

    public class SendAddr
    {
        #region Properties

        public string       JobName { get; set; }
        public List<string> To      { get; set; }
        public List<string> Cc      { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

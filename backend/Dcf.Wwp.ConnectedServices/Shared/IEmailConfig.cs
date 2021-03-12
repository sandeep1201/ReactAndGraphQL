using System.Collections.Generic;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    public interface IEmailConfig
    {
        string         SmtpServer { get; set; }
        int            SmtpPort   { get; set; }
        string         From       { get; set; }
        string         FromName   { get; set; }
        List<SendAddr> SendAddrs  { get; set; }
    }
}

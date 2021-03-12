using System.IO;
using System.Linq;
using System.Net.Mail;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.ConnectedServices.Shared;

namespace Dcf.Wwp.Api.Library.Domains
{
    /// <summary>
    /// Sends email using the SMTP Configurations
    /// </summary>
    public class SmtpEmail : ISmtpEmail
    {
        #region Properties

        private readonly IEmailConfig _emailConfig;

        #endregion

        #region Methods

        public SmtpEmail(IEmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(string jobName, MemoryStream stream, string fileName, string fileType, string env, string subject, string body)
        {
            if (!_emailConfig.SendAddrs.Select(i => i.JobName).Contains(jobName)) return;

            switch (env.ToLower())
            {
                case "wwpdev":
                    env = "DEV";
                    break;
                case "wwpsys":
                    env = "SYS";
                    break;
                case "wwpacc":
                    env = "ACC";
                    break;
                case "wwptrn":
                    env = "TRN";
                    break;
                case "wwp":
                    env = "PRD";
                    break;
            }

            stream.Position = 0;
            var smtpClient = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.SmtpPort);
            var mail = new MailMessage
                       {
                           From        = new MailAddress(_emailConfig.From, _emailConfig.FromName),
                           Subject     = $"{env}: {subject}",
                           Attachments = { new Attachment(stream, fileName, "text/csv") },
                           Body        = body
                       };

            _emailConfig.SendAddrs.First(i => i.JobName == jobName).To.ForEach(t => mail.To.Add(t));
            if (_emailConfig.SendAddrs.First(i => i.JobName == jobName).Cc?.Count > 0) _emailConfig.SendAddrs.First(i => i.JobName == jobName).Cc.ForEach(c => mail.CC.Add(c));
            smtpClient.Send(mail);
        }

        #endregion
    }
}

using System.IO;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ISmtpEmail
    {
        #region Properties

        #endregion

        #region Methods

        void SendEmail(string jobName, MemoryStream stream, string fileName, string fileType, string env, string subject, string body);

        #endregion
    }
}

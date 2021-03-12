using System.IO;

namespace Dcf.Wwp.Batch.Interfaces
{
    public interface ISmtpEmail
    {
        #region Properties

        #endregion

        #region Methods

        void SendEmail(string jobName, FileStream stream, string fileName, string fileType, string env, string subject, string body);

        #endregion
    }
}

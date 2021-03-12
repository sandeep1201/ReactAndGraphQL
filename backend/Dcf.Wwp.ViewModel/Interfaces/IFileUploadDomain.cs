using System.Collections.Generic;
using System.IO;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IFileUploadDomain
    {
        #region Properties

        #endregion

        #region Methods

        bool                  UploadFile(MemoryStream    file, string pin, int employabilityPlanId);
        byte[]                RetrieveDocument(string    docId);
        List<DocumentRequest> RetrieveAllDocument(string pinNumber);
        bool                  UploadEPDoc(string         pin, int employabilityPlanId, byte[] contractFileStream);

        #endregion
    }
}

using System.Collections.Generic;

namespace Dcf.Wwp.ConnectedServices.FileUpload
{
    public class FileUploadConfig : IFileUploadConfig
    {
        #region Properties

        public string Env           { get; set; }
        public string Endpoint      { get; set; }
        public string CMServerName  { get; set; }
        public string CMUserId      { get; set; }
        public string CMUserPwd     { get; set; }

        #endregion

        #region Methods

        #endregion
    }

}

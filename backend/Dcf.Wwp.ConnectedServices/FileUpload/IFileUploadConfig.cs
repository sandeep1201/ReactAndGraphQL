
namespace Dcf.Wwp.ConnectedServices.FileUpload
{
    public interface IFileUploadConfig
    {
        #region Properties

        string Env          { get; set; }
        string Endpoint     { get; set; }
        string CMServerName { get; set; }
        string CMUserId     { get; set; }
        string CMUserPwd    { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}

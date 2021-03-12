using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ISupportiveServiceDomain
    {
        #region Properties

        #endregion

        #region Methods

        void GetSupportiveService(string pin, int id);
        void GetSupportiveServices(string pin, int epId);
        void InsertSupportiveService(SupportiveServiceContract SupportiveServiceContract, string pin, int id);
        void UpdateSupportiveService(SupportiveServiceContract SupportiveServiceContract, string pin, int id);
        void DeleteSupportiveService(SupportiveServiceContract SupportiveServiceContract, string pin, int id);

        void UpsertSupportiveService(SupportiveServiceContract SupportiveServiceContract, string pin, int epId);

        #endregion
    }
}

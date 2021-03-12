namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWorkHistorySectionEmploymentPreventionTypeBridgeRepository
    {
        IWorkHistorySectionEmploymentPreventionTypeBridge NewWorkHistorySectionEmploymentPreventionTypeBridge(
            IWorkHistorySection parentObject, string user);
    }
}
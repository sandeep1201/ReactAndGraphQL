
namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWorkingHistorySectionRepository
    {
        IWorkHistorySection NewWorkHistorySection(IParticipant parentParticipant, string user);
    }
}

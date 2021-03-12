namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IElevatedAccessRepository
    {
        IElevatedAccess NewElevatedAccess(string user, int workerId, int participantId, int? earId, string details);
    }
}

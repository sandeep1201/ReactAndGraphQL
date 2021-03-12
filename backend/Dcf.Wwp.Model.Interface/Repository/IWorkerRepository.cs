using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IWorkerRepository
    {
        IWorker              WorkerByWamsId(string               wamsId);
        IWorker              WorkerByMainframeId(string          mfUserId);
        IWorker              WorkerByWIUID(string                wiuid);
        IWorker              WorkerById(int                      id);
        IEnumerable<IWorker> WorkersByMainframeIds(List<string>  mfUserIdList);
        IWorker              GetOrCreateWorkerLogin(string       wamsId);
        IEnumerable<IWorker> GetWorkersByAgency(string           agencyCode);
        IEnumerable<IWorker> GetWorkersByOrganization(string     orgCode);
        List<IWorker>        GetWorkerInfosByWamsId(List<string> wamsIds);
        string               GetFnMFId();
        List<IWorker>        GetWorkersByAuthToken(string          agencyCode, string programId);
        IEnumerable<IWorker> GetWorkersByOrganizationByRole(string orgCode,    string roleCode);
        string               GetWorkerNameByWamsId(string          wamsId);

        string GetWorkerNameByWIUId(string wiuid);
    }
}

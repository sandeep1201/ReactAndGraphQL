using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class WorkersViewModel : BaseViewModel
    {
        public WorkersViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public IEnumerable<WorkerContract> GetWorkersForOrganization(string orgCode)
        {
            var workers = Repo.GetWorkersByOrganization(orgCode).OrderBy(x => x.FirstName);

            return workers.Select(item => new WorkerContract()
                                          {
                                              Id           = item.Id,
                                              WamsId       = item.WAMSId,
                                              WorkerId     = item.MFUserId,
                                              Wiuid        = item.WIUID,
                                              FirstName    = item.FirstName,
                                              LastName     = item.LastName,
                                              Organization = item.Organization?.AgencyName,
                                              IsActive     = item.WorkerActiveStatusCode == "ACTIVE"
                                          });
        }

        public IEnumerable<WorkerContract> GetWorkersForOrganization(string orgCode, string roleCode)
        {
            var workers = Repo.GetWorkersByOrganizationByRole(orgCode, roleCode).OrderBy(x => x.FirstName);

            return workers.Select(item => new WorkerContract()
                                          {
                                              Id            = item.Id,
                                              WamsId        = item.WAMSId,
                                              WorkerId      = item.MFUserId,
                                              Wiuid         = item.WIUID,
                                              FirstName     = item.FirstName,
                                              MiddleInitial = item.MiddleInitial,
                                              LastName      = item.LastName,
                                              Organization  = item.Organization?.AgencyName,
                                              IsActive      = item.WorkerActiveStatusCode == "ACTIVE"
                                          });
        }
    }
}

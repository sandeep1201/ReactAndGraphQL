using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWorkerRepository
    {
        public IWorker WorkerById(int id)
        {
            return GetWorkerLoginDetails(x => x.Id == id).FirstOrDefault();
        }

        public IWorker WorkerByWamsId(string wamsId)
        {
            return GetWorkerLoginDetails(x => x.WAMSId == wamsId).FirstOrDefault();
        }

        public IWorker WorkerByMainframeId(string mfUserId)
        {
            return GetWorkerLoginDetails(usr => usr.MFUserId == mfUserId).FirstOrDefault();
        }

        public IWorker WorkerByWIUID(string wiuid)
        {
            return GetWorkerLoginDetails(usr => usr.WIUID == wiuid).FirstOrDefault();
        }

        public IEnumerable<IWorker> WorkersByMainframeIds(List<string> mfUserIdList)
        {
            // Doing the equvialent of WHERE MFUserId in {{mfUserIdList}}
            return GetWorkerLoginDetails(x => mfUserIdList.Contains(x.MFUserId));
        }

        private IQueryable<Worker> GetWorkerLoginDetails(Expression<Func<Worker, bool>> filter)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Workers.Where(filter)
                      .Where(x => (!x.IsDeleted)
                                  && (x.Organization.InActivatedDate == null || x.Organization.InActivatedDate >= currentDate)
                                  && x.Organization.ActivatedDate <= currentDate)
                      .Include(x => x.Organization);
        }

        public IWorker GetOrCreateWorkerLogin(string wamsId)
        {
            var worker = WorkerByWamsId(wamsId);

            if (worker != null) return worker;

            worker = new Worker
                     {
                         WAMSId       = wamsId,
                         ModifiedDate = DateTime.Now,
                         ModifiedBy   = wamsId
                     };

            _db.Workers.Add((Worker) worker);

            return (worker);
        }

        public IEnumerable<IWorker> GetWorkersByAgency(string entSecCode)
        {
            var workerList = GetAllWorkersByOrganizationCode(entSecCode);

            return workerList;
        }

        public IEnumerable<IWorker> GetWorkersByOrganization(string entSecCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            return _db.Workers.Where(x => (x.Organization.EntsecAgencyCode   == entSecCode && !x.IsDeleted)
                                          && (x.Organization.InActivatedDate == null || x.Organization.InActivatedDate >= currentDate)
                                          && (x.Organization.ActivatedDate <= currentDate) && (x.WorkerActiveStatusCode == "ACTIVE"));
        }

        public IEnumerable<IWorker> GetWorkersByOrganizationByRole(string orgCode, string roleCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var t = _db.Workers.Where(x => (x.Organization.EntsecAgencyCode == orgCode && x.Roles.Contains(roleCode) && !x.IsDeleted)
                                           && (x.Organization.InActivatedDate == null || x.Organization.InActivatedDate >= currentDate)
                                           && (x.Organization.ActivatedDate <= currentDate));
            return (t);
        }

        public List<IWorker> GetWorkerInfosByWamsId(List<string> wamsIds)
        {
            return _db.Workers.AsNoTracking().Where(x => wamsIds.Contains(x.WAMSId)).ToList<IWorker>();
        }

        public string GetFnMFId()
        {
            var mfId = _db.SpecialInitiatives.AsNoTracking().FirstOrDefault(i => i.ParameterName == "FnMFId")?.ParameterValue;
            return mfId;
        }

        public string GetWorkerNameByWamsId(string wamsId)
        {
            var name = string.Empty;
            var worker = _db.Workers.Where(i => i.WAMSId == wamsId)
                            .Select(i => i)
                            .FirstOrDefault();

            if (worker != null)
            {
                name = $"{worker.FirstName} {worker.MiddleInitial}. {worker.LastName}".Replace(" . ", " ");
            }

            return name;
        }

        public string GetWorkerNameByWIUId(string wiuid)
        {
            var name = string.Empty;
            var worker = _db.Workers.Where(i => i.WIUID == wiuid)
                            .Select(i => i)
                            .FirstOrDefault();

            if (worker != null)
            {
                name = $"{worker.FirstName} {worker.MiddleInitial}. {worker.LastName}".Replace(" . ", " ");
            }

            return name;
        }

        public List<IWorker> GetWorkersByAuthToken(string entSecCode, string authcode)
        {
            var orgWorkerList = GetAllWorkersByOrganizationCode(entSecCode);
            var authId        = _db.Authorizations?.Where(x => x.Name.Trim() == authcode.Trim()).Select(y => y.Id).FirstOrDefault();

            if (authId == null) return null;
            var roleCodeList = GetRoleList(authId);

            if (roleCodeList.Count == 0) return null;
            var authorizedWorkerList = GetAuthorizedWorkerList(roleCodeList);

            if (authorizedWorkerList.Count == 0) return null;
            var workerList = orgWorkerList?.Where(x => authorizedWorkerList.Any(y => x.Id == y.Id)).ToList();

            return workerList;
        }

        private IEnumerable<IWorker> GetAllWorkersByOrganizationCode(string entSecCode)
        {
            var currentDate = _authUser.CDODate ?? DateTime.Today;
            var orgIds = (entSecCode == "WI")
                             ? _db.Organizations?.Where(x => (x.InActivatedDate == null || x.InActivatedDate >= currentDate) && x.ActivatedDate <= currentDate)
                                  .Select(x => x.Id).Distinct()
                             : _db.Organizations?.Where(x => (x.EntsecAgencyCode == entSecCode)
                                                             && ((x.InActivatedDate == null || x.InActivatedDate >= currentDate) && x.ActivatedDate <= currentDate))
                                  .Select(x => x.Id);

            var workerList = _db.Workers?.Where(y => y.OrganizationId.HasValue && orgIds.Contains(y.OrganizationId.Value)).ToList();

            return workerList;
        }

        public List<string> GetRoleList(int? authId)
        {
            var roleList = new List<string>();
            foreach (var roleAuth in _db.RoleAuthorizations)
            {
                if (roleAuth.AuthorizationId != authId) continue;

                var roleCode = _db.Roles?.Where(x => x.Id == roleAuth.RoleId).Select(y => y.Code).FirstOrDefault();
                roleList.Add(roleCode);
            }

            return roleList;
        }

        public List<IWorker> GetAuthorizedWorkerList(List<string> roleList)
        {
            var authorizedWorkerList = new List<IWorker>();
            foreach (var worker in _db.Workers)
            {
                var workerRoles = worker.Roles.Split(',').ToList();

                if (workerRoles != null && workerRoles.Count > 0)
                {
                    authorizedWorkerList.AddRange((from role in workerRoles where roleList.Contains(role) select worker).Cast<IWorker>());
                }
            }

            return authorizedWorkerList;
        }
    }
}

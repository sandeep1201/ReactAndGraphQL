using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWorkHistorySectionEmploymentPreventionTypeBridgeRepository
    {
        public IWorkHistorySectionEmploymentPreventionTypeBridge NewWorkHistorySectionEmploymentPreventionTypeBridge(IWorkHistorySection parentObject, string user)
        {
            IWorkHistorySectionEmploymentPreventionTypeBridge obj = new WorkHistorySectionEmploymentPreventionTypeBridge();
            obj.WorkHistorySection = parentObject;
            obj.ModifiedBy = user;
            obj.ModifiedDate = DateTime.Now;
            obj.IsDeleted = false;

            _db.WorkHistorySectionEmploymentPreventionTypeBridges.Add((WorkHistorySectionEmploymentPreventionTypeBridge)obj);
            return obj;
        }
    }
}
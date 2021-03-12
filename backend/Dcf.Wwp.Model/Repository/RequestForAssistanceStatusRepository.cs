using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRequestForAssistanceStatusRepository
    {
        public IRequestForAssistanceStatus GetRequestForAssistanceStatus(string statusName)
        {
            var rfastatus = _db.RequestForAssistanceStatuses.FirstOrDefault(i => i.Name == statusName);

            return (rfastatus);
        }

        public IEnumerable<IRequestForAssistanceStatus> GetRequestForAssistanceStatusesWhere(Expression<Func<IRequestForAssistanceStatus, bool>> clause)
        {
            var rfaStatuses = new List<IRequestForAssistanceStatus>();

            try
            {
                rfaStatuses = _db.RequestForAssistanceStatuses
                                 .Where(clause)
                                 .AsNoTracking()
                                 .ToList();
            }
            catch (NullReferenceException ex)
            {
                // you should never hit this...
            }

            return (rfaStatuses);
        }
    }
}

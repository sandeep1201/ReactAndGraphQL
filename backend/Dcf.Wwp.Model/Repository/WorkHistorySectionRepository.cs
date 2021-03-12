using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IWorkHistorySection NewWorkHistorySection(IParticipant parentParticipant, string user)
        {
            var section = new WorkHistorySection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            section.ParticipantId = parentParticipant.Id;
            _db.WorkHistorySections.Add(section);

            return section;
        }
    }
}

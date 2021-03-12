using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IChildYouthSectionRepository
    {
        public IChildYouthSection NewChildYouthSection(int participantId, string user)
        {
            var cys = new ChildYouthSection();
            cys.ParticipantId = participantId;
            cys.ModifiedBy = user;
            cys.ModifiedDate = DateTime.Now;

            _db.ChildYouthSections.Add(cys);

            return cys;
        }
    }
}
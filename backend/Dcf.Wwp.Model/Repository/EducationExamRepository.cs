using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRepository
    {
        public IEducationExam NewEducationExam(IParticipant participant, string user)
        {
            IEducationExam e = new EducationExam();
            e.ParticipantId = participant?.Id;
            e.ModifiedDate = DateTime.Now;
            e.ModifiedBy = user;
            e.IsDeleted = false;
            _db.EducationExams.Add((EducationExam)e);
            return e;
        }
    }
}

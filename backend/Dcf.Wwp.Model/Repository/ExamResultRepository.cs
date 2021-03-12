using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRepository
    {
        public IExamResult ExamResultById(int? id)
        {
            return null;
            //return (from x in _db.ExamResults where x.Id == id select x).SingleOrDefault();
        }

        public IExamResult NewExamResult(IEducationExam parentObject, string user)
        {
            IExamResult e = new ExamResult();
            e.EducationExam = parentObject;
            e.IsDeleted = false;
            e.ModifiedBy = user;
            e.ModifiedDate = DateTime.Now;
            _db.ExamResults.Add((ExamResult)e);
            return e;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository:IRepository
    {
        public IEnumerable<IExamSubjectMaxScoreType> ExamSubjectMaxScoreTypes()
        {
            return null;

            //var ext =from x in _db.ExamSubjectMaxScoreTypes select x;
            //return ext;
        }
    }
}

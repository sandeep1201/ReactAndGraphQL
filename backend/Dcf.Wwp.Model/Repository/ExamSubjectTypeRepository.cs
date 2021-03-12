using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IExamSubjectTypeRepository
    {
        public IEnumerable<IExamSubjectType> ExamSubjectsByExamType(string examType)
        {
            return (from x in _db.ExamSubjectTypeBridges where (x.ExamType.Name == examType) select x.ExamSubjectType).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<IExamSubjectType> ExamSubjectTypes()
        {
            return null;

            //var est = from x in _db.ExamSubjectTypes orderby x.SortOrder select x;
            //return est;
        }
    }
}

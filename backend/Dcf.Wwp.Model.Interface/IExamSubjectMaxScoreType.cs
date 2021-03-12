using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IExamSubjectMaxScoreType
    {
         DateTime? CreatedDate { get; set; }
         Int32 Id { get; set; }
         Int32? ExamTypeId { get; set; }
         Int32? ExamSubjectTypeId { get; set; }
         String MaxScore { get; set; }
         Boolean IsDeleted { get; set; }
         String ModifiedBy { get; set; }
         DateTime? ModifiedDate { get; set; }

         //IExamSubjectType ExamSubjectType { get; set; }
         IExamType ExamType { get; set; }

    }
}

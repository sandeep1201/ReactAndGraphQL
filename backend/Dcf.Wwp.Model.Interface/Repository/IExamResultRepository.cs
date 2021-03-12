using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
   public interface IExamResultRepository
    {
        IExamResult ExamResultById(Int32? id);
        IExamResult NewExamResult(IEducationExam parentObject, String user);
    }
}

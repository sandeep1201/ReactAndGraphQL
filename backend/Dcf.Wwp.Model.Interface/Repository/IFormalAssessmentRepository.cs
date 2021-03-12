using System;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
   public interface IFormalAssessmentRepository
   {
       IFormalAssessment NewFormalAssessment(IBarrierDetail barrierDetail);
       void DeleteFormalAssements(Expression<Func<IFormalAssessment, bool>> clause);
   }
}

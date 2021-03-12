using System;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IFormalAssessmentRepository
    {
        public IFormalAssessment NewFormalAssessment(IBarrierDetail barrierDetail)
        {
            var fas = new FormalAssessment { BarrierDetail = barrierDetail as BarrierDetail };
            _db.FormalAssessments.Add(fas);

            return (fas);
        }

        public void DeleteFormalAssements(Expression<Func<IFormalAssessment, bool>> clause)
        {
            var entities = _db.FormalAssessments.Where(clause).ToList();

            entities.ForEach(i => _db.FormalAssessments.Remove((FormalAssessment)i));
        }
    }
}

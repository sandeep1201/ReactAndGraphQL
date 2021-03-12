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
    public partial class Repository : IGoalRepository
    {
        public IEnumerable<IGoal> AllGoalsByEP(int epId)
        {
            //return _db.EmployabilityPlans.SingleOrDefault(x => x.Id == epId)?.Goals.AsEnumerable<IGoal>(); original code - 04/16/2019

            var r = _db.EmployabilityPlanGoalBridges
                       .Where(i => i.EmployabilityPlanId == epId)
                       .Select(i => i.Goal).ToList();

            return (r);
        }
    }
}
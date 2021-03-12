using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IGoalStep : ICommonModelFinal, ICloneable
    {
        Int32    Id                  { get; set; }
        Int32    GoalId              { get; set; }
        String   Details             { get; set; }
        Boolean? IsGoalStepCompleted { get; set; }
        IGoal     Goal                { get; set; }
    }
}

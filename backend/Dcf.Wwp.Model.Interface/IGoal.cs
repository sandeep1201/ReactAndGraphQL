using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IGoal : ICommonModelFinal, ICloneable
    {
        Int32     Id                  { get; set; }
        Int32?    GoalTypeId          { get; set; }
        DateTime? BeginDate           { get; set; }
        String    Name                { get; set; }
        String    Details             { get; set; }
        Boolean?  IsGoalEnded         { get; set; }
        Int32?    GoalEndReasonId     { get; set; }
        String    EndReasonDetails    { get; set; }

        IGoalEndReason         GoalEndReason     { get; set; }
        IGoalType              GoalType          { get; set; }
        ICollection<IGoalStep> GoalSteps         { get; set; }
    }
}
